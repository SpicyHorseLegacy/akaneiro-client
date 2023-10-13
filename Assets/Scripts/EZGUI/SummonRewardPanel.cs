using UnityEngine;
using System.Collections;

public class SummonRewardPanel : MonoBehaviour {
	
	//Instance
	public static SummonRewardPanel Instance = null;
	
	public UIPanel		 			basePanel;
	public int 						totalKarma;
	public int 						totalExp;
	private Rect 					rect = new Rect(0,0,1,1);
	public UIPanel		 			dataPanel;
	public UIListItemContainer  	itemContainer;
	public UIScrollList				list;
	public UIButton     			dataContinueBtn;
	public UIPanel					expPanel;
	public SpriteText 				expKarmaText;
	public SpriteText 				expExpText;
	public SpriteText 				levelText;
	public UIProgressBar 			levelBar;
	public UIProgressBar 			levelEffBar;
	public UIButton     			expContinueBtn;
	
	private bool					isCalc 		= false;
	private float 		 			increment   = 0.01f;
	private bool 		 			isNextLv	= false;
	public UIButton  				npc;
	
	void Awake() {
		Instance = this;
	}
	
	// Use this for initialization
	void Start () {
		dataContinueBtn.AddInputDelegate(DataContinueBtnDelegate);
		expContinueBtn.AddInputDelegate(ExpContinueBtnDelegate);
	}
	
	// Update is called once per frame
	void Update () {
		if(isCalc){
			levelBar.Value += increment;
			if(levelBar.Value >= levelEffBar.Value){
				if(isNextLv){
					ResetBar();
				}else{
					isCalc = false;
					levelBar.Value = levelEffBar.Value;
				}
			}
		}
	}
	
