using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LightTurnonoff : MonoBehaviour {

	// Use this for initialization
	
	//public string levelState = "night";
	
	//public string[]  LevelStateList = {"s2"};
	
	//FadeInOut fade = null;
	
	
	public Transform[] AreaRanges = new Transform[0];
	
	public Light[] GlobalLightList = new Light[0];
	
	List<Rect> NeededRects = new List<Rect>();
	
	
	void Start () 
	{
		 foreach(Transform it in AreaRanges)
		 {
			  if(it == null)
				 continue;
			  if(it.collider == null)
				continue;
			  if(it.GetComponent<BoxCollider>())
			  {
				  float sizex = it.GetComponent<BoxCollider>().size.x;
			      sizex *= it.localScale.x;
				  float sizez = it.GetComponent<BoxCollider>().size.y;
				  sizez *= it.localScale.z;
				
				  Rect tempRect = new Rect(it.position.x - sizex/2f,it.position.z - sizez/2f,sizex,sizez);
				  
				  NeededRects.Add(tempRect);
				  
				  /*
				  if(Player.Instance.transform.position.x >= minPoint.x && Player.Instance.transform.position.x <= maxPoint.x
				     && Player.Instance.transform.position.z >= minPoint.z && Player.Instance.transform.position.z <= maxPoint.z)
				  {
					 bEnterInArea = true;
					 break;
				  }
				  */
			  }
		 }
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		
		  //// if(GlobalGameState.bInCave)
		   //{
			//  ChangeChildvis(transform,false);
			  //return;
		   //}
	       /*
		   if( fade == null)
		      fade = Camera.mainCamera.GetComponent<FadeInOut>();
		   if( fade.ratio < 0.9999f)
			  return;
		   */
		
		  // bool bturnOn = false;
		   //for(int i = 0; i < LevelStateList.Length; i++)
			//{
				//if( GlobalGameState.state == LevelStateList[i])
				//{
					//bturnOn = true;
					//break;
				//}
			//}
		   //ChangeChildvis(transform,bturnOn);
		
		 bool bEnterInArea = false;
		
		 foreach(Rect rect in NeededRects)
		 {
			Vector3 tempPoint = Player.Instance.transform.position;
			
			tempPoint.y = tempPoint.z;
			
			if(rect.Contains(tempPoint))
			{
				bEnterInArea = true;
				break;
			}
		 }
		
		 if( bEnterInArea )
		 {
			foreach(Light lit in GlobalLightList)
			{
				if(lit == null)
					continue;
				
				lit.enabled = true;
				
				if(lit.gameObject!= null && lit.gameObject.GetComponent<RandomLighting>())
				   lit.gameObject.GetComponent<RandomLighting>().enabled = true;
			}
		 }
		 else
		 {
			foreach(Light lit in GlobalLightList)
			{
				if(lit == null)
					continue;
				
				lit.enabled = false;
				
				if(lit.gameObject!= null && lit.gameObject.GetComponent<RandomLighting>())
				   lit.gameObject.GetComponent<RandomLighting>().enabled = false;
			}
		 }
	}
	
	
	
/*
public void ChangeChildvis(Transform tsm,bool bshow)
{
	    if(tsm && tsm.GetComponent<Light>())
		{
		   Light PlantLight = tsm.GetComponent<Light>(); 
		 	
		   PlantLight.enabled = bshow;
			
		}
		
		for(int i = 0; i < tsm.GetChildCount();i++)
		{
			ChangeChildvis(tsm.GetChild(i),bshow);
		}
		
}
*/
	
}
