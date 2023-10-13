using System;
using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

[CustomEditor (typeof (SelfAnimation))]
public class SelfAnimationEditor : Editor {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void OnSceneGUI () 
	{
		//Debug.Log("Begin OnSceneGUI");
		/*
		SelfAnimation myTarget = target as SelfAnimation;
		
		if(Application.isPlaying)
				return;
	    
		if( myTarget.id == 0)
		{
			SelfAnimation[]  TriggerList  = FindObjectsOfType(typeof(SelfAnimation)) as SelfAnimation[];
			
			int maxID = 0;
			
			foreach( SelfAnimation it in TriggerList)
			{
				if( it.id > maxID )
					maxID = it.id;
			}
			
			myTarget.id = ++maxID;
			
			EditorUtility.SetDirty (myTarget);
		}
		else
		{
		    SelfAnimation[]  TriggerList  = FindObjectsOfType(typeof(SelfAnimation)) as SelfAnimation[];
			
			bool bSame = false;
		    
			foreach( SelfAnimation it in TriggerList)
			{
				if( it != myTarget && it.id == myTarget.id)
				{
				   	bSame = true;
					break;
				}
			}
			
			if(bSame)
			{
			   int maxID = 0;
			
			   foreach( SelfAnimation it in TriggerList)
			   {
				 if( it.id > maxID )
					maxID = it.id;
			   }
				
			   myTarget.id = ++maxID;
				
			   EditorUtility.SetDirty (myTarget);
			   	
			}	
		}  
		*/
		           
	}
	
	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI ();
		
	}
}
