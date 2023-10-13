using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;
using System.Reflection;
using System.Collections;

public class LoadBundleTool : MonoBehaviour {

    //public static LoadBundleTool _Instance;

    public bool IsDebug = false;

    public Transform _Parent;

    public string[] Paths;

    class CachedDownloadData
    {
        public UnityEngine.Object DownLoadedObj = null;
        public WWW DownLoadedWeb = null;
		public bool IsAssemblyDone = false;

        public CachedDownloadData()
        {
            DownLoadedObj = null;
            DownLoadedWeb = null;
        }

        public CachedDownloadData(WWW _tempWWW)
        {
            DownLoadedObj = null;
            DownLoadedWeb = _tempWWW;
        }

        public void ClearAll()
        {
            if (DownLoadedWeb != null)
            {
                if (DownLoadedWeb.assetBundle)
                    DownLoadedWeb.assetBundle.Unload(true);
                DownLoadedWeb.Dispose();
            }
            if (DownLoadedObj != null)
                DestroyImmediate(DownLoadedObj, true);
        }
    }
    class CachedXMLData
    {
        public string Content = null;
        public WWW DownLoadedWeb = null;

        public CachedXMLData()
        {
            Content = "";
            DownLoadedWeb = null;
        }

        public CachedXMLData(WWW _tempWWW)
        {
            Content = _tempWWW.text;
            DownLoadedWeb = _tempWWW;
        }

        public void ClearAll()
        {
            if (DownLoadedWeb != null)
            {
                DownLoadedWeb.Dispose();
            }
        }
    }

    List<string> allbundlesNeedToBeDownload = new List<string>();
    List<string> nextCheckList = new List<string>();

    Dictionary<string, CachedXMLData> CachedXML = new Dictionary<string, CachedXMLData>();
    Dictionary<string, CachedDownloadData> CachedObjs = new Dictionary<string, CachedDownloadData>();

    public List<GameObject> SceneThingsPrefabs = new List<GameObject>();
    public List<NpcBase> MonsterPrefabs = new List<NpcBase>();
    public List<InteractiveObj> InteractiveObjPrefabs = new List<InteractiveObj>();
	
	public List<GameObject> NewObjects = new List<GameObject>();

    string SceneContent = "";
    public string SceneName = "";

    enum bundleStepEnum
    {
        LoadingXML,
        LoadingXMLDone,
        LoadingBundles,
        LoadingBunelesDone,
        Building,
        BuildingDone,
		None,
    }

    bundleStepEnum bundleStep = bundleStepEnum.None;

    void Awake()
    {
       // _Instance = this;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.X) && IsDebug)
        {
            LoadBundleByName(Paths);
        }

        if (bundleStep == bundleStepEnum.LoadingXMLDone)
        {
            bundleStep = bundleStepEnum.LoadingBundles;
            StartCoroutine(LoadBundlesFromPath(allbundlesNeedToBeDownload.ToArray()));
        }

        if (bundleStep == bundleStepEnum.LoadingBunelesDone)
        {
            #region Build Assets From Bundle
            // build bunle;
#if NGUI		
#else			
            if (_UI_CS_LoadProgressCtrl.Instance) {
                _UI_CS_LoadProgressCtrl.Instance.LoadingSteps = _UI_CS_LoadProgressCtrl.EnumLoadingSteps.BuildingBundles;
            }
#endif
            int i = 0;
            foreach (string _path in Paths)
            {
                i++;

                UnityEngine.Object _resultObj = GetBundleFromPath(_path);

                #region Check if object is npcbase / interactive object / scene thing, add it to the prefab list.
                if (_resultObj.GetType() == typeof(GameObject))
                {
                    GameObject _go = (GameObject)_resultObj;
                    if (_go.GetComponent<NpcBase>())
                    {
                        MonsterPrefabs.Add(_go.GetComponent<NpcBase>());
                        _go.GetComponent<NpcCreateModel>().CreateAniamtionModel(_go.GetComponent<NpcBase>());
                    }
                    if (_go.GetComponent<InteractiveObj>())
                        InteractiveObjPrefabs.Add(_go.GetComponent<InteractiveObj>());
                    if (_go.GetComponent<Bundle_Scene_Flag>())
                        SceneThingsPrefabs.Add(_go);
                }
                if (_resultObj.GetType() == typeof(Transform))
                {
                    Transform _go = (Transform)_resultObj;
                    if (_go.GetComponent<NpcBase>())
                    {
                        MonsterPrefabs.Add(_go.GetComponent<NpcBase>());
                        _go.GetComponent<NpcCreateModel>().CreateAniamtionModel(_go.GetComponent<NpcBase>());
                    }
                    if (_go.GetComponent<InteractiveObj>())
                        InteractiveObjPrefabs.Add(_go.GetComponent<InteractiveObj>());
                    if (_go.GetComponent<Bundle_Scene_Flag>())
                        SceneThingsPrefabs.Add(_go.gameObject);
                }
                #endregion
            }

            ShowAllSceneThings();
			// we put these new object in a empty object which is unactive. so that, if we create a new object from this prefab, we don't need to setactive(true) again.
			// but when unity set active to a object, the function "Awake()" could be called. and in some script, these are some codes to create new gameobjects.
			// That would cause some bug, because when we build the bundle, we find the children by childID, it's not stable if a new gameobject insert to the parent.
			// So we record what game objects we created, and set them all active at the end.
			if(NewObjects.Count > 0)
			{
				foreach(GameObject _obj in NewObjects)
					_obj.SetActive(true);
			}
			
			bundleStep = bundleStepEnum.BuildingDone;

            #endregion

            #region CleanUp

            allbundlesNeedToBeDownload.Clear();
            nextCheckList.Clear();
            Resources.UnloadUnusedAssets();

            #endregion

            if (CS_SceneInfo.Instance)
                CS_SceneInfo.Instance.BundleDownloadDone();
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            //Debug.Log("Space");
            //CachedObjs.Clear();
            //Resources.UnloadUnusedAssets();
        }
    }

