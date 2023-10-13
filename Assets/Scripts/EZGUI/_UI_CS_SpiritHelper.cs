using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class _UI_CS_SpiritHelper : MonoBehaviour {
	
	public  UIListItemContainer  				m_ItemContainer;
	public  List<_UI_CS_SpiritHelperItem> 		m_SHItemList   = new List<_UI_CS_SpiritHelperItem>();
	public  UIScrollList						m_List;
	private int 								m_SpiritHelperCurrentIdx;
	private Rect 					 			m_rect;
	public  int 								m_count;
	public static _UI_CS_SpiritHelper Instance;
	
	void Awake(){
		Instance = this;
	}
	
	// Use this for initialization
	void Start () {
		m_SpiritHelperCurrentIdx = 0;
		m_rect.width = 1;
		m_rect.height = 1;
		m_count = 0;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void AddSpiritHelperListElement(_UI_CS_SpiritHelperItem element){
		_UI_CS_SpiritHelperItem temp = new _UI_CS_SpiritHelperItem();
		temp = element;
		m_SHItemList.Add(temp);
	}
	
	//初始化列表 
	public void InitSpiritHelper(){
		int n,m;
		m_count = m_SHItemList.Count;
		n =	m_count/3;
		m = m_count%3;
		if(m > 0)
			n++;	
		for(int j =0;j<n;j++){	
			if(m > 0){
				if(j+1 == n){
					AddSpiritHelperListChild(m);
				}else{
					AddSpiritHelperListChild(3);	
				}
			}else{
				AddSpiritHelperListChild(3);
			}
		}
	}
	
	public void AddSpiritHelperListChild(int childCount){
		UIListItemContainer item;
		item = (UIListItemContainer)m_List.CreateItem((GameObject)m_ItemContainer.gameObject);
		//Reset after manipulations
		m_List.clipContents 	= true;
		m_List.clipWhenMoving 	= true;
		Calculate();
		for(int i = 0;i<3;i++){
			if(childCount > i){
				item.transform.GetComponent<_UI_CS_SpiritHelperRawItemCtrl>().item[i].m_ShInfo = m_SHItemList[m_SpiritHelperCurrentIdx];
				item.transform.GetComponent<_UI_CS_SpiritHelperRawItemCtrl>().item[i].m_ListID = m_SpiritHelperCurrentIdx;
				item.transform.GetComponent<_UI_CS_SpiritHelperRawItemCtrl>().item[i].m_AtkIconButton.SetColor(Color.white);
				//to do: lock spirit
				item.transform.GetComponent<_UI_CS_SpiritHelperRawItemCtrl>().item[i].m_AtkIconButton.SetUVs(m_rect);
				if(_PlayerData.Instance.playerLevel >= m_SHItemList[m_SpiritHelperCurrentIdx].m_levelReq){
					item.transform.GetComponent<_UI_CS_SpiritHelperRawItemCtrl>().item[i].m_AtkIconButton.SetTexture(
				      _UI_CS_SpiritInfo.Instance.spirirtIcon[item.transform.GetComponent<_UI_CS_SpiritHelperRawItemCtrl>().item[i].m_ShInfo.m_iconID]
																													);
					item.transform.GetComponent<_UI_CS_SpiritHelperRawItemCtrl>().item[i].m_AtkIconButton.SetColor(_UI_Color.Instance.color3);
					//to do: can summon
					if(m_SHItemList[m_SpiritHelperCurrentIdx].m_IsSummoned){
						item.transform.GetComponent<_UI_CS_SpiritHelperRawItemCtrl>().item[i].m_Summoned.gameObject.layer = LayerMask.NameToLayer("EZGUI_CanTouch");
					}else{
						if(m_SHItemList[m_SpiritHelperCurrentIdx].m_IsFreeDay){
							item.transform.GetComponent<_UI_CS_SpiritHelperRawItemCtrl>().item[i].m_Summoned.gameObject.layer = LayerMask.NameToLayer("EZGUI_CanTouch");
						}else{
							item.transform.GetComponent<_UI_CS_SpiritHelperRawItemCtrl>().item[i].m_Summoned.gameObject.layer = LayerMask.NameToLayer("Default");
						}
					}	
				}else{
					item.transform.GetComponent<_UI_CS_SpiritHelperRawItemCtrl>().item[i].m_AtkIconButton.SetTexture( _UI_CS_SpiritInfo.Instance.spirirtIcon[7]);
					item.transform.GetComponent<_UI_CS_SpiritHelperRawItemCtrl>().item[i].m_Summoned.gameObject.layer = LayerMask.NameToLayer("Default");
				}
				m_SpiritHelperCurrentIdx++;
				if(m_SpiritHelperCurrentIdx >=  m_SHItemList.Count){
					m_SpiritHelperCurrentIdx = 0;
				}
			}else{
				item.transform.GetComponent<_UI_CS_SpiritHelperRawItemCtrl>()
					.item[i].transform.position = new Vector3(item.transform.GetComponent<_UI_CS_SpiritHelperRawItemCtrl>().item[i].transform.position.x,
					                                       item.transform.GetComponent<_UI_CS_SpiritHelperRawItemCtrl>().item[i].transform.position.y,
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
