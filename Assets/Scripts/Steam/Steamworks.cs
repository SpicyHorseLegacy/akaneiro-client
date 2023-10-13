using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using ManagedSteam;
using ManagedSteam.Exceptions;
using ManagedSteam.CallbackStructures;
using ManagedSteam.SteamTypes;
using ManagedSteam.Utility;
using System.Runtime.InteropServices;

public class Steamworks : MonoBehaviour
{
	public bool isSteamWork = false;
	public string steamID;
	public string steamWebServiceHost = "http://mw.angry-red.com";
	
	public string sessionKeyResponse = "Connecting...";
	
	public string curGameLang = "";
	public string purchaseKey = "";
	
	// session
	private IntPtr sessionPtr;
	private string sessionKey = string.Empty;
	private byte[] sessionArray = new byte[512];
	private uint sessionLength;
	private int sessionInt;
	
	private string character = string.Empty;
	
	/// <summary>
	/// Use this property to access the Steamworks API and active instance
	/// </summary>
	public static Steam SteamInterface { get; private set; }
	public static Steamworks activeInstance { get; private set; }

#region Public
	public void GetAuthSession() {
		GetAuthSessionTicket();
		Debug.Log("GetAuthSession(): new steam ticket request is started, waiting for steam callback");
	}
	
	public void RefreshUser() {
		StartCoroutine(SendMsgToServerForUserRefresh());
		Debug.Log("RefreshUser(): refresh user coroutine is started");
	}
	
	public void ShowShop(string group_id) {
		string tGameCode = "";
#if NGUI
		tGameCode = VersionManager.Instance.gameCode;
#else
		tGameCode = ClientLogicCtrl.Instance.gameCode;
#endif
		UrlOpener.Open(steamWebServiceHost + 
			"/client/steam/shop?steam_id=" + steamID + 
			"&token=" + tGameCode +
			"#" + group_id
		);
		Debug.Log("ShowShop(): opening steam browser");
	}
	
	public void StartPayment(string text) {
		StartCoroutine(SendMsgToServerForTransactionsInit(text));
		Debug.Log("StartPayment(): new transaction coroutine is started");
	}
	
	public void DLCValidate(string character) {
		this.character = character;
		StartCoroutine(SendMsgToServerForDLCValidate());
		Debug.Log("DLCValidate(): dlc validate coroutine is started");
	}
#endregion

#region Private
	/// <summary>
	/// akaneiro steam web server authorization coroutine
	/// </summary>
	private IEnumerator SendMsgToServerForSteamLogin() {
		WWWForm form = new WWWForm();
		form.AddField("ticket", sessionKey);
		WWW request = new WWW(steamWebServiceHost + "/client/steam/user/authorize", form);
		yield return request;
		
		Dictionary<string,object> success = ProcessRequest(request);
		if (success == null)
			yield break;
		
		if (success.ContainsKey("token") && success.ContainsKey("server")) {
			// grab incoming data
			string token = success["token"].ToString();
			string server = success["server"].ToString();
#if NGUI
			VersionManager.Instance.ip = GetIP(server);
			VersionManager.Instance.port = GetPort(server);
			VersionManager.Instance.gameCode = token;
			LoginScreenCtrl.Instance.ConnectServer(VersionManager.Instance.ip,VersionManager.Instance.port);
			VersionManager.Instance.SetPlatformType(EUserType.eUserType_Steam);
#else
			// set client info
			ClientLogicCtrl.Instance.ip = GetIP(server);
			ClientLogicCtrl.Instance.port = GetPort(server);
			ClientLogicCtrl.Instance.gameCode = token;
			ClientLogicCtrl.Instance.isClientVer = true;
			// set service variables
			Platform.Instance.platformType.Set(EUserType.eUserType_Steam);
			// finalize login routine
			ClientLogicCtrl.Instance.Login();
#endif
			steamID = SteamInterface.User.GetSteamID().AsUInt64.ToString();
			InvokeRepeating("RefreshUser", 60*30, 60*30);
		} else {
			Debug.LogError("SendMsgToServerForSteamLogin(): Incomplete data recieved" );
#if NGUI
			PopUpBox.PopUpErr("WEB Server error: Incomplete data recieved.");
#else
			_UI_CS_PopupBoxCtrl.PopUpError("WEB Server error: Incomplete data recieved");
#endif
		}
	}
	
