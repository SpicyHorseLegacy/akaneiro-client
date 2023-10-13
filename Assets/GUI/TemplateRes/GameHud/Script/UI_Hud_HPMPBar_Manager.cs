using UnityEngine;
using System.Collections;

public class UI_Hud_HPMPBar_Manager : MonoBehaviour {

    public static UI_Hud_HPMPBar_Manager Instance;

    void Awake()
    {
        Instance = this;
    }

    #region Interface

    [SerializeField]
    UI_Hud_HPMPBar_Control HPBar;
    [SerializeField]
    UI_Hud_HPMPBar_Control MPBar;

    public void UpdateHPMP(float _curHP, float _maxHP, float _curMP, float _maxMP)
    {
        HPBar.UpdateAllInfo(_curHP, _maxHP);
        MPBar.UpdateAllInfo(_curMP, _maxMP);
    }

    #endregion
}
