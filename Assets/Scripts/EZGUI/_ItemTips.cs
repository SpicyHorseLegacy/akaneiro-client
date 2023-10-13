using UnityEngine;
using System.Collections;

public enum ItemPosOffestType{
	
	LEFT_TOP  = 1,	
	RIGHT_TOP = 2,	
	LEFT_BOT  = 3,	
	RIGHT_BOT = 4,	
	LEFT_MIDDLE = 5,
	RIGHT_MIDDLE = 6,
	MAXOR
	
}

public enum ItemValueType{
	
	TRANSMUTE  = 1,	
	SALE		= 2,
	MAX
	
}

public enum ItemRightClickType{
	
	TRANSMUTE  = 1,	
	BUY 		= 2,	
	SALE  		= 3,
	EQUIP  		= 4,	
	UNEQUIP  	= 5,	
	MAXOR
	
}

/// <summary>
/// _ item tips. it is ctrl item tips.
/// </summary>
public class _ItemTips : MonoBehaviour {
	//Instance
	public static _ItemTips Instance = null;
	
	public Transform  RootObj;
	
	private int   	  EleCount = 0;
	
	public UIButton   BG0;
	public UIButton   BG1;
	public UIButton   BG2;
	public UIButton   BG3;
	public UIButton   BG4;
	public UIButton   BG5;
	public UIButton   BG6;
	public SpriteText Name;
	public SpriteText Type;
	public SpriteText Level;
	public UIButton	  ADIcon;
	public SpriteText ADName;
	public SpriteText AD;
	public SpriteText SpeedName;
	public SpriteText Speed;
	public SpriteText EleName;
	public SpriteText EncName;
	public SpriteText GemName;
	public SpriteText EleDescrition;
	public SpriteText EncDescrition;
	public SpriteText GemDescrition;
	public UIButton   EleIcon;
	public UIButton   EncIcon;
	public UIButton   GemIcon;
	public UIButton   EquipBG;
	public SpriteText Equipped; 
	public SpriteText Money;
	public SpriteText RcType;
	
	public Transform  Pos1Obj;
	public Transform  Pos2Obj;
	public Transform  Pos3Obj;
	public Transform  Pos4Obj;
	public Transform  Pos5Obj;
	
	public Transform  ConsumableObj;
	public UIButton   BG7;
	public UIButton   BG8;
	public UIButton   BG9;
	public SpriteText ConsumableName;
	public SpriteText ConsumableDescription2;
	
	private const float dealyTime = 1f;
	
	public Camera     TipsCanera;
	
	public bool isShowItemTip = false;
	
	void Awake(){
		Instance = this;
	}
	
	// Use this for initialization
	void Start () {
		HideEquipped();
		TipsCanera.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
	}
	
