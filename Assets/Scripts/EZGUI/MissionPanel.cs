using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class MissionPanel : MonoBehaviour {
	
	//Instance
	public static MissionPanel Instance = null;
	public int currentMissionID = 0;
	public string currentMissionName = "";
	
	private const int MaxMapCount 			= 1;
	private const int MaxRegionCount		= 9;
	private const int MaxLevelCount 		= 4;
	
	public string[,] RegionName = new string[MaxMapCount, MaxRegionCount];
	public string[,] LevelCap = new string[MaxMapCount, MaxRegionCount];
	public string[, ,] LevelName = new string[MaxMapCount, MaxRegionCount, MaxLevelCount];
	public string[,,] MapInfoDescription = new string[MaxMapCount, MaxRegionCount,4];
	public string[,] RegionDescription = new string[MaxMapCount, MaxRegionCount];
	//0: mission desxription 1:suc 2:recommended level 3:cool down 4:enemy info  5:enemy icon 6:mat info 7:mat icon 8:boss info
	public string[, , ,] LevelDescription = new string[MaxMapCount, MaxRegionCount, MaxLevelCount, 9];
	
	public Transform MissionSound;
	
	public float receiveServerMsgTime = 0f;
	
	public  List<SAcceptMissionRelate2> missionInfoList = new List<SAcceptMissionRelate2>(); 
	public  List<MissionArea> missionList = new List<MissionArea>(); 
	
	public UIButton exitBtn;
	
	public int openedMissions ;
	
	void Awake() {
		Instance = this;
	}
	
	// Use this for initialization
	void Start () {
		exitBtn.AddInputDelegate(ExitDelegate);
		InitMapDescription();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
#region InterFace
	public void AwakeMissionMap() {
		CS_Main.Instance.g_commModule.SendMessage(
			   		ProtocolGame_SendRequest.MissionListReq()
												 );
		InitImage();
	}
	
	public void LeaveMissionMap() {
		MissionSelect.Instance.LeaveSelectMssion();
		transform.GetComponent<UIPanel>().Dismiss();
	}
	
	public void InitMissionMapInfo() {
		
	}
	
	public int GetCurrentMapID() {
		return 0; //now only one map.
	}
	
	public int GetCurrentRegionID() {
		return (currentMissionID/100%10-1);
	}
	
	public int GetCurrentMissionID() {
		return (currentMissionID/10%10-1);
	}
	
	public int GetCurrentLevelID() {
		return (currentMissionID%10-1);
	}
	
	public UIPanel mapBgPanel;
	public void InitMissionMap() {
		_UI_CS_ScreenCtrl.Instance.SetNextScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_BOUNTY_MASTER);
		MouseCtrl.Instance.SetMouseStats(MouseIconType.PALM);
		MoneyBadgeInfo.Instance.Hide(false);
		_UI_CS_FightScreen.Instance.m_CS_Ingame_NormalPanel.Dismiss();
		receiveServerMsgTime = Time.time;
//		RestArea();
		InitArea();
		transform.GetComponent<UIPanel>().BringIn();
		mapBgPanel.BringIn();
	}
	
	public void ResetCoolTime(int missionID, int type) {
		for(int i =0;i<missionList.Count;i++) {
			if((missionList[i].areaID/10*10) == (missionID/10*10)) {
				int tempTime = 0;
				switch(type) {
				case EDecreaseCooldownType.eDecreaseCooldownType_All:
					tempTime = -99999;
					break;
				case EDecreaseCooldownType.eDecreaseCooldownType_30:
					tempTime = (-30*60);
					break;
				case EDecreaseCooldownType.eDecreaseCooldownType_60:
					tempTime = (-60*60);
					break;
				}
				missionList[i].AddCoolDownTime(tempTime);
				return;
			}
		}
		LogManager.Log_Error("ResetCoolTime no mission.");
	}
	
	public void LoadImg() {
		TextureDownLoadingMan.Instance.DownLoadingTexture("WorldMap",worldMapBg.transform);
		TextureDownLoadingMan.Instance.DownLoadingTexture("Linshi",MissionSelect.Instance.buyItemBg1.transform);
	}
#endregion
	
#region Local
	private void InitMapDescription() {
		string fileName = "MissionDescription.Description";
		int    tempID = 0;
		int    mapIdx = 0;
		int    regionIdx = 0;
		int    levelIdx = 0;
		string _fileName = LocalizeManage.Instance.GetLangPath(fileName);
		TextAsset item = (TextAsset)Resources.Load(_fileName, typeof(TextAsset));
		string[] itemRowsList = item.text.Split('\n');
		for (int i = 3; i < itemRowsList.Length - 1; ++i){
			string pp = itemRowsList[i];	
		   	string[] vals = pp.Split(new char[] { '	', '	' });	
			tempID = int.Parse(vals[0]);	
			if(6000 == tempID){
				continue;
			}	
			mapIdx = 0;
			regionIdx = (tempID % 1000) / 100 -1;
			levelIdx = tempID % 100 / 10 - 1;
			if(0 == levelIdx){
				RegionName[mapIdx, regionIdx] = vals[1];
				LevelCap[mapIdx, regionIdx] = vals[10];
				RegionDescription[mapIdx, regionIdx] = vals[2];
				MapInfoDescription[mapIdx, regionIdx,0] = vals[3];
				MapInfoDescription[mapIdx, regionIdx,1] = vals[4];
				MapInfoDescription[mapIdx, regionIdx,2] = vals[5];
				MapInfoDescription[mapIdx, regionIdx,3] = vals[6];
			}
			LevelName[mapIdx, regionIdx, levelIdx] = vals[7];
			LevelDescription[mapIdx, regionIdx, levelIdx, 0] = vals[8];
			LevelDescription[mapIdx, regionIdx, levelIdx, 1] = vals[9];
			LevelDescription[mapIdx, regionIdx, levelIdx, 2] = vals[3];
			LevelDescription[mapIdx, regionIdx, levelIdx, 3] = vals[11];
			LevelDescription[mapIdx, regionIdx, levelIdx, 4] = vals[12];
			LevelDescription[mapIdx, regionIdx, levelIdx, 5] = vals[13];
			LevelDescription[mapIdx, regionIdx, levelIdx, 6] = vals[14];
			LevelDescription[mapIdx, regionIdx, levelIdx, 7] = vals[15];
			LevelDescription[mapIdx, regionIdx, levelIdx, 8] = vals[16];
			
		}
		LogManager.Log_Info("InitMapDescription Ok");
	}
	
	public void InitAreaIcon() {
		for(int i = 0;i<missionList.Count;i++) {
			TextureDownLoadingMan.Instance.DownLoadingTexture(missionList[i].bundleName,missionList[i].icon.transform);
		}
	}
	
	public UIButton worldMapBg;
	private void InitImage(){
//		TextureDownLoadingMan.Instance.DownLoadingTexture("Figure_use6",MissionSelect.Instance.npc.transform);
		TextureDownLoadingMan.Instance.DownLoadingTexture("WorldMap",worldMapBg.transform);
		TextureDownLoadingMan.Instance.DownLoadingTexture("Play",MissionSelect.Instance.mianBtnBg.transform);
		TextureDownLoadingMan.Instance.DownLoadingTexture("Pattern",MissionSelect.Instance.mainInfobg.transform);
		TextureDownLoadingMan.Instance.DownLoadingTexture("Linshi",MissionSelect.Instance.buyItemBg1.transform);
		TextureDownLoadingMan.Instance.DownLoadingTexture("Linshi",MissionSelect.Instance.buyItemBg2.transform);
		TextureDownLoadingMan.Instance.DownLoadingTexture("Cool_1",MissionSelect.Instance.finishEleBg1.transform);
		InitAreaIcon();
	}
	
	private void RestArea() {
		for(int i = 0;i <missionList.Count;i++) {
			missionList[i].InitAreaInfo(true,null);
		}
//		isHideArea = false;
	}
	
//	public bool isHideArea = false;
	private void InitArea() {
//		isHideArea = false;
		for(int i = 0;i <missionList.Count;i++) {
			CheckActiveArea(missionList[i]);
		}
		
	}
	
	public UIPanel fbPanel;
	private void CheckActiveArea(MissionArea info) {
		for(int i = 0;i <missionInfoList.Count;i++) {
			if(info.areaID == missionInfoList[i].missionID) {
				CheckActiveArea(missionInfoList[i]);
				//---------------------------------------------------------------------------------->>mm
				openedMissions = i ;
				//Debug.Log (" a Mission in The Pocket");
				//---------------------------------------------------------------------------------->>#mm
				return;
			}
		}
		//---------------------------------------------------------------------------------->>mm
		if (GameObject.Find("UI manager").gameObject.GetComponent<isFacebookBuild>().isFacebook == true){
			Debug.Log ("***************"+openedMissions+"***************");
			// if the player unlocked 5100
			if (isFacebookBuild.NeedToPay == true){	
				if (openedMissions <= 2){ //>= 4  //test with <= 2
					//it is a facebook version, and the player have finished the demo
					GameObject.Find("facebook panel").gameObject.GetComponent<UIPanel>().BringIn();
					fbPanel.BringIn();
				}else{
					//nothing, the plyer still did not finished the demo
				}
			}
		}else{
			// it is not a facebook version anyway
		}
		//---------------------------------------------------------------------------------->>#mm
		info.InitAreaInfo(true,null);
		
	}
	
	private void CheckActiveArea(SAcceptMissionRelate2 info) {
		for(int i = 0;i <missionList.Count;i++) {
			if(missionList[i].areaID == info.missionID) {
				missionList[i].InitAreaInfo(false,info);
				return;
			}
		}
	}
	
	void ExitDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:
				MoneyBadgeInfo.Instance.Hide(false);
				LeaveMissionMap();
				_UI_CS_ScreenCtrl.Instance.SetNextScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_INGAME_NORMAL);
				MouseCtrl.Instance.SetMouseStats(MouseIconType.SWARD1);
				_UI_CS_FightScreen.Instance.m_CS_Ingame_NormalPanel.BringIn();
				Player.Instance.ReactivePlayer();
                GameCamera.BackToPlayerCamera();
				BGManager.Instance.ExitOutsideAudio();
				break;
		}	
	}
#endregion

}