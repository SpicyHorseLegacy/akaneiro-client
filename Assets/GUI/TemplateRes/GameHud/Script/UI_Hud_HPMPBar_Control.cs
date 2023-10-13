using UnityEngine;
using System.Collections;

public class UI_Hud_HPMPBar_Control : MonoBehaviour
{
    #region Interface

    [SerializeField]  UIFilledSprite BarValue;
    [SerializeField]  UIFilledSprite Bar_Effect_Value;
    [SerializeField]  UILabel LabelValue;
	
	float _targetPersent;
	
	void Update()
	{
		if(Mathf.Abs(Bar_Effect_Value.fillAmount - _targetPersent) > 0.01f)
		{
			Bar_Effect_Value.fillAmount = Mathf.Lerp(Bar_Effect_Value.fillAmount, _targetPersent, Time.deltaTime * 0.75f);
		}else
		{
			Bar_Effect_Value.fillAmount = _targetPersent;
		}
	}
	
    public void UpdateAllInfo(float _curValue, float _maxValue)
    {
        LabelValue.text = ((int)_curValue).ToString();
        BarValue.fillAmount = _curValue / _maxValue;
        _targetPersent = _curValue / _maxValue;
    }
    #endregion
}
