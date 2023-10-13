using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(NPC_Toss))]
public class EnemyAbility_Toss_Editor : NpcBaseAbility_Editor
{
    SerializedProperty ChargingVFXPrefab;
    SerializedProperty ThrowSoundPrefab;
    SerializedProperty ChargingHand;
    SerializedProperty ProjectilePrefab;

	public override void OnEnable()
	{
		base.OnEnable();
		ChargingVFXPrefab = serializedObject.FindProperty("ChargingVFXPrefab");
		ThrowSoundPrefab = serializedObject.FindProperty("ThrowSoundPrefab");
		ChargingHand = serializedObject.FindProperty("ChargingHand");
		ProjectilePrefab = serializedObject.FindProperty("ProjectilePrefab");
	}

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI();
	
		EditorGUILayout.PropertyField(ChargingVFXPrefab, new GUIContent("Charging VFX Prefab"));
		EditorGUILayout.PropertyField(ThrowSoundPrefab, new GUIContent("Throw Sound Prefab"));
	    ChargingHand.enumValueIndex = EditorGUILayout.Popup("Postion Type", ChargingHand.enumValueIndex, ChargingHand.enumNames);
	    EditorGUILayout.PropertyField(ProjectilePrefab, new GUIContent("Projectile Prefab"));
	
	    serializedObject.ApplyModifiedProperties();
	}
}
