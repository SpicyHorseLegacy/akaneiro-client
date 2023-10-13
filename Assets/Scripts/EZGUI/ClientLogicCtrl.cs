using UnityEngine;
using System.Collections;
using System.Text.RegularExpressions;

/// <summary>
/// Client version  use it.But if this ver is steam ! use down !.
/// Does not contain A. STEAM login please go to steamworks.cs !
/// </summary>
public class ClientLogicCtrl : MonoBehaviour {
	
	public static 	ClientLogicCtrl Instance = null;
	public bool 	isClientVer = false;

	public string 	loginUrl = "http://spicyworld.spicyhorse.com/client.php?mod=login";
	//public string 	loginUrl = "http://192.168.6.214/spicy/client.php?mod=login";
	public string 	payUrl 	 = "http://spicyworld.spicyhorse.com/client.php?mod=recharge";
	
	public string 	 receiveMsg 	= "";
	public string 	 uid 			= "0";
	//------------------------------------------------
	// this only use spicyhorse buy someting. so,if steam please use gamecode. thanks
	public string 	 token 			= "";
	//Login redeem and more
	public string 	 gameCode 		= "";
	//-------------------------------------------------
	public string 	 ip		 		= "";
	public string 	 port			= "";
	public string 	 key			= "";
	
	public string 	 _msg 			= "0";
	
	public bool		 isConnectTestServer = false;
	private string	 generalName = "akaneiro";
	private string	 testServerName = "akaneiro_test";
	
	public string 	 surplus		= "";
	private bool	 isPayOk		= false;
	
