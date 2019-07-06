using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

public class Settings {

    public const float FhdToHD = 720f / 1080;

    public enum LevelType {Normal, Endless};

    public static LevelType levelType;

    public static string language {
		get { 
			if (!PlayerPrefs.HasKey ("Language")) {

                if (LanguageController.IsKnownLanguage (Application.systemLanguage.ToString ())) {

                    PlayerPrefs.SetString ("Language", Application.systemLanguage.ToString ());
                } else {

                    if (LanguageController.IsKnownLanguage ("English")) {
                        
                        PlayerPrefs.SetString ("Language", "English");
                    } else {
                        
                        PlayerPrefs.SetString ("Language", LanguageController.languages [0]);
                    }
                }
            }

			return PlayerPrefs.GetString ("Language"); 
		}

		set { 

			PlayerPrefs.SetString ("Language", value);
            GUIController.OnLanguageChange ();
		}
	}
    
    public static List<string> GetItems() {

         return new List<string> {

            "Freezing"
            , "Bomb"
            , "Gold"
            , "Heal"
            , "Power"
        };
    }
    
    public static List <List<string>> GetSkills() {

         return new List<List<string>> {
             
             new List<string> {
                 
                 "LightningDamage"
                 , "Lightning20"
                 , "LightningCritical"
                 , "Lightning50"
                 , "LightningCriticalPower"
             }
             
             , new List<string> {
                 
                 "MagicAttackSpeed"
                 , "Magic20"
                 , "Chaotic"
                 , "Magic50"
                 , "Fatal"
             }
             
             , new List<string> {
                 
                 "LazerEffect"
                 , "Lazer20"
                 , "UnitSlow"
                 , "Lazer50"
                 , "UnitLife"
             }
         };
    }

    public static bool isTutorial {
		get { 
			if (!PlayerPrefs.HasKey ("isTutorial")) 
			    return true;
			
			return (PlayerPrefs.GetInt ("isTutorial") == 1);
		}
		set {
			PlayerPrefs.SetInt ("isTutorial", value ? 1 : 0);
		}
		
	}
    
	public const string adsIdAndroid = "ca-app-pub-4782252445867445/3279633612";
    public const string bannerIdAndroid = "ca-app-pub-4782252445867445/9557347216";

	public static bool music {
		get {

            if (!PlayerPrefs.HasKey ("music"))
                return true;

			return (PlayerPrefs.GetInt ("music") == 1);
		}
		set {

            if (value) 
                AudioController.instance.UnMuteMusic ();
           else 
                AudioController.instance.MuteMusic ();

			PlayerPrefs.SetInt ("music", value ? 1 : 0);
		}
		
	}
	
	public static bool sounds {
		get {
            
            if (!PlayerPrefs.HasKey ("sounds"))
                return true;

            return (PlayerPrefs.GetInt ("sounds") == 1); }
		set {

            if (value) 
                AudioController.instance.UnMuteSounds ();
           else 
                AudioController.instance.MuteSounds ();

			PlayerPrefs.SetInt ("sounds", value ? 1 : 0);
		}
		
	}

	public static bool vibration {

		get {
            
            if (!PlayerPrefs.HasKey ("vibration"))
                return false;

            return (PlayerPrefs.GetInt ("vibration") == 1); }
		set {

			PlayerPrefs.SetInt ("vibration", value ? 1 : 0);
		}
		
	}

    public static int diamonds {

        get {

            if (!PlayerPrefs.HasKey ("diamonds")) {

                PlayerPrefs.SetInt ("diamonds", 100);
            }

            return PlayerPrefs.GetInt ("diamonds");
        }

        set {

            PlayerPrefs.SetInt ("diamonds", value);
        }
    }

    public static int stars {

        get {

            if (!PlayerPrefs.HasKey ("stars")) {

                PlayerPrefs.SetInt ("stars", 0);
            }

            return PlayerPrefs.GetInt ("stars");
        }

        set {

            PlayerPrefs.SetInt ("stars", value);
        }
    }

    
    public static int level {

        get {

            if (!PlayerPrefs.HasKey ("currentLevel")) {

                PlayerPrefs.SetInt ("currentLevel", 1);
            }

            return PlayerPrefs.GetInt ("currentLevel");
        }

        set {

            PlayerPrefs.SetInt ("currentLevel", value);
        }
    }

    public static bool isAds {

        get {

            if (!PlayerPrefs.HasKey ("isAds")) {

                PlayerPrefs.SetInt ("isAds", 1);
            }

            return PlayerPrefs.GetInt ("isAds") == 1;
        }

        set {

            PlayerPrefs.SetInt ("isAds", value ? 1 : 0);
        }
    }

    public static string settingsPath  {
		
		get {return PlayerPrefs.GetString ("settingsPath");}
		set {PlayerPrefs.SetString ("settingsPath", value);}
		
	}
    
