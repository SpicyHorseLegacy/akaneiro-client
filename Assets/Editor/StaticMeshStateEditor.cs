using UnityEngine;
using System.Collections;
using UnityEditor;




[CustomEditor(typeof(StaticMeshState))] 
public class StaticMeshStateEditor: Editor
{
	private StaticMeshState theScript;
	
	
	void OnEnable() 
	{
         //Debug.Log("StaticMeshState was enabled");
		 theScript = (StaticMeshState)target;
	    
	}
	
	void OnDisable()
	{
		//Debug.Log("StaticMeshState was disabled");
	}
	
	void OnDestroy()
	{
		//Debug.Log("StaticMeshState was destroyed");
		
	}
	
	
	
	
}