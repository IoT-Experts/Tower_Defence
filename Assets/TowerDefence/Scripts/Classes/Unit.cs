using UnityEngine;
using System.Collections;

public class Unit : Killable {

	public ObjectAnimation objectAnimation;

	public string type;

	public float speed;
	protected float damage;

	public override int roadProgress {
		get {return roadIndex;}
	}


    private GUIImage rootImage;

	protected Vector2 targetPosition;
	protected int roadIndex;
	protected int roadNumber;

	protected Vector2 GetTruePosition (Vector2 falsePosition) {

		return falsePosition + new Vector2 (0, gameObject.transform.localScale.y/2f);
	}
	
	public Unit () {

	}

    public Unit (string _type, TeamColor _color, int _road) : this (_color, _road - 1) {

        type = _type;
		
		size = new Vector2 (1.5f, 1.5f) * Settings.FhdToHD;

        if (UnitsSettings.Buffs (type).Contains ("Boss")) {

		    size = new Vector2 (3f, 3f) * Settings.FhdToHD;
        }
		healthBarDeltaPosition =  UnitsSettings.HealthBarPosition (_type) / 4f * Settings.FhdToHD;//new Vector2 (0, 0.7f);
		shadowDeltaPosition = UnitsSettings.ShadowPosition (_type) / 4f * Settings.FhdToHD; //new Vector2 (0.1f, -0.575f);;

        try {

            healthMax = BalanceSettings.health [_type];
		    speed = BalanceSettings.speed [_type];
		    damage = BalanceSettings.damage [_type];
		    goldReward = BalanceSettings.price [_type];
		    defence = BalanceSettings.defence [_type];
        } catch (System.Exception e) {
            
            Debug.LogError ("Wrong" + type);
        }

		objectAnimation.Load (_type + "Walk", -0.6f);
		objectAnimation.Play (-2);
    }

	protected Unit (TeamColor _color, int _road) {

		gameObject = GamePullController.CreateImage ();
        gameObject.name = "Unit";

		shadow = GamePullController.CreateImage ();
        shadow.GetComponent <Renderer> ().material.mainTexture = ResourcesController.Load ("Textures/Shadow") as Texture;
		healthBar = new HealthBar ();
		
		GameController.unitsDictionary.Add (gameObject, this);

		objectAnimation = new ObjectAnimation (gameObject);
		
		color = _color;
		
		roadIndex = 0;
		var count = (color == TeamColor.Red?GameController.RoadRed.Count:GameController.RoadBlue.Count);
		Vector3 _targetPosition;
		
		if (color == TeamColor.Red) {

			if (_road == -1)
				do {
					roadNumber = Random.Range (0, count);
				} while (GameController.RoadRed[roadNumber].Count == 0);
			else 
				roadNumber = _road;


			_targetPosition = GameController.RoadRed[roadNumber][roadIndex];
			
		} else {

			if (_road == -1)
				do {
					roadNumber = Random.Range (0, count);
				} while (GameController.RoadBlue[roadNumber].Count == 0);
			else 
				roadNumber = _road;
			
            _targetPosition = new Vector3 ();
            try {

                _targetPosition = GameController.RoadBlue[roadNumber][roadIndex];
            } catch (System.Exception) {

                Debug.LogError (roadNumber + " " + roadIndex + "; " + GameController.RoadBlue.Count + " ");
            }
			 
		}
		
		layer = _targetPosition.z;
		targetPosition = GetTruePosition (_targetPosition);
		mapPosition = targetPosition;
		isKilling = false;
	}


