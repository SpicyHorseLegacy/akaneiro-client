using UnityEngine;
using System.Collections;

public class SelectChara : MonoBehaviour {
	//Instance
	public static SelectChara Instance = null;
	public UIPanel					RootPanel;
	[SerializeField]
	public SCharacterInfoLogin [] 	CharacterList = new SCharacterInfoLogin[3];
	public UIButton 				ExitBtn;
	public UIButton 				ContinueBtn;
	public SelectCharaStats    []	SelectBtn;
	public _UI_CS_Hide  			Model;
	public UIButton  				DelBtn;
	
	public UIPanel 					DelMsgBoxPanel;
	public SpriteText				DelMsgText;
	public UIButton 				SureDelBtn;
	public UIButton 				CancelDelBtn;

	public UIPanel 					TutorialMsgBoxPanel;
	public UIButton 				SureTutBtn;
	public UIButton 				CancelTutBtn;
	
	public UIButton 				CreateCharaBtn;
	public UIButton 				clickLinkBtn;
	public UIButton 				kickStartkBtn;
	
//	public UIButton 				fullScreenBtn;
	public UIStateToggleBtn 		fullScreen;
	
	public UIButton  				bg;
	
	void Awake(){
		Instance = this;
	}
	
	public void AwakeSelectChara(){
//		Tutorial.Instance.isTutorial = false;
		TutorialMan.Instance.SetTutorialFlag(false);
		MoneyBadgeInfo.Instance.Hide(false);
		RootPanel.BringIn();
	}
	
	// Use this for initialization
	void Start () {
		InitImage();
		ClearCharaList();
		ContinueBtn.AddInputDelegate(ContinueBtnDelegate);
		DelBtn.AddInputDelegate(DelDelegate);
		SureDelBtn.AddInputDelegate(SureDelDelegate);
		CancelDelBtn.AddInputDelegate(CancelDelDelegate);
		SureTutBtn.AddInputDelegate(SureTutDelegate);
		CancelTutBtn.AddInputDelegate(CancelTutDelegate);
		CreateCharaBtn.AddInputDelegate(CreateCharaBtnDelegate);
		clickLinkBtn.AddInputDelegate(ClickLinkBtnDelegate);
		kickStartkBtn.AddInputDelegate(KickStartkBtnDelegate);
		fullScreen.AddInputDelegate(FullScreenBtnDelegate);
	}
	
	private void InitImage(){
		bg.SetUVs(new Rect(0,0,1,1));
		//downloading image
		TextureDownLoadingMan.Instance.DownLoadingTexture("Chara_Bk",bg.transform);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void ClearCharaList(){
		for(int i = 0;i<3;i++){
//			if(CharacterList[i] != null)
//				CharacterList[i].ID = 0;
			SelectBtn[i].SetStats(false,null);
			SelectBtn[i].GetComponent<UIRadioBtn>().Value = false;
		}
	}
	
	public int GetCurrentSelectIdx(){
		for(int i = 0;i<3;i++){
			if(SelectBtn[i].isSelect){
				return i;
			}
		}
		return -1;
	}
	
	void DelDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:
				DelMsgBoxPanel.BringIn();	
				DelMsgText.Text = SelectBtn[GetCurrentSelectIdx()].info.nickname + " ?";
				break;
		}	
	}
	
	void ContinueBtnDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
			case POINTER_INFO.INPUT_EVENT.TAP:
				LogManager.Log_Debug("--- SelectCharacter ---");
				int currentIdx = GetCurrentSelectIdx();
				if(-1 != currentIdx){
					CS_Main.Instance.g_commModule.SendMessage(
				   		ProtocolGame_SendRequest.UserSelectCharacter(SelectBtn[currentIdx].info.ID)
					);
//					_UI_CS_PopupBoxCtrl.Instance.m_popUp_MSGText.Text = "Loading...";
					LocalizeManage.Instance.GetDynamicText(_UI_CS_PopupBoxCtrl.Instance.m_popUp_MSGText,"LOADING");
					_UI_CS_PopupBoxCtrl.Instance.m_CS_popUpBox.BringIn();
				}
				break;
		}	
	}	
		
	void SureDelDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:
				int curIdx = GetCurrentSelectIdx();
				DelMsgBoxPanel.Dismiss();	
				if(-1 != curIdx){
					CS_Main.Instance.g_commModule.SendMessage(
				   		ProtocolGame_SendRequest.UserDelCharacter(SelectBtn[curIdx].info.ID)
					);
				}
				break;
		}	
	}	
	
	void CancelDelDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:
				DelMsgBoxPanel.Dismiss();
				break;
		}	
	}	
	
	void SureTutDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:
				int curIdx = GetCurrentSelectIdx();
