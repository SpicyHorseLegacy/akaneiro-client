using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(DamageSource))]
public class DamageSource_Editor : Editor {

    SerializedProperty IsImpactSFX;
    SerializedProperty ImpactSFX;

    SerializedProperty IsImpactVFX;
    SerializedProperty ImpactVFX;

    public virtual void OnEnable()
    {
        IsImpactSFX = serializedObject.FindProperty("IsPlayImpactSound");
        ImpactSFX = serializedObject.FindProperty("ImpactSoundPrefab");
        IsImpactVFX = serializedObject.FindProperty("IsPlayImpactVFX");
        ImpactVFX = serializedObject.FindProperty("ImpactVFXPrefab");
    }

    public override void OnInspectorGUI()
    {
        //base.OnInspectorGUI();
        IsImpactSFX.boolValue = EditorGUILayout.Toggle("Is Play Impact SFX", IsImpactSFX.boolValue);

        if (IsImpactSFX.boolValue)
            EditorGUILayout.PropertyField(ImpactSFX, new GUIContent("Impact SFX"));

        IsImpactVFX.boolValue = EditorGUILayout.Toggle("Is Play Impact VFX", IsImpactVFX.boolValue);

        if (IsImpactVFX.boolValue)
            EditorGUILayout.PropertyField(ImpactVFX, new GUIContent("Impact VFX"));
    }

}
