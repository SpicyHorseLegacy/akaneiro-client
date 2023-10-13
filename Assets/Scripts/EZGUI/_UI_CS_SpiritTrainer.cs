using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class _UI_CS_SpiritTrainer : MonoBehaviour {
	public static _UI_CS_SpiritTrainer Instance;
	public UIPanel			spiritTrainerPanel;
	public UIButton			fareWellBtn;
	public UIButton			fareWellIcon;
	public SpriteText		fareWellText;	
	public List<_UI_CS_SpiritHelperItem>	SpiritList	= new List<_UI_CS_SpiritHelperItem>();
	public int IconIdx;
	public List<string> 	TipsList   					= new List<string>();
	public SpriteText    	TipsText;
	public Transform 		SpiritPos;
//	public UIButton  npc;
	public UIButton  smallBg;

	void Awake() {
		Instance = this;
	}
	
	// Use this for initialization
	void Start () {
		BGMInfo.Instance.isPlayUpGradeEffectSound = true;
		fareWellBtn.AddInputDelegate(FareWellBtnDelegate);
		ReadShopTipsInfo();
		//need first inti, becase anyOther will use pet info.20130518 fix.
		InitSpiritItem();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
#region Local
	private void InitImage(){
		//downloading image
//		TextureDownLoadingMan.Instance.DownLoadingTexture("Figure_use5",npc.transform);
		TextureDownLoadingMan.Instance.DownLoadingTexture("Dia_Box3",smallBg.transform);
	}
	
	private void ReadShopTipsInfo(){
		string fileName = "SpiritHelperShopTip.Description";
		string _fileName = LocalizeManage.Instance.GetLangPath(fileName);
		TextAsset item = (TextAsset)Resources.Load(_fileName, typeof(TextAsset));
		string[] itemRowsList = item.text.Split('\n');
		for (int i = 3; i < itemRowsList.Length - 1; ++i){
			string pp = itemRowsList[i];
		   	string[] vals = pp.Split(new char[] { '	', '	' });	
			TipsList.Add(vals[0]);
		}
		LogManager.Log_Info("SpiritHelperShopTips Ok");
	}
	
	private void ChangeShopTips(){
		int tempIdx = Random.Range(0,TipsList.Count);
		if(TipsList.Count >0)
			TipsText.Text = TipsList[tempIdx];
	}
	
	private void FareWellBtnDelegate(ref POINTER_INFO ptr){
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
				_UI_CS_FightScreen.Instance.m_CS_Ingame_NormalPanel.BringIn();
				_UI_CS_SpiritTrainer.Instance.spiritTrainerPanel.Dismiss();
				_UI_Spirit3DmodelCtrl.Instance.Hide(_UI_CS_SpiritTrainer.Instance.IconIdx);
				_UI_CS_ScreenCtrl.Instance.SetNextScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_INGAME_NORMAL);
                Player.Instance.ReactivePlayer();
                GameCamera.BackToPlayerCamera();
				fareWellIcon.SetColor(_UI_Color.Instance.color2);
				fareWellText.SetColor(_UI_Color.Instance.color4);
				break;
		}	
	}
#endregion

#region Interface
	public void AwakeSpiritTrainer(){
		spiritTrainerPanel.BringIn();
		MoneyBadgeInfo.Instance.Hide(false);
		_UI_CS_ScreenCtrl.Instance.SetNextScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_SPIRT_TRAINER);
		ChangeShopTips();
		InitImage();
	}
	
	public int SearchSpiritItem(int id){
		for(int i = 0;i<SpiritList.Count;i++ ){
			if(SpiritList[i].m_type == id){
				return i;
			}
		}	
		return -1;
	}
	
	public int GetSpiritHelperIconID(int id){
		for(int i = 0;i<SpiritList.Count;i++ ){
			if(SpiritList[i].m_type == id){
				return SpiritList[i].m_iconID;
			}
		}	
		return 0;
	}
	
	public string GetSpiritHelperName(int id){
		for(int i = 0;i<SpiritList.Count;i++ ){
			if(SpiritList[i].m_type == id){
				return SpiritList[i].m_name;
			}
		}	
		return "Unknow Spirit Helper.";
	}
	
	public void InitSpiritItem(){
		SpiritList.Clear();
		string _fileName = LocalizeManage.Instance.GetLangPath("PetPrice.Price");
		TextAsset item = (TextAsset)Resources.Load(_fileName, typeof(TextAsset));
		string[] itemRowsList = item.text.Split('\n');
		for (int i = 3; i < itemRowsList.Length - 1; ++i){
			_UI_CS_SpiritHelperItem shItem = new _UI_CS_SpiritHelperItem();
			string pp = itemRowsList[i];
			   	string[] vals 			= pp.Split(new char[] { '	', '	' });	
				shItem.m_type 			= int.Parse(vals[0]);	
				shItem.m_name 			= vals[2];	
				shItem.m_Descript 		= vals[4];	
				shItem.m_Buffdescript 	= vals[5];	
				shItem.m_PayMoney 		= int.Parse(vals[3]);
				shItem.m_iconID 		= (int.Parse(vals[1]) - 1);
				shItem.m_levelReq		= int.Parse(vals[7]);
				shItem.m_BuyTime 		= "0";
				shItem.m_LeaveTime 		= "0";
				shItem.m_level 			= 1;
				shItem.m_IsFreeDay 		= false;
				shItem.m_CurrentExp 	= 0;
				shItem.m_CurrentExpMax 	= 100;
				shItem.m_isShopShow		= int.Parse(vals[8]);
				shItem.m_isShow = false;
				SpiritList.Add(shItem);
		}
	}
	
	public void InitSpiritTrainer(){
		_UI_CS_SpiritHelper.Instance.m_SHItemList.Clear();
		_UI_CS_SpiritHelper.Instance.m_List.ClearList(false);	
		for(int i = 0; i<SpiritList.Count;i++ ){
			if(SpiritList[i].m_isShopShow == 1) {
				_UI_CS_SpiritHelper.Instance.AddSpiritHelperListElement(SpiritList[i]);
			}
			if(SpiritList[i].m_isShopShow == 0) {
				if(SpiritList[i].m_isShow) {
				_UI_CS_SpiritHelper.Instance.AddSpiritHelperListElement(SpiritList[i]);
				}
			}
		}
		_UI_CS_SpiritHelper.Instance.InitSpiritHelper();
	}
	
	public long GetPetMaxExp(int level){
		string _fileName = LocalizeManage.Instance.GetLangPath("PetLevelUp.LevelXP");
		TextAsset item = (TextAsset)Resources.Load(_fileName, typeof(TextAsset));
		string[] itemRowsList = item.text.Split('\n');	
		for (int i = 3; i < itemRowsList.Length - 1; ++i){
			string pp = itemRowsList[i];
			string[] vals = pp.Split(new char[] { '	', '	' });	
			if(0 == string.Compare((level+1).ToString(),vals[0])) {	
				return long.Parse(vals[1]);	
			}	
		}
		return 100;
	}
#endregion

	
}
