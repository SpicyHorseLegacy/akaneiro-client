using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor (typeof (WerewolfRanged))]
public class WerewolfRangedEditor : NpcBaseEditor 
{
	protected override void OnSceneGUI () 
	{
		base.OnSceneGUI();
	}
}