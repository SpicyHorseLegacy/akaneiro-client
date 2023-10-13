using UnityEngine;
using System.Collections;

public class PlayerAbilityShot : PlayerAbilityBaseState {
    public enum EnumSteadyShootProcess
    {
        OutState,
        Mount,
        IdleBeforeCharge,
        Charging,
        IdleAfterCharge,
        Shooted,
    }

    public enum EventAfterMount
    {
        Shoot = 0,
        Idle,
        Charging,
        StartedCharging,
        HaveShoot,
    }

    public EnumSteadyShootProcess shootProcess;

	Collider ShootPlane;
	
	[HideInInspector]
	public Vector3 tempMousePos;			// record the mouse position when player actives the shoot ability

    public float JudgementRadius = 2.0f; // higher number means easier to find target.

	//Bow and Arrow
	public Transform BowPrefab;
	public Transform BowQuiverPrefab;
	public Transform ArrowPrefab;
	
	//Sound
	public Transform DrawBowSoundPrefab;
	public Transform FireSoundPrefab;
	
	//VFX
	public Transform ChargeEffectPrefab;
	public Transform ChargedEffectPrefab;
	public Transform FireEffectPrefab;
	public Transform FullyChargedEffectPrefab;
	public Transform ImpactEffectPrefab;	
	
	//Bow and Arrow
	[HideInInspector] public Transform Bow;
	[HideInInspector] public Transform BowQuiver;
	[HideInInspector] public Transform Arrow;
	[HideInInspector] public Transform ArrowSocket;
	
	//Sound
	[HideInInspector] public Transform DrawBowSound;
	[HideInInspector] public Transform FireSound;
	[HideInInspector] public Transform ImpactSound;
	
	//VFX
	[HideInInspector] public Transform ChargeEffect;
	[HideInInspector] public Transform ChargedEffect;
	[HideInInspector] public Transform FullyChargedEffect;

	[HideInInspector] public float PowerUpTime=0f;
	[HideInInspector] public  Vector3 ShootDir = Vector3.zero;
	
	public float ChargingTime = 1f;
    public int GetUseAbilityOKInfomationCount = 0;  // this ability could receive 2 callbacks. after receiving the 2nd one, calculate dmg.

    public EventAfterMount eventAfterMounting = 0;  // what should do after mounting

    protected bool bISKeyBoardActive = false;
    protected int mouseIndex = 0;

    protected int targetObjID;

    public override void Initial()
    {
        base.Initial();

        // check if there is a bow and quiver. if not, create one.
        CreateBowAndQuiver();

        //Sound
        if (!DrawBowSound && DrawBowSoundPrefab) DrawBowSound = newSoundForAbility(DrawBowSoundPrefab);
        if (!FireSound && FireSoundPrefab) FireSound = newSoundForAbility(FireSoundPrefab);

        Bow.gameObject.SetActiveRecursively(false);
        BowQuiver.gameObject.SetActiveRecursively(false);
    }
	
