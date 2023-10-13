using UnityEngine;
using System.Collections;

public class InGameScreenHudCtrl : MonoBehaviour
{
    public void RegisterSingleTemplateEvent(string _templateName)
    {
        if (_templateName == "GameHud_AllyInfoGroup" && UI_Hud_AllyGroup_Manager.Instance)
        {
            UI_Hud_AllyGroup_Manager.Instance.UI_Hud_AllyInfoGroupAddAllyBTN_Clicked_Event += new UI_Hud_AllyGroup_Manager.Handle_Hud_AllyInfoGroupAddAllyBTN_Clicked_Delegate(AddAllyBTNClicked);
        }

        if (_templateName == "GameHud_AbilitySlots" && UI_Hud_AbilitySlot_Manager.Instance)
        {
            UI_Hud_AbilitySlot_Manager.Instance.UI_GameHud_AbiSlot_Click_Event += new UI_Hud_AbilitySlot_Manager.Handle_GameHud_AbiSlots_ClickIcon_Delegate(UI_GameHud_AbiSlot_Clicked);
            UI_Hud_AbilitySlot_Manager.Instance.UI_GameHud_AbiSlot_StartDraging_Event += new UI_Hud_AbilitySlot_Manager.Handle_GameHud_AbiSlots_StartDragging_Delete(ShowDragItem);
            UI_Hud_AbilitySlot_Manager.Instance.UI_GameHud_AbiSlot_UpdateDraging_Event += new UI_Hud_AbilitySlot_Manager.Handle_GameHud_AbiSlots_UpdateDragging_Delete(UpdateDragItemPosition);
            UI_Hud_AbilitySlot_Manager.Instance.UI_GameHud_AbiSlot_ExitDraging_Event += new UI_Hud_AbilitySlot_Manager.Handle_GameHud_AbiSlots_ExitDragging_Delete(DeleteDragItem);
            UI_Hud_AbilitySlot_Manager.Instance.UI_GameHud_AbiSlot_HoveringSlot_Event += new UI_Hud_AbilitySlot_Manager.Handle_GameHud_AbiSlots_HoveringSlot_Delete(HoverSlot);
        
			if(PlayerDataManager.Instance.isExistMissionList) {
	            GUIManager.Instance.AddTemplate("MissionObjective");
	            GUIManager.Instance.AddTemplate("XPBar");
	            GUIManager.Instance.AddTemplate("GameHud_KillChain");
			}
		}

        if (_templateName == "GameHud_BuffBar" && UI_Hud_BuffBar_Manager.Instance)
        {
            InitBuffs();
        }
	}

    public void UnregisterSingleTemplateEvent(string _templateName)
    {
        if (_templateName == "GameHud_AllyInfoGroup" && UI_Hud_AllyGroup_Manager.Instance)
        {
            UI_Hud_AllyGroup_Manager.Instance.UI_Hud_AllyInfoGroupAddAllyBTN_Clicked_Event -= new UI_Hud_AllyGroup_Manager.Handle_Hud_AllyInfoGroupAddAllyBTN_Clicked_Delegate(AddAllyBTNClicked);
        }

        if (_templateName == "GameHud_AbilitySlots" && UI_Hud_AbilitySlot_Manager.Instance)
        {
            UI_Hud_AbilitySlot_Manager.Instance.UI_GameHud_AbiSlot_Click_Event -= new UI_Hud_AbilitySlot_Manager.Handle_GameHud_AbiSlots_ClickIcon_Delegate(UI_GameHud_AbiSlot_Clicked);
            UI_Hud_AbilitySlot_Manager.Instance.UI_GameHud_AbiSlot_StartDraging_Event -= new UI_Hud_AbilitySlot_Manager.Handle_GameHud_AbiSlots_StartDragging_Delete(ShowDragItem);
            UI_Hud_AbilitySlot_Manager.Instance.UI_GameHud_AbiSlot_UpdateDraging_Event -= new UI_Hud_AbilitySlot_Manager.Handle_GameHud_AbiSlots_UpdateDragging_Delete(UpdateDragItemPosition);
            UI_Hud_AbilitySlot_Manager.Instance.UI_GameHud_AbiSlot_ExitDraging_Event -= new UI_Hud_AbilitySlot_Manager.Handle_GameHud_AbiSlots_ExitDragging_Delete(DeleteDragItem);
        }
		
		if (_templateName == "GameHud_BuffBar" && UI_Hud_BuffBar_Manager.Instance)
        {
            RemoveAllBuffs();
        }
    }

