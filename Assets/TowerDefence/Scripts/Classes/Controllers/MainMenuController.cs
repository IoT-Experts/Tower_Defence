using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

public class MainMenuController {

    public static MainMenuController instance;

	private static GUIObject button = null;
    private static GUIObject startArrow = null;
    private static GUIObject secondArrow = null;
    private static GUIButton start;
    private static GUIObject text;
    private static GUIObject selection;
    private static GUIObject cogwheel1;
    private static GUIObject cogwheel2;
    private static GUIObject cogwheel3;

    public List <IUpdatable> updatables;

    private bool isNextSceneLoaded;
	
	private static float _cogwheelsRotation;
    private static float cogwheelsRotation { // 0..1, converted to -0..360 grad

        get { return _cogwheelsRotation; }
        set {

            _cogwheelsRotation = value;

            if (cogwheel1 != null) {

                cogwheel1.rotation = _cogwheelsRotation * 360;
                cogwheel2.rotation = _cogwheelsRotation * -360;
                cogwheel3.rotation = _cogwheelsRotation * 360;
            }
        }

    }
    
    private static float cogwheelsSpeed = 0.14f;
    private static float cogwheelsTargetSpeed = 0.14f;

    private static float _secondArrowRotation;
    private static float secondArrowMaxCooldown = 10f;
    private static float secondArrowCooldown;
    private static float secondArrowRotation { // 0..1, converted to -0..360 grad

        get { return _secondArrowRotation; }
        set {

            _secondArrowRotation = value;

            while (_secondArrowRotation > 1)
                _secondArrowRotation -=1;
            
            while (_secondArrowRotation < 0)
                _secondArrowRotation +=1;

            secondArrow.gameObject.transform.rotation = Quaternion.Euler (new Vector3 (90, _secondArrowRotation * 360, 0));
        }

    }

    private static float _secondArrowTargetRotation;
    private static float secondArrowTargetRotation { // 0..1. secondArrowRotation goes to it's value over time

        get { return _secondArrowTargetRotation; }
        set {

            _secondArrowTargetRotation = value;

            while (_secondArrowTargetRotation > 1)
                _secondArrowTargetRotation -=1;
            
            while (_secondArrowTargetRotation < 0)
                _secondArrowTargetRotation +=1;
        }
    }
    private static float secondArrowSpeed; //(0..1)/seconds. Speed of secondArrow rotating to the secondArrowTargetRotation
    

    private static float _loadingPosition;
    private static float loadingPosition { //0..1, converted to -53..53 grad to match startArrow's angles

        get { return _loadingPosition; }
        set {

            _loadingPosition = value;
            startArrow.gameObject.transform.rotation = Quaternion.Euler (new Vector3 (90, 53 * 2 * (_loadingPosition - 0.5f), 0));
        }
    }
    private static float loaded; //0..1, how much is loaded. loadingPosition goes to it's value over time

   
    public void OnClick () {

        cogwheelsTargetSpeed = - cogwheelsTargetSpeed;
    }
    
    private float bubblesMaxCooldown = 5f;
    private float bubblesCooldown;

	private void UpdateLanguage () {

        start.texture = Resources.Load ("$" + Settings.language + "/Interface/StartMenu/Start") as Texture;
		text.texture = Resources.Load ("$" + Settings.language + "/Interface/StartMenu/Text") as Texture;

        text.sizeInMeters = new Vector2 (-1, -1);
        
        selection.positionInMeters = new Vector2 (-14.613f, Settings.language == "English"?1.78f : -3.52f) * Settings.FhdToHD;
        text.positionInMeters = new Vector2 (-14.63f, Settings.language == "English"?7.67f : 7.73f) * Settings.FhdToHD;
    }

    private void MakeBubble () {

        bubblesCooldown = bubblesMaxCooldown;
        new Bubble (new Vector2 (16.73f + (UnityEngine.Random.value - 0.5f) * (1.7f), -10.16f) * Settings.FhdToHD, 4.5f * Settings.FhdToHD, -4.1f
            , (UnityEngine.Random.value > 0.5f?1:-1) * 0.7f, 3f);
    }

