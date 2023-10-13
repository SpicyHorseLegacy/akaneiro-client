using System.Collections.Generic;
using System.IO;
using System.Reflection;
using System;
using UnityEditor;
using UnityEngine;
using System.Security.Cryptography;

public class BuildBundleMonsters
{
    [MenuItem("Build/Bundle/Monsters")]
    public static void Execute()
    {
        string[] existingGams = Directory.GetFiles("Assets/Prefabs/CH/Enemies", "*.prefab", SearchOption.AllDirectories);

        List<Transform> monsters = new List<Transform>();

        foreach (string MonsterFile in existingGams)
        {
            string[] MonsterDatas = MonsterFile.Split(CreateAssetbundles.pathSeparator, System.StringSplitOptions.RemoveEmptyEntries);
            Transform MonsterTransform = AssetDatabase.LoadAssetAtPath(MonsterFile, typeof(Transform)) as Transform;
            monsters.Add(MonsterTransform);
        }

        BuildMonsters(monsters.ToArray());
	}

    public static void BuildBundles(GameObject[] Objs)
    {
        MonsterBundlePrepare();

        foreach (GameObject _obj in Objs)
        {
            BuildTargetObject(_obj);
        }

        MonsterBundleEnd();
    }

    public static void BuildMonsters(Transform[] npcs)
    {
        MonsterBundlePrepare();

        foreach (Transform _npc in npcs)
        {
            BuildTargetObject(_npc.gameObject);
        }

        MonsterBundleEnd();
    }

    static List<string> ResourcePath = new List<string>();
    static List<string> HasBundledPath = new List<string>();

    public static void MonsterBundlePrepare()
    {
        if (!Directory.Exists("Assets/bundles/"))
            Directory.CreateDirectory("Assets/bundles/");
        if (!Directory.Exists("Assets/bundles/Mats/"))
            Directory.CreateDirectory("Assets/bundles/Mats/");

        ResourcePath.Clear();
        HasBundledPath.Clear();

        Debug.Log("Build Bundle Start.");
    }

    public static void MonsterBundleEnd()
    {
        ResourcePath.Clear();
        HasBundledPath.Clear();

        AssetDatabase.DeleteAsset("Assets/bundles");

        RebuildMonsterBundleXML.LoadVAR();

        Debug.Log("Build Bundle Done.");
    }

    static string childPos(Transform _tran)
    {
        if (_tran && _tran.parent)
        {
            for (int i = 0; i < _tran.parent.childCount; i++)
            {
                if (_tran == _tran.parent.GetChild(i))
                {
                    //fileWriter.AddAttribute("Child_Position", i);
                    return i.ToString();
                }
            }
        }
        return "";
    }

