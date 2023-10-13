using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using System.Security.Cryptography;
using System.Collections;

public class CreateCurrentSceneBuddle
{
    public static void BuildSceneBundle(string SceneRelativePath, GameObject[] _bebundledObjs)
    {
        ControlFolder(true);

        BuildBundleMonsters.MonsterBundlePrepare();
		
		string _currenSceneString = SceneRelativePath;
        string[] _tempPathstrings = SceneRelativePath.Split(CreateAssetbundles.pathSeparator, System.StringSplitOptions.RemoveEmptyEntries);
		
        string _mapName = _tempPathstrings[_tempPathstrings.Length - 1].Replace(".unity", "");
		
		XMLFileWriter _fileWriter = new XMLFileWriter();
        string _filePath = CreateCurrentSceneBuddle.AssetbundlePath + "XML" + Path.DirectorySeparatorChar + "SCENE" + Path.DirectorySeparatorChar;
        if (!Directory.Exists(_filePath))
            Directory.CreateDirectory(_filePath);
        _filePath += _mapName + ".xml";
        _fileWriter.BindWithFile(_filePath);

        _fileWriter.NodeBegin(_mapName);
		
        RecordBreakablesAndMonster(SceneRelativePath,_fileWriter);

        string _mapBundlePath = AssetbundlePath + "Scenes" + Path.DirectorySeparatorChar;
		if (!Directory.Exists(_mapBundlePath))
            Directory.CreateDirectory(_mapBundlePath);
		_mapBundlePath += _mapName + ".unity3d";
		
        // copy all data to a new scene file.
        string _copymapName = _mapName + "_copy.unity";
		
        _tempPathstrings[_tempPathstrings.Length - 1] = _copymapName;
        string _newSceneString = string.Join("/", _tempPathstrings);
		
        bool bSuccess = EditorApplication.SaveScene(_newSceneString, true);
		
        // create bundles for mesh / sfx / vfx
        FindAndBuild(_bebundledObjs, _mapName, _fileWriter);
		
        // delete _GP
		GameObject _go = GameObject.Find("_GP");
		if(_go) UnityEngine.Object.DestroyImmediate(_go);

        // save these changes
        bSuccess = EditorApplication.SaveScene(SceneRelativePath);

        // create bundle for scene
        string[] _levels = { SceneRelativePath };
        string error = "";
        ControlFolder(false);
        BuildBundleMonsters.MonsterBundleEnd();

#if  UNITY_ANDROID
		error = BuildPipeline.BuildStreamedSceneAssetBundle(_levels, _mapBundlePath, BuildTarget.Android);
#else
        error = BuildPipeline.BuildStreamedSceneAssetBundle(_levels, _mapBundlePath, BuildTarget.WebPlayer);
#endif

        // restore all data
        bSuccess = EditorApplication.OpenScene(_newSceneString);
        bSuccess = EditorApplication.SaveScene(SceneRelativePath, true);
        bSuccess = EditorApplication.OpenScene(SceneRelativePath);
		ControlFolder(true);

        bSuccess = AssetDatabase.DeleteAsset(_newSceneString);
		
		_fileWriter.NodeEnd(_mapName);
        _fileWriter.Flush();
        _fileWriter.ShutDown();
    }
	
