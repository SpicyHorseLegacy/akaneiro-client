using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class EnmeyAbilityConditionBuilderWindow : EditorWindow {
	
	static NPCAbilityBaseState RootAbi;
	
	List<NPCAbilityBaseState.AbilityCondition> abiConditions = new List<NPCAbilityBaseState.AbilityCondition>();
	
	static public void Init(NPCAbilityBaseState _root)
    {
        // Get existing open window or if none, make a new one:
        EnmeyAbilityConditionBuilderWindow window = (EnmeyAbilityConditionBuilderWindow)EditorWindow.GetWindow(typeof(EnmeyAbilityConditionBuilderWindow));
		RootAbi = _root;
		window.abiConditions.Clear();
		foreach(NPCAbilityBaseState.AbilityCondition _abicon in RootAbi.AbilityConditions)
		{
			window.abiConditions.Add(_abicon);
		}
    }
	
	void OnGUI()
    {
		GUI.backgroundColor = Color.green;
        if (GUILayout.Button("New Condition"))
        {
            abiConditions.Add(new NPCAbilityBaseState.AbilityCondition());
        }
        GUI.backgroundColor = Color.white;
		EditorGUILayout.Space();
		
		for(int i = abiConditions.Count -1; i >= 0; i --)
		{
			NPCAbilityBaseState.AbilityCondition _abicon = abiConditions[i];
			GUILayout.Label("Ability Condition : " + (i + 1));
			
			EditorGUILayout.BeginHorizontal();
			_abicon.AbiCondition = (NPCAbilityBaseState.AbilityCondition.AbiConditionEnum)EditorGUILayout.EnumPopup("Abi Conditon", _abicon.AbiCondition);
		    _abicon.Num = EditorGUILayout.FloatField("Num", _abicon.Num);
			if(GUILayout.Button("-", GUILayout.Width(50)))
			{
				abiConditions.RemoveAt(i);
			}
			EditorGUILayout.EndHorizontal();
		}
		
		EditorGUILayout.Space();
		
		if(GUILayout.Button("OK"))
		{
			RootAbi.AbilityConditions.Clear();
			foreach(NPCAbilityBaseState.AbilityCondition _abicon in abiConditions)
			{
				RootAbi.AbilityConditions.Add(_abicon);
			}
			Close();
		}
	}
    
}