//				Tutorial.Instance.isTutorial = true;
				TutorialMan.Instance.SetTutorialFlag(true);
//				Tutorial.Instance.stepOldLady = 0;
				CS_SceneInfo.Instance.gClientMonsterTutial = 0;
//				TutorialMsgBoxPanel.Dismiss();
				CS_Main.Instance.g_commModule.SendMessage(
			   		ProtocolGame_SendRequest.UserSelectCharacter(SelectBtn[curIdx].info.ID)
				);
				break;
		}	
	}	
	
	void CancelTutDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:
				int curIdx = GetCurrentSelectIdx();
//				Tutorial.Instance.isTutorial = false;
				TutorialMan.Instance.SetTutorialFlag(false);
				TutorialMsgBoxPanel.Dismiss();
				CS_Main.Instance.g_commModule.SendMessage(
					   		ProtocolGame_SendRequest.TutorialEnterReq(SelectBtn[curIdx].info.ID)
				);
				break;
		}	
	}	
	
	void CreateCharaBtnDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:
				RootPanel.Dismiss();
				MoneyBadgeInfo.Instance.Hide(true);
				_UI_CS_CreateMenu.Instance.m_CS_CharacterCreatePanel.BringIn();
				_UI_CS_CreateMenu.Instance.SelectDis(1);
				Model.Hide(true);	
				PlayerInfoBar.Instance.IsHideSommonAllyBtn(true,0);
				_UI_CS_ScreenCtrl.Instance.SetNextScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_CREATE);
				_UI_CS_CreateMenu.Instance.GotoCreaterScenes();
				GameCamera.Instance.gameCamera.camera.enabled = false;
				StartCoroutine(WaitPanelPass());
			break;
		}	
	}
	
	void ClickLinkBtnDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:
				if(ClientLogicCtrl.Instance.isClientVer){
					UrlOpener.Open("http://spicyworld.spicyhorse.com/social/forum.php?gid=47");
				}else{
					Application.ExternalEval("window.open('http://spicyworld.spicyhorse.com/social/forum.php?gid=47','_blank')");
				}
			break;
		}	
	}
	
	void KickStartkBtnDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:
				if(ClientLogicCtrl.Instance.isClientVer){
					UrlOpener.Open("http://tinyurl.com/kickaka");
				}else{
					Application.ExternalEval("window.open('http://tinyurl.com/kickaka','_blank')");
				}
			break;
		}	
	}
	
	void FullScreenBtnDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:
				OptionCtrl.Instance.IsFullScreen(OptionCtrl.Instance.isFullScreen);
			break;
		}	
	}

	public IEnumerator WaitPanelPass(){	
		yield return null;
		AsyncOperation async= Application.LoadLevelAsync("CharacterCreate");
		yield return async;
	}
	
	public void Wait1fStart(){	
	    StartCoroutine(Wait1f());	
	}
	
	private IEnumerator Wait1f(){
		yield return null;
        UpdateModelEquip(GetCurrentSelectIdx());
	}

    public void UpdateModelEquip(int index)
    {
        if (-1 == index)
        {
            return;
        }
        _UI_CS_DownLoadPlayer.Instance.equipmentMan.DetachAllItems(SelectBtn[index].info.sex);
        _UI_CS_DownLoadPlayer.Instance.usingLatestConfig = true;
        foreach (itemuuid equip in SelectBtn[index].info.equipinfo)
        {
            Transform item = UnityEngine.Object.Instantiate(ItemPrefabs.Instance.GetItemPrefab(equip.itemID, 0, equip.prefabID)) as Transform;
            SItemInfo itemInfo = new SItemInfo();
            itemInfo.gem = equip.gemID;
            itemInfo.element = equip.elementID;
            itemInfo.enchant = equip.enchantID;
            _UI_CS_DownLoadPlayer.Instance.equipmentMan.UpdateItemInfoBySlot((uint)equip.slotPart, item, itemInfo, true, SelectBtn[index].info.sex);
        }
        _UI_CS_DownLoadPlayer.Instance.equipmentMan.UpdateEquipment(SelectBtn[index].info.sex);
        _UI_CS_DownLoadPlayer.Instance.usingLatestConfig = true;
        Model.Hide(true);
    }

    public int FindEmptyCharaIdx()
    {
        for (int i = 0; i < 3; i++)
        {
            //if(null == CharacterList[i] && 0 == CharacterList[i].ID){
            if (null != SelectBtn[i] && SelectBtn[i].isEmpty)
            {
                return i;
            }
        }
        return -1;
    }
	
	public int FindExistCharaIdx(){
		for(int i = 0;i<3;i++){
//			if(null != CharacterList[i] && 0 != CharacterList[i].ID){
			if(null != SelectBtn[i] && !SelectBtn[i].isEmpty){
					return i;
			}
		}
		return -1;
	}
	
	public void SetActiveCharaBtn(int idx){
		int j = 0;
		ClearCharaList();
		for(int i=0;i<3;i++){
			if(null != CharacterList[i]){
				if(0 != CharacterList[i].ID){
					SelectBtn[j].SetStats(true,CharacterList[i]);
					j++;
				}
			}
		}
		if(0 != j){
			if(SelectBtn.Length >= idx && idx != -1){
				SelectBtn[idx].isSelect = true;
				SelectBtn[idx].GetComponent<UIRadioBtn>().Value = true;
			}
			
			if(j < 3){
				//TODO: temp,because now no add buy char slot.
//				HideCreateBtn();
				SelectBtn[j].ShowCreateBtn();
			}else{
				HideCreateBtn();
			}
		}else{
			SelectBtn[0].ShowCreateBtn();
		}
	}
	
	public void HideCreateBtn(){
		CreateCharaBtn.transform.position = new Vector3(999f,999f,999f);
	}
	
	public void DelinfoFromCharaList(int id){
		for(int i = 0;i<3;i++){
			if(null != CharacterList[i]){
				if(CharacterList[i].ID == id){
					CharacterList[i].ID = 0;
				}
			}
		}
	}
	
	public void SavePlayerInfoTOList(){
		if(null != _PlayerData.Instance){	
			int currentIdx = GetCurrentSelectIdx();
			SelectBtn[currentIdx].info.level = _PlayerData.Instance.playerLevel;
			SelectBtn[currentIdx].info.Exp   = (ulong)_PlayerData.Instance.playerCurrentExp;
			SelectBtn[currentIdx].info.equipinfo.Clear();
			for(int i = 0;i<Inventory.Instance.equipmentArray.Length;i++){
				if(!Inventory.Instance.equipmentArray[i].m_IsEmpty){
					itemuuid itemInfo = new itemuuid();
					itemInfo.itemID = Inventory.Instance.equipmentArray[i].m_ItemInfo.ID;
					itemInfo.prefabID = Inventory.Instance.equipmentArray[i].m_ItemInfo.perfrab;
					itemInfo.elementID = Inventory.Instance.equipmentArray[i].m_ItemInfo.element;
					itemInfo.enchantID = Inventory.Instance.equipmentArray[i].m_ItemInfo.enchant;
					itemInfo.gemID = Inventory.Instance.equipmentArray[i].m_ItemInfo.gem;	
					itemInfo.slotPart = i;
					SelectBtn[currentIdx].info.equipinfo.Add(itemInfo);
				}
			}
			UpdateModelEquip(currentIdx);
		}
	}
	
}
