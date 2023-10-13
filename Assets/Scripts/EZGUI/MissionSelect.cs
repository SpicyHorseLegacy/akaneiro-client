using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class SCoolDownCost{
	public int time;
	public int crystal;
	public int sub30Min;
	public int sub60Min;
}
public class MissionSelect : MonoBehaviour {

	//Instance
	public static MissionSelect Instance = null;
	public UIButton mianBtnBg;
	public UIButton exitBtn;
	
//	public UIButton npc;
	
	public SpriteText missionName;
	public SpriteText description;
	public SpriteText boosInfo;
	
	public SpriteText recLevel;
	
	public UIButton huntBtn;
	public SpriteText huntText;
	public SpriteText huntTitelext;
	
	
	public UIButton coolDownBtn;
	public SpriteText coolDownTime;
	public SpriteText finishNow;
	public SpriteText coolDownTitelext;
	public UIButton coolDownIcon;
	public UIButton coolDownBg;
	public SpriteText coolDownTitle;
	
	public UIButton lockBtn;
	public SpriteText lockText;
	public SpriteText lockTitelext;
	
	public MissionLevelBtn [] levelBtn;
	
	public SAcceptMissionRelate2 currentInfo;
	
	public UIPanel coolDownFinishPanel;
	public SpriteText finishValText;
	public SpriteText finishInfoText;
	public UIButton halfHourBtn;
	public SpriteText halfHourValText;
	public SpriteText halfHourInfoText;
	public UIButton oneHourBtn;
	public SpriteText oneHourValText;
	public SpriteText oneHourInfoText;
	public UIButton halfHourMidBtn;
	public SpriteText halfHourMidValText;
	public SpriteText halfHourMidInfoText;
	public UIButton finishBtn;
	public UIButton selectExitBtn;
	
	public SpriteText coolDownInterTime;
	
	public UIButton missionFinishIcon;
	public UIButton finishEleBg1;
	
	public UIButton mainInfobg;
	public UIButton missionIcon;
	public SpriteText missionIconName;
	public UIButton enemyEleIcon;
	public SpriteText enemyEleInfo;
	public UIButton matIcon;
	public SpriteText matInfo;
	
	public UIButton buyItemBg1;
	public UIButton buyItemBg2;
	
	public  List<SCoolDownCost> coolDownCostList = new List<SCoolDownCost>(); 
	
	void Awake() {
		Instance = this;
	}
	
	// Use this for initialization
	void Start () {
		InitCostCoolDown();
		exitBtn.AddInputDelegate(ExitDelegate);
		huntBtn.AddInputDelegate(HuntDelegate);
		coolDownBtn.AddInputDelegate(CoolDownBtnDelegate);
		halfHourBtn.AddInputDelegate(HalfHourDelegate);
		oneHourBtn.AddInputDelegate(OneHourDelegate);
		halfHourMidBtn.AddInputDelegate(OneHourDelegate);
		finishBtn.AddInputDelegate(FinishDelegate);
		selectExitBtn.AddInputDelegate(SelectExitDelegate);
	}
	
	// Update is called once per frame
	void Update () {
		if(_UI_CS_ScreenCtrl.Instance.IsScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_MISSION_SELECT)){
			if(isUpdateCoolDownTime) {
				CalcCoolDownTime();
			}
		}
	}
	
#region InterFace
	public void AwakeSelectMission(SAcceptMissionRelate2 info) {
		if(info == null) {
			LogManager.Log_Error("mission area info err");
			return;
		}
		_UI_CS_ScreenCtrl.Instance.SetNextScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_MISSION_SELECT);
		currentInfo = info;
		UpdateLevelState();
		isUpdateCoolDownTime  = true;
		fcoolDownTime = info.coolDownTime;
		CalcCoolDownTime();
		SetScenceName();
		SetLevelDescription();
//		SetRecommedLevel();
		SetBossInfo();
		SetEnemyInfo();
		SetMatInfo();
		SetCoolDownInfo();
		SetAreaName();
		#region mission complete description
