using UnityEngine;
using System.Collections;

public class _UI_CS_ToolsTip : MonoBehaviour {
	
	//Instance
	public static _UI_CS_ToolsTip Instance = null;
	
	public Vector3 ToolTipPos;
	
	public UIPanel WeaponPanel;
	
//	public UIButton	  WeaponBg;
	
	public SpriteText WeaponName;
	public SpriteText WeaponType;
	public SpriteText WeaponRequiresLevel;
	public SpriteText WeaponValue;
	public SpriteText WeaponATK;
	public SpriteText WeaponSpeed;
	public SpriteText WeaponEle;
	public SpriteText WeaponEnc;
	public SpriteText WeaponGem;
	public UIButton   WeaponEleUp;
	public UIButton   WeaponEleDown;
	public UIButton   WeaponEncUp;
	public UIButton   WeaponEncDown;
	public UIButton   WeaponGemUp;
	public UIButton   WeaponGemDown;
	public UIButton   WeaponSpeedUp;
	public UIButton   WeaponSpeedDown;
	public UIButton   WeaponAtkUp;
	public UIButton   WeaponAtkDown;
	public SpriteText WeaponEleLevel;
	public SpriteText WeaponEncLevel;
	public SpriteText WeaponGemLevel;
	
	public UIButton	  WeaponEleIcon;
	public UIButton	  WeaponEncIcon;
	public UIButton	  WeaponGemIcon;
	
	
	public UIPanel ArmorPanel;
	
//	public UIButton   ArmorBg;
	
	public SpriteText ArmorName;
	public SpriteText ArmorType;
	public SpriteText ArmorRequiresLevel;
	public SpriteText ArmorValue;
	public SpriteText ArmorDef;
	public SpriteText ArmorSet;
	public SpriteText ArmorEnc;
	public SpriteText ArmorGem;
	public SpriteText ArmorEle;
	
	public UIButton    ArmorEleUp;
	public UIButton    ArmorEleDown;
	public UIButton    ArmorEncUp;
	public UIButton    ArmorEncDown;
	public UIButton    ArmorGemUp;
	public UIButton    ArmorGemDown;
//	public UIButton    ArmorSpeedUp;
//	public UIButton    ArmorSpeedDown;
	public UIButton    ArmorDefUp;
	public UIButton    ArmorDefDown;
	public SpriteText ArmorEleLevel;
	public SpriteText ArmorEncLevel;
	public SpriteText ArmorGemLevel;
	
	public UIButton	  ArmorEleIcon;
	public UIButton	  ArmorEncIcon;
	public UIButton	  ArmorGemIcon;
	
	
	public UIPanel AccessoryPanel;
	
//	public UIButton   AccessoryBg;
	
	public SpriteText AccessoryName;
	public SpriteText AccessoryType;
	public SpriteText AccessoryRequiresLevel;
	public SpriteText AccessoryValue;
	public SpriteText AccessoryEle;
	public SpriteText AccessoryEnc;
	public SpriteText AccessoryGem;
	
	public UIButton    AccessoryEleUp;
	public UIButton    AccessoryEleDown;
	public UIButton    AccessoryEncUp;
	public UIButton    AccessoryEncDown;
	public UIButton    AccessoryGemUp;
	public UIButton    AccessoryGemDown;

	public SpriteText  AccessoryEleLevel;
	public SpriteText  AccessoryEncLevel;
	public SpriteText  AccessoryGemLevel;
	
	public UIButton	   AccessoryEleIcon;
	public UIButton	   AccessoryEncIcon;
	public UIButton	   AccessoryGemIcon;
//	public Transform  IngameToolsTip;
//	public SpriteText IngameName;
//	public bool isShowInGameToolsTip = false;
	
	void Awake()
	{
		Instance = this;
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
//		if(isShowInGameToolsTip){
//			//IngameToolsTip.transform.position = _UI_CS_Ctrl.Instance.m_UI_Camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x   , Input.mousePosition.y ,_UI_CS_Ctrl.Instance.m_UI_Camera.nearClipPlane));
//			//IngameToolsTip.transform.position = new Vector3(IngameToolsTip.transform.position.x - 8,IngameToolsTip.transform.position.y - 6.5f ,-3);
//		}else{
//			//IngameToolsTip.position = new Vector3(1000f,1000f,1000f);
//		}
		
	}
	
