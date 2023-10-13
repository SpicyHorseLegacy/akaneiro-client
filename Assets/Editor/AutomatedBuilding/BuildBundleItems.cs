using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using System.Security.Cryptography;
using System.Collections;


public class BuildBundleItems
{
	public static Dictionary<string, GameObject> mapCachedItems = new Dictionary<string, GameObject>();
	
	static string iconNameBoy  = "";
	static string iconNameGirl = "";
	
    // This method creates an assetbundle of each SkinnedMeshRenderer
    // found in any selected character fbx, and adds any materials that
    // are intended to be used by the specific SkinnedMeshRenderer.
    [MenuItem("Build/Bundle/Items")]
    public static void Execute()
    {
		Debug.Log("Start item assetbunle buiding");
        mapCachedItems.Clear();

        string strItemInfo = "Assets/Prefabs/Loot/ItemInfo.prefab";

        string ActualItemDirectory = "Assets/Prefabs/Loot/ItemDir/";

        string[] strGetItems = Directory.GetFiles("Assets/Prefabs/Loot", "*.prefab", SearchOption.AllDirectories);

        string[] strAnotherGetItems = Directory.GetFiles("Assets/Prefabs/GAM", "*.prefab", SearchOption.AllDirectories);

        ItemPrefabs TotalItemInfos = AssetDatabase.LoadAssetAtPath(strItemInfo, typeof(ItemPrefabs)) as ItemPrefabs;

        List<_UI_CS_ItemPrefabs> NewUICSItemPerfabs = new List<_UI_CS_ItemPrefabs>();

        int jk = 0;

        if (TotalItemInfos != null)
        {
            if (TotalItemInfos.itemPrefabs != null)
            {
                foreach (_UI_CS_ItemPrefabs CSitem in TotalItemInfos.itemPrefabs)
                {
                    if (CSitem != null)
                    {
                        // if(jk >= 1)
                        // break;

                        //jk += 1;

                        if (!Directory.Exists(ActualItemDirectory))
                            Directory.CreateDirectory(ActualItemDirectory);

                        int lp = 0;

                        for (int i = 0; i < CSitem.item.Length; i++)
                        {
                            if (CSitem.item[i] == null)
                                continue;

                            //if(lp >= 1)
                            //break;
                            // lp += 1;

                            string strNewItem = CSitem.item[i].name;

                            GameObject oldItem = GetOldPrefab(CSitem.item[i].gameObject, strNewItem, strGetItems);

                            if (oldItem == null)
                                oldItem = GetOldPrefab(CSitem.item[i].gameObject, strNewItem, strAnotherGetItems);

                            GameObject NewItem = null;

                            if (oldItem != null)
                                NewItem = GetObjPrefab(oldItem, strNewItem, ActualItemDirectory, strGetItems);

                            if (NewItem != null && NewItem.GetComponent<Item>())
                            {
                                ProcessItem(NewItem, oldItem);

                                CSitem.item[i] = NewItem.GetComponent<Item>();
                                //EditorUtility.SetDirty(CSitem.item[i].gameObject);
                            }
                        }
                        EditorUtility.SetDirty(CSitem.gameObject);
                    }
                }
            }
        }
    }


    public static void CreateSingleItemBundle(GameObject _targetItem)
    {
        string ActualItemDirectory = "Assets/Prefabs/Loot/ItemDir/";
        string[] strGetItems = Directory.GetFiles("Assets/Prefabs/Loot", "*.prefab", SearchOption.AllDirectories);
        string[] strAnotherGetItems = Directory.GetFiles("Assets/Prefabs/GAM", "*.prefab", SearchOption.AllDirectories);

        if (!Directory.Exists(ActualItemDirectory))
            Directory.CreateDirectory(ActualItemDirectory);

        string strNewItem = _targetItem.name;

        GameObject oldItem = GetOldPrefab(_targetItem, strNewItem, strGetItems);

        if (oldItem == null)
            oldItem = GetOldPrefab(_targetItem, strNewItem, strAnotherGetItems);

        GameObject NewItem = null;

        if (oldItem != null)
            NewItem = GetObjPrefab(oldItem, strNewItem, ActualItemDirectory, strGetItems);

        if (NewItem != null && NewItem.GetComponent<Item>())
        {
            ProcessItem(NewItem, oldItem);
        }
    }