	//if money == -1, it calc mineself;
    public void UpdateToolsTipInfo(ItemDropStruct infoObj, int money, ItemRightClickType rcType, ItemPosOffestType poType, Vector3 pos, float width, float height)
    {
        if (null == infoObj)
        {
            LogManager.Log_Warn("When update item tips , item Dont find infoObj");
            return;
        }
		
		if(pos.z < -5||pos.z > 5) {
			pos.z = -3;
		}
		
        isShowItemTip = true;
        TipsCanera.enabled = true;
        if (infoObj._TypeID > 8)
        {
            IntiConsumableInfo(infoObj._PropsName, infoObj._PropsDes, infoObj._PropsDes2, poType, pos,infoObj._TypeID);
        }
        else
        {
            string itemName = "";
            string itemType = "";
            string itemLevel = infoObj.info_Level.ToString();
            string itemAD = "";
            string itemSS = "";
            float itemVal = 0;

            itemVal = (infoObj.info_gemVal + infoObj.info_encVal + infoObj.info_eleVal);

            if (infoObj._TypeID == 7 || infoObj._TypeID == 8)
            {
                //weapon
                itemAD = ((int)(infoObj.info_MinAtc * infoObj.info_Modifier)).ToString() + " - " + ((int)(infoObj.info_MaxAtc * infoObj.info_Modifier)).ToString();
                itemSS = infoObj.info_Speed;
//                itemName = infoObj.info_EncName + infoObj.info_GemName + infoObj.info_EleName + infoObj.info_TypeName;
                SetWeaponAD(itemAD);
                SetWeaponSS(itemSS);
                itemType = infoObj.info_hand + infoObj.info_TypeName;
                SetWeaponType(itemType);
            }
            else if (1 == infoObj._TypeID || 3 == infoObj._TypeID || 4 == infoObj._TypeID || 6 == infoObj._TypeID)
            {
                //armor
                itemAD = ((int)(infoObj.info_MinDef * infoObj.info_Modifier)).ToString();
                itemSS = infoObj.info_Set;
                if (infoObj._TypeID == 4)
                {
//                    itemName = GetCloakName(infoObj);
                }
                else
                {
//                    itemName = infoObj.info_EncName + infoObj.info_GemName + infoObj._TypeName + infoObj._TypelastName;
                }
                SetArmorAD(itemAD);
                SetArmorSS();
                itemType = infoObj.info_ArmorLevel + infoObj.info_TypeName;
                SetArmorType(itemType);
            }
            else if (2 == infoObj._TypeID || 5 == infoObj._TypeID)
            {
                //accesery
                itemAD = ((int)(infoObj.info_MinDef * infoObj.info_Modifier)).ToString();
                itemSS = "";
//                itemName = infoObj.info_EncName + infoObj.info_EleName + infoObj.info_GemName + infoObj.info_TypeName;
                SetAccessoryAD(itemAD);
                SetAccessorySS();
                itemType = infoObj.info_TypeName;
                SetAccessorType(itemType);
            }
            itemName =  _UI_CS_ItemVendor.Instance.GetItemName(infoObj);
            SetName(itemName, itemVal);
            //			SetType(itemType);
            SetLevel(infoObj.info_Level);
            SetSpecialInfo(infoObj);
            ValueAndRCInfo(infoObj, money, rcType);
            SetTipPos(poType, pos, width, height);
        }
    }
	
	public void ItemTipFadeIn() {
//		transform.GetComponent<UIPanel>().BringIn();
		
	}
	
	public void ItemTipFadeOut() {
//		transform.GetComponent<UIPanel>().Dismiss();
	}
	
	public string GetCloakName(ItemDropStruct info){
		string name = "";
		name = info.info_EncName  + info.info_GemName + info._QualityName + info._CloakName;
		return name;
	}
	
	private void IntiConsumableInfo(string name ,string dec1,string dec2,ItemPosOffestType poType,Vector3 pos,int type){
		ConsumableName.renderer.material = LocalizeFontManager.Instance.GetCurrentMat();
        ConsumableName.SetFont(LocalizeFontManager.Instance.GetCurrentFont(), LocalizeFontManager.Instance.GetCurrentMat());
		ConsumableName.Text = name;
		ConsumableDescription2.renderer.material = LocalizeFontManager.Instance.GetCurrentMat();
		ConsumableDescription2.SetFont(LocalizeFontManager.Instance.GetCurrentFont(),LocalizeFontManager.Instance.GetCurrentMat());
		ConsumableDescription2.Text = dec2;
		float x = 0f;	float y = 0f;	float z = 0f;
		float tHeight = 0f;
		tHeight = BG7.height + BG7.height + 0.2f;
		switch(poType){
		case ItemPosOffestType.LEFT_TOP:
			x = (0-BG7.width);
			y = tHeight;
			z = 0;
			break;
		case ItemPosOffestType.RIGHT_TOP:
			x = 0;
			y = tHeight;
			z = 0;
			break;
		case ItemPosOffestType.LEFT_BOT:
			x = (0-BG7.width);
			y = 0;
			z = 0;
			break;
		case ItemPosOffestType.RIGHT_BOT:
			x = 0; y = 0; z = 0;
			break;
		default:
			break;
		}
		
		if(type == 11 || type == 12 || type == 13) {
			BG9.transform.localPosition = new Vector3(BG9.transform.localPosition.x,BG9.transform.localPosition.y,999f);
		}else {
			BG9.transform.localPosition = new Vector3(BG9.transform.localPosition.x,BG9.transform.localPosition.y,-0.1f);
		}
		
		ConsumableObj.transform.position = new Vector3(pos.x + x,pos.y + y,pos.z + z -1.5f);
//		ShowConTips(new Vector3(pos.x + x,pos.y + y,pos.z + z -1.5f));
	}
	
