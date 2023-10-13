using System;
using UnityEditor;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

[CustomEditor (typeof (NpcSpawner))]
public class NpcSpawnerEditor : Editor {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void OnSceneGUI () 
	{
		//Debug.Log("Begin OnSceneGUI");
		NpcSpawner spawner = target as NpcSpawner;
		
		if( spawner.id == 0)
		{
			NpcSpawner[]  MonsterSpawnList  = FindObjectsOfType(typeof(NpcSpawner)) as NpcSpawner[];
			
			int maxID = 0;
			
			foreach( NpcSpawner it in MonsterSpawnList)
			{
				if( it.id > maxID )
					maxID = it.id;
			}
			
			spawner.id = ++maxID;
		}
		else
		{
		    NpcSpawner[]  MonsterSpawnList  = FindObjectsOfType(typeof(NpcSpawner)) as NpcSpawner[];
			
			bool bSame = false;
		    
			foreach( NpcSpawner it in MonsterSpawnList)
			{
				if( it != spawner && it.id == spawner.id)
				{
				   	bSame = true;
					break;
				}
			}
			
			if(bSame)
			{
			   int maxID = 0;
			
			   foreach( NpcSpawner it in MonsterSpawnList)
			   {
				 if( it.id > maxID )
					maxID = it.id;
			   }
				
			   spawner.id = ++maxID;
			   	
			}	
		}
		
		
		Handles.color = Color.red;
		foreach(NpcSpawner.Spawner  s in spawner.SpawnerList)
		{
			foreach(Transform SpawnPoint in s.SpawnLocation)
			{
				
				Handles.DrawWireArc(SpawnPoint.position, spawner.transform.up,spawner.transform.forward,360f,s.MovableRadius);
			}
		}
		
		Handles.color = Color.yellow;
		foreach(NpcSpawner.Spawner  s in spawner.SpawnerList)
		{
			foreach(Transform SpawnPoint in s.SpawnLocation)
			{
				
				Handles.DrawWireArc(SpawnPoint.position, spawner.transform.up,spawner.transform.forward,360f,s.WanderRadius);
			}
		}
		
		Handles.color = Color.green;
		foreach(NpcSpawner.Spawner  s in spawner.SpawnerList)
		{
			if(s.Bcircle)
			{
			   foreach(Transform SpawnPoint in s.SpawnLocation)
			   {
				  if(s.InitialState == NpcSpawner.EStateType.Spawn)
				  {
						if(s.WanderWaypoint != null && s.WanderWaypoint.Length > 0)
						{
							Handles.DrawWireArc(s.WanderWaypoint[0].position, spawner.transform.up,spawner.transform.forward,360f,s.SpawnArea);
						}
						else
						{
							Handles.DrawWireArc(SpawnPoint.position, spawner.transform.up,spawner.transform.forward,360f,s.SpawnArea);
						}
				   }
				   else
				   {
				      Handles.DrawWireArc(SpawnPoint.position, spawner.transform.up,spawner.transform.forward,360f,s.SpawnArea);
					}
			   }
			}
			else
			{
			   foreach(Transform SpawnPoint in s.SpawnLocation)
			   {
				  //Handles.DrawWireArc(SpawnPoint.position, spawner.transform.up,spawner.transform.forward,360f,s.SpawnArea);
				   Vector3 tempOffset = Vector3.zero;
					
				    	
				   float ax = s.SpawnWidth/2 * (-1);
					
				   float az = s.SpawnHeight/2 * (-1);
				  
				   tempOffset.x = ax * Mathf.Cos(s.SpawnRotation) - az * Mathf.Sin(s.SpawnRotation);
					
				   tempOffset.z = az * Mathf.Cos(s.SpawnRotation) + ax * Mathf.Sin(s.SpawnRotation);
					
				
				   Vector3 LeftUpperCorn = SpawnPoint.position + tempOffset;
					
				   if(s.InitialState == NpcSpawner.EStateType.Spawn)
				   {
						if(s.WanderWaypoint != null && s.WanderWaypoint.Length > 0)
							LeftUpperCorn = s.WanderWaypoint[0].position + tempOffset;
				       		
				   }
					
				   ax = s.SpawnWidth/2;
					
				   az = s.SpawnHeight/2 * (-1);
				  
				   tempOffset.x = ax * Mathf.Cos(s.SpawnRotation) - az * Mathf.Sin(s.SpawnRotation);
					
				   tempOffset.z = az * Mathf.Cos(s.SpawnRotation) + ax * Mathf.Sin(s.SpawnRotation);
					
				   Vector3 RightUpperCorn = SpawnPoint.position + tempOffset;
					
					if(s.InitialState == NpcSpawner.EStateType.Spawn)
				   {
						if(s.WanderWaypoint != null && s.WanderWaypoint.Length > 0)
							RightUpperCorn = s.WanderWaypoint[0].position + tempOffset;
				       		
				   }
					
					
				   ax = s.SpawnWidth/2 * (-1);
					
				   az = s.SpawnHeight/2;
				  
				   tempOffset.x = ax * Mathf.Cos(s.SpawnRotation) - az * Mathf.Sin(s.SpawnRotation);
					
				   tempOffset.z = az * Mathf.Cos(s.SpawnRotation) + ax * Mathf.Sin(s.SpawnRotation);
					
					
				   Vector3 LeftBottomCorn = SpawnPoint.position + tempOffset;
					
				   if(s.InitialState == NpcSpawner.EStateType.Spawn)
				   {
						if(s.WanderWaypoint != null && s.WanderWaypoint.Length > 0)
							LeftBottomCorn = s.WanderWaypoint[0].position + tempOffset;
				       		
				   }
					
				   ax = s.SpawnWidth/2;
					
				   az = s.SpawnHeight/2;
				  
				   tempOffset.x = ax * Mathf.Cos(s.SpawnRotation) - az * Mathf.Sin(s.SpawnRotation);
					
				   tempOffset.z = az * Mathf.Cos(s.SpawnRotation) + ax * Mathf.Sin(s.SpawnRotation);
					
				   Vector3 RightBottomCorn = SpawnPoint.position + tempOffset;
					
				   if(s.InitialState == NpcSpawner.EStateType.Spawn)
				   {
						if(s.WanderWaypoint != null && s.WanderWaypoint.Length > 0)
							RightBottomCorn = s.WanderWaypoint[0].position + tempOffset;
				       		
				   }
					
				   Handles.DrawLine(LeftUpperCorn,RightUpperCorn);
					
				   Handles.DrawLine(RightUpperCorn,RightBottomCorn);
				   
				   Handles.DrawLine(RightBottomCorn,LeftBottomCorn);
					
				   Handles.DrawLine(LeftBottomCorn,LeftUpperCorn);
					
					
				  // Vector3 LeftCorn = SpawnPoint.position;
					
			   }
			}
		}