	public override void Enter()
	{
		base.Enter();
		
		//	Show bow, bowquiver
		Bow.gameObject.SetActiveRecursively(true);
		BowQuiver.gameObject.SetActiveRecursively(true);

        // adjust arrow position and rotation
        if (ArrowPrefab && ArrowSocket)
        {
            Arrow = CS_Main.Instance.SpawnObject(ArrowPrefab);
            Arrow.transform.parent = ArrowSocket;
            Arrow.transform.localPosition = Vector3.zero;
            Arrow.transform.localRotation = Quaternion.identity;
            Destroy(Arrow.collider);
        }

		//Reset ability variables 
		PowerUpTime=0f;
		
		// Aka stop movement
        Owner.GetComponent<PlayerMovement>().StopMove(false);

		//	hide current weapon
		Player.Instance.SetPlayerWeaponVisible(false);
	
		// Play Draw bow animation and sound
		// if the backtoidle evnent is not at the end of animation and player keep using the same ability, the next animation request would come before the first animation finished.
		// if don't restore the animation, the 2nd animation won't play, but just crossfade the idle animation, so player enters ability state and keep playing the 1st animation which has actived the event
		// so player couldn't back to normal state again.
		AnimationModel.animation[playerController.abilityManager.AbiAniManager.ShotPrepare.name].time = 0;
		AnimationModel.animation[playerController.abilityManager.AbiAniManager.ShotPrepare.name].wrapMode = WrapMode.ClampForever;
		AnimationModel.animation.CrossFade(playerController.abilityManager.AbiAniManager.ShotPrepare.name);
		Bow.animation["WP_2H_Bow_Mount"].time = 0;
		Bow.animation.CrossFade("WP_2H_Bow_Mount");
        shootProcess = EnumSteadyShootProcess.Mount;
		
		// enable Shoot Plane for aiming
		ShootPlane = Player.Instance.transform.FindChild("AbilitiesRange").collider;
		ShootPlane.transform.gameObject.layer = LayerMask.NameToLayer("AbilityShootPlane");
		ShootPlane.enabled = true;

		step = PrepareStep.WaitForMouseDown;
        eventAfterMounting = EventAfterMount.Shoot;

        GetUseAbilityOKInfomationCount = 0;
	}

	public override void Exit()
	{
		base.Exit();
		
		//stop charge VFX
		if(ChargeEffect)
			DestroyImmediate(ChargeEffect.gameObject);
		if(ChargedEffect)
            DestroyImmediate(ChargedEffect.gameObject);
		
		//show current weapon
		Player.Instance.SetPlayerWeaponVisible(true);
		
		//hide bow and bowquiver
		if(Bow)			Bow.gameObject.SetActiveRecursively(false);
		if(BowQuiver)	BowQuiver.gameObject.SetActiveRecursively(false);
        if(Arrow)       Destroy(Arrow.gameObject);
		if(ShootPlane)	ShootPlane.enabled = false;
		
		Owner.GetComponent<PlayerMovement>().bStopMove = false;

        shootProcess = EnumSteadyShootProcess.OutState;
        eventAfterMounting = EventAfterMount.Idle;
        bISKeyBoardActive = false;
        mouseIndex = 0;
        targetObjID = 0;
	}
	
    // if active this ability by clicking GUI, call this function
	public override void PrepareForAbilityWithoutKeyboardInput ()
	{
		base.PrepareForAbilityWithoutKeyboardInput ();

        step = PrepareStep.WaitForMouseDown;
		eventAfterMounting = EventAfterMount.Idle;

		AnimationModel.animation.CrossFadeQueued(playerController.abilityManager.AbiAniManager.ShotIdle.name);
		Bow.animation.CrossFadeQueued("WP_2H_Bow_Idle");
	}

	public override void UseAbilityResult(SUseSkillResult useSkillResult)
	{
		base.UseAbilityResult(useSkillResult);
		step = PrepareStep.WaitForAnimationFinish;
	}
	
	public override AbilityObject On_SkillObjectEnter(SSkillObjectEnter skillObjectInfo)
	{
        if (ArrowPrefab)
        {
            Vector3 pos = skillObjectInfo.pos;
            pos.y = Owner.position.y + 1.3f;
            Transform arrow = Instantiate(ArrowPrefab, pos, Quaternion.identity) as Transform;

            Physics.IgnoreCollision(ShootPlane, arrow.collider);
            Physics.IgnoreCollision(arrow.collider, Owner.collider);

            arrow.GetComponent<Arrow>().resetArrow();
            arrow.GetComponent<Arrow>().ObjID = skillObjectInfo.objectID;
            arrow.GetComponent<Arrow>().Shoot(skillObjectInfo.direction);
            arrow.GetComponent<Arrow>().Owner = Owner;
            arrow.GetComponent<Arrow>().DestAbility = this;
            arrow.GetComponent<Arrow>().SkillObjectInfo = skillObjectInfo;

            return arrow.GetComponent<Arrow>();
        }
        return null;
	}

