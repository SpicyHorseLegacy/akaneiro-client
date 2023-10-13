using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SelfAnimation : BaseExportNode {

	// Use this for initialization
	public int id = 0;
	//public Collider pCollider = null;
	bool bIsAnimation = false;
	
	class AnimationStatus
	{
	   public string AnimName;
	   public float DelayTime = 0;
	}
	
	public Collider[] PartnerColliders = new Collider[0];
	
	List<AnimationStatus> m_Animque= new List<AnimationStatus>();
	
	
	public Transform SoundPrefab = null;
	
	Transform SoundInst = null;
	
	void Start () {
	
	}
	
	Transform PlaySound(Transform prefab, Transform sound)
	{
		if(sound==null && prefab!=null)
		{
			sound = Instantiate(prefab) as Transform;
			//Parenting to make sure the sound gets deleted when the npc gets deleted.
			//If this is undesired in some cases then remove this line but then
			//you MUST delete the sounds another way.
			sound.parent = this.transform;
		}
		
		if(sound!=null)
		{
			sound.position = transform.position;
			sound.rotation = transform.rotation;
			SoundCue.Play(sound.gameObject);
		}
		
		return sound;
	}
	
	// Update is called once per frame
	void Update () {
	
		if(CS_SceneInfo.Instance != null)
		{
			if(CS_SceneInfo.Instance.IsTriggerAnimation(id))
			{
				CS_SceneInfo.TriggerAnimData pAnimaData = CS_SceneInfo.Instance.GetTriggerAnimation(id);
				if( pAnimaData.Aniname.Length > 0)
				{
				   transform.animation[pAnimaData.Aniname].layer = -1;
				   if( pAnimaData.IsLoopAnim)
				      transform.animation[pAnimaData.Aniname].wrapMode = WrapMode.Loop;
				   else
			          transform.animation[pAnimaData.Aniname].wrapMode = WrapMode.Once;
				
				   AnimationStatus animStatus = new AnimationStatus();
				   animStatus.AnimName = pAnimaData.Aniname;
				   animStatus.DelayTime = pAnimaData.AnimDelayTime;
				
				   m_Animque.Add(animStatus);
				}
				
			    Renderer[] renderers = gameObject.GetComponentsInChildren<Renderer>();
				
				if(pAnimaData.bDisplay && renderers != null)
				{
				   foreach(Renderer it in renderers)
						it.enabled = true;
					
				   
				}
				else if(!pAnimaData.bDisplay && renderers != null)
				{
					 foreach(Renderer it in renderers)
						it.enabled = false;
				}
				
				
				
				if(pAnimaData.bCollision)
				{   
					if(gameObject.GetComponent<Collider>() == null)
					   gameObject.AddComponent<BoxCollider>();
						
					if(transform.collider != null)
					{
					   transform.collider.enabled = true;
					   transform.collider.isTrigger = false;
						
					}
					
					foreach(Collider it in PartnerColliders)
					{
						it.enabled = true;
						it.isTrigger = false;
					}
				}
				else
				{
					if(transform.collider != null)
					{
					   transform.collider.enabled = false;
					   transform.collider.isTrigger = true;
					}
					
					foreach(Collider it in PartnerColliders)
					{
						it.enabled = false;
						it.isTrigger = true;
						
					}
				}
				
			
				CS_SceneInfo.Instance.RemoveTriggerAnimation(id);
			}
		}
		
	
		for(int i = 0; i < m_Animque.Count;i++)
		{
			m_Animque[i].DelayTime -= Time.deltaTime;
			if(m_Animque[i].DelayTime <= 0)
			{
				if(i == 0 )
				{
					SoundInst = PlaySound(SoundPrefab,SoundInst);
				}
				bIsAnimation = true;
				transform.animation.Play(m_Animque[i].AnimName);
				m_Animque.RemoveAt(i);
				break;
			}
			
		}
		
		if( bIsAnimation)
		{
		    if( !transform.animation.isPlaying)
			{
				if(transform.collider)
				   transform.collider.isTrigger = true;
				bIsAnimation = false;
			}
		}
			
	}
	
	public override string DoExport()
	{	
		XMLStringWriter xmlWriter = new XMLStringWriter();
		
		xmlWriter.NodeBegin("GamDoor");
		
		xmlWriter.AddAttribute("ID",id);
		
	    xmlWriter.AddAttribute("PosX",transform.position.x);
		
	    xmlWriter.AddAttribute("PosY",transform.position.y);
		
		xmlWriter.AddAttribute("PosZ",transform.position.z);
		
		xmlWriter.NodeEnd("GamDoor");
		
	
		return xmlWriter.Result;
	}
}
