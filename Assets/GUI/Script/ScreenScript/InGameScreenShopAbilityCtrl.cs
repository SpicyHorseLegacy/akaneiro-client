using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

public class InGameScreenShopAbilityCtrl : BaseScreenCtrl {
	 
    public static InGameScreenShopAbilityCtrl Instance;

    UI_TypeDefine.UI_AbiInfo_data[] Abidatas = new UI_TypeDefine.UI_AbiInfo_data[0];
    UI_TypeDefine.UI_MasteryInfo_data[] Masterydatas = new UI_TypeDefine.UI_MasteryInfo_data[0];
    UI_TypeDefine.UI_LearnAbilityCoolDown_data[] Cooldowndatas = new UI_TypeDefine.UI_LearnAbilityCoolDown_data[0];
    UI_TypeDefine.UI_AbilityShop_AbiDetail_data _detaildata = new UI_TypeDefine.UI_AbilityShop_AbiDetail_data();

    override protected void Awake() { base.Awake(); Instance = this; }

    #region Events
    protected override void RegisterSingleTemplateEvent(string _templateName)
    {
		base.RegisterSingleTemplateEvent(_templateName);
		
        if (_templateName == "Shop_Ability" && UI_AbilityShop_Manager.Instance)
        {
            UI_AbilityShop_Manager.Instance.TopBarClicked_Event += ChangeToAskUI;
            UI_AbilityShop_Manager.Instance.AbilityItemClicked_Event += AbilityItemBTNClicked;
			UI_AbilityShop_Manager.Instance.MasteryItemClicked_Event += MasteryItemBTNClicked;
            UI_AbilityShop_Manager.Instance.AbilityCloseClicked_Event += CloseAbilityShop;
            UI_AbilityShop_Manager.Instance.AbilityLearnClicked_Event += LearnAbility;
            UI_AbilityShop_Manager.Instance.AbilitySpeedUpClicked_Event += SpeedUpAbility;

			UI_AbilityShop_Manager.Instance.InitAbilityShop();
        }

        if (_templateName == "SpeedUpBox" && SpeedUpBoxManager.Instance)
        {
            SpeedUpBoxManager.Instance.OnHalfHourDelegate += SpeedUpHalfHour;
            SpeedUpBoxManager.Instance.OnHourDelegate += SpeedUpHour;
            SpeedUpBoxManager.Instance.OnNowDelegate += SpeedUpFinish;

            SpeedUpBoxManager.Instance.Hide();
        }
		
		if(_templateName == "MoneyBar" && MoneyBarManager.Instance)
		{
			MoneyBarManager.Instance.OnAddKarmaDelegate +=	AddKarmaDelegate;
			MoneyBarManager.Instance.OnAddCrystalDelegate += AddCrystalDelegate;
			MoneyBarManager.Instance.SetKarmaVal(PlayerDataManager.Instance.GetKarmaVal());
			MoneyBarManager.Instance.SetCrystalVal(PlayerDataManager.Instance.GetCrystalVal());
		}
		
		if (_templateName == "KarmaRecharge" && KarmaRechargeManager.Instance) {
			KarmaRechargeManager.Instance.OnExitDelegate +=	ExitRechargeKarmaDelegate;
			KarmaRechargeManager.Instance.OnAddKarmaDelegate += RechargeKarmaValDelegate;
			InitKarmaRechargeData();
		}
		if (_templateName == "CrystalRecharge" && CrystalRechargeManager.Instance) {
			CrystalRechargeManager.Instance.OnExitDelegate +=	ExitRechargeCrystalDelegate;
			CrystalRechargeManager.Instance.OnAddKarmaDelegate += RechargeKarmaValDelegate;		
			InitCrystalRechargeData();
		}

        if (_templateName == "FoodSlot" && FoodSoltManager.Instance)
        {
            UpdateFoodSlot();
        }
    }

