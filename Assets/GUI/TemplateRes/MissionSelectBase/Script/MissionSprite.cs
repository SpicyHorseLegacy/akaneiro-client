using UnityEngine;
using System.Collections;

public class MissionSprite : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		UpdateTime();
	}
	
	#region Interface
	private MissionSelectData data;
	public void SetMissionSprite(MissionSelectData data){
		this.data =data;
		mTimeTemp = long.Parse(MissionWindowManager.GetTimeStamp()) - data.lastTime;
		if(mTimeTemp>data.coolDownTime){
			data.isCooldown = true;
		}else {
			data.isCooldown = false;
			SetTime(data.coolDownTime - mTimeTemp);
		}
		SetCoolDownBar(!data.isCooldown);
		SetStarBar(data.isCooldown,data.difficultyData);
	}
	#endregion
	
	#region Local
	
	[SerializeField]
	private Transform coolDownBar;
	private void SetCoolDownBar(bool isActive){
		NGUITools.SetActive(coolDownBar.gameObject,isActive);
	}
	
	[SerializeField]
	private Transform starBar;
	
	[SerializeField]
	private Transform [] stars;
	private void SetStarBar(bool isActive,MissionDifficultyData [] difficultyData){
		NGUITools.SetActive(starBar.gameObject,isActive);
		if(starBar){
			for(int i=0;i<(int)MissionLevels.Max;i++){
				NGUITools.SetActive(stars[i].gameObject,difficultyData[i].isPass);
			}
		}
	}
	
	[SerializeField]
	private UILabel cooldownTime;
	private void SetTime(long time){
		cooldownTime.text = string.Format("{0:G}h {1:G}m {2:G}s",time/3600,(time-(time/3600)*3600)/60,time%60);
	}
	
	private long mTimeTemp;
	private void UpdateTime(){
		if(data==null){
			return;
		}
		if(data.isCooldown){
			return;
		}
		if(data.coolDownTime - mTimeTemp<=0){
			data.isCooldown = true;
			SetCoolDownBar(!data.isCooldown);
			SetStarBar(data.isCooldown,data.difficultyData);
			return;
		}
		mTimeTemp = long.Parse(MissionWindowManager.GetTimeStamp()) - data.lastTime;;
		SetTime(data.coolDownTime - mTimeTemp);
	}
	
	[SerializeField]
	private Transform missionWindow;
	
	private void OnMissionBtnDegelate(){
		missionWindow.GetComponent<MissionWindowManager>().SetMissionInfo(data);
		missionWindow.position = new Vector3(0,0,-1f);
	}
	#endregion
}
