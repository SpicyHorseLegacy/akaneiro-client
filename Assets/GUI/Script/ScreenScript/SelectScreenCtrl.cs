using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class SelectScreenCtrl : MonoBehaviour {
	
	public static SelectScreenCtrl Instance;
	
	void Awake() {
		Instance = this;
		RegisterInitEvent();
	}
	
	void Start () 
    {
        UI_BlackBackground_Control.CloseBlackBackground();
	}
		
	#region Interface
	public void InitCharaList() {
		SCharacterInfoLogin[] charList = PlayerDataManager.Instance.GetCharaLoginList().ToArray();
		SelectListManager.Instance.CleanList();
		PlayerModel pm = SelectBaseManager.Instance.GetPlayerModel();
		foreach(SCharacterInfoLogin info in charList) {
			SelectElementInfo tempInfo = new SelectElementInfo();
			tempInfo.name = info.nickname;tempInfo.sex = info.sex.Get();
			tempInfo.level = info.level;tempInfo.type = info.style;
			tempInfo.curExp = (long)info.Exp;tempInfo.maxExp = PlayerDataManager.Instance.GetMaxExp(info.level);
			tempInfo.ID = info.ID;
			SelectListManager.Instance.AddElement(tempInfo);
		}
		if(charList.Length > 0) {
			pm.Hide(false);
			SelectChara(charList[charList.Length-1].ID);
			SelectBaseManager.Instance.HideDelBtn(false);
		}else {
			pm.Hide(true);
			SelectBaseManager.Instance.HideDelBtn(true);
		}
	}
	#endregion
	
	#region Local
	#region event create and destory
	//MAX template count.//
	private int initDelegateCount = 2;
	private void TemplateInitEnd() {
		if(GUIManager.Instance.GetTemplateInitEndCount() >= initDelegateCount) {
			RegisterTemplateEvent();
			Init();
//			GUILogManager.LogErr("SelectScreenCtrl template init ok.");
		}
	}
	private void RegisterInitEvent() {
		GUIManager.Instance.OnInitEndDelegate += TemplateInitEnd;
		GUIManager.Instance.OnScreenManagerDestroy += DestoryAllEvent;
	}
	
	private void RegisterTemplateEvent() {
		if(SelectBaseManager.Instance) {
			SelectBaseManager.Instance.OnFullScreenDelegate += this.FullScreenDelegate;
		}
		if(SelectListManager.Instance) {
			SelectListManager.Instance.OnTypeIconChange += this.ChangePlayerTypeIcon;
			SelectListManager.Instance.OnSelectCharaChange += this.ChangeSelectChara;
			SelectListManager.Instance.OnCreateBtnDelegate += this.CreateBtnDelegate;
			SelectBaseManager.Instance.OnDelBtnDelegate += this.DelBtnDelegate;
			SelectBaseManager.Instance.OnDelOKBtnDelegate += this.DelOKBtnDelegate;
			SelectBaseManager.Instance.OnPlayBtnDelegate += this.PlayBtnDelegate;
			SelectBaseManager.Instance.OnYesDelegate += this.TutorialYesBtnDelegate;
		}
	}
	
	private void DestoryAllEvent() {
		if(SelectBaseManager.Instance) {
			SelectBaseManager.Instance.OnFullScreenDelegate -= this.FullScreenDelegate;
		}
		if(SelectListManager.Instance) {
			SelectListManager.Instance.OnTypeIconChange -= this.ChangePlayerTypeIcon;
			SelectListManager.Instance.OnSelectCharaChange -= this.ChangeSelectChara;
			SelectListManager.Instance.OnCreateBtnDelegate -= this.CreateBtnDelegate;
			SelectBaseManager.Instance.OnDelBtnDelegate -= this.DelBtnDelegate;
			SelectBaseManager.Instance.OnDelOKBtnDelegate -= this.DelOKBtnDelegate;
			SelectBaseManager.Instance.OnPlayBtnDelegate -= this.PlayBtnDelegate;
			SelectBaseManager.Instance.OnYesDelegate -= this.TutorialYesBtnDelegate;
		}
		GUIManager.Instance.OnInitEndDelegate -= TemplateInitEnd;
		GUIManager.Instance.OnScreenManagerDestroy -= DestoryAllEvent;
	}
	#endregion
	
	private void TutorialYesBtnDelegate() {
		PlayBtnDelegate();
	}
	
	private void FullScreenDelegate(bool isChecked) {
        SelectBaseManager.Instance.SetFullScreenFlag(isChecked);
        GameManager.Instance.SetFullScreenFlag(isChecked);
	}
	
	private void ChangePlayerTypeIcon(int type,int sex,SelectElement obj) {
		string iconName =  PlayerDataManager.Instance.GetPlayerIcon(type,sex);
		obj.SetTypeIcon(iconName);
	}
	
	private void Init() {
        GameManager.Instance.SetFullScreenFlag(false);
        SelectBaseManager.Instance.SetFullScreenFlag(false);
		SelectBaseManager.Instance.Init();
		InitCharaList();
		if(TutorialMan.Instance.GetTutorialFlag()) {
			PlayBtnDelegate();
		}
		UI_Fade_Control.Instance.FadeIn();
	}
	
	[SerializeField]
	private SCharacterInfoLogin curSelectInfo;
	private void SelectChara(int id) {
		SCharacterInfoLogin charData = PlayerDataManager.Instance.GetCharaLoginIdx(id);
		curSelectInfo = charData;
		SelectBaseManager.Instance.UpdateCharacterAttrInfo(charData.attrVec);
		UpdatePlayerAllEquipment(charData.equipinfo,charData.sex);
	}
	public int GetCurSelectCharaID() {
		return curSelectInfo.ID;
	}
	
	public void DelBtnDelegate() {
		if(PlayerDataManager.Instance.GetCharaListCount() > 0) {
			SelectBaseManager.Instance.HideDeletePanle(false,curSelectInfo.nickname);
		}else {
			SelectBaseManager.Instance.HideDeletePanle(true,"");
		}
	}
	
	public void DelOKBtnDelegate() {
		CS_Main.Instance.g_commModule.SendMessage(
		   		ProtocolGame_SendRequest.UserDelCharacter(curSelectInfo.ID)
		);
		SelectBaseManager.Instance.HideDeletePanle(true,"");
	}
	
	public void CreateBtnDelegate() {
		UI_Fade_Control.Instance.FadeOutAndIn("CreateScreen");
		StartCoroutine(ChangeToCreateScreen());
	}
	
	public IEnumerator ChangeToCreateScreen() {	
		AsyncOperation async= Application.LoadLevelAsync("CharacterCreateNGUI");
		yield return async;
		GUILogManager.LogInfo("Screen To CharacterCreate screen");
	}
	
	public void ChangeSelectChara(SelectElementInfo data) {
		SelectChara(data.ID);
	}
	
	private void UpdatePlayerAllEquipment(vectorSItemuuid equipList,ESex _gender) {
		PlayerModel pm = SelectBaseManager.Instance.GetPlayerModel();
        pm.UpdatePlayerAllEquipment(equipList, _gender);
	}
	
	public void PlayBtnDelegate() {
		CS_Main.Instance.g_commModule.SendMessage(
	   		ProtocolGame_SendRequest.UserSelectCharacter(curSelectInfo.ID)
		);
		PopUpBox.PopUpErr(LocalizeManage.Instance.GetDynamicText("LOADING"));
	}
	#endregion
}
