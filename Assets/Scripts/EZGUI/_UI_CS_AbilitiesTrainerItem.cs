using UnityEngine;
using System.Collections;

public class _UI_CS_AbilitiesTrainerItem{

	public int 		 	  m_id;
	public string 		  m_name;
	public int	  		  m_level;
	public int	  		  m_playerLevel;
	public int	  		  m_type;
	public Texture2D	  m_icon;
	public int    		  m_skVal;
	public int    		  m_fkVal;
	public bool    		  m_isMaxLevel;
	public string 		  m_Des1;
	public string 		  m_Des2;

	public int			  m_MasteryClass;
    public int            m_IsAbilityOrMastery;    // 0 : ability // 1 : mastery
	
	public _UI_CS_AbilitiesTrainerItem(){}
	
	public _UI_CS_AbilitiesTrainerItem(int type)
	{
		m_type = type;
	}
}
