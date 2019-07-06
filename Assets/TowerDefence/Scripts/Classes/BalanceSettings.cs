using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BalanceSettings {


	public static Dictionary <string, int> price;
	public static Dictionary <string, float> damage;
	public static Dictionary <string, float> deltaDamage;
	public static Dictionary <string, float> attackRange;
	public static Dictionary <string, float> attackSpeed;
	public static Dictionary <string, int> health;
	public static Dictionary <string, float> speed;
	public static Dictionary <string, float> defence;


	public static void ParseBalance (string map) {

		var maps = map.Split ('|');

		int i = 1;
		int q;
		float w;
		Debug.Log (map);
		while (i < maps.Length) {

			deltaDamage[maps[i+0]] = 0;

			if (int.TryParse (maps[i+1], out q)) price[maps[i+0]] = int.Parse (maps[i+1]);
			if (maps[i+2] != "") damage[maps[i+0]] = float.Parse (maps[i+2].Split ('$')[0]);
			if (maps[i+2] != "" & maps[i+2].Contains ("$")) deltaDamage[maps[i+0]] = float.Parse (maps[i+2].Split ('$')[1]);
			if (float.TryParse (maps[i+3], out w)) attackRange[maps[i+0]] = float.Parse (maps[i+3]);
			if (float.TryParse (maps[i+4], out w)) attackSpeed[maps[i+0]] = float.Parse (maps[i+4]);
			if (int.TryParse (maps[i+5], out q)) health[maps[i+0]] = int.Parse (maps[i+5]);
			if (float.TryParse (maps[i+6], out w)) speed[maps[i+0]] = float.Parse (maps[i+6]);
			if (float.TryParse (maps[i+7], out w)) defence[maps[i+0]] = float.Parse (maps[i+7]);
			i+=8;

		}

	}

	public BalanceSettings () {

		price = new Dictionary<string, int> ();
		damage = new Dictionary<string, float> ();
		deltaDamage = new Dictionary<string, float> ();
		attackRange = new Dictionary<string, float> ();
		attackSpeed = new Dictionary<string, float> ();
		health = new Dictionary<string, int> ();
		speed = new Dictionary<string, float> ();
		defence = new Dictionary<string, float> ();




	}
	

}
