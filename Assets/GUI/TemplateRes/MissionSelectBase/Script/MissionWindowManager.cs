using UnityEngine;
using System.Collections;

public class MissionDifficultyData{
	public bool isLock;
	public bool isPass;
	public int exp;
	public int karma;
	public Color color;
	public string name;
	public string recommendedLv;
	public MissionDifficultyData(bool islock,bool isPass,int exp,int karma,string name,string lv,Color color){
		this.isLock = islock;
		this.isPass = isPass;
		this.exp = exp;
		this.karma = karma;
		this.name = name;
		this.recommendedLv = lv;
		this.color = color;
	}
	public MissionDifficultyData(){
		
	}
}

public class MissionSelectData{
	public bool isCooldown;
	public string name;
	public string info;
	public string bossInfo;
	public string icon;
	public string enemyIcon;
	public string enemyInfo;
	public string materialIcon;
	public string materialInfo;
	public MissionDifficultyData [] difficultyData = new MissionDifficultyData[(int)MissionLevels.Max];
	public string recommendedLv;
	public bool isEnabled;
	public long coolDownTime;
	public long lastTime;
}

public enum MissionLevels{
	Easy = 0,
	Medium,
	Hard,
	Overrun,
	Max,
}

public class MissionWindowManager : MonoBehaviour {
	
	public static MissionWindowManager Instance;
	void Awake(){
		Instance = this;
	}
	// Use this for initialization
	void Start () {
//		test();
	}
	
	// Update is called once per frame
	void Update () {
		UpdateTime();
	}
	
	#region test
	private void test(){
//		data = new MissionSelectData();
//		data.isCooldown = false;
//		data.name = "text";
//		data.info = "sdfsdfsdfsdf";
//		data.bossInfo = "";
//		data.icon = null;
//		data.enemyIcon = null;
//		data.enemyInfo = "sdfsdfsdfsf";
//		data.materialIcon = null;
//		data.materialInfo = "dfsdfsdfsdffff";
		
//		MissionDifficultyData mdata = new MissionDifficultyData(false,true,999,999,"name","9",Color.green);
//		data.difficultyData[(int)MissionLevels.Easy] = mdata;
//		mdata = new MissionDifficultyData(true,false,999,999,"name","10",Color.yellow);
//		data.difficultyData[(int)MissionLevels.Medium] = mdata;
//		mdata = new MissionDifficultyData(true,false,999,999,"name","11",new Color(1.0f,0.5f,0f,1.0f));
//		data.difficultyData[(int)MissionLevels.Hard] = mdata;
//		mdata = new MissionDifficultyData(true,false,999,999,"name","12",Color.red);
//		data.difficultyData[(int)MissionLevels.Overrun] = mdata;

//		data.recommendedLv = "12";
//		data.coolDownTime = 30*60;
//		data.lastTime = 1374689680;
//		SetMissionInfo(data);
	}
	#endregion
	
	
	#region Interface
	private void Init(){
		curSelectLv = MissionLevels.Easy;
	}
	
	private MissionSelectData data;
	public void SetMissionInfo(MissionSelectData data){
		this.data = data;
		Init();
		SetMissionName(data.name);
		SetMissionBossInfo(data.bossInfo);
		SetMissionInfo(data.info);
		SetMissionIcon(data.icon);
		SetMissionAddress(data.name);
		SetEnemyIcon(data.enemyIcon);
		SetEnemyInfo(data.enemyInfo);
		SetMaterialIcon(data.materialIcon);
		SetMaterialInfo(data.materialInfo);
		SetdifficultyLevelsInfo(data.difficultyData);
		SetCurHardIcon(data.difficultyData[(int)curSelectLv].color);
		SetRecommendedLv(data.recommendedLv);
		SetMainBtn(!data.difficultyData[(int)curSelectLv].isLock,data.isCooldown);
		SetDescription(data.isCooldown);
		mTimeTemp = long.Parse(GetTimeStamp()) - data.lastTime;
		if(mTimeTemp>data.coolDownTime){
			data.isCooldown = true;
			SetDescription(data.isCooldown);
		}
		else{
			data.isCooldown = false;
			SetDescription(data.isCooldown);
			SetTime(data.coolDownTime - mTimeTemp);
		}
		UpdateMissionBar();
	}
	#region delegate
	public delegate void Handle_MissionHuntBtnDelegate();
    public event Handle_MissionHuntBtnDelegate OnMissionHuntBtnDelegate;
	private void MissionHuntBtnDelegate() {
		if(OnMissionHuntBtnDelegate != null) {
			OnMissionHuntBtnDelegate();
		}
	}
	public delegate void Handle_SetUITextureDelegate(string imgName,UITexture missionIcon);
    public event Handle_SetUITextureDelegate OnSetUITextureDelegate;
	private void SetUITextureDelegate(string imgName,UITexture missionIcon) {
		if(OnSetUITextureDelegate != null) {
			OnSetUITextureDelegate(imgName,missionIcon);
		}
	}
	#endregion
	#endregion
	
	#region Local
	[SerializeField]
	private UILabel missionName;
	private void SetMissionName(string name){
		missionName.text = name;
	}
	
	[SerializeField]
	private UILabel missionInfo;
	private void SetMissionInfo(string info){
		missionInfo.text = info;
	}
	
	[SerializeField]
	private UILabel missionBossInfo;
	private void SetMissionBossInfo(string info){
		missionBossInfo.text = info;
	}
	
	[SerializeField]
	private UITexture missionIcon;
	private void SetMissionIcon(string imgName) {
		SetUITextureDelegate(imgName,missionIcon);
	}
	
	[SerializeField]
	private UILabel missionAddress;
	private void SetMissionAddress(string text){
		missionAddress.text = text;
	}
	
