using UnityEngine;
using System.Collections;

public class _MissSeleWin : MonoBehaviour {
	
	public static _MissSeleWin Instance;
    private static Texture2D m_transparentTexture = null;
	
	void Awake() {
		Instance = this;
	}
	
	// Use this for initialization
	void Start () {
		GUIManager.Instance.AddTemplateInitEnd();
	}
	
	// Update is called once per frame
	void Update () {
		if(isUpdateCoolDownTime) {
			CalcCoolDownTime();
		}
	}
	
	#region Interface
	private MissionSeleData data;
	public void InitSeleWinData(MissionSeleData info,int missID) {

        if (m_transparentTexture == null)
        {
            m_transparentTexture = new Texture2D(1, 1, TextureFormat.ARGB32, false);
            m_transparentTexture.SetPixel(0, 0, new Color(1f, 1f, 1f, 0f));
            m_transparentTexture.Apply();
        }
        
        Show();
		curMissionID = missID;
		data = info;
		HideDialogInfo(false);
		SetMissionTilte(data.localData.missName);
		SetMissionDesc(data.localData.missInfo);
		SetrecommendLv(data.localData.recommendLv);
		SetBoosInfo(data.localData.bossInfo);
		SetAreaIcon(data.localData.missIconName);
		SetAreaName(data.localData.areaName);
		SetAreaInfo(data.localData.areaDesc);
		SetEnmeyIcon(data.localData.enemyIconName);
		SetEnemyInfo(data.localData.enemyInfo);
		SetMatIcon1(data.localData.matIconName1);
		SetMatIcon2(data.localData.matIconName2);
		SetMatIcon3(data.localData.matIconName3);
		SetMatInfo(data.localData.matInfo);
		fcoolDownTime = info.serData.coolDownTime;
		CalcCoolDownTime();
		InitLevelBtn();
		PlayerDataManager.Instance.SetScenseName(data.scenseName);
	}
	
	public void Hide(bool ani) {
		ani = false;
		gameObject.GetComponent<AnimationPlayer>().Hide(true,ani);
	}
	public void Show() {
		gameObject.GetComponent<AnimationPlayer>().Hide(false,false);
	}
	
	private int curMissionID = 0;
	public void SetCurMissID(int missID) {
		curMissionID = missID;
		PlayerDataManager.Instance.SetMissionID(missID);
	}
	public int GetCurMissID() {
		return curMissionID;
	}
	
	public int GetCurrentMapID() {
		return 0; //now only one map.//
	}
	
	public int GetCurrentRegionID() {
		return (curMissionID/100%10-1);
	}
	
	public int GetCurrentMissionID() {
		return (curMissionID/10%10-1);
	}
	
	public int GetCurrentLevelID() {
		return (curMissionID%10-1);
	}
	#endregion
	
	#region Local
	[SerializeField]
	private UILabel missionTitle;
	private void SetMissionTilte(string title) {
		missionTitle.text = title;
	}
	[SerializeField]
	private UILabel missionDesc;
	private void SetMissionDesc(string desc) {
		missionDesc.text = desc;
	}
	[SerializeField]
	private UILabel recommendLv;
	private void SetrecommendLv(string desc) {
		recommendLv.text = desc;
	}
	[SerializeField]
	private UILabel boosInfo;
	private void SetBoosInfo(string info) {
		boosInfo.text = info;
	}
	[SerializeField]
	private UISprite areaIcon;
	private void SetAreaIcon(string name) {
		areaIcon.spriteName = name;
	}
	[SerializeField]
	private UILabel areaInfo;
	private void SetAreaInfo(string info) {
		areaInfo.text = info;
	}
	[SerializeField]
	private UILabel areaName;
	private void SetAreaName(string name) {
		areaName.text = name;
	}
	[SerializeField]
	private UITexture enemyIcon;
	private void SetEnmeyIcon(string name) {
        if (name == "0") // No Icon
        {
            enemyIcon.mainTexture = m_transparentTexture;
            return;
		}
		_MissionSelectBaseManager.Instance.SetUITextureDelegate(name,enemyIcon);
	}
	[SerializeField]
	private UILabel enemyInfo;
	private void SetEnemyInfo(string info) {
		enemyInfo.text = info;
	}
	[SerializeField]
	private UITexture matIcon1;
	private void SetMatIcon1(string name) {
        if (name == "0") // No Icon
        {
            matIcon1.mainTexture = m_transparentTexture;
            return;
		}
		_MissionSelectBaseManager.Instance.SetUITextureDelegate(name,matIcon1);
	}
	//
	[SerializeField]
	private UITexture matIcon2;
	private void SetMatIcon2(string name) {
        if (name == "0") // No Icon
        {
            matIcon2.mainTexture = m_transparentTexture;
            return;
		}
		_MissionSelectBaseManager.Instance.SetUITextureDelegate(name,matIcon2);
	}
	[SerializeField]
	private UITexture matIcon3;
	private void SetMatIcon3(string name) {
        if (name == "0") // No Icon
        {
            matIcon3.mainTexture = m_transparentTexture;
            return;
		}
		_MissionSelectBaseManager.Instance.SetUITextureDelegate(name,matIcon3);
	}
	[SerializeField]
	private UILabel matInfo;
	private void SetMatInfo(string info) {
		matInfo.text = info;
	}
	
