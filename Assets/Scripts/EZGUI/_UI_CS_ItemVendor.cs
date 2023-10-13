using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class _UI_CS_ItemVendor : MonoBehaviour {
	
	public static 			_UI_CS_ItemVendor Instance;
	public UIPanel 			ItemVendorPanel;
	public UIButton 		fareWellBtn;
	public UIButton 		fareWellIcon;
	public SpriteText 		fareWellText;
	public UIPanelTab 		ItemTab;
	public UIPanelTab 		KeysTab;
	public UIPanelTab 		SellTab;
	public UIPanel  		Msg2Panel;
	public UIButton 		SureBtn;
	public _UI_CS_ItemVendorRawItemCtrl specialsItemList;
	private int 			CurrentItemType = 0;
	public bool 			m_isReceiveItemList;
	public  List<SShopItemInfo> 		m_ShopItemList   = new List<SShopItemInfo>();
	public ItemDropStruct 	buyInfo  = new ItemDropStruct();
	public SShopItemInfo 	BuyOkoInfo = new SShopItemInfo();
	public UIButton 		sBuyIcon;
	public SpriteText 		Buy2NameText;
	public float 			whiteVal 	= 0;
	public float 			greenVal 	= 1;
	public float 			blueVal 	= 6;
	public float 			purpleVal 	= 11;
	public float 			brownVal 	= 15;
	public List<string> 	TipsList    = new List<string>();
	public SpriteText    	TipsText;
	public Transform		bagPosObj;
	public Transform		BuySound;
	public Transform		SellSound;
	
//	public UIButton  		npc;
	public UIButton  		smallBg;
	
	void Awake(){
		Instance = this;
	}
	
	public void AwakeItemVendor(){
		InitImage();
		if(2 == CurrentItemType){
//			BagPanel.Hide(false);
			MouseCtrl.Instance.SetMouseStats(MouseIconType.SELL);
//			StartCoroutine(SkipOneFrameForBag());	
			BagBring();
		}
		ItemVendorPanel.BringIn();
		MoneyBadgeInfo.Instance.Hide(false);
		_UI_CS_ScreenCtrl.Instance.SetNextScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_ITEM_VENDOR);
		 ChangeShopTips();
	}
	
	private void InitImage(){
//		npc.SetUVs(new Rect(0,0,1,1));
		//downloading image
//		TextureDownLoadingMan.Instance.DownLoadingTexture("Figure_use1",npc.transform);
		TextureDownLoadingMan.Instance.DownLoadingTexture("Dia_Box3",smallBg.transform);
	}
	
	void ReadShopTipsInfo(){
		string fileName = "ItemShopTip.Description";
		string _fileName = LocalizeManage.Instance.GetLangPath(fileName);
		TextAsset item = (TextAsset)Resources.Load(_fileName, typeof(TextAsset));
		string[] itemRowsList = item.text.Split('\n');
		for (int i = 3; i < itemRowsList.Length - 1; ++i){
			string pp = itemRowsList[i];
		   	string[] vals = pp.Split(new char[] { '	', '	' });	
			TipsList.Add(vals[0]);
		}
		LogManager.Log_Info("ItemShopTips Ok");
	}
	
	public void ChangeShopTips(){
		int tempIdx = Random.Range(0,TipsList.Count);
		if(TipsList.Count >0)
			TipsText.Text = TipsList[tempIdx];
	}
	
	public void InitItemVendor(){
		_UI_CS_ItemVendor_Keys.Instance.InitItemList();
	}
	
	// Use this for initialization
	void Start () {
		fareWellBtn.AddInputDelegate(FareWellBtnDelegate);
		ItemTab.AddInputDelegate(ItemTabDelegate);
		KeysTab.AddInputDelegate(KeysTabDelegate);
		SellTab.AddInputDelegate(SellTabDelegate);
		SureBtn.AddInputDelegate(SureDelegate);
		m_isReceiveItemList = false;	
		ReadShopTipsInfo();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void FareWellBtnDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
			case POINTER_INFO.INPUT_EVENT.PRESS:
					fareWellIcon.SetColor(_UI_Color.Instance.color1);
					fareWellText.SetColor(_UI_Color.Instance.color1);	
				break;
			case POINTER_INFO.INPUT_EVENT.MOVE:
					fareWellIcon.SetColor(_UI_Color.Instance.color1);
					fareWellText.SetColor(_UI_Color.Instance.color1);	
				break;
			case POINTER_INFO.INPUT_EVENT.RELEASE:
			case POINTER_INFO.INPUT_EVENT.MOVE_OFF:
			case POINTER_INFO.INPUT_EVENT.RELEASE_OFF:	
					fareWellIcon.SetColor(_UI_Color.Instance.color2);
					fareWellText.SetColor(_UI_Color.Instance.color4);
			break;
			
		   case POINTER_INFO.INPUT_EVENT.TAP:
//					MouseCtrl.Instance.iconType = MouseIconType.SWARD1;
					MouseCtrl.Instance.SetMouseStats(MouseIconType.SWARD1);
					_UI_CS_FightScreen.Instance.m_CS_Ingame_NormalPanel.BringIn();
					_UI_CS_BagCtrl.Instance.Hide(true);
					_UI_CS_PopupBoxCtrl.Instance.m_CS_popUpBox.Dismiss();
					Msg2Panel.Dismiss();
					ItemVendorPanel.Dismiss();
					_UI_CS_ScreenCtrl.Instance.SetNextScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_INGAME_NORMAL);
	                Player.Instance.ReactivePlayer();
                    GameCamera.BackToPlayerCamera();
					BGMInfo.Instance.isPlayUpGradeEffectSound = true;
					fareWellIcon.SetColor(_UI_Color.Instance.color2);
					fareWellText.SetColor(_UI_Color.Instance.color4);	
				break;
		   default:
				break;
		}	
	}
	
	void KeysTabDelegate(ref POINTER_INFO ptr)
	{
		switch(ptr.evt)
		{
		   case POINTER_INFO.INPUT_EVENT.TAP:
				CurrentItemType = 1;
				Msg2Panel.Dismiss();
				_UI_CS_BagCtrl.Instance.Hide(true);
//				MouseCtrl.Instance.iconType = MouseIconType.SWARD1;
				MouseCtrl.Instance.SetMouseStats(MouseIconType.SWARD1);
				break;
		   default:
				break;
		}	
	}
	
	void ItemTabDelegate(ref POINTER_INFO ptr)
	{
		switch(ptr.evt)
		{
		   case POINTER_INFO.INPUT_EVENT.TAP:
				CurrentItemType = 0;
				Msg2Panel.Dismiss();
				_UI_CS_BagCtrl.Instance.Hide(true);
//				MouseCtrl.Instance.iconType = MouseIconType.SWARD1;
				MouseCtrl.Instance.SetMouseStats(MouseIconType.SWARD1);
				break;
		   default:
				break;
		}	
	}
	
	void SellTabDelegate(ref POINTER_INFO ptr)
	{
		switch(ptr.evt)
		{
		   case POINTER_INFO.INPUT_EVENT.TAP:
				CurrentItemType = 2;
				Msg2Panel.Dismiss();
//				StartCoroutine(SkipOneFrameForBag());
				BagBring();
				MouseCtrl.Instance.SetMouseStats(MouseIconType.SELL);
				break;
		   default:
				break;
		}	
	}

	void PassDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:
				Msg2Panel.Dismiss();
				break;
		   default:
				break;
		}	
	}
		
	
	void SureDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:
				Msg2Panel.Dismiss();
				break;
		   default:
				break;
		}	
	}
	
	void ThanksDelegate(ref POINTER_INFO ptr)
	{
		switch(ptr.evt)
		{
		   case POINTER_INFO.INPUT_EVENT.TAP:
				Msg2Panel.Dismiss();
				break;
		   default:
				break;
		}	
	}

	public void PopUpBuyOkMenu(){
		SetColorForName(Buy2NameText,buyInfo);
		sBuyIcon.SetUVs(new Rect(0,0,1,1));
		ItemPrefabs.Instance.GetItemIcon(buyInfo._ItemID,buyInfo._TypeID,buyInfo._PrefabID,sBuyIcon);
		Msg2Panel.BringIn();
		StartCoroutine(DismissTips());
	}
	
	//sometime tips no dismiss,so,todo it.
	public IEnumerator DismissTips(){	
		yield return new WaitForSeconds(0.1f);
		_ItemTips.Instance.DismissItemTip();
	}

	// bag frame ctrl;
	public IEnumerator SkipOneFrameForBag(){	
		yield return new WaitForSeconds(0.3f);
		_UI_CS_BagCtrl.Instance.ob.transform.position = bagPosObj.transform.position;
	}
	
	public void BagBring() {
		_UI_CS_BagCtrl.Instance.ob.transform.position = bagPosObj.transform.position;
	}
	
