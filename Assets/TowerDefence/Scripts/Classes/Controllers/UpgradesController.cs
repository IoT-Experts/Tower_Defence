using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class UpgradeController {

    public static UpgradeController instance;
    
    private List <List <string>> skills;

    private GUIImage select;

    private List <GUIObject> cameraObjects;
    private Dictionary <GUIObject, Vector2> cameraObjectPositions;

    private List <GUIObject> skillsObjects;

    private Actions.VoidGUIButton onResetClick;

    private string GetSkillIcon (string skill, bool isLearned) {
        
        if (skill.Contains ("Critical")) {
            
            return "Textures/Upgrades/" + (isLearned ? "" : "Not") + "Learned/Critical";
        }

        return "Textures/Upgrades/" + (isLearned ? "" : "Not") + "Learned/" + skill;
    }
    
    private string GetSkillName (string skill) {

        if (skill.Contains ("Critical")) {
            
            return Settings.GetText ("Critical" + "SkillName");
        }

        return Settings.GetText (skill + "SkillName");
    }
    private string GetSkillBottomName (string skill) {

        if (skill.Contains ("Critical")) {
            
            return Settings.GetText ("Critical" + "SkillBottomName");
        }

        return Settings.GetText (skill + "SkillBottomName");
    }
    
    private string GetSkillDescription (string skill) {

        if (skill.Contains ("Critical")) {
            
            return  Settings.GetText ("Critical" + "SkillDescription");
        }

        return Settings.GetText (skill + "SkillDescription");
    }

    private int GetSkillPrice (string skill) {

        return 4;
    }
    
    private void CreateItemSelect (Vector2 position, float deltaLayer) {

        if (select != null) {

            select.Destroy ();

        }

        select = new GUIImage ("Textures/Upgrades/Select", -4.8f - deltaLayer, position, new Vector2 (-1, -1) * Settings.FhdToHD, false);
    }

    private void CreateSkill (string name, bool isLearned, bool isForbiddenToLearn, Vector2 position, bool isUpgradable, float deltaLayer) {


        var skillButton = new GUIButton (GetSkillIcon (name, isLearned), -4.9f - deltaLayer, position
            , new Vector2 (-1, -1) * Settings.FhdToHD, false
            , (b) => {

                CreateItemSelect (position, deltaLayer);
                SetSkillInterface (Settings.GetTotalStars (), GetSkillPrice (name), name, isLearned, isForbiddenToLearn, deltaLayer);
            });


        skillsObjects.Add (skillButton);
        
        skillsObjects.Add (new GUIImage ("Textures/Upgrades/Hotspot", -4.8f - deltaLayer, position
            , new Vector2 (-1, -1) * Settings.FhdToHD, false));

        if (isForbiddenToLearn) {

            skillsObjects.Add (new GUIImage ("Textures/Upgrades/BlackCircle", -4.7f - deltaLayer, position
                , new Vector2 (-1, -1) * Settings.FhdToHD, false));
        }

        if (isUpgradable) {

            skillsObjects.Add (new GUIImage ("Textures/Upgrades/Arrow", -4.9f - deltaLayer, position + new Vector2 (0, 2f)
                , new Vector2 (-1, -1) * Settings.FhdToHD, false));
        }
    }

    private void CreateSkills (float deltaLayer) {

        foreach (var s in skillsObjects) {

            s.Destroy ();
        }

        skillsObjects = new List<GUIObject> ();

        for (int i = 0; i < 3; i++) {

            for (int q = 0; q < skills [i].Count; q++) {

                CreateSkill (skills [i][q],Settings.IsLearned (skills [i] [q])
                    , q != 0 && !Settings.IsLearned (skills [i][q - 1]), new Vector2 (-10.24f + i * 4.07f, -3.19f +  q * 4.2f), q != skills [i].Count - 1, deltaLayer);
            }
        }


    }
    
    private List <GUIImage> starsCount;
    private List <GUIImage> starsPrice;
    private GUIText skillName;
    private GUIText skillDescription;
    private GUIText notEnoughStars;
    private GUIButton improve;
    private GUIButton reset;      

    private void SetSkillInterface (int starsValue, int starPrice, string skill, bool isLearned, bool isForbiddenToLearn, float deltaLayer) {
        
        Debug.Log (starsValue);

        starsCount [0].textureName = "Textures/SelectLevel/Interface/" + (starsValue / 100) % 10;
        starsCount [1].textureName = "Textures/SelectLevel/Interface/" + (starsValue / 10) % 10;
        starsCount [2].textureName = "Textures/SelectLevel/Interface/" + (starsValue / 1) % 10;

        foreach (var s in starsPrice) {

            cameraObjects.Remove (s);
            s.Destroy ();
        }

        starsPrice = new List<GUIImage> ();
        
        for (int i = 0; i < starPrice; i++) {

            starsPrice.Add (new GUIImage ("Textures/Upgrades/Star", -2.6f - deltaLayer, new Vector2 (6.1f + 1.3f * i, -3.08f)
                , new Vector2 (-1, -1) * Settings.FhdToHD, false));
        }

        foreach (var s in starsPrice) {

            AddCameraObject (s);
        }

        skillName.text = GetSkillName (skill);
        skillDescription.text = GetSkillDescription (skill);
        notEnoughStars.text = (starsValue >= starPrice ? "" : "NOT ENOUGH STARS");

        if (isLearned) {

            //reset -= "Unavailable";

            improve -= "Pressed";
            improve -= "Unavailable";
            improve += "Unavailable";
            
            improve.OnClick = (b) => {

            };

            /*reset.OnClick = (b) => {

                Settings.SellSkill (skill);
                Settings.AddTotalStars (starPrice);
                SetSkillInterface (Settings.stars, starPrice, skill, false, isForbiddenToLearn, deltaLayer);
                CreateSkills (deltaLayer);
            };
            */
        } else {
            
            improve -= "Unavailable";
            
            

            improve.OnClick = (b) => {

                if (Settings.AddTotalStars (-starPrice)) {
                    
                    Settings.BuySkill (skill);

                    reset -= "Unavailable";

                    reset.OnClick = (b1) => {
            
                        onResetClick (b1);
                    };
                
                    SetSkillInterface (Settings.stars, starPrice, skill, true, isForbiddenToLearn, deltaLayer);
                    CreateSkills (deltaLayer);
                }
            };

            
        }

        if (isForbiddenToLearn) {
            
            improve -= "Pressed";
            improve -= "Unavailable";
            improve += "Unavailable";

            improve.OnClick = (b) => {

            };
        }

    }
    
    private void SetStartSkillInterface (float deltaLayer) {

        CreateItemSelect (new Vector2 (-10.24f + 0 * 4.07f, -3.19f +  0 * 4.2f), deltaLayer);
        SetSkillInterface (Settings.GetTotalStars (), GetSkillPrice (skills [0] [0]), skills [0] [0], Settings.IsLearned (skills [0] [0])
            , false, deltaLayer);
    }

    private void CreateSkillInterface (float deltaLayer) {

        starsCount = new List<GUIImage> ();
        starsPrice = new List<GUIImage> ();

        int starsValue = Settings.GetTotalStars ();
        
        AddCameraObject (new GUIImage ("Textures/Inventory/CountBar", -2.5f - deltaLayer, new Vector2 (5.17f, 5.45f), new Vector2 (-1, -1), false));

        starsCount.Add (new GUIImage ("Textures/SelectLevel/Interface/" + (starsValue / 100) % 10, -2.6f - deltaLayer
            , new Vector2 (5.17f, 5.45f) + new Vector2 (-0.5f, 0), new Vector2 (-1, -1), false));
        starsCount.Add (new GUIImage ("Textures/SelectLevel/Interface/" + (starsValue / 10) % 10, -2.6f - deltaLayer
            , new Vector2 (5.17f, 5.45f) + new Vector2 (0f, 0), new Vector2 (-1, -1), false));
        starsCount.Add (new GUIImage ("Textures/SelectLevel/Interface/" + (starsValue / 1) % 10, -2.6f - deltaLayer
            , new Vector2 (5.17f, 5.45f) + new Vector2 (0.5f, 0), new Vector2 (-1, -1), false));

        skillName = new GUIText ("ACID FOG", -2 - deltaLayer, new Vector2 (7.35f, 3.52f), new Vector2 (0.15f, 0.15f), GUIText.FontName.Font5);
        skillDescription = new GUIText ("asdasd\nasdasd", -2 - deltaLayer, new Vector2 (7.35f, 2.54f), new Vector2 (0.08f, 0.08f)
            , GUIText.FontName.Font5, false, TextAnchor.UpperCenter);
        
        notEnoughStars = new GUIText ("NOT ENOUGH STARS", -2 - deltaLayer, new Vector2 (7.35f, -1.66f), new Vector2 (0.09f, 0.09f)
            , GUIText.FontName.Font5);
        
        AddCameraObject (new GUIImage ("Textures/Upgrades/PricePanel", -2.6f - deltaLayer, new Vector2 (3.9f, -3.08f)
            , new Vector2 (-1, -1) * Settings.FhdToHD, false));
        AddCameraObject (new GUIText ("PRICE", -2 - deltaLayer, new Vector2 (3.97f, -3.12f), new Vector2 (0.09f, 0.09f)
            , GUIText.FontName.Font5));

        starsPrice.Add (new GUIImage ("Textures/Upgrades/Star", -2.6f - deltaLayer, new Vector2 (6.1f, -3.08f)
            , new Vector2 (-1, -1) * Settings.FhdToHD, false));

        improve = new GUIButton ("Textures/Upgrades/ButtonUnavailable", -2.6f - deltaLayer, new Vector2 (4.51f, -5.08f)
            , new Vector2 (-1, -1) * Settings.FhdToHD, false
            , (b) => {

            }, (b) => {

                if (!b.textureName.Contains ("Unavailable")) {

                    b += "Pressed";
                }
            }, (b) => {

                b -= "Pressed";
            }
        );
        
        onResetClick = (b) => {

            foreach (var s1 in skills) {

                foreach (var s in s1) {

                    if (Settings.IsLearned (s)) {

                        Settings.SellSkill (s);
                        Settings.AddTotalStars (GetSkillPrice (s));
                    }
                }

                    
                reset -= "Pressed";
                reset -= "Unavailable";
                reset += "Unavailable";

                onResetClick = (b1) => {

                };

                CreateSkills (deltaLayer);
                SetStartSkillInterface (deltaLayer);
            }

        };

        reset = new GUIButton ("Textures/Upgrades/Button", -2.6f - deltaLayer, new Vector2 (9.52f, -5.08f)
            , new Vector2 (-1, -1) * Settings.FhdToHD, false
            , onResetClick, (b) => {

                if (!b.textureName.Contains ("Unavailable")) {

                    b += "Pressed";
                }
            }, (b) => {

                b -= "Pressed";
            }
        );
        
        AddCameraObject (new GUIText ("IMPROVE", -2 - deltaLayer, new Vector2 (4.51f, -5.08f), new Vector2 (0.09f, 0.09f)
            , GUIText.FontName.Font5));
        AddCameraObject (new GUIText ("RESET", -2 - deltaLayer, new Vector2 (9.52f, -5.08f), new Vector2 (0.09f, 0.09f)
            , GUIText.FontName.Font5));

        AddCameraObject (skillName);
        AddCameraObject (skillDescription);
        AddCameraObject (notEnoughStars);
        AddCameraObject (improve);
        AddCameraObject (reset);
        
        foreach (var s in starsPrice) {

            AddCameraObject (s);
        }
        
        foreach (var s in starsCount) {

            AddCameraObject (s);
        }
    }

    private void AddCameraObject (GUIObject obj) {

        cameraObjects.Add (obj);
        cameraObjectPositions.Add (obj, obj.positionInMeters);
    }

    private void Create () {

        float deltaLayer = 0f;

        AddCameraObject (new GUIImage ("Textures/Upgrades/Background", -3 - deltaLayer, new Vector2 (0, 0)
            , new Vector2 (-1, -1) * Settings.FhdToHD, false));
        AddCameraObject (new GUIImage ("Textures/Upgrades/SkillsBackground", -5 - deltaLayer, new Vector2 (-6.24f, 0)
            , new Vector2 (-1, -1) * Settings.FhdToHD, false));

        
        AddCameraObject (new GUIImage ("Textures/Upgrades/NamePanel", -2.9f - deltaLayer, new Vector2 (-10.18f, -5.32f)
            , new Vector2 (-1, -1) * Settings.FhdToHD, false));
        AddCameraObject (new GUIText ("LIGHTNING", -2.8f - deltaLayer, new Vector2 (-10.18f, -5.32f), new Vector2 (0.09f, 0.09f), GUIText.FontName.Font5));
        
        AddCameraObject (new GUIImage ("Textures/Upgrades/NamePanel", -2.9f - deltaLayer, new Vector2 (-6.1f, -5.32f)
            , new Vector2 (-1, -1) * Settings.FhdToHD, false));
        AddCameraObject (new GUIText ("MAGIC", -2.8f - deltaLayer, new Vector2 (-6.1f, -5.32f), new Vector2 (0.09f, 0.09f), GUIText.FontName.Font5));
        
        AddCameraObject (new GUIImage ("Textures/Upgrades/NamePanel", -2.9f - deltaLayer, new Vector2 (-2.07f, -5.32f)
            , new Vector2 (-1, -1) * Settings.FhdToHD, false));
        AddCameraObject (new GUIText ("LAZER", -2.8f - deltaLayer, new Vector2 (-2.07f, -5.32f), new Vector2 (0.09f, 0.09f), GUIText.FontName.Font5));
        
        var back = new GUIButton ("Textures/Back", -2 - deltaLayer, new Vector2 (11f, 5.5f), new Vector2 (-1, -1));
        
        AddCameraObject (back);

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

        CreateSkills (deltaLayer);
        CreateSkillInterface (deltaLayer);
        SetStartSkillInterface (deltaLayer);

    }

	public UpgradeController () {

        instance = this;

        skills = Settings.GetSkills ();

        cameraObjects = new List<GUIObject> ();
        cameraObjectPositions = new Dictionary<GUIObject, Vector2> ();
        skillsObjects = new List<GUIObject> ();

        new ForbidController ();

        UpdateController.toUpdate = Update;
        UpdateController.toFixedUpdate = FixedUpdate;

        new GUIController ();

        new SlideController (new Vector2 (0, 0), new Vector2 (0 + CameraController.heightInMeters / 2f, 9 + CameraController.heightInMeters / 2f)
            , SlideController.Mode.Slide, 3);

        CameraController.ResizeCamera ( Mathf.Min (CameraController.GetWidthInMeters (1080 / 50f * Settings.FhdToHD), 1920 / 50f * Settings.FhdToHD) );
        CameraController.cameraPosition = new Vector2 (0, 0);

        UpdateController.Timer (0.1f, () => {
            
            Create ();
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


        if (!ForbidController.IsForbidden ()) {

            SlideController.frictionDelta = CameraController.widthInMeters/Screen.width;
		    SlideController.instance.Update (deltaTime);
        }

        foreach (var c in cameraObjects) {

            c.positionInMeters = CameraController.cameraPosition + cameraObjectPositions [c];
        }
	}

    public void Destroy () {

        UpdateController.StopAllTimers ();
        UpdateController.toFixedUpdate = null;
        UpdateController.toUpdate = null;
        GamePullController.Destroy ();
    }

}