    public virtual void MountFinished()
    {
        shootProcess = EnumSteadyShootProcess.IdleBeforeCharge;

        if (eventAfterMounting == EventAfterMount.Charging)
        {
            startCharging();
            if (bISKeyBoardActive)
                step = PrepareStep.WaitForReleaseKey;
            else
                step = PrepareStep.WaitForMouseUp;
        }

        if (eventAfterMounting == EventAfterMount.Shoot)
            Shoot();
    }

	// callback by event at the end of mount animation
	public virtual void Shoot()	
	{
        //Debug.Log("Shoot");
		//Stop draw bow sound
		if(DrawBowSound.audio.isPlaying)
			SoundCue.Stop(DrawBowSound.gameObject);
		
		//stop charge VFX
        if (ChargeEffect)
        {
            ChargeEffect.parent = null;
            ChargeEffect.GetComponent<DestructAfterTime>().DestructNow();
        }

        if (ChargedEffect)
        {
            ChargedEffect.parent = null;
            ChargedEffect.GetComponent<DestructAfterTime>().DestructNow();
        }

        #region Find_Target
        // FIND TARGET BY RAYCAST.
        // calculate shoot direction
        RaycastHit hit;
        int layer = 1 << LayerMask.NameToLayer("AbilityShootPlane");
        if (Player.Instance.HoverTarget)
        {
            Vector3 targetPos = Player.Instance.HoverTarget.position;
            targetPos = new Vector3(targetPos.x, ShootPlane.transform.position.y, targetPos.z);
            ShootDir = (targetPos - new Vector3(Arrow.transform.position.x, ShootPlane.transform.position.y, Arrow.transform.position.z)).normalized;
        }
        else
        {
            Ray ray = Camera.main.ScreenPointToRay(tempMousePos);
            if (Physics.Raycast(ray.origin, ray.direction, out hit, 100f, layer))
            {
                Debug.DrawLine(hit.point, hit.point + Vector3.up * 5, Color.white, 3f);
                Vector3 targetPos = hit.point;
                targetPos = new Vector3(hit.point.x, ShootPlane.transform.position.y, hit.point.z);
                ShootDir = (targetPos - new Vector3(Arrow.transform.position.x, ShootPlane.transform.position.y, Arrow.transform.position.z)).normalized;
            }
        }

        targetObjID = 0;
        layer = 1 << LayerMask.NameToLayer("NPC") | 1 << LayerMask.NameToLayer("Breakable") | 1 << LayerMask.NameToLayer("InteractiveOBJ");
        //layer |= 1 << LayerMask.NameToLayer("Collision") | 1 << LayerMask.NameToLayer("DashThroughWall") | 1 << LayerMask.NameToLayer("Walkable") | 1 << LayerMask.NameToLayer("Default");

        Vector3 ori = (Player.Instance.GetComponent<PlayerMovement>().pointOnTheGround(Owner.position) + Vector3.up * (0.15f + Player.Instance.GetComponent<CharacterController>().radius * JudgementRadius));
        if (Physics.Raycast(ori, ShootDir.normalized, out hit, JudgementRadius, layer))
        {
            if (hit.transform.GetComponent<BaseObject>() && hit.transform.GetComponent<BaseObject>().ObjType != ObjectType.Ally && hit.transform.GetComponent<BaseObject>().ObjType != ObjectType.Player)
            {
                targetObjID = hit.transform.GetComponent<BaseObject>().ObjID;
            }
        }

        if (targetObjID <= 0)
        {
            for (int i = 0; i < 20; i++)
            {
                Vector3 point1 = Player.Instance.GetComponent<PlayerMovement>().pointOnTheGround(Owner.position + ShootDir.normalized * i) + Vector3.up * (0.15f + Player.Instance.GetComponent<CharacterController>().radius * JudgementRadius);
                Vector3 point2 = Player.Instance.GetComponent<PlayerMovement>().pointOnTheGround(Owner.position + ShootDir.normalized * (i + 1)) + Vector3.up * (0.15f + Player.Instance.GetComponent<CharacterController>().radius * JudgementRadius);
                Vector3 dir = point2 - point1;

                Debug.DrawLine(point1, point2, new Color(0, 0, (1 / 20.0f) * i), 3);

                if (Physics.CapsuleCast(point1, point1 + Vector3.up, Player.Instance.GetComponent<CharacterController>().radius * JudgementRadius, dir, out hit, 1, layer))
                {
                    //Debug.Log("Shoot : " + hit.transform.name);
                    Debug.DrawLine(new Vector3(Owner.position.x, ShootPlane.transform.position.y, Owner.position.z), hit.point, Color.yellow, 3);

                    PlayImpactEffectAtPos(hit.point);

                    if (hit.transform.GetComponent<BaseObject>() && hit.transform.GetComponent<BaseObject>().ObjType != ObjectType.Ally && hit.transform.GetComponent<BaseObject>().ObjType != ObjectType.Player)
                    {
                        targetObjID = hit.transform.GetComponent<BaseObject>().ObjID;
                    }
                    break;
                }

                // because if point2 is much higher than point1, capsulecast doesn't work fine. so recheck if there is a wall at the height of point2.
                if (Mathf.Abs(point1.y - point2.y) > 0.5f)
                {
                    point1.y = point2.y;
                    dir = point2 - point1;
                    if (Physics.CapsuleCast(point1, point1 + Vector3.up, Player.Instance.GetComponent<CharacterController>().radius * JudgementRadius, dir, out hit, 1, layer))
                    {
                        //Debug.Log("Shoot : " + hit.transform.name);
                        Debug.DrawLine(new Vector3(Owner.position.x, ShootPlane.transform.position.y, Owner.position.z), hit.point, Color.yellow, 3);

                        PlayImpactEffectAtPos(hit.point);

                        if (hit.transform.GetComponent<BaseObject>() && hit.transform.GetComponent<BaseObject>().ObjType != ObjectType.Ally && hit.transform.GetComponent<BaseObject>().ObjType != ObjectType.Player)
                        {
                            targetObjID = hit.transform.GetComponent<BaseObject>().ObjID;
                        }
                        break;
                    }
                }
            }
        }
        #endregion

        PlayFireAnimation();
		
		// send shoot message to server second time. that means fire.
        SendUseAbilityMessageToServer(targetObjID);
		
		eventAfterMounting = EventAfterMount.HaveShoot;
        shootProcess = EnumSteadyShootProcess.Shooted;
        step = PrepareStep.WaitForServerCallback;
	}
	
