using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Abilitie{
	public string name;
	public string info;
	public string icon;
	public Abilitie(string name,string info,string icon){
		this.name = name;
		this.info = info;
		this.icon = icon;
	}
}

public class EquipmentData : DataBase{
	public Color equipmentColor;
	public string name;
	public int level;
	public string part;
	public EquipmentType type;
	public string equipmentIcon;
	public string armorOrAamageVal;
	public string speed;
	public List<Abilitie> abilities = new List<Abilitie>();
	public int sellValue;
	public int playerLevel;
	
}

public enum EquipmentType{
	weapon = 0,
	armor = 1,
};

public class EquipmentTipInfo : TooltipInfoBase {
	
	
	// Use this for initialization
	void Start () {
		//test();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	#region Test
	void test(){
		EquipmentData data = new EquipmentData();
		data.equipmentColor = Color.blue;
		data.name = "changdao";
		data.level = 11;
		data.part = "shuangshou";
		data.type = EquipmentType.weapon;
		data.equipmentIcon = null;
		data.armorOrAamageVal = "111";
		data.speed = "Fast";
		Abilitie a = new Abilitie("1","11",null);
		Abilitie b = new Abilitie("2","22",null);
		Abilitie c = new Abilitie("3","33",null);
		data.abilities.Add(a);
		data.abilities.Add(b);
		data.abilities.Add(c);
		data.sellValue = 123;
		data.playerLevel = 9;
		SetEquipmentTip(data);
	}
	#endregion
	#region Interface
	public void SetEquipmentTip(EquipmentData data){
		if(data==null){
			return;
		}
		SetBorderColor(data.equipmentColor);
		SetName(data.equipmentColor,data.name);
		SetLevel(data.level,data.playerLevel);
		SetPart(data.part);
		SetSpeed(data.speed);
		SetEquipmentIcon(data.equipmentIcon);
		SetArmorOrAamageVal(data.armorOrAamageVal);
		SetAbilities(data.abilities);
		SetSellValue(data.sellValue);
		SetVelBar(data.abilities.Count);
		if(data.type == EquipmentType.weapon){
			NGUITools.SetActive(speedTransform.gameObject,true);
		}
		else{
			NGUITools.SetActive(speedTransform.gameObject,false);
		}
	}
	
	private bool isCompare = false;
	public void setIsCompare(bool compare){
		isCompare = compare;
	}
	
	public bool getIsCompare(){
		return isCompare;
	}
	#endregion
	
	#region Local
	
	
	[SerializeField]
	private UISprite border;
	private void SetBorderColor(Color c){
		border.color = c;
	}
	
	[SerializeField]
	private UILabel name;
	private void SetName(Color c,string text){
		name.text = text;
		name.color = c;
	}
	
	[SerializeField]
	private UILabel level;
	private void SetLevel(int val,int playerLevel){
		if(playerLevel<val){
			level.text = string.Format("Requires Level {0}",val);
			level.color = Color.red;
		}
		else{
			level.text = string.Format("Level {0}",val);
			level.color = Color.white;
		}
		
	}
	
	[SerializeField]
	private UILabel part;
	private void SetPart(string text){
		part.text = text;
	}
	
	[SerializeField]
	private Transform speedTransform;
	
	[SerializeField]
	private UILabel speed;
	private void SetSpeed(string text){
		speed.text = text;
	}
	
	[SerializeField]
	private UISprite equipmentIcon;
	private void SetEquipmentIcon(string iconName){
		equipmentIcon.spriteName = iconName;
	}
	
	[SerializeField]
	private UILabel armorOrAamageVal;
	private void SetArmorOrAamageVal(string val){
		armorOrAamageVal.text = val;
	}
	
	[SerializeField] 
	private UILabel []abilitiesName;
	[SerializeField] 
	private UILabel []abilitiesInfo;
	[SerializeField]
	private UISprite []abilitiesIcon;
	private void SetAbilities(List<Abilitie> abilities){
		for(int i = 0 ; i<abilities.Count;i++){
			abilitiesName[i].text = abilities[i].name;
			abilitiesInfo[i].text = abilities[i].info;
			abilitiesIcon[i].spriteName = abilities[i].icon;
		}
	}
	
	[SerializeField]
	private UILabel sellValue;
	private void SetSellValue(int val){
		sellValue.text=val.ToString();
	}
	
	[SerializeField]
	private Transform abilities1;
	[SerializeField]
	private Transform abilities2;
	[SerializeField]
	private Transform abilities3;
	[SerializeField]
	private Transform endBar;
	private void SetVelBar(int num){
		switch(num){
		case 0:
			NGUITools.SetActive(abilities1.gameObject,false);
			NGUITools.SetActive(abilities2.gameObject,false);
			NGUITools.SetActive(abilities3.gameObject,false);
			endBar.position = abilities1.position;
			break;
		case 1:
			NGUITools.SetActive(abilities2.gameObject,false);
			NGUITools.SetActive(abilities3.gameObject,false);
			endBar.position = abilities2.position;
			break;
		case 2:
			NGUITools.SetActive(abilities3.gameObject,false);
			endBar.position = abilities3.position;
			break;		
		}
		
	}
	#region  Show Equipment Left&Right
	private Vector3 v_EquipmentLeft;
	private Vector3 v_EquipmentRight;
	
	public void SetEquipmentLeftVector3(Vector3 v){
		v_EquipmentLeft = v;
	}
	
	public void SetEquipmentRightVector3(Vector3 v){
		v_EquipmentRight = v;
	}
	
	/// <summary>
	/// Shows the left.这个函数还不完整，需要补充//
	/// </summary>/
	private void ShowLeft(){
		EquipmentLeft.localPosition = v_EquipmentLeft; 
	}
	private void ShowRight(){
		EquipmentRight.localPosition = v_EquipmentRight; 
	}
	
	private void HideLeft(){
		EquipmentLeft.localPosition = new Vector3(999f,999f,-2f);
	}
	private void HideRight(){
		EquipmentRight.localPosition = new Vector3(999f,999f,-2f); 
	}
	#endregion
	#endregion
	
	#region override
	[SerializeField]
	private Transform EquipmentLeft;
	[SerializeField]
	private Transform EquipmentRight;
	
	
	
	public override void Show(Vector3 v,DataBase data){
		SetEquipmentTip((EquipmentData)data);
        gameObject.transform.position = v; 
		ShowLeft();
		ShowRight();
	}
	
	public override void Hide(){
		gameObject.transform.position = new Vector3(999f,999f,-2f); 
		HideLeft();
		HideRight();
	}
	#endregion
}
