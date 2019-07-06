using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GamePullController {
    
    public class PreoderItem {

        public string target;
        public int count;
        public Actions.VoidVoid onEnd;

        public PreoderItem (string _target, int _count, Actions.VoidVoid _onEnd) {

            target = _target;
            count = _count;
            onEnd = _onEnd;
        }
    }

    public static bool isPreordered = false;

	private static GameObject tempGameObject;
    private static Dictionary <string, int> id;
    
    private static Dictionary <GameObject, Actions.VoidGameObject> history;
    
    private static Dictionary <string, KeyValuePair <Actions.GameObjectVoid, Actions.VoidGameObject>> preorders;

    private static Dictionary <string, string> preoderNameTranslates;

    private static int GetId (string name) {

        if (!id.ContainsKey (name))
            id[name] = 0;

        return id[name]++;
    }

    private static void AddToHistory (GameObject gameObject, Actions.VoidGameObject action) {

        if (!history.ContainsKey (gameObject)) {

            history.Add (gameObject, action);
        }
    }

	public GamePullController () {

        if (id == null) {

            id = new Dictionary<string, int> ();
        }

        history = new Dictionary <GameObject, Actions.VoidGameObject> ();
		PullController.Create ();

        if (preorders == null) {

            preorders = new  Dictionary<string, KeyValuePair<Actions.GameObjectVoid, Actions.VoidGameObject>> ();
            preoderNameTranslates = new Dictionary<string, string> ();
            
            preoderNameTranslates.Add ("GUIButton", "GUIImage");
            preoderNameTranslates.Add ("GUIImageAlphaExpensive", "GUIImageAlpha");
            preoderNameTranslates.Add ("GUIButtonAlpha", "GUIImageAlpha");
            
            preorders.Add ("GUIImageAlpha", new KeyValuePair<Actions.GameObjectVoid, Actions.VoidGameObject> (CreateImageAlpha, DestroyImageAlpha));
            preorders.Add ("GUIImage", new KeyValuePair<Actions.GameObjectVoid, Actions.VoidGameObject> (CreateImage, DestroyImage));
            preorders.Add ("Audio", new KeyValuePair<Actions.GameObjectVoid, Actions.VoidGameObject> (CreateAudio, DestroyAudio));
            preorders.Add ("Text", new KeyValuePair<Actions.GameObjectVoid, Actions.VoidGameObject> (CreateText, DestroyText));
            
        }

	}

    public static void Destroy () {

        var tempHistory = new Dictionary <GameObject, Actions.VoidGameObject> ();
        
        foreach (var g in history) {

            if (g.Key == null) {

                continue;
            }

            if (!g.Key.name.Contains ("OnlyDirectDestroying")) {

                tempHistory.Add (g.Key, g.Value);
            }
        }

        foreach (var g in tempHistory) {

            g.Value (g.Key);
        }

        history = new Dictionary<GameObject, Actions.VoidGameObject> ();
    }

    public static void Preorder (List <PreoderItem> toPreorder,  Actions.VoidIntInt onProcess = null, Actions.VoidVoid onEnd = null) {

        if (preorders == null) {

            Debug.LogError ("Preorders are not created");
        } else {

            int done = 0;
            int total  = 0;

            for (int i = 0; i < toPreorder.Count; i++) {

                total += toPreorder [i].count;
            }

            UpdateController.LaunchIt (toPreorder.Count, 0, (q) => {
                
                List <GameObject> created = new List<GameObject> ();

                string realName = preoderNameTranslates.ContainsKey (toPreorder [q].target) ? preoderNameTranslates [toPreorder [q].target] : toPreorder [q].target;
                
                if (preorders.ContainsKey (realName)) {

                    UpdateController.LaunchIt (toPreorder [q].count, 0, (i) => {

                        created.Add (preorders [realName].Key ());
                        toPreorder [q].onEnd ();
                        done ++;

                        if (onProcess != null) {

                            onProcess (done, total);
                        }

                    }, () => {
                        
                        for (int i = 0; i < toPreorder [q].count; i++) {

                            preorders [realName].Value (created [i]);
                        }

                        if (done >= total) {

                            Debug.Log (done + " " + total);
                            if (onEnd != null) {

                                onEnd ();
                            }

                            isPreordered = true;
                        }
                    });
                    
                } else {

                    Debug.LogWarning ("No such target: '" + toPreorder [q].target + "' as '" + realName + "'");
                }

            });

        }

    }

	public static GameObject CreateButton () {
		
		tempGameObject = CreateImage ();
        tempGameObject.name = "GUIButton" + GetId ("GUIButtonUndestroyable");

        return tempGameObject;
	}
	
	public static GameObject CreateImageAlpha () {
		
		tempGameObject = PullController.GetObject ("GUIImageAlphaUndestroyable");

		if (tempGameObject == null) {

			tempGameObject = GameObject.Instantiate (ResourcesController.Load ("Prefabs/GUI/GUIObjectExpensive")) as GameObject;
		}
        

		tempGameObject.name = "GUIImageAlpha" + GetId ("GUIImageAlphaUndestroyable");
		AddToHistory (tempGameObject, DestroyImageAlpha);
		return tempGameObject;
	}
    
	public static GameObject CreateImageAlphaExpensive () {
		
		tempGameObject = CreateImageAlpha ();
        tempGameObject.name = "GUIImageAlphaExpensive" + GetId ("GUIImageAlphaExpensiveUndestroyable");

        return tempGameObject;
	}
	
	public static GameObject CreateButtonAlpha () {
		
		tempGameObject = CreateImageAlpha ();
        tempGameObject.name = "GUIButtonAlpha" + GetId ("GUIButtonAlphaUndestroyable");

        return tempGameObject;
	}
    
	public static GameObject CreateImage () {
		
		tempGameObject = PullController.GetObject ("GUIImageUndestroyable");

		if (tempGameObject == null) {

			tempGameObject = GameObject.Instantiate (ResourcesController.Load ("Prefabs/GUI/GUIObject")) as GameObject;
		}
        
		tempGameObject.name = "GUIImage" + GetId ("GUIImageUndestroyable");
		AddToHistory (tempGameObject, DestroyImage);
		return tempGameObject;
	}
    
	public static GameObject CreateAudio () {
		
		tempGameObject = PullController.GetObject ("AudioUndestroyable");

		if (tempGameObject == null) {

			tempGameObject = GameObject.Instantiate (ResourcesController.Load ("Prefabs/Audio")) as GameObject;
		}
        
		tempGameObject.name = "Audio" + GetId ("AudioUndestroyable");
		return tempGameObject;
	}
    
    public static GameObject CreateText () {

		tempGameObject = PullController.GetObject ("TextUndestroyable");

		if (tempGameObject == null) {

            tempGameObject = GameObject.Instantiate (ResourcesController.Load ("Prefabs/GUI/TextMeshObject")) as GameObject;
		}

        tempGameObject.name = "Text" + GetId ("TextUndestroyable");
		AddToHistory (tempGameObject, DestroyText);
		return tempGameObject;

       
    }

    public static GameObject CreateBlock () {

		tempGameObject = PullController.GetObject ("Block");

		if (tempGameObject == null) {

            tempGameObject = GameObject.Instantiate (ResourcesController.Load ("Prefabs/Block")) as GameObject;
		}
        tempGameObject.name = "Block" + GetId ("Block");
		AddToHistory (tempGameObject, DestroyBlock);
		return tempGameObject;

       
    }

    public static GameObject CreateCoolEffect () {

		tempGameObject = PullController.GetObject ("CoolEffect");

		if (tempGameObject == null) {

            tempGameObject = GameObject.Instantiate (ResourcesController.Load ("Prefabs/CoolLetterEffect")) as GameObject;
		}

        tempGameObject.name = "CoolEffect" + GetId ("CoolEffect");
		AddToHistory (tempGameObject, DestroyCoolEffect);
		return tempGameObject;
    }

    
	
	public static void DestroyButton (GameObject gameObject) {
		
		DestroyImage (gameObject);
	}
	
	public static void DestroyImage (GameObject gameObject) {
		
		PullController.AddObject ("GUIImageUndestroyable", gameObject);

        if (gameObject.GetComponent <Renderer> ()) {
                
            if ( gameObject.GetComponent <Renderer> ().material.HasProperty ("_Color")) {

                var color = gameObject.GetComponent <Renderer> ().material.color;
		        gameObject.GetComponent <Renderer> ().material.color = new Color (color.r, color.g, color.b, 1f);
            }
            
            gameObject.GetComponent <Renderer> ().material.SetTextureScale ("_MainTex", new Vector2 (1f, 1f));
            gameObject.GetComponent <Renderer> ().material.SetTextureOffset ("_MainTex", new Vector2 (0f, 0f));
            gameObject.GetComponent<Renderer> ().material.mainTextureOffset = new Vector2 (0, 0);
        }
        
        history.Remove (gameObject);
	}
	
	public static void DestroyButtonAlpha (GameObject gameObject) {
		
		DestroyImageAlpha (gameObject);
	}
	
	public static void DestroyImageAlpha (GameObject gameObject) {
		
		PullController.AddObject ("GUIImageAlphaUndestroyable", gameObject);

        if (gameObject.GetComponent <Renderer> ()) {
                
            if ( gameObject.GetComponent <Renderer> ().material.HasProperty ("_Color")) {

                var color = gameObject.GetComponent <Renderer> ().material.color;
		        gameObject.GetComponent <Renderer> ().material.color = new Color (color.r, color.g, color.b, 1f);
            }
            
            gameObject.GetComponent <Renderer> ().material.SetTextureScale ("_MainTex", new Vector2 (1f, 1f));
            gameObject.GetComponent <Renderer> ().material.SetTextureOffset ("_MainTex", new Vector2 (0f, 0f));
            gameObject.GetComponent<Renderer> ().material.mainTextureOffset = new Vector2 (0, 0);
        }

        history.Remove (gameObject);
	}
	
	public static void DestroyImageAlphaExpensive (GameObject gameObject) {
		
		DestroyImageAlpha (gameObject);
	}
	
	public static void DestroyAudio (GameObject gameObject) {
		
		PullController.AddObject ("AudioUndestroyable", gameObject);
        history.Remove (gameObject);
	}

	public static void DestroyBlock (GameObject gameObject) {
		
        if (gameObject.GetComponent <Rigidbody> ()) {

            GameObject.Destroy (gameObject.GetComponent <Rigidbody> ());
        }

		PullController.AddObject ("Block", gameObject);
        history.Remove (gameObject);
	}

	public static void DestroyText (GameObject gameObject) {
		
        if (gameObject == null) 
            return;

		PullController.AddObject ("TextUndestroyable", gameObject);
        history.Remove (gameObject);
	}
    
	public static void DestroyCoolEffect (GameObject gameObject) {
		
		PullController.AddObject ("CoolEffect", gameObject);
        history.Remove (gameObject);
	}


}
