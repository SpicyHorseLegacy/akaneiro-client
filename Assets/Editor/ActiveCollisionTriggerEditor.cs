using System;
using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

//Trigger
[CustomEditor (typeof (ActiveCollisionTrigger))]
public class ActiveCollisionTriggerEditor : Editor {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void OnSceneGUI () 
	{
		//Debug.Log("Begin OnSceneGUI");
		TriggerBase myTrigger = target as TriggerBase;
		
		Handles.color = Color.red;
		
		foreach(Transform s in myTrigger.TriggerNpcList)
		{
			if( s != null)
		       Handles.DrawLine(myTrigger.transform.position,s.position);	
		}
		
		if( myTrigger.id == 0)
		{
			TriggerBase[]  TriggerList  = FindObjectsOfType(typeof(TriggerBase)) as TriggerBase[];
			
			int maxID = 0;
			
			foreach( TriggerBase it in TriggerList)
			{
				if( it.id > maxID )
					maxID = it.id;
			}
			
			myTrigger.id = ++maxID;
		}
		else
		{
		    TriggerBase[]  TriggerList  = FindObjectsOfType(typeof(TriggerBase)) as TriggerBase[];
			
			bool bSame = false;
		    
			foreach( TriggerBase it in TriggerList)
			{
				if( it != myTrigger && it.id == myTrigger.id)
				{
				   	bSame = true;
					break;
				}
			}
			
			if(bSame)
			{
			   int maxID = 0;
			
			   foreach( TriggerBase it in TriggerList)
			   {
				 if( it.id > maxID )
					maxID = it.id;
			   }
				
			   myTrigger.id = ++maxID;
			   	
			}	
		}               
	}
	
	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI ();
		
	}
}
