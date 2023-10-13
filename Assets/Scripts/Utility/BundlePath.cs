//using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;

public class BundlePath {
	
	public static string UIbundleBaseURL
	{
		get
        {
            if (Application.platform == RuntimePlatform.WindowsWebPlayer || Application.platform == RuntimePlatform.OSXWebPlayer)
                return Application.dataPath + "/assetbundles/UI/";
            else if (Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.OSXDashboardPlayer)
                return "file://" + Application.dataPath + "/../assetbundles/UI/";
            else if (Application.platform == RuntimePlatform.Android)
#if __TEST_EXT_BUNDLES__
                return "file://" + Application.persistentDataPath + "/assetbundles/UI/";
#else
                return "jar:file://" + Application.dataPath + "!/assets/UI/";
#endif
            else
                return "file://" + Application.dataPath + "/../assetbundles/UI/";

        }
	}
	
	
    // Returns correct assetbundle base url, whether in the editor, standalone or
    // webplayer, on Mac or Windows.
    public static string AssetbundleBaseURL
    {
		get
        {
            if (Application.platform == RuntimePlatform.WindowsWebPlayer || Application.platform == RuntimePlatform.OSXWebPlayer)
                return Application.dataPath + "/assetbundles/";
            else if (Application.platform == RuntimePlatform.OSXPlayer || Application.platform == RuntimePlatform.OSXDashboardPlayer)
                return "file://" + Application.dataPath + "/../assetbundles/";
            else if (Application.platform == RuntimePlatform.Android)
#if __TEST_EXT_BUNDLES__
                return "file://" + Application.persistentDataPath + "/assetbundles/UI/";
#else
                return "jar:file://" + Application.dataPath + "!/assets/";
#endif
            else
                return "file://" + Application.dataPath + "/../assetbundles/";

        }
    }
}