	static void FindAndBuild(GameObject[] _objs, string _mapName, XMLFileWriter _fileWriter)
	{
		_fileWriter.NodeBegin("Scene_Objs");

        foreach (GameObject _go in _objs)
		{
			if(_go)
			{
				Debug.Log("Bundling" + _go);
				_go.name = _mapName + "_" + _go.name;
				
				_fileWriter.NodeBegin("Obj_Name");
                _fileWriter.AddAttribute("Content", _go.name);
                _fileWriter.AddAttribute("Pos_X", _go.transform.position.x);
                _fileWriter.AddAttribute("Pos_Y", _go.transform.position.y);
                _fileWriter.AddAttribute("Pos_Z", _go.transform.position.z);
		        _fileWriter.NodeEnd("Obj_Name");

                _go.AddComponent<Bundle_Scene_Flag>();
                BuildBundleMonsters.BuildTargetObject(_go);

				Object.DestroyImmediate(_go);
			}
		}
		
		_fileWriter.NodeEnd("Scene_Objs");
	}
	
	
    public static string AssetbundlePath
    {
        get
        {
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

    public static void RecordBreakablesAndMonster(string SceneRelativePath, XMLFileWriter _fileWriter)
    {
        string[] path = SceneRelativePath.Split(CreateAssetbundles.pathSeparator, System.StringSplitOptions.RemoveEmptyEntries);
        string SceneName = path[path.Length - 1].Replace(".unity", "");

        #region Breakable Objects
        List<string> _tempBreakableObjs = new List<string>();
        foreach (InteractiveObj _obj in FindAllBreakableObjPrefabInThisScene())
        {
            _tempBreakableObjs.Add(_obj.transform.name);
        }
        #endregion

        #region Monsters
        List<string> _tempMonsters = new List<string>();
        foreach (NpcBase _obj in FindAllMonsterPrefabInThisScene())
        {
            _tempBreakableObjs.Add(_obj.transform.name);
        }
        #endregion

        #region Write into the file
        _fileWriter.NodeBegin("Breakable_Objs");
        foreach (string _bObjs in _tempBreakableObjs)
        {
			string _temp = _bObjs.Replace(" ",  "_space_");
            _fileWriter.NodeBegin("Obj_Name");
            _fileWriter.AddAttribute("Content", _temp);
            _fileWriter.NodeEnd("Obj_Name");
        }
        _fileWriter.NodeEnd("Breakable_Objs");

        _fileWriter.NodeBegin("Monster_Objs");
        foreach (string _mObjs in _tempMonsters)
        {
			string _temp = _mObjs.Replace(" ",  "_space_");
            _fileWriter.NodeBegin("Obj_Name");
            _fileWriter.AddAttribute("Content", _temp);
            _fileWriter.NodeEnd("Obj_Name");
        }
        _fileWriter.NodeEnd("Monster_Objs");
        #endregion
    }
    
    public static GameObject[] FindAllSceneObjs()
    {
        List<GameObject> _tempObjs = new List<GameObject>();

        GameObject[] _allObjs = GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[];
        foreach (GameObject _obj in _allObjs)
        {
            if (_obj.transform.parent == null)
            {
                if (!_obj.name.ToLower().Contains("_gp") &&
                    !_obj.name.ToLower().Contains("playerctrl") &&
                    !_obj.name.ToLower().Contains("terrain") &&
                    !_obj.name.ToLower().Contains("player") &&
                    !_obj.name.ToLower().Contains("collision") &&
                    !_obj.name.ToLower().Contains("lights") &&
                    !_obj.name.ToLower().Contains("occlusion area"))
                _tempObjs.Add(_obj);
            }
        }
        return _tempObjs.ToArray();
    }
    
    public static NpcBase[] FindAllMonsterPrefabInThisScene()
    {
        List<int> _monsterTypes = FindAllMonsterIDsInThisScene();
        string[] _existingMonster = Directory.GetFiles("Assets/Prefabs/CH/Enemies", "*.prefab", SearchOption.AllDirectories);

        List<NpcBase> _tempMonsters = new List<NpcBase>();

        foreach (int it in _monsterTypes)
        {
            foreach (string _monsterFile in _existingMonster)
            {
                Transform MonsterTransform = AssetDatabase.LoadAssetAtPath(_monsterFile, typeof(Transform)) as Transform;
                NpcBase theMonster = null;
                theMonster = MonsterTransform.GetComponent<NpcBase>();

                if (theMonster != null && theMonster.TypeID == it)
                {
                    _tempMonsters.Add(theMonster);
                }
            }
        }

        return _tempMonsters.ToArray();
    }

    public static List<int> FindAllMonsterIDsInThisScene()
    {
        NpcSpawner[] MonsterSpawnList = Object.FindObjectsOfType(typeof(NpcSpawner)) as NpcSpawner[];

        List<int> MonsterTypeList = new List<int>();

        bool bRepeat = false;

        foreach (NpcSpawner it in MonsterSpawnList)
        {
            foreach (NpcSpawner.Spawner s in it.SpawnerList)
            {
                if (s.NpcPrefabArray != null)
                {
                    foreach (Transform perfab in s.NpcPrefabArray)
                    {
                        if (perfab != null && perfab.GetComponent<NpcBase>() != null)
                        {
                            bRepeat = false;

                            foreach (int type in MonsterTypeList)
                            {
                                if (type == perfab.GetComponent<NpcBase>().TypeID)
                                {
                                    bRepeat = true;
                                    break;
                                }
                            }

                            if (!bRepeat)
                                MonsterTypeList.Add(perfab.GetComponent<NpcBase>().TypeID);

                            if (perfab.GetComponent<NpcBase>().bSummonerNpc)
                            {
                                if (perfab.GetComponent<NpcBase>().SummonSpawner != null && perfab.GetComponent<NpcBase>().SummonSpawner.NpcPrefabArray != null)
                                {
                                    foreach (Transform p2 in perfab.GetComponent<NpcBase>().SummonSpawner.NpcPrefabArray)
                                    {
                                        bool bRepeat2 = false;

                                        foreach (int type2 in MonsterTypeList)
                                        {
                                            if (type2 == p2.GetComponent<NpcBase>().TypeID)
                                            {
                                                bRepeat2 = true;
                                                break;
                                            }
                                        }

                                        if (!bRepeat2)
                                            MonsterTypeList.Add(p2.GetComponent<NpcBase>().TypeID);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        return MonsterTypeList;

    }

    public static InteractiveObj[] FindAllBreakableObjPrefabInThisScene()
    {
        List<int> _breakableTypes = FindAllBreakableObjIDsInThisScene();
        string[] _existingBreakables = Directory.GetFiles("Assets/Prefabs/GAM", "*.prefab", SearchOption.AllDirectories);

        List<InteractiveObj> _tempBreakableObjs = new List<InteractiveObj>();

        foreach (int bktypeID in _breakableTypes)
        {
            foreach (string breakit in _existingBreakables)
            {
                InteractiveHandler tempBreakable = AssetDatabase.LoadAssetAtPath(breakit, typeof(InteractiveHandler)) as InteractiveHandler;

                if (tempBreakable == null)
                    continue;

                if (tempBreakable.TypeID == bktypeID)
                {
                    //CreateAllMonsterbundles.BuildTargetObject(tempBreakable.gameObject);
                    _tempBreakableObjs.Add(tempBreakable);
                }
            }
        }

        return _tempBreakableObjs.ToArray();
    }

    public static List<int> FindAllBreakableObjIDsInThisScene()
    {
        InteractiveHandler[] InteractiveObjectList = Object.FindObjectsOfType(typeof(InteractiveHandler)) as InteractiveHandler[];

        List<int> BreakableTypeList = new List<int>();

        foreach (InteractiveHandler it in InteractiveObjectList)
        {
            bool bRepeat = false;

            foreach (int type in BreakableTypeList)
            {
                if (type == it.TypeID)
                {
                    bRepeat = true;
                    break;
                }
            }

            if (!bRepeat)
                BreakableTypeList.Add(it.TypeID);
        }

        return BreakableTypeList;
    }

    public static List<int> SaveShopNpcRelation()
    {
        ShopNpc[] ShopNpcList = Object.FindObjectsOfType(typeof(ShopNpc)) as ShopNpc[];

        List<int> ShopTypeList = new List<int>();

        foreach (ShopNpc it in ShopNpcList)
        {
            bool bRepeat = false;

            foreach (int type in ShopTypeList)
            {
                if (type == (int)it.npcType)
                {
                    bRepeat = true;
                    break;
                }
            }

            if (!bRepeat)
                ShopTypeList.Add((int)it.npcType);
        }

        return ShopTypeList;

    }

    public static void ControlFolder(bool flag)
    {
        GameObject gp = GameObject.Find("_GP");

        if (gp != null)
        {
            if (flag)
            {
                gp.SetActive(true);

                CaveDiffState[] CaveInst = Object.FindObjectsOfType(typeof(CaveDiffState)) as CaveDiffState[];

                AdjustPlayerLight[] PlayerLightInst = Object.FindObjectsOfType(typeof(AdjustPlayerLight)) as AdjustPlayerLight[];

                TriggerBase[] TriggerList = Object.FindObjectsOfType(typeof(TriggerBase)) as TriggerBase[];

                if (CaveInst != null)
                {
                    foreach (CaveDiffState iter in CaveInst)
                    {
                        iter.pTrigggerIDs.Clear();

                        foreach (TriggerBase iter2 in iter.pTriggers)
                        {
                            if (iter2 != null)
                                iter.pTrigggerIDs.Add(iter2.id);
                        }

                        foreach (Trigger_Unified iter3 in iter.pNewTriggers)
                        {
                            if (iter3 != null)
                                iter.pTrigggerIDs.Add(iter3.UnityID);
                        }

                    }
                }

                if (PlayerLightInst != null)
                {
                    foreach (AdjustPlayerLight itr in PlayerLightInst)
                    {
                        itr.pTrigggerIDs.Clear();

                        foreach (TriggerBase itr2 in itr.pTriggers)
                        {
                            if (itr2 != null)
                                itr.pTrigggerIDs.Add(itr2.id);
                        }

                        foreach (Trigger_Unified iter3 in itr.pNewTriggers)
                        {
                            if (iter3 != null)
                                itr.pTrigggerIDs.Add(iter3.UnityID);
                        }
                    }
                }


            }
            else
            {
                Object.DestroyImmediate(gp);
            }
        }

        gp = GameObject.Find("PlayerCtrl");

        if (gp != null)
        {
            gp.SetActiveRecursively(flag);
            gp.active = true;
        }
    }

    // This method creates an assetbundle of each SkinnedMeshRenderer
    // found in any selected character fbx, and adds any materials that
    // are intended to be used by the specific SkinnedMeshRenderer.
    [MenuItem("Character Generator/Create CurrentScene Assetbundles for OSX")]
    static void ExecuteOSX()
    {
        ControlFolder(false);

        string scenepath = EditorApplication.currentScene;

        string[] leves = { scenepath };

        string[] path = EditorApplication.currentScene.Split(CreateAssetbundles.pathSeparator, System.StringSplitOptions.RemoveEmptyEntries);

        string MapPath = AssetbundlePath + path[path.Length - 1].Replace(".unity", "") + ".unity3d";

        string error = BuildPipeline.BuildPlayer(leves, MapPath, BuildTarget.StandaloneOSXIntel, BuildOptions.BuildAdditionalStreamedScenes);

        Debug.Log(error);
    }

}
