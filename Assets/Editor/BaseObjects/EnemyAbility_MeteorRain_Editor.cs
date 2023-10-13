using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(NPC_MeteorRainState))]
public class EnemyAbility_MeteorRain_Editor : NpcBaseAbility_Editor
{
    SerializedProperty MeteorPrefab;

    public override void OnEnable()
    {
        base.OnEnable();
        MeteorPrefab = serializedObject.FindProperty("MeteorRainPrefab");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.PropertyField(MeteorPrefab, new GUIContent("Meteor Prefab"));

        serializedObject.ApplyModifiedProperties();
    }
}
