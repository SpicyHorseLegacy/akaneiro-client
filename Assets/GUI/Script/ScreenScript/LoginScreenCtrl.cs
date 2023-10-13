using UnityEngine;
using System.Collections;

public class LoginScreenCtrl : MonoBehaviour {
	
	public static LoginScreenCtrl Instance;
	
	void Awake() {
		Instance = this;
		RegisterInitEvent();
	}
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

	}
	
	#region Interface
	public void AwakeLogin() {
		FadeInLogo();
	}
	#endregion
	
	#region Local
	#region event create and destory
	private int initDelegateCount = 2;
	private void TemplateInitEnd() {
		if(GUIManager.Instance.GetTemplateInitEndCount() >= initDelegateCount) {
			Init();
//			GUILogManager.LogErr("LoginScreenCtrl template init ok.");
		}
	}
	private void RegisterInitEvent() {
		GUIManager.Instance.OnInitEndDelegate += TemplateInitEnd;
		GUIManager.Instance.OnScreenManagerDestroy += DestoryAllEvent;
	}
	
	private void RegisterTemplateEvent() {
		if(LoginInputManager.Instance) {
			LoginInputManager.Instance.OnLoginDelegate += this.LoginDelegate;
			LoginInputManager.Instance.OnRegistrationDelegate += this.RegistrationDelegate;
		}
	}
	
	private void DestoryAllEvent() {
		if(LoginInputManager.Instance) {
			LoginInputManager.Instance.OnLoginDelegate -= this.LoginDelegate;
			LoginInputManager.Instance.OnRegistrationDelegate -= this.RegistrationDelegate;
		}
		GUIManager.Instance.OnInitEndDelegate -= TemplateInitEnd;
		GUIManager.Instance.OnScreenManagerDestroy -= DestoryAllEvent;
	}
	#endregion
	
	private void Init() {
		StartCoroutine(StartAwakeLogin());
		RegisterTemplateEvent();
		InitInput();
	}

	// logo fade.//
	private void FadeInLogo() {
		if(LoginLogoManager.Instance != null) {
			LoginLogoManager.Instance.PlayLogoFadeIn();
		}else {
			GUILogManager.LogErr("LoginLogoManager.Instance is null");
		}
	}
	
	//wait LoginLogoManager.Instance init.//
	protected IEnumerator StartAwakeLogin() {
		yield return new WaitForSeconds(1f);
		AwakeLogin();
	}
	
	private void SteamLoginInit() {
		LoginInputManager.Instance.HideAllInput();
		PopUpBox.PopUpErr(Steamworks.activeInstance.sessionKeyResponse);
		if(Steamworks.activeInstance != null) {
			Steamworks.activeInstance.GetAuthSession();
		}
	}
	
	private void InitInput() {
		//steam版本auto login//
		if(VersionManager.Instance.GetPlatformType().Get() == EUserType.eUserType_Steam) {
			SteamLoginInit();
		}else {
			//客户端版本不需要ip&port//
			if( VersionManager.Instance.GetVersionType() == VersionType.NormalClientVersion)
                LoginInputManager.Instance.HideIpAndPort();
            if (VersionManager.Instance.GetVersionType() == VersionType.WebVersion ||
                VersionManager.Instance.GetVersionType() == VersionType.TestServerWebVersion)
			{
                LoginInputManager.Instance.HideAllInput();
				StartCoroutine(VersionManager.Instance.NotifyLoadDone());
				return;
			}

			//init account, password and checkBox.//
			if(VersionManager.Instance.GetVersionType() == VersionType.InternalWebVersion ||
				VersionManager.Instance.GetVersionType() == VersionType.NormalClientVersion) {
				int isRememberAcc = int.Parse(DataManager.Instance.GetMapValue(DataListType.AccountData,"rememberAcc"));
				if(isRememberAcc == 1) {
					string account = DataManager.Instance.GetMapValue(DataListType.AccountData,"account");
					string password = DataManager.Instance.GetMapValue(DataListType.AccountData,"password");
					string ip = DataManager.Instance.GetMapValue(DataListType.AccountData,"ip");
					string port = DataManager.Instance.GetMapValue(DataListType.AccountData,"port");
					LoginInputManager.Instance.InitInputState(true,account,password,ip,port);
				}else {
					LoginInputManager.Instance.InitInputState(false,"","","","");
				}
			}
		}
//		//init ip//
//		try {
//			if(VersionManager.Instance.GetVersionType() == VersionType.InternalWebVersion) {
//				string ip = DataManager.Instance.GetMapValue(DataListType.AccountData,"ip");
//				LoginInputManager.Instance.UpdateIP(ip);
//			}
//		}catch(System.Exception e) {
//			GUILogManager.LogErr("UpdateIP err.");
//		}
	}
	
	private void SetLoginInfo(string account,string uid,string password,int rem,string ip,string port) {
		DataManager.Instance.UpdateValue(DataListType.AccountData,"uid",uid);
		DataManager.Instance.UpdateValue(DataListType.AccountData,"account",account);
		DataManager.Instance.UpdateValue(DataListType.AccountData,"password",password);
		DataManager.Instance.UpdateValue(DataListType.AccountData,"rememberAcc",rem.ToString());
		DataManager.Instance.UpdateValue(DataListType.AccountData,"ip",ip);
		DataManager.Instance.UpdateValue(DataListType.AccountData,"port",port);
		DataManager.Instance.Save(DataListType.AccountData);
	}
	
	private void LoginDelegate() {
		//remember account.//
		SetLoginInfo(LoginInputManager.Instance.GetAccountFromInput(),
			LoginInputManager.Instance.GetAccountFromInput(),
			LoginInputManager.Instance.GetPassWordFromInput(),
			LoginInputManager.Instance.GetRememberFlag()?1:0,
			LoginInputManager.Instance.GetIPFromInput(),
			LoginInputManager.Instance.GetPortFromInput());
		//now we only client and editor need click login.//
		//other version will auto login.//
		if(VersionManager.Instance.GetVersionType() == VersionType.NormalClientVersion) {
			if(LoginInputManager.Instance.GetEmailValid()) {
				StartCoroutine(SendMsgToServerForLogin());
			}else {
				PopUpBox.PopUpErr(LocalizeManage.Instance.GetDynamicText("LOGININPUTERR"));
			}
		}else {
			//editor mode.//
//			LogManager.Log_Info("Account: "+ LoginInputManager.Instance.GetAccountFromInput());
//			LogManager.Log_Info("Password: "+ LoginInputManager.Instance.GetPassWordFromInput());
//			LogManager.Log_Info("Ip: "+ LoginInputManager.Instance.GetIPFromInput());
			ConnectServer(LoginInputManager.Instance.GetIPFromInput(),LoginInputManager.Instance.GetPortFromInput());
		}
	}
	
	public void ConnectServer(string ip,string port) {
		CS_Main.Instance.g_commModule.Connect(ip,short.Parse(port));
		PopUpBox.PopUpErr(LocalizeManage.Instance.GetDynamicText("CON"));
	}
	
	private string generalName = "akaneiro";
	private string testServerName = "akaneiro_test";
	public IEnumerator SendMsgToServerForLogin(){
		PopUpBox.PopUpErr(LocalizeManage.Instance.GetDynamicText("CONTOSERVER"));
		WWWForm handelServer = new WWWForm();
		handelServer.AddField("email",DataManager.Instance.GetMapValue(DataListType.AccountData,"account"));
		handelServer.AddField("password",DataManager.Instance.GetMapValue(DataListType.AccountData,"password"));
		if(VersionManager.Instance.GetVersionType() == VersionType.TestServerWebVersion) {
			handelServer.AddField("game",testServerName);
		}else{
			handelServer.AddField("game",generalName);
		}
		handelServer.AddField("other",VersionManager.Instance.GetVersionNumber());
		WWW url = new WWW(VersionManager.Instance.GetLoginUrl(),handelServer);
		yield return url;
		if(url.error != null) {
			GUILogManager.LogErr("Error link server.");
		}else {
			int tmepErr = CheckErr(url.text);
			if( tmepErr == 0){
				PopUpBox.Hide(true);
				GUILogManager.LogWarn("link server ok :receive zzx msg");
				if(VersionManager.Instance.InitLoginInfo(url.text)){
					ConnectServer(DataManager.Instance.GetMapValue(DataListType.AccountData,"ip"),DataManager.Instance.GetMapValue(DataListType.AccountData,"port"));
				}
			}else if( tmepErr == 1){
				PopUpBox.PopUpErr(LocalizeManage.Instance.GetDynamicText("ACCORPASSERR"));
			}
		}
	}
	
	private string registerUrl = "https://spicyworld.spicyhorse.com/register.html";
	private void RegistrationDelegate() {
		UrlOpener.Open(registerUrl);
	}
	
	/// <summary>
	/// Checks the riceive msg.no use json lib
	/// </summary>
	private int CheckErr(string msg){
		//temp
		//{"uid":"0","message":"error"}
		string[] ele = msg.Split(',');
		string[] eleSon = ele[1].Split(':');
		string tempString = "";
		tempString = eleSon[1].Substring(1,eleSon[1].Length-2);
		LogManager.Log_Info("CheckErr:"+tempString);
		if(tempString.Contains("error")) {
			return 1;
		}else{
			return 0;
		}
	}
	#endregion
}
