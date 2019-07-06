using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TowersSettings {
    
    public static TowersSettings instance;
    
    public static List <string> startTowers;
	private static Dictionary <string, Vector2> attackPosition;
	private static Dictionary <string, Vector2> towerPlacePosition;
	private static Dictionary <string, List <string>> upgrades;
	private static Dictionary <string, List <string>> buffs;
    
    private static void CheckInstance () {

        if (instance == null) {

            new TowersSettings ();
        }
    }
    
    public static int GetPrice (string name) {

        CheckInstance ();

        int price = BalanceSettings.price [name];
        int result = price;
        
        if (Buffs (name).Contains (Tower.Buff.LazerSkills.ToString ())) {
            
            if (Settings.IsLearned ("Lazer20")) {

                result = price * 80 / 100;
            }
            
            if (Settings.IsLearned ("Lazer50")) {

                result = price * 50 / 100;
            }
        }
        
        if (Buffs (name).Contains (Tower.Buff.LightningSkills.ToString ())) {
            
            if (Settings.IsLearned ("Lightning20")) {

                result = price * 80 / 100;
            }
            
            if (Settings.IsLearned ("Lightning50")) {

                result = price * 50 / 100;
            }
        }
        
        if (Buffs (name).Contains (Tower.Buff.MagicSkills.ToString ())) {
            
            if (Settings.IsLearned ("Magic20")) {

                result = price * 80 / 100;
            }
            
            if (Settings.IsLearned ("Magic50")) {

                result = price * 50 / 100;
            }
        }

        Logger.Log (name, false);
        Logger.Log (Buffs (name), false);
        Logger.Log (result);
        Logger.Log (Settings.IsLearned ("Magic20") + " " + Settings.IsLearned ("Lightning20") + " " + Settings.IsLearned ("Lazer20"));

        return result;
    }
    
    public static Vector2 AttackPosition (string target) {

        CheckInstance ();

        if (attackPosition.ContainsKey (target)) {

            return attackPosition [target];
        }
        
        Debug.LogError (target + "is not found");
        return new Vector2 (0, 0);
    }
    
    public static Vector2 TowerPlacePosition (string target) {

        CheckInstance ();

        if (towerPlacePosition.ContainsKey (target)) {

            return towerPlacePosition [target];
        }
        
        Debug.LogError (target + "is not found");
        return new Vector2 (0, 0);
    }

    public static List <string> Buffs (string target) {
        
        CheckInstance ();

        if (buffs.ContainsKey (target)) {

            return buffs [target];
        }
        
        Debug.LogError (target + "is not found");
        return new List<string> ();
    }

    public static List <string> Upgrades (string target) {
        
        CheckInstance ();

        if (upgrades.ContainsKey (target)) {

            return upgrades [target];
        }
        
        Debug.LogError (target + "is not found");
        return new List<string> ();
    }
    
    public TowersSettings () {

        instance = this;
        
        attackPosition = new Dictionary<string, Vector2> ();
        towerPlacePosition = new Dictionary<string, Vector2> ();
        buffs = new Dictionary<string, List<string>> ();
        upgrades = new Dictionary<string, List<string>> ();
        startTowers = new List<string> ();

        var text = (ResourcesController.LoadOnce ("Towers") as TextAsset).text;
        var towers = text.Split ('\n');

        foreach (var s in towers) {

            if (s == "") {

                continue;
            }

            var res = s.Split ('|');

            var name = res [0];

            if (name == "") {

                continue;
            }
        
            var ups = res [1].Split ('☺');
            
            upgrades.Add (name, new List<string> ());

            for (int i = 1; i < ups.Length; i++) {

                upgrades [name].Add (ups [i]);
            }

            var bfs = res [2].Split ('☺');

            buffs.Add (name, new List<string> ());
            
            for (int i = 1; i < bfs.Length; i++) {

                buffs [name].Add (bfs [i]);
            }

            if (res [3] == "True") {

                startTowers.Add (name);
            }

            attackPosition.Add  (name, new Vector2 (float.Parse (res [4].Split ('☺') [0]), float.Parse (res [4].Split ('☺') [1])));
            towerPlacePosition.Add  (name, new Vector2 (float.Parse (res [5].Split ('☺') [0]), float.Parse (res [5].Split ('☺') [1])));
        }
      
    }
}
