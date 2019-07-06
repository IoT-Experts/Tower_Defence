using UnityEngine;
using System.Collections.Generic;

public class LazerController {

	private static List <Lazer> lazers = new List<Lazer> ();

    public static void Add (Lazer lazer) {

        lazers.Add (lazer);
    }

    public static void Remove (Lazer lazer) {

        lazers.Remove (lazer);
    }

    public static void Update (float deltaTime) {

        for (int i = lazers.Count - 1; i >= 0; i--) {

            lazers [i].Update (deltaTime);
        }
    }

    public LazerController () {

        lazers = new List<Lazer> ();
    }
}
