using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections.Generic;

public class BuildBundleScenes
{
	[MenuItem("Build/Bundle/Scenes")]
	public static void Execute()
	{
		Debug.Log("Bundling scene's assets");
		ProcessScene("Assets/Scenes/Areas/Hub_Village_Tutorial.unity");
		ProcessScene("Assets/Scenes/Areas/Hub_Village.unity");
		string[] scenes_dirs = Directory.GetDirectories("Assets/Scenes/Areas", "CoreScenes", SearchOption.AllDirectories);
		foreach (string scenes_dir in scenes_dirs) {
			ProcessDirectory(scenes_dir);
		}
		Debug.Log("Bundling scene's assets done");
	}
	
	public static void ProcessDirectory(string scenes_dir)
	{
		Debug.Log("Processing dir: " + scenes_dir);
		string[] scenes_files = Directory.GetFiles(scenes_dir, "*.unity", SearchOption.TopDirectoryOnly);
		foreach (string scene_file in scenes_files) {
			try {
				ProcessScene(scene_file);
			} catch (System.Exception ex) {
				Debug.LogException(ex);
				Debug.LogError("ProcessScene:" + scene_file + " Failed");
			}
		}		
	}
	
	public static void ProcessScene(string scene_file)
	{
		Debug.Log("Processing scene: " + scene_file);
		string mapName = scene_file.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);	
		if (EditorApplication.OpenScene(mapName)) {
			List<GameObject> game_objects = new List<GameObject>();
			foreach (NpcBase game_object in CreateCurrentSceneBuddle.FindAllMonsterPrefabInThisScene()) {
				game_objects.Add(game_object.gameObject);
			}
			foreach (InteractiveObj game_object in CreateCurrentSceneBuddle.FindAllBreakableObjPrefabInThisScene()) {
				game_objects.Add(game_object.gameObject);
			}
			BuildBundleMonsters.BuildBundles(game_objects.ToArray());
			CreateCurrentSceneBuddle.BuildSceneBundle(mapName, CreateCurrentSceneBuddle.FindAllSceneObjs());
		} else {
			Debug.LogError("Failed to open map: " + scene_file);
		}
	}
}
