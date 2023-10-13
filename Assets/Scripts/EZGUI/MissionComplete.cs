using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class MissionComplete : MonoBehaviour {
	
	//Instance
	public static MissionComplete Instance = null;
	
	public UIPanel basePanel;
	
	public List<ItemDropStruct> materialsList = new List<ItemDropStruct>();
	
	public  UIListItemContainer  	itemContainer;
	public  UIScrollList			list;
	
	public UIButton thanksBtn;
	
	void Awake(){
		Instance = this;
	}
	
	// Use this for initialization
	void Start () {
		 thanksBtn.AddInputDelegate(ThanksBtnDelegate);	
	}
	
	// Update is called once per frame
	void Update () {
		CalcLevelBar();
	}
	
#region Interface
	public SpriteText missionNameText;
	// init info
	public void InitMissionCompleteInfo(string missionName,int threat) {
		missionNameText.Text = missionName;
		UpdateThreatIcon(threat);
	}
	
	public SpriteText ReviveInfoText;
	public SpriteText ReviveValText;
	public void SetReviveVal() {
		if(RevivePanel.Instance.GetRevivalCount() == 0) {
			ReviveInfoText.Hide(true);
			ReviveValText.Hide(true);
		}else {
			ReviveInfoText.Hide(false);
			ReviveValText.Hide(false);
			ReviveValText.Text = RevivePanel.Instance.GetPunismentReward(RevivePanel.Instance.GetRevivalCount()-1).ToString();
		}
	}
	
	//popup mission success menu
	 
	public void AwakeMissionComplete() {
		_UI_CS_ScreenCtrl.Instance.SetNextScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_MISSION_SUMMARY);
		_UI_CS_FightScreen.Instance.m_CS_Ingame_NormalPanel.Dismiss();
		_UI_CS_FightScreen.Instance.m_fightPanel.Dismiss();
		_UI_MiniMap.Instance.isShowBigMap = false;
		_UI_MiniMap.Instance.isShowSmallMap = false;
		MoneyBadgeInfo.Instance.Hide(false);
        GameCamera.EnterMissionCompleteState();
		InitImg();
		calcLevelBar = true;
		//setting in game score.//
		UpdateIngameBound(_UI_CS_MissionLogic.Instance.MissionScore,_UI_CS_MissionLogic.Instance.MissionKarma);
		totalExperienceMax = (int.Parse(threatBoundExpText.text) + int.Parse(ingameBoundExpText.text));
		UpdateMatListForPunismentReward();
		InitLevelBarInfo();
		basePanel.BringIn();
		SetReviveVal();
		PlayTelInVFX();
		PlayMissionSound();
	}
	
	public void ResetMaterialList() {
		materialsList.Clear();
		#region mission complete list
		list.ClearList(true);
		#endregion
	}
	
	public SpriteText threatBoundExpText;
	public SpriteText threatBoundKaramText;
	public void UpdateThreatBound(int exp,int karma) {
		threatBoundExpText.Text = exp.ToString();
		threatBoundKaramText.Text = karma.ToString();
	}
	
	public SpriteText ingameBoundExpText;
	public SpriteText ingameBoundKaramText;
	public void UpdateIngameBound(int exp,int karma) {
		int rc = RevivePanel.Instance.GetRevivalCount();
		if(rc != 0){
			int pr = RevivePanel.Instance.GetPunismentReward(rc-1);
			exp = exp*pr/100;
			karma = karma*pr/100;
		}
		ingameBoundExpText.Text = exp.ToString();
		ingameBoundKaramText.Text = karma.ToString();
	}
	
	//add mat 
	public void AddMaterialItem(ItemDropStruct mat) {
		materialsList.Add(mat);
		#region mission mat list
		AddMatToMissionComplete(mat);
		#endregion
	}
	
	public void AddMatToMissionComplete(ItemDropStruct mat) {
		//check this item is already.
		for(int i = 0;i<list.Count;i++) {
			if(list.GetItem(i).gameObject.GetComponent<Materials>().info._ItemID == mat._ItemID) {
				list.GetItem(i).gameObject.GetComponent<Materials>().count++;
				list.GetItem(i).gameObject.GetComponent<Materials>().countText.Text = list.GetItem(i).gameObject.GetComponent<Materials>().count.ToString();
				return;
			}
		}
		// new material.//
		UIListItemContainer item = (UIListItemContainer)list.CreateItem((GameObject)itemContainer.gameObject);
		list.clipContents = true; list.clipWhenMoving = true;
		item.GetComponent<Materials>().info = mat;
		item.GetComponent<Materials>().count = 1;
		item.GetComponent<Materials>().countText.Text = "1";
		ItemPrefabs.Instance.GetItemIcon(mat._ItemID,mat._TypeID,mat._PrefabID,item.GetComponent<Materials>().icon);
//		LogManager.Log_Error("list count:"+list.Count);
	}
	
	// this is reset mat list
	public void UpdateMatListForMissionComplete(ItemDropStruct mat,int count) {
		UIListItemContainer item = (UIListItemContainer)list.CreateItem((GameObject)itemContainer.gameObject);
		list.clipContents = true; list.clipWhenMoving = true;
		item.GetComponent<Materials>().info = mat;
		item.GetComponent<Materials>().count = count;
		item.GetComponent<Materials>().countText.Text = count.ToString();
		ItemPrefabs.Instance.GetItemIcon(mat._ItemID,mat._TypeID,mat._PrefabID,item.GetComponent<Materials>().icon);
	}
	
	//mission complete calc mat 
	private void UpdateMatListForPunismentReward() {
//		LogManager.Log_Error("list count:"+list.Count);
		int rc = RevivePanel.Instance.GetRevivalCount();
		int pr = 0;
		if(rc != 0){
			pr = RevivePanel.Instance.GetPunismentReward(rc-1);
		}else {
			pr = 100;
		}
//		LogManager.Log_Error("GetPunismentReward:"+pr);
		for(int i = list.Count-1;i>=0;i--) {
//			LogManager.Log_Error("idx:"+i);
//			LogManager.Log_Error("count sour:"+list.GetItem(i).gameObject.GetComponent<Materials>().count);
			int temp = list.GetItem(i).gameObject.GetComponent<Materials>().count*pr;
//			LogManager.Log_Error("step 1:"+temp);
			temp+= 50;
			temp = (temp/100);
//			LogManager.Log_Error("step 2:"+temp);
			list.GetItem(i).gameObject.GetComponent<Materials>().count = temp;
			if(temp >0) {
				list.GetItem(i).gameObject.GetComponent<Materials>().countText.Text = temp.ToString();
//				LogManager.Log_Error("setting item: "+list.GetItem(i).gameObject.GetComponent<Materials>().countText.text);
			}else {
				list.RemoveItem(i,true);
//				LogManager.Log_Error("remove item");
			}
		}
//		LogManager.Log_Error("end it");
	}
	
	public bool isCompleteMission = false;
	public void CheckIsMissionComplete(){
		//查看剩余点数分配 任务完成不请求 1.任务完成完结会请求 2.会冲突对于任务完成界面.//
		if(isCompleteMission){
			isCompleteMission = false;
			AwakeMissionComplete();
		}else{
			CS_Main.Instance.g_commModule.SendMessage(
		   		ProtocolGame_SendRequest.hasAssignedTalentPointReq()
			);
		}
	}
	
	public void CloseMissionComplete(){
		basePanel.Dismiss();
		MoneyBadgeInfo.Instance.Hide(false);
	}
	
	public Transform telInVFX;
	public void PlayTelInVFX() {
		Transform telInstance = UnityEngine.Object.Instantiate(telInVFX)as Transform;
		if(telInstance != null&&Player.Instance != null){
			telInstance.position = Player.Instance.transform.position;
		}
	}
