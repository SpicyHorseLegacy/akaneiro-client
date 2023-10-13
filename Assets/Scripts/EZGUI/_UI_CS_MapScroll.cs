using UnityEngine;
using System.Collections;

//main function no need.
public class _UI_CS_MapScroll : MonoBehaviour {
	
	public static _UI_CS_MapScroll  Instance;
	public bool 					IsExistMission;
	
	void Awake(){
		Instance = this;
	}
	
	// Use this for initialization
	void Start () {
		IsExistMission = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void EnterMap(string mapName){
		DismissPanel();
	}
	
	void DismissPanel(){
		if(IsExistMission){
			_UI_CS_FightScreen.Instance.m_fightPanel.BringIn();
			_UI_CS_MissionLogic.Instance.MissionBgPanel.BringIn();
		}else{
			MoneyBadgeInfo.Instance.Hide(false);
		}
		//任务完成界面移到DailyReward之后.
//		if(_UI_CS_MissionSummary.Instance.isCompleteMission){
//			_UI_CS_MissionSummary.Instance.isCompleteMission = false;
//			_UI_CS_MissionSummary.Instance.AwakeCompleteSummary();
//		}
	}
	
	public string SceneNameToMapName(string sceneName){
		//to do: update state icon pos;
		PlayerInfoBar.Instance.UpdateAllyPos();
		OptionCtrl.Instance.BackToVillageBtn.gameObject.layer = LayerMask.NameToLayer("EZGUI");
		OptionCtrl.Instance.BackToVillageBtn.spriteText.gameObject.layer = LayerMask.NameToLayer("EZGUI");
		if(0 == string.Compare(sceneName,"Hub_Village")){
			IsExistMission = false;
			OptionCtrl.Instance.BackToVillageBtn.gameObject.layer = LayerMask.NameToLayer("Default");
			OptionCtrl.Instance.BackToVillageBtn.spriteText.gameObject.layer = LayerMask.NameToLayer("Default");
			//village no id ,so hard code.
			return "Welcome, Red Hunter";
		}else if(0 == string.Compare(sceneName,"A1_M1")){
			IsExistMission = true;
			return MissionPanel.Instance.LevelName[0,0,0];
		}else if(0 == string.Compare(sceneName,"A1_M2")){
			IsExistMission = true;
			return MissionPanel.Instance.LevelName[0,0,1];
		}else if(0 == string.Compare(sceneName,"A1_M3")){
			IsExistMission = true;
			return MissionPanel.Instance.LevelName[0,0,2];
		}else if(0 == string.Compare(sceneName,"A1_M4")){
			IsExistMission = true;
			return "Early Winter";
		}else if(0 == string.Compare(sceneName,"A2_M1")){
			IsExistMission = true;
			return MissionPanel.Instance.LevelName[0,1,0];
		}else if(0 == string.Compare(sceneName,"A2_M2")){
			IsExistMission = true;
			return MissionPanel.Instance.LevelName[0,1,1];
		}else if(0 == string.Compare(sceneName,"A2_M3")){
			IsExistMission = true;
			return MissionPanel.Instance.LevelName[0,1,2];
		}else if(0 == string.Compare(sceneName,"A2_M4")){
			IsExistMission = true;
			return "A Yokai in Need...";
		}else if(0 == string.Compare(sceneName,"A3_M1")){
			IsExistMission = true;
			return MissionPanel.Instance.LevelName[0,2,0];
		}else if(0 == string.Compare(sceneName,"A3_M2")){
			IsExistMission = true;
			return MissionPanel.Instance.LevelName[0,2,1];
		}else if(0 == string.Compare(sceneName,"A3_M3")){
			IsExistMission = true;
			return MissionPanel.Instance.LevelName[0,2,2];
		}else if(0 == string.Compare(sceneName,"A3_M4")){
			IsExistMission = true;
			return "The Walking Fruit";
		}else if(0 == string.Compare(sceneName,"A4_M1")){
			IsExistMission = true;
			return MissionPanel.Instance.LevelName[0,3,0];
		}else if(0 == string.Compare(sceneName,"A4_M2")){
			IsExistMission = true;
			return MissionPanel.Instance.LevelName[0,3,1];
		}else if(0 == string.Compare(sceneName,"A4_M3")){
			IsExistMission = true;
			return MissionPanel.Instance.LevelName[0,3,2];
		}else if(0 == string.Compare(sceneName,"A4_M4")){
			IsExistMission = true;
			return "Deep Tremors";
		}else if(0 == string.Compare(sceneName,"A5_M1")){
			IsExistMission = true;
			return MissionPanel.Instance.LevelName[0,4,0];
		}else if(0 == string.Compare(sceneName,"A5_M2")){
			IsExistMission = true;
			return MissionPanel.Instance.LevelName[0,4,1];
		}else if(0 == string.Compare(sceneName,"A5_M3")){
			IsExistMission = true;
			return MissionPanel.Instance.LevelName[0,4,2];
		}else if(0 == string.Compare(sceneName,"A6_M1")){
			IsExistMission = true;
			return MissionPanel.Instance.LevelName[0,5,0];
		}else if(0 == string.Compare(sceneName,"A6_M2")){
			IsExistMission = true;
			return MissionPanel.Instance.LevelName[0,5,1];
		}else if(0 == string.Compare(sceneName,"A6_M3")){
			IsExistMission = true;
			return MissionPanel.Instance.LevelName[0,5,2];
		}else if(0 == string.Compare(sceneName,"A7_M1")){
			IsExistMission = true;
			return MissionPanel.Instance.LevelName[0,6,0];
		}else if(0 == string.Compare(sceneName,"A7_M2")){
			IsExistMission = true;
			return MissionPanel.Instance.LevelName[0,6,1];
		}else if(0 == string.Compare(sceneName,"A7_M3")){
			IsExistMission = true;
			return MissionPanel.Instance.LevelName[0,6,2];
		}else if(0 == string.Compare(sceneName,"A8_M1")){
			IsExistMission = true;
			return MissionPanel.Instance.LevelName[0,7,0];
		}else if(0 == string.Compare(sceneName,"A8_M2")){
			IsExistMission = true;
			return MissionPanel.Instance.LevelName[0,7,1];
		}else if(0 == string.Compare(sceneName,"A8_M3")){
			IsExistMission = true;
			return MissionPanel.Instance.LevelName[0,7,2];
		}else if(0 == string.Compare(sceneName,"TWILIGHT")){
			IsExistMission = true;
			return "Twilight Demo";
		}else if(0 == string.Compare(sceneName,"Hub_Village_Tutorial")){
			IsExistMission = true;
			//to do : tutorial hide summon btn
			PlayerInfoBar.Instance.IsHideSommonAllyBtn(true,0);
			return "Welcome, Red Hunter";
		}else if(0 == string.Compare(sceneName,"EmptyScenes")){
			IsExistMission = false;
			return "Back to Main Menu";
		}else{
			IsExistMission = true;
			return "Secret Mission";
		}
	}	
}
