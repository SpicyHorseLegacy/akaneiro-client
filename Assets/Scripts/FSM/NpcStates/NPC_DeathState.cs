using UnityEngine;
using System.Collections;

public class NPC_DeathState : NpcState {
	
	Transform DeathSound;
	float FadeValue=0f;

	public override void Enter()
	{
		//Debug.Log("entry death state.");
        // remove from player all enemy list which player finds enmey from.
		if(Player.Instance.AllEnemys.IndexOf(Owner)!= -1)
		{
			Player.Instance.AllEnemys.Remove(Owner);
			if(npc.AroundIndex!=-1)
			{
				Player.Instance.AllAroundPos[npc.AroundIndex]=false;
				npc.AroundIndex=-1;
			}
		}
        if (Player.Instance.AttackTarget == Owner)
        {
            Player.Instance.AttackTarget = null;
        }
		
		//play death sound
		npc.CommonKilledSound = npc.PlaySound(Owner.GetComponent<NpcSoundEffect>().CommonKilledSoundPrefab,npc.CommonKilledSound);
		npc.DeathSound = npc.PlaySound(Owner.GetComponent<NpcSoundEffect>().DeathSoundPrefab,npc.DeathSound);
        if (npc.DeathSound != null)
        {
            npc.DeathSound.transform.parent = null;
            npc.DeathSound.gameObject.AddComponent<DestructAfterTime>();
			DestructAfterTime.DestructGameObjectAfterTime(npc.DeathSound.transform, npc.DeathSound.audio.clip.length);
        }
		
		if(Owner.collider)
			Object.Destroy(Owner.collider);
		
		npc.BuffMan.RemoveAllBuffs();
		
		npc.PlayDeathAnim();

        if (Owner.GetComponent<NpcCreateModel>().WantedVFX_Foot_Instance)
        {
            if (Owner.GetComponent<NpcCreateModel>().WantedVFX_Foot_Instance.GetComponentInChildren<DestroyAfterFadeOut>())
                Owner.GetComponent<NpcCreateModel>().WantedVFX_Foot_Instance.GetComponentInChildren<DestroyAfterFadeOut>().GoToHell();

            //Owner.GetComponent<NpcCreateModel>().WantedVFX_Foot_Instance.gameObject.AddComponent<DestroyAfterTimeWithSpeed>();
            //Owner.GetComponent<NpcCreateModel>().WantedVFX_Foot_Instance.GetComponent<DestroyAfterTimeWithSpeed>().time = 1;
            //Owner.GetComponent<NpcCreateModel>().WantedVFX_Foot_Instance.GetComponent<DestroyAfterTimeWithSpeed>().Speed = Vector3.up * -1;
            //Owner.GetComponent<NpcCreateModel>().WantedVFX_Foot_Instance.GetComponent<DestroyAfterTimeWithSpeed>().MoveObjs.Add(Owner.GetComponent<NpcCreateModel>().WantedVFX_Foot_Instance);
        }

        if (CS_SceneInfo.Instance)
            CS_SceneInfo.Instance.RemoveMonsterByID(npc.ObjID);
	}
	
	public override void Execute()
	{
		if(npc.IsDeathAnimFinished())
		{
			if(npc.IsBoss)
			{
				Renderer NpcRenderer =  Owner.GetComponentInChildren<Renderer>();
				if(NpcRenderer)
				{
					for(int i = 0; i < NpcRenderer.materials.Length;i++)
					{
						Material mtl =  NpcRenderer.materials[i];
				    	if(mtl.HasProperty("_FadeValue"))
				        {
							FadeValue += Time.deltaTime;
							FadeValue = Mathf.Clamp01(FadeValue);
				           	mtl.SetFloat("_FadeValue",FadeValue);
				        }
					}
					NpcRenderer=null;
				}
				
				if(FadeValue>=0.9999f)
				{
					npc.DeathAnimationFinished();
				}
			}
			else
			{
				//particle
				if(npc.DeathEffect && npc.DeathEffectInst==null)
				{
					Vector3 DeathParticlePos = Owner.position;
					Component[] all = Owner.GetComponentsInChildren<Component>();
					foreach(Component T in all)
					{
						if(T.name == "Bip001")
						{
							DeathParticlePos = T.transform.position;
						}
					}
					
					
					npc.DeathEffectInst = Object.Instantiate(npc.DeathEffect,DeathParticlePos,Owner.rotation) as Transform;
				}

				npc.DeathAnimationFinished();
			}
		}
	}
}