	public void DismissToolTips(){
		AccessoryPanel.transform.position = new Vector3(1000f,1000f,1000f);
		WeaponPanel.transform.position = new Vector3(1000f,1000f,1000f);
		ArmorPanel.transform.position = new Vector3(1000f,1000f,1000f);	
	}
	
/// //////////////////////////////////////////////////////////////////////////////
	
	public void IsPopUpInGameToolsTip(bool isShow){
		
//		if(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_INGAME_NORMAL == _UI_CS_ScreenCtrl.Instance.currentScreenType){
//		
//			isShowInGameToolsTip = isShow;
//		}
	}
	
	public void UpdateToolsTipInfo(int idx,string name,string type,string requiresLevel,
	                               string Value,string gem,string enc,string ad_val,
	                               string ss_val,string ele,int eleVal,int encVal,int gemVal,int eleIconIdx,int encIconIdx,int gemIconIdx){
		
		
		
		SetName(idx,name,Value);
		SetType(idx,type);
		SetRequiresLevel(idx,requiresLevel);
		SetValue(idx,Value);
		SetGem(idx,gem);
		SetEnc(idx,enc);
		
		SetEleLevel(idx,eleVal.ToString());
		SetEncLevel(idx,encVal.ToString());
		SetGemLevel(idx,gemVal.ToString());
		
		SetEleIcon(idx,eleIconIdx);
		SetEncIcon(idx,encIconIdx);
		SetGemIcon(idx,gemIconIdx);
		
		//////////////////////////////////////////////////
		
		SetAD_Val(idx,ad_val);
		SetSS_Val(idx,ss_val);
		SetEle(idx,ele);
		
	}

	public void SetWeaponName(string name,string val){
		WeaponName.Text = name;
		float itemVal = float.Parse(val);
		_UI_Color.Instance.SetNameColor(itemVal,WeaponName);	
	}
	
	public void SetArmorName(string name,string val){
		ArmorName.Text = name;
		float itemVal = float.Parse(val);
		_UI_Color.Instance.SetNameColor(itemVal,ArmorName);	
	}
	
	public void SetAccessoryName(string name,string val){
		AccessoryName.Text = name;
		float itemVal = float.Parse(val);
		_UI_Color.Instance.SetNameColor(itemVal,AccessoryName);	
	}
	
	
	public void SetName(int idx, string name, string val){
		switch(idx){
		case 0:
				SetWeaponName(name,val);
				break;
		case 1:
				SetArmorName(name,val);
				break;
		case 2:
				SetAccessoryName(name,val);
				break;
		default:
				break;
		}
	}
	
	public void SetEleLevel(int idx, string val){
		switch(idx){
		case 0:
				SetWeaponEleLevel(val);
				break;
		case 1:
				SetArmorEleLevel(val);
				break;
		case 2:
				SetAccessoryEleLevel(val);
				break;
		default:
				break;
		}
	}
	
	public void SetWeaponEleLevel(string type){
		WeaponEleLevel.Text = type;
	}
	
	public void SetArmorEleLevel(string type){
		ArmorEleLevel.Text = type;
	}
	
	public void SetAccessoryEleLevel(string type){
		AccessoryEleLevel.Text = type;
	}
	
	public void SetEncLevel(int idx, string val){
		switch(idx){
		case 0:
				SetWeaponEncLevel(val);
				break;
		case 1:
				SetArmorEncLevel(val);
				break;
		case 2:
				SetAccessoryEncLevel(val);
				break;
		default:
				break;
		}
	}
	
	public void SetWeaponEncLevel(string type){
		WeaponEncLevel.Text = type;
	}
	
	public void SetArmorEncLevel(string type){
		ArmorEncLevel.Text = type;
	}
	
