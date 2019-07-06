using UnityEngine;
using System.Collections;

public class SelectLevelController {
    
    public static SelectLevelController instance;

    public const int maxLevel = 9;

    private GUIImage selected;

    Vector2 LevelPlacePosition (int levelNumber) {

        switch (levelNumber) {

            case 0: return new Vector2 (0, 0);
            case 1: return new Vector2 (-15.47f, -1.4f) * Settings.FhdToHD;
            case 2: return new Vector2 (-7.9f, -1.17f) * Settings.FhdToHD;
            case 3: return new Vector2 (1.82f, -4.22f) * Settings.FhdToHD;
            case 4: return new Vector2 (-0.67f, 3.03f) * Settings.FhdToHD;
            case 5: return new Vector2 (-9.32f, 6f) * Settings.FhdToHD;
            case 6: return new Vector2 (-1.5f, 8.15f) * Settings.FhdToHD;
            case 7: return new Vector2 (8.1f, 8.13f) * Settings.FhdToHD;
            case 8: return new Vector2 (14.89f, 5.5f) * Settings.FhdToHD;
            case 9: return new Vector2 (15.5f, 0.4f) * Settings.FhdToHD;
        }

        return new Vector2 (0, 0);
    }

    private Vector2 LinePosition (int index) {

        switch (index) {

            case 0: return new Vector2 (0, 0);
            case 1: return new Vector2 (-15.47f, -1.4f) * Settings.FhdToHD;
            case 2: return new Vector2 (-7.9f, -1.17f) * Settings.FhdToHD;
            case 3: return new Vector2 (1.82f, -4.22f) * Settings.FhdToHD;
            case 4: return new Vector2 (-0.67f, 3.03f) * Settings.FhdToHD;
            case 5: return new Vector2 (-9.32f, 6f) * Settings.FhdToHD;
            case 6: return new Vector2 (-1.5f, 8.15f) * Settings.FhdToHD;
            case 7: return new Vector2 (8.1f, 8.13f) * Settings.FhdToHD;
            case 8: return new Vector2 (14.89f, 5.5f) * Settings.FhdToHD;
            case 9: return new Vector2 (15.5f, 0.4f) * Settings.FhdToHD;
        }

        return new Vector2 (0, 0);
    }

    private void CreateLevelLine (int index) {

        //var line = new GUIImage ("Textures/SelectLevel/Lines/" + (index - 1), -0.9f - 3f, LinePosition (index), new Vector2 (-1, -1));
        //Debug.Log (line.gameObject.name);
    }

