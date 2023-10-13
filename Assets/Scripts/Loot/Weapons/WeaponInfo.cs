using UnityEngine;
using System.Collections;

public class WeaponInfo {
	
	static public WeaponInfo Instance;
	
	//type name
	public string[] WeaponName_List = {"Axe","Katana","Club","Mace","Nodachi","Great Axe","Great Maul"};
	
	//damage min and max
	public int[] DamageMinList = {4,10,6,14,20,10,26};
	public int[] DamageMaxList = {20,16,10,16,34,40,32};
	
	//Attack speed description
	public string[] AttackSpeedDescList = {"Fast","Fast","Very Fast","Normal","Slow","Slow","Very Slow"};
	
	//attack range and angle
	public float[] AttackRangeList = {1.5f,1.5f,1.5f,1.5f,2f,2f,2f};
	public int[] AttackAngleList = {60,60,60,60,90,90,90};
	
	//prefix1
	public string[] Prefix1_Name_List = {"Decrepit","Fragile","Cracking","Worn","Blank",
										"Polished","Shining","Sterling","Gleaming","Resplendent",
										"Glimmering","Glowing","Bright","Luminescent","Blazing",
										"Eerie","Enchanted","Hypnotizing","Wicked","Vorpal"};
	
	public float[] Prefix1_Modifier_List={0.5f,0.6f,0.7f,0.8f,1f,1.1f,1.2f,1.3f,1.4f,1.6f,1.7f,1.8f,1.9f,2f,2.2f,2.3f,2.4f,2.5f,2.6f,3f};
	
	//prefix2
	public string[] Prefix2_Name_List = {"Angry","Furious","Rage Driven","Awakened","Knowing",
										"Sentient","Thirsty","Vampiric","All Consuming","Quick",
										"Swift","Mercurial","Scary","Frightening","Horrifying",
										"Staggering","Stunning","Paralyzing","Scaling"};
	
	public float[] Prefix2_Chance = {13.20f,2.55f,1.00f,13.20f,2.55f,0.90f,13.20f,2.55f,0.90f,13.20f,2.55f,0.90f,13.20f,2.55f,0.90f,13.20f,2.55f,0.90f};

	public float[] Prefix2_Modifier_1_List={0.05f,0.1f,0.2f,0.05f,0.1f,0.2f,0.05f,0.1f,0.2f,0.05f,0.1f,0.2f,2f,3f,5f,1f,2f,2.5f};	
	
	public void Start()
	{
		Instance = this;
	}

}