    public void ShowAllSceneThings()
    {
        XMLParser _parser = new XMLParser();
        XMLNode _xmlContent = _parser.Parse(SceneContent);

        XMLNodeList _components = _xmlContent.GetNodeList("" + SceneName + ">0>Scene_Objs>0>Obj_Name");
        _components = _xmlContent.GetNodeList("" + SceneName + ">0>Scene_Objs>0>Obj_Name");
        if (_components != null && _components.Count > 0)
        {
            for (int i = 0; i < _components.Count; i++)
            {
                XMLNode _bundleInfoNode = (XMLNode)_components[i];
                string _sceneObjString = _bundleInfoNode.GetValue("@Content");

                foreach (GameObject _obj in SceneThingsPrefabs)
                {
                    if (_obj.name.Contains(_sceneObjString))
                    {
                        float x = float.Parse(_bundleInfoNode.GetValue("@Pos_X"));
                        float y = float.Parse(_bundleInfoNode.GetValue("@Pos_Y"));
                        float z = float.Parse(_bundleInfoNode.GetValue("@Pos_Z"));
                        _obj.transform.position = new Vector3(x, y, z);
                        _obj.transform.parent = transform;
						if(_obj.GetComponentInChildren<BGManager>())
							_obj.GetComponentInChildren<BGManager>().ResetInstance();
                    }
                }
            }
        }
    }

    public void DestroyAllSceneThings()
    {
        foreach (GameObject _go in SceneThingsPrefabs)
        {
            Destroy(_go);
        }
        SceneThingsPrefabs.Clear();
    }

    #region Bundle
	
	public void LoadBundleByScene(string _content, string _SceneName)
	{
		
		SceneContent = _content;
        SceneName = _SceneName;
		
		XMLParser _parser = new XMLParser();
        XMLNode _xmlContent = _parser.Parse(_content);

        XMLNodeList _components = _xmlContent.GetNodeList("" + _SceneName + ">0>BundlePath");
		
		allbundlesNeedToBeDownload.Clear();
		if (_components != null && _components.Count > 0)
        {
            for (int i = 0; i < _components.Count; i++)
            {
                XMLNode _bundleInfoNode = (XMLNode)_components[i];
                allbundlesNeedToBeDownload.Add(_bundleInfoNode.GetValue("@bundle"));
            }
        }
		bundleStep = bundleStepEnum.LoadingXMLDone;
	}

