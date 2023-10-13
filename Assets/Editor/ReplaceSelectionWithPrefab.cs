using UnityEngine;
using UnityEditor;
using System.Collections;
 

public class ReplaceSelectionWithPrefab : ScriptableWizard
{
    public GameObject prefab;
    public bool includeChildren = false;
    public bool excludePrefabs = false;
    public bool deleteSelectedObjects = true;
    public bool copyPosition = true;
    public bool copyRotation = true;
    public bool copyScale = true;
    private SelectionMode modePrefs;

    [MenuItem("Custom/Replace Selection with Prefab %r")]
    static void DoSet()
    {
        ScriptableWizard.DisplayWizard("Replace Selection with Prefab", typeof(ReplaceSelectionWithPrefab), "Set");
    }

    void OnWizardUpdate()
    {
        helpString = "Duplicate the selected prefab and place it around the scene to the position of the selected objects. Tick Include Children to also place the Prefab at the position of the children of selected objects or tick Exclude Prefabs if you don't want instanced objects (from prefabs) to be changed. Tick DeleteSelectedObjects to delete the original selection after placing the prefab. The new objects are nicely placed in the same hierarchy as the old ones.";
    }

    void OnWizardCreate()
    {
        if (includeChildren || excludePrefabs)
            modePrefs = (SelectionMode.ExcludePrefab | SelectionMode.Editable | SelectionMode.Deep);
        else
            modePrefs = (SelectionMode.Editable);

        Object[] objs = Selection.GetFiltered(typeof(GameObject), modePrefs);

        foreach (GameObject go in objs)
        {
            GameObject clone = EditorUtility.InstantiatePrefab(prefab) as GameObject;
            clone.name = prefab.name;
            clone.transform.parent = go.transform.parent;
            if (copyPosition)
            {
                clone.transform.localPosition = go.transform.localPosition;
            }

            if (copyRotation)
            {
                clone.transform.localRotation = go.transform.localRotation;
            }

            if (copyScale)
            {
                clone.transform.localScale = go.transform.localScale;
            }
        }

        if (deleteSelectedObjects)
        {
            foreach (GameObject go in objs)
            {
                DestroyImmediate(go.gameObject);
            }
        }
    }

}