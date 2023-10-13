using UnityEngine;
using System.Collections;

public class ArmorInfo  {
	
	static public ArmorInfo Instance;
	
	//type name
	public string[] ArmorName_List = {"Helm","Cloak","Breastplate","Breeches"};
	
	//armor amount
	public int[] ArmorAmount_List = {4,4,8,4};
	
	//prefix1
	public string[] Prefix1_Name_List = {"Broken","Cracking","Shabby","Used","Blank","Tough","Sturdy"};
	
	public float[] Prefix1_Modifier_List= {0.5f,0.6f,0.7f,0.8f,1f,1.1f,1.2f,1.3f,1.4f,1.6f,1.7f,1.8f,1.9f,2f,2.2f,2.3f,2.4f,2.5f,2.6f,3f};
	
	//prefix2
	public string[] Prefix2_Name_List = {"Thorny","Spiked","Razor Sharp","Reinforced","Augmented","Fortified","Faint","Blurry","Obfuscated","Scaling" };

	public float[] Prefix2_Modifier_1_List = {1f,3f,5f,0.05f,0.1f,0.2f,1f,3f,5f};
	
	public float[] Prefix2_Chance = {25f,7f,1f,25f,7f,1f,25f,7f,1f,0f};
	
	//color
	public float[] Prefix_ColorBonus_List = {0.05f,0.02f,0.02f,0.02f,0.01f,0.01f,0.01f,0.01f,0.02f,};
	
	public void Start()
	{
		Instance = this;
	}	
}