    public void LoadBundleBySceneXML(string _content, string _SceneName)
    {
		
        SceneContent = _content;
        SceneName = _SceneName;

        List<string> _bundleNames = new List<string>();

        #region Find what bundles need to be load.
        XMLParser _parser = new XMLParser();
        XMLNode _xmlContent = _parser.Parse(_content);

        XMLNodeList _components = _xmlContent.GetNodeList("" + SceneName + ">0>Breakable_Objs>0>Obj_Name");

        if (_components != null && _components.Count > 0)
        {
            for (int i = 0; i < _components.Count; i++)
            {
                XMLNode _bundleInfoNode = (XMLNode)_components[i];
                _bundleNames.Add(_bundleInfoNode.GetValue("@Content"));
            }
        }

        _components = _xmlContent.GetNodeList("" + SceneName + ">0>Monster_Objs>0>Obj_Name");
        if (_components != null && _components.Count > 0)
        {
            for (int i = 0; i < _components.Count; i++)
            {
                XMLNode _bundleInfoNode = (XMLNode)_components[i];
                _bundleNames.Add(_bundleInfoNode.GetValue("@Content"));
            }
        }

        _components = _xmlContent.GetNodeList("" + SceneName + ">0>Scene_Objs>0>Obj_Name");
        if (_components != null && _components.Count > 0)
        {
            for (int i = 0; i < _components.Count; i++)
            {
                XMLNode _bundleInfoNode = (XMLNode)_components[i];
                _bundleNames.Add(_bundleInfoNode.GetValue("@Content"));
            }
        }
        #endregion

        LoadBundleByName(_bundleNames.ToArray());
    }

    public void LoadBundleByName(string[] _names)
    {
		allbundlesNeedToBeDownload.Clear();

        for (int i = 0; i < _names.Length; i++)
        {
            string _s = _names[i];
            _s = "Objs/Obj_" + _s + ".asset";
            _names[i] = _s;

//            Debug.LogError(_names[i]);
        }
        LoadBundleByPath(_names);
    }

    public void LoadBundleByPath(string[] _paths)
    {
        bundleStep = bundleStepEnum.LoadingXML;
        allbundlesNeedToBeDownload.Clear();

        Paths = _paths;
#if NGUI
		LoadingScreenCtrl.Instance.SetDownLoadProgress("Loading...",0);
#else
        if (_UI_CS_LoadProgressCtrl.Instance) {
            _UI_CS_LoadProgressCtrl.Instance.LoadingSteps = _UI_CS_LoadProgressCtrl.EnumLoadingSteps.Analysising;
            _UI_CS_LoadProgressCtrl.Instance.LoadBarAni(0);
            _UI_CS_LoadProgressCtrl.Instance.LoadObjNameText.Text = "Loading...";
        }
#endif
        StartCoroutine(LoadXMLFromPath(Paths));
    }

