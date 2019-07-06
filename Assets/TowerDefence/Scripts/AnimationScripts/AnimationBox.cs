using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class AnimationBox {
	
	private static List <TextureList> animations_ = new List<TextureList> ();
	private static List <string> names = new List<string> ()
	#region Animation names
		{ "AssassinWalk"
		, "StandartBearerWalk"
		, "Bang"
		, "BangNoFire"
		, "GuardsmanWalk"
		, "TowerBuildAnimation"
		, "ArumaTowerAttack"
		, "LifeStealerTowerAttack"
		, "MageTowerAttack"
		, "TeslaTowerAttack"
		, "ThorTowerAttack"
		, "TyphoonTowerAttack"
		, "VolcanoTowerAttack"
		, "Bomb"
        , "TornadoTowerAttack"
        , "ThunderTowerAttack"
		, "AsuraWalk"
		, "Asura2Walk"
		, "DeathMasterWalk"
		, "DuergarWalk"
		, "Duergar2Walk"
		, "ElephantWalk"
		, "NotusWalk"
		};
	#endregion

	private static bool isLoaded = false;

    public static bool Contains (string name) {

        return names.Contains (name);
    }

	public static void Load (string name) {
		
		TextureList target = new TextureList (name);
		Object [] ResourcesList = Resources.LoadAll ("Animations/"+name, typeof (Texture));

        if (ResourcesList.Length == 0) {

            Debug.LogError ("No animation: '" + name + "'"); 
        }

		target.textures = new List<Texture> ();
		for (int i = 0; i < ResourcesList.Length; i++){
			target.textures.Add (ResourcesList[i] as Texture);
		}
		
		animations_.Add (target);
	}

	
	public static TextureList GetAnimation (string name){
		
		foreach (TextureList result in animations_){
			if (result.name == name) return result;
		}
		return null;
	}
	
	public static void LoadAnimations () {

		if (isLoaded)
			return;

		foreach (string animation in names){
			Load (animation);
		}
		isLoaded = true;
	}
}
