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

public class MissionSelectScreenCtrl : BaseScreenCtrl
{
	public static MissionSelectScreenCtrl Instance;

    override protected void Awake() { base.Awake(); Instance = this; }
	
	#region Local
	#region event create and destory
    override protected void DestoryAllEvent()
    {
        base.DestoryAllEvent();
		if(_MissionSelectBaseManager.Instance) {
			_MissionSelectBaseManager.Instance.OnCloseDelegate -= Exit;
			_MissionSelectBaseManager.Instance.OnSetUITextureDelegate -= SetTextureIcon;
		}
		if(_MissSeleWin.Instance) {
			_MissSeleWin.Instance.OnHuntDelegate -= Hunt;
		}
	}

    #region Events
    protected override void RegisterSingleTemplateEvent(string _templateName)
    {
        base.RegisterSingleTemplateEvent(_templateName);

        if (_templateName == "MissionSelectBase" && _MissSeleWin.Instance)
        {
            if (_MissionSelectBaseManager.Instance)
            {
                _MissionSelectBaseManager.Instance.OnCloseDelegate += Exit;
                _MissionSelectBaseManager.Instance.OnSetUITextureDelegate += SetTextureIcon;
            }
            if (_MissSeleWin.Instance)
            {
                _MissSeleWin.Instance.OnHuntDelegate += Hunt;
            }
            _MissSeleWin.Instance.Hide(false);
            InitArea();
        }
        if (_templateName == "SpeedUpBox" && SpeedUpBoxManager.Instance)
        {
            SpeedUpBoxManager.Instance.Hide();
            SpeedUpBoxManager.Instance.SetReceiveSerTime(PlayerDataManager.Instance.GetReceiveSerMsgTime());
            foreach (CoolDownCost data in PlayerDataManager.Instance.coolDownCostList)
            {
                //SpeedUpBoxManager.Instance.cdList.Add(data);
            }
        }

        if(_templateName == "MoneyBar" && MoneyBarManager.Instance) {
			MoneyBarManager.Instance.OnAddKarmaDelegate +=	AddKarmaDelegate;
			MoneyBarManager.Instance.OnAddCrystalDelegate += AddCrystalDelegate;
			MoneyBarManager.Instance.SetKarmaVal(PlayerDataManager.Instance.GetKarmaVal());
			MoneyBarManager.Instance.SetCrystalVal(PlayerDataManager.Instance.GetCrystalVal());
		}
		if (_templateName == "KarmaRecharge" && KarmaRechargeManager.Instance) {
			KarmaRechargeManager.Instance.OnExitDelegate +=	ExitRechargeKarmaDelegate;
			KarmaRechargeManager.Instance.OnAddKarmaDelegate += RechargeKarmaValDelegate;
			InitKarmaRechargeData();
		}
		if (_templateName == "CrystalRecharge" && CrystalRechargeManager.Instance) {
			CrystalRechargeManager.Instance.OnExitDelegate +=	ExitRechargeCrystalDelegate;
			CrystalRechargeManager.Instance.OnAddKarmaDelegate += RechargeKarmaValDelegate;		
			InitCrystalRechargeData();
		}
    }
    
	override protected void UnregisterSingleTemplateEvent(string _templateName)
    {
        base.UnregisterSingleTemplateEvent(_templateName);

        if (_templateName == "MissionSelectBase" && _MissSeleWin.Instance)
        {
            if (_MissionSelectBaseManager.Instance)
            {
                _MissionSelectBaseManager.Instance.OnCloseDelegate -= Exit;
                _MissionSelectBaseManager.Instance.OnSetUITextureDelegate -= SetTextureIcon;
            }
            if (_MissSeleWin.Instance)
            {
                _MissSeleWin.Instance.OnHuntDelegate -= Hunt;
            }
        }

		if(_templateName == "MoneyBar" && MoneyBarManager.Instance) {
			MoneyBarManager.Instance.OnAddKarmaDelegate -=	AddKarmaDelegate;
			MoneyBarManager.Instance.OnAddCrystalDelegate -= AddCrystalDelegate;
		}
		if (_templateName == "KarmaRecharge" && KarmaRechargeManager.Instance) {
			KarmaRechargeManager.Instance.OnExitDelegate -=	ExitRechargeKarmaDelegate;
			KarmaRechargeManager.Instance.OnAddKarmaDelegate -= RechargeKarmaValDelegate;
		}
		if (_templateName == "CrystalRecharge" && CrystalRechargeManager.Instance) {
			CrystalRechargeManager.Instance.OnExitDelegate -=	ExitRechargeCrystalDelegate;
			CrystalRechargeManager.Instance.OnAddKarmaDelegate -= RechargeKarmaValDelegate;			
		}
    }
    #endregion
	#endregion
	private void Init() {
		
		
	}
	
	string informationDisplay = "info";
	
	void OnGUI()
	{
		GUI.Label(new Rect(10, 10, 100, 100), informationDisplay);	
	}
	
