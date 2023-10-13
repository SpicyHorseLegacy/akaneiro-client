using UnityEngine;
using System.Collections;

public class _UI_CS_SpiritHelperItemEx : MonoBehaviour {

	public _UI_CS_SpiritHelperItem m_ShInfo;
	public UIButton m_AtkIconButton;
	public UIButton m_BgIconButton;
	private Rect  		 m_rect;
	public int    		 m_ListID;
	public UIButton 	 m_Summoned;
	
	// Use this for initialization
	void Start () {
		m_BgIconButton.AddInputDelegate(IconDelegate);
		m_rect.width = 1;
		m_rect.height = 1;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void IconDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
			case POINTER_INFO.INPUT_EVENT.MOVE:	
		    case POINTER_INFO.INPUT_EVENT.NO_CHANGE:
				m_AtkIconButton.SetColor(_UI_Color.Instance.color1);
				m_BgIconButton.SetColor(_UI_Color.Instance.color1);
				m_Summoned.SetColor(_UI_Color.Instance.color1);
				_UI_CS_SpiritTrainer_Cost.Instance.SpiritCostBring(m_ShInfo);
			break;
			case POINTER_INFO.INPUT_EVENT.RELEASE_OFF:
		    case POINTER_INFO.INPUT_EVENT.MOVE_OFF:
				m_AtkIconButton.SetColor(_UI_Color.Instance.color3);
				m_BgIconButton.SetColor(_UI_Color.Instance.color3);
				m_Summoned.SetColor(_UI_Color.Instance.color3);
				_UI_CS_SpiritTrainer_Cost.Instance.SpiritCostDismiss();
			break;
		}	
	}
}
