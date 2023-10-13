using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class CollectBundleInfo : MonoBehaviour {
	
	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Space))
		{
			CollectSingleSceneBundleInfo();
		}
	}
	
	List<string> allbundlesNeedToBeDownload = new List<string>();
	List<string> nextCheckList = new List<string>();
	
	public void CollectSingleSceneBundleInfo()
	{
		allbundlesNeedToBeDownload.Clear();
		StartCoroutine(DownloadMonsterBundle("Hub_Village"));
	}
	
	public IEnumerator DownloadMonsterBundle(string _mapname)
    {
        string _sceneXMLPath = BundlePath.AssetbundleBaseURL + "XML/SCENE/" + _mapname + ".xml";

        WWW _bundleInfo = new WWW(_sceneXMLPath);

        yield return _bundleInfo;
		
		XMLParser _parser = new XMLParser();
		XMLNode _xmlContent = _parser.Parse(_bundleInfo.text);
		
		List<string> _bundleNames = new List<string>();
		
		XMLNodeList _components = _xmlContent.GetNodeList("" + _mapname + ">0>Breakable_Objs>0>Obj_Name");
        if (_components != null && _components.Count > 0)
        {
            for (int i = 0; i < _components.Count; i++)
            {
                XMLNode _bundleInfoNode = (XMLNode)_components[i];
                _bundleNames.Add(_bundleInfoNode.GetValue("@Content"));
            }
        }

        _components = _xmlContent.GetNodeList("" + _mapname + ">0>Monster_Objs>0>Obj_Name");
        if (_components != null && _components.Count > 0)
        {
            for (int i = 0; i < _components.Count; i++)
            {
                XMLNode _bundleInfoNode = (XMLNode)_components[i];
                _bundleNames.Add(_bundleInfoNode.GetValue("@Content"));
            }
        }

        _components = _xmlContent.GetNodeList("" + _mapname + ">0>Scene_Objs>0>Obj_Name");
        if (_components != null && _components.Count > 0)
        {
            for (int i = 0; i < _components.Count; i++)
            {
                XMLNode _bundleInfoNode = (XMLNode)_components[i];
                _bundleNames.Add(_bundleInfoNode.GetValue("@Content"));
            }
        }
		
		for (int i = 0; i < _bundleNames.Count; i++)
        {
            string _s = _bundleNames[i];
            _s = "Objs/Obj_" + _s + ".asset";
            _bundleNames[i] = _s;
        }

		StartCoroutine(LoadXMLFromPath(_bundleNames.ToArray(), _mapname));
	}
	
	public IEnumerator LoadXMLFromPath(string[] _paths, string _mapname)	
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
            StartCoroutine(LoadXMLFromPath(nextCheckList.ToArray(), _mapname));
        }
        else
        {
			CreateFileForScene(allbundlesNeedToBeDownload.ToArray(), _mapname);
        }
	}
	
	void CreateFileForScene(string[] _needBundles, string _mapname)
	{
		XMLFileWriter _fileWriter = new XMLFileWriter();
        string _filePath =  "Assets/Resources/XML/SCENE/";
        if (!Directory.Exists(_filePath))
            Directory.CreateDirectory(_filePath);
        _filePath += _mapname + "_AllBundleInfo.xml";
        _fileWriter.BindWithFile(_filePath);

        _fileWriter.NodeBegin(_mapname);
		RecordAllInfo(_needBundles, "Objs",_fileWriter);
		RecordAllInfo(_needBundles, "Mats",_fileWriter);
		RecordAllInfo(_needBundles, "Graphics",_fileWriter);
		RecordAllInfo(_needBundles, "Sounds",_fileWriter);
		_fileWriter.NodeEnd(_mapname);
		_fileWriter.Flush();
        _fileWriter.ShutDown();
		Debug.Log("Complete : " + _mapname);
	}
	
	void RecordAllInfo(string[] _needBundles, string _type, XMLFileWriter _fileWriter)
	{
		for(int i = 0; i < _needBundles.Length; i++)
		{
			string[] strings = _needBundles[i].Split('/');
			if(strings[0].Contains(_type))
			{
				_fileWriter.NodeBegin("BundlePath");
			    _fileWriter.AddAttribute("bundle", _needBundles[i]);
				_fileWriter.NodeEnd("BundlePath");
			}
		}
	}
}
