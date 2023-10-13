using UnityEngine;
using UnityEditor;

public class BuildBundleAll {
    
	[MenuItem("Build/Bundle/All")]
    public static void Execute()
    {
		try {
			Debug.Log("Bundling assets");
			
			// Described in documentation
			BuildBundleAkaneiro.Execute();			// Akaneiro
			BuildBundleItems.Execute();				// Items
			BuildBundleMonsters.Execute();			// Mosters
			CreateTemplateBundles.Execute(); 		// GUI
			BuildBundleScenes.Execute();			// Scenes assets

			// Not described in documentation, but somehow used
			BuildBundleTexturesAll.Execute();
			
			Debug.Log("Bundling assets done");
		} catch (System.Exception ex) {
			Debug.LogException(ex);
			Debug.LogError("Bundling assets failed.");
			throw ex;
		}
	}
}
