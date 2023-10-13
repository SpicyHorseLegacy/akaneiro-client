using UnityEngine;
using System.Collections;

public class _UI_CS_ItemVendorRareItemEx : MonoBehaviour {

	public _UI_CS_ItemVendorItem m_ItemVendorInfo;
//	public UIButton m_SIconButton;
	public UIButton 			m_BgIconButton;
	public UIButton 			m_ItemIcon;
	public UIButton 			m_LevelBgIcon;
	public SpriteText    		m_ValText;
	private Rect  				m_rect;
	public int    				m_ListID;			//in specials menu ,it is isDel flag;
	public SpriteText   	 	m_OffText;
	public SpriteText    		m_TimeText;
	public SpriteText    		m_CountText;
	
	// Use this for initialization
	void Start () {
		m_BgIconButton.AddInputDelegate(IconDelegate);
		m_rect.width = 1;
		m_rect.height = 1;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void IconDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:
				if( 99 != m_ItemVendorInfo.m_type){
					_UI_CS_ItemVendorRare.Instance.sItemInfo 				= m_ItemVendorInfo.info;
					_UI_CS_ItemVendorRare.Instance.sBuyInfo.ID 				= m_ItemVendorInfo.info._ItemID;
					_UI_CS_ItemVendorRare.Instance.sBuyInfo.currencyType 	= 1;
					_UI_CS_ItemVendorRare.Instance.sBuyInfo.count 			= 1;
					_UI_CS_ItemVendorRare.Instance.sBuyInfo.perfrab 		= m_ItemVendorInfo.info._PrefabID;
					_UI_CS_ItemVendorRare.Instance.sBuyInfo.level 			= (uint)m_ItemVendorInfo.info.info_Level;
					_UI_CS_ItemVendorRare.Instance.sBuyInfo.enchant 		= m_ItemVendorInfo.info._EnchantID;
					_UI_CS_ItemVendorRare.Instance.sBuyInfo.element 		= m_ItemVendorInfo.info._EleID;
					_UI_CS_ItemVendorRare.Instance.sBuyInfo.gem 			= m_ItemVendorInfo.info._GemID;
					_UI_CS_ItemVendorRare.Instance.sShopInfo3 				= m_ItemVendorInfo;
				}
				break;
		}	
	}
}
