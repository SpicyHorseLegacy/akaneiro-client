using UnityEngine;
using System.Collections;

public class RevivePanel : MonoBehaviour {
	
	public static RevivePanel Instance;
	public UIPanel basePanel;
	void Awake(){
		Instance = this;
	}

	public UIButton villageBtn;
	public UIButton karmaBtn;
	public UIButton crystalBtn;
	public UIButton scrollBtn;
	public UIButton restartBtn;
	// Use this for initialization
	void Start () {
		villageBtn.AddInputDelegate(VillageDelegate);
		karmaBtn.AddInputDelegate(KarmaDelegate);
		crystalBtn.AddInputDelegate(CrystalDelegate);
		scrollBtn.AddInputDelegate(ScrollDelegate);
		restartBtn.AddInputDelegate(RestartDelegate);
		InitPunishmentReward();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

#region Interface
	public SpriteText RevivalKarmaText;
	public SpriteText MissionCompleteTolRd;
	public void AwakeRevival(){
		IngameMenuBack();
		_UI_CS_FightScreen.Instance.m_CS_Ingame_NormalPanel.Dismiss();
		_UI_MiniMap.Instance.isShowSmallMap	 = false;
		_UI_MiniMap.Instance.isShowBigMap	 = false;
		_UI_CS_ScreenCtrl.Instance.SetNextScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_REVIVAL_MENU);
		basePanel.BringIn();
		int reviveKarma = 0;
		reviveKarma = GetRevivalKarma();
		if(reviveKarma == 0){
			LocalizeManage.Instance.GetDynamicText(RevivalKarmaText,"FREE");
		}else{
			RevivalKarmaText.Text = reviveKarma.ToString();
		}
		InitImg();
		ShowRealMoneyBtn();
		UpdateMatList();
		UpdateMissionEarning();
	}
	
	//if you use karma revival, mission complete (total (ingame)karma and exp) * it/100.//
	private int [] punishmentReward = {0,0,0,0,0,0,0};
	private void InitPunishmentReward() {
		string _fileName = LocalizeManage.Instance.GetLangPath("RevivalCost.Costs");
		TextAsset item = (TextAsset)Resources.Load(_fileName, typeof(TextAsset));
		string[] itemRowsList = item.text.Split('\n');
		for (int i = 3; i < itemRowsList.Length; ++i){
			string pp = itemRowsList[i];
		   	string[] vals = pp.Split(new char[] { '	', '	' });	
			if(int.Parse(vals[0]) == 0){
				punishmentReward[0] = int.Parse(vals[1]);
				punishmentReward[1] = int.Parse(vals[2]);
				punishmentReward[2] = int.Parse(vals[3]);
				punishmentReward[3] = int.Parse(vals[4]);
				punishmentReward[4] = int.Parse(vals[5]);
				punishmentReward[5] = int.Parse(vals[6]);
				punishmentReward[6] = int.Parse(vals[7]);
				return;
			}
		}
	}
	
	public int GetPunismentReward(int count) {
		if(count < 0) count = 0;
		if(count > punishmentReward.Length - 1)count = punishmentReward.Length - 1;
		
		return punishmentReward[count];
	}
	
	public int GetPunismentReward() {
		return GetPunismentReward(RevivalCount);
	}
	
	private int RevivalCount = 0;
	public void SetRevivalCount(int count) {
		if(count < 0) count = 0;
		if(count > punishmentReward.Length - 1)count = punishmentReward.Length - 1;
		
		RevivalCount = count;
	}
	public int GetRevivalCount() {
		return RevivalCount;
	}
	public void AddRevivalCount() {
		RevivalCount++;
	}
	
	public SpriteText reviveItemCountText;
	public void UpdateRevivalItemCount(int count) {
		reviveItemCountText.Text = count.ToString();
	}
#endregion
#region Local
	public int GetRevivalKarma(){
		string _fileName = LocalizeManage.Instance.GetLangPath("RevivalCost.Costs");
		TextAsset item = (TextAsset)Resources.Load(_fileName, typeof(TextAsset));
		string[] itemRowsList = item.text.Split('\n');
		int idx = 0;
		//because first time is 0;//
		idx = RevivalCount+1;
		for (int i = 3; i < itemRowsList.Length; ++i){
			string pp = itemRowsList[i];
		   	string[] vals = pp.Split(new char[] { '	', '	' });	
			if(int.Parse(vals[0]) == _PlayerData.Instance.playerLevel){
				if((idx)>7){
					idx = 7;
				}
				return int.Parse(vals[(idx)]);
			}
		}
		return 0;
	}
	
