using UnityEngine;
using System.Collections;

public class GateClientTrigger : MonoBehaviour {

	// Use this for initialization
	
	public int triggerID = 0;
    
	Rect myRect;
	
	bool bTouch = false;
	
	[System.Serializable]
    public class GateData
	{
		public Transform gate = null;
		public bool bShow = false;
	}
	
	public GateData[] Gates;
	
	
	void Awake() 
	{
	    if(transform.GetComponent<BoxCollider>())
		{
			float sizex = transform.GetComponent<BoxCollider>().size.x;
		    sizex *= transform.localScale.x;
		    float sizez = transform.GetComponent<BoxCollider>().size.z;
		    sizez *= transform.localScale.z;
			myRect = new Rect(transform.position.x - sizex/2f,transform.position.z - sizez/2f,sizex,sizez);
			
		}
	}
	
	// Update is called once per frame
	void Update() 
	{
		if(Player.Instance == null) {
			return;
		}
		
	   Vector3 tempPoint = Player.Instance.transform.position;
			
	   tempPoint.y = tempPoint.z;
			
	   if(myRect.Contains(tempPoint) && !bTouch)
	   {
		   bTouch = true;
		   if( Gates != null)
			{
				foreach(GateData it in Gates)
				{
					if(it.gate == null)
						continue;
					Renderer[] gateRenders =  it.gate.GetComponentsInChildren<Renderer>();
					
					if( gateRenders == null)
						continue;
					
					if(it.bShow)
					{
					    if(it.gate.collider != null)
						   it.gate.collider.isTrigger = false;
						
						foreach(Renderer it2 in gateRenders)
						{
							it2.enabled = true;
						}
					}
					else
					{
						if(it.gate.collider != null)
						   it.gate.collider.isTrigger = true;
						
						foreach(Renderer it2 in gateRenders)
						{
							it2.enabled = false;
						}
					}
				}
			}
		  
	   }
	}
}