	private void InitArea() {
		foreach(_MissSpirte area in _MissionSelectBaseManager.Instance.areas) {
			SAcceptMissionRelate2 tSerData = PlayerDataManager.Instance.GetSerMissData(area.missID);
			MissDescData tLocalData = PlayerDataManager.Instance.GetMissDescData(area.missID+1);
			
			if(tSerData != null && tLocalData != null) {
				MissionSeleData tData = new MissionSeleData();
				tData.serData = tSerData;
				tData.localData = tLocalData;
				area.SetMissData(tData);
				Debug.LogWarning ("MISSION =====>" + area.missID);
				informationDisplay = area.missID.ToString();
				
				if (Steamworks.activeInstance != null && Steamworks.activeInstance.isSteamWork)
				{
					Steam.Instance.Stats.SetAchievement(area.missID.ToString());
					Steam.Instance.Stats.StoreStats();
				}
			}else {
				area.Hide(true);
			}
		}
		
		/*
		for (int c=0; c< _MissionSelectBaseManager.Instance.areas.Length-1; c++)
		{
			if (PlayerDataManager.Instance.GetSerMissData(_MissionSelectBaseManager.Instance.areas[c].missID) != null)
			{
			if (PlayerDataManager.Instance.GetSerMissData(_MissionSelectBaseManager.Instance.areas[c].missID).IsHaveCompleted != 0)
				{
					if (_MissionSelectBaseManager.Instance.areas[c+1] != null)
					{
						SAcceptMissionRelate2 tSerData = PlayerDataManager.Instance.GetSerMissData(_MissionSelectBaseManager.Instance.areas[c+1].missID);
						MissDescData tLocalData = PlayerDataManager.Instance.GetMissDescData(_MissionSelectBaseManager.Instance.areas[c+1].missID+1);
						MissionSeleData tData = new MissionSeleData();
						tData.serData = tSerData;
						tData.localData = tLocalData;
						_MissionSelectBaseManager.Instance.areas[c+1].SetMissData(tData);
						Debug.LogWarning ("MISSION =====>" + _MissionSelectBaseManager.Instance.areas[c+1].missID);
						informationDisplay = _MissionSelectBaseManager.Instance.areas[c+1].missID.ToString();
					
						if (Steamworks.activeInstance != null && Steamworks.activeInstance.isSteamWork)
						{
							Steam.Instance.Stats.SetAchievement(_MissionSelectBaseManager.Instance.areas[c+1].missID.ToString());
							Steam.Instance.Stats.StoreStats();
						}
					}
					else
					{
						_MissionSelectBaseManager.Instance.areas[c+1].Hide(true);
					}
				}
			}
		}
		*/
	}
	
	private void Exit() {
		GameCamera.BackToPlayerCamera();
		Player.Instance.ReactivePlayer();
		GUIManager.Instance.ChangeUIScreenState("IngameScreen");
	}
	
	private void Hunt() {
		CS_Main.Instance.g_commModule.SendMessage(ProtocolGame_SendRequest.AcceptMission(1001,_MissSeleWin.Instance.GetCurMissID()));
	}
	
	private void SetTextureIcon(string imgName,UITexture missionIcon) {
		if(string.Compare(imgName,"0") != 0) {
			TextureDownLoadingMan.Instance.DownLoadingTexture(imgName,missionIcon);
		}
	}
	
	private bool isPopUpRecharge = false;
	private void ExitRechargeKarmaDelegate() {
		GUIManager.Instance.RemoveTemplate("KarmaRecharge");
		isPopUpRecharge = false;
	}
	private void ExitRechargeCrystalDelegate() {
		GUIManager.Instance.RemoveTemplate("CrystalRecharge");
		isPopUpRecharge = false;
	}
	private void RechargeKarmaValDelegate(string content) {
		if (Steamworks.activeInstance != null) {
			Steamworks.activeInstance.StartPayment(content);
		}
		switch(VersionManager.Instance.GetVersionType()) {
		case VersionType.WebVersion:
			Application.ExternalCall("select_gold", content);
			break;
		case VersionType.NormalClientVersion:
			StartCoroutine(VersionManager.Instance.SendMsgToServerForPay(content));
			break;
		default:
			StartCoroutine(VersionManager.Instance.SendMsgToServerForPay(content));
			break;
		}
	}
	private void AddKarmaDelegate() {
		if(isPopUpRecharge) {
			return;
		}
		if (Steamworks.activeInstance) {
			Steamworks.activeInstance.ShowShop("karma");
		}else {
		GUIManager.Instance.AddTemplate("KarmaRecharge");
		isPopUpRecharge = true;
		}
	}
	private void AddCrystalDelegate() {
		if(isPopUpRecharge) {
			return;
		}
		if (Steamworks.activeInstance) {
			Steamworks.activeInstance.ShowShop("crystal");
		}else {
			GUIManager.Instance.AddTemplate("CrystalRecharge");
			isPopUpRecharge = true;
		}
	}
	
	private void InitKarmaRechargeData() {
		KarmaRechargeManager.Instance.SetKarmaInfo(PlayerDataManager.Instance.karmaRechargTitle);
		for(int i = 0;i<7;i++) {
			KarmaRechargeManager.Instance.SetKarmaVal(i,PlayerDataManager.Instance.rechargeValData.karmaVal[i]);
			KarmaRechargeManager.Instance.SetPayVal(i,PlayerDataManager.Instance.rechargeValData.karmaPayVal[i]);
		}
	}
	private void InitCrystalRechargeData() {
		CrystalRechargeManager.Instance.SetKarmaInfo(PlayerDataManager.Instance.crystalRechargTitle);
		for(int i = 0;i<7;i++) {
			CrystalRechargeManager.Instance.SetKarmaVal(i,PlayerDataManager.Instance.rechargeValData.crystalVal[i]);
			CrystalRechargeManager.Instance.SetPayVal(i,PlayerDataManager.Instance.rechargeValData.crystalPayVal[i]);
		}
	}
	#endregion
}