    void InitBuffs()
    {
        for (int i = 0; i < Player.Instance.BuffMan.Buffs.Count; i++)
        {
            UI_Hud_BuffBar_Manager.Instance.UpdateBuff(Player.Instance.BuffMan.Buffs[i]);
        }
    }
	
	void RemoveAllBuffs()
	{
		UI_Hud_BuffBar_Manager.Instance.RemoveAllBuffs();
	}

    #region Delegate from AllyInfoGroup

    void AddAllyBTNClicked()
    {
        // call summon ally screen;
    }

    #endregion

    #region Delegate from ability slot
	
	public void StartCoolDown(int _abiid, float _cooldown)
	{
		if(UI_Hud_AbilitySlot_Manager.Instance)
			UI_Hud_AbilitySlot_Manager.Instance.CoolDown(_abiid,_cooldown);
	}
	
    void UI_GameHud_AbiSlot_Clicked(UI_Hud_AbilitySlot_Single_Control _slot, bool _isLeftOrRightKey)
    {
        //Player.Instance.abilityManager.UseAbilityFromID(_slot._AbiData.AbiID);
		PlayerAbilityManager _abiManager = (PlayerAbilityManager) Player.Instance.abilityManager;
		_abiManager.UseAbility(_slot._AbiData.AbiID, false);
    }

    void ShowDragItem(UI_Hud_AbilitySlot_Single_Control _slot, Vector3 _mousepos)
    {
        if (UI_Hud_DragItem_Manager.Instance && _slot && _slot._AbiData != null)
        {
            UI_TypeDefine.UI_GameHud_DragItem_data _newdata = new UI_TypeDefine.UI_GameHud_DragItem_data(_slot._AbiData.IconSpriteName);
            _newdata.ItemType = UI_TypeDefine.UI_GameHud_DragItem_data.EnumDragItemType.AbilitySlot;
            _newdata.ItemColor = _slot._AbiData.AbiColor;
            _newdata.Param = _slot._AbiData;
			_newdata.AbiID = _slot._AbiData.AbiID;
            UI_Hud_DragItem_Manager.Instance.ShowItem(_newdata);
            UI_Hud_DragItem_Manager.Instance.UpdatePosition(_mousepos);
        }
    }

    void UpdateDragItemPosition(UI_Hud_AbilitySlot_Single_Control _slot, Vector3 _mousepos)
    {
        if (UI_Hud_DragItem_Manager.Instance)
        {
            UI_Hud_DragItem_Manager.Instance.UpdatePosition(_mousepos);
        }
    }

    void DeleteDragItem(UI_Hud_AbilitySlot_Single_Control _slot, Vector3 _mousepos)
    {
        if (UI_Hud_DragItem_Manager.Instance)
        {
            UI_Hud_DragItem_Manager.Instance.Dispose(_mousepos);
        }
    }

    void HoverSlot(UI_Hud_AbilitySlot_Single_Control _slot, bool _isHover)
    {
        if (UI_Hud_DragItem_Manager.Instance)
        {
            if (_isHover)
            {
                UI_Hud_DragItem_Manager.Instance.EnterHover();
            }
            else
            {
                UI_Hud_DragItem_Manager.Instance.ExitHover();
            }
        }
    }
    #endregion
}
