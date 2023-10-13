using UnityEngine;
using System.Collections;

public class _UI_CS_ScrollList_ButtonEx : MonoBehaviour
{
	[SerializeField]
	public UIScrollList m_List;        // The Scroll List
	//[SerializeField]
	public bool m_Inverse = false;
	//[SerializeField]
	private float m_Speed = 8f;
	
	//private bool m_IsActive = false;
	private UIButton m_Button;
	
	public float Size;
	
	public float MaxSize;
	
	//public bool isPicBtn;
	
	public bool IsActiv;
	
	public bool IsLevelFlag = false;
	
	// Use this for initialization
	void Start () 
	{
		m_List.ScrollPosition = MaxSize;
		m_Button = GetComponent<UIButton>();
		m_Button.AddInputDelegate(ButtonDelegate);
		IsActiv = false;
		
	}
	
	// Update is called once per frame
	void Update ()
	{

	}
	
	private void ButtonDelegate (ref POINTER_INFO ptr)
	{
		float temp;
		switch(ptr.evt)
		{
			case POINTER_INFO.INPUT_EVENT.TAP:
				
//			if(IsActiv){
//				IsActiv = false;
//				if (m_Inverse){	
//					
//					temp = Mathf.Clamp01(m_List.ScrollPosition  - Size);
//					if(temp < MaxSize )
//						temp = MaxSize;
//					m_List.ScrollPosition = temp;
//					
//				}else{
//
//					temp = Mathf.Clamp01(m_List.ScrollPosition  + Size);
//					if(temp > MaxSize+(m_List.Count-3)*Size)
//						temp = MaxSize+(m_List.Count-3)*Size;
//					m_List.ScrollPosition = temp;
//
//				}
//			}
			break;
		}
	}
}
