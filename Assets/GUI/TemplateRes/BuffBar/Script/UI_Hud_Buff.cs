using UnityEngine;
using System.Collections;

public class UI_Hud_Buff : MonoBehaviour {

    [SerializeField]  UISprite Icon;
    [SerializeField]  UILabel Label_StackNum;
    [SerializeField]  UILabel Label_CountDown;

    BaseBuff m_basebuff;

    public int ID
    {
        get
        {
            if (m_basebuff != null)
            {
                return m_basebuff.ID;
            }
            return 0;
        }
    }
    bool m_isForeverBuff = false;

    void Update()
    {
        if (m_basebuff != null && !m_isForeverBuff)
        {
            updateTime();
        }
    }

    public void UpdateBuff(BaseBuff _buff)
    {
        m_basebuff = _buff;
        Icon.spriteName = BuffInfo.GetIconByID(m_basebuff.ID).name;
        Label_StackNum.text = "" + m_basebuff.StackNum;
        Label_StackNum.gameObject.SetActive(m_basebuff.StackNum > 0);
        m_isForeverBuff = m_basebuff.LifeTime < 0;
        if (m_isForeverBuff)
        {
            Label_CountDown.text = "";
        }
        else
        {
            updateTime();
        }
    }

    void updateTime()
    {
        int _min = (int)(m_basebuff.CurLife / 60);
        int _sec = (int)m_basebuff.CurLife % 60;
        Label_CountDown.text = "" + _min + " : " + _sec;
    }
	
}
