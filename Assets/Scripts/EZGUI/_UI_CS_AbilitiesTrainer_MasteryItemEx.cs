using UnityEngine;
using System.Collections;

public class _UI_CS_AbilitiesTrainer_MasteryItemEx : _UI_CS_AbilitiesTrainer_AllAItemEx {

    public SingleMastery Mastery_Info;
	public SpriteText    m_NextLvAttributionNeededText;
	
	public override void IconDelegate(ref POINTER_INFO ptr)
	{
		switch(ptr.evt)
		{
		   case POINTER_INFO.INPUT_EVENT.TAP:
			
				_UI_CS_AbilitiesTrainer.Instance.PopMsg(1);
				_UI_CS_AbilitiesTrainer.Instance.CurItemTypeInMSG = 1;
				_UI_CS_AbilitiesTrainer.Instance.IconBtn.SetUVs(new Rect(0,0,1,1));
				_UI_CS_AbilitiesTrainer.Instance.IconBtn.SetTexture(m_AAInfo.m_icon);
				_UI_CS_AbilitiesTrainer.Instance.levelText.Text = (m_AAInfo.m_level).ToString();
				_UI_CS_AbilitiesTrainer.Instance.nameText.Text = m_AAInfo.m_name;
			
				if(-1 != m_AAInfo.m_skVal){
				
					_UI_CS_AbilitiesTrainer.Instance.PaySkText.Text = m_AAInfo.m_skVal.ToString();
				
				}else{
				
					_UI_CS_AbilitiesTrainer.Instance.PaySkText.Text = "------";
				
				}
			
//				if(-1 != m_AAInfo.m_fkVal){
//				
//					_UI_CS_AbilitiesTrainer.Instance.PaySfText.Text = m_AAInfo.m_fkVal.ToString();
//				
//				}else{
//				
//					_UI_CS_AbilitiesTrainer.Instance.PaySfText.Text = "------";
//				
//				}
			
				break;
		   default:
				break;
		}	
	}
}
