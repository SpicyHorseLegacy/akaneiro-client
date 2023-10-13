using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class NPC_ChaseState : NpcState 
{
	Transform TransformTarget = null;
	
	public List<Vector3> LastMoveTarget = new List<Vector3>();
	
	public float StateSpeed = 0;
	float SpeedModifier = 1f;
    float nextChasingSound = 0;                 // in chasing mode, play chasing sound every  between 5 to 10 sec randomly.

    bool firstTimeFindPlayer = true;            // every time enter this state, check if it's the first time that getting player, play alarm sound. and never play again
	
	public override void SetNPC (NpcBase _npc)
	{
		base.SetNPC (_npc);
		
		StateSpeed = npc.AttackState.SpeedAttackState;
		
		npc.bFindPath=false;
		npc.bReachTargetPoint=false;
	}
	
	public override void Enter()
	{
		//print("enter chaseing state");
	
		if(Player.Instance.AllEnemys.IndexOf(Owner)== -1)
		{
			Player.Instance.AllEnemys.Add(Owner);
		}

		npc.PlayRunAnim();
		
		if(npc.AroundIndex !=-1 )
		{
			Player.Instance.AllAroundPos[npc.AroundIndex]=false;		
			npc.AroundIndex = -1;
		}
		//Debug.Log("entry Chase state.");
		
		npc.bReachTargetPoint = false;
		npc.curPathPointIndex=0;

        // alarm sound
        if (firstTimeFindPlayer && npc.GetComponent<NpcSoundEffect>().AlarmSoundPrefab)
        {
            //if(Random.Range(0, 100) > 50)
                SoundCue.PlayPrefabAndDestroy(npc.GetComponent<NpcSoundEffect>().AlarmSoundPrefab, npc.transform.position);
            firstTimeFindPlayer = false;
        }
#if NGUI
#else
        if (!_UI_CS_DebugInfo.Instance.AttackPlayerEnemies.Contains(npc))
        {
            _UI_CS_DebugInfo.Instance.AttackPlayerEnemies.Add(npc);
        }
#endif
        nextChasingSound = Random.Range(5.0f, 10.0f);
	}
	
	public override void Execute()
	{
		float tempSpeed = StateSpeed * SpeedModifier;
		
	    npc.MoveOnPath(LastMoveTarget,tempSpeed);
		
        nextChasingSound -= Time.deltaTime;
		
        if (nextChasingSound < 0)
        {
            SoundCue.PlayPrefabAndDestroy(npc.GetComponent<NpcSoundEffect>().AttackIdeSoundPrefab, npc.transform.position);
            nextChasingSound = Random.Range(5.0f, 10.0f);
        }
	}
	
	public void AdjustPosition()
	{
		RaycastHit hit;
				
        int layer = 1<<LayerMask.NameToLayer("Walkable");
		
		for(int i = 0; i < LastMoveTarget.Count;i++)
		{
			Vector3 temp = LastMoveTarget[i];
		
		    temp.y = 100;
		
            if(Physics.Raycast(temp,Vector3.down,out hit,100f,layer))
			{
		       temp.y = hit.point.y;
			}
			LastMoveTarget[i] = temp;
		}
	}
	
	public override void Exit()
	{
		//print("exit chaseing state");
		//stop move on path
		
		npc.bReachTargetPoint = true;
		npc.bFindPath=false;
		RestoreSpeed();
#if NGUI
#else
        if (_UI_CS_DebugInfo.Instance.AttackPlayerEnemies.Contains(npc))
        {
            _UI_CS_DebugInfo.Instance.AttackPlayerEnemies.Remove(npc);
        }
#endif
	}
	
	public void RestoreSpeed()
	{
		StateSpeed = npc.AttackState.SpeedAttackState;
		
		SpeedModifier = 1f;
		
		if(npc.RunAnim != null)
		{
			npc.AnimationModel.animation[npc.RunAnim.name].speed = 1f;
		}
		
	}
	
	public void ChangSpeed(float ftoss,bool bModifier)
	{
		if(bModifier)
		{
		   SpeedModifier = ftoss;
		}
		else
		{
		   StateSpeed = npc.AttackState.SpeedAttackState * ftoss;
		}
		
		if(npc.RunAnim != null && npc.AttackState.SpeedAttackState > 0f )
		{
		   float tempToss = StateSpeed / npc.AttackState.SpeedAttackState;
		   
		   tempToss *= SpeedModifier;
			
		   npc.AnimationModel.animation[npc.RunAnim.name].speed = tempToss;
			
		   //Debug.Log("Monster " + npc.ObjID.ToString() +  "RunSpeed rate is " + tempToss.ToString());
		}		
	}

}
