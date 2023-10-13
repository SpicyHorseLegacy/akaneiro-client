using UnityEngine;
using System.Collections;

public class PlayerAbilityBaseState : AbilityBaseState
{
    #region Define Variables
	
	public string AbilityName = "";
	
	public AbilityDetailInfo.EnumAbilityType AbilityType;
	
    //UI Button
	protected _UI_CS_UseAbilities Ability_UI_Button;			// 技能对应的UI按钮，只有玩家才有这个属性
	
	[HideInInspector]	public bool isCancelable;				// 如果技能是需要选择方向的状态时，如果并没有确认释放的时候是可以取消的

    public enum PrepareStep
    {
        WaitForServerCallback,
        WaitForMouseDown,
        WaitForMouseUp,
        WaitForAnimationFinish,
        WaitForMoveDone,
        WaitForReleaseKey,
    }
    public PrepareStep step;
    #endregion

    #region Override Enter-Execute-Exit
    public override void Enter ()
	{
		base.Enter ();
		isCancelable = true;
	}
	
	public override void Execute ()
	{
        base.Execute();
		
		if(step == PrepareStep.WaitForAnimationFinish)
		{
			if(!AnimationModel.animation.isPlaying)
			{
				Player.Instance.FSM.ChangeState(Player.Instance.IS);
			}
		}
	}
	
	public override void Exit ()
	{
		base.Exit ();
		
		Player.Instance.CanActiveAbility = true;
	}
    #endregion

    #region Connect With Server
    /// <summary>
    /// if player send an ability using request to server, and server reply to cliend if it's applied.
    /// this result contains mp costing infomation.
    /// 如果玩家使用一个技能，会发送请求给服务器，如果服务器同意使用这个技能，就会回调这个函数，结果包括扣除的MP。
    /// </summary>
    /// <param name="useSkillResult"></param>
	public override void UseAbilityOK (SUseSkillResult useSkillResult)
	{
		base.UseAbilityOK (useSkillResult);

        CS_SceneInfo.Instance.On_UpdateAttribution(Owner, this, useSkillResult.attributeChangeVec, false);
		
		if(Ability_UI_Button)	Ability_UI_Button.AbilitieCoolDownStart();

        string battleInfo = "Ability : " + name + "!!";
        BattleInfoPanel.Instance.ADD_Info(battleInfo);
	}
	
    /// <summary>
    /// if this ability is in progress on server side. after a certain time, server send back a result of this ability.
    /// there is damage, buffs in the result.
    /// </summary>
    /// <param name="useSkillResult"></param>
	public override void UseAbilityResult(SUseSkillResult useSkillResult)
	{
        base.UseAbilityResult(useSkillResult);
        CS_SceneInfo.Instance.On_UpdateResult(this, useSkillResult);
	}
	
    /// <summary>
    /// if server denied the ability request, client would get this callback.
    /// generally, player should change state to idle.
    /// 如果服务器拒绝了这个技能请求，则会回调这个函数，一般来说，就会退出这个技能状态
    /// </summary>
    /// <param name="skillID"></param>
    /// <param name="reason"></param>
	public override void UseAbilityFailed(uint skillID, EServerErrorType reason)
	{
        base.UseAbilityFailed(skillID, reason);

        Debug.Log("use ability failed : " + reason.GetString() );

        if(!Player.Instance.FSM.IsInState(Player.Instance.DS))
		    Player.Instance.FSM.ChangeState(Player.Instance.IS);
	}

    /// <summary>
    ///	发送使用技能的信息给服务器
    /// </summary>
    /// <param name='skill_id'>
    /// 所要使用的技能ID
    /// </param>
    /// <param name='target_id'>
    /// 目标的ID，如果没有目标就是0
    /// </param>
    /// <param name='skill_pos'>
    /// 使用技能的位置，如果是指向性技能就是方向，如果是定点技能，就是定点的位置
    /// </param>
    public virtual void SendUseAbilityRequest(uint skill_id, uint target_id, Vector3 skill_pos)
    {
        //send use ability request to server
        if (CS_Main.Instance != null)
        {
            CS_Main.Instance.g_commModule.SendMessage(ProtocolBattle_SendRequest.UseFightSkill(skill_id, target_id, skill_pos, CS_SceneInfo.Instance.SyncTime));
            step = PrepareStep.WaitForServerCallback;
        }
    }
    #endregion

    #region Input Functions
    /// <summary>
    /// this is the smarter way to cast ability.
    /// because if this ability needs to choose direction or a position, choosing where mouse position instead automatically.
	/// 直接激活技能，通过鼠标的位置来控制技能的方向和位置，主要是用于智能施法的情况
	/// </summary>
	/// <param name='mousePos'>
	/// Mouse position. 
	/// </param>
	public virtual void AcitveAbilityWithMousePos(Vector3 mousePos){}

    /// <summary>
    /// this function should be called when player release keyboard keys for abilities.
    /// </summary>
    public virtual void UIKeyboardKeyUP() { }
	
	/// <summary>
	/// if player active the ability by clicking GUI, this function should be called.
    /// some abilities need to choose direction or postion.
    /// 如果玩家激活这个技能通过点击UI上的按钮，则走这个Function，因为有些技能是需要选择方向和位置的
	/// </summary>
	public virtual void PrepareForAbilityWithoutKeyboardInput(){}
    #endregion

    #region Helper Functions
    /// <summary>
    /// bind the ability to a Button on UI.
    /// </summary>
    /// <param name="button"></param>
	public void SetAbilityButton(_UI_CS_UseAbilities button)
	{
		Ability_UI_Button = button;
	}

    /// <summary>
    /// 在跑动时释放了“可以在跑动中释放的技能”，跑到目标点后，进行处理
    /// </summary>
    public virtual void PlayerAnimationAfterRun() { }

    public bool IsManaOK()
    {
        return Player.Instance.AttrMan.Attrs[EAttributeType.ATTR_CurMP] >= Info.ManaCost;
    }
    #endregion
}