    protected override void UnregisterSingleTemplateEvent(string _templateName)
    {
		base.UnregisterSingleTemplateEvent(_templateName);
		
        if (_templateName == "Shop_Ability" && UI_AbilityShop_Manager.Instance)
        {
            UI_AbilityShop_Manager.Instance.TopBarClicked_Event -= ChangeToAskUI;
            UI_AbilityShop_Manager.Instance.AbilityItemClicked_Event -= AbilityItemBTNClicked;
			UI_AbilityShop_Manager.Instance.MasteryItemClicked_Event -= MasteryItemBTNClicked;
            UI_AbilityShop_Manager.Instance.AbilityCloseClicked_Event -= CloseAbilityShop;
            UI_AbilityShop_Manager.Instance.AbilityLearnClicked_Event -= LearnAbility;
            UI_AbilityShop_Manager.Instance.AbilitySpeedUpClicked_Event -= SpeedUpAbility;
        }

        if (_templateName == "SpeedUpBox" && SpeedUpBoxManager.Instance)
        {
            SpeedUpBoxManager.Instance.OnHalfHourDelegate -= SpeedUpHalfHour;
            SpeedUpBoxManager.Instance.OnHalfHourDelegate -= SpeedUpHour;
            SpeedUpBoxManager.Instance.OnHalfHourDelegate -= SpeedUpFinish;
        }
		
		if(_templateName == "MoneyBar" && MoneyBarManager.Instance)
		{
			MoneyBarManager.Instance.OnAddKarmaDelegate -=	AddKarmaDelegate;
			MoneyBarManager.Instance.OnAddCrystalDelegate -= AddCrystalDelegate;
		}
		if (_templateName == "KarmaRecharge" && KarmaRechargeManager.Instance) {
			KarmaRechargeManager.Instance.OnExitDelegate -=	ExitRechargeKarmaDelegate;
			KarmaRechargeManager.Instance.OnAddKarmaDelegate -= RechargeKarmaValDelegate;
		}
		if (_templateName == "CrystalRecharge" && CrystalRechargeManager.Instance) {
			CrystalRechargeManager.Instance.OnExitDelegate -=	ExitRechargeCrystalDelegate;
			CrystalRechargeManager.Instance.OnAddKarmaDelegate -= RechargeKarmaValDelegate;			
		}
    }
    #endregion

    #region Food Bar...
    private static Texture2D emptyImg = null;

    public void UpdateFoodSlot()
    {
        if (!FoodSoltManager.Instance)
        {
            return;
        }

        if (emptyImg == null)
        {
            emptyImg = new Texture2D(1, 1, TextureFormat.ARGB32, false);
            emptyImg.SetPixel(0, 0, new Color(1f, 1f, 1f, 0f));
            emptyImg.Apply();
        }

        int idx = -1;
        for (int i = 0; i < 3; i++)
        {
            InventorySlot info = FoodSoltManager.Instance.GetFoodItemData(i + 1);
            idx = PlayerDataManager.Instance.foodList[i];
            if (idx != -1)
            {
                _ItemInfo data = PlayerDataManager.Instance.GetBagItemData(idx);
                if (!data.empty)
                {
                    info.SetEmptyFlag(false);
                    info.SetData(data);
                    ItemPrefabs.Instance.GetItemIcon(data.localData._ItemID, data.localData._TypeID, data.localData._PrefabID, info.GetIcon());
                    info.ChangeBGColor(PlayerDataManager.Instance.GetNameColor(data.localData.info_eleVal + data.localData.info_encVal + data.localData.info_gemVal));
                    info.SetCount(data.count);
                }
                else
                {
                    PlayerDataManager.Instance.EmptyFoodSlot(i);
                    info.EmptySlot(emptyImg);
                }
            }
            else
            {
                info.EmptySlot(emptyImg);
            }
        }
    }
    #endregion
			
	#region MoneyBar
	private void AddKarmaDelegate() {
		if(isPopUpRecharge) {
			return;
		}
		if (Steamworks.activeInstance) {
			Steamworks.activeInstance.ShowShop("karma");
		}else {
		GUIManager.Instance.AddTemplate("KarmaRecharge");
		isPopUpRecharge = true;
		}
	}
	private void AddCrystalDelegate() {
		if(isPopUpRecharge) {
			return;
		}
		if (Steamworks.activeInstance) {
			Steamworks.activeInstance.ShowShop("crystal");
		}else {
			GUIManager.Instance.AddTemplate("CrystalRecharge");
			isPopUpRecharge = true;
		}
	}
	#endregion
	
	#region Recharge
	private bool isPopUpRecharge = false;
	private void ExitRechargeKarmaDelegate() {
		GUIManager.Instance.RemoveTemplate("KarmaRecharge");
		isPopUpRecharge = false;
	}
	private void ExitRechargeCrystalDelegate() {
		GUIManager.Instance.RemoveTemplate("CrystalRecharge");
		isPopUpRecharge = false;
	}
	private void RechargeKarmaValDelegate(string content) {
		if (Steamworks.activeInstance != null) {
			Steamworks.activeInstance.StartPayment(content);
		}
		switch(VersionManager.Instance.GetVersionType()) {
		case VersionType.WebVersion:
			Application.ExternalCall("select_gold", content);
			break;
		case VersionType.NormalClientVersion:
			StartCoroutine(VersionManager.Instance.SendMsgToServerForPay(content));
			break;
		default:
			StartCoroutine(VersionManager.Instance.SendMsgToServerForPay(content));
			break;
		}
	}
	
