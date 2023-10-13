using UnityEngine;
using System.Collections;

public class UI_CoolDownCycle_Control : MonoBehaviour {

    [SerializeField]
    UIFilledSprite CycleSprite;

    [SerializeField]
    UILabel CurTimeLabel;

    UI_TypeDefine.UI_LearnAbilityCoolDown_data data;
    bool isStart = false;
    double curcooldown;

    void Update()
    {
        if (isStart && curcooldown > 0 && data != null)
        {
            curcooldown -= Time.deltaTime;
            if (curcooldown < 0)
            {
                CooldDownFinish();
                return;
            }
            CycleSprite.fillAmount = Mathf.Clamp01((float)curcooldown / data.MaxAbiCooldown);
            CurTimeLabel.text = converTimeToString((int)curcooldown);
        }
    }

	public void UpdateAllInfo(UI_TypeDefine.UI_LearnAbilityCoolDown_data _tempdata)
    {
        data = _tempdata;
        if (data != null)
        {
            curcooldown = data.CurAbiCooldown;
        }
        CycleSprite.fillAmount = Mathf.Clamp01(_tempdata.CurAbiCooldown / _tempdata.MaxAbiCooldown);
        CurTimeLabel.text = converTimeToString((int)curcooldown);
    }

    public void StartCoolDown()
    {
        isStart = true;
    }

    public void StopCoolDown()
    {
        isStart = false;
    }

    public double GetCurCooldown()
    {
        return curcooldown;
    }

    void CooldDownFinish()
    {
        isStart = false;
        curcooldown = 0;
        gameObject.SetActive(false);
    }

    string converTimeToString(int time)
    {
        string _timestring = "";
        if (time / 3600 > 0) _timestring += time / 3600 + "H ";
        if (time % 3600 / 60 > 0 || time / 3600 > 0) _timestring += time % 3600 / 60 + "M ";
        _timestring += time % 3600 % 60 + "S";
        return _timestring;
    }
}
