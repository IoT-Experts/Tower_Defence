using UnityEngine;
using System.Collections;

public class ScenePassageController {

	public static ScenePassageController instance = null;

    private Actions.VoidVoid onEnd;

    private float loadProgress; // 0..1
    
    private GUIImage top;
    private GUIImage bottom;
    private GUIImage circle;

    public float animationTime = 3f;

    bool isCreated;

	public static void OnSceneLoaded () {

        
	}

	public void LoadScene < SceneController > (Actions.VoidAction beforeEnd = null) where SceneController : new () {
        
        if (loadProgress > 0 && loadProgress < 1) {

            return;
        }

        if (beforeEnd == null) {

            beforeEnd = (a) => { 

                a ();
            };
        }

        Create ();

        loadProgress = 0;
        isCreated = false;

        onEnd = () => {

            beforeEnd ( () => {
                
                if (AudioController.instance != null) {

                    AudioController.instance.ClearSounds ();
                }

			    new SceneController ();
                UpdateController.OnSceneLoaded ();
		        
                OnSceneLoaded ();
            });
        };

        UpdateController.AddFixedUpdatable ("ScenePassageControllerUpdateOnlyDirectRemoving", Update);
	}

	public ScenePassageController () {

		if (instance != null) {

			return;
		}

		instance = this;
        onEnd = () => { };
	}

    private void Create () {
        
        top = new GUIImage ("Textures/ScenePassage/Top", -0.1f, new Vector2 (0, 100), new Vector2 (-1, -1), false);
        bottom = new GUIImage ("Textures/ScenePassage/Bottom", -0.15f, new Vector2 (0, -100), new Vector2 (-1, -1), false);
        circle = new GUIImage ("Textures/ScenePassage/Circle", -0.05f, new Vector2 (0, -100), new Vector2 (-1, -1), false);
        
        top.isOnlyDirectDestroying = true;
        bottom.isOnlyDirectDestroying = true;
        circle.isOnlyDirectDestroying = true;
    }

    private void Destroy () {

        top.Destroy ();
        bottom.Destroy ();
        circle.Destroy ();
    }

    private void OnProgress (float progress) {

        if (progress <= 0.33f) {

            top.positionInMeters = new Vector2 (0, 1.56f * progress / 0.33f + 20f * (0.33f - progress) / 0.33f) * Settings.FhdToHD;
            circle.positionInMeters = top.positionInMeters - new Vector2 (0, 1.55f) * Settings.FhdToHD;
            bottom.positionInMeters = new Vector2 (0, -5.36f * progress / 0.33f -16.5f * (0.33f - progress) / 0.33f) * Settings.FhdToHD;

            return;
        }

        if (progress <= 0.66f) {

            top.positionInMeters = new Vector2 (0, 1.56f) * Settings.FhdToHD;
            circle.positionInMeters = top.positionInMeters - new Vector2 (0, 1.55f) * Settings.FhdToHD;
            bottom.positionInMeters = new Vector2 (0, -5.36f) * Settings.FhdToHD;

            circle.rotation = (progress - 0.33f) * 2 * 360;
            return;
        }

        top.positionInMeters = new Vector2 (0, 1.56f * (1 - progress) / 0.33f + 20f * (progress - 0.66f) / 0.33f) * Settings.FhdToHD;
        circle.positionInMeters = top.positionInMeters - new Vector2 (0, 1.55f) * Settings.FhdToHD;
        bottom.positionInMeters = new Vector2 (0, -5.36f * (1 - progress) / 0.33f - 16.5f * (progress - 0.66f) / 0.33f) * Settings.FhdToHD;
    }


    private void Update (float deltaTime) {
	    
        loadProgress += deltaTime / animationTime;

        OnProgress (loadProgress);

        if (loadProgress >= 0.35f) {
            
            if (!isCreated) {

                onEnd ();
                isCreated = true;
            }
        }

        if (loadProgress >= 1f) {

            UpdateController.RemoveFixedUpdatable ("ScenePassageControllerUpdateOnlyDirectRemoving");
            Destroy ();
        }
    }
}
