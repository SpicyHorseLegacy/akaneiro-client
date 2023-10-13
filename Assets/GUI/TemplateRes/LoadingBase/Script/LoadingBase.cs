using UnityEngine;
using System.Collections;

public class LoadingBase : MonoBehaviour
{
    public static LoadingBase Instance;

    private const int ScreenWidth = 1280;

    // Current Page
    private int m_currentPage = 1;
    // Are we switching pages ?
    private bool m_isSliding = false;
    // Pages velocities
    private float m_firstPageCurrentVelocity;
    private float m_secondPageCurrentVelocity;
    private float m_thirdPageCurrentVelocity;
    // Pages have been switched manually ?
    private bool m_pagesHaveBeenSwitched = false;

    void Awake()
    {
        Instance = this;
        GUIManager.Instance.AddTemplateInitEnd();
    }

    void Start()
    {
        Init();
    }

    void Update()
    {
        // Switching pages with smooth animations
        if (m_isSliding)
        {
            Vector3 firstPageCurrentPosition = comicFirstPage.transform.parent.localPosition;
            Vector3 secondPageCurrentPosition = comicSecondPage.transform.parent.localPosition;
            Vector3 thirdPageCurrentPosition = comicThirdPage.transform.parent.localPosition;

            int firstPageTargetPosition = (m_currentPage - 1) * -ScreenWidth;
            int secondPageTargetPosition = (m_currentPage - 2) * -ScreenWidth;
            int thirdPageTargetPosition = (m_currentPage - 3) * -ScreenWidth;

            // If we didn't reach the target position yet
            if ((Mathf.Abs(firstPageCurrentPosition.x - firstPageTargetPosition) > 0.01f) && (Mathf.Abs(secondPageCurrentPosition.x - secondPageTargetPosition) > 0.01f) && (Mathf.Abs(thirdPageCurrentPosition.x - thirdPageTargetPosition) > 0.01f))
            {
                firstPageCurrentPosition.x = Mathf.SmoothDamp(firstPageCurrentPosition.x, firstPageTargetPosition, ref m_firstPageCurrentVelocity, 0.2f);
                secondPageCurrentPosition.x = Mathf.SmoothDamp(secondPageCurrentPosition.x, secondPageTargetPosition, ref m_secondPageCurrentVelocity, 0.2f);
                thirdPageCurrentPosition.x = Mathf.SmoothDamp(thirdPageCurrentPosition.x, thirdPageTargetPosition, ref m_thirdPageCurrentVelocity, 0.2f);
            }
            // Target position reached, stopping animations
            else
            {
                firstPageCurrentPosition.x = firstPageTargetPosition;
                secondPageCurrentPosition.x = secondPageTargetPosition;
                thirdPageCurrentPosition.x = thirdPageTargetPosition;

                m_isSliding = false;
            }

            comicFirstPage.transform.parent.localPosition = firstPageCurrentPosition;
            comicSecondPage.transform.parent.localPosition = secondPageCurrentPosition;
            comicThirdPage.transform.parent.localPosition = thirdPageCurrentPosition;
        }
    }

    #region Interface...
    [SerializeField]
    private UITexture comicFirstPage;

    [SerializeField]
    private UITexture comicSecondPage;

    [SerializeField]
    private UITexture comicThirdPage;

    public UITexture GetComicWidget(int index)
    {
        switch (index)
        {
            case 0:
                return comicFirstPage;
            case 1:
                return comicSecondPage;
            case 2:
                return comicThirdPage;
        }

        return comicFirstPage;
    }

    // Switch pages to the left
    public void GoLeft()
    {
        m_isSliding = m_pagesHaveBeenSwitched = true;
        m_currentPage--;
    }

    // Switch pages to the right
    public void GoRight(bool automaticSwitch = false)
    {
        m_isSliding = true;
        m_pagesHaveBeenSwitched = !automaticSwitch;
        m_currentPage++;
    }

    [SerializeField]
    private UILabel storyInfoFirstPage;
    
    [SerializeField]
    private UILabel storyInfoSecondPage;

    [SerializeField]
    private UILabel storyInfoThirdPage;

    public void SetStoryInfo(string text, int page)
    {
        switch (page)
        {
            case 1:
                storyInfoFirstPage.text = text;
                break;
            case 2:
                storyInfoSecondPage.text = text;
                break;
            case 3:
                storyInfoThirdPage.text = text;
                break;
        }
    }

    [SerializeField]
    private UILabel missionName;
    public void SetMissionName(string text)
    {
        missionName.text = text;
    }

    [SerializeField]
    private UILabel areaName;
    public void SetAreaName(string text)
    {
        areaName.text = "[ff0000]" + text;
    }

    [SerializeField]
    private UILabel randomInfo;
    public void SetRandomInfo(string text)
    {
        randomInfo.text = text;
    }

    [SerializeField]
    private UILabel loadingInfo;
    public void SetLoadingInfo(string text)
    {
        loadingInfo.text = text;
    }

    [SerializeField]
    private NGUISlider progress;
    private bool m_switchedToSecondPage = false;
    private bool m_switchedToThirdPage = false;
    public void SetProgressValue(float loadingPercentage)
    {
        progress.sliderValue = loadingPercentage;

        if (loadingPercentage > 0.33f && !m_pagesHaveBeenSwitched && !m_switchedToSecondPage)
        {
            // Automatic switch to image two
            if (OnArrowRightDelegate != null)
                OnArrowRightDelegate(true);

            m_switchedToSecondPage = true;
        }
        else if (loadingPercentage > 0.66f && !m_pagesHaveBeenSwitched && !m_switchedToThirdPage)
        {
            // Automatic switch to image three
            if (OnArrowRightDelegate != null)
                OnArrowRightDelegate(true);

            m_switchedToThirdPage = true;
        }
    }

    public float GetProgressValue()
    {
        return progress.sliderValue;
    }

    [SerializeField]
    private Transform leftArrow;
    public void HideLeftArrow(bool hide)
    {
        NGUITools.SetActive(leftArrow.gameObject, !hide);
    }

    [SerializeField]
    private Transform rightArrow;
    public void HideRightArrow(bool hide)
    {
        NGUITools.SetActive(rightArrow.gameObject, !hide);
    }
    #endregion

    #region Local...
    private void Init()
    {
        SetProgressValue(0);
    }

    public delegate void Handle_StartDelegate();
    public event Handle_StartDelegate OnStartDelegate;
    private void StartDelegate()
    {
        if (OnStartDelegate != null)
        {
            OnStartDelegate();
        }
    }

    public delegate void Handle_ArrowRightDelegate(bool automaticSwitch = false);
    public event Handle_ArrowRightDelegate OnArrowRightDelegate;
    private void ArrowRightDelegate()
    {
        if (OnArrowRightDelegate != null)
        {
            OnArrowRightDelegate(false);
        }
    }

    public delegate void Handle_ArrowLeftDelegate();
    public event Handle_ArrowLeftDelegate OnArrowLeftDelegate;
    private void ArrowLeftDelegate()
    {
        if (OnArrowLeftDelegate != null)
        {
            OnArrowLeftDelegate();
        }
    }

    public delegate void Handle_DescriptionDelegate();
    public event Handle_DescriptionDelegate OnDescriptionDelegate;
    private void DescriptionDelegate()
    {
        if (OnDescriptionDelegate != null)
        {
            OnDescriptionDelegate();
        }
    }
    #endregion
}