	private void InitKarmaRechargeData() {
		KarmaRechargeManager.Instance.SetKarmaInfo(PlayerDataManager.Instance.karmaRechargTitle);
		for(int i = 0;i<7;i++) {
			KarmaRechargeManager.Instance.SetKarmaVal(i,PlayerDataManager.Instance.rechargeValData.karmaVal[i]);
			KarmaRechargeManager.Instance.SetPayVal(i,PlayerDataManager.Instance.rechargeValData.karmaPayVal[i]);
		}
	}
	private void InitCrystalRechargeData() {
		CrystalRechargeManager.Instance.SetKarmaInfo(PlayerDataManager.Instance.crystalRechargTitle);
		for(int i = 0;i<7;i++) {
			CrystalRechargeManager.Instance.SetKarmaVal(i,PlayerDataManager.Instance.rechargeValData.crystalVal[i]);
			CrystalRechargeManager.Instance.SetPayVal(i,PlayerDataManager.Instance.rechargeValData.crystalPayVal[i]);
		}
	}
	#endregion

    public void InitShopAbility()
    {
        if (!UI_AbilityShop_Manager.Instance)
        {
            GUIManager.Instance.AddTemplate("Shop_BG");
            GUIManager.Instance.AddTemplate("Shop_BG2");
            GUIManager.Instance.AddTemplate("Shop_Ability");
            GUIManager.Instance.AddTemplate("SpeedUpBox");
        }
    }

    public void ExitShopAbility()
    {
        //GUIManager.Instance.RemoveTemplate("Shop_Ability");
        //GUIManager.Instance.RemoveTemplate("Shop_BG2");
        //GUIManager.Instance.RemoveTemplate("Shop_BG");
        //GUIManager.Instance.RemoveTemplate("SpeedUpBox");

        if (GUIManager.Instance != null)
            GUIManager.Instance.ChangeUIScreenState("IngameScreen");

        GameCamera.BackToPlayerCamera();
        Player.Instance.ReactivePlayer();
    }

    public void CooldownUpdated(ECooldownType _cdtype, int _id, long targettime)
	{
        UpdateMainCooldown();
        UpdateDetailCooldown();
    }

    public void LearnAbilitySuccess(bool isAbility, int AbiID)
    {
        if (isAbility)
        {
            SpeedUpAbility(true, AbiID, AbilityInfo.GetAbilityDetailInfoByID(AbiID).TrainingTime);
        }
        else
        {
            SpeedUpAbility(false, AbiID, Player.Instance.MasteryManager.GetMasteryByType(new EMasteryType(AbiID)).NextInfo.TrainTime);
        }
    }

	// when ability cooldown is finished, server send message to client.
    public void AbilityLevelUp(int _id)
    {
		if(UI_AbilityShop_Manager.Instance)
		{
	        AbilityDetailInfo.EnumAbilityType _type = (AbilityDetailInfo.EnumAbilityType)(_id / 100 * 100);
	        foreach (UI_TypeDefine.UI_AbiInfo_data _data in Abidatas)
	        {
	            if (_data.AbiType == _type)
	            {
	                _data.Level++;
	                UI_AbilityShop_Manager.Instance.AbilityLevelup(_data);
	                break;
	            }
	        }
	        AbilityItemBTNClicked((int)_type);
		}
    }

	// when mastery cooldown is finished, server send message to client.
    public void MasteryLevelup(int _id)
    {
        EMasteryType _type = null;
        switch (_id / 1000)
        {
            case 1:
                _type = new EMasteryType(EMasteryType.e1HWeaponType);
                break;
            case 2:
                _type = new EMasteryType(EMasteryType.eDualWeaponType);
                break;
            case 3:
                _type = new EMasteryType(EMasteryType.e2HWeaponType);
                break;
            case 4:
                _type = new EMasteryType(EMasteryType.eLightArmorType);
                break;
            case 5:
                _type = new EMasteryType(EMasteryType.eMediumArmorType);
                break;
            case 6:
                _type = new EMasteryType(EMasteryType.eHeavyArmorType);
                break;
        }
        foreach (UI_TypeDefine.UI_MasteryInfo_data _data in Masterydatas)
        {
            if (_data.MasteryType.Get() == _type.Get())
            {
                Debug.Log(_type.Get());
                _data.Level++;
                UI_AbilityShop_Manager.Instance.MasteryLevelup(_data);
                break;
            }
        }
        MasteryItemBTNClicked(_type.Get());
    }

    #region Delegate From AbilityShop

