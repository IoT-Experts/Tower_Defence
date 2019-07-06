using UnityEngine;
using System.Collections.Generic;

class LightningBolt : MonoBehaviour {
		public List<GameObject> ActiveLineObj;
	public List<GameObject> InactiveLineObj;

	
    public float Alpha { get; set; }

    public float FadeOutRate { get; set; }

    public Color Tint { get; set; }

    public Vector2 Start { get { return ActiveLineObj[0].GetComponent<Line> ().A; } }

    public Vector2 End { get { return ActiveLineObj[ActiveLineObj.Count-1].GetComponent<Line> ().B; } }

    public bool IsComplete { get { return Alpha <= 0; } }

	public void Initialize(int maxSegments) {

				ActiveLineObj = new List<GameObject> ();
		InactiveLineObj = new List<GameObject> ();

		for(int i = 0; i < maxSegments; i++) {

						GameObject line = (GameObject)GameObject.Instantiate(LightningController.linePrefab);

						line.transform.parent = transform;

						line.SetActive(false);

						InactiveLineObj.Add(line);
		}
	}

	public void ActivateBolt(Vector2 source, Vector2 dest, Color color, float thickness,float layer) {

				Tint = color;
		
				Alpha = 1.5f;
		
				FadeOutRate = 0.12f;

						if(Vector2.Distance(dest, source) <= 0) {

			Vector2 adjust = Random.insideUnitCircle;
			if(adjust.magnitude <= 0) adjust.x += .1f;
			dest += adjust;
		}
		
				Vector2 slope = dest - source;
		Vector2 normal = (new Vector2(slope.y, -slope.x)).normalized;
		
				float distance = slope.magnitude;
		
		List<float> positions = new List<float> ();
		positions.Add(0);
		
		for (int i = 0; i < 4; i++) {

									positions.Add (Random.Range(0.12f, 0.91f));
		}
		
		positions.Sort ();
		
		const float Sway = 80;
		const float Jaggedness = 1 / Sway;
		
				float spread = 0.2f;
		
				Vector2 prevPoint = source;
		
				float prevDisplacement = 0;
		
		for (int i = 1; i < positions.Count; i++) {

						int inactiveCount = InactiveLineObj.Count;
			if(inactiveCount <= 0) break;
			
			float pos = positions[i];
			
						float scale = (distance * Jaggedness) * (pos - positions[i - 1]);
			
						float envelope = pos > 0.95f ? 20 * (1 - pos) : spread;
			
			float displacement = Random.Range(-Sway, Sway);
			displacement -= (displacement - prevDisplacement) * (1 - scale);
			displacement *= envelope;
			
						Vector2 point = source + (pos * slope) + (displacement * normal);
			
			ActivateLine(prevPoint, point, thickness,layer);
			prevPoint = point;
			prevDisplacement = displacement;
		}
		
		ActivateLine(prevPoint, dest, thickness,layer);
	}


	private Vector2 Perpendicular(Vector2 target) {

		return new Vector2 (target.y,-target.x);

	}

	public void DeactivateSegments () {

		for(int i = ActiveLineObj.Count - 1; i >= 0; i--) {

			GameObject line = ActiveLineObj[i];
			line.SetActive(false);
			ActiveLineObj.RemoveAt(i);
			InactiveLineObj.Add(line);
		}
	}

	void ActivateLine(Vector2 A, Vector2 B, float thickness,float layer) {

				int inactiveCount = InactiveLineObj.Count;

				if(inactiveCount <= 0) return;

				GameObject line = InactiveLineObj[inactiveCount - 1];

				line.SetActive(true);

				Line lineComponent = line.GetComponent<Line> ();
		lineComponent.SetColor(Color.white);
		lineComponent.A = A;
		lineComponent.B = B;
		lineComponent.Thickness = thickness;
		lineComponent.layer = layer;
		InactiveLineObj.RemoveAt(inactiveCount - 1);
		ActiveLineObj.Add(line);
	}
	
	public void Draw () {

				if (Alpha <= 0) return;

		foreach (GameObject obj in ActiveLineObj) {

			Line lineComponent = obj.GetComponent<Line> ();
			lineComponent.SetColor(Tint * (Alpha * 0.6f));
			lineComponent.Draw ();
		}
	}
	
	public void UpdateBolt () {

		Alpha -= FadeOutRate;
	}


}