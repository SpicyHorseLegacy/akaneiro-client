using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class CreateCurrentSceneBundleTool : EditorWindow
{
    class BundleCheckItem
    {
        public bool IsChecked = true;
        public GameObject BundleItem = null;

        public BundleCheckItem(GameObject _new)
        {
            IsChecked = true;
            BundleItem = _new;
        }
    }

    BundleCheckItem[] Monsters;
    BundleCheckItem[] InteractiveObjs;
    BundleCheckItem[] SceneObjs;

    [MenuItem("Bundle Tools/Create Current Scene Bundle")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        CreateCurrentSceneBundleTool window = (CreateCurrentSceneBundleTool)EditorWindow.GetWindow(typeof(CreateCurrentSceneBundleTool), false, EditorApplication.currentScene);

        window.minSize = new Vector3(620, 400);
        window.maxSize = new Vector3(620, 1000);
    }

    void OnGUI()
    {
        if (GUILayout.Button("Refresh"))
        {
            Refresh();
        }

        EditorGUILayout.BeginVertical();
        {
            EditorGUILayout.HelpBox("" + EditorApplication.currentScene, MessageType.None);


            EditorGUILayout.BeginHorizontal();
            {
                #region Breakable Objects
                EditorGUILayout.BeginVertical("box", GUILayout.Width(200), GUILayout.MinHeight(300));
                {
                    DrawBundleGroupForAnArray(InteractiveObjs, "Interactive Objs", new Color(0, 139.0f / 255, 0), Color.white);
                }
                EditorGUILayout.EndVertical();
                #endregion

                #region Monsters
                EditorGUILayout.BeginVertical("box", GUILayout.Width(200), GUILayout.MinHeight(300));
                {
                    DrawBundleGroupForAnArray(Monsters, "Monster", new Color(178.0f / 255, 34.0f / 255, 34.0f / 255), Color.white);
                }
                EditorGUILayout.EndVertical();
                #endregion

                #region Scene Objects
                EditorGUILayout.BeginVertical("box", GUILayout.Width(200), GUILayout.MinHeight(300));
                {
					EditorGUILayout.HelpBox(" ", MessageType.None);
					if(SceneObjs != null)
					{
                        EditorGUILayout.BeginHorizontal();
                        {
                            Color _tempTC = GUI.contentColor;
                            GUI.contentColor = Color.green;
                            if (GUILayout.Button("+", GUILayout.Width(15)))
                            {
                                if (SceneObjs != null && SceneObjs.Length > 0)
                                {
                                    foreach (BundleCheckItem _item in SceneObjs)
                                    {
                                        _item.IsChecked = true;
                                    }
                                }
                            }
                            GUI.contentColor = Color.red;
                            if (GUILayout.Button("-", GUILayout.Width(15)))
                            {
                                foreach (BundleCheckItem _item in SceneObjs)
                                {
                                    _item.IsChecked = false;
                                }
                            }
                            GUI.contentColor = _tempTC;

                            int _num = 0;
                            if (SceneObjs != null)
                                _num = SceneObjs.Length;

                            int _okNum = 0;

                            if (SceneObjs != null && SceneObjs.Length > 0)
                            {
                                foreach (BundleCheckItem _item in SceneObjs)
                                {
                                    if (_item.IsChecked)
                                        _okNum++;
                                }
                            }

                            EditorGUILayout.HelpBox("Scene Objs" + " (" + _okNum + "/" + _num + ")", MessageType.None);
                        }
                        EditorGUILayout.EndHorizontal();
	
	                    foreach (BundleCheckItem _item in SceneObjs)
	                    {
	                        DrawACheckBox(_item);
	                    }
					}
                }
                EditorGUILayout.EndVertical();
                #endregion
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            {
                if (GUILayout.Button("Create Scene!"))
                {
                    List<GameObject> _bebundledObjs = new List<GameObject>();
                    foreach (BundleCheckItem _item in SceneObjs)
                    {
                        if (_item.IsChecked)
                            _bebundledObjs.Add(_item.BundleItem);
                    }
                    CreateCurrentSceneBuddle.BuildSceneBundle(EditorApplication.currentScene, _bebundledObjs.ToArray());

                    Refresh();
                }
                if (GUILayout.Button("Create All!"))
                {
                    List<BundleCheckItem> _bundles = new List<BundleCheckItem>();
                    foreach (BundleCheckItem _item in InteractiveObjs)
                    {
                        _bundles.Add(_item);
                    }
                    foreach (BundleCheckItem _item in Monsters)
                    {
                        _bundles.Add(_item);
                    }
                    BuildBundlesFromItems(_bundles.ToArray());

                    List<GameObject> _bebundledObjs = new List<GameObject>();
                    foreach (BundleCheckItem _item in SceneObjs)
                    {
                        if (_item.IsChecked)
                            _bebundledObjs.Add(_item.BundleItem);
                    }
                    CreateCurrentSceneBuddle.BuildSceneBundle(EditorApplication.currentScene, _bebundledObjs.ToArray());

                    Refresh();
                }
            }
            EditorGUILayout.EndHorizontal();

        }
        EditorGUILayout.EndVertical();
    }

    void Refresh()
    {
        List<BundleCheckItem> _tempArray = new List<BundleCheckItem>();

        foreach (NpcBase _npc in CreateCurrentSceneBuddle.FindAllMonsterPrefabInThisScene())
        {
            _tempArray.Add(new BundleCheckItem(_npc.gameObject));
        }
        Monsters = _tempArray.ToArray();
        _tempArray.Clear();

        foreach (InteractiveObj _interactiveObj in CreateCurrentSceneBuddle.FindAllBreakableObjPrefabInThisScene())
        {
            _tempArray.Add(new BundleCheckItem(_interactiveObj.gameObject));
        }
        InteractiveObjs = _tempArray.ToArray();
        _tempArray.Clear();

        foreach (GameObject _sceneObj in CreateCurrentSceneBuddle.FindAllSceneObjs())
        {
            _tempArray.Add(new BundleCheckItem(_sceneObj));
        }
        SceneObjs = _tempArray.ToArray();
        _tempArray.Clear();
    }

    void BuildBundlesFromItems(BundleCheckItem[] _items)
    {
        if (_items != null && _items.Length > 0)
        {
            List<GameObject> _tempObjs = new List<GameObject>();
            foreach (BundleCheckItem _item in _items)
            {
                if (_item.IsChecked)
                    _tempObjs.Add(_item.BundleItem);
            }

            if (_tempObjs.Count > 0)
                BuildBundleMonsters.BuildBundles(_tempObjs.ToArray());
        }
        else
        {
            Debug.LogWarning("Nothing selected!");
        }
    }

    void DrawBundleGroupForAnArray(BundleCheckItem[] _items, string _title)
    {
        DrawBundleGroupForAnArray(_items, _title, GUI.backgroundColor, GUI.contentColor);
    }

    void DrawBundleGroupForAnArray(BundleCheckItem[] _items, string _title, Color _bgC, Color _textC)
    {
        Color _tempBGC = GUI.backgroundColor;
        Color _tempTC = GUI.contentColor;

        GUI.backgroundColor = _bgC;
        GUI.contentColor = _textC;

        if (GUILayout.Button("Create!"))
        {
            BuildBundlesFromItems(_items);
        }

        GUI.backgroundColor = _tempBGC;
        GUI.contentColor = _tempTC;

        EditorGUILayout.BeginHorizontal();
        {
            GUI.contentColor = Color.green;
            if (GUILayout.Button("+", GUILayout.Width(15)))
            {
                if (_items != null && _items.Length > 0)
                {
                    foreach (BundleCheckItem _item in _items)
                    {
                        _item.IsChecked = true;
                    }
                }
            }
            GUI.contentColor = Color.red;
            if (GUILayout.Button("-", GUILayout.Width(15)))
            {
                foreach (BundleCheckItem _item in _items)
                {
                    _item.IsChecked = false;
                }
            }
            GUI.contentColor = _tempTC;

            int _num = 0;
            if (_items != null)
                _num = _items.Length;

            int _okNum = 0;

            if (_items != null && _items.Length > 0)
            {
                foreach (BundleCheckItem _item in _items)
                {
                    if (_item.IsChecked)
                        _okNum++;
                }
            }

            EditorGUILayout.HelpBox(_title + " (" + _okNum + "/" + _num + ")", MessageType.None);
        }
        EditorGUILayout.EndHorizontal();

        if (_items != null && _items.Length > 0)
        {
            foreach (BundleCheckItem _checkItem in _items)
            {
                DrawACheckBox(_checkItem);
            }
        }
    }

    void DrawACheckBox(BundleCheckItem _item)
    {
        EditorGUILayout.BeginHorizontal();
        {
            _item.IsChecked = EditorGUILayout.Toggle(_item.IsChecked);
            if (_item.BundleItem)
                EditorGUILayout.LabelField(_item.BundleItem.name, GUILayout.Width(180));
        }
        EditorGUILayout.EndHorizontal();
    }
}