    public static string likeURL = "https://www.facebook.com/";
    public static string rateURL = "https://www.play.google.com/";
		
    public static string GetText (string key) {

        return TranslationsController.GetText (key, language);
    }

    public Settings () {

    }

    public static bool AddItemCount (string item, int toAdd) {

        if (PlayerPrefs.HasKey (item + "Count")) {

            if (PlayerPrefs.GetInt (item + "Count") + toAdd >= 0) {

                PlayerPrefs.SetInt (item + "Count", PlayerPrefs.GetInt (item + "Count") + toAdd);
                return true;
            } else {

                return false;
            }
        } else {

            if (toAdd < 0) {

                return false;
            } else {

                PlayerPrefs.SetInt (item + "Count", toAdd);
                return true;
            }

        }
    }

    public static int GetItemCount (string item) {

        if (PlayerPrefs.HasKey (item + "Count")) {

            return PlayerPrefs.GetInt (item + "Count");
        }

        return 0;
    }

    public static void SetRecordWords (int words) {

        if (!PlayerPrefs.HasKey (level + "_words")) { 

				PlayerPrefs.SetInt (level + "_words", words);
                return;
        }

        PlayerPrefs.SetInt (level + "_words", PlayerPrefs.GetInt (level + "_words") == 0 ? words 
            : Mathf.Min (words, PlayerPrefs.GetInt (level + "_words")));
    }
    
    public static void SetRecordPoints (int points) {

        if (!PlayerPrefs.HasKey (level + "_points")) { 

				PlayerPrefs.SetInt (level + "_points", points);
                return;
        }

        PlayerPrefs.SetInt (level + "_points", Mathf.Max (points, PlayerPrefs.GetInt (level + "_points")));
    }
    
    public static void SetStars (int currentLevel, int stars) {

        if (!PlayerPrefs.HasKey (currentLevel + "_stars")) { 

                Settings.stars += stars;
				PlayerPrefs.SetInt (currentLevel + "_stars", stars);
                return;
        }

        Settings.stars -= PlayerPrefs.GetInt (currentLevel + "_stars");
        Settings.stars += Mathf.Max (stars, PlayerPrefs.GetInt (currentLevel + "_stars"));

        PlayerPrefs.SetInt (currentLevel + "_stars", Mathf.Max (stars, PlayerPrefs.GetInt (currentLevel + "_stars")));
    }
    
    public static int GetStars (int currentLevel) {

        if (!PlayerPrefs.HasKey (currentLevel + "_stars")) { 

                return 0;
        }

        return PlayerPrefs.GetInt (currentLevel + "_stars");
    }

    public static bool AddTotalStars (int toAdd) {

        if (!PlayerPrefs.HasKey ("stars")) { 

				PlayerPrefs.SetInt ("stars", 0);
        }

        if (toAdd + PlayerPrefs.GetInt ("stars") >= 0) {

            PlayerPrefs.SetInt ("stars", PlayerPrefs.GetInt ("stars") + toAdd);
            return true;
        }

        return false;
    }
    
    public static int GetTotalStars () {

        if (!PlayerPrefs.HasKey ("stars")) { 

                return 0;
        }

        return PlayerPrefs.GetInt ("stars");
    }

    public static void BuySkill (string skill) {

		PlayerPrefs.SetInt (skill + "_learned", 1);
    }

    public static void SellSkill (string skill) {

		PlayerPrefs.SetInt (skill + "_learned", 0);
    }
    
    public static bool IsLearned (string skill) {

        if (!PlayerPrefs.HasKey (skill + "_learned")) { 

                return false;
        }

        return PlayerPrefs.GetInt (skill + "_learned") == 1;
    }
    
    private static string baseFontLetters = "AaBbCcDdEeFfGgHhIiJjKkLlMmNnOoPpQqRrSsTtUuVvWwXxYyZz[{\\|]}^~_#&</>";
    
    private static char GetRightChar (char c, ref string fontLetters) {


        for (int i = 0; i < fontLetters.Length ; i++) {
         
            if (fontLetters [i] == c)
            return baseFontLetters [i];
        }

        return c;
    }

    public static string TranslateText (string text, bool isCustomLanguage = false, string customLanguage = "English") {

        string toUseLanguage = isCustomLanguage ? customLanguage : language;
        
        var fontLetters = LanguageController.LanguageFontLetters (toUseLanguage);
        string res = "";

        for (int i = 0; i < text.Length; i++) {

            res += GetRightChar (text [i], ref fontLetters);
        }

        return res;
    }

    public static string ReadFromFile (string path) {

        StreamReader file = new StreamReader (path);
		string res = file.ReadToEnd();
		file.Close();

        return res;
    }
}
