using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class CreateSingleItemBundle : EditorWindow
{
    List<GameObject> Items = new List<GameObject>();

	[MenuItem("Bundle Tools/Creat Single Item Bundle")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        CreateSingleItemBundle window = (CreateSingleItemBundle)EditorWindow.GetWindow(typeof(CreateSingleItemBundle));
    }

    void OnGUI()
    {
        GUI.backgroundColor = Color.red;
        if (GUILayout.Button("CleanALL"))
        {
            Items.Clear();
        }
        GUI.backgroundColor = Color.white;
        EditorGUILayout.Space();

        for (int i = Items.Count - 1; i >= 0; i--)
        {
            GameObject _gameobj = Items[i];
            EditorGUILayout.BeginHorizontal();
            Items[i] = EditorGUILayout.ObjectField("item : " + i, Items[i], typeof(GameObject)) as GameObject;
            if (GUILayout.Button("-", GUILayout.Width(30)))
            {
                Items.RemoveAt(i);
            }
            EditorGUILayout.EndHorizontal();
        }
        if (GUILayout.Button("+ new"))
        {
            Items.Add(null);
        }

        EditorGUILayout.Space();
        if (GUILayout.Button("Create!!!!"))
        {
			BuildBundleItems.mapCachedItems.Clear();
            foreach(GameObject _item in Items)
                BuildBundleItems.CreateSingleItemBundle(_item);
        }
    }
}