	public void AwakeSummonReward(mapFriendHireReward friendsInfo){
		npc.SetUVs(new Rect(0,0,1,1));
		//downloading image
		TextureDownLoadingMan.Instance.DownLoadingTexture("Figure_use6",npc.transform);
		if(friendsInfo.Count != 0&&_UI_CS_EventRewards.Instance.IsFirstLogin){
			_UI_CS_ScreenCtrl.Instance.SetNextScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_SUMMON_REWARD);
			MoneyBadgeInfo.Instance.Hide(true);
			_UI_CS_FightScreen.Instance.m_CS_Ingame_NormalPanel.Dismiss();
			basePanel.BringIn();
			InitSummonRewardList(friendsInfo);
			dataPanel.BringIn();
		}else{
			//检测是否MISSION结算
//			_UI_CS_MissionSummary.Instance.CheckIsMissionComplete();
			MissionComplete.Instance.CheckIsMissionComplete();
		}
		_UI_CS_EventRewards.Instance.IsFirstLogin = false;
	}
	
	private void InitSummonRewardList(mapFriendHireReward friendsInfo){
		list.ClearList(true);
		totalKarma	= 0;
		totalExp	= 0;
		foreach (SFriendHireReward rewardInfo in friendsInfo.Values){
            UIListItemContainer item = (UIListItemContainer)list.CreateItem((GameObject)itemContainer.gameObject);
			item.GetComponent<SummonAllyRewardItem>().karma.Text = rewardInfo.sk.ToString();
			totalKarma += rewardInfo.sk;
			item.GetComponent<SummonAllyRewardItem>().exp.Text = rewardInfo.exp.ToString();
			totalExp += rewardInfo.exp;
			item.GetComponent<SummonAllyRewardItem>().name.Text = rewardInfo.charName.ToString();
			//初始化头像Icon
			item.GetComponent<SummonAllyRewardItem>().icon.SetUVs(rect);	
			item.GetComponent<SummonAllyRewardItem>().icon.SetTexture(_PlayerData.Instance.GetPlayerIcon(rewardInfo.style,rewardInfo.sex));
        }
		transform.GetComponent<CalculateSlider>().Calculate();
	}
	
	void DataContinueBtnDelegate(ref POINTER_INFO ptr)
	{
		switch(ptr.evt)
		{
		   case POINTER_INFO.INPUT_EVENT.TAP:
				CS_Main.Instance.g_commModule.SendMessage(
				   		ProtocolGame_SendRequest.ProcessFriendReward()
					);
				dataPanel.Dismiss();
				InitExpPanel();
				expPanel.BringIn();
				break;
		}	
	}
	
	void ExpContinueBtnDelegate(ref POINTER_INFO ptr)
	{
		switch(ptr.evt)
		{
		   case POINTER_INFO.INPUT_EVENT.TAP:
				basePanel.Dismiss();
				expPanel.Dismiss();
				MoneyBadgeInfo.Instance.Hide(false);
				_UI_CS_FightScreen.Instance.m_CS_Ingame_NormalPanel.BringIn();
				_UI_CS_ScreenCtrl.Instance.SetNextScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_INGAME_NORMAL);
				//检测是否MISSION结算
//				_UI_CS_MissionSummary.Instance.CheckIsMissionComplete();
				MissionComplete.Instance.CheckIsMissionComplete();
				break;
		}	
	}
	
	void InitExpPanel(){
		expExpText.Text = totalExp.ToString();
		expKarmaText.Text = totalKarma.ToString();
		levelText.Text = _PlayerData.Instance.playerLevel.ToString();
		InitLevelBarInfo();
		isCalc = true;
	}
	
	private void InitLevelBarInfo(){
		//get level min exp
		long tcurexp= _PlayerData.Instance.readCurExpVal(_PlayerData.Instance.playerLevel);
		if(totalExp >= (_PlayerData.Instance.playerCurrentExp-tcurexp)){
			tcurexp = _PlayerData.Instance.readCurExpVal(_PlayerData.Instance.playerLevel-1);
			isNextLv = true;
			// alrd lv up so
			long curExp = 0;
			long maxExp = _PlayerData.Instance.ReadMaxExpVal(_PlayerData.Instance.playerLevel-1);
			curExp = (maxExp-tcurexp) - (totalExp - (_PlayerData.Instance.playerCurrentExp-tcurexp)) ;
			if(0 > curExp){
				curExp = 0;
			}
			levelBar.Value = (float)curExp/(float)(maxExp-tcurexp);
			levelEffBar.Value = 1;
			levelText.Text = (_PlayerData.Instance.playerLevel-1).ToString();
		}else{
			isNextLv = false;
			tcurexp= _PlayerData.Instance.readCurExpVal(_PlayerData.Instance.playerLevel);
			long curExp = _PlayerData.Instance.playerCurrentExp - totalExp - tcurexp;
			if(0 > curExp){
				curExp = 0;
			}
			long maxExp = _PlayerData.Instance.ReadMaxExpVal(_PlayerData.Instance.playerLevel);
			levelBar.Value = (float)(curExp-tcurexp)/(float)(maxExp-tcurexp);
			levelEffBar.Value = (float)((_PlayerData.Instance.playerCurrentExp)-tcurexp)/(float)(maxExp-tcurexp);
			levelText.Text = _PlayerData.Instance.playerLevel.ToString();
		}
	}
	
	void ResetBar(){
			isNextLv = false;
			long curExp = 0;
			long maxExp = _PlayerData.Instance.ReadMaxExpVal(_PlayerData.Instance.playerLevel);
			long tcurexp= _PlayerData.Instance.readCurExpVal(_PlayerData.Instance.playerLevel);
			levelBar.Value = (float)(curExp-tcurexp)/(float)(maxExp-tcurexp);
			levelEffBar.Value = (float)((_PlayerData.Instance.playerCurrentExp)-tcurexp)/(float)(maxExp-tcurexp);
			levelText.Text = _PlayerData.Instance.playerLevel.ToString();
	}
}
