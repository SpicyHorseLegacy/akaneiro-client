using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class RareShopList : MonoBehaviour {
	
	public static RareShopList 					Instance;
	public  UIListItemContainer  				container;
	public  List<_UI_CS_ItemVendorItem> 		infoList   = new List<_UI_CS_ItemVendorItem>();
	public  UIScrollList						list;
	private int 								currentIdx;
	private Rect 					 			rect;
	public  int 								count;
	
	void Awake(){
		Instance = this;
	}
	
	// Use this for initialization
	void Start () {
		currentIdx  = 0;
		count 		= 0;
		rect.width  = 1;
		rect.height = 1;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void AddElement(_UI_CS_ItemVendorItem element){
		_UI_CS_ItemVendorItem temp = new _UI_CS_ItemVendorItem();
		temp = element;
		infoList.Add(temp);
	}
	
	public void ClearList(){
		infoList.Clear();
		list.ClearList(true);
	}
	
	public void AddListChild(int childCount){
		UIListItemContainer item;
		item = (UIListItemContainer)list.CreateItem((GameObject)container.gameObject);
		//Reset after manipulations
		list.clipContents 	= true;
		list.clipWhenMoving 	= true;
		transform.GetComponent<CalculateSlider>().Calculate();
		for(int i = 0;i<5;i++){
			if(childCount > i){
				item.transform.GetComponent<_UI_CS_ItemVendorRawItemCtrl>().item[i].transform.GetComponent<_UI_CS_ItemVendorItemEx>().isRareShopItem = true;
				item.transform.GetComponent<_UI_CS_ItemVendorRawItemCtrl>().item[i].transform.GetComponent<_UI_CS_ItemVendorItemEx>().m_ItemVendorInfo = infoList[currentIdx];
				item.transform.GetComponent<_UI_CS_ItemVendorRawItemCtrl>().item[i].transform.GetComponent<_UI_CS_ItemVendorItemEx>().m_ListID = currentIdx;
				item.transform.GetComponent<_UI_CS_ItemVendorRawItemCtrl>().item[i].m_ValText.Text = infoList[currentIdx].m_Val.ToString();
				if(infoList[currentIdx].m_count != -1){
					item.transform.GetComponent<_UI_CS_ItemVendorRawItemCtrl>().item[i].count.Text = infoList[currentIdx].m_count.ToString();
				}else{
					item.transform.GetComponent<_UI_CS_ItemVendorRawItemCtrl>().item[i].count.Text = "";
					item.transform.GetComponent<_UI_CS_ItemVendorRawItemCtrl>().item[i].countBg.Hide(true);
				}
				if(infoList[currentIdx].m_shopItem.moneyType == 1) {
					item.transform.GetComponent<_UI_CS_ItemVendorRawItemCtrl>().item[i].piceBgK.gameObject.layer = LayerMask.NameToLayer("EZGUI_CanTouch");
					item.transform.GetComponent<_UI_CS_ItemVendorRawItemCtrl>().item[i].piceBgC.gameObject.layer = LayerMask.NameToLayer("Default");
				}else {
					item.transform.GetComponent<_UI_CS_ItemVendorRawItemCtrl>().item[i].piceBgK.gameObject.layer = LayerMask.NameToLayer("Default");
					item.transform.GetComponent<_UI_CS_ItemVendorRawItemCtrl>().item[i].piceBgC.gameObject.layer = LayerMask.NameToLayer("EZGUI_CanTouch");
				}
				item.transform.GetComponent<_UI_CS_ItemVendorRawItemCtrl>().item[i].m_ItemIcon.SetUVs(rect);
				ItemPrefabs.Instance.GetItemIcon(infoList[currentIdx].m_ID,infoList[currentIdx].m_type,infoList[currentIdx].m_iconID,item.transform.GetComponent<_UI_CS_ItemVendorRawItemCtrl>().item[i].m_ItemIcon);
				float itemVal = 0;
				itemVal = (infoList[currentIdx].info.info_gemVal + infoList[currentIdx].info.info_encVal + infoList[currentIdx].info.info_eleVal);
				_UI_Color.Instance.SetNameColor(itemVal,item.transform.GetComponent<_UI_CS_ItemVendorRawItemCtrl>().item[i].m_LevelBgIcon);
				currentIdx++;
				if(currentIdx >=  infoList.Count){
					currentIdx = 0;
				}
			}else{
				item.transform.GetComponent<_UI_CS_ItemVendorRawItemCtrl>().item[i].transform.position = new Vector3(item.transform.GetComponent<_UI_CS_ItemVendorRawItemCtrl>().item[i].transform.position.x,
					                                       item.transform.GetComponent<_UI_CS_ItemVendorRawItemCtrl>().item[i].transform.position.y,20f);
			}
		}
	}
	
	public void InitItemList(){
		int n,m;
		count = infoList.Count;
		n =	count/5;
		m = count%5;
		if(m > 0)
			n++;
		for(int j =0;j<n;j++){	
			if(m > 0){
				if(j+1 == n){
					AddListChild(m);
				}else{
					AddListChild(5);
				}	
			}else{
				AddListChild(5);
			}
		}
	}
	
	
	
}
