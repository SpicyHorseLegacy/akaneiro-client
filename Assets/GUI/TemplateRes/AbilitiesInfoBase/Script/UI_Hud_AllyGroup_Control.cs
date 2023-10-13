using UnityEngine;
using System.Collections;

public class UI_Hud_AllyGroup_Control : MonoBehaviour {

    [SerializeField]  UITexture Portrait;
    [SerializeField]  UILabel LevelLabel;
    [SerializeField]  NGUISlider HPBar;
    [SerializeField]  NGUISlider MPBar;

    public UI_TypeDefine.UI_GameHud_AllyInfo_data DATA { get { return _Data; } }
    private UI_TypeDefine.UI_GameHud_AllyInfo_data _Data;

    public void UpdateAllInfo(UI_TypeDefine.UI_GameHud_AllyInfo_data _data)
    {
        Portrait.mainTexture = _data.PortraitTex;
        LevelLabel.text = _data.Level.ToString();
        UpdateHP(_data.CurHP, _data.MAXHP);
        UpdateMP(_data.CurMP, _data.MAXMP);
        _Data = _data;
    }

    public void UpdateHP(float _curHP, float _maxHP)
    {
        _Data.CurHP = _curHP;
        _Data.MAXHP = _maxHP;
        HPBar.sliderValue = _curHP / _maxHP;
    }

    public void UpdateMP(float _curMP, float _maxMP)
    {
        _Data.CurMP = _curMP;
        _Data.MAXMP = _maxMP;
        MPBar.sliderValue = _curMP / _maxMP;
    }

}
