using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class _UI_CS_AbilitiesTrainer_Armor : _UI_CS_AbilityTrainer_Mastery
{
    public static _UI_CS_AbilitiesTrainer_Armor Instance;

    protected override void Awake()
    {
        base.Awake();

        Instance = this;
    }

    public override void ResetAllItems()
    {
        base.ResetAllItems();

        foreach (SingleMastery _info in MasteryInfo.Instance.Masteries)
        {
            if (_info.MasteryType == EnumMasteryClass.Armor)
            {
                UpdateMasteryInfoByID(_info);
            }
        }
    } 
}
