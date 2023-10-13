using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NPC_SpawnState : NpcState {
	
	public Vector3 LastMovePosition = Vector3.zero;
	
	public bool bMove = false;
	
	float mfSpeed = 0f;
	
	public NPC_SpawnState()
	{
	}
	
	public NPC_SpawnState(NpcBase o)
	{
		Owner=o.transform;
		npc = o;
	}
	
	public override void Enter()
	{
		npc.PlaySpawnAnim();
		
		if( npc.IsBoss)
		{
		    npc.BossSpawnSound = npc.PlaySound(npc.GetComponent<NpcSoundEffect>().BossSpawnnSoundPrefab,npc.BossSpawnSound);
		}
	
		StartCoroutine( delayShow());
	
	}
	
	public override void Execute()
	{
	
		if(bMove)
	      npc.SpawnOnPath(LastMovePosition,mfSpeed);
		
		
		if(!npc.AnimationModel.animation.isPlaying || npc.SpawnAnims==null)
		{ 
			 //Debug.Log("Monster " + npc.ObjID.ToString() + " Speed is " + mfSpeed.ToString()  + "SpawnAnimation end");
			 npc.PlayIdleAnim();
		     bMove = false;	
			 
		}
	}
	
	public override void Exit()
	{
		bMove = false;
		
		Renderer[] NpcRenderers = Owner.GetComponentsInChildren<Renderer>();
	    foreach(Renderer NpcRenderer in NpcRenderers)
		{
		  NpcRenderer.enabled = true;
	    }
	
	    int layer = 1 << LayerMask.NameToLayer("Player");
		
		Collider[] colliders;
		
	    colliders = Physics.OverlapSphere(Owner.position, 1f, layer);
		
	    if(colliders.Length > 0)
		{
			 if(Player.Instance != null && Player.Instance.GetComponent<PlayerMovement>()!= null)
		     {
					if(Owner.collider != null)
					{
						bool bRepeat = false;
						foreach(Transform tran in  Player.Instance.GetComponent<PlayerMovement>().togetherObjs)
						{
							if(tran == Owner)
							{
								bRepeat = true;
								break;
							}	
						}
						if(!bRepeat)
						{
						  Owner.collider.isTrigger = true;
					      Player.Instance.GetComponent<PlayerMovement>().togetherObjs.Add(Owner);
						  //Debug.Log("monster " + npc.ObjID + " is too near to player");
						}
					}
		     }
	    }
   }
	
	
	public void SetSpeed(float speed)
	{
		mfSpeed = speed;
	}
	
	IEnumerator  delayShow()
	{
		yield return new WaitForSeconds(0.1f);
		
		Renderer[] NpcRenderers = Owner.GetComponentsInChildren<Renderer>();
	    foreach(Renderer NpcRenderer in NpcRenderers)
		{
		  NpcRenderer.enabled = true;
	    }
	}
	
}