    public static void BuildMaterial(Material _mat)
    {
        if (_mat == null) return;

        UnityEngine.Material _temp = (UnityEngine.Material)_mat;
        if (_temp != null)
        {
            UnityEngine.Material _tempValue = new Material(_temp);

            XMLFileWriter fileWriter = new XMLFileWriter();
            string _filePath = "Assets/Resources/" + "XML" + Path.DirectorySeparatorChar + "MAT" + Path.DirectorySeparatorChar;
            if (!Directory.Exists(_filePath))
                Directory.CreateDirectory(_filePath);
            _filePath += "Mat_" + _mat.name + ".xml";
            fileWriter.BindWithFile(_filePath);
            fileWriter.NodeBegin("Mat_" + _mat.name);

            fileWriter.NodeBegin("Component");
            fileWriter.AddAttribute("Type", "" + _tempValue.GetType().ToString());

            System.Reflection.PropertyInfo[] p_infos = _tempValue.GetType().GetProperties();
            foreach (System.Reflection.PropertyInfo _info in p_infos)
            {
                if ( _info.PropertyType == typeof(UnityEngine.Texture) ||
                    _info.PropertyType == typeof(UnityEngine.Texture2D))
                {
                    if (!_info.GetValue(_tempValue, null).Equals(null))
                    {
                        fileWriter.NodeBegin("VAR");
                        fileWriter.AddAttribute("Name", _info.Name);
                        fileWriter.AddAttribute("Field_Or_Property", "Field");

                        fileWriter.NodeBegin("Member");
                        fileWriter.AddAttribute("POS", -1);
                        fileWriter.AddAttribute("Type", _info.GetValue(_tempValue, null).GetType().ToString());
                        RecordBundlePath(_info.GetValue(_tempValue, null), fileWriter);

                        DetachObject(_info.GetValue(_tempValue, null), fileWriter);
                        _info.SetValue(_tempValue, null, null);

                        fileWriter.NodeEnd("Member");
                        fileWriter.NodeEnd("VAR");
                    }
                }
            }
            DetachTextreFromMaterial(_tempValue, "_EdgeMaskTex", fileWriter);
            DetachTextreFromMaterial(_tempValue, "_FadeOutMask", fileWriter);
            DetachTextreFromMaterial(_tempValue, "_NoiseTex", fileWriter);
            DetachTextreFromMaterial(_tempValue, "_MaskTex", fileWriter);
            DetachTextreFromMaterial(_tempValue, "_DetailTex", fileWriter);
			DetachTextreFromMaterial(_tempValue, "_TintMaskTex", fileWriter);
			DetachTextreFromMaterial(_tempValue, "_WaterTex", fileWriter);
			DetachTextreFromMaterial(_tempValue, "_CloudTex", fileWriter);
			DetachTextreFromMaterial(_tempValue, "_FadeTex", fileWriter);
			DetachTextreFromMaterial(_tempValue, "_PulsingTex", fileWriter);

            fileWriter.NodeEnd("Component");
            fileWriter.NodeEnd("Mat_" + _mat.name);

            fileWriter.Flush();
            fileWriter.ShutDown();

            RecordResource(_tempValue);
        }
    }

	public static void BuildTargetObject(GameObject _target)
	{
        if (_target.Equals(null)) return;

		if (ResourcePath.Count > 0)
        {
            foreach (string _path in ResourcePath)
            {
                BuildBundle(AssetDatabase.LoadAssetAtPath(_path, typeof(UnityEngine.Object)));
            }
            ResourcePath.Clear();
        }

        // sometimes, the value in a script maybe is the object self.
        if (HasBundledPath.Contains(_target.name)) return;

        HasBundledPath.Add(_target.name);

        UnityEngine.GameObject _tempObj = UnityEngine.Object.Instantiate(_target) as UnityEngine.GameObject;
        _tempObj.name = _tempObj.name.Replace("(Clone)", "");
        _tempObj.name = _tempObj.name.Replace(" ", "_space_");

        if (_target.GetComponent<Animation>())
           UnityEngine.Object.DestroyImmediate(_tempObj.GetComponent<Animation>());

        Component[] _coms = _tempObj.GetComponentsInChildren(typeof(Component));
        if (_coms.Length > 0)
        {
            XMLFileWriter fileWriter = new XMLFileWriter();
            string _filePath = "Assets/Resources/" + "XML" + Path.DirectorySeparatorChar;
            if (!Directory.Exists(_filePath))
                Directory.CreateDirectory(_filePath);
            _filePath += _tempObj.name + ".xml";
            fileWriter.BindWithFile(_filePath);

            fileWriter.NodeBegin(_tempObj.name);

            foreach (Component _c in _coms)
            {
                if (_c.GetType() != typeof(UnityEngine.Transform) && _c.GetType() != typeof(UnityEngine.Rigidbody) && _c.GetType() != typeof(UnityEngine.Collider) &&
                    _c.GetType() != typeof(UnityEngine.ParticleEmitter) && _c.GetType() != typeof(UnityEngine.ParticleAnimator))
                {
                    fileWriter.NodeBegin("Component");
                    fileWriter.AddAttribute("Type", "" + _c.GetType().ToString());

                    Transform _curTran = _c.transform;
                    string _path = childPos(_curTran);
                    // if parent isn't the root, record.
                    while (_curTran.parent != null && _curTran.parent.parent != null)
                    {
                        string _temps = _path;
                        _path = childPos(_curTran.parent) + ">" + _temps;
                        _curTran = _curTran.parent;
                    }

                    fileWriter.AddAttribute("Path", "" + _path);
					DetachAll(_c.GetType(), _c, fileWriter);
					
                    fileWriter.NodeEnd("Component");
                }
            }

            fileWriter.NodeEnd(_tempObj.name);

            fileWriter.Flush();
            fileWriter.ShutDown();
        }
		
        UnityEngine.Object _bundleObj = GetObjPrefab(_tempObj, _tempObj.name + "_temp");
        string bundlePathStr = CreateCurrentSceneBuddle.AssetbundlePath + "Objs" + Path.DirectorySeparatorChar;
        if (!Directory.Exists(bundlePathStr))
            Directory.CreateDirectory(bundlePathStr);
        bundlePathStr += "Obj_" + _target.name + ".asset";
        bundlePathStr = bundlePathStr.Replace(" ", "_space_");
        BuildPipeline.BuildAssetBundle(_bundleObj, null, bundlePathStr, BuildAssetBundleOptions.CollectDependencies, CreateAssetbundles.GetCurrentTarget());

        if (ResourcePath.Count > 0)
        {
            foreach (string _path in ResourcePath)
            {
                BuildBundle(AssetDatabase.LoadAssetAtPath(_path, typeof(UnityEngine.Object)));
            }
            ResourcePath.Clear();
        }
	}

