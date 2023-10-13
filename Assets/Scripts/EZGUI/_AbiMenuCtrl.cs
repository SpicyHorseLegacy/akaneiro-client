using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class _AbiMenuCtrl : MonoBehaviour {
	
	public static _AbiMenuCtrl Instance;
	
	public List<_UI_CS_AbilitiesItem> 	ExistProwessList   = new List<_UI_CS_AbilitiesItem>();
	public List<_UI_CS_AbilitiesItem> 	ExistFortitudeList = new List<_UI_CS_AbilitiesItem>();
	public List<_UI_CS_AbilitiesItem> 	ExistCunningList   = new List<_UI_CS_AbilitiesItem>();
	
	public List<int> 	AllProwessList   = new List<int>();
	public List<int> 	AllFortitudeList = new List<int>();
	public List<int> 	AllCunningList   = new List<int>();
	
	public _UI_CS_UseAbilities	[] UseAbiGroupAList;
	public _UI_CS_UseAbilities	[] UseAbiGroupBList;
	public _UI_CS_UseAbilities 	[] UseAbiGroupCList;
	
	public _UI_CS_UseAbilities 	[] CurrentUseAbiGroupList;
	
	public List<_AbiMenuObj> AbiObjList   = new List<_AbiMenuObj>();
	
	public UIButton 	TabProwessIcon;
	public UIButton 	TabFortitudeIcon;
	public UIButton 	TabCunningIcon;
	
	public UIRadioBtn	TabProwessBG;
	public UIRadioBtn 	TabFortitudeBG;
	public UIRadioBtn 	TabCunningBG;
	
	public UIRadioBtn	GroupABtn;
	public UIRadioBtn 	GroupBBtn;
	public UIRadioBtn 	GroupCBtn;
	public SpriteText	GroupText; 
	public int			GroupIdx;

    public AbilityDetailInfo.EDisciplineType CurrentDisciplineType = AbilityDetailInfo.EDisciplineType.EDT_Prowess;
	
	public int 			CurrentAbiObjIdx 	= 0;
	public int 			PreAbiBarObjIdx 	= 0;
	
	public Transform    UseAbiPrefab;
	
	public bool 		isOperate 	 = false;
	public bool 		isBarOperate = false;
	
//	public UIButton		BG;
	
	public Transform  	SwapSound;
	public Transform  	GroupSound;
	
	void Awake() {
		Instance = this;
	}
	
	// Use this for initialization
	void Start () {
		InitAbiMenuStat();
		TabProwessBG.AddInputDelegate(TabProwessBGDelegate);
		TabFortitudeBG.AddInputDelegate(TabFortitudeBGDelegate);
		TabCunningBG.AddInputDelegate(TabCunningBGDelegate);
		GroupABtn.AddInputDelegate(GroupABtnDelegate);
		GroupBBtn.AddInputDelegate(GroupBBtnDelegate);
		GroupCBtn.AddInputDelegate(GroupCBtnDelegate);
		CurrentUseAbiGroupList[0].GetComponent<UIButton>().AddInputDelegate(CurrentUseAbiBar0Delegate);
		CurrentUseAbiGroupList[1].GetComponent<UIButton>().AddInputDelegate(CurrentUseAbiBar1Delegate);
		CurrentUseAbiGroupList[2].GetComponent<UIButton>().AddInputDelegate(CurrentUseAbiBar2Delegate);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void InitAbiMenuStat(){
		for(int i = 0;i<AbiObjList.Count;i++){
			AbiObjList[i].black.gameObject.layer 					= LayerMask.NameToLayer("EZGUI_CanTouch");
			AbiObjList[i].lockIcon.gameObject.layer 				= LayerMask.NameToLayer("EZGUI_CanTouch");
			AbiObjList[i].icon1.gameObject.layer 					= LayerMask.NameToLayer("EZGUI_CanTouch");
			AbiObjList[i].equipBg.gameObject.layer 					= LayerMask.NameToLayer("EZGUI_CanTouch");
			AbiObjList[i].HighLight.gameObject.layer 				= LayerMask.NameToLayer("EZGUI_CanTouch");
		}
		TabProwessIcon.gameObject.layer 							= LayerMask.NameToLayer("EZGUI_CanTouch");
		TabProwessIcon.SetColor(_UI_Color.Instance.color1);
		TabFortitudeIcon.gameObject.layer 							= LayerMask.NameToLayer("EZGUI_CanTouch");
		TabFortitudeIcon.SetColor(_UI_Color.Instance.color3);
		TabCunningIcon.gameObject.layer 							= LayerMask.NameToLayer("EZGUI_CanTouch");
		TabCunningIcon.SetColor(_UI_Color.Instance.color3);
		CurrentUseAbiGroupList[0].m_HighLight.gameObject.layer 		= LayerMask.NameToLayer("EZGUI_CanTouch");
		CurrentUseAbiGroupList[1].m_HighLight.gameObject.layer 		= LayerMask.NameToLayer("EZGUI_CanTouch");
		CurrentUseAbiGroupList[2].m_HighLight.gameObject.layer 		= LayerMask.NameToLayer("EZGUI_CanTouch");
		AbilitieTip.Instance.TipsBG.gameObject.layer 				= LayerMask.NameToLayer("EZGUI_CanTouch");
	
		for(int i = 0;i<3;i++){
			Transform item = UnityEngine.Object.Instantiate(UseAbiPrefab)as Transform;
			LogManager.Log_Info("");
			UseAbiGroupAList[i] = item.GetComponent<_UI_CS_UseAbilities>();
		}
		for(int i = 0;i<3;i++){
			Transform item = UnityEngine.Object.Instantiate(UseAbiPrefab)as Transform;
			UseAbiGroupBList[i] = item.GetComponent<_UI_CS_UseAbilities>();
		}
		for(int i = 0;i<3;i++){
			Transform item = UnityEngine.Object.Instantiate(UseAbiPrefab)as Transform;
			UseAbiGroupCList[i] = item.GetComponent<_UI_CS_UseAbilities>();
		}
	}
	
	public void LeaveAbiSetting(){
		HighLightCurrentAbiBar(false);
	}
	
	public int GetGroupEmptySlotIdx(){
		for(int i =0;i<3;i++){
			if(CurrentUseAbiGroupList[i].m_isEmpty){
				return i;
			}
		}
		return 0;
	}
	
	//tod: learn new skill, update already equip abilities;
	public void CheckEquipAbilitie(int id){
		int i = 0;
		for(i = 0;i<3;i++){
			if(!UseAbiGroupAList[i].m_isEmpty){
				if((id-1) == UseAbiGroupAList[i].m_abilitiesInfo.m_AbilitieID){
					AddAbilitiesItemToUseGroup(id,0,i);
					SetCurrentGroup(0);
					CS_Main.Instance.g_commModule.SendMessage(
					   		ProtocolBattle_SendRequest.SetSkillShortcut(id,0,i)
					);
				}
			}
		}
		for(i = 0;i<3;i++){
			if(!UseAbiGroupBList[i].m_isEmpty){
				if((id-1) == UseAbiGroupBList[i].m_abilitiesInfo.m_AbilitieID){
					AddAbilitiesItemToUseGroup(id,1,i);
					SetCurrentGroup(1);
					CS_Main.Instance.g_commModule.SendMessage(
					   		ProtocolBattle_SendRequest.SetSkillShortcut(id,1,i)
					);
				}
			}
		}
		for(i = 0;i<3;i++){
			if(!UseAbiGroupCList[i].m_isEmpty){
				if((id-1) == UseAbiGroupCList[i].m_abilitiesInfo.m_AbilitieID){
					AddAbilitiesItemToUseGroup(id,2,i);
					SetCurrentGroup(2);
					CS_Main.Instance.g_commModule.SendMessage(
					   		ProtocolBattle_SendRequest.SetSkillShortcut(id,2,i)
					);
				}
			}
		}
	}
	
	public void SetCurrentGroup(int idx){
		GroupIdx = idx;
		switch(idx){
		case 0:
			GroupABtn.Value = true;
			GroupBBtn.Value = false;
			GroupCBtn.Value = false;
			for(int i =0;i<3;i++){
				if(!UseAbiGroupAList[i].m_isEmpty){
					CurrentUseAbiGroupList[i].SetUseAbiIcon(UseAbiGroupAList[i].m_abilitiesInfo.m_AbilitieID);
					CurrentUseAbiGroupList[i].m_HighLight.Hide(true);
					CurrentUseAbiGroupList[i].m_isEmpty 			= UseAbiGroupAList[i].m_isEmpty;
					CurrentUseAbiGroupList[i].m_isCoolDownFinish 	= UseAbiGroupAList[i].m_isCoolDownFinish;
					CurrentUseAbiGroupList[i].m_isCoolDownStop 		= UseAbiGroupAList[i].m_isCoolDownStop;
					CurrentUseAbiGroupList[i].m_abilitiesInfo 		= UseAbiGroupAList[i].m_abilitiesInfo;
					CurrentUseAbiGroupList[i].m_groupId 			= UseAbiGroupAList[i].m_groupId;
					CurrentUseAbiGroupList[i].m_IdxId 				= UseAbiGroupAList[i].m_IdxId;
					CurrentUseAbiGroupList[i].m_coolDownTime 		= UseAbiGroupAList[i].m_coolDownTime;
				}else{
					CurrentUseAbiGroupList[i].SetUseAbiIcon(0);
					CurrentUseAbiGroupList[i].m_HighLight.Hide(true);
					CurrentUseAbiGroupList[i].m_isEmpty 			= true;
				}
			}
			GroupText.Text = "A";
			break;
		case 1:
			GroupABtn.Value = false;
			GroupBBtn.Value = true;
			GroupCBtn.Value = false;
			for(int i =0;i<3;i++){
				if(!UseAbiGroupBList[i].m_isEmpty){
					CurrentUseAbiGroupList[i].SetUseAbiIcon(UseAbiGroupBList[i].m_abilitiesInfo.m_AbilitieID);
					CurrentUseAbiGroupList[i].m_HighLight.Hide(true);
					CurrentUseAbiGroupList[i].m_isEmpty 			= UseAbiGroupBList[i].m_isEmpty;
					CurrentUseAbiGroupList[i].m_isCoolDownFinish 	= UseAbiGroupBList[i].m_isCoolDownFinish;
					CurrentUseAbiGroupList[i].m_isCoolDownStop 		= UseAbiGroupBList[i].m_isCoolDownStop;
					CurrentUseAbiGroupList[i].m_abilitiesInfo 		= UseAbiGroupBList[i].m_abilitiesInfo;
					CurrentUseAbiGroupList[i].m_groupId 			= UseAbiGroupBList[i].m_groupId;
					CurrentUseAbiGroupList[i].m_IdxId 				= UseAbiGroupBList[i].m_IdxId;
					CurrentUseAbiGroupList[i].m_coolDownTime 		= UseAbiGroupBList[i].m_coolDownTime;
				}else{
					CurrentUseAbiGroupList[i].SetUseAbiIcon(0);
					CurrentUseAbiGroupList[i].m_HighLight.Hide(true);
					CurrentUseAbiGroupList[i].m_isEmpty = true;
				}
			}
			GroupText.Text = "B";
			break;
		case 2:
			GroupABtn.Value = false;
			GroupBBtn.Value = false;
			GroupCBtn.Value = true;
			for(int i =0;i<3;i++){
				if(!UseAbiGroupCList[i].m_isEmpty){
					CurrentUseAbiGroupList[i].SetUseAbiIcon(UseAbiGroupCList[i].m_abilitiesInfo.m_AbilitieID);
					CurrentUseAbiGroupList[i].m_HighLight.Hide(true);
					CurrentUseAbiGroupList[i].m_isEmpty 			= UseAbiGroupCList[i].m_isEmpty;
					CurrentUseAbiGroupList[i].m_isCoolDownFinish 	= UseAbiGroupCList[i].m_isCoolDownFinish;
					CurrentUseAbiGroupList[i].m_isCoolDownStop 		= UseAbiGroupCList[i].m_isCoolDownStop;
					CurrentUseAbiGroupList[i].m_abilitiesInfo 		= UseAbiGroupCList[i].m_abilitiesInfo;
					CurrentUseAbiGroupList[i].m_groupId 			= UseAbiGroupCList[i].m_groupId;
					CurrentUseAbiGroupList[i].m_IdxId 				= UseAbiGroupCList[i].m_IdxId;
					CurrentUseAbiGroupList[i].m_coolDownTime 		= UseAbiGroupCList[i].m_coolDownTime;
				}else{
					CurrentUseAbiGroupList[i].SetUseAbiIcon(0);
					CurrentUseAbiGroupList[i].m_HighLight.Hide(true);
					CurrentUseAbiGroupList[i].m_isEmpty = true;
				}
			}
			GroupText.Text = "C";
			break;
		default:
			break;
		}
	}
	
	public void AddAbilitiesItemToUseGroup(int abilitiesID,int groupIdx, int idx){
		_UI_CS_AbilitiesItem 	tempAItem 	 = new _UI_CS_AbilitiesItem();
		_UI_CS_UseAbilities 	tempUseAItem = new _UI_CS_UseAbilities();
		int index;
//		int aID = (abilitiesID/100) * 100 + 1;
		int aID = abilitiesID;
		int aLevel = abilitiesID % 100;
		for(index = 0; index < AbilityInfo.Instance.PlayerAbilityPool.Count;index++){
			AbilityBaseState ability = AbilityInfo.Instance.PlayerAbilityPool[index];
			if(ability.id == aID){
				tempAItem.m_AbilitieID = abilitiesID;
				tempAItem.m_type       = (int)ability.Info.Discipline;
				tempAItem.m_name       = ability.name;
				tempAItem.m_details1   = ability.Info.Description1;
                tempAItem.m_details2   = ability.Info.Description1;
                tempAItem.m_details3   = ability.Info.Description1;
                tempAItem.m_details4   = ability.Info.Description1;
				tempAItem.m_Cooldown   = ability.Info.CoolDown;
				tempAItem.m_EnergyCost = ability.Info.ManaCost;
				tempAItem.m_level 	   = aLevel;
				switch(groupIdx){
				case 0:
						UseAbiGroupAList[idx].m_abilitiesInfo = tempAItem;
						UseAbiGroupAList[idx].m_isEmpty = false;
						UseAbiGroupAList[idx].m_groupId = groupIdx;
						UseAbiGroupAList[idx].m_IdxId   = idx;
						UseAbiGroupAList[idx].m_coolDownTime = ability.Info.CoolDown;
						break;
				case 1:
						UseAbiGroupBList[idx].m_abilitiesInfo = tempAItem;
						UseAbiGroupBList[idx].m_isEmpty = false;
						UseAbiGroupBList[idx].m_groupId = groupIdx;
						UseAbiGroupBList[idx].m_IdxId   = idx;
						UseAbiGroupBList[idx].m_coolDownTime = ability.Info.CoolDown;
						break;
				case 2:
						UseAbiGroupCList[idx].m_abilitiesInfo = tempAItem;
						UseAbiGroupCList[idx].m_isEmpty = false;
						UseAbiGroupCList[idx].m_groupId = groupIdx;
						UseAbiGroupCList[idx].m_IdxId   = idx;
						UseAbiGroupCList[idx].m_coolDownTime = ability.Info.CoolDown;
						break;
					default:
						break;
				}
				return;
			}else{
				LogManager.Log_Warn("Service send unKnow Skill id"+abilitiesID);
			}
		}
	}
	
	public void AddAbilitiesItem(int abilitiesID){
		_UI_CS_AbilitiesItem tempAItem = new _UI_CS_AbilitiesItem();
		int index;
//		int aID = (abilitiesID/100) * 100 + 1;
		int aID = abilitiesID;
		int aLevel = abilitiesID % 100;
		for(index = 0; index < AbilityInfo.Instance.PlayerAbilityPool.Count;index++){
			AbilityBaseState ability = AbilityInfo.Instance.PlayerAbilityPool[index];
			if(ability.id == aID){
				tempAItem.m_AbilitieID = abilitiesID;
				tempAItem.m_type       = (int)ability.Info.Discipline;
				tempAItem.m_name       = ability.name;
				tempAItem.m_details1   = ability.Info.Description1;
                tempAItem.m_details2   = ability.Info.Description1;
                tempAItem.m_details3   = ability.Info.Description1;
                tempAItem.m_details4   = ability.Info.Description1;
				tempAItem.m_Cooldown   = ability.Info.CoolDown;
				tempAItem.m_EnergyCost = ability.Info.ManaCost;
				tempAItem.m_level 	   = aLevel;
				switch(tempAItem.m_type){
				case 1:
						ExistProwessList.Add(tempAItem);
						break;
				case 2:
						ExistFortitudeList.Add(tempAItem);
						break;
				case 4:
						ExistCunningList.Add(tempAItem);
						break;
					default:
						break;
				}
				return;
			}else{
				LogManager.Log_Warn("Service send unKnow Skill id"+abilitiesID);
			}
		}
	}
	
	
	
	public int GetCurrentAbiGroupIdx(){
		//ezgui change state : it will change at next frame;
//		if(GroupCBtn.Value){
//			return 2;
//		}else if(GroupBBtn.Value){
//			return 1;
//		}else{
//			return 0;
//		}
		return GroupIdx;
	}
	
	public bool IsEquipAbilitie(int abiID){
		int groupIdx = GetCurrentAbiGroupIdx();
		switch(groupIdx){
		case 0:
			for(int i =0;i<3;i++){
				if(!UseAbiGroupAList[i].m_isEmpty){
					if(abiID == UseAbiGroupAList[i].m_abilitiesInfo.m_AbilitieID){
						return true;
					}
				}
			}
			break;
		case 1:
			for(int i =0;i<3;i++){
				if(!UseAbiGroupBList[i].m_isEmpty){
					if(abiID == UseAbiGroupBList[i].m_abilitiesInfo.m_AbilitieID){
						return true;
					}
				}
			}
			break;
		case 2:
			for(int i =0;i<3;i++){
				if(!UseAbiGroupCList[i].m_isEmpty){
					if(abiID == UseAbiGroupCList[i].m_abilitiesInfo.m_AbilitieID){
						return true;
					}
				}
			}
			break;
		default:
			break;
		}
		return false;
	}

    public void InitAbiObjInfo(AbilityDetailInfo.EDisciplineType type)
    {
		switch((int)type){
            case (int)AbilityDetailInfo.EDisciplineType.EDT_Prowess:
				for(int i =0;i<AbiObjList.Count;i++){
					if(i <= (ExistProwessList.Count-1)){
//						int tempID =  IsExistAbi(ExistProwessList[i].m_AbilitieID,AbilityBaseState.EDisciplineType.EDT_Prowess);
//						if(-1 != tempID){
							AbiObjList[i].isAbiEmpty = false;
							bool isEquip = IsEquipAbilitie(ExistProwessList[i].m_AbilitieID);
							AbiObjList[i].InitObjInfo(ExistProwessList[i],false,(ExistProwessList[i].m_AbilitieID%100),isEquip);
//						}else{
//							AbiObjList[i].isAbiEmpty = true;
//							AbiObjList[i].InitObjInfo(ExistProwessList[i],true,1,false);
//						}
					}else{
						AbiObjList[i].HideObj();
					}	
				}
			break;
            case (int)AbilityDetailInfo.EDisciplineType.EDT_Fortitude:
				for(int i =0;i<AbiObjList.Count;i++){
					if(i <= (ExistFortitudeList.Count-1)){
//						int tempID =  IsExistAbi(ExistFortitudeList[i].m_AbilitieID,AbilityBaseState.EDisciplineType.EDT_Fortitude);
//						if(-1 != tempID){
							bool isEquip = IsEquipAbilitie(ExistFortitudeList[i].m_AbilitieID);
							AbiObjList[i].InitObjInfo(ExistFortitudeList[i],false,(ExistFortitudeList[i].m_AbilitieID%100),isEquip);
//						}else{
//							AbiObjList[i].InitObjInfo(ExistFortitudeList[i],true,1,false);
//						}
					}else{
						AbiObjList[i].HideObj();
					}	
				}
			break;
            case (int)AbilityDetailInfo.EDisciplineType.EDT_Cunning:
				for(int i =0;i<AbiObjList.Count;i++){
					if(i <= (ExistCunningList.Count-1)){
//						int tempID =  IsExistAbi(ExistCunningList[i].m_AbilitieID,AbilityBaseState.EDisciplineType.EDT_Cunning);
//						if(-1 != tempID){
							bool isEquip = IsEquipAbilitie(ExistCunningList[i].m_AbilitieID);
							AbiObjList[i].InitObjInfo(ExistCunningList[i],false,(ExistCunningList[i].m_AbilitieID%100),isEquip);
//						}else{
//							AbiObjList[i].InitObjInfo(ExistCunningList[i],true,1,false);
//						}
					}else{
						AbiObjList[i].HideObj();
					}	
				}
			break;
		default:
			break;
		}
	}

    public int IsExistAbi(int id, AbilityDetailInfo.EDisciplineType type)
    {
		switch((int)type){
            case (int)AbilityDetailInfo.EDisciplineType.EDT_Prowess:
			for(int i =0;i<AllProwessList.Count;i++){
//				int tempID = (int)((id*0.01f)*100)+1;
//				if(AllProwessList[i] == tempID){
				if(AllProwessList[i] == id){
					return id;
				}
			}
			break;
            case (int)AbilityDetailInfo.EDisciplineType.EDT_Fortitude:
			for(int i =0;i<AllFortitudeList.Count;i++){
//				int tempID = (int)((id*0.01f)*100)+1;
//				if(AllFortitudeList[i] == tempID){
				if(AllFortitudeList[i] == id){
					return id;
				}
			}
			break;
            case (int)AbilityDetailInfo.EDisciplineType.EDT_Cunning:
			for(int i =0;i<AllCunningList.Count;i++){
//				int tempID = (int)((id*0.01f)*100)+1;
//				if(AllCunningList[i] == tempID){
				if(AllCunningList[i] == id){
					return id;
				}
			}
			break;
		default:
			break;
		}
		return -1;
	}
	
	void TabProwessBGDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:
//				BG.SetColor(_UI_Color.Instance.color11);
                CurrentDisciplineType = AbilityDetailInfo.EDisciplineType.EDT_Prowess;
                InitAbiObjInfo(AbilityDetailInfo.EDisciplineType.EDT_Prowess);
                SetTabIconColor(AbilityDetailInfo.EDisciplineType.EDT_Prowess);
				HighLightCurrentAbiBar(false);
				break;
		   default:
				break;
		}	
	}
	
	void TabFortitudeBGDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:
