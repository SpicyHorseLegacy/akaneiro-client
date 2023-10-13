using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InGameScreenShopConsumableItemCtrl : BaseScreenCtrl
{
    public static InGameScreenShopConsumableItemCtrl Instance;

    override protected void Awake() { base.Awake(); Instance = this; }

    #region Event
    protected override void RegisterSingleTemplateEvent(string _templateName)
    {
        base.RegisterSingleTemplateEvent(_templateName);

        if (_templateName == "Shop_ConsumableItem" && UI_ConsumableItemShop_Manager.Instance)
        {
            UI_ConsumableItemShop_Manager.Instance.ConsumableItemCloseClicked_Event += ExitConsumableItemShop;
            UI_ConsumableItemShop_Manager.Instance.ConsumableItemTopBarClicked_Event += UpdateAllShopInfo;

            InitScreen();
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

        if (_templateName == "Shop_ConsumableItem" && UI_ConsumableItemShop_Manager.Instance)
        {
            UI_ConsumableItemShop_Manager.Instance.ConsumableItemCloseClicked_Event -= ExitConsumableItemShop;
            UI_ConsumableItemShop_Manager.Instance.ConsumableItemTopBarClicked_Event -= UpdateAllShopInfo;
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
		GUIManager.Instance.AddTemplate("KarmaRecharge");
		isPopUpRecharge = true;
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

    public void ExitConsumableItemShop()
    {
        if (GUIManager.Instance != null)
            GUIManager.Instance.ChangeUIScreenState("IngameScreen");

        GameCamera.BackToPlayerCamera();
        Player.Instance.ReactivePlayer();
    }

    public void BuyItemSuccess(int itemType, int itemID, int buycount)
    {
        UI_ConsumableItemShop_Manager.Instance.BuyItemSuccess(itemType, itemID, buycount);
    }

    void InitScreen()
    {
        if (UI_ConsumableItemShop_Manager.Instance)
        {
            UI_ConsumableItemShop_Manager.Instance.TopBarBTNClicked(UI_TypeDefine.EnumConsumableItemShopUITYPE.Drink);
        }
    }

    void UpdateAllShopInfo(UI_TypeDefine.EnumConsumableItemShopUITYPE _type)
    {
        if (UI_ConsumableItemShop_Manager.Instance)
        {
            List<UI_TypeDefine.UI_ConsumableItemShop_Item_data> _templist = new List<UI_TypeDefine.UI_ConsumableItemShop_Item_data>();
            foreach (ConsumableItemInfo _info in ConsumableItemsInfoManager.GetAllInfoByType((ConsumableItemInfo.Enum_ConsumableType)((int)_type)))
            {
                UI_TypeDefine.UI_ConsumableItemShop_Item_data _newdata = new UI_TypeDefine.UI_ConsumableItemShop_Item_data();
                _newdata.ID = _info.ID;
                _newdata.ItemName = _info.Name;
                _newdata.ShortDescription = _info.ShortDescription;
                ItemDeployInfo.SItemProps _propinfo = ItemDeployInfo.GetItemPropsByID(_info.ID);
                _newdata.PriceType = new EMoneyType(_propinfo.isUseRealMoney == 0 ? 2 : 1);
                _newdata.Price = _propinfo.saleVal;
                _newdata.FullDescription = _propinfo.propsDes2;
                _templist.Add(_newdata);
            }
            UI_ConsumableItemShop_Manager.Instance.UpdateMainConsumableInfo(_templist.ToArray(), _type);
            _templist.Clear();
            _templist = null;
        }
    }
}
