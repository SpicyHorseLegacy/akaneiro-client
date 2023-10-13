using System;
using System.Collections;

public class CommModule : INetworkEventHandler
{
	private TcpSession        _session = null;
    private TcpSessionFactory _factory = null;

    public CommModule()
    {
        _factory = new TcpSessionFactory(this);
    }

    public void    Connect(string ip, short port)
    {
		LogManager.Log_Warn("Connect to server.Ip:"+ip+" Port:"+port);
        if (_session != null)
        {
            Console.WriteLine("Already connected, disconnect first!");
            _session.Close();

            return;
        }

        _session = _factory.CreateSession(ip, port);
    }

    public bool     SendMessage(Packet packet)
    {
        if (_session == null)
        {
            Console.WriteLine("Not connected yet!");
            return false;
        }

		_session.SendMessageToGame(packet.GetByteStream());

        return true;
    }

// 	public bool SendMessage(Packet packet)
// 	{
// 		if (_session == null)
// 		{
// 			Console.WriteLine("Not connected yet!");
// 			return false;
// 		}
// 
// 
// 		_session.SendMessage(
// 			ProtocolGame_SendRequest.SendMsgToGame(packet).GetByteStream()
// 			);
// 
// 		return true;
// 	}

    public void     Shutdown()
    {
        if (_session != null)
        {
            _session.Close();

            return;
        }

        Console.WriteLine("Not connected yet!");
    }

    public void     Update()
    {
        if (_factory != null)
        {
            _factory.Update();
        }
    }

	public int		GetPing()
	{
		if (_session == null)
		{
			Console.WriteLine("Not connected yet!");
			return -1;
		}

		return _session.GetlastPing();
	}

    public void     SendTestMessage()
    {
        Packet pak = new Packet();
        int size = 0;
        pak.WriteInt(size);
        
        short type = 0;
        pak.WriteShort(type);

        string strMsg = "This is a test msg.";
        pak.WriteString(strMsg);

        size = pak.get_used_size() - 6;
        pak.Patch(size, 0);

		SendMessage(pak);
    }


    public void     OnConnect(TcpSession socket, ESessionError errCode) {
		LogManager.Log_Debug("Connected server...");
		#region err
        if (errCode != ESessionError.SUCCESS)
        {
#if NGUI
			GUILogManager.LogErr("OnConnect fail. err:" + errCode);
			if(PopUpBox.isShowDisConnectText) {
				PopUpBox.PopUpErr(LocalizeManage.Instance.GetDynamicText("CONABORTED"));
			}
#else
			_UI_CS_FightScreen.Instance.m_DisconnectPanel.Dismiss();
			LocalizeManage.Instance.GetDynamicText(OptionCtrl.Instance.giftMsg,"CONABORTED");
			LogManager.Log_Warn("Cannot connect to server!" + errCode);
			_UI_CS_TestModeCtrl.Instance.Info.Text = errCode.ToString();
            return;
#endif
			_session.Close();
            _session = null;
        }
		#endregion
#if NGUI
		switch(VersionManager.Instance.GetVersionType()) {
		case VersionType.SteamClientVersion:
			if(Steamworks.activeInstance!= null) {
				if(Steamworks.activeInstance.isSteamWork) {
					SendMessage(
						ProtocolGame_SendRequest.UserLogin(
							Steamworks.activeInstance.steamID,
							VersionManager.Instance.GetPlatformType(),
							VersionManager.Instance.GetClientType(),					
							"",
							VersionManager.Instance.gameCode
						)
					);
				}
			}
			break;
		case VersionType.TestServerWebVersion:
		case VersionType.WebVersion:
            SendMessage(ProtocolGame_SendRequest.UserLogin(DataManager.Instance.GetMapValue(DataListType.AccountData, "uid"),
				VersionManager.Instance.GetPlatformType(),
				VersionManager.Instance.GetClientType(),
				"",
				DataManager.Instance.GetMapValue(DataListType.AccountData,"token")));	
			break;
		case VersionType.NormalClientVersion:
			SendMessage(ProtocolGame_SendRequest.UserLogin(DataManager.Instance.GetMapValue(DataListType.AccountData,"uid"),
				VersionManager.Instance.GetPlatformType(),
				VersionManager.Instance.GetClientType(),
				"",
				DataManager.Instance.GetMapValue(DataListType.AccountData,"gameCode")));	
			break;
		default:
			SendMessage(ProtocolGame_SendRequest.UserLogin(DataManager.Instance.GetMapValue(DataListType.AccountData,"account"),
				VersionManager.Instance.GetPlatformType(),
				VersionManager.Instance.GetClientType(),
				DataManager.Instance.GetMapValue(DataListType.AccountData,"password"),
				""));
			break;
		}
#else
		#region Steam
		if(Steamworks.activeInstance!= null) {
			if(Steamworks.activeInstance.isSteamWork) {
				SendMessage(
					ProtocolGame_SendRequest.UserLogin(
					Steamworks.activeInstance.steamID,
					Platform.Instance.platformType,
					Platform.Instance.clientType,					
					"",					
					// this gamecode == token.
					//why use gamecode,you can ask ZhuZongXiang. Token alread use spicyHors buy someing.
					ClientLogicCtrl.Instance.gameCode					
					)
				);
				return;
			}
		}
		#endregion
		if(_UI_CS_TestModeCtrl.Instance.IsTestMode){
			#region test mode
			SendMessage(
				ProtocolGame_SendRequest.UserLogin(
					_UI_CS_TestModeCtrl.Instance.m_login_NameEditText.Text,
					Platform.Instance.platformType,
					Platform.Instance.clientType,
					_UI_CS_TestModeCtrl.Instance.m_login_PassWordEditText.Text,
					""
				)
			);
			#endregion
		}else{
			if(WebLoginCtrl.Instance.IsWebLogin){
				#region spicyhorse web version
				SendMessage(
					ProtocolGame_SendRequest.UserLogin(_UI_CS_Login.Instance.m_login_NameEditText.Text,Platform.Instance.platformType,Platform.Instance.clientType,"",_UI_CS_Login.Instance.Token));
				#endregion
			}else if(ClientLogicCtrl.Instance.isClientVer){
				#region log
				//debug info
				LogManager.Log_Warn("@uid:"+ClientLogicCtrl.Instance.uid);
				LogManager.Log_Warn("@platformType:"+Platform.Instance.platformType.Get().ToString());
				LogManager.Log_Warn("@gameCode:"+ClientLogicCtrl.Instance.gameCode);
				#endregion
				#region client version 
				SendMessage(
					ProtocolGame_SendRequest.UserLogin(ClientLogicCtrl.Instance.uid,Platform.Instance.platformType,Platform.Instance.clientType,"",ClientLogicCtrl.Instance.gameCode));
				#endregion
			}else{
				#region editor
				LogManager.Log_Warn("name:" +_UI_CS_Login.Instance.m_login_NameEditText.Text);
				SendMessage(
					ProtocolGame_SendRequest.UserLogin(_UI_CS_Login.Instance.m_login_NameEditText.Text,Platform.Instance.platformType,Platform.Instance.clientType,
											   _UI_CS_Login.Instance.m_login_PassWordEditText.Text,""));
				#endregion
			}
		}
#endif
    }

