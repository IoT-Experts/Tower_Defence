using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public abstract class Killable : Colorable {

	public enum Effect {Stun, Root, Heal, Slow, Visible}
	protected Dictionary <Effect, float> effects;

	protected float _healthMax;
	public float healthMax {
		get {return _healthMax;}
		set {_healthMax = value; health = value;}

	}

	public Killable () {
		effects = new Dictionary<Effect, float> ();
		effects.Add (Effect.Heal, 0);
		effects.Add (Effect.Root, 0);
		effects.Add (Effect.Stun, 0);
		effects.Add (Effect.Slow, 0);
		effects.Add (Effect.Visible, 0);
	}

	protected int goldReward;

	public float defence = 0;

	protected float _health;
	public float health {
		get {return _health;}
		set {

			if (isKilling)
				return;

			if (_health > value) {

				_health = Mathf.Min (_health, value+defence*(Random.value*2+1));
			} else 
				_health = value;

			if (_health > healthMax)
				_health = healthMax;

			if (healthBar != null)
				healthBar.health = _health/_healthMax;
			
			if (_health <=0 && !isKilling) {
				_health = 0;
				killedPosition = mapPosition;
				killedLayer = layer;

				if (healthBar != null)
					healthBar.Destroy ();

                Kill ();

				GameController.IncreaseGold (color == TeamColor.Red?TeamColor.Blue:TeamColor.Red, goldReward);
			}
		}

	}

	protected float direction;

	
	public GameObject gameObject;
	protected HealthBar healthBar;
	protected GameObject shadow;
	
	
	protected Vector2 killedPosition;
	protected float killedLayer;

	public Vector2 mapPosition {
		
		get { 
			if (isKilling)
				return killedPosition;

			return new Vector2 (gameObject.transform.position.x, gameObject.transform.position.z);
		}
		set {
			gameObject.transform.position = new Vector3 (value.x, gameObject.transform.position.y, value.y);

			if (healthBar != null)
				healthBar.mapPosition = mapPosition + healthBarDeltaPosition;

			if (shadow != null)
				shadow.transform.position = new Vector3 (value.x+shadowDeltaPosition.x*direction, gameObject.transform.position.y-0.1f
			                                         	, value.y+shadowDeltaPosition.y);
		}
		
	}
	
	public Vector2 size {
		
		get {return new Vector2 (gameObject.transform.localScale.x, gameObject.transform.localScale.y); }
		set {
			gameObject.transform.localScale = new Vector3 (value.x, value.y, 1);

			if (shadow != null)
				shadow.transform.localScale = new Vector3 (value.x, value.y*0.43f, 1);
		}
		
	}
	
	public float layer {
		get {
			if (isKilling)
				return killedLayer;

			return gameObject.transform.position.y;
		}
		set {
			gameObject.transform.position = new Vector3 (gameObject.transform.position.x, value, gameObject.transform.position.z);

			if (healthBar != null)
				healthBar.layer= layer + 0.1f;

			
			if (shadow != null)
				shadow.transform.position = new Vector3 (shadow.transform.position.x, gameObject.transform.position.y-0.1f
				                                         , shadow.transform.position.z);
		}
	}


	public abstract int roadProgress {
		get;
	}


	public bool isKilling;
	protected Vector2 healthBarDeltaPosition;
	protected Vector2 shadowDeltaPosition;


	public void AddEffect (Effect effect, float time) {

		if (!effects.ContainsKey (effect)) {
			effects.Add (effect, time);
		} else {
			effects[effect] += time;
		}
	}

	protected abstract void Kill ();

	public abstract void Damage (float damage, string attackerType);

}