    private void CreateLevelPlace (int levelNumber) {

        if (levelNumber < Settings.level) {

            new GUIButton ("Textures/SelectLevel/MapElements/Achieved", -0.5f - 3f, LevelPlacePosition (levelNumber), new Vector2 (-1, -1), true, (b) => {
                
                Settings.levelType = Settings.LevelType.Normal;
                LevelController.level = levelNumber;
                ScenePassageController.instance.LoadScene <GameController> ((a) => { Destroy (); a (); });
            }, (b) => {

                b += "Selected";
            }, (b) => {
                
                b -= "Selected";
            });
            
            int stars = Settings.GetStars (levelNumber);

            if (stars > 0) {

                new GUIImage ("Textures/SelectLevel/MapElements/Star", -0.4f - 3f, LevelPlacePosition (levelNumber) + new Vector2 (-1.3f + 0.446f, 2.74f - 2.02f)
                    , new Vector2 (-1, -1)).rotation = 24.65f;
            }
            
            if (stars > 1) {

                new GUIImage ("Textures/SelectLevel/MapElements/Star", -0.4f - 3f, LevelPlacePosition (levelNumber) + new Vector2 (-0.89f + 0.446f, 2.89f - 2.02f)
                    , new Vector2 (-1, -1)).rotation = 11.17f;
            }
                
            if (stars > 2) {

                new GUIImage ("Textures/SelectLevel/MapElements/Star", -0.4f - 3f, LevelPlacePosition (levelNumber) + new Vector2 (0, 2.94f - 2.02f)
                    , new Vector2 (-1, -1));
            }
                    
            if (stars > 3) {

                new GUIImage ("Textures/SelectLevel/MapElements/Star", -0.4f - 3f, LevelPlacePosition (levelNumber) + new Vector2 (0.89f - 0.446f, 2.89f - 2.02f)
                    , new Vector2 (-1, -1)).rotation = -11.17f;
            }

            if (stars > 4) {

                new GUIImage ("Textures/SelectLevel/MapElements/Star", -0.4f - 3f, LevelPlacePosition (levelNumber) + new Vector2 (1.3f - 0.446f, 2.74f - 2.02f)
                    , new Vector2 (-1, -1)).rotation = -24.65f;
            }

        } else {

            if (levelNumber == Settings.level) {

                new GUIButton ("Textures/SelectLevel/MapElements/Current", -0.5f - 3f, LevelPlacePosition (levelNumber), new Vector2 (-1, -1), true, (b) => {
                    
                    Settings.levelType = Settings.LevelType.Normal;
                    LevelController.level = levelNumber;
                    ScenePassageController.instance.LoadScene <GameController> ((a) => { Destroy (); a (); });
                }, (b) => {

                    b += "Selected";
                }, (b) => {
                
                    b -= "Selected";
                });

            } else {

                new GUIImage ("Textures/SelectLevel/MapElements/Future", -0.5f - 3f, LevelPlacePosition (levelNumber), new Vector2 (-1, -1), true);
            }

        }

    }

    private void Create () {

        if (UpdateController.themeName != "GlobalMapTheme") {
            
            if (UpdateController.theme != null) {

                AudioController.instance.RemoveAudio (UpdateController.theme);
            }

            UpdateController.theme = AudioController.instance.CreateAudio ("GlobalMapTheme", true, true);
            UpdateController.themeName = "GlobalMapTheme";
        }

        Vector2 territoryPosition = new Vector2 (0, 0);
        Vector2 selectedPosition = new Vector2 (0, 0);

        if (Settings.level > 9) {

            Settings.level = 9;
        }

        switch (Settings.level) {
            
            case 1: territoryPosition = new Vector2 (0, 0); selectedPosition = new Vector2 (-15.25f, -5.03f); break;
            case 2: territoryPosition = new Vector2 (0, 0); selectedPosition = new Vector2 (-8.3f, -5.03f); break;
            case 3: territoryPosition = new Vector2 (0, 0); selectedPosition = new Vector2 (7.113f, -5.42f); break;
            case 4: territoryPosition = new Vector2 (0, 0); selectedPosition = new Vector2 (-1.9f, 1.74f); break;
            case 5: territoryPosition = new Vector2 (5.139f, 0.03f); selectedPosition = new Vector2 (-12.43f, 5.19f); break;
            case 6: territoryPosition = new Vector2 (10.17f, 0.03f); selectedPosition = new Vector2 (-1.49f, 7.01f); break;
            case 7: territoryPosition = new Vector2 (11.8f, -1.27f); selectedPosition = new Vector2 (10.27f, 5.78f); break;
            case 8: territoryPosition = new Vector2 (11.8f, -3.15f); selectedPosition = new Vector2 (14.12f, 5.03f); break;
            case 9: selectedPosition = new Vector2 (11.92f, -3.15f); break;
        }

        territoryPosition *= Settings.FhdToHD;
        selectedPosition *= Settings.FhdToHD;

        new GUIImage ("Textures/SelectLevel/MapElements/Background", -3 - 3f, new Vector2 (0, 0), new Vector2 (-1, -1));
        new GUIImage ("Textures/SelectLevel/MapElements/LevelPlaces", -2 - 3f, new Vector2 (0, 0), new Vector2 (-1, -1));
        selected = new GUIImageAlpha ("Textures/SelectLevel/MapElements/" + Settings.level + "Selected", -1.5f - 3f, selectedPosition, new Vector2 (-1, -1));    

        if (Settings.level < maxLevel) {

            new GUIImage ("Textures/SelectLevel/MapElements/" + Settings.level, -1 - 3f, territoryPosition, new Vector2 (-1, -1));
        }

        for (int i = 1; i <= maxLevel; i++) {

            CreateLevelPlace (i);
        }

        for (int i = 1; i < maxLevel; i++) {

            CreateLevelLine (i);
        }
        
        CreateInterface ();
    }