//		_UI_CS_MissionSummary.Instance.SetMissionCompleteDescription(MissionPanel.Instance.LevelDescription[MissionPanel.Instance.GetCurrentMapID(),MissionPanel.Instance.GetCurrentRegionID(),MissionPanel.Instance.GetCurrentMissionID(),1]);
		#endregion
		UpdateFinishCoolDownBtn(true,0f);
		transform.GetComponent<UIPanel>().BringIn();
	}
	
	public void SetAreaIcon(Texture2D img) {
		missionIcon.SetUVs(new Rect(0,0,1,1));
		missionIcon.SetTexture(img);
		missionFinishIcon.SetUVs(new Rect(0,0,1,1));
		missionFinishIcon.SetTexture(img);
	}
	
	public void ShowNextBtn(int type) {
		switch(type) {
		case 1:
			IsShowHuntBtn(true);
			IsShowCoolDownBtn(false);
			IsShowLockBtn(false);
			break;
		case 2:
			IsShowHuntBtn(false);
			IsShowCoolDownBtn(true);
			IsShowLockBtn(false);
			break;
		case 3:
			IsShowHuntBtn(false);
			IsShowCoolDownBtn(false);
			IsShowLockBtn(true);
			break;
		}
	}
	
	public void SetLevelDescription() {
		missionName.Text = MissionPanel.Instance.LevelName[MissionPanel.Instance.GetCurrentMapID(),MissionPanel.Instance.GetCurrentRegionID(),MissionPanel.Instance.GetCurrentMissionID()];
		description.Text = MissionPanel.Instance.LevelDescription[MissionPanel.Instance.GetCurrentMapID(),MissionPanel.Instance.GetCurrentRegionID(),MissionPanel.Instance.GetCurrentMissionID(),0];
	}
	
	public void SetRecommedLevel(int level) {
		recLevel.Text = (int.Parse(MissionPanel.Instance.LevelDescription[MissionPanel.Instance.GetCurrentMapID(),MissionPanel.Instance.GetCurrentRegionID(),MissionPanel.Instance.GetCurrentMissionID(),2])+level-1).ToString();
	}
	
	public void SetBossInfo() {
		boosInfo.Text = MissionPanel.Instance.LevelDescription[MissionPanel.Instance.GetCurrentMapID(),MissionPanel.Instance.GetCurrentRegionID(),MissionPanel.Instance.GetCurrentMissionID(),8];
	}
	
	public void SetEnemyInfo() {
		enemyEleInfo.Text = MissionPanel.Instance.LevelDescription[MissionPanel.Instance.GetCurrentMapID(),MissionPanel.Instance.GetCurrentRegionID(),MissionPanel.Instance.GetCurrentMissionID(),4];
		DownLoadInfoIcon(MissionPanel.Instance.LevelDescription[MissionPanel.Instance.GetCurrentMapID(),MissionPanel.Instance.GetCurrentRegionID(),MissionPanel.Instance.GetCurrentMissionID(),5],enemyEleIcon);
	}
	
	public void SetMatInfo() {
		matInfo.Text = MissionPanel.Instance.LevelDescription[MissionPanel.Instance.GetCurrentMapID(),MissionPanel.Instance.GetCurrentRegionID(),MissionPanel.Instance.GetCurrentMissionID(),6]; 
		DownLoadInfoIcon(MissionPanel.Instance.LevelDescription[MissionPanel.Instance.GetCurrentMapID(),MissionPanel.Instance.GetCurrentRegionID(),MissionPanel.Instance.GetCurrentMissionID(),7],matIcon);
	}
	
	public void SetCoolDownInfo() {
		coolDownTitle.Text = MissionPanel.Instance.LevelDescription[MissionPanel.Instance.GetCurrentMapID(),MissionPanel.Instance.GetCurrentRegionID(),MissionPanel.Instance.GetCurrentMissionID(),3];
	}
	
	public void SetAreaName() {
		missionIconName.Text = MissionPanel.Instance.RegionName[MissionPanel.Instance.GetCurrentMapID(),MissionPanel.Instance.GetCurrentRegionID()];
	}
	
	//idx; 1~4//
	public void ChooseLevelThreat(int idx) {
		int tempID = MissionPanel.Instance.currentMissionID;
		MissionPanel.Instance.currentMissionID = (tempID/10*10+idx);
		UpdateLevelSelectHighLight(idx);
		SetRecommedLevel(idx);
		levelBtn[idx-1].UpdateShowBtnState();
		UpdateDiffcult(idx);
		#region init mission complete info
		MissionComplete.Instance.InitMissionCompleteInfo(missionName.text,idx-1);
		#endregion
	}
	
	public void LeaveSelectMssion() {
		_UI_CS_ScreenCtrl.Instance.SetNextScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_BOUNTY_MASTER);
		MouseCtrl.Instance.SetMouseStats(MouseIconType.PALM);
		transform.GetComponent<UIPanel>().Dismiss();
	}
	
	
