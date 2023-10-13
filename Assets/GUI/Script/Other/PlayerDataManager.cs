using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System;

public class PlayerDataManager : MonoBehaviour {
	  
	public static PlayerDataManager Instance = null;
	
	void Awake() {
		Instance = this;
//		//test//
//		SetMissionID(5534);
//		InitMissionDataList();
	}
	
	// Use this for initialization
	void Start () {
		InitData();
		GUIManager.Instance.OnChangeScreenName += ChangeScreenNameDelegate;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	#region Interface
	
	public void InitData() {
		InitPunishmentReward();
		InitLocalMissionDataLis();
		InitCostCoolDown();
		InitBagData();
		InitEquipData();
		InitFoodData();
		InitStashData();
		ReadStashPice();
		IntitRechargeValData();
		ReadRechargTitleData();
	}
	
	[HideInInspector]  public string ChaName;
	[HideInInspector]  public int CurLV;

	[HideInInspector]  
    public long CurrentExperience
    {
        get { return _curexp; }
        set { _curexp = value; }
	}
    private long _curexp;

    [HideInInspector]
    public long CurrentMaximumExperience { get { return GetMaxExp(CurLV + 1); } }
    public long PreviousMaximumExperience { get { return GetMaxExp(CurLV); } }

	[HideInInspector]  public ESex Gender = new ESex();
	[HideInInspector]  public AttributionManager Attris;
	[HideInInspector]  public EquipementManager Equips;
	
	private SAccountInfo myAccountInfo;
	public void SetAccountInfo(SAccountInfo info) {
		myAccountInfo = info;
	}
	public SAccountInfo GetAccountInfo() {
		return myAccountInfo;
	}
	public int GetAccountType() {
		return myAccountInfo.type;
	}
	
	private List<SCharacterInfoLogin> characterInfoLoginList = new List<SCharacterInfoLogin>();
	public void SetCharaLoginListInfo(vectorCharacterInfoLogins info) {
		characterInfoLoginList.Clear();
		foreach(SCharacterInfoLogin charaData in info) {
			characterInfoLoginList.Add(charaData);
		}
	}
	public SCharacterInfoLogin GetCharaLoginIdx(int id) {
		foreach(SCharacterInfoLogin data in characterInfoLoginList) {
			if(data.ID == id) {
				return data;
			}
		}
		return null;
	}
    public void UpdateCharacterInfoLogin()
    {
        foreach (SCharacterInfoLogin data in characterInfoLoginList)
        {
            if (PlayerDataManager.Instance.ChaName == data.nickname)
            {
                data.level = CurLV;

                vectorAttrChange v = new vectorAttrChange();

                SAttributeChange m = new SAttributeChange();
                m.attributeType = new EAttributeType(EAttributeType.ATTR_MaxHP);
                m.value = PlayerDataManager.Instance.GetBaseAttrs(EAttributeType.ATTR_MaxHP);
                v.Add(m);
                m = new SAttributeChange();
                m.attributeType = new EAttributeType(EAttributeType.ATTR_MaxMP);
                m.value = PlayerDataManager.Instance.GetBaseAttrs(EAttributeType.ATTR_MaxMP);
                v.Add(m);
                m = new SAttributeChange();
                m.attributeType = new EAttributeType(EAttributeType.ATTR_Power);
                m.value = PlayerDataManager.Instance.GetBaseAttrs(EAttributeType.ATTR_Power);
                v.Add(m);
                m = new SAttributeChange();
                m.attributeType = new EAttributeType(EAttributeType.ATTR_Defense);
                m.value = PlayerDataManager.Instance.GetBaseAttrs(EAttributeType.ATTR_Defense);
                v.Add(m);
                m = new SAttributeChange();
                m.attributeType = new EAttributeType(EAttributeType.ATTR_Skill);
                m.value = PlayerDataManager.Instance.GetBaseAttrs(EAttributeType.ATTR_Skill);
                v.Add(m);

                data.attrVec = v;
            }
        }
        
        // TODO : equipement 
        /*data.equipinfo = new vectorSItemuuid();

        itemuuid item = new itemuuid();*/
    }
	public int GetCharaListCount() {
		return characterInfoLoginList.Count;
	}
	public List<SCharacterInfoLogin> GetCharaLoginList() {
		return characterInfoLoginList;
	}
	public void RemoveCharaFromList(int id) {
		foreach(SCharacterInfoLogin data in characterInfoLoginList) {
			if(data.ID == id) {
				characterInfoLoginList.Remove(data);
				return;
			}
		}
	}
	public void AddCharacterToList(SCharacterInfoLogin chara) {
		characterInfoLoginList.Add(chara);
	}
	
	//return max level.//
	public long GetMaxExp(int level) {
		string fileName = null;
		fileName = "Level.level";
		string _fileName = LocalizeManage.Instance.GetLangPath(fileName);
		if(null != fileName){
		TextAsset item = (TextAsset)Resources.Load(_fileName, typeof(TextAsset));
			string[] itemRowsList = item.text.Split('\n');
			for (int i = 0; i < itemRowsList.Length; ++i){
				string pp = itemRowsList[i];
				string[] ppList = pp.Split('\t');
				if(ppList[0].Equals((level).ToString())) {
				   	string[] vals = pp.Split(new char[] { '	', '	' });	
						return long.Parse(vals[1]);
				}
			}
		}
		return 0;
	}

	[SerializeField]
	private string [] playerIcons;
	public string GetPlayerIcon(int disType,int sex){
		if(sex == ESex.eSex_Female){
			switch(disType){
			case 1:
				return playerIcons[0];
			case 2:
				return playerIcons[1];
			case 4:
				return playerIcons[2];
			}
		}else{
			switch(disType){
			case 1:
				return playerIcons[3];
			case 2:
				return playerIcons[4];
			case 4:
				return playerIcons[5];
			}
		}
		return playerIcons[0];
	}
	
	public string DisciplineTypeToString(AbilityDetailInfo.EDisciplineType type) {
		switch(type) {
		case AbilityDetailInfo.EDisciplineType.EDT_Prowess:
			return "Prowess";
		case AbilityDetailInfo.EDisciplineType.EDT_Fortitude:
			return "Fortitude";
		case AbilityDetailInfo.EDisciplineType.EDT_Cunning:
			return "Cunning";
		default:
			return "Unknow";
		}
	}
	
	private SCharacterInfoBasic curPlayerData;
	public void SetCurPlayerInfoBase(SCharacterInfoBasic data) {
		curPlayerData = data;
	}
	public SCharacterInfoBasic GetPlayerInfoBase() {
		return curPlayerData;
	}
	
	private int[] BaseAttrs = new int[EAttributeType.ATTR_Max];
	public void SetBaseAttrs(int type,int vale) {
		BaseAttrs[type] = vale;
	}
	public int GetBaseAttrs(int type) {
		return BaseAttrs[type];
	}

    public int GetCurAttrs(int type)
    {
        if (Player.Instance != null && Player.Instance.AttrMan != null)
            return Player.Instance.AttrMan.Attrs[type];
        return 0;
    }

    public float GetCurEleAttrs(int type)
    {
        if (Player.Instance != null && Player.Instance.AttrMan != null)
            return Player.Instance.AttrMan.EleAttrs[type];
        return 0;
    }

	public float GetCurEleChanceAttrs(int type)
	{
		if (Player.Instance != null && Player.Instance.AttrMan != null)
			return Player.Instance.AttrMan.EleChanceAttrs[type];
		return 0;
	}
	
	public int GetArmor()
	{
		int armorPoint = 0;
		for(int i = 0; i < equipList.Count; i++)
		{
			armorPoint += GetArmorFromItem(equipList[i].localData);
		}
		return armorPoint;
	}
	
	int GetArmorFromItem(SItemInfo _iteminfo)
	{
		if(_iteminfo == null) return 0;
		ItemDropStruct tempItem = ItemDeployInfo.Instance.GetItemObject(_iteminfo.ID,
																		_iteminfo.perfrab,
																		_iteminfo.gem,
																		_iteminfo.enchant,
																		_iteminfo.element,
																		(int)_iteminfo.level);

        return GetArmorFromItem(tempItem);
	}
	
	int GetArmorFromItem(ItemDropStruct _iteminfo)
	{
		if(_iteminfo == null) return 0;
		return (int)(_iteminfo.info_MinDef * _iteminfo.info_Modifier);
	}
	
	public float GetDPS_MainWPN()
	{
		if(Player.Instance.EquipementMan.RightHandWeapon != null && Player.Instance.EquipementMan.RightHandWeapon.GetComponent<WeaponBase>().ItemInfo != null)
		{
			SItemInfo _tempinfo = Player.Instance.EquipementMan.RightHandWeapon.GetComponent<WeaponBase>().ItemInfo;
			ItemDropStruct tempItem = ItemDeployInfo.Instance.GetItemObject(_tempinfo.ID,_tempinfo.perfrab,_tempinfo.gem,_tempinfo.enchant,_tempinfo.element,(int)_tempinfo.level);
			float avgDamage_WPN = ((tempItem.info_MinAtc * tempItem.info_Modifier) + (tempItem.info_MaxAtc * tempItem.info_Modifier)) / 2;
			float dpsWPN = (avgDamage_WPN + Player.Instance.AttrMan.Attrs[EAttributeType.ATTR_PhyAtk])/ getWeaponAttackSpeed(0);
			if (Player.Instance.EquipementMan.GetWeaponType() == WeaponBase.EWeaponType.WT_NoneWeapon)
                 dpsWPN /= 2.0f;
			return dpsWPN;
		}
		return 0;
	}
	
	public float GetDPS_SubWPN()
	{
		if (Player.Instance.EquipementMan.GetWeaponType() == WeaponBase.EWeaponType.WT_TwoHandWeaponAxe || Player.Instance.EquipementMan.GetWeaponType() == WeaponBase.EWeaponType.WT_TwoHandWeaponSword || Player.Instance.EquipementMan.GetWeaponType() == WeaponBase.EWeaponType.WT_OneHandWeapon)
		{
			return 0;
		}else{
			if(Player.Instance.EquipementMan.LeftHandWeapon != null && Player.Instance.EquipementMan.LeftHandWeapon.GetComponent<WeaponBase>().ItemInfo != null)
			{
				SItemInfo _tempinfo = Player.Instance.EquipementMan.LeftHandWeapon.GetComponent<WeaponBase>().ItemInfo;
				ItemDropStruct tempItem = ItemDeployInfo.Instance.GetItemObject(_tempinfo.ID,_tempinfo.perfrab,_tempinfo.gem,_tempinfo.enchant,_tempinfo.element,(int)_tempinfo.level);
				float avgDamage_WPN = ((tempItem.info_MinAtc * tempItem.info_Modifier) + (tempItem.info_MaxAtc * tempItem.info_Modifier)) / 2;
				float dpsWPN = (avgDamage_WPN + Player.Instance.AttrMan.Attrs[EAttributeType.ATTR_PhyAtk])/ getWeaponAttackSpeed(1);
				if (Player.Instance.EquipementMan.GetWeaponType() == WeaponBase.EWeaponType.WT_NoneWeapon)
			        dpsWPN /= 2.0f;
			    return dpsWPN;
			}
		}
		return 0;
	}
	
	float getWeaponAttackSpeed(int hand)
    {
		if(Player.Instance == null)
			return 99999999;
			
        float aniLength = Player.Instance.AnimationModel.animation["Aka_0H_Attack_1"].length;

        Transform wp = null;
        if (hand == 0)
            wp = Player.Instance.EquipementMan.RightHandWeapon;
        else
            wp = Player.Instance.EquipementMan.LeftHandWeapon;

        if (wp != null)
        {
            if (wp && wp.GetComponent<WeaponBase>())
            {
                switch (wp.GetComponent<WeaponBase>().WeaponType)
                {
                    case WeaponBase.EWeaponType.WT_NoneWeapon:
                        aniLength = Player.Instance.AnimationModel.animation["Aka_0H_Attack_1"].length;
                        break;

                    case WeaponBase.EWeaponType.WT_OneHandWeapon:
                    case WeaponBase.EWeaponType.WT_DualWeapon:
                        aniLength = Player.Instance.AnimationModel.animation["Aka_1H_Attack_1"].length;
                        break;

                    case WeaponBase.EWeaponType.WT_TwoHandWeaponAxe:
                        aniLength = Player.Instance.AnimationModel.animation["Aka_2H_Attack_1"].length;
                        break;

                    case WeaponBase.EWeaponType.WT_TwoHandWeaponSword:
                        aniLength = Player.Instance.AnimationModel.animation["Aka_2HNodachi_Attack_1"].length;
                        break;
                }
                aniLength /= Player.Instance.AttrMan.Attrs[EAttributeType.ATTR_AttackSpeed] / 100.0f;
                aniLength *= wp.GetComponent<WeaponBase>().AttackSpeedFactor;
            }
        }

        return aniLength;
    }

	public long offest1970Time = 0;
	public long Update1970OffestTime() {
		TimeSpan ts = DateTime.UtcNow - new DateTime(1970,1,1,0,0,0,0);
		return Convert.ToInt64(ts.TotalSeconds);
	}
	
	public int ReadHpIncVal(int level){
		string fileName = null;
		int hpS = 0;int hpD = 0;int hpF = 0;
		fileName = "Level.level";	
		string _fileName = LocalizeManage.Instance.GetLangPath(fileName);
		if(null != fileName){
		TextAsset item = (TextAsset)Resources.Load(_fileName, typeof(TextAsset));
			string[] itemRowsList = item.text.Split('\n');
			for (int i = 0; i < itemRowsList.Length; ++i){	
				string pp = itemRowsList[i];
				string[] ppList = pp.Split('\t');
				if(ppList[0].Equals((level-1).ToString())) {	
				   	string[] vals = pp.Split(new char[] { '	', '	' });	
						hpS =  int.Parse(vals[4]);
				}
				if(ppList[0].Equals((level).ToString())) {
				   	string[] vals = pp.Split(new char[] { '	', '	' });	
						hpD =  int.Parse(vals[4]);
						hpF = hpD - hpS;
						return hpF;
				}
			}
		}
		return 0;
	}
	#endregion
	
	#region daily reward
	[HideInInspector]
	
	public int dailyRewarType = 0;
	[HideInInspector]
	public SDayReward dailyWardData = new SDayReward();
	#endregion
	
	#region summon reward
	[HideInInspector]
	public mapFriendHireReward summonReward = new mapFriendHireReward();
	#endregion
	
	#region Events
	public  List<EventStruct> EventsList = new List<EventStruct>();
	public  List<EventStruct> BanAList = new List<EventStruct>();
	public  List<EventStruct> BanCList = new List<EventStruct>();
	public void AddToBanList(string scrStr){
		string[] vals = scrStr.Split('@');
		EventStruct temp = new EventStruct();
		if(vals[0] != null) {
			temp.type 	 = int.Parse(vals[0]);
		}else {
			temp.type = 0;
		}
		if(vals[1] != null) {
			temp.eventId = int.Parse(vals[1]);
		}else {
			temp.eventId = 0;
		}
		if(vals[2] != null) {
			temp.count 	 = int.Parse(vals[2]);
		}else {
			temp.count 	 = 0;
		}
		if(vals[3] != null) {
			if(int.Parse(vals[3]) == 1) {
				BanAList.Add(temp);
			}else {
				BanCList.Add(temp);
			}
		}
	}
	private void IntoEventsList(EventStruct temp) {
		string sendData = "";
		sendData = temp.type.ToString()+"@"+temp.eventId.ToString()+"@"+temp.count.ToString()+"@"+temp.eventType.ToString();
		for(int i = 0;i<BanAList.Count;i++){
			if(BanAList[i].eventId == temp.eventId){	
				return;
			}
		}
		for(int i = 0;i<BanCList.Count;i++){
			if(BanCList[i].eventId == temp.eventId){
				return;
			}
		}
		if(temp.eventType == 1){
			BanAList.Add(temp);
			EEventDBType eedbta = new EEventDBType(EEventDBType.eEventType_Account);
			CS_Main.Instance.g_commModule.SendMessage(
			   		ProtocolGame_SendRequest.UpdateEvent(eedbta,temp.eventId,sendData)
													 );
		}else if(temp.eventType == 2){
			BanCList.Add(temp);
			EEventDBType eedbtc = new EEventDBType(EEventDBType.eEventType_Character);
			CS_Main.Instance.g_commModule.SendMessage(
				   	ProtocolGame_SendRequest.UpdateEvent(eedbtc,temp.eventId,sendData)
													 );
		}
		EventsList.Add(temp);
	}
	public void CheckEvent(EM_EVENT_TYPE events){
		switch((int)events){
		case (int)EM_EVENT_TYPE.EM_LEVELUP:
			CheckLevelUpEvent();
			break;
		case (int)EM_EVENT_TYPE.EM_WEEK:
			CheckWeekEvent();
			break;
		case (int)EM_EVENT_TYPE.EM_MONEY:
			CheckMoneyEvent();
			break;
		}
	}
	private void CheckLevelUpEvent(){
		string _fileName = LocalizeManage.Instance.GetLangPath("EventSystem.levelUp");
		TextAsset item = (TextAsset)Resources.Load(_fileName, typeof(TextAsset));
		string[] itemRowsList = item.text.Split('\n');
		for (int i = 3; i < itemRowsList.Length - 1; ++i){
			string pp = itemRowsList[i];
		   	string[] vals = pp.Split(new char[] { '	', '	' });	
			if(int.Parse(vals[4]) == CurLV){
				EventStruct temp = new EventStruct();
				temp.eventId = int.Parse(vals[0]);
				temp.title = vals[1];
				temp.description1 = vals[2];
				temp.description2 = vals[3];
				temp.eventType = int.Parse(vals[5]);
				IntoEventsList(temp);
			}
		}
	}
	private void CheckWeekEvent(){
		string _fileName = LocalizeManage.Instance.GetLangPath("EventSystem.Week");
		TextAsset item = (TextAsset)Resources.Load(_fileName, typeof(TextAsset));
		string[] itemRowsList = item.text.Split('\n');
		for (int i = 3; i < itemRowsList.Length - 1; ++i){
			string pp = itemRowsList[i];
		   	string[] vals = pp.Split(new char[] { '	', '	' });	
			if(int.Parse(vals[4]) == (int)System.DateTime.Now.DayOfWeek){
				EventStruct temp = new EventStruct();
				temp.eventId = int.Parse(vals[0]);
				temp.title = vals[1];
				temp.description1 = vals[2];
				temp.description2 = vals[3];
				temp.eventType = int.Parse(vals[5]);
				IntoEventsList(temp);
			}
		}
	}
	private void CheckMoneyEvent(){
		string _fileName = LocalizeManage.Instance.GetLangPath("EventSystem.money");
		TextAsset item = (TextAsset)Resources.Load(_fileName, typeof(TextAsset));
		string[] itemRowsList = item.text.Split('\n');
		for (int i = 3; i < itemRowsList.Length - 1; ++i){
			string pp = itemRowsList[i];
		   	string[] vals = pp.Split(new char[] { '	', '	' });	
			if(int.Parse(vals[4]) <= int.Parse(MoneyBadgeInfo.Instance.skText.text)){
				EventStruct temp = new EventStruct();
				temp.eventId = int.Parse(vals[0]);
				temp.title = vals[1];
				temp.description1 = vals[2];
				temp.description2 = vals[3];
				temp.eventType = int.Parse(vals[5]);
				IntoEventsList(temp);
			}
		}
	}
	#endregion
	
	#region Mission
	#region MissionObjectactive
	public int GetRegionID() {
		return (missionID/100%10-1);
	}
	
//	public int GetMissionID() {
//		return (missionID/10%10-1);
//	}
	
	public int GetLevelID() {
		return (missionID%10-1);
	}
	
	public List<_UI_CS_Branch> branchList = new List<_UI_CS_Branch>();
	
	public bool isExistMissionList = false;
	
	private GameObject curMissionData;
	public void InitMissionDataList() {
		if(null != curMissionData)
			Destroy(curMissionData.gameObject);
		
		int regID = GetRegionID();
		int missID = GetMissionID();
		
		if(missID == 0) {
			isExistMissionList = false;
			return;
		}else {
			isExistMissionList = true;
			GUILogManager.LogInfo("Exist mission list, mission id: "+missID);
		}

		GUILogManager.LogInfo("InitMissionDataList regID|"+regID+" missID|"+missID);
		branchList.Clear();
		
		for(int i = 0;i<_UI_CS_MapInfo.Instance.Itemlist[regID+1].levelList.Length;i++) {
			if(missID == _UI_CS_MapInfo.Instance.Itemlist[regID+1].levelList[i].ID) {
				curMissionData = UnityEngine.Object.Instantiate(_UI_CS_MapInfo.Instance.Itemlist[regID+1].levelList[i].gameObject)as GameObject;
				if(curMissionData != null) {
					for(int j = 0;j< curMissionData.GetComponent<_UI_CS_MapLevelItem>().branchArray.Count;j++) {
						branchList.Add(curMissionData.GetComponent<_UI_CS_MapLevelItem>().branchArray[j]);
					}
					GUILogManager.LogInfo("InitMissionObjList missID|"+missID);
					return;
				}else {
					GUILogManager.LogInfo("InitMissionObjList curMissionData is null.");
				}
			}
		}
		GUILogManager.LogErr("InitMissionDataList faile. can't find mission data.");
	}
	
	
	// check mission process //
	public void CheckMissionProgress(int type,int objID,int recycle) {
		if(0 == branchList.Count) {
			return;
		}
		//Hunt//
		if(type == (int)_UI_CS_RamusTask.MISSION_TYPE.HUNT) {
			for(int i=0;i<branchList.Count;i++) {
				for(int j = 0;j< branchList[i].taskArray[0].SubObject.Count;j++) {
					if((int)_UI_CS_RamusTask.MISSION_TYPE.HUNT == (int)branchList[i].taskArray[0].SubObject[j].typeID && objID == branchList[i].taskArray[0].SubObject[j].objectID) {
						branchList[i].taskArray[0].SubObject[j].CurrentVal++;
						if(MissionObjectiveManager.Instance) {
							MissionObjectiveManager.Instance.ChangeTaskCurVal(branchList[i].taskArray[0].SubObject[j].CurrentVal,i,j);
						}
						//Is Complete //
						if(branchList[i].taskArray[0].SubObject[j].CurrentVal >= branchList[i].taskArray[0].SubObject[j].count) {
							CompleteTrack(i,j);
							break;
						}
					}
				}
			}
		}
		//DESTORY//
		if(type == (int)_UI_CS_RamusTask.MISSION_TYPE.DESTORY) {
			for(int i=0;i<branchList.Count;i++) {
				for(int j = 0;j< branchList[i].taskArray[0].SubObject.Count;j++) {
					if((int)_UI_CS_RamusTask.MISSION_TYPE.DESTORY == (int)branchList[i].taskArray[0].SubObject[j].typeID && objID == branchList[i].taskArray[0].SubObject[j].objectID) {
						branchList[i].taskArray[0].SubObject[j].CurrentVal++;
						if(branchList[i].taskArray[0].SubObject[j].CurrentVal >= branchList[i].taskArray[0].SubObject[j].count) {
							CompleteTrack(i,j);
							break;
						}
					}
				}
			}
		}
		//Travel//
		if(type == (int)_UI_CS_RamusTask.MISSION_TYPE.TRAVEL) {
			for(int i=0;i<branchList.Count;i++) {
				for(int j = 0;j< branchList[i].taskArray[0].SubObject.Count;j++) {
					if((int)_UI_CS_RamusTask.MISSION_TYPE.TRAVEL == (int)branchList[i].taskArray[0].SubObject[j].typeID) {
						branchList[i].taskArray[0].SubObject[j].CurrentValToString = "";
						if(objID >= branchList[i].taskArray[0].SubObject[j].objectID - 5&& objID <= branchList[i].taskArray[0].SubObject[j].objectID + 5
							&&recycle >= branchList[i].taskArray[0].SubObject[j].recycle - 5&&recycle <= branchList[i].taskArray[0].SubObject[j].recycle + 5) {
							// send reach infomation //
							CS_Main.Instance.g_commModule.SendMessage(ProtocolGame_SendRequest.exploretaskcompleteReq(PlayerDataManager.Instance.GetMissionID(),(int)branchList[i].BranchID,(int)branchList[i].taskArray[0].TaskID,(int)branchList[i].taskArray[0].SubObject[j].typeID,branchList[i].taskArray[0].SubObject[j].objectID));
							CompleteTrack(i,j);
						}
					}
				}
			}
		}
		//PROTECT//
		if(type == (int)_UI_CS_RamusTask.MISSION_TYPE.PROTECT) {
			//...//
		}
		//Collect//
		if(type == (int)_UI_CS_RamusTask.MISSION_TYPE.COLLECT) {
			for(int i=0;i<branchList.Count;i++) {
				for(int j = 0;j< branchList[i].taskArray[0].SubObject.Count;j++) {
					if((int)_UI_CS_RamusTask.MISSION_TYPE.COLLECT == (int)branchList[i].taskArray[0].SubObject[j].typeID 
						&& objID == branchList[i].taskArray[0].SubObject[j].objectID
						&& recycle == branchList[i].taskArray[0].SubObject[j].recycle) {
						branchList[i].taskArray[0].SubObject[j].CurrentVal++;
						if(branchList[i].taskArray[0].SubObject[j].CurrentVal >= branchList[i].taskArray[0].SubObject[j].count) {
							CompleteTrack(i,j);
						}
					}			
				}
			}
		}
		//Survive//
		if(type == (int)_UI_CS_RamusTask.MISSION_TYPE.SURVIVE) {
			for(int i=0;i<branchList.Count;i++){
				for(int j = 0;j< branchList[i].taskArray[0].SubObject.Count;j++) {
					if((int)_UI_CS_RamusTask.MISSION_TYPE.SURVIVE == (int)branchList[i].taskArray[0].SubObject[j].typeID) {
						branchList[i].taskArray[0].SubObject[j].CurrentVal++;
						if(0 == branchList[i].taskArray[0].SubObject[j].count) {
							if(branchList[i].taskArray[0].CurrentPassimeVal >= branchList[i].taskArray[0].SubObject[j].recycle) {
								CompleteTrack(i,j);
								return;
							}
						}else {
							if(branchList[i].taskArray[0].SubObject[j].CurrentVal >= branchList[i].taskArray[0].SubObject[j].count) {
								CompleteTrack(i,j);
								return;
							}
						}
					}
				}
			}
		}
	}
	
	//Complete track Logic //
	public void CompleteTrack(int branchIdx , int trackIdx) {
		branchList[branchIdx].taskArray[0].SubObject.Remove(branchList[branchIdx].taskArray[0].SubObject[trackIdx]);
		MissionObjectiveManager.Instance.RemoveTask(branchIdx,trackIdx);
		MissionObjectiveManager.Instance.isUpdateMissionList = true;
		//Is track Complete //
		if(0 == branchList[branchIdx].taskArray[0].SubObject.Count) {
			branchList[branchIdx].taskArray.Remove(branchList[branchIdx].taskArray[0]);
			//is task complete //
			if(0 == branchList[branchIdx].taskArray.Count) {
				branchList.Remove(branchList[branchIdx]);
				//is branch complete //
				if(0 == branchList.Count) {
					GUILogManager.LogInfo("Mission Complete.");
				}
			}	
		}
	}
	
	public string GetCurMissionTaskName(int branch,int task) {
		foreach(_UI_CS_Branch banchObj in branchList) {
			if(banchObj.BranchID == branch) {
				foreach(_UI_CS_Task taskObj in banchObj.taskArray) {
					if(taskObj.TaskID == task) {
						return taskObj.taskName;
					}
				}
			}
		}
		return "Unkonw Task";
	}
	#endregion
	
	private string scenseName = "";
	public void SetScenseName(string name) {
		scenseName = name;
	}
	public string GetScenseName() {
		return scenseName;
	}
	
	private int missionID = 0;
	public void SetMissionID(int id) {
		// 6001 means back to villiage.
		if(id == 6001)
		{
			isExistMissionList = false;
		}
		missionID = id;
	}
	public int GetMissionID() {
		return missionID;
	}
	
	private int threatBoundExp;
	public void SetThreatBoundExp(int exp) {
		threatBoundExp = exp;
	}
	public int GetThreatBoundExp() {
		return threatBoundExp;
	}
	private int threatBoundKarma;
	public void SetThreatBoundKarma(int karma) {
		threatBoundKarma = karma;
	}
	public int GetThreatBoundKarma() {
		return threatBoundKarma;
	}
	private bool isMissionComplete = false;
	public void SetMissionCompleteFlag(bool mc) {
		isMissionComplete = mc;
	}
	public bool GetMissionCompleteFlag() {
		return isMissionComplete;
	}
	public int GetInGameKarma() {
        return MissionKarma;
	}
	public int GetInGameExp() {
		return MissionScore;
	}
	private string MissionName = "";
	public void SetMissionName(string name) {
		MissionName = name;
	}
	public string GetMissionName() {
		return MissionName;
	}
	private int missionThreat = 0;
	private string threatText = "";
	public void SetMissionThreat(int threat) {
		missionThreat = threat;
		SetThreatText(threat);
	}
	public int GetMissionThreat() {
		return missionThreat;
	}
	private void SetThreatText(int threat) {
		switch(threat) {
		case 0:
			threatText = "EASY";
			break;
		case 1:
			threatText = "NORMAL";
			break;
		case 2:
			threatText = "HARD";
			break;
		case 3:
			threatText = "OVERRUN";
			break;
		}
	}
	public string GetThreatText() {
		return threatText;
	}

    public int MissionScore;
    public int MissionKarma;
    public int CurrentMissionScore;
    public void SetMissionScore(int score)
    {
        MissionScore += score;
        if (Hud_XPBar_Manager.Instance != null) Hud_XPBar_Manager.Instance.SetNewXP(MissionScore);
    }
    public void RsetMissionScore()
    {
        CurrentMissionScore = 0;
        MissionScore = 0;
        MissionKarma = 0;
    }
	
	public void AddMissionKarma(int karma){
		MissionKarma+=karma;
	}

	public List<IngameMaterialStruct> materialsList = new List<IngameMaterialStruct>();
	public void ResetIngameMaterial() {
		materialsList.Clear();
	}
	public void AddIngameMaterial(ItemDropStruct mat) {
		IngameMaterialStruct tMat = ChangeIngameMatExist(mat);
		if(tMat == null) {
			IngameMaterialStruct _tMat = new IngameMaterialStruct();
			_tMat.data = mat;
			_tMat.count = 1;
			materialsList.Add(_tMat);
		}else {
			tMat.count++;
		}
		
	}
	private IngameMaterialStruct ChangeIngameMatExist(ItemDropStruct data) {
		foreach(IngameMaterialStruct mat in materialsList) {
			if(mat.data._ItemID == data._ItemID) {
				return mat;
			}
		}
		return null;
	}
	
	private List<SAcceptMissionRelate2> serMissDataList = new List<SAcceptMissionRelate2>(); 
	public void RestSerMissDataList() {
		serMissDataList.Clear();
	}
	public void AddSerMissDataEle(SAcceptMissionRelate2 ele) {
		serMissDataList.Add(ele);
	}
	public SAcceptMissionRelate2 GetSerMissData(int missiID) {
		foreach(SAcceptMissionRelate2 data in serMissDataList) {
			if(data.missionID == missiID) {
				return data;
			}
		}
		return null;
	}
	
	private float receiveServerMsgTime = 0;
	public void UpdateReceiveSerMsgTime() {
		receiveServerMsgTime = Time.time;
	}
	public float GetReceiveSerMsgTime() {
		return receiveServerMsgTime;
	}
	private List<MissDescData> LocalMissDataList = new List<MissDescData>();
	public void RestLocalMissDataList() {
		LocalMissDataList.Clear();
	}
	public void AddLocalMissDataEle(MissDescData ele) {
		LocalMissDataList.Add(ele);
	}
	public MissDescData GetMissDescData(int missID) {
		foreach(MissDescData data in LocalMissDataList) {
			if(data.missID == missID) {
				return  data;
			}
		}
		return null;
	}
	public void InitLocalMissionDataLis() {
		LocalMissDataList.Clear();
		string fileName = "MissionDescription.Description";
		string _fileName = LocalizeManage.Instance.GetLangPath(fileName);
		TextAsset item = (TextAsset)Resources.Load(_fileName, typeof(TextAsset));
		string[] itemRowsList = item.text.Split('\n');
		for (int i = 3; i < itemRowsList.Length - 1; ++i){
			string pp = itemRowsList[i];	
		   	string[] vals = pp.Split(new char[] { '	', '	' });
			MissDescData data = new MissDescData();
			data.missID = int.Parse(vals[0]);
			data.areaName = vals[1];
			data.areaDesc = vals[2];
			data.recommendLv = vals[3];
			data.lockAreaDesc1 = vals[4];
			data.lockAreaDesc2 = vals[5];
			data.lockAreaDesc3 = vals[6];
			data.missName = vals[7];
			data.missInfo = vals[8];
			data.missSucc = vals[9];
			data.missCDDesc = vals[11];
			data.enemyIconName = vals[13];
			data.enemyInfo = vals[12];
			data.matIconName1 = vals[15];
			data.matIconName2 = vals[16];
			data.matIconName3 = vals[17];
			data.matInfo = vals[14];
			data.bossInfo = vals[18];
			data.rcLow = vals[20];
			data.rcMed = vals[21];
			data.rcHi = vals[22];
			data.rcOvr = vals[23];
			data.missIconName = vals[24];
			AddLocalMissDataEle(data);
		}
	}
	#endregion
	
	#region Revive
	private int reviveCount = 0;
	public void SetReviveCount(int count) {
		reviveCount = count;
	}
	public int GetReviveCount() {
		return reviveCount;
	}
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
	#endregion
	
	#region Cool Down
	public  List<CoolDownCost> coolDownCostList = new List<CoolDownCost>();
    // record the cool down information
    public mapTypeCooldown abiCoolDownList;

	void InitCostCoolDown() {
		coolDownCostList.Clear();
		string _fileName = LocalizeManage.Instance.GetLangPath("MissionsThreatCycle.CoolDown");
		TextAsset item = (TextAsset)Resources.Load(_fileName, typeof(TextAsset));
		string[] itemRowsList = item.text.Split('\n');
		for (int i = 3; i < itemRowsList.Length - 1; ++i){
			CoolDownCost temp = new CoolDownCost();
			string pp = itemRowsList[i];
			string[] vals = pp.Split(new char[] { '	', '	' });
			temp.time		= int.Parse(vals[0]);		
			temp.crystal 	= int.Parse(vals[1]);	
			temp.sub30Min 	= int.Parse(vals[2]);
			temp.sub60Min 	= int.Parse(vals[3]);
			coolDownCostList.Add(temp);
		}
	}

    public void SetAbiCoolDownList(mapTypeCooldown list)
    {
        abiCoolDownList = list;
    }

    public void SetAbiCoolDownList(int cooldownType, int id, long targetTime)
    {
        //delete//
        if (targetTime == 0)
        {
            if (abiCoolDownList.ContainsKey(cooldownType))
            {
                if (abiCoolDownList[cooldownType].ContainsKey(id))
                {
                    abiCoolDownList[cooldownType].Remove(id);
                }
            }
        }
        else
        {
            //update or add//
            SCooldownInfo sInfo = new SCooldownInfo();
            if (sInfo.cooldownType == null)
            {
                sInfo.cooldownType = new ECooldownType();
            }
            sInfo.cooldownType.Set(cooldownType);
            sInfo.id = id;
            sInfo.targetTime = targetTime;

            if (abiCoolDownList.ContainsKey(cooldownType))
            {
                if (abiCoolDownList[cooldownType].ContainsKey(id))
                {
                    abiCoolDownList[cooldownType][id].targetTime = targetTime;
                }
                else
                {
                    abiCoolDownList[cooldownType].Add(id, sInfo);
                }
            }
            else
            {
                mapCooldownInfo mapInfo = new mapCooldownInfo();
                mapInfo.Add(id, sInfo);
                abiCoolDownList.Add(cooldownType, mapInfo);
            }
        }
    }

    public long GetCoolDownTime(ECooldownType type, int abiID)
    {
        long time = 0;
        if (abiCoolDownList != null)
        {
            if (abiCoolDownList.ContainsKey(type.Get()))
            {
                //because server return me next level skill.So need +1//
                if (abiCoolDownList[type.Get()].ContainsKey(abiID + 1))
                {
                    time = Get1970Time();
                    time = abiCoolDownList[type.Get()][abiID + 1].targetTime - time;
                    return time;
                }
            }
        }
        return time;
    }
	
	long Get1970Time() {
		TimeSpan ts = DateTime.UtcNow - new DateTime(1970,1,1,0,0,0,0);
		return Convert.ToInt64(ts.TotalSeconds + offest1970Time);
	}
	public long GetCurrect1970Time()
	{
		return Get1970Time();
	}

	#endregion
	
	[HideInInspector]
	public int SynType(int type) {
		if(type == 1) {
			return 2;
		}else if(type == 2) {
			return 1;
		}else if(type == 3) {
			return 3;
		}else if(type == 4) {
			return 4;
		}
		return 0;
	}
	
	#region Equipment
	public  List<_ItemInfo> equipList = new List<_ItemInfo>();
	public void ResetEquipList() {
		equipList.Clear();
	}
	public void AddEquipItem(_ItemInfo info) {
		equipList.Add(info);
	}
	public void InitEquipData() {
		ResetEquipList();
		for(int i =0;i<9;i++) {
			_ItemInfo data = new _ItemInfo();
			data.serData = null;data.localData = null;
			data.slot = i;
			data.empty = true;
			data.isChange = false;
			AddEquipItem(data);
		}
	}
	public _ItemInfo GetEquipItemData(int solt) {
		foreach(_ItemInfo data in equipList) {
			if(data.slot == solt) {
				return data;
			}
		}
		return null;
	}
	public void UpdateEquipSlot(_ItemInfo data) {
		for(int i=0;i<equipList.Count;i++) {
			if(equipList[i].slot == data.slot) {
				equipList[i] = data;
				return;
			}	
		}
	}
	
	public void UpdateEquipSlot(SItemInfo serInfo) { 
		_ItemInfo data = GetEquipItemData((int)serInfo.slot);
		if(data != null) {
			ItemDropStruct localInfo = ItemDeployInfo.Instance.GetItemObject(serInfo.ID,serInfo.perfrab,serInfo.gem,serInfo.enchant,serInfo.element,(int)serInfo.level);
			data.slot = (int)serInfo.slot;
			data.count = (int)serInfo.count;
			data.empty = false;
			data.localData = localInfo;
			data.serData = serInfo;
		}
	}
	public void EmptyEquipSlot(int slot) {
		if(slot < 0 || slot>8) {
			GUILogManager.LogErr("EmptyEquipSlot err.slot:"+slot);
			return;
		}
		_ItemInfo item = GetEquipItemData(slot);
		item.empty = true;
	}
	public void SetEquipSoltChange(int slot,bool change) {
		_ItemInfo data = GetEquipItemData(slot);
		if(data == null) {
			GUILogManager.LogErr("SetEquipSoltChange err.slot:"+slot);
			return;
		}
		data.isChange = change;
	}
	public void SwapEquipToBagSlot(int eqp,int bag) {
		_ItemInfo tBagData = GetBagItemData(bag);
		_ItemInfo tequipData = GetEquipItemData(eqp);
		
		_ItemInfo srcItem = new _ItemInfo();
		srcItem.slot = tBagData.slot;
		srcItem.count = tequipData.count;
		srcItem.empty = tequipData.empty;
		srcItem.isChange = tequipData.isChange;
		srcItem.localData = tequipData.localData;
		srcItem.serData = tequipData.serData;
		
		_ItemInfo dstItem = new _ItemInfo();
		dstItem.slot = tequipData.slot;
		dstItem.count = tBagData.count;
		dstItem.empty = tBagData.empty;
		dstItem.isChange = tBagData.isChange;
		dstItem.localData = tBagData.localData;
		dstItem.serData = tBagData.serData;
		
		UpdateEquipSlot(dstItem);
		UpdateBagSlot(srcItem);
		
		InitPlayerModelEquips();
	}
	
	public void InitPlayerModelEquips() {
		Player.Instance.EquipementMan.DetachAllItems(Gender);
		Transform itemSrc = null;Transform itemDest = null;
		foreach(_ItemInfo data in PlayerDataManager.Instance.equipList) {
			if(data.localData == null) {
				continue;
			}
			itemSrc = ItemPrefabs.Instance.GetItemPrefab(data.localData._ItemID,data.localData._TypeID,data.localData._PrefabID);
			if(itemSrc != null) {
				itemDest = UnityEngine.Object.Instantiate(itemSrc)as Transform;  
			}
			if(itemDest) {
                Player.Instance.EquipementMan.UpdateItemInfoBySlot((uint)data.slot,itemDest,data.serData,true,Gender);
            }
		}
		Player.Instance.EquipementMan.UpdateEquipment(Gender);
		Player.Instance.GetComponent<PreLoadPlayer>().usingLatestConfig = true;
	}
	#endregion
	
	#region Stash
	public void SwapStashToBagSlot(int stash,int bag) {
		_ItemInfo tBagData = GetBagItemData(bag);
		_ItemInfo tStashData = GetStashItemData(stash);
		
		_ItemInfo srcItem = new _ItemInfo();
		srcItem.slot = tBagData.slot;
		srcItem.count = tStashData.count;
		srcItem.empty = tStashData.empty;
		srcItem.isChange = tStashData.isChange;
		srcItem.localData = tStashData.localData;
		srcItem.serData = tStashData.serData;
		
		_ItemInfo dstItem = new _ItemInfo();
		dstItem.slot = tStashData.slot;
		dstItem.count = tBagData.count;
		dstItem.empty = tBagData.empty;
		dstItem.isChange = tBagData.isChange;
		dstItem.localData = tBagData.localData;
		dstItem.serData = tBagData.serData;
		
		UpdateStashSlot(dstItem);
		UpdateBagSlot(srcItem);
	}
	private const int stashTabMaxSlot = 12;
	public int GetStashMaxSlot() {
		return stashTabMaxSlot;
	}
	private int curStashTabIdx = 1;
	private int stashMaxTab = 0;
	public void SetStashMaxTab(int maxSlot) {
		stashMaxTab = (int)(maxSlot / stashTabMaxSlot);
	}
	public void SetStashMaxTabForCount(int count) {
		stashMaxTab = count;
	}
	public int GetMaxStashTab() {
		return stashMaxTab;
	}
	public void SetCurStashTapIdx(int idx) {
		curStashTabIdx = idx;
	}
	public int GetCurStashIdx() {
		return curStashTabIdx;
	}
	public  List<_ItemInfo> stashList = new List<_ItemInfo>();
	public void InitStashData() {
		ResetStashList();
		for(int i =0;i<60;i++) {
			_ItemInfo data = new _ItemInfo();
			data.serData = null;data.localData = null;
			data.slot = i+1;
			data.empty = true;
			data.isChange = false;
			AddStashItem(data);
		}
	}
	public void ResetStashList() {
		stashList.Clear();
	}
	public void AddStashItem(_ItemInfo info) {
		stashList.Add(info);
	}
	public _ItemInfo GetStashItemData(int solt) {
		foreach(_ItemInfo data in stashList) {
			if(data.slot == solt) {
				return data;
			}
		}
		return null;
	}
	public void SetStashSoltChange(int slot,bool change) {
		_ItemInfo data = GetStashItemData(slot);
		if(data == null) {
			GUILogManager.LogErr("SetStashSoltChange err.slot:"+slot);
			return;
		}
		data.isChange = change;
	}
	public void UpdateStashSlot(SItemInfo serInfo) {
		_ItemInfo data = GetStashItemData((int)serInfo.slot+1);
		if(data != null) {
			ItemDropStruct localInfo = ItemDeployInfo.Instance.GetItemObject(serInfo.ID,serInfo.perfrab,serInfo.gem,serInfo.enchant,serInfo.element,(int)serInfo.level);
			data.slot = (int)serInfo.slot+1;
			data.count = (int)serInfo.count;
			data.empty = false;
			data.localData = localInfo;
			data.serData = serInfo;
		}
	}
	public void UpdateStashSlot(_ItemInfo data) {
		for(int i=0;i<stashList.Count;i++) {
			if(stashList[i].slot == data.slot) {
				stashList[i] = data;
				return;
			}	
		}
	}
	public void EmptyStashSlot(int idx) {
		if(idx < 0 || idx>59) {
			GUILogManager.LogErr("EmptyStashSlot err.Idx:"+idx);
			return;
		}
		stashList[idx].empty = true;
	}
	public void SwapStashToStashSlot(int src,int dst) {
		_ItemInfo srcItem = GetStashItemData(src);
		_ItemInfo dstItem = GetStashItemData(dst);
		int tSrcSlot = srcItem.slot;
		int tDstSlot = dstItem.slot;

		_ItemInfo tempData = new _ItemInfo();
		tempData = srcItem;
		srcItem = dstItem;
		dstItem = tempData;
		
		srcItem.slot = tSrcSlot;
		dstItem.slot = tDstSlot;
		
		UpdateStashSlot(srcItem);
		UpdateStashSlot(dstItem);
	}
	private int [] stashPice = new int[5];
	public int GetUnlockStashPice() {
		if(stashMaxTab>4) {
			return 0;
		}
		return stashPice[stashMaxTab];
	}
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
	
	#region Bag
	public  List<_ItemInfo> bagList = new List<_ItemInfo>(); 
	public void InitBagData() {
		ResetBagList();
		for(int i =0;i<40;i++) {
			_ItemInfo data = new _ItemInfo();
			data.serData = null;data.localData = null;
			data.slot = i+1;
			data.empty = true;
			data.isChange = false;
			AddBagItem(data);
		}
	}
	public void SetSoltIsChange(int slot,bool change) {
		_ItemInfo data = GetBagItemData(slot);
		data.isChange = change;
	}
	public void UpdateBagSlot(SItemInfo serInfo) {
		_ItemInfo data = GetBagItemData((int)serInfo.slot+1);
		if(data != null) {
			ItemDropStruct localInfo = ItemDeployInfo.Instance.GetItemObject(serInfo.ID,serInfo.perfrab,serInfo.gem,serInfo.enchant,serInfo.element,(int)serInfo.level);
			data.slot = (int)serInfo.slot+1;
			data.count = (int)serInfo.count;
			data.empty = false;
			data.localData = localInfo;
			data.serData = serInfo;
		}
	}
	public void UpdateBagSlot(_ItemInfo data) {
		for(int i=0;i<bagList.Count;i++) {
			if(bagList[i].slot == data.slot) {
				bagList[i] = data;
				return;
			}	
		}
	}
	public _ItemInfo GetBagItemData(int solt) {
		foreach(_ItemInfo data in bagList) {
			if(data.slot == solt) {
				return data;
			}
		}
		return null;
	}
	public void EmptyBagSlot(int idx) {
        _ItemInfo data = GetBagItemData(idx);
		data.empty = true;
        data.localData = null;
        data.serData = null;
	}
	public void SwapBagToBagSlot(int src,int dst) {
		_ItemInfo srcItem = GetBagItemData(src);
		_ItemInfo dstItem = GetBagItemData(dst);
		int tSrcSlot = srcItem.slot;
		int tDstSlot = dstItem.slot;

		_ItemInfo tempData = new _ItemInfo();
		tempData = srcItem;
		srcItem = dstItem;
		dstItem = tempData;
		
		srcItem.slot = tSrcSlot;
		dstItem.slot = tDstSlot;
		
		UpdateBagSlot(srcItem);
		UpdateBagSlot(dstItem);
	}
	public void ResetBagList() {
		bagList.Clear();
	}
	public void AddBagItem(_ItemInfo info) {
		bagList.Add(info);
	}
	#endregion
	
	#region Food
	public List<int> foodList = new List<int>(); 
	public void ResetFoodList() {
		foodList.Clear();
	}
	public void AddFoodItem(int slot) {
		foodList.Add(slot);
	}
	public _ItemInfo GetFoodItemData(int slot) {
		int tSolt = foodList[slot-1];
		if(tSolt == -1) {
			return null;
		}
		foreach(_ItemInfo data in bagList) {
			if(data.slot == tSolt) {
				return data;
			}
		}
		return null;
	}
	public void InitFoodData() {
		ResetFoodList();
		for(int i =0;i<3;i++) {
			AddFoodItem(-1);
		}
	}
	public void UpdateFoodSlot(int slot,int bagSlot) {
        for (int i = 0; i < 3; i++)
        { 
            if (i == slot-1)
                foodList[i] = bagSlot;

            else if (foodList[i] == bagSlot)
                foodList[i] = -1;
        }
	}
    public void RemoveFoodItem(int bagSlot)
    {
        for (int i = 0; i < 3; i++)
            if (foodList[i] == bagSlot)
            {
                foodList[i] = -1;
                return;
            }
    }
	public void SwapFoodSlot(int sSlot,int dSlot) {
		int tIdx = 0;
		tIdx = foodList[sSlot-1];
		foodList[sSlot-1] = foodList[dSlot-1];
		foodList[dSlot-1] = tIdx;
	}
	public void EmptyAllFoodSlot()
	{
		for(int i= 0; i < 3; i++)
			EmptyFoodSlot(i);
	}
	public void EmptyFoodSlot(int idx) {
		if(idx < 0 || idx > 2) {
			GUILogManager.LogErr("EmptyFoodSlot err.Idx:"+idx);
			return;
		}
		foodList[idx] = -1;
	}
	public void CheckFoodList(int sSlot,int dSlot) {
		for (int i = 0; i < 3; i++) {
			if (foodList[i] == sSlot) {
				foodList[i] = dSlot;
				continue;
			}
			if (foodList[i] == dSlot) {
				foodList[i] = sSlot;
				continue;
			}
		}
	}
	#endregion
	
	#region Item Attr
	public Color [] levelColor;
	public Color [] levelTextColor;
	public float [] levelNmb;
	public string GetItemName(ItemDropStruct info) {
		string tName = "";
		if(info._TypeID == 7 || info._TypeID == 8){
			  tName = info.info_EncName+ info.info_GemName + info.info_EleName+ info.info_TypeName;
		}else if(1 == info._TypeID|| 3 == info._TypeID||4 == info._TypeID||6 == info._TypeID){
			if(info._TypeID == 4){
			  tName = GetCloakName(info);
			}else{
			  tName = info.info_EncName + info.info_GemName  + info._TypeName  + info._TypelastName;
			}	
		}else if(2 == info._TypeID|| 5 == info._TypeID){
			  tName = info.info_EncName + info.info_EleName + info.info_GemName  + info.info_TypeName;	
		}else{
			tName = info._PropsName;
		}
		tName.Trim();
		return tName;
	}
	public int GetItemValLevel(float val) {
		if(val < levelNmb[0]){
			return 0;
		}else if( (levelNmb[0] - 0.01) < val && val  < levelNmb[1]){
			return 1;
		}else if( (levelNmb[1] - 0.01) < val && val < levelNmb[2]){
			return 2;
		}else if( (levelNmb[2] - 0.01) < val && val < levelNmb[3]){
			return 3;
		}else if( (levelNmb[3] - 0.01) < val){
			return 4;
		}	
		return 0;
	}
	public Color GetNameColor(float val) {
		if(val < levelNmb[0]){
			return levelColor[0];
		}else if( (levelNmb[0] - 0.01) < val && val  < levelNmb[1]){
			return levelColor[1];
		}else if( (levelNmb[1] - 0.01) < val && val < levelNmb[2]){
			return levelColor[2];
		}else if( (levelNmb[2] - 0.01) < val && val < levelNmb[3]){
			return levelColor[3];
		}else if( (levelNmb[3] - 0.01) < val){
			return levelColor[4];
		}	
		return levelColor[0];
	}
	public Color GetNameTextColor(float val) {
		if(val < levelNmb[0]){
			return levelTextColor[0];
		}else if( (levelNmb[0] - 0.01) < val && val  < levelNmb[1]){
			return levelTextColor[1];
		}else if( (levelNmb[1] - 0.01) < val && val < levelNmb[2]){
			return levelTextColor[2];
		}else if( (levelNmb[2] - 0.01) < val && val < levelNmb[3]){
			return levelTextColor[3];
		}else if( (levelNmb[3] - 0.01) < val){
			return levelTextColor[4];
		}	
		return levelTextColor[0];
	}
	public int GetItemValue(int valueType,int level,float eleVal,float encVal,float gemVal,float itemVal){
		int itemValue = 0;
		switch(valueType){
			//trabsmute
		case 1:
			itemValue = (int)((level)*(1+eleVal+encVal+gemVal)*(itemVal*0.2f));
			return itemValue;
			//sale
		case 2:
			itemValue = (int)((level)*(1+eleVal+encVal+gemVal)*(itemVal*0.4f));
			return itemValue;
		default:
			return -1;
		}
	}
	public string GetCloakName(ItemDropStruct data){
		string name = "";
		name = data.info_EncName  + data.info_GemName + data._QualityName + data._CloakName;
		return name;
	}
	#endregion

    #region PetInfo
    private int m_currentPetId = -1;

    public int CurrentPetId
    {
        get { return m_currentPetId;  }
        set { m_currentPetId = value; }
    }

    [SerializeField]
    private string[] petIcons;
    public string GetPetIcon(int id)
    {
        int idRoot = int.Parse(id.ToString().Substring(0, id.ToString().Length - 2));

        switch (idRoot)
        {
            case 10:
                return petIcons[0];
            case 20:
                return petIcons[1];
            case 30:
                return petIcons[2];
            case 40:
                return petIcons[3];
            case 50:
                return petIcons[4];
            case 60:
                return petIcons[5];
            case 70:
                return petIcons[6];
            case 11:
                return petIcons[7];
            case 21:
                return petIcons[8];
            case 80:
                return petIcons[9];
            case 90:
                return petIcons[10];
            case 100:
                return petIcons[11];
            case 120:
                return petIcons[12];
            case 140:
                return petIcons[13];
            case 130:
                return petIcons[14];
            case 110:
                return petIcons[15];
        }

        return null;
    }
    #endregion
	
	#region ItemShop
	[HideInInspector]
	public bool isUpdateRareShopItem = false;
	[HideInInspector]
	public bool isDownLoadShopData = false;
	private int InitShopDataFlag = 0;
	public int GetIntiShopFlag() {
		return InitShopDataFlag;
	}
	public void AddInitShopDataFlag() {
		InitShopDataFlag++;
		if(InitShopDataFlag >= 2) {
			isDownLoadShopData = true;
			GUIManager.Instance.ChangeUIScreenState("ItemShop");
		}
	}
	public void ClearInitShopData() {
		InitShopDataFlag = 0;
	}
	
	public  List<ItemShopObjData> itemShopSpecialList = new List<ItemShopObjData>();
	public  List<ItemShopObjData> itemShopWeaponList = new List<ItemShopObjData>();
	public  List<ItemShopObjData> itemShopArmorList = new List<ItemShopObjData>();
	public  List<ItemShopObjData> itemShopAccessoriesList = new List<ItemShopObjData>();
	
	public void ClearItemShopList(bool isSpecial) {
		if(isSpecial) {
			itemShopSpecialList.Clear();
		}else {
			itemShopWeaponList.Clear();
			itemShopArmorList.Clear();
			itemShopAccessoriesList.Clear();
		}
	}
	public void AddItemShopData(SShopItemInfo data,bool isSpecial) {
		ItemShopObjData temp = new ItemShopObjData();
		temp.serData = data;
		ItemDropStruct tLocalData = ItemDeployInfo.Instance.GetItemObject(data.ID,data.perfrab,data.gem,data.enchant,data.element,(int)data.level);
		temp.localData = tLocalData;
		if(isSpecial) {
			itemShopSpecialList.Add(temp);
		}else {
			if(temp.localData != null) {
				switch(temp.localData._TypeID) {
				case 7:
				case 8:
					itemShopWeaponList.Add(temp);
					break;
				case 2:
				case 5:
					itemShopAccessoriesList.Add(temp);
					break;
				default:
					itemShopArmorList.Add(temp);
					break;
				}
			}
		}
	}
	public ItemShopObjData GetItemShopData(bool isSpecial,int id,int perfab,int gem,int enchant,int element,int level) {
		if(isSpecial) {
			foreach(ItemShopObjData data in itemShopSpecialList) {
				if(data.localData._ItemID == id && data.localData._PrefabID == perfab && data.localData._GemID == gem && data.localData._EnchantID == enchant
					&& data.localData._EleID == element && data.localData.info_Level == level) {
					return data;
				}
			}
			return null;
		}
		ItemDropStruct tLocalData = ItemDeployInfo.Instance.GetItemObject(id,perfab,gem,enchant,element,level);
		if(tLocalData != null) {
			switch(tLocalData._TypeID) {
			case 7:
			case 8:
				foreach(ItemShopObjData data in itemShopWeaponList) {
					if(data.localData._ItemID == id && data.localData._PrefabID == perfab && data.localData._GemID == gem && data.localData._EnchantID == enchant
						&& data.localData._EleID == element && data.localData.info_Level == level) {
						return data;
					}
				}
				return null;
			case 2:
			case 5:
				foreach(ItemShopObjData data in itemShopAccessoriesList) {
					if(data.localData._ItemID == id && data.localData._PrefabID == perfab && data.localData._GemID == gem && data.localData._EnchantID == enchant
						&& data.localData._EleID == element && data.localData.info_Level == level) {
						return data;
					}
				}
				return null;
			default:
				foreach(ItemShopObjData data in itemShopArmorList) {
					if(data.localData._ItemID == id && data.localData._PrefabID == perfab && data.localData._GemID == gem && data.localData._EnchantID == enchant
						&& data.localData._EleID == element && data.localData.info_Level == level) {
						return data;
					}
				}
				return null;
			}
		}
		return null;
	}
	
	public void RemoveItemShopData(SBuyitemInfo info,bool isSpecial) {
		ItemShopObjData data = GetItemShopData(isSpecial,info.ID,info.perfrab,info.gem,info.enchant,info.element,(int)info.level);
		if(data == null) {
			GUILogManager.LogErr("RemoveItemShopData fail.");
			return;
		}
		ItemDropStruct tLocalData = ItemDeployInfo.Instance.GetItemObject(info.ID,info.perfrab,info.gem,info.enchant,info.element,(int)info.level);
		if(isSpecial) {
			CS_Main.Instance.g_commModule.SendMessage(ProtocolGame_SendRequest.RequestPlayerShopInfo(true));
		}else {
			bool isRemoveSuccess = false;
			switch(tLocalData._TypeID) {
			case 7:
			case 8:
				isRemoveSuccess = itemShopWeaponList.Remove(data);
				break;
			case 2:
			case 5:
				isRemoveSuccess = itemShopAccessoriesList.Remove(data);
				break;
			default:
				isRemoveSuccess = itemShopArmorList.Remove(data);
				break;
			}
			GUILogManager.LogInfo("RemoveItemShopData isRemoveSuccess:"+isRemoveSuccess.ToString());
			if(ItemShopScreenCtrl.Instance) {
				ItemShopScreenCtrl.Instance.UpdateCurItemShopList();
			}
		}
	}
	#endregion
	
	#region Money
	private int karmaVal;
	private int crystalVal;
	public void SetKarmaVal(int karma) {
		karmaVal = karma;
		if(MoneyBarManager.Instance) {
			MoneyBarManager.Instance.SetKarmaVal(karma);
		}
	}
	public int GetKarmaVal() {
		return karmaVal;
	}
	public void SetCrystalVal(int crystal) {
		crystalVal = crystal;
		if(MoneyBarManager.Instance) {
			MoneyBarManager.Instance.SetCrystalVal(crystal);
		}
	}
	public int GetCrystalVal() {
		return crystalVal;
	}
	#endregion
	
	#region Recharge
	public RechargeValData rechargeValData = new RechargeValData();
	private void IntitRechargeValData() {
		string fileName = "BuyKarma.Costs";
		string _fileName = LocalizeManage.Instance.GetLangPath(fileName);
		TextAsset item = (TextAsset)Resources.Load(_fileName, typeof(TextAsset));
		string[] itemRowsList = item.text.Split('\n');
		for (int i = 3; i < itemRowsList.Length-1; ++i){
			string pp = itemRowsList[i];
			string[] vals = pp.Split(new char[] { '	', '	' });
			if(int.Parse(vals[0]) == VersionManager.Instance.GetPlatformType().Get()){
				rechargeValData.karmaVal[0] = vals[1];
				rechargeValData.karmaVal[1] = vals[3];
				rechargeValData.karmaVal[2] = vals[5];
				rechargeValData.karmaVal[3] = vals[7];
				rechargeValData.karmaVal[4] = vals[9];
				rechargeValData.karmaVal[5] = vals[11];
				rechargeValData.karmaVal[6] = vals[13];
				
				rechargeValData.crystalVal[0] = vals[22];
				rechargeValData.crystalVal[1] = vals[24];
				rechargeValData.crystalVal[2] = vals[26];
				rechargeValData.crystalVal[3] = vals[28];
				rechargeValData.crystalVal[4] = vals[30];
				rechargeValData.crystalVal[5] = vals[32];
				rechargeValData.crystalVal[6] = vals[34];
				
				rechargeValData.karmaPayVal[0] = vals[15] + "$";
				rechargeValData.karmaPayVal[1] = vals[16] + "$";
				rechargeValData.karmaPayVal[2] = vals[17] + "$";
				rechargeValData.karmaPayVal[3] = vals[18] + "$";
				rechargeValData.karmaPayVal[4] = vals[19] + "$";
				rechargeValData.karmaPayVal[5] = vals[20] + "$";
				rechargeValData.karmaPayVal[6] = vals[21] + "$";
					
				rechargeValData.crystalPayVal[0] = vals[36] + "$";
				rechargeValData.crystalPayVal[1] = vals[37] + "$";
				rechargeValData.crystalPayVal[2] = vals[38] + "$";
				rechargeValData.crystalPayVal[3] = vals[39] + "$";
				rechargeValData.crystalPayVal[4] = vals[40] + "$";
				rechargeValData.crystalPayVal[5] = vals[41] + "$";
				rechargeValData.crystalPayVal[6] = vals[42] + "$";
				return;
			}	
		}
	}
	public string karmaRechargTitle;
	public string crystalRechargTitle;
	void ReadRechargTitleData() {
		string fileName = "BuyKarma.Info";
		string _fileName = LocalizeManage.Instance.GetLangPath(fileName);
		TextAsset item = (TextAsset)Resources.Load(_fileName, typeof(TextAsset));
		string[] itemRowsList = item.text.Split('\n');
		for (int i = 3; i < itemRowsList.Length; ++i){		
			string pp = itemRowsList[i];		
			string[] vals = pp.Split(new char[] { '	', '	' });	
				if(i == 3){
					karmaRechargTitle = vals[0];
				}else if(i == 4){
					crystalRechargTitle = vals[0];
					return;
				}
		}	
	}
	#endregion
	
	#region Ping
	public float sendPingTime;
	#endregion

    #region Revive
    private int RevivalCount = 0;
    public void SetRevivalCount(int count)
    {
		if(count < 0) count = 0;
		if(count > punishmentReward.Length - 1)count = punishmentReward.Length - 1;
		
        RevivalCount = count;
    }
    public int GetRevivalCount()
    {
        return RevivalCount;
    }
    public void AddRevivalCount()
    {
        RevivalCount++;
    }
    #endregion

#region mail

    public SMailInfo[] MailList = new SMailInfo[0];


#endregion
	
	#region TutorialPanel
	public string tutorialTitle;
	public string tutorialContent;
	public void ShowTutorialPanel(string title,string content) {
		tutorialTitle = title;
		tutorialContent = content;
		GUIManager.Instance.AddTemplate("TutorialPanel");
	}
	#endregion

    private void ChangeScreenNameDelegate(string name) {
		if(mapDisableEnable.Instance) {
			if(string.Compare(name,"IngameScreen") == 0) {
				mapDisableEnable.Instance.turnMapOn();
			}else {
				mapDisableEnable.Instance.turnMapOff();
			}
		}
	}
}

public class _ItemInfo {
	public int slot;
	public int count;
	public bool empty;
	public bool isChange = true;
	public SItemInfo serData;
	public ItemDropStruct localData;
}

public class CoolDownCost{
	public int time;
	public int crystal;
	public int sub30Min;
	public int sub60Min;
}

public class IngameMaterialStruct {
	public ItemDropStruct data;
	public int count;
	
}

public class ItemShopObjData {
	public SShopItemInfo serData;
	public ItemDropStruct localData;
}

public class MissDescData {
	public int missID;
	public string areaName;
	public string areaDesc;
	public string recommendLv;
	public string lockAreaDesc1;
	public string lockAreaDesc2;
	public string lockAreaDesc3;
	public string missName;
	public string missInfo;
	public string missSucc;
	public string missCDDesc;
	public string enemyIconName;
	public string enemyInfo;
	public string matIconName1;
	public string matIconName2;
	public string matIconName3;
	public string matInfo;
	public string bossInfo;
	public string rcLow;
	public string rcMed;
	public string rcHi;
	public string rcOvr;
	public string missIconName;
}

public class RechargeValData {
	public string [] karmaVal = {"0","0","0","0","0","0","0"};
	public string [] crystalVal = {"0","0","0","0","0","0","0"};
	public string [] karmaPayVal = {"0","0","0","0","0","0","0"};
	public string [] crystalPayVal = {"0","0","0","0","0","0","0"};
}
