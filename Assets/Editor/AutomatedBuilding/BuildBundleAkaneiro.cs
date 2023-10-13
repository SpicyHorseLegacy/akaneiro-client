using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using System.Security.Cryptography;

class BuildBundleAkaneiro
{

    // This method creates an assetbundle of each SkinnedMeshRenderer
    // found in any selected character fbx, and adds any materials that
    // are intended to be used by the specific SkinnedMeshRenderer.
    [MenuItem("Build/Bundle/Akaneiro")]
    public static void Execute()
    {
		//Caching.CleanCache();
		string[] aka_char_folders = Directory.GetDirectories("Assets/Model/Character/Avatar", "*", SearchOption.TopDirectoryOnly);
        BuildBundlesFromFoldersAndOptions(aka_char_folders, true, true);
    }

    public static void BuildBundlesFromFolders(string[] folders)
    {
        BuildBundlesFromFoldersAndOptions(folders, false, false);
    }

    public static void BuildBundlesFromFoldersAndOptions(string[] folders, bool isCollectCE, bool isBuildAni)
    {
        List<Object> SelObjects = CollectObjectsFromFolders(folders);
        buildFBXBundles(SelObjects);

        if (isCollectCE)
            collectCharacterElement();

        if (isBuildAni)
            BuildAnimationBundle();
    }

    static List<Object> CollectObjectsFromFolders(string[] folders)
    {
        List<Object> SelObjects = new List<Object>();
        Object theObj = null;
		
        foreach (string _folder in folders)
        {
			foreach (string eachFBXfile in Directory.GetFiles(_folder, "*.fbx"))
            {
                theObj = AssetDatabase.LoadMainAssetAtPath(eachFBXfile);
                if (theObj != null)
                    SelObjects.Add(theObj);
            }
			
			foreach (string eachFBXfile in Directory.GetFiles(_folder, "*.FBX"))
            {
                theObj = AssetDatabase.LoadMainAssetAtPath(eachFBXfile);
                if (theObj != null)
                    SelObjects.Add(theObj);
            }
        }

        return SelObjects;
    }

