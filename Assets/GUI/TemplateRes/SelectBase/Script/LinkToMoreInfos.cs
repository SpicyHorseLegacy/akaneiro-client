using UnityEngine;
using System.Collections;

public class LinkToMoreInfos : MonoBehaviour
{
    [SerializeField]
    public Texture2D m_newsTexture = null;
    
    [SerializeField]
    public string m_linkToMoreInfos = @"http://spicyworld.spicyhorse.com/social/forum.php?gid=47";

    private float m_currentVelocity;
    private float m_currentTarget = -1f;
    private float m_currentAlpha = 0f;

    public float CurrentTarget
    {
        set { m_currentTarget = value; }
    }

    void Update()
    {
        if (m_currentTarget != -1f)
        {
            m_currentAlpha = Mathf.SmoothDamp(m_currentAlpha, m_currentTarget, ref m_currentVelocity, 0.2f);

            if (m_currentAlpha > 0.9f && !collider.enabled)
                collider.enabled = true;
            else if (m_currentAlpha <= 0.9f && collider.enabled)
                collider.enabled = false;
        }
    }

    void OnGUI()
    {
        if (m_newsTexture != null)
        {
            GUI.color = new Color(1f, 1f, 1f, m_currentAlpha);
            GUI.DrawTexture(new Rect((Screen.width - m_newsTexture.width) / 2f, (Screen.height - m_newsTexture.height) / 2f, m_newsTexture.width, m_newsTexture.height), m_newsTexture);
        }
    }

    void OnClick()
    {
#if UNITY_WEBPLAYER
        Application.ExternalEval("window.open('" + m_linkToMoreInfos + "','Spicy Forum')");
#else
        Application.OpenURL(m_linkToMoreInfos);
#endif
    }
}
