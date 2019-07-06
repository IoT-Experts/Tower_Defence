using UnityEngine;
using System.Collections;

public class Bomb {

	public GameObject gameObject;

    public ObjectAnimation animation;
	
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
		
		set { gameObject.GetComponent<Renderer> ().material.mainTexture = value;} 
	}

	public Bomb (Vector2 position) {

		gameObject = GamePullController.CreateImage ();
		texture = Resources.Load ("Textures/UserInterface/Bomb") as Texture;

        animation = new ObjectAnimation (gameObject);
        animation.Load ("Bomb");
        animation.Play (-2);

		size = new Vector2 (1, 1);
		mapPosition = position;
		layer = 0.5f;

		BombsController.bombs.Add (this);
	}

	public void Update (Killable target) {

		if (Mathf.Pow (mapPosition.x - target.mapPosition.x, 2) + Mathf.Pow (mapPosition.y - target.mapPosition.y, 2)  <= 0.7f) {

			target.health -= GameController.bombDamage;
			Destroy ();
		} 
	}


	public void Destroy () {

        animation.Destroy ();
		GamePullController.DestroyImage (gameObject);
		BombsController.bombs.Remove (this);
	}
}
