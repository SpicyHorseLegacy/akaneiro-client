using UnityEngine;
using System.Collections;

public class PlayerAttributionManager : AttributionManager
{
    public float nextRegenTime = 0;

    void Awake()
    {
        Attrs[EAttributeType.ATTR_MoveSpeed] = 100;
        Attrs[EAttributeType.ATTR_AttackSpeed] = 100;
    }

    public override void UpdateAttrs(vectorAttrChange playerAttrVec)
    {
        base.UpdateAttrs(playerAttrVec);

        if (Player.Instance.AttrMan.Attrs[EAttributeType.ATTR_CurHP] <= 0 && !Player.Instance.FSM.IsInState(Player.Instance.DS))
            Player.Instance.FSM.ChangeState(Player.Instance.DS);
#if NGUI
        if (UI_Hud_HPMPBar_Manager.Instance)
        {
            UI_Hud_HPMPBar_Manager.Instance.UpdateHPMP(Attrs[EAttributeType.ATTR_CurHP], Attrs[EAttributeType.ATTR_MaxHP], Attrs[EAttributeType.ATTR_CurMP], Attrs[EAttributeType.ATTR_MaxMP]);
        }
#else		
		if(_UI_CS_ScreenCtrl.Instance.IsScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_INGAME_MENU_INFO)){
			_PlayerData.Instance.UpdatePlayerInfo();
		}
#endif		
    }

    public void RegenHPAndMP()
    {
        if (Time.time > nextRegenTime)
        {
            Attrs[EAttributeType.ATTR_CurHP] += Attrs[EAttributeType.ATTR_HPRecover];
            if (Attrs[EAttributeType.ATTR_CurHP] > Attrs[EAttributeType.ATTR_MaxHP])
                Attrs[EAttributeType.ATTR_CurHP] = Attrs[EAttributeType.ATTR_MaxHP];

            Attrs[EAttributeType.ATTR_CurMP] += Attrs[EAttributeType.ATTR_MPRecover];
            if (Attrs[EAttributeType.ATTR_CurMP] > Attrs[EAttributeType.ATTR_MaxMP])
                Attrs[EAttributeType.ATTR_CurMP] = Attrs[EAttributeType.ATTR_MaxMP];

#if NGUI
            if (UI_Hud_HPMPBar_Manager.Instance)
            {
                UI_Hud_HPMPBar_Manager.Instance.UpdateHPMP(Attrs[EAttributeType.ATTR_CurHP], Attrs[EAttributeType.ATTR_MaxHP], Attrs[EAttributeType.ATTR_CurMP], Attrs[EAttributeType.ATTR_MaxMP]);
            }
#else
			if(_UI_CS_ScreenCtrl.Instance.IsScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_INGAME_MENU_INFO)){
				_PlayerData.Instance.UpdatePlayerInfo();
			}
#endif			
            nextRegenTime += 0.5f;
        }
    }
	
}
