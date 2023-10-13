using UnityEngine;
using System.Collections;

public class DeleteNPCAutomatically : MonoBehaviour {

    public NpcBase npc;
    public float timer = 5;

    void Update()
    {
        timer -= Time.deltaTime;
        if (timer < 0)
        {
            if (npc.FSM.IsInState(npc.DS))
            {
                Destroy(this);
            }
            else
            {
                npc.FSM.ChangeState(npc.DS);
            }
            Destroy(this);

            //CS_SceneInfo.Instance.RemoveMonsterByID(npc.ObjID);
            //
			//Destroy(npc.gameObject);
        }
    }

}
