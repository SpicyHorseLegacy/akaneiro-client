using System;
using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

//Trigger
[CustomEditor (typeof (Jubokko))]
public class JubokkoEditor : NpcBaseEditor
{
    protected override void OnSceneGUI () 
	{
		base.OnSceneGUI();
	}
}