    private void Create () {

        if (UpdateController.themeName != "GlobalMapTheme") {

            if (UpdateController.theme != null) {

                AudioController.instance.RemoveAudio (UpdateController.theme);
            }

            UpdateController.theme = AudioController.instance.CreateAudio ("GlobalMapTheme", true, true);
            UpdateController.themeName = "GlobalMapTheme";
        }
        
		var background0 = new GUIImage ();
		background0.texture = Resources.Load ("Interface/StartMenu/Background0") as Texture;
        background0.layer = -5;
        background0.sizeInMeters = new Vector2 (-1, -1);
        background0.positionInMeters = new Vector2 (0, 0) * Settings.FhdToHD;
        
		var background1 = new GUIImage ();
		background1.texture = Resources.Load ("Interface/StartMenu/Background1") as Texture;
        background1.layer = -3;
        background1.sizeInMeters = new Vector2 (-1, -1);
        background1.positionInMeters = new Vector2 (0, 0) * Settings.FhdToHD;

		var greenWater = new GUIButtonAlpha ();
		greenWater.texture = Resources.Load ("Interface/StartMenu/GreenWater") as Texture;
        greenWater.layer = -4;
        greenWater.sizeInMeters = new Vector2 (-1, -1);
        greenWater.positionInMeters = new Vector2 (16.73f, -7.5f) * Settings.FhdToHD;
        greenWater.gameObject.name += "Through";
        greenWater.OnClick = (t) => {

            MakeBubble ();
        };
        
		var red = new GUIImage ();
		red.texture = Resources.Load ("Interface/StartMenu/Red") as Texture;
        red.layer = -4;
        red.sizeInMeters = new Vector2 (-1, -1);
        red.positionInMeters = new Vector2 (-3.94f, -0.25f) * Settings.FhdToHD;

		var green = new GUIImage ();
		green.texture = Resources.Load ("Interface/StartMenu/Green") as Texture;
        green.layer = -4;
        green.sizeInMeters = new Vector2 (-1, -1);
        green.positionInMeters = new Vector2 (6.02f, -0.25f) * Settings.FhdToHD;
        
        
        
		var redHotspot = new GUIImage ();
		redHotspot.texture = Resources.Load ("Interface/StartMenu/Hotspot") as Texture;
        redHotspot.layer = -3.9f;
        redHotspot.sizeInMeters = new Vector2 (-1, -1);
        redHotspot.positionInMeters = new Vector2 (-4.24f, -0.19f) * Settings.FhdToHD;
        
		var greenHotspot = new GUIImage ();
		greenHotspot.texture = Resources.Load ("Interface/StartMenu/Hotspot") as Texture;
        greenHotspot.layer = -3.9f;
        greenHotspot.sizeInMeters = new Vector2 (-1, -1);
        greenHotspot.positionInMeters = new Vector2 (5.73f, -0.19f) * Settings.FhdToHD;
        
		startArrow = new GUIImage ();
		startArrow.texture = Resources.Load ("Interface/StartMenu/Arrow1") as Texture;
        startArrow.layer = -3.8f;
        startArrow.sizeInMeters = new Vector2 (-1, -1);
        startArrow.positionInMeters = new Vector2 (1.06f, -2.22f) * Settings.FhdToHD;

		secondArrow = new GUIImage ();
		secondArrow.texture = Resources.Load ("Interface/StartMenu/Arrow0") as Texture;
        secondArrow.layer = -3.8f;
        secondArrow.sizeInMeters = new Vector2 (-1, -1);
        secondArrow.positionInMeters = new Vector2 (-6.9f, 7.74f) * Settings.FhdToHD;
        
		start = new GUIButton ();
		start.texture = Resources.Load ("$" + Settings.language + "/Interface/StartMenu/Start") as Texture;
        start.layer = -2;
        start.sizeInMeters = new Vector2 (-1, -1);
        start.positionInMeters = new Vector2 (1.08f, -5.3f) * Settings.FhdToHD;
        start.OnButtonDown = (t) => {

            start.texture = Resources.Load ("$" + Settings.language + "/Interface/StartMenu/StartPushed") as Texture;
        };
        start.OnButtonUp = (t) => {

            start.texture = Resources.Load ("$" + Settings.language + "/Interface/StartMenu/Start") as Texture;
        };
        start.OnClick = (t) => {
            
            if (!GamePullController.isPreordered) {

                var toPreorder = new List <GamePullController.PreoderItem> ();
        
                toPreorder.Add (new GamePullController.PreoderItem ("GUIImage", 70, () => {}));
                toPreorder.Add (new GamePullController.PreoderItem ("Text", 10, () => {}));
                toPreorder.Add (new GamePullController.PreoderItem ("GUIImageAlpha", 4, () => {}));
                //toPreorder.Add (new GamePullController.PreoderItem ("Audio", 0, () => {}));

                GamePullController.Preorder (toPreorder, (done, total) => {
                    
                    loaded = done * 1f / total;
                }, () => {

                    isNextSceneLoaded = true;
                });
            } else {

                UpdateController.LaunchIt (10, 0.02f, (q) => {

                    loaded = q / 9f;   
                }, () => {

                    isNextSceneLoaded = true;
                });
            }
        };

        
		cogwheel1 = new GUIImage ();
		cogwheel1.texture = Resources.Load ("Interface/StartMenu/Cogwheel1") as Texture;
        cogwheel1.layer = -4.9f;
        cogwheel1.sizeInMeters = new Vector2 (-1, -1);
        cogwheel1.positionInMeters = new Vector2 (16.52f, 7.42f) * Settings.FhdToHD;
        
		cogwheel2 = new GUIImage ();
		cogwheel2.texture = Resources.Load ("Interface/StartMenu/Cogwheel2") as Texture;
        cogwheel2.layer = -4;
        cogwheel2.sizeInMeters = new Vector2 (-1, -1);
        cogwheel2.positionInMeters = new Vector2 (13.82f, 3.25f) * Settings.FhdToHD;

		cogwheel3 = new GUIImage ();
		cogwheel3.texture = Resources.Load ("Interface/StartMenu/Cogwheel0") as Texture;
        cogwheel3.layer = -4;
        cogwheel3.sizeInMeters = new Vector2 (-1, -1);
        cogwheel3.positionInMeters = new Vector2 (9.38f, 9.05f) * Settings.FhdToHD;

        
		text = new GUIImage ();
		text.texture = Resources.Load ("$" + Settings.language + "/Interface/StartMenu/Text") as Texture;
        text.layer = -2;
        text.sizeInMeters = new Vector2 (-1, -1);
        text.positionInMeters = new Vector2 (-14.63f, 7.67f) * Settings.FhdToHD;
        
        
		var englishBox = new GUIButton  ();
		englishBox.texture = Resources.Load ("$English/Interface/StartMenu/Box") as Texture;
        englishBox.layer = -2;
        englishBox.sizeInMeters = new Vector2 (-1, -1);
        englishBox.positionInMeters = new Vector2 (-14.63f, 0.5f) * Settings.FhdToHD;
        englishBox.OnButtonDown = (t) => {

            englishBox.texture = Resources.Load ("$English/Interface/StartMenu/BoxPushed") as Texture;
        };
        englishBox.OnButtonUp = (t) => {

            englishBox.texture = Resources.Load ("$English/Interface/StartMenu/Box") as Texture;
        };
        englishBox.OnClick = (t) => {
            
            if (Settings.language == "English")
                return;

            RotateSecondArrow ();
            Settings.language = "English";
            UpdateLanguage ();
        };

        var russianBox = new GUIButton  ();
		russianBox.texture = Resources.Load ("$Russian/Interface/StartMenu/Box") as Texture;
        russianBox.layer = -2;
        russianBox.sizeInMeters = new Vector2 (-1, -1);
        russianBox.positionInMeters = new Vector2 (-14.63f, -4.8f) * Settings.FhdToHD;
        russianBox.OnButtonDown = (t) => {

            russianBox.texture = Resources.Load ("$Russian/Interface/StartMenu/BoxPushed") as Texture;
        };
        russianBox.OnButtonUp = (t) => {

            russianBox.texture = Resources.Load ("$Russian/Interface/StartMenu/Box") as Texture;
        };
        russianBox.OnClick = (t) => {
            
            if (Settings.language == "Russian")
                return;

            RotateSecondArrow ();
            Settings.language = "Russian";
            UpdateLanguage ();
        };

        
		selection = new GUIImage ();
		selection.texture = Resources.Load ("Interface/StartMenu/Selection") as Texture;
        selection.layer = -1.5f;
        selection.sizeInMeters = new Vector2 (-1, -1);
        selection.positionInMeters = new Vector2 (-14.613f, 1.78f) * Settings.FhdToHD;
        
        UpdateLanguage ();

        UpdateController.Timer (0.1f, () => {

            new OptionsController (() => {

                Application.Quit ();
            }, null, false);
        });

        loadingPosition = 0;
        loaded = 0;
        cogwheelsRotation = 0;
        secondArrowRotation = 0;
        secondArrowTargetRotation = 0;
        secondArrowSpeed = 0.2f;
        secondArrowCooldown = secondArrowMaxCooldown;
        bubblesCooldown = bubblesMaxCooldown;
    }

