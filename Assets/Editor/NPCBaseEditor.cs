using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using System.IO;

[CustomEditor (typeof (NpcBase))]
public class NpcBaseEditor : Editor 
{
	protected Color m_VisionColor = new Color(1,0,0,.1f);
	protected Color m_VisionConeColor = new Color(1,0,0,.1f);
	protected Color m_AlertColor = new Color(1,1,0,1f);
    protected Color m_SizeColor = Color.green;
	
	List<NpcBase>  MonsterList = null;
	
	protected virtual void OnSceneGUI () 
	{
		//Debug.Log("Begin OnSceneGUI");
		NpcBase theScript = target as NpcBase;
		/*
		if( MonsterList == null)
		{
		   MonsterList = new List<NpcBase>();
			
		   string[] existingMonsters = Directory.GetFiles("Assets/Prefabs/CH/Enemies","*.prefab",SearchOption.AllDirectories);
			
		   foreach (string MonsterFile in existingMonsters)
           {
			  NpcBase theMonster =  AssetDatabase.LoadAssetAtPath(MonsterFile,typeof(NpcBase)) as NpcBase;
			  if( theMonster != null)
			     MonsterList.Add(theMonster);
		   }
		}

		if( theScript.TypeID == 0)
		{
		   
		
			int maxID = 0;
			
			foreach( NpcBase it in MonsterList)
			{
				if( it.TypeID > maxID && it != theScript)
					maxID = it.TypeID;
			}
			
			theScript.TypeID = ++maxID;
		}
		else
		{
			if(Application.isPlaying)
				return;
			
		   
			
			bool bSame = false;
		    
			foreach( NpcBase it in MonsterList)
			{
				if( it != theScript && it.TypeID == theScript.TypeID)
				{
				   	bSame = true;
					break;
				}
			}
			
			if(bSame)
			{
			   int maxID = 0;
			
			   foreach( NpcBase it in MonsterList)
			   {
				 if( it.TypeID > maxID  && it != theScript)
					maxID = it.TypeID;
			   }
				
			   theScript.TypeID = ++maxID;
			   	
			}	
		}
		*/
		
		//Handles.color = m_VisionColor;
		//Handles.DrawSolidDisc(theScript.transform.position, theScript.transform.up, theScript.VisionRadius);
		
		Handles.color = m_AlertColor;
		Handles.DrawWireArc(theScript.transform.position, theScript.transform.up, theScript.transform.forward, 360f, theScript.AlertDistance);	                
		
		Handles.color = m_VisionConeColor;
		Handles.DrawSolidArc(theScript.transform.position,theScript.transform.up, Quaternion.Euler(new Vector3(0f, theScript.transform.rotation.eulerAngles.y - (theScript.VisionAngle / 2), 0f)) * new Vector3(0f,0f,1f), theScript.VisionAngle, theScript.VisionRadius);

        Handles.color = m_SizeColor;
        Handles.DrawWireArc(theScript.transform.position, theScript.transform.up, theScript.transform.forward, 360f, theScript.AvoidanceRadius);
	  
		
		if(theScript.bSummonerNpc &&  theScript.SummonSpawner != null)
		{
			Handles.color = Color.red;
			
			Handles.DrawWireArc(theScript.transform.position, theScript.transform.up,theScript.transform.forward,360f,theScript.SummonSpawner.MovableRadius);
			
			Handles.color = Color.cyan;
			
			Handles.DrawWireArc(theScript.transform.position, theScript.transform.up,theScript.transform.forward,360f,theScript.SummonSpawner.WanderRadius);
			
			Handles.color = Color.green;
			
			NpcSpawner.Spawner s = theScript.SummonSpawner;
			
			if(s.Bcircle)
			{
			   Handles.DrawWireArc(theScript.transform.position, theScript.transform.up,theScript.transform.forward,360f,s.SpawnArea);
			}
			else
			{	  
			   Vector3 tempOffset = Vector3.zero;
		
			   float ax = s.SpawnWidth/2 * (-1);
					
			   float az = s.SpawnHeight/2 * (-1);
				  
			   tempOffset.x = ax * Mathf.Cos(s.SpawnRotation) - az * Mathf.Sin(s.SpawnRotation);
					
			   tempOffset.z = az * Mathf.Cos(s.SpawnRotation) + ax * Mathf.Sin(s.SpawnRotation);
					
				
		       Vector3 LeftUpperCorn = theScript.transform.position + tempOffset;
					
			   ax = s.SpawnWidth/2;
					
			   az = s.SpawnHeight/2 * (-1);
				  
			   tempOffset.x = ax * Mathf.Cos(s.SpawnRotation) - az * Mathf.Sin(s.SpawnRotation);
					
			   tempOffset.z = az * Mathf.Cos(s.SpawnRotation) + ax * Mathf.Sin(s.SpawnRotation);
					
			   Vector3 RightUpperCorn = theScript.transform.position + tempOffset;
					
					
			   ax = s.SpawnWidth/2 * (-1);
					
			   az = s.SpawnHeight/2;
				  
			   tempOffset.x = ax * Mathf.Cos(s.SpawnRotation) - az * Mathf.Sin(s.SpawnRotation);
					
			   tempOffset.z = az * Mathf.Cos(s.SpawnRotation) + ax * Mathf.Sin(s.SpawnRotation);
					
					
			   Vector3 LeftBottomCorn = theScript.transform.position + tempOffset;
					
			   ax = s.SpawnWidth/2;
					
			   az = s.SpawnHeight/2;
				  
			   tempOffset.x = ax * Mathf.Cos(s.SpawnRotation) - az * Mathf.Sin(s.SpawnRotation);
					
			   tempOffset.z = az * Mathf.Cos(s.SpawnRotation) + ax * Mathf.Sin(s.SpawnRotation);
					
			   Vector3 RightBottomCorn = theScript.transform.position + tempOffset;
				
					
			   Handles.DrawLine(LeftUpperCorn,RightUpperCorn);
					
			   Handles.DrawLine(RightUpperCorn,RightBottomCorn);
				   
			   Handles.DrawLine(RightBottomCorn,LeftBottomCorn);
					
			   Handles.DrawLine(LeftBottomCorn,LeftUpperCorn);
					
			}
			
			
		}
			
	}
}
