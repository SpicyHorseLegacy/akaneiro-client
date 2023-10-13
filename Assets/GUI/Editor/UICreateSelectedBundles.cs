using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.IO;

class UICreateSelectedBundles : EditorWindow {
	Vector2 mScroll = Vector2.zero;
	void OnGUI(){
		bool create = false;
		EditorGUIUtility.LookLikeControls(80f);
		GUILayout.Label("Select one or more files.");
		
	    GUI.backgroundColor = Color.green;
		create = GUILayout.Button("Create", GUILayout.Width(76f));
		
		DrawSeparator();
		GUI.backgroundColor = Color.gray;
		List<Object> objects = GetSelectedObject();
		int index = 0;
		mScroll = GUILayout.BeginScrollView(mScroll);
		foreach(Object tt in objects){
			//Debug.Log("ToString = "+tt.ToString());
			
			++index;
			GUILayout.BeginHorizontal();
			GUILayout.Label(index.ToString(), GUILayout.Width(24f));
			GUILayout.Label(tt.name);
			GUILayout.EndHorizontal();
		}
		GUILayout.EndScrollView();
		if(create){
			CreateBundles(objects);
			Close();
		}
		
	}
	
	void OnSelectionChange(){
		Repaint();
	}
	
	List<Object> GetSelectedObject ()
	{
		List<Object> objectlist = new List<Object>();

		if (Selection.objects != null && Selection.objects.Length > 0)
		{
			Object[] objects = Selection.objects;
//			Object[] objects = EditorUtility.CollectDependencies(Selection.objects);
			//Object[] objects = EditorUtility.CollectDeepHierarchy(Selection.objects);
			foreach (Object o in objects)
			{
				Object tex = o as Object;
				if (tex != null && (NGUISettings.atlas == null || NGUISettings.atlas.texture != tex)) objectlist.Add(tex);
			}
		}
		return objectlist;
	}
	
	
	void CreateBundles(List<Object> ObjList){
		foreach(Object eachObj in ObjList) {
		    string bundlePath = BundlesType.AssetbundleBaseURLForUI + eachObj.name + ".assetbundle";
			BundlesType.IsExistAssetsBundlesFile(BundlesType.AssetbundleBaseURLForUI);
			bool bPass = false;
            bPass = BuildPipeline.BuildAssetBundle(eachObj, null, bundlePath, BuildAssetBundleOptions.CollectDependencies, BundlesType.GetCurrentTarget());
			if(!bPass) {
				GUILogManager.LogErr("Bulid Template fail,template name: "+eachObj.name);
			}
		}
	}
	
	public void DrawSeparator ()
	{
		GUILayout.Space(12f);

		if (Event.current.type == EventType.Repaint)
		{
			Texture2D tex = EditorGUIUtility.whiteTexture;
			Rect rect = GUILayoutUtility.GetLastRect();
			GUI.color = new Color(0f, 0f, 0f, 0.25f);
			GUI.DrawTexture(new Rect(0f, rect.yMin + 6f, Screen.width, 4f), tex);
			GUI.DrawTexture(new Rect(0f, rect.yMin + 6f, Screen.width, 1f), tex);
			GUI.DrawTexture(new Rect(0f, rect.yMin + 9f, Screen.width, 1f), tex);
			GUI.color = Color.white;
		}
	}
}