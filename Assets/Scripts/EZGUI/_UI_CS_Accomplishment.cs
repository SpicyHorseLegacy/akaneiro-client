using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class _UI_CS_Accomplishment : MonoBehaviour {
	
	public  UIListItemContainer  				m_ItemContainer;
	public  List<_UI_CS_AccomplishmentItem> 	m_AccItemList   = new List<_UI_CS_AccomplishmentItem>();
	public  UIScrollList						m_List;
	private int 								m_AccomplishmentCurrentIdx;
	private Rect 					 			m_rect;
	public  int 								m_count;
	
	public static _UI_CS_Accomplishment Instance;
	
	void Awake()
	{
		Instance = this;
	}
	
	// Use this for initialization
	void Start () {
		m_AccomplishmentCurrentIdx = 0;
		m_rect.width = 1;
		m_rect.height = 1;
		m_count = 0;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void AddAccomplishmentListElement(_UI_CS_AccomplishmentItem element){
		_UI_CS_AccomplishmentItem temp = new _UI_CS_AccomplishmentItem();
		temp = element;
		m_AccItemList.Add(temp);
	}
	
	//初始化列表 
	public void InitAccomplishment(){
		int n,m;
		
		m_count = m_AccItemList.Count;

		n =	m_count/2;
		m = m_count%2;
		if(m > 0)
			n++;
		
		for(int j =0;j<n;j++){	
			if(m > 0){
				if(j+1 == n){
					AddAccomplishmentListChild(1);
				}else{
					AddAccomplishmentListChild(2);
				}	
			}else{
				AddAccomplishmentListChild(2);
			}
		}
		
	}
	
	public void AddAccomplishmentListChild(int childCount){
		
		UIListItemContainer item;

		item = (UIListItemContainer)m_List.CreateItem((GameObject)m_ItemContainer.gameObject);
		//Reset after manipulations
		m_List.clipContents 	= true;
		m_List.clipWhenMoving 	= true;
		Calculate();
		 
		for(int i = 0;i<2;i++){
			if(childCount > i){
				item.transform.GetComponent<_UI_CS_AccRawItemCtrl>()
				.item[i].transform.GetComponent<_UI_CS_AccomplishmentItemEx>()
				.m_accInfo = m_AccItemList[m_AccomplishmentCurrentIdx];
				item.transform.GetComponent<_UI_CS_AccRawItemCtrl>()
				.item[i].transform.GetComponent<_UI_CS_AccomplishmentItemEx>()
				.m_ListID = m_AccomplishmentCurrentIdx;
				item.transform.GetComponent<_UI_CS_AccRawItemCtrl>().item[i].m_iconButton.SetUVs(m_rect);
//				item.transform.GetComponent<_UI_CS_AccRawItemCtrl>().item[i].m_iconButton.SetTexture(
//			    _UI_CS_Resource.Instance.m_AccomplishmentIcon[m_AccItemList[m_AccomplishmentCurrentIdx].m_iconID]);
				
				item.transform.GetComponent<_UI_CS_AccRawItemCtrl>().item[i].m_ProgressBar.Value = m_AccItemList[m_AccomplishmentCurrentIdx].m_value;
				item.transform.GetComponent<_UI_CS_AccRawItemCtrl>().item[i].m_name.Text = m_AccItemList[m_AccomplishmentCurrentIdx].m_name;
				item.transform.GetComponent<_UI_CS_AccRawItemCtrl>().item[i].m_info.Text = m_AccItemList[m_AccomplishmentCurrentIdx].m_details;
				item.transform.GetComponent<_UI_CS_AccRawItemCtrl>().item[i].m_ksVal.Text = m_AccItemList[m_AccomplishmentCurrentIdx].m_KsVal.ToString();
				
				
				m_AccomplishmentCurrentIdx++;
				if(m_AccomplishmentCurrentIdx >=  m_AccItemList.Count){
					m_AccomplishmentCurrentIdx = 0;
					//LogManager.Log_Debug("m_AccomplishmentCurrentIdx >=  m_AccomplishmentList.Count");
				}

			}else{
				item.transform.GetComponent<_UI_CS_AccRawItemCtrl>()
					.item[i].transform.position = new Vector3(item.transform.GetComponent<_UI_CS_AccRawItemCtrl>().item[i].transform.position.x,
					                                       item.transform.GetComponent<_UI_CS_AccRawItemCtrl>().item[i].transform.position.y,
					                                        20f);
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