	[SerializeField]
	private UITexture enemyIcon;
	private void SetEnemyIcon(string iconName){
		SetUITextureDelegate(iconName,enemyIcon);
	}
	
	[SerializeField]
	private UILabel enemyInfo;
	private void SetEnemyInfo(string text){
		enemyInfo.text = text;
	}
	
	[SerializeField]
	private UITexture materialIcon;
	private void SetMaterialIcon(string iconName){
		SetUITextureDelegate(iconName,materialIcon);
	}
	
	[SerializeField]
	private UILabel materialInfo;
	private void SetMaterialInfo(string text){
		materialInfo.text = text;
	}
	
	[SerializeField]
	private Transform [] difficultyLevels;
	private void SetdifficultyLevelsInfo(MissionDifficultyData[] data){
		if(difficultyLevels.Length == data.Length){
			for(int i=0;i<difficultyLevels.Length;i++){
				difficultyLevels[i].GetComponent<MissionDifficultyBtn>().SetSelectBar(data[i]);
			}
		}
	}
	
	[SerializeField]
	private UILabel curHardInfo;
	private void SetCurHardInfo(string text,Color color){
		curHardInfo.text = text;
		curHardInfo.color = color;
	}
	
	[SerializeField]
	private UISprite curHardIcon;
	private void SetCurHardIcon(Color color){
		curHardIcon.color = color;
	}
	
	[SerializeField]
	private UILabel RecommendedLv;
	private void SetRecommendedLv(string lv){
		RecommendedLv.text = lv;
	}
	
	[SerializeField]
	private NGUIButton huntBtn; 
	[SerializeField]
	private NGUIButton speedupBtn; 
	[SerializeField]
	private NGUIButton lockedBtn; 
	[SerializeField]
	private UILabel btnName; 
	private void SetMainBtn(bool isEnabled,bool isCooldown){
		lockedBtn.isEnabled = false;
		if(isEnabled){
			if(isCooldown){
				NGUITools.SetActive(huntBtn.gameObject,true);
				NGUITools.SetActive(speedupBtn.gameObject,false);
				NGUITools.SetActive(lockedBtn.gameObject,false);
			}else if(!isCooldown) {
				NGUITools.SetActive(huntBtn.gameObject,false);
				NGUITools.SetActive(speedupBtn.gameObject,true);
				NGUITools.SetActive(lockedBtn.gameObject,false);
			}
		}else{
			NGUITools.SetActive(huntBtn.gameObject,false);
			NGUITools.SetActive(speedupBtn.gameObject,false);
			NGUITools.SetActive(lockedBtn.gameObject,true);
		}
	}
	
	[SerializeField]
	private Transform description;
	[SerializeField]
	private Transform cooldownBar;
	private void SetDescription(bool isCooldown){
		if(isCooldown){
			NGUITools.SetActive(description.gameObject,true);
			NGUITools.SetActive(cooldownBar.gameObject,false);
		}
		else{
			NGUITools.SetActive(description.gameObject,false);
			NGUITools.SetActive(cooldownBar.gameObject,true);
		}
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
			UpdateMissionBar();
			return;
		}
		mTimeTemp = long.Parse(GetTimeStamp()) - data.lastTime;;
		SetTime(data.coolDownTime - mTimeTemp);
	}
	
	//获取一个正确的时间，现在是取得本地的时间，如果改动本地时间这样会让cooldown失效。因此需要修改成服务器时间//
	public static string GetTimeStamp(){  
	    System.TimeSpan ts = System.DateTime.UtcNow - new System.DateTime(1970, 1, 1, 0, 0, 0, 0);  
	    return System.Convert.ToInt64(ts.TotalSeconds).ToString();  
	} 
	
	private void UpdateMissionBar(){
		SetMainBtn(data.isEnabled,data.isCooldown);
		SetDescription(data.isCooldown);
		if(data.difficultyData[(int)curSelectLv].isLock){
			SetCurHardInfo("COMPLETE HARD TO UNLOCK",new Color(1.0f,0.5f,0f,1.0f));
		}
		else{
			if(!data.isCooldown){
				SetCurHardInfo("ON COOLDOWN",Color.white);
			}
			else{
				SetCurHardInfo("READY",Color.white);
			}
		}
		SetCurHardIcon(data.difficultyData[(int)curSelectLv].color);
		SetRecommendedLv("Recommended level:"+data.difficultyData[(int)curSelectLv].recommendedLv);
		SetMainBtn(!data.difficultyData[(int)curSelectLv].isLock,data.isCooldown);
	}
	#endregion
	
	
	#region Delegate
	
	private MissionLevels curSelectLv = MissionLevels.Easy;
	private void OnEasyDelegate(){
		
		curSelectLv = MissionLevels.Easy;
		UpdateMissionBar();
	}
	
	private void OnMedDelegate(){
		
		curSelectLv = MissionLevels.Medium;
		UpdateMissionBar();
	}
	
	private void OnHardDelegate(){
		
		curSelectLv = MissionLevels.Hard;
		UpdateMissionBar();
	}
	
	private void OnOverDelegate(){
		
		curSelectLv = MissionLevels.Overrun;
		UpdateMissionBar();
	}
	
	private void OnCloseDelegate(){
		
		gameObject.transform.position = new Vector3(999f,999f,-1f);
		data = null;
	}
	
	[SerializeField]
	private Transform speedupWin;
	private void OnMissionSpeedUpBtnDelegate(){
		speedupWin.GetComponent<SpeedUpManager>().SetMissionInfo(data);
		speedupWin.position = new Vector3(0,0,-1.1f);
	}
	#endregion
}
