using UnityEngine;
using System.Collections;

public class ItemShopScreenCtrl : BaseScreenCtrl {
	
	public static ItemShopScreenCtrl Instance;
	
	override protected void Awake() { base.Awake(); Instance = this; }
	
	#region Local
	#region event create and destory
	
	protected override void RegisterSingleTemplateEvent(string _templateName)
    {
        base.RegisterSingleTemplateEvent(_templateName);
		
		GetComponent<InventoryCtrl>().RegisterSingleTemplateEvent(_templateName);

        if (_templateName == "ItemShop" && ItemShopManager.Instance)
        {
			ItemShopManager.Instance.OnExitDelegate += ExitDelegate;
			ItemShopManager.Instance.OnTabDelegate += TabDelegate;
			
			ItemShopManager.Instance.UpdateTab(1);
			UpdateItemShopList(1);

		}
		if(_templateName == "TipsManager" && TipManager.Instance)
		{
			HideTip();
		}
	}
	
	protected override void UnregisterSingleTemplateEvent(string _templateName)
    {
        base.UnregisterSingleTemplateEvent(_templateName);
		
		GetComponent<InventoryCtrl>().UnregisterSingleTemplateEvent(_templateName);
		
		if (_templateName == "ItemShop" && ItemShopManager.Instance)
        {
			ItemShopManager.Instance.OnExitDelegate -= ExitDelegate;
			ItemShopManager.Instance.OnTabDelegate -= TabDelegate;
		}
	}
	
	#endregion
	private int curType;
	//1:speical | 2:weapon | 3:armor | 4:accessories
	private void UpdateItemShopList(int type) {
		Transform curRoot;
		ItemShopObject itemObj;
//		if(curType == type) {
//			return;
//		}
		curType = type;
		if(ItemShopManager.Instance) {
			curRoot = ItemShopManager.Instance.GetCurRoot();
			//clearn list//
			if(curRoot != null) {
				foreach(ItemShopObject obj in ItemShopManager.Instance.list) {
					obj.OnSelectDelegate -= SelectItemObj;
					obj.OnBuyDelegate -= BuyItem;
				}
				ItemShopManager.Instance.list.Clear();
				Destroy(curRoot.gameObject);
			}
			//create root//
			ItemShopManager.Instance.CreateRoot();
			//init obj;
			switch(type) {
			case 1:
				for(int i=0;i<PlayerDataManager.Instance.itemShopSpecialList.Count;i++){
					itemObj = ItemShopManager.Instance.AddItemObj(PlayerDataManager.Instance.itemShopSpecialList[i],true);
					itemObj.OnSelectDelegate += SelectItemObj;
					itemObj.OnBuyDelegate += BuyItem;
				}
				break;
			case 2:
				for(int i=0;i<PlayerDataManager.Instance.itemShopWeaponList.Count;i++){
					itemObj = ItemShopManager.Instance.AddItemObj(PlayerDataManager.Instance.itemShopWeaponList[i],false);
					itemObj.OnSelectDelegate += SelectItemObj;
					itemObj.OnBuyDelegate += BuyItem;
				}
				break;
			case 3:
				for(int i=0;i<PlayerDataManager.Instance.itemShopArmorList.Count;i++){
					itemObj = ItemShopManager.Instance.AddItemObj(PlayerDataManager.Instance.itemShopArmorList[i],false);
					itemObj.OnSelectDelegate += SelectItemObj;
					itemObj.OnBuyDelegate += BuyItem;
				}
				break;
			case 4:
				for(int i=0;i<PlayerDataManager.Instance.itemShopAccessoriesList.Count;i++){
					itemObj = ItemShopManager.Instance.AddItemObj(PlayerDataManager.Instance.itemShopAccessoriesList[i],false);
					itemObj.OnSelectDelegate += SelectItemObj;
					itemObj.OnBuyDelegate += BuyItem;
				}
				break;
			}
			//update
			if(curRoot != null) {
				curRoot.GetComponent<UIGrid>().Reposition();
//				GUILogManager.LogInfo("Update list.");
			}
		}
	}
	
	private void SelectItemObj(ItemShopObjData _data,ItemShopObject obj) {
		ItemShopManager.Instance.HideAllHighLight();
		obj.HideHighLight(false);
		HideTip();
		ShowTip(_data.localData);
		UpdateCurEquip(_data);
	}
	
