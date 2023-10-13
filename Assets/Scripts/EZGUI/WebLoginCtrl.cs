using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;


public class WebLoginCtrl : MonoBehaviour {
	//Instance
	public static WebLoginCtrl Instance = null;
	public bool	  IsWebLogin = false;
	
	private string loginURL = string.Empty;
	private string loginAPI = string.Empty;
	
	public  string  MyVersion = "";
	public static string globalVersion ;
	
	private bool	isVersionOk = true;
	
	void Awake(){
		Instance = this;
		globalVersion = MyVersion;
	}
	
	// Use this for initialization
	void Start () {
//		if(IsWebLogin){
//	 		StartCoroutine(NotifyLoadDone());
//			_UI_CS_Login.Instance.m_login_LoginS.Dismiss();
//		}else{
//			_UI_CS_Login.Instance.m_login_LoginS.BringIn();
//		}
	}
	
	public IEnumerator NotifyLoadDone()
    {
        yield return 0;
		LogManager.Log_Info("NotifyLoadDone");
		LocalizeManage.Instance.GetDynamicText(_UI_CS_PopupBoxCtrl.Instance.m_popUp_MSGText,"LOADINGPWAM");
		_UI_CS_PopupBoxCtrl.Instance.m_CS_popUpBox.BringIn();
        yield return new WaitForSeconds(2f);
		LogManager.Log_Info("send <load_player_done> to web server.");
        Application.ExternalCall("load_player_done");
		_UI_CS_PopupBoxCtrl.Instance.m_CS_popUpBox.Dismiss();
        yield break;
    }
	
	public void APIUrl(string _url)
    {
		loginURL = _url;
        LogManager.Log_Info("api " + _url);
    }
	
	public void API1(string _url)
	{
		loginAPI = _url;
	}

	 public void User(string _user)
	 {
		LogManager.Log_Info("user " + _user);
		ClientLogicCtrl.Instance.uid = _user;
		_UI_CS_Login.Instance.m_login_NameEditText.Text = _user;
	 }
                
    public void Token(string _token)
    {
		LogManager.Log_Info("Token " + _token);
		_UI_CS_Login.Instance.Token = _token;
		
    }
	
	public void _MessageType(string _msg)
    {
        LogManager.Log_Info("Message: " + _msg);
		ClientLogicCtrl.Instance._msg = _msg;
    }
	
    public void Auth(string _codes)
    {
        LogManager.Log_Info("Auth: " + _codes);
    }

    public void SetType(int _type)
    {
        LogManager.Log_Info("set type string: " + _type);
		Platform.Instance.platformType.Set(_type);
    }
	
	public void Version(string _ver)
    {
		LogManager.Log_Info("Version " + _ver);
		if(0 == string.Compare(MyVersion,_ver)){
			isVersionOk = true;
		}else{
			isVersionOk = false;
		}
    }

    public void SetSSL(int _type)
    {
        LogManager.Log_Info("set SSL : " + _type);
    }
                
    public void Links(string a_ipandprot)
    {
        LogManager.Log_Info("Ip and port: " + a_ipandprot);  
    }

    public void Links2(string ip, string p)
    {
        LogManager.Log_Info("ip port:" + ip + " " + p);
    }
	
	public void LoginLogic(string _ip, string _port){
		if(isVersionOk){
			CS_Main.Instance.g_commModule.Connect(_ip,short.Parse(_port));
//			_UI_CS_PopupBoxCtrl.Instance.m_popUp_MSGText.Text = "Connecting...";
			LocalizeManage.Instance.GetDynamicText(_UI_CS_PopupBoxCtrl.Instance.m_popUp_MSGText,"CON");
		}else{
//			_UI_CS_PopupBoxCtrl.Instance.m_popUp_MSGText.Text = "The client and server versions do not match.Please update your client.";
			LocalizeManage.Instance.GetDynamicText(_UI_CS_PopupBoxCtrl.Instance.m_popUp_MSGText,"VERSIONERR");
		}
		_UI_CS_PopupBoxCtrl.Instance.m_CS_popUpBox.BringIn();
	
	}
	
	[HideInInspector]
    public bool mNeedLogin = true;
    public void NeedLogin(string needLogin)
    {
        mNeedLogin = needLogin != "0" ? true : false;

        if (!mNeedLogin)
        {
          //StartCoroutine(WaitLogin());
			StartCoroutine(selectServer());
        }
    }
	
    IEnumerator selectServer()
    {
        //string url = loginURL + loginAPI + "?user=" + userID + "&type=" + MenuObject.Instance.LoginType + "&codes=" + MenuObject.Instance.Token + "&ip=" + _ip + "&port=" + _port;
		string url = loginURL + loginAPI + "?user=" + _UI_CS_Login.Instance.m_login_NameEditText.text + "&type=" + Platform.Instance.platformType.Get() + "&codes=" + _UI_CS_Login.Instance.Token + "&message="+ClientLogicCtrl.Instance._msg;

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
			//1
			_UI_CS_PopupBoxCtrl.Instance.PopUpDeleBox("Login timeout!",RefreshCallback);
        }
        else if (desc == "SystemError" || desc == "AlreadyOnline" || desc == "LoginInProgress" )
        {
			//2
//			_UI_CS_PopupBoxCtrl.Instance.PopUpDeleBox("SystemError",selectServer);
			selectServer();
		}
        else if (desc == "InvalidRegion")
        {
            LogManager.Log_Error("Could not connect to the server: " + desc);
            //1   
			_UI_CS_PopupBoxCtrl.Instance.PopUpDeleBox("Could not connect to the server: " + desc,RefreshCallback);
        }
        else
		{
			string[] sArray = desc.Split(':');
			if (sArray.Length == 2)
			{
				LogManager.Log_Info("ipandport" + sArray[0] + ":" + sArray[1]);
				_UI_CS_Login.Instance.m_login_IPEditText.text = sArray[0];
				_UI_CS_Login.Instance.Port = sArray[1];
				LoginLogic(sArray[0], sArray[1]);
			}
			else
			{
				LogManager.Log_Error("Could not connect to the server: " + desc);
				//1
				_UI_CS_PopupBoxCtrl.Instance.PopUpDeleBox("Could not connect to the server: " + desc,RefreshCallback);
			}
		}
    }
	
	public void RefreshCallback(){
	
		Application.ExternalCall("refresh_page");
	
	}
	
	
	
//check_version(){

	
}
