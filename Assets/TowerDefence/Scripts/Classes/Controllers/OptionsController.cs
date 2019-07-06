using UnityEngine;
using System.Collections;

public class OptionsController {

    public static OptionsController instance;

	public static GUIImage underSettings;
	private static GUIButton underSettingsExit;
	private static GUIButton underSettingsMusic;
	private static GUIButton underSettingsSounds;
	private static GUIButton underSettingsReplay;
	public static bool isUnderSettingsOpening = false;

    public static float underSettingsPosition {

		get { return 1 - underSettings.gameObject.GetComponent<Renderer> ().material.mainTextureOffset.y / (-0.87f); }

		set {
			underSettings.gameObject.GetComponent<Renderer> ().material.mainTextureOffset = new Vector2 (0, -0.87f*(1-value));
			
			underSettingsExit.top = (0 + 226/4f)*(1-value) + (0 + 226/4f + 500 / 2f)*value;
			if (value >= 0.2f)
				underSettingsMusic.top = (0 + 226/4f)*(1-value) + (0 + 226/4f + 500 / 2f)*value - 135 / 2f;
			
			if (value >= 0.4f)
				underSettingsSounds.top = (0 + 226/4f)*(1-value) + (0 + 226/4f + 500 / 2f)*value - 250 / 2f;
			
			if (value >= 0.7f && underSettingsReplay != null)
				underSettingsReplay.top = (0 + 226/4f)*(1-value) + (0 + 226/4f + 500 / 2f)*value - 370 / 2f;

		}
		
	}

    public OptionsController (Actions.VoidVoid onExit, Actions.VoidVoid onReplay = null, bool isCreateReplay = true, float deltaLayer = -1f) {

        instance = this;

        isUnderSettingsOpening = false;

        var options = new GUIButton ("Textures/UserInterface/Settings1", null, (0 + 226/4f), (10+127/4f), null, 127 / 2f, 127 / 2f, deltaLayer, true);

		options.OnClick = (t) => {
            
			GameController.isPaused = !GameController.isPaused;

			isUnderSettingsOpening = !isUnderSettingsOpening;

			if (isUnderSettingsOpening) 
				options.texture = Resources.Load ("Textures/UserInterface/Settings2") as Texture;
			else
				options.texture = Resources.Load ("Textures/UserInterface/Settings1") as Texture;
		};
		underSettings = new GUIImage ("Textures/UserInterface/UnderSettings", null, (0 + 226/2f + 35 + 612/2f) / 2f 
            , (10+127/4f), null, 165 / 2f, 612 / 2f, -1 - 1f, true);
		underSettings.gameObject.GetComponent <BoxCollider> ().enabled = false;
		underSettingsExit = new GUIButton ("Textures/UserInterface/Exit", null, (0 + 226/4f), (10+127/4f), null, 81 / 2f, 92 / 2f, -0.5f + deltaLayer, true);
		underSettingsExit.OnClick = (t) => {

            onExit ();
		};
		underSettingsMusic = new GUIButton ("Textures/UserInterface/Music"+(Settings.music?"":"Off"), null, (0 + 226/4f)
            , (10+127/4f), null, 59 / 2f, 80 / 2f, -0.5f + deltaLayer, true);
		underSettingsMusic.OnClick = (t) => {

			Settings.music = !Settings.music;
			
			if (Settings.music)
				underSettingsMusic.texture = Resources.Load ("Textures/UserInterface/Music") as Texture;
			else
				underSettingsMusic.texture = Resources.Load ("Textures/UserInterface/MusicOff") as Texture;
		};
		underSettingsSounds = new GUIButton ("Textures/UserInterface/Sounds"+(Settings.sounds?"":"Off"), null
            , (0 + 226/4f), (10+127/4f), null, 68 / 2f, 70 / 2f, -0.5f + deltaLayer, true);
		underSettingsSounds.OnClick = (t) => { 
			
			Settings.sounds = !Settings.sounds;
			
			if (Settings.sounds)
				underSettingsSounds.texture = Resources.Load ("Textures/UserInterface/Sounds") as Texture;
			else
				underSettingsSounds.texture = Resources.Load ("Textures/UserInterface/SoundsOff") as Texture;
		};

        if (isCreateReplay) {

            underSettingsReplay = new GUIButton ("Textures/UserInterface/Replay", null, (0 + 226/4f)
                , (10+127/4f), null, 75 / 2f, 64 / 2f, -0.5f + deltaLayer, true);
		    underSettingsReplay.OnClick = (t) => {

                if (onReplay != null) {

                    onReplay ();
                }
			    //TODO
		    };
        } else {

            underSettingsReplay = null;
        }
		underSettingsPosition = 0;


        UpdateController.AddFixedUpdatable ("OptionsControllerFixedUpdate", Update);
    }

    public void Update (float deltaTime) {

        if (isUnderSettingsOpening && underSettingsPosition < 1) {
			underSettingsPosition += 0.04f;
		} 
		
		if (!isUnderSettingsOpening && underSettingsPosition > 0) {
			underSettingsPosition -= 0.04f;
		} 
    }

    public void Destroy () {

        UpdateController.RemoveFixedUpdatable ("OptionsControllerFixedUpdate");
    }
}
