using UnityEngine;
using System.Collections;

public class DailyRewardGift : MonoBehaviour {
	
	//Instance
	public static DailyRewardGift Instance = null;
	
	public UIPanel 		basePanel;
	public UIButton 	exitBtn;
	public UIPanel 		thanksPanel;
	public UIButton 	thanksSureBtn;
	public UIButton 	thanksIcon;
	public SpriteText 	thanksNameText;
	public Texture2D	karmaPic;
	public Texture2D	realMoneyPic;
	public Texture2D	unknowPic;
	private Rect		rect = new Rect(0,0,1,1);
	public DailyRewardItem items;
	private ItemDropStruct tempItem ;
	private int [] giftStateArray = new int[20];
	public int dailyRewardFileIndex = 0;
//	public UIButton  npc;
	void Awake(){
		Instance = this;
	}
	
	// Use this for initialization
	void Start () {
		thanksSureBtn.AddInputDelegate(ThanksSureBtnDelegate);
		exitBtn.AddInputDelegate(ExitBtnDelegate);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void AwakeDailyReward(EServerErrorType error,int dayIdx,vectorInt vector){
//		npc.SetUVs(new Rect(0,0,1,1));
		//downloading image
//		TextureDownLoadingMan.Instance.DownLoadingTexture("Figure_use6",npc.transform);
		//判断是否需要弹出daily Reward 界面
		if(error.Get() != EServerErrorType.eSuccess){
			if(error.Get() == EServerErrorType.eDayRewardError_AlreadyGet){
				//检测是否MISSION结算
//				_UI_CS_MissionSummary.Instance.CheckIsMissionComplete();
				//请求好友召唤奖励 不用，因为已经领过每日奖励说明领过了好友奖励
				CS_Main.Instance.g_commModule.SendMessage(
			   		ProtocolGame_SendRequest.GetFriendReward()
				);
				return;
			}else{
				_UI_CS_PopupBoxCtrl.PopUpError(error);
			}
		}
		
//		//判断是否在village(只有village有mission)
//		if(!_UI_CS_MapScroll.Instance.IsExistMission){
//			return;
//		}
		
		//初始化奖励界面，弹出
		InitDailyRewardList(dayIdx,vector);
		_UI_CS_ScreenCtrl.Instance.SetNextScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_EVENT_REWARDS);
		MoneyBadgeInfo.Instance.Hide(true);
		_UI_CS_FightScreen.Instance.m_CS_Ingame_NormalPanel.Dismiss();
		basePanel.BringIn();
	}
	
	public void InitCurDayGift(int rewardType,ItemDropStruct info,int count){
		thanksIcon.SetUVs(rect);
		switch(rewardType){
				case 1:
					thanksIcon.SetTexture(karmaPic);
//					thanksNameText.Text = count.ToString() + " Karma Shards";
					LocalizeManage.Instance.GetDynamicText(thanksNameText,"KS");
					thanksNameText.Text = (count.ToString() + " "  + thanksNameText.text);
				break;
				case 2:
					thanksIcon.SetTexture(realMoneyPic);
//					thanksNameText.Text = count.ToString()+ " Karma Crystals";
					LocalizeManage.Instance.GetDynamicText(thanksNameText,"KC");
					thanksNameText.Text = (count.ToString() + " " + thanksNameText.text);
				break; 
				case 4:
					ItemPrefabs.Instance.GetItemIcon(info._ItemID,info._TypeID,info._PrefabID,thanksIcon);
					thanksNameText.Text = info._PropsName;
				break;
				case 5:
					ItemPrefabs.Instance.GetItemIcon(info._ItemID,info._TypeID,info._PrefabID,thanksIcon);
					_UI_CS_ItemVendor.Instance.SetColorForName(thanksNameText,info);
				break;
			}
	}
	
