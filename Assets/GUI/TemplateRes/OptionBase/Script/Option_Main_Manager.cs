using UnityEngine;
using System.Collections;

public class Option_Main_Manager : MonoBehaviour {

    public static Option_Main_Manager Instance;

    void Awake()
    {
        Instance = this;
        GUIManager.Instance.AddTemplateInitEnd();
    }

    #region Interface

    public enum EnumOptionBTNType
    {
        None,
        Setting,
        HelpIndex,
        Gift,
        BugReport,
        BackToVillage,
        LogOut,
        QuitGame,
        Max,
    }

    [SerializeField]
    public UITiledSprite m_backToVillageBtnBackground;

    [SerializeField]
    public GameObject m_quitBtn;

    [SerializeField]
    public GameObject m_quitConfirm;

    private bool m_inVillageNow = false;

    public void Init()
    {
        // If we are in village disable button back to village
        if (PlayerDataManager.Instance.GetMissionID() >= 6000 && PlayerDataManager.Instance.GetMissionID() < 6010)
        {
            m_backToVillageBtnBackground.color = new Color(0.53f, 0.56f, 0.61f, 1f);
            m_inVillageNow = true;
        }
        // else enable it
        else
        {
            m_backToVillageBtnBackground.color = new Color(0.15f, 0.53f, 0.97f, 1f);
            m_inVillageNow = false;
        }

#if !UNITY_STANDALONE
        m_quitBtn.SetActive(false);
#endif
    }

    public void ShowConfirm()
    {
        m_quitConfirm.SetActive(true);
    }
    #endregion

    #region Delegates
    public delegate void Handle_OptionMenuBTNClicked_Delegate(EnumOptionBTNType _menuBTN);
    public event Handle_OptionMenuBTNClicked_Delegate OptionMenuBTNClicked_Event;
    public delegate void Handle_OptionCloseBTNClicked_Delegate();
    public event Handle_OptionCloseBTNClicked_Delegate CloseBTNClicked_Event;
    
    void SettingBTNPressed()
    {
        if (OptionMenuBTNClicked_Event != null)
            OptionMenuBTNClicked_Event(EnumOptionBTNType.Setting);
    }
    void HelpBTNPressed()
    {
        if (OptionMenuBTNClicked_Event != null)
            OptionMenuBTNClicked_Event(EnumOptionBTNType.HelpIndex);
    }
    void GiftBTNPressed()
    {
        if (OptionMenuBTNClicked_Event != null)
            OptionMenuBTNClicked_Event(EnumOptionBTNType.Gift);
    }
    void BugReportBTNPressed()
    {
        if (OptionMenuBTNClicked_Event != null)
            OptionMenuBTNClicked_Event(EnumOptionBTNType.BugReport);
    }
    void BackToVillageBTNPressed()
    {
        if (OptionMenuBTNClicked_Event != null && !m_inVillageNow)
            OptionMenuBTNClicked_Event(EnumOptionBTNType.BackToVillage);
    }
    void LogOutBTNPressed()
    {
        if (OptionMenuBTNClicked_Event != null)
            OptionMenuBTNClicked_Event(EnumOptionBTNType.LogOut);
    }
    void QuitBTNPressed()
    {
        if (OptionMenuBTNClicked_Event != null)
            OptionMenuBTNClicked_Event(EnumOptionBTNType.QuitGame);
    }
	
	void CloseBTNClicked()
	{
		Player.Instance.ReactivePlayer();
		GUIManager.Instance.ChangeUIScreenState("IngameScreen");
	}

    void YesDelegate()
    {
        Application.Quit();
    }

    void NoDelegate()
    {
        m_quitConfirm.SetActive(false);
    }
    #endregion
}
