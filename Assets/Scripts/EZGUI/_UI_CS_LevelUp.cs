using UnityEngine;
using System.Collections;

public class _UI_CS_LevelUp : MonoBehaviour {
	//Instance
	public static _UI_CS_LevelUp Instance = null;
	public UIPanel		 LevelUpBgPanel;
	public UIPanel		 LevelUpAttrPanel;
	public UIPanel		 LevelUpDisPanel;
	public UIPanel		 LevelUpLogoPanel;
	public UIPanel		 LevelUpLogoTPanel;
	public UIButton		 AttrNextBtn;
	public UIButton 	 DisNextBtn;
	public SpriteText    CurrentLevelText;
	public bool 		 IsLevelUp = false;
	public UIRadioBtn    CunningBtn;
	public UIRadioBtn    FortitudeBtn;
	public UIRadioBtn    ProwessBtn;
	public int 			 ProwessPoint = 0;
	public int 			 CunningPoint = 0;
	public int 			 FortitudePoint = 0;
	private int 		 AddPoint = 0;
	
	public SpriteText    ATKText;
	public SpriteText    DEFText;
	public SpriteText    CRTText;
	public SpriteText    HPText;
	
	public SpriteText    ATKADDText;
	public SpriteText    DEFADDText;
	public SpriteText    CRTADDText;
	public SpriteText    HPADDText;
	
	private int 		 LastAddPointIdx = 0;
	
	public SpriteText    addPointTipText;
	
	public Transform 	levelSoundPrefab;
	
	private int defP = 0;
	private int atkP = 0;
	private int crtP = 0;
	
	public UIButton  levelBg;
	
	void Awake(){
		Instance = this;
	}
	
	public void AwakeLevelUp(){
		InitImage();
		if (BGManager.Instance){
				BGManager.Instance.StopOriginalBG();
		}
		_UI_CS_LevelUp.Instance.CurrentLevelText.Text = _PlayerData.Instance.playerLevel.ToString();
		SoundCue.PlayPrefabAndDestroy(levelSoundPrefab);
		_UI_CS_ScreenCtrl.Instance.SetNextScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_LEVELUP);
		MoneyBadgeInfo.Instance.Hide(true);
		LevelUpBgPanel.BringIn();
		LevelUpLogoTPanel.BringIn();
		LevelUpLogoPanel.BringIn();
		PlayerInfoBar.Instance.UpdatePlayerLevel(int.Parse(CurrentLevelText.text));
		StartCoroutine(StartLogoCB());
	}
	
	private void InitImage(){
//		levelBg.SetUVs(new Rect(0,0,1,1));
		//downloading image
		TextureDownLoadingMan.Instance.DownLoadingTexture("LevelUP_P_LevleUP",levelBg.transform);
	}
	
	//
	public void AssignTalentAckLogic(){
		CalcVal();
		LevelUpDisPanel.Dismiss();
		LevelUpAttrPanel.BringIn();	
	}
	
	private IEnumerator StartLogoCB(){
		yield return new WaitForSeconds(2f);
		LevelUpLogoTPanel.Dismiss();
		LevelUpLogoPanel.Dismiss();
		LevelUpDisPanel.BringIn();
	}
	
	// Use this for initialization
	void Start () {
		DisNextBtn.AddInputDelegate(DisNextBtnDelegate);
		AttrNextBtn.AddInputDelegate(AttrNextBtnDelegate);
		CunningBtn.AddInputDelegate(CunningBtnDelegate);
		FortitudeBtn.AddInputDelegate(FortitudeBtnDelegate);
		ProwessBtn.AddInputDelegate(ProwessBtnDelegate);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void CunningBtnDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:	
//				addPointTipText.Text = "All Cunning abilities receive a bonus.     Skill is increased.";
				LocalizeManage.Instance.GetDynamicText(addPointTipText,"ALLCARABSII");
				break;
		   default:
				break;
		}	
	}
	
	public void FortitudeBtnDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:	
//				addPointTipText.Text = "All Fortitude abilities receive a bonus.     Defense is increased.";
				LocalizeManage.Instance.GetDynamicText(addPointTipText,"ALLCARABDII");
				break;
		   default:
				break;
		}	
	}
	
	public void ProwessBtnDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:	
