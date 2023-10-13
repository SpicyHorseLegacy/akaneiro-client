using UnityEngine;
using System.Collections;

public class GateClient : MonoBehaviour {
	
    public bool bShow = true;
	public int MonsterSkilled = 0;
	
	Renderer[] gateRenders = null;
	// Use this for initialization
	void Start () {
		
		gateRenders = transform.GetComponentsInChildren<Renderer>();

	}
	
	// Update is called once per frame
	
	
	public void OpenDoor()
	{
		if(transform.collider != null)
		    transform.collider.isTrigger = false;
						
		foreach(Renderer it in gateRenders)
	    {
		   it.enabled = true;
		}
					
	}
	
	public void CloseDoor()
	{
		if(transform.collider != null)
		   transform.collider.isTrigger = true;
						
		foreach(Renderer it in gateRenders)
		{
		   it.enabled = false;
		}
	}
}