	public void InitDailyRewardList(int curDays,vectorInt vector){
		int rewardType = 0;
		TextAsset item;
		string _fileName;
		switch(dailyRewardFileIndex) {
		case 0:
			_fileName = LocalizeManage.Instance.GetLangPath("DayReward.reward");
			item = (TextAsset)Resources.Load(_fileName, typeof(TextAsset));
			break;
		case 1:
			_fileName = LocalizeManage.Instance.GetLangPath("DayReward.reward2");
			item = (TextAsset)Resources.Load(_fileName, typeof(TextAsset));
			break;
		case 2:
			_fileName = LocalizeManage.Instance.GetLangPath("DayReward.reward3");
			item = (TextAsset)Resources.Load(_fileName, typeof(TextAsset));
			break;
		case 3:
			_fileName = LocalizeManage.Instance.GetLangPath("DayReward.reward4");
			item = (TextAsset)Resources.Load(_fileName, typeof(TextAsset));
			break;
		default:
			_fileName = LocalizeManage.Instance.GetLangPath("DayReward.reward");
			item = (TextAsset)Resources.Load(_fileName, typeof(TextAsset));
			break;
		}
		string[] itemRowsList = item.text.Split('\n');
		for (int i = 3; i < itemRowsList.Length&&i<21; ++i){
			//Ps:i>curDay+3-1;
//			if(i<=(curDays+2)){
				items.giftItems[i-3].icon.SetUVs(rect);
				vector.CopyTo(giftStateArray);
				if(giftStateArray[i-3] == 1){
					items.giftItems[i-3].receive.gameObject.layer = LayerMask.NameToLayer("EZGUI_CanTouch");
				}else{
					items.giftItems[i-3].receive.gameObject.layer = LayerMask.NameToLayer("Default");
				}
				
				string pp = itemRowsList[i];
			   	string[] vals = pp.Split(new char[] { '	', '	' });	
				switch(int.Parse(vals[1])){
					case 1:
						rewardType = 1;
						items.giftItems[i-3].icon.SetTexture(karmaPic);
					break;
					case 2:
						rewardType = 2;
						items.giftItems[i-3].icon.SetTexture(realMoneyPic);
					break; 
					case 4:
						rewardType = 4;
						tempItem = ItemDeployInfo.Instance.GetItemObject(int.Parse(vals[3]),1,0,0,0,1);
						ItemPrefabs.Instance.GetItemIcon(tempItem._ItemID,tempItem._TypeID,tempItem._PrefabID,
														   items.giftItems[i-3].icon
														 );
					break;
					case 5:
						rewardType = 5;
						tempItem = ItemDeployInfo.Instance.GetItemObject(int.Parse(vals[3]),int.Parse(vals[4]),int.Parse(vals[5]),int.Parse(vals[7]),int.Parse(vals[6]),int.Parse(vals[8]));
						ItemPrefabs.Instance.GetItemIcon(tempItem._ItemID,tempItem._TypeID,tempItem._PrefabID,
														   items.giftItems[i-3].icon
														 );
					break;
				}
				
				if(i == (curDays+2)){
					InitCurDayGift(rewardType,tempItem,int.Parse(vals[2]));
				}
				
//			}else{
//				rewardType = 0;
//				items.giftItems[i-3].icon.SetUVs(rect);
//				items.giftItems[i-3].icon.SetTexture(unknowPic);
//				items.giftItems[i-3].receive.gameObject.layer = LayerMask.NameToLayer("Default");
//			}
		}
		thanksPanel.BringIn();
	}
	
	void ThanksSureBtnDelegate(ref POINTER_INFO ptr)
	{
		switch(ptr.evt)
		{
		   case POINTER_INFO.INPUT_EVENT.TAP:
				thanksPanel.Dismiss();
				break;
		}	
	}
		
	void ExitBtnDelegate(ref POINTER_INFO ptr)
	{
		switch(ptr.evt)
		{
		   case POINTER_INFO.INPUT_EVENT.TAP:
				basePanel.Dismiss();
				_UI_CS_ScreenCtrl.Instance.SetNextScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_INGAME_NORMAL);
				MoneyBadgeInfo.Instance.Hide(false);
				_UI_CS_FightScreen.Instance.m_CS_Ingame_NormalPanel.BringIn();
				//查看剩余点数分配 移到好友奖励后
//				CS_Main.Instance.g_commModule.SendMessage(
//			   		ProtocolGame_SendRequest.hasAssignedTalentPointReq()
//				);
				//请求好友召唤奖励
				CS_Main.Instance.g_commModule.SendMessage(
			   		ProtocolGame_SendRequest.GetFriendReward()
				);
				break;
		}	
	}
}
