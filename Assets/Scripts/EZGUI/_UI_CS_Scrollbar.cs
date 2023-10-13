using UnityEngine;
using System.Collections;

public class _UI_CS_Scrollbar : MonoBehaviour 
{
	[SerializeField]
	private UIButton m_ButtonOne;
	[SerializeField]
	private UIButton m_ButtonTwo;
	
	//private bool m_IsTouching = false;
	private UISlider m_Scrollbar;
	
	// Use this for initialization
	void Start ()
	{
		m_Scrollbar = GetComponent<UISlider>();
		m_Scrollbar.AddInputDelegate(TouchDelegate);
	}
	
	private void TouchDelegate (ref POINTER_INFO ptr)
	{
	   // Display a message in the console if
	   // the pointer is dragged over the control:
	   if(ptr.evt == POINTER_INFO.INPUT_EVENT.MOVE || ptr.evt == POINTER_INFO.INPUT_EVENT.DRAG)
		{
			//m_IsTouching = true;
			m_ButtonOne.SetState((int)m_Scrollbar.GetKnob().controlState);
			m_ButtonTwo.SetState((int)m_Scrollbar.GetKnob().controlState);
			//Debug.Log(m_Scrollbar.GetKnob().controlState.ToString());
		}
	      
		
		if(ptr.evt == POINTER_INFO.INPUT_EVENT.MOVE_OFF || ptr.evt == POINTER_INFO.INPUT_EVENT.RELEASE_OFF)
		{
			//m_IsTouching = false;
			m_ButtonOne.SetState(0);
			m_ButtonTwo.SetState(0);
		}
	}
}
