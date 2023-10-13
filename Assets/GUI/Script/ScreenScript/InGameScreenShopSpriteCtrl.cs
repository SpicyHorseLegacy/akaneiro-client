using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InGameScreenShopSpriteCtrl : BaseScreenCtrl
{
    public static InGameScreenShopSpriteCtrl Instance;

    override protected void Awake() { base.Awake(); Instance = this; }

    #region Event
    protected override void RegisterSingleTemplateEvent(string _templateName)
    {
        base.RegisterSingleTemplateEvent(_templateName);

        if (_templateName == "Shop_Sprite" && UI_SpriteShop_Manager.Instance)
        {
            UI_SpriteShop_Manager.Instance.SpriteShopCloseClicked_Event += ExitShopSprite;
            UI_SpriteShop_Manager.Instance.SpriteShopItemClicked_Event += ShowPetDetailInfo;
            UI_SpriteShop_Manager.Instance.SpriteSummonBTNClicked_Event += SummonPet;
            UI_SpriteShop_Manager.Instance.SpriteChooseBTNClicked_Event += ChoosePet;
            UpdatePetItemsInfo();
        }
		
		if(_templateName == "Shop_Sprite_Success" && UI_SpriteShop_SuccessPanel.Instance)
		{
            string _petname = PetsInfo.GetPetListInfoByID(choosePetID).m_name;
            GameObject _model = GetPetModel(_petname, false);
			UI_SpriteShop_SuccessPanel.Instance.ShowPetModel(_model, _petname);
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

        if (_templateName == "Shop_Sprite" && UI_SpriteShop_Manager.Instance)
        {
            UI_SpriteShop_Manager.Instance.SpriteShopCloseClicked_Event -= ExitShopSprite;
            UI_SpriteShop_Manager.Instance.SpriteShopItemClicked_Event -= ShowPetDetailInfo;
            UI_SpriteShop_Manager.Instance.SpriteSummonBTNClicked_Event -= SummonPet;
            UI_SpriteShop_Manager.Instance.SpriteChooseBTNClicked_Event -= ChoosePet;
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
	
    #region Public

    public void ExitShopSprite()
    {
        if (GUIManager.Instance != null)
            GUIManager.Instance.ChangeUIScreenState("IngameScreen");

        GameCamera.BackToPlayerCamera();
        Player.Instance.ReactivePlayer();
    }

    public void UpdatePetItemsInfo()
    {
        if (UI_SpriteShop_Manager.Instance)
        {
            List<UI_TypeDefine.UI_SpriteShop_PetItem_data> _petdatas = new List<UI_TypeDefine.UI_SpriteShop_PetItem_data>();
            foreach (SinglePetInfo _petinfo in Player.Instance.PetManager.BasePetsInfo.Values)
            {
                if (_petinfo.IsShow)
                {
                    UI_TypeDefine.UI_SpriteShop_PetItem_data _data = new UI_TypeDefine.UI_SpriteShop_PetItem_data();
                    _data.PetID = _petinfo.CurLvPetInfo.m_ID;
					_data.isKSPet = !_petinfo.CurLvPetInfo.m_IsShopShow;
                    _data.isLocked = (PlayerDataManager.Instance.CurLV < _petinfo.CurLvPetInfo.m_LevelRequid);
                    _data.RequirdLevel = _petinfo.CurLvPetInfo.m_LevelRequid;
                    _data.PetName = _petinfo.CurLvPetInfo.m_name;
                    _data.PetIconName = PetsInfo.GetPetIconByType(_petinfo.CurLvPetInfo.m_Type).name;
                    _data.PetSimpleIconName = PetsInfo.GetPetSimpleIconByType(_petinfo.CurLvPetInfo.m_Type).name;
                    _data.PetSimpleDescription = _petinfo.CurLvPetInfo.m_ShortDescription;
                    _data.PetDetailDescription = _petinfo.CurLvPetInfo.m_BuffDescription;
                    _data.IsMaxLv = true;
                    _data.CurLevel = _petinfo.CurLevel;
                    _data.CurExp = _petinfo.CurEXP;
                    if (_petinfo.NextLvPetInfo != null)
                    {
                        _data.IsMaxLv = false;
						_data.MaxExp = _petinfo.NextLvPetInfo.m_MaxExp;
					}
                    _data.BuyTime = (long)_petinfo.BuyTime;
					_data.MaxTime = (long)_petinfo.LeftTime;
                    _data.Price1HourValue = _petinfo.CurLvPetInfo.Price_Hour;
                    _data.Price1HourType = _petinfo.CurLvPetInfo.MoneyType_Hour;
                    _data.Price1DayValue = _petinfo.CurLvPetInfo.Price_Day;
                    _data.Price1DayType = _petinfo.CurLvPetInfo.MoneyType_Day;
                    _data.Price1WeekValue = _petinfo.CurLvPetInfo.Price_Week;
                    _data.Price1WeekType = _petinfo.CurLvPetInfo.MoneyType_Week;

                    _petdatas.Add(_data);
                }
            }
            UI_SpriteShop_Manager.Instance.InitAllPets(_petdatas.ToArray());
        }
    }

    public int choosePetID;
    public void ChoosePetSuccess(int _petid)
    {
        choosePetID = _petid;
        GUIManager.Instance.AddTemplate("Shop_Sprite_Success");
    }
    #endregion

    #region Detail
    void ShowPetDetailInfo(UI_TypeDefine.UI_SpriteShop_PetItem_data petdata)
    {
        if (UI_SpriteShop_Manager.Instance)
        {
            GameObject _newpetmodel = GetPetModel(petdata.PetName, petdata.isLocked);
            UI_SpriteShop_Manager.Instance.ShowPetModel(_newpetmodel);
        }
    }

    GameObject GetPetModel(string _petname, bool _islocked)
    {
        GameObject _newpetmodel = UnityEngine.Object.Instantiate(PetsInfo.GetModelByName(_petname)) as GameObject;
        foreach (Transform _t in _newpetmodel.GetComponentsInChildren<Transform>())
        {
            _t.gameObject.layer = LayerMask.NameToLayer("NGUI");
            if (_t.renderer != null)
            {
                for (int i = 0; i < _t.renderer.materials.Length; i++)
                {
                    Material _mat = _t.renderer.materials[i];
                    if (_mat.HasProperty("_EdgeWidth"))
                    {
                        _mat.SetFloat("_EdgeWidth", 0);
                    }

                    if (_islocked && _mat.HasProperty("_EmissiveColor"))
                    {
                        _mat.SetColor("_EmissiveColor", Color.black);
                        if (_mat.HasProperty("_EdgeWidth"))
                        {
                            _mat.SetFloat("_EdgeWidth", 100);
                        }
                    }
                }
            }
        }
        if (_newpetmodel.GetComponent<SpiritCreature>()) UnityEngine.Object.Destroy(_newpetmodel.GetComponent<SpiritCreature>());
		return _newpetmodel;
    }

    void SummonPet
		(int _petid, int _buytime)
    {
        CS_Main.Instance.g_commModule.SendMessage(
                    ProtocolGame_SendRequest.BuyPet(_petid, _buytime)
                );
    }

    void ChoosePet(int _petid)
    {
        CS_Main.Instance.g_commModule.SendMessage(
                    ProtocolGame_SendRequest.choosePet(_petid)
                );
    }
    #endregion
}