	public void CreateBowAndQuiver()
	{
		if(Bow && BowQuiver && ArrowSocket)
			return;
		
		if(AnimationModel)
		{
			Component[] all = AnimationModel.GetComponentsInChildren<Component>();
			foreach(Component T in all)
			{
				if(T.name == "Bip001 Prop2")
				{
					Bow = CS_Main.Instance.SpawnObject(BowPrefab);
					Bow.transform.parent = T.transform;
					Bow.transform.localPosition = Vector3.zero;
					Quaternion rotation = Quaternion.identity;
					Vector3 angle = rotation.eulerAngles;
					angle.x = 0f;
					rotation.eulerAngles = angle;
					Bow.transform.localRotation = rotation;						
	
					Bow.animation["WP_2H_Bow_Mount"].layer = -1;
					Bow.animation["WP_2H_Bow_Mount"].wrapMode = WrapMode.Once;
					Bow.animation["WP_2H_Bow_Idle"].layer = -1;
					Bow.animation["WP_2H_Bow_Idle"].wrapMode = WrapMode.Loop;
					Bow.animation["WP_2H_Bow_Shoot"].layer = -1;
					Bow.animation["WP_2H_Bow_Shoot"].wrapMode = WrapMode.Once;
					Bow.animation["WP_2H_Bow_Charging"].layer = -1;
					Bow.animation["WP_2H_Bow_Charging"].wrapMode = WrapMode.Once;
				}
				if(T.name == "Bip001 Prop1")
				{
					ArrowSocket = T.transform;
				}
				if(T.name == "Bip001 Pelvis")
				{
					BowQuiver = CS_Main.Instance.SpawnObject(BowQuiverPrefab);
					BowQuiver.transform.parent = T.transform;
					BowQuiver.transform.localPosition = Vector3.zero;
					BowQuiver.transform.position += Vector3.up * 0.3f;
					BowQuiver.transform.localRotation = Quaternion.identity;
				}
				if(Bow && BowQuiver && ArrowSocket)
				{
					break;
				}
			}
		}
	}
	
