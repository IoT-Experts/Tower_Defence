using UnityEngine;
using System.Collections;

public class HealthBar {
	
	private GameObject healthBarRed;
	private GameObject healthBarGreen;

	private Vector2 _size = new Vector2 (1, 0.1f);
	public Vector2 size {

		get { return _size; }
		set {
			_size = value;
			healthBarRed.transform.localScale = new Vector3 (size.x, size.y, 1);
			healthBarGreen.transform.localScale = new Vector3 (size.x, size.y, 1);
		}

	}

	private float _health; // [0..1]
	public float health {

		set {
			_health = Mathf.Clamp (value, 0, 1);
			SetGreenPosition ();

		}


	}

	private float _layer;
	public float layer {

		set {
			_layer = value; 
			healthBarRed.transform.position = new Vector3 (_mapPosition.x, _layer, _mapPosition.y);
			healthBarGreen.transform.position = new Vector3 (healthBarGreen.transform.position.x, _layer + 0.3f, healthBarGreen.transform.position.z);

		
		}

	}

	private Vector2 _mapPosition;
	public Vector2 mapPosition {

		set {
			_mapPosition = value;
			healthBarRed.transform.position = new Vector3 (value.x, _layer, value.y);
			SetGreenPosition ();
		}


	}

	public HealthBar () {
		
		healthBarRed = GamePullController.CreateImage ();
		healthBarGreen = GamePullController.CreateImage ();
        
        healthBarRed.GetComponent <Renderer> ().material.mainTexture = ResourcesController.Load ("Textures/HealthBars/HealthBarRed") as Texture;
        healthBarGreen.GetComponent <Renderer> ().material.mainTexture = ResourcesController.Load ("Textures/HealthBars/HealthBarGreen") as Texture;
		
		healthBarRed.transform.localScale = new Vector3 (size.x, size.y, 1);
		healthBarGreen.transform.localScale = new Vector3 (size.x, size.y, 1);

		health = 1;

	}

	private void SetGreenPosition () {

		healthBarGreen.transform.position = new Vector3 (_mapPosition.x+(-1+_health)*size.x/2f, _layer + 0.3f, _mapPosition.y);
		healthBarGreen.transform.localScale = new Vector3 (_health*size.x, healthBarGreen.transform.localScale.y, healthBarGreen.transform.localScale.z);

	}

	public void Destroy () {

		GamePullController.DestroyImage (healthBarRed);
		GamePullController.DestroyImage (healthBarGreen);
		healthBarRed = null;
		healthBarGreen = null;
	}

}
