using UnityEngine;
using UnityEditor;
using System.IO;
using System.Collections;

public class RebuildMonsterBundleXML{

	[MenuItem("Bundle Tools/RebuildXML")]
    public static void LoadVAR()
    {
        Debug.Log("Start Rebuild XML.");
        if (!Directory.Exists("Assets/Resources/XML"))
            Directory.CreateDirectory("Assets/Resources/XML");

		string[] existingXML = Directory.GetFiles("Assets/Resources/XML", "*.xml", SearchOption.AllDirectories);
		
		foreach(string _xmlString in existingXML)
		{
			//Debug.Log(_xmlString);
			AssetDatabase.ImportAsset(_xmlString, ImportAssetOptions.ForceUpdate);
			string _fileName = _xmlString;
	        TextAsset _textAsset = (TextAsset)Resources.LoadAssetAtPath(_fileName, typeof(TextAsset));
	        XMLParser _parser = new XMLParser();
	        XMLNode _xmlContent = _parser.Parse(_textAsset.text);
	
	        string[] _tempStrings = _fileName.Split(CreateAssetbundles.pathSeparator, System.StringSplitOptions.RemoveEmptyEntries);
	        _fileName = _tempStrings[_tempStrings.Length - 1];
	        if (_fileName.Contains(".xml"))
	            _fileName = _fileName.Replace(".xml", "");
	
	        XMLNodeList _components = _xmlContent.GetNodeList(_fileName + ">0>Component");
	
	        // xml
	        XMLFileWriter _fileWriter = new XMLFileWriter();
	        string _filePath =  CreateCurrentSceneBuddle.AssetbundlePath + "XML" + Path.DirectorySeparatorChar;
            if (_fileName.Contains("Mat_")) _filePath += "MAT" + Path.DirectorySeparatorChar;
	        if (!Directory.Exists(_filePath))
	            Directory.CreateDirectory(_filePath);
	        _filePath += _fileName + ".xml";
	        _fileWriter.BindWithFile(_filePath);
	
	        _fileWriter.NodeBegin(_fileName);
	
			if(_components != null && _components.Count > 0)
			{
		        for (int i = 0; i < _components.Count; i++)
		        {
		            // Component
		            XMLNode _componentNode = (XMLNode)_components[i];
		            string _componentPath = _componentNode.GetValue("@Path");
                    if (_componentPath == null) _componentPath = "";
		            string _componentName = _componentNode.GetValue("@Type");
                    string _componentPOS = _componentNode.GetValue("@Child_Position");
		
		            string _bundlePath = "" + _componentName;
		
		            // Var in Component
                    LoadVAR(_componentNode.GetNodeList("VAR"), _componentPath, _componentPOS, _bundlePath, _fileWriter);
		        }
		
		        _fileWriter.NodeEnd(_fileName);
		        _fileWriter.Flush();
		        _fileWriter.ShutDown();
			}
		}

        AssetDatabase.DeleteAsset("Assets/Resources/XML");
        Debug.Log("Rebuild XML Done.");
    }

    static void LoadVAR(XMLNodeList _vars, string _componentPath, string _componentPOS, string _bundlePath, XMLFileWriter _fileWriter)
    {
        if (_vars != null && _vars.Count > 0)
        {
            for (int j = 0; j < _vars.Count; j++)
            {
                XMLNode _varNode = (XMLNode)_vars[j];
                string _varName = _varNode.GetValue("@Name");
				
                // Member in Var
                XMLNodeList _members = _varNode.GetNodeList("Member");
                if (_members != null && _members.Count > 0)
                {
                    for (int k = 0; k < _members.Count; k++)
                    {
                        XMLNode _memberNode = (XMLNode)_members[k];
                        string _memberType = _memberNode.GetValue("@Type");
                        string _memberBundlePath = _memberNode.GetValue("@BundlePath");
                        string _memberPos = _memberNode.GetValue("@POS");
						
						string _tempBundlePath = _bundlePath + ">" + _varName;

                        if (_memberBundlePath != null && _memberBundlePath.Length > 0)
                        {
                            // record bundle path;
                            _fileWriter.NodeBegin("BundleInfo");

                            _fileWriter.NodeBegin("ComponentPath");
                            if (_componentPOS != null)
                                _fileWriter.AddAttribute("POS", _componentPOS);

                            _fileWriter.AddAttribute("Path",_componentPath);
                            _fileWriter.NodeEnd("ComponentPath");

                            _fileWriter.NodeBegin("BundlePath");
                            _fileWriter.AddAttribute("attrPath", _tempBundlePath);
                            _fileWriter.AddAttribute("attPOS", _memberPos);
                            _fileWriter.AddAttribute("bundlePath",_memberBundlePath);
                            _fileWriter.NodeEnd("BundlePath");

                            _fileWriter.NodeEnd("BundleInfo");
                        }
                        LoadVAR(_memberNode.GetNodeList("VAR"), _componentPath, _componentPOS, _tempBundlePath, _fileWriter);
                    }
                }
            }
        }
    }
}