//				addPointTipText.Text = "All Prowess abilities receive a bonus.     Power is increased.";
				LocalizeManage.Instance.GetDynamicText(addPointTipText,"ALLCARABPII");
				break;
		   default:
				break;
		}	
	}

	
	public void AttrNextBtnDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:
				LevelUpAttrPanel.Dismiss();
				CS_Main.Instance.g_commModule.SendMessage(
			   		ProtocolGame_SendRequest.hasAssignedTalentPointReq()
				);
				break;
		   default:
				break;
		}	
	}
	
	public void LeaveLevelUp(){
		if (BGManager.Instance){
            BGMInfo.AutoPlayBGM = true;
			BGManager.Instance.PlayOriginalBG();
            BGMInfo.AutoPlayBGM = false;
		}
		LevelUpBgPanel.Dismiss();
		LevelUpAttrPanel.Dismiss();
		LevelUpDisPanel.Dismiss();
		#region missioon complete
//		_UI_CS_MissionSummary.Instance.CloseSummary();
		MissionComplete.Instance.CloseMissionComplete();
		#endregion
		_UI_CS_ScreenCtrl.Instance.SetNextScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_INGAME_NORMAL);
		MoneyBadgeInfo.Instance.Hide(false);
		_UI_CS_FightScreen.Instance.m_CS_Ingame_NormalPanel.BringIn();
		_UI_MiniMap.Instance.isShowSmallMap = true;
		//事件提示入口
		EventSystem.Instance.CheckEvent(EM_EVENT_TYPE.EM_MONEY);
		EventSystem.Instance.AwakeEventPanel();
	}
	
	public void DisNextBtnDelegate(ref POINTER_INFO ptr)
	{
		switch(ptr.evt)
		{
		   case POINTER_INFO.INPUT_EVENT.TAP:	
					SendAddPointMsg();
					//first process,dont wait server ack
					AssignTalentAckLogic();
					
				break;
		   default:
				break;
		}	
	}

	public void InitPlayerBaseNum(){
	
		string fileName = "GameConfig.xml";
	
	}
	
	public int GetAddPointDis(){
		if(CunningBtn.Value){
			return 2;
		}else if(FortitudeBtn.Value){
			return 1;
		}else{
			return 0;
		}
	}
	
	public void SendAddPointMsg(){
		//todo: before use
		//AddPoint = (int.Parse(CurrentLevelText.text) - ProwessPoint - CunningPoint - FortitudePoint - 1);
		//now it add point. one by one
		AddPoint = 1;
		int temp = GetAddPointDis();
		LastAddPointIdx = temp;
		switch(temp){
		case 0:
			CS_Main.Instance.g_commModule.SendMessage(
   				ProtocolGame_SendRequest.assignTalentReq(AddPoint,0,0)
			);
			break;
		case 1:
			CS_Main.Instance.g_commModule.SendMessage(
   				ProtocolGame_SendRequest.assignTalentReq(0,AddPoint,0)
			);
			break;
		case 2:
			CS_Main.Instance.g_commModule.SendMessage(
   				ProtocolGame_SendRequest.assignTalentReq(0,0,AddPoint)
			);
			break;
		}
		
	}
	
	public int GetPlayerAddAtk(){
		switch(_PlayerData.Instance.CharactorInfo.style){
		case 1:
			return 6;
		case 2:
			return 4;
		case 4:
			return 2;
		default:
			return 0;
		}
	}
	
	public int GetPlayerAddDef(){
		switch(_PlayerData.Instance.CharactorInfo.style){
		case 1:
			return 2;
		case 2:
			return 6;
		case 4:
			return 4;
		default:
			return 0;
		}
	}
	
	public int GetPlayerAddCrt(){
		switch(_PlayerData.Instance.CharactorInfo.style){
		case 1:
			return 4;
		case 2:
			return 2;
		case 4:
			return 6;
		default:
			return 0;
		}
	}
	
	
	private void SetPlayerAttruInc(int lastPoint){
		switch(lastPoint){
		case 0:
			ATKADDText.Text = "+ 3";	
			DEFADDText.Text = "+ 1";
			CRTADDText.Text = "+ 2";
			atkP = 3;
			defP = 1;
			crtP = 2;	
			return;
		case 1:
			ATKADDText.Text = "+ 2";	
			DEFADDText.Text = "+ 3";
			CRTADDText.Text = "+ 1";
			atkP = 2;
			defP = 3;
			crtP = 1;	
			return;
		case 2:
			ATKADDText.Text = "+ 1";
			DEFADDText.Text = "+ 2";
			CRTADDText.Text = "+ 3";
			atkP = 1;
			defP = 2;
			crtP = 3;	
			return;
		}
	}
	
	public void CalcVal(){
	
		int def = 0;
		int atk = 0;
		int hp  = 0;
		int crt = 0;
		
		//重新修改4个属性->只是玩家四个基础属性
		atk = _PlayerData.Instance.BaseAttrs[EAttributeType.ATTR_Power];
		def = _PlayerData.Instance.BaseAttrs[EAttributeType.ATTR_Defense];
		crt = _PlayerData.Instance.BaseAttrs[EAttributeType.ATTR_Skill];
		hp = _PlayerData.Instance.BaseAttrs[EAttributeType.ATTR_MaxHP];
		
		SetPlayerAttruInc(LastAddPointIdx);
		
		ATKText.Text = (atk+atkP).ToString();
		DEFText.Text = (def+defP).ToString();
		CRTText.Text = (crt+crtP).ToString();
		HPText.Text = hp.ToString();


		HPADDText.Text = _PlayerData.Instance.ReadHpIncVal(_PlayerData.Instance.playerLevel).ToString();
	}
	
}
