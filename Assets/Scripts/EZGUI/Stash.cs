using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class stashInfo {
	public SItemInfo	  serInfo;
	public ItemDropStruct dropInfo;
	public bool isEmpty = true;
}

public class Stash : MonoBehaviour {
	
	public static Stash Instance = null;
	
	void Awake() {
		Instance = this;
	}
	
	[SerializeField]
	private UIButton exitBtn;
	[SerializeField]
	private UIButton newBoxBtn;
	[SerializeField]
	private UIButton buyExitBtn;
	[SerializeField]
	private UIButton buyBoxSureBtn;
	// Use this for initialization
	void Start () {
		ReadStashPice();
		InitList();
		exitBtn.AddInputDelegate(ExitBtnDelegate);
		newBoxBtn.AddInputDelegate(NewBoxBtnDelegate);
		buyExitBtn.AddInputDelegate(BuyExitBtnDelegate);
		buyBoxSureBtn.AddInputDelegate(BuyBoxSureBtnDelegate);
		tabs[0].AddInputDelegate(box1Delegate);
		tabs[1].AddInputDelegate(box2Delegate);
		tabs[2].AddInputDelegate(box3Delegate);
		tabs[3].AddInputDelegate(box4Delegate);
		tabs[4].AddInputDelegate(box5Delegate);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	#region Interface 
	[SerializeField]
	private UIPanel BasePanel;
	[SerializeField]
	private Transform StashSound;
	public void AwakeStash() {
		_UI_CS_ScreenCtrl.Instance.SetNextScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_STASH);
		_UI_CS_FightScreen.Instance.m_CS_Ingame_NormalPanel.Dismiss();
		BasePanel.BringIn();
		StartCoroutine(BringBag());
		MoneyBadgeInfo.Instance.Hide(false);
		Player.Instance.FreezePlayer();
		UpdateStashInfo();
		SoundCue.PlayPrefabAndDestroy(StashSound);
	}
	
	public stashInfo GetStashInfo(int solt) {
		return list[solt];
	}
	
	public void AddItemToStash(int solt,stashInfo itemInfo) {
		list[solt] = itemInfo;
		UpdateStashInfo();
	}
	public void DelItemFromStash(int solt) {
		list[solt].isEmpty = true;
		UpdateStashInfo();
	}
	[SerializeField]
	private UIRadioBtn [] tabs;
	private int maxTab = 1;
	
	[SerializeField]
	private SpriteText pice;
	public void SetMaxTab(int size) {
//		LogManager.Log_Error("size:"+size);
		maxTab = size;
		if(maxTab >= 5) {
			maxTab = 5;
			newBoxBtn.controlIsEnabled = false;
			newBoxBtn.SetColor(_UI_Color.Instance.color3);
			return;
		}
		pice.Text =stashPice[maxTab].ToString();
	}
	public void UpdateTabState() {
		for(int i =0;i<5;i++) {
			tabs[i].controlIsEnabled = false;
		}
		for(int i =0;i<maxTab;i++) {
			tabs[i].controlIsEnabled =true;
		}
		tabs[curBoxIdx].Value = true;
	}
	
	public void InitStashInfo(SBagInfo info) {
		curBoxIdx = 0;
		SetMaxTab((int)(info.capacity/12));
		UpdateTabState();
		foreach (SItemInfo iteminfo in info.iteminfos) {	
			ItemDropStruct tempItem = ItemDeployInfo.Instance.GetItemObject(iteminfo.ID,iteminfo.perfrab,iteminfo.gem,iteminfo.enchant,iteminfo.element,(int)iteminfo.level);
			stashInfo temp = new stashInfo();
			temp.dropInfo = tempItem;
			temp.serInfo = iteminfo;
			temp.isEmpty = false;
			list[iteminfo.slot] = temp;
		}
		UpdateStashInfo();
	}
	
	public void DismissBuyBoxPanel() {
		buyBoxPanel.Dismiss();
	}
	
	public _UI_CS_InventoryItem GetStashSlot(int slot) {
		return itemGroup.m_InventoryGroup[slot];
	}
	#endregion
	
	#region Local
	private void ExitBtnDelegate(ref POINTER_INFO ptr) {
		switch(ptr.evt){
			case POINTER_INFO.INPUT_EVENT.TAP:
				CloseStash();
			break;
		}
	}
	
	[SerializeField]
	private UIPanel buyBoxPanel;
	private void NewBoxBtnDelegate(ref POINTER_INFO ptr) {
		switch(ptr.evt){
			case POINTER_INFO.INPUT_EVENT.TAP:
				buyBoxPanel.BringIn();
			break;
		}
	}
		
	private void BuyExitBtnDelegate(ref POINTER_INFO ptr) {
		switch(ptr.evt){
			case POINTER_INFO.INPUT_EVENT.TAP:
				DismissBuyBoxPanel();
			break;
		}
	}
	
	private void BuyBoxSureBtnDelegate(ref POINTER_INFO ptr) {
		switch(ptr.evt){
			case POINTER_INFO.INPUT_EVENT.TAP:
				CS_Main.Instance.g_commModule.SendMessage(
				   		ProtocolGame_SendRequest.buyStash()
				);	
			break;
		}
	}

	private stashInfo[] list = new stashInfo[60];
	private void InitList() {
		for(int i = 0;i<list.Length;i++) {
			stashInfo temp = new stashInfo();
			list[i] = temp;
			list[i].isEmpty = true;
		}
	}
	
