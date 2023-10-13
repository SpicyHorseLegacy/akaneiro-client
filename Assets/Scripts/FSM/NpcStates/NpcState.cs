using UnityEngine;
using System.Collections;

public class NpcState : State {
	
	public NpcBase npc;
	
	public virtual void SetNPC(NpcBase _npc)
	{
		Owner = _npc.transform;
		npc = _npc;
	}
	
}
