using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class _UI_CS_AbilitiesTrainer_Weapon : _UI_CS_AbilityTrainer_Mastery
{
	public static _UI_CS_AbilitiesTrainer_Weapon Instance;

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
            if (_info.MasteryType == EnumMasteryClass.Weapon)
            {
                UpdateMasteryInfoByID(_info);
            }
        }
    }
}