	void Awake(){
		Instance = this;
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

#region Local
	/// <summary>
	/// check input format is email.
	/// </summary>
	/// <returns>
	/// true/false
	/// </returns>
	public bool GetEmailValid(){
		string sEmail = "";
		sEmail = _UI_CS_Login.Instance.m_login_NameEditText.text;
		Regex r = new Regex(@"^\s*([A-Za-z0-9_+-]+(\.\w+)*@([\w-]+\.)+\w{2,10})\s*$");
		if(r.IsMatch(sEmail)){
			return true;
		}else{
			return false;
		}
	}
	
	/// <summary>
	/// Checks the riceive msg.no use json lib
	/// </summary>
	/// <returns>
	/// The error.
	/// </returns>
	/// <param name='msg'>
	/// Message.
	/// </param>
	private int CheckErr(string msg){
		//temp
		//{"uid":"0","message":"error"}
		string[] ele = msg.Split(',');
		string[] eleSon = ele[1].Split(':');
		string tempString = "";
		tempString = eleSon[1].Substring(1,eleSon[1].Length-2);
		LogManager.Log_Info("CheckErr:"+tempString);
		if(tempString.Contains("error")){
			return 1;
		}else{
			return 0;
		}
	}
	
	/// <summary>
	/// Checks the success for pay.
	/// </summary>
	/// <returns>
	/// The success for pay.
	/// </returns>
	/// <param name='msg'>
	/// If set to <c>true</c> message.
	/// </param>
	private bool CheckSuccessForPay(string msg){
		//temp
		//{"message":"success","surplus":1380}
		string[] ele = msg.Split(',');
		string[] eleSon = ele[2].Split(':');
		string tempString = "";
		tempString = eleSon[1].Substring(1,eleSon[1].Length-2);
		LogManager.Log_Info("CheckSuccessForPay:"+tempString);
		if(tempString.Contains("success")){
			isPayOk = true;
			return true;
		}else{
			isPayOk = false;
			return false;
		}
	}
	
	private bool CheckVerErr(string msg){
		string[] ele = msg.Split(',');
		string[] ele1 = ele[2].Split(':');
		string tempString = "";
		tempString = ele1[1].Substring(1,ele1[1].Length-2);
		LogManager.Log_Info("CheckVerErr:"+tempString);
		if(tempString.Contains("vererr")){
			return false;
		}else{
			return true;
		}
	}
	
	/// <summary>
	/// check receive msg.
	/// Does not contain A. STEAM login please go to steamworks.cs
	/// </summary>
	/// <returns>
	/// The login info.
	/// </returns>
	/// <param name='msg'>
	/// If set to <c>true</c> message.
	/// </param>
	private bool InitLoginInfo(string msg){
		if(!CheckVerErr(msg)){
			LocalizeManage.Instance.GetDynamicText(_UI_CS_PopupBoxCtrl.Instance.m_popUp_MSGText,"VERSIONERR");
			_UI_CS_PopupBoxCtrl.Instance.m_CS_popUpBox.BringIn();
			return false;
		}
		string[] ele = msg.Split(',');
		uid 		= GetUid(ele[0]);
		LogManager.Log_Info("uid:"+uid);
		token 		= GetToken(ele[1]);
		LogManager.Log_Info("token:"+token);
		gameCode 	= GetGameCode(ele[3]);
		LogManager.Log_Info("gameCode:"+gameCode);
		ip 			= GetIP(ele[2]);
		LogManager.Log_Info("ip:"+ip);
		port 		= GetPort(ele[2]);
		LogManager.Log_Info("port:"+port);
		key 		= GetKey(ele[4]);
		LogManager.Log_Info("key:"+key);
		return true;
	}
	
	public string GetUID(string sourceID) {
		return sourceID.Substring(0,sourceID.Length-2);
	}
	
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
	
	private void InitPayInfo(string msg){
		string[] ele = msg.Split(',');
		LogManager.Log_Info("InitPayInfo;msg:"+ele[3]);
		surplus 	 = GetSurplus(ele[3]);
		LogManager.Log_Info("surplus:"+surplus);
		token 	 	 = GetPayToken(ele[0]);
		LogManager.Log_Info("token:"+token);
		key 	 	 = GetPaykey(ele[1]);
		LogManager.Log_Info("key:"+key);
	}
	
	string GetPaykey(string msg){
		string[] ele = msg.Split(':');
		return ele[1].Substring(1,ele[1].Length-2);
	}
	
	string GetPayToken(string msg){
		string[] ele = msg.Split(':');
		return ele[1].Substring(1,ele[1].Length-2);
	}
	
	private string GetSurplus(string msg){
		string[] ele = msg.Split(':');
		return ele[1].Substring(0,ele[1].Length-1);
	}
	
	public void ShowPaySurplus(string karma){
		if(isPayOk){
			LocalizeManage.Instance.GetDynamicText(_UI_CS_PopupBoxCtrl.Instance.m_popUp_MSGText,"PURCHASEOK");
			_UI_CS_PopupBoxCtrl.Instance.m_popUp_MSGText.Text += surplus;
			_UI_CS_PopupBoxCtrl.Instance.m_CS_popUpBox.BringIn();
		}else{
			_UI_CS_PopupBoxCtrl.Instance.m_CS_popUpBox.Dismiss();
			string payMoneyUrl = GetPayMoneyUrl(karma);
			LogManager.Log_Warn(payMoneyUrl);
			UrlOpener.Open(payMoneyUrl);
		}
	}
	
	private string GetPayMoneyUrl(string karma){
		string url = "http://spicyworld.spicyhorse.com/client.php?mod=payment&uid="+uid+"&token="+token+"&game=akaneiro&currency="+karma+"&key="+key;
		return url;
	}
	
	private void ShowPayUnlockMapSurplus(string karma){
		if(isPayOk){
			LocalizeManage.Instance.GetDynamicText(_UI_CS_PopupBoxCtrl.Instance.m_popUp_MSGText,"PURCHASEOK");
			_UI_CS_PopupBoxCtrl.Instance.m_popUp_MSGText.Text += surplus;
			_UI_CS_PopupBoxCtrl.Instance.m_CS_popUpBox.BringIn();
		}else{
			_UI_CS_PopupBoxCtrl.Instance.m_CS_popUpBox.Dismiss();
			string payMoneyUrl = GetPayUnlockMapUrl(karma);
			LogManager.Log_Warn(payMoneyUrl);
			UrlOpener.Open(payMoneyUrl);
		}
	}
	
	private string GetPayUnlockMapUrl(string karma){
		string url = "http://spicyworld.spicyhorse.com/client.php?mod=payment&uid="+uid+"&token="+token+"&game=akaneiro&currency="+karma+"&key="+key;
		return url;
	}
#endregion
	
#region Interface
	/// <summary>
	/// Sends the message to server for login.
	/// Does not contain A. STEAM login please go to steamworks.cs
	/// </summary>
	/// <returns>
	/// The message to server for login.
	/// </returns>
	public IEnumerator SendMsgToServerForLogin(){
		LocalizeManage.Instance.GetDynamicText(_UI_CS_PopupBoxCtrl.Instance.m_popUp_MSGText,"CONTOSERVER");
		_UI_CS_PopupBoxCtrl.Instance.m_CS_popUpBox.BringIn();
		WWWForm handelServer = new WWWForm();
		handelServer.AddField("email",_UI_CS_Login.Instance.m_login_NameEditText.text);
		handelServer.AddField("password",_UI_CS_Login.Instance.m_login_PassWordEditText.text);
		if(isConnectTestServer){
			handelServer.AddField("game",testServerName);
		}else{
			handelServer.AddField("game",generalName);
		}
		handelServer.AddField("other",WebLoginCtrl.Instance.MyVersion);
		WWW url = new WWW(loginUrl,handelServer);
		yield return url;
		if(url.error != null){
			Debug.LogWarning("Error link server."+url.text);
			if(url.text != null) {
				_UI_CS_PopupBoxCtrl.Instance.m_popUp_MSGText.Text = url.text;
				_UI_CS_PopupBoxCtrl.Instance.m_CS_popUpBox.BringIn();
			}else {
				_UI_CS_PopupBoxCtrl.Instance.m_popUp_MSGText.Text = "server return null.";
				_UI_CS_PopupBoxCtrl.Instance.m_CS_popUpBox.BringIn();
			}
		}else{
			int tmepErr = CheckErr(url.text);
			if( tmepErr == 0){
				_UI_CS_PopupBoxCtrl.Instance.m_CS_popUpBox.Dismiss();
				Debug.LogWarning("link server ok :receive zzx msg");
				receiveMsg = url.text;
				if(InitLoginInfo(url.text)){
					Login();
				}
			}else if( tmepErr == 1){
				LocalizeManage.Instance.GetDynamicText(_UI_CS_PopupBoxCtrl.Instance.m_popUp_MSGText,"ACCORPASSERR");
				_UI_CS_PopupBoxCtrl.Instance.m_CS_popUpBox.BringIn();
			}
		}
	}
	
	/// <summary>
	/// Sends the message to server for pay.
	/// Does not contain A. STEAM login please go to steamworks.cs
	/// </summary>
	/// <returns>
	/// The message to server for pay.
	/// </returns>
	/// <param name='karma'>
	/// Karma.
	/// </param>
	public IEnumerator SendMsgToServerForPay(string karma){
		WWWForm handelServer = new WWWForm();
		handelServer.AddField("uid",uid);
		handelServer.AddField("token",token);
		if(isConnectTestServer){
			handelServer.AddField("game",testServerName);
		}else{
			handelServer.AddField("game",generalName);
		}
		handelServer.AddField("currency",karma);
		WWW url = new WWW(payUrl,handelServer);
		yield return url;
		if(url.error != null){
			Debug.LogWarning("Error link server."+url.error);
			_UI_CS_PopupBoxCtrl.Instance.m_popUp_MSGText.Text = url.text;
			_UI_CS_PopupBoxCtrl.Instance.m_CS_popUpBox.BringIn();
		}else{
			Debug.LogWarning("link server ok :receive zzx msg");
			CheckSuccessForPay(url.text);
			receiveMsg = url.text;
			InitPayInfo(url.text);
			ShowPaySurplus(karma);
		}
	}
	
	/// <summary>
	/// Sends the message to server for pay unlock map.
	/// now this function no use.
	/// </summary>
	/// <returns>
	/// The message to server for pay unlock map.
	/// </returns>
	/// <param name='karma'>
	/// Karma.
	/// </param>
	public IEnumerator SendMsgToServerForPayUnlockMap(string karma){
		WWWForm handelServer = new WWWForm();
		handelServer.AddField("uid",uid);
		handelServer.AddField("token",token);
		if(isConnectTestServer){
			handelServer.AddField("game",testServerName);
		}else{
			handelServer.AddField("game",generalName);
		}
		handelServer.AddField("currency",karma);
		WWW url = new WWW(payUrl,handelServer);
		yield return url;
		if(url.error != null){
			Debug.LogWarning("Error link server."+url.text);
			_UI_CS_PopupBoxCtrl.Instance.m_popUp_MSGText.Text = url.text;
			_UI_CS_PopupBoxCtrl.Instance.m_CS_popUpBox.BringIn();
		}else{
			Debug.LogWarning("link server ok :receive zzx msg");
			Debug.LogWarning(url.text);
			CheckSuccessForPay(url.text);
			receiveMsg = url.text;
			InitPayInfo(url.text);
			ShowPayUnlockMapSurplus(karma);
		}
	}
	
	/// <summary>
	/// Login use it.but first you need init ip,port and password.
	/// </summary>
	public void Login(){
		CS_Main.Instance.g_commModule.Connect(ip,short.Parse(port));
		LocalizeManage.Instance.GetDynamicText(_UI_CS_PopupBoxCtrl.Instance.m_popUp_MSGText,"CON");
		_UI_CS_PopupBoxCtrl.Instance.m_CS_popUpBox.BringIn();
	}
#endregion	
}
