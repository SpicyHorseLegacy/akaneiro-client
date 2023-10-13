using UnityEngine;
using System.Collections;

public class _UI_ScroolBtn : MonoBehaviour {
	
	public UIScrollList m_List;
	public bool 		m_Inverse = false;
	private float		m_speed;
	
	private UIButton 	m_Button;
	
	private bool 		m_isUpdate = false;
	
	public  float		k = 300;
	
	// Use this for initialization
	void Start () {
		m_Button = GetComponent<UIButton>();
		m_Button.AddInputDelegate(ButtonDelegate);
		
		
	}
	
	// Update is called once per frame
	void Update () {
		
		float temp;
		if(m_isUpdate){
		
			UIScrollKnob theKnob = m_List.slider.GetKnob();
			
			if((m_List.slider.width - theKnob.width) > theKnob.width){
				
				m_speed = theKnob.width / 300;
				
			}else{
				
				m_speed = theKnob.width / 100;
				
			}
			
			if (m_Inverse){	
					
					temp = Mathf.Clamp01(m_List.ScrollPosition  - m_speed);
					if(temp < 0 )
						temp = 0;
					m_List.ScrollPosition = temp;
					
				}else{

					temp = Mathf.Clamp01(m_List.ScrollPosition  + m_speed);
					if(temp > 1)
						temp = 1;
					m_List.ScrollPosition = temp;

				}

		}
		
	}
	
	private void ButtonDelegate (ref POINTER_INFO ptr)
	{
		
		switch(ptr.evt)
		{
			case POINTER_INFO.INPUT_EVENT.TAP:
				
				m_isUpdate = false;	
			
			break;
			case POINTER_INFO.INPUT_EVENT.RELEASE:
				
				m_isUpdate = false;	
			
			break;
			case POINTER_INFO.INPUT_EVENT.RELEASE_OFF:
				
				m_isUpdate = false;	
			
			break;
			case POINTER_INFO.INPUT_EVENT.PRESS:
				
				m_isUpdate = true;	
			
			break;
		}
	}
}
