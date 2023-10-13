using UnityEngine;
using System.Collections;

public class UI_CharInfo_Tooltips_AbiNameGroup : UI_CharInfo_Tooltips_Abi
{
    #region Interface

    [SerializeField]
    UILabel AbiNameLabel;

    [SerializeField]
    UILabel LevelLabel;

    public override void UpdateAllInfo(CharInfo_Tooltips_Ability_Data _data)
    {
        base.UpdateAllInfo(_data);

        AbiNameLabel.text = _data.AbiName;
        AbiNameLabel.color = _data.AbiColor;
        LevelLabel.text = "Rank : " + _data.CurLevel + " / " + _data.MaxLevel;
        LevelLabel.color = _data.AbiColor;
    }

    #endregion
}
