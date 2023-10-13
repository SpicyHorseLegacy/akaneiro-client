using UnityEngine;
using System.Collections;

public class TeleportInteractive : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		
		RaycastHit hit;
		
		int myLayer = gameObject.layer;
		
		if(Physics.Raycast(ray.origin,ray.direction,out hit,100f,myLayer))
		{
			if( hit.transform == transform)
			{
				 Renderer NpcRenderer = transform.GetComponentInChildren<Renderer>();
				 if(NpcRenderer)
				 {
					for(int i = 0; i < NpcRenderer.materials.Length;i++)
					{
						Material mtl =  NpcRenderer.materials[i];
				    	if(mtl.HasProperty("_EmissiveColor"))
				        {
						    mtl.SetColor("_EmissiveColor",Color.white);
						}	
					}
				}
				
			}
			else
			{
				Renderer NpcRenderer = transform.GetComponentInChildren<Renderer>();
				 if(NpcRenderer)
				 {
					for(int i = 0; i < NpcRenderer.materials.Length;i++)
					{
						Material mtl =  NpcRenderer.materials[i];
				    	if(mtl.HasProperty("_EmissiveColor"))
				        {
							mtl.SetColor("_EmissiveColor",Color.black);
						}	
					}
				}
				
			}
		  
		}
	
	}
}
