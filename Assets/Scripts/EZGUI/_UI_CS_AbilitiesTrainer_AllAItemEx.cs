using UnityEngine;
using System.Collections;

public class _UI_CS_AbilitiesTrainer_AllAItemEx : MonoBehaviour {

	public _UI_CS_AbilitiesTrainerItem m_AAInfo;
	public UIButton		 m_AIconButton;
	public UIButton 	 m_BgIconButton;
	public SpriteText    m_NameText;
	public SpriteText    m_LevelText;
	public SpriteText    m_PlayerLevelText;
	public SpriteText    m_ValText;
	private Rect  m_rect;
	public int    m_ListID;
	
	// Use this for initialization
	void Start () {
		m_BgIconButton.AddInputDelegate(IconDelegate);
		m_AIconButton.AddInputDelegate(IconDelegate);;
		m_rect.width = 1;
		m_rect.height = 1;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public virtual void IconDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.MOVE:	
		   case POINTER_INFO.INPUT_EVENT.NO_CHANGE:
				m_AIconButton.SetColor(_UI_Color.Instance.color1);
				m_ValText.SetColor(_UI_Color.Instance.color1);
				m_NameText.SetColor(_UI_Color.Instance.color1);
				_UI_CS_AbilitiesTrainer.Instance.ShowAbiInfo(m_AAInfo,m_BgIconButton.color);
				_UI_CS_AbilitiesTrainer.Instance.RightClickLogic();
			break;
		   case POINTER_INFO.INPUT_EVENT.RELEASE_OFF:
		   case POINTER_INFO.INPUT_EVENT.MOVE_OFF:
				m_ValText.SetColor(_UI_Color.Instance.color3);
				m_NameText.SetColor(_UI_Color.Instance.color3);
				m_AIconButton.SetColor(_UI_Color.Instance.color3);
				_UI_CS_AbilitiesTrainer.Instance.DismissAbiInfo();
			break;
		}	
	}

}
