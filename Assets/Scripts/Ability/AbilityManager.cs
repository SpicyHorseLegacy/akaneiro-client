using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AbilityManager : MonoBehaviour {
	
	public AbilityAnimationManager AbiAniManager;
	//[HideInInspector]
	public List<AbilityBaseState> Abilities = new List<AbilityBaseState>();
	
	public BaseAttackableObject player;
	
	void Awake ()
	{
		AbiAniManager = transform.GetComponent<AbilityAnimationManager>();
	}

    // add skill to owner
    public virtual void AddSkill(int _skillID)
    {
        foreach (AbilityBaseState ability in Abilities)
        {
            if (ability.id == _skillID)
                return;
        }
		if(AbilityInfo.Instance)
		{
	        AbilityBaseState _newability = AbilityInfo.Instance.newAbilityByIDAndOwner((uint)_skillID, player.ObjType);
	        if (_newability != null)
	        {
	            _newability.SetOwner(player);
	            _newability.Initial();
	            _newability.transform.parent = transform;
	            Abilities.Add(_newability);
	        }
		}
    }
	
	public virtual void SetAllAbilities ()
	{
        AddSkill((int)AbilityIDs.NormalAttack_1H_ID);

        AbilityBaseState[] abilities = transform.GetComponentsInChildren<AbilityBaseState>();
        foreach (AbilityBaseState ability_state in abilities)
        {
            ability_state.SetOwner(player);
            ability_state.Initial();

            if (player != Player.Instance)
            {
                Abilities.Add(ability_state);
            }
        }
	}
	
	// restore all abilities to the default ones.
	public virtual void SetAllDefaultSkills(){	}
	
	/// <summary>
	/// 通过ID获取到目前这个Manager中的技能
	/// </summary>
	/// <param name="id">
	/// A <see cref="System.UInt32"/>
	/// </param>
	/// <returns>
	/// A <see cref="AbilityBaseState"/>
	/// </returns>
	public AbilityBaseState GetAbilityByID(uint id)
	{
		if(id == 0) return null;
		
		foreach(AbilityBaseState ability in Abilities)
		{
			if(ability.id == id)
			{
				return ability;
			}
		}
		
		return null;	
	}

    public virtual void UseAbilityFromID(int _id)
    {
        foreach(AbilityBaseState _ability in Abilities)
        {
            if (_ability.id == _id)
            {
				if(_ability.Info.ManaCost <= Player.Instance.AttrMan.Attrs[EAttributeType.ATTR_CurMP])
				{
	                if (!player.FSM.IsInState(_ability))
	                    player.FSM.ChangeState(_ability);
	                else
	                    _ability.Enter();
				}
            }
        }
    }

    #region Handle_Message_From_Server
    public void on_BeginAbility(SUseSkillResult useSkillResult)
	{
		//ability result
		foreach(AbilityBaseState ability_state in Abilities)
		{
			if(ability_state.id == useSkillResult.skillID)
			{
				ability_state.UseAbilityOK(useSkillResult);
				return;
			}
		}
	}
	
	public void On_UseAbilityResult(SUseSkillResult useSkillResult)
	{
		//ability result
		foreach(AbilityBaseState ability_state in Abilities)
		{
			if(ability_state.id == useSkillResult.skillID)
			{
				ability_state.UseAbilityResult(useSkillResult);
				return;
			}
		}
	}
	
	public void On_UseAbilityFailed(uint skillID, EServerErrorType reason)
	{
		foreach(AbilityBaseState ability_state in Abilities)
		{
			if(ability_state.id == skillID)
			{
				ability_state.UseAbilityFailed(skillID,reason);
				return;
			}
		}
    }
    #endregion
}
