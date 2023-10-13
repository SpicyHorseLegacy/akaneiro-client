using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor (typeof (WolfElite))]
public class WolfEliteEditor : NpcBaseEditor 
{
	protected override void OnSceneGUI () 
	{
		base.OnSceneGUI();
	}
}