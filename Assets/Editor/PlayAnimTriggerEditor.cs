using UnityEngine;
using UnityEditor;
using System.Collections;

[CustomEditor (typeof (PlayAnimTrigger))]
public class PlayAnimTriggerEditor : Editor {

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
		     if(s != null)
		   Handles.DrawLine(myTrigger.transform.position,s.position);	
		}
		
		Handles.color = Color.green;
		PlayAnimTrigger myAnimTrigger = target as PlayAnimTrigger;
		foreach( PlayAnimTrigger.AnimationDataStruct AnimData in myAnimTrigger.AnimDataArrays)
		{
			if(AnimData != null && AnimData.AnimObj)
			 Handles.DrawLine(myTrigger.transform.position,AnimData.AnimObj.transform.position);
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
		/*
	   GameObject theObj = (GameObject)target;
	   if( theObj != null && theObj.GetComponent<Lever>() != null)
		{
			theObj.GetComponent<Lever>().TriggerID = myTrigger.id;
		}
		*/
		
	}
	
	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI ();
		
	}
}