    public UnityEngine.Object GetBundleFromPath_GameObject(string _path)
    {
        Transform _obj = ((GameObject)CachedObjs[_path].DownLoadedObj).transform;
		if(!CachedObjs[_path].IsAssemblyDone)
		{
			string[] _splitPath = _path.Split('/');
	        string _xmlName = _splitPath[_splitPath.Length - 1];
	        if (_xmlName.Contains("Obj_"))
	        {
	            _xmlName = _xmlName.Replace("Obj_", "");
	            _xmlName = _xmlName.Replace(".asset", "");
	        }
	
	        XMLParser _parser = new XMLParser();
	        string _content = CachedXML[_path].Content;
	        XMLNode _xmlContent = _parser.Parse(_content);
	        XMLNodeList _components = _xmlContent.GetNodeList("" + _xmlName + ">0>BundleInfo");
			if (_components != null && _components.Count > 0)
	        {
	            for (int i = 0; i < _components.Count; i++)
	            {
	                XMLNode _bundleInfoNode = (XMLNode)_components[i];
	                XMLNode _gameObjPathNode = _bundleInfoNode.GetNode("ComponentPath>0");
	                string _gameObjPath = _gameObjPathNode.GetValue("@Path");
	                Transform _tempBaseObj = _obj;
	                if (_gameObjPath != null && _gameObjPath != "")
	                {
						bool _showdebug = false;
						if(_tempBaseObj.name.Contains("CH_Tree_RootsBlood_temp"))
						{
							Debug.LogWarning("child path " + _tempBaseObj.name + " :: " + _gameObjPath);
							_showdebug = true;
						}
	                    string[] _childPath = _gameObjPath.Split('>');
	                    for (int _childIndex = 0; _childIndex < _childPath.Length; _childIndex++)
	                    {
	                        if (_tempBaseObj != null)
	                        {
								if(_showdebug)Debug.Log("" + _tempBaseObj.name + " :: " + _tempBaseObj.childCount + " -- " + _childPath[_childIndex]);
	                            _tempBaseObj = _tempBaseObj.transform.GetChild(int.Parse(_childPath[_childIndex]));
								
								// before, when boss is killed, code would kill the gameobject of sfx as well. that makes reloading pop up some null object error
								// so fixed it by setting gameobject active to false.
								// if we need to re-use it. the gameobject should be set to be active.
								// todo : try to save the active state of object when create the bundle ,and reset the active state when we start to use it.
								if(_tempBaseObj.GetComponent<AudioTrigger>())
									_tempBaseObj.gameObject.SetActive(true);
	                        }
	                    }
	                }
	                if (_tempBaseObj != null)
	                {
	                    _bundleInfoNode = _bundleInfoNode.GetNode("BundlePath>0");
	                    int _POS = int.Parse(_bundleInfoNode.GetValue("@attPOS"));
	                    string _pathString = _bundleInfoNode.GetValue("@attrPath");
	                    string _bundlePath = _bundleInfoNode.GetValue("@bundlePath");
	
	                    string[] _strings = _pathString.Split('>');
	                    int _index = 0;
	
	                    Type _tempType = FindTypeInLoadedAssemblies(_strings[_index]);
	                    object _tempObj = (object)_tempBaseObj.GetComponent(_tempType);
	                    object _baseObj = null;
	                    if (_tempObj != null)
	                    {
	                        FieldInfo _tempF1 = null;
	                        PropertyInfo _tempP1 = null;
	                        while (_index < _strings.Length - 1)
	                        {
	                            _tempF1 = _tempObj.GetType().GetField(_strings[_index + 1]);
	                            if (_tempF1 != null)
	                            {
	                                _baseObj = _tempObj;
	                                _tempObj = _tempF1.GetValue(_tempObj);
	                            }
	                            else
	                            {
	                                _tempP1 = _tempObj.GetType().GetProperty(_strings[_index + 1]);
	                                if (_tempP1 != null)
	                                {
	                                    _baseObj = _tempObj;
	                                    _tempObj = _tempP1.GetValue(_tempObj, null);
	                                }
	                            }
	                            _index++;
	                        }
	
	                        if (_tempF1 != null || _tempP1 != null)
	                        {
	                            _bundlePath = _bundlePath.Replace("\\", "/");
	                            UnityEngine.Object _newAttrDataObj = GetBundleFromPath(_bundlePath);
	
	                            if (_newAttrDataObj != null)
	                            {
	                                if (_POS == -1)
	                                {
	                                    if (_newAttrDataObj.GetType() == typeof(GameObject))
	                                    {
	                                        GameObject _attrObj = (GameObject)_newAttrDataObj;
	                                        if (_tempF1 != null)
	                                        {
	                                            _tempF1.SetValue(_baseObj, _attrObj.transform);
	                                        }
	                                        if (_tempP1 != null)
	                                        {
	                                            _tempP1.SetValue(_baseObj, _attrObj.transform, null);
	                                        }
	                                    }
	                                    else
	                                    {
	                                        if (_tempF1 != null)
	                                        {
	                                            if (_tempF1.FieldType == typeof(GameObject) && _newAttrDataObj.GetType() == typeof(Transform))
	                                            {
	                                                Transform _newAttrDataTran = (Transform)_newAttrDataObj;
	                                                _tempF1.SetValue(_baseObj, _newAttrDataTran.gameObject);
	                                            }else
												{
	                                            	_tempF1.SetValue(_baseObj, _newAttrDataObj);
												}
	                                        }
	                                        if (_tempP1 != null)
	                                        {
	                                            _tempP1.SetValue(_baseObj, _newAttrDataObj, null);
	                                        }
	                                    }
	                                }
	                                else
	                                {
	                                    System.Array _array = (System.Array)_tempObj;
	
	                                    if (_newAttrDataObj.GetType() == typeof(GameObject))
	                                    {
	                                        GameObject _attrObj = (GameObject)_newAttrDataObj;
	                                        _array.SetValue(_attrObj.transform, _POS);
	                                    }
	                                    else
	                                    {
	                                        _array.SetValue(_newAttrDataObj, _POS);
	                                        if (_newAttrDataObj.GetType() == typeof(Material) && (_baseObj.GetType().IsSubclassOf(typeof(Renderer)) || _baseObj.GetType().Equals(typeof(Renderer))))
	                                        {
	                                            Renderer _renderer = (Renderer)_baseObj;
	                                            _renderer.materials = (Material[])_tempObj;
	                                        }
	                                    }
	                                }
	                            }
	                        }
	                    }
	                }
	            }
	        }
			CachedObjs[_path].IsAssemblyDone = true;
		}
        return _obj;
    }

