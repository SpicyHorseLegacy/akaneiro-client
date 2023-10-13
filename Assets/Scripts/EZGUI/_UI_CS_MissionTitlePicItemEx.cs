using UnityEngine;
using System.Collections;

public class _UI_CS_MissionTitlePicItemEx : MonoBehaviour {

	public 	_UI_CS_MissionTitlePicItem  m_ItemInfo;
	public 	UIButton 					m_BgIconButton;
	public 	SpriteText    				m_NameText;
	private Rect  						m_rect;
	public 	int    						m_ListID;
	
	// Use this for initialization
	void Start () {
		m_BgIconButton.AddInputDelegate(IconDelegate);
		m_rect.width  = 1;
		m_rect.height = 1;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void IconDelegate(ref POINTER_INFO ptr)
	{
		switch(ptr.evt)
		{
//		   case POINTER_INFO.INPUT_EVENT.TAP:
//				break;
		   default:
				break;
		}	
	}
}
