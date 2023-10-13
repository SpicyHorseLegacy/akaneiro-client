using UnityEngine;
using System.Collections;

public class _UI_CS_ItemConsumableEx : MonoBehaviour {
	
	public ItemDropStruct 	ItemInfo;
	public UIButton 		BgButton;
	public UIButton 		ItemIcon;
	public UIButton 		SpiceBg;
	public UIButton 		CpiceBg;
	public SpriteText    	ValText;
	private Rect  			rect;
	public int 				ListID;
	
	// Use this for initialization
	void Start () {
		BgButton.AddInputDelegate(BGDelegate);
		rect.x 		= 0;
		rect.y 		= 0;
		rect.width 	= 1;
		rect.height = 1;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void BGDelegate(ref POINTER_INFO ptr)
	{
		switch(ptr.evt)
		{
		   case POINTER_INFO.INPUT_EVENT.TAP:
				_UI_CS_Consumable.Instance.CurrentBuyItemInfo = ItemInfo;
				_UI_CS_Consumable.Instance.ThanksPanel.Dismiss();
				_UI_CS_Consumable.Instance.CostPanel.BringIn();
				ItemPrefabs.Instance.GetItemIcon(ItemInfo._ItemID,ItemInfo._TypeID,ItemInfo._PrefabID,_UI_CS_Consumable.Instance.CostIcon);
				_UI_CS_Consumable.Instance.CostNameText.Text = ItemInfo._PropsName;
				_UI_CS_Consumable.Instance.CostDesText.Text = ItemInfo._PropsDes2;
				_UI_CS_Consumable.Instance.CostBuyNumText.Text = "1";
				_UI_CS_Consumable.Instance.CostSale1Text.Text = ItemInfo._SaleVal.ToString();
//				_UI_CS_Consumable.Instance.CostSale2Text.Text = ItemInfo._SaleVal.ToString();
				_UI_CS_Consumable.Instance.itemPice = ItemInfo._SaleVal;
				ItemPrefabs.Instance.GetItemIcon(ItemInfo._ItemID,ItemInfo._TypeID,ItemInfo._PrefabID,_UI_CS_Consumable.Instance.ThanksIcon);
				_UI_CS_Consumable.Instance.ThanksNameText.Text = ItemInfo._PropsName;
				if(ItemInfo._isUseRealMoney == 1){
					_UI_CS_Consumable.Instance.sPiceBg.gameObject.layer = LayerMask.NameToLayer("Default");
					_UI_CS_Consumable.Instance.cPiceBg.gameObject.layer = LayerMask.NameToLayer("EZGUI_CanTouch");
				}else{
					_UI_CS_Consumable.Instance.sPiceBg.gameObject.layer = LayerMask.NameToLayer("EZGUI_CanTouch");
					_UI_CS_Consumable.Instance.cPiceBg.gameObject.layer = LayerMask.NameToLayer("Default");
				}
				break;
		   default:
				break;
		}	
	}
}
