//Author:jiangwei
//date:2011-9-6
//static meshs need to be show or hidden

using UnityEngine;
using System.Collections;

public class StaticMeshState : MonoBehaviour {
	
	//public string levelState = "night";
	
	public string[] LevelStateList = {"s1"};
	
	Renderer[] mRenderers;
	
	Trigger[] mNpcTriggers;
	
	int repeatFlag = -1;

	// Use this for initialization
	void Start () {
		
		//mRenderers =   transform.GetComponentsInChildren<Renderer>();
		
		//mNpcTriggers = transform.GetComponentsInChildren<Trigger>();
	}
	
	// Update is called once per frame
	void Update () {
		
		
		bool binstate = false;
		
		for(int i = 0; i < LevelStateList.Length; i++)
		{
			if(GlobalGameState.state == LevelStateList[i])
			{
				binstate = true;
				break;
			}
		}
		if(!binstate)
		{
			if( repeatFlag != 0)
			{
			  // ChangeChildvis(transform,false);
			   ActiveAllchildren(transform,false);
			  
			   repeatFlag = 0;
			}
		}
		else
		{
			if( repeatFlag != 1)
			{
			   //ChangeChildvis(transform,true);
			   ActiveAllchildren(transform,true);
			   
			   repeatFlag = 1;
			}
		}
	}
	
	
	void ActiveAllchildren(Transform tsm,bool bshow)
	{
	   // BroadcastMessage("OtherThingsShow",bshow,SendMessageOptions.DontRequireReceiver);
		tsm.gameObject.SetActiveRecursively(bshow);
	
		tsm.gameObject.active = true;
		
		MonoBehaviour[] MyComponents = transform.GetComponents<MonoBehaviour>();
		foreach( MonoBehaviour eachComp in MyComponents)
		{
			if( eachComp.GetType() == typeof(StaticMeshState))
			     continue;
			
			eachComp.enabled = bshow;
				
		}
		if(tsm.renderer)
		   tsm.renderer.enabled = bshow;
		
	}
	
	void ChangeChildvis(Transform tsm,bool bshow)
	{
		if(mRenderers == null)
		   return;
			
		Collider pCollider =  null;
	    
		foreach(Renderer rd in mRenderers)
		{
			rd.enabled = bshow;
			pCollider = rd.transform.collider;
			if( pCollider)
				
			{
				if( bshow )
					pCollider.isTrigger = false;
				else
					pCollider.isTrigger = true;
			}	
			
		}
		
		
		if( mNpcTriggers == null)
			return;
	
		foreach(Trigger tg in mNpcTriggers)
		{
			if(tg.transform.renderer)
				tg.transform.renderer.enabled = false;
			
			if(tg.transform.collider)
				tg.transform.collider.isTrigger = true;
			
		    tg.TriggerOn = bshow;
			
		}
		
	}
}
