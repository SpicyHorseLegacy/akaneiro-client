using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor (typeof (Oni))]
public class OniEditor : NpcBaseEditor 
{
	protected override void OnSceneGUI () 
	{
		base.OnSceneGUI();
	}
}