	private void ShowConTips(Vector3 vec3) {
		//延迟一段时间后显示TIP//
		StartCoroutine(ConDelayShowTip(vec3));
	}
	
	private IEnumerator ConDelayShowTip(Vector3 vec3) {
		yield return new WaitForSeconds(dealyTime);
		ConsumableObj.transform.position = vec3;
	}
	
	private void HideConTip() {
		ConsumableObj.transform.position = new Vector3(999f,999f,999f);
		StopCoroutine("ConDelayShowTip");
	}
	
	public void DismissItemTip(){
		isShowItemTip = false;
		RootObj.transform.position = new Vector3(999f,999f,999f);
		ConsumableObj.transform.position = new Vector3(999f,999f,999f);
		ItemEquipTips.Instance.DismissCompareTips();
		TipsCanera.enabled = false;
	}
	
	private void HideEquipped(){
		EquipBG.Hide(true);
		Equipped.Text = "";
	}
	
	private void SetTipPos(ItemPosOffestType poType,Vector3 pos,float width,float height){
		float x = 0f;
		float y = 0f;
		float z = 0f;
		
		float tHeight = 0f;
		tHeight = BG1.height + (EleCount*BG2.height) +BG5.height;
		
		switch(poType){
		case ItemPosOffestType.LEFT_TOP:
			x = (0-BG1.width) - width/2;
			y = tHeight + height/2;
			z = 0;
			break;
		case ItemPosOffestType.RIGHT_TOP:
			x = 0 + width/2;
			y = tHeight + height/2;
			z = 0;
			break;
		case ItemPosOffestType.LEFT_BOT:
			x = (0-BG1.width) - width/2;
			y = 0 - height/2;
			z = 0;
			break;
		case ItemPosOffestType.RIGHT_BOT:
			x = 0+ width/2; y = 0 - height/2; z = 0;
			break;
		case ItemPosOffestType.LEFT_MIDDLE:
			x = (0-BG1.width)- width/2; 
			y = tHeight/2; 
			z = 0;
			break;
		case ItemPosOffestType.RIGHT_MIDDLE:
			x = 0+ width/2; 
			y = tHeight/2; 
			z = 0;
			break;
		default:
			break;
		}
		
		RootObj.transform.position = new Vector3(pos.x + x,pos.y + y,pos.z + z -1.5f);
//		ShowTips(new Vector3(pos.x + x,pos.y + y,pos.z + z -1.5f));
	}
	
	private void ShowTips(Vector3 vec3) {
		//延迟一段时间后显示TIP//
		StartCoroutine(DelayShowTip(vec3));
	}
	
	private IEnumerator DelayShowTip(Vector3 vec3) {
		yield return new WaitForSeconds(dealyTime);
		RootObj.transform.position = vec3;
	}
	
	private void HideTip() {
		RootObj.transform.position = new Vector3(999f,999f,999f);
		StopCoroutine("DelayShowTip");
	}
	
