using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BombsController {

	public static List < Bomb > bombs;

	public static void Create () {

		bombs = new List<Bomb> ();
	}

	public static void Update (Killable target) {

		for (int i = bombs.Count -1; i>=0; i--) { 

			bombs[i].Update (target);
		}

	}

}
