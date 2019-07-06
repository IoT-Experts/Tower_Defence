using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

public class LevelController {
	
    public static int level;

	private static Vector3 ParseRoad (string road) {
		var roads = road.Split ('$');
		return new Vector3 (float.Parse (roads[0]), float.Parse (roads[1]), float.Parse (roads[2]));
	}


	private static void ParseMap (string map) {


		var maps = map.Split ('|');
		
		int i = 0;

		GameController.RoadRed = new List<List<Vector3>>();
		GameController.RoadBlue = new List<List<Vector3>>();

		for (int q = 0; q < 6; q++) {

			GameController.RoadRed.Add (new List<Vector3>());

			int lng = int.Parse (maps[i]);

			i++;
			for (int p = 0; p < lng; p++) {
				GameController.RoadRed[q].Add (ParseRoad (maps[i]));

				i++;
			}

		}
		
		
		for (int q = 0; q < 6; q++) {
			
			GameController.RoadBlue.Add (new List<Vector3>());
			
			int lng = int.Parse (maps[i]);
			i++;
			for (int p = 0; p < lng; p++) {
				GameController.RoadBlue[q].Add (ParseRoad (maps[i]));
				i++;
			}
		}

        
        Logger.LogList (GameController.RoadBlue);
        Logger.LogList (GameController.RoadRed);

		for (; i < maps.Length - 1; i+=7) {
			switch (maps[i]) {

			case "Layer":
				

				GameObject mapPlane = GamePullController.CreateImage ();
				
				float x, y, w, h;
				w = float.Parse (maps[i+5]);
				h = float.Parse (maps[i+6]);
				x=float.Parse (maps[i+3]);
				y=float.Parse (maps[i+4]);
				mapPlane.transform.position = new Vector3 (x, int.Parse (maps[i+1]), y);
				mapPlane.transform.localScale = new Vector3 (w, h, 1);
				mapPlane.name = "Map";
				mapPlane.GetComponent<Renderer>().material.mainTexture = Resources.Load ("Levels/Level" + level + "/"+maps[i+2].Split ('.')[0]) as Texture;

				if (maps[i+2].Split ('.')[0] == "Map") {

					GameController.mapWidth = w;
					GameController.mapHeight = h;

				}
				
				break;

			case "Tower":
					
				GameObject towerPlace = GamePullController.CreateImage ();
                towerPlace.gameObject.GetComponent <BoxCollider> ().enabled = true;

				switch (maps[i+2]) {

				    case "Red":
					    GameController.towerPlacesRed.Add (towerPlace);
					    break;
				    case "Blue":
					    GameController.towerPlacesBlue.Add (towerPlace);
					    break;
				}

				towerPlace.transform.position = new Vector3 (float.Parse (maps[i+3]), float.Parse (maps[i+1]), float.Parse (maps[i+4]));
				towerPlace.GetComponent<Renderer>().material.mainTexture 
					= Resources.Load ("Textures/Towers/TowerPlace"+maps[i+2]) as Texture;
				towerPlace.transform.localScale = new Vector3 (3.08f* Settings.FhdToHD, 4.13f* Settings.FhdToHD, 1);
				towerPlace.name = "TowerPlace"+maps[i+2];
				break;

			default:
				
				/*
				PointObject pointObject = new PointObject (PointObject.PointObjectType.Road, 0.6f, 0.6f,
				                                           new Vector2 (float.Parse (maps[i+3]), float.Parse (maps[i+4])), maps[i] + maps[i+2]);
				*/
				break;
				
				
			}
			
			
		}
		
	}
	
	
	public static void LoadMap () {

#if UNITY_STANDALONE_WIN_FALSE

		StreamReader file = new StreamReader (MainMenuController.settingsPath + "/Level.txt");
		ParseMap (file.ReadToEnd ());
		file.Close ();
#else
		ParseMap ((Resources.Load ("Levels/Level" + level + "/Level") as TextAsset).text);
#endif	

		
	}
}
