using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor (typeof (SpiritWerewolf))]
public class SpiritWerewolfEditor : NpcBaseEditor 
{
	protected override void OnSceneGUI () 
	{
		base.OnSceneGUI();
	}
}