    static bool IsObjInScene(UnityEngine.Object _obj)
    {
        UnityEngine.Object[] objs = GameObject.FindObjectsOfType(_obj.GetType());
        foreach (UnityEngine.Object _tempObj in objs)
        {
            if (_tempObj == _obj)
            {
                Debug.Log(_obj.name);
                return true;
            }
        }
        return false;
    }
	
	static void DetachAll(Type _t, object _c, XMLFileWriter _fileWriter)
	{
        if (_c == null || _t == typeof(UnityEngine.Transform) || _t == typeof(UnityEngine.GameObject)) return;

        #region FieldInfo
        System.Reflection.FieldInfo[] f_infos = _t.GetFields();
        foreach (System.Reflection.FieldInfo _info in f_infos)
        {
            if (_info.Name == "SummonSpawner")
            {
                // although we don't care about summonspawner infomation, and we don't need these infomation in bundle.
                _info.SetValue(_c, null);
            } 
			else
			{
                if (_info.IsPublic)
                {
                    if (_info.FieldType.BaseType == typeof(System.Array))
                    {
                        System.Array _tempArray = _info.GetValue(_c) as System.Array;

                        if (_tempArray.Length > 0)
                        {
							if (
								(
									_tempArray.GetValue(0).GetType().FullName.Contains("+") &&
									!_tempArray.GetValue(0).GetType().IsEnum
								) ||
                                _tempArray.GetValue(0).GetType() == typeof(UnityEngine.Transform) ||
                                _tempArray.GetValue(0).GetType() == typeof(UnityEngine.GameObject) ||
                                _tempArray.GetValue(0).GetType() == typeof(UnityEngine.Material) ||
                                _tempArray.GetValue(0).GetType() == typeof(UnityEngine.Texture) ||
                                _tempArray.GetValue(0).GetType() == typeof(UnityEngine.Texture2D) ||
                                _tempArray.GetValue(0).GetType() == typeof(UnityEngine.AnimationClip) ||
                                _tempArray.GetValue(0).GetType() == typeof(UnityEngine.AudioClip))
                            {
                                // Check if the value is child of object
                                _fileWriter.NodeBegin("VAR");
                                _fileWriter.AddAttribute("Name", _info.Name);
                                _fileWriter.AddAttribute("Field_Or_Property", "Field");

                                for (int i = _tempArray.Length - 1; i >= 0; i--)
                                {
                                    if (!_tempArray.GetValue(i).Equals(null))
                                    {
                                        // Check if the value is in the scene. if so, that means this object is a child of main asset. we don't need to figure it.
                                        if (_tempArray.GetValue(i).GetType() == typeof(UnityEngine.Transform) ||
                                            _tempArray.GetValue(i).GetType() == typeof(UnityEngine.GameObject))
                                        {
                                            if (IsObjInScene((UnityEngine.Object)_tempArray.GetValue(i)))
                                                continue;
                                        }

                                        _fileWriter.NodeBegin("Member");
                                        _fileWriter.AddAttribute("POS", i);
                                        _fileWriter.AddAttribute("Type", _tempArray.GetValue(i).GetType().ToString());
                                        RecordBundlePath(_tempArray.GetValue(i), _fileWriter);

                                        DetachObject(_tempArray.GetValue(i), _fileWriter);

                                        _fileWriter.NodeEnd("Member");
                                        // we need to keep the custom class infomation in array, so if the value is custom class, keep it.
                                        if (!_tempArray.GetValue(0).GetType().FullName.Contains("+"))
                                            _tempArray.SetValue(null, i);
                                    }
                                }

                                _fileWriter.NodeEnd("VAR");
                            }
                        }
                    }
                    else if ((_info.FieldType.FullName.Contains("+") && !_info.FieldType.IsEnum) ||
                            _info.FieldType == typeof(UnityEngine.Transform) ||
                            _info.FieldType == typeof(UnityEngine.GameObject) ||
                            _info.FieldType == typeof(UnityEngine.Material) ||
                            _info.FieldType == typeof(UnityEngine.Texture) ||
                            _info.FieldType == typeof(UnityEngine.Texture2D) ||
                            _info.FieldType == typeof(UnityEngine.AnimationClip) ||
                            _info.FieldType == typeof(UnityEngine.AudioClip))
                    {
                        if (!_info.GetValue(_c).Equals(null))
                        {
                            // Check if the value is in the scene. if so, that means this object is a child of main asset. we don't need to figure it.
                            if (_info.GetValue(_c).GetType() == typeof(UnityEngine.Transform) ||
                                _info.GetValue(_c).GetType() == typeof(UnityEngine.GameObject))
                            {
                                if (IsObjInScene((UnityEngine.Object)_info.GetValue(_c)))
                                    continue;
                            }

                            _fileWriter.NodeBegin("VAR");
                            _fileWriter.AddAttribute("Name", _info.Name);
                            _fileWriter.AddAttribute("Field_Or_Property", "Field");

                            _fileWriter.NodeBegin("Member");
                            _fileWriter.AddAttribute("POS", -1);
                            _fileWriter.AddAttribute("Type", _info.GetValue(_c).GetType().ToString());
                            RecordBundlePath(_info.GetValue(_c), _fileWriter);

                            DetachObject(_info.GetValue(_c), _fileWriter);

                            if (!_info.FieldType.FullName.Contains("+"))
                            	_info.SetValue(_c, null);

                            _fileWriter.NodeEnd("Member");
                            _fileWriter.NodeEnd("VAR");
                        }
                    }
                }
            }
        }
        #endregion

        #region PropertyInfo
        System.Reflection.PropertyInfo[] p_infos = _t.GetProperties();
        foreach (System.Reflection.PropertyInfo _info in p_infos)
        {
            // because if load materials directly, unity editor would throw memory leak warning out.
            // so, we only need to load sharedmaterials instead.
			// because we don't care about any Transfom in bones array.
			// so, we ignore the bones in SkinMeshRenderer
            if (_info.PropertyType.BaseType == typeof(System.Array) && _info.Name != "materials" && _info.Name != "bones")
            {
                System.Array _tempArray = _info.GetValue(_c, null) as System.Array;
				
				if(_tempArray.Length > 0)
				{
                    if (_tempArray.GetValue(0).GetType() == typeof(UnityEngine.Material) ||
						_tempArray.GetValue(0).GetType() == typeof(UnityEngine.Texture) ||
                        _tempArray.GetValue(0).GetType() == typeof(UnityEngine.Texture2D) ||
                        _tempArray.GetValue(0).GetType() == typeof(UnityEngine.AnimationClip) ||
                        _tempArray.GetValue(0).GetType() == typeof(UnityEngine.AudioClip))
                    {
                        _fileWriter.NodeBegin("VAR");
                        _fileWriter.AddAttribute("Name", _info.Name);
                        _fileWriter.AddAttribute("Field_Or_Property", "Property");

                        for (int i = _tempArray.Length - 1; i >= 0; i--)
                        {
                            if (!_tempArray.GetValue(i).Equals(null))
                            {
                                _fileWriter.NodeBegin("Member");
                                _fileWriter.AddAttribute("POS", i);
                                _fileWriter.AddAttribute("Type", _tempArray.GetValue(i).GetType().ToString());
                                RecordBundlePath(_tempArray.GetValue(i), _fileWriter);

                                DetachObject(_tempArray.GetValue(i), _fileWriter);
                                //_tempArray.SetValue(null, i);

                                _fileWriter.NodeEnd("Member");
                            }
                        }

                        _fileWriter.NodeEnd("VAR");
                    }
				}
            }
            else if (_info.PropertyType == typeof(UnityEngine.Texture) ||
                    _info.PropertyType == typeof(UnityEngine.Texture2D) ||
                    _info.PropertyType == typeof(UnityEngine.AnimationClip) ||
                    _info.PropertyType == typeof(UnityEngine.AudioClip))
            {
                if (!_info.GetValue(_c, null).Equals(null))
                {
                    _fileWriter.NodeBegin("VAR");
                    _fileWriter.AddAttribute("Name", _info.Name);
                    _fileWriter.AddAttribute("Field_Or_Property", "Property");

                    _fileWriter.NodeBegin("Member");
                    _fileWriter.AddAttribute("POS", -1);
                    _fileWriter.AddAttribute("Type", _info.GetValue(_c, null).GetType().ToString());
                    RecordBundlePath(_info.GetValue(_c, null), _fileWriter);
					
                    var _temp = _info.GetValue(_c, null);
                    _info.SetValue(_c, null, null);
                    DetachObject(_temp, _fileWriter);

                    _fileWriter.NodeEnd("Member");

                    _fileWriter.NodeEnd("VAR");
                }
            }
        }
        #endregion

        #region More work for some special components
        if (_c.GetType().IsSubclassOf(typeof(Renderer)) || _c.GetType().Equals(typeof(Renderer)))
        {
            Renderer _temp = (Renderer)_c;
            _temp.materials = new Material[_temp.sharedMaterials.Length];
        }
        #endregion

        RecordResource(_c);
	}

