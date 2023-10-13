using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PF_Speech_Trigger : TriggerBase {
	
	public int MaxTriggerCount=0; //0 means trigger infinite
	public float TriggerDelayTime=0f;
	public bool  InsideArea;
	
	[System.Serializable]
	public class cSpeechObj
	{
		public bool bRandom = false;
		public bool bLoop   = false;
		public int NpcID;
		public int[] WordList;
		public float SpeakTime;
	}
	
	public cSpeechObj[] SpeechList = new cSpeechObj[0];
	

	public NpcSpawner[] DisableSpawnerList = new NpcSpawner[0];
	
	public TriggerBase[] EnableTriggerList = new TriggerBase[0];
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
			  xmlWriter.AddAttribute("kind",1);
			  xmlWriter.AddAttribute("Type",0);
				
			  xmlWriter.NodeEnd("TriggerResult");
		   }	
		}
		
		foreach(NpcSpawner it in DisableSpawnerList)
		{
			  if( it == null) continue;
		  
			  xmlWriter.NodeBegin("TriggerResult");
			  xmlWriter.AddAttribute("ID",it.id);
			  xmlWriter.AddAttribute("kind",2);
			  xmlWriter.AddAttribute("Type",0);

			  xmlWriter.NodeEnd("TriggerResult");
		  	
		}
		
		foreach(TriggerBase it in EnableTriggerList)
		{
			 if( it == null) continue;
		  
			  xmlWriter.NodeBegin("EnableTrigger");
			  xmlWriter.AddAttribute("ID",it.id);
			  xmlWriter.NodeEnd("EnableTrigger");
		}
		
		foreach(cSpeechObj it in SpeechList)
		{
			 if( it == null) continue;
			 
			 xmlWriter.NodeBegin("TriggerResult");
			
			
			 xmlWriter.AddAttribute("ID",it.NpcID);
			  
			 xmlWriter.AddAttribute("isRandom",it.bRandom);
			
			 xmlWriter.AddAttribute("Type",5);
			
			 xmlWriter.AddAttribute("isLoop",it.bLoop);
			
			 xmlWriter.AddAttribute("InsideArea",InsideArea);
			
			 xmlWriter.AddAttribute("SpeakTime",it.SpeakTime);
			
			// List<int> tempList = new List<int>();
			 List<int> resultList = new List<int>();
			
			// foreach(int index in it.WordList)
				//tempList.Add(index);
			
			 //for(int i = 0 ;i < it.WordList.Length;i++)
			// {
				//int itemp = Random.Range(0,tempList.Count);
				
				//resultList.Add(tempList[itemp]);
				
				//tempList.RemoveAt(itemp);
			// }
			
			// if( it.bRandom == false)
			 //{
				//resultList.Clear();
				
		     foreach(int index in it.WordList)
				resultList.Add(index);
			
			 //}
			
			 foreach(int index in resultList)
			 {
				xmlWriter.NodeBegin("Word");
				xmlWriter.AddAttribute("WhichWord",index);
				xmlWriter.NodeEnd("Word");
			 }
			  
			 xmlWriter.NodeEnd("TriggerResult");
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