	public void SetAccessoryEncLevel(string type){
		AccessoryEncLevel.Text = type;
	}

	public void SetGemLevel(int idx, string val){
		switch(idx){
		case 0:
				SetWeaponGemLevel(val);
				break;
		case 1:
				SetArmorGemLevel(val);
				break;
		case 2:
				SetAccessoryGemLevel(val);
				break;
		default:
				break;
		}
	}
	
	public void SetWeaponGemLevel(string type){
		//WeaponGemLevel.Text = type;
		WeaponGemLevel.Text = "";
	}
	
	public void SetArmorGemLevel(string type){
		//ArmorGemLevel.Text = type;
		ArmorGemLevel.Text = "";
	}
	
	public void SetAccessoryGemLevel(string type){
		//AccessoryGemLevel.Text = type;
		AccessoryGemLevel.Text = "";
	}
	
	public void SetWeaponType(string type){
		WeaponType.Text = type;
	}
	
	public void SetArmorType(string type){
		ArmorType.Text = type;
	}
	
	public void SetAccessoryType(string type){
		AccessoryType.Text = type;
	}
	
	public void SetType(int idx, string type){
		switch(idx){
		case 0:
				SetWeaponType(type);
				break;
		case 1:
				SetArmorType(type);
				break;
		case 2:
				SetAccessoryType(type);
				break;
		default:
				break;
		}
	}
	
	public void SetWeaponRequiresLevel(string requiresLevel){
		WeaponRequiresLevel.Text = requiresLevel;
	}
	
	public void SetArmorRequiresLevel(string requiresLevel){
		ArmorRequiresLevel.Text = requiresLevel;
	}
	
	public void SetAccessoryRequiresLevel(string requiresLevel){
		AccessoryRequiresLevel.Text = requiresLevel;
	}
	
	public void SetRequiresLevel(int idx, string requiresLevel){
		switch(idx){
		case 0:
				SetWeaponRequiresLevel(requiresLevel);
				break;
		case 1:
				SetArmorRequiresLevel(requiresLevel);
				break;
		case 2:
				SetAccessoryRequiresLevel(requiresLevel);
				break;
		default:
				break;
		}
	}
	
	public void SetWeaponValue(string Value){
		WeaponValue.Text = Value;
	}
	
	public void SetArmorValue(string Value){
		ArmorValue.Text = Value;
	}
	
	public void SetAccessoryValue(string Value){
		AccessoryValue.Text = Value;
	}
	
	public void SetValue(int idx, string Value){
		switch(idx){
		case 0:
				SetWeaponValue(Value);
				break;
		case 1:
				SetArmorValue(Value);
				break;
		case 2:
				SetAccessoryValue(Value);
				break;
		default:
				break;
		}
	}
	
	public void SetWeaponGem(string gem){
		WeaponGem.Text = gem;
	}
	
	public void SetArmorGem(string gem){
		ArmorGem.Text = gem;
	}
	
	public void SetAccessoryGem(string gem){
		AccessoryGem.Text = gem;
	}
	
	public void SetGem(int idx, string gem){
		switch(idx){
		case 0:
				SetWeaponGem(gem);
				break;
		case 1:
				SetArmorGem(gem);
				break;
		case 2:
				SetAccessoryGem(gem);
				break;
		default:
				break;
		}
	}
	
	public void SetWeaponEnc(string enc){
		WeaponEnc.Text = enc;
	}
	
	public void SetArmorEnc(string enc){
		ArmorEnc.Text = enc;
	}
	
	public void SetAccessoryEnc(string enc){
		AccessoryEnc.Text = enc;
	}
	
	public void SetEnc(int idx, string enc){
		switch(idx){
		case 0:
				SetWeaponEnc(enc);
				break;
		case 1:
				SetArmorEnc(enc);
				break;
		case 2:
				SetAccessoryEnc(enc);
				break;
		default:
				break;
		}
	}
	
	public void SetWeaponEle(string ele){
		WeaponEle.Text = ele;
	}
	
