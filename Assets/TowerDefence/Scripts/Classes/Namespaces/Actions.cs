using UnityEngine;
using System.Collections;

namespace Actions {
    
    public delegate void VoidVoid ();
    public delegate void VoidFloat (float f);
    public delegate void VoidInt (int i);
    public delegate void VoidString (string s);
    public delegate void VoidIntInt (int a, int b);
    public delegate float FloatFloat (float f);
    public delegate void VoidVector2 (Vector2 v);
    public delegate void VoidGameObject (GameObject go);
    public delegate GameObject GameObjectVoid ();
    public delegate void VoidAction (VoidVoid a);
    public delegate bool BoolVoid ();
    public delegate void VoidGUIButton (GUIButton t);

}
