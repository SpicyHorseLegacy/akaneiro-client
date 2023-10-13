using UnityEngine;
using System.Collections;

public class AbilityInQueue {
	
	public BaseObject source;
    public DelayedAbilityBaseState Ability;
	
	public int QueueID;
    public bool IsPlayImpactSound = true;
	
	public SUseSkillResult abilityResult;
	
	public AbilityInQueue(DelayedAbilityBaseState ability)
	{
		Ability = ability;
	}
	
	public void SetAbiliyResult(SUseSkillResult result)
	{
		abilityResult = result;
	}
	
	public void CalculateResult()
	{
        if (abilityResult != null && source != null)
        {
            CS_SceneInfo.Instance.On_UpdateResult(Ability, abilityResult);
		}
		if(Ability != null)
			Ability.RemoveAbilityInQueue(this);
	}
}