    private void CreateInterface () {

        new OptionsController (() => {

            ScenePassageController.instance.LoadScene <MainMenuController> ((a) => {instance.Destroy (); a ();});
        }, null, false, -1.5f);
        
        new GUIImage ("Textures/SelectLevel/Interface/RightBar", null, null, 10 + 541 / 4f, 10 + 315 / 4f, 541 / 2f, 315 / 2f, -3f, true);
        new GUIButton ("Textures/SelectLevel/Interface/HellStation", null, null, 44 + 180 / 4f, 28 + 315 / 4f, 180 / 2f, 180 / 2f, -2.5f, true
            , (b) => {

                Settings.levelType = Settings.LevelType.Endless;
                LevelController.level = 4;
                ScenePassageController.instance.LoadScene <GameController> ((a) => { Destroy (); a (); });
            }, (b) => {

                b += "Pushed";
            }, (b) => {

                b -= "Pushed";
            }
        );
        new GUIButton ("Textures/SelectLevel/Interface/Achievments", null, null, 44 + 112 + 180 / 4f, 28 + 315 / 4f, 180 / 2f, 180 / 2f, -2.5f, true
            , (b) => {
                
                ScenePassageController.instance.LoadScene <AchievementController> ((a) => { Destroy (); a (); });
            }, (b) => {

                b += "Pushed";
            }, (b) => {

                b -= "Pushed";
            }
        );

        new GUIImage ("Textures/SelectLevel/Interface/LeftBar", 10 + 541 / 4f, null, null, 10 + 315 / 4f, 541 / 2f, 315 / 2f, -3f, true);
        new GUIButton ("Textures/SelectLevel/Interface/Upgrades", 44 + 180 / 4f, null, null, 28 + 315 / 4f, 180 / 2f, 180 / 2f, -2.5f, true
            , (b) => {

                ScenePassageController.instance.LoadScene <UpgradeController> ((a) => { Destroy (); a (); });
            }, (b) => {

                b += "Pushed";
            }, (b) => {

                b -= "Pushed";
            }
        );
        new GUIButton ("Textures/SelectLevel/Interface/Inventory", 44 + 112 + 180 / 4f, null, null, 28 + 315 / 4f, 180 / 2f, 180 / 2f, -2.5f, true
            , (b) => {
                
                ScenePassageController.instance.LoadScene <InventoryController> ((a) => { Destroy (); a (); });
            }, (b) => {

                b += "Pushed";
            }, (b) => {

                b -= "Pushed";
            }
        );

        new GUIImage ("Textures/SelectLevel/Interface/Stars", 10 + 322 / 4f, 10 + 137 / 4f, null, null, 322 / 2f, 137 / 2f, -3f, true);
        new GUIImage ("Textures/SelectLevel/Interface/StarsBar", 10 + 18 + 322 / 4f, 8 + 137 / 4f, null, null, 135 / 2f, 76 / 2f, -2.5f, true);
        new GUIImage ("Textures/SelectLevel/Interface/" + (Settings.stars / 100) % 10, 10 + 18 - 37 / 2f + 322 / 4f, 8 + 137 / 4f, null, null
            , 34 / 2f, 60 / 2f, -2.5f, true);
        new GUIImage ("Textures/SelectLevel/Interface/" + (Settings.stars / 10) % 10, 10 + 18 + 322 / 4f, 8 + 137 / 4f, null, null
            , 34 / 2f, 60 / 2f, -2.5f, true);
        new GUIImage ("Textures/SelectLevel/Interface/" + (Settings.stars / 1) % 10, 10 + 18 + 37 / 2f + 322 / 4f, 8 + 137 / 4f, null, null
            , 34 / 2f, 60 / 2f, -2.5f, true);

        new GUIImage ("Textures/SelectLevel/Interface/Diamonds", 180 + 322 / 4f, 10 + 137 / 4f, null, null, 416 / 2f, 136 / 2f, -3f, true);
        new GUIImage ("Textures/SelectLevel/Interface/DiamondsBar", 180 + 5 + 322 / 4f, 8 + 137 / 4f, null, null, 173 / 2f, 76 / 2f, -2.5f, true);
        new GUIImage ("Textures/SelectLevel/Interface/" + (Settings.diamonds / 1000) % 10, 180 + 5 - 1.5f * 37 / 2f + 322 / 4f, 8 + 137 / 4f, null, null
            , 34 / 2f, 60 / 2f, -2.5f, true);
        new GUIImage ("Textures/SelectLevel/Interface/" + (Settings.diamonds / 100) % 10, 180 + 5 - 0.5f * 37 / 2f + 322 / 4f, 8 + 137 / 4f, null, null
            , 34 / 2f, 60 / 2f, -2.5f, true);
        new GUIImage ("Textures/SelectLevel/Interface/" + (Settings.diamonds / 10) % 10, 180 + 5 + 0.5f * 37 / 2f + 322 / 4f, 8 + 137 / 4f, null, null
            , 34 / 2f, 60 / 2f, -2.5f, true);
        new GUIImage ("Textures/SelectLevel/Interface/" + (Settings.diamonds / 1) % 10, 180 + 5 + 1.5f * 37 / 2f + 322 / 4f, 8 + 137 / 4f, null, null
            , 34 / 2f, 60 / 2f, -2.5f, true);
        
        new GUIButton ("Textures/SelectLevel/Interface/Plus", 180 + 63 + 322 / 4f, 8 + 137 / 4f, null, null, 65 / 2f, 66 / 2f, -2.5f, true
            , (b) => {

                new BankMenu (() => {

                    }, () => {
                    
                        new SlideController (1920 / 100f * Settings.FhdToHD, 1080 / 100f * Settings.FhdToHD, SlideController.Mode.SlideAndZoom, 3);
                    }
                );
            }, (b) => {

                b += "Pushed";
            }, (b) => {

                b -= "Pushed";
            }
        );
    }

