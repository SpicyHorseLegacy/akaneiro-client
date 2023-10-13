using UnityEngine;
using System.Collections;

public class ChaInfo_LvGroup_Manager : MonoBehaviour
{

    void Awake()
    {
        _BarState = EnumNameBarState.ShowName;
    }

    #region Interface

    enum EnumNameBarState
    {
        NONE,
        ShowName,
        ShowEXP,
        MAX,
    }

    [SerializeField]  private UILabel LV_value_Label;
    [SerializeField]  private UILabel Name_value_Label;
	[SerializeField]  private UILabel Exp_value_Label;
    [SerializeField]  private NGUISlider Exp_Bar;
    
    private EnumNameBarState _BarState;
    private string _Name;
    private float _CurExp;
    private float _MaxExp;

    public void UpdateLV(int _lv)
    {
        LV_value_Label.text = "" + _lv;
    }

    public void UpdateName(string _name)
    {
        _Name = _name;
        if(_BarState == EnumNameBarState.ShowName)
            Name_value_Label.text = _name;
    }

    public void UpdateEXP(float _curexp, float _maxexp)
    {
		Exp_value_Label.text = "" + (int)_curexp + " / " + _maxexp;
        _CurExp = _curexp;
        _MaxExp = _maxexp;
        Exp_Bar.sliderValue = _curexp/_maxexp;

        if(_BarState == EnumNameBarState.ShowEXP)
            Name_value_Label.text = "" + _CurExp + " / " + _MaxExp;
    }

    public void UpdateLvEntireGroup(int _lv, string _name, float _curexp, float _maxexp)
    {
        UpdateLV(_lv);
        UpdateName(_name);
        UpdateEXP(_curexp, _maxexp);
    }

    #endregion

    #region local

    #region BTN callback

    void EXPBTNPressed()
    {
        switch (_BarState)
        {
            case EnumNameBarState.ShowName:
                {
                    _BarState = EnumNameBarState.ShowEXP;
                    Name_value_Label.text = "" + _CurExp + " / " + _MaxExp; 
                }
                break;
            case EnumNameBarState.ShowEXP:
                {
                    _BarState = EnumNameBarState.ShowName;
                    Name_value_Label.text = "" + _Name;
                }
                break;
        }
    }

    #endregion

    #endregion
}
