using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class speedUpCost{
	public int time;
	public int crystal;
	public int sub30Min;
	public int sub60Min;
}
public class SpeedUpManager : MonoBehaviour {

	// Use this for initialization
	void Start () {
		InitCostCoolDown();
	}
	
	// Update is called once per frame
	void Update () {
		UpdateTime();
	}
	
	#region Interface
	private MissionSelectData data;
	public void SetMissionInfo(MissionSelectData data){
		this.data = data;
		mTimeTemp = long.Parse(MissionWindowManager.GetTimeStamp()) - data.lastTime;
		if(mTimeTemp>data.coolDownTime){
			data.isCooldown = true;
		}
		else{
			data.isCooldown = false;
			SetTime(data.coolDownTime - mTimeTemp);
		}
		SetMissionIcon(data.materialIcon);
	}
	
	
	
	#region Delegate
	public delegate void Handle_FinishBtnDelegate();
    public event Handle_FinishBtnDelegate OnFinishBtnDelegate;
	private void FinishBtnDelegate() {
		if(OnFinishBtnDelegate != null) {
			OnFinishBtnDelegate();
		}
	}
	
	public delegate void Handle_SpeedUpOneHourBtnDelegate();
    public event Handle_SpeedUpOneHourBtnDelegate OnSpeedUpOneHourBtnDelegate;
	private void SpeedUpOneHourBtnDelegate() {
		if(OnSpeedUpOneHourBtnDelegate != null) {
			OnSpeedUpOneHourBtnDelegate();
		}
	}
	
	public delegate void Handle_SpeedUpHalfHourBtnDelegate();
    public event Handle_SpeedUpHalfHourBtnDelegate OnSpeedUpHalfHourBtnDelegate;
	private void SpeedUpHalfHourBtnDelegate() {
		if(OnSpeedUpHalfHourBtnDelegate != null) {
			OnSpeedUpHalfHourBtnDelegate();
		}
	}
	#endregion
	#endregion
	
	#region Local
	[SerializeField]
	private UILabel finishValue;
	private void SetSinishValue(int val){
		finishValue.text = val.ToString();
	}
	private int GetFinishValue(){
		return int.Parse(finishValue.text);
	}
	
	
	[SerializeField]
	private UILabel halfHourValue;
	private void SetHalfHourValue(int val){
		halfHourValue.text = val.ToString();
	}
	private int GetHalfHourValue(){
		return int.Parse(halfHourValue.text);
	}
	
	[SerializeField]
	private UILabel oneHourValue;
	private void SetOneHourValue(int val){
		oneHourValue.text = val.ToString();
	}
	private int GetOneHourValue(){
		return int.Parse(oneHourValue.text);
	}
	
	[SerializeField]
	private UISprite missionIcon;
	private void SetMissionIcon(string name){
		missionIcon.spriteName = name;
	}
	
	[SerializeField]
	private UILabel cooldownTime; 
	private void SetTime(long time){
		cooldownTime.text = string.Format("{0:G}h {1:G}m {2:G}s",time/3600,(time-(time/3600)*3600)/60,time%60);
	}
	
	private long mTimeTemp;
	private void UpdateTime(){
		if(!NGUITools.GetActive(gameObject)){
			return;
		}
		if(data==null){
			return;
		}
		if(data.isCooldown){
			return;
		}
		if(data.coolDownTime - mTimeTemp<=0){
			data.isCooldown = true;
			return;
		}
		mTimeTemp = long.Parse(MissionWindowManager.GetTimeStamp()) - data.lastTime;
		SetTime(data.coolDownTime - mTimeTemp);
		CheckShowCoolDownBtnState(data.coolDownTime - mTimeTemp);
	}
	
	private List<speedUpCost> speedUpCostList = new List<speedUpCost>(); 
	//=============
	void InitCostCoolDown() {
		speedUpCostList.Clear();
		string _fileName = LocalizeManage.Instance.GetLangPath("MissionsThreatCycle.CoolDown");
		TextAsset item = (TextAsset)Resources.Load(_fileName, typeof(TextAsset));
		string[] itemRowsList = item.text.Split('\n');
		for (int i = 3; i < itemRowsList.Length - 1; ++i){
			speedUpCost temp = new speedUpCost();
			string pp = itemRowsList[i];
			string[] vals = pp.Split(new char[] { '	', '	' });
			temp.time		= int.Parse(vals[0]);		
			temp.crystal 	= int.Parse(vals[1]);	
			temp.sub30Min 	= int.Parse(vals[2]);
			temp.sub60Min 	= int.Parse(vals[3]);
			speedUpCostList.Add(temp);
		}
	}
	
	void CheckShowCoolDownBtnState(float time) {
		int imin = (int)time/60;
		int isec = (int)time%60;
		if(isec == 0) {
			isec = 0;
		}else {
			isec = 1;
		}
		for(int i = 0;i<speedUpCostList.Count;i++) {
			if(imin+isec <= speedUpCostList[i].time) {
				if(speedUpCostList[i].crystal == 0) {
					finishValue.text = "FREE";
					halfHourValue.text = "";
					oneHourValue.text = "";
				}else {
					finishValue.text= speedUpCostList[i].crystal.ToString();
					halfHourValue.text = speedUpCostList[i].sub30Min.ToString();
					oneHourValue.text = speedUpCostList[i].sub60Min.ToString();
				}
				return;
			}
		}
	}
	
	#region Delegate
	private void OnCloseDelegate(){
		gameObject.transform.position = new Vector3(999f,999f,-1f);
		data = null;
	}
	#endregion
	#endregion
	
}
