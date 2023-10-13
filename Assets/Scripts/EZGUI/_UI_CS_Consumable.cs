using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class _UI_CS_Consumable : MonoBehaviour {
	
	public static _UI_CS_Consumable  Instance;
	
	public UIPanel 	ConsumablePanel;
	
	public UIButton 	BackBtn;
	public UIButton 	BackIconBtn;
	public SpriteText	BackTextBtn;
	
	public  List<int> 		SaleItemList   = new List<int>();
	
	private bool 		isInit = false;
	
	public UIPanel 		CostPanel;
	public UIButton 	CostIcon;
	public SpriteText	CostNameText;
	public SpriteText	CostDesText;
	public SpriteText	CostBuyNumText;
	public int			CostBuyNum = 1;
	public SpriteText	CostSale1Text;
//	public SpriteText	CostSale2Text;
	public UIButton 	CostAddIcon;
	public UIButton 	CostSubIcon;
	public UIButton 	sPiceBg;
	public UIButton 	cPiceBg;
	public UIButton 	BuyIcon;
	public UIPanel 		ThanksPanel;
	public UIButton 	ThanksIcon;
	public SpriteText	ThanksNameText;
//	public UIButton 	ThanksCancel;
	public UIButton 	ThanksBtn;
	public int			itemPice = 0;
	
	public UIPanelTab 	foodTab;
	public UIPanelTab 	scrollTab;
	
	//0: food 1: fuwen 2:box 3:repair 4:home
	public _UI_CS_ConItemList	[]	ItemListArray;
	
	public  List<string> 		TipsList   = new List<string>();
	public SpriteText    		TipsText;
	
	public ItemDropStruct 	CurrentBuyItemInfo;
	
//	public UIButton  				npc;
	public UIButton  				smallBg;
	
	void Awake(){
		Instance = this;
	}
	
	// Use this for initialization
	void Start () {
		BackBtn.AddInputDelegate(BackBtnDelegate);
		CostAddIcon.AddInputDelegate(CostAddDelegate);
		CostSubIcon.AddInputDelegate(CostSubDelegate);
		BuyIcon.AddInputDelegate(CostBuyDelegate);
//		ThanksCancel.AddInputDelegate(ThanksCancelDelegate);
		ThanksBtn.AddInputDelegate(ThanksBtnDelegate);
		foodTab.AddInputDelegate(FoodTabDelegate);
		scrollTab.AddInputDelegate(ScrollTabDelegate);
		ReadShopTipsInfo();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void IntiConsumable(){
		ReadSaleItemFile(); 
		InitItemList();
	}
	
	void ReadShopTipsInfo(){
		string fileName = "ConsumableShopTip.Description";
		string _fileName = LocalizeManage.Instance.GetLangPath(fileName);
		TextAsset item = (TextAsset)Resources.Load(_fileName, typeof(TextAsset));
		string[] itemRowsList = item.text.Split('\n');
		for (int i = 3; i < itemRowsList.Length - 1; ++i){
			string pp = itemRowsList[i];
		   	string[] vals = pp.Split(new char[] { '	', '	' });	
			TipsList.Add(vals[0]);
		}
		LogManager.Log_Info("ConsumableShopTips Ok");
	}
	
	public void ChangeShopTips(){
		int tempIdx = Random.Range(0,TipsList.Count);
		if(TipsList.Count >0)
			TipsText.Text = TipsList[tempIdx];
	}
	
	void BackBtnDelegate(ref POINTER_INFO ptr)
	{
		switch(ptr.evt)
		{
			case POINTER_INFO.INPUT_EVENT.PRESS:
					BackIconBtn.SetColor(_UI_Color.Instance.color1);
					BackTextBtn.SetColor(_UI_Color.Instance.color1);	
			
				break;
			
			case POINTER_INFO.INPUT_EVENT.MOVE:
					BackIconBtn.SetColor(_UI_Color.Instance.color1);
					BackTextBtn.SetColor(_UI_Color.Instance.color1);	
				break;
			
			case POINTER_INFO.INPUT_EVENT.RELEASE:
			case POINTER_INFO.INPUT_EVENT.MOVE_OFF:
			case POINTER_INFO.INPUT_EVENT.RELEASE_OFF:
					
				BackIconBtn.SetColor(_UI_Color.Instance.color2);
				BackTextBtn.SetColor(_UI_Color.Instance.color4);
			
			break;
			
		   case POINTER_INFO.INPUT_EVENT.TAP:
				_UI_CS_ScreenCtrl.Instance.SetNextScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_INGAME_NORMAL);
				_UI_CS_FightScreen.Instance.m_CS_Ingame_NormalPanel.BringIn();
				ConsumablePanel.Dismiss();
				CostPanel.Dismiss();
				ThanksPanel.Dismiss();
				BackIconBtn.SetColor(_UI_Color.Instance.color2);
				BackTextBtn.SetColor(_UI_Color.Instance.color4);

                Player.Instance.ReactivePlayer();
                GameCamera.BackToPlayerCamera();
			
				break;
		   default:
				break;
		}	
	}
	
	void CostBuyDelegate(ref POINTER_INFO ptr)
	{
		switch(ptr.evt)
		{
		   case POINTER_INFO.INPUT_EVENT.TAP:
				
				CS_Main.Instance.g_commModule.SendMessage(
		   			ProtocolGame_SendRequest.BuyConsumeItem(2,CurrentBuyItemInfo._ItemID,CostBuyNum)
											 );
				CostBuyNum = 1;
				break;
		   default:
				break;
		}	
	}
	
	void CostAddDelegate(ref POINTER_INFO ptr)
	{
		switch(ptr.evt)
		{
		   case POINTER_INFO.INPUT_EVENT.TAP:
				CostBuyNum++;
				if(CostBuyNum > 9){
					CostBuyNum = 9;
				}
				CostBuyNumText.Text = CostBuyNum.ToString();
				CostSale1Text.Text = (CostBuyNum * itemPice).ToString();
				break;
		   default:
				break;
		}	
	}
	
	void CostSubDelegate(ref POINTER_INFO ptr)
	{
		switch(ptr.evt)
		{
		   case POINTER_INFO.INPUT_EVENT.TAP:
				CostBuyNum--;
				if(CostBuyNum < 1){
					CostBuyNum =1;
				}
				CostBuyNumText.Text = CostBuyNum.ToString();
				CostSale1Text.Text = (CostBuyNum * itemPice).ToString();
				break;
		   default:
				break;
		}	
	}
	
//	void ThanksCancelDelegate(ref POINTER_INFO ptr)
//	{
//		switch(ptr.evt)
//		{
//		   case POINTER_INFO.INPUT_EVENT.TAP:
//				ThanksPanel.Dismiss();
//				break;
//		   default:
//				break;
//		}	
//	}
	
	void ThanksBtnDelegate(ref POINTER_INFO ptr)
	{
		switch(ptr.evt)
		{
		   case POINTER_INFO.INPUT_EVENT.TAP:
				ThanksPanel.Dismiss();
				break;
		   default:
				break;
		}	
	}
	
	void FoodTabDelegate(ref POINTER_INFO ptr)
	{
		switch(ptr.evt)
		{
		   case POINTER_INFO.INPUT_EVENT.TAP:
				CostPanel.Dismiss();
				break;
		   default:
				break;
		}	
	}
	
	void ScrollTabDelegate(ref POINTER_INFO ptr)
	{
		switch(ptr.evt)
		{
		   case POINTER_INFO.INPUT_EVENT.TAP:
				CostPanel.Dismiss();
				break;
		   default:
				break;
		}	
	}
	
	public void ReadSaleItemFile(){
		SaleItemList.Clear();
		string fileName = "ConsumableItem.General";
		if(null != fileName){
		string _fileName = LocalizeManage.Instance.GetLangPath(fileName);
		TextAsset item = (TextAsset)Resources.Load(_fileName, typeof(TextAsset));
			string[] itemRowsList = item.text.Split('\n');
			for (int i = 3; i < itemRowsList.Length - 1; ++i){
				string pp = itemRowsList[i];	
				string[] vals = pp.Split(new char[] { '	', '	' });		
				SaleItemList.Add(int.Parse(vals[0]));
			}
		}	
	}
	
	public void InitItemList(){
		for(int i = 0;i<SaleItemList.Count;i++){
			ItemDropStruct tempItem = ItemDeployInfo.Instance.GetItemObject(SaleItemList[i],1,0,0,0,1);
			switch(tempItem._SecTypeID){
			case 4:
				ItemListArray[0].AddElement(tempItem);
				break;
			case 5:
				ItemListArray[0].AddElement(tempItem);
				break;
			case 6:
				ItemListArray[1].AddElement(tempItem);
				break;
			case 7:
				ItemListArray[2].AddElement(tempItem);
				break;
			case 8:
				ItemListArray[3].AddElement(tempItem);
				break;
			case 9:
				ItemListArray[4].AddElement(tempItem);
				break;
			default:
				break;
			}
		}
		ItemListArray[0].InitItemList();
		ItemListArray[1].InitItemList();
		ItemListArray[2].InitItemList();
		ItemListArray[3].InitItemList();
		ItemListArray[4].InitItemList();
	}
	
	public void AwakeConsumable(){
		InitImage();
		_UI_CS_ScreenCtrl.Instance.SetNextScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_CONSUMABLE_MENU);
		ConsumablePanel.BringIn ();
		MoneyBadgeInfo.Instance.Hide(false);
		if(!isInit){
			isInit = true;
			IntiConsumable();
		}
		ChangeShopTips();
		Player.Instance.FreezePlayer();
	}
	
	private void InitImage(){
//		npc.SetUVs(new Rect(0,0,1,1));
		//downloading image
//		TextureDownLoadingMan.Instance.DownLoadingTexture("Figure_use2",npc.transform);
		TextureDownLoadingMan.Instance.DownLoadingTexture("Dia_Box3",smallBg.transform);
	}
}