    void ChangeToAskUI(UI_TypeDefine.EnumAbilityShopUITYPE _askType)
    {
        if (_askType == UI_TypeDefine.EnumAbilityShopUITYPE.NONE ||
            _askType == UI_TypeDefine.EnumAbilityShopUITYPE.MAX ||
            _askType == UI_TypeDefine.EnumAbilityShopUITYPE.LoadingBundle ||
            (UI_AbilityShop_Manager.Instance && UI_AbilityShop_Manager.Instance.UIType == UI_TypeDefine.EnumAbilityShopUITYPE.LoadingBundle))
        {
            return;
        }

        // for datas (abi/mastery/cooldown)
        int _index = 0;

        #region search all ability info
        List<UI_TypeDefine.UI_AbiInfo_data> _abidatas = new List<UI_TypeDefine.UI_AbiInfo_data>();

        int StartAbi = 0;
        int EndAbi = 0;
        switch (_askType)
        {
            case UI_TypeDefine.EnumAbilityShopUITYPE.Prowess:
                StartAbi = (int)AbilityDetailInfo.EnumAbilityType.SwathOfDestruction;
                EndAbi = (int)AbilityDetailInfo.EnumAbilityType.MeteorOfRain;
                break;
            case UI_TypeDefine.EnumAbilityShopUITYPE.Fortitude:
                StartAbi = (int)AbilityDetailInfo.EnumAbilityType.SkinOfStone;
                EndAbi = (int)AbilityDetailInfo.EnumAbilityType.Chimend;
                break;
            case UI_TypeDefine.EnumAbilityShopUITYPE.Cunning:
                StartAbi = (int)AbilityDetailInfo.EnumAbilityType.SteadyShoot;
                EndAbi = (int)AbilityDetailInfo.EnumAbilityType.DarkHunter;
                break;
        }

        _index = 0;
        for (int i = StartAbi; i <= EndAbi; i += 100)
        {
            if (System.Enum.IsDefined(typeof(AbilityDetailInfo.EnumAbilityType), i))
            {
                UI_TypeDefine.UI_AbiInfo_data _newData = null;
                if (_index < Abidatas.Length)
                    _newData = Abidatas[_index];
                else
                    _newData = new UI_TypeDefine.UI_AbiInfo_data();
                _index++;

                _newData.AbiType = (AbilityDetailInfo.EnumAbilityType)i;
                PlayerAbilityManager _aniManager = (PlayerAbilityManager) Player.Instance.abilityManager ;
                foreach (PlayerAbilityBaseState _abi in _aniManager.Abilities)
                {
                    if (_abi.AbilityType == (AbilityDetailInfo.EnumAbilityType)i)
                    {
						_newData.AbiName = _abi.AbilityName;
						if(_abi.Info != null)
						{
                    		//_newData.IconSpriteName = AbilityInfo.Instance.AbilityIcons[_abi.Info.IconID].name;
						}
						if(_abi.icon != null)
							_newData.IconSpriteName = _abi.icon.name;
                        _newData.Level = _abi.Level;
                    }
                }
                _abidatas.Add(_newData);
            }
        }
        Abidatas = _abidatas.ToArray();
        _abidatas.Clear();
        _abidatas = null;
        #endregion

        #region search mastery info
        List<UI_TypeDefine.UI_MasteryInfo_data> _masterydatas = new List<UI_TypeDefine.UI_MasteryInfo_data>();

        EMasteryType weaponmastery = null;
        EMasteryType armormastery = null;

        switch (_askType)
        {
            case UI_TypeDefine.EnumAbilityShopUITYPE.Prowess:
                weaponmastery = new EMasteryType(EMasteryType.eDualWeaponType);
                armormastery = new EMasteryType(EMasteryType.eMediumArmorType);
                break;
            case UI_TypeDefine.EnumAbilityShopUITYPE.Fortitude:
                weaponmastery = new EMasteryType(EMasteryType.e2HWeaponType);
                armormastery = new EMasteryType(EMasteryType.eHeavyArmorType);
                break;
            case UI_TypeDefine.EnumAbilityShopUITYPE.Cunning:
                weaponmastery = new EMasteryType(EMasteryType.e1HWeaponType);
                armormastery = new EMasteryType(EMasteryType.eLightArmorType);
                break;
        }
        _index = 0;
        UI_TypeDefine.UI_MasteryInfo_data _newmasterydata = null;
        _newmasterydata = GetMasteryDataByType(weaponmastery, _index);
        if (_newmasterydata != null)
        {
            _masterydatas.Add(_newmasterydata);
            _index++;
        }
        _newmasterydata = GetMasteryDataByType(armormastery, _index);
        if (_newmasterydata != null)
        {
            _masterydatas.Add(_newmasterydata);
        }
        Masterydatas = _masterydatas.ToArray();
        _masterydatas.Clear();
        _masterydatas = null;
        #endregion

        if (UI_AbilityShop_Manager.Instance)
        {
            UI_AbilityShop_Manager.Instance.UpdateTitleType(_askType);
            UI_AbilityShop_Manager.Instance.UpdateMainAbilityInfo(Abidatas);
            UI_AbilityShop_Manager.Instance.UpdateMainMasteryInfo(Masterydatas);
        }

        UpdateMainCooldown();
    }

