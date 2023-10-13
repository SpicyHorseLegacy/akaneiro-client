using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class CreateScreenCtrl : MonoBehaviour {
	
	public class ItemInfo{
		public int itemID;
		public int perfabID;
		public int slot;
	}
	
	public static CreateScreenCtrl Instance;
	
	void Awake() {
		Instance = this;
		InitInfo();
		RegisterInitEvent();
	}
	
	void Start () 
    {
        UI_BlackBackground_Control.CloseBlackBackground();
	}
	
	#region Interface
	public void BackDelegate() {
		UI_Fade_Control.Instance.FadeOutAndIn("SelectScreen");
		StartCoroutine(ChangeToEmpetScreen());
	}
	public IEnumerator ChangeToEmpetScreen() {	
		AsyncOperation async= Application.LoadLevelAsync("EmptyScenes");
		yield return async;
		GUILogManager.LogInfo("Screen To EmptyScenes screen");
	}
	#endregion
	
	#region Local
	#region event create and destory
	//MAX template count.//
	private int initDelegateCount = 1;
	private void TemplateInitEnd() {
		if(GUIManager.Instance.GetTemplateInitEndCount() >= initDelegateCount) {
			RegisterTemplateEvent();
			Init();
		}
	}
	private void RegisterInitEvent() {
		GUIManager.Instance.OnInitEndDelegate += TemplateInitEnd;
		GUIManager.Instance.OnScreenManagerDestroy += DestoryAllEvent;
	}
	
	private void RegisterTemplateEvent() {
		if(CreateBase.Instance) {
			CreateBase.Instance.OnProwessDelegate += ProwessDelegate;
			CreateBase.Instance.OnFortitudeDelegate += FortitudeDelegate;
			CreateBase.Instance.OnCunningDelegate += CunningDelegate;
			CreateBase.Instance.OnBackDelegate += BackDelegate;
			CreateBase.Instance.OnFemaleDelegate += SexChange;
			CreateBase.Instance.OnMaleDelegate += SexChange;
			CreateBase.Instance.OnCreateDelegate += CreateDelegate;
		}
	}
	
	private void DestoryAllEvent() {
		if(CreateBase.Instance) {
			CreateBase.Instance.OnProwessDelegate -= ProwessDelegate;
			CreateBase.Instance.OnFortitudeDelegate -= FortitudeDelegate;
			CreateBase.Instance.OnCunningDelegate -= CunningDelegate;
			CreateBase.Instance.OnBackDelegate -= BackDelegate;
			CreateBase.Instance.OnFemaleDelegate -= SexChange;
			CreateBase.Instance.OnMaleDelegate -= SexChange;
			CreateBase.Instance.OnCreateDelegate -= CreateDelegate;
		}
		GUIManager.Instance.OnInitEndDelegate -= TemplateInitEnd;
		GUIManager.Instance.OnScreenManagerDestroy -= DestoryAllEvent;
	}
	#endregion
	private void Init() {
		InitCreatePlayerModelEquipData();
		//Default.//
		CreateInfo.Instance.SetCurObject(infoList[1]);
		UpdatePlayerAllEquipment(AbilityDetailInfo.EDisciplineType.EDT_Fortitude,CreateBase.Instance.GetSex());
		PlayModelAni(AbilityDetailInfo.EDisciplineType.EDT_Fortitude);
		UI_Fade_Control.Instance.FadeIn();
	}
	
	private List<CreateDisciplineInfo> infoList = new List<CreateDisciplineInfo>();
	private string fileName = "CreateCharaInfo.Description";
	private void InitInfo() {
		infoList.Clear();
		TextAsset file = null;
		string _fileName = LocalizeManage.Instance.GetLangPath(fileName);
		file = (TextAsset)Resources.Load(_fileName, typeof(TextAsset));
		if(file == null) {
			GUILogManager.LogErr("data file not exist. file name: <"+_fileName+">");
		}
		string[] fileRowList = file.text.Split('\n');
		for (int j = 3; j < fileRowList.Length - 1; j++) {
			string pp = fileRowList[j];
			string[] vals = pp.Split(new char[] { '	', '	' });
			if(vals[0] != null) {
				CreateDisciplineInfo tempInfo = new CreateDisciplineInfo();
				tempInfo.disciplineName = vals[7];
				tempInfo.disciplineIcon = vals[8];
				tempInfo.disciplineInfo = vals[0] + "\n\n" + vals[1];
				tempInfo.bounesInfo = vals[2];
				tempInfo.masteryInfo = vals[4] + "\n" + vals[5];
				GetAbilitiesInfo(vals[6],tempInfo);
				GetAbilitiesInfo2(vals[10],tempInfo);
				GetAbilitiesInfo3(vals[11],tempInfo);
				infoList.Add(tempInfo);
			}
		}
	}
	
	private void GetAbilitiesInfo(string abiID,CreateDisciplineInfo info) {
		int ID =0;
		try{
			ID = int.Parse(abiID);
		}catch(System.Exception e) {
			GUILogManager.LogErr("Create Init Abilities Info err,Abilitie ID: "+abiID);
			return;
		}
		for(int index = 0; index < AbilityInfo.Instance.PlayerAbilityPool.Count;index++){
			AbilityBaseState ability = AbilityInfo.Instance.PlayerAbilityPool[index];
			if(ability.id == ID){	
				info.abilityName = ability.Info.Name;
				info.abilityIcon = ability.icon;
				info.abilityLevel = "" + ability.Level;
				info.abilityForDisciplineName = PlayerDataManager.Instance.DisciplineTypeToString(ability.Info.Discipline);
				info.abilityInfo = ability.Info.Description1;
				return;
			}
		}
		
		GUILogManager.LogErr("Create Init Abilities Info cant find target obj,Abilitie ID: "+abiID);
	}
	//
	private void GetAbilitiesInfo2(string abiID,CreateDisciplineInfo info) {
		int ID =0;
		try{
			ID = int.Parse(abiID);
		}catch(System.Exception e) {
			GUILogManager.LogErr("Create Init Abilities Info err,Abilitie ID: "+abiID);
			return;
		}
		for(int index = 0; index < AbilityInfo.Instance.PlayerAbilityPool.Count;index++){
			AbilityBaseState ability = AbilityInfo.Instance.PlayerAbilityPool[index];
			if(ability.id == ID){	
				info.abilityName2 = ability.Info.Name;
				info.abilityIcon2 = ability.icon;
				info.abilityInfo2 = ability.Info.Description1;
				return;
			}
		}
		
		GUILogManager.LogErr("Create Init Abilities Info cant find target obj,Abilitie ID: CCCCCCCC"+abiID);
	}
	private void GetAbilitiesInfo3(string abiID,CreateDisciplineInfo info) {
		int ID =0;
		try{
			ID = int.Parse(abiID);
		}catch(System.Exception e) {
			GUILogManager.LogErr("Create Init Abilities Info err,Abilitie ID: "+abiID);
			return;
		}
		for(int index = 0; index < AbilityInfo.Instance.PlayerAbilityPool.Count;index++){
			AbilityBaseState ability = AbilityInfo.Instance.PlayerAbilityPool[index];
			if(ability.id == ID){	
				info.abilityName3 = ability.Info.Name;
				info.abilityIcon3 = ability.icon;
				info.abilityInfo3 = ability.Info.Description1;
				return;
			}
		}
		
		GUILogManager.LogErr("Create Init Abilities Info cant find target obj,Abilitie ID: XXXXXX"+abiID);
	}
	//mm
	
	private void ProwessDelegate() {
		CreateInfo.Instance.SetCurObject(infoList[0]);
		UpdatePlayerAllEquipment(AbilityDetailInfo.EDisciplineType.EDT_Prowess,CreateBase.Instance.GetSex());
		PlayModelAni(AbilityDetailInfo.EDisciplineType.EDT_Prowess);
	}
	private void FortitudeDelegate() {
		CreateInfo.Instance.SetCurObject(infoList[1]);
		UpdatePlayerAllEquipment(AbilityDetailInfo.EDisciplineType.EDT_Fortitude,CreateBase.Instance.GetSex());
		PlayModelAni(AbilityDetailInfo.EDisciplineType.EDT_Fortitude);
	}
	private void CunningDelegate() {
		CreateInfo.Instance.SetCurObject(infoList[2]);
		UpdatePlayerAllEquipment(AbilityDetailInfo.EDisciplineType.EDT_Cunning,CreateBase.Instance.GetSex());
		PlayModelAni(AbilityDetailInfo.EDisciplineType.EDT_Cunning);
	}
	private void SexChange() {
		UpdatePlayerAllEquipment(CreateBase.Instance.GetCurType(),CreateBase.Instance.GetSex());
		PlayModelAni(CreateBase.Instance.GetCurType());
	}
	private void CreateDelegate() {
		//Check input name length//
		int length = CreateBase.Instance.GetInputText().Length;
		if( length < 3 || length > 12){
			PopUpBox.PopUpErr(LocalizeManage.Instance.GetDynamicText("CREATECHARINPUTERR"));
			return ;
		}
		SCharacterCreationData scd = new SCharacterCreationData();
		scd.nickname = CreateBase.Instance.GetInputText();
		scd.style = (int)CreateBase.Instance.GetCurType();
		mapIntInt avatarTraits = new mapIntInt();
		scd.avatarTraits = avatarTraits;
		scd.sex = new ESex();
		scd.sex.Set(CreateBase.Instance.GetSex().Get());
		CS_Main.Instance.g_commModule.SendMessage(
	   		ProtocolGame_SendRequest.UserCreateCharacter(scd)
		);
		PopUpBox.PopUpErr(LocalizeManage.Instance.GetDynamicText("CREATEINGCHARA"));
	}

	private void UpdatePlayerAllEquipment(AbilityDetailInfo.EDisciplineType type,ESex _gender) {
		PlayerModel pm = CreateBase.Instance.GetPlayerModel();
		SItemInfo itemInfo	= new SItemInfo();
		pm.equipmentMan.DetachAllItems(_gender);
		switch(type) {
		case AbilityDetailInfo.EDisciplineType.EDT_Prowess:
			foreach(ItemInfo info in prowessItemList) {
				Transform item = UnityEngine.Object.Instantiate(ItemPrefabs.Instance.GetItemPrefab(info.itemID, 0, info.perfabID)) as Transform;
				pm.equipmentMan.UpdateItemInfoBySlot((uint)info.slot, item, itemInfo, true,_gender);	
			}
			break;
		case AbilityDetailInfo.EDisciplineType.EDT_Fortitude:
			foreach(ItemInfo info in fortitudeItemList) {
				Transform item = UnityEngine.Object.Instantiate(ItemPrefabs.Instance.GetItemPrefab(info.itemID, 0, info.perfabID)) as Transform;
				pm.equipmentMan.UpdateItemInfoBySlot((uint)info.slot, item, itemInfo, true,_gender);	
			}
			break;
		case AbilityDetailInfo.EDisciplineType.EDT_Cunning:
			foreach(ItemInfo info in cunningItemList) {
				Transform item = UnityEngine.Object.Instantiate(ItemPrefabs.Instance.GetItemPrefab(info.itemID, 0, info.perfabID)) as Transform;
				pm.equipmentMan.UpdateItemInfoBySlot((uint)info.slot, item, itemInfo, true,_gender);	
			}
			break;
		}
        pm.equipmentMan.UpdateEquipment(_gender);
		pm.usingLatestConfig = true;
	}
	
	private void PlayModelAni(AbilityDetailInfo.EDisciplineType type) {
		//sound//
		PlayModelSound(type);
		string aniName = "";
		switch(type) {
		case AbilityDetailInfo.EDisciplineType.EDT_Prowess:
			aniName = "POW_UI_Idle_2";
			break;
		case AbilityDetailInfo.EDisciplineType.EDT_Fortitude:
			aniName = "FOR_UI_Idle_2";
			break;
		case AbilityDetailInfo.EDisciplineType.EDT_Cunning:
			aniName = "CUN_UI_Idle_2";
			break;
		}
		PlayerModel pm = CreateBase.Instance.GetPlayerModel();
		
		Animation _ani = null;
		Animation[] allanims = pm.GetComponentsInChildren<Animation>();
		for(int i = 0; i < allanims.Length; i ++)
		{
			if(allanims[i].transform.name == "Aka_Model")
			{
				_ani = allanims[i];
			}
		}
		
		_ani[aniName].layer = 99;
		_ani.CrossFade(aniName);
	}
	
	[SerializeField]
	private Transform prowessSound;
	[SerializeField]
	private Transform fortitudeSound;
	[SerializeField]
	private Transform cunningSound;
	private void PlayModelSound(AbilityDetailInfo.EDisciplineType type) {
		switch(type) {
		case AbilityDetailInfo.EDisciplineType.EDT_Prowess:
			if(fortitudeSound.GetComponent<AudioSource>().isPlaying) {
				fortitudeSound.GetComponent<AudioSource>().Stop();
			}
			if(cunningSound.GetComponent<AudioSource>().isPlaying) {
				cunningSound.GetComponent<AudioSource>().Stop();
			}
			SoundCue.PlayPrefabAndDestroy(prowessSound);
			break;
		case AbilityDetailInfo.EDisciplineType.EDT_Fortitude:
			if(prowessSound.GetComponent<AudioSource>().isPlaying) {
				prowessSound.GetComponent<AudioSource>().Stop();
			}
			if(cunningSound.GetComponent<AudioSource>().isPlaying) {
				cunningSound.GetComponent<AudioSource>().Stop();
			}
			SoundCue.PlayPrefabAndDestroy(fortitudeSound);
			break;
		case AbilityDetailInfo.EDisciplineType.EDT_Cunning:
			if(prowessSound.GetComponent<AudioSource>().isPlaying) {
				prowessSound.GetComponent<AudioSource>().Stop();
			}
			if(fortitudeSound.GetComponent<AudioSource>().isPlaying) {
				fortitudeSound.GetComponent<AudioSource>().Stop();
			}
			SoundCue.PlayPrefabAndDestroy(cunningSound);
			break;
		}
	}
	
	#region Init Create player model equipment
	private List<ItemInfo> prowessItemList = new List<ItemInfo>();
	private List<ItemInfo> fortitudeItemList = new List<ItemInfo>();
	private List<ItemInfo> cunningItemList = new List<ItemInfo>();
	private void InitCreatePlayerModelEquipData() {
		InitProwessEquip();
		InitFortitudeEquip();
		InitCunningEquip();
	}
	private void InitProwessEquip() {
		prowessItemList.Clear();
		ItemInfo p0Temp = new ItemInfo();
		p0Temp.slot = 6;p0Temp.itemID = 1002;p0Temp.perfabID = 1;
		prowessItemList.Add(p0Temp);
		ItemInfo p1Temp = new ItemInfo();
		p1Temp.slot = 7;p1Temp.itemID = 1002;p1Temp.perfabID = 1;
		prowessItemList.Add(p1Temp);
		ItemInfo p2Temp = new ItemInfo();
		p2Temp.slot = 0;p2Temp.itemID = 2001;p2Temp.perfabID = 301;
		prowessItemList.Add(p2Temp);
		ItemInfo p3Temp = new ItemInfo();
		p3Temp.slot = 2;p3Temp.itemID = 2002;p3Temp.perfabID = 301;
		prowessItemList.Add(p3Temp);
		ItemInfo p4Temp = new ItemInfo();
		p4Temp.slot = 8;p4Temp.itemID = 2003;p4Temp.perfabID = 301;
		prowessItemList.Add(p4Temp);
	}
	private void InitFortitudeEquip() {
		fortitudeItemList.Clear();
		ItemInfo f1Temp = new ItemInfo();
		f1Temp.slot = 6;f1Temp.itemID = 1007;f1Temp.perfabID = 1;
		fortitudeItemList.Add(f1Temp);
		ItemInfo f2Temp = new ItemInfo();
		f2Temp.slot = 0;f2Temp.itemID = 2001;f2Temp.perfabID = 401;
		fortitudeItemList.Add(f2Temp);
		ItemInfo f3Temp = new ItemInfo();
		f3Temp.slot = 2;f3Temp.itemID = 2002;f3Temp.perfabID = 401;
		fortitudeItemList.Add(f3Temp);
		ItemInfo f4Temp = new ItemInfo();
		f4Temp.slot = 8;f4Temp.itemID = 2003;f4Temp.perfabID = 401;
		fortitudeItemList.Add(f4Temp);
	}
	private void InitCunningEquip() {
		cunningItemList.Clear();
		ItemInfo c1Temp = new ItemInfo();
		c1Temp.slot = 6;c1Temp.itemID = 1001;c1Temp.perfabID = 1;
		cunningItemList.Add(c1Temp);
		ItemInfo c2Temp = new ItemInfo();
		c2Temp.slot = 0;c2Temp.itemID = 2001;c2Temp.perfabID = 201;
		cunningItemList.Add(c2Temp);
		ItemInfo c3Temp = new ItemInfo();
		c3Temp.slot = 2;c3Temp.itemID = 2002;c3Temp.perfabID = 201;
		cunningItemList.Add(c3Temp);
		ItemInfo c4Temp = new ItemInfo();
		c4Temp.slot = 8;c4Temp.itemID = 2003;c4Temp.perfabID = 201;
		cunningItemList.Add(c4Temp);
	}
	#endregion
	#endregion
}