//				BG.SetColor(_UI_Color.Instance.color12);
                CurrentDisciplineType = AbilityDetailInfo.EDisciplineType.EDT_Fortitude;
                InitAbiObjInfo(AbilityDetailInfo.EDisciplineType.EDT_Fortitude);
                SetTabIconColor(AbilityDetailInfo.EDisciplineType.EDT_Fortitude);
				HighLightCurrentAbiBar(false);
				break;
		   default:
				break;
		}	
	}
	
	void TabCunningBGDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:
//				BG.SetColor(_UI_Color.Instance.color13);
                CurrentDisciplineType = AbilityDetailInfo.EDisciplineType.EDT_Cunning;
                InitAbiObjInfo(AbilityDetailInfo.EDisciplineType.EDT_Cunning);
                SetTabIconColor(AbilityDetailInfo.EDisciplineType.EDT_Cunning);
				HighLightCurrentAbiBar(false);
				break;
		   default:
				break;
		}	
	}
	
	void GroupABtnDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:
				SetCurrentGroup(0);	
				InitAbiObjInfo(CurrentDisciplineType);
				HighLightCurrentAbiBar(false);
				break;
		   default:
				break;
		}	
	}
	
	void GroupBBtnDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:
				SetCurrentGroup(1);		
				InitAbiObjInfo(CurrentDisciplineType);
				HighLightCurrentAbiBar(false);
				break;
		   default:
				break;
		}	
	}
	
	void GroupCBtnDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:
				SetCurrentGroup(2);		
				InitAbiObjInfo(CurrentDisciplineType);
				HighLightCurrentAbiBar(false);
				break;
		   default:
				break;
		}	
	}

    void SetTabIconColor(AbilityDetailInfo.EDisciplineType type)
    {
		TabProwessIcon.SetColor(_UI_Color.Instance.color3);
		TabFortitudeIcon.SetColor(_UI_Color.Instance.color3);
		TabCunningIcon.SetColor(_UI_Color.Instance.color3);
		switch((int)type){
            case (int)AbilityDetailInfo.EDisciplineType.EDT_Prowess:
			TabProwessIcon.SetColor(_UI_Color.Instance.color1);
			break;
            case (int)AbilityDetailInfo.EDisciplineType.EDT_Fortitude:
			TabFortitudeIcon.SetColor(_UI_Color.Instance.color1);
			break;
            case (int)AbilityDetailInfo.EDisciplineType.EDT_Cunning:
			TabCunningIcon.SetColor(_UI_Color.Instance.color1);
			break;
		default:
			break;
		}
	}
	
	public void HighLightCurrentAbiBar(bool isShow){
		isOperate 	 = false;
		isBarOperate = false;
		for(int i=0;i<3;i++){
			if(isShow){
				CurrentUseAbiGroupList[i].m_HighLight.Hide(false);
			}else{
				CurrentUseAbiGroupList[i].m_HighLight.Hide(true);
				AbiObjList[CurrentAbiObjIdx].ShowHighLight(false);
			}
		}
	}
	
	public void CurrentUseAbiRightClick(int idx){
		if(!CurrentUseAbiGroupList[idx].m_isEmpty){
			if(Input.GetButtonDown("Fire2")){
				isBarOperate = true;
				CurrentUseAbiGroupList[0].m_HighLight.Hide(false);
				PreAbiBarObjIdx = 0;
				switch(GroupIdx){
				case 0:
					UseAbiGroupAList[idx].m_isEmpty = true;
					break;
				case 1:
					UseAbiGroupBList[idx].m_isEmpty = true;
					break;
				case 2:
					UseAbiGroupCList[idx].m_isEmpty = true;
					break;
				default:
					break;
				}
				SetCurrentGroup(GroupIdx);
				InitAbiObjInfo(CurrentDisciplineType);
				HighLightCurrentAbiBar(false);
				LogManager.Log_Debug("--- SetSkillShortcut ---");
				CS_Main.Instance.g_commModule.SendMessage(
		   			ProtocolBattle_SendRequest.SetSkillShortcut(0,GroupIdx,idx)
				);
				SoundCue.PlayPrefabAndDestroy(SwapSound);
			}
		}
	}
	
	void CurrentUseAbiBar0Delegate(ref POINTER_INFO ptr)
	{
		switch(ptr.evt)
		{
		   case POINTER_INFO.INPUT_EVENT.MOVE:	
					if(!CurrentUseAbiGroupList[0].m_isEmpty){
						CurrentUseAbiRightClick(0);
					}
				break;
		   case POINTER_INFO.INPUT_EVENT.NO_CHANGE:
					if(!CurrentUseAbiGroupList[0].m_isEmpty){
						AbilitieTip.Instance.AbiTipShow(CurrentUseAbiGroupList[0].transform.position,CurrentUseAbiGroupList[0].m_abilitiesInfo,AbiPosOffestType.LEFT_TOP);
						CurrentUseAbiRightClick(0);
					}
				break;
		   case POINTER_INFO.INPUT_EVENT.MOVE_OFF:
		   case POINTER_INFO.INPUT_EVENT.RELEASE_OFF:
					AbilitieTip.Instance.HideTip();
				break;
		   case POINTER_INFO.INPUT_EVENT.TAP:
				if(isOperate){
					LogManager.Log_Debug("--- SetSkillShortcut ---");
					CS_Main.Instance.g_commModule.SendMessage(
			   			ProtocolBattle_SendRequest.SetSkillShortcut(AbiObjList[CurrentAbiObjIdx].abilitiesInfo.m_AbilitieID,GroupIdx,0)
					);
					SoundCue.PlayPrefabAndDestroy(SwapSound);
					ChangeAbiBarEleInfo(GroupIdx,0,AbiObjList[CurrentAbiObjIdx].abilitiesInfo,false);
					SetCurrentGroup(GroupIdx);	
					InitAbiObjInfo(CurrentDisciplineType);
					HighLightCurrentAbiBar(false);
				}else{
					if(isBarOperate){
						isBarOperate = false;
						CurrentUseAbiGroupList[PreAbiBarObjIdx].m_HighLight.Hide(true);
						CurrentUseAbiGroupList[0].m_HighLight.Hide(true);
						SwapAbiBarEleInfo(GroupIdx,PreAbiBarObjIdx,0);
						SetCurrentGroup(GroupIdx);
					}else{
						isBarOperate = true;
						CurrentUseAbiGroupList[0].m_HighLight.Hide(false);
						PreAbiBarObjIdx = 0;
					}
				}
				break;
		   default:
				break;
		}	
	}
	
	void CurrentUseAbiBar1Delegate(ref POINTER_INFO ptr)
	{
		switch(ptr.evt)
		{
		   case POINTER_INFO.INPUT_EVENT.MOVE:	
					if(!CurrentUseAbiGroupList[1].m_isEmpty){
						CurrentUseAbiRightClick(1);
					}
				break;
		   case POINTER_INFO.INPUT_EVENT.NO_CHANGE:
					if(!CurrentUseAbiGroupList[1].m_isEmpty){
						AbilitieTip.Instance.AbiTipShow(CurrentUseAbiGroupList[1].transform.position,CurrentUseAbiGroupList[1].m_abilitiesInfo,AbiPosOffestType.LEFT_TOP);
						CurrentUseAbiRightClick(1);
					}
				break;
		   case POINTER_INFO.INPUT_EVENT.MOVE_OFF:
		   case POINTER_INFO.INPUT_EVENT.RELEASE_OFF:
					AbilitieTip.Instance.HideTip();
				break;
		   case POINTER_INFO.INPUT_EVENT.TAP:
				if(isOperate){
					LogManager.Log_Debug("--- SetSkillShortcut ---");
					CS_Main.Instance.g_commModule.SendMessage(
			   			ProtocolBattle_SendRequest.SetSkillShortcut(AbiObjList[CurrentAbiObjIdx].abilitiesInfo.m_AbilitieID,GroupIdx,1)
					);
					SoundCue.PlayPrefabAndDestroy(SwapSound);
					ChangeAbiBarEleInfo(GroupIdx,1,AbiObjList[CurrentAbiObjIdx].abilitiesInfo,false);
					SetCurrentGroup(GroupIdx);	
					InitAbiObjInfo(CurrentDisciplineType);
					HighLightCurrentAbiBar(false);
				}else{
					if(isBarOperate){
						isBarOperate = false;
						CurrentUseAbiGroupList[PreAbiBarObjIdx].m_HighLight.Hide(true);
						CurrentUseAbiGroupList[1].m_HighLight.Hide(true);
						SwapAbiBarEleInfo(GroupIdx,PreAbiBarObjIdx,1);
						SetCurrentGroup(GroupIdx);
					}else{
						isBarOperate = true;
						CurrentUseAbiGroupList[1].m_HighLight.Hide(false);
						PreAbiBarObjIdx = 1;
					}
				}

			
				break;
		   default:
				break;
		}	
	}
	
	void CurrentUseAbiBar2Delegate(ref POINTER_INFO ptr)
	{
		switch(ptr.evt)
		{
		   case POINTER_INFO.INPUT_EVENT.MOVE:	
					if(!CurrentUseAbiGroupList[2].m_isEmpty){
						CurrentUseAbiRightClick(2);
					}
				break;
		   case POINTER_INFO.INPUT_EVENT.NO_CHANGE:
					if(!CurrentUseAbiGroupList[2].m_isEmpty){
						AbilitieTip.Instance.AbiTipShow(CurrentUseAbiGroupList[2].transform.position,CurrentUseAbiGroupList[2].m_abilitiesInfo,AbiPosOffestType.LEFT_TOP);
						CurrentUseAbiRightClick(2);
					}
				break;
		   case POINTER_INFO.INPUT_EVENT.MOVE_OFF:
		   case POINTER_INFO.INPUT_EVENT.RELEASE_OFF:
					AbilitieTip.Instance.HideTip();
				break;
		   case POINTER_INFO.INPUT_EVENT.TAP:
				if(isOperate){
					LogManager.Log_Debug("--- SetSkillShortcut ---");
					CS_Main.Instance.g_commModule.SendMessage(
			   			ProtocolBattle_SendRequest.SetSkillShortcut(AbiObjList[CurrentAbiObjIdx].abilitiesInfo.m_AbilitieID,GroupIdx,2)
					);
					SoundCue.PlayPrefabAndDestroy(SwapSound);
					ChangeAbiBarEleInfo(GroupIdx,2,AbiObjList[CurrentAbiObjIdx].abilitiesInfo,false);
					SetCurrentGroup(GroupIdx);	
					InitAbiObjInfo(CurrentDisciplineType);
					HighLightCurrentAbiBar(false);
				}else{
					if(isBarOperate){
						isBarOperate = false;
						CurrentUseAbiGroupList[PreAbiBarObjIdx].m_HighLight.Hide(true);
						CurrentUseAbiGroupList[2].m_HighLight.Hide(true);
						SwapAbiBarEleInfo(GroupIdx,PreAbiBarObjIdx,2);
						SetCurrentGroup(GroupIdx);
					}else{
						isBarOperate = true;
						CurrentUseAbiGroupList[2].m_HighLight.Hide(false);
						PreAbiBarObjIdx = 2;
					}
				}

			
				break;
		   default:
				break;
		}	
	}
	
	public void ChangeAbiBarEleInfo(int groupIdx,int idx,_UI_CS_AbilitiesItem info,bool isEmpty){
		switch(groupIdx){
		case 0:
			UseAbiGroupAList[idx].m_isEmpty 			 = isEmpty;
			UseAbiGroupAList[idx].m_isCoolDownFinish 	 = true;
			UseAbiGroupAList[idx].m_isCoolDownStop 	 	 = false;
			UseAbiGroupAList[idx].m_abilitiesInfo 	 	 = info;
			UseAbiGroupAList[idx].m_groupId 		 	 = groupIdx;
			UseAbiGroupAList[idx].m_IdxId 			 	 = idx;
			UseAbiGroupAList[idx].m_coolDownTime 	 	 = info.m_Cooldown;
			break;
		case 1:
			UseAbiGroupBList[idx].m_isEmpty 			 = isEmpty;
			UseAbiGroupBList[idx].m_isCoolDownFinish 	 = true;
			UseAbiGroupBList[idx].m_isCoolDownStop 		 = false;
			UseAbiGroupBList[idx].m_abilitiesInfo 		 = info;
			UseAbiGroupBList[idx].m_groupId 		 	 = groupIdx;
			UseAbiGroupBList[idx].m_IdxId 				 = idx;
			UseAbiGroupBList[idx].m_coolDownTime 	 	 = info.m_Cooldown;
			break;
		case 2:
			UseAbiGroupCList[idx].m_isEmpty 			 = isEmpty;
			UseAbiGroupCList[idx].m_isCoolDownFinish 	 = true;
			UseAbiGroupCList[idx].m_isCoolDownStop 		 = false;
			UseAbiGroupCList[idx].m_abilitiesInfo 		 = info;
			UseAbiGroupCList[idx].m_groupId 		 	 = groupIdx;
			UseAbiGroupCList[idx].m_IdxId 				 = idx;
			UseAbiGroupCList[idx].m_coolDownTime 	 	 = info.m_Cooldown;
			break;
		default:
			break;
		}
	}
	
	void SwapAbiBarEleInfo(int groupIdx,int idx1,int idx2){
		bool isEmpty = false;
		bool isCoolDF = false;
		bool isCoolDS = false;
		_UI_CS_AbilitiesItem info;
		int  nGroup = 0;
		int  nIdx = 0;
		float coolDT = 0f;
		switch(groupIdx){
		case 0:
			isEmpty  = UseAbiGroupAList[idx1].m_isEmpty;
			isCoolDF = UseAbiGroupAList[idx1].m_isCoolDownFinish;
			isCoolDS = UseAbiGroupAList[idx1].m_isCoolDownStop;
			info 	 = UseAbiGroupAList[idx1].m_abilitiesInfo;
			nGroup 	 = UseAbiGroupAList[idx1].m_groupId;
//			nIdx	 = UseAbiGroupAList[idx1].m_IdxId;
			coolDT	 = UseAbiGroupAList[idx1].m_coolDownTime;
			
			UseAbiGroupAList[idx1].m_isEmpty 			 = UseAbiGroupAList[idx2].m_isEmpty;
			UseAbiGroupAList[idx1].m_isCoolDownFinish 	 = UseAbiGroupAList[idx2].m_isCoolDownFinish;
			UseAbiGroupAList[idx1].m_isCoolDownStop 	 = UseAbiGroupAList[idx2].m_isCoolDownStop;
			UseAbiGroupAList[idx1].m_abilitiesInfo 		 = UseAbiGroupAList[idx2].m_abilitiesInfo;
			UseAbiGroupAList[idx1].m_groupId 		 	 = UseAbiGroupAList[idx2].m_groupId;
//			UseAbiGroupAList[idx1].m_IdxId 				 = UseAbiGroupAList[idx2].m_IdxId;
			UseAbiGroupAList[idx1].m_coolDownTime 	 	 = UseAbiGroupAList[idx2].m_coolDownTime;
			
			UseAbiGroupAList[idx2].m_isEmpty 			 = isEmpty;
			UseAbiGroupAList[idx2].m_isCoolDownFinish 	 = isCoolDF;
			UseAbiGroupAList[idx2].m_isCoolDownStop 	 = isCoolDS;
			UseAbiGroupAList[idx2].m_abilitiesInfo 		 = info;
			UseAbiGroupAList[idx2].m_groupId 		 	 = nGroup;
//			UseAbiGroupAList[idx2].m_IdxId 				 = nIdx;
			UseAbiGroupAList[idx2].m_coolDownTime 	 	 = coolDT;
		break;
		case 1:
			isEmpty  = UseAbiGroupBList[idx1].m_isEmpty;
			isCoolDF = UseAbiGroupBList[idx1].m_isCoolDownFinish;
			isCoolDS = UseAbiGroupBList[idx1].m_isCoolDownStop;
			info 	 = UseAbiGroupBList[idx1].m_abilitiesInfo;
			nGroup 	 = UseAbiGroupBList[idx1].m_groupId;
//			nIdx	 = UseAbiGroupBList[idx1].m_IdxId;
			coolDT	 = UseAbiGroupBList[idx1].m_coolDownTime;
			
			UseAbiGroupBList[idx1].m_isEmpty 			 = UseAbiGroupBList[idx2].m_isEmpty;
			UseAbiGroupBList[idx1].m_isCoolDownFinish 	 = UseAbiGroupBList[idx2].m_isCoolDownFinish;
			UseAbiGroupBList[idx1].m_isCoolDownStop 	 = UseAbiGroupBList[idx2].m_isCoolDownStop;
			UseAbiGroupBList[idx1].m_abilitiesInfo 		 = UseAbiGroupBList[idx2].m_abilitiesInfo;
			UseAbiGroupBList[idx1].m_groupId 		 	 = UseAbiGroupBList[idx2].m_groupId;
//			UseAbiGroupBList[idx1].m_IdxId 				 = UseAbiGroupBList[idx2].m_IdxId;
			UseAbiGroupBList[idx1].m_coolDownTime 	 	 = UseAbiGroupBList[idx2].m_coolDownTime;
			
			UseAbiGroupBList[idx2].m_isEmpty 			 = isEmpty;
			UseAbiGroupBList[idx2].m_isCoolDownFinish 	 = isCoolDF;
			UseAbiGroupBList[idx2].m_isCoolDownStop 	 = isCoolDS;
			UseAbiGroupBList[idx2].m_abilitiesInfo 		 = info;
			UseAbiGroupBList[idx2].m_groupId 		 	 = nGroup;
//			UseAbiGroupBList[idx2].m_IdxId 				 = nIdx;
			UseAbiGroupBList[idx2].m_coolDownTime 	 	 = coolDT;
			break;
		case 2:
			isEmpty  = UseAbiGroupCList[idx1].m_isEmpty;
			isCoolDF = UseAbiGroupCList[idx1].m_isCoolDownFinish;
			isCoolDS = UseAbiGroupCList[idx1].m_isCoolDownStop;
			info 	 = UseAbiGroupCList[idx1].m_abilitiesInfo;
			nGroup 	 = UseAbiGroupCList[idx1].m_groupId;
//			nIdx	 = UseAbiGroupCList[idx1].m_IdxId;
			coolDT	 = UseAbiGroupCList[idx1].m_coolDownTime;
			
			UseAbiGroupCList[idx1].m_isEmpty 			 = UseAbiGroupCList[idx2].m_isEmpty;
			UseAbiGroupCList[idx1].m_isCoolDownFinish 	 = UseAbiGroupCList[idx2].m_isCoolDownFinish;
			UseAbiGroupCList[idx1].m_isCoolDownStop 	 = UseAbiGroupCList[idx2].m_isCoolDownStop;
			UseAbiGroupCList[idx1].m_abilitiesInfo 		 = UseAbiGroupCList[idx2].m_abilitiesInfo;
			UseAbiGroupCList[idx1].m_groupId 		 	 = UseAbiGroupCList[idx2].m_groupId;
//			UseAbiGroupCList[idx1].m_IdxId 				 = UseAbiGroupCList[idx2].m_IdxId;
			UseAbiGroupCList[idx1].m_coolDownTime 	 	 = UseAbiGroupCList[idx2].m_coolDownTime;
			
			UseAbiGroupCList[idx2].m_isEmpty 			 = isEmpty;
			UseAbiGroupCList[idx2].m_isCoolDownFinish 	 = isCoolDF;
			UseAbiGroupCList[idx2].m_isCoolDownStop 	 = isCoolDS;
			UseAbiGroupCList[idx2].m_abilitiesInfo 		 = info;
			UseAbiGroupCList[idx2].m_groupId 		 	 = nGroup;
//			UseAbiGroupCList[idx2].m_IdxId 				 = nIdx;
			UseAbiGroupCList[idx2].m_coolDownTime 	 	 = coolDT;
			break;
		default:
			break;
		}
		LogManager.Log_Debug("--- SetSkillShortcut ---");
		CS_Main.Instance.g_commModule.SendMessage(
   			ProtocolBattle_SendRequest.SetSkillShortcut(UseAbiGroupAList[idx1].m_abilitiesInfo.m_AbilitieID,groupIdx,idx1)
		);
		LogManager.Log_Debug("--- SetSkillShortcut ---");
		CS_Main.Instance.g_commModule.SendMessage(
   			ProtocolBattle_SendRequest.SetSkillShortcut(UseAbiGroupAList[idx2].m_abilitiesInfo.m_AbilitieID,groupIdx,idx2)
		);
		SoundCue.PlayPrefabAndDestroy(SwapSound);
	}
	
	public _UI_CS_UseAbilities GetUseAbilitiesID(int idx){
		if(CurrentUseAbiGroupList[idx].m_isEmpty)
			return null;
		return CurrentUseAbiGroupList[idx];
	}
	
	public void UpDateIngameAbilitiesIcon(){
		for(int i = 0;i<3;i++){
			if(!CurrentUseAbiGroupList[i].m_isEmpty){
				_UI_CS_FightScreen.Instance.m_IngameNormal_AbilitiesBtn[i].SetUVs(new Rect(0,0,1,1));
				_UI_CS_FightScreen.Instance.m_IngameNormal_AbilitiesBtn[i].SetTexture(
				AbilityInfo.Instance.GetAbilityByID((uint)CurrentUseAbiGroupList[i].m_abilitiesInfo.m_AbilitieID/100*100+1).icon);
				_UI_CS_FightScreen.Instance.m_abiMaskEffest[i].InitMesh();
				_UI_CS_FightScreen.Instance.m_abiMaskEffest[i].StartEffect(GetUseAbilitiesID(i).m_coolDownTime);
			}else{
				_UI_CS_FightScreen.Instance.m_IngameNormal_AbilitiesBtn[i].SetUVs(new Rect(0,0,1,1));
				_UI_CS_FightScreen.Instance.m_IngameNormal_AbilitiesBtn[i].SetTexture(_UI_CS_Resource.Instance.m_EquipmentIcon[0]);
			}
		}
	}
	
	public void SwitchGroup(){
		int i = (GroupIdx+1);
		if(i>2){
			i = 0;
		}
		GroupIdx = i;
		SetCurrentGroup(GroupIdx);	
		InitAbiObjInfo(CurrentDisciplineType);
		HighLightCurrentAbiBar(false);
	}
	
	public void ClearAbiList(){
		ExistProwessList.Clear();
		ExistFortitudeList.Clear();
		ExistCunningList.Clear();
		for(int i = 0;i<3;i++){
			UseAbiGroupAList[i].m_isEmpty = true;
			UseAbiGroupBList[i].m_isEmpty = true;
			UseAbiGroupCList[i].m_isEmpty = true;
		}
	}
	
	public void ChangeExistAbiInfo(int sId,int dId){
		_UI_CS_AbilitiesItem tempAItem = new _UI_CS_AbilitiesItem();
		tempAItem.m_AbilitieID = 0;
		int index;
		int aLevel = dId % 100;
		bool isFind = true;
		for(index = 0; index < AbilityInfo.Instance.PlayerAbilityPool.Count;index++){
			AbilityBaseState ability = AbilityInfo.Instance.PlayerAbilityPool[index];
			if(ability.id == dId){
				tempAItem.m_AbilitieID = dId;
				tempAItem.m_type       = (int)ability.Info.Discipline;
				tempAItem.m_name       = ability.name;
                tempAItem.m_details1   = ability.Info.Description1;
                tempAItem.m_details2   = ability.Info.Description1;
                tempAItem.m_details3   = ability.Info.Description1;
                tempAItem.m_details4   = ability.Info.Description1;
				tempAItem.m_Cooldown   = ability.Info.CoolDown;
				tempAItem.m_EnergyCost = ability.Info.ManaCost;
				tempAItem.m_level 	   = aLevel;
			}
		}
		if(0 == tempAItem.m_AbilitieID){
			LogManager.Log_Warn("Service send unKnow Skill id by ChangeExistAbiInfo"+dId);
			return ;
		}
		switch(tempAItem.m_type){
		case 1:
				for(index = 0;index < ExistProwessList.Count;index++){
					if(ExistProwessList[index].m_AbilitieID == sId){
						isFind = false;
						ExistProwessList[index] = tempAItem;
					}
				}
				if(isFind){
					ExistProwessList.Add(tempAItem);
				}
				break;
		case 2:
				for(index = 0;index < ExistFortitudeList.Count;index++){
					if(ExistFortitudeList[index].m_AbilitieID == sId){
						isFind = false;
						ExistFortitudeList[index] = tempAItem;
					}
				}
				if(isFind){
					ExistFortitudeList.Add(tempAItem);
				}
				break;
		case 4:
				for(index = 0;index < ExistCunningList.Count;index++){
					if(ExistCunningList[index].m_AbilitieID == sId){
						isFind = false;
						ExistCunningList[index] = tempAItem;
					}
				}
				if(isFind){
					ExistCunningList.Add(tempAItem);
				}
				break;
			default:
				break;
		}
        InitAbiObjInfo(AbilityDetailInfo.EDisciplineType.EDT_Prowess);
		SetCurrentGroup(0);
		UpDateIngameAbilitiesIcon();
	}
	
	public int GetAbiLevelFromProwessExistAbiIdx(int id){
		for(int i = 0;i < ExistProwessList.Count;i++){
			if(id == (ExistProwessList[i].m_AbilitieID/100*100+1)){
				return i;
			}
		}
		return -1;
	}
	
	public int GetAbiLevelFromFortitudeExistAbiIdx(int id){
		for(int i = 0;i < ExistFortitudeList.Count;i++){
			if(id == (ExistFortitudeList[i].m_AbilitieID/100*100+1)){
				return i;
			}
		}
		return -1;
	}
	
	public int GetAbiLevelFromCunningExistAbiIdx(int id){
		for(int i = 0;i <ExistCunningList.Count;i++){
			if(id == (ExistCunningList[i].m_AbilitieID/100*100+1)){
				return i;
			}
		}
		return -1;
	}
}
