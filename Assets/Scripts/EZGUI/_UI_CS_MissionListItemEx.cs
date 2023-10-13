using UnityEngine;
using System.Collections;

public class _UI_CS_MissionListItemEx : MonoBehaviour {

	public 	_UI_CS_MissionListItem  	m_ItemInfo;
	public 	UIButton 					m_BgIconButton;
	public 	SpriteText    				m_NameText;
	public 	SpriteText    				m_XpText;
	public 	SpriteText    				m_SkText;
	public 	UIButton 					m_RightIconButton;
	public 	UIButton 					m_NewIconButton;
	public  int 						levelID;
	public  string 						mapName;
	private Rect  						m_rect;
	public 	int    						m_ListID;
	public int 							missionID;
	
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
		   case POINTER_INFO.INPUT_EVENT.TAP:
				
			
				//_UI_CS_BountyMaster.Instance.LevelIdx = levelID;
//				_UI_CS_BountyMaster.Instance.MissionmapName = mapName;
//				_UI_CS_BountyMaster.Instance.MissionmapID = missionID;
//				_UI_CS_BountyMaster.Instance.popupMsgMenu(0);
//				_UI_CS_BountyMaster.Instance.SetSkAndXpVal(m_SkText.text,m_XpText.text);
			
			
				break;
		   default:
				break;
		}	
	}
}