    static void ProcessItem(GameObject NewGameObject, GameObject OldGameObject)
    {
        if (NewGameObject == null || OldGameObject == null)
            return;

        if (NewGameObject.GetComponent<ItemDownLoading>() == null)
            NewGameObject.AddComponent<ItemDownLoading>();

        if (OldGameObject.GetComponent<MeshFilter>())
        {
            if (OldGameObject.GetComponent<MeshFilter>().sharedMesh != null)
            {
                string MeshName = OldGameObject.GetComponent<MeshFilter>().sharedMesh.name + "_mesh";

                bool bPass = BuildAddiveBunddles(OldGameObject.GetComponent<MeshFilter>().sharedMesh, null, MeshName);

                if (NewGameObject.GetComponent<ItemDownLoading>() && bPass)
                    NewGameObject.GetComponent<ItemDownLoading>().MeshBundle = MeshName;
            }
        }

        if (NewGameObject.GetComponent<MeshFilter>())
            NewGameObject.GetComponent<MeshFilter>().mesh = null;

        if (OldGameObject.renderer && OldGameObject.renderer.sharedMaterial)
        {
            string mtlName = OldGameObject.renderer.sharedMaterial.name + "_mtl";

            bool bPass = BuildAddiveBunddles(OldGameObject.renderer.sharedMaterial, null, mtlName);

            if (NewGameObject.GetComponent<ItemDownLoading>() && bPass)
                NewGameObject.GetComponent<ItemDownLoading>().MaterialBundle = mtlName;
        }


        if (NewGameObject.renderer && NewGameObject.renderer.sharedMaterials != null)
        {
            for (int k = 0; k < NewGameObject.renderer.sharedMaterials.Length; k++)
                NewGameObject.renderer.sharedMaterials[k] = null;
            NewGameObject.renderer.material = null;
            NewGameObject.renderer.materials = NewGameObject.renderer.sharedMaterials;
        }

        if (OldGameObject.GetComponent<Item>())
        {
            string IconName = "";

            if (OldGameObject.GetComponent<Item>().Normal_State_IconBoy != null)
            {
                iconNameBoy = (string)OldGameObject.GetComponent<Item>().Normal_State_IconBoy.name + "_icon";
                Debug.LogWarning("IconName: " + iconNameBoy);
                bool bPass = BuildAddiveBunddles(OldGameObject.GetComponent<Item>().Normal_State_IconBoy, null, iconNameBoy);
                #region Test Code
                if (bPass == false)
                {
                    Debug.LogError("BulidBunddles Err,IconName: " + iconNameBoy);
                }
                #endregion
                if (NewGameObject.GetComponent<ItemDownLoading>() && bPass)
                {
                    Debug.LogWarning("1");
                    NewGameObject.GetComponent<ItemDownLoading>().BoyIconBundle = iconNameBoy;
                    EditorUtility.SetDirty(NewGameObject);
                }
                Debug.LogWarning("boyIcon " + NewGameObject.GetComponent<ItemDownLoading>().BoyIconBundle);
            }


            if (OldGameObject.GetComponent<Item>().Normal_State_IconGirl != null)
            {

                iconNameGirl = (string)OldGameObject.GetComponent<Item>().Normal_State_IconGirl.name + "_icon";
                Debug.LogWarning("IconName: " + iconNameGirl);
                bool bPass = BuildAddiveBunddles(OldGameObject.GetComponent<Item>().Normal_State_IconGirl, null, iconNameGirl);
                #region Test Code
                if (bPass == false)
                {
                    Debug.LogError("BulidBunddles Err,IconName: " + iconNameGirl);
                }
                #endregion
                if (NewGameObject.GetComponent<ItemDownLoading>() && bPass)
                {
                    Debug.LogWarning("2");
                    NewGameObject.GetComponent<ItemDownLoading>().GirlIconBundle = iconNameGirl;
                    EditorUtility.SetDirty(NewGameObject);
                }
                Debug.LogWarning("girlIcon " + NewGameObject.GetComponent<ItemDownLoading>().GirlIconBundle);
            }
        }
        if (OldGameObject.GetComponent<WeaponBase>())
        {
            if (OldGameObject.GetComponent<WeaponBase>().AttackSound != null)
            {
                string SoundName = OldGameObject.GetComponent<WeaponBase>().AttackSound.name;

                bool bPass = BuildAddiveBunddles(OldGameObject.GetComponent<WeaponBase>().AttackSound, null, SoundName);
                if (NewGameObject.GetComponent<ItemDownLoading>() && bPass)
                {
                    NewGameObject.GetComponent<ItemDownLoading>().SoundBundle = SoundName;
                    EditorUtility.SetDirty(NewGameObject);
                }
            }
            if (OldGameObject.GetComponent<WeaponBase>().sheath != null)
            {
                string sheathName = OldGameObject.GetComponent<WeaponBase>().sheath.name;

                bool bPass = BuildAddiveBunddles(OldGameObject.GetComponent<WeaponBase>().sheath, null, sheathName);
                if (NewGameObject.GetComponent<ItemDownLoading>() && bPass)
                    NewGameObject.GetComponent<ItemDownLoading>().SheathBundle = sheathName;
            }
        }
        if (NewGameObject.GetComponent<Item>())
        {
            NewGameObject.GetComponent<Item>().Normal_State_IconBoy = null;

            NewGameObject.GetComponent<Item>().Normal_State_IconGirl = null;
        }
        if (NewGameObject.GetComponent<WeaponBase>())
        {
            NewGameObject.GetComponent<WeaponBase>().AttackSound = null;

            NewGameObject.GetComponent<WeaponBase>().sheath = null;
        }

        //			NewGameObject.GetComponent<ItemDownLoading>().BoyIconBundle  = iconNameBoy;
        //			NewGameObject.GetComponent<ItemDownLoading>().GirlIconBundle = iconNameGirl;

        EditorUtility.SetDirty(NewGameObject);
    }
	