	private EReviveType reviveType = new  EReviveType();
	void VillageDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
			case POINTER_INFO.INPUT_EVENT.TAP:	
				_UI_CS_MissionLogic.Instance.MissionBgPanel.Dismiss();
				_UI_CS_Teleport.Instance.HideTelport();
				RevivePanel.Instance.basePanel.Dismiss();
				_UI_CS_FightScreen.Instance.m_CS_Ingame_NormalPanel.Dismiss();
				Player.Instance.FSM.ChangeState(Player.Instance.IS);
				_UI_CS_ScreenCtrl.Instance.SetNextScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_LOADING);
				_UI_MiniMap.Instance.isShowSmallMap	 = false;
				_UI_MiniMap.Instance.isShowBigMap	 = false;
				_UI_CS_MissionLogic.Instance.RsetMissionScore();
				_UI_CS_LoadProgressCtrl.Instance.m_LoadingMapName.Text = _UI_CS_MapScroll.Instance.SceneNameToMapName("Hub_Village");
				_UI_CS_LoadProgressCtrl.Instance.AwakeLoading(0);
				CS_Main.Instance.g_commModule.SendMessage(ProtocolBattle_SendRequest.EnterScene("Hub_Village"));
			break;
		}	
	}
	void KarmaDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
			case POINTER_INFO.INPUT_EVENT.TAP:	
				reviveType.Set(EReviveType.eReviveType_Money);
				CS_Main.Instance.g_commModule.SendMessage(ProtocolBattle_SendRequest.ReviveReq(reviveType));
			break;
		}	
	}
	void CrystalDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
			case POINTER_INFO.INPUT_EVENT.TAP:	
				reviveType.Set(EReviveType.eReviveType_Crystal);
				 CS_Main.Instance.g_commModule.SendMessage(ProtocolBattle_SendRequest.ReviveReq(reviveType));
			break;
		}	
	}
	void ScrollDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
			case POINTER_INFO.INPUT_EVENT.TAP:	
				reviveType.Set(EReviveType.eReviveType_Item);
				CS_Main.Instance.g_commModule.SendMessage(ProtocolBattle_SendRequest.ReviveReq(reviveType));
			break;
		}	
	}
	void RestartDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
			case POINTER_INFO.INPUT_EVENT.TAP:	
				
			break;
		}	
	}
	
	//when player death, first close the ingame menu//
	void IngameMenuBack(){
		_AbiMenuCtrl.Instance.UpDateIngameAbilitiesIcon();
		_UI_CS_IngameMenu.Instance.m_CS_Ingame_MenuPanel.Dismiss();
		_UI_CS_FightScreen.Instance.m_CS_Ingame_NormalPanel.BringIn();
		_UI_CS_ScreenCtrl.Instance.SetNextScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_INGAME_NORMAL);
		Inventory.Instance.BagPanel.HideBag();
		Inventory.Instance.bagItemArray[0].CancelPress();
		SurveillanceCamera.Instance.ShutDown();
		if(null != ScenesLightCtrl.Instance)
		ScenesLightCtrl.Instance.OpenLight();
		_UI_CS_DownLoadPlayerForInv.Instance.CloseLight();
		_UI_CS_IngameMenu.Instance.isLockInvLogic = false;
	}
	
	//if scroll enough,show scroll btn.else show crystal btn//
	void ShowRealMoneyBtn() {
		int count = int.Parse(reviveItemCountText.text);
		if(count>0) {
			scrollBtn.transform.localPosition = new Vector3(0,0,0.1f);
			crystalBtn.transform.localPosition = new Vector3(0,0,999f);
		}else {
			scrollBtn.transform.localPosition = new Vector3(0,0,999f);
			crystalBtn.transform.localPosition = new Vector3(0,0,0.1f);
		}
	}
	
	public  UIListItemContainer itemContainer;
	public  UIScrollList list;
	void UpdateMatList() {
		list.ClearList(true);
		for(int i = 0;i<MissionComplete.Instance.materialsList.Count;i++) {
			AddMatToRevival(MissionComplete.Instance.materialsList[i]);
		}
	}
	
	private void AddMatToRevival(ItemDropStruct mat) {
		//check this item is already.
		for(int i = 0;i<list.Count;i++) {
			if(list.GetItem(i).gameObject.GetComponent<Materials>().info._ItemID == mat._ItemID) {
				list.GetItem(i).gameObject.GetComponent<Materials>().count++;
				list.GetItem(i).gameObject.GetComponent<Materials>().countText.Text = list.GetItem(i).gameObject.GetComponent<Materials>().count.ToString();
				return;
			}
		}
		// new material.//
		UIListItemContainer item = (UIListItemContainer)list.CreateItem((GameObject)itemContainer.gameObject);
		list.clipContents = true; list.clipWhenMoving = true;
		item.GetComponent<Materials>().info = mat;
		item.GetComponent<Materials>().count = 1;
		item.GetComponent<Materials>().countText.Text = "1";
		ItemPrefabs.Instance.GetItemIcon(mat._ItemID,mat._TypeID,mat._PrefabID,item.GetComponent<Materials>().icon);
//		LogManager.Log_Error("list count:"+list.Count);
	}
	
	public SpriteText karmaText;
	public SpriteText expText;
	void UpdateMissionEarning() {
		expText.Text = _UI_CS_MissionLogic.Instance.MissionScore.ToString();
		karmaText.Text = _UI_CS_MissionLogic.Instance.MissionKarma.ToString();
		MissionCompleteTolRd.Text = GetPunismentReward().ToString();
	}
	
	public UIButton titleImg;
	private void InitImg() {
		TextureDownLoadingMan.Instance.DownLoadingTexture("LevelUP_P_LevleUP",titleImg.transform);
	}
#endregion
	
}