	public void SetArmorEle(string ele){
		ArmorEle.Text = ele;
	}
	
	public void SetAccessoryEle(string ele){
		AccessoryEle.Text = ele;
	}
	
	public void SetEle(int idx, string ele){
		switch(idx){
		case 0:
				SetWeaponEle(ele);
				break;
		case 1:
				SetArmorEle(ele);
				break;
		case 2:
				SetAccessoryEle(ele);
				break;
		default:
				break;
		}
	}
	
	public void SetEleIcon(int idx, int ele){
		switch(idx){
		case 0:
				SetWeaponEleIcon(ele);
				break;
		case 1:
				SetArmorEleIcon(ele);
				break;
		case 2:
				SetAccessoryEleIcon(ele);
				break;
		default:
				break;
		}
	}
	
	public void SetWeaponEleIcon(int ele){
		if(0 != ele){
			
			WeaponEleIcon.transform.position = new Vector3(WeaponEleIcon.transform.position.x,WeaponEleIcon.transform.position.y,999f);
			WeaponEleIcon.SetUVs(new Rect(0,0,1,1));
			WeaponEleIcon.SetTexture(_UI_CS_ElementsInfo.Instance.EleIcon[ele-1]);
			
		}else{
			
			WeaponEleIcon.transform.position = new Vector3(WeaponEleIcon.transform.position.x,WeaponEleIcon.transform.position.y,-999);
			
		}
	}
	
	public void SetArmorEleIcon(int ele){
		if(0 != ele){
			
			ArmorEleIcon.transform.position = new Vector3(ArmorEleIcon.transform.position.x,ArmorEleIcon.transform.position.y,999f);
			ArmorEleIcon.SetUVs(new Rect(0,0,1,1));
			ArmorEleIcon.SetTexture(_UI_CS_ElementsInfo.Instance.EleIcon[ele-1]);
			
		}else{
			
			ArmorEleIcon.transform.position = new Vector3(ArmorEleIcon.transform.position.x,ArmorEleIcon.transform.position.y,-999);
			
		}
	}
	
	public void SetAccessoryEleIcon(int ele){
		if(0 != ele){
			
			AccessoryEleIcon.transform.position = new Vector3(AccessoryEleIcon.transform.position.x,AccessoryEleIcon.transform.position.y,999f);
			AccessoryEleIcon.SetUVs(new Rect(0,0,1,1));
			AccessoryEleIcon.SetTexture(_UI_CS_ElementsInfo.Instance.EleIcon[ele-1]);
			
		}else{
			
			AccessoryEleIcon.transform.position = new Vector3(AccessoryEleIcon.transform.position.x,AccessoryEleIcon.transform.position.y,-999);
			
		}
	}
	
	public void SetEncIcon(int idx, int ele){
		switch(idx){
		case 0:
				SetWeaponEncIcon(ele);
				break;
		case 1:
				SetArmorEncIcon(ele);
				break;
		case 2:
				SetAccessoryEncIcon(ele);
				break;
		default:
				break;
		}
	}
	
	public void SetWeaponEncIcon(int ele){
		if(0 != ele){
			
			WeaponEncIcon.transform.position = new Vector3(WeaponEncIcon.transform.position.x,WeaponEncIcon.transform.position.y,999f);
			WeaponEncIcon.SetUVs(new Rect(0,0,1,1));
			WeaponEncIcon.SetTexture(_UI_CS_ElementsInfo.Instance.EncIcon[ele-1]);
			
		}else{
			
			WeaponEncIcon.transform.position = new Vector3(WeaponEncIcon.transform.position.x,WeaponEncIcon.transform.position.y,-999);
			
		}
	}
	
	public void SetArmorEncIcon(int ele){
		if(0 != ele){
			
			ArmorEncIcon.transform.position = new Vector3(ArmorEncIcon.transform.position.x,ArmorEncIcon.transform.position.y,999f);
			ArmorEncIcon.SetUVs(new Rect(0,0,1,1));
			ArmorEncIcon.SetTexture(_UI_CS_ElementsInfo.Instance.EncIcon[ele-1]);
			
		}else{
			
			ArmorEncIcon.transform.position = new Vector3(ArmorEncIcon.transform.position.x,ArmorEncIcon.transform.position.y,-999);
			
		}
	}
	