	private IEnumerator SendMsgToServerForUserRefresh() {
		string tGameCode = "";
#if NGUI
		tGameCode = VersionManager.Instance.gameCode;
#else
		tGameCode = ClientLogicCtrl.Instance.gameCode;
#endif
		WWWForm form = new WWWForm();
		form.AddField("steam_id", steamID);
		form.AddField("token", tGameCode);
		WWW request = new WWW(steamWebServiceHost + "/client/steam/user/refresh", form);
		yield return request;
		
		Dictionary<string,object> success = ProcessRequest(request);
		if (success == null)
			yield break;
		
		if (success.ContainsKey("token")) {
			// grab incoming data
			string token = success["token"].ToString();
			// set client info
#if NGUI
			VersionManager.Instance.gameCode = token;
#else
			ClientLogicCtrl.Instance.gameCode = token;
#endif	
		} else {
			Debug.LogError("SendMsgToServerForSteamLogin(): Incomplete data recieved" );
#if NGUI
			PopUpBox.PopUpErr("WEB Server error: Incomplete data recieved");
#else
			_UI_CS_PopupBoxCtrl.PopUpError("WEB Server error: Incomplete data recieved");
#endif
		}
	}
	
	/// <summary>
	/// akaneiro steam web server transaction init coroutine
	/// </summary>
	private IEnumerator SendMsgToServerForTransactionsInit(string text) {
		string tGameCode = "";
#if NGUI
		tGameCode = VersionManager.Instance.gameCode;
#else
		tGameCode = ClientLogicCtrl.Instance.gameCode;
#endif
		WWWForm form = new WWWForm();
		form.AddField("steam_id", steamID);
		form.AddField("token", tGameCode);
		form.AddField("item_id", text);
		WWW request = new WWW(steamWebServiceHost + "/client/steam/transaction/init", form);
		yield return request;
		
		Dictionary<string,object> success = ProcessRequest(request);
		if (success == null)
			yield break;
		
		if (success.ContainsKey("msg")) {
			Debug.Log("SendMsgToServerForTransactionsInit(): " + success["msg"].ToString());
		} else {
			Debug.LogError("SendMsgToServerForTransactionsInit(): Incomplete data recieved" );
#if NGUI
			PopUpBox.PopUpErr("WEB Server error: Incomplete data recieved");
#else
			_UI_CS_PopupBoxCtrl.PopUpError("WEB Server error: Incomplete data recieved");
#endif
		}
	}
	
	/// <summary>
	/// akaneiro steam web server transaction complete coroutine
	/// </summary>
	private IEnumerator SendMsgToServerForTransactionsComplete(ulong orderid) {
		string tGameCode = "";
#if NGUI
		tGameCode = VersionManager.Instance.gameCode;
#else
		tGameCode = ClientLogicCtrl.Instance.gameCode;
#endif
		WWWForm form = new WWWForm();
		form.AddField("steam_id", steamID);
		form.AddField("character", character);
		form.AddField("token", tGameCode);
		form.AddField("order_id", orderid.ToString());
		WWW request = new WWW(steamWebServiceHost + "/client/steam/transaction/complete", form);
		yield return request;
		
		Dictionary<string,object> success = ProcessRequest(request);
		if (success == null)
			yield break;
		
		if (success.ContainsKey("msg")) {
			Debug.Log("SendMsgToServerForTransactionsComplete(): " + success["msg"].ToString());
		} else {
			Debug.LogError("SendMsgToServerForTransactionsComplete(): Incomplete data recieved" );
#if NGUI
			PopUpBox.PopUpErr("WEB Server error: Incomplete data recieved");
#else
			_UI_CS_PopupBoxCtrl.PopUpError("WEB Server error: Incomplete data recieved");
#endif
		}
	}
	
