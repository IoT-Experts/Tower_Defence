using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AchievementController {


    private const int achievementsCount = 50;

    public static AchievementController instance;

    private static int gotAchievements;
    private static GUIObject background;

    private float height = 0;

    public static bool AddProgress (string target, int toAdd) { // true if new achievement

        if (PlayerPrefs.HasKey ("AchievementProgress" + target)) {

            PlayerPrefs.SetInt ("AchievementProgress" + target, PlayerPrefs.GetInt ("AchievementProgress" + target) + toAdd);
        } else {

            PlayerPrefs.SetInt ("AchievementProgress" + target, toAdd);
        }

        return CheckProgress (target);
    }

    public static string AddProgressToGroup (string group, int toAdd) {

        string result = "";

        switch (group) {

            case "Standart":

                for (int i = 1; i <= 6; i++) {

                    if (AddProgress ("Standart" + i, toAdd)) {

                        result = "Standart" + i;
                    }
                }

                break;
        }

        return result;
    } 

    private static bool CheckProgress (string target) {

        if (!PlayerPrefs.HasKey ("AchievementProgress" + target)) {

            return false;
        }

        int result = PlayerPrefs.GetInt ("AchievementProgress" + target);

        bool isGot = IsAchievementGot (target);

        if (isGot) {

            return false;
        }

        switch (target) {
            
            case "Word1":
                isGot = result >= 1;
                break;
            
        }

        if (isGot) {

            PlayerPrefs.SetInt ("AchievementState" + target, 1);
            Settings.diamonds += GetReward (target);
        }

        return isGot;
    }

    private static bool IsAchievementGot (string target) {

        if (PlayerPrefs.HasKey ("AchievementState" + target)) {

            return PlayerPrefs.GetInt ("AchievementState" + target) == 1;
        } 

        return false;
    }


    private string GetAchievementIcon (string target) {
        
        if (target.Contains ("Gold")) {

            return "Textures/Achievements/Gold";
        }
        
        if (target.Contains ("Observe")) {

            return "Textures/Achievements/Observe";
        }
        
        if (target.Contains ("KillBosses")) {

            return "Textures/Achievements/KillBosses";
        }
        
        if (target.Contains ("KillEnemies")) {

            return "Textures/Achievements/KillEnemies";
        }
        
        if (target.Contains ("Build") || target.Contains ("Tower")) {

            return "Textures/Achievements/Build";
        }
        
        Debug.LogWarning ("No Icon for achievement");

        return "Textures/Achievements/Gold";
    }
    
    public static string GetAchievementDescription (string target) {

        return Settings.GetText (target + "Description");
    }

    public static int GetReward (string target) {

        switch (target) {
            
            case "Gold100":
                return 30;
            case "Build500":
                return 100;
        }

        return 97;
    }

    private void DrawAchievement (Vector2 position, string achievement, float deltaLayer) {
        
        new GUIText (GetAchievementDescription (achievement), -1.8f + deltaLayer
            , position + new Vector2 (-7.1f + 5.82f, 1.13f), new Vector2 (0.15f, 0.15f)
            , GUIText.FontName.Font5, false, TextAnchor.MiddleLeft);

        new GUIText ("REWARD: " + GetReward (achievement), -1.8f + deltaLayer
            , position + new Vector2 (-7.97f + 5.82f, -1.76f), new Vector2 (0.11f, 0.11f)
            , GUIText.FontName.Font5, false, TextAnchor.MiddleLeft); 

        new GUIImage ((IsAchievementGot (achievement) ? "Textures/Achievements/Completed" : "Textures/Achievements/Locked")
            , -1.9f - deltaLayer, position + new Vector2 (-9.17f + 5.82f, -1.22f), new Vector2 (-1, -1), false);
        new GUIImage (GetAchievementIcon (achievement), -1.9f - deltaLayer, position + new Vector2 (-9.17f + 5.82f, 1.74f), new Vector2 (-1, -1), false);
    }

    private void CreateLine (Vector2 position, string leftAchievement, string rightAchievement, float deltaLayer) {

        var panel = new GUIImage ("Textures/Achievements/Panel", -2 - deltaLayer, position + new Vector2 (0, 0), new Vector2 (-1, -1), false);

        DrawAchievement (position - new Vector2 (5.82f, 0), leftAchievement, deltaLayer);
        DrawAchievement (position + new Vector2 (5.82f + 0.3f, 0), rightAchievement, deltaLayer);
    }

    private void CreateAchievementLines (float deltaLayer) {

        var lineSize = 350f / 50f;

        List <string> leftAchievements = new List<string> ();
        List <string> rightAchievements = new List<string> ();
        
        leftAchievements.Add ("KillEnemies10"); rightAchievements.Add ("KillBosses10");
        leftAchievements.Add ("Gold100"); rightAchievements.Add ("Build500");
        leftAchievements.Add ("Tower10"); rightAchievements.Add ("Gold500");
        leftAchievements.Add ("Observe"); rightAchievements.Add ("KillBosses25");
        leftAchievements.Add ("KillEnemies100"); rightAchievements.Add ("KillEnemies500");
        leftAchievements.Add ("Towers50"); rightAchievements.Add ("Gold1000");

        for (int i = 0; i < Mathf.Min (leftAchievements.Count, rightAchievements.Count); i++) {
            
            CreateLine (new Vector2 (0, -lineSize * i), leftAchievements [i], rightAchievements [i], deltaLayer);

            height = lineSize * (i + 1);
        }
    }
    
    private void Create () {

        var deltaLayer = 0f;

        background = new GUIImage ("Textures/Achievements/Background", -3 - deltaLayer, CameraController.cameraPosition, new Vector2 (-1, -1), false);
        
        Debug.Log (CameraController.sizeInMeters * 50f);

        new GUIImage ("Textures/Achievements/TopBar", 1280 / 2f, 143 / 2f, null, null, 1280, 143, -0.3f);
        var back = new GUIButton ("Textures/Back", null, 127 / 2f, 85 / 2f + 16, null, 85, 85, -0.2f);
        
        back.OnClick = (b) => {
            
            CameraController.cameraPosition = new Vector2 (0, 0);
            ScenePassageController.instance.LoadScene <SelectLevelController> ((a) => {Destroy (); a(); });
        };
        back.OnButtonDown =  (b) => {

            b += "Pressed";
        };
        back.OnButtonUp = (b) => {

            b -= "Pressed";
        };

        CreateAchievementLines (deltaLayer);
    }

	public AchievementController () {

        instance = this;

        UpdateController.toUpdate = Update;
        UpdateController.toFixedUpdate = FixedUpdate;

        new GUIController ();

        CameraController.ResizeCamera ( Mathf.Min (CameraController.GetWidthInMeters (1080 / 50f * Settings.FhdToHD), 1920 / 50f * Settings.FhdToHD) );
        CameraController.cameraPosition = new Vector2 (0, 0);

        UpdateController.Timer (0.1f, () => {
            
            Create ();
        
            new SlideController (new Vector2 (0, 0), new Vector2 (height, 360 / 50f), SlideController.Mode.Slide, 3);
        });
    }

	private void FixedUpdate (float deltaTime) {
        
    }

	private void Update (float deltaTime) {
		
		if (Input.GetKeyDown(KeyCode.Escape)) {

            if (!MenusController.RemoveMenus ()) {
                
                CameraController.cameraPosition = new Vector2 (0, 0);
			    ScenePassageController.instance.LoadScene <SelectLevelController> ((a) => {Destroy (); a ();});
            }
        }


	    SlideController.frictionDelta = CameraController.widthInMeters/Screen.width;
		SlideController.instance.Update (deltaTime);
		
        if (background != null) {

            background.positionInMeters = CameraController.cameraPosition;
        }
	}

    public void Destroy () {

        UpdateController.StopAllTimers ();
        UpdateController.toFixedUpdate = null;
        UpdateController.toUpdate = null;
        GamePullController.Destroy ();
    }

}
