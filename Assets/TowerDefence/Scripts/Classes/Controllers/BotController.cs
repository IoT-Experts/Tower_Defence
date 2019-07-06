using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BotController : MonoBehaviour {

	public static int money;
	public static int health;
	public static Colorable.TeamColor color;
	
	private static int playProgress;

	public static WavesController wavesController;
	
	private static List <GameObject> towerPlaces {

		get {
			
			if (color == Colorable.TeamColor.Red)
				return GameController.towerPlacesRed;
			else
				return GameController.towerPlacesBlue;

		}

	}

	
	public static void Create () {
		
		money = 150;
		health = 25;
		playProgress = 0;
		color = Colorable.TeamColor.Blue;
		wavesController = new WavesController (color);
		wavesController.Load (Settings.settingsPath);
	}


	
	public static void Update (float deltaTime) {
        /*
		wavesController.Update (deltaTime);
		if (wavesController.isWaitingForWave)
			wavesController.StartWave ();

		if (health <= 0) 
			health = 0;

		if (towerPlaces.Count <= 0) 
			return;

		var rnd = Random.Range (0, towerPlaces.Count);
		var towerPlace = towerPlaces[rnd];

		switch (playProgress) {

		case 1:
		playProgress +=GameController.CreateTower (Tower.TowerType.Steam, color, towerPlace, BalanceSettings.price["Steam"])?1:0;
			break;
		default:
		playProgress +=GameController.CreateTower (Tower.TowerType.Tesla, color, towerPlace, BalanceSettings.price["Tesla"])?1:0;
			break;
            
		}
		*/
	}
}
