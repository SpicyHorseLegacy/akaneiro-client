using UnityEngine;
using System.Collections;

public class MissionEndTrigger : TriggerBase 
{
	public Vector3 OtherPos;
	public string MapName;
	

	public int MaxTriggerCount=0;
	public float TriggerDelayTime=0f;
	
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	

	public override string DoExport()
	{
			
		XMLStringWriter xmlWriter = new XMLStringWriter();
		
		xmlWriter.NodeBegin("Triggers");
		
		xmlWriter.AddAttribute("id",id);
		
		xmlWriter.AddAttribute("TriggerType",TriggerType);
		
		xmlWriter.AddAttribute("PosX",transform.position.x);
		
	    xmlWriter.AddAttribute("PosY",transform.position.y);
		
		xmlWriter.AddAttribute("PosZ",transform.position.z);
		
		xmlWriter.AddAttribute("rot",transform.eulerAngles.y);
		
		if(gameObject.GetComponent<BoxCollider>())
		{
			float sizex = gameObject.GetComponent<BoxCollider>().size.x;
			sizex *= transform.localScale.x;
			
			float sizey = gameObject.GetComponent<BoxCollider>().size.y;
			sizey *= transform.localScale.y;
			
			float sizez = gameObject.GetComponent<BoxCollider>().size.z;
			sizez *= transform.localScale.z;
			
			
			xmlWriter.AddAttribute("SizeX",sizex);
			xmlWriter.AddAttribute("SizeY",sizey);
			xmlWriter.AddAttribute("SizeZ",sizez);
		}
		else if(gameObject.GetComponent<SphereCollider>())
		{
			float fRadius = gameObject.GetComponent<SphereCollider>().radius;
			
			fRadius *= 2;
			
			float sizex = fRadius * transform.localScale.x;
			
			float sizey = fRadius * transform.localScale.y;
			
			float sizez = fRadius * transform.localScale.z;
		   
			xmlWriter.AddAttribute("SizeX",sizex);
			xmlWriter.AddAttribute("SizeY",sizey);
			xmlWriter.AddAttribute("SizeZ",sizez);
		}
		
	
		xmlWriter.AddAttribute("TriggerOn",TriggerOn);
		xmlWriter.AddAttribute("DelayTime",TriggerDelayTime);
		xmlWriter.AddAttribute("TriggerCount",MaxTriggerCount);
		
		xmlWriter.NodeBegin("TriggerResult");
		xmlWriter.AddAttribute("ID",id);
		xmlWriter.AddAttribute("Name",MapName);
		xmlWriter.AddAttribute("Type",4);
		xmlWriter.NodeEnd("TriggerResult");
			
		xmlWriter.NodeBegin("ChangeMap");
		
		xmlWriter.AddAttribute("id",id);
		xmlWriter.AddAttribute("Name",MapName);
		
		xmlWriter.AddAttribute("PosX",OtherPos.x);
		xmlWriter.AddAttribute("PosY",OtherPos.y);
		xmlWriter.AddAttribute("PosZ",OtherPos.z);
		
	
		xmlWriter.NodeEnd("ChangeMap");
	
		return xmlWriter.Result;
	}
	
    public override string GetNodeType()
	{
		string output = "Trigger";
		return output;
	}
	
}