    public bool     OnReceive(TcpSession socket, byte[] byteMsg)
    {
        //Console.WriteLine("OnReceive: {0}", byteMsg.Length);
        //Debug.Log("OnReceive");

        Packet pak = new Packet(byteMsg);

        if( !ProtocolGame_ReceiveCallback.Parse(_sessionGameClient, pak) )
        {
			pak = new Packet(byteMsg);
			if (!ProtocolBattle_ReceiveCallback.Parse(_sessionBattleClient, pak))
			{
				Console.WriteLine("OnReceive parse failed!");
			}
        }
        
        return true;
    }

    public void OnClose(TcpSession socket, ESessionError errCode) {
        _session = null;
#if NGUI
		GUILogManager.LogErr("OnClose. err:" + errCode.ToString());
		GameManager.Instance.SetDisconnectFlag(true);
		if(GameManager.Instance.GetCheatFlag()) {
			GameManager.Instance.SetCheatFlag(false);
			PopUpBox.PopUpErr(LocalizeManage.Instance.GetDynamicText("DISCONCHEAT"));
		}else {
			if(PopUpBox.isShowDisConnectText) {
				PopUpBox.PopUpErr(LocalizeManage.Instance.GetDynamicText("DISFROMSERVER"));
			}
		}
#else
		LogManager.Log_Debug("--- OnClose() errcCode : " + errCode.ToString() + " ---");
		_UI_CS_FightScreen.Instance.isCheckPing = false;
		if(_UI_CS_Ctrl.Instance.m_isCheat){
			_UI_CS_Ctrl.Instance.m_isCheat = false;
			LocalizeManage.Instance.GetDynamicText(_UI_CS_FightScreen.Instance.disConnectText,"DISCONCHEAT");
			_UI_CS_FightScreen.Instance.m_DisconnectPanel.BringIn();
		}else{
			LocalizeManage.Instance.GetDynamicText(_UI_CS_FightScreen.Instance.disConnectText,"DISFROMSERVER");
			_UI_CS_FightScreen.Instance.m_DisconnectPanel.BringIn();
		}
#endif
    }

    
	//private ClientCSharp.Logic.SessionLobbyClient _sessionLobbyClient = new ClientCSharp.Logic.SessionLobbyClient();
	private ClientCSharp.Logic.SessionGameClient _sessionGameClient = new ClientCSharp.Logic.SessionGameClient();
	private ClientCSharp.Logic.SessionBattleClient _sessionBattleClient = new ClientCSharp.Logic.SessionBattleClient();
}