	public void SetAccessoryEncIcon(int ele){
		if(0 != ele){
			
			AccessoryEncIcon.transform.position = new Vector3(AccessoryEncIcon.transform.position.x,AccessoryEncIcon.transform.position.y,999f);
			AccessoryEncIcon.SetUVs(new Rect(0,0,1,1));
			AccessoryEncIcon.SetTexture(_UI_CS_ElementsInfo.Instance.EncIcon[ele-1]);
			
		}else{
			
			AccessoryEncIcon.transform.position = new Vector3(AccessoryEncIcon.transform.position.x,AccessoryEncIcon.transform.position.y,-999);
			
		}
	}
	
	public void SetGemIcon(int idx, int ele){
		switch(idx){
		case 0:
				SetWeaponGemIcon(ele);
				break;
		case 1:
				SetArmorGemIcon(ele);
				break;
		case 2:
				SetAccessoryGemIcon(ele);
				break;
		default:
				break;
		}
	}
	
	public void SetWeaponGemIcon(int ele){
		if(0 != ele){
			
			WeaponGemIcon.transform.position = new Vector3(WeaponGemIcon.transform.position.x,WeaponGemIcon.transform.position.y,999f);
			WeaponGemIcon.SetUVs(new Rect(0,0,1,1));
			WeaponGemIcon.SetTexture(_UI_CS_ElementsInfo.Instance.GemIcon[ele-1]);
			
		}else{
			
			WeaponGemIcon.transform.position = new Vector3(WeaponGemIcon.transform.position.x,WeaponGemIcon.transform.position.y,-999);
			
		}
	}
	
	public void SetArmorGemIcon(int ele){
		if(0 != ele){
			
			ArmorGemIcon.transform.position = new Vector3(ArmorGemIcon.transform.position.x,ArmorGemIcon.transform.position.y,999f);
			ArmorGemIcon.SetUVs(new Rect(0,0,1,1));
			ArmorGemIcon.SetTexture(_UI_CS_ElementsInfo.Instance.GemIcon[ele-1]);
			
		}else{
			
			ArmorGemIcon.transform.position = new Vector3(ArmorGemIcon.transform.position.x,ArmorGemIcon.transform.position.y,-999);
			
		}
	}
	
	public void SetAccessoryGemIcon(int ele){
		if(0 != ele){
			
			AccessoryGemIcon.transform.position = new Vector3(AccessoryGemIcon.transform.position.x,AccessoryGemIcon.transform.position.y,999f);
			AccessoryGemIcon.SetUVs(new Rect(0,0,1,1));
			AccessoryGemIcon.SetTexture(_UI_CS_ElementsInfo.Instance.GemIcon[ele-1]);
			
		}else{
			
			AccessoryGemIcon.transform.position = new Vector3(AccessoryGemIcon.transform.position.x,AccessoryGemIcon.transform.position.y,-999);
			
		}
	}
	
/// ////////////////////////////////////////////////////////////////////////////////////////
	
	public void SetWeaponATK(string atk){
		WeaponATK.Text = atk;
	}
	
	public void SetArmorDef(string def){
		ArmorDef.Text = def;
	}
	
	
	public void SetAD_Val(int idx, string val){
		switch(idx){
		case 0:
				SetWeaponATK(val);
				break;
		case 1:
				SetArmorDef(val);
				break;
		default:
				break;
		}
	}
	
	public void SetWeaponSpeed(string speed){
		WeaponSpeed.Text = speed;
	}
	
	public void SetArmorSet(string Set){
		ArmorSet.Text = Set;
	}
	
	
	public void SetSS_Val(int idx, string val){
		switch(idx){
		case 0:
				SetWeaponSpeed(val);
				break;
		case 1:
				SetArmorSet(val);
				break;
		default:
				break;
		}
	}
	
	
	
	
}
