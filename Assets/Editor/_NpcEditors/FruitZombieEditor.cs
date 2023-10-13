using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor (typeof (FruitZombie))]
public class FruitZombieEditor : NpcBaseEditor 
{
	protected override void OnSceneGUI () 
	{
		base.OnSceneGUI();
	}
}