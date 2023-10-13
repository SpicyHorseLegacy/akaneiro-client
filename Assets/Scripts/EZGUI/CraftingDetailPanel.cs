using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CraftingDetailPanel : MonoBehaviour 
{
	static public CraftingDetailPanel Instance = null;
	public CraftingAnimPanel animPanel;
	public UIPanel craftingControlPanel;
    public SpriteText craftText;
	[SerializeField] CraftingAttributeBtn attrItem;
	[SerializeField] CraftingStarsControler starController = null;
	[SerializeField] UIButton[] buyMatBtns;
	[SerializeField] UIButton[] buyMatBtnFrames;
	[SerializeField] UIButton[] matIcons;
	[SerializeField] UIButton[] matBgs;
	[SerializeField] UIButton[] matPriceBgs;
	[SerializeField] UIButton[] csMoneyIcons;
	[SerializeField] SpriteText[] matCountText;
	[SerializeField] SpriteText[] matCostText;
	[SerializeField] SpriteText[] buyBtnText;
	[SerializeField] UIButton kamaUpgrade;
	[SerializeField] UIButton csUpgrade;
	[SerializeField] SpriteText kamaCraftingPriceText;
	[SerializeField] SpriteText csCraftingPriceText;
	[SerializeField] SpriteText kamaChanceText;
	[SerializeField] SpriteText csChanceText;
	
	int[] matIDs = new int[]{0,0,0,0};
	int[] matNeedCounts = new int[]{0,0,0,0};
	int[] matCounts = new int[]{0,0,0,0};
	int[] matPrice = new int[]{0,0,0,0};
	
	UpgradeRecipe curRecipe;
	int RecipeType = 0;
	
	mapCraftMaterial matsMap = new mapCraftMaterial();
	
	void Awake()
	{
		Instance = this;
	}
	
	void Start()
	{
		kamaUpgrade.AddValueChangedDelegate(OnKamaUpgradeClicked);
		csUpgrade.AddValueChangedDelegate(OnCsUpgradeClicked);
		
		foreach(UIButton btn in buyMatBtns)
		{
			btn.AddValueChangedDelegate(OnBuyMatBtnClicked);
		}
	}
	
	void OnKamaUpgradeClicked(IUIObject obj)
	{
		int kamaMoney = int.Parse( MoneyBadgeInfo.Instance.skText.Text );
		if(kamaMoney < curRecipe.ksPrize)
		{
			_UI_CS_PopupBoxCtrl.PopUpError(new EServerErrorType(EServerErrorType.eItemError_BuyNotEnoughMoneny));
		}
		else
		{
			animPanel.attrLevel = attrItem.Level;
			CS_Main.Instance.g_commModule.SendMessage(ProtocolGame_SendRequest.CraftingItem(2, (uint)CraftingWeaponPanel.Instance.craftingItem.m_Slot, new ECraftType( curRecipe.type ), new EMoneyType( EMoneyType.eMoneyType_SK ), curRecipe.attrID - 1, matsMap, false));
		}
	}
	
	void OnCsUpgradeClicked(IUIObject obj)
	{
		int totalMatCost = 0;
		foreach(var costTx in matCostText)
		{
			int matCost = 0;
			int.TryParse(costTx.Text, out matCost);
			totalMatCost +=  matCost;
		}
		
		int csMoney = int.Parse(MoneyBadgeInfo.Instance.fkText.Text);
		if(csMoney < totalMatCost + curRecipe.crPrize)
		{
			_UI_CS_PopupBoxCtrl.PopUpError(new EServerErrorType(EServerErrorType.eItemError_BuyNotEnoughMoneny));
			return;
		}
		
		animPanel.attrLevel = attrItem.Level + 1;
		CS_Main.Instance.g_commModule.SendMessage(ProtocolGame_SendRequest.CraftingItem(2, (uint)CraftingWeaponPanel.Instance.craftingItem.m_Slot, new ECraftType( curRecipe.type ), new EMoneyType( EMoneyType.eMoneyType_FK ), curRecipe.attrID - 1, matsMap, true));
	
	}
	
	bool isMatMeetTheRequirment()
	{
		for(int i = 0; i < 4; ++i)
		{
			if(matNeedCounts[i] > matCounts[i])
				return false;
		}
		
		return true;
	}
	
	void OnBuyMatBtnClicked(IUIObject obj)
	{
		for(int i = 0; i < buyMatBtns.Length; ++i)
		{
			if(buyMatBtns[i] == obj)
			{
				CS_Main.Instance.g_commModule.SendMessage(ProtocolGame_SendRequest.BuyConsumeItem(1, matIDs[i], matNeedCounts[i] - matCounts[i]));
			}
		}
	}
	
	public void Setup(int attrID, int itemType, UpgradeRecipes.UpgradeType upgradeType, int recipeType)
	{
		ClearMatInfos();
		
		curRecipe = UpgradeRecipes.Instance.GetRecipe(attrID + 1, recipeType);
		RecipeType = recipeType;
		
		if(curRecipe == null)
			attrItem.Hide();
		else
			attrItem.Setup(attrID + 1, itemType, upgradeType);
		
		craftingControlPanel.BringIn();
		animPanel.GetComponent<UIPanel>().Dismiss();
		
		UpdateControlPanel();
	}
	
	public void ReSetup()
	{
		Setup(attrItem.AttrID - 1, attrItem.ItemType, attrItem.UpgradeType, RecipeType);
	}
	
	public void Upgraded()
	{
		Setup(attrItem.AttrID, attrItem.ItemType, attrItem.UpgradeType, RecipeType);
	}
	
	public void UpdateControlPanel()
	{
		starController.SetupLevel(attrItem.Level);
		craftText.Hide(false);
		
		if(curRecipe == null)
		{
			craftText.Text = "This item can not be further upgraded.";
			craftingControlPanel.Dismiss();
			starController.SetupLevel(0);
			starController.HideSlots(true);
		}
		else
		{
			starController.HideSlots(false);
			
			craftText.Text = "The following materials are needed to upgrade this attribute.";
			
			craftingControlPanel.BringIn();
			
			UpdateMatBtns();
			
			kamaCraftingPriceText.Text = curRecipe.ksPrize.ToString();
			int csCraftPrice = curRecipe.crPrize;
			for(int i = 0; i < matIDs.Length; ++i)
			{
				if(matNeedCounts[i] > matCounts[i])
					csCraftPrice += matPrice[i] * (matNeedCounts[i] - matCounts[i]);
			}
			csCraftingPriceText.Text = csCraftPrice.ToString();
			kamaChanceText.Text = curRecipe.ksChance + "% Success";
			csChanceText.Text = curRecipe.crChance + "% Success";
		}
	}
	
	private void ClearMatInfos()
	{
		for(int i = 0; i < 4; ++i)
		{
			matIDs[i] = 0;
			matNeedCounts[i] = 0;
			matCounts[i] = 0;
			matPrice[i] = 0;
		}
	}
	
	private void UpdateMatBtns()
	{
		matsMap.Clear();
	
		if(curRecipe.mat1 != 0)
			SetupMatBtn(0, curRecipe.mat1, curRecipe.mat1Count);
		else
			HideMatBtn(0, true);
		
		if(curRecipe.mat2 != 0)
			SetupMatBtn(1, curRecipe.mat2, curRecipe.mat2Count);
		else
			HideMatBtn(1, true);
		
		if(curRecipe.mat3 != 0)
			SetupMatBtn(2, curRecipe.mat3, curRecipe.mat3Count);
		else
			HideMatBtn(2, true);
		
		if(curRecipe.mat4 != 0)
			SetupMatBtn(3, curRecipe.mat4, curRecipe.mat4Count);
		else
			HideMatBtn(3, true);
		
		bool canKamaCraft = true;
		for(int i = 0; i < 4; ++i)
		{
			if(matNeedCounts[i] > matCounts[i])
				canKamaCraft = false;
		}
		
		kamaUpgrade.controlIsEnabled = canKamaCraft;
	}
	
	private void SetupMatBtn(int matIndex, int matID, int matNeedCount)
	{	
		HideMatBtn(matIndex, false);
		
		matIDs[matIndex] = matID;
		matNeedCounts[matIndex] = matNeedCount;
	
		int itemCount = GetItemCountInInv(matID, matNeedCount);
		
		matCounts[matIndex] = itemCount;
		
		ItemDropStruct item = ItemDeployInfo.Instance.GetItemObject(matID,1,0,0,0,1);
		matPrice[matIndex] = item._SaleVal;
		matIcons[matIndex].GetComponent<Materials>().info = item;
		ItemPrefabs.Instance.GetItemIcon(item._ItemID, item._TypeID, item._PrefabID, matIcons[matIndex]);
		matCountText[matIndex].Text = matNeedCount + "/" + itemCount;
		int itemLack = matNeedCount - itemCount;
		if(itemLack <= 0)
		{
			matCostText[matIndex].Text = "--";
			buyBtnText[matIndex].Text = "OK";
			buyMatBtns[matIndex].controlIsEnabled = false;
			matCountText[matIndex].Color = Color.white;
		}
		else
		{
			matCostText[matIndex].Text = (itemLack * matPrice[matIndex]).ToString();
			buyBtnText[matIndex].Text = "GET x" + itemLack;
			buyMatBtns[matIndex].controlIsEnabled = true;
			matCountText[matIndex].Color = Color.red;
		}
	}
	
	private void HideMatBtn(int matIndex, bool hide)
	{
		matCountText[matIndex].Hide(hide);
		matCostText[matIndex].Hide(hide);
		buyBtnText[matIndex].Hide(hide);
		buyMatBtns[matIndex].Hide(hide);
		buyMatBtnFrames[matIndex].Hide(hide);
		csMoneyIcons[matIndex].Hide(hide);
		matIcons[matIndex].Hide(hide);
		matPriceBgs[matIndex].Hide(hide);
		matBgs[matIndex].Hide(hide);
		
		matIDs[matIndex] = 0;
		matNeedCounts[matIndex] = 0;
		matCounts[matIndex] = 0;
	}
	
	private int GetItemCountInInv(int itemID, int needCount)
	{
		int itemCount = 0;
		int usedCount = 0;
		foreach(_UI_CS_InventoryItem invItem in Inventory.Instance.bagItemArray)
		{
			if(invItem.m_IsEmpty || invItem.ItemStruct == null) {
				continue;
			}
			
			ItemDropStruct itemStruct = invItem.ItemStruct;
			
			if( itemStruct._ItemID == itemID )
			{
				if(invItem.m_ItemInfo == null)
					continue;
				
				itemCount += (int)invItem.m_ItemInfo.count;
				
				if(usedCount > needCount)
					continue;
					
				// Add this slot to the mat map
				if(!matsMap.ContainsKey(itemID))
					matsMap.Add(itemID, new vectorItemSlot());
				
				var itemSlots = matsMap[itemID];
				SItemSlot slot = new SItemSlot();
				slot.slot = (uint)invItem.m_Slot;
				if(usedCount + invItem.m_ItemInfo.count > needCount)
				{
					long delta = usedCount + invItem.m_ItemInfo.count - needCount;
					slot.count = (uint)needCount;
					usedCount = needCount;
				}
				else
				{		
					slot.count = (uint)invItem.m_ItemInfo.count;
					usedCount += (int)(usedCount + invItem.m_ItemInfo.count);
				}
				itemSlots.Add(slot);
			}
		}
		
		return itemCount;
	}
}
