using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UpdateController : MonoBehaviour {

    public static UpdateController instance;

    private const int version = 2;

    public enum Platform {PC, Phone}
	public static Platform platform;

    public static Actions.VoidFloat toUpdate;
    public static Actions.VoidFloat toFixedUpdate;
    
    private static Dictionary <string, Actions.VoidFloat> updatables;
    private static Dictionary <string, Actions.VoidFloat> fixedUpdatables;
    
    private static Dictionary <string, Actions.VoidVector2> clickListeners;
    private static Dictionary <string, string> clickListenerTypes;

    private static List <string> updatablesToRemove;
    private static List <string> fixedUpdatablesToRemove;
    private static List <string> clickListenersToRemove;

    public static GameObject theme;
    public static string themeName = "";

    public static float timeSinceLevelLoad {

        get {return time - lastSceneLoadTime;}
    }
    
    public static float time {

        get {return Time.time;}
    }

    private static float lastSceneLoadTime = 0;

    public static void OnSceneLoaded () {

        lastSceneLoadTime = Time.time;
        CameraController.cameraPosition = new Vector2 (0, 0);
    }

    private IEnumerator IETimer (float time, Actions.VoidVoid func) {

        yield return new WaitForSeconds (time);

        func ();
    }

    private void _Timer (float time, Actions.VoidVoid func) {

        StartCoroutine (IETimer (time, func));
    }

    public static void Timer (float time, Actions.VoidVoid func) {

        instance._Timer (time, func);
    }

    public static void StopAllTimers () {

        instance.StopAllCoroutines ();
        
        var newFixedUpdatables = new Dictionary<string, Actions.VoidFloat> ();
        var newUpdatables = new Dictionary<string, Actions.VoidFloat> ();
        
        foreach (var f in fixedUpdatables) {

            if (f.Key.Contains ("OnlyDirectRemoving")) {

                newFixedUpdatables.Add (f.Key, f.Value);
            }
        }
        
        foreach (var u in updatables) {

            if (u.Key.Contains ("OnlyDirectRemoving")) {

                newUpdatables.Add (u.Key, u.Value);
            }
        }

        fixedUpdatables = newFixedUpdatables;
        updatables = newUpdatables;
    }

    private IEnumerator IELaunchIt (float count, float deltaTime, Actions.VoidInt func, Actions.VoidVoid onEnd) {

        for (int i = 0; i < count; i++) {

            func (i);
            yield return new WaitForSeconds (deltaTime);
        }

        onEnd ();
    }

    private void _LanunchIt (float count, float deltaTime, Actions.VoidInt func, Actions.VoidVoid onEnd) {

        StartCoroutine (IELaunchIt (count, deltaTime, func, onEnd));
    }

    public static void LaunchIt (float count, float deltaTime, Actions.VoidInt func, Actions.VoidVoid onEnd = null) {

        instance._LanunchIt (count, deltaTime, func, onEnd == null ? () => { } : onEnd);
    }

    public static void AddUpdatable (string key, Actions.VoidFloat value) {

        if (updatables.ContainsKey (key)) {

            Debug.LogWarning ("Reset Updatable");
            updatables [key] = value;
        }
        updatables.Add (key, value);
    }

    public static void AddFixedUpdatable (string key, Actions.VoidFloat value) {

        if (fixedUpdatables.ContainsKey (key)) {

            Debug.LogWarning ("Reset FixedUpdatable");
            fixedUpdatables [key] = value;
        } else {

            fixedUpdatables.Add (key, value);
        }
    }
    
    public static void RemoveUpdatable (string key) {

        updatablesToRemove.Add (key);
    }

    public static void RemoveFixedUpdatable (string key) {

        fixedUpdatablesToRemove.Add (key);
    }

    public static void AddClickListener (string type, string name, Actions.VoidVector2 action) {

        if (clickListeners.ContainsKey (name)) {

            Debug.LogWarning ("Reset ClickListener");
            clickListeners [name] = action;
            clickListenerTypes [name] = type;
        } else {

            clickListeners.Add (name, action);
            clickListenerTypes.Add (name, type);
        }
    }

    public static void RemoveClickListener (string name) {

        clickListenersToRemove.Add (name);
    } 
    
	void Start () {

        Screen.sleepTimeout = SleepTimeout.NeverSleep;
	    
		#if UNITY_IPHONE
		        platform = Platform.Phone;
        #endif

        #if UNITY_ANDROID
		        platform = Platform.Phone;
        #endif

        #if UNITY_STANDALONE_OSX
		        platform = Platform.PC;
        #endif

        #if UNITY_STANDALONE_WIN
		        platform = Platform.PC;
        #endif

        #if UNITY_EDITOR
		        platform = Platform.PC;
        #endif	

        instance = this;

        updatables = new Dictionary<string, Actions.VoidFloat> ();
        fixedUpdatables = new Dictionary<string, Actions.VoidFloat> ();
        clickListeners = new Dictionary<string, Actions.VoidVector2> ();
        clickListenerTypes = new Dictionary<string, string> ();

        updatablesToRemove = new List<string> ();
        fixedUpdatablesToRemove = new List<string> ();
        clickListenersToRemove = new List<string> ();

        new ScenePassageController ();
		new GamePullController ();
        new LanguageController ();
        new Settings ();
        new TranslationsController ();
        new MenusController ();
        new ResourcesController ();
        new AdsController ();
        new IAPController ();
        new LightningController ();
        new GUIController ();
        new SlideController (1920 / 100f * Settings.FhdToHD, 1080 / 100f * Settings.FhdToHD, SlideController.Mode.SlideAndZoom, 3);

        CameraController.ResizeCamera ( Mathf.Min (CameraController.GetWidthInMeters (1080 / 50f * Settings.FhdToHD), 1920 / 50f * Settings.FhdToHD) );
        for (int i = 0; i < 100; i++) {

            LightningController.CreateBolt ();
        }

        AddFixedUpdatable ("LightningControllerFixedUpdate", LightningController.Update);
        
        ScenePassageController.instance.LoadScene <MainMenuController> ();

        Timer (2, () => {

            AdsController.instance.ShowBanner ();
        });
	}

    void FixedUpdate () {

        if (toFixedUpdate != null) {

            toFixedUpdate (Time.fixedDeltaTime);
        }

        foreach (var f in fixedUpdatables) {

            f.Value (Time.fixedDeltaTime);
        }

        foreach (var f in fixedUpdatablesToRemove) {

            fixedUpdatables.Remove (f);
        }

        fixedUpdatablesToRemove.Clear ();
    }

	void Update () {
	
        if (toUpdate != null) {

            toUpdate (Time.deltaTime);
        }

        foreach (var u in updatables) {

            u.Value (Time.deltaTime);
        }

        foreach (var u in updatablesToRemove) {

            updatables.Remove (u);
        }

        updatablesToRemove.Clear ();
        
	}

    private static void CallClickListeners (string type, Vector2 target) {

        foreach (var c in clickListenersToRemove) {
            
            clickListeners.Remove (c);
            clickListenerTypes.Remove (c);
        }

        clickListenersToRemove.Clear ();

        foreach (var c in clickListenerTypes) {

            if (c.Value == type) {

                clickListeners [c.Key] (target);
            }
        }
    }
    
    public static void OnClick (Vector2 target) {
        
        CallClickListeners ("OnClick", target);
    }
    
    public static void OnOver (Vector2 target) {
        
        CallClickListeners ("OnOver", target);
    }
    
    public static void OnButtonDown (Vector2 target) {
        
        CallClickListeners ("OnButtonDown", target);
    }
    
    public static void OnButtonUp (Vector2 target) {
        
        CallClickListeners ("OnButtonUp", target);
    }
}
