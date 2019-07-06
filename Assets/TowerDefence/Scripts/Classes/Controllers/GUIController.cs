using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class GUIController : IUpdatable {

	public static float layer = 99;

    private const int version = 2;
    
    public static GUIController instance;

	public static float GUIBackgroundWidth = 1080;

	public static float width {
		get { return Screen.width; }
	}

	private static List <GUIObject> objects = new List<GUIObject> ();
	private static Dictionary <GameObject, GUIButton> objectsDictionary = new Dictionary <GameObject, GUIButton> ();

    private static bool isButtonDown;

	public GUIController () {

        if (instance != null) {
            
		    objects = new List<GUIObject> ();
		    objectsDictionary = new Dictionary <GameObject, GUIButton> ();
            isButtonDown = false;
            return;
        }

        instance = this;

		objects = new List<GUIObject> ();
		objectsDictionary = new Dictionary <GameObject, GUIButton> ();
        isButtonDown = false;
        
        UpdateController.AddClickListener ("OnClick", "GUIOnClick", OnClick);
        UpdateController.AddClickListener ("OnButtonDown", "GUIOnButtonDown", OnButtonDown);
        UpdateController.AddClickListener ("OnButtonUp", "GUIOnButtonUp", (v) => { OnButtonUp (); });
        UpdateController.AddClickListener ("OnOver", "GUIOnOver", OnOver);
	}

	public static void OnCameraUpdate () {

		foreach (var obj in objects) {

			obj.SetPosition ();
			obj.SetSize ();
		}

	}
    
	public static void OnLanguageChange () {

		foreach (var obj in objects) {

            if (obj is GUIText) {

                (obj as GUIText).OnLanguageChange ();
            }
		}

	}

    
	public static Transform CreateText (ref TextMesh textObject) {

		GameObject text = GamePullController.CreateText ();
		textObject = text.GetComponent<TextMesh> ();
		return text.transform;
	}
	
	public static void Add (GameObject gameObject, GUIButton guiObject) {

        try {

            objectsDictionary.Add(gameObject, guiObject);
            objects.Add(guiObject);
        } catch (Exception e) {

            Debug.LogError (e + " " + gameObject.name);
        }
    }

	public static void Add (GUIObject guiObject) {

		objects.Add (guiObject);
	}
	
	public static void Remove (GameObject gameObject, GUIObject guiObject) {
		
        if (guiObject != null) {

            objects.Remove (guiObject);
        }
        
        if (gameObject != null) {
            
		    objectsDictionary.Remove (gameObject);
        }
	}
	
	public static void Remove (GUIObject guiObject) {
		
		objects.Remove (guiObject);
	}
	
    public static void OnClick (Vector2 position) {
        
		Ray ray;
		RaycastHit hit;
		
		ray = Camera.main.ScreenPointToRay (position);


		if (Physics.Raycast (ray, out hit, 200)) {


			if (hit.transform.gameObject.name.Contains ("GUIButton")) {
				GUIController.OnClick (hit.transform.gameObject);
			}
		} 
	}

	public static void OnClick (GameObject gameObject) {
		
        try {

            objectsDictionary[gameObject].Click ();
        } catch (Exception e) {

            Debug.LogError (gameObject.name + " Error " + e.ToString ());
        }
		
	}

    public static void OnButtonDown (Vector2 position) {
        
		Ray ray;
		RaycastHit hit;
		
		ray = Camera.main.ScreenPointToRay (position);

		if (Physics.Raycast (ray, out hit, 200)) {

			if (hit.transform.gameObject.name.Contains ("GUIButton")) {

				GUIController.OnButtonDown (hit.transform.gameObject);
			}
		} 
	}

	public static void OnButtonDown (GameObject gameObject) {
		
        isButtonDown = true;
        
        try {

            objectsDictionary[gameObject].ButtonDown ();
        } catch (Exception e) {

            Debug.LogError (gameObject.name + " Error " + e.ToString ());
        }
		
	}

    public static void OnOver (Vector2 position) {

		Ray ray;
		RaycastHit hit;
		
		ray = Camera.main.ScreenPointToRay (position);


		if (Physics.Raycast (ray, out hit, 200)) {

			if (hit.transform.gameObject.name.Contains ("GUIButton")) {

				GUIController.OnOver (hit.transform.gameObject);
			}
		} 
	}

	public static void OnOver (GameObject gameObject) {

        if (isButtonDown) {

            try {

		        objectsDictionary[gameObject].ButtonDownOver ();
            } catch (Exception e) {

                Debug.LogError (gameObject.name + " Error " + e.ToString ());
            }
        }
		
	}

	public static void OnButtonUp () {
		
        isButtonDown = false;

		foreach (var button in objectsDictionary) 
			button.Value.ButtonUp ();
		
	}

    public void Update (float deltaTime) {

    }
}
