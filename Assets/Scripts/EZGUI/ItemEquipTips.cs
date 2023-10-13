using UnityEngine;
using System.Collections;

public class ItemEquipTips : MonoBehaviour {
	//Instance
	public static ItemEquipTips Instance = null;
	
	public Transform  RootObj1;
	public Transform  RootObj2;
	public Transform  RootObj3;
	public Transform  EquipObjA;
	public Transform  EquipObjB;
	
	private int   	  EleCountA = 0;
	private int   	  EleCountB = 0;
	
	public UIButton   BG0A;
	public UIButton   BG1A;
	public UIButton   BG2A;
	public UIButton   BG3A;
	public UIButton   BG4A;
	public UIButton   BG5A;
	
	public UIButton   BG0B;
	public UIButton   BG1B;
	public UIButton   BG2B;
	public UIButton   BG3B;
	public UIButton   BG4B;
	public UIButton   BG5B;
	
	public SpriteText NameA;
	public SpriteText TypeA;
	public SpriteText LevelA;
	public UIButton	  ADIconA;
	public SpriteText ADNameA;
	public SpriteText ADA;
	public SpriteText SpeedNameA;
	public SpriteText SpeedA;
	public SpriteText EleNameA;
	public SpriteText EncNameA;
	public SpriteText GemNameA;
	public SpriteText EleDescritionA;
	public SpriteText EncDescritionA;
	public SpriteText GemDescritionA;
	public UIButton   EleIconA;
	public UIButton   EncIconA;
	public UIButton   GemIconA;
	public SpriteText MoneyA;
	
	public SpriteText NameB;
	public SpriteText TypeB;
	public SpriteText LevelB;
	public UIButton	  ADIconB;
	public SpriteText ADNameB;
	public SpriteText ADB;
	public SpriteText SpeedNameB;
	public SpriteText SpeedB;
	public SpriteText EleNameB;
	public SpriteText EncNameB;
	public SpriteText GemNameB;
	public SpriteText EleDescritionB;
	public SpriteText EncDescritionB;
	public SpriteText GemDescritionB;
	public UIButton   EleIconB;
	public UIButton   EncIconB;
	public UIButton   GemIconB;
	public SpriteText MoneyB;
	
	public Transform  Pos1ObjA;
	public Transform  Pos2ObjA;
	public Transform  Pos3ObjA;
	public Transform  Pos4ObjA;
	
	public Transform  Pos1ObjB;
	public Transform  Pos2ObjB;
	public Transform  Pos3ObjB;
	public Transform  Pos4ObjB;
	


	void Awake(){
		Instance = this;
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void DismissCompareTips(){
		EquipObjA.transform.position = new Vector3(999f,999f,999f);
		EquipObjB.transform.position = new Vector3(999f,999f,999f);
	}
	
	public void ShowCompareTips(ItemDropStruct infoObj,bool isEquip){
		DismissCompareTips();
		if(isEquip) {
			return;
		}
		
		//进入合成界面和卖物品不显示装备tips//
		if(_UI_CS_ScreenCtrl.Instance.IsScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_CRAFTING)) {
			return;
		}
		//进入残酷不显示装备tips//
		if(_UI_CS_ScreenCtrl.Instance.IsScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_STASH)) {
			return;
		}
		