    void UpdateMainCooldown()
    {
        if (UI_AbilityShop_Manager.Instance)
        {
            #region collect cooldown info
            List<UI_TypeDefine.UI_LearnAbilityCoolDown_data> _cooldowndatas = new List<UI_TypeDefine.UI_LearnAbilityCoolDown_data>();
            int _index = 0;
            for (int i = 0; i < Abidatas.Length; i++)
            {
                ECooldownType _cooldowntype = new ECooldownType(ECooldownType.eCooldownType_Skill);
                int id = (int)Abidatas[i].AbiType + Abidatas[i].Level;
                if (PlayerDataManager.Instance.GetCoolDownTime(_cooldowntype, id) > 0)
                {
                    UI_TypeDefine.UI_LearnAbilityCoolDown_data _newdata = null;
                    if (_index < Cooldowndatas.Length)
                        _newdata = Cooldowndatas[_index];
                    else
                        _newdata = new UI_TypeDefine.UI_LearnAbilityCoolDown_data();
                    _newdata.IsAbilityOrMastery = true;
                    _newdata.AbiType = Abidatas[i].AbiType;
                    _newdata.CurAbiCooldown = PlayerDataManager.Instance.GetCoolDownTime(_cooldowntype, id);
                    if (Abidatas[i].Level < 6)
                        _newdata.MaxAbiCooldown = AbilityInfo.GetAbilityDetailInfoByID(id + 1).TrainingTime;
                    else
                        _newdata.MaxAbiCooldown = AbilityInfo.GetAbilityDetailInfoByID(id).TrainingTime;
                    _cooldowndatas.Add(_newdata);
                }
            }

            for (int i = 0; i < Masterydatas.Length; i++)
            {
                SingleMastery mastery = Player.Instance.MasteryManager.GetMasteryByType(Masterydatas[i].MasteryType);
                if (mastery != null)
                {
                    ECooldownType _cooldowntype = new ECooldownType(ECooldownType.eCooldownType_Mastery);
                    int id = 0;
                    if (mastery.Info != null) id = mastery.Info.ID;
                    else id = mastery.BaseInfo.ID - 1;  // if there is no info for this mastery, that means player doesn't have this mastery;
                    if (PlayerDataManager.Instance.GetCoolDownTime(_cooldowntype, id) > 0)
                    {
                        UI_TypeDefine.UI_LearnAbilityCoolDown_data _newdata = null;
                        if (_index < Cooldowndatas.Length)
                            _newdata = Cooldowndatas[_index];
                        else
                            _newdata = new UI_TypeDefine.UI_LearnAbilityCoolDown_data();
                        _newdata.IsAbilityOrMastery = false;
                        _newdata.MasteryType = mastery.MasteryClass;
                        _newdata.CurAbiCooldown = PlayerDataManager.Instance.GetCoolDownTime(_cooldowntype, id);
                        _newdata.MaxAbiCooldown = mastery.NextInfo.TrainTime;
                        _cooldowndatas.Add(_newdata);
                    }
                }
            }
            Cooldowndatas = _cooldowndatas.ToArray();
            _cooldowndatas.Clear();
            _cooldowndatas = null;
            #endregion

            UI_AbilityShop_Manager.Instance.UpdateAbilityCoolDown(Cooldowndatas);
        }
    }

    /// <summary>
    /// create mastery data
    /// </summary>
    /// <param name="_mastery"></param>
    /// <param name="_index"></param>
    /// <returns></returns>
    UI_TypeDefine.UI_MasteryInfo_data GetMasteryDataByType(EMasteryType _mastery, int _index)
    {
        if (_mastery != null)
        {
            UI_TypeDefine.UI_MasteryInfo_data _newmasteryData = null;
            if (_index < Masterydatas.Length)
                _newmasteryData = Masterydatas[_index];
            else
                _newmasteryData = new UI_TypeDefine.UI_MasteryInfo_data();

            SingleMastery _wm = Player.Instance.MasteryManager.GetMasteryByType(_mastery);
            _newmasteryData.MasteryType = _mastery;
            _newmasteryData.Level = _wm.CurLv;
            _newmasteryData.MasteryName = _wm.BaseInfo.shortName;
            _newmasteryData.IconSpriteName = MasteryInfo.Instance.GetIconByType(_mastery).name;
            return _newmasteryData;
        }
        return null;
    }

