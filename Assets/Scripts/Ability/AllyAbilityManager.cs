using UnityEngine;
using System.Collections;

public class AllyAbilityManager : AbilityManager
{
    public override void SetAllAbilities()
    {
        AddSkill((int)AbilityIDs.NormalAttack_1H_ID);
    }
}
