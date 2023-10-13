using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class CreateMonsterBundlesTool : EditorWindow
{
    List<Transform> NPCS = new List<Transform>();

    private bool collapsed;
    private int arraySize;

    [MenuItem("Bundle Tools/Create Single Monster Bundle")]
    static void Init()
    {
        // Get existing open window or if none, make a new one:
        CreateMonsterBundlesTool window = (CreateMonsterBundlesTool)EditorWindow.GetWindow(typeof(CreateMonsterBundlesTool));
    }

    void OnGUI()
    {
        GUI.backgroundColor = Color.red;
        if (GUILayout.Button("CleanALL"))
        {
            NPCS.Clear();
        }
        GUI.backgroundColor = Color.white;
        EditorGUILayout.Space();

        for (int i = NPCS.Count - 1; i >= 0; i--)
        {
            Transform _npc = NPCS[i];
            EditorGUILayout.BeginHorizontal();
            NPCS[i] = EditorGUILayout.ObjectField("" + i, _npc, typeof(Transform)) as Transform;
            if (GUILayout.Button("-", GUILayout.Width(30)))
            {
                NPCS.RemoveAt(i);
            }
            EditorGUILayout.EndHorizontal();
        }

        EditorGUILayout.Space();

        if(GUILayout.Button("Add One"))
        {
            NPCS.Add(null);
        }

        EditorGUILayout.Space();

        if (GUILayout.Button("Create!!!!"))
        {
            List<Transform> tempFolders = new List<Transform>();
            BuildBundleMonsters.MonsterBundlePrepare();
            for (int i = NPCS.Count - 1; i >= 0; i--)
            {
                //tempFolders.Add(NPCS[i].transform);
                BuildBundleMonsters.BuildTargetObject(NPCS[i].transform.gameObject);
            }
            BuildBundleMonsters.MonsterBundleEnd();
            //CreateAllMonsterbundles.BuildMonsters(tempFolders.ToArray());
        }
    }
}
