using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LightningController {
		public static GameObject boltPrefab;
	public static GameObject linePrefab;

		private static List<GameObject> activeBoltsObj;
	private static List<GameObject> inactiveBoltsObj;
	public static int maxBolts = 100;



	private class ToAttack {
		
		public float timeLeft = 0;
		public Vector2 startAttack;
		public Killable endAttack;
		public float coolDown;
		public float maxTime;
		public float layer;
        public float size;

		public ToAttack (Vector2 _s, Killable _e, float _layer, float _t, float _size) {

			startAttack = _s;
			endAttack = _e;
			timeLeft = _t;
			coolDown = 0;
			maxTime = _t;
            layer = _layer;
            size = _size;
		}


	}

	private static List <ToAttack> attacks = new List<ToAttack> ();

	private static GameObject poolHolder;

	public static void CreateBolt () {
		
				GameObject bolt = (GameObject)GameObject.Instantiate(boltPrefab);
		
						
				bolt.GetComponent<LightningBolt> ().Initialize(25);
		
				bolt.SetActive(false);
		
				inactiveBoltsObj.Add(bolt);
	}

	public LightningController () {

		boltPrefab = ResourcesController.Load("Prefabs/Bolt") as GameObject;
		linePrefab = ResourcesController.Load("Prefabs/Line") as GameObject;

		activeBoltsObj = new List<GameObject> ();
		inactiveBoltsObj = new List<GameObject> ();
		attacks = new List<ToAttack> ();
	}

	public static void Update(float deltaTime) {

		for(int i =  attacks.Count - 1; i>=0; i--) {

			if (attacks[i].timeLeft>0) { 

				if (attacks[i].coolDown <=0) {
					attacks[i].coolDown = attacks[i].maxTime/16f;
					CreatePooledBolt(attacks[i].startAttack, attacks[i].endAttack.mapPosition
					                 , Random.value>0.5f? Color.cyan:Color.magenta , attacks [i].size, attacks[i].layer);
				
				} else
					attacks[i].coolDown -= deltaTime;

				attacks[i].timeLeft -= deltaTime;
			} else {

				attacks.RemoveAt(i);
			}
		}
				GameObject boltObj;
		LightningBolt boltComponent;
		
				int activeLineCount = activeBoltsObj.Count;
		
			for (int i = activeLineCount - 1; i >= 0; i--) {

					boltObj = activeBoltsObj[i];
			
					boltComponent = boltObj.GetComponent<LightningBolt> ();
			
					if(boltComponent.IsComplete) {

							boltComponent.DeactivateSegments ();
				
							boltObj.SetActive(false);
				
							activeBoltsObj.RemoveAt(i);
			    inactiveBoltsObj.Add(boltObj);
			}
		}

		
				for(int i = 0; i < activeBoltsObj.Count; i++) {

			activeBoltsObj[i].GetComponent<LightningBolt> ().UpdateBolt ();
			activeBoltsObj[i].GetComponent<LightningBolt> ().Draw ();
		}


	}

	
	private static void CreatePooledBolt(Vector2 source, Vector2 dest, Color color, float thickness, float layer) {

				if(inactiveBoltsObj.Count > 0) {

						GameObject boltObj = inactiveBoltsObj[inactiveBoltsObj.Count - 1];

						boltObj.SetActive(true);

						activeBoltsObj.Add(boltObj);
			inactiveBoltsObj.RemoveAt(inactiveBoltsObj.Count - 1);

						LightningBolt boltComponent =  boltObj.GetComponent<LightningBolt> ();

						boltComponent.ActivateBolt(source, dest, color, thickness,layer);
		}
	}

	
	public static void Attack(Vector2 start, Killable end, float layer, float time, float size = 2f) {
		
		attacks.Add(new ToAttack (start, end, layer, time, size));

	}

}