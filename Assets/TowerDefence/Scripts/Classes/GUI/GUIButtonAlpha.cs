using UnityEngine;
using System.Collections;

public class GUIButtonAlpha : GUIButton {
	
	public GUIButtonAlpha () : base (false) {
		
		gameObject = GamePullController.CreateButtonAlpha ();
		animation = new ObjectAnimation (gameObject);
		
		isClickable = true;

		GUIController.Add (gameObject, this);
	}
	

    
	public GUIButtonAlpha (string _texture, float _layer, Vector2 _positionInMeters, Vector2 _sizeInMeters, bool _isStatic = true
        , Actions.VoidGUIButton _OnClick = null, Actions.VoidGUIButton _OnButtonDown = null
        , Actions.VoidGUIButton _OnButtonUp = null, Actions.VoidGUIButton _OnButtonDownOver = null) : this () {

        layer = _layer;
        sizeInMeters = _sizeInMeters;
        isStatic = _isStatic;
		textureName = _texture;
        positionInMeters = _positionInMeters;

        OnClick = _OnClick;
        OnButtonDown = _OnButtonDown;
        OnButtonUp = _OnButtonUp;
        OnButtonDownOver = _OnButtonDownOver;
        
		isClickable = true;
    }
	
	public GUIButtonAlpha (string _texture, float? leftPos, float? topPos, float? rightPos, float? botPos, float widthInPixels, float heightInPixels, float _layer = 0, bool _useScale = false) : base (false) {
		
		gameObject = GamePullController.CreateButtonAlpha ();
		animation = new ObjectAnimation (gameObject);
		
		layer = _layer;
		useScale = _useScale;
		
		textureName = _texture;
		sizeInPixels = new Vector2 (widthInPixels, heightInPixels);
		
		if (leftPos!=null)
			left = leftPos;
		if (topPos!=null)
			top = topPos;
		
		if (rightPos!=null)
			right = rightPos;
		
		if (botPos!=null)
			bottom = botPos;
		
		isClickable = true;
		
		GUIController.Add (gameObject, this);
		
	}
	
	
	public override void Destroy () {
		
		GamePullController.DestroyButtonAlpha (gameObject);
		animation.Destroy ();
		GUIController.Remove (gameObject, this);
	}
	
    public static GUIButtonAlpha operator + (GUIButtonAlpha a, string b) {

        a.textureName += b;
        return a;
    }

    public static GUIButtonAlpha operator - (GUIButtonAlpha a, string b) {
        

        if (a.textureName.Contains (b)) {
                
            a.textureName = a.textureName.Remove (a.textureName.IndexOf (b), b.Length);
        }

        return a;
    }
	
}
