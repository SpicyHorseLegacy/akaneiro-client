using UnityEngine;
using System.Collections;

public class UI_CharInfo_Tooltips_AbiDetailGroup : UI_CharInfo_Tooltips_Abi
{
    #region Interface

    [SerializeField]
    UILabel AbiDetailLabel;

    public void UpdateAllInfo(string _description, int _energy)
    {
        string _finalTXT = _description;
        _finalTXT += "\n[00ffdb]Energy Required : " + _energy + "[-]";
    }

    public override void UpdateAllInfo(CharInfo_Tooltips_Ability_Data _data)
    {
        base.UpdateAllInfo(_data);

        string _finalTXT = _data.Description;
        _finalTXT += "\n[00ffdb]Energy Required : " + _data.EnergyRequired + "[-]";
        if (_data.BuffName.Length > 0)
        {
            _finalTXT += "\n[ff0002]" + _data.BuffName + "[-]";
            if (_data.BuffDescription.Length > 0)
                _finalTXT += "\n" + _data.BuffDescription;
        }
        AbiDetailLabel.text = _finalTXT;
    }

    #endregion
}
