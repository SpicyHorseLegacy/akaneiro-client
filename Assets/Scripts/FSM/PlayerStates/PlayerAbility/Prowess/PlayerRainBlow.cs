using UnityEngine;
using System.Collections;

public class PlayerRainBlow : PlayerAbilityBaseState {
	
	//NpcBase npcAI;
	
	//Sound
	public Transform SoundPrefab;
		
	//VFX
	public Transform UseAbilityVFXPrefab;	

	[HideInInspector] public Transform sound;
	
	 public int HitCount=0;

	public int[] HitDamage = new int[3];
	public bool isHitEnd;
	
	public AnimationClip RainBlow_0H;
	public AnimationClip RainBlow_1H;
	public AnimationClip RainBlow_2H;
	public AnimationClip RainBlow_2HNodachi;

    bool isPlayingAnimation = false;
    float animationLength = 0;
	
	public override void Enter()
	{
		base.Enter();
		
		//init sound and vfx
		if(sound == null && SoundPrefab != null)	sound = newSoundForAbility(SoundPrefab);
		
		step = PrepareStep.WaitForMouseDown;
		
		HitCount=0;
		isHitEnd = false;
		
		Owner.GetComponent<PlayerMovement>().StopMove(false);
		
		Player.Instance.CanActiveAbility = false;
	}
	
	public override void Execute()
	{
		base.Execute();
		
        if(isPlayingAnimation)
        {
            animationLength -= Time.deltaTime;
            if(animationLength < 0){
                Player.Instance.FSM.ChangeState(Player.Instance.IS);
                isPlayingAnimation = false;
            }
        }

		if(step == PrepareStep.WaitForMouseDown || step == PrepareStep.WaitForMouseUp)
		{
			Player.Instance.GetComponent<PlayerMovement>().LookAtMousePoint(false);
#if NGUI
			if(Input.GetMouseButtonDown(0) && step == PrepareStep.WaitForMouseDown)
			{
				step = PlayerAbilityBaseState.PrepareStep.WaitForMouseUp;
			}
			if(Input.GetMouseButtonUp(0) && step == PrepareStep.WaitForMouseUp)
			{
				AcitveAbilityWithMousePos(Input.mousePosition);
			}
#else
			if(Input.GetMouseButtonUp(0) && !_UI_CS_Ctrl.Instance.m_UI_Manager.GetComponent<UIManager>().DidAnyPointerHitUI())
			{
				AcitveAbilityWithMousePos(Input.mousePosition);
			}
#endif
		}
	}

	public override void Exit()
	{

		base.Exit();
		Owner.GetComponent<PlayerMovement>().bStopMove = false;
		Player.Instance.CanActiveAbility = true;
	}
	
	public override void PrepareForAbilityWithoutKeyboardInput ()
	{
		base.PrepareForAbilityWithoutKeyboardInput ();

		if(Player.Instance.AttackTarget)
		{
			Player.Instance.GetComponent<PlayerMovement>().LookAtPosition(Player.Instance.AttackTarget.position);
			PlayRainOfBlowAnim();
            Vector3 dir = Player.Instance.AttackTarget.position - Player.Instance.transform.position;
            dir.y = 0;
            SendUseAbilityRequest((uint)id, 0, dir);
		}else{
			Player.Instance.PlayIdleAnim(true);
			step = PrepareStep.WaitForMouseDown;
		}
	}
	
	public override void AcitveAbilityWithMousePos (Vector3 mousePos)
	{
		base.AcitveAbilityWithMousePos (mousePos);

        Owner.GetComponent<PlayerMovement>().LookAtMousePoint(mousePos, false);
       
        PlayRainOfBlowAnim();
        SendUseAbilityRequest((uint)id, 0, Owner.GetComponent<PlayerMovement>().PlayerObj.forward);
        Player.Instance.CanActiveAbility = false;
		//SendUseAbilityRequest();
	}

    public override void UseAbilityResult(SUseSkillResult useSkillResult)
    {
        base.UseAbilityResult(useSkillResult);
        return;

        /*
        if (Ability_UI_Button)
            Ability_UI_Button.AbilitieCoolDownStart();

        step = PrepareStep.WaitForAnimationFinish;

        // 因为RainBlow的计算方式比较特殊，分不同次结算，所有不走base。useabilityresult函数
        receivedID ++;
        if(AbilityQueue != null && AbilityQueue.QueueID == receivedID - 1)
        	recordResult(useSkillResult);
        else
        	CS_SceneInfo.Instance.On_UpdateResult(playerController, useSkillResult);
          */
    }

