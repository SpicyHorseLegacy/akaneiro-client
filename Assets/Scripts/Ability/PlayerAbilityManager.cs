using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerAbilityManager : AbilityManager
{
    SPlayerSkillInfo PlayerSkillInfo;

	// record the ability slot bar info.
	int[] AbilitySlots = new int[6];
	
    void Start()
    {
		SetAllDefaultSkills();
		for(int i = 0; i < Abilities.Count; i++)
		{
			Abilities[i].Info = null;
			AbilityDetailInfo _tempinfo = AbilityInfo.GetAbilityDetailInfoByID(Abilities[i].id);
			if(_tempinfo != null)
			{
				Abilities[i].Info = _tempinfo;
				Abilities[i].SetOwner(player);
			}
		}
    }
	
	public PlayerAbilityBaseState GetAbilityByType(AbilityDetailInfo.EnumAbilityType _abitype)
	{
		for(int i = 0; i < Abilities.Count; i++)
		{
			PlayerAbilityBaseState _ability = (PlayerAbilityBaseState)Abilities[i];
			if(_ability.AbilityType == _abitype)
				return _ability;
		}
		return null;
	}
	
	public int GetAbilityIDInSlot(int _index)
	{
		if(_index < 0 || _index >= AbilitySlots.Length) return 0;
		return AbilitySlots[_index];
	}
	
	public void SetAbilitySlot(int _index, int _abiID)
	{
		if(_index < 0 || _index >= AbilitySlots.Length) return;
		AbilitySlots[_index] = _abiID;
	}

    public void InitAbilitiesBySKillInfo(SPlayerSkillInfo _playerskilInfo)
    {
        PlayerSkillInfo = _playerskilInfo;
        foreach (SSkillInfo skillinfo in PlayerSkillInfo.skillInfoVec)
        {
            AddSkill(skillinfo.skillID);
        }
#if NGUI
		// set ability slot bar.
		AbilitySlots = new int[6];
		foreach(SSkillShortcut _skillslot in PlayerSkillInfo.skillShortcutVec)
		{
			if(_skillslot.groupIdx == 0)
				SetAbilitySlot(_skillslot.idx, _skillslot.skillID);
			if(_skillslot.groupIdx == 1)
				SetAbilitySlot(_skillslot.idx + 3, _skillslot.skillID);
		}
#endif
    }

    public override void SetAllAbilities()
    {
        AddSkill((int)AbilityIDs.NormalAttack_1H_ID);
    }

    public override void AddSkill(int _skillID)
    {
		foreach (AbilityBaseState ability in Abilities)
        {
            if (ability.id == _skillID)
                return;
        }
		
		if(AbilityInfo.Instance)
		{
	        PlayerAbilityBaseState _newability = (PlayerAbilityBaseState)AbilityInfo.Instance.newAbilityByIDAndOwner((uint)_skillID, player.ObjType);
	        if (_newability != null)
	        {
	            _newability.SetOwner(player);
	            _newability.Initial();
	            _newability.transform.parent = transform;
	            
				// try to find the same type ability, and replace it.
				bool _hasSameAbility = false;
				for(int i = 0; i < Abilities.Count; i ++)
        		{
					PlayerAbilityBaseState _playerabi = (PlayerAbilityBaseState)Abilities[i];
					if(_playerabi.AbilityType == _newability.AbilityType)
					{
						UnityEngine.Object.Destroy(Abilities[i].gameObject);
						Abilities[i] = _newability;
						_hasSameAbility = true;
						break;
					}
				}
				
				if(!_hasSameAbility)
					Abilities.Add(_newability);
	        }
		}
    }
	
	public override void SetAllDefaultSkills()
	{
		// remove all skills
		for(int i = 0; i < Abilities.Count;i++)
		{
			UnityEngine.Object.Destroy(Abilities[i].gameObject);
		}
		Abilities.Clear();
		
		// add all default skills
		for(int i = (int)AbilityDetailInfo.EnumAbilityType.SwathOfDestruction; i <= (int)AbilityDetailInfo.EnumAbilityType.DarkHunter; i+= 100)
		{
			AddSkill(i);
		}
		AddSkill((int)AbilityDetailInfo.EnumAbilityType.Revive);
		AddSkill((int)AbilityDetailInfo.EnumAbilityType.ReviveSuper);
		AddSkill((int)AbilityIDs.NormalAttack_1H_ID);
	}

    #region Use Ability
    /// <summary>
    /// 这个函数只有Aka主角能用上，只有在有操作才会进入这里
    /// </summary>
    /// <param name="AbilityObjectUI">
    /// A <see cref="_UI_CS_UseAbilities"/>
    /// </param>
    public void UseAbility(_UI_CS_UseAbilities AbilityObjectUI, bool isKeyBoardInput)
    {
        UseAbility(AbilityObjectUI, Input.mousePosition, isKeyBoardInput);
    }

    public void UseAbility(_UI_CS_UseAbilities AbilityObjectUI, Vector3 mousePos, bool isKeyBoardInput)
    {
        int abilityID = AbilityObjectUI.m_abilitiesInfo.m_AbilitieID;
        PlayerAbilityBaseState abi = (PlayerAbilityBaseState)GetAbilityByID((uint)abilityID);
        abi.SetAbilityButton(AbilityObjectUI);

		UseAbility(abilityID, mousePos, isKeyBoardInput);
    }
	
	public void UseAbility(int _abiid, bool isKeyBoardInput)
	{
		UseAbility(_abiid, Input.mousePosition, isKeyBoardInput);
	}
	
	public void UseAbility(int _abiid, Vector3 mousePos, bool isKeyBoardInput)
	{
		uint abilityID = (uint)_abiid;

        // if can't active ability, return
        if (!player.GetComponent<Player>().CanActiveAbility) return;

        PlayerAbilityBaseState abi = (PlayerAbilityBaseState)GetAbilityByID(abilityID);
        //print(abi.transform.name);

        // if there is no ability as ID, return
        if (abi == null || abi.Info == null)
            return;

        // if it's already in this ablity state, return 
        if (player.GetComponent<Player>().FSM.IsInState(abi))
            return;

        // if player is in stun or knockback state
        if (player.GetComponent<Player>().FSM.IsInState(Player.Instance.SS) || player.GetComponent<Player>().FSM.IsInState(Player.Instance.KS))
            return;

        // if it's in attack, check if could active ability after attack animation
        if (player.ObjType == ObjectType.Player && player.GetComponent<Player>().FSM.IsInState(GetAbilityByID((uint)AbilityIDs.NormalAttack_1H_ID)))
        {
            MeleeAttackState attackstate = (MeleeAttackState)GetAbilityByID((uint)AbilityIDs.NormalAttack_1H_ID);
            // if get true, that means player could active ability after attack
            if (!attackstate.CanUseNewAbility(_abiid, isKeyBoardInput))
                return;
        }

        // if Energy is not enough, return
        if (player.ObjType == ObjectType.Player && player.AttrMan.Attrs[EAttributeType.ATTR_CurMP] < abi.Info.ManaCost)
        {
            //Debug.Log("[Ability] Not enough Energy!!!");
            return;
        }
		
        // player would enter InRunState if only active ability by clicking GUI
        // player enter the new state without stopping running. And player starts to do the prepare animation when player reaches the target position.
        if (!isKeyBoardInput)
        {
            if (abilityID == (int)AbilityIDs.SwathOfDestruction_ID ||
                abilityID == (int)AbilityIDs.SwathOfFlame_ID ||
                abilityID == (int)AbilityIDs.SwathOfDestructionIII_ID ||
                abilityID == (int)AbilityIDs.SwathOfDestructionIV_ID ||
                abilityID == (int)AbilityIDs.SwathOfDestructionV_ID ||
                abilityID == (int)AbilityIDs.SwathOfDestructionVI_ID ||

                abilityID == (int)AbilityIDs.NinjaEscape_I_ID ||
                abilityID == (int)AbilityIDs.NinjaEscape_II_ID ||
                abilityID == (int)AbilityIDs.NinjaEscape_III_ID ||
                abilityID == (int)AbilityIDs.NinjaEscape_IV_ID ||
                abilityID == (int)AbilityIDs.NinjaEscape_V_ID ||
                abilityID == (int)AbilityIDs.NinjaEscape_VI_ID)
            {
                if (player.transform.GetComponent<PlayerMovement>().IsMoving)
                {
                    player.GetComponent<Player>().FSM.ChangeStateInRunstate(abi);
                    return;
                }
            }
        }
        player.GetComponent<Player>().FSM.ChangeState(abi);

        if (isKeyBoardInput)
        {
            abi.AcitveAbilityWithMousePos(mousePos);
        }
        else
        {
            abi.PrepareForAbilityWithoutKeyboardInput();
        }
	}
	
	public void KeyboardKeyUp(int _abiid)
	{
		PlayerAbilityBaseState abi = (PlayerAbilityBaseState)GetAbilityByID((uint)_abiid);

        if (abi)
            abi.UIKeyboardKeyUP();
	}
	
    public void KeyboardKeyUp(_UI_CS_UseAbilities AbilityObjectUI)
    {
        int abilityID = AbilityObjectUI.m_abilitiesInfo.m_AbilitieID;

        KeyboardKeyUp(abilityID);
    }
    #endregion
}

[System.Serializable]
public class PlayerAbilityClientRule_Single
{
    public AbilityDetailInfo.EnumAbilityType AbiType;
    public AbilityDetailInfo AbiInfo;
    public string IconName;
    public string Name;
    public int CurLEVEL;
}
 