	private void UpdateCurEquip(ItemShopObjData _data) {
		invCtrl.UpdatePlayerModeEquip();
		if(_data != null) {
			PlayerModel pm = invCtrl.GetEquipModel();
			if(pm != null) {
				int _type = 0;
				switch(_data.localData._TypeID) {
				case 1:
					_type = 0;
					break;
				case 2:
					_type = 1;
					break;
				case 3:
					_type = 2;
					break;
				case 4:
					_type = 3;
					break;
				case 5:
					_type = 4;
					break;
				case 6:
					_type = 8;
					break;
				case 7:
				case 8:
					_type = 7;
					pm.equipmentMan.DetachItem(EquipementManager.EEquipmentType.LeftHand_Weapon,PlayerDataManager.Instance.Gender);
					pm.equipmentMan.DetachItem(EquipementManager.EEquipmentType.RightHand_Weapon,PlayerDataManager.Instance.Gender);
					break;
				}
				
				if(_type == 1 ||_type == 4) {
					return;
				}
				
				SItemInfo sInfo = new SItemInfo();
				sInfo.ID = _data.serData.ID;
				sInfo.element = _data.serData.element;
				sInfo.enchant = _data.serData.enchant;
				sInfo.gem = _data.serData.gem;
				sInfo.perfrab = _data.serData.perfrab;
				sInfo.level = _data.serData.level;
				sInfo.slot = (uint)_type;
				
				Transform item = UnityEngine.Object.Instantiate(ItemPrefabs.Instance.GetItemPrefab(_data.localData._ItemID, 0, _data.localData._PrefabID)) as Transform;
				pm.equipmentMan.UpdateItemInfoBySlot((uint)_type, item, sInfo, true,PlayerDataManager.Instance.Gender);	
				pm.equipmentMan.UpdateEquipment(PlayerDataManager.Instance.Gender);
				pm.usingLatestConfig = true;
			}
		}
	}
	
	private void BuyItem(ItemShopObjData data,bool isSpecial) {
		SBuyitemInfo  sItemInfo  = new SBuyitemInfo();
		sItemInfo.ID 			 = data.localData._ItemID;
		sItemInfo.currencyType   = 1;
		sItemInfo.count 		 = 1;
		sItemInfo.perfrab 		 = data.localData._PrefabID;
		sItemInfo.level 		 = (uint)data.localData.info_Level;
		sItemInfo.enchant 		 = data.localData._EnchantID;
		sItemInfo.element 		 = data.localData._EleID;
		sItemInfo.gem 			 = data.localData._GemID;
		sItemInfo.UUID			 = data.serData.UUID;
		if(isSpecial){
			//rare item
			CS_Main.Instance.g_commModule.SendMessage(ProtocolGame_SendRequest.BuyItem(true,sItemInfo));
			PlayerDataManager.Instance.isUpdateRareShopItem = true;
		}else {
			//normal shop
			CS_Main.Instance.g_commModule.SendMessage(ProtocolGame_SendRequest.BuyItem(false,sItemInfo));
		}
	}
	
	[SerializeField]
	private InventoryCtrl invCtrl;
	
	private void HideTip() {
		TipManager.Instance.curTip.Hide(true);
		TipManager.Instance.equpTip.Hide(true);
	}
	
	private void ShowTip(ItemDropStruct data) {
		if (null == data){
            return;
        }
		TipManager.Instance.HideUseBtn(true);
		TipManager.Instance.HideEquipBtn(true);
		TipManager.Instance.HideSellBtn(true);
		invCtrl.ShowSeleTip(data,true);
		invCtrl.CheckEquipTip(data,false);
	}

	private int curTabIdx = 1;
	private void TabDelegate(int idx) {
		curTabIdx = idx;
		UpdateCurItemShopList();
		HideTip();
	}
	
	public void UpdateCurItemShopList() {
		ItemShopManager.Instance.UpdateTab(curTabIdx);
		UpdateItemShopList(curTabIdx);
	}
		
	private void ExitDelegate() {
		GUIManager.Instance.ChangeUIScreenState("IngameScreen");
		Player.Instance.ReactivePlayer();
        GameCamera.BackToPlayerCamera();
	}

	[SerializeField] ItemShopSuccessPanel m_buyitemsucpanel;
	public void BuyItemSuc(SBuyitemInfo _info)
	{
		ItemShopManager.Instance.BuyItemSuc(_info);
		UpdateCurItemShopList();
	}
	#endregion
	
}