	private IEnumerator SendMsgToServerForDLCValidate() {
		string tGameCode = "";
#if NGUI
		tGameCode = VersionManager.Instance.gameCode;
#else
		tGameCode = ClientLogicCtrl.Instance.gameCode;
#endif
		// Request DLCs count
		int dlc_count = SteamInterface.Apps.GetDLCCount();
		if (dlc_count < 1) {
			Debug.Log("SendMsgToServerForTransactionsComplete(): user doesn't own any dlc.");
			yield break;
		}
		// Populate DLCs
		string dlcs = "";
		for (int i=0; i<dlc_count; i++) {
			AppsGetDLCDataByIndexResult dlc = SteamInterface.Apps.GetDLCDataByIndex(i);
			Debug.Log("SendMsgToServerForDLCValidate(): Adding DLC to request: " + dlc.Name + " appid: " + dlc.AppID);
			dlcs += dlc.AppID;
			if (i<(dlc_count-1)) {
				dlcs += ",";
			}
		}
		
		WWWForm form = new WWWForm();
		form.AddField("steam_id", steamID);
		form.AddField("token", tGameCode);
		form.AddField("character", character);
		form.AddField("dlcs", dlcs);
		WWW request = new WWW(steamWebServiceHost + "/client/steam/dlc/validate", form);
		yield return request;
		
		Dictionary<string,object> success = ProcessRequest(request);
		if (success == null)
			yield break;
		
		if (success.ContainsKey("msg")) {
			Debug.Log("SendMsgToServerForDLCValidate(): " + success["msg"].ToString());
		} else {
			Debug.LogError("SendMsgToServerForDLCValidate(): Incomplete data recieved" );
			_UI_CS_PopupBoxCtrl.PopUpError("WEB Server error: Incomplete data recieved");
		}
	}
#endregion

#region UnityPrivate
	private void Awake()
	{
		if(!isSteamWork) {
			return;
		}
		
	}
	
	private void Start() {
		if(!isSteamWork) {
			return;
		}

		initSteamSDK();
		sessionPtr = Marshal.AllocHGlobal(512);
	}
		
	private void Update() {
		if (SteamInterface != null)
		{
			// Makes sure that callbacks are sent.
			// Make sure that you call this from some other place if you use 'Time.timeScale = 0' 
			// to pause the game.
			SteamInterface.Update();
		}
	}
	
	private void OnDestroy()
	{
		// Only cleanup if the object being destroyed "owns" the Steam instance.
		if (activeInstance == this)
		{
			activeInstance = null;
			Cleanup();
		}
	}

	private void OnApplicationQuit()
	{
		// Always cleanup when shutting down
		Cleanup();
	}

	private void Cleanup()
	{
		if (SteamInterface != null)
		{
			if (Application.isEditor)
			{
				// Only release managed handles if we run from inside the editor. This enables us 
				// to use the library again without restarting the editor.
				SteamInterface.ReleaseManagedResources();
			}
			else
			{
				// We are running from a standalone build. Shutdown the library completely
				SteamInterface.Shutdown();
			}
			
			if(sessionPtr != null) {
				Marshal.FreeHGlobal(sessionPtr);
			}
			SteamInterface = null;
		}
	}
#endregion

#region SteamPrivate
	private void initSteamSDK() {
		bool error = false;
		string status = "Unknown";
		
		// Makes sure that only one instance of this object is in use at a time
		if (SteamInterface == null)
		{
			try
			{
				// Starts the library. This will, and can, only be done once.
				SteamInterface = Steam.Initialize();
			}
			catch (AlreadyLoadedException e)
			{
				status = "The native dll is already loaded, this should not happen if ReleaseManagedResources is used and Steam.Initialize() is only called once.";
				Debug.LogError(status, this);
				Debug.LogException(e, this);
				error = true;
			}
			catch (SteamInitializeFailedException e)
			{
				status = "Could not initialize the native Steamworks API. This is usually caused by a missing steam_appid.txt file or if the Steam client is not running.";
				Debug.LogError(status, this);
				Debug.LogException(e, this);
				error = true;
			}
			catch (SteamInterfaceInitializeFailedException e)
			{
				status = "Could not initialize the wanted versions of the Steamworks API. Make sure that you have the correct Steamworks SDK version. See the documentation for more info.";
				Debug.LogError(status, this);
				Debug.LogException(e, this);
				error = true;
			}
			catch (DllNotFoundException e)
			{
				status = "Could not load a dll file. Make sure that the steam_api.dll/libsteam_api.dylib file is placed at the correct location. See the documentation for more info.";
				Debug.LogError(status, this);
				Debug.LogException(e, this);
				error = true;
			}

			if (error)
			{
				SteamInterface = null;
				// Set style
				// Set message
				string message = "UNABLE TO INITIALIZE STEAM SDK.\n\n";
				message += "Please make sure that you are attempting to run Akaneiro from within the Steam application.\n\n\n\n";
				message += "ERROR RETURNED BY STEAM SDK: \n\n";
				message += status + "\n\n\n\n";
				message += "If you think that this message is not your fault,\n";
				message += "please contact our technical support at http://support.spicyhorse.com/\n\n\n\n";
				message += "PRESS SPACE TO TERMINATE APPLICATION";
				BSOD.Fatal(message);
			}
			else
			{
				status = "Steamworks initialized and ready to use.";
				
				// Prevent destruction of this object
				GameObject.DontDestroyOnLoad(this);
				activeInstance = this;
				// set callbacks
				SteamInterface.ExceptionThrown += ExceptionThrown;
				SteamInterface.User.GetAuthSessionTicketResponse += AuthSessionTicketReq;
				SteamInterface.User.MicroTxnAuthorizationResponse += MicroTxnAuthorizationResponseCallback;
			}
		}
		else
		{
			// Another Steamworks object is already created, destroy this one.
			Destroy(this);
		}
	}
	
