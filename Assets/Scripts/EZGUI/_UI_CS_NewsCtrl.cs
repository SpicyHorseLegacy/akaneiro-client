using UnityEngine;
using System.Collections;

public class _UI_CS_NewsCtrl : MonoBehaviour {
	
	//Instance
	public static _UI_CS_NewsCtrl Instance = null;
	
	public string [] newsItems;
	
	public  UIScrollList						m_List;
	
	public  UIListItemContainer  				m_ItemContainer;
	
	
	void Awake(){
		
		Instance = this;
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void InitNewsInfo(){
		
		m_List.ClearList(false);
		
		for(int i = 0; i < newsItems.Length;i++){
			
			AddListChild(newsItems[i]);
			
		}
		
	}
	
	public void AddListChild(string info){

		UIListItemContainer item;
		item = (UIListItemContainer)m_List.CreateItem((GameObject)m_ItemContainer.gameObject);
		
		m_List.clipContents 	= true;
		m_List.clipWhenMoving 	= true;
		
		Calculate();
		
		item.GetComponent<_UI_CS_NewsListItemCtrl>().item[0].GetComponent<_UI_CS_NewsItem>().m_InfoText.Text = info;

	}
	
	public void Calculate()
	{
		if (m_List != null && m_List.slider != null){

            m_List.PositionItems();

            Vector2 newKnobSize;

            if (m_List.ContentExtents > m_List.viewableArea.y){
                float ratio = m_List.ContentExtents / m_List.viewableArea.y;
                newKnobSize = new Vector2((m_List.viewableArea.y / ratio), m_List.slider.knobSize.y);
				m_List.slider.Hide(false);
            }
            else{
                newKnobSize = new Vector2(0f, 0f);
				m_List.slider.Hide(true);
            }

            UIScrollKnob theKnob = m_List.slider.GetKnob();
            theKnob.SetSize(newKnobSize.x, newKnobSize.y);
            m_List.slider.stopKnobFromEdge = newKnobSize.x / 2;
			Vector3 newStartPos = m_List.slider.CalcKnobStartPos();
            theKnob.SetStartPos(newStartPos);
            theKnob.SetMaxScroll(m_List.slider.width - (m_List.slider.stopKnobFromEdge * 2f));

            m_List.ScrollListTo(0f);
            theKnob.SetPosition(0f);
        }
	}
}
