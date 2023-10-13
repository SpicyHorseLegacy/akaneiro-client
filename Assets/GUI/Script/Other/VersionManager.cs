using UnityEngine;
using System.Collections;

public enum VersionType {
	InternalWebVersion = 0,
	WebVersion,
	TestServerWebVersion,
	NormalClientVersion,
	SteamClientVersion,
	TypeMax
} 

public enum PlatformType {
	SpicyHorse = EUserType.eUserType_SpicyHorse,
	Aeria = EUserType.eUserType_Aeria,
	Armor = EUserType.eUserType_Armor,
	Facebook = EUserType.eUserType_Facebook,
	Kongregate = EUserType.eUserType_Kongregate,
	Steam = EUserType.eUserType_Steam,
	TypeMax
}

public class VersionManager : MonoBehaviour {
	
	public static VersionManager	Instance;
	
	void Awake () {
		Instance = this;
	}
	
	// Use this for initialization
	void Start () {
		SetVersionNumber(myVerNumber.ToString());
		//default//
		SetPlatformType((int)iPlatformType);
		
		// todo : change the client type for different type.
		SetClientType(EClientType.eClientType_WebPlayer);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	#region Interface
	#region Login Data
	[HideInInspector]
	public string receiveMsg = "";
	[HideInInspector]
	public string uid = "0";
	[HideInInspector]
	public string token = "";
	[HideInInspector]
	public string gameCode = "";
	[HideInInspector]
	public string ip = "";
	[HideInInspector]
	public string port = "";
	[HideInInspector]
	public string key = "";
	[HideInInspector]
	public string msg = "0";
	[HideInInspector]
	public string surplus = "";
	private string generalName = "akaneiro";
	private string testServerName = "akaneiro_test";
	private bool isConnectTestServer = false;
	private string loginURL = string.Empty;
	private string loginAPI = string.Empty;
	[HideInInspector]
	public bool isVersionOk = true;
	
	public IEnumerator NotifyLoadDone()
    {
        yield return 0;
		PopUpBox.PopUpErr(LocalizeManage.Instance.GetDynamicText("LOADINGPWAM"));
        yield return new WaitForSeconds(2f);
		LogManager.Log_Info("send <load_player_done> to web server.");
        Application.ExternalCall("load_player_done");
		PopUpBox.Hide(true);
        yield break;
    }
	
	public void APIUrl(string _url) {
		GUILogManager.LogInfo("api "+_url);
		loginURL = _url;
    }
	public void API1(string _url) {
		GUILogManager.LogInfo("API1 "+_url);
		loginAPI = _url;
	}
	 public void User(string _user) {
		GUILogManager.LogInfo("user "+_user);
		uid = _user;
	 }            
    public void Token(string _token) {
		GUILogManager.LogInfo("Token "+_token);
		token = _token;
    }
	public void _MessageType(string _msg) {
		GUILogManager.LogInfo("Message "+_msg);
		msg = _msg;
    }
    public void Auth(string _codes) {
		GUILogManager.LogInfo("Auth "+_codes);
    }
    public void SetType(int _type) {
		GUILogManager.LogInfo("set type string:  "+_type);
		SetPlatformType(_type);
    }
	public void Version(string _ver) {
		GUILogManager.LogInfo("Version "+_ver);
		if(0 == string.Compare(myVerNumber,_ver)) {
			isVersionOk = true;
		}else {
			isVersionOk = false;
		}
    }
    public void SetSSL(int _type) {
		GUILogManager.LogInfo("set SSL : " + _type);
    }           
    public void Links(string a_ipandprot) {
		GUILogManager.LogInfo("Ip and port: " + a_ipandprot);
    }
    public void Links2(string ip, string p) {
        GUILogManager.LogInfo("ip port:" + ip + " " + p);
    }
	
	public bool mNeedLogin = true;
    public void NeedLogin(string needLogin)
    {
		GUILogManager.LogInfo("NeedLogin : " + needLogin);
        mNeedLogin = needLogin != "0" ? true : false;
        if (!mNeedLogin)
        {
			StartCoroutine(selectServer());
        }
    }

    IEnumerator selectServer()
    {
		string url = loginURL + loginAPI + "?user=" + uid + "&type=" + GetPlatformType().Get() + "&codes=" + token + "&message="+ msg;

        WWW wwwGetQuest = new WWW(url);

        yield return wwwGetQuest;

        string desc;

        if (wwwGetQuest.error != null)
        {

            desc = "SystemError";
			LogManager.Log_Error("Failed to visit: " + url);
			LogManager.Log_Error("Error: " + wwwGetQuest.error );
        }
        else
        {
            desc = wwwGetQuest.text;
        }

        wwwGetQuest.Dispose();
        wwwGetQuest = null;
		
        LogManager.Log_Info("Ip and port: " + desc);
        if (desc == "Timeout")
        {
            LogManager.Log_Warn("Login timeout!");
			PopUpBox.PopUpDeleBox("Login timeout!",RefreshCallback);
        }
        else if (desc == "SystemError" || desc == "AlreadyOnline" || desc == "LoginInProgress" )
        {
			selectServer();
		}
        else if (desc == "InvalidRegion")
        {
            LogManager.Log_Error("Could not connect to the server: " + desc);
			PopUpBox.PopUpDeleBox("Could not connect to the server: " + desc,RefreshCallback);
        }
        else
		{
			string[] sArray = desc.Split(':');
			if (sArray.Length == 2)
			{
                LogManager.Log_Info("Start Login : " + uid + " :: " + token + " :: " + sArray[0] + " :: " + sArray[1]);
                InitLoginInfoForWeb(uid, token, sArray[0], sArray[1]);
				
				LoginLogic(sArray[0], sArray[1]);
			}
			else
			{
				LogManager.Log_Error("Could not connect to the server: " + desc);
				PopUpBox.PopUpDeleBox("Could not connect to the server: " + desc,RefreshCallback);
			}
		}
    }
	
	void LoginLogic(string _ip, string _port){
		if(isVersionOk){
			CS_Main.Instance.g_commModule.Connect(_ip,short.Parse(_port));
			PopUpBox.PopUpErr(LocalizeManage.Instance.GetDynamicText("CON"));
		}else{
			PopUpBox.PopUpErr(LocalizeManage.Instance.GetDynamicText("VERSIONERR"));
		}
	}
	void RefreshCallback(){
		Application.ExternalCall("refresh_page");
	}
	
	#endregion
	[SerializeField]private PlatformType iPlatformType = PlatformType.SpicyHorse;
	private EUserType platformType = new EUserType();
	public void SetPlatformType(int type) {
		platformType.Set(type);
	}
	public EUserType GetPlatformType() {
		return platformType;
	}
	
	[SerializeField]private EClientType clientType = new EClientType();
	public void SetClientType(int type) {
		clientType.Set(type);
	}
	public EClientType GetClientType() {
		return clientType;
	}
	
	[SerializeField]
	private VersionType myVersionType = VersionType.InternalWebVersion;
	public VersionType GetVersionType() {
		return myVersionType;
	}
	
	[SerializeField]
	private string myVerNumber = "unknow version";
	public void SetVersionNumber(string ver) {
		myVerNumber = ver;
//		DataManager.Instance.UpdateValue(DataListType.AccountData,"version",ver);
//		DataManager.Instance.Save(DataListType.AccountData);
	}
	public string GetVersionNumber() {
		return myVerNumber;
	}
	
    void InitLoginInfoForWeb(string _uid, string _token, string _ip, string _port)
    {
        DataManager.Instance.UpdateValue(DataListType.AccountData, "uid",_uid);
        DataManager.Instance.UpdateValue(DataListType.AccountData, "token", _token);
        DataManager.Instance.UpdateValue(DataListType.AccountData, "ip", _ip);
        DataManager.Instance.UpdateValue(DataListType.AccountData, "port", _port);
    }

	/// <summary>
	/// check receive msg.
	/// Does not contain A. STEAM login please go to steamworks.cs
	/// for spicyhorse.
	/// </summary>
	/// </param>
	public bool InitLoginInfo(string msg){
		if(!CheckVerErr(msg)){
			PopUpBox.PopUpErr(LocalizeManage.Instance.GetDynamicText("VERSIONERR"));
			return false;
		}
		string[] ele = msg.Split(',');
		VersionManager.Instance.uid = GetUid(ele[0]);
		DataManager.Instance.UpdateValue(DataListType.AccountData,"uid",GetUid(ele[0]));
		VersionManager.Instance.token = GetToken(ele[1]);
		DataManager.Instance.UpdateValue(DataListType.AccountData,"token",GetToken(ele[1]));
		VersionManager.Instance.gameCode = GetGameCode(ele[3]);
		DataManager.Instance.UpdateValue(DataListType.AccountData,"gameCode",GetGameCode(ele[3]));
		VersionManager.Instance.ip = GetIP(ele[2]);
		DataManager.Instance.UpdateValue(DataListType.AccountData,"ip",GetIP(ele[2]));
		VersionManager.Instance.port = GetPort(ele[2]);
		DataManager.Instance.UpdateValue(DataListType.AccountData,"port",GetPort(ele[2]));
		VersionManager.Instance.key = GetKey(ele[4]);
		DataManager.Instance.UpdateValue(DataListType.AccountData,"key",GetKey(ele[4]));
		DataManager.Instance.Save(DataListType.AccountData);
		return true;
	}
	
	private string spicyhorseLoginUrl = "http://spicyworld.spicyhorse.com/client.php?mod=login";
	public string GetLoginUrl() {
		switch(myVersionType) {
		case VersionType.InternalWebVersion:
			return spicyhorseLoginUrl;
		case VersionType.NormalClientVersion:
		case VersionType.TestServerWebVersion:
		case VersionType.WebVersion:
			return spicyhorseLoginUrl;
		case VersionType.SteamClientVersion:
			return spicyhorseLoginUrl;
		default:
			return spicyhorseLoginUrl;
		}
	}
	
	//account to uid.//
	public string GetUIDFormAccount(string account) {
		return account.Substring(0,account.Length-2);
	}
	
	private string rechargeUrl = "http://spicyworld.spicyhorse.com/client.php?mod=recharge";
	public IEnumerator SendMsgToServerForPay(string content){
		bool isPaySuccess = false;
		WWWForm handelServer = new WWWForm();
		handelServer.AddField("uid",uid);
		handelServer.AddField("token",token);
		if(isConnectTestServer){
			handelServer.AddField("game",testServerName);
		}else{
			handelServer.AddField("game",generalName);
		}
		handelServer.AddField("currency",content);
		WWW url = new WWW(rechargeUrl,handelServer);
		yield return url;
		if(url.error != null){
			GUILogManager.LogErr("Error link server:"+url.error);
			PopUpBox.PopUpErr(url.text);
		}else{
			GUILogManager.LogWarn("link server success from zzx.");
			receiveMsg = url.text;
			isPaySuccess = CheckSuccessForPay(url.text);
			InitPayData(url.text);
			ShowPaySurplus(content,isPaySuccess);
		}
	}
	private void InitPayData(string msg){
		if(msg.CompareTo("") == 0){
			GUILogManager.LogInfo("zzx return string is empty.");
			return;
		}
		string[] ele = msg.Split(',');
		GUILogManager.LogInfo("InitPayInfo;msg:"+ele[3]);
		surplus 	 = GetSurplus(ele[3]);
		LogManager.Log_Info("surplus:"+surplus);
		token 	 	 = GetPayToken(ele[0]);
		GUILogManager.LogInfo("token:"+token);
		key 	 	 = GetPaykey(ele[1]);
		GUILogManager.LogInfo("key:"+key);
	}
	private string GetSurplus(string msg){
		string[] ele = msg.Split(':');
		return ele[1].Substring(0,ele[1].Length-1);
	}
	string GetPayToken(string msg){
		string[] ele = msg.Split(':');
		return ele[1].Substring(1,ele[1].Length-2);
	}
	string GetPaykey(string msg){
		string[] ele = msg.Split(':');
		return ele[1].Substring(1,ele[1].Length-2);
	}
	
	public void ShowPaySurplus(string content,bool isPaySuccess){
		if(isPaySuccess) {
			PopUpBox.PopUpErr(LocalizeManage.Instance.GetDynamicText("PURCHASEOK"));
		}else {
			string payMoneyUrl = GetPayMoneyUrl(content);
			GUILogManager.LogErr(payMoneyUrl);
			UrlOpener.Open(payMoneyUrl);
		}
	}
	private string GetPayMoneyUrl(string content){
		string url = "http://spicyworld.spicyhorse.com/client.php?mod=payment&uid="+uid+"&token="+token+"&game=akaneiro&currency="+content+"&key="+key;
		return url;
	}
	
	//temp : {"message":"success","surplus":1380}
	private bool CheckSuccessForPay(string msg){
		if(msg.CompareTo("") == 0){
			GUILogManager.LogInfo("zzx return string is empty.");
			return false;
		}
		string[] ele = msg.Split(',');
		string[] eleSon = ele[2].Split(':');
		string tempString = "";
		tempString = eleSon[1].Substring(1,eleSon[1].Length-2);
		GUILogManager.LogInfo("CheckSuccessForPay:"+tempString);
		if(tempString.Contains("success")){
			return true;
		}else{
			return false;
		}
	}
	#endregion
	
	#region Local
	#region no use json lib
	private string GetKey(string msg){
		string[] ele = msg.Split(':');
		return ele[1].Substring(1,ele[1].Length-2);
	}
	private string GetUid(string msg){
		string[] ele = msg.Split(':');
		return ele[1].Substring(1,ele[1].Length-2);
	}
	private string GetToken(string msg){
		string[] ele = msg.Split(':');
		return ele[1].Substring(1,ele[1].Length-2);
	}
	private string GetGameCode(string msg){
		string[] ele = msg.Split(':');
		return ele[1].Substring(1,ele[1].Length-2);
	}
	private string GetIP(string msg){
		string[] ele = msg.Split(':');
		return ele[1].Substring(1,ele[1].Length-1);
	}
	private string GetPort(string msg){
		string[] ele = msg.Split(':');
		return ele[2].Substring(0,ele[2].Length-1);
	}
	//for spicyhorse web.server maker<zhu>//
	public bool CheckVerErr(string msg){
		string[] ele = msg.Split(',');
		string[] ele1 = ele[2].Split(':');
		string tempString = "";
		tempString = ele1[1].Substring(1,ele1[1].Length-2);
		GUILogManager.LogErr("CheckVerErr:"+tempString);
		if(tempString.Contains("vererr")){
			return false;
		}else{
			return true;
		}
	}
	#endregion
	#endregion

}
