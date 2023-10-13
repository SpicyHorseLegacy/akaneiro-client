using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor(typeof(NPC_AbilitiesBuilder))]
public class EnemyAbilityBuilderEditor : Editor
{
    string[] options = { "NPC_MeteorRainState", "NPC_ShockWaveState", "NPC_Slow", "NPC_Toss", "NPC_RainOfBlow", "NPC_WhirleWind", "NPC_SkyStrike", "NPC_AreaHoT" };
    int index = 0;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        Rect rect = GUILayoutUtility.GetRect(18, 18, "TextField");
        index = EditorGUI.Popup(
            rect,
            "Component:",
            index,
            options);

        if (GUILayout.Button("Add Component"))
            AddComponentToObjects();
    }

    void AddComponentToObjects()
    {
        if (!Selection.activeGameObject)
        {
            Debug.LogError("Please select at least one GameObject first");
            return;
        }
        foreach (GameObject go in Selection.gameObjects)
        {
            if (!go.GetComponent(options[index]))
                go.AddComponent(options[index]);
        }
    }
}
