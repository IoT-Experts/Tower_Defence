using UnityEngine;
using System.Collections;

public class Missile : IUpdatable {

	private GameObject gameObject;
	private Killable target;

	private float damage;
	private float speed;

    string towerType;

	private Vector2 mapPosition {
		
		get {return new Vector2 (gameObject.transform.position.x, gameObject.transform.position.z);}
		set {gameObject.transform.position = new Vector3 (value.x, gameObject.transform.position.y, value.y);}
		
	}
	
	private float layer {
		get {return gameObject.transform.position.y;}
		set {gameObject.transform.position = new Vector3 (gameObject.transform.position.x, value, gameObject.transform.position.z);}
	}
	
	private Vector2 size {
		
		get {return new Vector2 (gameObject.transform.localScale.x, gameObject.transform.localScale.y); }
		set {gameObject.transform.localScale = new Vector3 (value.x, value.y, 1);}
		
	}
	
	private Texture texture {
		
		set {gameObject.GetComponent<Renderer>().material.mainTexture = value;} 
		
	}



	public Missile (string _towerType, Texture _texture, Vector2 _position, Vector2 _size, float _damage, float _speed, Killable _target) {
		
        towerType = _towerType;

		gameObject = GamePullController.CreateImage ();
		GameController.missiles.Add (this);

		mapPosition = _position;
		damage = _damage;
		target = _target;
		texture = _texture;
		size = _size;
		speed = _speed;
		layer = _target.layer;

	}


	public void Update (float deltaTime) {

		if (Vector2.Distance (target.mapPosition, mapPosition)>0.1f) {
			
			Vector2 direction = (target.mapPosition - mapPosition);
			direction.Normalize ();

			mapPosition += direction*deltaTime*speed;
			layer = target.layer;
			
		} else {

			target.Damage (damage, towerType);
			Destroy ();
		}

	}


	private void Destroy () {

		GameController.missilesToDestroy.Add (this);
		GamePullController.DestroyImage (gameObject);
	}

}
