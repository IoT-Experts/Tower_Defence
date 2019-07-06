using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class Tower : Colorable {

	public string type;

	public enum Buff {Damage, AttackSpeed, AttackRange
        , LightningAttack, MissileAttack, MageAttack, LazerAttack
        , FarestAttack
        , MagicSkills, LazerSkills, LightningSkills,
    };
	protected Dictionary <Buff, float> buffs;

	protected GameObject gameObject;
	protected ObjectAnimation animation;

	public Vector3 towerPlacePosition;

	public void UseBuff (Buff buff, float value) {

        buffs [buff] = value;
	}

    public bool HasBuff (Buff buff) {

        return (buffs.ContainsKey (buff) && buffs [buff] > 0);
    }


	public Vector2 mapPosition {
		
		get {return new Vector2 (gameObject.transform.position.x, gameObject.transform.position.z);}
		set {gameObject.transform.position = new Vector3 (value.x, gameObject.transform.position.y, value.y);}
		
	}

	public Vector2 size {
		
		get {return new Vector2 (gameObject.transform.localScale.x, gameObject.transform.localScale.y); }
		set {gameObject.transform.localScale = new Vector3 (value.x, value.y, 1);}
		
	}
	
	public float layer {
		get {return gameObject.transform.position.y;}
		set {gameObject.transform.position = new Vector3 (gameObject.transform.position.x, value, gameObject.transform.position.z);}
	}

	protected Texture texture {
		
        get {return gameObject.GetComponent<Renderer>().material.mainTexture; }
		set {gameObject.GetComponent<Renderer>().material.mainTexture = value;} 
		
	}

	
	
	public float _attackRange;
	public float attackRange {
		get { return _attackRange*buffs[Buff.AttackRange]; }
		set { _attackRange = value; }
	}

	protected float _damage;
	public float damage {
		get { return _damage*buffs[Buff.Damage]; }
		set { _damage = value; }
	}

	protected float _damageDelta;
	public float damageDelta {
		get { return _damageDelta*buffs[Buff.Damage]; }
		set { _damageDelta = value; }
	}

	public float _attackSpeed;
	public float attackSpeed {
		get {

            return _attackSpeed*buffs[Buff.AttackSpeed]
                * ((buffs.ContainsKey (Buff.MagicSkills) && Settings.IsLearned ("MagicAttackSpeed"))
                    ? 1.2f : 1
                );
        }
		set { _attackSpeed = value; }
	}

    protected float reloadTime = 0;
	protected string attackAnimaton;
	protected float attackAnimatonTime;

    private float randomedDamage {

        get {

            return (damage + (UnityEngine.Random.value-0.5f)*2*damageDelta) 
                * (
                    (
                        (buffs.ContainsKey (Buff.LightningSkills) && Settings.IsLearned ("LightningCritical"))
                        || (buffs.ContainsKey (Buff.MagicSkills) && Settings.IsLearned ("MagicCritical"))
                        || (buffs.ContainsKey (Buff.LazerSkills) && Settings.IsLearned ("LazerCritical"))
                    )
                 
                    && UnityEngine.Random.value < 0.2f ? 2 : 1
                )
                * ((buffs.ContainsKey (Buff.LightningSkills) && Settings.IsLearned ("LightningCriticalPower"))
                    && UnityEngine.Random.value < 0.2f ? 2 : 1
                )
                * ((buffs.ContainsKey (Buff.LightningSkills) && Settings.IsLearned ("LightningDamage"))
                    ? 1.2f : 1
                )
                * (GameController.isDoubleDamage ? 2 : 1);
        }
    }
    
	public Vector2 positionByTowerPlace {
		set {mapPosition = value - TowersSettings.TowerPlacePosition (type) * Settings.FhdToHD; } //+ new Vector2 (0, 0.27f);}
	}

	private Vector2 attackPosition {
		get {return mapPosition + TowersSettings.AttackPosition (type) * Settings.FhdToHD; }
	}

	protected void AttackAnimation () {

        if (AnimationBox.Contains (attackAnimaton)) {

		    animation.Load (attackAnimaton, -attackAnimatonTime);
		    animation.Play (-1);
        } else {

            Debug.LogWarning ("No attack animation: " + attackAnimaton);
        }
	}
    
    private Killable FindNearestToTowerEnemyToAttack () {

        Killable result = null;

		foreach (Killable unit in GameController.units) {
			if (!unit.isKilling && unit.color != color && Vector2.Distance (attackPosition, unit.mapPosition) <= attackRange)
			if (result == null || Vector2.Distance (attackPosition, unit.mapPosition) < Vector2.Distance (attackPosition, result.mapPosition)) {

				result = unit;
			}
		}

		return result;
    }
    
    private Killable FindFarestEnemyToAttack () {

        Killable result = null;
		int maxProgress = 0;

		foreach (Killable unit in GameController.units) {
			if (!unit.isKilling && unit.color != color)
			if ((Mathf.Pow (mapPosition.x - unit.mapPosition.x, 2) + Mathf.Pow (mapPosition.y - unit.mapPosition.y, 2) 
			   <= Mathf.Pow (attackRange + 0.1f, 2))) {

				if (maxProgress < unit.roadProgress) {
					maxProgress = unit.roadProgress;
					result = unit;
				}
					
			}
		}

		return result;
    }

	protected Killable FindEnemyToAttack () {

		if (HasBuff (Buff.FarestAttack)) {

            return FindFarestEnemyToAttack ();
        }

        return FindNearestToTowerEnemyToAttack ();
	}

	private void AttackMissile (Killable unit) {

        if (unit == null)
			return;

		AttackAnimation ();

		Missile missile = new Missile (type, Resources.Load ("Textures/Missiles/Cannonner") as Texture, attackPosition, new Vector2 (0.3f, 0.3f),
		                               randomedDamage, 8f, unit);
    }

    private void AttackParabolic (Killable unit) {
		
		if (unit == null)
			return;
		
		AttackAnimation ();

            
		MissileParabolic missile = new MissileParabolic (type, Resources.Load ("Textures/Missiles/SteamMissile") as Texture, attackPosition
            , new Vector2 (0.7f, 0.7f), randomedDamage, 1f, unit, 2f);
       
		
		
	}
    
    private void AttackLightning (Killable unit) {
		
		if (unit == null)
			return;

		AttackAnimation ();

		LightningController.Attack (attackPosition, unit, -1 - 15f, 0.3f);
		unit.Damage (randomedDamage, type);
		
	}
    
    private void AttackLazer (Killable unit) {
		
		if (unit == null)
			return;

        attackAnimatonTime = 0.3f;

		AttackAnimation ();
    
        UpdateController.Timer (attackAnimatonTime / 2f, () => {

            new Lazer (attackPosition, unit, new Color (0, 1f, 1f), () => {
            
		        unit.Damage (randomedDamage, type);
            }, 0.6f, 0.4f);
        });
	}

	protected void Attack (Killable unit) {
        
	    if (HasBuff (Buff.LightningAttack)) {

            AttackLightning (unit);
            return;
        }

        if (HasBuff (Buff.LazerAttack)) {

            AttackLazer (unit);
            return;
        }

	    if (HasBuff (Buff.MageAttack)) {

            AttackParabolic (unit);
            return;
        }

	    if (HasBuff (Buff.MissileAttack)) {

            AttackMissile (unit);
            return;
        }


        Debug.LogError ("No attack type on " + type); 
	}

	public void Update (float deltaTime) {

		if (reloadTime > 0) {
			reloadTime -= Time.deltaTime;
		} else {

			reloadTime = 1/attackSpeed;
			Attack (FindEnemyToAttack ());
		}

	}
	
    public Tower (string _type, Colorable.TeamColor _color) {


        reloadTime = 0.6f;
		type = _type;
        color = _color;

		gameObject = GamePullController.CreateImage ();
		animation = new ObjectAnimation (gameObject);

        gameObject.name += "Tower";
        gameObject.GetComponent <BoxCollider> ().enabled = true;

		texture = Resources.Load ("Textures/Towers/" + type + "Tower") as Texture;

		attackAnimaton = type + "TowerAttack";
		attackAnimatonTime = 0.2f;

        try {

            damage = BalanceSettings.damage [type];
		    damageDelta = BalanceSettings.deltaDamage [type];
		    attackRange = BalanceSettings.attackRange [type];
		    attackSpeed = BalanceSettings.attackSpeed [type];
        } catch (Exception) {

            Debug.LogError ("Wrong tower type: " + type);
        }
 
		layer = 6;
		size = new Vector2 (texture.width / 50f, texture.height / 50f) / 2f * Settings.FhdToHD;

		GameController.towers.Add (this);
		GameController.towersDictionary.Add (gameObject, this);

		buffs = new Dictionary<Buff, float> (GameController.buffs[color]);

        foreach (var buff in TowersSettings.Buffs (type)) {

            buffs.Add ((Buff) Enum.Parse (typeof (Buff), buff), 1);
        }
	}

    public Tower (Tower toRemove, string _type) : this (_type, toRemove.color){

        positionByTowerPlace = new Vector2 (toRemove.towerPlacePosition.x, toRemove.towerPlacePosition.z);
		layer = toRemove.towerPlacePosition.y;
		towerPlacePosition = toRemove.towerPlacePosition;
		
		toRemove.Destroy (false);
    }

	public void Destroy (bool createTowerPlace = true) {

        if (createTowerPlace) {

            GameObject towerPlace = GamePullController.CreateImage ();
            towerPlace.GetComponent <BoxCollider> ().enabled = true;
		
		    switch (color) {
		    case TeamColor.Red:
			    GameController.towerPlacesRed.Add (towerPlace);
			    break;
		    case TeamColor.Blue:
			    GameController.towerPlacesBlue.Add (towerPlace);
			    break;
		    }
		    towerPlace.transform.position = towerPlacePosition;
		    towerPlace.GetComponent<Renderer>().material.mainTexture 
			    = Resources.Load ("Textures/Towers/TowerPlace"+color) as Texture;
		    towerPlace.transform.localScale = new Vector3 (3.08f * Settings.FhdToHD, 4.13f * Settings.FhdToHD, 1);
            towerPlace.name = "TowerPlace"+color;
        }
		
		GameController.towers.Remove (this);
		GameController.towersDictionary.Remove (gameObject);
		GamePullController.DestroyImage (gameObject);
	}

}
