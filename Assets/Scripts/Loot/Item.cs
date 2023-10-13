using UnityEngine;
using System.Collections;


public class Item : BaseExportNode 
{
	//Weapon Inventory Appearence
	public Texture2D Normal_State_IconBoy;
	public Texture2D Normal_State_IconGirl;
	
	[HideInInspector] public Transform Owner;
	[HideInInspector] public bool IsEquipted=false;
	
	[HideInInspector] public int ObjectID;
	public int TypeID = 0;
	public int PrefabID = 0;
	
	public enum EPC
	{
		PrefixColor_White,
		PrefixColor_Red,
		PrefixColor_Green,
		PrefixColor_Blue,
		PrefixColor_Yellow,
		PrefixColor_Pink,
		PrefixColor_Cyan,
		PrefixColor_Black,
		PrefixColor_Gold,
	}
	
	public enum EItem_Type
	{
		EItem_Weapon=0,
		EItem_Armor,
		EItem_Accessory,
		EItem_Consumable,
		EItem_Material,
		EItem_Quest,
	}
	
	public EItem_Type ItemType;
	
	[HideInInspector] public int Prefix1_Level=1;
	[HideInInspector] public string Prefix1_Name;
	[HideInInspector] public int Prefix2_Level;
	[HideInInspector] public float Prefix1_Modifier_Percent = 0.5f;
	[HideInInspector] public string Prefix2_Name;
	[HideInInspector] public string Prefix2_Description;
	[HideInInspector] public float Prefix2_Modifier_1;
	[HideInInspector] public EPC PrefixColor_Color;
	[HideInInspector] public string PrefixColor_Description;
	[HideInInspector] public float PrefixColor_Bonus;
	[HideInInspector] public int Suffix_Level; 
	[HideInInspector] public string Suffix_Name;
	[HideInInspector] public float Suffix_Modifier_1;
	[HideInInspector] public float Suffix_Modifier_2;
	[HideInInspector] public string Suffix_Description;
	
	public static Transform MouseItem;
	
	float LastTipTime = 0f;
	bool mbToggleTip = false;
	float TipPeriod = 0.5f;

    public SItemInfo ItemInfo;
	
	public _UI_CS_IngameToolTipEz itemTip;
	
	public virtual void Start()
	{
//		if(gridSize_W <= 0 || gridSize_H <= 0)
//			Debug.Log("Please setup the correct grid size of each Item Prefab");
	}

    public void SetOwner(Transform p)
	{
		Owner = p;
		IsEquipted=true;
		//transform.gameObject.layer = LayerMask.NameToLayer("Default");
	}
	
	public void ShowTip()
	{
//		if(Time.timeScale == 0 )
//			return;
//		if(InGameUIControl.Instance.bShowTip)
//		{
//			HideTip();
//			return;
//		}
//		
//		if(!mbToggleTip)
//		{
//			mbToggleTip = true;
//			LastTipTime = Time.realtimeSinceStartup;
//		}
//		if(Time.realtimeSinceStartup - LastTipTime < TipPeriod )
//		      return;
//		
//	    UIButton tipTransform = InGameUIControl.Instance.LootToolTip;
//		if( tipTransform == null)
//		   return;
//		
//		//if(transform == MouseItem)
//			//return;
//	  
//		float ranValue = 0.5f;
//		
//	    Vector3 TipScreenPos = Camera.main.camera.WorldToScreenPoint(new Vector3(transform.position.x , transform.position.y , transform.position.z));
//		
//		TipScreenPos.x = Input.mousePosition.x;
//		
//		TipScreenPos.y = Input.mousePosition.y;
//		     
//	    tipTransform.transform.position = InGameUIControl.Instance.EZGUICamera.camera.ScreenToWorldPoint(new Vector3(TipScreenPos.x, TipScreenPos.y, InGameUIControl.Instance.EZGUICamera.camera.nearClipPlane + ranValue));
//		
//		Vector3 temPos = tipTransform.transform.position;
//		
//		temPos.x -= 3.5f;
//		
//		temPos.y += 3.5f;
//		
//		Vector3 temPos2 = temPos;
//		
//		temPos2.x -= tipTransform.width/2f;
//		temPos2.y += tipTransform.height/2f;
//		
//		TipScreenPos = InGameUIControl.Instance.EZGUICamera.camera.WorldToScreenPoint(temPos2);
//		if(TipScreenPos.x > Screen.width || TipScreenPos.y > Screen.height )
//		{
//			if(TipScreenPos.x > Screen.width)
//			    TipScreenPos.x = Screen.width;
//			if( TipScreenPos.y > Screen.height)
//				TipScreenPos.y = Screen.height;
//			temPos2 = InGameUIControl.Instance.EZGUICamera.camera.ScreenToWorldPoint(TipScreenPos);
//			
//			temPos2.x += tipTransform.width/2f;
//		    temPos2.y -= tipTransform.height/2f;
//			
//			temPos = temPos2;
//			
//		}
//	
//		tipTransform.transform.position = temPos;
//		
//		tipTransform.Hide(false);
//		
//		MouseItem = transform;
			
	}
	
	public void HideTip()
	{
//
//		if(Time.timeScale == 0)
//			return;
//		
//	    mbToggleTip = false;
//		
//		UIButton tipTransform = InGameUIControl.Instance.LootToolTip;
//		if( tipTransform == null)
//		   return;
//		
//		//if( MouseItem == transform)
//		 tipTransform.Hide(true);
//		 
//		 //MouseItem = null;
//		
	}
	


}
