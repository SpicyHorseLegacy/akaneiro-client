using UnityEngine;
using System.Collections;

public class MonsterAbilityManager : AbilityManager
{
    public override void SetAllAbilities()
    {
        base.SetAllAbilities();

        if (player.ObjType == ObjectType.Enermy)
        {
            NpcBase _npc = (NpcBase)player;
            foreach (NpcBase.NpcAttackProperty _attackproperty in _npc.AttackState.AttackArray)
            {
                switch (_attackproperty.ElementalDamageType)
                {
                    case NpcBase.ElemDamageKind.Flame:
                        AddSkill(30001);
                        break;
                    case NpcBase.ElemDamageKind.Frost:
                        AddSkill(30002);
                        break;
                    case NpcBase.ElemDamageKind.Explosion:
                        AddSkill(30003);
                        break;
                    case NpcBase.ElemDamageKind.Storm:
                        AddSkill(30004);
                        break;
                }
            }
        }

    }
}
