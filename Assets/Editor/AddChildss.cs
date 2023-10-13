using UnityEngine;
using UnityEditor;
using System.Collections;

public class AddChildss : ScriptableObject
{
    [MenuItem ("GameObject/Add Child ^n")]
    static void MenuAddChild()
    {
        Transform[] transforms = Selection.GetTransforms(SelectionMode.TopLevel | SelectionMode.OnlyUserModifiable);

        foreach(Transform transform in transforms)
        {
            GameObject newChild = new GameObject("_Child");
            newChild.transform.parent = transform;
			newChild.transform.localPosition = Vector3.zero;
            newChild.layer = transform.gameObject.layer;
			Selection.activeGameObject = newChild;
        }
    }

    [MenuItem("Edit/Extra Settings/Show MT Rendering Status")]
    static void ShowMTRenderingStatus()
    {

        UnityEditor.EditorUtility.DisplayDialog("MT Rendering Status",

            (UnityEditor.PlayerSettings.MTRendering)

            ? "Multi-Threading Rendering is currently ON"

            : "Multi-Threading Rendering is currently OFF"

            , "OK");

    }

    [MenuItem("Edit/Extra Settings/Enable MT Rendering")]
    static void EnableMTRendering()
    {

        UnityEditor.PlayerSettings.MTRendering = true;

    }



    [MenuItem("Edit/Extra Settings/Disable MT Rendering")]
    static void DisableMTRendering()
    {

        UnityEditor.PlayerSettings.MTRendering = false;

    }
}
 