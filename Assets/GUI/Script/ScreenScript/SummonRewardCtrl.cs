using UnityEngine;
using System.Collections;

public class SummonRewardCtrl : MonoBehaviour {

	public static SummonRewardCtrl Instance;
	
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
	#endregion
	
	#region Local
	#region event create and destory
	private void RegisterInitEvent() {
		GUIManager.Instance.OnInitEndDelegate += TemplateInitEnd;
		GUIManager.Instance.OnScreenManagerDestroy += DestoryAllEvent;
	}
	
	//MAX template count.//
	private int initDelegateCount = 1;
	private void TemplateInitEnd() {
		if(GUIManager.Instance.GetTemplateInitEndCount() >= initDelegateCount) {
			RegisterTemplateEvent();
			Init();
//			GUILogManager.LogErr("SummonRewardCtrl template init ok.");
		}
	}
	
	private void RegisterTemplateEvent() {
		if(SummonRewardManager.Instance) {
			SummonRewardManager.Instance.OnContinueDelegate += ContinueDelegate;
		}
	}
	
	private void DestoryAllEvent() {
		if(SummonRewardManager.Instance) {
			SummonRewardManager.Instance.OnContinueDelegate -= ContinueDelegate;
		}
		GUIManager.Instance.OnInitEndDelegate -= TemplateInitEnd;
		GUIManager.Instance.OnScreenManagerDestroy -= DestoryAllEvent;
	}
	#endregion
	private void Init() {
		InitSummonReward(PlayerDataManager.Instance.summonReward);
	}
	
	private void ContinueDelegate() {
		
	}
	
	private void InitSummonReward(mapFriendHireReward rewardInfo) {
		if(rewardInfo.Count != 0) {
			SummonRewardManager.Instance.ClearList();
			foreach (SFriendHireReward info in rewardInfo.Values){
				SummonRewardManager.Instance.AddElement(PlayerDataManager.Instance.GetPlayerIcon(info.style,info.sex),info.sk,info.exp);
			}
		}else {
			if(PlayerDataManager.Instance.GetMissionCompleteFlag()) {
				GUIManager.Instance.ChangeUIScreenState("SuccessScreen");
			}else {
				CS_Main.Instance.g_commModule.SendMessage(ProtocolGame_SendRequest.hasAssignedTalentPointReq());
			}
		}
	}
	#endregion
}
