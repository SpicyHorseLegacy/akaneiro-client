using UnityEngine;
using System.Collections;

public class UI_Hud_BloodEffect_Manager : MonoBehaviour {

    public static UI_Hud_BloodEffect_Manager Instance;

    void Awake()
    {
        Instance = this;
    }

    #region Interface

    [SerializeField]
    private UIWidget[] _widgets;

    [SerializeField]
    UISprite DeathEffect;

    #endregion

    #region Public

    public void UpdateBloodEffect(float _curhp, float _maxhp)
    {
        float _per = _curhp / _maxhp;
        float _opitical = 0;
        if (_per < 0.8 && _per >= 0.6)
        {
            _opitical = 0.25f;
        }else if(_per < 0.6 && _per >= 0.4)
        {
            _opitical = 0.5f;
        }else if(_per < 0.4 && _per >= 0.2)
        {
            _opitical = 0.75f;
        }
        else if (_per < 0.2 && _per > 0)
        {
            _opitical = 1f;
        }
        foreach (UIWidget _w in _widgets)
        {
            _w.alpha = _opitical;
        }
    }

    public void EnableDeathEffect()
    {
        TweenAlpha.Begin(DeathEffect.gameObject, 0.5f, 0.5f);
    }

    public void DisableDeathEffect()
    {
        TweenAlpha.Begin(DeathEffect.gameObject, 0.5f, 0);
    }

    #endregion

}
