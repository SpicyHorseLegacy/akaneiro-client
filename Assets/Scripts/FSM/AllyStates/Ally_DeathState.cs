using UnityEngine;
using System.Collections;

public class Ally_DeathState : State {

    AllyNpc Executer;
	float fBegining = 0f;
	
	public  void SetAlly(AllyNpc o)
	{
		Executer = o;
		Owner = Executer.transform;
	}
	
	public override void Enter()
	{
		Owner = Executer.transform;
		
		Executer.PlayDeathAnim();
		Executer.IsDead =true;
		
		fBegining = Time.time;
	}
	
	public override void Execute()
	{
		if(Time.time - fBegining > 5f)
		{
			DeleteAlly();
			//Debug.Log("ally forced to be delete");
			return;
		}
		
		if(Executer.AnimationModel != null)
		{
			if(!Executer.AnimationModel.animation.isPlaying)
			{
				DeleteAlly();
			}
		}
	}
	
	public override void Exit()
	{
	
	}
	
	void DeleteAlly()
	{
		for(int i = 0; i < CS_SceneInfo.Instance.AllyNpcList.Count;i++)
		{
		   if(CS_SceneInfo.Instance.AllyNpcList[i].ObjID == Executer.ObjID)
		   {
			  CS_SceneInfo.Instance.AllyNpcList.RemoveAt(i);
			  break;
		   }
	    }
        if (Executer.DeathEffect)
        {
            Vector3 DeathParticlePos = Owner.position;
            Component[] all = Owner.GetComponentsInChildren<Component>();
            foreach (Component T in all)
            {
                if (T.name == "Bip001")
                {
                    DeathParticlePos = T.transform.position;
                }
            }


            Object.Instantiate(Executer.DeathEffect, DeathParticlePos, Owner.rotation);
        }
        Destroy(Executer.gameObject);
	}
}