#region Interface
	public void SetColorForName(SpriteText name,ItemDropStruct info){
		float itemVal = (info.info_gemVal + info.info_encVal + info.info_eleVal);
		string tName = "";
		if(itemVal < greenVal){
			name.SetColor(_UI_Color.Instance.color1);
		}else if( (greenVal - 0.01) < itemVal && itemVal  < _UI_CS_ItemVendor.Instance.blueVal){
			name.SetColor(_UI_Color.Instance.color15);
		}else if( (_UI_CS_ItemVendor.Instance.blueVal - 0.01) < itemVal && itemVal < _UI_CS_ItemVendor.Instance.purpleVal){
			name.SetColor(_UI_Color.Instance.color16);
		}else if( (_UI_CS_ItemVendor.Instance.purpleVal - 0.01) < itemVal && itemVal < _UI_CS_ItemVendor.Instance.brownVal){
			name.SetColor(_UI_Color.Instance.color17);
		}else if( (_UI_CS_ItemVendor.Instance.brownVal - 0.01) < itemVal){
			name.SetColor(_UI_Color.Instance.color18);
		}
		if(info._TypeID == 7 || info._TypeID == 8){
			  tName = info.info_EncName+ info.info_GemName + info.info_EleName+ info.info_TypeName;
		}else if(1 == info._TypeID|| 3 == info._TypeID||4 == info._TypeID||6 == info._TypeID){
			if(info._TypeID == 4){
			  tName = _ItemTips.Instance.GetCloakName(info);
			}else{
			  tName = info.info_EncName + info.info_GemName  + info._TypeName  + info._TypelastName;
			}	
		}else if(2 == info._TypeID|| 5 == info._TypeID){
			  tName = info.info_EncName + info.info_EleName + info.info_GemName  + info.info_TypeName;	
		}else{
			tName = info._PropsName;
		}
		name.Text = tName.Trim();
	}
	
	public string GetItemName(ItemDropStruct info) {
		string tName = "";
		if(info._TypeID == 7 || info._TypeID == 8){
			  tName = info.info_EncName+ info.info_GemName + info.info_EleName+ info.info_TypeName;
		}else if(1 == info._TypeID|| 3 == info._TypeID||4 == info._TypeID||6 == info._TypeID){
			if(info._TypeID == 4){
			  tName = _ItemTips.Instance.GetCloakName(info);
			}else{
			  tName = info.info_EncName + info.info_GemName  + info._TypeName  + info._TypelastName;
			}	
		}else if(2 == info._TypeID|| 5 == info._TypeID){
			  tName = info.info_EncName + info.info_EleName + info.info_GemName  + info.info_TypeName;	
		}else{
			tName = info._PropsName;
		}
		tName.Trim();
		return tName;
	}
	
	//如果第一次访问商店向服务器请求，否则读取上一次商店列表剩余物品//
	public void UpdateShopList() {
		_UI_CS_ItemVendorSpecials.Instance.ClearList();
		_UI_CS_ItemVendor_1hWeapon.Instance.ClearList();
		_UI_CS_ItemVendor_2hWeapon.Instance.ClearList();
		_UI_CS_ItemVendor_Accessory.Instance.ClearList();
		_UI_CS_ItemVendor_Chest.Instance.ClearList();
		_UI_CS_ItemVendor_Cloak.Instance.ClearList();
		_UI_CS_ItemVendor_Head.Instance.ClearList();
		_UI_CS_ItemVendor_Legs.Instance.ClearList();
		if(m_isReceiveItemList){
			for(int i =0;i<m_ShopItemList.Count;i++){
				_UI_CS_ItemVendorItem itemS = new _UI_CS_ItemVendorItem();
				ItemDropStruct tempItem = ItemDeployInfo.Instance.GetItemObject(m_ShopItemList[i].ID,m_ShopItemList[i].perfrab,m_ShopItemList[i].gem,m_ShopItemList[i].enchant,m_ShopItemList[i].element,(int)m_ShopItemList[i].level);
				if(null != tempItem){
					itemS.m_shopItem = m_ShopItemList[i];
					itemS.m_type = tempItem._TypeID;
					itemS.m_iconID = m_ShopItemList[i].perfrab;
					itemS.m_ID = tempItem._ItemID;
					itemS.m_name = tempItem.info_EleName + " " + tempItem.info_GemName + " " + tempItem.info_EleName + " " + tempItem.info_TypeName;
					itemS.m_Val =  m_ShopItemList[i].price;
					itemS.m_count = m_ShopItemList[i].leftBuyCount;
					itemS.moneyType = m_ShopItemList[i].moneyType;
					itemS.info = tempItem;
					if(0 != m_ShopItemList[i].isDiscount){
						_UI_CS_ItemVendorSpecials.Instance.SpecialsItemList.Add(itemS);
					}else{
						switch(itemS.m_type){
							case 1:
									_UI_CS_ItemVendor_Head.Instance.AddElement(itemS);
									break;
							case 2:
							case 5:
									_UI_CS_ItemVendor_Accessory.Instance.AddElement(itemS);
								break;
							case 3:
									_UI_CS_ItemVendor_Chest.Instance.AddElement(itemS);
									break;
							case 4:
									_UI_CS_ItemVendor_Cloak.Instance.AddElement(itemS);
								break;
							case 6:
									_UI_CS_ItemVendor_Legs.Instance.AddElement(itemS);
								break;
							case 7:
									_UI_CS_ItemVendor_1hWeapon.Instance.AddElement(itemS);
									break;
							case 8:
									_UI_CS_ItemVendor_2hWeapon.Instance.AddElement(itemS);
								break;
						}
					}
				}
	        }
			_UI_CS_ItemVendorSpecials.Instance.InitItem();	
			_UI_CS_ItemVendor_1hWeapon.Instance.InitItemList();
			_UI_CS_ItemVendor_2hWeapon.Instance.InitItemList();
			_UI_CS_ItemVendor_Accessory.Instance.InitItemList();
			_UI_CS_ItemVendor_Chest.Instance.InitItemList();
			_UI_CS_ItemVendor_Cloak.Instance.InitItemList();
			_UI_CS_ItemVendor_Head.Instance.InitItemList();
			_UI_CS_ItemVendor_Legs.Instance.InitItemList();
			_UI_CS_FightScreen.Instance.m_CS_Ingame_NormalPanel.Dismiss();
			AwakeItemVendor();
		}else{
			CS_Main.Instance.g_commModule.SendMessage(
	   			ProtocolGame_SendRequest.RequestPlayerShopInfo(false)
			);
		}
	}
#endregion
}
