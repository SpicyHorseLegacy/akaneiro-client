using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor (typeof (WolfLord))]
public class WolfLordEditor : NpcBaseEditor 
{
	protected override void OnSceneGUI () 
	{
		base.OnSceneGUI();
	}
}