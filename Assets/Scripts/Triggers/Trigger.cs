using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Trigger : TriggerBase {
	
	public int MaxTriggerCount=0; //0 means trigger infinite
	public float TriggerDelayTime=0f;
	
	[HideInInspector]
	public int TriggeredCount=0;
	
	// Use this for initialization
	public virtual void Start () {
		if(MaxTriggerCount<0)
			MaxTriggerCount=0;
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
		
		foreach( Transform it in TriggerNpcList)
		{
		   if( it == null) continue;
		   NpcSpawner mMonsterSpawner = it.GetComponent<NpcSpawner>();
		   if(mMonsterSpawner != null)
		   {
			  xmlWriter.NodeBegin("TriggerResult");
			  xmlWriter.AddAttribute("ID",mMonsterSpawner.id);
			  xmlWriter.AddAttribute("Name",mMonsterSpawner.name);
			  xmlWriter.AddAttribute("Type",0);
			  xmlWriter.NodeEnd("TriggerResult");
			 
		   }	
		}
		
		xmlWriter.NodeEnd("Triggers");
		
	
		return xmlWriter.Result;
	}
	
	public override string GetNodeType()
	{
		string output = "Trigger";
		return output;
	}
}
