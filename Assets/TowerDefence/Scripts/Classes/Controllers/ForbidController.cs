using UnityEngine;
using System.Collections.Generic;

public class ForbidController {

    public static ForbidController instance;

	private static int forbidLevel;
    private static Dictionary <string, bool> forbids;
    
    public static void Forbid () {

        forbidLevel ++;
    }

    public static void Forbid (string type) {

        if (forbids.ContainsKey (type)) {

            forbids [type] = true;
        } else {

            forbids.Add (type, true);
        }
    }

    public static void Allow () {
    
        forbidLevel --;
    }

    public static void Allow (string type) {

        if (forbids.ContainsKey (type)) {

            forbids [type] = false;
        } else {

            forbids.Add (type, false);
        }
    }

    public static bool IsForbidden () {

        int res = forbidLevel;

        foreach (var f in forbids) {

            if (f.Value) {

                res ++;
            }
        }
        
        return res > 0;
    }

    public static bool IsForbidden (string name) {

        return forbids.ContainsKey (name);
    }

    public ForbidController () {

        instance = this;

        forbids = new Dictionary<string, bool> ();
        forbidLevel = 0;
    }
}
