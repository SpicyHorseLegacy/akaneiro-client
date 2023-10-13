using UnityEngine;
using System.Collections;

public class MissionArea : MonoBehaviour {
	
	public UIButton 	timeIcon;
	public SpriteText 	coolDownTime;
	public UIButton 	starsBg;
	public UIButton [] 	stars;
	public UIButton 	icon;
	public UIButton 	timeBg;
	public bool			isLockArea = true;
	public Transform    NewMissionVFX;
	public int areaID = 0;
	public string bundleName;
	public bool isBossMission =false;
	
	// Use this for initialization
	void Start () {
		icon.AddInputDelegate(IconDelegate);
	}
	
	// Update is called once per frame
	void Update () {
		if(_UI_CS_ScreenCtrl.Instance.IsScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_BOUNTY_MASTER)){
			if(isUpdateCoolDownTime) {
				CalcCoolDownTime();
			}
		}
	}
	
#region interface
	private Color unlockAreaColor = new Color(0.235f,0.235f,0.235f,1);
	private float fcoolDownTime = 0f;
	private bool  isUpdateCoolDownTime = false;
	public SAcceptMissionRelate2 areaInfo;
	public void InitAreaInfo(bool isLock,SAcceptMissionRelate2 info) {
		areaInfo = info;
		HideArea(false);
		if(info != null) {
			fcoolDownTime = info.coolDownTime;
			SetStars(info.star);
		}
		isLockArea = isLock;
		isShowNewMissionVFX(false);
		if(isLock) {
			HideArea(true);
			icon.controlIsEnabled = false;
		}else {
			icon.SetColor(Color.white);
			isUpdateCoolDownTime = true;
			timeIcon.gameObject.layer = LayerMask.NameToLayer("EZGUI_CanTouch");
			CalcCoolDownTime();
			if(CheckIsNewMission()) {
				isShowNewMissionVFX(true);
			}else {
				if(isBossMission) {
					isShowNewMissionVFX(true);
				}
			}
			icon.controlIsEnabled = true;
		}
	} 
	
	public void AddCoolDownTime(int min) {
		fcoolDownTime +=  min;
		MissionSelect.Instance.fcoolDownTime += min;
		if(fcoolDownTime < 0) {
			fcoolDownTime = 0;
		}
		if(MissionSelect.Instance.fcoolDownTime < 0) {
			MissionSelect.Instance.fcoolDownTime = 0;
			MissionSelect.Instance.isUpdateCoolDownTime = false;
		}
		areaInfo.coolDownTime = (int)fcoolDownTime;
		MissionSelect.Instance.ChooseLevelThreat(1);
	}
#endregion
	
#region local
	private const int straCount = 4;
	public  bool []  isHideStar;
	private void IsHideStars(bool hide) {
		if(hide) {
			starsBg.gameObject.layer = LayerMask.NameToLayer("Default");
			for(int i = 0;i<straCount;i++) {
				stars[i].gameObject.layer = LayerMask.NameToLayer("Default");
			}
		}else {
			starsBg.gameObject.layer = LayerMask.NameToLayer("EZGUI_CanTouch");
			for(int i = 0;i<straCount;i++) {
				if(isHideStar[i]) {
					stars[i].gameObject.layer = LayerMask.NameToLayer("Default");
				}else {
					stars[i].gameObject.layer = LayerMask.NameToLayer("EZGUI_CanTouch");
				}
			}
		}
	}
	
	private void HideArea(bool isHide) {
		isUpdateCoolDownTime = false;
		icon.Hide(isHide);
		IsHideStars(isHide);
		coolDownTime.Hide(isHide);
		timeBg.Hide(isHide);
		timeIcon.Hide(isHide);
	}
	
	private void SetStars(int stars) {
		for(int i=0;i<straCount;i++ ) {
			isHideStar[i] = true;
		}
		if((stars & 8) == 8) {
			isHideStar[3] = false;
		}
		if((stars & 4) == 4) {
			isHideStar[2] = false;
		}
		if((stars & 2) == 2) {
			isHideStar[1] = false;
		}
		if((stars & 1) == 1) {
			isHideStar[0] = false;
		}
	}
	
	private Transform VFXInstance;
	private bool CheckIsNewMission() {
		for(int i = 0;i<straCount;i++) {
			if(isHideStar[i] == false) {
				return false;
			}
		}
		return true;
	}
	
	private void isShowNewMissionVFX(bool isShow) {
		if(isShow) {
			VFXInstance = UnityEngine.Object.Instantiate(NewMissionVFX)as Transform;
			if(VFXInstance != null){
				VFXInstance.position = new Vector3(icon.transform.position.x,icon.transform.position.y-2.5f,icon.transform.position.z-0.1f);
				VFXInstance.parent = icon.transform;
//				VFXInstance.gameObject.layer = LayerMask.NameToLayer("EZGUI_CanTouch");
			}
		}else {
			if(VFXInstance != null){
				Destroy(VFXInstance.gameObject);
			}
		}
	}
	
	private void CalcCoolDownTime(){
		string strCdt = "";
		float tempTime = Time.time - MissionPanel.Instance.receiveServerMsgTime;
		tempTime = (fcoolDownTime - tempTime);
		if(tempTime < 0.1f){
			isUpdateCoolDownTime = false;
			coolDownTime.Text = "";
			timeBg.Hide(true);
			timeIcon.gameObject.layer = LayerMask.NameToLayer("Default");
			IsHideStars(false);
		}else{
			strCdt = ((int)tempTime/3600).ToString()+"h "+((int)tempTime/60%60).ToString()+"m "+((int)tempTime%60).ToString()+"s";	
			coolDownTime.Text = strCdt;
			timeBg.Hide(false);
			IsHideStars(true);
		}
	}
	
	void IconDelegate(ref POINTER_INFO ptr) {
		switch(ptr.evt){
		case POINTER_INFO.INPUT_EVENT.TAP:
				if(!isLockArea) {
					MissionPanel.Instance.currentMissionID = areaID;
					MissionSelect.Instance.ChooseLevelThreat(1);
					//Select mission screen bring.
					MissionSelect.Instance.AwakeSelectMission(areaInfo);
					MissionSelect.Instance.ChooseLevelThreat(1);
					TextureDownLoadingMan.Instance.DownLoadingTexture(bundleName,MissionSelect.Instance.missionIcon.transform);
					TextureDownLoadingMan.Instance.DownLoadingTexture(bundleName,MissionSelect.Instance.missionFinishIcon.transform);
				}
			break;
		case POINTER_INFO.INPUT_EVENT.MOVE:
				MouseCtrl.Instance.SetMouseStats(MouseIconType.SWARD1);
			break;
		case POINTER_INFO.INPUT_EVENT.RELEASE_OFF:	
		case POINTER_INFO.INPUT_EVENT.MOVE_OFF:
				MouseCtrl.Instance.SetMouseStats(MouseIconType.PALM);
			break;
		}	
	}
#endregion
	
}
