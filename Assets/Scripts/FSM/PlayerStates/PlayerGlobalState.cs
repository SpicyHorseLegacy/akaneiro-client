using UnityEngine;
using System.Collections;

public class PlayerGlobalState : PlayerState 
{

    [HideInInspector]   public bool IsSkillOn = false;			// 是否正有技能在释放，用于控制可以在跑动中释放的技能，跑动完后进入技能准备，比如“冲刺”技能，

    [HideInInspector]   public bool IsHoldingLMB = false;       // after ability which is actived in melee attack mode finished, if player is still holding LMB, back to normal attack state.

	public override void Enter()
	{
		Owner = Player.Instance.transform;
	}
	
	public override void Execute()
	{
        if (Player.Instance.AttrMan.Attrs[EAttributeType.ATTR_CurHP] > 0f)
		{
			PlayerAttributionManager attrMan = (PlayerAttributionManager) Player.Instance.AttrMan;
            attrMan.RegenHPAndMP();
		}

        if (DrawOutline.Instance) DrawOutline.Instance.Execute();

        if (Input.GetMouseButtonUp(0)) IsHoldingLMB = false;
	}
}
