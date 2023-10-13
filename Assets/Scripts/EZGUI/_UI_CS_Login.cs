using UnityEngine;
using System.Collections;

public class _UI_CS_Login : MonoBehaviour {
	//Instance
	public static _UI_CS_Login Instance = null;
	
	public UIPanel 		m_CS_loginPanel;
	
	//CS_Login
	public UIPanel 		m_login_LogoPanel;
	public UIButton 	m_login_LoginButton;
	public UIButton 	registerBtn;
	public UITextField 	m_login_NameEditText;
	public UITextField 	m_login_PassWordEditText;
	public UITextField 	m_login_IPEditText;
	public UIButton 	m_login_BGButton;
	public UIPanel 		m_login_LoginS;
	private bool 		m_isAccountDown;
	private bool 		m_isPassWord1Down;
	private bool 		m_isPassWord2Down;
	public UIPanel 		m_ReconnectPanel;
	public UIButton 	m_SureBtn;
	public UIButton 	m_CancelBtn;
	public SpriteText 	CharaName;
	public SpriteText 	Mission;
	public SpriteText 	VerInfo;
	public string Date = "20120925";
	public string DTime  = "00:00";
	public string Token = "";
	public string Port = "7001";
	public string registerUrl = "https://spicyworld.spicyhorse.com/register.html";
	private string akaneiroAcc = "akaneiroAcc";
	private string akaIsRebAcc = "akaIsRebAcc";
	public UIButton  				bg;
	
//	public EUserType loginType = new EUserType();
	
	void Awake() {
		Instance = this;
	}