#endregion
	
#region Local
	void InitCostCoolDown() {
		coolDownCostList.Clear();
		string _fileName = LocalizeManage.Instance.GetLangPath("MissionsThreatCycle.CoolDown");
		TextAsset item = (TextAsset)Resources.Load(_fileName, typeof(TextAsset));
		string[] itemRowsList = item.text.Split('\n');
		for (int i = 3; i < itemRowsList.Length - 1; ++i){
			SCoolDownCost temp = new SCoolDownCost();
			string pp = itemRowsList[i];
			string[] vals = pp.Split(new char[] { '	', '	' });
			temp.time		= int.Parse(vals[0]);		
			temp.crystal 	= int.Parse(vals[1]);	
			temp.sub30Min 	= int.Parse(vals[2]);
			temp.sub60Min 	= int.Parse(vals[3]);
			coolDownCostList.Add(temp);
		}
	}
	
	void DownLoadInfoIcon(string idxStr,UIButton btn) {
		btn.SetUVs(new Rect(0,0,1,1));
		if(string.Compare(idxStr,"0") == 0) {
			btn.SetTexture(_UI_CS_Resource.Instance.m_EquipmentIcon[0]);
		}else {
			TextureDownLoadingMan.Instance.DownLoadingTexture(idxStr,btn.transform);
		}
	}
	
	public UIButton coverImg;
	private Color blue = new Color(0.243f,0.486f,0.984f);
	private Color gray = new Color(0.478f,0.478f,0.478f);
	void UpdateFinishCoolDownBtn(bool isReset,float time) {
		if(isReset) {
			oneHourBtn.SetColor(blue);
			halfHourBtn.SetColor(blue);
			coverImg.Hide(true);
			return;
		}
		if(time > 5400) {
			oneHourBtn.SetColor(blue);
			halfHourBtn.SetColor(blue);
			coverImg.Hide(true);
		}else {
			oneHourBtn.SetColor(gray);
			halfHourBtn.SetColor(gray);
			coverImg.Hide(false);
		}
	}
	
	private void UpdateDiffcult(int dif) {
		switch(dif) {
		case 1:
			levelBtn[0].icon.Hide(false);
			levelBtn[1].icon.Hide(true);
			levelBtn[2].icon.Hide(true);
			levelBtn[3].icon.Hide(true);
			break;
		case 2:
			levelBtn[0].icon.Hide(true);
			levelBtn[1].icon.Hide(false);
			levelBtn[2].icon.Hide(true);
			levelBtn[3].icon.Hide(true);
			break;
		case 3:
			levelBtn[0].icon.Hide(true);
			levelBtn[1].icon.Hide(true);
			levelBtn[2].icon.Hide(false);
			levelBtn[3].icon.Hide(true);
			break;
		case 4:
			levelBtn[0].icon.Hide(true);
			levelBtn[1].icon.Hide(true);
			levelBtn[2].icon.Hide(true);
			levelBtn[3].icon.Hide(false);
			break;
		}
	}
	
	void ShowCostCoolDownBtnState(int type,SCoolDownCost info) {
		switch(type) {
		case 1:
			halfHourBtn.Hide(false);
			halfHourValText.Hide(false);
			halfHourInfoText.Hide(false);
			oneHourBtn.Hide(false);
			oneHourValText.Hide(false);
			oneHourInfoText.Hide(false);
			halfHourMidBtn.Hide(true);
			halfHourMidValText.Hide(true);
			halfHourMidInfoText.Hide(true);

			halfHourValText.Text = info.sub30Min.ToString();
			oneHourValText.Text = info.sub60Min.ToString();
			break;
		case 2:
			halfHourBtn.Hide(true);
			halfHourValText.Hide(true);
			halfHourInfoText.Hide(true);
			oneHourBtn.Hide(true);
			oneHourValText.Hide(true);
			oneHourInfoText.Hide(true);
			halfHourMidBtn.Hide(false);
			halfHourMidValText.Hide(false);
			halfHourMidInfoText.Hide(false);
			
			halfHourMidValText.Text = info.sub30Min.ToString();
			break;
		case 3:
			halfHourBtn.Hide(true);
			halfHourValText.Hide(true);
			halfHourInfoText.Hide(true);
			oneHourBtn.Hide(true);
			oneHourValText.Hide(true);
			oneHourInfoText.Hide(true);
			halfHourMidBtn.Hide(true);
			halfHourMidValText.Hide(true);
			halfHourMidInfoText.Hide(true);
			break;
		}
		if(info.crystal == 0) {
			finishValText.Text = "FREE";
		}else {
			finishValText.Text = info.crystal.ToString();
		}
	}
	
	void CheckShowCoolDownBtnState() {
		float tempTime = Time.time - MissionPanel.Instance.receiveServerMsgTime;
		tempTime = (fcoolDownTime - tempTime);
		int imin = (int)tempTime/60;
		int isec = (int)tempTime%60;
		UpdateFinishCoolDownBtn(false,tempTime);
		if(isec == 0) {
			isec = 0;
		}else {
			isec = 1;
		}
		for(int i = 0;i<coolDownCostList.Count;i++) {
			if(imin+isec <= coolDownCostList[i].time) {
				if(coolDownCostList[i].sub60Min != 0 && coolDownCostList[i].sub30Min != 0) {
					ShowCostCoolDownBtnState(1,coolDownCostList[i]);
					return;
				}else if(coolDownCostList[i].sub60Min == 0 && coolDownCostList[i].sub30Min != 0) {
					ShowCostCoolDownBtnState(1,coolDownCostList[i]);
					return;
				}else if(coolDownCostList[i].sub60Min == 0 && coolDownCostList[i].sub30Min == 0) {
					ShowCostCoolDownBtnState(1,coolDownCostList[i]);
					return;
				}
			}
		}
	}
	
	void SetScenceName() {
		MissionPanel.Instance.currentMissionName = _UI_CS_MapInfo.Instance.Itemlist[MissionPanel.Instance.GetCurrentRegionID()+1].levelList[_UI_CS_MapInfo.Instance.GetLevelIndex(MissionPanel.Instance.GetCurrentMissionID(),0)].mapName;
	}
	
	void UpdateLevelState() {
		UpdateLevelStars();
		UpdateLevelReward();
		UpdateLevelLockState();
	}
	
	void UpdateLevelReward() {
		string karmaReward = "";
		string expReward = "";
		int idx = 0;
		for(int i = 0;i<4;i++) {
			idx = _UI_CS_MapInfo.Instance.GetLevelIndex(MissionPanel.Instance.GetCurrentMissionID(),i);
			expReward = _UI_CS_MapInfo.Instance.Itemlist[MissionPanel.Instance.GetCurrentRegionID()+1].levelList[idx].xp.ToString();
			karmaReward = _UI_CS_MapInfo.Instance.Itemlist[MissionPanel.Instance.GetCurrentRegionID()+1].levelList[idx].sk.ToString();
			levelBtn[i].exp.Text = expReward;
			levelBtn[i].karma.Text = karmaReward;
		}
		
	}
	
	void UpdateLevelStars() {
		for(int i=0;i<4;i++ ) {
			levelBtn[i].star.Hide(true);
		}
		if((currentInfo.star & 8) == 8) {
			levelBtn[3].star.Hide(false);
		}
		if((currentInfo.star & 4) == 4) {
			levelBtn[2].star.Hide(false);
		}
		if((currentInfo.star & 2) == 2) {
			levelBtn[1].star.Hide(false);
		}
		if((currentInfo.star & 1) == 1) {
			levelBtn[0].star.Hide(false);
		}
	}
	
	void UpdateLevelLockState() {
		for(int i = 0;i<4;i++) {
			levelBtn[i].IsLockLevel();
		}
	}
	
	void UpdateLevelSelectHighLight(int idx) {
		for(int i = 0;i<4;i++) {
			levelBtn[i].highLightOver.Hide(true);
			levelBtn[i].highLight.Hide(true);
		}
		levelBtn[idx-1].highLightOver.Hide(false);
		levelBtn[idx-1].highLight.Hide(true);
	}

	void ExitDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:
				LeaveSelectMssion();
				break;
		}	
	}
		
	void HuntDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:
				CS_Main.Instance.g_commModule.SendMessage(
			   		ProtocolGame_SendRequest.AcceptMission(1001,MissionPanel.Instance.currentMissionID)
													 );
				break;
		}	
	}
		
	void CoolDownBtnDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:
				CheckShowCoolDownBtnState();
				coolDownFinishPanel.BringIn();
				break;
		}	
	}
	
	void HalfHourDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:
				CS_Main.Instance.g_commModule.SendMessage(
			   		ProtocolGame_SendRequest.decreaseCoolDown(MissionPanel.Instance.currentMissionID,EDecreaseCooldownType.eDecreaseCooldownType_30)
													 );
				break;
		}	
	}
	
	void OneHourDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:
				CS_Main.Instance.g_commModule.SendMessage(
			   		ProtocolGame_SendRequest.decreaseCoolDown(MissionPanel.Instance.currentMissionID,EDecreaseCooldownType.eDecreaseCooldownType_60)
													 );
				break;
		}	
	}
	
	void FinishDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:
				CS_Main.Instance.g_commModule.SendMessage(
			   			ProtocolGame_SendRequest.decreaseCoolDown(MissionPanel.Instance.currentMissionID,EDecreaseCooldownType.eDecreaseCooldownType_All)
														 );
				break;
		}	
	}
	
	void SelectExitDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:
				coolDownFinishPanel.Dismiss();
				break;
		}	
	}
	
	void HideDescription(bool isHide) {
//		missionName.Hide(isHide);
		description.Hide(isHide);
		boosInfo.Hide(isHide);
	}

	void IsShowHuntBtn(bool isShow) {
		if(isShow) {
			huntBtn.Hide(false);
			huntText.Hide(false);
			huntTitelext.Hide(false);
//			recLevel.Hide(false);
		}else {
			huntBtn.Hide(true);
			huntText.Hide(true);
			huntTitelext.Hide(true);
//			recLevel.Hide(true);
		}
	}
	
	void IsShowCoolDownBtn(bool isShow) {
		if(isShow) {
			coolDownBtn.Hide(false);
			coolDownTime.Hide(false);
			coolDownTitelext.Hide(false);
			finishNow.Hide(false);
			coolDownIcon.Hide(false);
			coolDownBg.Hide(false);
			coolDownTitle.Hide(false);
			//hide description
			HideDescription(true);
		}else {
			coolDownBtn.Hide(true);
			coolDownTime.Hide(true);
			coolDownTitelext.Hide(true);
			finishNow.Hide(true);
			coolDownIcon.Hide(true);
			coolDownBg.Hide(true);
			coolDownTitle.Hide(true);
			//hide description
			HideDescription(false);
		}
	}
	
	void IsShowLockBtn(bool isShow) {
		if(isShow) {
			lockBtn.Hide(false);
			lockText.Hide(false);
			lockTitelext.Hide(false);
		}else {
			lockBtn.Hide(true);
			lockText.Hide(true);
			lockTitelext.Hide(true);
		}
	}
	
	public float fcoolDownTime = 0f;
	public bool  isUpdateCoolDownTime = true;
	private void CalcCoolDownTime(){
		string strCdt = "";
		float tempTime = Time.time - MissionPanel.Instance.receiveServerMsgTime;
		tempTime = (fcoolDownTime - tempTime);
		if(tempTime < 0.1f){
			isUpdateCoolDownTime = false;
			coolDownTime.Text = "";
			coolDownInterTime.Text = "00:00:00";
		}else{
			strCdt = ((int)tempTime/3600).ToString()+"h "+((int)tempTime/60%60).ToString()+"m "+((int)tempTime%60).ToString()+"s";	
			coolDownTime.Text = strCdt;
			coolDownInterTime.Text = strCdt;
			CheckShowCoolDownBtnState();
		}
	}
	
	
#endregion
}