    static void DetachTextreFromMaterial(Material _m, string _string, XMLFileWriter _fileWriter)
    {
        if (_m.HasProperty(_string))
        {
            _fileWriter.NodeBegin("VAR");
            _fileWriter.AddAttribute("Name", _string);
            _fileWriter.AddAttribute("Field_Or_Property", "Property");

            _fileWriter.NodeBegin("Member");
            _fileWriter.AddAttribute("POS", -1);
            _fileWriter.AddAttribute("Type", _m.GetTexture(_string).GetType().ToString());
            RecordBundlePath(_m.GetTexture(_string), _fileWriter);

            UnityEngine.Texture _tempTex = _m.GetTexture(_string);
            _m.SetTexture(_string, null);
            DetachObject(_tempTex, _fileWriter);

            _fileWriter.NodeEnd("Member");

            _fileWriter.NodeEnd("VAR");
        }
    }

	static void DetachObject(object _o, XMLFileWriter _fileWriter)
	{
		if(_o.Equals(null)) return;
		var _value = _o;
		
        Type _valueType = _value.GetType();
        if (_valueType.FullName.Contains("+") && !_valueType.IsEnum)
        {
            DetachAll(_valueType, _value, _fileWriter);
        }
        else if (_valueType == typeof(UnityEngine.Transform))
        {
            UnityEngine.Transform _temp = (UnityEngine.Transform)_value;
            if (_temp != null)
                BuildTargetObject(_temp.gameObject);
        }
        else if (_valueType == typeof(UnityEngine.GameObject))
        {
            BuildTargetObject((UnityEngine.GameObject)_value);
        }
        else if (_valueType == typeof(UnityEngine.Material))
        {
            Material _valueMat = (Material)_value;
            BuildMaterial(_valueMat);
        }
        else if (_valueType == typeof(UnityEngine.AnimationClip) ||
                _valueType == typeof(UnityEngine.AudioClip) ||
                _valueType == typeof(UnityEngine.Texture) ||
                _valueType == typeof(UnityEngine.Texture2D))
        {
            DetachAll(_valueType, _value, _fileWriter);
        }
	}

