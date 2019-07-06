using UnityEngine;
using System.Collections;

public class PlayerController {
	
	public static int money;
	public static int health;
	public static Colorable.TeamColor color;

	public static WavesController wavesController;

	public static void Create () {

		money = 160;
		health = 25;
		color = Colorable.TeamColor.Red;
		wavesController = new WavesController (Colorable.TeamColor.Blue);
		wavesController.Load (Settings.settingsPath);
	}

	public static void Update (float deltaTime) {

		wavesController.Update (deltaTime);
		
		if (wavesController.isWaitingForWave)
			wavesController.StartWave ();

		if (health <= 0)
            { }//TODO Application.LoadLevel ("MainMenuScene");
	}


}
