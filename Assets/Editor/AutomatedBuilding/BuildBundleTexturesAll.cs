using UnityEngine;
using UnityEditor;

public class BuildBundleTexturesAll : MonoBehaviour {

	[MenuItem("Build/Bundle/Textures/All")]
    public static void Execute()
    {
		Debug.Log("Bundling textures bundles");
		
		BuildBundleTexturesCartoon.Execute();	// Cartoon/comics textures
		BuildBundleTexturesMaterial.Execute();	// 
		BuildBundleTexturesMiniMap.Execute();	//
		BuildBundleTexturesNPC.Execute();		//
		BuildBundleTexturesOtherBg.Execute();	//
		BuildBundleTexturesWorldMap.Execute();	//

		Debug.Log("Bundling textures bundles done");
	}
}
