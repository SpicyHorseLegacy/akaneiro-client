using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class AbilitieShopObj {
	public AbilityBaseState currentInfo;
	public AbilityBaseState nextInfo;
}

public class AbilitiesShop : MonoBehaviour {
	
	public static AbilitiesShop Instance = null;
	
	void Awake() {
		Instance = this;
	}
	 
	// Use this for initialization
	void Start () {
		prowerBtn.AddInputDelegate(ProwerTabDelegate);
		fortitudeBtn.AddInputDelegate(FortitudeTabDelegate);
		cunningBtn.AddInputDelegate(CunningTabDelegate);
		exitBtn.AddInputDelegate(ExitBtnDelegate);
		
		seppedUpBtn.AddInputDelegate(SpeedUpBtnDelegate);
		learnBtn.AddInputDelegate(LearnBtnDelegate);
		
		finishBtn.AddInputDelegate(FinishBtnDelegate);
		oneHourBtn.AddInputDelegate(OneHourBtnDelegate);
		halfHourBtn.AddInputDelegate(HalfHourBtnDelegate);
		
		speedUpPanelexitBtn.AddInputDelegate(SpeedUpPanelexitBtn);
	}
	
	// Update is called once per frame
	void Update () {
		UpdateCoolDownTime();
	}
	
