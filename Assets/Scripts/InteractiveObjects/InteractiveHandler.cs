using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//include all interactive objects.
public class InteractiveHandler : InteractiveObj {
	
	public string name;
	
	// In fact, player always hits the object and take damage, if health <= 0, use the object.
	// But something doesn't need an attack animation. Eg. Treasure / trigger
	public bool NeedsAttackAnimationToUSE = false;
	
	public int LootLevel = 1;

	public AnimationClip DeathAnim;
	
	public AnimationClip ImpactAnim;

    public Transform ImpactAndDeathVFXPos;
	
	public Transform ImpactFX;
	
	public Transform ImpactSound;
	
	public Transform DeathFX;
	
	public Transform DeathSound;
	
	public bool  DestroyOnDeath = false;
	
	public bool  DestroyCollision = true;
	
	Transform myImpactSound = null;
	
	Transform myDeathSound = null;
	
	[System.Serializable]
	public class SpawnCondition
	{
	   public float DelayTime=0f;
	   public Transform Spawner;
	}
		
	[System.Serializable]
	public class TriggerOtherSpawner
	{
		public bool LoopingSpawner = false;
		public int loopCount = 0;
		
		public List<SpawnCondition> EnableSpawnerList;
		public List<SpawnCondition> DisableSpawnerList;
		public List<SpawnCondition> DisableObjects;
		public List<SpawnCondition> ToggleObjects;
	}
	
	
	public TriggerOtherSpawner LinkedEvents;
	
	public float MinKarmaValue = 0;
	
	public float MaxKarmaValue = 0;
	
	public Transform[] KarmaPerfabs = new Transform[0];
	
	public int AppearChance = 100;
	
    public int Exp = 0;
	
	public List<NpcSpawner> CollidderAreaSpawnerList = new  List<NpcSpawner>();
	
	int ReaLKarmaValue = 0;
	
	public PlayAnimTrigger.AnimationDataStruct[] AnimDataArrays = new PlayAnimTrigger.AnimationDataStruct[0];
	
	// Use this for initialization
    public virtual void Start()
    {
        gameObject.AddComponent<Animation>();

        AvoidanceRadius = 1;

        ObjType = ObjectType.IteractiveObj;

        if (DeathAnim != null)
        {
            animation.AddClip(DeathAnim, DeathAnim.name);
            transform.animation[DeathAnim.name].layer = -1;
            transform.animation[DeathAnim.name].wrapMode = WrapMode.Once;
        }

        if (ImpactAnim != null)
        {
            animation.AddClip(ImpactAnim, ImpactAnim.name);
            transform.animation[ImpactAnim.name].layer = -1;
            transform.animation[ImpactAnim.name].wrapMode = WrapMode.Once;
        }

        if (AttrMan)
        {
            AttrMan.Attrs[EAttributeType.ATTR_CurHP] = Health;
            AttrMan.Attrs[EAttributeType.ATTR_MaxHP] = Health;
        }
    }
	
	// Update is called once per frame
	public virtual void Update () 
	{
		Renderer[] NpcRenderers = transform.GetComponentsInChildren<Renderer>();
		
		foreach(Renderer NpcRenderer in NpcRenderers)
	    {
			for(int i = 0; i < NpcRenderer.materials.Length;i++)
			{
			   Material mtl =  NpcRenderer.materials[i];
				
			   if(mtl.HasProperty("_FogEnable"))
			   {
					mtl.SetFloat("_FogEnable",1f);
			   }
				
			   if(mtl.HasProperty("_FogColor"))
			   {
					mtl.SetColor("_FogColor",RenderSettings.fogColor);
			   }
				
			   if(mtl.HasProperty("_FogStartDis"))
			   {
					mtl.SetFloat("_FogStartDis",RenderSettings.fogStartDistance);
			   }

               if (mtl.HasProperty("_FogEndDis") && CS_SceneInfo.Instance)
			   {
					mtl.SetFloat("_FogEndDis",RenderSettings.fogEndDistance * CS_SceneInfo.Instance.NpcRangeFactor);
			   }
			}
		}
	}
	
	//Just calculate damage, don't destroy object when hp is less than 0. But destroy when server asks [OnObjectLeave function in CS_SceneInfo.cs].
    public override void TakeDamage(int damage)
    {
        AttrMan.Attrs[EAttributeType.ATTR_CurHP] += damage;

        if (AttrMan.Attrs[EAttributeType.ATTR_CurHP] > AttrMan.Attrs[EAttributeType.ATTR_MaxHP])
            AttrMan.Attrs[EAttributeType.ATTR_CurHP] = AttrMan.Attrs[EAttributeType.ATTR_MaxHP];

        if(AttrMan.Attrs[EAttributeType.ATTR_CurHP] > 0)
		{
            PlayImpactAnim();
			PlayImpactSound();
		    PlayImpactFX();
		}
    }