	// Use this for initialization
	void Start () {
		InitPlayerAccount();
		AwakeLogin();
		m_login_LoginButton.AddValueChangedDelegate(LoginButtonDelegate);
		isRebAccBtn.AddInputDelegate(IsRebAccBtnDelegate);
		registerBtn.AddValueChangedDelegate(RegisterDelegate);
		m_SureBtn.AddValueChangedDelegate(SureBtnDelegate);
		m_CancelBtn.AddValueChangedDelegate(CancelBtnDelegate);
		PlatformInit();
		VerInfo.Text = Date + " / " + DTime;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

#region Interface
	public void AwakeLogin(){
		InitImage();
		MoneyBadgeInfo.Instance.Hide(true);
		m_CS_loginPanel.BringIn();
		m_login_LogoPanel.BringIn();
		_UI_CS_ScreenCtrl.Instance.SetNextScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_LOGIN);
		
		//steam
		if(Steamworks.activeInstance != null) {
			if(Steamworks.activeInstance.isSteamWork) {
				SteamLoginInit();
				return;
			}
		}
		
		//web
		if(WebLoginCtrl.Instance.IsWebLogin){
	 		StartCoroutine(WebLoginCtrl.Instance.NotifyLoadDone());
			_UI_CS_Login.Instance.m_login_LoginS.Dismiss();
		}else{
			_UI_CS_Login.Instance.m_login_LoginS.BringIn();
		}
 
		StartCoroutine(Wait05Seconds());
	}
#endregion

#region Local
	void IsRebAccBtnDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:
				if(GetIsRebAcc()){
					SetIsRebAcc(false);
				}else{
					SetIsRebAcc(true);
				}
				break;
		}	
	}
	
	private bool GetIsRebAcc() {
		string temp = PlayerPrefs.GetString(akaIsRebAcc);
		if(string.Compare(temp,"") == 0) {
			return false;
		}
		if(string.Compare(temp,"false") == 0) {
			return false;
		}else {
			return true;
		}
	}
	
	private void SteamLoginInit() {
		_UI_CS_Login.Instance.m_login_LoginS.Dismiss();
		_UI_CS_PopupBoxCtrl.Instance.m_popUp_MSGText.Text = Steamworks.activeInstance.sessionKeyResponse;
		_UI_CS_PopupBoxCtrl.Instance.m_CS_popUpBox.BringIn();
		if(Steamworks.activeInstance != null) {
			Steamworks.activeInstance.GetAuthSession();
		}
	}
	
	private void InitImage(){
		bg.SetUVs(new Rect(0,0,1,1));
		//downloading image
		TextureDownLoadingMan.Instance.DownLoadingTexture("Load_BK",bg.transform);
	}
	
	public UIStateToggleBtn isRebAccBtn;
	void InitPlayerAccount(){
		if(GetIsRebAcc()) {
			string temp = PlayerPrefs.GetString(akaneiroAcc);
			m_login_NameEditText.Text = temp; 
			isRebAccBtn.SetToggleState(0);
			//Debug.Log ("*-*player name is : " + temp);
		 }else {
			isRebAccBtn.SetToggleState(1);
		}
	}
	
	private void SetPlayerAcc(){
		PlayerPrefs.SetString(akaneiroAcc,m_login_NameEditText.Text);
	}
	
	private void SetIsRebAcc(bool isReb){
		if(isReb) {
			PlayerPrefs.SetString(akaIsRebAcc,"true");
		}else {
			PlayerPrefs.SetString(akaIsRebAcc,"false");
		}
	}
	
	private void CancelBtnDelegate(IUIObject obj){
		UIButton btn  = obj as UIButton;
		if (btn != null){
			m_ReconnectPanel.Dismiss();
			CS_Main.Instance.g_commModule.SendMessage(
				   		ProtocolGame_SendRequest.UserReconnect(false)
			);
//			_UI_CS_PopupBoxCtrl.Instance.m_popUp_MSGText.Text = "Reconnecting ...";
			LocalizeManage.Instance.GetDynamicText(_UI_CS_PopupBoxCtrl.Instance.m_popUp_MSGText,"RECON");
			_UI_CS_PopupBoxCtrl.Instance.m_CS_popUpBox.BringIn();
			_UI_CS_Ctrl.Instance.m_isReconnect = false;
		}
	}
	
	private void SureBtnDelegate(IUIObject obj){
		UIButton btn  = obj as UIButton;
		if (btn != null){
			m_ReconnectPanel.Dismiss();
			CS_Main.Instance.g_commModule.SendMessage(
				   		ProtocolGame_SendRequest.UserReconnect(true)
			);
			//_UI_CS_EventRewards.Instance.IsFirstLogin = false;
//			_UI_CS_PopupBoxCtrl.Instance.m_popUp_MSGText.Text = "Reconnecting ...";
			LocalizeManage.Instance.GetDynamicText(_UI_CS_PopupBoxCtrl.Instance.m_popUp_MSGText,"RECON");
			_UI_CS_PopupBoxCtrl.Instance.m_CS_popUpBox.BringIn();
		}
	}
	
	void RegisterDelegate(IUIObject obj){
		UIButton btn  = obj as UIButton;
		if (btn != null){
			UrlOpener.Open(registerUrl);
		}
	}
	
	 void LoginButtonDelegate(IUIObject obj){
		UIButton btn  = obj as UIButton;
		if (btn != null){
			//Client Ver or normal Ver
			if(ClientLogicCtrl.Instance.isClientVer){
				if(ClientLogicCtrl.Instance.GetEmailValid()){
					StartCoroutine(ClientLogicCtrl.Instance.SendMsgToServerForLogin());
				}else{
					LocalizeManage.Instance.GetDynamicText(_UI_CS_PopupBoxCtrl.Instance.m_popUp_MSGText,"LOGININPUTERR");
					_UI_CS_PopupBoxCtrl.Instance.m_CS_popUpBox.BringIn();
				}
				SetPlayerAcc();
			}else{
				CS_Main.Instance.g_commModule.Connect(m_login_IPEditText.text,7001);
				LocalizeManage.Instance.GetDynamicText(_UI_CS_PopupBoxCtrl.Instance.m_popUp_MSGText,"CON");
				_UI_CS_PopupBoxCtrl.Instance.m_CS_popUpBox.BringIn();
				SetPlayerAcc();
			}
//			Steamworks.activeInstance.GetAuthSession();
		}
	}
	
	private IEnumerator Wait05Seconds() {
		yield return new WaitForSeconds(0.5f);
		_UI_CS_Ctrl.Instance.m_UI_Manager.FocusObject = _UI_CS_Login.Instance.m_login_NameEditText;
	}
	
	void PlatformInit() {
		if(ClientLogicCtrl.Instance.isClientVer){
			m_login_IPEditText.transform.position = new Vector3(999,999,999);
		}else{
			//only client ver have register.
			registerBtn.transform.position = new Vector3(999,999,999);
		}
	}
#endregion
	
	
}