    // load prefab by name
	static GameObject GetOldPrefab(GameObject go, string name,string[] ItemSummarys)
	{
		GameObject ReplaceGamj = null;
		
	    string[] keyStrList = name.Split('_');
		
		string NewPerfabName = name;
		
		string tempko = name;

        if (keyStrList != null && keyStrList.Length > 0)
        {
            // assemble item name
            if (keyStrList[keyStrList.Length - 1] == "info")
            {
                tempko = "";

                for (int i = 0; i < keyStrList.Length - 1; i++)
                {
                    tempko += keyStrList[i];
                    if (i < keyStrList.Length - 2)
                        tempko += "_";
                }
                tempko += ".prefab";
            }
            else
            {
                tempko += ".prefab";
            }

            foreach (string str in ItemSummarys)
            {
                string[] templist = str.Split(CreateAssetbundles.pathSeparator, System.StringSplitOptions.RemoveEmptyEntries);

                if (templist.Length > 0)
                {
                    if (templist[templist.Length - 1] == tempko)
                    {
                        ReplaceGamj = (GameObject)AssetDatabase.LoadMainAssetAtPath(str);
                        break;
                    }
                }
            }
        }

		return ReplaceGamj;
	}
	
    /// <summary>
    /// check if there is an info prefab in FilePath, if not, create one.
    /// </summary>
    /// <param name="go"></param>
    /// <param name="name"></param>
    /// <param name="FilePath">New prefab should be located here.</param>
    /// <param name="ItemSummarys"></param>
    /// <returns></returns>
	static GameObject GetObjPrefab(GameObject go, string name,string FilePath,string[] ItemSummarys)
    {
		
		string[] keyStrList = name.Split('_');
		
		string NewPerfabName = name;
	
		if(keyStrList != null && keyStrList.Length > 0)
		{
			if(keyStrList[keyStrList.Length - 1] != "info")
			{
				NewPerfabName += "_info";	
			}
		}
			
		string filename = FilePath + NewPerfabName + ".prefab";
		
		GameObject NewGameObj = null;
		
		Object tempPrefab = null;
		
		foreach(string key in mapCachedItems.Keys)
		{
			Debug.Log(key);
		}
		if(mapCachedItems.ContainsKey(NewPerfabName))   //File.Exists(filename))
		{
		   //AssetDatabase.LoadMainAssetAtPath(filename);
				
		   NewGameObj = mapCachedItems[NewPerfabName];
		   
		   return NewGameObj;
		
		}
		else
		{
		   if(File.Exists(filename))
			  tempPrefab = AssetDatabase.LoadMainAssetAtPath(filename);
		   else
	          tempPrefab = PrefabUtility.CreateEmptyPrefab(filename);
			
		   if(go != null)
               NewGameObj = PrefabUtility.ReplacePrefab(go, tempPrefab, ReplacePrefabOptions.ReplaceNameBased);
			
		   if(NewGameObj != null)
		      mapCachedItems.Add(NewPerfabName,NewGameObj);	
			
		}
        return NewGameObj;
    }
	
    static bool BuildAddiveBunddles(Object MainObject,Object[] FuObjects,string Bundlename)
	{
		string BundlePath = CreateCurrentSceneBuddle.AssetbundlePath + Bundlename + ".assetbundle";
		
		bool bPass = false;
		
        bPass = BuildPipeline.BuildAssetBundle(MainObject, FuObjects, BundlePath, BuildAssetBundleOptions.CollectDependencies, CreateAssetbundles.GetCurrentTarget());
		
		return bPass;
	}
	
}
