using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InGameScreencharInfoCtrl : MonoBehaviour
{
    public UI_TypeDefine.EnumCharInfoUITYPE CurCharInfoUI;
	public bool chatWindowStatus = false;
	
    void Awake()
    {
        CurCharInfoUI = UI_TypeDefine.EnumCharInfoUITYPE.NONE;
		chatWindowStatus = false;
    }

    void Update()
    {
		if (PlayerDataManager.Instance.GetMissionID() >= 6000 && PlayerDataManager.Instance.GetMissionID() < 6010){ // village
			//mm
			if(Hud_XPBar_Manager.Instance != null){
				Hud_XPBar_Manager.Instance.DestroyXP();
				Debug.Log("XP -------->> Destroyed");
				Hud_XPBar_Manager.Instance.DestroyXP();
			}
			
			//#mm
		}else{
			//mm
			if(MoneyBarManager.Instance != null){
				//MoneyBarManager.Instance.DestroyMoney();
				//Debug.Log("Money -------->> Destroyed");
				MoneyBarManager.Instance.DestroyMoney();
			}
			Hud_XPBar_Manager.Instance.SetKarmaMissionVal(PlayerDataManager.Instance.MissionKarma);
			//#mm
		}
		if(StashManager.Instance == null)
		{
			if(Input.GetKeyDown(KeyCode.Escape))
			{
				if(CurCharInfoUI == UI_TypeDefine.EnumCharInfoUITYPE.Abilities ||
				   CurCharInfoUI == UI_TypeDefine.EnumCharInfoUITYPE.PlayerStats ||
				   CurCharInfoUI == UI_TypeDefine.EnumCharInfoUITYPE.Inventory ||
				   CurCharInfoUI == UI_TypeDefine.EnumCharInfoUITYPE.Trials)
				{
					CloseCharInfo();
				}else
				{
					GUIManager.Instance.ChangeUIScreenState("OptionScreen");
				}
				return;
			}
		}
		
        if (Input.GetKeyDown(KeyCode.A) && chatWindowStatus == false)
        {
			if(StashManager.Instance != null) {
				return;
			}
			
            if (CurCharInfoUI == UI_TypeDefine.EnumCharInfoUITYPE.NONE)
            {
                InitCharInfo(UI_TypeDefine.EnumCharInfoUITYPE.Abilities);
            }
            else if (CurCharInfoUI != UI_TypeDefine.EnumCharInfoUITYPE.Abilities && CurCharInfoUI != UI_TypeDefine.EnumCharInfoUITYPE.LoadingBundle)
            {
                ChangeUITitle(UI_TypeDefine.EnumCharInfoUITYPE.Abilities);
            }
			else if (CurCharInfoUI == UI_TypeDefine.EnumCharInfoUITYPE.Abilities)
			{
				CloseCharInfo();
			}
        }

        if (Input.GetKeyDown(KeyCode.C) && chatWindowStatus == false)
        {
			if(StashManager.Instance != null) {
				return;
			}
			
            if (CurCharInfoUI == UI_TypeDefine.EnumCharInfoUITYPE.NONE)
            {
                InitCharInfo(UI_TypeDefine.EnumCharInfoUITYPE.PlayerStats);
            }
            else if (CurCharInfoUI != UI_TypeDefine.EnumCharInfoUITYPE.PlayerStats && CurCharInfoUI != UI_TypeDefine.EnumCharInfoUITYPE.LoadingBundle)
            {
                ChangeUITitle(UI_TypeDefine.EnumCharInfoUITYPE.PlayerStats);
            }
			else if (CurCharInfoUI == UI_TypeDefine.EnumCharInfoUITYPE.PlayerStats)
			{
				CloseCharInfo();
			}
        }
		
		if (Input.GetKeyDown(KeyCode.I)&& chatWindowStatus == false)
        {
			if(StashManager.Instance != null) {
				return;
			}
			
            if (CurCharInfoUI == UI_TypeDefine.EnumCharInfoUITYPE.NONE)
            {
                InitCharInfo(UI_TypeDefine.EnumCharInfoUITYPE.Inventory);
            }
            else if (CurCharInfoUI != UI_TypeDefine.EnumCharInfoUITYPE.Inventory && CurCharInfoUI != UI_TypeDefine.EnumCharInfoUITYPE.LoadingBundle)
            {
                ChangeUITitle(UI_TypeDefine.EnumCharInfoUITYPE.Inventory);
            }
			else if (CurCharInfoUI == UI_TypeDefine.EnumCharInfoUITYPE.Inventory)
			{
				CloseCharInfo();
			}
        }
		
		/* TODO : reactivate Trails
         * if (Input.GetKeyDown(KeyCode.T) && chatWindowStatus == false)
        {
			if(StashManager.Instance != null) {
				return;
			}
			
            if (CurCharInfoUI == UI_TypeDefine.EnumCharInfoUITYPE.NONE)
            {
                InitCharInfo(UI_TypeDefine.EnumCharInfoUITYPE.Trials);
            }
            else if (CurCharInfoUI != UI_TypeDefine.EnumCharInfoUITYPE.Trials && CurCharInfoUI != UI_TypeDefine.EnumCharInfoUITYPE.LoadingBundle)
            {
                ChangeUITitle(UI_TypeDefine.EnumCharInfoUITYPE.Trials);
            }
			else if (CurCharInfoUI == UI_TypeDefine.EnumCharInfoUITYPE.Trials)
			{
				CloseCharInfo();
			}
        }*/
		
		
		// press escape button to close any character info screen.
		if (Input.GetKeyDown(KeyCode.Escape))
		{
			if(StashManager.Instance != null) {
				return;
			}
			
			if (CurCharInfoUI != UI_TypeDefine.EnumCharInfoUITYPE.NONE && CurCharInfoUI != UI_TypeDefine.EnumCharInfoUITYPE.LoadingBundle)
			{
				CloseCharInfo();
			}
		}
    }

    public void RegisterSingleTemplateEvent(string _templateName)
    {
		if(_templateName == "GameHud_BTNGroup_CharInfo" && UI_Hud_BTNGroup_CharInfo_Manager.Instance)
		{
			UI_Hud_BTNGroup_CharInfo_Manager.Instance.UI_Hud_CharInfoBTNGroup_Clicked_Event += HandleUI_Hud_BTNGroup_CharInfo_ManagerInstanceUI_Hud_CharInfoBTNGroup_Clicked_Event;
		}
		
        if (_templateName == "CharInfo_BG" && UI_CharInfoBG_Manager.Instance)
        {
            UI_CharInfoBG_Manager.Instance.TopbarPressed_Event += new UI_CharInfoBG_Manager.Handle_UICharInfoTopBarBTNPressed_Delegate(ChangeUITitle);
            UI_CharInfoBG_Manager.Instance.UICharInfo_CloseBTN_Event += new UI_CharInfoBG_Manager.Handle_UICharInfoCloseBTNClicked_Delegate(CloseCharInfo);
        }
		

        if (_templateName == "CharInfo_Stat" && ChaInfo_Manager.Instance)
        {
            CurCharInfoUI = UI_TypeDefine.EnumCharInfoUITYPE.PlayerStats;

            if (UI_CharInfoBG_Manager.Instance)
                UI_CharInfoBG_Manager.Instance.SetBTNHighLight(UI_TypeDefine.EnumCharInfoUITYPE.PlayerStats);
			
			InitCharStats();
        }

        if (_templateName == "CharInfo_Abilities" && UI_AbiInfo_Manager.Instance)
        {
            UI_AbiInfo_Manager.Instance.UI_Abi_ClickIcon_Event += new UI_AbiInfo_Manager.Handle_CharInfo_Abi_ClickIcon_Delegate(ShowTooltipsForAbility);
            UI_AbiInfo_Manager.Instance.UI_Abi_StartDraging_Event += new UI_AbiInfo_Manager.Handle_CharInfo_Abi_StartDragging_Delete(ShowDragItem);
            UI_AbiInfo_Manager.Instance.UI_Abi_UpdateDraging_Event += new UI_AbiInfo_Manager.Handle_CharInfo_Abi_UpdateDragging_Delete(UpdateDragItemPosition);
            UI_AbiInfo_Manager.Instance.UI_Abi_ExitDraging_Event += new UI_AbiInfo_Manager.Handle_CharInfo_Abi_ExitDragging_Delete(DeleteDragItem);

            UI_AbiInfo_Manager.Instance.UpdateAllAbilitiesInfo();

            CurCharInfoUI = UI_TypeDefine.EnumCharInfoUITYPE.Abilities;

            if (UI_CharInfoBG_Manager.Instance)
                UI_CharInfoBG_Manager.Instance.SetBTNHighLight(UI_TypeDefine.EnumCharInfoUITYPE.Abilities);
        }
		
		if (_templateName == "CharInfo_Trials" && Trials_Manager.Instance)
        {
			CurCharInfoUI = UI_TypeDefine.EnumCharInfoUITYPE.Trials;
			if (UI_CharInfoBG_Manager.Instance)
                UI_CharInfoBG_Manager.Instance.SetBTNHighLight(UI_TypeDefine.EnumCharInfoUITYPE.Trials);
		}
          
    }

    public void UnregisterSingleTemplateEvent(string _templateName)
    {
        if (_templateName == "CharInfo_BG" && UI_CharInfoBG_Manager.Instance)
        {
            UI_CharInfoBG_Manager.Instance.TopbarPressed_Event -= new UI_CharInfoBG_Manager.Handle_UICharInfoTopBarBTNPressed_Delegate(ChangeUITitle);
            UI_CharInfoBG_Manager.Instance.UICharInfo_CloseBTN_Event -= new UI_CharInfoBG_Manager.Handle_UICharInfoCloseBTNClicked_Delegate(CloseCharInfo);
            CurCharInfoUI = UI_TypeDefine.EnumCharInfoUITYPE.NONE;
        }

        if (_templateName == "CharInfo_Abilities" && UI_AbiInfo_Manager.Instance)
        {
            UI_AbiInfo_Manager.Instance.UI_Abi_ClickIcon_Event -= new UI_AbiInfo_Manager.Handle_CharInfo_Abi_ClickIcon_Delegate(ShowTooltipsForAbility);
            UI_AbiInfo_Manager.Instance.UI_Abi_StartDraging_Event -= new UI_AbiInfo_Manager.Handle_CharInfo_Abi_StartDragging_Delete(ShowDragItem);
            UI_AbiInfo_Manager.Instance.UI_Abi_UpdateDraging_Event -= new UI_AbiInfo_Manager.Handle_CharInfo_Abi_UpdateDragging_Delete(UpdateDragItemPosition);
            UI_AbiInfo_Manager.Instance.UI_Abi_ExitDraging_Event -= new UI_AbiInfo_Manager.Handle_CharInfo_Abi_ExitDragging_Delete(DeleteDragItem);
        }
		
		if (_templateName == "CharInfo_Trials" && Trials_Manager.Instance)
        {
			
		}
		
		if(_templateName == "GameHud_BTNGroup_CharInfo" && UI_Hud_BTNGroup_CharInfo_Manager.Instance)
		{
			UI_Hud_BTNGroup_CharInfo_Manager.Instance.UI_Hud_CharInfoBTNGroup_Clicked_Event -= HandleUI_Hud_BTNGroup_CharInfo_ManagerInstanceUI_Hud_CharInfoBTNGroup_Clicked_Event;
		}
    }

    #region Local
	
	void InitCharStats()
	{
		ChaInfo_Manager.ChaInfoData _data = new ChaInfo_Manager.ChaInfoData();

        if (PlayerDataManager.Instance)
        {
            int curAttr = PlayerDataManager.Instance.GetCurAttrs(EAttributeType.ATTR_Power);
            int baseAttr = PlayerDataManager.Instance.GetBaseAttrs(EAttributeType.ATTR_Power);
            _data.BaseStat_POW = "" + baseAttr;
            if (curAttr - baseAttr != 0)
                _data.BaseStat_POW += " ( +" + (curAttr - baseAttr).ToString() + " )";

            curAttr = PlayerDataManager.Instance.GetCurAttrs(EAttributeType.ATTR_Defense);
            baseAttr = PlayerDataManager.Instance.GetBaseAttrs(EAttributeType.ATTR_Defense);
            _data.BaseStat_DEF = "" + baseAttr;
            if (curAttr - baseAttr != 0)
                _data.BaseStat_DEF += " ( +" + (curAttr - baseAttr).ToString() + " )";

            curAttr = PlayerDataManager.Instance.GetCurAttrs(EAttributeType.ATTR_Skill);
            baseAttr = PlayerDataManager.Instance.GetBaseAttrs(EAttributeType.ATTR_Skill);
            _data.BaseStat_SKL = "" + baseAttr;
            if (curAttr - baseAttr != 0)
                _data.BaseStat_SKL += " ( +" + (curAttr - baseAttr).ToString() + " )";

            curAttr = PlayerDataManager.Instance.GetCurAttrs(EAttributeType.ATTR_MaxHP);
            baseAttr = PlayerDataManager.Instance.GetBaseAttrs(EAttributeType.ATTR_MaxHP);
            _data.BaseStat_HP = "" + PlayerDataManager.Instance.GetCurAttrs(EAttributeType.ATTR_CurHP);
            if (curAttr - baseAttr != 0)
                _data.BaseStat_HP += " / ( " + baseAttr + " + " + (curAttr - baseAttr) + " )";
            else
                _data.BaseStat_HP += " / " + baseAttr;
			_data.BaseStat_HPRegen = "" + PlayerDataManager.Instance.GetCurAttrs(EAttributeType.ATTR_HPRecover);

            curAttr = PlayerDataManager.Instance.GetCurAttrs(EAttributeType.ATTR_MaxMP);
            baseAttr = PlayerDataManager.Instance.GetBaseAttrs(EAttributeType.ATTR_MaxMP);
            _data.BaseStat_MP = "" + PlayerDataManager.Instance.GetCurAttrs(EAttributeType.ATTR_CurMP);
            if (curAttr - baseAttr != 0)
                _data.BaseStat_MP += " / ( " + baseAttr + " + " + (curAttr - baseAttr) + " )";
            else
                _data.BaseStat_MP += " / " + baseAttr;
			_data.BaseStat_MPRegen = "" + PlayerDataManager.Instance.GetCurAttrs(EAttributeType.ATTR_MPRecover);

            if (PlayerDataManager.Instance)
            {
				_data.Bonuses_RDPS = "" + (int)PlayerDataManager.Instance.GetDPS_MainWPN();
				int dpsL = (int)PlayerDataManager.Instance.GetDPS_SubWPN();
                if (dpsL == 0)
                    _data.Bonuses_LDPS = "None";
                else
                    _data.Bonuses_LDPS = "" + dpsL;
            }

            _data.Bonuses_CriChance = string.Format("{0:0.00}", (Player.Instance.AttrMan.Attrs[EAttributeType.ATTR_Critical] / 100).ToString()) + " %";
            _data.Bonuses_CriDMG = "" + PlayerDataManager.Instance.GetCurAttrs(EAttributeType.ATTR_CriticalDamage) + " %";
            _data.Bonuses_DMGReduction = string.Format("{0:0.00}", (Player.Instance.AttrMan.Attrs[EAttributeType.ATTR_DamageReduction] / 100).ToString()) + " %";
			_data.Bonuses_AttackSpeed = string.Format("{0:0.00}", Player.Instance.AttrMan.Attrs[EAttributeType.ATTR_AttackSpeed].ToString()) + " %";
			_data.Bonuses_MoveSpeed = string.Format("{0:0.00}", Player.Instance.AttrMan.Attrs[EAttributeType.ATTR_MoveSpeed].ToString()) + " %";
			_data.BOnuses_TotalArmor = "" + PlayerDataManager.Instance.GetArmor();

            _data.DMGBonus_Fire = "" + PlayerDataManager.Instance.GetCurEleChanceAttrs(EStatusElementType.StatusElement_Flame) + " %";
			_data.DMGBonus_Frost = "" + PlayerDataManager.Instance.GetCurEleChanceAttrs(EStatusElementType.StatusElement_Frost) + " %";
			_data.DMGBonus_Blast = "" + PlayerDataManager.Instance.GetCurEleChanceAttrs(EStatusElementType.StatusElement_Explosion) + " %";
			_data.DMGBonus_Storm = "" + PlayerDataManager.Instance.GetCurEleChanceAttrs(EStatusElementType.StatusElement_Storm) + " %";

            _data.Resistance_Fire = "" + (PlayerDataManager.Instance.GetCurEleAttrs(EStatusElementType.StatusElement_FlameResist) * 100) + " %";
			_data.Resistance_Frost = "" + (PlayerDataManager.Instance.GetCurEleAttrs(EStatusElementType.StatusElement_FrostResist) * 100) + " %";
			_data.Resistance_Blast = "" + (PlayerDataManager.Instance.GetCurEleAttrs(EStatusElementType.StatusElement_ExplosionResist) * 100) + " %";
			_data.Resistance_Storm = "" + (PlayerDataManager.Instance.GetCurEleAttrs(EStatusElementType.StatusElement_StormResist) * 100) + " %";
        }

        ChaInfo_Manager.Instance.UpdateChaInfo(_data);
	}
	
	void HandleUI_Hud_BTNGroup_CharInfo_ManagerInstanceUI_Hud_CharInfoBTNGroup_Clicked_Event (UI_TypeDefine.EnumCharInfoUITYPE _targetui)
    {
		if (CurCharInfoUI == UI_TypeDefine.EnumCharInfoUITYPE.NONE)
        {
            InitCharInfo(_targetui);
        }
        else if (CurCharInfoUI != _targetui && CurCharInfoUI != UI_TypeDefine.EnumCharInfoUITYPE.LoadingBundle)
        {
            ChangeUITitle(_targetui);
        }
    }
	
    void CloseCharInfo()
    {
        switch (CurCharInfoUI)
        {
            case UI_TypeDefine.EnumCharInfoUITYPE.Inventory:
				GUIManager.Instance.RemoveTemplate("CharInfo_Inventory");
                GUIManager.Instance.RemoveTemplate("CharInfo_Tooltips");
                break;
            case UI_TypeDefine.EnumCharInfoUITYPE.Abilities:
                GUIManager.Instance.RemoveTemplate("CharInfo_Abilities");
                GUIManager.Instance.RemoveTemplate("CharInfo_Tooltips");
                break;
            case UI_TypeDefine.EnumCharInfoUITYPE.PlayerStats:
                GUIManager.Instance.RemoveTemplate("CharInfo_Stat");
                break;
            case UI_TypeDefine.EnumCharInfoUITYPE.Trials:
				GUIManager.Instance.RemoveTemplate("CharInfo_Trials");
                break;
        }

        GUIManager.Instance.RemoveTemplate("CharInfo_Equips");

        CurCharInfoUI = UI_TypeDefine.EnumCharInfoUITYPE.LoadingBundle;

        GUIManager.Instance.RemoveTemplate("CharInfo_BG");
    }

    void InitCharInfo(UI_TypeDefine.EnumCharInfoUITYPE askUIType)
    {
        CurCharInfoUI = UI_TypeDefine.EnumCharInfoUITYPE.LoadingBundle;

        GUIManager.Instance.AddTemplate("CharInfo_Equips");
        GUIManager.Instance.AddTemplate("CharInfo_BG");

        switch (askUIType)
        {
            case UI_TypeDefine.EnumCharInfoUITYPE.Inventory:
				GUIManager.Instance.AddTemplate("CharInfo_Inventory");
                GUIManager.Instance.AddTemplate("CharInfo_Tooltips");
                break;
            case UI_TypeDefine.EnumCharInfoUITYPE.Abilities:
                GUIManager.Instance.AddTemplate("CharInfo_Abilities");
                GUIManager.Instance.AddTemplate("CharInfo_Tooltips");
                break;
            case UI_TypeDefine.EnumCharInfoUITYPE.PlayerStats:
                GUIManager.Instance.AddTemplate("CharInfo_Stat");
                break;
            case UI_TypeDefine.EnumCharInfoUITYPE.Trials:
				GUIManager.Instance.AddTemplate("CharInfo_Trials");
                break;
        }
    }

    void ChangeUITitle(UI_TypeDefine.EnumCharInfoUITYPE askUIType)
    {
        switch (CurCharInfoUI)
        {
            case UI_TypeDefine.EnumCharInfoUITYPE.Inventory:
				GUIManager.Instance.RemoveTemplate("CharInfo_Inventory");
                GUIManager.Instance.RemoveTemplate("CharInfo_Tooltips");
                break;
            case UI_TypeDefine.EnumCharInfoUITYPE.Abilities:
                GUIManager.Instance.RemoveTemplate("CharInfo_Abilities");
                GUIManager.Instance.RemoveTemplate("CharInfo_Tooltips");
                break;
            case UI_TypeDefine.EnumCharInfoUITYPE.PlayerStats:
                GUIManager.Instance.RemoveTemplate("CharInfo_Stat");
                break;
            case UI_TypeDefine.EnumCharInfoUITYPE.Trials:
				GUIManager.Instance.RemoveTemplate("CharInfo_Trials");
                break;
        }

        CurCharInfoUI = UI_TypeDefine.EnumCharInfoUITYPE.LoadingBundle;

        switch (askUIType)
        {
            case UI_TypeDefine.EnumCharInfoUITYPE.Inventory:
				GUIManager.Instance.AddTemplate("CharInfo_Inventory");
                GUIManager.Instance.AddTemplate("CharInfo_Tooltips");
                break;
            case UI_TypeDefine.EnumCharInfoUITYPE.Abilities:
                GUIManager.Instance.AddTemplate("CharInfo_Abilities");
                GUIManager.Instance.AddTemplate("CharInfo_Tooltips");
                break;
            case UI_TypeDefine.EnumCharInfoUITYPE.PlayerStats:
                GUIManager.Instance.AddTemplate("CharInfo_Stat");
                break;
            case UI_TypeDefine.EnumCharInfoUITYPE.Trials:
				GUIManager.Instance.AddTemplate("CharInfo_Trials");
                break;
        }
    }

    void CloseCharInfoBTNClicked()
    {

    }

    void ShowTooltipsForAbility(AbiInfo_SingleAbi_Control _abi_control, bool isSameBTN)
    {
        if (!isSameBTN)
        {
            if (UI_CharInfo_Tooltips_Manager.Instance && _abi_control != null)
            {
                UI_CharInfo_Tooltips_Abi.CharInfo_Tooltips_Ability_Data _data = new UI_CharInfo_Tooltips_Abi.CharInfo_Tooltips_Ability_Data();

                _data.MaxLevel = 6;
                _data.Description = "Description";
                _data.EnergyRequired = 300;
                _data.BuffName = "Buff";
                _data.BuffDescription = "Buff Description";

                UI_CharInfo_Tooltips_Manager.Instance.Show(_data);
                UI_CharInfo_Tooltips_Manager.Instance.UpdatePosition(_abi_control.transform.localPosition + new Vector3(55, 40, 0), _abi_control.transform.localPosition);
            }
        }
        else
        {
            if (UI_CharInfo_Tooltips_Manager.Instance)
                UI_CharInfo_Tooltips_Manager.Instance.Dispose();
        }
    }

    void ShowDragItem(AbiInfo_SingleAbi_Control _abi_control, Vector3 _mousepos)
    {
        if (UI_Hud_DragItem_Manager.Instance)
        {
            //UI_TypeDefine.UI_GameHud_DragItem_data _newdata = new UI_TypeDefine.UI_GameHud_DragItem_data(_abi_control.AbiData.IconSpriteName);
			UI_TypeDefine.UI_GameHud_DragItem_data _newdata = new UI_TypeDefine.UI_GameHud_DragItem_data("");
            _newdata.ItemType = UI_TypeDefine.UI_GameHud_DragItem_data.EnumDragItemType.Ability;
            //_newdata.ItemColor = _abi_control.AbiData.AbiColor;
			//_newdata.AbiID = _abi_control.AbiData.AbiID;
            //_newdata.Param = _abi_control.AbiData;
            UI_Hud_DragItem_Manager.Instance.ShowItem(_newdata);
            UI_Hud_DragItem_Manager.Instance.UpdatePosition(_mousepos);
        }

        if (UI_CharInfo_Tooltips_Manager.Instance)
            UI_CharInfo_Tooltips_Manager.Instance.Dispose();
    }

    void UpdateDragItemPosition(AbiInfo_SingleAbi_Control _abi_control, Vector3 _mousepos)
    {
        if (UI_Hud_DragItem_Manager.Instance)
        {
            UI_Hud_DragItem_Manager.Instance.UpdatePosition(_mousepos);
        }
    }

    void DeleteDragItem(AbiInfo_SingleAbi_Control _abi_control, Vector3 _mousepos)
    {
        if (UI_Hud_DragItem_Manager.Instance)
        {
            UI_Hud_DragItem_Manager.Instance.Dispose(_mousepos);
        }
    }

    #endregion
}
