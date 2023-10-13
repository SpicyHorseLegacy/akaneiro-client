using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class InGameScreenShopCraftingCtrl : BaseScreenCtrl
{
	public static InGameScreenShopCraftingCtrl Instance;

    override protected void Awake() { base.Awake(); Instance = this; }

    #region Event
    protected override void RegisterSingleTemplateEvent(string _templateName)
    {
        base.RegisterSingleTemplateEvent(_templateName);

        if (_templateName == "Shop_Crafting" && UI_CraftingShop_Manager.Instance)
        {
            UI_CraftingShop_Manager.Instance.CraftingShopCloseClicked_Event += ExitShopCrafting;
            UI_CraftingShop_Manager.Instance.CraftingShopSlotClicked_Event += SetCurShowingItemInfo;
            UI_CraftingShop_Manager.Instance.CraftingShopAttrClicked_Event += CollectAttributeInfo; 

            UpdateEquipmentAndInventory();
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

        if (_templateName == "Shop_Crafting" && UI_CraftingShop_Manager.Instance)
        {
            UI_CraftingShop_Manager.Instance.CraftingShopCloseClicked_Event -= ExitShopCrafting;
            UI_CraftingShop_Manager.Instance.CraftingShopSlotClicked_Event -= SetCurShowingItemInfo;
            UI_CraftingShop_Manager.Instance.CraftingShopAttrClicked_Event -= CollectAttributeInfo;
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

    public void InitShopCrafting()
    {
        if (!UI_AbilityShop_Manager.Instance)
        {
            GUIManager.Instance.AddTemplate("Shop_BG");
            GUIManager.Instance.AddTemplate("Shop_BG2");
            GUIManager.Instance.AddTemplate("Shop_Crafting");
        }
    }

    //public 

    public void ExitShopCrafting()
    {
        if (GUIManager.Instance != null)
            GUIManager.Instance.ChangeUIScreenState("IngameScreen");

        GameCamera.BackToPlayerCamera();
        Player.Instance.ReactivePlayer();
    }
	
    // buy material success, update count of material. ball back from server.
	public void BuyMaterialSuc(int itemType)
	{
		
        if (UI_CraftingShop_Manager.Instance)
		{
            UI_CraftingShop_Manager.Instance.CraftItemAttribute(UI_CraftingShop_Manager.Instance.CurAttribute);
			UI_CraftingShop_Manager.Instance.BuyMaterialSuccess();
		}
	}
	
    // crafting success. ball back from server.
	public void CraftingOK()
	{
		if(UI_CraftingShop_Manager.Instance)
        	UI_CraftingShop_Manager.Instance.CraftingOK();
	}

    // crafting failed. ball back from server.
	public void CraftingFailed()
	{
		if(UI_CraftingShop_Manager.Instance)
			UI_CraftingShop_Manager.Instance.CraftingFailed();
	}
	
	#endregion

    #region Local

    void UpdateEquipmentAndInventory()
    {
        if (UI_CraftingShop_Manager.Instance)
        {
			UI_CraftingShop_Manager.Instance.ShowItemsOrCraft(true);

            List<_ItemInfo> _equipmentsinfo = new List<_ItemInfo>();
            foreach (_ItemInfo data in PlayerDataManager.Instance.equipList)
            {
                if(!data.empty)
                    _equipmentsinfo.Add(data);
            }

            UI_CraftingShop_Manager.Instance.UpdateEquipItems(_equipmentsinfo.ToArray());

            _equipmentsinfo.Clear();
            foreach (_ItemInfo data in PlayerDataManager.Instance.bagList)
            {
                if (!data.empty && ItemPrefabs.Instance.IsEquipement(data.localData._ItemID))
				{
					_equipmentsinfo.Add(data);
				}
            }
            UI_CraftingShop_Manager.Instance.UpdateBagItems(_equipmentsinfo.ToArray());

            _equipmentsinfo.Clear();
            _equipmentsinfo = null;
        }
    }

    // player have clicked an item slot.
    void SetCurShowingItemInfo(_ItemInfo _info)
    {
        
    }

    // Click an attribute in detail panel, show craft materials on right side.
    void CollectAttributeInfo(ItemDropStruct _iteminfo, UI_TypeDefine.ENUM_UI_CraftingShop_ItemAttribute _attrType)
    {
        if (_attrType != UI_TypeDefine.ENUM_UI_CraftingShop_ItemAttribute.NONE && _attrType != UI_TypeDefine.ENUM_UI_CraftingShop_ItemAttribute.MAX)
        {
			// get the recipe.
            int attrID = 0;
            UpgradeRecipes.UpgradeType _recipetype = UpgradeRecipes.UpgradeType.NONE;
            switch (_attrType)
            {
                case UI_TypeDefine.ENUM_UI_CraftingShop_ItemAttribute.Level:
                    attrID = _iteminfo._LVID;
                    _recipetype = UpgradeRecipes.UpgradeType.LEVELUP;
                    break;
                case UI_TypeDefine.ENUM_UI_CraftingShop_ItemAttribute.Element:
                    attrID = _iteminfo._EleID;
					_recipetype = UpgradeRecipes.UpgradeType.ELEM;
                    break;
                case UI_TypeDefine.ENUM_UI_CraftingShop_ItemAttribute.Enchant:
                    attrID = _iteminfo._EnchantID;
					_recipetype = UpgradeRecipes.UpgradeType.ENCHANT;
                    break;
                case UI_TypeDefine.ENUM_UI_CraftingShop_ItemAttribute.Gem:
                    attrID = _iteminfo._GemID;
					_recipetype = UpgradeRecipes.UpgradeType.GEM;
                    break;
            }

            UpgradeRecipe curRecipe = UpgradeRecipes.Instance.GetRecipe(attrID + 1, getRecipeInfos(attrID, _iteminfo._TypeID, _recipetype));
            UI_CraftingShop_Manager.Instance.UpdateCraftAllInfo(curRecipe, _iteminfo, _attrType);
        }
        else
            Debug.LogWarning("[Crafting] Can't collect attribute info, because the attribute type is not correct! Please have a check!");
    }

    int getRecipeInfos(int attrID, int itemType, UpgradeRecipes.UpgradeType upgradeType)
    {
        if (upgradeType == UpgradeRecipes.UpgradeType.LEVELUP)
            return 8;

        if (upgradeType == UpgradeRecipes.UpgradeType.GEM)
            return 7;

        if (itemType == 7 || itemType == 8)
        {

            if (upgradeType == UpgradeRecipes.UpgradeType.ELEM)
                return 1;
            else if (upgradeType == UpgradeRecipes.UpgradeType.ENCHANT)
                return 2;
        }
        else if (itemType == 2 || itemType == 5)
        {
            if (upgradeType == UpgradeRecipes.UpgradeType.ELEM)
                return 5;
            else if (upgradeType == UpgradeRecipes.UpgradeType.ENCHANT)
                return 6;
        }
        else
        {
            if (upgradeType == UpgradeRecipes.UpgradeType.ELEM)
                return 3;
            else if (upgradeType == UpgradeRecipes.UpgradeType.ENCHANT)
                return 4;
        }
        return 0;
    }

    #endregion
}
