using UnityEngine;
using System.Collections;

public class _UI_CS_ItemVendorItemEx : MonoBehaviour {

	public _UI_CS_ItemVendorItem m_ItemVendorInfo;
	public UIButton m_BgIconButton;
	public UIButton m_ItemIcon;
	public UIButton m_LevelBgIcon;
	public SpriteText    m_ValText;
	private Rect  m_rect;
	public int    m_ListID;			//in specials menu ,it is isDel flag;
	public SpriteText    m_OffText;
	public ItemPosOffestType PosOffest;
	private Vector3 mousePos;
	private bool isShowItemTips = true;
	public bool isRareShopItem = false;
	public SpriteText    count;
	public UIButton		 countBg;
	
	public UIButton		 piceBgK;
	public UIButton		 piceBgC;
	// Use this for initialization
	void Start () {
		m_BgIconButton.AddInputDelegate(IconDelegate);
		m_rect.width = 1;
		m_rect.height = 1;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void IconDelegate(ref POINTER_INFO ptr)
	{
		switch(ptr.evt)
		{
		   case POINTER_INFO.INPUT_EVENT.PRESS:	
				{
					isShowItemTips = false;
				}
				break;
	       case POINTER_INFO.INPUT_EVENT.DRAG:
				{
					_ItemTips.Instance.DismissItemTip();
					ItemEquipTips.Instance.DismissCompareTips();
				}
				break;
		   case POINTER_INFO.INPUT_EVENT.MOVE:	
		   case POINTER_INFO.INPUT_EVENT.NO_CHANGE:
				{
					if(!isShowItemTips){
						return;
					}	
					mousePos = UIManager.instance.uiCameras[0].camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y,UIManager.instance.uiCameras[0].camera.nearClipPlane + 1));
					_ItemTips.Instance.UpdateToolsTipInfo(m_ItemVendorInfo.info,_ItemTips.Instance.GetItemValue(ItemValueType.SALE,m_ItemVendorInfo.info.info_Level,m_ItemVendorInfo.info.info_eleVal,m_ItemVendorInfo.info.info_encVal,m_ItemVendorInfo.info._GemEffectVal,m_ItemVendorInfo.info._ItemVal)
												,ItemRightClickType.BUY,PosOffest,
						new Vector3(mousePos.x,mousePos.y,m_BgIconButton.transform.position.z),m_ItemIcon.width,m_ItemIcon.height);
					ItemEquipTips.Instance.ShowCompareTips(m_ItemVendorInfo.info,false);
					RcBuyLogic();
				}
				break;
		   case POINTER_INFO.INPUT_EVENT.TAP:
					isShowItemTips = true;
				break;
		   case POINTER_INFO.INPUT_EVENT.MOVE_OFF:
		   case POINTER_INFO.INPUT_EVENT.RELEASE:
		   case POINTER_INFO.INPUT_EVENT.RELEASE_OFF:
				{
					isShowItemTips = true;
					_ItemTips.Instance.DismissItemTip();
				}
				break;
		   default:
				break;
		}	
	}
	
	
	public void RcBuyLogic(){
		if(Input.GetButtonDown("Fire2")){
			//todo: 2013 01 13 add rare shop right logic;
			SBuyitemInfo  sItemInfo  = new SBuyitemInfo();
			sItemInfo.ID 			 = m_ItemVendorInfo.info._ItemID;
			sItemInfo.currencyType  = 1;
			sItemInfo.count 		 = 1;
			sItemInfo.perfrab 		 = m_ItemVendorInfo.info._PrefabID;
			sItemInfo.level 		 = (uint)m_ItemVendorInfo.info.info_Level;
			sItemInfo.enchant 		 = m_ItemVendorInfo.info._EnchantID;
			sItemInfo.element 		 = m_ItemVendorInfo.info._EleID;
			sItemInfo.gem 			 = m_ItemVendorInfo.info._GemID;
			sItemInfo.UUID			 = m_ItemVendorInfo.uuid;
			LogManager.Log_Warn("--!-- sItemInfo: " + sItemInfo.ID);
			if(isRareShopItem){
				//rare item right logic
				_UI_CS_ItemVendorRare.Instance.buyInfo	 = m_ItemVendorInfo.info;
				_UI_CS_ItemVendorRare.Instance.buyOkInfo = m_ItemVendorInfo.m_shopItem;
				CS_Main.Instance.g_commModule.SendMessage(
					   		ProtocolGame_SendRequest.BuyItem(true,sItemInfo));
				return;
			}
			// normal shop rigght logic;	
			_UI_CS_ItemVendor.Instance.buyInfo 	 	= m_ItemVendorInfo.info;
			_UI_CS_ItemVendor.Instance.BuyOkoInfo 	= m_ItemVendorInfo.m_shopItem;
			CS_Main.Instance.g_commModule.SendMessage(
					   		ProtocolGame_SendRequest.BuyItem(false,sItemInfo));
		}
	}
}
