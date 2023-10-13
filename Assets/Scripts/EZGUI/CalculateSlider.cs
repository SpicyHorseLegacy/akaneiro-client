using UnityEngine;
using System.Collections;

public class CalculateSlider : MonoBehaviour {
	
	public UIScrollList			m_List;
	public bool					isTop = true;		
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void Calculate(){
		if (m_List != null && m_List.slider != null){
            // Ask scroll list to position items
            m_List.PositionItems();
            // Var to hold new knob size
            Vector2 newKnobSize;
            // Determine the new knob size as a percentage of the size of the viewable area
            // If the content is smaller than the viewable size then we won't show a knob
            if (m_List.ContentExtents > m_List.viewableArea.y){
                float ratio = m_List.ContentExtents / m_List.viewableArea.y;
                newKnobSize = new Vector2((m_List.viewableArea.y / ratio), m_List.slider.knobSize.y);
				m_List.slider.Hide(false);
            }
            else{
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
           
           
			if(!isTop){
				 m_List.ScrollListTo(1f);
				// Added by me.
           		 theKnob.SetPosition(1f);
			}else{
				 m_List.ScrollListTo(0f);
				 theKnob.SetPosition(0f);
			}
			
        }
	}
}