    /// <summary>
    /// this is useless. Because server did some changes. Server could send results 3 times for this ability.
    /// </summary>
    /// <param name="useSkillResult"></param>
    /*
    void recordResult(SUseSkillResult useSkillResult)
	{
        //Debug.Log("RecordRainBlowResult!");
		// record 3 hit damage
		int count = 0;
		foreach(SSkillEffect se in useSkillResult.skillEffectVec)
		{
			foreach(SAttributeChange attributeChange in se.attributeChangeVec)
			{
				if(attributeChange.attributeType.Get() == EAttributeType.ATTR_CurHP)
				{
					HitDamage[count] = attributeChange.value;
					count++;
					if(count > 2) return;
				}
			}
		}
		// 如果是在第一次攻击后，但是又是最后一次攻击之前，则把已经攻击了的攻击计算了
		if(HitCount > 0)
		{
			for(int i = 0; i < HitCount ; i++)
			{
				CalculateRainBlowWithCount(i);
			}
		}
	}
    */
	
	public void PlayRainOfBlowAnim()
	{
		HitCount = 0;

        WeaponBase.EWeaponType wt = Player.Instance.EquipementMan.GetWeaponType();
		
		if(wt == WeaponBase.EWeaponType.WT_NoneWeapon)
			playerController.abilityManager.AbiAniManager.RainBlowActive = RainBlow_0H;
        else if (wt == WeaponBase.EWeaponType.WT_OneHandWeapon || wt == WeaponBase.EWeaponType.WT_DualWeapon)
			playerController.abilityManager.AbiAniManager.RainBlowActive = RainBlow_1H;	
		else if(wt == WeaponBase.EWeaponType.WT_TwoHandWeaponAxe)
			playerController.abilityManager.AbiAniManager.RainBlowActive = RainBlow_2H;
		else
			playerController.abilityManager.AbiAniManager.RainBlowActive = RainBlow_2HNodachi;
		
		// play animation
		// if the backtoidle evnent is not at the end of animation and player keep using the same ability, the next animation request would come before the first animation finished.
		// if don't restore the animation, the 2nd animation won't play, but just crossfade the idle animation, so player enters ability state and keep playing the 1st animation which has actived the event
		// so player couldn't back to normal state again.
		AnimationModel.animation[playerController.abilityManager.AbiAniManager.RainBlowActive.name].time = 0;
        AnimationModel.animation[playerController.abilityManager.AbiAniManager.RainBlowActive.name].speed = 2;
		AnimationModel.animation[playerController.abilityManager.AbiAniManager.RainBlowActive.name].wrapMode = WrapMode.ClampForever;
		AnimationModel.animation.CrossFade(playerController.abilityManager.AbiAniManager.RainBlowActive.name, 0.1f);

        isPlayingAnimation = true;
        animationLength = AnimationModel.animation[playerController.abilityManager.AbiAniManager.RainBlowActive.name].length / AnimationModel.animation[playerController.abilityManager.AbiAniManager.RainBlowActive.name].speed;

		PlaySound();
	}
	
	public void PlayVFX()
	{
		if(UseAbilityVFXPrefab)
		{
			Vector3 pos = Player.Instance.transform.position + Vector3.up + Player.Instance.transform.GetComponent<PlayerMovement>().PlayerObj.forward * 1.5f;
			Quaternion rotation = Player.Instance.transform.rotation;
			Transform particle = CS_Main.Instance.SpawnObject(UseAbilityVFXPrefab, pos, rotation);
		}
	}
	
	void PlaySound()
	{
		//play sound
        if (sound)
        {
            SoundCue.Play(sound.gameObject);
        }
	}
	
	public void CalculateRainBlow()
	{
		CalculateRainBlowWithCount(HitCount);
		
		HitCount++;
		
		if(HitCount > 2){
			isHitEnd = true;
			
			// delete AbilityQueue after the last hit, meaning calculating is done
			//if(AbilityQueue != null)
			{
			//	AbilityQueue.abilityResult = null;
			//	AbilityQueue.CalculateResult();
			}
		}
	}
	
	public void CalculateRainBlowWithCount(int count)
	{
        /*
		if(Player.Instance.AttackTarget)
		{
			if(Mathf.Abs(HitDamage[count]) > 0){
                bool tempDeathFlag = Player.Instance.AttackTarget.GetComponent<NpcBase>().bNotifyDead;
                Player.Instance.AttackTarget.GetComponent<NpcBase>().bNotifyDead = false;
				Player.Instance.AttackTarget.GetComponent<BaseHitableObject>().TakeDamage(HitDamage[count], Player.Instance.transform);
				HitDamage[count] = 0;
                if (tempDeathFlag)
                {
                    Player.Instance.AttackTarget.GetComponent<NpcBase>().NotifyDie();
                }
			}
		}
        */
	}
}
