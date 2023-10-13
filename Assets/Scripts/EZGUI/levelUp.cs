using UnityEngine;
using System.Collections;

public class levelUp : MonoBehaviour {
	//Instance
	public static levelUp Instance = null;
	public UIPanel basePanel;
	
	public UIButton upgradeBtn;
	
	public bool isLevelUp = false;
	
	public UIRadioBtn    CunningBtn;
	public UIRadioBtn    FortitudeBtn;
	public UIRadioBtn    ProwessBtn;
	
	void Awake() {
		Instance = this;
	}
	
	// Use this for initialization
	void Start () {
		upgradeBtn.AddInputDelegate(UpgradeBtnDelegate);
		CunningBtn.AddInputDelegate(CunningBtnDelegate);
		FortitudeBtn.AddInputDelegate(FortitudeBtnDelegate);
		ProwessBtn.AddInputDelegate(ProwessBtnDelegate);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	#region Interface
//	public SpriteText currLevel;
	public Transform levelSoundPrefab;
	
	public void AwakeLevelUp() {
		InitImage();
		if (BGManager.Instance) {
				BGManager.Instance.StopOriginalBG();
		}
		GameCamera.EnterMissionCompleteState();
//		currLevel.Text = _PlayerData.Instance.playerLevel.ToString();
		SoundCue.PlayPrefabAndDestroy(levelSoundPrefab);
		_UI_CS_ScreenCtrl.Instance.SetNextScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_LEVELUP);
		MoneyBadgeInfo.Instance.Hide(true);
		_UI_CS_FightScreen.Instance.m_CS_Ingame_NormalPanel.Dismiss();
		PlayerInfoBar.Instance.UpdatePlayerLevel(_PlayerData.Instance.playerLevel);
		basePanel.BringIn();
		Player.Instance.PlayLevelUpAnim();
		PlayVFX();
		CalcVal(GetAddPointDis());
	}
	
	public Transform levelUpVFX;
	public Transform levelUpSound;
	public Transform levelUpPSound;
	public Transform levelUpFSound;
	public Transform levelUpCSound;
	public void PlayVFX() {
		Transform vfxInstance = UnityEngine.Object.Instantiate(levelUpVFX)as Transform;
		if(vfxInstance != null&&Player.Instance != null){
			vfxInstance.position = Player.Instance.transform.position;
			SoundCue.PlayPrefabAndDestroy(levelUpSound);
		}
	}
	
	public void WaitAniPTime() {
//		yield return new WaitForSeconds(1f);
		SoundCue.PlayPrefabAndDestroy(levelUpPSound);
	}
	public void WaitAniFTime() {
//		yield return new WaitForSeconds(1f);
		SoundCue.PlayPrefabAndDestroy(levelUpFSound);
	}
	public void WaitAniCTime() {
//		yield return new WaitForSeconds(1f);
		SoundCue.PlayPrefabAndDestroy(levelUpCSound);
	}
	
	public void LeaveLevelUp(){
		if (BGManager.Instance){
            BGMInfo.AutoPlayBGM = true;
			BGManager.Instance.PlayOriginalBG();
            BGMInfo.AutoPlayBGM = false;
		}
		basePanel.Dismiss();
		#region missioon complete
		MissionComplete.Instance.CloseMissionComplete();
		#endregion
		_UI_CS_ScreenCtrl.Instance.SetNextScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_INGAME_NORMAL);
		MoneyBadgeInfo.Instance.Hide(false);
		_UI_CS_FightScreen.Instance.m_CS_Ingame_NormalPanel.BringIn();
		_UI_MiniMap.Instance.isShowSmallMap = true;
		//事件提示入口
		EventSystem.Instance.CheckEvent(EM_EVENT_TYPE.EM_MONEY);
		EventSystem.Instance.AwakeEventPanel();
		GameCamera.EnterNomalState();
	}
	#endregion
	#region Local
	public UIButton titleBg;
	public UIButton infoBg;
	private void InitImage(){
		titleBg.SetUVs(new Rect(0,0,1,1));
		infoBg.SetUVs(new Rect(0,0,1,1));
		TextureDownLoadingMan.Instance.DownLoadingTexture("LevelUP_P_LevleUP",titleBg.transform);
		TextureDownLoadingMan.Instance.DownLoadingTexture("Cool_1",infoBg.transform);
	}
	
	private void UpgradeBtnDelegate(ref POINTER_INFO ptr) {
		switch(ptr.evt) {
		   case POINTER_INFO.INPUT_EVENT.TAP:	
				SendAddPointMsg();
				break;
		}	
	}
	
	private void CunningBtnDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:	
				CalcVal(2);
				break;
		}	
	}
	
	private void FortitudeBtnDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:	
				CalcVal(1);
				break;
		}	
	}
	
	private void ProwessBtnDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:	
				CalcVal(0);
				break;
		}	
	}
	
	private int GetAddPointDis(){
		if(CunningBtn.Value){
			return 2;
		}else if(FortitudeBtn.Value){
			return 1;
		}else{
			return 0;
		}
	}
	
	private void SendAddPointMsg(){
		int AddPoint = 1;
		int temp = GetAddPointDis();
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
	
	public SpriteText atkText;public SpriteText defText;public SpriteText criText;public SpriteText hpText;
	public SpriteText atkAddText;public SpriteText defAddText;public SpriteText crtAddText;public SpriteText hpAddText;
	private int defP = 0;private int atkP = 0;private int crtP = 0;
	private void CalcVal(int currID){
		
		int def = 0;
		int atk = 0;
		int hp  = 0;
		int crt = 0;
		
		//重新修改4个属性->只是玩家四个基础属性//
		atk = _PlayerData.Instance.BaseAttrs[EAttributeType.ATTR_Power];
		def = _PlayerData.Instance.BaseAttrs[EAttributeType.ATTR_Defense];
		crt = _PlayerData.Instance.BaseAttrs[EAttributeType.ATTR_Skill];
		hp  = _PlayerData.Instance.BaseAttrs[EAttributeType.ATTR_MaxHP];
		
		SetPlayerAttruInc(currID);
		hpAddText.Text = _PlayerData.Instance.ReadHpIncVal(_PlayerData.Instance.playerLevel).ToString();
		
		atkText.Text = (atk+atkP).ToString();
		defText.Text = (def+defP).ToString();
		criText.Text = (crt+crtP).ToString();
		hpText.Text = (hp+int.Parse(hpAddText.text)).ToString();
		 
	}
	
	private void SetPlayerAttruInc(int currPoint){
		switch(currPoint){
		case 0:
			atkAddText.Text = "+ 30";	
			defAddText.Text = "+ 10";
			crtAddText.Text = "+ 20";
			atkP = 30;
			defP = 10;
			crtP = 20;	
			return;
		case 1:
			atkAddText.Text = "+ 20";	
			defAddText.Text = "+ 30";
			crtAddText.Text = "+ 10";
			atkP = 20;
			defP = 30;
			crtP = 10;	
			return;
		case 2:
			atkAddText.Text = "+ 10";
			defAddText.Text = "+ 20";
			crtAddText.Text = "+ 30";
			atkP = 10;
			defP = 20;
			crtP = 30;	
			return;
		}
	}
	#endregion
}
