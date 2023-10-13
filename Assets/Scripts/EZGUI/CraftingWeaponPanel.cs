using UnityEngine;
using System.Collections;

public class CraftingWeaponPanel : MonoBehaviour 
{
	public static CraftingWeaponPanel Instance = null;
	
	void Awake() 
	{
		Instance = this;
	}
	
	void Start()
	{
		foreach(CraftingAttributeBtn btn in attributeBtns)
		{
			btn.Hide();
		}
		
		swapItemBtn.AddInputDelegate(swapWeaponDelegate);
		foreach(CraftingAttributeBtn btn in attributeBtns)
		{
			btn.GetComponent<UIRadioBtn>().AddValueChangedDelegate(AttributeSelectDelegate);
		}
		
		txCantUpgradeItemInfo.Hide(true);
		txSelectItemInfo.Hide(true);
	}
	
	void AttributeSelectDelegate(IUIObject obj)
	{
		UIRadioBtn btn  = obj as UIRadioBtn;
		if(btn == null || !btn.Value)
			return;
		
		var craftingBtn = btn.GetComponent<CraftingAttributeBtn>();
		CraftingPanel.Instance.DissmissInv();
		craftingControlPanel.Setup(craftingBtn.AttrID, craftingBtn.ItemType, craftingBtn.UpgradeType, craftingBtn.recipeTypeID);
	}
	
	void swapWeaponDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:
				StartCoroutine( CraftingPanel.Instance.BringInInv() );
				break;
		}	
	}
	
	int getUpgradeType(int itemType, UpgradeRecipes.UpgradeType upgradeType)
	{
		if(itemType == 7 || itemType == 8)
		{
			if(upgradeType == UpgradeRecipes.UpgradeType.ELEM)
				return 1;
			else if(upgradeType == UpgradeRecipes.UpgradeType.ENCHANT)
				return 2;
			else if(upgradeType == UpgradeRecipes.UpgradeType.GEM)
				return 7;
			else
				return -1;
		}
		else if(itemType == 2 || itemType == 5)
		{
			if(upgradeType == UpgradeRecipes.UpgradeType.ELEM)
				return 5;
			else if(upgradeType == UpgradeRecipes.UpgradeType.ENCHANT)
				return 6;
			else if(upgradeType == UpgradeRecipes.UpgradeType.GEM)
				return 7;
			else
				return -1;
		}
		else
		{
			if(upgradeType == UpgradeRecipes.UpgradeType.ELEM)
				return 3;
			else if(upgradeType == UpgradeRecipes.UpgradeType.ENCHANT)
				return 4;
			else if(upgradeType == UpgradeRecipes.UpgradeType.GEM)
				return 7;
			else
				return -1;
		}
	}
	
#region Local
	public _UI_CS_InventoryItem craftingItem;
	[SerializeField]
	private SpriteText txItemName;
	[SerializeField]
	private SpriteText txItemLevelNum;
	[SerializeField]
	private SpriteText txWeaponTypeInfo;
	[SerializeField]
	private SpriteText txSelectItemInfo;
	[SerializeField]
	private SpriteText txCantUpgradeItemInfo;
	[SerializeField]
	private SpriteText txDragItemInfo;
	[SerializeField]
	private CraftingAttributeBtn[] attributeBtns;
	[SerializeField]
	private UIButton swapItemBtn;
	[SerializeField]
	private CraftingDetailPanel craftingControlPanel;
	
	private void ClearCraftingItemIcon() {
		craftingItem.ClearItemInfo();
	}
	
#endregion
	
#region Interface
	private int sourceItemSlot = 0;
	public void SetSourceItemSlot(int slot) {
//		LogManager.Log_Error("SetSourceItemSlot: "+slot);
		sourceItemSlot = slot;
	}
	
	public int GetSourceItemSlot() {
//		LogManager.Log_Error("GetSourceItemSlot: "+sourceItemSlot);
		return sourceItemSlot;
	}
	
	public void ClearBtnState()
	{
		foreach(CraftingAttributeBtn btn in attributeBtns)
		{
			btn.GetComponent<UIRadioBtn>().Value = false;
		}
	}
	
	public void UpdateItemInfo() {
		ItemDropStruct dropedItem = craftingItem.ItemStruct;
		if(dropedItem == null)
			return;
		
		_UI_CS_ItemVendor.Instance.SetColorForName(txItemName, dropedItem);
		txItemLevelNum.Text = dropedItem.info_Level.ToString();
		txWeaponTypeInfo.Text = dropedItem._TypeName;
		
		bool noAttr = dropedItem._EleID == 0 && dropedItem._EnchantID == 0 && dropedItem._GemID == 0;
		txSelectItemInfo.Hide(noAttr);
		txCantUpgradeItemInfo.Hide(!noAttr);
		txDragItemInfo.Hide(true);
		
		int attrBtnIndex = 0;
		if(dropedItem._EleID != 0)
		{
			CraftingAttributeBtn attrBtn = attributeBtns[attrBtnIndex++];
			attrBtn.Setup(dropedItem._EleID, dropedItem._TypeID, UpgradeRecipes.UpgradeType.ELEM);
		}
		
		if(dropedItem._EnchantID != 0)
		{
			CraftingAttributeBtn attrBtn = attributeBtns[attrBtnIndex++];
			attrBtn.Setup(dropedItem._EnchantID, dropedItem._TypeID, UpgradeRecipes.UpgradeType.ENCHANT);
		}
		
		if(dropedItem._GemID != 0)
		{
			CraftingAttributeBtn attrBtn = attributeBtns[attrBtnIndex++];
			attrBtn.Setup(dropedItem._GemID, dropedItem._TypeID, UpgradeRecipes.UpgradeType.GEM);
		}
		
		// Hide rest of attribut btns
		for(; attrBtnIndex < attributeBtns.Length; ++attrBtnIndex)
		{
			attributeBtns[attrBtnIndex].Hide();
		}
	}
	
	public void Init()
	{
		txCantUpgradeItemInfo.Hide(true);
		txSelectItemInfo.Hide(true);
		txDragItemInfo.Hide(false);
		
		craftingItem.ClearItemInfo();
		txItemLevelNum.Text = "";
		txItemName.Text = "";
		txWeaponTypeInfo.Text = "";
		
		foreach(CraftingAttributeBtn btn in attributeBtns)
			btn.Hide();
	}
#endregion
	
}
