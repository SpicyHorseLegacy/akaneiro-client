using UnityEngine;
using System.Collections;

public class NPC_RainOfBlow : NPCAbilityBaseState
{
    public override void Enter()
    {
        base.Enter();

        //Vector3 dir = Vector3.zero;
        //if (Player.Instance)
        //    dir = Vector3.Normalize(Player.Instance.transform.position - Owner.position);
        //Owner.rotation = Quaternion.LookRotation(dir);
    }

    public override void UseAbilityOK(SUseSkillResult useSkillResult)
    {
        base.UseAbilityOK(useSkillResult);
        Owner.rotation = Quaternion.LookRotation(useSkillResult.pos);
    }

    public override void UseAbilityResult(SUseSkillResult useSkillResult)
    {
        base.UseAbilityResult(useSkillResult);
    }
}