        for(int i=0; i<spawner.TriggerLinkList.Count; i++)
        {
            Trigger_Unified trigger = spawner.TriggerLinkList[i];
            if (trigger == null || !trigger.IsLinkToSpawner(spawner))
            {
                spawner.TriggerLinkList.Remove(trigger);
            }
            else
            {
                Handles.color = Color.green;
                Handles.DrawLine(spawner.transform.position, trigger.transform.position);
            }
        }	                
	}
	
	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI ();
		if (GUILayout.Button ("ExportData",GUILayout.Width (100)))
		{
			//string[] path = EditorApplication.currentScene.Split(char.Parse("/"));
           
            //path[path.Length - 1] = path[path.Length - 1].Replace(".unity", "_MonsterSpawner.csv");
			
			//string strMstSpawnFile = string.Join("/", path);
			
			string strMstSpawnFile = EditorUtility.SaveFilePanel("MonsterSpawn Export .csv file", "", "MonsterSpawn", "csv");
			
			string MonsterString = "";
	        
			List<string[]>ls = new List<string[]>();
			
			NpcSpawner spawner = target as NpcSpawner;
			
			string[] strProperties;
			bool bAppend = true;
			if (!File.Exists(strMstSpawnFile)) 
			{
			  strProperties = new string[16];
			  strProperties[0] = "isRandomNpc";
		      strProperties[1] = "MonsterName";
			  strProperties[2] = "SpawnLocation";
			  strProperties[3] = "WanderPoint";
			  strProperties[4] = "SpawnDelay";
			  strProperties[5] = "SpawnCount";
			  strProperties[6] = "SpawnRadius";
		      ls.Add(strProperties);
			  bAppend = false;
			}
				
			foreach(NpcSpawner.Spawner  s in spawner.SpawnerList)
			{
				strProperties = new string[16];
			
				if(s.IsRandomSpawnNpc)
					strProperties[0] = "true";
				else
					strProperties[0] = "false";
				
				MonsterString = "";
				
				if(s.IsRandomSpawnNpc)
				{
					
					for(int i = 0; i <  s.NpcPrefabArray.Length; i++)
					{
						MonsterString += s.NpcPrefabArray[i].name;
						if(i < s.NpcPrefabArray.Length -1)
							MonsterString += "|";
					}
					strProperties[1] = MonsterString;
				}
				else
				{
					if(s.NpcPrefabArray[0])
				       MonsterString = s.NpcPrefabArray[0].name;
					strProperties[1] = MonsterString;
				}
				MonsterString = "";
				for(int i = 0; i <  s.SpawnLocation.Length; i++)
				{
					Vector3 sp =  s.SpawnLocation[i].position;
					MonsterString += "(";
					MonsterString += Convert.ToString(sp.x);
					MonsterString += "|";
					MonsterString += Convert.ToString(sp.y);
					MonsterString += "|";
					MonsterString += Convert.ToString(sp.z);
					MonsterString += ")";
					if(i < s.SpawnLocation.Length -1)
							MonsterString += "||";
					
				}
				strProperties[2] = MonsterString;
				MonsterString = "";
				for(int i = 0; i < s.WanderWaypoint.Length;i++)
				{
					Vector3 sp =  s.WanderWaypoint[i].position;
					MonsterString += "(";
					MonsterString += Convert.ToString(sp.x);
					MonsterString += "|";
					MonsterString += Convert.ToString(sp.y);
					MonsterString += "|";
					MonsterString += Convert.ToString(sp.z);
					MonsterString += ")";
					if(i < s.WanderWaypoint.Length -1)
							MonsterString += "||";
				}
				strProperties[3] = MonsterString;
			    strProperties[4] = Convert.ToString(s.SpawnDelay);
				strProperties[5] = Convert.ToString(s.SpawnNpcCount);
				strProperties[6] = Convert.ToString(s.SpawnArea);
				
				ls.Add(strProperties);	
			}
			
			if(strMstSpawnFile.Length > 0)
			   CSVHelper.WriteCSV(strMstSpawnFile,bAppend,ls);
		}
	}
}
