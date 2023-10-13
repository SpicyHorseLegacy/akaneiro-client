using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class _UI_CS_MissionTitlePic : MonoBehaviour {
	
	public  UIListItemContainer  				m_ItemContainer;
	public  List<_UI_CS_MissionTitlePicItem> 	m_ItemList   = new List<_UI_CS_MissionTitlePicItem>();
	public  UIScrollList						m_List;
	private int 								m_CurrentIdx;
	private Rect 					 			m_rect;
	public  int 								m_count;
	
	public static _UI_CS_MissionTitlePic Instance;
	
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
	
	public void AddElement(_UI_CS_MissionTitlePicItem element){
		_UI_CS_MissionTitlePicItem temp = new _UI_CS_MissionTitlePicItem();
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
		 
		//for(int i = 0;i<1;i++){
		if(m_ItemList[m_CurrentIdx].IsUse && 0 != m_CurrentIdx){
			
			item.transform.GetComponent<_UI_CS_MissionTitlePicRawItemCtrl>()
			.item[0].transform.GetComponent<_UI_CS_MissionTitlePicItemEx>()
			.m_ItemInfo = m_ItemList[m_CurrentIdx];
			
			item.transform.GetComponent<_UI_CS_MissionTitlePicRawItemCtrl>()
			.item[0].transform.GetComponent<_UI_CS_MissionTitlePicItemEx>()
			.m_ListID = m_CurrentIdx;
			
			
			
			item.transform.GetComponent<_UI_CS_MissionTitlePicRawItemCtrl>().item[0].m_NameText.Text = m_ItemList[m_CurrentIdx].m_name.ToString();

			//item.transform.GetComponent<_UI_CS_MissionTitlePicRawItemCtrl>().item[0].m_BgIconButton.SetUVs(new Rect(0,0,1,1));
			item.transform.GetComponent<_UI_CS_MissionTitlePicRawItemCtrl>().item[0].m_BgIconButton.SetTexture(m_ItemList[m_CurrentIdx].Icon);

			

		}else{
				item.transform.GetComponent<_UI_CS_MissionTitlePicRawItemCtrl>()
				.item[0].transform.position = new Vector3(item.transform.GetComponent<_UI_CS_MissionTitlePicRawItemCtrl>().item[0].transform.position.x,
					                                       item.transform.GetComponent<_UI_CS_MissionTitlePicRawItemCtrl>().item[0].transform.position.y,
					                                        20f);
		}
		
		
	    m_CurrentIdx++;
		if(m_CurrentIdx >=  m_ItemList.Count){
			m_CurrentIdx = 0;
			//LogManager.Log_Debug("m_CurrentIdx >=  m_ItemList.Count");
		}
			
		//}

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
