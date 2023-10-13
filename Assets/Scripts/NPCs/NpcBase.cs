using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class NpcBase : BaseAttackableObject {
	
	public string NpcName;
//	public bool   isBoss = false;

    public int maxHealth;

    public bool IsRandomRotationSpawn = true;

    public bool IsBoss = false;
	public bool IsWanted = false;
    public bool IsObjective = false;
    public int Poweress = 10;
    public int Fortitude = 10;
    public int Cunning = 10;
	public int  RewardWantedExp;
	public int  RewardWantedSk;
	
	//[HideInInspector]
	public bool bAIDisable = false;
	
	public int EnemyLevel = 1;
	
	public int LootLevel = 1;
	
	public int  TypeID = 0;
	
	public enum EInitStateType
	{
		Idle=0,
		Wander,
		Attack,
		Sleep,
		Spawn,
	}
	
	[HideInInspector]
	public EInitStateType InitState = EInitStateType.Idle;		
	
	//health bar
	float min_nearClipPlane = 0f;
	float max_nearClipPlane = 1.0f;
	public bool showHealth = true;
	public bool showHealthValue = true;
	SpriteText DamageTextTip;
	Vector3 screenPos = new Vector3();
	
	//pathfinding
	[HideInInspector]
	public Vector3 [] pathPoints;
	[HideInInspector]
	public int curPathPointIndex=-1;
	[HideInInspector]
	public bool bFindPath=false;
	[HideInInspector]
	public bool bReachTargetPoint=false;
	[HideInInspector]
	public bool IsPathfinding=false; 
	[HideInInspector]
	public bool bMovable=true;
	[HideInInspector]
	public int AroundIndex=-1;
	
	
	//search Player
	public float VisionAngle=360f;
	public float VisionRadius=20f;
	public float AlertDistance=20f;

    [HideInInspector]
    public Transform EnemyEffect;
	
	public int CriticalDeathThreshold = 0;
	
	public Transform CriticalDeathFX;
	
	[System.Serializable]
	public class StatusChange
	{
		public bool CanStun = false;
		public bool CanPoison = false;
		public bool CanSlow   = false;
		public bool CanVampiricDrain = false;
		public bool CanKnockback = false;
		public bool CanFear = false;
	}
	
	public enum ElemDamageKind
	{
		None = -1,
		Flame = 0,
		Frost,
		Explosion,
		Storm,
		
	}
	
	public StatusChange StatusImmunity;
	
	[System.Serializable]
	public class ElementDamageCondition
	{
		public ElemDamageKind ElementalDamageType;
		public float          ElementalDamageModifier;
	}
	
	public ElementDamageCondition[] ElementalDamages;
	
	public int MinKarmaValue = 1;
	
	public int MaxKarmaValue = 100;
	
	public Transform[] KarmaPerfabs = new Transform[0];
	
	public float GeneralAnimBlendTime=0.1f;
	
	//public float Acceleration=2f;
	public int XP = 50;
	[HideInInspector]
	public Vector3 fixPoint;
	
	[HideInInspector]
	public NpcSpawner MySpawner;
	[HideInInspector]
	public Transform Owner;
	
	//wander state 
	public float SpeedWanderState=1.0f;
	public float TurnSpeedWanderState=90.0f;
	public float WanderTime=5f;
	public float IdleTime=2f;
	public int IdleChance=50;
	[HideInInspector]
	public Transform[] WanderPoints;
	
	//flee state
	[System.Serializable]
	public class FleeProperty
	{
		public int FleeWhenGetDeathAlertChance =0;
		public int FleeWhenDamagedChance = 0;
		public float SpeedFleeState = 5f;
		public float TurnSpeedFleeState = 5f;
		public float FleeTime = 3.5f;
	}
	
	public FleeProperty flee_state;
	
	//attack state
	public int AttackOnEnemySightChance=100;
	public int AttackOnDamageChance=100; 
	public int AttackOnAlertChance=100;
	[HideInInspector]
	public int GoToWanderAfterAttackChance=0;
	[HideInInspector]
	public int GoToWanderWhenOutOfRangeChance=0;
	[HideInInspector]
	public int WanderWhenOutOfVisionChance= 0;
	

	public enum EAttackBehavior
	{
		AttackByChance=0,
		AttackBySequence,
	}
	
	[System.Serializable]
	public class NpcAttackProperty
	{
		public int AttackChance = 100;
		public int AnimIndexAttack=0;
		public float ResetTime= 0.5f;
		public int AttackDamage=10;
		//public int AttackAngle=120;
		public int HitChance=100;
		public int AttackCount=1;
		//public float AttackRadiusOffset=1f;
		public float MinAttackDistance=1f;
		public float MaxAttackDistance=1f;
		public float TurnSpeedAttackAnim=0f;
		public ElemDamageKind ElementalDamageType;
		public Transform ProjectilePrefab;
	}
	
	[System.Serializable]
	public class ProjectileProperty
	{
		public float ProjectileSpeed = 0f;
		public Transform ChargeEffectParticle;
	}

	[System.Serializable]
	public class AttackStateProperty
	{
        public float SpeedAttackState = 5.0f;
		//public float SpeedBonus = 1;
		public float TurnSpeedAttackState = 12f;
		public EAttackBehavior AttackBehavior;
		public NpcAttackProperty[] AttackArray;
		//public ProjectileProperty[] ProjectileArray;
	}
    public float DPS
	{
		get
		{
			float tempDPS = 0;
	        for(int i = 0; i < AttackState.AttackArray.Length; i++)
	        {
	            NpcAttackProperty attackProperty = AttackState.AttackArray[i];
	            tempDPS += (attackProperty.AttackDamage / AttackAnims[i].length) * (attackProperty.AttackChance / 100.0f);
	        }
			return tempDPS;
		}
	}
	
	public AttackStateProperty AttackState; 
	[HideInInspector]
	public int CurAttackPropertyIndex = 0;
	protected Item drop;
	
	//damage particle
	public Transform TakeDamageEffect;
	public Transform TakeDamageEffect1;
	public Transform TakeDamageEffect2;
	
	
	[HideInInspector]
	public NpcGlobalState NGS;
	[HideInInspector]
	public NPC_SpawnState SpawnState;
	[HideInInspector]
	public NPC_SleepState SleepState;
	[HideInInspector]
	public NPC_AlertState AlertState;	
	[HideInInspector]
	public NPC_WakupState WakeupState;
	[HideInInspector]
	public NPC_IdleState IS;
	[HideInInspector]
	public NPC_WanderState WS;
	[HideInInspector]
	public NPC_DeathState DS;
	[HideInInspector]
	public NPC_NormalMeleeAttackState AS;
	[HideInInspector]
	public NPC_ChaseState CS;
	[HideInInspector]
	public Npc_ForceWindState ForceWindState;
	[HideInInspector]
	public NPC_StunState StunState;
	[HideInInspector]
	public NPC_KnockBackState KnockbackState;
	[HideInInspector]
	public Npc_FleeState FleeState;
	
	[HideInInspector]
	public Vector3 SpawnPoint=Vector3.zero;
	
	public float MovableRadius=30f;
	
	public float WanderRadius=5f;
	
	[HideInInspector]
	public bool IsRangedNpc=false;
	
	//damage state
	[HideInInspector]
	public bool IsShadowBladeDamageEffect=false;
	[HideInInspector]
	public float ShadowBladeDamageEffectTime=0f;
	[HideInInspector]
	public bool IsForceWindEffect=false;
	[HideInInspector]
	public float ForceWindEffectTime=0f;
	[HideInInspector]
	public Vector3 ForceWindDir;
	
	//animation clip
	public AnimationClip[] SpawnAnims;
	public AnimationClip SleepAnim;
	public AnimationClip WakeupAnim;
	public AnimationClip AlertAnim;
	public AnimationClip IdleAnim;
	public AnimationClip AttackIdleAnim;
	public AnimationClip RunAnim;
	public AnimationClip WalkAnim;
	public AnimationClip TurnLeftAnim;
	public AnimationClip TurnRightAnim;
	public AnimationClip DamageAnim;
	public AnimationClip[] DeathAnims = new AnimationClip[0];
	public AnimationClip StunAnim;
	public AnimationClip KnockBackAnim;

	public AnimationClip[] AttackAnims;
	
	[HideInInspector]
	public int SpawnAnimIndex=0;
	[HideInInspector]
	public int SleepAnimIndex=0;
	[HideInInspector]
	public int WakeupAnimIndex=0;
	[HideInInspector]
	public int AttackAnimIndex=0;
	[HideInInspector]
	public int DeathAnimIndex = 0;
	
	public bool PlayDamageAnimation=true;
	
	//public bool mbisBoss = false;
	
	//sound
	[HideInInspector]
	public static Transform IdleSound = null;
	//[HideInInspector]
	public static Transform AttackSound = null;
	[HideInInspector]
	public Transform DeathSound;
	[HideInInspector]
	public Transform PainSound = null;
	[HideInInspector]
	public Transform BodyFallSound;
	[HideInInspector]
	public Transform CommonKilledSound;
	[HideInInspector]
	public Transform BossSpawnSound;
	[HideInInspector]
    public Transform BossStartSound;
	
	public Transform DeathEffect;
	[HideInInspector]
	public Transform DeathEffectInst;
	//[HideInInspector]
	public bool bNotifyDead = false;
	
	Renderer[] NpcHeadBarRenders;
	
	public enum EAnimationStateType
	{
		AniIdle=0,
		AniChase,
		AniWander,
		AniAttack,
		AniKnockBack,
		AniSleep,
		AniSpawn,
		AniFlee,
		AniWakeUp,
		AniGoHome,
		AniMax,
	}
	
	[System.Serializable]
	public class ResetTimeStruc
	{
		public float WakeupTime = 0f;
		public float SleepResetTime = 0f;
		public float[] SpawnResetTimes;
	}
	
	[HideInInspector]
	public EAnimationStateType LastAnimationState;

    [HideInInspector]
    public int TargetObjectID
    {
        get
        {
            return _targetObjID;
        }
        set
        {
            _targetObjID = value;
            if (_targetObjID == 1)
            {
                AttackTarget = Player.Instance;
            }
            else
            {
                for (int i = 0; i < CS_SceneInfo.Instance.AllyNpcList.Count; i++)
                {
                    if (_targetObjID == CS_SceneInfo.Instance.AllyNpcList[i].ObjID)
                    {
                        AttackTarget = CS_SceneInfo.Instance.AllyNpcList[i];
                        break;
                    }
                }
            }
        }
    }
    int _targetObjID;
	[HideInInspector]
	public bool bWanderReverse = false;
	
	public DeathResult[] DeathResults = new DeathResult[0];
	
	public bool bSummonerNpc = false;
	
	public NpcSpawner.Spawner SummonSpawner = new NpcSpawner.Spawner();

	public State curstate = null;
	
	public Transform[] Weapons = new Transform[2];
	
    public ResetTimeStruc AdditinalResetTime = new  ResetTimeStruc();
	
	float mBornTime = 0;
	
	protected override void Awake()
	{
        base.Awake();

        if (GetComponent<NpcCreateModel>())
            GetComponent<NpcCreateModel>().CreateModelForOwner();
        else
            AnimationModel = transform;

        gameObject.AddComponent<Animation>();

        ObjType = ObjectType.Enermy;

        FSM = new NpcStateMachine(transform);

        // initial all state, includes ability states
        InitialState();

		FSM.SetCurrentState(IS);
		
		mBornTime = Time.time;
	}
	
	public virtual void InitialState()
	{
		GameObject states = new GameObject();
		states.transform.name = "STATES";
		states.transform.parent = transform;
		
		NGS = states.AddComponent<NpcGlobalState>();
		NGS.SetNPC(this);
		NGS.Enter();
		FSM.SetGlobalState(NGS);
		
		SpawnState = states.AddComponent<NPC_SpawnState>();					SpawnState.SetNPC(this);
		SleepState = states.AddComponent<NPC_SleepState>();					SleepState.SetNPC(this);
		AlertState = states.AddComponent<NPC_AlertState>();					AlertState.SetNPC(this);
		WakeupState = states.AddComponent<NPC_WakupState>();				WakeupState.SetNPC(this);
		IS = states.AddComponent<NPC_IdleState>();							IS.SetNPC(this);
		WS = states.AddComponent<NPC_WanderState>();						WS.SetNPC(this);
		DS = states.AddComponent<NPC_DeathState>();							DS.SetNPC(this);
		CS = states.AddComponent<NPC_ChaseState>();							CS.SetNPC(this);
		ForceWindState = states.AddComponent<Npc_ForceWindState>();			ForceWindState.SetNPC(this);
		StunState = states.AddComponent<NPC_StunState>();					StunState.SetNPC(this);
		KnockbackState = states.AddComponent<NPC_KnockBackState>();			KnockbackState.SetNPC(this);
		FleeState = states.AddComponent<Npc_FleeState>();					FleeState.SetNPC(this);

        if (abilityManager)
            abilityManager.SetAllAbilities();

		AS = (NPC_NormalMeleeAttackState)abilityManager.GetAbilityByID((uint)AbilityIDs.NormalAttack_1H_ID);
	}
	
	// Use this for initialization
    public virtual void Start()
    {
        if (!AnimationModel) print("" + transform.name);
        //set animations 
        if (AnimationModel.animation != null)
        {
            AnimationModel.animation.wrapMode = WrapMode.Loop;

            foreach (AnimationClip clip in SpawnAnims)
            {
                if (clip != null)
                {
                    if (AnimationModel.animation[clip.name] != null)
                    {
                        AnimationModel.animation[clip.name].layer = 1;
                        AnimationModel.animation[clip.name].wrapMode = WrapMode.Once;
                    }
                }
            }

            if (SleepAnim != null)
            {
                if (!AnimationModel.animation[SleepAnim.name]) AnimationModel.animation.AddClip(SleepAnim, SleepAnim.name);
                AnimationModel.animation[SleepAnim.name].layer = 1;
                AnimationModel.animation[SleepAnim.name].wrapMode = WrapMode.Loop;
            }

            if (WakeupAnim != null)
            {
                if (!AnimationModel.animation[WakeupAnim.name]) AnimationModel.animation.AddClip(WakeupAnim, WakeupAnim.name);
                AnimationModel.animation[WakeupAnim.name].layer = 1;
                AnimationModel.animation[WakeupAnim.name].wrapMode = WrapMode.Once;
            }

            if (AlertAnim != null)
            {
                AnimationModel.animation[AlertAnim.name].layer = 1;
                AnimationModel.animation[AlertAnim.name].wrapMode = WrapMode.Once;
            }

            if (IdleAnim != null)
            {
                //AnimationModel.animation[IdleAnim.name].layer = -1;
            }

            if (AttackIdleAnim != null)
            {
                //AnimationModel.animation[AttackIdleAnim.name].layer = -1;
            }

            if (RunAnim != null)
            {
                //AnimationModel.animation[RunAnim.name].layer = -1;
            }

            if (WalkAnim != null)
            {
                //AnimationModel.animation[WalkAnim.name].layer = -1;
            }

            if (TurnLeftAnim != null)
            {
                //AnimationModel.animation[TurnLeftAnim.name].layer = -1;
            }

            if (TurnRightAnim != null)
            {
                //AnimationModel.animation[TurnRightAnim.name].layer = -1;
            }

            foreach (AnimationClip clip in AttackAnims)
            {
                if (clip != null)
                {
                    AnimationModel.animation[clip.name].layer = 1;
                    AnimationModel.animation[clip.name].wrapMode = WrapMode.Once;
                }
            }

            if (DamageAnim != null)
            {
                AnimationModel.animation[DamageAnim.name].layer = 1;
                AnimationModel.animation[DamageAnim.name].wrapMode = WrapMode.Once;
            }

            if (DeathAnims != null)
            {
                foreach (AnimationClip clip in DeathAnims)
                {
                    if (clip != null)
                    {
                        AnimationModel.animation[clip.name].layer = 2;
                        AnimationModel.animation[clip.name].wrapMode = WrapMode.Once;
                    }
                }
            }

            if (StunAnim != null)
            {
                AnimationModel.animation[StunAnim.name].layer = 1;
                AnimationModel.animation[StunAnim.name].wrapMode = WrapMode.Once;
            }

            if (KnockBackAnim != null)
            {
                AnimationModel.animation[KnockBackAnim.name].layer = 1;
            }
        }

        SpawnPoint = transform.position;

        if (AttackState.AttackArray.Length == 0)
        {
            AttackState.AttackArray = new NpcAttackProperty[1];
            AttackState.AttackArray[0] = new NpcAttackProperty();
        }

        Renderer[] NpcRenderers = transform.GetComponentsInChildren<Renderer>();

        foreach (Renderer NpcRenderer in NpcRenderers)
        {
            for (int i = 0; i < NpcRenderer.materials.Length; i++)
            {
                Material mtl = NpcRenderer.materials[i];

                if (mtl.HasProperty("_FogEnable"))
                {
                    mtl.SetFloat("_FogEnable", 1f);
                }

                if (mtl.HasProperty("_FogColor"))
                {
                    mtl.SetColor("_FogColor", RenderSettings.fogColor);
                }

                if (mtl.HasProperty("_FogStartDis"))
                {
                    mtl.SetFloat("_FogStartDis", RenderSettings.fogStartDistance);
                }

                if (mtl.HasProperty("_FogEndDis"))
                {
                    mtl.SetFloat("_FogEndDis", RenderSettings.fogEndDistance * CS_SceneInfo.Instance.NpcRangeFactor);
                }
            }
        }
    }

	public void Enable(bool IsEnable)
	{
		gameObject.SetActive(IsEnable);
	}
	
	// Update is called once per frame
    public virtual void Update()
    {
        curstate = FSM.GetCurrentState();

        float totalDis = Mathf.Abs(transform.position.x - Player.Instance.transform.position.x) + Mathf.Abs(transform.position.z - Player.Instance.transform.position.z);

        if (totalDis >= 40f)
        {
            if (Player.Instance.AllEnemys.IndexOf(transform) != -1)
            {
                Player.Instance.AllEnemys.Remove(transform);
            }
            return;
        }

        FSM.Update();

        if (BuffMan)
            BuffMan.Execute();
        else
            Debug.LogError("" + transform.name + " missed Buff Manager, please have a check");

        Renderer[] NpcRenderers = transform.GetComponentsInChildren<Renderer>();

        foreach (Renderer NpcRenderer in NpcRenderers)
        {
            for (int i = 0; i < NpcRenderer.materials.Length; i++)
            {
                Material mtl = NpcRenderer.materials[i];
                if (mtl.HasProperty("_FogColor"))
                {
                    mtl.SetColor("_FogColor", RenderSettings.fogColor);
                }

                if (mtl.HasProperty("_FogStartDis"))
                {
                    mtl.SetFloat("_FogStartDis", RenderSettings.fogStartDistance);
                }

                if (mtl.HasProperty("_FogEndDis"))
                {
                    mtl.SetFloat("_FogEndDis", RenderSettings.fogEndDistance * CS_SceneInfo.Instance.NpcRangeFactor);
                }
            }
        }

        if (CS_SceneInfo.Instance != null)
            CS_SceneInfo.Instance.PopNewContent(transform);
    }

    public override void TakeDamage(int damage)
    {
        base.TakeDamage(damage);

        if (damage < 0 && AttrMan.Attrs[EAttributeType.ATTR_CurHP] > 0)
        {
            AnimationModel.animation.Blend(DamageAnim.name);
            transform.GetComponent<NpcSoundEffect>().PlayPainSound();
        }

        // for normal situation, take damage first, get death notify 2nd. But for MeleeAttack / Rain of Blows, there should has a bit delay.
        if (bNotifyDead)
            FSM.ChangeState(DS);
    }
	
	public virtual void DeathAnimationFinished()
	{
		if(Player.Instance.AttackTarget == transform)
		{
			Player.Instance.AttackTarget=null;
		}
		
		HideHealthBarWhenNPCisDead();
		
		Destroy(gameObject);
	}

	//anim
	public virtual void PlaySpawnAnim()
	{
		if(SpawnAnims.Length > SpawnAnimIndex)
		{
			SpawnAnims[SpawnAnimIndex].wrapMode = WrapMode.Once;
			AnimationModel.animation.Play(SpawnAnims[SpawnAnimIndex].name,PlayMode.StopAll);

            // play spawn from below sound.
            if (SpawnAnims[SpawnAnimIndex].name.ToLower().Contains("below"))
            {
                if (transform.GetComponent<NpcSoundEffect>() && transform.GetComponent<NpcSoundEffect>().SpawnFromBelowSoundPrefab)
                {
					SoundCue.PlayPrefabAndDestroy(transform.GetComponent<NpcSoundEffect>().SpawnFromBelowSoundPrefab, transform.position);
                }
            }
			
			if(SpawnAnims[SpawnAnimIndex].name.ToLower().Contains("above"))
			{
				if (transform.GetComponent<NpcSoundEffect>() && transform.GetComponent<NpcSoundEffect>().SpawnFromAboveSoundPrefab)
                {
					SoundCue.PlayPrefabAndDestroy(transform.GetComponent<NpcSoundEffect>().SpawnFromAboveSoundPrefab, transform.position);
                }
			}
		}		
	}
	
	public virtual void PlaySleepAnim()
	{
		
	   AnimationModel.animation.CrossFade(SleepAnim.name,GeneralAnimBlendTime);
			
	}	
	
	public virtual void PlayWakeupAnim()
	{
	
	   AnimationModel.animation.CrossFade(WakeupAnim.name,GeneralAnimBlendTime);
	}	

	public virtual void PlayOnAlertAnim()
	{
		if(AlertAnim != null)
		{
			AnimationModel.animation.CrossFade(AlertAnim.name,0.01f);
		}
	}	
	
	public virtual void PlayIdleAnim()
	{
		if(FSM.IsInState(AS) || FSM.IsLastState(AS))
		{
			if(AttackIdleAnim != null)
			{
				AnimationModel.animation.CrossFade(AttackIdleAnim.name,GeneralAnimBlendTime,PlayMode.StopAll);
			}
			else
			{
				if(IdleAnim != null)
				{
					AnimationModel.animation.CrossFade(IdleAnim.name,GeneralAnimBlendTime,PlayMode.StopAll);
				}
			}
		}
		else
		{
			if(IdleAnim != null)
			{
				AnimationModel.animation.CrossFade(IdleAnim.name,GeneralAnimBlendTime,PlayMode.StopAll);
			}
		}
	}
	
	public virtual void PlayDeathAnim()
	{
		if(DeathAnims.Length > 0)
		{
			DeathAnimIndex = Random.Range(0,DeathAnims.Length);
			AnimationModel.animation.CrossFade(DeathAnims[DeathAnimIndex].name,GeneralAnimBlendTime, PlayMode.StopAll);
		}
	}
	
	public virtual void PlayWanderAnim()
	{
		if(WalkAnim != null)
		{
			//int ilayer = AnimationModel.animation[WalkAnim.name].layer;
			//Debug.Log("Walk layer" + ilayer.ToString());
			AnimationModel.animation[WalkAnim.name].wrapMode = WrapMode.Loop;
			AnimationModel.animation.CrossFade(WalkAnim.name,GeneralAnimBlendTime);
			
		}
	}
	
	public virtual void PlayTurnRightAnim()
	{
		if(TurnRightAnim != null)
		{
			AnimationModel.animation[TurnLeftAnim.name].wrapMode = WrapMode.Once;
			AnimationModel.animation.CrossFade(TurnRightAnim.name,GeneralAnimBlendTime);
		}
	}
	public virtual void PlayTurnLeftAnim()
	{
		if(TurnLeftAnim != null)
		{
			AnimationModel.animation[TurnRightAnim.name].wrapMode = WrapMode.Once;
			AnimationModel.animation.CrossFade(TurnLeftAnim.name,GeneralAnimBlendTime);
		}
	}
	
	public virtual void PlayRunAnim()
	{
		if(RunAnim != null)
		{
			//int ilayer = AnimationModel.animation[RunAnim.name].layer;
			//Debug.Log("Run layer" + ilayer.ToString());
			AnimationModel.animation[RunAnim.name].wrapMode = WrapMode.Loop;
			AnimationModel.animation.CrossFade(RunAnim.name,GeneralAnimBlendTime,PlayMode.StopAll);
			//AnimationModel.animation.Play(RunAnim.name,PlayMode.StopAll);
		}
	}
	
	void PlayImmediateRunAnim()
	{
		if(RunAnim != null)
		{
			
			AnimationModel.animation[RunAnim.name].wrapMode = WrapMode.Loop;
			AnimationModel.animation.Play(RunAnim.name);
		}
	}
	
	//attack
	public virtual void PlayAttackAnim()
	{
		//AttackAnimIndex = Random.Range(0,AttackAnims.Length);
		AttackAnimIndex = AttackState.AttackArray[CurAttackPropertyIndex].AnimIndexAttack;
		if(AttackAnims.Length > AttackAnimIndex)
		{
			//	????????????????????,???????????????,??????5?
			//if(animation.IsPlaying (AttackAnims[AttackAnimIndex].name))
			//{
			//	print("IsPlaying" + AttackAnims[AttackAnimIndex].name);
			//	animation[AttackAnims[AttackAnimIndex].name].speed = 5;
			//	
			//	var newState = animation.PlayQueued(AttackAnims[AttackAnimIndex].name);
			//	newState.speed = 5;
			//	
			//}else{
			//	animation[AttackAnims[AttackAnimIndex].name].speed = 1;
			//	animation.Play(AttackAnims[AttackAnimIndex].name);
			//}
			  //AnimationModel.animation.stop();
			 // AnimationModel.animation.Play(AttackAnims[AttackAnimIndex].name);
            AnimationModel.animation[AttackAnims[AttackAnimIndex].name].time = 0;
			AnimationModel.animation.CrossFade(AttackAnims[AttackAnimIndex].name,GeneralAnimBlendTime,PlayMode.StopAll);
			
			// play attack sound
			transform.GetComponent<NpcSoundEffect>().PlayAttackSound(AttackAnimIndex);
		}
	}

	public virtual bool IsAttackAnimFinish()
	{
		if(AttackAnims.Length > AttackAnimIndex)
		{
			return !AnimationModel.animation.IsPlaying(AttackAnims[AttackAnimIndex].name);
		}
		else
			return true;
	}
	
	
	public virtual void PlayDamageAnim()
	{
		if(!PlayDamageAnimation)
			return;
		if(FSM.IsInState(WakeupState) || FSM.IsInState(SleepState))
			return;
		
		if(RunAnim != null && AnimationModel.animation.IsPlaying(RunAnim.name))
			return;
		
		if(IsAttackAnimFinish() == false)
		    return;
		
		if(DamageAnim != null)
		{
			AnimationModel.animation.CrossFade(DamageAnim.name,GeneralAnimBlendTime);
		}
		
	}
	
	public virtual void PlayStunAnim() 
	{
		if(StunAnim != null)
		{
			AnimationModel.animation[StunAnim.name].wrapMode = WrapMode.Loop;
			AnimationModel.animation.CrossFade(StunAnim.name,GeneralAnimBlendTime);
		}
	}
	
	public virtual void PlayKnockBackAnim()
	{
		if(KnockBackAnim != null)
		{
			AnimationModel.animation[KnockBackAnim.name].wrapMode = WrapMode.Once;
			AnimationModel.animation.CrossFade(KnockBackAnim.name,GeneralAnimBlendTime);
		}
	}
	
	//death
	public virtual bool IsDeathAnimFinished() 
	{
		if (DeathAnims == null)
		{
			return true;
		}
		if (DeathAnims.Length == 0)
		{
			return true;
		}

		return !AnimationModel.animation.IsPlaying(DeathAnims[DeathAnimIndex].name);
	}
	
	public Transform PlaySound(Transform prefab, Transform sound)
	{
		if(sound==null && prefab!=null)
		{
			sound = Instantiate(prefab) as Transform;
			//Parenting to make sure the sound gets deleted when the npc gets deleted.
			//If this is undesired in some cases then remove this line but then
			//you MUST delete the sounds another way.
			sound.parent = this.transform;
		}
		
		if(sound!=null)
		{
			sound.position = transform.position;
			sound.rotation = transform.rotation;
			SoundCue.Play(sound.gameObject);
		}
		
		return sound;
	}
	
	public void HealthBarUpdate()
	{
	}
	
	public void HideHealthBarWhenNPCisDead()
	{
	}
	
	public void AnimateDamageText()
	{
	}
	
	
	public void WanderOnPath(List<Vector3> TargetToMoves,float fspeed)
	{
		//return;
		
		if(TargetToMoves.Count < 1  || bReachTargetPoint)
		{
		    //PlayIdleAnim();
			return;
		}
		
		if(curPathPointIndex >= TargetToMoves.Count -1)
		   curPathPointIndex = TargetToMoves.Count -1;
		else if(curPathPointIndex <= 0)
		   curPathPointIndex = 0;
	
		Vector3 direction = TargetToMoves[curPathPointIndex] - transform.position;
		
		direction.y = 0;
		
		
		float dis = direction.magnitude;
		
		//Debug.Log(dis);
		
		if(direction.magnitude>0f )
		{
			transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation(direction), TurnSpeedWanderState * Time.deltaTime);
		}
	     /*
		// Modify speed so we slow down when we are not facing the target
		Vector3 forward = transform.forward;
		forward.y=0;
		float speedModifier = Vector3.Dot(forward, direction.normalized);
		speedModifier = Mathf.Clamp(speedModifier,0.1f,1f);
		//Debug.Log("speedModifier: " + speedModifier);
		
		float angle = Vector3.Angle(forward,direction.normalized);
		//Debug.Log("angle : " + angle);
		
		// Move the character
		direction = forward * SpeedWanderState;// * speedModifier;
		*/
		
		direction.Normalize();
		
								
	
		
		direction =  direction * fspeed;
		
		if(fspeed*Time.deltaTime<dis)
		{
		  // Vector3 tOldPos = transform.position;
			
		   transform.position = transform.position + direction*Time.deltaTime;
		   /*
		   Vector3 forward = transform.TransformDirection(Vector3.forward);
			
		   Vector3 dir3 = transform.position - tOldPos;
			
		   dir3.y = 0;
			
		   dir3.Normalize();
			
	       float AngleDiff = Vector3.Dot(forward, dir3);
		
		   if(AngleDiff <= 0.9f)
		   {
			   Vector3 LeftOrRight = Vector3.Cross(forward,dir3);
		       if(LeftOrRight.y > 0f) // turn right
		       {
		          PlayTurnRightAnim();
		       }
			   else // turn left
		       {
			       PlayTurnLeftAnim();
			   }
			}
			*/
			
			PlayWanderAnim();
		}
		else
		{   
			
			if(curPathPointIndex >= TargetToMoves.Count-1)
			{
				
			    transform.position = TargetToMoves[curPathPointIndex];
				
				
				bReachTargetPoint=true;
				
				curPathPointIndex=0;
				
				PlayIdleAnim();
			}
			else
			{
				transform.position = TargetToMoves[curPathPointIndex];
				curPathPointIndex++;
				PlayWanderAnim();
			}
			
		}
		
		RaycastHit hit;
		if(Physics.Raycast(transform.position + Vector3.up*5f,Vector3.down,out hit,10f,1 << LayerMask.NameToLayer("Walkable")))
		{
		   transform.position = hit.point;
		}
		
		if(!AnimationModel.animation.IsPlaying(WalkAnim.name))
		{
			bool bPlay = true;
			
			if(AnimationModel.animation.IsPlaying(TurnLeftAnim.name))
				bPlay = false;
			
			if(AnimationModel.animation.IsPlaying(TurnRightAnim.name))
				bPlay = false;
			
			 if(bPlay)
		        PlayWanderAnim();
		}
		   
	}
   
	
	public void SpawnOnPath(Vector3 TargetToMove,float tSpeed)
	{	
		if(bReachTargetPoint)
		{
			return;
		}
	
		Vector3 direction = TargetToMove - transform.position;
		
		direction.y = 0;
		
		float dis = direction.magnitude;
	
		// Rotate towards the target
		float turn_speed = AttackState.TurnSpeedAttackState;
		
		if(direction.magnitude>0f )
		{
			transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation(direction), turn_speed * Time.deltaTime);
		}
		
		direction.Normalize();
		
		direction =  direction *  tSpeed;
		
		if( tSpeed * Time.deltaTime < dis)
		{	
		    transform.position =transform.position + direction*Time.deltaTime;
			
			bReachTargetPoint=false;
		}
		else
		{
			transform.position = TargetToMove;
			
			bReachTargetPoint=true;
			
			//Debug.Log("Monster " + ObjID.ToString() + " Speed is " + tSpeed.ToString()  + "have reach spawn end");
				
		    curPathPointIndex=0;
		}
		
	}
	
    Vector3 Delaygap = Vector3.zero;
	
	public void MoveOnPath(List<Vector3> TargetToMoves,float RunSpeed)
	{
		if(TargetToMoves.Count < 1 )
		{
		    //PlayIdleAnim();
			return;
		}
		
		if(bReachTargetPoint)
		{
		   // predict position for network delay
			/*
		   if(Player.Instance.AttrMan.Attrs[EAttributeType.ATTR_CurHP] <= 0)
		   {
			  if(!FSM.IsInState(IS))
			      FSM.ChangeState(IS);
			  return;
		   }
			
		   Transform ChaseObject = null;
			
		   if (TargetObjectID == 1)
		   {
			 ChaseObject = Player.Instance.transform;
		   }
		   else
		   {
		      for( int i = 0; i < CS_SceneInfo.Instance.AllyNpcList.Count;i++)
		      {
                if (TargetObjectID == CS_SceneInfo.Instance.AllyNpcList[i].ObjID)
			    {
				   ChaseObject =  CS_SceneInfo.Instance.AllyNpcList[i].transform;
				   break;
			    }
			  }
		   }
			
		   if(ChaseObject == null)
			  ChaseObject = Player.Instance.transform;
		   
		   AnimationModel.animation[RunAnim.name].speed = 1f;
			
		   Vector3 tempDir = ChaseObject.position - transform.position;
		   tempDir.y = 0f;
		   
		   Vector3 tempDiff = tempDir;
		   tempDir.Normalize();
		   transform.forward = tempDir;
			
		   float fStep =  CS.StateSpeed * Time.deltaTime;
		   	
		   float fdist = tempDiff.magnitude - Delaygap.magnitude;
			
		   if( fStep > fdist)
		   {
			   fStep = fdist;
			   FSM.ChangeState(IS);
		   }
	 	   else
		   {
			   //PlayIdleAnim();
		      PlayRunAnim();
		   }
		 	
	       transform.position = transform.position + fStep * tempDir;
		   
	       RaycastHit hit;
		   if(Physics.Raycast(transform.position + Vector3.up*20f,Vector3.down,out hit,20f,1 << LayerMask.NameToLayer("Walkable")))
		   {
			   transform.position = hit.point;
		   }*/
			
		   PlayIdleAnim();
	
		   return;
			
		}
		
		if( curPathPointIndex >= TargetToMoves.Count)
			curPathPointIndex = TargetToMoves.Count - 1;
		
		Vector3 direction = TargetToMoves[curPathPointIndex] - transform.position;
		
		direction.y = 0;
		
		float dis = direction.magnitude;
	
		// Rotate towards the target
		float turn_speed = AttackState.TurnSpeedAttackState;
		
		if(direction.magnitude>0f )
		{
			transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation(direction), turn_speed * Time.deltaTime);
		}
	   
		
		direction.Normalize();
		
		//direction =  direction * RunSpeed * AttackState.SpeedBonus;
        direction = direction * RunSpeed;
		
		//if( RunSpeed * AttackState.SpeedBonus * Time.deltaTime < dis)
        if(RunSpeed * Time.deltaTime < dis)
		{	
			//PlayRunAnim();
			PlayImmediateRunAnim();
			
			//if(!AnimationModel.animation.IsPlaying(RunAnim.name))
			  // PlayImmediateRunAnim();
			
		    transform.position =transform.position + direction*Time.deltaTime;
			
			bReachTargetPoint=false;
			
			
		}
		else
		{
			if(curPathPointIndex >= TargetToMoves.Count-1)
			{
				
			    transform.position = TargetToMoves[curPathPointIndex];
				
				//Debug.Log("Pathfinding pos :"+pathPoints[curPathPointIndex] + " MoveTarget : "+TargetToMove );

				//reach target point,stop move
				
				bReachTargetPoint=true;
				
				curPathPointIndex=0;
				
				//PlayIdleAnim();
			}
			else
			{
				//PlayRunAnim();
				 PlayImmediateRunAnim();
				//if(!AnimationModel.animation.IsPlaying(RunAnim.name))
				  // PlayImmediateRunAnim();
				transform.position = TargetToMoves[curPathPointIndex];
				curPathPointIndex++;
			}
		}
		
		RaycastHit hit2;
		if(Physics.Raycast(transform.position + Vector3.up*5f,Vector3.down,out hit2,10f,1 << LayerMask.NameToLayer("Walkable")))
		{
			transform.position = hit2.point;
		}
		
		
		
	
	}
	
	public void PathComplete (Vector3[] points) 
	{
		pathPoints = points;
		curPathPointIndex=0;
		bFindPath=true;
		IsPathfinding=false;
		//Debug.Log(points[0]);
	}
	
	public void PathError()
	{
		bFindPath=false;
		IsPathfinding=false;
	}
	
	public void PlayBodyFallSound()
	{
		BodyFallSound = PlaySound(GetComponent<NpcSoundEffect>().BodyFallSoundPrefab ,BodyFallSound);
	}
	
	public bool IsTooCrowd()
	{
		Player player =  Player.Instance;
		int angle = player.AroundAngle;
		angle = Mathf.Clamp(angle, 18,90);
		if(player.AllEnemys.Count>= 360/angle && player.AllEnemys.IndexOf(transform)==-1)
			return true;
		else
			return false;
	}
	
	
	public override string DoExport()
	{
		XMLStringWriter xmlWriter = new XMLStringWriter();
		
		xmlWriter.NodeBegin("Monster");
		
		xmlWriter.AddAttribute("ID",TypeID);

        xmlWriter.AddAttribute("IsBoss", IsBoss);
		
		xmlWriter.AddAttribute("Name",NpcName);

        xmlWriter.AddAttribute("AvoidanceRadius", AvoidanceRadius);
		
		xmlWriter.AddAttribute("Level",EnemyLevel);
		
		xmlWriter.AddAttribute("LootLevel",LootLevel);
		
		xmlWriter.AddAttribute("StopAI",bAIDisable);
		
		xmlWriter.AddAttribute("IsWanted",IsWanted);
		
		xmlWriter.AddAttribute("IsSummoner",bSummonerNpc);
		
		xmlWriter.AddAttribute("RewardWantedExp",RewardWantedExp);
		
		xmlWriter.AddAttribute("RewardWantedSk",RewardWantedSk);
		
		xmlWriter.AddAttribute("MaxHealth", maxHealth);
		
		xmlWriter.AddAttribute("XP",XP);
		
		xmlWriter.AddAttribute("VisionRadius",VisionRadius);
		
		xmlWriter.AddAttribute("AlertDistance",AlertDistance);
	
		xmlWriter.AddAttribute("WanderSpeed",SpeedWanderState);
		
		xmlWriter.AddAttribute("WanderTime",WanderTime);
		
		xmlWriter.AddAttribute("IdleTime",IdleTime);
		
		xmlWriter.AddAttribute("IdleChance",IdleChance);
	
		xmlWriter.AddAttribute("CanStun",StatusImmunity.CanStun);
		
		xmlWriter.AddAttribute("CanPoison",StatusImmunity.CanPoison);
		
		xmlWriter.AddAttribute("CanSlow",StatusImmunity.CanSlow);
		
		xmlWriter.AddAttribute("CanVampiricDrain",StatusImmunity.CanVampiricDrain);
		
		xmlWriter.AddAttribute("CanKnockBack",StatusImmunity.CanKnockback);
		
		xmlWriter.AddAttribute("CanFear",StatusImmunity.CanFear);
		
		xmlWriter.AddAttribute("InitialState",InitState);
		
		xmlWriter.AddAttribute("AttackSightChance",AttackOnEnemySightChance);
		
		xmlWriter.AddAttribute("AttackOnDamageChance",AttackOnDamageChance);
		
		xmlWriter.AddAttribute("AttackOnAlertChance",AttackOnAlertChance);
		
		xmlWriter.AddAttribute("RunSpeed",AttackState.SpeedAttackState);
		
		xmlWriter.AddAttribute("TurnSpeed",AttackState.TurnSpeedAttackState);
		
		xmlWriter.AddAttribute("AttackBehavior",AttackState.AttackBehavior);
		
		xmlWriter.AddAttribute("WanderRadius",WanderRadius);
			
	    xmlWriter.AddAttribute("MovableRadius",MovableRadius);
		
		xmlWriter.AddAttribute("minKarma",MinKarmaValue);
		
		xmlWriter.AddAttribute("maxKarma",MaxKarmaValue);
		
		if(WakeupAnim != null)
		{
		   xmlWriter.AddAttribute("WakeUpTime",WakeupAnim.length + AdditinalResetTime.WakeupTime);  
		}

        if (ElementalDamages != null)
        {
            foreach (ElementDamageCondition it in ElementalDamages)
            {
                xmlWriter.NodeBegin("ElementDamageNode");
                xmlWriter.AddAttribute("ElementDamageModifier", it.ElementalDamageModifier);
                xmlWriter.AddAttribute("ElementDamageType", (int)it.ElementalDamageType);
                xmlWriter.NodeEnd("ElementDamageNode");
            }
        }

        if (SpawnAnims != null && SpawnAnims.Length > 0)
        {
            for (int i = 0; i < SpawnAnims.Length; i++)  //AnimationClip it in SpawnAnims)
            {
                float addtime = 0f;
                if (AdditinalResetTime.SpawnResetTimes != null && AdditinalResetTime.SpawnResetTimes.Length > i)
                    addtime = AdditinalResetTime.SpawnResetTimes[i];

                xmlWriter.NodeBegin("SpawnAniTime");

                xmlWriter.AddAttribute("Duration", SpawnAnims[i].length + addtime);

                xmlWriter.NodeEnd("SpawnAniTime");
            }
        }

        for (int i = 0; i < KarmaPerfabs.Length; i++)
        {
            xmlWriter.NodeBegin("KarmaGenerate");

            xmlWriter.AddAttribute("KarmaID", (int)KarmaPerfabs[i].GetComponent<KarmaController>().KarmaType);

            xmlWriter.AddAttribute("Value", 1);

            xmlWriter.AddAttribute("HealingAmount", 9);

            xmlWriter.NodeEnd("KarmaGenerate");
        }
		
		if( AttackState.AttackArray != null && AttackState.AttackArray.Length > 0)
		{
			xmlWriter.NodeBegin("AttackArray");

			for( int i = 0; i < AttackState.AttackArray.Length; i++)
			{
				NpcAttackProperty it = AttackState.AttackArray[i];
				
				xmlWriter.NodeBegin("AttackProperty");
		
				xmlWriter.AddAttribute("AttackChance",it.AttackChance);
				xmlWriter.AddAttribute("AttackAnimIndex",it.AnimIndexAttack);
				xmlWriter.AddAttribute("AttackAnimDuration", it.ResetTime + AttackAnims[it.AnimIndexAttack].length);
				
#if UNITY_EDITOR
				AnimationEvent[] AETS = AnimationUtility.GetAnimationEvents( AttackAnims[it.AnimIndexAttack]);
				
				foreach(AnimationEvent its in AETS)
				{
					if(its.functionName == "AttackPlayer")
					{
						xmlWriter.AddAttribute("AttackActionTime", its.time);
					}
				}

#endif				
				xmlWriter.AddAttribute("AttackDamage",it.AttackDamage);
				xmlWriter.AddAttribute("AttackCount",it.AttackCount);
				xmlWriter.AddAttribute("ElementAttackType",    (int)it.ElementalDamageType);
				//xmlWriter.AddAttribute("ProjectileIndexAttack",it.ProjectileIndexAttack);
				xmlWriter.AddAttribute("AttackMinDistance",it.MinAttackDistance);
				xmlWriter.AddAttribute("AttackMaxDistance",it.MaxAttackDistance);
				
				xmlWriter.NodeEnd("AttackProperty");
			}
			
			xmlWriter.NodeEnd("AttackArray");
		}

        NPCAbilityBaseState[] abis = abilityManager.transform.GetComponents<NPCAbilityBaseState>();
        if (abis.Length > 0)
		{
			xmlWriter.NodeBegin("SkillArray");

            foreach (NPCAbilityBaseState abi in abis)
			{
				xmlWriter.NodeBegin("SkillProperty");

                xmlWriter.AddAttribute("ActiveTime", abi.ActiveTime);
                xmlWriter.AddAttribute("SkillID", (int)abi.id);
				xmlWriter.AddAttribute("SkillCD", (int)abi.AbilityCoolDown * 1000);
                xmlWriter.AddAttribute("SkillCDDIF", (int)abi.AbilityCoolDownDif * 1000);

                xmlWriter.AddAttribute("AbiConditionMax", (int)NPCAbilityBaseState.AbilityCondition.AbiConditionEnum.ABICONDITIONMAX);
                for (int i = 0; i < (int)NPCAbilityBaseState.AbilityCondition.AbiConditionEnum.ABICONDITIONMAX; i++)
                {
                    float _tempnum = -1;
                    foreach (NPCAbilityBaseState.AbilityCondition _abiCondition in abi.AbilityConditions)
                    {
                        if (i == (int)_abiCondition.AbiCondition)
                        {
                            _tempnum = _abiCondition.Num;
                            break;
                        }
                    }
                    xmlWriter.AddAttribute("AbiCondition" + i, _tempnum);
                }

                float anilength = 2.0f;
                if(abi.CastAnimation)
				{
					if(abi.GetType() == typeof(NPC_SkyStrike))
					{
//						Debug.LogError(abi.id);
						AbilityInfoLoader AbilityInfomation = new AbilityInfoLoader();
       					AbilityInfomation.LoadAbilityDetailInfo();
						anilength = AbilityInfomation.GetAbilityDetailInfoByID(abi.id).AnimationDuration;
					}else
					{
						anilength = abi.CastAnimation.length;
					}
				}  
                xmlWriter.AddAttribute("SkillDuration", anilength * 1000);
				xmlWriter.AddAttribute("SkillPosType", (int)abi.PosType);
                xmlWriter.AddAttribute("SkillPosOffsetX", (float)abi.PositionOffset.x);
                xmlWriter.AddAttribute("SkillPosOffsetZ", (float)abi.PositionOffset.y);
				
				xmlWriter.NodeEnd("SkillProperty");
			}
			xmlWriter.NodeEnd("SkillArray");
		}
		
        /*
		if(AttackState.ProjectileArray != null && AttackState.ProjectileArray.Length > 0)
		{
			xmlWriter.NodeBegin("ProjectileArray");
			
			foreach( ProjectileProperty it in AttackState.ProjectileArray)
			{
				xmlWriter.NodeBegin("ProjectileProperty");
				
				xmlWriter.AddAttribute("ProjectileSpeed",it.ProjectileSpeed);
				
				xmlWriter.NodeEnd("ProjectileProperty");
			}
		
			xmlWriter.NodeEnd("ProjectileArray");
		}
         */
		
		xmlWriter.NodeBegin("FleeState");
		
		xmlWriter.AddAttribute("FleeSpeed",flee_state.SpeedFleeState);
		xmlWriter.AddAttribute("FleeTime",flee_state.FleeTime);
		xmlWriter.AddAttribute("FleeOnDeathChance",flee_state.FleeWhenGetDeathAlertChance);
		xmlWriter.AddAttribute("FleeDamageChance",flee_state.FleeWhenDamagedChance);
			
		xmlWriter.NodeEnd("FleeState");
		
		if(DeathResults.Length > 0)
		{
			//print("DoExport!YMW");
			xmlWriter.NodeBegin("DeathResultArray");
			
			foreach( DeathResult dr in DeathResults )
			{
				xmlWriter.NodeBegin("DeathResultProperty");
				
				xmlWriter.AddAttribute("DeathResultType", (int)dr.Type);
				xmlWriter.AddAttribute("DeathResultID", dr.ID);
				xmlWriter.AddAttribute("DeathResultChance", dr.Chance);
				
				xmlWriter.NodeEnd("DeathResultProperty");
			}
			
			xmlWriter.NodeEnd("DeathResultArray");
		}
	    
		if(bSummonerNpc && SummonSpawner != null)
		{
	       	xmlWriter.NodeBegin("SummonSpawner");
			
			Transform newModel = null;
			
			if(SummonSpawner.BNodeCreate)
			{
			   Component[] all = transform.GetComponentsInChildren<Component>();
			   
			   if(transform.GetComponent<NpcCreateModel>())
			   {
			      if(transform.GetComponent<NpcCreateModel>().ModelPrefab != null)
				  {
					  newModel = (Transform)Object.Instantiate(transform.GetComponent<NpcCreateModel>().ModelPrefab );
						
					  if( newModel != null)
					    all = newModel.GetComponentsInChildren<Component>();
				  }
			   }
				
			   List<Transform> Socklist = new List<Transform>();
			   	
	           foreach(Component T in all)
		       {
			      if(T.transform.tag == SummonSpawner.NodeTags[0])
			      {
				     Socklist.Add(T.transform);
			      }
			   }
				
			   SummonSpawner.SpawnLocation = Socklist.ToArray();
			   SummonSpawner.SpawnNpcCount = Socklist.Count;      
			}
			
			xmlWriter.AddAttribute("IsNodeCreate",SummonSpawner.BNodeCreate);
		    
			xmlWriter.AddAttribute("IsRandom", SummonSpawner.IsRandomSpawnNpc);
			
			xmlWriter.AddAttribute("SpawnRadius",SummonSpawner.SpawnArea);
			
			xmlWriter.AddAttribute("IsCircleSpawn",SummonSpawner.Bcircle);
			
			xmlWriter.AddAttribute("SpawnCount",SummonSpawner.SpawnNpcCount);
			
			xmlWriter.AddAttribute("SpawnDelayTime",SummonSpawner.SpawnDelay);
			
			xmlWriter.AddAttribute("SpawnDelayPernpc",SummonSpawner.SpawnDelayPerNpc);
			
			xmlWriter.AddAttribute("SpawnRepeatCount",SummonSpawner.SpawnRepeatCount);
			
			xmlWriter.AddAttribute("InitialState",SummonSpawner.InitialState);
			
			xmlWriter.AddAttribute("WanderRadius",SummonSpawner.WanderRadius);
			
			xmlWriter.AddAttribute("MovableRadius",SummonSpawner.MovableRadius);
			
			xmlWriter.AddAttribute("IfOverrideNpcState",SummonSpawner.StateChangeOverride.OverrideNpcStateChange);
			
			xmlWriter.AddAttribute("AttackOnEnemySightChance",SummonSpawner.StateChangeOverride.AttackOnEnemySightChance);
			
			xmlWriter.AddAttribute("AttackOnDamageChance",SummonSpawner.StateChangeOverride.AttackOnDamageChance);
			
			xmlWriter.AddAttribute("AttackOnAlertChance",SummonSpawner.StateChangeOverride.AttackOnAlertChance);
			
			xmlWriter.AddAttribute("WanderAfterAttackChance",SummonSpawner.StateChangeOverride.WanderAfterAttackChance);
			
			xmlWriter.AddAttribute("WanderOutOfRangeChance",SummonSpawner.StateChangeOverride.WanderWhenOutOfRangeChance);
			
			xmlWriter.AddAttribute("WanderOutOfVisionChance",0); //SummonSpawner.StateChangeOverride.WanderWhenOutOfVisionChance);
			
			if(!SummonSpawner.Bcircle && (SummonSpawner.SpawnHeight > 0 ||  SummonSpawner.SpawnWidth > 0))
			{
				Vector3 EdgePoint = GetHorizontalVector(SummonSpawner);
				xmlWriter.NodeBegin("HorizonVector");
				xmlWriter.AddAttribute("X",EdgePoint.x);
				xmlWriter.AddAttribute("Y",EdgePoint.y);
				xmlWriter.AddAttribute("Z",EdgePoint.z);
				xmlWriter.NodeEnd("HorizonVector");
				Vector3 EdgePoint2 = GetVerticalVector(SummonSpawner);
				xmlWriter.NodeBegin("VerticalVector");
				xmlWriter.AddAttribute("X",EdgePoint2.x);
				xmlWriter.AddAttribute("Y",EdgePoint2.y);
				xmlWriter.AddAttribute("Z",EdgePoint2.z);
				xmlWriter.NodeEnd("VerticalVector");
				
			}
			
		    if( SummonSpawner.NpcPrefabArray!= null && SummonSpawner.NpcPrefabArray.Length > 0)
			{
			  xmlWriter.NodeBegin("MonsterIDs");
				
		      for(int i = 0; i < SummonSpawner.NpcPrefabArray.Length;i++)
		      {	
				  if( SummonSpawner.NpcPrefabArray[i] == null)
						continue;
					
				  NpcBase theMonster = SummonSpawner.NpcPrefabArray[i].GetComponent<NpcBase>();
				  if(theMonster)
				  {
					  xmlWriter.NodeBegin("MonsterID");
						
					  xmlWriter.AddAttribute("id",theMonster.TypeID);
						
					  xmlWriter.NodeEnd("MonsterID");
				  }
				   
		      }
		      xmlWriter.NodeEnd("MonsterIDs");
			}
			
			if(SummonSpawner.SpawnLocation != null && SummonSpawner.SpawnLocation.Length > 0 && newModel != null) //&& SummonSpawner.BNodeCreate)
			{
				
				foreach(Transform it in SummonSpawner.SpawnLocation)
				{
				   xmlWriter.NodeBegin("SpawnOffset");
				   
				   xmlWriter.AddAttribute("PosX",it.position.x - newModel.position.x);
		
	               xmlWriter.AddAttribute("PosY",it.position.y - newModel.position.y);
		
	               xmlWriter.AddAttribute("PosZ",it.position.z - newModel.position.z);
				
				   xmlWriter.NodeEnd("SpawnOffset");
				}
			}
			
			if( newModel != null)
				Object.DestroyImmediate(newModel.gameObject);
			
		    xmlWriter.NodeEnd("SummonSpawner");	
		}
	
		LootDrop myDrop = transform.GetComponent<LootDrop>();
		if(myDrop != null)
		{
			myDrop.DoOtherExport(xmlWriter);
		}
        
		xmlWriter.NodeEnd("Monster");
		
		return xmlWriter.Result;
	}
	
	public void SetNextMoveTarget(float ftime,vectorServerPosition positions)
	{
		if(bNotifyDead)
			return;
		
		if(LastAnimationState == EAnimationStateType.AniChase)
		{
			Transform ChaseObject = null;
			
		    if (TargetObjectID == 1)
		    {
			  ChaseObject = Player.Instance.transform;
		    }
		    else
		    {
		       for( int i = 0; i < CS_SceneInfo.Instance.AllyNpcList.Count;i++)
		       {
                 if (TargetObjectID == CS_SceneInfo.Instance.AllyNpcList[i].ObjID)
			     {
				   ChaseObject =  CS_SceneInfo.Instance.AllyNpcList[i].transform;
				   break;
			     }
			   }
		    }
			
		   if(ChaseObject == null)
			  ChaseObject = Player.Instance.transform;
			
		   if( CS.LastMoveTarget == null)
			   CS.LastMoveTarget = new List<Vector3>();
		
		   CS.LastMoveTarget.Clear();
			
		   float de = 0f;
			
		   float se = 0f;
			
		   Vector3 objPos = transform.position - ChaseObject.position;
			
		   objPos.y = 0f;
			
		   float currentDist = objPos.magnitude;
		   
		   for(int i = 0; i < positions.Count -1;i++)
		   {
			   Vector3 d1 = Vector3.zero; 
			   SServerPosition temp = (SServerPosition)positions[i];
			   d1.x = temp.fx;
			   d1.z = temp.fz;
				
			   Vector3 d2 = Vector3.zero; 
			   temp = (SServerPosition)positions[i+1];
			   d2.x = temp.fx;
			   d2.z = temp.fz;	
				
			   de += (d1 - d2).magnitude;
				
			   if( i == 0)
					continue;
				
			   Vector3 dest = ChaseObject.position;
				
			   dest.y = 0f;
				
			   //Vector3 tmp2 = d1 - transform.position;
			   //tmp2.y = 0f;
				
			   if((d1 - dest).magnitude + 0.1f < currentDist )
			   {
					 CS.LastMoveTarget.Add(d1);
			   }
				
		   }
			
		   if( positions.Count > 0)
		   {
		      SServerPosition temp = (SServerPosition)positions[positions.Count - 1];
			
			  Vector3 d1 = Vector3.zero; 
			   
			  d1.x = temp.fx;
			  d1.z = temp.fz;
			  
			  //Vector3 dest = ChaseObject.position;
				
			  //dest.y = 0f;
				
			  // if((d1 - dest).magnitude + 0.1f < currentDist)
		      CS.LastMoveTarget.Add(d1);
		   }
			
		   if(CS.LastMoveTarget.Count == 0)
		   {
				FSM.ChangeState(IS);
				return;
		   }
			
		   bReachTargetPoint = false;
		   curPathPointIndex = 0;
			
		   CS.AdjustPosition();
				
		   if(!FSM.IsInState(CS))
		   {
	          FSM.ChangeState(CS);
		   }
		   else
		   {
			  PlayRunAnim();
		   }
			
		   float sTime =  Time.time - mBornTime;
		   
		   float dTime = ftime;
			
		   float fDiffTime = sTime - dTime;
			
		   Vector3 sOffest = transform.position - CS.LastMoveTarget[0];
		   
		   sOffest.y = 0f;
			
		   se = sOffest.magnitude;
			
		   for(int i = 1;i < CS.LastMoveTarget.Count;i++)
		   {
				Vector3 temp = CS.LastMoveTarget[i] - CS.LastMoveTarget[i-1];
				temp.y = 0f;
				se += temp.magnitude;
		   }
		   if(fDiffTime < 0f)
			 fDiffTime = 0f;
				
		   //if( fDiffTime > 0f)
           //    ;// Debug.Log("monster " + ObjID.ToString() + " TimeDiffers " + fDiffTime.ToString() + " seconds");
				
		   float pDiff = de - CS.StateSpeed * fDiffTime;
				
		   if(pDiff < 0.01f)
		      pDiff = 0.01f;
				
		   float toss = 1f;
			  
		   toss = se/(pDiff);
			  
		   if(toss < 1f)
			  toss = 1f;
		   if(toss > 2f)
			  toss = 2f;
				
		   CS.ChangSpeed(toss,true);
				
		   if(ChaseObject != null)
		      Delaygap = ChaseObject.position -  CS.LastMoveTarget[ CS.LastMoveTarget.Count -1];
		}
		else if(LastAnimationState == EAnimationStateType.AniSpawn)
		{
			
		}
		else if( LastAnimationState == EAnimationStateType.AniKnockBack)
		{
			
		}
		else if( LastAnimationState == EAnimationStateType.AniWander)
		{
		   if( WS.LastMoveTarget == null)
			   WS.LastMoveTarget = new List<Vector3>();
			
		   WS.LastMoveTarget.Clear();
			
		   foreach(SServerPosition pos in positions)
		   {
			  Vector3 temp = Vector3.zero;
			  temp.x = pos.fx;
			  temp.z = pos.fz;
			  WS.LastMoveTarget.Add(temp);
		   }
			
		   bReachTargetPoint = false;
			
		   curPathPointIndex = 0;
			
		   WS.AdjustPosition();
			
		   if(!FSM.IsInState(WS))
		     FSM.ChangeState(WS);
		}
		else if( LastAnimationState == EAnimationStateType.AniFlee)
		{
			
		}
		else if( LastAnimationState == EAnimationStateType.AniGoHome )
		{
		   if( CS.LastMoveTarget == null)
			   CS.LastMoveTarget = new List<Vector3>();
		
		   CS.LastMoveTarget.Clear();
			
		   foreach(SServerPosition pos in positions)
		   {
			  Vector3 temp = Vector3.zero;
			  temp.x = pos.fx;
			  temp.z = pos.fz;
			  CS.LastMoveTarget.Add(temp);
		   }
		   bReachTargetPoint = false;
		   curPathPointIndex = 0;
			
		   CS.AdjustPosition();
			
		   if(!FSM.IsInState(CS))
	           FSM.ChangeState(CS);
		
		   CS.ChangSpeed(2.0f,true);
		}
		
	}
	
	public BaseObject AttackTarget;
	// call from event
	// 0 : right hand
	// 1 : left hand
	public virtual void AttackPlayer ( int side )
	{
		//transform.GetComponent<NpcSoundEffect>().PlayImpactSound(side);
	}
	
	public void CalculateResult(SUseSkillResult result)
	{
		
	}
	
	// received attack commonder from server
	public  void ReceiveAttackResult(SUseSkillResult result)
	{
		// record attack target
		if(result.destObejctID == 1)
		{
			AttackTarget = Player.Instance;
		}
		else
		{
			foreach( AllyNpc it in  CS_SceneInfo.Instance.AllyNpcList)
		    {
				AttackTarget = it;
				break;
		    }
		}
		
		CS_SceneInfo.Instance.On_UpdateResult(AS, result);
	}
	
	public void NotifyDie()
	{
        print("Notify die object : " + ObjID);
		bNotifyDead = true;
		
		// for normal situation, take damage first, get death notify 2nd. But for MeleeAttack, there should has a bit delay.
        if (AttrMan.Attrs[EAttributeType.ATTR_CurHP] <= 0)
			FSM.ChangeState(DS);

        gameObject.AddComponent<DeleteNPCAutomatically>();
        gameObject.GetComponent<DeleteNPCAutomatically>().npc = this;
	}
	
	public void EquipWeaponWhichSide(Transform weapon, int side)
	{
		Weapons[side] = weapon;
		/*
		if(side == 0)
			Weapon_Right = weapon;
		if(side == 1)
			Weapon_Left = weapon;
			*/
	}
	
	public  string MonsterSceneDoExport()
	{
		XMLStringWriter xmlWriter = new XMLStringWriter();
		
		xmlWriter.NodeBegin("MapMonster");
		
		xmlWriter.AddAttribute("ID",TypeID);
		
	    xmlWriter.AddAttribute("PosX",transform.position.x);
		
	    xmlWriter.AddAttribute("PosY",transform.position.y);
		
	    xmlWriter.AddAttribute("PosZ",transform.position.z);
		
	
		xmlWriter.NodeEnd("MapMonster");
		
		return xmlWriter.Result;
	}
	
	Vector3 GetHorizontalVector( NpcSpawner.Spawner s)
	{
	   Vector3 result = Vector3.zero;
	   
	   Vector3 tempOffset = Vector3.zero;
					
	   float ax = s.SpawnWidth/2 * (-1);
								  
	   tempOffset.x = ax * Mathf.Cos(s.SpawnRotation);
					
	   tempOffset.z = ax * Mathf.Sin(s.SpawnRotation);
					
	   Vector3 LeftUpperCorn = tempOffset;
					
	   ax = s.SpawnWidth/2;
							  
	   tempOffset.x = ax * Mathf.Cos(s.SpawnRotation);
					
	   tempOffset.z = ax * Mathf.Sin(s.SpawnRotation);
					
	   Vector3 RightUpperCorn =  tempOffset;
		
	   result = (RightUpperCorn - LeftUpperCorn) * 0.5f;
	   
	   return  result;
	}
	
	Vector3 GetVerticalVector(NpcSpawner.Spawner s)
	{
	   Vector3 result = Vector3.zero;
	   
	   Vector3 tempOffset = Vector3.zero;
		 									
	   float az = s.SpawnHeight/2 * (-1);
				  
	   tempOffset.x = az * Mathf.Sin(s.SpawnRotation) * (-1);
					
	   tempOffset.z = az * Mathf.Cos(s.SpawnRotation);
					
	   Vector3 RightUpperCorn =  tempOffset;
			
	   az = s.SpawnHeight/2;
				  
	   tempOffset.x = az * Mathf.Sin(s.SpawnRotation) * (-1);
					
	   tempOffset.z = az * Mathf.Cos(s.SpawnRotation);
					
	   Vector3 RightBottomCorn = tempOffset;
		
	   result = (RightBottomCorn - RightUpperCorn) * 0.5f;
	   
	   return  result;
	}
}