    static void RecordResource(object _o)
    {
		if(_o.Equals(null)) return;
		
        var _value = _o;
        Type _valueType = _o.GetType();

        if (_valueType == typeof(UnityEngine.Material))
        {
            UnityEngine.Material _mat = (UnityEngine.Material)_value;
            AssetDatabase.CreateAsset(_mat, "Assets/bundles/Mats/" + _mat.name + ".mat");
            ResourcePath.Add(AssetDatabase.GetAssetPath((UnityEngine.Object)_value));
        }
        else if (_valueType == typeof(UnityEngine.AnimationClip) ||
                _valueType == typeof(UnityEngine.AudioClip) ||
                _valueType == typeof(UnityEngine.Texture) ||
                _valueType == typeof(UnityEngine.Texture2D))
        {
            ResourcePath.Add(AssetDatabase.GetAssetPath((UnityEngine.Object)_value));
        }
    }

    static void RecordBundlePath(object _o, XMLFileWriter _fileWriter)
    {
        if (_o.Equals(null))
			return;

        var _value = _o;
        Type _valueType = _value.GetType();

        UnityEngine.Object _bundleObj = null;
        string bundlePathStr = "";
        if (_valueType == typeof(UnityEngine.GameObject) || _valueType == typeof(UnityEngine.Transform))
        {
            _bundleObj = (UnityEngine.Object)_value;
            bundlePathStr = "Objs" + Path.DirectorySeparatorChar;
            bundlePathStr += "Obj_" + _bundleObj.name + ".asset";
        }
        else if (_valueType == typeof(UnityEngine.Material))
        {
            _bundleObj = (UnityEngine.Object)_value;
            bundlePathStr = "Mats" + Path.DirectorySeparatorChar;
            bundlePathStr += "Mat_" + _bundleObj.name + ".asset";
        }
        else if (_valueType == typeof(UnityEngine.AnimationClip))
        {
            _bundleObj = (UnityEngine.Object)_value;
            bundlePathStr = "Anims" + Path.DirectorySeparatorChar;
            bundlePathStr += "Anim_" + _bundleObj.name + ".asset";
        }
        else if (_valueType == typeof(UnityEngine.AudioClip))
        {
            _bundleObj = (UnityEngine.Object)_value;
            bundlePathStr = "Sounds" + Path.DirectorySeparatorChar;
            bundlePathStr += "Aud_" + _bundleObj.name + ".asset";
        }
        else if (_valueType == typeof(UnityEngine.Texture) || _valueType == typeof(UnityEngine.Texture2D))
        {
            _bundleObj = AssetDatabase.LoadAssetAtPath(AssetDatabase.GetAssetPath((UnityEngine.Object)_value), typeof(UnityEngine.Texture));
            bundlePathStr = "Graphics" + Path.DirectorySeparatorChar;
            bundlePathStr += "Tex_" + _bundleObj.name + ".asset";
        }

        bundlePathStr = bundlePathStr.Replace(" ", "_space_");

        // check if recording any bundle path, write the info into the file.
        if (bundlePathStr != CreateCurrentSceneBuddle.AssetbundlePath)
            _fileWriter.AddAttribute("BundlePath", bundlePathStr);
    }