	private void CloseStash() {
		DissmissBag();
		BasePanel.Dismiss();
		_UI_CS_ScreenCtrl.Instance.SetNextScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_INGAME_NORMAL);
		_UI_CS_FightScreen.Instance.m_CS_Ingame_NormalPanel.BringIn();
		Player.Instance.ReactivePlayer();
        GameCamera.BackToPlayerCamera();
	}
	
	[SerializeField]
	private Transform bagPos;
	private IEnumerator BringBag() {
		yield return null;
		_UI_CS_BagCtrl.Instance.ob.transform.position = bagPos.transform.position;
	}
	private void DissmissBag() {
		_UI_CS_BagCtrl.Instance.Hide(true);
	}
	
	private int curBoxIdx = 0;
	public int GetCurSoltIdx(int slot) {
		return (curBoxIdx*12 + slot);
	}
	[SerializeField]
	private _UI_CS_InventoryItemGroup itemGroup;
	private void UpdateStashInfo() {
		int idx = 0;
		CleanStash();
		int starIdx = GetCurSoltIdx(0);
		for(int i = starIdx;i<(starIdx+12);i++) {
			idx = i%12;
			if(!list[i].isEmpty) {
				itemGroup.m_InventoryGroup[idx].ItemStruct = list[i].dropInfo;
				itemGroup.m_InventoryGroup[idx].m_ItemInfo = list[i].serInfo;
				itemGroup.m_InventoryGroup[idx].setIsEmpty(false);
				itemGroup.m_InventoryGroup[idx].SetItemTypeID(list[i].dropInfo._TypeID);
				ItemPrefabs.Instance.GetItemIcon(list[i].dropInfo._ItemID,list[i].dropInfo._TypeID,list[i].dropInfo._PrefabID,itemGroup.m_InventoryGroup[idx].m_MyIconBtn);
				ItemPrefabs.Instance.GetItemIcon(list[i].dropInfo._ItemID,list[i].dropInfo._TypeID,list[i].dropInfo._PrefabID,itemGroup.m_InventoryGroup[idx].ClonIcon);	
				itemGroup.m_InventoryGroup[idx].UpdateItemHighLevel();
				itemGroup.m_InventoryGroup[idx].UpdateCountText();
			}else {
				itemGroup.m_InventoryGroup[idx].ItemStruct = null;
				itemGroup.m_InventoryGroup[idx].setIsEmpty(true);
				itemGroup.m_InventoryGroup[idx].SetIconTexture(null);
				itemGroup.m_InventoryGroup[idx].SetClonIconTexture(null);
				itemGroup.m_InventoryGroup[idx].UpdateItemHighLevel();
				itemGroup.m_InventoryGroup[idx].UpdateCountText();
			}
			itemGroup.m_InventoryGroup[idx].HighLightBG.Hide(true);
			if(!list[i].isEmpty){
				float itemVal = (list[i].dropInfo.info_gemVal + list[i].dropInfo.info_encVal + list[i].dropInfo.info_eleVal);
				_UI_Color.Instance.SetNameColor(itemVal,itemGroup.m_InventoryGroup[idx].BG);
			}else {
				itemGroup.m_InventoryGroup[idx].BG.SetColor(_UI_Color.Instance.color10);
			}
			itemGroup.m_InventoryGroup[idx].UpdateItemHighLevel();
		}
	}
	private void CleanStash() {
		for(int i = 0;i<itemGroup.m_GroupItemsCount;i++) {
			itemGroup.m_InventoryGroup[i].m_IsEmpty = true;
		}
	}
	#region boxTab
	private void box1Delegate(ref POINTER_INFO ptr) {
		switch(ptr.evt){
			case POINTER_INFO.INPUT_EVENT.TAP:
				curBoxIdx = 0;
				UpdateStashInfo();
			break;
		}
	}
	private void box2Delegate(ref POINTER_INFO ptr) {
		switch(ptr.evt){
			case POINTER_INFO.INPUT_EVENT.TAP:
				curBoxIdx = 1;
				UpdateStashInfo();
			break;
		}
	}
	private void box3Delegate(ref POINTER_INFO ptr) {
		switch(ptr.evt){
			case POINTER_INFO.INPUT_EVENT.TAP:
				curBoxIdx = 2;
				UpdateStashInfo();
			break;
		}
	}
	private void box4Delegate(ref POINTER_INFO ptr) {
		switch(ptr.evt){
			case POINTER_INFO.INPUT_EVENT.TAP:
				curBoxIdx = 3;
				UpdateStashInfo();
			break;
		}
	}
	private void box5Delegate(ref POINTER_INFO ptr) {
		switch(ptr.evt){
			case POINTER_INFO.INPUT_EVENT.TAP:
				curBoxIdx = 4;
				UpdateStashInfo();
			break;
		}
	}
	
	private int [] stashPice = new int[5];
	private void ReadStashPice() {
		string _fileName = LocalizeManage.Instance.GetLangPath("stash.basic");
		TextAsset item = (TextAsset)Resources.Load(_fileName, typeof(TextAsset));
		string[] itemRowsList = item.text.Split('\n');
		for (int i = 3; i < itemRowsList.Length - 1; ++i){
			string pp = itemRowsList[i];
			string[] vals = pp.Split(new char[] { '	', '	' });
			if(i-3 < 5) {
				stashPice[i-3] = int.Parse(vals[1]);
			}
		}
	}
	#endregion
	#endregion
	
}
