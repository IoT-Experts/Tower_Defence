using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UnitsSettings {
    
    public static UnitsSettings instance;
    
	private static Dictionary <string, Vector2> shadowPosition;
	private static Dictionary <string, Vector2> healthBarPosition;
	private static Dictionary <string, List <string>> buffs;
    
    private static void CheckInstance () {

        if (instance == null) {

            new UnitsSettings ();
        }
    }
    
    public static Vector2 ShadowPosition (string target) {

        CheckInstance ();

        if (shadowPosition.ContainsKey (target)) {

            return shadowPosition [target];
        }
        
        Debug.LogError (target + "is not found");
        return new Vector2 (0, 0);
    }
    
    public static Vector2 HealthBarPosition (string target) {

        CheckInstance ();

        if (healthBarPosition.ContainsKey (target)) {

            return healthBarPosition [target];
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

    public UnitsSettings () {

        instance = this;
        
        shadowPosition = new Dictionary<string, Vector2> ();
        healthBarPosition = new Dictionary<string, Vector2> ();
        buffs = new Dictionary<string, List<string>> ();

        var text = (ResourcesController.LoadOnce ("Units") as TextAsset).text;
        var units = text.Split ('\n');

        foreach (var s in units) {

            if (s == "") {

                continue;
            }

            var res = s.Split ('|');

            var name = res [0];
        
            var bfs = res [1].Split ('☺');

            if (!buffs.ContainsKey (name)) {

                buffs.Add (name, new List<string> ());
            }
            
            for (int i = 1; i < bfs.Length; i++) {

                buffs [name].Add (bfs [i]);
            }
            if (!shadowPosition.ContainsKey (name)) {

                shadowPosition.Add  (name, new Vector2 (float.Parse (res [2].Split ('☺') [0]), float.Parse (res [2].Split ('☺') [1])));
            }

            if (!healthBarPosition.ContainsKey (name)) {

                healthBarPosition.Add  (name, new Vector2 (float.Parse (res [3].Split ('☺') [0]), float.Parse (res [3].Split ('☺') [1])));
            }
        }
      
    }
}
