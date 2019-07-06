using UnityEngine;
using System.Collections;

public class Bubble : GUIImageAlpha, IUpdatable {

	private readonly Vector2 startPosition;
    private readonly float deltaY;
    private readonly float dispersion;
    private readonly float maxTime;

    private float time;

    public Bubble (Vector2 _startPosition, float _deltaY, float _layer, float _dispersion, float _maxTime) : base (true) {

		texture = Resources.Load ("Interface/StartMenu/Bubble") as Texture;
        layer = _layer;
        startPosition = _startPosition;
        sizeInMeters = new Vector2 (-1, -1);
        positionInMeters = startPosition;
        dispersion = _dispersion;
        deltaY = _deltaY;
        maxTime = _maxTime;
        time = 0;
        MainMenuController.instance.updatables.Add (this);
    }

    public void Update (float deltaTime) {

        time += deltaTime;
        positionInMeters = startPosition + new Vector2 (dispersion * Mathf.Sin (time), deltaY * time / maxTime);

        //if (time >= maxTime / 2f)
            //gameObject.GetComponent <Renderer> ().material.color -= new Color (0, 0, 0, 0.01f);//2 * deltaTime / maxTime);

        if (time >= maxTime) {
            MainMenuController.instance.updatables.Remove (this);
            base.Destroy ();
        }
    }   
}
