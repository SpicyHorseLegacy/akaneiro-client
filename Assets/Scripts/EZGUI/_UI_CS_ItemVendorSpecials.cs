using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class _UI_CS_ItemVendorSpecials : MonoBehaviour {

	public static _UI_CS_ItemVendorSpecials Instance;
	
	//idx 0 big ,1~8 small
	public _UI_CS_ItemVendorItemEx [] SpecialsItem;
	
//	public Texture2D bigDelIcon;
//	public Texture2D smallDelIcon;
	
	public  List<_UI_CS_ItemVendorItem> 		SpecialsItemList   = new List<_UI_CS_ItemVendorItem>();
	private Rect 					 			rect;
	
	void Awake()
	{
		Instance = this;
	}
	
	// Use this for initialization
	void Start () {
		rect.width = 1;
		rect.height = 1;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void InitItem(){
		for(int i = 0;i < 9;i++){
			if(i < (SpecialsItemList.Count )){		
				SpecialsItem[i].transform.position = new Vector3(SpecialsItem[i].transform.position.x,SpecialsItem[i].transform.position.y,-3);	
				SpecialsItem[i].m_ItemVendorInfo = SpecialsItemList[i];	
				SpecialsItem[i].m_ListID = i;	
				SpecialsItem[i].m_ValText.Text = SpecialsItemList[i].m_Val.ToString();
				SpecialsItem[i].m_ItemIcon.SetUVs(rect);
				ItemPrefabs.Instance.GetItemIcon(
				                                      SpecialsItem[i].m_ItemVendorInfo.m_ID,                                                                                
				                                      SpecialsItem[i].m_ItemVendorInfo.m_type,
				                                      SpecialsItem[i].m_ItemVendorInfo.m_iconID,
				                                      SpecialsItem[i].m_ItemIcon                                 	
												);
				float itemVal = 0;
				itemVal = (SpecialsItemList[i].info.info_gemVal + SpecialsItemList[i].info.info_encVal + SpecialsItemList[i].info.info_eleVal);
				_UI_Color.Instance.SetNameColor(itemVal,SpecialsItem[i].m_LevelBgIcon);	
				if(null != SpecialsItem[i].m_OffText && null != SpecialsItemList[i]){	
					SpecialsItem[i].m_OffText.Text = (SpecialsItemList[i].m_shopItem.isDiscount * 10).ToString();	
				}
			}else{
				SpecialsItem[i].transform.position = new Vector3(SpecialsItem[i].transform.position.x,SpecialsItem[i].transform.position.y,1000);
			}
			SpecialsItem[i].GetComponent<_UI_CS_ItemVendorItemEx>().m_ItemIcon.gameObject.layer = LayerMask.NameToLayer("EZGUI_CanTouch");
		}
	}
	
	public void ClearList(){
		
		SpecialsItemList.Clear();
		
		for(int i = 0; i < 9;i++){
			
			SpecialsItem[i].transform.position = new Vector3(SpecialsItem[i].transform.position.x,SpecialsItem[i].transform.position.y,1000);
		
		}
	}
	
	
}