	void OnGUI () {

		//TODO settingsPath = GUI.TextField (new Rect (0, 0, 800, 50), settingsPath);
	}

	private void RotateSecondArrow () {

        
        secondArrowTargetRotation = UnityEngine.Random.Range (30, 70) / 100f + secondArrowRotation;
        secondArrowSpeed = UnityEngine.Random.Range (70, 90) / 100f;
        secondArrowCooldown = secondArrowMaxCooldown;
    }

    public MainMenuController () {

        instance = this;

        updatables = new List<IUpdatable> ();

        UpdateController.toUpdate = Update;
        UpdateController.toFixedUpdate = FixedUpdate;

        new SlideController (1920 / 100f * Settings.FhdToHD, 1080 / 100f * Settings.FhdToHD, SlideController.Mode.SlideAndZoom, 3);
        new GUIController ();

        CameraController.ResizeCamera ( Mathf.Min (CameraController.GetWidthInMeters (1080 / 50f * Settings.FhdToHD), 1920 / 50f * Settings.FhdToHD) );

        isNextSceneLoaded = false;

        Create ();
    }

    private void FixedUpdate (float deltaTime) {

        for (int i = updatables.Count - 1; i >= 0; i--) {

            updatables [i].Update (deltaTime);
        }

        secondArrowCooldown -= deltaTime;
        if (secondArrowCooldown <= 0) {
            
            RotateSecondArrow ();
        }
        bubblesCooldown -= deltaTime;
        if (bubblesCooldown <= 0) {

            MakeBubble ();
        }

        if (cogwheelsSpeed != cogwheelsTargetSpeed)
            cogwheelsSpeed += Mathf.Sign (cogwheelsTargetSpeed - cogwheelsSpeed) * 0.005f;

        if (loadingPosition < loaded) {

            loadingPosition += deltaTime * 0.44f;
        } else {

            if (isNextSceneLoaded) {
                
                ScenePassageController.instance.LoadScene <SelectLevelController> ((a) => { Destroy (); a (); });
                isNextSceneLoaded = false;
            }
        }

        cogwheelsRotation += deltaTime * cogwheelsSpeed;
        


        if (Mathf.Abs (secondArrowRotation - secondArrowTargetRotation) > 0.01f) 
            secondArrowRotation += Mathf.Sign (secondArrowTargetRotation - secondArrowRotation) * Mathf.Sqrt (Mathf.Abs (secondArrowTargetRotation - secondArrowRotation))
                * secondArrowSpeed * deltaTime;

    }

	private void Update (float deltaTime) {
		
		if (UpdateController.platform == UpdateController.Platform.Phone) {
			if (Input.GetKeyDown (KeyCode.Escape))
				Application.Quit ();
		}

		AnimationController.Update (deltaTime);
		
		SlideController.frictionDelta = CameraController.widthInMeters/Screen.width;
		SlideController.instance.Update (deltaTime);
	}

    public void Destroy () {
        
        cogwheel1.rotation = 0;
        cogwheel2.rotation = 0;
        cogwheel3.rotation = 0;
        cogwheel1 = null;
        cogwheel2 = null;
        cogwheel3 = null;
        UpdateController.StopAllTimers ();
        UpdateController.toFixedUpdate = null;
        UpdateController.toUpdate = null;
        GamePullController.Destroy ();
    }
}