#endregion
	
#region Local
	[SerializeField]
	private Transform MissionCompleteSoundPrefab;
	private void PlayMissionSound() {
		if (MissionCompleteSoundPrefab && BGManager.Instance) {
			BGManager.Instance.PlayOutsideBGM(MissionCompleteSoundPrefab,0,2);
		}
		if(PointSoundPrefab) {
			_pointLoopSound = SoundCue.PlayInstance(PointSoundPrefab.gameObject);
		}
	}
	
	public UIButton [] threatIcons;
	//update threat icon
	private void UpdateThreatIcon(int threatLevel) {
		for(int i = 0;i<4;i++) {
			threatIcons[i].Hide(true);
		}
		threatIcons[threatLevel].Hide(false);
	}
	
	public UIButton titleImg;
	private void InitImg() {
		TextureDownLoadingMan.Instance.DownLoadingTexture("LevelUP_P_LevleUP",titleImg.transform);
	}
	
	void ThanksBtnDelegate(ref POINTER_INFO ptr) {
		switch(ptr.evt) {
		   case POINTER_INFO.INPUT_EVENT.TAP:
				_UI_CS_MissionLogic.Instance.RsetMissionScore();	
				CloseMissionComplete();
//				if(_UI_CS_LevelUp.Instance.IsLevelUp){
				if(levelUp.Instance.isLevelUp){
//					_UI_CS_LevelUp.Instance.IsLevelUp = false;
					levelUp.Instance.isLevelUp = false;
					CS_Main.Instance.g_commModule.SendMessage(
				   		ProtocolGame_SendRequest.hasAssignedTalentPointReq()
					);
				}else{
                    if (BGManager.Instance) {
                        BGMInfo.AutoPlayBGM = true;
                        BGManager.Instance.PlayOriginalBG(2);
                        BGMInfo.AutoPlayBGM = false;
                    }
					Time.timeScale = 1;	
					_UI_CS_ScreenCtrl.Instance.SetNextScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_INGAME_NORMAL);
					_UI_CS_FightScreen.Instance.m_CS_Ingame_NormalPanel.BringIn();
					_UI_MiniMap.Instance.isShowSmallMap = true;
                    GameCamera.EnterNomalState();
                    //Player.Instance.ReactivePlayer();
				}
				GameCamera.BackToPlayerCamera();
				break;
		}	
	}
	
	private bool calcLevelBar = false;
	private bool isCalcNextLv = false;
	public UIProgressBar levelBar;
	public UIProgressBar levelEffBar;
	public SpriteText    levelValText;
	private float 		 incrementLevel  = 0.01f;
	[SerializeField]
	private Transform PointSoundPrefab;
	private Transform _pointLoopSound;
	[SerializeField]
	private Transform EndSoundPrefab;
	private void CalcLevelBar() {
		if(calcLevelBar){
			levelBar.Value += incrementLevel;
			if(levelBar.Value >= levelEffBar.Value){
				if(isCalcNextLv){
					InitLevelBarInfo2();
				}else{
//					LogManager.Log_Error("calcLevelBar = false");
					calcLevelBar = false;
					levelBar.Value = levelEffBar.Value;
					if(_pointLoopSound) {
						SoundCue.StopAndDestroyInstance(_pointLoopSound.gameObject); }
					if(EndSoundPrefab) {
						SoundCue.PlayPrefabAndDestroy(EndSoundPrefab); }
				}
			}
			levelValText.Text = _PlayerData.Instance.playerLevel.ToString();
		}
	}
	
	public int totalExperienceMax;
	private void InitLevelBarInfo() {
//		LogManager.Log_Error("InitLevelBarInfo");
		long tcurexp = _PlayerData.Instance.readCurExpVal(_PlayerData.Instance.playerLevel);
		if(totalExperienceMax >= (_PlayerData.Instance.playerCurrentExp-tcurexp)){
			// alrd lv up so
			isCalcNextLv = true;
			long curExp = 0;
			long maxExp = _PlayerData.Instance.ReadMaxExpVal(_PlayerData.Instance.playerLevel-1);
			curExp = maxExp - (totalExperienceMax - (_PlayerData.Instance.playerCurrentExp- tcurexp)) ;
			if(0 > curExp){
				curExp = 0;
			}
			tcurexp = _PlayerData.Instance.readCurExpVal(_PlayerData.Instance.playerLevel-1);
			curExp = (curExp-tcurexp);
			if(curExp < 0) {
				curExp = 0;
			}
			levelBar.Value = (float)curExp/(float)(maxExp-tcurexp);
			levelEffBar.Value = 1;
			levelValText.Text = (_PlayerData.Instance.playerLevel-1).ToString();
		}else{
			isCalcNextLv = false;
			long curExp = _PlayerData.Instance.playerCurrentExp - totalExperienceMax;
			if(curExp < 0) {
				curExp = 0;
			}
			long maxExp = _PlayerData.Instance.ReadMaxExpVal(_PlayerData.Instance.playerLevel);
			levelBar.Value = (float)curExp/(float)(maxExp-tcurexp);
			levelEffBar.Value = (float)(_PlayerData.Instance.playerCurrentExp-tcurexp)/(float)(maxExp-tcurexp);
			levelValText.Text = _PlayerData.Instance.playerLevel.ToString();
		}
	}
	
	public void InitLevelBarInfo2() {
//			LogManager.Log_Error("InitLevelBarInfo2");
			isCalcNextLv = false;
			long curExp = 0;
			long maxExp = _PlayerData.Instance.ReadMaxExpVal(_PlayerData.Instance.playerLevel);
			long tcurexp = _PlayerData.Instance.readCurExpVal(_PlayerData.Instance.playerLevel);
			levelBar.Value = (float)curExp/(float)maxExp;
			levelEffBar.Value = (float)(_PlayerData.Instance.playerCurrentExp-tcurexp)/(float)(maxExp-tcurexp);
			levelValText.Text = _PlayerData.Instance.playerLevel.ToString();
	}
#endregion
	
}
