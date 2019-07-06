using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

public class WavesController {

	public bool isWaitingForWave;
    
    private static string lastMap;
    private static float lastTime;

	private float currentTime;
	private Dictionary <float, List <WaveElement> > waves;

	private List < WaveElement > toUse;

	public Colorable.TeamColor color;

	public WavesController (Colorable.TeamColor _color) {

		color = _color;
		isWaitingForWave = true;
		currentTime = 0;
		waves = new Dictionary<float, List<WaveElement>> ();
		toUse = new List<WaveElement> ();
	}

	public void StartWave () {

		isWaitingForWave = false;
	}


	
	void ParseMap (string map) {

        lastMap = map;
		
		var maps = map.Split ('|');
		
		int i = 1;

		float time = 0;

		for (; i < maps.Length; i+=5) {
			
			var b = new WaveElement (maps[i+0], int.Parse (maps[i+1]), float.Parse (maps[i+2]), float.Parse (maps[i+3]), int.Parse (maps[i+4]));
            
            if (lastTime < time + b.startTime + b.deltaTime * b.count) {

                lastTime = time + b.startTime + b.deltaTime * b.count;
            }

			if (waves.ContainsKey (time+b.startTime)) {

				waves[time+b.startTime].Add (b);

			} else {

				waves.Add (time+b.startTime, new List<WaveElement> ());
				waves[time+b.startTime].Add (b);
			}

			if (b.type == "-1")
				time += b.startTime;
		}
	}

	public void Load (string mapPath) {

		if (UpdateController.platform == UpdateController.Platform.Phone || true) {
			ParseMap ((Resources.Load ("Levels/Level" + LevelController.level + "/Waves")as TextAsset).text);
			
		} else {
			StreamReader file = new StreamReader (mapPath + "/Waves.txt");
			ParseMap (file.ReadToEnd ());
			file.Close ();
		}
	}

    private void Reload () {

        Debug.Log ("Reload");

        currentTime = 0;
		waves = new Dictionary<float, List<WaveElement>> ();
		toUse = new List<WaveElement> ();
        ParseMap (lastMap);
    }

	public void Update (float deltaTime) {

		if (isWaitingForWave)
			return;

        if (currentTime > lastTime) {

            if (Settings.levelType == Settings.LevelType.Endless) {

                Reload ();
            } else {

                GameController.instance.OnWin ();
            }
            return;
        }

		if (waves.ContainsKey (currentTime)) {
			if (waves[currentTime].Count>0) {

				foreach (var w in waves[currentTime]) {
					if (w.type == "-1" && !w.used) {
						isWaitingForWave = true;
						toUse = new List<WaveElement> ();
						w.used = true;
						return;
					}
				}

				
				foreach (var w in waves[currentTime]) {
					if (!w.used) {
						toUse.Add (w);

					}
				}

			}

		}


		currentTime += deltaTime;
		currentTime = Mathf.Round (currentTime * 100f) / 100f;

		foreach (var w in toUse) {
			if (!w.used) {

				if (w.timeForNext > 0) {
					w.timeForNext -= deltaTime;

				} else {

					Unit unit = null;
					
                    unit = new Unit (w.type, color, w.road);
					
					GameController.units.Add (unit);

					w.count --;
					w.timeForNext = w.deltaTime;
					if (w.count == 0) 
						w.used = true;
				}
			}
		}
	}
}
