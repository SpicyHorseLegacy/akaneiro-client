using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor (typeof (Wolf))]
public class WolfEditor : NpcBaseEditor 
{
	protected override void OnSceneGUI () 
	{
		base.OnSceneGUI();
	}
}