	private void GetAuthSessionTicket() {
		SteamInterface.User.GetAuthSessionTicket(sessionPtr, 512, out sessionLength);
	}
	
	/// <summary>
	/// Steam sdk callback for new session ticket
	/// </summary>
	private void AuthSessionTicketReq(GetAuthSessionTicketResponse req) {
		if(req.Result == Result.OK) {
			sessionKeyResponse = "Validating steam session ticket";
			for(int i = 0; i<sessionLength; i++) {
				sessionArray[i] = Marshal.ReadByte(sessionPtr, i);
			}
			sessionKey = Convert.ToBase64String(sessionArray, 0, (int)sessionLength);
			StartCoroutine(SendMsgToServerForSteamLogin());
		} else {
			sessionKeyResponse = "Steam session ticket request failed";
		}
	}
	
	private void MicroTxnAuthorizationResponseCallback(MicroTxnAuthorizationResponse response) {
		StartCoroutine(SendMsgToServerForTransactionsComplete(response.OrderID));
	}
	
	/// <summary>
	/// This method is called when an exception have been thrown from native code.
	/// We print the exception so we can see what is went wrong.
	/// </summary>
	private void ExceptionThrown(Exception e) {
		Debug.LogException(e, this);
	}
#endregion

#region PrivateHelpers
	private Dictionary<string,object> ProcessRequest(WWW request) {
		if (request.error != null)
		{
//			Debug.LogError("WEB Server communication error: " + request.error);
#if NGUI
			PopUpBox.PopUpErr("WEB Server communication error: " + request.error);
#else
			_UI_CS_PopupBoxCtrl.PopUpError("WEB Server communication error: " + request.error);
#endif
		}
		else
		{
			Dictionary<string,object> response = MiniJSONN.Json.Deserialize(request.text) as Dictionary<string, object>;
			
			if (response == null) {
#if NGUI
			PopUpBox.PopUpErr("WEB Server response is invalid. Please notify support.");
#else
				_UI_CS_PopupBoxCtrl.PopUpError("WEB Server response is invalid. Please notify support.");
#endif
			}
			else if (response.ContainsKey("success"))
			{
				Dictionary<string,object> ret = response["success"] as Dictionary<string,object>;
				if (ret == null) {
#if NGUI
					PopUpBox.PopUpErr("WEB Server is insane. Please notify support.");
#else
					_UI_CS_PopupBoxCtrl.PopUpError("WEB Server is insane. Please notify support.");
#endif
				}
				return ret;
			} 
			else if (response.ContainsKey("error"))
			{
				Dictionary<string, object> error = response["error"] as Dictionary<string, object>;
				if (error.ContainsKey("msg")) {
#if NGUI
					PopUpBox.PopUpErr("WEB Server error: " + error["msg"].ToString());
#else
					_UI_CS_PopupBoxCtrl.PopUpError("WEB Server error: " + error["msg"].ToString());
#endif
				} else {
#if NGUI
					PopUpBox.PopUpErr("WEB Server error: UNKNOWN ERROR");
#else
					_UI_CS_PopupBoxCtrl.PopUpError("WEB Server error: UNKNOWN ERROR");
#endif
				}
			} else {
#if NGUI
					PopUpBox.PopUpErr("WEB Server is insane. Please notify support.");
#else
				_UI_CS_PopupBoxCtrl.PopUpError("WEB Server is insane. Please notify support.");
#endif
			}
		}
		return null;
	}
	
	private string GetIP(string text) {
		string[] ele = text.Split(':');
		return ele[0];
	}
	
	private string GetPort(string text) {
		string[] ele = text.Split(':');
		return ele[1];
	}
#endregion
}
