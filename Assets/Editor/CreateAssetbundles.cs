using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using System.Security.Cryptography;

public class CreateAssetbundles
{
    public static readonly string[] pathSeparator = new string[] { string.Format("{0}",Path.AltDirectorySeparatorChar), string.Format("{0}",Path.DirectorySeparatorChar)};

    public static BuildTarget GetCurrentTarget()
    {
        // Add more target platform specifications here

#if  UNITY_ANDROID
        return BuildTarget.Android;
#elif UNITY_IPHONE
        return BuildTarget.iPhone;
#else
        return BuildTarget.WebPlayer;
#endif
    }

    // This method creates an assetbundle of each SkinnedMeshRenderer
    // found in any selected character fbx, and adds any materials that
    // are intended to be used by the specific SkinnedMeshRenderer.
    [MenuItem("Character Generator/Create All/Scenes Bundles")]
    public static void Execute()
    {
		
		//string[] mapList ={"Hub_Village","Hub_Village_Tutorial","EmptyScenes","A1_M1","A1_M2","A1_M3","A2_M1","A2_M2","A2_M3","A3_M1","A3_M2","A3_M3","A4_M1","A4_M2","A4_M3","A5_M1","A5_M2","A5_M3"};
		
		//string[] mapList ={"Hub_Village"};
		
		//List<string> RealMapList = new List<string>();
		
		//string[] strGetMaps = Directory.GetFiles( "Assets/Scenes","*.unity",SearchOption.AllDirectories);

        string[] strMapDirs = Directory.GetDirectories("Assets/Scenes/Areas", "CoreScenes", SearchOption.AllDirectories);
        //Debug.Log(Path.AltDirectorySeparatorChar);
        //Debug.Log(Path.DirectorySeparatorChar);

        foreach (string it in strMapDirs)
        {
            string[] strGetMaps = Directory.GetFiles(it, "*.unity", SearchOption.TopDirectoryOnly);
            foreach( string it2 in strGetMaps)
            {
                string mapName = it2.Replace(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar);
                //RealMapList.Add(it2);

                bool bSuccess = EditorApplication.OpenScene(mapName);
                if (bSuccess)
                    CreateCurrentSceneBuddle.BuildSceneBundle(mapName, new GameObject[0]);
                else
                    Debug.LogError("Failed to open map: " + it2);
            }
        }
		
        //foreach(string it in strGetMaps)
        //{
        //    string []templist = it.Split('\\');
			
        //    foreach(string it2 in mapList)
        //    {
        //        int index = templist.Length - 1;
				
        //        if(index < 0)
        //            index = 0;
				
        //        string temp = templist[index].Replace(".unity", "");
				
        //        temp = temp.ToLower();
				
        //        if(it2.ToLower() == temp)
        //        {
        //            RealMapList.Add(it);
        //            break;
        //        }
        //    }	
        //}
		
        //foreach(string MapPath in RealMapList)
        //{
        //    bool bSuccess = EditorApplication.OpenScene(MapPath);
			
        //    string NewMapPath =  MapPath.Replace("\\", "/");
			
        //    CreateCurrentSceneBuddle.BuildSceneBundle(NewMapPath);
        //}
	
    }

    static Object GetPrefab(GameObject go, string name)
    {
        Object tempPrefab = EditorUtility.CreateEmptyPrefab("Assets/" + name + ".prefab");
        tempPrefab = EditorUtility.ReplacePrefab(go, tempPrefab);
        Object.DestroyImmediate(go);
        return tempPrefab;
    }

    public static string AssetbundlePath
    {
        get { 
			#if  UNITY_ANDROID
                #if __TEST_EXT_BUNDLES__
                    return "assetbundles/";
                #else
                    return "Assets/StreamingAssets/";
                #endif
            #else
			return "assetbundles" + Path.DirectorySeparatorChar;  
            #endif
		
			
		   }
    }
	
	public static void BuildBunddleList(string mapName)
	{
		//string mapName = "Aka_TestBox_1";
		
		string bundleName2 = mapName + ".unity3d";
		
		string MapPath = CreateAssetbundles.AssetbundlePath +  mapName + ".unity3d";
		
		List<string> assetList = new List<string>();
		
		assetList.Add(mapName);
		
		BundleManager BDMGR = (BundleManager)Resources.Load("BundleList");
		
		if( BDMGR == null)
		{
			BDMGR = ScriptableObject.CreateInstance<BundleManager>();
		    
		}
		if( BDMGR != null)
		{
			BDMGR.Init();
		    BDMGR.Add(assetList, bundleName2, CreateAssetbundles.GetMD5HashFromUncompressFile(), CreateAssetbundles.GetFileSize(MapPath));
		}
		
	    string stringBundleMgr = "Assets/Resources/BundleList.asset";
		
		if(File.Exists(stringBundleMgr))
		{
		   //AssetDatabase.DeleteAsset(stringBundleMgr);
			
			EditorUtility.SetDirty(BDMGR);
		}
		else
		{
		   AssetDatabase.CreateAsset(BDMGR, stringBundleMgr);
		}

	}
	
	public static string GetMD5HashFromUncompressFile()
	{
		FileStream file = new FileStream("Temp/uncompressedData", FileMode.Open);
		MD5 md5 = new MD5CryptoServiceProvider();
		byte[] retVal = md5.ComputeHash(file);
		file.Close();
		
		return System.BitConverter.ToString(retVal);
	}
	
    public static int GetFileSize(string path)
	{
		FileStream file = new FileStream(path, FileMode.Open);
		int size = (int)file.Length;
		file.Close();
		
		return size;
	}
}