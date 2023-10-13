using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using System.Security.Cryptography;
using System.Collections;

public class UISound {
	public static List<Transform> list = new List<Transform>();
	[MenuItem("Tools/Sound/UISound")]
	static void Execute() {
		#region load asset
		string assetPath = "Assets/Audio/SFX/UI";
		string[] strMapDirs = Directory.GetDirectories(assetPath, "*", SearchOption.AllDirectories);
		for(int i = 0;i<strMapDirs.Length;i++) {
			string[] strGetMaps = Directory.GetFiles(strMapDirs[i], "*.prefab", SearchOption.TopDirectoryOnly);
            foreach( string it2 in strGetMaps) {
				list.Add((Transform)AssetDatabase.LoadAssetAtPath(it2,typeof(Transform)));
			}
		}	
		#endregion
		#region print list element name
//		for(int j = 0;j <list.Count;j++) {
//			Debug.LogError(list[j].name);
//		}
		#endregion
		#region check sound asset <UIButton>
		UIButton[] obj = Object.FindObjectsOfType(typeof(UIButton)) as UIButton[];
		Debug.LogWarning("UIButton count: " + obj.Length);
		for ( int i = 0; i < obj.Length; ++i){
			if(obj[i].soundOnClick!=null) {
				Transform tran = GetTransform(obj[i].soundOnClick.name);
				if(tran != null) {
					obj[i].SoundOnClick = tran;
				}
			}
			if(obj[i].soundOnOver!=null) {
				Transform tran = GetTransform(obj[i].soundOnOver.name);
				if(tran != null) {
					obj[i].SoundOnOver = tran;
				}
			}
			
			UnityEditor.EditorUtility.SetDirty(obj[i]);
		}
		#endregion
		#region check sound asset <UIPanelTab>
		UIPanelTab[] obj2 = Object.FindObjectsOfType(typeof(UIPanelTab)) as UIPanelTab[];
		Debug.LogWarning("UIPanelTab count: " + obj2.Length);
		for ( int i = 0; i < obj2.Length; ++i){
			if(obj2[i].soundToPlay!=null) {
				Transform tran = GetTransform(obj2[i].soundToPlay.name);
				if(tran != null) {
					obj2[i].SoundToPlay = tran;
				}
			}
			UnityEditor.EditorUtility.SetDirty(obj[i]);
		}
		#endregion 
		#region check sound asset <UIRadioBtn>
		UIRadioBtn[] obj3 = Object.FindObjectsOfType(typeof(UIRadioBtn)) as UIRadioBtn[];
		Debug.LogWarning("UIRadioBtn count: " + obj3.Length);
		for ( int i = 0; i < obj3.Length; ++i){
			if(obj3[i].soundToPlay!=null) {
				Transform tran = GetTransform(obj3[i].soundToPlay.name);
				if(tran != null) {
					obj3[i].SoundToPlay = tran;
				}
			}
			UnityEditor.EditorUtility.SetDirty(obj[i]);
		}
		#endregion
		Debug.LogWarning("UI Sound Setting OK.");
	}
	
	static Transform GetTransform(string name) {
		foreach(Transform tran in list) {
			if(string.Compare(tran.name,name)==0) {
				return tran;
			}
		}
		return null;
	}
}