    public SelectLevelController () {
        
        instance = this;

        UpdateController.toUpdate = Update;
        UpdateController.toFixedUpdate = FixedUpdate;

        new SlideController (1920 / 100f * Settings.FhdToHD, 1080 / 100f * Settings.FhdToHD, SlideController.Mode.Slide, 3);
        new GUIController ();

        CameraController.ResizeCamera ( Mathf.Min (CameraController.GetWidthInMeters (1080 / 50f * Settings.FhdToHD), 1920 / 50f * Settings.FhdToHD) );

        Create ();
    }

	private void FixedUpdate (float deltaTime) {

        selected.color = new Color (1, 1, 1, 0.5f - 0.5f * Mathf.Cos (UpdateController.timeSinceLevelLoad * 3f));
    }

	private void Update (float deltaTime) {
		
		if (Input.GetKeyDown(KeyCode.Escape)) {

            if (!MenusController.RemoveMenus ()) {

			    ScenePassageController.instance.LoadScene <MainMenuController> ((a) => {Destroy (); a ();});
            }
        }

        OptionsController.instance.Update (deltaTime);

		AnimationController.Update(deltaTime);
		
	    SlideController.frictionDelta = CameraController.widthInMeters/Screen.width;
		SlideController.instance.Update (deltaTime);
		
	}

    public void Destroy () {

        OptionsController.instance.Destroy ();
        UpdateController.StopAllTimers ();
        UpdateController.toFixedUpdate = null;
        UpdateController.toUpdate = null;
        GamePullController.Destroy ();
    }
}