    public UnityEngine.Object GetBundleFromPath_Material(string _path)
    {
        string[] _splitPath = _path.Split('/');
        string _xmlName = _splitPath[_splitPath.Length - 1];
        _xmlName = _xmlName.Replace(".asset", "");

        XMLParser _parser = new XMLParser();
        string _content = CachedXML[_path].Content;
        XMLNode _xmlContent = _parser.Parse(_content);
        XMLNodeList _components = _xmlContent.GetNodeList("" + _xmlName + ">0>BundleInfo");

        Material _newMaterial = (Material)CachedObjs[_path].DownLoadedObj;

        if (_components != null && _components.Count > 0)
        {
            for (int i = 0; i < _components.Count; i++)
            {
                XMLNode _bundleInfoNode = (XMLNode)_components[i];
                _bundleInfoNode = _bundleInfoNode.GetNode("BundlePath>0");
                int _POS = int.Parse(_bundleInfoNode.GetValue("@attPOS"));
                string _pathString = _bundleInfoNode.GetValue("@attrPath");
                string _bundlePath = _bundleInfoNode.GetValue("@bundlePath");

                _bundlePath = _bundlePath.Replace("\\", "/");
                UnityEngine.Object _newAttrDataObj = GetBundleFromPath(_bundlePath);
                if (_newAttrDataObj != null)
                {
                    string[] _strings = _pathString.Split('>');
                    PropertyInfo _tempP1 = _newMaterial.GetType().GetProperty(_strings[_strings.Length - 1]);

                    if (_tempP1 != null)
                    {
                        _tempP1.SetValue(_newMaterial, _newAttrDataObj, null);
                    }
                    else
                    {
                        if (_newMaterial.HasProperty(_strings[_strings.Length - 1]))
                        {
                            _newMaterial.SetTexture(_strings[_strings.Length - 1], (Texture)_newAttrDataObj);
                        }
                    }
                }
            }
        }
        return _newMaterial;
    }

    public UnityEngine.Object GetBundleFromPath(string _path)
    {
        if (!CachedObjs.ContainsKey(_path)) return null;

        if (CachedObjs[_path].DownLoadedObj)
        {
            if(CachedObjs[_path].DownLoadedObj.GetType() == typeof(UnityEngine.GameObject))
                return GetBundleFromPath_GameObject(_path);
            if(CachedObjs[_path].DownLoadedObj.GetType() == typeof(UnityEngine.Material))
                return GetBundleFromPath_Material(_path);

            return CachedObjs[_path].DownLoadedObj;
        }

        return null;
    }

    public IEnumerator LoadXMLFromPath(string[] _paths)	
	{
        nextCheckList.Clear();
       
        foreach (string _path in _paths)
        {
			
//			Debug.LogWarning(_path);
            string _tempPath = BundlePath.AssetbundleBaseURL + _path;
            if (!allbundlesNeedToBeDownload.Contains(_path))
                allbundlesNeedToBeDownload.Add(_path);
            else
                continue;

            string[] _splitPath = _tempPath.Split('/');
            if (_splitPath[_splitPath.Length - 1].Contains("Obj_") || _splitPath[_splitPath.Length - 1].Contains("Mat_"))
            {
                string _xmlName = _splitPath[_splitPath.Length - 1].Replace("Obj_", "");
                _xmlName = _xmlName.Replace(".asset", "");
                string _xmlPath = "";
                for (int i = 0; i < _splitPath.Length - 2; i++)
                {
                    _xmlPath += _splitPath[i] + "/";
                }
				
                _xmlPath += "XML/" ;
				if(_splitPath[_splitPath.Length - 1].Contains("Mat_"))
					_xmlPath += "MAT/";
				_xmlPath += _xmlName + ".xml";

                WWW _xmlWWW = new WWW(_xmlPath);
                yield return _xmlWWW;
                if (!CachedXML.ContainsKey(_path))
                {
                    CachedXML.Add(_path, new CachedXMLData(_xmlWWW));
                }
				
                XMLParser _parser = new XMLParser();
                XMLNode _xmlContent = _parser.Parse(_xmlWWW.text);

                XMLNodeList _components = _xmlContent.GetNodeList("" + _xmlName + ">0>BundleInfo");

                if (_components != null && _components.Count > 0)
                {
                    for (int i = 0; i < _components.Count; i++)
                    {
                        XMLNode _bundleInfoNode = (XMLNode)_components[i];
                        _bundleInfoNode = _bundleInfoNode.GetNode("BundlePath>0");
                        string _bundlePath = _bundleInfoNode.GetValue("@bundlePath");
                        _bundlePath = _bundlePath.Replace("\\", "/");
                        if (!nextCheckList.Contains(_bundlePath))
                            nextCheckList.Add(_bundlePath);
                    }
                }
            }
        }
        if (nextCheckList.Count > 0)
        {
            StartCoroutine(LoadXMLFromPath(nextCheckList.ToArray()));
        }
        else
        {
            bundleStep = bundleStepEnum.LoadingXMLDone;
        }
	}

