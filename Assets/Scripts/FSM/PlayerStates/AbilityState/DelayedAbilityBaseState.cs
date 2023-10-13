using UnityEngine;
using System.Collections;

public class DelayedAbilityBaseState : AbilityBaseState {

	/// <summary>
    /// ŒÆËãµ±Ç°ResultµÄœá¹û 
    /// </summary>
    public virtual void CalculateResult()
    {
        if (AbilityQueue != null)
            AbilityQueue.CalculateResult();
    }

    // ÓÃÓÚ¿ØÖÆÑÓ³ÙµÄŽúÂë --------------------------------------------------------------------------------------------------
    // ÊÍ·ÅÒ»žöŒŒÄÜ£¬ŸÍ»ážøžÃŒŒÄÜÔöŒÓÒ»žöAbilityInQueueµÄÊµÀý£¬×÷ÓÃÊÇµÈŽýÊÕµœ·þÎñÆ÷»ØÖŽ£¬ÓÃÓÚŽ¢ŽæAbilityResult
    // Èç¹ûµœžÃŒÆËãAbilityResultœá¹ûµÄÊ±ºòŸÍœøÐÐŒÆËã£¬²¢ÇÒÉŸ³ýÕâžöÊµÀý
    // Èç¹ûÊÕµœ»ØÖŽÑÓ³ÙÁË£¬ÔÚÐèÒªŒÆËãAbilityResultÖ®ºó£¬ÔòÍš¹ýIDÅÐ¶ÏÊÇ·ñ¶ÔÓŠµÄAbilityInQueueÊÇ·ñ»¹ŽæÔÚ£¬²»ŽæÔÚŸÍÖ±œÓŒÆËãAbilityResultÁË
    // Í¬Ò»Ê±ŒäÖ»ÓÐÒ»žöÊµÀý£¬ÒòÎªŒŒÄÜ¶ŒÊÇŽ®ÐÐµÄ£¬²»¿ÉÄÜÍ¬Ê±ÊÍ·ÅÒ»žöŒŒÄÜ
    public AbilityInQueue AbilityQueue;
    public int queueID;
    public int receivedID;

    /// <summary>
    /// News the ability in queue.
    /// </summary>
    public void NewAbilityInQueue()
    {
        AbilityInQueue abilityqueue = new AbilityInQueue(this);
        abilityqueue.source = playerController;
        abilityqueue.QueueID = queueID;
        abilityqueue.IsPlayImpactSound = IsPlayImpactSound;
        queueID++;
        if (AbilityQueue != null) AbilityQueue.CalculateResult();
        AbilityQueue = abilityqueue;

    }

    /// <summary>
    /// Removes the ability in queue.
    /// </summary>
    /// <param name='abilityqueue'>
    /// Abilityqueue.
    /// </param>
    public void RemoveAbilityInQueue(AbilityInQueue abilityqueue)
    {
        AbilityQueue = null;
    }
    // --------------------------------------------------------------------------------------------------

    public override void UseAbilityResult(SUseSkillResult useSkillResult)
    {
        receivedID++;
        if (AbilityQueue != null && AbilityQueue.QueueID == receivedID - 1)
        {
            AbilityQueue.SetAbiliyResult(useSkillResult);

            if (CS_SceneInfo.Instance.CheckIfInteractiveObjAndCalculateNow(useSkillResult))
                AbilityQueue.CalculateResult();
        }
        else
        {
            CS_SceneInfo.Instance.On_UpdateResult(this, useSkillResult);	// if there is not AbilityInQueue object, that means the received infomation is late. Calculate it now.
        }
    }

    public override void UseAbilityFailed(uint skillID, EServerErrorType reason)
    {
        base.UseAbilityFailed(skillID, reason);
        receivedID++;
        if (AbilityQueue != null) AbilityQueue.CalculateResult();
    }
	
}
