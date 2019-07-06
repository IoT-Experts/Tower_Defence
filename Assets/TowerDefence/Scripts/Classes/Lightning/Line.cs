using UnityEngine;
using System.Collections;

public class Line : MonoBehaviour {

	public Vector2 A;
    public Vector2 B;
    public float Thickness;

    private float _layer = 0;
    public float layer {
        
        get { return GUIController.layer + _layer; }
        set { _layer = value; }
    }

	public GameObject StartCapChild, LineChild, EndCapChild;

	public Line(Vector2 a, Vector2 b, float thickness, float _layer) {

		A = a;
		B = b;
		Thickness = thickness;
		layer = _layer;
	}

	public void SetColor(Color color) {

		StartCapChild.GetComponent<SpriteRenderer> ().color = color;
		LineChild.GetComponent<SpriteRenderer> ().color = color;
		EndCapChild.GetComponent<SpriteRenderer> ().color = color;
	}

	public void Draw () {

		Vector2 difference = B - A;
		float rotation = Mathf.Atan2(difference.y, difference.x) * Mathf.Rad2Deg;

		LineChild.transform.localScale = new Vector3(100 * (difference.magnitude / LineChild.GetComponent<SpriteRenderer> ().sprite.rect.width)
		                                        , Thickness
		                                        , LineChild.transform.localScale.z);

		StartCapChild.transform.localScale = new Vector3(StartCapChild.transform.localScale.x
		                                                 , Thickness
		                                                 , StartCapChild.transform.localScale.z);


		EndCapChild.transform.localScale = new Vector3(EndCapChild.transform.localScale.x
		                                               , Thickness
		                                               , EndCapChild.transform.localScale.z);
        

		LineChild.transform.rotation = Quaternion.Euler(new Vector3(90,0, rotation));
		StartCapChild.transform.rotation = Quaternion.Euler(new Vector3(90,0, rotation));
		EndCapChild.transform.rotation = Quaternion.Euler(new Vector3(90,0, rotation + 180));


		LineChild.transform.position = new Vector3 (A.x, layer,A.y );
		StartCapChild.transform.position = new Vector3 (A.x,  layer,A.y);
		EndCapChild.transform.position = new Vector3 (A.x,  layer,A.y);

		rotation *= Mathf.Deg2Rad;

		float lineChildWorldAdjust = LineChild.transform.localScale.x * LineChild.GetComponent<SpriteRenderer> ().sprite.rect.width / 2f;
		float startCapChildWorldAdjust = StartCapChild.transform.localScale.x * StartCapChild.GetComponent<SpriteRenderer> ().sprite.rect.width / 2f;
		float endCapChildWorldAdjust = EndCapChild.transform.localScale.x * EndCapChild.GetComponent<SpriteRenderer> ().sprite.rect.width / 2f;

		LineChild.transform.position += new Vector3 (.01f * Mathf.Cos(rotation) * lineChildWorldAdjust
		                                                , 0,.01f * Mathf.Sin(rotation) * lineChildWorldAdjust);

		StartCapChild.transform.position -= new Vector3 (.01f * Mathf.Cos(rotation) * startCapChildWorldAdjust
		                                                    , 0,.01f * Mathf.Sin(rotation) * startCapChildWorldAdjust);

		EndCapChild.transform.position += new Vector3 (.01f * Mathf.Cos(rotation) * lineChildWorldAdjust * 2
		                                                , 0,.01f * Mathf.Sin(rotation) * lineChildWorldAdjust * 2);
		EndCapChild.transform.position += new Vector3 (.01f * Mathf.Cos(rotation) * endCapChildWorldAdjust
		                                                , 0,.01f * Mathf.Sin(rotation) * endCapChildWorldAdjust);
	}
}