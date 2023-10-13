using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class CreateAKABundlesInFolderSelection : EditorWindow {

    List<string> Folders = new List<string>();

    [MenuItem("Bundle Tools/Create AKA model Bundle")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        CreateAKABundlesInFolderSelection window = (CreateAKABundlesInFolderSelection)EditorWindow.GetWindow(typeof(CreateAKABundlesInFolderSelection));
    }

    void OnGUI()
    {
        GUI.backgroundColor = Color.red;
        if (GUILayout.Button("CleanALL"))
        {
            Folders.Clear();
        }
        GUI.backgroundColor = Color.white;
        EditorGUILayout.Space();

        for (int i = Folders.Count-1; i >=0 ; i--)
        {
            string _folder = Folders[i];
            EditorGUILayout.BeginHorizontal();
            Folders[i] = GUILayout.TextField(_folder);
            if (GUILayout.Button("-", GUILayout.Width(30)))
            {
                Folders.RemoveAt(i);
            }
            EditorGUILayout.EndHorizontal();
        }
        if (GUILayout.Button("+ new"))
        {
            Folders.Add("");
        }

        EditorGUILayout.Space();
        if (GUILayout.Button("Create!!!!"))
        {
            List<string> tempFolders = new List<string>();
            for (int i = Folders.Count - 1; i >= 0; i--)
            {
                string _folder = Folders[i];
                tempFolders.Add("Assets/Model/Character/Avatar/CH_AKA_" + _folder);
            }

            BuildBundleAkaneiro.BuildBundlesFromFolders(tempFolders.ToArray());
        }
    }

}
