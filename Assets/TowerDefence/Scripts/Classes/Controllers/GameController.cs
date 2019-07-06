using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class GameController {

    public static GameController instance;

    private static int id = 0;
	
	public static float mapWidth = 38.4f;
	public static float mapHeight = 31.6f;
    
    public const int unitsCountToHeal = 3;
    private static int _killedUnitsToHeal;
    public static int killedUnitsToHeal {

        get { return _killedUnitsToHeal; }
        set {

            _killedUnitsToHeal = value;

            if (killedUnitsToHeal >= unitsCountToHeal) {

                killedUnitsToHeal = 0;
                PlayerController.health ++;
            }
        }
    }
	
	public static List <Unit> units = new List<Unit>();
	public static Dictionary <GameObject, Unit> unitsDictionary = new Dictionary <GameObject, Unit>();
	
	public static List <Unit> unitsToDestroy = new List<Unit>();
	
	public static List <Tower> towers = new List<Tower>();
	public static Dictionary <GameObject, Tower> towersDictionary = new Dictionary <GameObject, Tower>();

	public static Dictionary <Colorable.TeamColor, Dictionary <Tower.Buff, float>> buffs;

	public static List <IUpdatable> missiles = new List<IUpdatable>();
	public static List <IUpdatable> missilesToDestroy = new List<IUpdatable>();

	public static List < List <Vector3> > RoadRed;
	public static List < List <Vector3> > RoadBlue;
	
	public static List <GameObject> towerPlacesRed;
	public static List <GameObject> towerPlacesBlue;

	public static string input;
	public static bool isLoaded = false;

	public static float bombDamage = 100;

	public static float timeSkillFreezing = 0;

	public static bool isPaused;
	public static bool isStarted;

    private GUIText playerLifeText;
	private GUIText playerMoneyText;

    private int startLife;

    private bool isEnded = false;

    private static List <string> inventoryItems;
    private static List <GUIImage> inventoryButtons;

    private static float timeDoubleDamage = 0f;

    public static bool isDoubleDamage {

        get {

            return timeDoubleDamage > 0;
        }
    }

    public string playerLife {

		set {

            playerLifeText.text = value;
        }
	}

	public string playerMoney {

		set {

            playerMoneyText.text = value;
        }
	}

	public string botMoney {

		set {

            //botMoneyText.text = value;
        }
	}

    private static float underPowerUpsPosition {
		
		get { return 1 - underPowerUps.gameObject.GetComponent<Renderer> ().material.mainTextureOffset.x / (-0.87f); }
		
		set {
			underPowerUps.gameObject.GetComponent<Renderer> ().material.mainTextureOffset = new Vector2 (-0.87f*(1-value), 0);
			powerUps.gameObject.transform.rotation = Quaternion.Euler (new Vector3 (90, 90*value, 0));
			
			underPowerUpsLeft.right =(10+214/4f + 785/4f + 322 / 2f) * value + (10+214/4f + 30 / 2f)*(1-value);
            
			if (value >= 0.8f)
				underPowerUpsRight.right = (10+214/4f + 785/4f + 322 / 2f) * value + (10+214/4f + 30)*(1-value)  - 274 / 2f - 322 / 2f;

            if (inventoryButtons == null || inventoryButtons.Count <= 0) {

                return;
            }
            
            foreach (var inv in inventoryButtons) {

                inv.right = powerUps.right;
            }

			if (value >= 0.53f)
				inventoryButtons [0].right = (10+214/4f + 785/4f + 322 / 2f) * value + (10+214/4f + 30)*(1-value)  - 274 / 2f - 210 / 2f + 94 * 0 / 2f;
            
			if (value >= 0.43f)
				inventoryButtons [1].right = (10+214/4f + 785/4f + 322 / 2f) * value + (10+214/4f + 30)*(1-value)  - 274 / 2f - 210 / 2f + 94 * 1 / 2f;
            
			if (value >= 0.33f)
				inventoryButtons [2].right = (10+214/4f + 785/4f + 322 / 2f) * value + (10+214/4f + 30)*(1-value)  - 274 / 2f - 210 / 2f + 94 * 2 / 2f;
            
			if (value >= 0.23f)
				inventoryButtons [3].right = (10+214/4f + 785/4f + 322 / 2f) * value + (10+214/4f + 30)*(1-value)  - 274 / 2f - 210 / 2f + 94 * 3 / 2f;
            
			if (value >= 0.13f)
				inventoryButtons [4].right = (10+214/4f + 785/4f + 322 / 2f) * value + (10+214/4f + 30)*(1-value)  - 274 / 2f - 210 / 2f + 94 * 4 / 2f;
		}
		
	}

    
	public static GUIImage underPowerUps;
	private static GUIButton underPowerUpsLeft;
	private static GUIButton underPowerUpsRight;
	public static bool isUnderPowerUpsOpening = false;
	private static GUIButton powerUps;

	private static float skill1MaxCooldown = 45f;
	private static float _skill1Cooldown;
	public static float skill1Cooldown {
		get { return _skill1Cooldown; }
		set {
			_skill1Cooldown = value;

			if (_skill1Cooldown < 0)
				_skill1Cooldown = 0;

			//GUIController.skill1Cooldown = value/skill1MaxCooldown;
		}
	}
	
	private static float skill2MaxCooldown = 45f;
	private static float _skill2Cooldown;
	public static float skill2Cooldown {
		get { return _skill2Cooldown; }
		set {
			_skill2Cooldown = value;
			
			if (_skill2Cooldown < 0)
				_skill2Cooldown = 0;
			
			//GUIController.skill2Cooldown = value/skill2MaxCooldown;
		}
	}
	
	private static float skill3MaxCooldown = 45f;
	private static float _skill3Cooldown;
	public static float skill3Cooldown {
		get { return _skill3Cooldown; }
		set {
			_skill3Cooldown = value;
			
			if (_skill3Cooldown < 0)
				_skill3Cooldown = 0;
			
			//GUIController.skill3Cooldown = value/skill3MaxCooldown;
		}
	}
	private static bool isUsingSkill;
	private static int skillUsed;

    private static GUIImageAlpha damageImage;

    public static void DamageAnimation () {

        if (damageImage != null) {

            return;
        }

        damageImage = new GUIImageAlpha ("Textures/Gameplay/Damage", -1f, new Vector2 (0, 0), CameraController.sizeInMeters);

        int n = 10;

        UpdateController.LaunchIt (n, 0.5f / n, (t) => {

            damageImage.color = new Color (1, 1, 1, 1 - t * 1f / n);
        }, () => {

            damageImage.Destroy ();
            damageImage = null;
        });
    }

    private static void UseInventoryItem (string name) {

        switch (name) {
            
            case "Bomb":
                SkillBomb ();
                return;
                
            case "Heal":
                SkillHeal ();
                return;
                
            case "Gold":
                SkillGold ();
                return;
                
            case "Power":
                SkillDoubleDamage ();
                return;

            case "Freezing":
                SkillFreezing ();
                return;

        }
    }

	public static void SkillFreezing () {
		
        if (!Settings.AddItemCount ("Freezing", -1)) {

            return;
        }

        Debug.Log ("Freezing");

        ActivateSkillFreezing ();
	}

	public static void Skill2 () {
		if (skill2Cooldown > 0)
			return;
		
		isUsingSkill = true;
		skillUsed = 2;
	}

    public static void SkillHeal () {
		
        if (!Settings.AddItemCount ("Heal", -1)) {

            return;
        }

        Debug.Log ("Heal");
        
        PlayerController.health += 5;
	}

    public static void SkillDoubleDamage () {
		
        if (!Settings.AddItemCount ("Power", -1)) {

            return;
        }
        
        Debug.Log ("DoubleDamage");

        timeDoubleDamage = 10f;
	}

    public static void SkillGold () {
		
        if (!Settings.AddItemCount ("Gold", -1)) {

            return;
        }
        
        Debug.Log ("Gold");

        PlayerController.money += 200;
	}


	public static void SkillBomb () {

        if (!Settings.AddItemCount ("Bomb", -1)) {

            return;
        }
		
        Debug.Log ("Bomb");

		isUsingSkill = true;
		skillUsed = 3;
	}
	
	public static void ActivateSkillFreezing () {
		
		foreach (Killable unit in units) {
			if (!unit.isKilling && unit.color != PlayerController.color) {
				
				unit.AddEffect (Killable.Effect.Root, 5f);
			}
			
		}
		
		isUsingSkill = false;
		skill1Cooldown = skill1MaxCooldown;
	}
	
	public static void ActivateSkill2 (Vector2 position) {
		
		foreach (Killable unit in units) {
			if (!unit.isKilling && unit.color == PlayerController.color)
				if ((Mathf.Pow (position.x - unit.mapPosition.x, 2) + Mathf.Pow (position.y - unit.mapPosition.y, 2) 
				    <= Mathf.Pow (5 + 0.1f, 2))) {
				
				unit.AddEffect (Killable.Effect.Heal, 10f);
			}
			
		}
		
		isUsingSkill = false;
		skill2Cooldown = skill2MaxCooldown;
	}
	
	
	public static void ActivateSkillBomb (Vector2 position) {

		float minDelta = 1000;
		Vector3 minRoad = new Vector3 ();

		foreach (var road in (PlayerController.color != Colorable.TeamColor.Red?RoadRed:RoadBlue) ) {

			foreach (var roadPosition in road) {
				
				if ((Mathf.Pow (position.x - roadPosition.x, 2) + Mathf.Pow (position.y - roadPosition.y, 2) 
				     <= Mathf.Pow (0.5f + 0.1f, 2))) {

					if (minDelta > Mathf.Pow (position.x - roadPosition.x, 2) + Mathf.Pow (position.y - roadPosition.y, 2)) {

						minDelta = Mathf.Pow (position.x - roadPosition.x, 2) + Mathf.Pow (position.y - roadPosition.y, 2);
						minRoad = roadPosition;
					}
				}
			}

		}

		if (minDelta != 1000) {

			Bomb bomb = new Bomb (new Vector2 (minRoad.x, minRoad.y));
			isUsingSkill = false;
			skill3Cooldown = skill3MaxCooldown;
		}
	}

	public static void ReduceHealth (Colorable.TeamColor color, int damage) {

		if (color == PlayerController.color)
			PlayerController.health -= damage;
		else
			BotController.health -= damage;

	}
	public static void IncreaseGold (Colorable.TeamColor color, int gold) {
		
		if (color == PlayerController.color)
			PlayerController.money += gold;
		else
			BotController.money += gold;
		
	}

	public static bool CreateTower (string type, Colorable.TeamColor color, GameObject towerPlace) {

        int cost = TowersSettings.GetPrice (type);
         
		if (color == PlayerController.color) {
			if (PlayerController.money < cost)
				return false;
			PlayerController.money -= cost;

		} else {
			
			if (BotController.money < cost)
				return false;
			BotController.money -= cost;
		}

		var towerPlacePosition = towerPlace.transform.position;

		if (color == Colorable.TeamColor.Red)
			towerPlacesRed.Remove (towerPlace);
		else
			towerPlacesBlue.Remove (towerPlace);
		
		GamePullController.DestroyImage (towerPlace);

		Unit builldAnimation = new Unit ();
		
        var buldAnimationGUIObject = new GUIImage ();
		builldAnimation.gameObject = buldAnimationGUIObject.gameObject; //GamePullController.CreateImage ();
		builldAnimation.objectAnimation = new ObjectAnimation (builldAnimation.gameObject, true);
		builldAnimation.mapPosition = new Vector2 (towerPlacePosition.x
		                                , towerPlacePosition.z);
		builldAnimation.size = new Vector2 (4, 4) * Settings.FhdToHD;
		builldAnimation.layer = towerPlacePosition.y + 0.1f;
		builldAnimation.objectAnimation.Load ("TowerBuildAnimation", -1.0f);
		builldAnimation.objectAnimation.Play (-1, () => {
            
			buldAnimationGUIObject.Destroy ();//GamePullController.DestroyImage (builldAnimation.gameObject);
			builldAnimation.objectAnimation.Destroy ();
        });

        UpdateController.Timer (0.9f, () => {

			Tower tower = new Tower (type, color);

			tower.positionByTowerPlace = new Vector2 (towerPlacePosition.x, towerPlacePosition.z);
			tower.layer = towerPlacePosition.y;
			tower.towerPlacePosition = towerPlacePosition;
		});

			

		if (color == PlayerController.color)
			RemoveTowerPlaceInterface ();

		return true;
	}


	public static void OnClick (Vector2 position) {
		
		Ray ray;
		RaycastHit hit;
        
        Debug.Log (position);
        ray = Camera.main.ScreenPointToRay (position);

        
        /*TODO
        if (Settings.scene == Settings.Scene.Menu) {

            MainMenuController.instance.OnClick ();
        }

        
		
        var hits = Physics.RaycastAll (ray);

        foreach (var h in hits) {
            Debug.Log (h.transform.gameObject);
            if (h.transform.gameObject.name.Contains ("Through")) {

                
			    if (h.transform.gameObject.name.Contains ("GUIButton"))
				    GUIController.OnClick (h.transform.gameObject);
            }
        }
        */

		if (Physics.Raycast (ray, out hit, 100)) {
            
        Debug.Log (hit.transform.gameObject.name);
			if (isUsingSkill) {

				switch (skillUsed) {
				case 1:
					break;
				case 2:
					ActivateSkill2 (new Vector2 (hit.point.x, hit.point.z));
					break;
				case 3:
					ActivateSkillBomb (new Vector2 (hit.point.x, hit.point.z));
					break;
				}
				return;
			}
            ///*TODO
			if (!(hit.transform.gameObject.name.Contains ("TowerPlaceInterface")))
				RemoveTowerPlaceInterface ();
            //*/
			
			if (!(hit.transform.gameObject.name.Contains ("UpgradeInterface")))
				instance.RemoveUpgradeInterface ();
            //*/
			if (!(hit.transform.gameObject.name.Contains ("TowerPlaceInterface")) && !(hit.transform.gameObject.name.Contains ("UpgradeInterface")) 
			    	&& !(hit.transform.gameObject.name.Contains ("Info")))
				instance.RemoveInfo ();
            
			if (hit.transform.gameObject.name.Contains ("Tower") && !hit.transform.gameObject.name.Contains ("TowerPlace")) {
                
				instance.CreateInfo (towersDictionary[hit.transform.gameObject].type + "Tower", towersDictionary[hit.transform.gameObject], null);
				

				if (towersDictionary[hit.transform.gameObject].color != PlayerController.color)
					return;

				instance.CreateUpgradeInterface (TowersSettings.GetPrice (towersDictionary[hit.transform.gameObject].type)/2
                    , new Vector2 (hit.transform.gameObject.transform.position.x
				        , hit.transform.gameObject.transform.position.z)

				                                     , () => {

					PlayerController.money += (int) TowersSettings.GetPrice (towersDictionary[hit.transform.gameObject].type)/2;
					towersDictionary[hit.transform.gameObject].Destroy ();
					instance.RemoveUpgradeInterface ();
				} 
													 , (s) => {
                    
                    if (TowersSettings.GetPrice (s) <= PlayerController.money) {

                        Unit builldAnimation = new Unit ();
		
                        var buldAnimationGUIObject = new GUIImage ();
		                builldAnimation.gameObject = buldAnimationGUIObject.gameObject; //GamePullController.CreateImage ();
		                builldAnimation.objectAnimation = new ObjectAnimation (builldAnimation.gameObject, true);
		                builldAnimation.mapPosition = new Vector2 (hit.transform.gameObject.transform.position.x
				                    , hit.transform.gameObject.transform.position.z);
		                builldAnimation.size = new Vector2 (4, 4) * Settings.FhdToHD;
		                builldAnimation.layer = hit.transform.gameObject.transform.position.y + 0.1f;
		                builldAnimation.objectAnimation.Load ("TowerBuildAnimation", -1.0f);
		                builldAnimation.objectAnimation.Play (-1, () => {
            
			                buldAnimationGUIObject.Destroy ();//GamePullController.DestroyImage (builldAnimation.gameObject);
			                builldAnimation.objectAnimation.Destroy ();
                        });

                        UpdateController.Timer (0.9f, () => {
                            
                            new Tower (towersDictionary[hit.transform.gameObject], s);
		                });

                        PlayerController.money -= (int) TowersSettings.GetPrice (s);
                    } 
					instance.RemoveUpgradeInterface ();
                                                         
				}, TowersSettings.Upgrades (towersDictionary[hit.transform.gameObject].type)

				                                     );
            }
            
			if (hit.transform.gameObject.name.Contains ("Unit")) {

				instance.CreateInfo (unitsDictionary[hit.transform.gameObject].type + "Unit", null, unitsDictionary[hit.transform.gameObject]);
            }
            
			if (hit.transform.gameObject.name.Contains ("TowerPlaceRed")) {
                
				if (PlayerController.color == Colorable.TeamColor.Red) {
					
				    RemoveTowerPlaceInterface ();
				    instance.CreateTowerPlaceInterface (new List<string> {
                        
                        TowersSettings.startTowers [2], TowersSettings.startTowers [0], TowersSettings.startTowers [1]
                    }, new Vector2 (hit.transform.gameObject.transform.position.x
				                                                         , hit.transform.gameObject.transform.position.z)
				                                            , () => {
					    CreateTower (TowersSettings.startTowers [2], Colorable.TeamColor.Red, hit.transform.gameObject);
				    } 
														    , () => {
					    CreateTower (TowersSettings.startTowers [0], Colorable.TeamColor.Red, hit.transform.gameObject);
					
				    } 
														    , () => {
					    CreateTower (TowersSettings.startTowers [1], Colorable.TeamColor.Red, hit.transform.gameObject);
					
				    });
                }
            }

			if (hit.transform.gameObject.name.Contains ("TowerPlaceBlue")) {

                if (PlayerController.color == Colorable.TeamColor.Blue) {

				    RemoveTowerPlaceInterface ();
				    instance.CreateTowerPlaceInterface (new List<string> {
                        
                        TowersSettings.startTowers [2], TowersSettings.startTowers [0], TowersSettings.startTowers [1]
                    },new Vector2 (hit.transform.gameObject.transform.position.x
				                                                         , hit.transform.gameObject.transform.position.z)
				                                            , () => {
					    CreateTower (TowersSettings.startTowers [2], Colorable.TeamColor.Blue, hit.transform.gameObject);
					
				    } 
														    , () => {
					    CreateTower (TowersSettings.startTowers [0], Colorable.TeamColor.Blue, hit.transform.gameObject);
					
				    } 
														    , () => {
					    CreateTower (TowersSettings.startTowers [1], Colorable.TeamColor.Blue, hit.transform.gameObject);
					
				    });
                }
            }
            //
            
		} else 
			RemoveTowerPlaceInterface ();
			

		       
	
	}
	
	List < GameObject > temp1 = new List < GameObject > ();
	List < GameObject > temp2 = new List < GameObject > ();
	List < GameObject > temp3 = new List < GameObject > ();
	List < GameObject > temp4 = new List < GameObject > ();

	public void Create () {

        if (UpdateController.themeName != "GameplayTheme") {
            
            if (UpdateController.theme != null) {

                AudioController.instance.RemoveAudio (UpdateController.theme);
            }

            UpdateController.theme = AudioController.instance.CreateAudio ("GameplayTheme", true, true);
            UpdateController.themeName = "GameplayTheme";
        }

        inventoryButtons = new List<GUIImage> ();
        inventoryItems = Settings.GetItems ();

		units = new List<Unit>();
		towers = new List<Tower>();
		unitsDictionary = new Dictionary <GameObject, Unit>();
		towersDictionary = new Dictionary <GameObject, Tower>();

		buffs = new Dictionary<Colorable.TeamColor, Dictionary <Tower.Buff, float>> ();
		buffs [Colorable.TeamColor.Red] = new Dictionary<Tower.Buff, float> ();
		buffs [Colorable.TeamColor.Blue] = new Dictionary<Tower.Buff, float> ();

		buffs [Colorable.TeamColor.Red] [Tower.Buff.Damage] = 1;
		buffs [Colorable.TeamColor.Red] [Tower.Buff.AttackSpeed] = 1;
		buffs [Colorable.TeamColor.Red] [Tower.Buff.AttackRange] = 1;
		buffs [Colorable.TeamColor.Blue] [Tower.Buff.Damage] = 1;
		buffs [Colorable.TeamColor.Blue] [Tower.Buff.AttackSpeed] = 1;
		buffs [Colorable.TeamColor.Blue] [Tower.Buff.AttackRange] = 1;

		missiles = new List<IUpdatable> ();
		towerPlacesRed = new List<GameObject> ();
		towerPlacesBlue = new List<GameObject> ();

		missilesToDestroy = new List<IUpdatable>();
		unitsToDestroy = new List<Unit>();
		
		LevelController.LoadMap ();

        CameraController.ResizeCamera ( Mathf.Min (CameraController.GetWidthInMeters (GameController.mapHeight), mapWidth));
        new SlideController (mapWidth / 2f, mapHeight / 2f, SlideController.Mode.SlideAndZoom, 3);

		
        CreateGameInterface ();
		PlayerController.Create ();
		//BotController.Create ();
		BombsController.Create ();

		timeSkillFreezing = 0;
		skill1Cooldown = 0;
		skill2Cooldown = 0;
		skill3Cooldown = 0;
		isLoaded = true;
		isUsingSkill = false;
		skillUsed = -1;
		isPaused = false;
		isStarted = false;

	}

    public static void CreateGameInterface () {
		
		var avatarPlayer = new GUIImage ("Textures/UserInterface/Player", (10 + 313 * 0.5f), (10 + 113 * 0.5f), null, null, 626 * 0.5f, 226 * 0.5f, -1 - 1f - 10f, true);

		instance.playerLifeText = new GUIText  ("", (313 / 2f) / Settings.FhdToHD, (10 + 113 / 4f - 26 / 2f + 13) / Settings.FhdToHD, null, null, new Vector2 (2.3f, 2.3f) * 1.5f, GUIText.FontName.Font0, -1f - 10f);
		instance.playerMoneyText = new GUIText ("", (313 / 2f) / Settings.FhdToHD, (10 + 113 / 4f + 18 / 2f + 10) / Settings.FhdToHD, null, null, new Vector2 (2.3f, 2.3f) * 1.5f, GUIText.FontName.Font0, -1f - 10f);


		var startBattle = new GUIButton ("Textures/UserInterface/Start", (GUIController.GUIBackgroundWidth) / 2f, (10 + 192/4f)
            , null, null, 514 / 2f, 192 / 2f, 0 - 1f - 10f, true);
		startBattle.OnClick = (t) => {

			startBattle.Destroy ();
			GameController.isStarted = true;
		};
		
		
		new OptionsController (() => { instance.OnLost (); }, () => {

            ScenePassageController.instance.LoadScene <GameController> ((a) => {instance.Destroy (); a ();});
        });
        

		//var avatarBot = new GUIImage ("Textures/UserInterface/Enemy", null, (10 + 226/4f), (20 + 127 + 636/4f), null, 636 / 2f, 226 / 2f, 0 - 1f, true);
		
		//botLifeText = new GUIText ("", null, (10 + 113 - 26), (25 + 127 + 313), null, new Vector2 (2f, 2f), GUIText.FontName.Font0, -1f);

        /*
		var skills = new GUIImage ("Textures/UserInterface/Skills", (10 + 603/2f), null, null, (10 + 209/2f), 603, 209, -1, true);
		var skill1 = new GUIButton ("Textures/UserInterface/SkillFreezing", (10 + 123), null, null, (127), 128, 128, 0, true);
		skill1.OnClick = (t) => {

			GameController.SkillFreezing ();
		};
		var skill1Black = new GUIImage ("Textures/UserInterface/Black", (10 + 123), null, null, (127), 145, 145, -0.5f, true);
		skill1Green = new GUIImage ("Textures/UserInterface/GreenAlpha", (10 + 123), null, null, (127), 145, 145, -0.25f, true);
		//skill1Green.gameObject.GetComponent <Renderer> ().material.shader = Shader.Find ("Legacy Shaders/Transparent/Cutout/Diffuse");
		var skill2 = new GUIButton ("Textures/UserInterface/Skill2", (10 + 302), null, null, (127), 128, 128, 0, true);
		skill2.OnClick = (t) => {
			
			GameController.Skill2 ();
		};
		var skill2Black = new GUIImage ("Textures/UserInterface/Black", (10 + 302), null, null, (127), 145, 145, -0.5f, true);
		skill2Green = new GUIImage ("Textures/UserInterface/GreenAlpha", (10 + 302), null, null, (127), 145, 145, -0.25f, true);
		//skill2Green.gameObject.GetComponent <Renderer> ().material.shader = Shader.Find ("Legacy Shaders/Transparent/Cutout/Diffuse");
		var skill3 = new GUIButton ("Textures/UserInterface/SkillBomb", (10 + 481), null, null, (127), 128, 128, 0, true);
		skill3.OnClick = (t) => {
			
			GameController.SkillBomb ();
		};
		skill3Green = new GUIImage ("Textures/UserInterface/GreenAlpha", (10 + 481), null, null, (127), 145, 145, -0.25f, true);
		//skill3Green.gameObject.GetComponent <Renderer> ().material.shader = Shader.Find ("Legacy Shaders/Transparent/Cutout/Diffuse");
		var skill3Black = new GUIImage ("Textures/UserInterface/Black", (10 + 481), null, null, (127), 145, 145, -0.5f, true);
        */

		var powerUpBase = new GUIImage ("Textures/UserInterface/PowerUpBase", null, null, 10+214/4f, 10 + 212/4f, 214 / 2f, 212 / 2f, -1.1f - 10f, true);
		powerUps = new GUIButton ("Textures/UserInterface/PowerUps", null, null, 10+214/4f, 10 + 212/4f + 15 / 2f, 120/ 2f, 120/ 2f, -1f - 10f, true);
		powerUps.OnClick = (t) => {

			isUnderPowerUpsOpening = !isUnderPowerUpsOpening;
			
		};
		underPowerUps = new GUIImage ("Textures/UserInterface/UnderPowerUps", null, null, 10+214/4f + 785/4f, 10 + 212/4f, 785 / 2f, 195 / 2f, -2f - 10f, true);
		underPowerUpsLeft = new GUIButton ("Textures/UserInterface/Left", null, null, 10+214/4f + 10, 10 + 212/4f+10 / 2f, 54 / 2f, 89 / 2f, -1.5f - 10f, true);
		underPowerUpsRight = new GUIButton ("Textures/UserInterface/Right", null, null, 10+214/4f, 10 + 212/4f+8 / 2f, 54 / 2f, 89 / 2f, -1.5f - 10f, true);
        
		underPowerUpsPosition = 0;
        
        for (int i = 0; i < 5; i++) {

            var q = i;
            inventoryButtons.Add (new GUIButton (InventoryController.GetItemIconGameplay (inventoryItems [i]), null, null
                , 10+214/4f, 10 + 235/4f, 172 / 4f, 172 / 4f, -1.9f - 10f, true, (b) => {
                    
                    UseInventoryItem (inventoryItems [q]);
                }));

        }

	}

    public static void LoadBalance () {
        
		if (UpdateController.platform == UpdateController.Platform.Phone || true) {

			BalanceSettings.ParseBalance ((Resources.Load ("Balance")as TextAsset).text);

		} else {

			StreamReader file = new StreamReader (Settings.settingsPath + "/Balance.txt");
			BalanceSettings.ParseBalance (file.ReadToEnd ());
			file.Close ();
		}
		
	}
    
	private static GUIImageAlpha info;
	private static GUIText infoText1;
	private static GUIText infoText2;
	private static GUIText infoText3;
	private static Tower infoTower;
	private static Unit infoUnit;

    private void CreateInfo (string target, Tower tower, Unit unit) {

        RemoveInfo ();

		if (unit != null && unit.isKilling)
			return;

		infoTower = tower;
		infoUnit = unit;

		//TODO? isUnderPowerUpsOpening = false;
		if (target.Contains ("Tower")) {

			info = new GUIImageAlpha ("Textures/Info/Info"+target, null, null, 10+214/4f+100+869/4f, 10 + 212/4f, 869 / 2f, 147 / 2f, -3f, true);
			info.gameObject.name += "Info";

			infoText1 = new GUIText (( (int) (tower.damage - tower.damageDelta) ) + "-" + ( (int) (tower.damage + tower.damageDelta) )
                , null, null, 10+214/4f+869/4f - 165 / 2f + 30, 10 + 212/4f + 25 / 2f + 5, new Vector2 (3.5f, 3.5f), GUIText.FontName.Font0, -2
                , TextAnchor.MiddleCenter, true);
			
			infoText2 = new GUIText (( (int) (tower.attackSpeed*10) ).ToString (), null, null, 10+214/4f+869/4f - 165 / 2f + 30, 10 + 212/4f - 35 / 2f+ 3
                , new Vector2 (3.5f, 3.5f), GUIText.FontName.Font0, -2
                , TextAnchor.MiddleCenter, true);


		} else {

            info = new GUIImageAlpha ("Textures/Info/Info"+target, null, null, 10+214/4f+100+1089/4f, 10 + 212/4f, 1089 / 2f, 147 / 2f, -3f, true);
			info.gameObject.name += "Info";

            infoText1 = new GUIText (( (int) (unit.health) ).ToString (), null, null, 10+214/4f+869/4f + 80, 10 + 212/4f + 21, new Vector2 (3.5f, 3.5f)
                , GUIText.FontName.Font0, -2, TextAnchor.MiddleCenter, true);
			infoText2 = new GUIText (( (int) (unit.defence) ).ToString (), null, null, 10+214/4f+869/4f + 80, 10 + 212/4f - 60 / 2f + 17, new Vector2 (3.5f, 3.5f)
                , GUIText.FontName.Font0, -2, TextAnchor.MiddleCenter, true);
			infoText3 = new GUIText (( (int) (unit.speed*10) ).ToString (), null, null, 10+214/4f+869/4f - 210 / 2f + 30, 10 + 212/4f - 25/2f + 15, new Vector2 (3.5f, 3.5f)
                , GUIText.FontName.Font0, -2, TextAnchor.MiddleCenter, true);
		}

	}

    private void UpdateInfo () {



		if (infoTower != null) {
			
			infoText1.text = ((int)(infoTower.damage - infoTower.damageDelta)) + "-" 
				+ ((int)(infoTower.damage + infoTower.damageDelta));
			infoText2.text = ((int)(infoTower.attackSpeed * 10)).ToString ();
		} else if (infoUnit != null && !infoUnit.isKilling) {

			infoText1.text = ((int)(infoUnit.health)).ToString ();
			infoText2.text = ((int)(infoUnit.defence)).ToString ();
			infoText3.text = ((int)(infoUnit.speed * 10)).ToString ();
		} else 
			RemoveInfo ();

	}

	private void RemoveInfo () {
		
		if (info != null)
			info.Destroy ();

		if (infoText1 != null)
			infoText1.Destroy ();
		
		if (infoText2 != null)
			infoText2.Destroy ();
		
		if (infoText3 != null)
			infoText3.Destroy ();

		info = null;
		infoText1 = null;
		infoText2 = null;
		infoText3 = null;
		infoTower = null;
		infoUnit = null;

	}
    
	private static GUIImage upRound = null;
	private static GUIButton sell = null;
	private static GUIButton upgrade = null;
	private static GUIButton upConfirm = null;
	private static GUIButton upgrade1 = null;
	private static GUIText upgrade1Text = null;
	private static GUIText sellText = null;

    private static void SellClick (Vector2 position, Actions.VoidVoid sellAction) {
		
		
		if (upConfirm != null) {
			upConfirm.Destroy ();
			upConfirm = null;
		}
		
		//CreateCannoneerButton (position, sellAction, upgradeAction);
		//CreateSteamButton (position, sellAction, upgradeAction);
		sell.Destroy ();
		sell = null;
		
		upConfirm = new GUIButton ();
		upConfirm.texture = Resources.Load ("Textures/UserInterface/Accept") as Texture;
		upConfirm.positionInMeters = position - new Vector2 (0, 333/2f/50f) * Settings.FhdToHD;
		upConfirm.sizeInMeters = new Vector2 (-1, -1) * Settings.FhdToHD;
		upConfirm.gameObject.name +="UpgradeInterface";
		upConfirm.OnClick = (t) => { sellAction (); };
		
		
	}

    private void CreateSellButton (int sellPrice, Vector2 position, Actions.VoidVoid sellAction) {
		
		if (sell != null)
			return;
		
		sell = new GUIButton ();
		sell.layer = -3;
		sell.texture = Resources.Load ("Textures/UserInterface/Sell") as Texture;
		sell.OnClick = (t) => {

			SellClick (position, sellAction);
		};
		sell.gameObject.name += "UpgradeInterface";
		sell.positionInMeters = position - new Vector2 (0, 333/2f/50f) * Settings.FhdToHD;
		sell.sizeInMeters = new Vector2 (-1, -1) * Settings.FhdToHD;
        if (sellText == null) sellText = new GUIText (sellPrice + "", -2.5f, sell.positionInMeters + new Vector2 (0, -93/50f) * Settings.FhdToHD
            , new Vector2 (0.13f, 0.13f));
	}

    private void CreateUpgrade1Button (Vector2 position, Actions.VoidString upgrade1Action, string type) {
		
		if (upgrade1 != null)
			return;
		
		upgrade1 = new GUIButton ();
		upgrade1.layer = -3;
		upgrade1.texture = Resources.Load ("Textures/UserInterface/" + type) as Texture;
		upgrade1.OnClick = (t) => {

			Upgrade1Click (position, upgrade1Action, type);
		};
		upgrade1.gameObject.name += "TowerPlaceInterface";
		upgrade1.positionInMeters = position + new Vector2 (0, 354/2f/50f) * Settings.FhdToHD;
        if (upgrade1Text == null) upgrade1Text = new GUIText (TowersSettings.GetPrice (type) + "", -2.5f, upgrade1.positionInMeters + new Vector2 (0, -93/50f) * Settings.FhdToHD
            , new Vector2 (0.13f, 0.13f));
		upgrade1.sizeInMeters = new Vector2 (170/50f, 252/50f) * Settings.FhdToHD;
	}

    private static void Upgrade1Click (Vector2 position, Actions.VoidString upgrade1Action, string type) {
		
		
		if (upConfirm != null) {
			upConfirm.Destroy ();
			upConfirm = null;
		}
		
		//CreateCannoneerButton (position, sellAction, upgradeAction);
		//CreateSteamButton (position, sellAction, upgradeAction);
		upgrade1.Destroy ();
		upgrade1 = null;
		
		upConfirm = new GUIButton ();
		upConfirm.texture = Resources.Load ("Textures/UserInterface/Accept") as Texture;
		upConfirm.positionInMeters = position + new Vector2 (0, 333/2f/50f) * Settings.FhdToHD;
		upConfirm.sizeInMeters = new Vector2 (-1, -1) * Settings.FhdToHD;
		upConfirm.gameObject.name +="UpgradeInterface";
		upConfirm.OnClick = (t) => { upgrade1Action (type); };
		
		
	}

    private void CreateUpgradeInterface (int sellPrice, Vector2 position, Actions.VoidVoid sellAction, Actions.VoidString upgrade1Action, List <string> upgrades) {
		
		upRound = new GUIImage ();
		upRound.texture = Resources.Load ("Textures/UserInterface/UpgradeBase") as Texture;
		upRound.layer = -6;
		upRound.positionInMeters = position;
		upRound.sizeInMeters = new Vector2 (354/50f, 373/50f) * Settings.FhdToHD;
		upRound.gameObject.name += "UpgradeInterface";

		CreateSellButton (sellPrice, position, sellAction);

        if (upgrades.Count > 0) {

            CreateUpgrade1Button (position, upgrade1Action, upgrades [0]);
        }
	}

    private void RemoveUpgradeInterface () {
		
		if (upRound == null)
			return;
		
		upRound.Destroy ();
		upRound = null;

		if (upConfirm != null)
			upConfirm.Destroy ();

		if (sell != null) 
			sell.Destroy ();
        
		if (upgrade1 != null)
			upgrade1.Destroy ();
        
		if (upgrade1Text != null)
			upgrade1Text.Destroy ();

		if (sellText != null)
			sellText.Destroy ();

		upConfirm = null;
		sell = null;
        upgrade1Text = null;
        sellText = null;
        upgrade1 = null;
        
	}

    
	private static GUIImage round = null;
	private static GUIButton cannoneer = null;
	private static GUIButton tesla = null;
	private static GUIButton steam = null;
	private static GUIButton confirm = null;
	private static GUIText cannoneerText = null;
	private static GUIText teslaText = null;
	private static GUIText steamText = null;
	private static GUIText confirmText = null;

    private void CreateTower1Button (string name, Vector2 position, Actions.VoidVoid tower1Action, Actions.VoidVoid tower2Action, Actions.VoidVoid tower3Action) {
		
		if (cannoneer != null)
			return;

		cannoneer = new GUIButton ();
		cannoneer.layer = -3;
		cannoneer.texture = Resources.Load ("Textures/UserInterface/" + name) as Texture;
		cannoneer.OnClick = (t) => {
			Tower1Click (name, position, tower1Action, tower2Action, tower3Action);
		};
		cannoneer.gameObject.name += "TowerPlaceInterface";
        
		cannoneer.positionInMeters = position - new Vector2 (354/2f/50f, 0) * Settings.FhdToHD;
        if (cannoneerText == null) cannoneerText = new GUIText (TowersSettings.GetPrice (name) +"", -2.5f, cannoneer.positionInMeters + new Vector2 (0, -91/50f) * Settings.FhdToHD
            , new Vector2 (0.13f, 0.13f));
		cannoneer.sizeInMeters = new Vector2 (170/50f, 252/50f) * Settings.FhdToHD;
	}
	
	private void CreateTower3Button (string name, Vector2 position, Actions.VoidVoid tower1Action, Actions.VoidVoid tower2Action, Actions.VoidVoid tower3Action) {
		
		if (steam != null)
			return;
		
		steam = new GUIButton ();
		steam.layer = -3;
		steam.texture = Resources.Load ("Textures/UserInterface/" + name) as Texture;
		steam.OnClick = (t) => {
			Tower3Click (name, position, tower1Action, tower2Action, tower3Action);
		};
		steam.gameObject.name += "TowerPlaceInterface";
		steam.positionInMeters = position + new Vector2 (354/2f/50f, 0) * Settings.FhdToHD;
        if (steamText == null) steamText = new GUIText (TowersSettings.GetPrice (name) +"", -2.5f, steam.positionInMeters + new Vector2 (0, -93/50f) * Settings.FhdToHD
            , new Vector2 (0.13f, 0.13f));
		steam.sizeInMeters = new Vector2 (170/50f, 252/50f) * Settings.FhdToHD;
	}
	
	private void CreateTower2Button (string name, Vector2 position, Actions.VoidVoid tower1Action, Actions.VoidVoid tower2Action, Actions.VoidVoid tower3Action) {
		
		if (tesla != null)
			return;

		tesla = new GUIButton ();
		tesla.layer = -3;
		tesla.texture = Resources.Load ("Textures/UserInterface/" + name) as Texture;
		tesla.OnClick = (t) => {
			Tower2Click (name, position, tower1Action, tower2Action, tower3Action);
		};
		tesla.gameObject.name += "TowerPlaceInterface";
        
		tesla.positionInMeters = position + new Vector2 (0, 373/2f/50f) * Settings.FhdToHD;
        if (teslaText == null) teslaText = new GUIText (TowersSettings.GetPrice (name) +"", -2.5f, tesla.positionInMeters + new Vector2 (0, -96/50f) * Settings.FhdToHD
            , new Vector2 (0.13f, 0.13f));
		tesla.sizeInMeters = new Vector2 (178/50f, 261/50f) * Settings.FhdToHD;
	}
	
	private void Tower1Click (string name, Vector2 position, Actions.VoidVoid tower1Action, Actions.VoidVoid tower2Action, Actions.VoidVoid tower3Action) {
		
		if (confirm != null) {
			confirm.Destroy ();
			confirm = null;
		};
		
		CreateTower2Button (name, position, tower1Action, tower2Action, tower3Action);
		CreateTower3Button (name, position, tower1Action, tower2Action, tower3Action);
		cannoneer.Destroy ();
		cannoneer = null;
        //cannoneerText.Destroy ();
        //cannoneerText = null;
		
		confirm = new GUIButton ();
		confirm.layer = -3;
		confirm.texture = Resources.Load ("Textures/UserInterface/Accept") as Texture;
		confirm.positionInMeters = position - new Vector2 (354/2f/50f, 0) * Settings.FhdToHD;
		confirm.sizeInMeters = new Vector2 (-1, -1) * Settings.FhdToHD;
		confirm.gameObject.name +="TowerPlaceInterface";
		confirm.OnClick = (t) => { tower1Action (); RemoveTowerPlaceInterface ();};
	}

	private void Tower3Click (string name, Vector2 position, Actions.VoidVoid tower1Action, Actions.VoidVoid tower2Action, Actions.VoidVoid tower3Action) {
		
		if (confirm != null) {
			confirm.Destroy ();
			confirm = null;
		};
		
		CreateTower2Button (name, position, tower1Action, tower2Action, tower3Action);
		CreateTower1Button (name, position, tower1Action, tower2Action, tower3Action);
		steam.Destroy ();
		steam = null;
        //steamText.Destroy ();
        //steamText = null;
		
		confirm = new GUIButton ();
		confirm.layer = -3;
		confirm.texture = Resources.Load ("Textures/UserInterface/Accept") as Texture;
		confirm.positionInMeters = position + new Vector2 (354/2f/50f, 0) * Settings.FhdToHD;
		confirm.sizeInMeters = new Vector2 (-1, -1) * Settings.FhdToHD;
		confirm.gameObject.name +="TowerPlaceInterface";
		confirm.OnClick = (t) => { tower3Action (); RemoveTowerPlaceInterface ();};
	}

	private void Tower2Click (string name, Vector2 position, Actions.VoidVoid tower1Action, Actions.VoidVoid tower2Action, Actions.VoidVoid tower3Action) {

		
		if (confirm != null) {
			confirm.Destroy ();
			confirm = null;
		}

		CreateTower1Button (name, position, tower1Action, tower2Action, tower3Action);
		CreateTower3Button (name, position, tower1Action, tower2Action, tower3Action);
		tesla.Destroy ();
		tesla = null;
        //teslaText.Destroy ();
        //teslaText = null;
		
		confirm = new GUIButton ();
		confirm.layer = -3;
		confirm.texture = Resources.Load ("Textures/UserInterface/Accept") as Texture;
		confirm.positionInMeters = position + new Vector2 (0, 373/2f/50f) * Settings.FhdToHD;
		confirm.sizeInMeters = new Vector2 (-1, -1) * Settings.FhdToHD;
		confirm.gameObject.name +="TowerPlaceInterface";
		confirm.OnClick = (t) => { tower2Action (); RemoveTowerPlaceInterface ();};


	}

    private void CreateTowerPlaceInterface (List <string> towers, Vector2 position
        , Actions.VoidVoid tower1Action, Actions.VoidVoid tower2Action, Actions.VoidVoid tower3Action) {

		round = new GUIImage ();
		round.texture = Resources.Load ("Textures/UserInterface/UpgradeBase") as Texture;
		round.layer = -4;
		round.positionInMeters = position;
		round.sizeInMeters = new Vector2 (354/50f, 373/50f) * Settings.FhdToHD;
		round.gameObject.name += "TowerPlaceInterface";

        Logger.Log (towers);

		CreateTower1Button (towers [0], position, tower1Action, tower2Action, tower3Action);
		CreateTower2Button (towers [1], position, tower1Action, tower2Action, tower3Action);
		CreateTower3Button (towers [2], position, tower1Action, tower2Action, tower3Action);
	}

    public static void RemoveTowerPlaceInterface () {

		if (round == null)
			return;

		round.Destroy ();
		round = null;
		if (cannoneer != null)
			cannoneer.Destroy ();

		if (tesla != null)
			tesla.Destroy ();
		
		if (confirm != null)
			confirm.Destroy ();

		if (steam != null)
			steam.Destroy ();
        
		if (cannoneerText != null)
			cannoneerText.Destroy ();

		if (teslaText != null)
			teslaText.Destroy ();
		
		if (confirmText != null)
			confirmText.Destroy ();

		if (steamText != null)
			steamText.Destroy ();

		cannoneer = null;
		tesla = null;
		confirm = null;
		steam = null;
        cannoneerText = null;
        teslaText = null;
        steamText = null;

	}

    public void OnWin () {

        if (units.Count > 0) {

            return;
        }

        int stars = 5 - startLife + PlayerController.health;

        if (stars <= 0) {

            stars = 1;
        }

        Settings.level = Mathf.Max (Settings.level, LevelController.level + 1);

        if (isEnded) {

            return;
        }
        
        isEnded = true;

        Settings.SetStars (LevelController.level, stars);

        ScenePassageController.instance.LoadScene <SelectLevelController> ((a) => {Destroy (); a ();});
    }

    public void OnLost () {
        
        if (isEnded) {

            return;
        }
        
        isEnded = true;

        ScenePassageController.instance.LoadScene <SelectLevelController> ((a) => {Destroy (); a ();});
    }

	public GameController () {
        
        GUIObject.StartListening ();

        instance = this;

        startLife = 25;
        killedUnitsToHeal = 0;
        
        UpdateController.toUpdate = Update;
        UpdateController.toFixedUpdate = FixedUpdate;

        Screen.sleepTimeout = SleepTimeout.NeverSleep;

		AnimationBox.LoadAnimations ();
		AnimationController.Create ();
        new ForbidController ();
		new GUIController ();
                
		new BalanceSettings ();
        new TowersSettings ();
        new GUIController ();
        new LazerController ();
        LoadBalance ();

        id ++;

        UpdateController.AddClickListener ("OnClick", "GameControllerOnClick" + id, OnClick);

		Create ();

        foreach (var v in GUIObject.Listen ()) {
            
            Debug.Log (v.gameObject.name);

            //v.sizeInMeters *= Settings.FhdToHD;
            //v.positionInMeters *= Settings.FhdToHD;
        }
    }

    private void FixedUpdate (float deltaTime) {
		
        if (timeDoubleDamage > 0) {

            timeDoubleDamage -= deltaTime;
        }

		if (isUnderPowerUpsOpening && underPowerUpsPosition < 1) {

            underPowerUpsPosition += 0.04f;
		} 
		
		if (!isUnderPowerUpsOpening && underPowerUpsPosition > 0) {

            underPowerUpsPosition -= 0.04f;
		}

		
		playerLife = PlayerController.health.ToString ();
		playerMoney = PlayerController.money.ToString ();
        
        if (PlayerController.health <= 0) {

            OnLost ();
            return;
        }

		if (isPaused || !isStarted)
			return;

        LazerController.Update (deltaTime);

		if (timeSkillFreezing > 0) {
			timeSkillFreezing -= deltaTime;
			CameraController.cameraPosition += new Vector2 (Random.value-0.5f, Random.value-0.5f)/2f;
		}
		
		if (skill1Cooldown > 0) {
			skill1Cooldown -= deltaTime;
		}
		
		if (skill2Cooldown > 0) {
			skill2Cooldown -= deltaTime;
		}
		
		if (skill3Cooldown > 0) {
			skill3Cooldown -= deltaTime;
		}

		foreach (var unit in units) {

			unit.Update (deltaTime);
		}
		
		foreach (var tower in towers) {
			tower.Update (deltaTime);
		}

		foreach (var missile in missiles) {
			missile.Update (deltaTime);
		}
		
		foreach (var missile in missilesToDestroy) {
			missiles.Remove (missile);
		}

		foreach (var unit in unitsToDestroy) {
			units.Remove (unit);
		}
		
		unitsToDestroy.Clear ();
		missilesToDestroy.Clear ();

		//BotController.Update (deltaTime);
		PlayerController.Update (deltaTime);
	}


	private void Update (float deltaTime) {

        UpdateInfo ();

		if (Input.GetKeyDown (KeyCode.Escape)) {
            
            if (!MenusController.RemoveMenus ()) {

                ScenePassageController.instance.LoadScene <SelectLevelController> ((a) => {Destroy (); a ();});
            }

        }

        if (!isPaused) {

            AnimationController.Update (deltaTime);
            LightningController.Update (deltaTime);
        }

        OptionsController.instance.Update (deltaTime);
        
        if (!ForbidController.IsForbidden ("GameControllerClick")) {
            
            SlideController.frictionDelta = CameraController.widthInMeters/Screen.width;
		    SlideController.instance.Update (deltaTime);
        }

        if (damageImage != null) {
                
            damageImage.positionInMeters = CameraController.cameraPosition;
        }
	}

    public void Destroy () {

        instance = null;
        
        UpdateController.toFixedUpdate = null;
        UpdateController.toUpdate = null;

        AnimationController.Destroy ();
        OptionsController.instance.Destroy ();
        UpdateController.RemoveClickListener ("GameControllerOnClick" + id);
        GamePullController.Destroy ();
        UpdateController.StopAllTimers ();
    }

}
