using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UI_Revive_Manager : MonoBehaviour {

    public static UI_Revive_Manager Instance;

	bool m_freeRevival;
	float m_countdown;
	[SerializeField] UILabel CountDown;
    [SerializeField] UILabel MissionEarning_XP;
    [SerializeField] UILabel MissionEarning_Karma;
    [SerializeField] UI_MoneyGroupBase Karma_Group;
	[SerializeField] UI_Revive_SingleMaterial Material_Prefab;
	[SerializeField] GameObject Material_Parent;

	[SerializeField] GameObject RetreatPopup;
	
    void Awake()
    {
        Instance = this;
    }

	void Update()
	{
		if(m_countdown > 0)
		{
			m_countdown -= Time.deltaTime;
			CountDown.text = "" + (int)m_countdown;
			if(m_countdown <= 0)
			{
				changetofree();
			}
		}
	}

	void changetofree()
	{
		UI_TypeDefine.UI_Money_data _karma = new UI_TypeDefine.UI_Money_data();
		_karma.Type = UI_TypeDefine.ENUM_UI_Money_Type.Karma;
		_karma.MoneyString = "FREE";
		Karma_Group.UpdateAllInfo(_karma);

		m_freeRevival = true;
	}

    public void UpdateAllReviveInfo(int _missionXP, int _missionKarma, UI_TypeDefine.UI_Money_data _karma, UI_TypeDefine.UI_Money_data _crystal, float _countdown)
    {
		m_freeRevival = false;
		m_countdown = _countdown;
		CountDown.text = "" + (int)m_countdown;
        MissionEarning_XP.text = "" + _missionXP;
        MissionEarning_Karma.text = "" + _missionKarma;
        Karma_Group.UpdateAllInfo(_karma);
		
		for (int i = 0; i < PlayerDataManager.Instance.materialsList.Count; i++)
        {
			UI_Revive_SingleMaterial _newmat = Instantiate(Material_Prefab) as UI_Revive_SingleMaterial;
			_newmat.transform.parent = Material_Parent.transform;
			_newmat.transform.localScale = Vector3.one;
			_newmat.transform.localPosition = new Vector3(-265.0f + 55.0f, 0, 0);
			_newmat.UpdateReviveMaterial(PlayerDataManager.Instance.materialsList[i]);
        }
    }

    void KarmaReviveBTNClicked()
    {
		if(m_freeRevival)
			CS_Main.Instance.g_commModule.SendMessage(ProtocolBattle_SendRequest.ReviveReq(new EReviveType(EReviveType.eReviveType_NoMoney)));
		else
        	CS_Main.Instance.g_commModule.SendMessage(ProtocolBattle_SendRequest.ReviveReq(new EReviveType(EReviveType.eReviveType_Money)));
    }
    void CrystalReviveBTNClicked()
    {
        CS_Main.Instance.g_commModule.SendMessage(ProtocolBattle_SendRequest.ReviveReq(new EReviveType(EReviveType.eReviveType_Crystal)));
    }
	//mm
	void RetreatBTNClicked(){
		RetreatPopup.gameObject.SetActive(true);
	}
	void RetreatYes(){
		Player.Instance.FSM.ChangeState(Player.Instance.IS);
		Player.Instance.ReactivePlayer();
		TutorialMan.Instance.SetTutorialFlag(false);
		int missionID = _UI_CS_MapInfo.Instance.SceneNameToMissionID("Hub_Village") + 1;
		UI_Fade_Control.Instance.FadeOutAndIn("Hub_Village", "Hub_Village", missionID - 1);
	}
	void RetreatNo(){
		RetreatPopup.gameObject.SetActive(false);
	}
	//#mm
    #region Delegates
    #endregion
}
