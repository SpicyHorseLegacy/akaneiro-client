using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

public class BundlesType {
	public static BuildTarget GetCurrentTarget() {
#if  UNITY_ANDROID
		return BuildTarget.Android;
#elif UNITY_IPHONE
		return BuildTarget.iPhone;
#else
		return BuildTarget.WebPlayer;
#endif
    }
	
	public static string AssetbundlePath {
        get { 
#if  UNITY_ANDROID
			return "asset/StreamingAssets/";
#else
			return "assetbundles" + Path.DirectorySeparatorChar;  
#endif
		   }
    }
	
	public static void IsExistAssetsBundlesFile(string filePath) {
		if (!Directory.Exists(filePath)) {
			Debug.LogWarning("filePath:"+filePath);
            Directory.CreateDirectory(filePath);
        }
	}
	
	public static string AssetbundleBaseURLForUI {
        get {
            if(Application.platform == RuntimePlatform.WindowsWebPlayer || Application.platform == RuntimePlatform.OSXWebPlayer)
                return Application.dataPath+"/assetbundles/UI/";
            else if (Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.OSXDashboardPlayer)
				return "file://" + Application.dataPath + "/../assetbundles/UI/";
			else if( Application.platform == RuntimePlatform.Android)
				return "jar:file://" + Application.dataPath + "!/assets/UI/";
			else
                return Application.dataPath + "/../assetbundles/UI/"; 
        }
    }
}
