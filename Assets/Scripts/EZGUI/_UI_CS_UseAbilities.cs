using UnityEngine;
using System.Collections;

public class _UI_CS_UseAbilities : MonoBehaviour {
	
	public UIButton 			m_iconBtn;
	public UIButton 			m_HighLight;
	public bool					m_isEmpty = true;
	public _UI_CS_AbilitiesItem m_abilitiesInfo;
	private Rect  				m_rect;
	public bool   				m_isCoolDownFinish = true;
	public bool   				m_isCoolDownStop   = false;
	private float 				m_coolDownStartTime;
	private float 				m_coolDownStopTime;
	public float 				m_coolDownTime = 0;
	public int 					m_groupId;
	public int 					m_IdxId;

	// Use this for initialization
	void Start () {
		m_rect.width = 1;
		m_rect.height = 1;
	}
	
	// Update is called once per frame
	void Update () {
		if(!m_isCoolDownFinish){
			if(!m_isCoolDownStop){
				if(Time.time - m_coolDownStartTime >=  m_abilitiesInfo.m_Cooldown){
					AbilitieCoolDownReset();
					m_coolDownTime = 0;
				}else{
					m_coolDownTime = (m_abilitiesInfo.m_Cooldown - (Time.time - m_coolDownStartTime));
				}
			}
		}
	}

	public void AbilitieCoolDownStart(){
		m_isCoolDownFinish = false;
		m_coolDownStartTime = Time.time;
		_UI_CS_FightScreen.Instance.m_abiMaskEffest[m_IdxId].InitMesh();
		_UI_CS_FightScreen.Instance.m_abiMaskEffest[m_IdxId].StartEffect(m_abilitiesInfo.m_Cooldown);
		
	}
	
	public void AbilitieCoolDownPause(){
		m_isCoolDownStop = true;
		m_coolDownStopTime = Time.time;
	}
	
	public void AbilitieCoolDownResume(){
		m_isCoolDownStop = false;
		m_coolDownStartTime += Time.time - m_coolDownStopTime;
		m_coolDownStopTime = 0;
	}
	
	public void AbilitieCoolDownReset(){
		m_isCoolDownFinish = true;
		m_coolDownStartTime = 0;
	}
	
	public void SetUseAbiIcon(int id){
		if(id<=0){
			m_iconBtn.SetUVs(new Rect(0,0,1,1));
			m_iconBtn.SetTexture(_UI_CS_Resource.Instance.m_EquipmentIcon[0]);
			return;
		}
		m_iconBtn.SetUVs(new Rect(0,0,1,1));
		m_iconBtn.SetTexture(AbilityInfo.Instance.GetAbilityByID((uint)id).icon);
	}
}
