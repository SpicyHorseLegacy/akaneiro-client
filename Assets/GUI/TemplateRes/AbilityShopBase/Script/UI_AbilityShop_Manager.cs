using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UI_AbilityShop_Manager : MonoBehaviour
{
    public static UI_AbilityShop_Manager Instance;

    void Awake()
    {
        Instance = this;
    } 

    [SerializeField]  UI_AbilityShop_Main_Abi_Control Main_Abi_Item_Prefab;
    [SerializeField]  Transform Main_Abi_Item_Parent;
    [SerializeField]  Transform Main_Mastery_Item_Parent;
    [SerializeField]  UI_AbilityShop_UITYPE_Title_Manager UIType_Title_Manager;
    [SerializeField]  UI_AbilityShop_DetailInfo_Control DetailInfo;
	[SerializeField]  UI_AbilityShop_Success SuccessPanel;
    public UI_TypeDefine.EnumAbilityShopUITYPE UIType;

    private List<UI_AbilityShop_Main_Abi_Control> Main_AbiInfo_Group = new List<UI_AbilityShop_Main_Abi_Control>();
    private List<UI_AbilityShop_Main_Abi_Control> Main_MasteryInfo_Group = new List<UI_AbilityShop_Main_Abi_Control>();

    #region Delegate

    public delegate void Handle_UIShopAbilityTopBarBTNClicked(UI_TypeDefine.EnumAbilityShopUITYPE askUIType);
    public event Handle_UIShopAbilityTopBarBTNClicked TopBarClicked_Event;

    public delegate void Handle_UIShopAbilityAbilityItemClicked(int AbiID);
    public event Handle_UIShopAbilityAbilityItemClicked AbilityItemClicked_Event;
	
	public delegate void Handle_UIShopAbilityMasteryItemClicked(int MastID);
    public event Handle_UIShopAbilityMasteryItemClicked MasteryItemClicked_Event;

    public delegate void Handle_UIShopAbilityCloseBTNClicked();
    public event Handle_UIShopAbilityCloseBTNClicked AbilityCloseClicked_Event;

    public delegate void Handle_UIShopAbilityLearnBTNClicked(bool isAbility, int AbilityID);
    public event Handle_UIShopAbilityLearnBTNClicked AbilityLearnClicked_Event;

    public delegate void Handle_UIShopAbilitySpeedUpBTNClicked(bool isAbility, int AbilityID, float CurCooldown);
    public event Handle_UIShopAbilitySpeedUpBTNClicked AbilitySpeedUpClicked_Event;

    #endregion

    #region Public

	public void InitAbilityShop()
	{
		SuccessPanel.gameObject.SetActive(false);
		UIType = UI_TypeDefine.EnumAbilityShopUITYPE.NONE;
		TopBarBTNClicked(UI_TypeDefine.EnumAbilityShopUITYPE.Prowess);
	}

	// change to the type on the top of screen
    public void UpdateTitleType(UI_TypeDefine.EnumAbilityShopUITYPE _uitype)
    {
        UIType = _uitype;
        UIType_Title_Manager.ChangeToTargetTitle(UIType);
    }

	// update ability info for all the cards
    public void UpdateMainAbilityInfo(UI_TypeDefine.UI_AbiInfo_data[] _abidatas)
    {
        int _index = 0;

        List<UI_AbilityShop_Main_Abi_Control> _tempunusedabiinfo = new List<UI_AbilityShop_Main_Abi_Control>();
        for (int i = 0; i < Main_AbiInfo_Group.Count; i++)
        {
            _tempunusedabiinfo.Add(Main_AbiInfo_Group[i]);
        }

        foreach (UI_TypeDefine.UI_AbiInfo_data _data in _abidatas)
        {
            UI_AbilityShop_Main_Abi_Control _newAbiInfo = null;
            if (_index < Main_AbiInfo_Group.Count)
            {
                // if there is 
                _newAbiInfo = Main_AbiInfo_Group[_index];
                for (int i = _tempunusedabiinfo.Count - 1; i >= 0; i--)
                {
                    if (_newAbiInfo == _tempunusedabiinfo[i])
                    {
                        _tempunusedabiinfo.RemoveAt(i);
                        break;
                    }
                }
            }
            else
            {
                _newAbiInfo = UnityEngine.Object.Instantiate(Main_Abi_Item_Prefab) as UI_AbilityShop_Main_Abi_Control;
                _newAbiInfo.transform.parent = Main_Abi_Item_Parent;
                _newAbiInfo.transform.localScale = Vector3.one;
                Main_AbiInfo_Group.Add(_newAbiInfo);
            }

            _newAbiInfo.UpdateAllInfo(_data);
            _index++;
        }

        // delete useless abiinfo items.
        if(_tempunusedabiinfo.Count > 0)
        {
            for (int i = _tempunusedabiinfo.Count - 1; i >= 0; i--)
            {
                UnityEngine.Object.Destroy(_tempunusedabiinfo[i]);
            }
            _tempunusedabiinfo.Clear();
        }
        _tempunusedabiinfo = null;

        RePositionMainAbiInfo();

        if (AbilityItemClicked_Event != null)
            AbilityItemClicked_Event((int)_abidatas[0].AbiType);
    }

	// update mastery info for all the cards
    public void UpdateMainMasteryInfo(UI_TypeDefine.UI_MasteryInfo_data[] _masterydatas)
    {
        int _index = 0;

        List<UI_AbilityShop_Main_Abi_Control> _tempunusedmasteryinfo = new List<UI_AbilityShop_Main_Abi_Control>();
        for (int i = 0; i < Main_MasteryInfo_Group.Count; i++)
        {
            _tempunusedmasteryinfo.Add(Main_MasteryInfo_Group[i]);
        }

        foreach (UI_TypeDefine.UI_MasteryInfo_data _data in _masterydatas)
        {
            UI_AbilityShop_Main_Abi_Control _newAbiInfo = null;
            if (_index < Main_MasteryInfo_Group.Count)
            {
                // if there is 
                _newAbiInfo = Main_MasteryInfo_Group[_index];
                for (int i = _tempunusedmasteryinfo.Count - 1; i >= 0; i--)
                {
                    if (_newAbiInfo == _tempunusedmasteryinfo[i])
                    {
                        _tempunusedmasteryinfo.RemoveAt(i);
                        break;
                    }
                }
            }
            else
            {
                _newAbiInfo = UnityEngine.Object.Instantiate(Main_Abi_Item_Prefab) as UI_AbilityShop_Main_Abi_Control;
                _newAbiInfo.transform.parent = Main_Mastery_Item_Parent;
                _newAbiInfo.transform.localScale = Vector3.one;
                Main_MasteryInfo_Group.Add(_newAbiInfo);
            }

            _newAbiInfo.UpdateAllInfo(_data);
            _index++;
        }

        // delete useless abiinfo items.
        if (_tempunusedmasteryinfo.Count > 0)
        {
            for (int i = _tempunusedmasteryinfo.Count - 1; i >= 0; i--)
            {
                UnityEngine.Object.Destroy(_tempunusedmasteryinfo[i]);
            }
            _tempunusedmasteryinfo.Clear();
        }
        _tempunusedmasteryinfo = null;

        RePositionMainMasteryInfo();
    }

    public void UpdateAbilityCoolDown(UI_TypeDefine.UI_LearnAbilityCoolDown_data[] _cooldowndatas)
    {
        for (int j = 0; j < Main_AbiInfo_Group.Count; j++)
        {
            UI_AbilityShop_Main_Abi_Control _abiBTN = Main_AbiInfo_Group[j];
            _abiBTN.ResetCoolDown();
            for (int i = 0; i < _cooldowndatas.Length; i++)
            {
                UI_TypeDefine.UI_LearnAbilityCoolDown_data _data = _cooldowndatas[i];
                if (_abiBTN.GetAbiInfo() != null && _data.IsAbilityOrMastery == true && _data.AbiType == _abiBTN.GetAbiInfo().AbiType)
                {
                    _abiBTN.UpdateCoolDownCycle(_data);
                }
            }
        }
        for (int j = 0; j < Main_MasteryInfo_Group.Count; j++)
        {
            UI_AbilityShop_Main_Abi_Control _masteryBTN = Main_MasteryInfo_Group[j];
            _masteryBTN.ResetCoolDown();
            for (int i = 0; i < _cooldowndatas.Length; i++)
            {
                UI_TypeDefine.UI_LearnAbilityCoolDown_data _data = _cooldowndatas[i];
                if (_masteryBTN.GetMasteryInfo() != null && _data.IsAbilityOrMastery == false && _data.MasteryType.Get() == _masteryBTN.GetMasteryInfo().MasteryType.Get())
                {
                    _masteryBTN.UpdateCoolDownCycle(_data);
                }
            }
        }
    }

    public void UpdateAbiiltyDetailInfo(UI_TypeDefine.UI_AbilityShop_AbiDetail_data _data)
    {
        DetailInfo.UpdateAllInfo(_data);
    }

    public void UpdateDetailAbilityCoolDown(bool isshow, bool isstart, UI_TypeDefine.UI_LearnAbilityCoolDown_data _data)
    {
        DetailInfo.UpdateAbilityCoolDown(isshow, isstart, _data);
    }

    public void AbilityLevelup(UI_TypeDefine.UI_AbiInfo_data _data)
    {
        foreach (UI_AbilityShop_Main_Abi_Control _btn in Main_AbiInfo_Group)
        {
            if (_btn.GetAbiInfo().AbiType == _data.AbiType)
            {
                _btn.UpdateAllInfo(_data);
                _btn.Play_Ani_Pop();
				SuccessPanel.gameObject.SetActive(true);
				SuccessPanel.UpdateBuySucInfo(_data.AbiName, _data.IconSpriteName);
            }
        }

    }

    public void MasteryLevelup(UI_TypeDefine.UI_MasteryInfo_data _data)
    {
        foreach (UI_AbilityShop_Main_Abi_Control _btn in Main_MasteryInfo_Group)
        {
            if (_btn.GetMasteryInfo().MasteryType == _data.MasteryType)
            {
                _btn.UpdateAllInfo(_data);
                _btn.Play_Ani_Pop();
				SuccessPanel.gameObject.SetActive(true);
				SuccessPanel.UpdateBuySucInfo(_data.MasteryName, _data.IconSpriteName);
            }
        }
    }

    public UI_AbilityShop_Main_Abi_Control GetAbilityBTNByType(AbilityDetailInfo.EnumAbilityType _type)
    {
        UI_AbilityShop_Main_Abi_Control _temp = null;
        foreach (UI_AbilityShop_Main_Abi_Control _btn in Main_AbiInfo_Group)
        {
            if (_btn.GetAbiInfo().AbiType == _type)
            {
                _temp = _btn;
                break;
            }
        }
        return _temp;
    }
    public UI_AbilityShop_Main_Abi_Control GetMasteryBTNByID(EMasteryType _type)
    {
        UI_AbilityShop_Main_Abi_Control _temp = null;
        foreach (UI_AbilityShop_Main_Abi_Control _btn in Main_MasteryInfo_Group)
        {
            if (_btn.GetMasteryInfo().MasteryType.Get() == _type.Get())
            {
                _temp = _btn;
                break;
            }
        }
        return _temp;
    }

	public void AbilityTrainFinished()
	{

	}

    #endregion

    #region Local

    void RePositionMainAbiInfo()
    {
        for (int i = 0; i < Main_AbiInfo_Group.Count; i++)
        {
            Main_AbiInfo_Group[i].transform.localPosition = new Vector3(-290 + i % 2 * 340, 80 - (int)(i / 2) * 70, 0);
        }
    }

    void RePositionMainMasteryInfo()
    {
        for (int i = 0; i < Main_MasteryInfo_Group.Count; i++)
        {
            Main_MasteryInfo_Group[i].transform.localPosition = new Vector3(-290 + i % 2 * 340, 0 - (int)(i / 2) * 70, 0);
        }
    }

    #endregion

    #region BTNCallback

    public void TopBarBTNClicked(UI_TypeDefine.EnumAbilityShopUITYPE _askType)
    {
        if (TopBarClicked_Event != null && _askType != UI_TypeDefine.EnumAbilityShopUITYPE.NONE && _askType != UI_TypeDefine.EnumAbilityShopUITYPE.MAX && UIType != _askType)
            TopBarClicked_Event(_askType);
    }

    public void AbilityItemBTNClicked(bool isAbility, int AbiID, UI_AbilityShop_Main_Abi_Control _btn)
    {
		if(isAbility)
		{
		    if (AbilityItemClicked_Event != null)
		        AbilityItemClicked_Event(AbiID);
		}else
		{
			if(MasteryItemClicked_Event != null)
				MasteryItemClicked_Event(AbiID);
		}
		
        List<UI_AbilityShop_Main_Abi_Control> _templist;
		if(isAbility)
		{
            _templist = Main_AbiInfo_Group;
        }
        else
        {
            _templist = Main_MasteryInfo_Group;
        }

        for (int i = 0; i < _templist.Count; i++)
        {
            if (_templist[i] != _btn)
                _templist[i].Unselected();
            else
                _templist[i].Beselected();
        }
    }

    void CloseBTNClicked()
    {
        if (AbilityCloseClicked_Event != null)
        {
            AbilityCloseClicked_Event();
        }
    }

    public void LearnBTNClicked(bool isAbility, int AbilityID)
    {
        if (AbilityLearnClicked_Event != null)
        {
            AbilityLearnClicked_Event(isAbility, AbilityID);
        }
    }

    public void SpeedUpBTNClicked(bool isAbility, int AbilityID, float CurCooldown)
    {
        if (AbilitySpeedUpClicked_Event != null)
        {
            AbilitySpeedUpClicked_Event(isAbility, AbilityID, CurCooldown);
        }
    }

    #endregion
}