	public void Update (float deltaTime) {

		if (isKilling)
			return;
		if (!(effects [Effect.Stun] > 0 || effects [Effect.Root] > 0)) {

			if (Vector2.Distance (targetPosition, mapPosition) > 0.1f) {

				Vector2 _direction = (targetPosition - mapPosition);
				_direction.Normalize ();

				gameObject.transform.localScale = new Vector3 ((_direction.x > 0 ? -1 : 1) * Mathf.Abs (size.x), size.y, 1);
				direction = (_direction.x > 0 ? -1 : 1);
				mapPosition += _direction * deltaTime * speed * Settings.FhdToHD;
		

			} else {
				
				roadIndex++;

				if (roadIndex == (color == TeamColor.Red ? GameController.RoadRed : GameController.RoadBlue) [roadNumber].Count) {
					Kill ();
					healthBar.Destroy ();
					GameController.ReduceHealth (color == TeamColor.Red ? TeamColor.Blue : TeamColor.Red, (int)damage);
                    GameController.DamageAnimation ();
				} else {
					Vector3 _targetPosition = (color == TeamColor.Red ? GameController.RoadRed : GameController.RoadBlue) [roadNumber] [roadIndex];
					layer = _targetPosition.z;
					targetPosition = GetTruePosition (_targetPosition);
				}

				BombsController.Update (this);
			}

		}
		
		if (effects [Effect.Stun] > 0) {

			effects [Effect.Stun] -= deltaTime;
			effects [Effect.Stun] = effects [Effect.Stun]<0?0:effects [Effect.Stun];
		}
		
		if (effects [Effect.Visible] > 0) {

			effects [Effect.Visible] -= deltaTime;
			effects [Effect.Visible] = effects [Effect.Visible]<0?0:effects [Effect.Visible];
		}
        
		if (effects [Effect.Slow] > 0) {

			effects [Effect.Slow] -= deltaTime;
			effects [Effect.Slow] = effects [Effect.Slow]<0?0:effects [Effect.Slow];
            speed = BalanceSettings.speed [type] * 0.5f;
		} else {

            speed = BalanceSettings.speed [type];
        }
		
		if (effects [Effect.Root] > 0) {
                
            if (rootImage == null) {

                rootImage = new GUIImage ("UserInterface/Freeze", layer - GUIController.layer + 0.1f, mapPosition, new Vector2 (1, 1), false);
            } else {

                rootImage.positionInMeters = mapPosition;
                rootImage.layer = layer - GUIController.layer + 0.1f;
            }

			effects [Effect.Root] -= deltaTime;
			effects [Effect.Root] = effects [Effect.Root]<0?0:effects [Effect.Root];
		} else {

            if (rootImage != null) {

                rootImage.Destroy ();
                rootImage = null;
            }
        }

		if (effects [Effect.Heal] > 0) {

			effects [Effect.Heal] -= deltaTime;
			effects [Effect.Heal] = effects [Effect.Heal]<0?0:effects [Effect.Heal];
			health +=healthMax*0.15f*deltaTime;
		}
	}

	public override void Damage (float damage, string attackerType)	{

		if (isKilling)
			return;

        float lastDefence = defence;

        if (TowersSettings.Buffs (attackerType).Contains (Tower.Buff.MagicSkills.ToString ()) 
                && Settings.IsLearned ("Chaotic")) {
                
            defence = 0;
        }

        if (TowersSettings.Buffs (attackerType).Contains (Tower.Buff.MagicSkills.ToString ()) 
                && Settings.IsLearned ("Fatal")) {
                
            damage = 100000017;
        }
        
		health -= damage;
        
        if (TowersSettings.Buffs (attackerType).Contains (Tower.Buff.LazerSkills.ToString ()) 
                && Settings.IsLearned ("LazerEffect")) {
                
            effects [Effect.Visible] =  1f;
        }
        
        if (TowersSettings.Buffs (attackerType).Contains (Tower.Buff.LazerSkills.ToString ()) 
                && Settings.IsLearned ("UnitSlow")) {
              
            effects [Effect.Slow] =  1f;
        }

        defence = lastDefence;

        if (health <= 0) {

            if (TowersSettings.Buffs (attackerType).Contains (Tower.Buff.MagicSkills.ToString ()) 
                && Settings.IsLearned ("UnitGold")) {
                
				GameController.IncreaseGold (color == TeamColor.Red?TeamColor.Blue:TeamColor.Red, goldReward / 2);
            }

            if (TowersSettings.Buffs (attackerType).Contains (Tower.Buff.MagicSkills.ToString ()) 
                && Settings.IsLearned ("UnitLife")) {

                GameController.killedUnitsToHeal ++;
            }
        }

	}


	protected override void Kill () {

		if (isKilling)
			return;
		
		isKilling = true;
		objectAnimation.Load ((Random.value>0.5f?"Bang":"BangNoFire"), 20);
		objectAnimation.Play (-1, Destroy);
		gameObject.transform.localScale = new Vector3 (1.5f, 1.5f, 1);

		if (shadow != null)
			GamePullController.DestroyImage (shadow);
	}

	public void Destroy () {

        if (rootImage != null) {

            rootImage.Destroy ();
        }

		GameController.unitsToDestroy.Add (this);
		GamePullController.DestroyImage (gameObject);
		objectAnimation.Destroy ();
		GameController.unitsDictionary.Remove (gameObject);
		gameObject = null;
		objectAnimation = null;
		healthBar = null;
		shadow = null;
	}

}
