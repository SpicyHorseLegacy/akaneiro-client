using UnityEngine;
using System.Collections;

public class _UI_CS_ScrollList_Button : MonoBehaviour
{
	[SerializeField]
	private UIScrollList m_List;        // The Scroll List
	//[SerializeField]
	public bool m_Inverse = false;
	//[SerializeField]
	public float m_Speed = 8f;
	
	private bool m_IsActive = false;
	private UIButton m_Button;
	
	// Use this for initialization
	void Start () 
	{
		m_Button = GetComponent<UIButton>();
		m_Button.AddInputDelegate(ButtonDelegate);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (m_IsActive)
		{
			if (m_Inverse)
			{
				m_List.ScrollPosition = Mathf.Clamp01(m_List.ScrollPosition  - (m_Speed * Time.deltaTime));
				//m_List.ScrollPosition = Mathf.Clamp01(m_List.ScrollPosition  - 10);
			}
			else
				m_List.ScrollPosition = Mathf.Clamp01(m_List.ScrollPosition  + (m_Speed * Time.deltaTime));
				//m_List.ScrollPosition = Mathf.Clamp01(m_List.ScrollPosition  + 10);
		}
	}
	
	private void ButtonDelegate (ref POINTER_INFO ptr)
	{
		switch(ptr.evt)
		{
			case POINTER_INFO.INPUT_EVENT.RELEASE:
				m_IsActive = false;
			break;
			case POINTER_INFO.INPUT_EVENT.RELEASE_OFF:
				m_IsActive = false;
			break;
			case POINTER_INFO.INPUT_EVENT.TAP:
				m_IsActive = false;
			break;
			case POINTER_INFO.INPUT_EVENT.PRESS:
				m_IsActive = true;
			break;
		}
	}
}
