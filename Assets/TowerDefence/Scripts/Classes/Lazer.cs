using UnityEngine;
using System.Collections;

public class Lazer {

    private GUIObject body;

    private readonly Vector2 start;
    private readonly Killable end;
    private readonly float length;
    private readonly float height;
    private readonly float speed;

    private Actions.VoidVoid onShot;

    private Vector2 _edgePosition;
    private Vector2 edgePosition {

        get {

            return _edgePosition;
        }

        set {

            _edgePosition = value;

            float width = length;

            if (Vector2.Distance (start, edgePosition) < length) {

                width = Vector2.Distance (start, edgePosition);
            }

            body.sizeInMeters = new Vector2 (width, height);
            
            var bodyDirection = end.mapPosition - edgePosition;

            if (bodyDirection.magnitude > 0) {

                body.rotation = (bodyDirection.x > 0 ? 1 : -1) * (90 + Vector2.Angle (bodyDirection, new Vector2 (0, -1)));
            }

            body.positionInMeters = edgePosition + width / 2f * bodyDirection.normalized;
            body.layer = end.layer - GUIController.layer;
        }
    }

    public void Update (float deltaTime) {

        var bodyDirection = end.mapPosition - edgePosition;

        if (bodyDirection.magnitude <= speed * deltaTime + 0.2f) {

            if (onShot != null) {

                onShot ();
            }

            Destroy ();

            return;
        }

        edgePosition += bodyDirection.normalized * speed * deltaTime;
    }

    public Lazer (Vector2 _start, Killable _end, Color color, Actions.VoidVoid _onShot, float _length = 1f, float _height = 0.3f, float _speed = 8f) {
        
        start = _start;
        end = _end;
        height = _height;
        length = _length;
        speed = _speed;
        onShot = _onShot;

        body = new GUIImageAlpha ("Sprites/MiddleSeg", end.layer - GUIController.layer, start, new Vector2 (0, 0), false);
        body.color = color;

        edgePosition = start;

        LazerController.Add (this);
    }

    public void Destroy () {

        body.Destroy ();
        LazerController.Remove (this);
    }
}