    void AbilityItemBTNClicked(int AbiID)
    {
        PlayerAbilityManager _aniManager = (PlayerAbilityManager)Player.Instance.abilityManager;
        foreach (PlayerAbilityBaseState _abi in _aniManager.Abilities)
        {
            if ((int)_abi.AbilityType == AbiID)
            {
                _detaildata.AbiID = AbiID;
                _detaildata.IsAbility = true;
                _detaildata.AbiName = _abi.AbilityName;
				if(_abi.Info != null)
				{
                	//_detaildata.IconSpriteName = AbilityInfo.Instance.AbilityIcons[_abi.Info.IconID].name;
				}
				if(_abi.icon != null) _detaildata.IconSpriteName = _abi.icon.name;
                _detaildata.AbiCurLV = _abi.Level;
                _detaildata.AbiMaxLV = 6;
                if (_detaildata.AbiCurLV > 0 && _abi.Info != null)
                {
                    _detaildata.AbiName = _abi.AbilityName;
                    _detaildata.AbiCurLVDescription = _abi.Info.Description1;
                    _detaildata.AbiCurLVDescription += "\n[00c6ff]Energy Required : " + _abi.Info.ManaCost + "[-]";
                    if (_abi.Info.AddEffectTitle1.Length > 0) _detaildata.AbiCurLVDescription += "\n[ff0000]" + _abi.Info.AddEffectTitle1 +" : [-]";
                    if (_abi.Info.AddEffectDesc1.Length > 0) _detaildata.AbiCurLVDescription += "\n" + _abi.Info.AddEffectDesc1;
                    if (_abi.Info.AddEffectTitle2.Length > 0) _detaildata.AbiCurLVDescription += "\n[ff0000]" + _abi.Info.AddEffectTitle2 + " : [-]";
                    if (_abi.Info.AddEffectDesc2.Length > 0) _detaildata.AbiCurLVDescription += "\n" + _abi.Info.AddEffectDesc2;
                }
                if (_detaildata.AbiCurLV < _detaildata.AbiMaxLV)
                {
                    AbilityDetailInfo info = AbilityInfo.GetAbilityDetailInfoByID(AbiID + _detaildata.AbiCurLV + 1);
                    _detaildata.AbiNextLVDescription = info.Description1;
                    _detaildata.AbiNextLVDescription += "\n[00c6ff]Energy Required : " + info.ManaCost + "[-]";
                    if (info.AddEffectTitle1.Length > 0) _detaildata.AbiNextLVDescription += "\n[ff0000]" + info.AddEffectTitle1 + " : [-]";
                    if (info.AddEffectDesc1.Length > 0) _detaildata.AbiNextLVDescription += "\n" + info.AddEffectDesc1;
                    if (info.AddEffectTitle2.Length > 0) _detaildata.AbiNextLVDescription += "\n[ff0000]" + info.AddEffectTitle2 + " : [-]";
                    if (info.AddEffectDesc2.Length > 0) _detaildata.AbiNextLVDescription += "\n" + info.AddEffectDesc2;
                    if (info.Extra.Length > 0) _detaildata.AbiNextLVDescription += "\n[ffd200]Extra : " + info.Extra + "[-]";
                    _detaildata.NeedAttr = info.Level.ToString();
                    switch (info.Discipline)
                    {
                        case AbilityDetailInfo.EDisciplineType.EDT_Prowess:
                            _detaildata.NeedAttr += " POW";
                            _detaildata.HaveAttr = PlayerDataManager.Instance.GetBaseAttrs(EAttributeType.ATTR_Power) + " POW";
                            break;
                        case AbilityDetailInfo.EDisciplineType.EDT_Fortitude:
                            _detaildata.NeedAttr += " DEF";
                            _detaildata.HaveAttr = PlayerDataManager.Instance.GetBaseAttrs(EAttributeType.ATTR_Defense) + " DEF";
                            break;
                        case AbilityDetailInfo.EDisciplineType.EDT_Cunning:
                            _detaildata.NeedAttr += " SKILL";
                            _detaildata.HaveAttr = PlayerDataManager.Instance.GetBaseAttrs(EAttributeType.ATTR_Skill) + " SKILL";
                            break;
                    }
                    _detaildata.TrainingTime = (int)info.TrainingTime;
                    _detaildata.KarmaCost = info.Karma;
                }
            }
        }

        if (UI_AbilityShop_Manager.Instance)
        {
            UI_AbilityShop_Manager.Instance.UpdateAbiiltyDetailInfo(_detaildata);
        }

        UpdateDetailCooldown();
    }
	
