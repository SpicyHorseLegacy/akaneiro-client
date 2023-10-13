using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class _UI_CS_MissionList : MonoBehaviour {
	
	public  UIListItemContainer  				m_ItemContainer;
	public  List<_UI_CS_MissionListItem> 		m_ItemList   = new List<_UI_CS_MissionListItem>();
	public  UIScrollList						m_List;
	public int 									m_CurrentIdx;
	private Rect 					 			m_rect;
	public  int 								m_count;
	

	
	public static _UI_CS_MissionList Instance;
	
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
	
	public void AddElement(_UI_CS_MissionListItem element){
		_UI_CS_MissionListItem temp = new _UI_CS_MissionListItem();
		temp = element;
		m_ItemList.Add(temp);
	}
	
	//初始化列表 
	public void InitItemList(){
		
		int n,m;
		
		m_count = m_ItemList.Count;

		n =	m_count/3;
		m = m_count%3;
		if(m > 0)
			n++;
		
		for(int j =0;j<n;j++){	
			if(m > 0){
				if(j+1 == n){
					AddItemListChild(m);
				}else{
					AddItemListChild(3);
				}	
			}else{
				AddItemListChild(3);
			}
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
			if(childCount > i&&m_ItemList[m_CurrentIdx].m_isUse && 2 < m_CurrentIdx){
				
				item.transform.GetComponent<_UI_CS_MissionListRawItemCtrl>()
				.item[i].transform.GetComponent<_UI_CS_MissionListItemEx>()
				.m_ItemInfo = m_ItemList[m_CurrentIdx];
				
				item.transform.GetComponent<_UI_CS_MissionListRawItemCtrl>()
				.item[i].transform.GetComponent<_UI_CS_MissionListItemEx>()
				.m_ListID = m_CurrentIdx;
				
				item.transform.GetComponent<_UI_CS_MissionListRawItemCtrl>().item[i].m_NameText.Text = m_ItemList[m_CurrentIdx].m_name.ToString();
				item.transform.GetComponent<_UI_CS_MissionListRawItemCtrl>().item[i].missionID       = m_ItemList[m_CurrentIdx].m_ID;
				item.transform.GetComponent<_UI_CS_MissionListRawItemCtrl>().item[i].m_SkText.Text = m_ItemList[m_CurrentIdx].m_Sk.ToString();
				item.transform.GetComponent<_UI_CS_MissionListRawItemCtrl>().item[i].m_XpText.Text = m_ItemList[m_CurrentIdx].m_Xp.ToString();
				item.transform.GetComponent<_UI_CS_MissionListRawItemCtrl>().item[i].levelID       = m_ItemList[m_CurrentIdx].m_levelID;
				
				item.transform.GetComponent<_UI_CS_MissionListRawItemCtrl>()
				.item[i].transform.GetComponent<_UI_CS_MissionListItemEx>()
				.mapName = m_ItemList[m_CurrentIdx].m_mapName;
			}else{
				item.transform.GetComponent<_UI_CS_MissionListRawItemCtrl>()
					.item[i].transform.position = new Vector3(item.transform.GetComponent<_UI_CS_MissionListRawItemCtrl>().item[i].transform.position.x,
					                                       item.transform.GetComponent<_UI_CS_MissionListRawItemCtrl>().item[i].transform.position.y,
					                                        20f);
			}
			
				
			m_CurrentIdx++;
				if(m_CurrentIdx >=  m_ItemList.Count){
					m_CurrentIdx = 0;
					//LogManager.Log_Debug("m_CurrentIdx >=  m_ItemList.Count");
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
            if (m_List.ContentExtents > m_List.viewableArea.x)
            {
                float ratio = m_List.ContentExtents / m_List.viewableArea.x;
                newKnobSize = new Vector2((m_List.viewableArea.x / ratio), m_List.slider.knobSize.y);
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