    public override void GoToHell()
    {
        base.GoToHell();
		
        IsUsed = true;
        PlayDeathAnim();
        PlayBreakSound();
        PlayDeathFX();

        if (Player.Instance.AttackTarget == transform)
            Player.Instance.AttackTarget = null;

        foreach (SpawnCondition it in LinkedEvents.DisableObjects)
        {
            if (it.Spawner.collider != null && it.Spawner.GetComponent<TriggerBase>() == null)
                it.Spawner.collider.isTrigger = false;
        }

        foreach (SpawnCondition it in LinkedEvents.ToggleObjects)
        {
            if (it.Spawner.collider != null && it.Spawner.GetComponent<TriggerBase>() == null)
                it.Spawner.collider.isTrigger = !it.Spawner.collider.isTrigger;
        }

        if (DestroyOnDeath)
        {
            Destroy(gameObject);
        }
        else
        {
            if (DestroyCollision)
            {
                if (transform.collider)
                    Destroy(transform.collider);
            }
        }
    }
	
    void PlayBreakSound()
	{	
		if(myDeathSound==null && DeathSound!=null)
		{
			myDeathSound = Instantiate(DeathSound) as Transform;
		}
		
		if(myDeathSound!=null)
		{
			myDeathSound.position = transform.position;
			myDeathSound.rotation = transform.rotation;
			SoundCue.Play(myDeathSound.gameObject);
		}
	}
	
    void PlayImpactSound()
	{
		if(myImpactSound==null && ImpactSound!=null)
		{
			myImpactSound = Instantiate(ImpactSound) as Transform;
		}
		
		if(myImpactSound!=null)
		{
			myImpactSound.position = transform.position;
			myImpactSound.rotation = transform.rotation;
			SoundCue.Play(myImpactSound.gameObject);
		}
	}
	
	void PlayImpactFX()
	{
		if(ImpactFX)
		{
			Transform FXinst =  Object.Instantiate(ImpactFX,transform.position,transform.rotation) as Transform;
            if (ImpactAndDeathVFXPos)
            {
                FXinst.position = ImpactAndDeathVFXPos.position;
                FXinst.rotation = ImpactAndDeathVFXPos.rotation;
            }
			
			if(FXinst != null)
			   FXinst.localScale = transform.localScale;
		}
	}
	
	void PlayDeathFX()
	{
		if(DeathFX)
		{
			Transform FXinst =  Object.Instantiate(DeathFX,transform.position,transform.rotation) as Transform;
            if (ImpactAndDeathVFXPos)
            {
                FXinst.position = ImpactAndDeathVFXPos.position;
                FXinst.rotation = ImpactAndDeathVFXPos.rotation;
            }

			if(FXinst != null)
			   FXinst.localScale = transform.localScale;
		}
	}
	
	void PlayDeathAnim()
	{
		if( DeathAnim != null){
			//Debug.Log("open");
		   	transform.animation.CrossFade(DeathAnim.name);
		}
	}
	
	public virtual void PlayImpactAnim()
	{
		if( ImpactAnim != null)
			transform.animation.Play(ImpactAnim.name); //CrossFade(ImpactAnim.name);
	}

    #region Export

