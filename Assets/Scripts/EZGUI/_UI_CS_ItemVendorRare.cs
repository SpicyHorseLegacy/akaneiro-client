using UnityEngine;
using System.Collections;

public class _UI_CS_ItemVendorRare : MonoBehaviour {
	
	public static _UI_CS_ItemVendorRare Instance;
	public UIPanel 						ItemVendorRarePanel;
	public UIButton 					fareWellBtn;
	public UIButton 					fareWellIcon;
	public SpriteText 					fareWellText;	
	public ItemDropStruct sItemInfo 		 = new ItemDropStruct();
	public SBuyitemInfo   sBuyInfo   		 = new SBuyitemInfo();
	public _UI_CS_ItemVendorItem  sShopInfo3 = new _UI_CS_ItemVendorItem();
	
	public UIPanel  		thanksPanel;
	public UIButton 		thanksBtn;
	public UIButton 		thanksIcon;
	public SpriteText 		thanksNameText;
	public SShopItemInfo 	buyOkInfo = new SShopItemInfo();
	public ItemDropStruct 	buyInfo   = new ItemDropStruct();
//	public UIButton  npc;
	public UIButton  smallBg;
	
	void Awake(){
		Instance = this;
	}

	// Use this for initialization
	void Start () {
		fareWellBtn.AddInputDelegate(FareWellBtnDelegate);
		thanksBtn.AddInputDelegate(ThanksBtnDelegate);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void AwakeItemVendor(){
		InitImage();
		ItemVendorRarePanel.BringIn();
		_UI_CS_FightScreen.Instance.m_CS_Ingame_NormalPanel.Dismiss();
		_UI_CS_ScreenCtrl.Instance.SetNextScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_ITEM_VENDOR_RARE);
		MoneyBadgeInfo.Instance.Hide(false);
	}
	
	private void InitImage(){
		//downloading image
//		TextureDownLoadingMan.Instance.DownLoadingTexture("Figure_use4",npc.transform);
		TextureDownLoadingMan.Instance.DownLoadingTexture("Dia_Box3",smallBg.transform);
	}
	
	public void InitItemVendor(){

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
				BGMInfo.Instance.isPlayUpGradeEffectSound = true;
				_UI_CS_FightScreen.Instance.m_CS_Ingame_NormalPanel.BringIn();
				ItemVendorRarePanel.Dismiss();
				_UI_CS_ScreenCtrl.Instance.SetNextScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_INGAME_NORMAL);
                Player.Instance.ReactivePlayer();
                GameCamera.BackToPlayerCamera();
				fareWellIcon.SetColor(_UI_Color.Instance.color2);
				fareWellText.SetColor(_UI_Color.Instance.color4);	
				break;
		}	
	}
	
	void ThanksBtnDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:
				thanksPanel.Dismiss();
				break;
		}	
	}
	
	public void PopUpBuyOkMenu(){
		_UI_CS_ItemVendor.Instance.SetColorForName(thanksNameText,buyInfo);
		thanksIcon.SetUVs(new Rect(0,0,1,1));
		ItemPrefabs.Instance.GetItemIcon(buyInfo._ItemID,buyInfo._TypeID,buyInfo._PrefabID,thanksIcon);
		thanksPanel.BringIn();
	}
	
}
