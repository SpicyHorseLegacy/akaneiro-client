using UnityEngine;
using System.Collections;

public class NPC_SleepState : NpcState {
	
	public override void Enter()
	{
		npc.PlaySleepAnim();
	}
	
	public override void Execute()
	{
	}
	
	public override void Exit()
	{
        if (Owner.GetComponent<NpcSoundEffect>().SleepWakeUpSoundPrefab)
        {
            SoundCue.PlayAtPosAndRotation(Owner.GetComponent<NpcSoundEffect>().SleepWakeUpSoundPrefab.gameObject, transform.position, transform.rotation);
        }
	}
}
