using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(NPC_Slow))]
public class EnemyAbility_Slow_Editor : NpcBaseAbility_Editor
{

    SerializedProperty SlowDownVFXPrefab;

    public override void OnEnable()
    {
        base.OnEnable();
        SlowDownVFXPrefab = serializedObject.FindProperty("SlowDownVFXPrefab");
    }

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EditorGUILayout.PropertyField(SlowDownVFXPrefab, new GUIContent("SlowDown VFX Prefab"));

        serializedObject.ApplyModifiedProperties();
    }
}

