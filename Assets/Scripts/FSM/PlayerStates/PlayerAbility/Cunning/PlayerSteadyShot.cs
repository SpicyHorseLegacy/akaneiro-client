using UnityEngine;
using System.Collections;

public class PlayerSteadyShot : PlayerAbilityShot {

    protected bool gotKeyboardUp = false;           // to control if player has released the key and player click the key again before ability doesn't finish, function should not do again.
    private bool bEnterFromMeleeAttack = false;

    public override void Enter()
    {
        base.Enter();

        // bEnterFromMeleeAttack is true, there is only one way. Player pressed shot ability button during meleeattack state and released the key also.
        if (bEnterFromMeleeAttack)
        {
            if (targetObjID > 0 && CS_SceneInfo.Instance.GetTargetByID(targetObjID))
            {
                Owner.GetComponent<PlayerMovement>().LookAtPosition(CS_SceneInfo.Instance.GetTargetByID(targetObjID).transform.position);
            }
            else
            {
                Owner.GetComponent<PlayerMovement>().LookAtTargetMouseInShootMode(tempMousePos);
            }
            eventAfterMounting = EventAfterMount.Shoot;
            step = PrepareStep.WaitForServerCallback;
        }
        else
        {
            gotKeyboardUp = false;
        }
        Player.Instance.CanActiveAbility = false;
    }

    public override void Execute()
    {
        base.Execute();

        if (step == PrepareStep.WaitForMouseDown || step == PrepareStep.WaitForMouseUp || step == PrepareStep.WaitForReleaseKey)
        {
            // if not shoot, character always look at mousepoint on ShootLayer
            Owner.GetComponent<PlayerMovement>().LookAtShootDir();

            if (step == PrepareStep.WaitForMouseDown)
            {
                if (Input.GetMouseButtonDown(mouseIndex))
                {
                    startCharging();
                    step = PrepareStep.WaitForMouseUp;
                }
            }
            if (step == PrepareStep.WaitForMouseUp || step == PrepareStep.WaitForReleaseKey)
            {
                // preform charging
                float MaxPowerUpTime = ChargingTime;
                if (PowerUpTime < MaxPowerUpTime)
                {
                    PowerUpTime += Time.deltaTime;
                    if (PowerUpTime > MaxPowerUpTime)
                    {
                        PowerUpTime = MaxPowerUpTime;

                        if (ChargeEffect != null)
                        {
                            ChargeEffect.GetComponent<DestructAfterTime>().DestructNow();
                        }

                        //play fully charge particle
                        if (FullyChargedEffectPrefab)
                        {
                            FullyChargedEffect = CS_Main.Instance.SpawnObject(FullyChargedEffectPrefab) as Transform;
                            FullyChargedEffect.position = Bow.position;
                            FullyChargedEffect.rotation = Bow.rotation;
                        }
                        if (ChargedEffectPrefab)
                        {
                            ChargedEffect = CS_Main.Instance.SpawnObject(ChargedEffectPrefab) as Transform;
                            ChargedEffect.parent = Bow;
                            ChargedEffect.position = Bow.position;
                            ChargedEffect.rotation = Bow.rotation;
                        }

                        shootProcess = EnumSteadyShootProcess.IdleAfterCharge;
                    }
                }

                if (Input.GetMouseButtonUp(mouseIndex)) //shoot
                {
                    tempMousePos = Input.mousePosition;
                    //eventAfterMounting = EventAfterMount.Shoot;
                    //Shoot();
                    //return;

                    if (shootProcess == EnumSteadyShootProcess.Mount)
                    {
                        eventAfterMounting = EventAfterMount.Shoot;
                        step = PrepareStep.WaitForServerCallback;
                    }
                    else
                    {
                        eventAfterMounting = EventAfterMount.Shoot;
                        Shoot();
                    }
                    /*
                    if (eventAfterMounting == EventAfterMount.StartedCharging)
                    {
                        eventAfterMounting = EventAfterMount.Shoot;
                        Shoot();
                    }
                    else
                    {
                        //if (eventAfterMounting != EventAfterMount.Idle)
                        {
                            eventAfterMounting = EventAfterMount.Shoot;
                            step = PrepareStep.WaitForServerCallback;
                        }
                    }
                    */
                }
            }
        }
    }

    public override void Exit()
    {
        base.Exit();

        bEnterFromMeleeAttack = false;
        Player.Instance.CanActiveAbility = true;
    }

    public override void UseAbilityOK(SUseSkillResult useSkillResult)
    {
        CS_SceneInfo.Instance.On_UpdateAttribution(Owner, this, useSkillResult.attributeChangeVec, false);

        //server would call back twice, the 2nd call is damage. Starting cooldown should happen this time.
        GetUseAbilityOKInfomationCount++;
        if (GetUseAbilityOKInfomationCount == 2 && Ability_UI_Button)
            Ability_UI_Button.AbilitieCoolDownStart();
    }

	public override void AcitveAbilityWithMousePos (Vector3 mousePos)
	{
		base.AcitveAbilityWithMousePos (mousePos);

        if (bEnterFromMeleeAttack) return;

        //Debug.Log("1111");
		eventAfterMounting = EventAfterMount.Charging;

        bISKeyBoardActive = true;
		step = PrepareStep.WaitForReleaseKey;

        mouseIndex = 0;
        if (Input.GetMouseButton(1))
        {
            bISKeyBoardActive = false;
            step = PrepareStep.WaitForMouseUp;
            mouseIndex = 1;
        }
	}
	
	public override void UIKeyboardKeyUP ()
	{
        if (Player.Instance.FSM.IsInState(Player.Instance.abilityManager.GetAbilityByID((uint)AbilityIDs.NormalAttack_1H_ID)))
        {
            print("active steadyshoot ability in melee attack state");
            bEnterFromMeleeAttack = true;
            tempMousePos = Input.mousePosition;
            return;
        }

		if(gotKeyboardUp) 	return;
		
		base.UIKeyboardKeyUP ();
		
		tempMousePos = Input.mousePosition;
        if (shootProcess == EnumSteadyShootProcess.Mount)
        {
            eventAfterMounting = EventAfterMount.Shoot;
            step = PrepareStep.WaitForServerCallback;
        }
        else
        {
            eventAfterMounting = EventAfterMount.Shoot;
            Shoot();
        }
        /*
		if(eventAfterMounting == EventAfterMount.StartedCharging)
		{
			eventAfterMounting = EventAfterMount.Shoot;
			Shoot();
		}else{
            if (eventAfterMounting != EventAfterMount.Idle)
            {
                eventAfterMounting = EventAfterMount.Shoot;
                step = PrepareStep.WaitForServerCallback;
            }
		}
        */
		
		gotKeyboardUp = true;
	}

    public override void MountFinished()
    {
        SendUseAbilityMessageToServer(0);
        base.MountFinished();
    }
}