	#region Interface
	[SerializeField]
	private UIPanel basePanel;
	public void AwakeAbilitiesShop() {
		_UI_CS_ScreenCtrl.Instance.SetNextScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_ABILITIES_TRAINER);
		MoneyBadgeInfo.Instance.Hide(false);
		basePanel.BringIn();
		InitImage();
		curSkillInfo = null;
		SetTab(AbilityDetailInfo.EDisciplineType.EDT_Prowess);
		leftPanel.Dismiss();
		speedUpPanel.Dismiss();
	}
	
	private mapTypeCooldown abiCoolDownList;
	public void SetAbiCoolDownList(mapTypeCooldown list) {
		abiCoolDownList = list;
	}
	
	public void SetAbiCoolDownList(int cooldownType, int id, long targetTime) {
		//delete//
		if(targetTime == 0) {
			if(abiCoolDownList.ContainsKey(cooldownType)) {
				if(abiCoolDownList[cooldownType].ContainsKey(id)) {
					abiCoolDownList[cooldownType].Remove(id);
				}
			}
		}else {
			//update or add//
			SCooldownInfo sInfo = new SCooldownInfo();
			if(sInfo.cooldownType == null) {
				sInfo.cooldownType = new ECooldownType();
			}
			sInfo.cooldownType.Set(cooldownType);
			sInfo.id = id;
			sInfo.targetTime = targetTime;
	
			if(abiCoolDownList.ContainsKey(cooldownType)) {
				if(abiCoolDownList[cooldownType].ContainsKey(id)) {
					abiCoolDownList[cooldownType][id].targetTime = targetTime;
				}else {
					abiCoolDownList[cooldownType].Add(id,sInfo);
				}
			}else {
				mapCooldownInfo mapInfo = new mapCooldownInfo();
				mapInfo.Add(id,sInfo);
				abiCoolDownList.Add(cooldownType,mapInfo);
			}
		}
		//update//
		SetTab(currentType);
	}
	
	public bool CheckCoolDownSkillList() {
		LearnSkillMsg lsm = new LearnSkillMsg();
		foreach(mapCooldownInfo info in abiCoolDownList.Values) {
			foreach(SCooldownInfo sInfo in info.Values) {
				if(sInfo.targetTime != 0) {
					
					speedUpPanel.BringIn();
					return true;
				}
			}
		}
		return false;
	}
	
	public long GetCoolDownTime(ECooldownType type,int abiID) {
		long time = 0;
		if(abiCoolDownList != null) {
				if(abiCoolDownList.ContainsKey(type.Get())) {
					//because server return me next level skill.So need +1//
					if(abiCoolDownList[type.Get()].ContainsKey(abiID+1)) {
						time = MailLeftPanel.Instance.Get1970Time();
						time = abiCoolDownList[type.Get()][abiID+1].targetTime - time;
//						LogManager.Log_Error("time#: "+ time);
						return time;
					}
				}
		}
//		LogManager.Log_Error("time##: "+ time);
		return time;
	}
	
	[SerializeField]
	private List<int> banAbiList  = new List<int>();
	private bool IsBanAbilitie(int id) {
		for(int i = 0;i<banAbiList.Count;i++){
			if(id == ((banAbiList[i]/100) * 100 + 1)){
				return true;
			}
		}
		return false;
	}
	
	[SerializeField]
	private UIButton seppedUpBtn;
	[SerializeField]
	private SpriteText LearnVal;
	[SerializeField]
	private UIButton learnBtn;
	[SerializeField]
	private SpriteText learnTime;
	private long speedUpTime = 0;
	private float receiveSpeedUpTime = 0;
	private bool isActiveUpDateTime = false;
	public void UpdateInfoBtn(bool isCoolDown,int val,long time) {
//		LogManager.Log_Error("time: "+ time);
		if(isCoolDown) {
			seppedUpBtn.transform.localPosition = new Vector3(0,0,0.1f);
			learnBtn.transform.localPosition = new Vector3(0,0,99f);
			isActiveUpDateTime = true;
			speedUpTime = time;
			receiveSpeedUpTime = Time.time;
		}else {
			isActiveUpDateTime = false;
			if(val == 0) {
				learnBtn.transform.localPosition = new Vector3(0,0,99f);
				LearnVal.Text = "Max";
				learnTime.Text = "Max";
				
			}else {
				learnBtn.transform.localPosition = new Vector3(0,0,0.1f);
				LearnVal.Text = val.ToString();
				learnTime.Text = GetTimeString(time);
			}
			seppedUpBtn.transform.localPosition = new Vector3(0,0,99f);
		}
	}
	
	public string GetTimeString(long time) {
		long lDay;long lHour; long lmin; long lsec;
		string sDay;string sHour; string smin; string sSec;
		lDay = time/86400;
		lHour = time/3600%24;
		lmin = time/60%60;
		lsec = time%60;
		if(lDay == 0) {sDay = "";}else{sDay = lDay.ToString() + " D ";}
		if(lHour == 0) {sHour = "";}else{sHour = lHour.ToString() + " H ";}
		if(lmin == 0) {smin = "";}else{smin = lmin.ToString() + " M ";}
		if(lsec == 0) {sSec = "";}else{sSec = lsec.ToString() + " S ";}
		string stime = sDay + sHour + smin + sSec;
		return stime;
	}
	
	#region Info Panel
	[SerializeField]
	private UIButton iIconC;
	[SerializeField]
	private SpriteText iNameC;
	[SerializeField]
	private SpriteText levelC;
	[SerializeField]
	private SpriteText IDescriptionC;
	[SerializeField]
	private SpriteText IEffectTitle1C;
	[SerializeField]
	private SpriteText IEffectDescription1C;
	[SerializeField]
	private SpriteText IEnC;
	[SerializeField]
	private SpriteText IEffectTitle2C;
	[SerializeField]
	private SpriteText IEffectDescription2C;
	[SerializeField]
	private SpriteText IDescriptionN;
	[SerializeField]
	private SpriteText IEffectTitle1N;
	[SerializeField]
	private SpriteText IEffectDescription1N;
	[SerializeField]
	private SpriteText IEffectTitle2N;
	[SerializeField]
	private SpriteText IEffectDescription2N;
	[SerializeField]
	private SpriteText IEnN;
	[SerializeField]
	private SpriteText IExtraN;
	[SerializeField]
	private SpriteText INeed;
	[SerializeField]
	private SpriteText IHave;
	public UIPanel leftPanel;
	public void UpdateAbilitieInfo(int type,int need,Texture2D icon,string name,int level,string curDesc,string curT1,string curD1,string curT2,string curD2,string curEn,string nextDesc,string nextT1,string nextD1,string nextT2,string nextD2,string nextEn,string extra) {
		iIconC.SetUVs(new Rect(0,0,1,1));
		iIconC.SetTexture(icon);
		iNameC.Text = name;
		levelC.Text = level.ToString();
		IDescriptionC.Text = curDesc;
		IEffectTitle1C.Text = curT1;
		IEffectDescription1C.Text = curD1;
		IEffectTitle2C.Text = curT2;
		IEffectDescription2C.Text = curD2;
		IDescriptionN.Text = nextDesc;
		IEffectTitle1N.Text = nextT1;
		IEffectDescription1N.Text = nextD1;
		IEffectTitle2N.Text = nextT2;
		IEffectDescription2N.Text = nextD2;
		if(extra != null) {
			if(string.Compare("",extra) == 0) {
				IExtraN.Text = "";
			}else {
				IExtraN.Text = "Extra: "+ extra;
			}
		}
		if(string.Compare("",curEn) == 0) {
			IEnC.Text = curEn;
		}else {
			IEnC.Text = "Energy Required: "+curEn;
		}
		if(string.Compare("",nextEn) == 0) {
			IEnN.Text = nextEn;
		}else {
			IEnN.Text = "Energy Required: "+nextEn;
		}
		switch(type) {
		case 1:
			IHave.Text = _PlayerData.Instance.BaseAttrs[EAttributeType.ATTR_Power].ToString() + " PROWESS";
			INeed.Text = need.ToString() + " PROWESS";
			break;
		case 2:
			IHave.Text = _PlayerData.Instance.BaseAttrs[EAttributeType.ATTR_Defense].ToString() + " DEFENSE";;
			INeed.Text = need.ToString() + " DEFENSE";
			break;
		case 4:
			IHave.Text = _PlayerData.Instance.BaseAttrs[EAttributeType.ATTR_Skill].ToString() + " SKILL";
			INeed.Text = need.ToString() + " SKILL";
			break;
		}
	}
	#endregion
	private LearnSkillMsg curSkillInfo;
	public void UpdateCurrentSelectSkillInfo(LearnSkillMsg info) {
		curSkillInfo = info;
	}
	
	#endregion
	
	#region Local
	private void UpdateCoolDownTime() {
		if(isActiveUpDateTime) {
			float time = Time.time - receiveSpeedUpTime;
			time = (speedUpTime - time);
			if(time > 0) {
				learnTime.Text =  GetTimeString((long)time);
				speedUpPanelTime.Text = GetTimeString((long)time);
			}else {
				isActiveUpDateTime = false;
				learnTime.Text = "Finish";
				speedUpPanelTime.Text = "Finish";
//				leftPanel.Dismiss();
			}
			CheckShowCoolDownBtnState(time);
		}
	}
	
	[SerializeField]
	private List<int> baseProwerAbiList  = new List<int>();
	[SerializeField]
	private List<int> baseFortitudeAbiList  = new List<int>();
	[SerializeField]
	private List<int> baseCunningAbiList  = new List<int>();
	[SerializeField]
	private UIScrollList abiList;
	[SerializeField]
	private  List<AbilitieShopObj> itemList = new List<AbilitieShopObj>();
	private int skillIndex = 0;
	//only update skill info list,mastery is server tell client.//
	private void UpdateAbilitiesInfo(AbilityDetailInfo.EDisciplineType type) {
		int temp = 0;
		//skill//
		itemList.Clear();
		switch(type) {
		case AbilityDetailInfo.EDisciplineType.EDT_Prowess:
			for(int i =0;i<baseProwerAbiList.Count;i++){
				temp = _AbiMenuCtrl.Instance.GetAbiLevelFromProwessExistAbiIdx(baseProwerAbiList[i]);
				if(temp >= 0) {
					InitShopListObj(_AbiMenuCtrl.Instance.ExistProwessList[temp].m_AbilitieID,true);
				}else {
					InitShopListObj(baseProwerAbiList[i],false);
				}
			}	
			break;
		case AbilityDetailInfo.EDisciplineType.EDT_Fortitude:
			for(int i =0;i<baseFortitudeAbiList.Count;i++){
				temp = _AbiMenuCtrl.Instance.GetAbiLevelFromFortitudeExistAbiIdx(baseFortitudeAbiList[i]);
				if(temp >= 0) {
					InitShopListObj(_AbiMenuCtrl.Instance.ExistFortitudeList[temp].m_AbilitieID,true);
				}else {
					InitShopListObj(baseFortitudeAbiList[i],false);
				}
			}	
			break;
		case AbilityDetailInfo.EDisciplineType.EDT_Cunning:
			for(int i =0;i<baseCunningAbiList.Count;i++){
				temp = _AbiMenuCtrl.Instance.GetAbiLevelFromCunningExistAbiIdx(baseCunningAbiList[i]);
				if(temp >= 0) {
					InitShopListObj(_AbiMenuCtrl.Instance.ExistCunningList[temp].m_AbilitieID,true);
				}else {
					InitShopListObj(baseCunningAbiList[i],false);
				}
			}	
			break;
		}
		//mastery//
		itemMList.Clear();
		foreach (SingleMastery _info in MasteryInfo.Instance.Masteries) {
            if (_info.MasteryType == EnumMasteryClass.Armor || _info.MasteryType == EnumMasteryClass.Weapon) {
                if(_info.Info != null) {
					if(_info.Info.Discipline == currentType) {
						itemMList.Add(_info);
					}
				}else {
					if(_info.BaseInfo.Discipline == currentType) {
						itemMList.Add(_info);
					}
				}
            }
        }
	}
	
	public void UpdateMasteryList() {
		//mastery//
		itemMList.Clear();
		foreach (SingleMastery _info in MasteryInfo.Instance.Masteries) {
            if (_info.MasteryType == EnumMasteryClass.Armor || _info.MasteryType == EnumMasteryClass.Weapon) {
				if(_info.Info != null) {
					if(_info.Info.Discipline == currentType) {
						itemMList.Add(_info);
					}
				}else {
					if(_info.BaseInfo.Discipline == currentType) {
						itemMList.Add(_info);
					}
				}
            }
        }
		UpdateMasteryScroll();
	}
	
	[SerializeField]
	private  UIListItemContainer itemContainer;
	[SerializeField]
	private  CalculateSlider skillCalc;
	private void UpdateAbilitiesScroll() {
		abiList.ClearList(true);
		skillIndex = 0;
		int n,m,count;
		count = itemList.Count;
		n =	count/2;
		m = count%2;
		if(m > 0) {n++;}
		for(int j =0;j<n;j++) {	
			if(m > 0) {
				if(j+1 == n) {
					AddItemToList(m,true);
				}else {
					AddItemToList(2,true);
				}	
			}else {
				AddItemToList(2,true);
			}
		}
	}
	
	private void AddItemToList(int count,bool isSkill) {
		UIListItemContainer item;
		if(isSkill) {
			item = (UIListItemContainer)abiList.CreateItem((GameObject)itemContainer.gameObject);
			//Reset after manipulations
			abiList.clipContents 	= true;
			abiList.clipWhenMoving 	= true;
			skillCalc.Calculate();
		}else {
			item = (UIListItemContainer)masteryList.CreateItem((GameObject)itemContainer.gameObject);
			//Reset after manipulations
			masteryList.clipContents 	= true;
			masteryList.clipWhenMoving 	= true;
			masteryCalc.Calculate();
		}
		for(int i = 0;i<2;i++){
			if(count > i){
				if(isSkill) {
					item.transform.GetComponent<items>().itemArray[i].transform.GetComponent<AbilitieShopItem>().sinfo = itemList[skillIndex].currentInfo;
					item.transform.GetComponent<items>().itemArray[i].transform.GetComponent<AbilitieShopItem>().sinfoN = itemList[skillIndex].nextInfo;
					item.transform.GetComponent<items>().itemArray[i].transform.GetComponent<AbilitieShopItem>().starMaxCount = 6;
					if(itemList[skillIndex].currentInfo != null) {
						item.transform.GetComponent<items>().itemArray[i].transform.GetComponent<AbilitieShopItem>().SetIcon(itemList[skillIndex].currentInfo.icon);
						item.transform.GetComponent<items>().itemArray[i].transform.GetComponent<AbilitieShopItem>().name.Text = itemList[skillIndex].currentInfo.Info.shortName;
						item.transform.GetComponent<items>().itemArray[i].transform.GetComponent<AbilitieShopItem>().SetStars(itemList[skillIndex].currentInfo.Level);
					}else {
						item.transform.GetComponent<items>().itemArray[i].transform.GetComponent<AbilitieShopItem>().SetIcon(itemList[skillIndex].nextInfo.icon);
						item.transform.GetComponent<items>().itemArray[i].transform.GetComponent<AbilitieShopItem>().name.Text = itemList[skillIndex].nextInfo.Info.shortName;
						item.transform.GetComponent<items>().itemArray[i].transform.GetComponent<AbilitieShopItem>().SetStars(itemList[skillIndex].nextInfo.Level);
					}
					item.transform.GetComponent<items>().itemArray[i].transform.GetComponent<AbilitieShopItem>().isMastery = false;
					item.transform.GetComponent<items>().itemArray[i].transform.GetComponent<AbilitieShopItem>().HideCoolDown();
					skillIndex++;
				}else {
					item.transform.GetComponent<items>().itemArray[i].transform.GetComponent<AbilitieShopItem>().mInfo = itemMList[masteryIndex];
					item.transform.GetComponent<items>().itemArray[i].transform.GetComponent<AbilitieShopItem>().SetIcon(itemMList[masteryIndex].Icon);
					item.transform.GetComponent<items>().itemArray[i].transform.GetComponent<AbilitieShopItem>().starMaxCount = 10;
					if(itemMList[masteryIndex].Info != null) {
						item.transform.GetComponent<items>().itemArray[i].transform.GetComponent<AbilitieShopItem>().name.Text = itemMList[masteryIndex].Info.shortName;
						item.transform.GetComponent<items>().itemArray[i].transform.GetComponent<AbilitieShopItem>().SetStars(itemMList[masteryIndex].Info.MasteryLevel);
					}else {
						item.transform.GetComponent<items>().itemArray[i].transform.GetComponent<AbilitieShopItem>().name.Text = itemMList[masteryIndex].BaseInfo.shortName;
						item.transform.GetComponent<items>().itemArray[i].transform.GetComponent<AbilitieShopItem>().SetStars(0);
					}
					item.transform.GetComponent<items>().itemArray[i].transform.GetComponent<AbilitieShopItem>().isMastery = true;
					item.transform.GetComponent<items>().itemArray[i].transform.GetComponent<AbilitieShopItem>().HideCoolDown();
					masteryIndex++;
				}
			}else {
				item.transform.GetComponent<items>().itemArray[i].position = new Vector3(
				item.transform.GetComponent<items>().itemArray[i].position.x,
				item.transform.GetComponent<items>().itemArray[i].position.y,20f);
			}
		}			
	}
	
	[SerializeField]
	private UIScrollList masteryList;
	[SerializeField]
	private  CalculateSlider masteryCalc;
	[SerializeField]
	private List<SingleMastery> itemMList = new List<SingleMastery>();
	private int masteryIndex = 0;
	private void UpdateMasteryScroll() {
		masteryList.ClearList(true);
		masteryIndex = 0;
		int n,m,count;
		count = itemMList.Count;
		n =	count/2;
		m = count%2;
		if(m > 0) {n++;}
		for(int j =0;j<n;j++) {	
			if(m > 0) {
				if(j+1 == n) {
					AddItemToList(m,false);
				}else {
					AddItemToList(2,false);
				}	
			}else {
				AddItemToList(2,false);
			}
		}
	}
	
	public void InitShopListObj(int id,bool isLearn){
		int index;
		for(index = 0; index < AbilityInfo.Instance.PlayerAbilityPool.Count;index++){
			AbilitieShopObj temp = new AbilitieShopObj();
			AbilityBaseState ability = AbilityInfo.Instance.PlayerAbilityPool[index];	
			//before is next level,but now use current level//
//			if(id+1 == ability.id){
			if(id == ability.id){
				if(ability.Info != null) {
					if(!isLearn) {
						//ability.Level = 0;
						temp.currentInfo = null;
						temp.nextInfo = ability;
						itemList.Add(temp);
						return;
					}else {
						//ability.level = ability.Info.ID%100;
						temp.currentInfo = ability;
					}
					for(index = 0; index < AbilityInfo.Instance.PlayerAbilityPool.Count;index++){
						AbilityBaseState abilityNext = AbilityInfo.Instance.PlayerAbilityPool[index];	
						if(id+1 == abilityNext.id){
							if(abilityNext.Info != null) {
								//abilityNext.level = ability.Info.ID%100;
								temp.nextInfo = abilityNext;
							}
						}
					}
				}
				itemList.Add(temp);
				return;
			}
		}	
	}
	
	[SerializeField]
	private UIButton leftBg3;
	private void InitImage(){
		//downloading image
		TextureDownLoadingMan.Instance.DownLoadingTexture("Pattern",leftBg3.transform);
	}
	public AbilityDetailInfo.EDisciplineType currentType;
	public void SetTab(AbilityDetailInfo.EDisciplineType type) {
		currentType = type;
		switch(type) {
		case AbilityDetailInfo.EDisciplineType.EDT_Prowess:
			prowerBtn.SetState(0);fortitudeBtn.SetState(1);cunningBtn.SetState(1);
			break;
		case AbilityDetailInfo.EDisciplineType.EDT_Fortitude:
			prowerBtn.SetState(1);fortitudeBtn.SetState(0);cunningBtn.SetState(1);
			break;
		case AbilityDetailInfo.EDisciplineType.EDT_Cunning:
			prowerBtn.SetState(1);fortitudeBtn.SetState(1);cunningBtn.SetState(0);
			break;
		}
		UpdateAbilitiesInfo(type);
		UpdateAbilitiesScroll();
		UpdateMasteryScroll();
//		leftPanel.Dismiss();
		speedUpPanel.Dismiss();
	}

	#region Delegate
	[SerializeField]
	private UIRadioBtn prowerBtn;
	void ProwerTabDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:	
				SetTab(AbilityDetailInfo.EDisciplineType.EDT_Prowess);
				break;
		}	
	}
	[SerializeField]
	private UIRadioBtn fortitudeBtn;
	void FortitudeTabDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:	
				SetTab(AbilityDetailInfo.EDisciplineType.EDT_Fortitude);
				break;
		}	
	}
	[SerializeField]
	private UIRadioBtn cunningBtn;
	void CunningTabDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:	
				SetTab(AbilityDetailInfo.EDisciplineType.EDT_Cunning);
				break;
		}	
	}
		
	[SerializeField]
	private UIButton exitBtn;
	void ExitBtnDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:	
				BGMInfo.Instance.isPlayUpGradeEffectSound = true;
				_UI_CS_ScreenCtrl.Instance.SetNextScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_INGAME_NORMAL);
				_UI_CS_FightScreen.Instance.m_CS_Ingame_NormalPanel.BringIn();
				basePanel.Dismiss();
                Player.Instance.ReactivePlayer();
                GameCamera.BackToPlayerCamera();
				break;
		}	
	}

	public UIPanel speedUpPanel;
	void SpeedUpBtnDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:	
				speedUpPanel.BringIn();
				break;
		}	
	}
	
	[SerializeField]
	private UIButton speedUpPanelexitBtn;
	void SpeedUpPanelexitBtn(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:	
				speedUpPanel.Dismiss();
				break;
		}	
	}
	[SerializeField]
	private UIButton coverImg;
	private Color blue = new Color(0.243f,0.486f,0.984f);
	private Color gray = new Color(0.478f,0.478f,0.478f);
	void UpdateFinishCoolDownBtn(bool isReset,float time) {
		if(isReset) {
			oneHourBtn.SetColor(blue);
			halfHourBtn.SetColor(blue);
			coverImg.Hide(true);
			return;
		}
		if(time > 5400) {
			oneHourBtn.SetColor(blue);
			halfHourBtn.SetColor(blue);
			coverImg.Hide(true);
		}else {
			oneHourBtn.SetColor(gray);
			halfHourBtn.SetColor(gray);
			coverImg.Hide(false);
		}
	}
	
	[SerializeField]
	private SpriteText finishValText;
	[SerializeField]
	private SpriteText halfHourValText;
	[SerializeField]
	private SpriteText oneHourValText;
	void CheckShowCoolDownBtnState(float time) {
		int imin = (int)time/60;
		int isec = (int)time%60;
		UpdateFinishCoolDownBtn(false,time);
		if(isec == 0) {
			isec = 0;
		}else {
			isec = 1;
		}
		for(int i = 0;i<MissionSelect.Instance.coolDownCostList.Count;i++) {
			if(imin+isec <= MissionSelect.Instance.coolDownCostList[i].time) {
				if(MissionSelect.Instance.coolDownCostList[i].crystal == 0) {
					finishValText.Text = "FREE";
					halfHourValText.Text = "";
					oneHourValText.Text = "";
				}else {
					finishValText.Text = MissionSelect.Instance.coolDownCostList[i].crystal.ToString();
					halfHourValText.Text = MissionSelect.Instance.coolDownCostList[i].sub30Min.ToString();
					oneHourValText.Text = MissionSelect.Instance.coolDownCostList[i].sub60Min.ToString();
				}
				return;
			}
		}
	}
	
	[SerializeField]
	private SpriteText speedUpPanelTime;
	[SerializeField]
	private UIButton finishBtn;
	void FinishBtnDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:	
				ECooldownType cooldownType = new ECooldownType();
				EDecreaseCooldownType decreaseCoolDownTyep = new EDecreaseCooldownType();
				decreaseCoolDownTyep.Set(EDecreaseCooldownType.eDecreaseCooldownType_All);
				if(curSkillInfo.isMastery) {
					cooldownType.Set(ECooldownType.eCooldownType_Mastery);
				}else {
					cooldownType.Set(ECooldownType.eCooldownType_Skill);
				}
				CS_Main.Instance.g_commModule.SendMessage(ProtocolGame_SendRequest.SpeedUpCoolDown(cooldownType,curSkillInfo.abilitieID,decreaseCoolDownTyep));
				break;
		}	
	}
	
	[SerializeField]
	private UIButton oneHourBtn;
	void OneHourBtnDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:	
				ECooldownType cooldownType = new ECooldownType();
				EDecreaseCooldownType decreaseCoolDownTyep = new EDecreaseCooldownType();
				decreaseCoolDownTyep.Set(EDecreaseCooldownType.eDecreaseCooldownType_60);
				if(curSkillInfo.isMastery) {
					cooldownType.Set(ECooldownType.eCooldownType_Mastery);
				}else {
					cooldownType.Set(ECooldownType.eCooldownType_Skill);
				}
				CS_Main.Instance.g_commModule.SendMessage(ProtocolGame_SendRequest.SpeedUpCoolDown(cooldownType,curSkillInfo.abilitieID,decreaseCoolDownTyep));
				break;
		}	
	}
	
	[SerializeField]
	private UIButton halfHourBtn;
	void HalfHourBtnDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:	
				ECooldownType cooldownType = new ECooldownType();
				EDecreaseCooldownType decreaseCoolDownTyep = new EDecreaseCooldownType();
				decreaseCoolDownTyep.Set(EDecreaseCooldownType.eDecreaseCooldownType_30);
				if(curSkillInfo.isMastery) {
					cooldownType.Set(ECooldownType.eCooldownType_Mastery);
				}else {
					cooldownType.Set(ECooldownType.eCooldownType_Skill);
				}
				CS_Main.Instance.g_commModule.SendMessage(ProtocolGame_SendRequest.SpeedUpCoolDown(cooldownType,curSkillInfo.abilitieID,decreaseCoolDownTyep));
				break;
		}	
	}
	
	void LearnBtnDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:	
				EMoneyType MoneyType = new EMoneyType();
				MoneyType.Set(EMoneyType.eMoneyType_SK);
				if(curSkillInfo != null) {
					if(curSkillInfo.isMastery) {
						CS_Main.Instance.g_commModule.SendMessage(
                                ProtocolGame_SendRequest.masteryLvlupReq(curSkillInfo.masteryType)
                        );
					}else {
						CS_Main.Instance.g_commModule.SendMessage(
					   		ProtocolGame_SendRequest.StudyAbility(curSkillInfo.abilitieID,curSkillInfo.abilitieID,MoneyType)
						);
					}
				}
				break;
		}	
	}
	#endregion
	
	#endregion
}

public class LearnSkillMsg {
	public bool isMastery;
	public EMasteryType masteryType;
	public int abilitieID;
}