	void MasteryItemBTNClicked(int mastID)
	{
		SingleMastery _mastery = Player.Instance.MasteryManager.GetMasteryByType(new EMasteryType(mastID));
        _detaildata.AbiID = mastID;
        _detaildata.IsAbility = false;
        _detaildata.AbiName = _mastery.BaseInfo.shortName;
        _detaildata.IconSpriteName = MasteryInfo.Instance.GetIconByType(_mastery.MasteryClass).name;
        _detaildata.AbiCurLV = _mastery.CurLv;
        _detaildata.AbiMaxLV = 10;
        if(_detaildata.AbiCurLV > 0 && _mastery.Info != null)
        {
            _detaildata.AbiCurLVDescription = _mastery.Info.Description;
            _detaildata.AbiCurLVDescription = _detaildata.AbiCurLVDescription.Replace("[ff0000]Condition:[-]", "\n[ff0000]Condition:[-]");
            _detaildata.AbiCurLVDescription += "\n" + _mastery.Info.Description2;
        }
        if (_detaildata.AbiCurLV < _detaildata.AbiMaxLV)
        {
            _detaildata.AbiNextLVDescription = "";
            if (_detaildata.AbiCurLV == 0)
            {
                _detaildata.AbiNextLVDescription += _mastery.NextInfo.Description + "\n";
                _detaildata.AbiNextLVDescription = _detaildata.AbiNextLVDescription.Replace("[ff0000]Condition:[-]", "\n[ff0000]Condition:[-]");
            }
            _detaildata.AbiNextLVDescription += _mastery.NextInfo.Description2;
            _detaildata.NeedAttr = _mastery.NextInfo.BaseStateLvNeeded.ToString();
            switch (_mastery.NextInfo.Discipline)
            {
                case AbilityDetailInfo.EDisciplineType.EDT_Prowess:
                    _detaildata.NeedAttr += " POW";
                    _detaildata.HaveAttr = Player.Instance.AttrMan.Attrs[EAttributeType.ATTR_Power] + " POW";
                    break;
                case AbilityDetailInfo.EDisciplineType.EDT_Fortitude:
                    _detaildata.NeedAttr += " DEF";
                    _detaildata.HaveAttr = Player.Instance.AttrMan.Attrs[EAttributeType.ATTR_Defense] + " DEF";
                    break;
                case AbilityDetailInfo.EDisciplineType.EDT_Cunning:
                    _detaildata.NeedAttr += " SKILL";
                    _detaildata.HaveAttr = Player.Instance.AttrMan.Attrs[EAttributeType.ATTR_Skill] + " SKILL";
                    break;
            }
            _detaildata.TrainingTime = _mastery.NextInfo.TrainTime;
            _detaildata.KarmaCost = _mastery.NextInfo.karmaCost;
        }
		
		if (UI_AbilityShop_Manager.Instance)
        {
            UI_AbilityShop_Manager.Instance.UpdateAbiiltyDetailInfo(_detaildata);
        }

        UpdateDetailCooldown();
	}

    void UpdateDetailCooldown()
    {
        int id = 0;
        ECooldownType _cooldowntype = new ECooldownType(ECooldownType.eCooldownType_Mastery);
        UI_TypeDefine.UI_LearnAbilityCoolDown_data _newdata = new UI_TypeDefine.UI_LearnAbilityCoolDown_data();
        _newdata.IsAbilityOrMastery = true;

        if (_detaildata.IsAbility)
        {
            _cooldowntype = new ECooldownType(ECooldownType.eCooldownType_Skill);
            id = _detaildata.AbiID + _detaildata.AbiCurLV;
        }
        else
        {
            SingleMastery mastery = Player.Instance.MasteryManager.GetMasteryByType(new EMasteryType(_detaildata.AbiID));
            if (mastery)
            {
                if (mastery.Info != null) id = mastery.Info.ID;
                else id = mastery.BaseInfo.ID - 1;  // if there is no info for this mastery, that means player doesn't have this mastery;
            }
        }

        bool isshow = true; // show cool down? if the ability is top level, hide cool down.
        bool isstart = false; // start counting cool down? if this ability/mastery is in cool down, start counting.
        
		// if there is a cooldown info for this id.
		if (PlayerDataManager.Instance.GetCoolDownTime(_cooldowntype, id) > 0)
        {
            isstart = true;

            if (_detaildata.IsAbility)
            {
                _newdata.AbiType = (AbilityDetailInfo.EnumAbilityType)_detaildata.AbiID;
                _newdata.MaxAbiCooldown = AbilityInfo.GetAbilityDetailInfoByID(id + 1).TrainingTime;
            }
            else
            {
                SingleMastery mastery = Player.Instance.MasteryManager.GetMasteryByType(new EMasteryType(_detaildata.AbiID));
                if (mastery)
                {
                    if (mastery.Info != null)
                        _newdata.MaxAbiCooldown = Player.Instance.MasteryManager.GetMasteryByType(new EMasteryType(_detaildata.AbiID)).NextInfo.TrainTime;
                    else
                        _newdata.MaxAbiCooldown = Player.Instance.MasteryManager.GetMasteryByType(new EMasteryType(_detaildata.AbiID)).BaseInfo.TrainTime;
                }
            }

            _newdata.CurAbiCooldown = PlayerDataManager.Instance.GetCoolDownTime(_cooldowntype, id);
        }
        else
        {
            if (_detaildata.AbiCurLV >= _detaildata.AbiMaxLV)
            {
                isshow = false;
            }
            else
            {
                if (_detaildata.IsAbility)
                {
                    _newdata.MaxAbiCooldown = AbilityInfo.GetAbilityDetailInfoByID(id + 1).TrainingTime;
                }
                else
                {
                    SingleMastery mastery = Player.Instance.MasteryManager.GetMasteryByType(new EMasteryType(_detaildata.AbiID));
                    if (mastery && mastery.NextInfo != null)
                    {
                        _newdata.MaxAbiCooldown = Player.Instance.MasteryManager.GetMasteryByType(new EMasteryType(_detaildata.AbiID)).NextInfo.TrainTime;
                    }
                }
            }
            _newdata.CurAbiCooldown = _newdata.MaxAbiCooldown;
        }
        if (UI_AbilityShop_Manager.Instance)
        {
            UI_AbilityShop_Manager.Instance.UpdateDetailAbilityCoolDown(isshow, isstart, _newdata);
        }
    }