    static void BuildBundle(object _o)
    {
		if(_o == null) return;
		
        var _value = _o;
        Type _valueType = _value.GetType();

        string bundlePathStr = CreateCurrentSceneBuddle.AssetbundlePath;
        UnityEngine.Object _bundleObj = null;

        if (_valueType == typeof(UnityEngine.Material))
        {
            _bundleObj = (UnityEngine.Object)_value;
            _bundleObj = AssetDatabase.LoadAssetAtPath(AssetDatabase.GetAssetPath((UnityEngine.Object)_value), typeof(UnityEngine.Material));
            bundlePathStr += "Mats" + Path.DirectorySeparatorChar;
            if (!Directory.Exists(bundlePathStr))
                Directory.CreateDirectory(bundlePathStr);
            bundlePathStr += "Mat_" + _bundleObj.name + ".asset";
        }
        else if (_valueType == typeof(UnityEngine.AnimationClip))
        {
            _bundleObj = (UnityEngine.Object)_value;
            bundlePathStr += "Anims" + Path.DirectorySeparatorChar;
            if (!Directory.Exists(bundlePathStr))
                Directory.CreateDirectory(bundlePathStr);
            bundlePathStr += "Anim_" + _bundleObj.name + ".asset";
        }
        else if (_valueType == typeof(UnityEngine.AudioClip))
        {
            _bundleObj = (UnityEngine.Object)_value;
            bundlePathStr += "Sounds" + Path.DirectorySeparatorChar;
            if (!Directory.Exists(bundlePathStr))
                Directory.CreateDirectory(bundlePathStr);
            bundlePathStr += "Aud_" + _bundleObj.name + ".asset";
        }
        else if (_valueType == typeof(UnityEngine.Texture) || _valueType == typeof(UnityEngine.Texture2D))
        {
            _bundleObj = AssetDatabase.LoadAssetAtPath(AssetDatabase.GetAssetPath((UnityEngine.Object)_value), typeof(UnityEngine.Texture));
            bundlePathStr += "Graphics" + Path.DirectorySeparatorChar;
            if (!Directory.Exists(bundlePathStr))
                Directory.CreateDirectory(bundlePathStr);
            bundlePathStr += "Tex_" + _bundleObj.name + ".asset";
        }

        bundlePathStr = bundlePathStr.Replace(" ", "_space_");

        if (_bundleObj != null)
        {
            if (!HasBundledPath.Contains(bundlePathStr))
            {
                HasBundledPath.Add(bundlePathStr);
                BuildPipeline.BuildAssetBundle(_bundleObj, null, bundlePathStr, BuildAssetBundleOptions.CollectDependencies, CreateAssetbundles.GetCurrentTarget());
            }
            else
            {
                //Debug.LogError("has bundled : " + bundlePathStr); 
            }
        }
    }

    static UnityEngine.Object GetObjPrefab(GameObject go, string name)
    {
        go.SetActive(false);
        UnityEngine.Object tempPrefab = PrefabUtility.CreateEmptyPrefab("Assets/bundles/" + name + ".prefab");
        tempPrefab = PrefabUtility.ReplacePrefab(go, tempPrefab);
        UnityEngine.Object.DestroyImmediate(go);
        return tempPrefab;
    }
}
