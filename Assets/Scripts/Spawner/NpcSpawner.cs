using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NpcSpawner : BaseExportNode {
	
	public enum EStateType
	{
		Idle=0,
		Wander,
		Attack,
		Sleep,
		Spawn,
	}
	
	[System.Serializable]
	public class CStateChange
	{
		public bool OverrideNpcStateChange = true;
		public int AttackOnEnemySightChance=90;
		public int AttackOnDamageChance=100; 
		public int AttackOnAlertChance=100;
		public int WanderAfterAttackChance=0;
		public int WanderWhenOutOfRangeChance=0;
		//public int WanderWhenOutOfVisionChance = 0;
		
	}
	
	[System.Serializable]
	public class Spawner
	{
		public bool IsRandomSpawnNpc=false;
		//public Transform NpcPrefab;
		public Transform[] NpcPrefabArray;
		public Transform[] SpawnLocation;
		public Transform[] WanderWaypoint;
		public EStateType InitialState = EStateType.Idle;
		public float SpawnDelay = 0f;
		public int   SpawnNpcCount=1;
		public float SpawnDelayPerNpc = 0f;
		public int   SpawnRepeatCount = 1;
		
		public bool  BNodeCreate = false;
		public string[]  NodeTags = {"Socket"};
		
		public bool  Bcircle = true;
		public float SpawnArea=0f;
	    public float SpawnWidth = 5f;
		public float SpawnHeight = 5f;
	    public float SpawnRotation = 0;
		
		public float WanderRadius = 5f;
		public float MovableRadius = 30f;
		
		public int SpawnAnimIndex=0;
		
		public bool bColliderWithArea = false;
		
		public CStateChange StateChangeOverride;
			
	}
	public List<Spawner> SpawnerList;
	
	[System.Serializable]
	public class SpawnCondition
	{
	   public int KillCondition = 0;
	   public float DelayTime=0f;
	   public Transform Spawner;
	}
	
	[System.Serializable]
	public class TriggerCondition
	{
	   public int KillCondition = 0;
	   public float DelayTime=0f;
	   public Transform trigger;
	}
	
	[System.Serializable]
	public class TriggerOtherSpawner
	{
		//public int KilledNpcCount=100;
		
		public bool LoopingSpawner = false;
		public int loopCount = 0;
		
	
		public List<SpawnCondition> EnableSpawnerList;
		public List<SpawnCondition> DisableSpawnerList;
		public List<TriggerCondition> AutoTriggerList;
		public List<TriggerCondition> ToggleTriggerList;
	}
	public TriggerOtherSpawner LinkedEvents;
	
	[HideInInspector]
	public int KilledNpcCount=0;
	

	Vector3 angle = Vector3.zero;
	float SpawnOffset=0f;
	
	public int id = 0;
	
    public bool bNone = false;
	
	public bool bLow = true;
	
	public bool bNormal = false;
	
	public bool bHard = false;

	// Use this for initialization
	void Start ()    
	{
	
	}
	
	public override string DoExport()
	{
		XMLStringWriter xmlWriter = new XMLStringWriter();
		
		xmlWriter.NodeBegin("MonsterGroup");

		xmlWriter.AddAttribute("id", id);
		
		int difficult = 1;
		
		if(bNone)
		   difficult |= (1 << 1);
	
		if(bLow)
		   difficult |= (1 << 2);
		
		if(bNormal)
		   difficult |= (1 << 3);
		
		if(bHard)
		   difficult |= (1 << 4);
		
		xmlWriter.AddAttribute("difficulty",difficult);
		
		foreach(Spawner s in SpawnerList)
		{
			xmlWriter.NodeBegin("Monster");
		
		    xmlWriter.AddAttribute("IsRandom", s.IsRandomSpawnNpc);
			
			xmlWriter.AddAttribute("SpawnRadius",s.SpawnArea);
			
			xmlWriter.AddAttribute("IsCircleSpawn",s.Bcircle);
			
			xmlWriter.AddAttribute("SpawnCount",s.SpawnNpcCount);
			
			xmlWriter.AddAttribute("SpawnDelayTime",s.SpawnDelay);
			
			xmlWriter.AddAttribute("SpawnDelayPernpc",s.SpawnDelayPerNpc);
			
			xmlWriter.AddAttribute("SpawnRepeatCount",s.SpawnRepeatCount);
			
			xmlWriter.AddAttribute("InitialState",s.InitialState);
			
			xmlWriter.AddAttribute("WanderRadius",s.WanderRadius);
			
			xmlWriter.AddAttribute("MovableRadius",s.MovableRadius);
			
			xmlWriter.AddAttribute("IfOverrideNpcState",s.StateChangeOverride.OverrideNpcStateChange);
			
			xmlWriter.AddAttribute("AttackOnEnemySightChance",s.StateChangeOverride.AttackOnEnemySightChance);
			
			xmlWriter.AddAttribute("AttackOnDamageChance",s.StateChangeOverride.AttackOnDamageChance);
			
			xmlWriter.AddAttribute("AttackOnAlertChance",s.StateChangeOverride.AttackOnAlertChance);
			
			xmlWriter.AddAttribute("WanderAfterAttackChance",s.StateChangeOverride.WanderAfterAttackChance);
			
			xmlWriter.AddAttribute("WanderOutOfRangeChance",s.StateChangeOverride.WanderWhenOutOfRangeChance);
			
			xmlWriter.AddAttribute("WanderOutOfVisionChance",0);//s.StateChangeOverride.WanderWhenOutOfVisionChance);
			
			xmlWriter.AddAttribute("SpawnAnimIndex",s.SpawnAnimIndex);
			
			xmlWriter.AddAttribute("ColliderWithArea",s.bColliderWithArea);
			
			if(!s.Bcircle && (s.SpawnHeight > 0 ||  s.SpawnWidth > 0))
			{
				Vector3 EdgePoint = GetHorizontalVector(s);
				xmlWriter.NodeBegin("HorizonVector");
				xmlWriter.AddAttribute("X",EdgePoint.x);
				xmlWriter.AddAttribute("Y",EdgePoint.y);
				xmlWriter.AddAttribute("Z",EdgePoint.z);
				xmlWriter.NodeEnd("HorizonVector");
				Vector3 EdgePoint2 = GetVerticalVector(s);
				xmlWriter.NodeBegin("VerticalVector");
				xmlWriter.AddAttribute("X",EdgePoint2.x);
				xmlWriter.AddAttribute("Y",EdgePoint2.y);
				xmlWriter.AddAttribute("Z",EdgePoint2.z);
				xmlWriter.NodeEnd("VerticalVector");
				
			}
			
		    if( s.NpcPrefabArray.Length > 0)
			{
			  xmlWriter.NodeBegin("SpawnIDs");
		      for(int i = 0; i < s.NpcPrefabArray.Length;i++)
		      {	
				  if( s.NpcPrefabArray[i] == null)
						continue;
					
				  NpcBase theMonster = s.NpcPrefabArray[i].GetComponent<NpcBase>();
				  if(theMonster)
				  {
					  xmlWriter.NodeBegin("SpawnID");
						
					  xmlWriter.AddAttribute("id",theMonster.TypeID);
						
					  xmlWriter.NodeEnd("SpawnID");
				  }
				   
		      }
		      xmlWriter.NodeEnd("SpawnIDs");
			}
			
			
			if( s.SpawnLocation.Length > 0)
			{
			   xmlWriter.NodeBegin("SpawnLocations");
			   for(int i = 0; i < s.SpawnLocation.Length;i++)
			   {
				 xmlWriter.NodeBegin("Point");
					Debug.Log(s.SpawnLocation[i].name);
				 xmlWriter.AddAttribute("X",s.SpawnLocation[i].position.x);
				 xmlWriter.AddAttribute("Y",s.SpawnLocation[i].position.y);
				 xmlWriter.AddAttribute("Z",s.SpawnLocation[i].position.z);
				 xmlWriter.NodeEnd("Point");
				
			   }
			   xmlWriter.NodeEnd("SpawnLocations");
			}
			
			if(s.WanderWaypoint.Length > 0)
			{
			   xmlWriter.NodeBegin("WanderWaypoints");
			
		       for(int i = 0; i < s.WanderWaypoint.Length;i++)
			   {
				  xmlWriter.NodeBegin("Point");
				  xmlWriter.AddAttribute("X",s.WanderWaypoint[i].position.x);
				  xmlWriter.AddAttribute("Y",s.WanderWaypoint[i].position.y);
				  xmlWriter.AddAttribute("Z",s.WanderWaypoint[i].position.z);
				  xmlWriter.NodeEnd("Point");
			   }
				
			   xmlWriter.NodeEnd("WanderWaypoints");
			}
			
		
			xmlWriter.NodeEnd("Monster");
		}
		
		xmlWriter.NodeBegin("LinkedEvents");
		
		xmlWriter.AddAttribute("LoopingSpawner",LinkedEvents.LoopingSpawner);
		
		xmlWriter.AddAttribute("LoopCount",LinkedEvents.loopCount);
		
		if( LinkedEvents.EnableSpawnerList.Count > 0)
		{
			xmlWriter.NodeBegin("EnableSpawnList");
			
			foreach( SpawnCondition it in LinkedEvents.EnableSpawnerList)
			{
				xmlWriter.NodeBegin("EnableSpawner");
				xmlWriter.AddAttribute("KillCount",it.KillCondition);
				xmlWriter.AddAttribute("DelayTime",it.DelayTime);
				if( it.Spawner != null && it.Spawner.GetComponent<NpcSpawner>())
				   xmlWriter.AddAttribute("MonsterGroupID",it.Spawner.GetComponent<NpcSpawner>().id);
				else
				   xmlWriter.AddAttribute("MonsterGroupID",-1);
				
				xmlWriter.NodeEnd("EnableSpawner");
			}
			xmlWriter.NodeEnd("EnableSpawnList");
		}
		
		if( LinkedEvents.DisableSpawnerList.Count > 0 )
		{
			xmlWriter.NodeBegin("DisableSpawnList");
			
			foreach( SpawnCondition it in LinkedEvents.DisableSpawnerList)
			{
				xmlWriter.NodeBegin("DisableSpawner");
				
				xmlWriter.AddAttribute("KillCount",it.KillCondition);
				xmlWriter.AddAttribute("DelayTime",it.DelayTime);
				
				if( it.Spawner != null && it.Spawner.GetComponent<NpcSpawner>())
				   xmlWriter.AddAttribute("MonsterGroupID",it.Spawner.GetComponent<NpcSpawner>().id);
				else
				   xmlWriter.AddAttribute("MonsterGroupID",-1);
				
				xmlWriter.NodeEnd("DisableSpawner");
				
			}
		    
			xmlWriter.NodeEnd("DisableSpawnList");
		}
		
		if( LinkedEvents.AutoTriggerList.Count > 0)
		{
			xmlWriter.NodeBegin("AutoTriggerList");
			
			foreach( TriggerCondition it in LinkedEvents.AutoTriggerList)
			{
				xmlWriter.NodeBegin("AutoTrigger");
				
				xmlWriter.AddAttribute("KillCount",it.KillCondition);
				xmlWriter.AddAttribute("DelayTime",it.DelayTime);
				
				if( it.trigger != null && it.trigger.GetComponent<TriggerBase>() )
				{
				   xmlWriter.AddAttribute("TriggerID",it.trigger.GetComponent<TriggerBase>().id);
				}
				else if(it.trigger != null && it.trigger.GetComponent<Trigger_Unified>())
				{
					xmlWriter.AddAttribute("TriggerID",it.trigger.GetComponent<Trigger_Unified>().UnityID);
				}
				else
				{
				   xmlWriter.AddAttribute("TriggerID",-1);
				}
				
				xmlWriter.NodeEnd("AutoTrigger");
				
			}
			
			xmlWriter.NodeEnd("AutoTriggerList");
		}
		
		if( LinkedEvents.ToggleTriggerList.Count > 0)
		{
			xmlWriter.NodeBegin("ToggleTriggerList");
			
			foreach( TriggerCondition it in LinkedEvents.ToggleTriggerList)
			{
				xmlWriter.NodeBegin("ToggleTrigger");
				
				xmlWriter.AddAttribute("KillCount",it.KillCondition);
				xmlWriter.AddAttribute("DelayTime",it.DelayTime);
				
				if( it.trigger != null && it.trigger.GetComponent<TriggerBase>())
				{
				   xmlWriter.AddAttribute("TriggerID",it.trigger.GetComponent<TriggerBase>().id);
				}
				else if(it.trigger != null && it.trigger.GetComponent<Trigger_Unified>())
				{
					xmlWriter.AddAttribute("TriggerID",it.trigger.GetComponent<Trigger_Unified>().UnityID);
				}
				else
				{
				   xmlWriter.AddAttribute("TriggerID",-1);
				}
				
				xmlWriter.NodeEnd("ToggleTrigger");
				
			}
			
			xmlWriter.NodeEnd("ToggleTriggerList");
		}
		
		xmlWriter.NodeEnd("LinkedEvents");
		
		xmlWriter.NodeEnd("MonsterGroup");
	
		return xmlWriter.Result;
	}
	
	public override string GetNodeType()
	{
		string output = "MonsterSpawner";
		return output;
	}
	
	Vector3 GetHorizontalVector( Spawner s)
	{
	   Vector3 result = Vector3.zero;
	   
	   Vector3 tempOffset = Vector3.zero;
					
	   float ax = s.SpawnWidth/2 * (-1);
								  
	   tempOffset.x = ax * Mathf.Cos(s.SpawnRotation);
					
	   tempOffset.z = ax * Mathf.Sin(s.SpawnRotation);
					
	   Vector3 LeftUpperCorn = tempOffset;
					
	   ax = s.SpawnWidth/2;
							  
	   tempOffset.x = ax * Mathf.Cos(s.SpawnRotation);
					
	   tempOffset.z = ax * Mathf.Sin(s.SpawnRotation);
					
	   Vector3 RightUpperCorn =  tempOffset;
		
	   result = (RightUpperCorn - LeftUpperCorn) * 0.5f;
	   
	   return  result;
	}
	
	Vector3 GetVerticalVector(Spawner s)
	{
	   Vector3 result = Vector3.zero;
	   
	   Vector3 tempOffset = Vector3.zero;
		 									
	   float az = s.SpawnHeight/2 * (-1);
				  
	   tempOffset.x = az * Mathf.Sin(s.SpawnRotation) * (-1);
					
	   tempOffset.z = az * Mathf.Cos(s.SpawnRotation);
					
	   Vector3 RightUpperCorn =  tempOffset;
			
	   az = s.SpawnHeight/2;
				  
	   tempOffset.x = az * Mathf.Sin(s.SpawnRotation) * (-1);
					
	   tempOffset.z = az * Mathf.Cos(s.SpawnRotation);
					
	   Vector3 RightBottomCorn = tempOffset;
		
	   result = (RightBottomCorn - RightUpperCorn) * 0.5f;
	   
	   return  result;
	}

    //Editor
    [HideInInspector]
    public List<Trigger_Unified> TriggerLinkList = new List<Trigger_Unified>();

    public void NotifyTriggerLink_Editor(Trigger_Unified _trigger)
    {
        if (!TriggerLinkList.Contains(_trigger))
        {
            TriggerLinkList.Add(_trigger);
        }
    }
}