    void CloseAbilityShop()
    {
        ExitShopAbility();
    }

    void LearnAbility(bool isAbility, int AbiID)
    {
        EMoneyType MoneyType = new EMoneyType(EMoneyType.eMoneyType_SK);
        if (!isAbility)
        {
            CS_Main.Instance.g_commModule.SendMessage(
                    ProtocolGame_SendRequest.masteryLvlupReq(new EMasteryType(AbiID))
            );
        }
        else
        {
            CS_Main.Instance.g_commModule.SendMessage(
                ProtocolGame_SendRequest.StudyAbility(AbiID, AbiID, MoneyType)
            );
        }
    }

    void SpeedUpAbility(bool isAbility, int AbiID, float CurCooldown)
    {
        SpeedUpBoxManager.Instance.PopUpSpeedUpBox(CurCooldown, "");
		SpeedUpBoxManager.Instance.PopUpSpeedUpBox(CurCooldown, "");
        SpeedupBox_IsAbility = isAbility;
		SpeedupBox_CurAbiID = AbiID;
        if (!isAbility)
        {
            SingleMastery mastery = Player.Instance.MasteryManager.GetMasteryByType(new EMasteryType(AbiID));
            if (mastery)
            {
                if (mastery.NextInfo != null) SpeedupBox_CurAbiID = mastery.NextInfo.ID;
                else SpeedupBox_CurAbiID = mastery.BaseInfo.ID;  // if there is no info for this mastery, that means player doesn't have this mastery;
            }
        }
    }

    #endregion

    #region Delegate From SpeedUp
    bool SpeedupBox_IsAbility;
    int SpeedupBox_CurAbiID;

    void SpeedUpHalfHour()
    {
        ECooldownType cooldownType = new ECooldownType();
        EDecreaseCooldownType decreaseCoolDownTyep = new EDecreaseCooldownType();
        decreaseCoolDownTyep.Set(EDecreaseCooldownType.eDecreaseCooldownType_30);

        if (SpeedupBox_IsAbility) cooldownType.Set(ECooldownType.eCooldownType_Skill);
        else cooldownType.Set(ECooldownType.eCooldownType_Mastery);

        CS_Main.Instance.g_commModule.SendMessage(ProtocolGame_SendRequest.SpeedUpCoolDown(cooldownType, SpeedupBox_CurAbiID, decreaseCoolDownTyep));
		
		if(SpeedUpBoxManager.Instance) SpeedUpBoxManager.Instance.Hide();
    }

    void SpeedUpHour()
    {
        ECooldownType cooldownType = new ECooldownType();
        EDecreaseCooldownType decreaseCoolDownTyep = new EDecreaseCooldownType();
        decreaseCoolDownTyep.Set(EDecreaseCooldownType.eDecreaseCooldownType_60);

        if (SpeedupBox_IsAbility) cooldownType.Set(ECooldownType.eCooldownType_Skill);
        else cooldownType.Set(ECooldownType.eCooldownType_Mastery);

        CS_Main.Instance.g_commModule.SendMessage(ProtocolGame_SendRequest.SpeedUpCoolDown(cooldownType, SpeedupBox_CurAbiID, decreaseCoolDownTyep));
		
		if(SpeedUpBoxManager.Instance) SpeedUpBoxManager.Instance.Hide();
    }

    void SpeedUpFinish()
    {
        ECooldownType cooldownType = new ECooldownType();
        EDecreaseCooldownType decreaseCoolDownTyep = new EDecreaseCooldownType();
        decreaseCoolDownTyep.Set(EDecreaseCooldownType.eDecreaseCooldownType_All);

        if (SpeedupBox_IsAbility) cooldownType.Set(ECooldownType.eCooldownType_Skill);
        else cooldownType.Set(ECooldownType.eCooldownType_Mastery);

        CS_Main.Instance.g_commModule.SendMessage(ProtocolGame_SendRequest.SpeedUpCoolDown(cooldownType, SpeedupBox_CurAbiID, decreaseCoolDownTyep));
		
		if(SpeedUpBoxManager.Instance) SpeedUpBoxManager.Instance.Hide();
    }

    #endregion
}
