using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.IO;

public class CreateTemplateBundles {
	
	[MenuItem("GUI/CreateTemplateBundles")]
    public static void Execute() {
		List<Object> ObjList = new List<Object>();
		string[] strObj = Directory.GetFiles( "Assets/GUI/Template","*",SearchOption.AllDirectories);
		foreach( string texFile in strObj) {
			Object NewObj = AssetDatabase.LoadAssetAtPath(texFile,typeof(Object)) as Object;
			if( NewObj != null)
				ObjList.Add(NewObj);
		}
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
}