	public void startCharging()
	{
		print("Start Charging!");

        shootProcess = EnumSteadyShootProcess.Charging;

        AnimationModel.animation.CrossFade(playerController.abilityManager.AbiAniManager.ShotCharging.name);
        AnimationModel.animation.CrossFadeQueued(playerController.abilityManager.AbiAniManager.ShotIdle.name);
		Bow.animation.CrossFade("WP_2H_Bow_Charging");
		SoundCue.Play(DrawBowSound.gameObject);
		
		if(ChargeEffect == null && Bow)
		{
			ChargeEffect = CS_Main.Instance.SpawnObject(ChargeEffectPrefab) as Transform;
			ChargeEffect.position = Bow.position;
			ChargeEffect.rotation = Bow.rotation;
			ChargeEffect.parent = Bow;
		}
		
		// player can't active another ability
		Player.Instance.CanActiveAbility = false;
		
		eventAfterMounting = EventAfterMount.StartedCharging;
	}
	
	/// <summary>
	/// Sends the use ability message to server.
	/// </summary>
	public void SendUseAbilityMessageToServer( int tarObjID )
	{
        //print("SendMessage!" + tarObjID + " || Count :" + GetUseAbilityOKInfomationCount + " || event : " + eventAfterMounting.ToString() + " || step : " + step.ToString());
        SendUseAbilityRequest((uint)id, (uint)tarObjID, ShootDir);
	}
	
	public override void SendUseAbilityRequest(uint skill_id, uint target_id, Vector3 skill_pos)
    {
        //send use ability request to server
        if (CS_Main.Instance != null)
        {
            CS_Main.Instance.g_commModule.SendMessage(ProtocolBattle_SendRequest.UseFightSkill(skill_id, target_id, skill_pos, CS_SceneInfo.Instance.SyncTime));
        }
    }
	
	/// <summary>
	/// 朝某个方向射箭
	/// </summary>
	/// <param name="dir">
	/// A <see cref="Vector3"/>
	/// </param>
	public virtual void PlayFireAnimation()
	{
		//Play Shoot animation
		AnimationModel.animation[playerController.abilityManager.AbiAniManager.ShotFire.name].time = 0;
		AnimationModel.animation[playerController.abilityManager.AbiAniManager.ShotFire.name].wrapMode = WrapMode.ClampForever;
		AnimationModel.animation.CrossFade(playerController.abilityManager.AbiAniManager.ShotFire.name);
		Bow.animation.CrossFade("WP_2H_Bow_Shoot",0f);
		
		//Play shoot sound
		SoundCue.Play(FireSound.gameObject);
		
		//Play shoot VFX
		Vector3 pos = Owner.position + Vector3.up * 1.2f + AnimationModel.transform.forward;
		Transform fire = CS_Main.Instance.SpawnObject(FireEffectPrefab, pos, AnimationModel.transform.rotation);
		
		step = PrepareStep.WaitForAnimationFinish;
	}

    public void PlayImpactEffectAtPos(Vector3 pos)
    {
        if (ImpactEffectPrefab)
        {
            Transform impact = Instantiate(ImpactEffectPrefab, pos, Quaternion.identity) as Transform;
            impact.forward = impact.position - Owner.position;
        }

        if (!ImpactSound && ImpactSoundPrefab) ImpactSound = newSoundForAbility(ImpactSoundPrefab);
        if (ImpactSound)
        {
            ImpactSound.position = pos;
            SoundCue.Play(ImpactSound.gameObject);
        }
    }
}
