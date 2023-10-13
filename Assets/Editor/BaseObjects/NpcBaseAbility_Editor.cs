using UnityEngine;
using UnityEditor;
using System.Collections;

public class NpcBaseAbility_Editor : DamageSource_Editor {

    SerializedProperty AbilityID;
    SerializedProperty PosType;
    SerializedProperty PositionOffset;
    SerializedProperty AbilityCoolDown;
    SerializedProperty AbilityCoolDownDif;

    SerializedProperty CastAnimation;
    SerializedProperty CastSound;

    SerializedProperty AbilityImpactVFX;
    SerializedProperty AbilityImpactPosition;
    SerializedProperty AbilityImpactSound;

	SerializedProperty AbilityConditions;

    public override void OnEnable()
    {
        base.OnEnable();

        AbilityID = serializedObject.FindProperty("id");
        PosType = serializedObject.FindProperty("PosType");
        PositionOffset = serializedObject.FindProperty("PositionOffset");
        AbilityCoolDown = serializedObject.FindProperty("AbilityCoolDown");
        AbilityCoolDownDif = serializedObject.FindProperty("AbilityCoolDownDif");
        CastAnimation = serializedObject.FindProperty("CastAnimation");
        CastSound = serializedObject.FindProperty("CastSoundPrefab");
        AbilityImpactVFX = serializedObject.FindProperty("AbilityImpactVFXPrefab");
        AbilityImpactPosition = serializedObject.FindProperty("AbilityImpactPosition");
        AbilityImpactSound = serializedObject.FindProperty("AbilityImpactSoundPrefab");

	}

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.Space();

        AbilityID.intValue = EditorGUILayout.IntField("Ability ID", AbilityID.intValue);
        if (AbilityID.intValue == 0)
        {
            EditorGUILayout.HelpBox("You should set a ID for this Ability!", MessageType.Error);
        }

        AbilityCoolDown.floatValue = EditorGUILayout.FloatField("Ability CoolDown", AbilityCoolDown.floatValue);
        AbilityCoolDownDif.floatValue = EditorGUILayout.FloatField("Ability CoolDown Dif", AbilityCoolDownDif.floatValue);
		
		NPCAbilityBaseState _abi = (NPCAbilityBaseState)target;
		EditorGUILayout.BeginHorizontal();
			EditorGUILayout.BeginHorizontal("box");
			string _abiConditionString = "";
			for(int i = 0 ; i < _abi.AbilityConditions.Count; i++ )
			{
				NPCAbilityBaseState.AbilityCondition _abicon = _abi.AbilityConditions[i];
				_abiConditionString += "[" + (i+1) + "] " + _abicon.AbiCondition.ToString() +  " || " + _abicon.Num;
				if(i != _abi.AbilityConditions.Count - 1)
				_abiConditionString += "\n";
			}
			if(_abiConditionString == "")
				_abiConditionString = "No Condition";
			GUILayout.Label(_abiConditionString);
			EditorGUILayout.EndHorizontal();
		if(GUILayout.Button("+", GUILayout.Width(50)))
		{
			EnmeyAbilityConditionBuilderWindow.Init(_abi);
		}
		EditorGUILayout.EndHorizontal();

        PosType.enumValueIndex = EditorGUILayout.Popup("Postion Type", PosType.enumValueIndex, PosType.enumNames);
        if (PosType.intValue == (int)NPCAbilityBaseState.AbilityPositionType.CurPosition
            || PosType.intValue == (int)NPCAbilityBaseState.AbilityPositionType.PlayerPosition)
        {
            PositionOffset.vector2Value = EditorGUILayout.Vector2Field("Offset", PositionOffset.vector2Value);
        }

        EditorGUILayout.Space();

        EditorGUILayout.PropertyField(CastAnimation, new GUIContent("Cast animation", "cast animation tooltip"));
        if (CastAnimation.objectReferenceValue == null)
        {
            EditorGUILayout.HelpBox("No animation!", MessageType.Error);
        }
        EditorGUILayout.PropertyField(CastSound, new GUIContent("Cast sound"));
        EditorGUILayout.PropertyField(AbilityImpactVFX, new GUIContent("Impact VFX Prefab"));
        EditorGUILayout.PropertyField(AbilityImpactPosition, new GUIContent("Ability Impact Position"));
        EditorGUILayout.PropertyField(AbilityImpactSound, new GUIContent("Ability Impact Sound Prefab"));

        EditorGUILayout.Space();
		
		serializedObject.ApplyModifiedProperties();
    }
	
    void ArrayGUI(SerializedObject obj, string name)
    {
        int size = obj.FindProperty(name + ".Array.size").intValue;
 
		EditorGUI.indentLevel = 1;
		
        int newSize = EditorGUILayout.IntField(name + " Size", size);
       
        if (newSize != size)
            obj.FindProperty(name + ".Array.size").intValue = newSize;
       
        EditorGUI.indentLevel = 1;
 
        for (int i=0;i<newSize;i++)
        {
			EditorGUILayout.BeginHorizontal();
            var prop1 = obj.FindProperty(string.Format("{0}.Array.data[{1}].AbiCondition", name, i));
	        prop1.enumValueIndex = EditorGUILayout.Popup("Condition" + i, prop1.enumValueIndex, prop1.enumNames);
			var prop2 = obj.FindProperty(string.Format("{0}.Array.data[{1}].Num", name, i));
	        prop2.floatValue = EditorGUILayout.FloatField("Num", prop2.floatValue);
			EditorGUILayout.EndHorizontal();
        }
		
		EditorGUI.indentLevel = -1;
    }
}
