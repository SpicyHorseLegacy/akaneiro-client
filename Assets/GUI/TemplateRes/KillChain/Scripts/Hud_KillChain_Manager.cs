using UnityEngine;
using System.Collections;

public class Hud_KillChain_Manager : MonoBehaviour
{
    public static Hud_KillChain_Manager Instance;

    [SerializeField] UISprite Bonus_Sprite;
    [SerializeField] UILabel Label_BonusXP;
    [SerializeField] UISprite Sprite_Blood;
    [SerializeField] UILabel Label_KillCount;

    [SerializeField] int GoodFlag;
    [SerializeField] int LethalFlag;
    [SerializeField] int SlayerFlag;

    int KillChain = 0;
    bool m_isStartChain = false;

    void Awake() { Instance = this; }

    public void InitKillChain()
    {
        Label_KillCount.alpha = 0;
        Sprite_Blood.alpha = 0;
        Label_BonusXP.alpha = 0;
        Bonus_Sprite.alpha = 0;
    }

    public void StartKillChain(int _chaincount)
    {
        KillChain = _chaincount - 1;
        m_isStartChain = true;
    }

    public void EndKillChain()
    {
        KillChain = 0;
        m_isStartChain = false; 
    }

    public void UpdateKillChain()
    {
        KillChain++;
        UpdateKillChain(KillChain);
    }

    public void UpdateKillChain(int _chaincount)
    {
        if (!m_isStartChain) return;

        Label_KillCount.text = "" + _chaincount.ToString() + " Kill" + (_chaincount > 1 ? "s" : "");
        Label_KillCount.alpha = 1;
        Sprite_Blood.alpha = 1;

        TweenAlpha.Begin(Label_KillCount.gameObject, 0.5f, 0);
        Label_KillCount.GetComponent<TweenAlpha>().delay = 1;

        TweenAlpha.Begin(Sprite_Blood.gameObject, 0.5f, 0);
        Sprite_Blood.GetComponent<TweenAlpha>().delay = 1;
    }

    public void UpdateBonus(int _chaincount, int _xp)
    {
        if (_chaincount >= SlayerFlag)
            Bonus_Sprite.spriteName = "KillRank_Slayer";
        else if (_chaincount >= LethalFlag)
            Bonus_Sprite.spriteName = "KillRank_Lethal";
        else if(_chaincount >= GoodFlag)
            Bonus_Sprite.spriteName = "KillRank_Good";
        else
            return;

        Label_BonusXP.text = "" + _xp + " XP";

        Label_BonusXP.alpha = 1;
        Bonus_Sprite.alpha = 1;

        TweenAlpha.Begin(Label_BonusXP.gameObject, 0.5f, 0);
        Label_BonusXP.GetComponent<TweenAlpha>().delay = 1;

        TweenAlpha.Begin(Bonus_Sprite.gameObject, 0.5f, 0);
        Bonus_Sprite.GetComponent<TweenAlpha>().delay = 1;
    }
}
