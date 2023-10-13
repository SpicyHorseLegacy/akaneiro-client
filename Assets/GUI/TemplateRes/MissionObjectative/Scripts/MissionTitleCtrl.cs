using UnityEngine;
using System.Collections;

public class MissionTitleCtrl : MonoBehaviour
{
    public static MissionTitleCtrl Instance;
    public static bool m_alreadyPlayed = false;

    public static void Reset()
    {
        m_alreadyPlayed = false;
    }

    void Awake()
    {
        Instance = this;
    }

    #region Interface
    [SerializeField]
    private UILabel areaName;
    public void SetAreaName(string name)
    {
        areaName.text = name;
    }
    [SerializeField]
    private UILabel missionName;
    public void SetMissionName(string name)
    {
        missionName.text = name;
    }

    public void Play()
    {
        if (m_alreadyPlayed == false)
        {
            m_alreadyPlayed = true;
            TweenAlpha.Begin(gameObject, 1, 1);
            gameObject.GetComponent<TweenAlpha>().eventReceiver = gameObject;
            gameObject.GetComponent<TweenAlpha>().callWhenFinished = "DelayDelegate";
        }
        else
        {
            this.gameObject.SetActive(false);
            MissionObjectiveManager.Instance.PlayMissionAni();
        }
    }

    private void DelayDelegate()
    {
        TweenDelay.Begin(gameObject, 2, "DismissDelegate", null);
    }

    public void DismissDelegate()
    {
        TweenAlpha.Begin(gameObject.gameObject, 1, 0);
        gameObject.GetComponent<TweenAlpha>().eventReceiver = gameObject;
        gameObject.GetComponent<TweenAlpha>().callWhenFinished = "TitleOutDelegate";
    }

    public delegate void Handle_TitleOutDelegate();
    public event Handle_TitleOutDelegate OnTitleOutDelegate;
    private void TitleOutDelegate()
    {
        if (OnTitleOutDelegate != null)
        {
            OnTitleOutDelegate();
        }
    }
    #endregion
}
