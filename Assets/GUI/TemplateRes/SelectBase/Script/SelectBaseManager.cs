using UnityEngine;
using System.Collections;

public class SelectBaseManager : MonoBehaviour
{
    public static SelectBaseManager Instance;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && m_mouseOverImportantThings == false)
        {
            m_newsCheckbox.isChecked = false;
        }
    }

    public bool m_mouseOverImportantThings = false;

    #region Interface
    public void Init()
    {
        HideDeletePanle(true, "");
        HideTutorialPanle(true);
    }

    [SerializeField]
    private UILabel delName;
    public void HideDeletePanle(bool isHide, string name)
    {
        NGUITools.SetActive(deletePanel.gameObject, !isHide);
        delName.text = name;
    }

    [SerializeField]
    private PlayerModel playerMod;
    public PlayerModel GetPlayerModel()
    {
        return playerMod;
    }

    [SerializeField]
    UILabel m_label_power;
    [SerializeField]
    UILabel m_label_defense;
    [SerializeField]
    UILabel m_label_skill;
    [SerializeField]
    UILabel m_label_maxhp;
    [SerializeField]
    UILabel m_label_maxmp;
    #endregion

    #region Public

    public void UpdateCharacterAttrInfo(vectorAttrChange attrvec)
    {
        foreach (SAttributeChange _value in attrvec)
        {
            switch (_value.attributeType.Get())
            {
                case EAttributeType.ATTR_MaxHP:
                    m_label_maxhp.text = "" + _value.value;
                    break;
                case EAttributeType.ATTR_MaxMP:
                    m_label_maxmp.text = "" + _value.value;
                    break;
                case EAttributeType.ATTR_Power:
                    m_label_power.text = "" + _value.value;
                    break;
                case EAttributeType.ATTR_Defense:
                    m_label_defense.text = "" + _value.value;
                    break;
                case EAttributeType.ATTR_Skill:
                    m_label_skill.text = "" + _value.value;
                    break;
            }
        }
    }

    #endregion

    #region Local
    [SerializeField]
    private GameObject m_newsGameObject;
    [SerializeField]
    private UICheckbox m_newsCheckbox;
    void ToggleNews(bool state)
    {
        m_newsGameObject.GetComponent<LinkToMoreInfos>().CurrentTarget = state ? 1f : 0f;
    }

    [SerializeField]
    private NGUIButton playBtn;
    public delegate void Handle_PlayBtnDelegate();
    public event Handle_PlayBtnDelegate OnPlayBtnDelegate;
    private void PlayDelegate()
    {
        if (OnPlayBtnDelegate != null)
        {
            OnPlayBtnDelegate();
        }
    }
    [SerializeField]
    private NGUIButton deleteBtn;
    public void HideDelBtn(bool hide)
    {
        NGUITools.SetActive(deleteBtn.gameObject, !hide);
    }

    public delegate void Handle_DelBtnDelegate();
    public event Handle_DelBtnDelegate OnDelBtnDelegate;
    private void DeleteDelegate()
    {
        if (OnDelBtnDelegate != null)
        {
            OnDelBtnDelegate();
        }
    }
    [SerializeField]
    private UICheckbox fullScreenBtn;
    public delegate void Handle_FullScreenDelegate(bool isChecked);
    public event Handle_FullScreenDelegate OnFullScreenDelegate;
    private void fullScreenDelegate(bool isChecked)
    {
        if (OnFullScreenDelegate != null)
        {
            OnFullScreenDelegate(isChecked);
        }
    }
    public bool GetFullScreenFlag()
    {
        return fullScreenBtn.isChecked;
    }
    public void SetFullScreenFlag(bool isFullScreen)
    {
        fullScreenBtn.isChecked = isFullScreen;

        if (isFullScreen)
            Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);

        else
            Screen.SetResolution(1280, 720, false);
    }

    [SerializeField]
    private NGUIPanel deletePanel;
    public delegate void Handle_DelOKBtnDelegate();
    public event Handle_DelOKBtnDelegate OnDelOKBtnDelegate;
    private void OKDelegate()
    {
        if (OnDelOKBtnDelegate != null)
        {
            OnDelOKBtnDelegate();
        }
        HideDeletePanle(true, "");
    }
    [SerializeField]
    private NGUIButton cancelBtn;
    private void CancelDelegate()
    {
        HideDeletePanle(true, "");
    }

    [SerializeField]
    private NGUIPanel tutorialPanel;
    public void HideTutorialPanle(bool isHide)
    {
        NGUITools.SetActive(tutorialPanel.gameObject, !isHide);
    }
    public delegate void Handle_YesDelegate();
    public event Handle_YesDelegate OnYesDelegate;
    private void YesDelegate()
    {
        if (OnYesDelegate != null)
        {
            OnYesDelegate();
        }
        HideDeletePanle(true, "");
    }
    private void NoDelegate()
    {
        HideDeletePanle(true, "");
    }
    #endregion
}