    static void buildFBXBundles(List<Object> SelObjects)
    {
        foreach (Object o in SelObjects)
        {
            if (!(o is GameObject)) continue;
            //if (o.name.Contains("@")) continue;
            // if (!AssetDatabase.GetAssetPath(o).Contains("/characters/")) continue;

            GameObject characterFBX = (GameObject)o;
            string name = characterFBX.name.ToLower();

            Debug.Log("******* Creating assetbundles for: " + name + " *******");

            // Create a directory to store the generated assetbundles.
            if (!Directory.Exists(AssetbundlePath))
                Directory.CreateDirectory(AssetbundlePath);


            // Delete existing assetbundles for current character.
            string[] existingAssetbundles = Directory.GetFiles(AssetbundlePath);
            foreach (string bundle in existingAssetbundles)
            {
                if (bundle.EndsWith(".assetbundle") && bundle.Contains("/assetbundles/" + name))
                    File.Delete(bundle);
            }

            // Save bones and animations to a seperate assetbundle. Any 
            // possible combination of CharacterElements will use these
            // assets as a base. As we can not edit assets we instantiate
            // the fbx and remove what we dont need. As only assets can be
            // added to assetbundles we save the result as a prefab and delete
            // it as soon as the assetbundle is created.
            GameObject characterClone = (GameObject)Object.Instantiate(characterFBX);

            // postprocess animations: we need them animating even offscreen
            //foreach (Animation anim in characterClone.GetComponentsInChildren<Animation>())
            //    anim.animateOnlyIfVisible = false;

            foreach (SkinnedMeshRenderer smr in characterClone.GetComponentsInChildren<SkinnedMeshRenderer>())
            {
                //GameObject ccv = smr.gameObject;
                Object.DestroyImmediate(smr.gameObject);
            }

            characterClone.AddComponent<SkinnedMeshRenderer>();
            Object characterBasePrefab = GetPrefab(characterClone, "characterbase");
            string path = AssetbundlePath + name + "_characterbase.assetbundle";

            BuildPipeline.BuildAssetBundle(characterBasePrefab, null, path, BuildAssetBundleOptions.CollectDependencies, CreateAssetbundles.GetCurrentTarget());

            AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(characterBasePrefab));

            // Collect materials.
            //Debug.LogError(GenerateMaterials.MaterialsPath(characterFBX));
            List<Material> materials = EditorHelpers.CollectAll<Material>(GenerateMaterials.MaterialsPath(characterFBX));

            // Create assetbundles for each SkinnedMeshRenderer.
            foreach (SkinnedMeshRenderer smr in characterFBX.GetComponentsInChildren<SkinnedMeshRenderer>(true))
            {
                List<Object> toinclude = new List<Object>();

                // Save the current SkinnedMeshRenderer as a prefab so it can be included
                // in the assetbundle. As instantiating part of an fbx results in the
                // entire fbx being instantiated, we have to dispose of the entire instance
                // after we detach the SkinnedMeshRenderer in question.
                GameObject rendererClone = (GameObject)EditorUtility.InstantiatePrefab(smr.gameObject);
                GameObject rendererParent = rendererClone.transform.parent.gameObject;
                rendererClone.transform.parent = null;
                Object.DestroyImmediate(rendererParent);
                Object rendererPrefab = GetPrefab(rendererClone, "rendererobject");
                toinclude.Add(rendererPrefab);

                //string sonName = smr.name;
                // Collect applicable materials.
                foreach (Material m in materials)
                {
                    if (m.name.ToLower().Contains(smr.name.ToLower())) toinclude.Add(m);
                }

                // When assembling a character, we load SkinnedMeshRenderers from assetbundles,
                // and as such they have lost the references to their bones. To be able to
                // remap the SkinnedMeshRenderers to use the bones from the characterbase assetbundles,
                // we save the names of the bones used.
                List<string> boneNames = new List<string>();
                foreach (Transform t in smr.bones)
                    boneNames.Add(t.name);
                string stringholderpath = "Assets/bonenames.asset";

                StringHolder holder = ScriptableObject.CreateInstance<StringHolder>();
                holder.content = boneNames.ToArray();
                AssetDatabase.CreateAsset(holder, stringholderpath);
                toinclude.Add(AssetDatabase.LoadAssetAtPath(stringholderpath, typeof(StringHolder)));

                // Save the assetbundle.
                //string bundleName = name + "_" + smr.name.ToLower();
                string bundleName = smr.name.ToLower();

                path = AssetbundlePath + bundleName + ".assetbundle";

                BuildPipeline.BuildAssetBundle(null, toinclude.ToArray(), path, BuildAssetBundleOptions.CollectDependencies, CreateAssetbundles.GetCurrentTarget());

                Debug.Log("Saved " + bundleName + " with " + (toinclude.Count - 2) + " materials");

                // Delete temp assets.
                AssetDatabase.DeleteAsset(AssetDatabase.GetAssetPath(rendererPrefab));
                AssetDatabase.DeleteAsset(stringholderpath);
            }
        }
    }

    static void collectCharacterElement()
    {
        // collect character elements infomation.
        //if (createdBundle)
        {
            UpdateCharacterElementDatabase.Execute();
        }
       // else
        {
        //    EditorUtility.DisplayDialog("Character Generator", "No Asset Bundles created. Select the characters folder in the Project pane to process all characters. Select subfolders to process specific characters.", "Ok");
        }
    }

    [MenuItem("Build/Bundle/Akaneiro Animation")]
    static void BuildAnimationBundle()
    {
        // create animation bundle
        List<Object> TotalAnimations = new List<Object>();
        string[] existingAnimations = Directory.GetFiles("Assets/Model/Character/CH_Aka/ANI", "*.anim", SearchOption.AllDirectories);
        foreach (string AnimationFile in existingAnimations)
        {
            Object theObj = AssetDatabase.LoadAssetAtPath(AnimationFile, typeof(AnimationClip));
            TotalAnimations.Add(theObj);
        }

        if (TotalAnimations.Count > 0)
        {
            string AnimationPath = AssetbundlePath + "Animations.assetbundle";
            BuildPipeline.BuildAssetBundle(null, TotalAnimations.ToArray(), AnimationPath, BuildAssetBundleOptions.CollectDependencies, CreateAssetbundles.GetCurrentTarget());
        }
    }

    static Object GetPrefab(GameObject go, string name)
    {
        Object tempPrefab = PrefabUtility.CreateEmptyPrefab("Assets/" + name + ".prefab");
        tempPrefab = PrefabUtility.ReplacePrefab(go, tempPrefab);
        Object.DestroyImmediate(go);
        return tempPrefab;
    }

    public static string AssetbundlePath
    {
        get { 
		    #if  UNITY_ANDROID
			return "Assets/StreamingAssets/";
            #else
			return "assetbundles" + Path.DirectorySeparatorChar;  
            //return BundlePath.AssetbundleBaseURL;
            #endif
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


    public static void BuildBunddleList()
    {
        string mapName = "Aka_TestBox_1";

        string bundleName2 = mapName + ".unity3d";

        string MapPath = CreateAssetbundles.AssetbundlePath + mapName + ".unity3d";

        List<string> assetList = new List<string>();

        assetList.Add(mapName);

        BundleManager BDMGR = (BundleManager)Resources.Load("BundleList");

        if (BDMGR == null)
            BDMGR = ScriptableObject.CreateInstance<BundleManager>();

        BDMGR.Init();

        BDMGR.Add(assetList, bundleName2, CreateAssetbundles.GetMD5HashFromUncompressFile(), CreateAssetbundles.GetFileSize(MapPath));


        string stringBundleMgr = "Assets/Resources/BundleList.asset";

        if (File.Exists(stringBundleMgr))
        {
            //AssetDatabase.DeleteAsset(stringBundleMgr);

            EditorUtility.SetDirty(BDMGR);
        }
        else
        {
            AssetDatabase.CreateAsset(BDMGR, stringBundleMgr);
        }

    }
	
}