	[SerializeField]
	private UISprite lvIcon;
	public void SetLvIconColor(int lv) {
		switch(lv) {
		case 0:
			lvIcon.color = Color.white;
			break;
		case 1:
			lvIcon.color = Color.green;
			break;
		case 2:
			lvIcon.color = Color.yellow;
			break;
		case 3:
			lvIcon.color = new Color(219f / 255f,124f / 255f,0f);
			break;
		case 4:
			lvIcon.color = Color.red;
			break;
		default:
			lvIcon.color = Color.white;
			break;
		}
	}
	
	#region coolDwon
	private float fcoolDownTime = 0f;
	public bool  isUpdateCoolDownTime = false;
	private void CalcCoolDownTime(){
		string strCdt = "";
		float tempTime = Time.time - PlayerDataManager.Instance.GetReceiveSerMsgTime();
		tempTime = (fcoolDownTime - tempTime);
		if(tempTime < 0.1f){
			HideCoolDown();
		}else{
			strCdt = ((int)tempTime/3600).ToString()+"h "+((int)tempTime/60%60).ToString()+"m "+((int)tempTime%60).ToString()+"s";	
			ShowCoolDown(strCdt);
		}
	}
	[SerializeField]
	private Transform coolDownRoot;
	[SerializeField]
	private UILabel coolTimeText;
	private void HideCoolDown() {
		isUpdateCoolDownTime = false;
		NGUITools.SetActive(coolDownRoot.gameObject,false);
		HideDialogInfo(false);
	}
	private void ShowCoolDown(string time) {
		isUpdateCoolDownTime = true;
		NGUITools.SetActive(coolDownRoot.gameObject,true);
		HideDialogInfo(true);
		coolTimeText.text = time;
	}
	#endregion
	
	[SerializeField]
	private Transform dialogInfoRoot;
	private void HideDialogInfo(bool hide) {
		NGUITools.SetActive(dialogInfoRoot.gameObject,!hide);
	}
	
	private void OnCloseDelegate() {
		Hide(true);
	}
	
	#region Init Level Btn
	[SerializeField]
	private UICheckbox easyBtn;
	[SerializeField]
	private _MissLevel [] levelBtns;
	private void InitLevelBtn() {
		for(int i = 0;i<4;i++) {
			levelBtns[i].InitLevelInfo(true,data);
		}
		for(int j = 0;j<data.serData.canAcceptMaxLvl;j++) {
			levelBtns[j].InitLevelInfo(false,data);
		}
		levelBtns[0].CheckDelegate();
		easyBtn.isChecked = true;
	}
	#endregion
	
	#region main Btn
	[SerializeField]
	private Transform huntRoot;
	[SerializeField]
	private Transform speedUpRoot;
	[SerializeField]
	private Transform lockRoot;
	public void UpdateMainBtns(bool isLock) {
		if(isLock) {
			NGUITools.SetActive(lockRoot.gameObject,true);
			NGUITools.SetActive(huntRoot.gameObject,false);
			NGUITools.SetActive(speedUpRoot.gameObject,false);
		}else if(isUpdateCoolDownTime) {
			NGUITools.SetActive(lockRoot.gameObject,false);
			NGUITools.SetActive(huntRoot.gameObject,false);
			NGUITools.SetActive(speedUpRoot.gameObject,true);
		}else {
			NGUITools.SetActive(lockRoot.gameObject,false);
			NGUITools.SetActive(huntRoot.gameObject,true);
			NGUITools.SetActive(speedUpRoot.gameObject,false);
		}
	}
	public delegate void Handle_HuntDelegate();
    public event Handle_HuntDelegate OnHuntDelegate;
	private void HuntDelegate() {
		if(OnHuntDelegate != null) {
			OnHuntDelegate();
		}
	}
	public delegate void Handle_SpeedUpDelegate();
    public event Handle_SpeedUpDelegate OnSpeedUpDelegate;
	private void SpeedUpDelegate() {
		if(OnSpeedUpDelegate != null) {
			OnSpeedUpDelegate();
		}
	}
	public delegate void Handle_LockDelegate();
    public event Handle_LockDelegate OnLockDelegate;
	private void LockDelegate() {
		if(OnLockDelegate != null) {
			OnLockDelegate();
		}
	}
	[SerializeField]
	private UILabel rcLv;
	[SerializeField]
	private UILabel mainBtnTitle;
	public void SetRecommedLv(string lv) {
		rcLv.text = lv;
	}
	public void SetMainBtnTitle(string title) {
		mainBtnTitle.text = title;
	}
	#endregion
	#endregion
}