	/// <summary>
	/// Download bundles from path.
	/// </summary>
	/// <returns>
	/// downloading result.
	/// </returns>
	/// <param name='_paths'>
	/// _paths.
	/// </param>
    public IEnumerator LoadBundlesFromPath(string[] _paths)
    {
        Debug.LogWarning("Start Downloading");
#if NGUI
#else
        if (_UI_CS_LoadProgressCtrl.Instance)
            _UI_CS_LoadProgressCtrl.Instance.LoadingSteps = _UI_CS_LoadProgressCtrl.EnumLoadingSteps.DownloadingBundles;
#endif
        int i = 0;

        foreach (string _path in _paths)
        {
            CachedDownloadData _newData = null;
            string _tempPath = BundlePath.AssetbundleBaseURL + _path;

            if (!CachedObjs.ContainsKey(_path))
            {
                WWW _tempWWW = new WWW(BundlePath.AssetbundleBaseURL + _path);
                yield return _tempWWW;

                _newData = new CachedDownloadData(_tempWWW);
				
                UnityEngine.Object _instanceObj = _tempWWW.assetBundle.mainAsset;
                if (_tempWWW.assetBundle.mainAsset.GetType() == typeof(GameObject) ||
                    _tempWWW.assetBundle.mainAsset.GetType() == typeof(Transform) ||
                    _tempWWW.assetBundle.mainAsset.GetType() == typeof(Material))
                {
                    _instanceObj = Instantiate(_tempWWW.assetBundle.mainAsset) as UnityEngine.Object;
                    if (_instanceObj.GetType() == typeof(GameObject))
                    {
                        GameObject _go = (GameObject)_instanceObj;
                        _go.transform.parent = _Parent;
						NewObjects.Add(_go);
                    }
                    if (_instanceObj.GetType() == typeof(Transform))
                    {
                        Transform _go = (Transform)_instanceObj;
                        _go.parent = _Parent;
						NewObjects.Add(_go.gameObject);
                    }
                }
                _newData.DownLoadedObj = _instanceObj;

                CachedObjs.Add(_path, _newData);
            }
            else
            {
                _newData = CachedObjs[_path];
            }

            i++;
#if NGUI
			LoadingScreenCtrl.Instance.SetDownLoadProgress("Loading...",((float)i / (float)_paths.Length) * 0.9f);
#else
            if (_UI_CS_LoadProgressCtrl.Instance) {
                _UI_CS_LoadProgressCtrl.Instance.LoadObjNameText.Text = "Loading...";
                _UI_CS_LoadProgressCtrl.Instance.LoadBarAni(((float)i / (float)_paths.Length) * 0.9f);
            }
#endif
        }
        bundleStep = bundleStepEnum.LoadingBunelesDone;
        Debug.LogWarning("Downloading all done!");
    }

    public static Type FindTypeInLoadedAssemblies(string typeName)
    {
        Type _type = null;
        foreach (var assembly in AppDomain.CurrentDomain.GetAssemblies())
        {
            _type = assembly.GetType(typeName);
            if (_type != null)
                break;
        }

        return _type;
    }
	
	public void HideSceneThings()
	{
		for(int i = 0; i < SceneThingsPrefabs.Count; i++)
		{
			SceneThingsPrefabs[i].transform.parent = _Parent;
		}
		SceneThingsPrefabs.Clear();
		MonsterPrefabs.Clear();
        InteractiveObjPrefabs.Clear();
	}
	
    public void ClearALL()
    {
		
        foreach (CachedDownloadData _obj in CachedObjs.Values)
        {
            _obj.ClearAll();
        }

        foreach (CachedXMLData _obj in CachedXML.Values)
        {
            _obj.ClearAll();
        }

        DestroyAllSceneThings();

        CachedObjs.Clear();
        MonsterPrefabs.Clear();
        InteractiveObjPrefabs.Clear();
        allbundlesNeedToBeDownload.Clear();
        NewObjects.Clear();

        Resources.UnloadUnusedAssets();

        Destroy(gameObject);
    }

    #endregion
}


