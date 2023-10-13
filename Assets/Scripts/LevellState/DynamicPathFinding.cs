//Author:jiangwei
//date:2011-9-6
// need to recaculate a path

using UnityEngine;
using System.Collections;

public class DynamicPathFinding : MonoBehaviour {

	// Use this for initialization
	
	//public bool bPassed = true;
	
	//public bool bRecaculatePath = false;
	
	public Transform[] EnablePass;
	public Transform[] DisablePass;
	
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
//--------------only test-------------------//
		/*
		if( Input.GetKeyDown(KeyCode.E))
		{
		   //go through
		   RecalculatePath(true); 
		 
		}
		*/
//------------------------------------------//
	
		
	
	}
	

	public void RecalculatePath(bool bpass)
	{	
		 if(bpass)
		 {
			 foreach(Transform T in EnablePass)
			 {
				Collider pCollider = T.collider;
				if( pCollider )
					pCollider.isTrigger = true;
			 }
			
		 }
		 else
		 {
			 foreach(Transform T in DisablePass)
			 {
				Collider pCollider = T.collider;
				if( pCollider )
					pCollider.isTrigger = false;
			 }
		 }
			 
		
	}
		
	
}
