using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;


//ver 1.0 每次读取物品信息判断是否已经存在，不存在加载信息;//
//ver 2.0 程序初始化时一次加载所有物品文件信息，然后初始化各个部件信息，从各个文件的信息结构体中//
public class ItemDeployInfo : MonoBehaviour {
	
	public static ItemDeployInfo Instance;
	
	public List<ItemDropMapping> 	_MappingList   = new List<ItemDropMapping>();
	public List<ItemDropStruct> 	_StructList   = new List<ItemDropStruct>();
	
	private static LocalizeManage localizeMgr_ = null;
	
	void Awake() {
		Instance = this;
	}
	
	// Use this for initialization
	void Start () {
		InitItemFile();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void InitItemFile() {
		InitPropsList();
		InitPrefabList();
		InitBaseInfoList();
		InitGemInfoList();
		InitEncInfoList();
		InitEleInfoList();
		InitModifierInfoList();
	}
	
	void setLocalizeText(LocalizeManage.Language _lang) {
		InitItemFile();
	}
	
#region GetItemObj	
	public ItemDropStruct GetItemObject(int ID,int PrefabID,int Gem,int Enchant,int Element,int level){
		int idx = -1;	
		// no need, 以前是每次读取文件，现在改为一次读取，空间换时间，所以不需要存储//
//		idx = IsExistElement(ID,PrefabID,Gem,Enchant,Element,level);
//		if(-1 != idx){
//			return GetItemDropStructFromList(idx);
//		}else{
			idx = ReadDeployInfo(ID,PrefabID,Gem,Enchant,Element,level);
//			ItemDropMapping tMapping = new ItemDropMapping();
//			tMapping.ID 	= ID;
//			tMapping.Index  = idx;
//			_MappingList.Add(tMapping);
			if(-1 != idx){	
				return GetItemDropStructFromList(idx);
			}else{
//				LogManager.Log_Error("Unknown id, I Can not find doc info" + ID);
			}
//		}
		return null;
	}
	
	public int IsExistElement(int ID,int PrefabID,int Gem,int Enchant,int Element,int level){
		
		foreach (ItemDropMapping mapping in _MappingList)
		{
			if(ID == mapping.ID&&PrefabID == mapping.PrefabID&& Gem == mapping.GemID
			   &&Enchant == mapping.EnchantID&&Element == mapping.EleID&&level == mapping.Level){
				return mapping.Index;
			}
		}
		
		return -1;
	}
	
	public int ReadDeployInfo(int ID,int PrefabID,int Gem,int Enchant,int Element,int level){
		
		ItemDropStruct temp = new ItemDropStruct();
		int nType = -1;
		string sPrefabName;
		int nMappingIdx;
		
		temp._ItemID    = ID;
		temp._PrefabID  = PrefabID;
		temp._GemID     = Gem;
		temp._EnchantID = Enchant;
		temp._EleID     = Element;
		temp.info_Level = level;
		
		nType = SetTypeID(temp,ID);
		
		if(-1 ==  nType){
//			LogManager.Log_Error("Unkonw id, I Can not find doc info <ID> " + ID);
			return -1;
		}
		
		sPrefabName = SetPrefabID(temp,nType,PrefabID);
		
		if(null ==  sPrefabName){
//			LogManager.Log_Error("Unkonw id, I Can not find doc info <PrefabID> " + PrefabID);
			return -1;
		}
		
		SetBaseInfo(temp,nType,ID);
		
		SetGemInfo(temp,nType,Gem);
		
		SetEncInfo(temp,nType,Enchant);
		
		SetEleInfo(temp,nType,Element);
		
		SetModifierInfo(temp,nType,level);
		
		nMappingIdx = InputToList(temp);
		
		return nMappingIdx;
	}
	
	public ItemDropStruct GetItemDropStructFromList(int idx){
		
		if(null != _StructList[idx]){
		
			return _StructList[idx];
			
		}else{
//			LogManager.Log_Warn("Unkonw id, I Can not find doc info <_StructList[idx]>" + idx);
			return null;
			
		}
	}
#endregion
	
#region Props
	public struct SItemProps {
		public int id;
		public int typeId;
		public int secType;
		public int saleVal;
		public string propsDes;
		public string propsDes2;
		public string propsName;
		public int	isUseRealMoney;
	}
	
	public List<SItemProps> 	_SItemPropsList   = new List<SItemProps>();
	
	public void InitPropsList() {
		_SItemPropsList.Clear();
		string _fileName = LocalizeManage.Instance.GetLangPath("Props.General");
		TextAsset item = (TextAsset)Resources.Load(_fileName, typeof(TextAsset));
		string[] itemRowsList = item.text.Split('\n');
		for (int i = 3; i < itemRowsList.Length - 1; ++i){
			SItemProps temp = new SItemProps();
			string pp = itemRowsList[i];
			string[] vals = pp.Split(new char[] { '	', '	' });
			temp.id			= int.Parse(vals[0]);		
			temp.typeId 	= int.Parse(vals[9]);		
			temp.secType 	= int.Parse(vals[14]);
			temp.saleVal 	= int.Parse(vals[4]);
			temp.propsDes  	= vals[15];
			temp.propsDes2 	= vals[3];
			temp.propsName 	= vals[1];
			temp.isUseRealMoney = int.Parse(vals[16]);	
			_SItemPropsList.Add(temp);
		}
	}

    public static SItemProps GetItemPropsByID(int _id)
    {
        foreach (SItemProps _iteminfo in Instance._SItemPropsList)
        {
            if (_iteminfo.id == _id)
                return _iteminfo;
        }
        return new SItemProps();
    }
	
	public int SetTypeID(ItemDropStruct obj,int ID){
		foreach(SItemProps sip in _SItemPropsList) {
			if(ID == sip.id) {
				obj._TypeID 	= sip.typeId;		
				obj._SecTypeID 	= sip.secType;
				obj._SaleVal 	= sip.saleVal;
				obj._PropsDes  	= sip.propsDes;
				obj._PropsDes2 	= sip.propsDes2;
				obj._PropsName 	= sip.propsName;
				obj._isUseRealMoney = sip.isUseRealMoney;	
				return sip.typeId;
			}
		}
		return -1;
	}
	
	//以前使用这个,现在使用新的重载函数//
//	public int SetTypeID(ItemDropStruct obj,int ID){
//		string _fileName = LocalizeManage.Instance.GetLangPath("Props.General");
//		TextAsset item = (TextAsset)Resources.Load(_fileName, typeof(TextAsset));
//		string[] itemRowsList = item.text.Split('\n');
//		for (int i = 3; i < itemRowsList.Length; ++i){
//			string pp = itemRowsList[i];
//			string[] vals = pp.Split(new char[] { '	', '	' });
//				if(string.Compare(ID.ToString(),vals[0])==0) {
//				obj._TypeID 	= int.Parse(vals[9]);		
//				obj._SecTypeID 	= int.Parse(vals[14]);
//				obj._SaleVal 	= int.Parse(vals[4]);
//				obj._PropsDes  	= vals[15];
//				obj._PropsDes2 	= vals[3];
//				obj._PropsName 	= vals[1];
//				obj._isUseRealMoney = int.Parse(vals[16]);		
//				return obj._TypeID;
//			}
//		}
//		return -1;
//	}
#endregion
	
#region Prefab
	public struct SItemPrefab {
		public int id;
		public string prefabName;
		public string typeName;
		public string qualityName;
		public string armorLevel;
		public string typelastNameHead;
		public string typelastNameBody;
		public string typelastNameLeg;
		public string cloakName;
	}
	
	public List<SItemPrefab> 	_SItemArmorAttrPrefabList   	  = new List<SItemPrefab>();
	public List<SItemPrefab> 	_SItemAccessoriesAttrPrefabList   = new List<SItemPrefab>();
	public List<SItemPrefab> 	_SItemCloakPrefabList   	 	  = new List<SItemPrefab>();
	public List<SItemPrefab> 	_SItemWeaponAttrPrefabList  	  = new List<SItemPrefab>();
	
	public void InitPrefabList() {
		_SItemArmorAttrPrefabList.Clear();
		_SItemAccessoriesAttrPrefabList.Clear();
		_SItemCloakPrefabList.Clear();
		_SItemWeaponAttrPrefabList.Clear();
		string 		_fileName = "";
		TextAsset 	item;
		_fileName = LocalizeManage.Instance.GetLangPath("ArmorAttribute.prefab");
		item = (TextAsset)Resources.Load(_fileName, typeof(TextAsset));
		string[] itemRowsList1 = item.text.Split('\n');
		for (int i = 3; i < itemRowsList1.Length - 1; ++i){
			SItemPrefab temp = new SItemPrefab();
			string pp = itemRowsList1[i];
			string[] vals = pp.Split(new char[] { '	', '	' });
			temp.id			= int.Parse(vals[0]);		
			temp.prefabName = vals[1];		
			temp.typeName 	= vals[4];
			temp.qualityName= "";
			temp.armorLevel = vals[9];
			temp.typelastNameHead = vals[5];
			temp.typelastNameBody = vals[6];
			temp.typelastNameLeg  = vals[7];
			temp.cloakName 	= "";
			_SItemArmorAttrPrefabList.Add(temp);
		}
		
		_fileName = LocalizeManage.Instance.GetLangPath("AccessoriesAttribute.prefab");
		item = (TextAsset)Resources.Load(_fileName, typeof(TextAsset));
		string[] itemRowsList2 = item.text.Split('\n');
		for (int i = 3; i < itemRowsList2.Length - 1; ++i){
			SItemPrefab temp = new SItemPrefab();
			string pp = itemRowsList2[i];
			string[] vals = pp.Split(new char[] { '	', '	' });
			temp.id			= int.Parse(vals[0]);		
			temp.prefabName = vals[1];		
			temp.typeName 	= "";
			temp.qualityName= "";
			temp.armorLevel = "";
			temp.typelastNameHead = "";
			temp.typelastNameBody = "";
			temp.typelastNameLeg  = "";
			temp.cloakName 	= "";
			_SItemAccessoriesAttrPrefabList.Add(temp);
		}
		
		_fileName = LocalizeManage.Instance.GetLangPath("ArmorAttribute.Prefab_cloak");
		item = (TextAsset)Resources.Load(_fileName, typeof(TextAsset));
		string[] itemRowsList3 = item.text.Split('\n');
		for (int i = 3; i < itemRowsList3.Length - 1; ++i){
			SItemPrefab temp = new SItemPrefab();
			string pp = itemRowsList3[i];
			string[] vals = pp.Split(new char[] { '	', '	' });
			temp.id			= int.Parse(vals[0]);		
			temp.prefabName = vals[1];		
			temp.typeName 	= "";
			temp.qualityName= vals[4];
			temp.armorLevel = "";
			temp.typelastNameHead = "";
			temp.typelastNameBody = "";
			temp.typelastNameLeg  = "";
			temp.cloakName 	= vals[6];
			_SItemCloakPrefabList.Add(temp);
		}
		
		_fileName = LocalizeManage.Instance.GetLangPath("WeaponAttribute.prefab");
		item = (TextAsset)Resources.Load(_fileName, typeof(TextAsset));
		string[] itemRowsList4 = item.text.Split('\n');
		for (int i = 3; i < itemRowsList4.Length - 1; ++i){
			SItemPrefab temp = new SItemPrefab();
			string pp = itemRowsList4[i];
			string[] vals = pp.Split(new char[] { '	', '	' });
			temp.id			= int.Parse(vals[0]);		
			temp.prefabName = vals[1];		
			temp.typeName 	= "";
			temp.qualityName= "";
			temp.armorLevel = "";
			temp.typelastNameHead = "";
			temp.typelastNameBody = "";
			temp.typelastNameLeg  = "";
			temp.cloakName 	= "";
			_SItemWeaponAttrPrefabList.Add(temp);
		}
	}
	
	public string SetPrefabID(ItemDropStruct obj,int type,int ID){
		switch(type) {
		case 1:
		case 3:	
		case 6:	
			foreach(SItemPrefab sip in _SItemArmorAttrPrefabList) {
				if(ID == sip.id) {
					obj._PrefabName = sip.prefabName;
					obj._TypeName	= sip.typeName;
					obj._QualityName= sip.qualityName;
					obj.info_ArmorLevel= sip.armorLevel;
					if(1 == type){
						obj._TypelastName = sip.typelastNameHead;
					}else if(3 == type){
						obj._TypelastName = sip.typelastNameBody;
					}else if(6 == type){
						obj._TypelastName = sip.typelastNameLeg;
					}
					obj._CloakName = sip.cloakName;
					return obj._PrefabName;
				}
			}
			return "";
		case 2:
		case 5:	
			foreach(SItemPrefab sip in _SItemAccessoriesAttrPrefabList) {
				if(ID == sip.id) {
					obj._PrefabName = sip.prefabName;
					obj._TypeName	= sip.typeName;
					obj._QualityName= sip.qualityName;
					obj.info_ArmorLevel= sip.armorLevel;
					obj._TypelastName = "";
					obj._CloakName = sip.cloakName;
					return obj._PrefabName;
				}
			}
			return "";
		case 4:
			foreach(SItemPrefab sip in _SItemCloakPrefabList) {
				if(ID == sip.id) {
					obj._PrefabName = sip.prefabName;
					obj._TypeName	= sip.typeName;
					obj._QualityName= sip.qualityName;
					obj.info_ArmorLevel= sip.armorLevel;
					obj._TypelastName = "";
					obj._CloakName = sip.cloakName;
					return obj._PrefabName;
				}
			}
			return "";
		case 7:
		case 8:
			foreach(SItemPrefab sip in _SItemWeaponAttrPrefabList) {
				if(ID == sip.id) {
					obj._PrefabName = sip.prefabName;
					obj._TypeName	= sip.typeName;
					obj._QualityName= sip.qualityName;
					obj.info_ArmorLevel= sip.armorLevel;
					obj._TypelastName = "";
					obj._CloakName = sip.cloakName;
					return obj._PrefabName;
				}
			}
			return "";
		}
		return "";
	}
	
	//以前使用这个,现在使用新的重载函数//
//	public string SetPrefabID(ItemDropStruct obj,int type,int ID){
//		string fileName = null;
//		switch(type){
//			case 1:
//			case 3:
//			case 6:
//				fileName = "ArmorAttribute.prefab";
//				break;
//			case 2:	
//			case 5:
//				fileName = "AccessoriesAttribute.prefab";
//				break;
//			case 4:	
//				fileName = "ArmorAttribute.Prefab_cloak";
//				break;
//			case 7:
//			case 8:	
//				fileName = "WeaponAttribute.prefab";
//				break;
//			case 9:
//				obj._PrefabName = "MissionItem";
//				return "MissionItem";
//			case 10:
//				obj._PrefabName = "EleItem";
//				return "EleItem";
//			case 11:
//				obj._PrefabName = "GemItem";
//				return "GemItem";
//			case 12:
//				obj._PrefabName = "EncItem";
//				return "EncItem";
//			case 13:
//				obj._PrefabName = "KarmaItem";
//				return "KarmaItem";
//			case 14:
//				obj._PrefabName = "ConsumableItem";
//				return "ConsumableItem";
//			default:
//				break;
//		}
//		if(null != fileName){
//		string _fileName = LocalizeManage.Instance.GetLangPath(fileName);
//		TextAsset item = (TextAsset)Resources.Load(_fileName, typeof(TextAsset));
//				string[] itemRowsList = item.text.Split('\n');
//				for (int i = 3; i < itemRowsList.Length -1; ++i){		
//					string pp = itemRowsList[i];		
//					string[] vals = pp.Split(new char[] { '	', '	' });	
//					if(int.Parse(vals[0]) == ID){	
//						obj._PrefabName = vals[1];
//						int temp = obj._ItemID %10;
//						if(1 == type || 3 == type || 6 == type){	
//							obj._TypeName = vals[4];
//							obj._QualityName = "";
//							obj.info_ArmorLevel = vals[9];
//							if(1 == type){
//								obj._TypelastName = vals[5];
//							}else if(3 == type){
//								obj._TypelastName = vals[6];
//							}else if(6 == type){	
//								obj._TypelastName = vals[7];
//							}
//						}else{
//							obj._TypeName = "";
//							obj._TypelastName = "";
//							if(4 == type){
//								obj._QualityName = vals[4];	
//								obj._CloakName	 = vals[6];
//							}
//						}
//						return vals[1];
//					}
//				}
//		}
//		return null;
//	}
#endregion
	
#region BaseInfo
	public struct SItemBaseInfo {
		public int id;
		public string typeName;
		public string setName;
		public float  minDef;
		public float  itemVal;
		public string hand;
		public float  minAtc;
		public float  maxAtc;
		public string speed;
	}
	
	public List<SItemBaseInfo> 	_SItemBaseInfoArmorList   	  = new List<SItemBaseInfo>();	
	public List<SItemBaseInfo> 	_SItemBaseInfoAccessoryList   = new List<SItemBaseInfo>();	
	public List<SItemBaseInfo> 	_SItemBaseInfoWeaponList   	  = new List<SItemBaseInfo>();	
	
	public void InitBaseInfoList() {
		_SItemBaseInfoArmorList.Clear();
		_SItemBaseInfoAccessoryList.Clear();
		_SItemBaseInfoWeaponList.Clear();
		string 		_fileName = "";
		TextAsset 	item;
		_fileName = LocalizeManage.Instance.GetLangPath("ArmorAttribute.Armor");
		item = (TextAsset)Resources.Load(_fileName, typeof(TextAsset));
		string[] itemRowsList1 = item.text.Split('\n');
		for (int i = 3; i < itemRowsList1.Length - 1; ++i){
			SItemBaseInfo temp = new SItemBaseInfo();
			string pp = itemRowsList1[i];
			string[] vals = pp.Split(new char[] { '	', '	' });
			temp.id			= int.Parse(vals[0]);		
			temp.typeName 	= vals[1];		
			temp.setName 	= vals[1];
			temp.minDef 	= float.Parse(vals[3]);
			temp.itemVal	= float.Parse(vals[4]);
			temp.hand		= "";
			temp.minAtc		= 0;
			temp.maxAtc		= 0;
			temp.speed	 	= "";
			_SItemBaseInfoArmorList.Add(temp);
		}
		
		_fileName = LocalizeManage.Instance.GetLangPath("AccessoriesAttribute.Accessory");
		item = (TextAsset)Resources.Load(_fileName, typeof(TextAsset));
		string[] itemRowsList2 = item.text.Split('\n');
		for (int i = 3; i < itemRowsList2.Length - 1; ++i){
			SItemBaseInfo temp = new SItemBaseInfo();
			string pp = itemRowsList2[i];
			string[] vals = pp.Split(new char[] { '	', '	' });
			temp.id			= int.Parse(vals[0]);		
			temp.typeName 	= vals[1];		
			temp.setName 	= "";
			temp.minDef 	= int.Parse(vals[4]);
			temp.itemVal	= float.Parse(vals[3]);
			temp.hand		= "";
			temp.minAtc		= 0;
			temp.maxAtc		= 0;
			temp.speed	 	= "";
			_SItemBaseInfoAccessoryList.Add(temp);
		}
		
		_fileName = LocalizeManage.Instance.GetLangPath("WeaponAttribute.weapon");
		item = (TextAsset)Resources.Load(_fileName, typeof(TextAsset));
		string[] itemRowsList3 = item.text.Split('\n');
		for (int i = 3; i < itemRowsList3.Length - 1; ++i){
			SItemBaseInfo temp = new SItemBaseInfo();
			string pp = itemRowsList3[i];
			string[] vals = pp.Split(new char[] { '	', '	' });
			temp.id			= int.Parse(vals[0]);		
			temp.typeName 	= vals[1];		
			temp.setName 	= "";
			temp.minDef 	= 0;
			temp.itemVal	= float.Parse(vals[10]);
			temp.hand		= vals[2];
			temp.minAtc		= float.Parse(vals[4]);
			temp.maxAtc		= float.Parse(vals[5]);
			temp.speed	 	= vals[9];
			_SItemBaseInfoWeaponList.Add(temp);
		}
	}
	
	public void SetBaseInfo(ItemDropStruct obj,int type,int ID){
		switch(type) {
		case 1:
		case 3:	
		case 4:	
		case 6:	
			foreach(SItemBaseInfo sip in _SItemBaseInfoArmorList) {
				if(ID == sip.id) {	
					obj.info_TypeName = sip.typeName;
					obj.info_Set      = sip.setName;
					obj.info_MinDef   = sip.minDef;
					obj._ItemVal      = sip.itemVal;
					obj.info_Speed	  = sip.speed;
					return;
				}
			}
			return;
		case 2:
		case 5:	
			foreach(SItemBaseInfo sip in _SItemBaseInfoAccessoryList) {
				if(ID == sip.id) {	
					obj.info_TypeName = sip.typeName;
					obj._ItemVal      = sip.itemVal;
					obj.info_Speed	  = sip.speed;
					obj.info_MinDef   = sip.minDef;
					return;
				}
			}
			return;
		case 7:
		case 8:	
			foreach(SItemBaseInfo sip in _SItemBaseInfoWeaponList) {
				if(ID == sip.id) {	
					obj.info_TypeName = sip.typeName;
					obj.info_hand 	  = sip.hand;
					obj.info_MinAtc   = sip.minAtc;
					obj.info_MaxAtc   = sip.maxAtc;
					obj.info_Speed    = sip.speed;
					obj._ItemVal      = sip.itemVal;
					return;
				}
			}
			return;
		}
		return;
	}

	//以前使用这个,现在使用新的重载函数//
//	public void SetBaseInfo(ItemDropStruct obj,int type,int ID){
//		string fileName = null;
//		switch(type){
//			case 1:
//			case 3:
//			case 6:
//				fileName = "ArmorAttribute.Armor";
//				break;
//			case 2:	
//			case 5:
//				fileName = "AccessoriesAttribute.Accessory";
//				break;
//			case 4:	
//				fileName = "ArmorAttribute.Armor";
//				break;
//			case 7:
//			case 8:
//				fileName = "WeaponAttribute.weapon";
//				break;
//			case 9:
//				return;
//			case 10:
//				return;
//			case 11:
//				return;
//			case 12:
//				return;
//			case 13:
//				return;
//			default:
//				break;
//		}
//		if(null != fileName){
//		string _fileName = LocalizeManage.Instance.GetLangPath(fileName);
//		TextAsset item = (TextAsset)Resources.Load(_fileName, typeof(TextAsset));
//			string[] itemRowsList = item.text.Split('\n');
//			for (int i = 3; i < itemRowsList.Length; ++i){	
//				string pp = itemRowsList[i];
//				if(pp.Contains(ID.ToString())) {	
//				   	string[] vals = pp.Split(new char[] { '	', '	' });	
//					if(1 == type|| 3 == type||4 == type||6 == type){	
//						obj.info_TypeName = vals[1];
//						obj.info_Set      = vals[1];
//						obj.info_MinDef   = float.Parse(vals[3]);
//						obj._ItemVal      = float.Parse(vals[4]);
//					}else if(2 == type|| 5 == type){
//						obj.info_TypeName = vals[1];
//						obj._ItemVal      = float.Parse(vals[3]);
//					}else if(7 == type|| 8 == type){
//						obj.info_TypeName = vals[1];
//						obj.info_hand 	  = vals[2];
//						obj.info_MinAtc   = float.Parse(vals[4]);
//						obj.info_MaxAtc   = float.Parse(vals[5]);
//						obj.info_Speed    = vals[9];
//						obj._ItemVal      = float.Parse(vals[10]);
//					}
//				}
//			}
//		}
//	}	
#endregion

#region SetGemInfo
	public struct SItemGemInfo {
		public int    id;
		public string gemName;
		public string gemeNameLv;
		public float  gemEffectVal;
		public float  gemVal;
		public string gemDesc1;
		public string gemDesc2;
		public int    gemIconIdx;
	}
	
	public List<SItemGemInfo> 	_SItemGemInfoList   	  = new List<SItemGemInfo>();	
	
	public void InitGemInfoList() {
		_SItemGemInfoList.Clear();
		string 		_fileName = "";
		TextAsset 	item;
		_fileName = LocalizeManage.Instance.GetLangPath("WeaponAttribute.Gem");
		item = (TextAsset)Resources.Load(_fileName, typeof(TextAsset));
		string[] itemRowsList1 = item.text.Split('\n');
		for (int i = 3; i < itemRowsList1.Length - 1; ++i){
			SItemGemInfo temp = new SItemGemInfo();
			string pp = itemRowsList1[i];
			string[] vals = pp.Split(new char[] { '	', '	' });
			temp.id				= int.Parse(vals[0]);		
			temp.gemName 		= vals[12];		
			temp.gemeNameLv 	= vals[1];
			temp.gemEffectVal 	= float.Parse(vals[6]);
			temp.gemVal			= float.Parse(vals[6]);
			temp.gemDesc1		= vals[9];
			temp.gemDesc2		= vals[10];
			temp.gemIconIdx		= int.Parse(vals[11]);
			_SItemGemInfoList.Add(temp);
		}
	}
	
	public void SetGemInfo(ItemDropStruct obj,int type,int ID){
		foreach(SItemGemInfo sip in _SItemGemInfoList) {
			if(ID == sip.id) {	
				obj.info_GemName  	= sip.gemName;
				obj.info_GemeNameLv = sip.gemeNameLv;
				obj._GemEffectVal   = sip.gemEffectVal;
				obj.info_gemVal     = sip.gemVal;
				obj.info_GemDesc1	= sip.gemDesc1;
				obj.info_GemDesc2	= sip.gemDesc2;
				obj.info_gemIconIdx	= sip.gemIconIdx;
				return;
			}
		}
	}
	
	//以前使用这个,现在使用新的重载函数//
//	public void SetGemInfo(ItemDropStruct obj,int type,int ID){
//		string fileName = null;
//		if(10 == type || 11 == type || 12 == type ||  13 == type ){
//			return;
//		}
//		if(0 == ID){
//			obj.info_GemName  = "";
//			obj.info_gemVal   = 0;
//			obj.info_GemDesc1 = "";
//			obj.info_GemDesc2 = "";
//			return;
//		}
//		fileName = "WeaponAttribute.Gem";
//		if(null != fileName){	
//		string _fileName = LocalizeManage.Instance.GetLangPath(fileName);
//		TextAsset item = (TextAsset)Resources.Load(_fileName, typeof(TextAsset));
//			string[] itemRowsList = item.text.Split('\n');
//			for (int i = 3; i < itemRowsList.Length - 1; ++i){		
//				string pp = itemRowsList[i];
//				string[] vals = pp.Split(new char[] { '	', '	' });	
//				if(int.Parse(vals[0]) == ID){
//						obj.info_GemName  	= vals[12];
//						obj.info_GemeNameLv	= vals[1];
//						obj._GemEffectVal  	= float.Parse(vals[6]);
//						obj.info_gemVal   	= float.Parse(vals[6]);
//						obj.info_GemDesc1 	= vals[9];
//						obj.info_GemDesc2 	= vals[10];
//						obj.info_gemIconIdx = int.Parse(vals[11]);
//					return;
//				}
//			}
//		}
//	}
#endregion
	
#region SetEncInfo	
	public struct SItemEncInfo {
		public int     id;
		public string  encNameLv;
		public string  encName;
		public string  encDesc1;
		public string  encDesc2;
		public float   encVal;
		public int     encIconIdx;
	}
	
	public List<SItemEncInfo> 	_SItemEncInfoArmorAttrList   	  		= new List<SItemEncInfo>();	
	public List<SItemEncInfo> 	_SItemEncInfoAccessoriesAttrAttrList   	= new List<SItemEncInfo>();	
	public List<SItemEncInfo> 	_SItemEncInfoWeaponAttrAttrList   	  	= new List<SItemEncInfo>();	
	
	public void InitEncInfoList() {
		_SItemEncInfoArmorAttrList.Clear();
		_SItemEncInfoAccessoriesAttrAttrList.Clear();
		_SItemEncInfoWeaponAttrAttrList.Clear();
		string 		_fileName = "";
		TextAsset 	item;
		_fileName = LocalizeManage.Instance.GetLangPath("ArmorAttribute.Enchant");
		item = (TextAsset)Resources.Load(_fileName, typeof(TextAsset));
		string[] itemRowsList1 = item.text.Split('\n');
		for (int i = 3; i < itemRowsList1.Length - 1; ++i){
			SItemEncInfo temp = new SItemEncInfo();
			string pp = itemRowsList1[i];
			string[] vals = pp.Split(new char[] { '	', '	' });
			temp.id			= int.Parse(vals[0]);		
			temp.encNameLv	= vals[1];
			temp.encName  	= vals[10];
			temp.encDesc1 	= vals[6];
			temp.encDesc2 	= vals[7];
			temp.encVal   	= float.Parse(vals[5]);
			temp.encIconIdx  = int.Parse(vals[9]);
			_SItemEncInfoArmorAttrList.Add(temp);
		}
		
		_fileName = LocalizeManage.Instance.GetLangPath("AccessoriesAttribute.Enchant");
		item = (TextAsset)Resources.Load(_fileName, typeof(TextAsset));
		string[] itemRowsList2 = item.text.Split('\n');
		for (int i = 3; i < itemRowsList2.Length - 1; ++i){
			SItemEncInfo temp = new SItemEncInfo();
			string pp = itemRowsList2[i];
			string[] vals = pp.Split(new char[] { '	', '	' });
			temp.id			 = int.Parse(vals[0]);		
			temp.encNameLv	 = vals[1];
			temp.encName  	 = vals[10];
			temp.encDesc1 	 = vals[6];
			temp.encDesc2 	 = vals[7];
			temp.encVal   	 = float.Parse(vals[5]);
			temp.encIconIdx  = int.Parse(vals[9]);
			_SItemEncInfoAccessoriesAttrAttrList.Add(temp);
		}
		
		_fileName = LocalizeManage.Instance.GetLangPath("WeaponAttribute.Enchant");
		item = (TextAsset)Resources.Load(_fileName, typeof(TextAsset));
		string[] itemRowsList3 = item.text.Split('\n');
		for (int i = 3; i < itemRowsList3.Length - 1; ++i){
			SItemEncInfo temp = new SItemEncInfo();
			string pp = itemRowsList3[i];
			string[] vals = pp.Split(new char[] { '	', '	' });
			temp.id			 = int.Parse(vals[0]);		
			temp.encNameLv	 = vals[1];
			temp.encName  	 = vals[10];
			temp.encDesc1 	 = vals[6];
			temp.encDesc2 	 = vals[7];
			temp.encVal   	 = float.Parse(vals[5]);
			temp.encIconIdx  = int.Parse(vals[9]);
			_SItemEncInfoWeaponAttrAttrList.Add(temp);
		}
	}
	
	public void SetEncInfo(ItemDropStruct obj,int type,int ID){
		switch(type) {
		case 1:
		case 3:	
		case 4:	
		case 6:	
			foreach(SItemEncInfo sip in _SItemEncInfoArmorAttrList) {
				if(ID == sip.id) {	
					obj.info_EncNameLv	= sip.encNameLv;
					obj.info_EncName  	= sip.encName;
					obj.info_EncDesc1 	= sip.encDesc1;
					obj.info_EncDesc2	= sip.encDesc2;
					obj.info_encVal   	= sip.encVal;
					obj.info_encIconIdx = sip.encIconIdx;
					return;
				}
			}
			return;
		case 2:	
		case 5:
			foreach(SItemEncInfo sip in _SItemEncInfoAccessoriesAttrAttrList) {
				if(ID == sip.id) {	
					obj.info_EncNameLv	= sip.encNameLv;
					obj.info_EncName  	= sip.encName;
					obj.info_EncDesc1 	= sip.encDesc1;
					obj.info_EncDesc2	= sip.encDesc2;
					obj.info_encVal   	= sip.encVal;
					obj.info_encIconIdx = sip.encIconIdx;
					return;
				}
			}
			return;
		case 7:
		case 8:
			foreach(SItemEncInfo sip in _SItemEncInfoWeaponAttrAttrList) {
				if(ID == sip.id) {	
					obj.info_EncNameLv	= sip.encNameLv;
					obj.info_EncName  	= sip.encName;
					obj.info_EncDesc1 	= sip.encDesc1;
					obj.info_EncDesc2	= sip.encDesc2;
					obj.info_encVal   	= sip.encVal;
					obj.info_encIconIdx = sip.encIconIdx;
					return;
				}
			}
			return;
		}
	}
	
	//以前使用这个,现在使用新的重载函数//
//	public void SetEncInfo(ItemDropStruct obj,int type,int ID){
//		string fileName = null;
//		if(0 == ID||-1 == ID){
//			obj.info_EncName  = "";
//			obj.info_encVal   = 0;
//			obj.info_EncDesc1 = "";
//			obj.info_EncDesc2 = "";
//			return;
//		}
//		switch(type){	
//			case 1:
//			case 3:
//			case 6:	
//				fileName = "ArmorAttribute.Enchant";
//				break;
//			case 2:	
//			case 5:	
//				fileName = "AccessoriesAttribute.Enchant";
//				break;
//			case 4:		
//				fileName = "ArmorAttribute.Enchant";
//				break;
//			case 7:
//			case 8:	
//				fileName = "WeaponAttribute.Enchant";
//				break;
//			case 9:
//				return;
//			case 10:
//				return;
//			case 11:
//				return;
//			case 12:
//				return;
//			case 13:
//				return;
//			default:
//				break;
//		}
//		if(null != fileName){
//		string _fileName = LocalizeManage.Instance.GetLangPath(fileName);
//		TextAsset item = (TextAsset)Resources.Load(_fileName, typeof(TextAsset));
//			string[] itemRowsList = item.text.Split('\n');
//			for (int i = 3; i < itemRowsList.Length; ++i){
//				string pp = itemRowsList[i];
//				string[] vals = pp.Split(new char[] { '	', '	' });	
//				if(int.Parse(vals[0]) == ID){	
//					if(1 == type|| 3 == type||4 == type||6 == type){	
//						obj.info_EncNameLv= vals[1];
//						obj.info_EncName  = vals[10];
//						obj.info_EncDesc1 = vals[6];
//						obj.info_EncDesc2 = vals[7];
//						obj.info_encVal   = float.Parse(vals[5]);
//						obj.info_encIconIdx = int.Parse(vals[9]);
//					}else if(2 == type|| 5 == type){	
//						obj.info_EncNameLv= vals[1];
//						obj.info_EncName  = vals[10];
//						obj.info_EncDesc1 = vals[6];
//						obj.info_EncDesc2 = vals[7];
//						obj.info_encVal   = float.Parse(vals[5]);
//						obj.info_encIconIdx = int.Parse(vals[9]);
//					}else if(7 == type|| 8 == type){
//						obj.info_EncNameLv= vals[1];
//						obj.info_EncName  = vals[10];
//						obj.info_EncDesc1 = vals[6];
//						obj.info_EncDesc2 = vals[7];
//						obj.info_encVal   = float.Parse(vals[5]);
//						obj.info_encIconIdx = int.Parse(vals[9]);
//					}
//					return;
//				}	
//			}
//		}
//	}
#endregion
	
#region SetEleInfo
	public struct SItemEleInfo {
		public int     id;
		public string  eleNameLv;
		public string  eleName;
		public string  eleDesc1;
		public string  eleDesc2;
		public float   eleVal;
		public int     eleIconIdx;
		public string   elePercentVal;
	}
	
	public List<SItemEleInfo> 	_SItemEleInfoArmorAttrList   	  		= new List<SItemEleInfo>();	
	public List<SItemEleInfo> 	_SItemEleInfoAccessoriesAttrAttrList   	= new List<SItemEleInfo>();	
	public List<SItemEleInfo> 	_SItemEleInfoWeaponAttrAttrList   	  	= new List<SItemEleInfo>();	
	
	public void InitEleInfoList() {
		_SItemEleInfoArmorAttrList.Clear();
		_SItemEleInfoAccessoriesAttrAttrList.Clear();
		_SItemEleInfoWeaponAttrAttrList.Clear();
		string 		_fileName = "";
		TextAsset 	item;
		_fileName = LocalizeManage.Instance.GetLangPath("ArmorAttribute.Element");
		item = (TextAsset)Resources.Load(_fileName, typeof(TextAsset));
		string[] itemRowsList1 = item.text.Split('\n');
		for (int i = 3; i < itemRowsList1.Length - 1; ++i){
			SItemEleInfo temp = new SItemEleInfo();
			string pp = itemRowsList1[i];
			string[] vals = pp.Split(new char[] { '	', '	' });
			temp.id			= int.Parse(vals[0]);		
			temp.eleNameLv	= vals[1];
			temp.eleName  	= vals[12];
			temp.eleDesc1 	= vals[4];
			temp.eleDesc2 	= vals[5];
			temp.elePercentVal	= "";
			temp.eleVal   	= float.Parse(vals[9]);
			temp.eleIconIdx = int.Parse(vals[11]);
			_SItemEleInfoArmorAttrList.Add(temp);
		}
		
		_fileName = LocalizeManage.Instance.GetLangPath("AccessoriesAttribute.Element");
		item = (TextAsset)Resources.Load(_fileName, typeof(TextAsset));
		string[] itemRowsList2 = item.text.Split('\n');
		for (int i = 3; i < itemRowsList2.Length - 1; ++i){
			SItemEleInfo temp = new SItemEleInfo();
			string pp = itemRowsList2[i];
			string[] vals = pp.Split(new char[] { '	', '	' });
			temp.id			= int.Parse(vals[0]);		
			temp.eleNameLv	= vals[1];
			temp.eleName  	= vals[12];
			temp.eleDesc1 	= vals[8];
			temp.eleDesc2 	= vals[9];
			temp.elePercentVal	= "";
			temp.eleVal   	= float.Parse(vals[7]);
			temp.eleIconIdx = int.Parse(vals[11]);
			_SItemEleInfoAccessoriesAttrAttrList.Add(temp);
		}
		
		_fileName = LocalizeManage.Instance.GetLangPath("WeaponAttribute.Element");
		item = (TextAsset)Resources.Load(_fileName, typeof(TextAsset));
		string[] itemRowsList3 = item.text.Split('\n');
		for (int i = 3; i < itemRowsList3.Length - 1; ++i){
			SItemEleInfo temp = new SItemEleInfo();
			string pp = itemRowsList3[i];
			string[] vals = pp.Split(new char[] { '	', '	' });
			temp.id			= int.Parse(vals[0]);		
			temp.eleNameLv	= vals[1];
			temp.eleName  	= vals[15];
			temp.elePercentVal	= vals[5];
			temp.eleDesc1 	= vals[9];
			temp.eleDesc2 	= vals[10];
			temp.eleVal   	= float.Parse(vals[8]);
			temp.eleIconIdx = int.Parse(vals[14]);
			_SItemEleInfoWeaponAttrAttrList.Add(temp);
		}
	}
	
	public void SetEleInfo(ItemDropStruct obj,int type,int ID){
		switch(type) {
		case 1:
		case 3:	
		case 4:	
		case 6:	
			foreach(SItemEleInfo sip in _SItemEleInfoArmorAttrList) {
				if(ID == sip.id) {	
						obj.info_EleNameLv= sip.eleNameLv;
						obj.info_EleName  = sip.eleName;
						obj.info_EleDesc1 = sip.eleDesc1;
						obj.info_EleDesc2 = sip.eleDesc2;
						obj.info_eleVal   = sip.eleVal;
						obj.info_eleIconIdx = sip.eleIconIdx;	
					return;
				}
			}
			return;
		case 2:	
		case 5:	
			foreach(SItemEleInfo sip in _SItemEleInfoAccessoriesAttrAttrList) {
				if(ID == sip.id) {	
						obj.info_EleNameLv= sip.eleNameLv;
						obj.info_EleName  = sip.eleName;
						obj.info_EleDesc1 = sip.eleDesc1;
						obj.info_EleDesc2 = sip.eleDesc2;
						obj.info_eleVal   = sip.eleVal;
						obj.info_eleIconIdx = sip.eleIconIdx;	
					return;
				}
			}
			return;
		case 7:	
		case 8:	
			foreach(SItemEleInfo sip in _SItemEleInfoWeaponAttrAttrList) {
				if(ID == sip.id) {	
						obj.info_EleNameLv= sip.eleNameLv;
						obj.info_EleName  = sip.eleName;
						obj.info_elePercentVal = sip.elePercentVal;
						obj.info_EleDesc1 = sip.eleDesc1;
						obj.info_EleDesc2 = sip.eleDesc2;
						obj.info_eleVal   = sip.eleVal;
						obj.info_eleIconIdx = sip.eleIconIdx;	
					return;
				}
			}
			return;
		}
	}
	
	//以前使用这个,现在使用新的重载函数//
//	public void SetEleInfo(ItemDropStruct obj,int type,int ID){
//		string fileName = null;
//		if(0 == ID){	
//			obj.info_EleName  = "";
//			obj.info_eleVal   = 0;
//			obj.info_EleDesc1 = "";
//			obj.info_EleDesc2 = "";
//			return;
//		}
//		switch(type){
//			case 1:
//			case 3:
//			case 6:
//				fileName = "ArmorAttribute.Element";
//				break;
//			case 2:	
//			case 5:	
//				fileName = "AccessoriesAttribute.Element";
//				break;
//			case 4:		
//				fileName = "ArmorAttribute.Element";
//				break;
//			case 7:
//			case 8:
//				fileName = "WeaponAttribute.Element";
//				break;
//			case 9:
//				return;
//			case 10:
//				return;
//			case 11:
//				return;
//			case 12:
//				return;
//			case 13:
//				return;
//			default:
//				break;
//		}
//		if(null != fileName){
//		string _fileName = LocalizeManage.Instance.GetLangPath(fileName);
//		TextAsset item = (TextAsset)Resources.Load(_fileName, typeof(TextAsset));
//			string[] itemRowsList = item.text.Split('\n');
//			for (int i = 3; i < itemRowsList.Length; ++i){		
//				string pp = itemRowsList[i];
//				string[] vals = pp.Split(new char[] { '	', '	' });				
//				if(int.Parse(vals[0]) == ID){
//					if(1 == type|| 3 == type||4 == type||6 == type){	
//						obj.info_EleNameLv= vals[1];
//						obj.info_EleName  = vals[12];
//						obj.info_EleDesc1 = vals[4];
//						obj.info_EleDesc2 = vals[5];
//						obj.info_eleVal   = float.Parse(vals[9]);
//						obj.info_eleIconIdx = int.Parse(vals[11]);	
//					}else if(2 == type|| 5 == type){	
//						obj.info_EleNameLv= vals[1];
//						obj.info_EleName  = vals[12];
//						obj.info_EleDesc1 = vals[8];
//						obj.info_EleDesc2 = vals[9];
//						obj.info_eleVal   = float.Parse(vals[7]);
//						obj.info_eleIconIdx = int.Parse(vals[11]);
//					}else if(7 == type|| 8 == type){
//						obj.info_EleNameLv  = vals[1];
//						obj.info_EleName  = vals[15];
//						obj.info_elePercentVal = vals[5];
//						obj.info_EleDesc1 = vals[9];
//						obj.info_EleDesc2 = vals[10];
//						obj.info_eleVal   = float.Parse(vals[8]);
//						obj.info_eleIconIdx = int.Parse(vals[14]);
//					}
//					return;
//				}
//			}
//		}
//	}
#endregion
	
#region SetModifierInfo
	
	public struct SItemModifierInfo {
		public int    level;
		public float  modifier;
	}
	
	public List<SItemModifierInfo> 	_SItemModifierInfoArmorAttrList   	  		= new List<SItemModifierInfo>();	
	public List<SItemModifierInfo> 	_SItemModifierInfoAccessoriesAttrAttrList   = new List<SItemModifierInfo>();	
	public List<SItemModifierInfo> 	_SItemModifierInfoWeaponAttrAttrList   	  	= new List<SItemModifierInfo>();	
	
	public void InitModifierInfoList() {
		_SItemModifierInfoArmorAttrList.Clear();
		_SItemModifierInfoAccessoriesAttrAttrList.Clear();
		_SItemModifierInfoWeaponAttrAttrList.Clear();
		string 		_fileName = "";
		TextAsset 	item;
		_fileName = LocalizeManage.Instance.GetLangPath("ArmorAttribute.Stat");
		item = (TextAsset)Resources.Load(_fileName, typeof(TextAsset));
		string[] itemRowsList1 = item.text.Split('\n');
		for (int i = 3; i < itemRowsList1.Length - 1; ++i){
			SItemModifierInfo temp = new SItemModifierInfo();
			string pp = itemRowsList1[i];
			string[] vals = pp.Split(new char[] { '	', '	' });
			temp.level				= int.Parse(vals[0]);
			temp.modifier			= float.Parse(vals[1]);		
			_SItemModifierInfoArmorAttrList.Add(temp);
		}
		
		_fileName = LocalizeManage.Instance.GetLangPath("ArmorAttribute.Stat");
		item = (TextAsset)Resources.Load(_fileName, typeof(TextAsset));
		string[] itemRowsList2 = item.text.Split('\n');
		for (int i = 3; i < itemRowsList2.Length - 1; ++i){
			SItemModifierInfo temp = new SItemModifierInfo();
			string pp = itemRowsList2[i];
			string[] vals = pp.Split(new char[] { '	', '	' });
			temp.level				= int.Parse(vals[0]);
			temp.modifier			= float.Parse(vals[1]);		
			_SItemModifierInfoAccessoriesAttrAttrList.Add(temp);
		}
		
		_fileName = LocalizeManage.Instance.GetLangPath("ArmorAttribute.Stat");
		item = (TextAsset)Resources.Load(_fileName, typeof(TextAsset));
		string[] itemRowsList3 = item.text.Split('\n');
		for (int i = 3; i < itemRowsList3.Length - 1; ++i){
			SItemModifierInfo temp = new SItemModifierInfo();
			string pp = itemRowsList3[i];
			string[] vals = pp.Split(new char[] { '	', '	' });
			temp.level				= int.Parse(vals[0]);
			temp.modifier			= float.Parse(vals[1]);		
			_SItemModifierInfoWeaponAttrAttrList.Add(temp);
		}
	}
	
	public void SetModifierInfo(ItemDropStruct obj,int type,int level){
		switch(type) {
		case 1:
		case 3:	
		case 4:	
		case 6:	
			foreach(SItemModifierInfo sip in _SItemModifierInfoArmorAttrList) {
				if(level == sip.level) {	
						obj.info_Modifier= sip.modifier;
					return;
				}
			}
			return;
		case 2:	
		case 5:	
			foreach(SItemModifierInfo sip in _SItemModifierInfoAccessoriesAttrAttrList) {
				if(level == sip.level) {	
						obj.info_Modifier= sip.modifier;
					return;
				}
			}
			return;
		case 7:	
		case 8:	
			foreach(SItemModifierInfo sip in _SItemModifierInfoWeaponAttrAttrList) {
				if(level == sip.level) {	
						obj.info_Modifier= sip.modifier;
					return;
				}
			}
			return;	
		}
	}
	
	//以前使用这个,现在使用新的重载函数//
//	public void SetModifierInfo(ItemDropStruct obj,int type,int level){
//		string fileName = null;
//		switch(type){	
//			case 1:
//			case 3:
//			case 6:
//				fileName = "ArmorAttribute.Stat";
//				break;
//			case 2:	
//			case 5:	
//				fileName = "AccessoriesAttribute.Stat";
//				break;	
//			case 4:	
//				fileName = "ArmorAttribute.Stat";
//				break;
//			case 7:
//			case 8:
//				fileName = "WeaponAttribute.Stat";
//				break;
//			case 9:
//				return;
//			case 10:
//				return;
//			case 11:
//				return;
//			case 12:
//				return;
//			case 13:
//				return;
//			default:
//				break;	
//		}
//		if(null != fileName){
//		string _fileName = LocalizeManage.Instance.GetLangPath(fileName);
//		TextAsset item = (TextAsset)Resources.Load(_fileName, typeof(TextAsset));
//			string[] itemRowsList = item.text.Split('\n');
//			for (int i = 3; i < itemRowsList.Length; ++i){	
//				string pp = itemRowsList[i];
//				if(i == (level + 3 - 1)){
//				   	string[] vals = pp.Split(new char[] { '	', '	' });	
//					obj.info_Modifier  = float.Parse(vals[1]);
//					return;
//				}
//			}
//		}
//	}
#endregion	
	 
	//now no need
	public int InputToList(ItemDropStruct temp){
		_StructList.Add(temp);
		return (_StructList.Count - 1);
	}
	
}