	private void SetSpecialInfo(ItemDropStruct infoObj){
		Transform tempPos;
		EleCount = 0;
		if(infoObj._EleID != 0){
			EleCount++;
			tempPos = GetBgPos(0);
			BG2.transform.position = tempPos.position;
			EleName.renderer.material = LocalizeFontManager.Instance.GetCurrentMat();
			EleName.SetFont(LocalizeFontManager.Instance.GetCurrentFont(),LocalizeFontManager.Instance.GetCurrentMat());
			EleName.Text = infoObj.info_EleNameLv;
			if(infoObj.info_eleIconIdx -1 >= 0) {
				EleIcon.SetUVs(new Rect(0,0,1,1));
				EleIcon.SetTexture(_UI_CS_ElementsInfo.Instance.EleIcon[infoObj.info_eleIconIdx-1]);
			}
			EleDescrition.renderer.material = LocalizeFontManager.Instance.GetCurrentMat();
			EleDescrition.SetFont(LocalizeFontManager.Instance.GetCurrentFont(),LocalizeFontManager.Instance.GetCurrentMat());
			EleDescrition.Text = (infoObj.info_EleDesc1 + infoObj.info_EleDesc2);
		}else{
			BG2.transform.position = new Vector3(500f,500f,500f);
		}
		if(infoObj._EnchantID != 0){
			EleCount++;
			tempPos = GetBgPos(0);
			BG3.transform.position = tempPos.position;
			EncName.renderer.material = LocalizeFontManager.Instance.GetCurrentMat();
			EncName.SetFont(LocalizeFontManager.Instance.GetCurrentFont(),LocalizeFontManager.Instance.GetCurrentMat());
			EncName.Text = infoObj.info_EncNameLv;
			if(infoObj.info_encIconIdx -1 >= 0) {
				EncIcon.SetUVs(new Rect(0,0,1,1));
				EncIcon.SetTexture(_UI_CS_ElementsInfo.Instance.EncIcon[infoObj.info_encIconIdx-1]);
			}
			EncDescrition.renderer.material = LocalizeFontManager.Instance.GetCurrentMat();
			EncDescrition.SetFont(LocalizeFontManager.Instance.GetCurrentFont(),LocalizeFontManager.Instance.GetCurrentMat());
			EncDescrition.Text = (infoObj.info_EncDesc1 + infoObj.info_EncDesc2);
		}else{
			BG3.transform.position = new Vector3(500f,500f,500f);
		}
		if(infoObj._GemID != 0){
			EleCount++;
			tempPos = GetBgPos(0);
			BG4.transform.position = tempPos.position;
			GemName.renderer.material = LocalizeFontManager.Instance.GetCurrentMat();
			GemName.SetFont(LocalizeFontManager.Instance.GetCurrentFont(),LocalizeFontManager.Instance.GetCurrentMat());
			GemName.Text = infoObj.info_GemeNameLv;
			if(infoObj.info_gemIconIdx -1 >= 0) {
				GemIcon.SetUVs(new Rect(0,0,1,1));
				GemIcon.SetTexture(_UI_CS_ElementsInfo.Instance.GemIcon[infoObj.info_gemIconIdx-1]);
			}
			GemDescrition.renderer.material = LocalizeFontManager.Instance.GetCurrentMat();
			GemDescrition.SetFont(LocalizeFontManager.Instance.GetCurrentFont(),LocalizeFontManager.Instance.GetCurrentMat());
			GemDescrition.Text = (infoObj.info_GemDesc1 + infoObj.info_GemDesc2);
		}else{
			BG4.transform.position = new Vector3(500f,500f,500f);
		}
	}
	
	private void ValueAndRCInfo(ItemDropStruct infoObj,int money,ItemRightClickType rcType){
		Transform tempPos;
		tempPos = GetBgPos(1);
		BG5.transform.position = tempPos.position;
		if(-1 != money){
			Money.Text = money.ToString();
		}else{
			Money.Text = "0";
		}
		tempPos = GetBgPos(2);
		if(EleCount != 3){
			BG6.transform.position = new Vector3(tempPos.position.x+1.5f,tempPos.position.y-0.15f,tempPos.position.z);
		}else{
			BG6.transform.position = new Vector3(tempPos.position.x+1.5f,tempPos.position.y,tempPos.position.z);
		}
		switch(rcType){
		case ItemRightClickType.BUY:
//			RcType.Text = "[#FF3C00]right-click[#FFFFFF] to buy";
			LocalizeManage.Instance.GetDynamicText(RcType,"RCB");
			break;
		case ItemRightClickType.SALE:
//			RcType.Text = "[#FF3C00]right-click[#FFFFFF] to sell";
			LocalizeManage.Instance.GetDynamicText(RcType,"RCS");
			break;
		case ItemRightClickType.TRANSMUTE:
//			RcType.Text = "[#FF3C00]right-click[#FFFFFF] to transmute";
			LocalizeManage.Instance.GetDynamicText(RcType,"RCT");
			break;
		case ItemRightClickType.EQUIP:
//			RcType.Text = "[#FF3C00]right-click[#FFFFFF] to equip";
			LocalizeManage.Instance.GetDynamicText(RcType,"RCE");
			break;
		case ItemRightClickType.UNEQUIP:
//			RcType.Text = "[#FF3C00]right-click[#FFFFFF] to unequip";
			LocalizeManage.Instance.GetDynamicText(RcType,"RCU");
			break;
		default:
			RcType.Text = "";
			break;
		}
	}
	
