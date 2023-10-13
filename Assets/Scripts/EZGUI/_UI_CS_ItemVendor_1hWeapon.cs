using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class _UI_CS_ItemVendor_1hWeapon : MonoBehaviour {
	
	public  UIListItemContainer  				m_ItemContainer;
	public  List<_UI_CS_ItemVendorItem> 		m_1hWeaponItemList   = new List<_UI_CS_ItemVendorItem>();
	public  UIScrollList						m_List;
	private int 								m_1hWeaponCurrentIdx;
	private Rect 					 			m_rect;
	public  int 								m_count;
	
	public static _UI_CS_ItemVendor_1hWeapon Instance;
	
	void Awake()
	{
		Instance = this;
	}
	
	// Use this for initialization
	void Start () {
		m_1hWeaponCurrentIdx = 0;
		m_rect.width = 1;
		m_rect.height = 1;
		m_count = 0;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void AddElement(_UI_CS_ItemVendorItem element){
		_UI_CS_ItemVendorItem temp = new _UI_CS_ItemVendorItem();
		temp = element;
		m_1hWeaponItemList.Add(temp);
	}
	
	public void ClearList(){
		
		m_1hWeaponItemList.Clear();
		m_List.ClearList(true);
		
	}
	
	//初始化列表 
	public void InitItemList(){
		int n,m;
		
		m_count = m_1hWeaponItemList.Count;

		n =	m_count/5;
		m = m_count%5;
		if(m > 0)
			n++;
		
		for(int j =0;j<n;j++){	
			if(m > 0){
				if(j+1 == n){
					Add1hWeaponItemListChild(m);
				}else{
					Add1hWeaponItemListChild(5);
				}	
			}else{
				Add1hWeaponItemListChild(5);
			}
		}
	}
	
	public void Add1hWeaponItemListChild(int childCount){
		UIListItemContainer item;
		item = (UIListItemContainer)m_List.CreateItem((GameObject)m_ItemContainer.gameObject);
		//Reset after manipulations
		m_List.clipContents 	= true;
		m_List.clipWhenMoving 	= true;
		Calculate();	 
		for(int i = 0;i<5;i++){
			if(childCount > i){
				item.transform.GetComponent<_UI_CS_ItemVendorRawItemCtrl>().item[i].transform.GetComponent<_UI_CS_ItemVendorItemEx>().isRareShopItem = false;
				item.transform.GetComponent<_UI_CS_ItemVendorRawItemCtrl>().item[i].transform.GetComponent<_UI_CS_ItemVendorItemEx>().m_ItemVendorInfo = m_1hWeaponItemList[m_1hWeaponCurrentIdx];
				item.transform.GetComponent<_UI_CS_ItemVendorRawItemCtrl>().item[i].transform.GetComponent<_UI_CS_ItemVendorItemEx>().m_ListID = m_1hWeaponCurrentIdx;
				item.transform.GetComponent<_UI_CS_ItemVendorRawItemCtrl>().item[i].m_ValText.Text = m_1hWeaponItemList[m_1hWeaponCurrentIdx].m_Val.ToString();
				if(m_1hWeaponItemList[m_1hWeaponCurrentIdx].m_count != -1){
					item.transform.GetComponent<_UI_CS_ItemVendorRawItemCtrl>().item[i].count.Text = m_1hWeaponItemList[m_1hWeaponCurrentIdx].m_count.ToString();
				}else{
					item.transform.GetComponent<_UI_CS_ItemVendorRawItemCtrl>().item[i].count.Text = "";
					item.transform.GetComponent<_UI_CS_ItemVendorRawItemCtrl>().item[i].countBg.gameObject.layer = LayerMask.NameToLayer("Default");
				}
				if(m_1hWeaponItemList[m_1hWeaponCurrentIdx].moneyType == 1) {
					item.transform.GetComponent<_UI_CS_ItemVendorRawItemCtrl>().item[i].piceBgK.gameObject.layer = LayerMask.NameToLayer("EZGUI_CanTouch");
					item.transform.GetComponent<_UI_CS_ItemVendorRawItemCtrl>().item[i].piceBgC.gameObject.layer = LayerMask.NameToLayer("Default");
				}else {
					item.transform.GetComponent<_UI_CS_ItemVendorRawItemCtrl>().item[i].piceBgK.gameObject.layer = LayerMask.NameToLayer("Default");
					item.transform.GetComponent<_UI_CS_ItemVendorRawItemCtrl>().item[i].piceBgC.gameObject.layer = LayerMask.NameToLayer("EZGUI_CanTouch");
				}
				item.transform.GetComponent<_UI_CS_ItemVendorRawItemCtrl>().item[i].m_ItemIcon.SetUVs(m_rect);
				ItemPrefabs.Instance.GetItemIcon(
				                                       item.transform.GetComponent<_UI_CS_ItemVendorRawItemCtrl>().item[i].m_ItemVendorInfo.m_ID,                                                                                
				                                       item.transform.GetComponent<_UI_CS_ItemVendorRawItemCtrl>().item[i].m_ItemVendorInfo.m_type,
				                                       item.transform.GetComponent<_UI_CS_ItemVendorRawItemCtrl>().item[i].m_ItemVendorInfo.m_iconID,
													   item.transform.GetComponent<_UI_CS_ItemVendorRawItemCtrl>().item[i].m_ItemIcon
												);
				float itemVal = 0;
				itemVal = (m_1hWeaponItemList[m_1hWeaponCurrentIdx].info.info_gemVal + m_1hWeaponItemList[m_1hWeaponCurrentIdx].info.info_encVal + m_1hWeaponItemList[m_1hWeaponCurrentIdx].info.info_eleVal);
				_UI_Color.Instance.SetNameColor(itemVal,item.transform.GetComponent<_UI_CS_ItemVendorRawItemCtrl>().item[i].m_LevelBgIcon);	
				m_1hWeaponCurrentIdx++;
				if(m_1hWeaponCurrentIdx >=  m_1hWeaponItemList.Count){
					m_1hWeaponCurrentIdx = 0;
				}
			}else{
				item.transform.GetComponent<_UI_CS_ItemVendorRawItemCtrl>()
					.item[i].transform.position = new Vector3(item.transform.GetComponent<_UI_CS_ItemVendorRawItemCtrl>().item[i].transform.position.x,
					                                       item.transform.GetComponent<_UI_CS_ItemVendorRawItemCtrl>().item[i].transform.position.y,20f);
			}
		}
	}
	
	public void Calculate()
	{
		if (m_List != null && m_List.slider != null)
        {
            // Ask scroll list to position items
            m_List.PositionItems();

            // Var to hold new knob size
            Vector2 newKnobSize;

            // Determine the new knob size as a percentage of the size of the viewable area
            // If the content is smaller than the viewable size then we won't show a knob
            if (m_List.ContentExtents > m_List.viewableArea.y)
            {
                float ratio = m_List.ContentExtents / m_List.viewableArea.y;
                newKnobSize = new Vector2((m_List.viewableArea.y / ratio), m_List.slider.knobSize.y);
				m_List.slider.Hide(false);
            }
            else
            {
                newKnobSize = new Vector2(0f, 0f);
				m_List.slider.Hide(true);
            }

            // Get a handle to the knob so we can change it
            UIScrollKnob theKnob = m_List.slider.GetKnob();
            //Debug.Log(theKnob);
            // Set the knob size based on our previous calculation
            theKnob.SetSize(newKnobSize.x, newKnobSize.y);

            // Now we need to make sure the knob doesn't go past the ends of the scrollview window size
            m_List.slider.stopKnobFromEdge = newKnobSize.x / 2;
            //Vector3 newStartPos = m_IngameMenu_AbilitiesTempList.slider.CalcKnobStartPos();
			Vector3 newStartPos = m_List.slider.CalcKnobStartPos();
            theKnob.SetStartPos(newStartPos);
            theKnob.SetMaxScroll(m_List.slider.width - (m_List.slider.stopKnobFromEdge * 2f));

            // Make sure the new text is scrolled to the top of the viewable area
            m_List.ScrollListTo(0f);
            // Added by me.
            theKnob.SetPosition(0f);
        }
	}
	
	
}
