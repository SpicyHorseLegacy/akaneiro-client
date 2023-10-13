using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

public class NewAbilityEnemySelectionWindow : EditorWindow
{
    static EnemyAbilityBuilder Root;

    List<NpcBase> abiEnemies = new List<NpcBase>();

    Vector2 scrollPosLeft;
	
	string SearchingString = "";

    static public void Init(EnemyAbilityBuilder _root)
    {
        // Get existing open window or if none, make a new one:
        NewAbilityEnemySelectionWindow window = (NewAbilityEnemySelectionWindow)EditorWindow.GetWindow(typeof(NewAbilityEnemySelectionWindow));
        Root = _root;
    }

    void OnGUI()
    {
        GUI.backgroundColor = Color.green;
        if (GUILayout.Button("Refresh"))
        {
            Refresh();
        }
        GUI.backgroundColor = Color.white;

		SearchingString = GUILayout.TextField(SearchingString);
		
        scrollPosLeft = EditorGUILayout.BeginScrollView(scrollPosLeft);
		
		List<NpcBase> filtedEnmeies = new List<NpcBase>();
		foreach (NpcBase abienemy in abiEnemies)
        {
			if(abienemy.NpcName.ToLower().Contains(SearchingString.ToLower()))
			{
				filtedEnmeies.Add(abienemy);
			}
		}
		
        foreach (NpcBase abienemy in filtedEnmeies)
        {
            if (GUILayout.Button(abienemy.NpcName))
            {
                Root.GetANewAbiEnemy(abienemy);
                Close();
            }
        }
        EditorGUILayout.EndScrollView();
    }

    void Refresh()
    {
        abiEnemies.Clear();

        string[] existingMonsters = new string[0];
		
		existingMonsters = Directory.GetFiles("Assets/Prefabs/CH/Enemies", "*.prefab", SearchOption.AllDirectories);

        foreach (string MonsterFile in existingMonsters)
        {
            NpcBase theMonster = AssetDatabase.LoadAssetAtPath(MonsterFile, typeof(NpcBase)) as NpcBase;

            if (theMonster && theMonster.abilityManager)
            {
                if (!theMonster.abilityManager.transform.GetComponent<NPCAbilityBaseState>())
                {
                    abiEnemies.Add(theMonster);
                }
            }
            else
            {
                //Debug.LogError("Name : " + theMonster.transform.name + " don't have ability manager!");
            }
        }
	}
}