    public override string DoExport()
	{
		XMLStringWriter xmlWriter = new XMLStringWriter();
		
		xmlWriter.NodeBegin("BreakableActor");
		
		xmlWriter.AddAttribute("ID",TypeID);
		
		xmlWriter.AddAttribute("LootLevel",LootLevel);
	
		xmlWriter.AddAttribute("Exp",Exp);
		
		xmlWriter.AddAttribute("Health",Health);
		
		xmlWriter.AddAttribute("Radius",radius);
		
		xmlWriter.AddAttribute("ApearChance",AppearChance);
		
		ReaLKarmaValue = (int)Random.Range(MinKarmaValue,MaxKarmaValue);
		
		xmlWriter.AddAttribute("Karma",ReaLKarmaValue);

        for (int i = 0; i < KarmaPerfabs.Length; i++)
        {
            xmlWriter.NodeBegin("KarmaGenerate");

            xmlWriter.AddAttribute("KarmaID", (int)KarmaPerfabs[i].GetComponent<KarmaController>().KarmaType);

            xmlWriter.AddAttribute("Value", 1);

            xmlWriter.AddAttribute("HealingAmount", 9);

            xmlWriter.NodeEnd("KarmaGenerate");
        }
	
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
			
	    xmlWriter.AddAttribute("PosX",transform.position.x);
		
	    xmlWriter.AddAttribute("PosY",transform.position.y);
		
	    xmlWriter.AddAttribute("PosZ",transform.position.z);
		
		xmlWriter.AddAttribute("rotX",transform.eulerAngles.x);
		
		xmlWriter.AddAttribute("rotY",transform.eulerAngles.y);
		
		xmlWriter.AddAttribute("rotZ",transform.eulerAngles.z);
		
		xmlWriter.AddAttribute("scaleX",transform.lossyScale.x);
		
		xmlWriter.AddAttribute("scaleY",transform.lossyScale.y);
		
		xmlWriter.AddAttribute("scaleZ",transform.lossyScale.z);
		
		xmlWriter.AddAttribute("ApearChance",AppearChance);
		
		if(CollidderAreaSpawnerList.Count > 0)
		{
			foreach( NpcSpawner it in CollidderAreaSpawnerList)
			{
				xmlWriter.NodeBegin("ColliderSpawner");
				xmlWriter.AddAttribute("ColliderSpawnerID",it.id);
				xmlWriter.NodeEnd("ColliderSpawner");
			}
			
		}
	
		foreach( SpawnCondition it in LinkedEvents.EnableSpawnerList)
		{
		    if( it == null || it.Spawner == null) 
				continue;
			
			if(it.Spawner.GetComponent<NpcSpawner>() == null) 
		        continue;
		   
			xmlWriter.NodeBegin("SpawnResult");
			
			xmlWriter.AddAttribute("DelayTime",it.DelayTime);
			xmlWriter.AddAttribute("ID",it.Spawner.GetComponent<NpcSpawner>().id);
			
			xmlWriter.NodeEnd("SpawnResult");
		}
		
		foreach( SpawnCondition it in LinkedEvents.DisableSpawnerList)
		{
			if( it == null || it.Spawner == null) 
				continue;
			
			if(it.Spawner.GetComponent<NpcSpawner>() == null)
			    continue;
			
			xmlWriter.NodeBegin("DisableSpawnResult");
			
			xmlWriter.AddAttribute("DelayTime",it.DelayTime);
			xmlWriter.AddAttribute("ID",it.Spawner.GetComponent<NpcSpawner>().id);
			
			xmlWriter.NodeEnd("DisableSpawnResult");
		}
		
		if(TypeID == 4001)
		{
			Debug.Log("4001");
		}
		
		foreach( SpawnCondition it in LinkedEvents.DisableObjects)
		{
			if( it == null || it.Spawner == null) 
				continue;
			
			if(it.Spawner.GetComponent<TriggerBase>() == null && it.Spawner.GetComponent<Trigger_Unified>() == null)
			    continue;
			
			xmlWriter.NodeBegin("DisableTrigger");
			
			xmlWriter.AddAttribute("DelayTime",it.DelayTime);
			if(it.Spawner.GetComponent<TriggerBase>())
				xmlWriter.AddAttribute("ID",it.Spawner.GetComponent<TriggerBase>().id);
			else if(it.Spawner.GetComponent<Trigger_Unified>())
				xmlWriter.AddAttribute("ID",it.Spawner.GetComponent<Trigger_Unified>().UnityID);
			else
				xmlWriter.AddAttribute("ID",-1);
			
			xmlWriter.NodeEnd("DisableTrigger");
			
		}
		
		foreach( SpawnCondition it in LinkedEvents.ToggleObjects)
		{
			if( it == null || it.Spawner == null) 
				continue;
			
			if(it.Spawner.GetComponent<TriggerBase>() == null && it.Spawner.GetComponent<Trigger_Unified>() == null)
			    continue;
			
			xmlWriter.NodeBegin("ToggleTrigger");
			
			xmlWriter.AddAttribute("DelayTime",it.DelayTime);
			
			if(it.Spawner.GetComponent<TriggerBase>())
				xmlWriter.AddAttribute("ID",it.Spawner.GetComponent<TriggerBase>().id);
			else if(it.Spawner.GetComponent<Trigger_Unified>())
				xmlWriter.AddAttribute("ID",it.Spawner.GetComponent<Trigger_Unified>().UnityID);
			else
				xmlWriter.AddAttribute("ID",-1);
			
			xmlWriter.NodeEnd("ToggleTrigger");
		}
		
		foreach( PlayAnimTrigger.AnimationDataStruct it in AnimDataArrays)
		{
		   if( it == null) 
				continue;
		   if(it.AnimObj == null)
				continue;
			
		   xmlWriter.NodeBegin("AnimationInfo");
		   xmlWriter.AddAttribute("id",it.AnimObj.id + TypeID);
		   xmlWriter.AddAttribute("realID",it.AnimObj.id);
		   xmlWriter.AddAttribute("Loop",it.IsLoopAnim);
		   xmlWriter.AddAttribute("AnimName",it.anim.name);
		   xmlWriter.AddAttribute("AnimDelayTime",it.AnimDelayTime);
		   xmlWriter.AddAttribute("bCollision",it.bCollision);
		   xmlWriter.AddAttribute("bCanShow",true);
		   xmlWriter.NodeEnd("AnimationInfo");
		  
		}
		
	
		xmlWriter.NodeEnd("MapBreakable");
		
		return xmlWriter.Result;
    }

    #endregion
}
