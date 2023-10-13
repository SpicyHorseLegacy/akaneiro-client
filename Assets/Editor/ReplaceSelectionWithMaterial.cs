using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ReplaceSelectionWithMaterial : ScriptableWizard
{
	public Material[] material_list = new Material[1];
	
	[MenuItem("Custom/Replace Materials")]
	static void CreateWizard()
	{
		ScriptableWizard.DisplayWizard("Replace Selection with Materials", typeof(ReplaceSelectionWithMaterial), "Apply");		
	}
	
	void OnWizardUpdate()
	{
		helpString = "Replace Selection with materials.";
	}
	
	void OnWizardCreate()
	{
		if(material_list == null) return;
			
		//Debug.Log(AssetDatabase.GetAssetPath(material_list[0]));
		Transform[] objs = Selection.GetTransforms(SelectionMode.TopLevel);
		
		foreach(Transform obj in objs)
		{
			MeshRenderer render = obj.GetComponentInChildren<MeshRenderer>();

			for(int i = 0; i < render.sharedMaterials.Length;i++)
			{
				render.sharedMaterials = material_list;
			}
		}
	}
}