	public int GetItemValue(ItemValueType valueType,int level,float eleVal,float encVal,float gemVal,float itemVal){
		int itemValue = 0;
		switch(valueType){
		case ItemValueType.TRANSMUTE:
			itemValue = (int)((level)*(1+eleVal+encVal+gemVal)*(itemVal*0.2f));
			return itemValue;
		case ItemValueType.SALE:
			itemValue = (int)((level)*(1+eleVal+encVal+gemVal)*(itemVal*0.4f));
			return itemValue;
		default:
			return -1;
		}
	}
	
	private Transform GetBgPos(int subordinate){
		switch(EleCount+subordinate){
		case 1:
			return Pos1Obj;
		case 2:
			return Pos2Obj;
		case 3:
			return Pos3Obj;
		case 4:
			return Pos4Obj;
		case 5:
			return Pos5Obj;
		default:
			return Pos5Obj;
		}
	}
	
	private void SetName(string name,float val){
		Name.renderer.material = LocalizeFontManager.Instance.GetCurrentMat();
		Name.SetFont(LocalizeFontManager.Instance.GetCurrentFont(),LocalizeFontManager.Instance.GetCurrentMat());
		Name.Text = name;
		float itemVal = val;
		_UI_Color.Instance.SetNameColor(itemVal,Name);
		_UI_Color.Instance.SetNameColor(itemVal,BG0);
	}
	
	private void SetWeaponType(string type){
		Type.Text = type;
	}
	
	private void SetArmorType(string type){
		Type.Text = type;
	}
	
	private void SetAccessorType(string type){
		Type.Text = type;
	}
	
	private void SetLevel(int Lv){
		if(null != _PlayerData.Instance){
			if(Lv <= _PlayerData.Instance.playerLevel){
				LocalizeManage.Instance.GetDynamicText(Level,"LEVEL");
				Level.Text += Lv.ToString();
				Level.SetColor(_UI_Color.Instance.color1);
			}else{
				LocalizeManage.Instance.GetDynamicText(Level,"REQLEVEL");
				Level.Text += Lv.ToString();
				Level.SetColor(_UI_Color.Instance.color14);
			}
			
		}
	}
	
	private void SetWeaponAD(string ad){
//		ADName.Text = "DAMAGE";
		LocalizeManage.Instance.GetDynamicText(ADName,"DAMAGE");
		AD.Text = ad;
		ADIcon.SetUVs(new Rect(0,0,1,1));
		ADIcon.SetTexture(_UI_CS_Resource.Instance.m_EquipmentIcon[1]);
	}
	
	private void SetArmorAD(string ad){
//		ADName.Text = "ARMOR";
		LocalizeManage.Instance.GetDynamicText(ADName,"ARMOR");
		AD.Text = ad;
		ADIcon.SetUVs(new Rect(0,0,1,1));
		ADIcon.SetTexture(_UI_CS_Resource.Instance.m_EquipmentIcon[2]);
	}
	
	private void SetAccessoryAD(string ad){
//		ADName.Text = "";		
		LocalizeManage.Instance.GetDynamicText(ADName,"ARMOR");
//		AD.Text = "";
		AD.Text = ad;
		ADIcon.SetUVs(new Rect(0,0,1,1));
//		ADIcon.SetTexture(_UI_CS_Resource.Instance.m_EquipmentIcon[0]);
		ADIcon.SetTexture(_UI_CS_Resource.Instance.m_EquipmentIcon[2]);
	}
	
	private void SetWeaponSS(string ss){
//		SpeedName.Text = "SPEED";
		LocalizeManage.Instance.GetDynamicText(SpeedName,"SPEED");
		Speed.Text = ss;
	}
	
	private void SetArmorSS(){
		SpeedName.Text = "";
		Speed.Text = "";
	}
	
	private void SetAccessorySS(){
		SpeedName.Text = "";
		Speed.Text = "";
	}
}