		if(null==infoObj){
			return;
		}
		switch(infoObj._TypeID){
		case 1:
			if(null != Inventory.Instance.equipmentArray[0] && !Inventory.Instance.equipmentArray[0].m_IsEmpty){
				UpdateToolsTipInfo(Inventory.Instance.equipmentArray[0].ItemStruct,1,_ItemTips.Instance.GetItemValue(ItemValueType.SALE,
					Inventory.Instance.equipmentArray[0].ItemStruct.info_Level,
					Inventory.Instance.equipmentArray[0].ItemStruct.info_eleVal,
					Inventory.Instance.equipmentArray[0].ItemStruct.info_encVal,
					Inventory.Instance.equipmentArray[0].ItemStruct._GemEffectVal,
					Inventory.Instance.equipmentArray[0].ItemStruct._ItemVal));
			}
			break;
		case 2:
			if(null != Inventory.Instance.equipmentArray[1] && !Inventory.Instance.equipmentArray[1].m_IsEmpty){
				UpdateToolsTipInfo(Inventory.Instance.equipmentArray[1].ItemStruct,1,_ItemTips.Instance.GetItemValue(ItemValueType.SALE,
					Inventory.Instance.equipmentArray[1].ItemStruct.info_Level,
					Inventory.Instance.equipmentArray[1].ItemStruct.info_eleVal,
					Inventory.Instance.equipmentArray[1].ItemStruct.info_encVal,
					Inventory.Instance.equipmentArray[1].ItemStruct._GemEffectVal,
					Inventory.Instance.equipmentArray[1].ItemStruct._ItemVal));
			}
			break;
		case 3:
			if(null != Inventory.Instance.equipmentArray[2] && !Inventory.Instance.equipmentArray[2].m_IsEmpty){
				UpdateToolsTipInfo(Inventory.Instance.equipmentArray[2].ItemStruct,1,_ItemTips.Instance.GetItemValue(ItemValueType.SALE,
					Inventory.Instance.equipmentArray[2].ItemStruct.info_Level,
					Inventory.Instance.equipmentArray[2].ItemStruct.info_eleVal,
					Inventory.Instance.equipmentArray[2].ItemStruct.info_encVal,
					Inventory.Instance.equipmentArray[2].ItemStruct._GemEffectVal,
					Inventory.Instance.equipmentArray[2].ItemStruct._ItemVal));
			}
			break;
		case 4:
			if(null != Inventory.Instance.equipmentArray[3] && !Inventory.Instance.equipmentArray[3].m_IsEmpty){
				UpdateToolsTipInfo(Inventory.Instance.equipmentArray[3].ItemStruct,1,_ItemTips.Instance.GetItemValue(ItemValueType.SALE,
					Inventory.Instance.equipmentArray[3].ItemStruct.info_Level,
					Inventory.Instance.equipmentArray[3].ItemStruct.info_eleVal,
					Inventory.Instance.equipmentArray[3].ItemStruct.info_encVal,
					Inventory.Instance.equipmentArray[3].ItemStruct._GemEffectVal,
					Inventory.Instance.equipmentArray[3].ItemStruct._ItemVal));
			}
			break;
		case 5:
			if(null != Inventory.Instance.equipmentArray[4] && !Inventory.Instance.equipmentArray[4].m_IsEmpty){
				UpdateToolsTipInfo(Inventory.Instance.equipmentArray[4].ItemStruct,1,_ItemTips.Instance.GetItemValue(ItemValueType.SALE,
					Inventory.Instance.equipmentArray[4].ItemStruct.info_Level,
					Inventory.Instance.equipmentArray[4].ItemStruct.info_eleVal,
					Inventory.Instance.equipmentArray[4].ItemStruct.info_encVal,
					Inventory.Instance.equipmentArray[4].ItemStruct._GemEffectVal,
					Inventory.Instance.equipmentArray[4].ItemStruct._ItemVal));
			}
			break;
		case 6:
			if(null != Inventory.Instance.equipmentArray[8] && !Inventory.Instance.equipmentArray[8].m_IsEmpty){
				UpdateToolsTipInfo(Inventory.Instance.equipmentArray[8].ItemStruct,1,_ItemTips.Instance.GetItemValue(ItemValueType.SALE,
					Inventory.Instance.equipmentArray[8].ItemStruct.info_Level,
					Inventory.Instance.equipmentArray[8].ItemStruct.info_eleVal,
					Inventory.Instance.equipmentArray[8].ItemStruct.info_encVal,
					Inventory.Instance.equipmentArray[8].ItemStruct._GemEffectVal,
					Inventory.Instance.equipmentArray[8].ItemStruct._ItemVal));
			}
			break;
		case 7:
		case 8:
			if(null != Inventory.Instance.equipmentArray[7] && !Inventory.Instance.equipmentArray[7].m_IsEmpty){
				if(null != Inventory.Instance.equipmentArray[6] && !Inventory.Instance.equipmentArray[6].m_IsEmpty){
					UpdateToolsTipInfo(Inventory.Instance.equipmentArray[6].ItemStruct,2,_ItemTips.Instance.GetItemValue(ItemValueType.SALE,
						Inventory.Instance.equipmentArray[6].ItemStruct.info_Level,
						Inventory.Instance.equipmentArray[6].ItemStruct.info_eleVal,
						Inventory.Instance.equipmentArray[6].ItemStruct.info_encVal,
						Inventory.Instance.equipmentArray[6].ItemStruct._GemEffectVal,
						Inventory.Instance.equipmentArray[6].ItemStruct._ItemVal));
					UpdateToolsTipInfo(Inventory.Instance.equipmentArray[7].ItemStruct,3,_ItemTips.Instance.GetItemValue(ItemValueType.SALE,
						Inventory.Instance.equipmentArray[7].ItemStruct.info_Level,
						Inventory.Instance.equipmentArray[7].ItemStruct.info_eleVal,
						Inventory.Instance.equipmentArray[7].ItemStruct.info_encVal,
						Inventory.Instance.equipmentArray[7].ItemStruct._GemEffectVal,
						Inventory.Instance.equipmentArray[7].ItemStruct._ItemVal));
				}else{
					UpdateToolsTipInfo(Inventory.Instance.equipmentArray[7].ItemStruct,1,_ItemTips.Instance.GetItemValue(ItemValueType.SALE,
						Inventory.Instance.equipmentArray[7].ItemStruct.info_Level,
						Inventory.Instance.equipmentArray[7].ItemStruct.info_eleVal,
						Inventory.Instance.equipmentArray[7].ItemStruct.info_encVal,
						Inventory.Instance.equipmentArray[7].ItemStruct._GemEffectVal,
						Inventory.Instance.equipmentArray[7].ItemStruct._ItemVal));
				}
			}else{
				if(null != Inventory.Instance.equipmentArray[6] && !Inventory.Instance.equipmentArray[6].m_IsEmpty){
					UpdateToolsTipInfo(Inventory.Instance.equipmentArray[6].ItemStruct,1,_ItemTips.Instance.GetItemValue(ItemValueType.SALE,
					Inventory.Instance.equipmentArray[6].ItemStruct.info_Level,
					Inventory.Instance.equipmentArray[6].ItemStruct.info_eleVal,
					Inventory.Instance.equipmentArray[6].ItemStruct.info_encVal,
					Inventory.Instance.equipmentArray[6].ItemStruct._GemEffectVal,
					Inventory.Instance.equipmentArray[6].ItemStruct._ItemVal));
				}	
			}
			break;
		default:
			break;
		}
	}
	
	public void UpdateToolsTipInfo(ItemDropStruct infoObj,int posIdx,int money){
		if(null == infoObj){
			LogManager.Log_Warn("When update item tips , item Dont find infoObj");
			return;
		}
		if(0 == infoObj._TypeID){
			return;
		}
		string  itemName  = "";
		string  itemType  = "";
		string  itemLevel = infoObj.info_Level.ToString();
		string  itemAD    = "";
		string  itemSS    = "";
		float   itemVal = 0;		
		
		itemVal = (infoObj.info_gemVal + infoObj.info_encVal + infoObj.info_eleVal);	
		if(infoObj._TypeID == 7 || infoObj._TypeID == 8){			
		     itemAD    =  ((int)(infoObj.info_MinAtc * infoObj.info_Modifier)).ToString() + " - " + ((int)(infoObj.info_MaxAtc * infoObj.info_Modifier )).ToString();
			 itemSS    = infoObj.info_Speed;
//			 itemName  = infoObj.info_EncName + infoObj.info_GemName + infoObj.info_EleName + infoObj.info_TypeName;			
			 SetWeaponAD(itemAD,posIdx);
			 SetWeaponSS(itemSS,posIdx);
			 itemType  = infoObj.info_hand + infoObj.info_TypeName;
			 SetWeaponType(itemType,posIdx);
		}else if(1 == infoObj._TypeID|| 3 == infoObj._TypeID||4 == infoObj._TypeID||6 == infoObj._TypeID){		
			 itemAD    = ((int)(infoObj.info_MinDef * infoObj.info_Modifier)).ToString();
			 itemSS    = infoObj.info_Set;		
			if(infoObj._TypeID == 4){
//				itemName = _ItemTips.Instance.GetCloakName(infoObj);
			}else{
//				itemName = infoObj.info_EncName + infoObj.info_GemName + infoObj._TypeName + infoObj._TypelastName;
			}		
			SetArmorAD(itemAD,posIdx);
			SetArmorSS(posIdx);
			itemType  = infoObj.info_ArmorLevel + infoObj.info_TypeName;
			SetArmorType(itemType,posIdx);
		}else if(2 == infoObj._TypeID|| 5 == infoObj._TypeID){			
			 itemAD  = ((int)(infoObj.info_MinDef * infoObj.info_Modifier)).ToString();
			 itemSS  = "";
//			 itemName = infoObj.info_EncName + infoObj.info_EleName + infoObj.info_GemName + infoObj.info_TypeName;	
			 SetAccessoryAD(itemAD,posIdx);
			 SetAccessorySS(posIdx);
			 itemType  = infoObj.info_TypeName;
			 SetAccessorType(itemType,posIdx);
		}	
		itemName =  _UI_CS_ItemVendor.Instance.GetItemName(infoObj);
		SetName(itemName,itemVal,posIdx);
//		SetType(itemType,posIdx);
		SetLevel(itemLevel,posIdx);
		SetSpecialInfo(infoObj,posIdx);
		ValueAndRCInfo(infoObj,money,posIdx);
		SetTipPos(posIdx);
	}
	
	private void SetWeaponAD(string ad,int posIdx){
		if(1 == posIdx || 2 == posIdx){
//			ADNameA.Text = "DAMAGE";
			LocalizeManage.Instance.GetDynamicText(ADNameA,"DAMAGE");
            ADA.renderer.material = LocalizeFontManager.Instance.GetCurrentMat();
            ADA.SetFont(LocalizeFontManager.Instance.GetCurrentFont(), LocalizeFontManager.Instance.GetCurrentMat());
			ADA.Text = ad;
			ADIconA.SetUVs(new Rect(0,0,1,1));
			ADIconA.SetTexture(_UI_CS_Resource.Instance.m_EquipmentIcon[1]);
		}else{
//			ADNameB.Text = "DAMAGE";
			LocalizeManage.Instance.GetDynamicText(ADNameB,"DAMAGE");
            ADB.renderer.material = LocalizeFontManager.Instance.GetCurrentMat();
            ADB.SetFont(LocalizeFontManager.Instance.GetCurrentFont(), LocalizeFontManager.Instance.GetCurrentMat());
			ADB.Text = ad;
			ADIconB.SetUVs(new Rect(0,0,1,1));
			ADIconB.SetTexture(_UI_CS_Resource.Instance.m_EquipmentIcon[1]);
		}
	}
	
	private void SetArmorAD(string ad,int posIdx){
		if(1 == posIdx || 2 == posIdx){
//			ADNameA.Text = "ARMOR";
			LocalizeManage.Instance.GetDynamicText(ADNameA,"ARMOR");
            ADA.renderer.material = LocalizeFontManager.Instance.GetCurrentMat();
            ADA.SetFont(LocalizeFontManager.Instance.GetCurrentFont(), LocalizeFontManager.Instance.GetCurrentMat());
			ADA.Text = ad;
			ADIconA.SetUVs(new Rect(0,0,1,1));
			ADIconA.SetTexture(_UI_CS_Resource.Instance.m_EquipmentIcon[2]);
		}else{
//			ADNameB.Text = "ARMOR";
			LocalizeManage.Instance.GetDynamicText(ADNameB,"ARMOR");
            ADB.renderer.material = LocalizeFontManager.Instance.GetCurrentMat();
            ADB.SetFont(LocalizeFontManager.Instance.GetCurrentFont(), LocalizeFontManager.Instance.GetCurrentMat());
			ADB.Text = ad;
			ADIconB.SetUVs(new Rect(0,0,1,1));
			ADIconB.SetTexture(_UI_CS_Resource.Instance.m_EquipmentIcon[2]);
		}
	}
	
	private void SetAccessoryAD(string ad,int posIdx){
		if(1 == posIdx || 2 == posIdx){
//			ADNameA.Text = "";
			LocalizeManage.Instance.GetDynamicText(ADNameA,"ARMOR");
//			ADA.Text = "";
			ADA.Text = ad;
			ADIconA.SetUVs(new Rect(0,0,1,1));
//			ADIconA.SetTexture(_UI_CS_Resource.Instance.m_EquipmentIcon[0]);
			ADIconA.SetTexture(_UI_CS_Resource.Instance.m_EquipmentIcon[2]);
		}else{
//			ADNameB.Text = "";
			LocalizeManage.Instance.GetDynamicText(ADNameB,"ARMOR");
//			ADB.Text = "";
			ADB.Text = ad;
			ADIconB.SetUVs(new Rect(0,0,1,1));
//			ADIconB.SetTexture(_UI_CS_Resource.Instance.m_EquipmentIcon[0]);
			ADIconB.SetTexture(_UI_CS_Resource.Instance.m_EquipmentIcon[2]);
		}
	}
	
	private void SetWeaponSS(string ss,int posIdx){
		if(1 == posIdx || 2 == posIdx){
//			SpeedNameA.Text = "SPEED";
			LocalizeManage.Instance.GetDynamicText(SpeedNameA,"SPEED");
            SpeedA.renderer.material = LocalizeFontManager.Instance.GetCurrentMat();
            SpeedA.SetFont(LocalizeFontManager.Instance.GetCurrentFont(), LocalizeFontManager.Instance.GetCurrentMat());
			SpeedA.Text = ss;
		}else{
//			SpeedNameB.Text = "SPEED";
			LocalizeManage.Instance.GetDynamicText(SpeedNameB,"SPEED");
            SpeedB.renderer.material = LocalizeFontManager.Instance.GetCurrentMat();
            SpeedB.SetFont(LocalizeFontManager.Instance.GetCurrentFont(), LocalizeFontManager.Instance.GetCurrentMat());
			SpeedB.Text = ss;
		}
	}
	
	private void SetArmorSS(int posIdx){
		if(1 == posIdx || 2 == posIdx){
			SpeedNameA.Text = "";
			SpeedA.Text = "";
		}else{
			SpeedNameB.Text = "";
			SpeedB.Text = "";
		}
	}
	
	private void SetAccessorySS(int posIdx){
		if(1 == posIdx || 2 == posIdx){
			SpeedNameA.Text = "";
			SpeedA.Text = "";
		}else{
			SpeedNameB.Text = "";
			SpeedB.Text = "";
		}
	}
	
	private void SetName(string name,float val,int posIdx){
		if(1 == posIdx || 2 == posIdx){
            NameA.renderer.material = LocalizeFontManager.Instance.GetCurrentMat();
            NameA.SetFont(LocalizeFontManager.Instance.GetCurrentFont(), LocalizeFontManager.Instance.GetCurrentMat());
			NameA.Text = name;
			float itemVal = val;
			_UI_Color.Instance.SetNameColor(itemVal,NameA);
			_UI_Color.Instance.SetNameColor(itemVal,BG0A);
		}else{
            NameB.renderer.material = LocalizeFontManager.Instance.GetCurrentMat();
            NameB.SetFont(LocalizeFontManager.Instance.GetCurrentFont(), LocalizeFontManager.Instance.GetCurrentMat());
			NameB.Text = name;
			float itemVal = val;
			_UI_Color.Instance.SetNameColor(itemVal,NameB);
			_UI_Color.Instance.SetNameColor(itemVal,BG0B);
		}
	}
	
