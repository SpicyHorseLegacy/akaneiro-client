using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraDepthStyle : MonoBehaviour {

	// Use this for initialization
	public Transform[] theObjects;
	List<Renderer> theRenders = new List<Renderer>();
	void Start () {
		if(Camera.mainCamera)
		  Camera.mainCamera.depthTextureMode = DepthTextureMode.None;
		else if(Camera.allCameras.Length > 0)
		   Camera.allCameras[0].depthTextureMode = DepthTextureMode.None;
		
		foreach(Transform T in theObjects)
		{
			if( T == null )
				continue;
			
			Renderer[] tempRenders =  T.GetComponentsInChildren<Renderer>();
			foreach(Renderer it in tempRenders)
			{
			   theRenders.Add(it);		
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		bool bIndepth = false;
		foreach(Renderer T in theRenders)
		{
			if(T.transform.gameObject.activeInHierarchy == false)
				continue;
			if(T.isVisible)
			{
			   bIndepth = true;
			   break;
			}
	        
		}
		if( bIndepth )
		{
			if(Camera.mainCamera)
			  Camera.mainCamera.depthTextureMode = DepthTextureMode.Depth;
			else if(Camera.allCameras.Length > 0)
			  Camera.allCameras[0].depthTextureMode = DepthTextureMode.Depth;
			   
		}
		else
		{
			if( Camera.mainCamera )
			  Camera.mainCamera.depthTextureMode = DepthTextureMode.None;
			else if(Camera.allCameras.Length > 0)
			  Camera.allCameras[0].depthTextureMode = DepthTextureMode.None;
		}
	
	}
}
