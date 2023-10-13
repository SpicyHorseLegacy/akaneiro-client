using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class _UI_CS_AinuElder_Mail : MonoBehaviour {
	
	public  UIListItemContainer  				m_ItemContainer;
	public  List<_UI_CS_AinuElderItem> 			m_ItemList   = new List<_UI_CS_AinuElderItem>();
	public  UIScrollList						m_List;
	private int 								m_CurrentIdx;
	private Rect 					 			m_rect;
	public  int 								m_count;
	
	public static _UI_CS_AinuElder_Mail Instance;
	
	void Awake()
	{
		Instance = this;
	}
	
	// Use this for initialization
	void Start () {
		m_CurrentIdx = 0;
		m_rect.width = 1;
		m_rect.height = 1;
		m_count = 0;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void AddElement(_UI_CS_AinuElderItem element){
		_UI_CS_AinuElderItem temp = new _UI_CS_AinuElderItem();
		temp = element;
		m_ItemList.Add(temp);
	}
	
	//初始化列表 
	public void InitItemList(){
		m_count = m_ItemList.Count;
		
		for(int j =0;j<m_count;j++){	
			AddItemListChild(1);
		}
	}
	
	public void AddItemListChild(int childCount){
		
		UIListItemContainer item;

		item = (UIListItemContainer)m_List.CreateItem((GameObject)m_ItemContainer.gameObject);
		//Reset after manipulations
		m_List.clipContents 	= true;
		m_List.clipWhenMoving 	= true;
		Calculate();
		 
		for(int i = 0;i<3;i++){
			if(childCount > i){
				
				item.transform.GetComponent<_UI_CS_AinuElderRawItemCtrl>()
				.item[i].transform.GetComponent<_UI_CS_AinuElderItemEx>()
				.m_Info = m_ItemList[m_CurrentIdx];
				
				item.transform.GetComponent<_UI_CS_AinuElderRawItemCtrl>()
				.item[i].transform.GetComponent<_UI_CS_AinuElderItemEx>()
				.m_ListID = m_CurrentIdx;
				
				item.transform.GetComponent<_UI_CS_AinuElderRawItemCtrl>().item[i].m_NameText.Text = m_ItemList[m_CurrentIdx].m_name;
				item.transform.GetComponent<_UI_CS_AinuElderRawItemCtrl>().item[i].m_DataText.Text = m_ItemList[m_CurrentIdx].m_time;
				item.transform.GetComponent<_UI_CS_AinuElderRawItemCtrl>().item[i].m_InfoText.Text = m_ItemList[m_CurrentIdx].m_Info;
				

//				item.transform.GetComponent<_UI_CS_SpiritHelperItem>().item[i].m_iconButton.SetUVs(m_rect);
//				item.transform.GetComponent<_UI_CS_SpiritHelperItem>().item[i].m_iconButton.SetTexture(
//			    _UI_CS_Resource.Instance.m_AccomplishmentIcon[m_SHItemList[m_CurrentIdx].m_iconID]);

				m_CurrentIdx++;
				if(m_CurrentIdx >=  m_ItemList.Count){
					m_CurrentIdx = 0;
					//LogManager.Log_Debug("m_CurrentIdx >=  m_ItemList.Count");
				}

			}else{
//				item.transform.GetComponent<_UI_CS_AinuElderRawItemCtrl>()
//					.item[i].transform.position = new Vector3(item.transform.GetComponent<_UI_CS_AinuElderRawItemCtrl>().item[i].transform.position.x,
//					                                       item.transform.GetComponent<_UI_CS_AinuElderRawItemCtrl>().item[i].transform.position.y,
//					                                        20f);
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
