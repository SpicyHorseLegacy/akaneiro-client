using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BreakableActor : InteractiveObj {
	
	public Transform BreakParticle;
	public int exp =50;
	public int KarmaChance = 100;
	public int Karma=50;
	public int TriggerID;

	public Transform BreakSoundPerfab;

	[System.Serializable]
	public class Spawner
	{
		public bool IsRandomSpawnNPC=false;
		public Transform NpcPrefab;
		public Transform[] NpcPrefabArray;
		public float SpawnDelayInSecond = 0f;
		public int SpawnCount=1;
		public int SpawnAinmIndex=0;
		public int SleepAinmIndex=0;
		public int WakeupAinmIndex=0;
		public float SpawnRadius=0f;
		public float MovableRadius = 30f;
		public float WanderRadius = 5f;
		public NpcSpawner.EStateType InitialAIState;
		public int AttackOnEnemySightChance=90;
		public int AttackOnDamageChance=100; 
		public int AttackOnAlertChance=100;
		public int GoToWanderAfterAttackChance=0;
		public int GoToWanderWhenOutOfRangeChance=0;
		
	}	

	public List<NpcSpawner> SpawnerList;
	
	public int AppearChance = 100;
	
	Transform BreakSound;
	
	// Use this for initialization
	public virtual void Start () {
		ObjType = ObjectType.BreakableObj;
	}
	
	public override void Active(int damage)
	{
		PlayBreakSound();
		
		if(BreakParticle)
			Object.Instantiate(BreakParticle,transform.position,transform.rotation);
		
		if(Health <= 0){
			
			IsUsed = true;
			
			if(collider)
				Destroy(collider);
			
			Player.Instance.AttackTarget=null;
		
			CS_SceneInfo.Instance.RemoveMiscThingByID(ObjID);
			
			Destroy(gameObject);
		}
	}
	
	public void PlayBreakSound()
	{
		if(BreakSound==null && BreakSoundPerfab!=null)
		{
			BreakSound = Instantiate(BreakSoundPerfab) as Transform;
		}
		
		if(BreakSound!=null)
		{
			BreakSound.position = transform.position;
			BreakSound.rotation = transform.rotation;
			SoundCue.Play(BreakSound.gameObject);
		}
	}

    #region Export
    public override string DoExport()
	{
		XMLStringWriter xmlWriter = new XMLStringWriter();
		
		xmlWriter.NodeBegin("BreakableActor");
		
		xmlWriter.AddAttribute("ID",TypeID);
	
		xmlWriter.AddAttribute("Exp",exp);
		xmlWriter.AddAttribute("KarmaChance",KarmaChance);
		xmlWriter.AddAttribute("Karma",Karma);
		xmlWriter.AddAttribute("Health",Health);
		xmlWriter.AddAttribute("Radius",radius);
		xmlWriter.AddAttribute("ApearChance",AppearChance);
		
	   
	    LootDrop myDrop = transform.GetComponent<LootDrop>();
		
		if(myDrop != null)
		{
			myDrop.DoOtherExport(xmlWriter);
		}
		
		if(DeathResults.Length > 0)
		{
			//print("DoExport!YMW");
			xmlWriter.NodeBegin("DeathResultArray");
			
			foreach( DeathResult dr in DeathResults )
			{
				xmlWriter.NodeBegin("DeathResultProperty");
				
				xmlWriter.AddAttribute("DeathResultType", (int)dr.Type);
				xmlWriter.AddAttribute("DeathResultID", dr.ID);
				xmlWriter.AddAttribute("DeathResultChance", dr.Chance);
				
				xmlWriter.NodeEnd("DeathResultProperty");
			}
			
			xmlWriter.NodeEnd("DeathResultArray");
		}
        
		xmlWriter.NodeEnd("BreakableActor");
		
		return xmlWriter.Result;
	}
	
	public  string BreakableSceneDoExport()
	{
		XMLStringWriter xmlWriter = new XMLStringWriter();
		
		xmlWriter.NodeBegin("MapBreakable");
		
		xmlWriter.AddAttribute("ID",TypeID);
		
		xmlWriter.AddAttribute("TriggerID",TriggerID);
		
	    xmlWriter.AddAttribute("PosX",transform.position.x);
		
	    xmlWriter.AddAttribute("PosY",transform.position.y);
		
	    xmlWriter.AddAttribute("PosZ",transform.position.z);
		
		xmlWriter.AddAttribute("rotX",transform.eulerAngles.x);
		
		xmlWriter.AddAttribute("rotY",transform.eulerAngles.y);
		
		xmlWriter.AddAttribute("rotZ",transform.eulerAngles.z);
		
		xmlWriter.AddAttribute("scaleX",transform.lossyScale.x);
		
		xmlWriter.AddAttribute("scaleY",transform.lossyScale.y);
		
		xmlWriter.AddAttribute("scaleZ",transform.lossyScale.z);
		
		foreach( NpcSpawner it in SpawnerList)
		{
		   if( it == null) continue;
		   
			xmlWriter.NodeBegin("SpawnResult");
			
			xmlWriter.AddAttribute("ID",it.id);
			
			xmlWriter.NodeEnd("SpawnResult");
		}
	
		xmlWriter.NodeEnd("MapBreakable");
		
		return xmlWriter.Result;
    }
    #endregion
}
