using UnityEngine;
using System.Collections;

public class _UI_CS_AbilitiesItemEx : MonoBehaviour {

	public _UI_CS_AbilitiesItem m_abilitiesInfo;
	public UIButton m_iconButton;
	public bool   m_isUse = false;
	private Rect  m_rect;
	public int    m_ListID;
	
	// Use this for initialization
	void Start () {
		m_iconButton.AddInputDelegate(IconDelegate);
		m_rect.width = 1;
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
				//if(m_isUse){
					//m_isUse = false;

				//}else{
					//搜索可使用槽
					int idx = _UI_CS_AbilitiesCtrl.Instance.FindAbilitiesSlot();
					if(-1 != idx){
						_UI_CS_AbilitiesCtrl.Instance.m_UseAbilities[idx].m_isEmpty = false;
						_UI_CS_AbilitiesCtrl.Instance.m_UseAbilities[idx].m_abilitiesInfo = m_abilitiesInfo;
						m_isUse = true;
						_UI_CS_AbilitiesCtrl.Instance.m_UseAbilities[idx].m_iconBtn.SetUVs(m_rect);
//						_UI_CS_AbilitiesCtrl.Instance.m_UseAbilities[idx].m_iconBtn.SetTexture(_UI_CS_Resource.Instance.m_AbilitiesIcon[m_abilitiesInfo.m_iconID]);
						_UI_CS_AbilitiesCtrl.Instance.m_UseAbilities[idx].m_iconBtn.SetTexture(AbilityInfo.Instance.GetAbilityByID((uint)m_abilitiesInfo.m_AbilitieID).icon);
//						_UI_CS_AbilitiesCtrl.Instance.m_UseAbilities[idx].m_level.Text = m_abilitiesInfo.m_level.ToString();
//						_UI_CS_AbilitiesCtrl.Instance.m_UseAbilities[idx].m_name.Text  = m_abilitiesInfo.m_name;
//						_UI_CS_AbilitiesCtrl.Instance.m_UseAbilities[idx].m_ListID     = m_ListID;
							
						LogManager.Log_Debug("--- SetSkillShortcut ---");
						CS_Main.Instance.g_commModule.SendMessage(
				   			ProtocolBattle_SendRequest.SetSkillShortcut(m_abilitiesInfo.m_AbilitieID,idx/3,idx%3)
						);
					}
				//}
				break;
		   default:
				break;
		}	
	}
}
