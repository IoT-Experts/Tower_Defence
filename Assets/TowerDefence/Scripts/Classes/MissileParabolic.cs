using UnityEngine;
using System.Collections;

public class MissileParabolic : IUpdatable {

	private GameObject gameObject;
	private Killable target;

	private float parabolicHeight;

	private float damage;
	private float speed;
	private float progress;
	private Vector2 startPosition;

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
	
	
	
	public MissileParabolic (string _towerType, Texture _texture, Vector2 _position, Vector2 _size, float _damage, float _speed, Killable _target, float _parabolicHeight) {
		
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
		parabolicHeight = _parabolicHeight;

		startPosition = mapPosition;
	}
	
	
	public void Update (float deltaTime) {
		
		if (Vector2.Distance (target.mapPosition, mapPosition)>0.1f) {

			progress += deltaTime*speed;
			
			float x2 = target.mapPosition.x - startPosition.x;
			float y2 = target.mapPosition.y - startPosition.y ;

			/*
			Vector2 dx = new Vector2 (target.mapPosition.x, 0);
			parabolicHeight = 1;

			Vector2 resultPosition = startPosition + progress*dx;


			//resultPosition += (Vector2.up).normalized*parabolicHeight*( Mathf.Pow (progress*dx.x, 2) - (Mathf.Pow (target.mapPosition.x, 2) 
			//                                                            - (Mathf.Pow (target.mapPosition.x, 2) - target.mapPosition.y)*progress*dx.x/target.mapPosition.x ) );

			float x2 = Mathf.Abs (target.mapPosition.x - startPosition.x);
			float y2 = target.mapPosition.y - startPosition.y ;
			resultPosition -= (Vector2.up)*parabolicHeight*( Mathf.Abs (progress*dx.x)*( Mathf.Abs (progress*dx.x) - (x2*x2-y2)/x2));
			
			mapPosition = resultPosition;
			*/
			
			/*
			Vector2 direction = (new Vector2  (startPosition.x + (x2*x2-y2)/x2, 0)  - startPosition);
			Vector2 resultPosition = startPosition + progress*direction;
			resultPosition += (new Vector2 (direction.y, Mathf.Abs (direction.x))).normalized*parabolicHeight*(1 - Mathf.Pow (2*progress - 1, 2));
			mapPosition = resultPosition;
			*/

			Vector2 direction = (target.mapPosition - startPosition);
			Vector2 resultPosition = startPosition + progress*direction;
			//resultPosition += (new Vector2 (direction.y, Mathf.Abs (direction.x))).normalized*parabolicHeight*(1 - Mathf.Pow (2*progress - 1, 2));
			mapPosition = resultPosition;

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
