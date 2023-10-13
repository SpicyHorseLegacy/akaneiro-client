using UnityEngine;
using System.Collections;

public class UI_Hud_FullscreenButton : MonoBehaviour 
{
    [SerializeField]
    public UILabel m_fullscreenLabel;

    [SerializeField]
    public UILabel m_windowLabel;

    void Start()
    {
        if (Screen.fullScreen == false)
        {
            m_fullscreenLabel.gameObject.SetActive(true);
            m_windowLabel.gameObject.SetActive(false);
        }
        else
        {
            m_fullscreenLabel.gameObject.SetActive(false);
            m_windowLabel.gameObject.SetActive(true);
        }
    }

    void FixedUpdate()
    {
        if (Screen.fullScreen == false)
        {
            m_fullscreenLabel.gameObject.SetActive(true);
            m_windowLabel.gameObject.SetActive(false);
        }
        else
        {
            m_fullscreenLabel.gameObject.SetActive(false);
            m_windowLabel.gameObject.SetActive(true);
        }
    }

    void ButtonClicked()
    {
        GameConfig.IsFullScreen = !Screen.fullScreen;

        if (Screen.fullScreen == false)
            Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
        else
            Screen.SetResolution(1280, 720, false);
    }
}