//	private void SetType(string type,int posIdx){
//		if(1 == posIdx || 2 == posIdx){
//			TypeA.Text = type;
//		}else{
//			TypeB.Text = type;
//		}
//	}
	
	private void SetWeaponType(string type,int posIdx){
		if(1 == posIdx || 2 == posIdx){
            TypeA.renderer.material = LocalizeFontManager.Instance.GetCurrentMat();
            TypeA.SetFont(LocalizeFontManager.Instance.GetCurrentFont(), LocalizeFontManager.Instance.GetCurrentMat());
			TypeA.Text = type;
		}else{
            TypeB.renderer.material = LocalizeFontManager.Instance.GetCurrentMat();
            TypeB.SetFont(LocalizeFontManager.Instance.GetCurrentFont(), LocalizeFontManager.Instance.GetCurrentMat());
			TypeB.Text = type;
		}
	}
	
	private void SetArmorType(string type,int posIdx){
		if(1 == posIdx || 2 == posIdx){
			TypeA.renderer.material = LocalizeFontManager.Instance.GetCurrentMat();
			TypeA.SetFont(LocalizeFontManager.Instance.GetCurrentFont(),LocalizeFontManager.Instance.GetCurrentMat());
			TypeA.Text = type;
		}else{
			TypeB.renderer.material = LocalizeFontManager.Instance.GetCurrentMat();
			TypeB.SetFont(LocalizeFontManager.Instance.GetCurrentFont(),LocalizeFontManager.Instance.GetCurrentMat());
			TypeB.Text = type;
		}
	}
	
	private void SetAccessorType(string type,int posIdx){
		if(1 == posIdx || 2 == posIdx){
			TypeA.renderer.material = LocalizeFontManager.Instance.GetCurrentMat();
			TypeA.SetFont(LocalizeFontManager.Instance.GetCurrentFont(),LocalizeFontManager.Instance.GetCurrentMat());
			TypeA.Text = type;
		}else{
			TypeB.renderer.material = LocalizeFontManager.Instance.GetCurrentMat();
			TypeB.SetFont(LocalizeFontManager.Instance.GetCurrentFont(),LocalizeFontManager.Instance.GetCurrentMat());
			TypeB.Text = type;
		}
	}
	
	private void SetLevel(string Lv,int posIdx){
		if(1 == posIdx || 2 == posIdx){
			LevelA.renderer.material = LocalizeFontManager.Instance.GetCurrentMat();
			LevelA.SetFont(LocalizeFontManager.Instance.GetCurrentFont(),LocalizeFontManager.Instance.GetCurrentMat());
			LevelA.Text = Lv;
		}else{
			LevelB.renderer.material = LocalizeFontManager.Instance.GetCurrentMat();
			LevelB.SetFont(LocalizeFontManager.Instance.GetCurrentFont(),LocalizeFontManager.Instance.GetCurrentMat());
			LevelB.Text = Lv;
		}
	}
	
	private void SetSpecialInfo(ItemDropStruct infoObj,int posIdx){
		Transform tempPos;
		EleCountA = 0;
		EleCountB = 0;
		if(1 == posIdx || 2 == posIdx){
			if(infoObj._EleID != 0){
				EleCountA++;
				tempPos = GetBgPosA(0);
				BG2A.transform.position = tempPos.position;
				EleNameA.renderer.material = LocalizeFontManager.Instance.GetCurrentMat();
				EleNameA.SetFont(LocalizeFontManager.Instance.GetCurrentFont(),LocalizeFontManager.Instance.GetCurrentMat());
				EleNameA.Text = infoObj.info_EleNameLv;
				EleIconA.SetUVs(new Rect(0,0,1,1));
				EleIconA.SetTexture(_UI_CS_ElementsInfo.Instance.EleIcon[infoObj.info_eleIconIdx-1]);
				EleDescritionA.renderer.material = LocalizeFontManager.Instance.GetCurrentMat();
				EleDescritionA.SetFont(LocalizeFontManager.Instance.GetCurrentFont(),LocalizeFontManager.Instance.GetCurrentMat());
				EleDescritionA.Text = (infoObj.info_EleDesc1 + infoObj.info_EleDesc2);
			}else{
				BG2A.transform.position = new Vector3(500f,500f,500f);
			}
			if(infoObj._EnchantID != 0){
				EleCountA++;
				tempPos = GetBgPosA(0);
				BG3A.transform.position = tempPos.position;
				EncNameA.renderer.material = LocalizeFontManager.Instance.GetCurrentMat();
				EncNameA.SetFont(LocalizeFontManager.Instance.GetCurrentFont(),LocalizeFontManager.Instance.GetCurrentMat());
				EncNameA.Text = infoObj.info_EncNameLv;
				EncIconA.SetUVs(new Rect(0,0,1,1));
				EncIconA.SetTexture(_UI_CS_ElementsInfo.Instance.EncIcon[infoObj.info_encIconIdx-1]);
				EncDescritionA.renderer.material = LocalizeFontManager.Instance.GetCurrentMat();
				EncDescritionA.SetFont(LocalizeFontManager.Instance.GetCurrentFont(),LocalizeFontManager.Instance.GetCurrentMat());
				EncDescritionA.Text = (infoObj.info_EncDesc1 + infoObj.info_EncDesc2);
			}else{
				BG3A.transform.position = new Vector3(500f,500f,500f);
			}
			if(infoObj._GemID != 0){
				EleCountA++;
				tempPos = GetBgPosA(0);
				BG4A.transform.position = tempPos.position;
				GemNameA.renderer.material = LocalizeFontManager.Instance.GetCurrentMat();
				GemNameA.SetFont(LocalizeFontManager.Instance.GetCurrentFont(),LocalizeFontManager.Instance.GetCurrentMat());
				GemNameA.Text = infoObj.info_GemeNameLv;
				GemIconA.SetUVs(new Rect(0,0,1,1));
				GemIconA.SetTexture(_UI_CS_ElementsInfo.Instance.GemIcon[infoObj.info_gemIconIdx-1]);
				GemDescritionA.renderer.material = LocalizeFontManager.Instance.GetCurrentMat();
				GemDescritionA.SetFont(LocalizeFontManager.Instance.GetCurrentFont(),LocalizeFontManager.Instance.GetCurrentMat());
				GemDescritionA.Text = (infoObj.info_GemDesc1 + infoObj.info_GemDesc2);
			}else{
				BG4A.transform.position = new Vector3(500f,500f,500f);
			}
		}else{
			if(infoObj._EleID != 0){
				EleCountB++;
				tempPos = GetBgPosB(0);
				BG2B.transform.position = tempPos.position;
				EleNameB.Text = infoObj.info_EleNameLv;
				EleIconB.SetUVs(new Rect(0,0,1,1));
				EleIconB.SetTexture(_UI_CS_ElementsInfo.Instance.EleIcon[infoObj.info_eleIconIdx-1]);
				EleDescritionB.Text = (infoObj.info_EleDesc1 + infoObj.info_EleDesc2);
			}else{
				BG2B.transform.position = new Vector3(500f,500f,500f);
			}
			if(infoObj._EnchantID != 0){
				EleCountB++;
				tempPos = GetBgPosB(0);
				BG3B.transform.position = tempPos.position;
				EncNameB.Text = infoObj.info_EncNameLv;
				EncIconB.SetUVs(new Rect(0,0,1,1));
				EncIconB.SetTexture(_UI_CS_ElementsInfo.Instance.EncIcon[infoObj.info_encIconIdx-1]);
				EncDescritionB.Text = (infoObj.info_EncDesc1 + infoObj.info_EncDesc2);
			}else{
				BG3B.transform.position = new Vector3(500f,500f,500f);
			}
			if(infoObj._GemID != 0){
				EleCountB++;
				tempPos = GetBgPosB(0);
				BG4B.transform.position = tempPos.position;
				GemNameB.Text = infoObj.info_GemName;
				GemIconB.SetUVs(new Rect(0,0,1,1));
				GemIconB.SetTexture(_UI_CS_ElementsInfo.Instance.GemIcon[infoObj.info_gemIconIdx-1]);
				GemDescritionB.Text = (infoObj.info_GemDesc1 + infoObj.info_GemDesc2);
			}else{
				BG4B.transform.position = new Vector3(500f,500f,500f);
			}
		}
	}
	
	private Transform GetBgPosA(int subordinate){
		switch(EleCountA+subordinate){
		case 1:
			return Pos1ObjA;
		case 2:
			return Pos2ObjA;
		case 3:
			return Pos3ObjA;
		case 4:
			return Pos4ObjA;
		default:
			return Pos4ObjA;
		}
	}
	
	private Transform GetBgPosB(int subordinate){
		switch(EleCountB+subordinate){
		case 1:
			return Pos1ObjB;
		case 2:
			return Pos2ObjB;
		case 3:
			return Pos3ObjB;
		case 4:
			return Pos4ObjB;
		default:
			return Pos4ObjB;
		}
	}
	
	private void ValueAndRCInfo(ItemDropStruct infoObj,int money,int posIdx){
		Transform tempPos;
		if(1 == posIdx || 2 == posIdx){
			tempPos = GetBgPosA(1);
			BG5A.transform.position = tempPos.position;
			if(-1 != money){
				MoneyA.Text = money.ToString();
			}else{
				MoneyA.Text = "0";
			}
		}else{
			tempPos = GetBgPosB(1);
			BG5B.transform.position = tempPos.position;
			if(-1 != money){
				MoneyB.Text = money.ToString();
			}else{
				MoneyB.Text = "0";
			}		
		}
	}
	
	private void SetTipPos(int posIdx){
		if(1 == posIdx || 2 == posIdx){
			EquipObjA.transform.position = RootObj1.transform.position;
		}else{
			EquipObjA.transform.position = RootObj2.transform.position;
			EquipObjB.transform.position = RootObj3.transform.position;
		}
	}
}
