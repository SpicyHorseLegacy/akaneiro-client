using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayAnimTrigger : TriggerBase {
	
	public int MaxTriggerCount=0; //0 means trigger infinite
	public float TriggerDelayTime=0f;
	

	[System.Serializable]
	public class AnimationDataStruct
	{
		public SelfAnimation AnimObj;
	    public AnimationClip anim;
	    public bool IsLoopAnim=false;
		public float AnimDelayTime;
		public bool bCollision = false;
		public bool bCanShow = true;
	}
	
	public AnimationDataStruct[] AnimDataArrays;
	
	
	// Use this for initialization
	void Start () {
		if(MaxTriggerCount<0)
			MaxTriggerCount=0;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	/*
	public virtual void OnTriggerEnter (Collider other)
	{
		if(MaxTriggerCount!=0 && TriggeredCount>= MaxTriggerCount)
			return;
		
		if(!TriggerOn)
			return;		
		
		TriggeredCount++;
		
		StartCoroutine("TriggerObjAnim");

	}
	
	IEnumerator TriggerObjAnim()
	{
		yield return new WaitForSeconds(TriggerDelayTime);
		
		if(AnimObj && AnimObj.animation && anim)
		{
			if(IsLoopAnim)
				AnimObj.animation[anim.name].wrapMode = WrapMode.Loop;
			else
				AnimObj.animation[anim.name].wrapMode = WrapMode.Once;
			
			AnimObj.animation.CrossFade(anim.name);
		}
	}
	
	
	public override void TriggerMe ()
	{
		base.TriggerMe ();
		
		OnTriggerEnter(Player.Instance.transform.collider);
	}
	*/
	
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
		
		foreach( AnimationDataStruct it in AnimDataArrays)
		{
		   if( it == null) 
				continue;
			
		   xmlWriter.NodeBegin("TriggerResult");
		   xmlWriter.AddAttribute("ID",it.AnimObj.id + id);
		   xmlWriter.AddAttribute("Name",it.AnimObj.transform.name);
		   xmlWriter.AddAttribute("Type",4);
		   xmlWriter.NodeBegin("AnimationInfo");
		   xmlWriter.AddAttribute("id",it.AnimObj.id + id);
		   xmlWriter.AddAttribute("realID",it.AnimObj.id);
		   xmlWriter.AddAttribute("Loop",it.IsLoopAnim);
		   xmlWriter.AddAttribute("AnimName",it.anim.name);
		   xmlWriter.AddAttribute("AnimDelayTime",it.AnimDelayTime);
		   xmlWriter.AddAttribute("bCollision",it.bCollision);
		   xmlWriter.AddAttribute("bCanShow",true);
		   xmlWriter.NodeEnd("AnimationInfo");
		   xmlWriter.NodeEnd("TriggerResult"); 
		}
		
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
