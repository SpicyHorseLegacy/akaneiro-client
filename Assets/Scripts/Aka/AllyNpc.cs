using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AllyNpc : BaseAttackableObject {

    public State CurState;

	public float StartMoveDistance=1.0f;
	public float AttackRange=1f;
	public float DetectNpcRadius=0.5f;
	
	//public int KarmaVal = 0;
	
	public int typeID = 0;
	
	[HideInInspector]
	public bool bAssetBundleReady=false;
	
	public enum EPathFindingState
	{
		PathFinding_Processing,
		PathFinding_Succeed,
		PathFinding_Failed,
		PathFinding_End,
	}
	//[HideInInspector]
	public EPathFindingState PathFindingState = AllyNpc.EPathFindingState.PathFinding_End;
	[HideInInspector]
	public Transform AttackTarget=null;
	

	[HideInInspector]
	public int AroundAngle = 45;
	[HideInInspector]
	public int[] NpcDistanceSortIndex = {0,0,0,0,0,0,0,0,0,0};
	
	[HideInInspector]
	public int NpcMoveDir=1;
	
	[HideInInspector]
	public bool IsDead=false;
	[HideInInspector]
	public bool IsRevive=false;
	[HideInInspector]
	public bool IsAttackTargetLocked=false;
	
	[HideInInspector]
	public int Level=1;

	[HideInInspector]
	public int Exp = 0;
	
	//normal state
	[HideInInspector]
	public Ally_IdleState IS;
	[HideInInspector]
	public Ally_RunState  RS;
    [HideInInspector]
    public Ally_StunState SS;
	[HideInInspector]
	public Ally_DeathState DS;
	[HideInInspector]
	public Ally_ReviveState ReviveS;
	[HideInInspector]
	public Ally_DamageState Damage_State;
	
	[HideInInspector]
	public bool IsFirstPathFinding=true;

	//DamageText
	public bool showDamage = true;
	public float DamageText_Offset_Y = 2.3f;
	public float DamageText_Range_y = 5f;
	public float DamageText_Speed_y = 1f;
	Vector3 DamageText_Initial_Pos;
	//SpriteText DamageTextTip;
	SpriteText ExpTextTip;
	SpriteText LevelUpTip;
    Vector3 ScreenPos = new Vector3();
	Color m_Color = new Color(0,0,0,1);
	
	//effect
	public Transform ImpactByAttackParticle;
	
    public bool showExp = true;
	Vector3 ExpText_Initial_Pos;
	
	public bool showLevelUp = true;
	bool m_bLevelUp = false;
	Vector3 LevelUp_Initial_Pos;
	
	
	Transform SpawnOut_Sound;
	Transform FatalBlow_Sound;
	Transform BodyFall_Sound;
	Transform TakeDamage_Sound;
		
	public Transform WeaponItemSound;
	public Transform ArmorItemSound;
	
	//spirit helper
	[HideInInspector]
	public SpiritBase AttachedSpirit = null;
	
	[HideInInspector]
	public Vector3 SpawnPosition = Vector3.zero;
	public Vector3 SpawnRotation = Vector3.zero;
	
	[HideInInspector]
	public SUseSkillResult useSkillResult;
	
	public int AllyDistance = 2;
    public float EnemyDistance = 5.0f;
	
	public int MaxAllyDitance = 10;
	
	[HideInInspector]
	public bool bInitialActive = false;
	
	public	   ObjStatePrefab stateObj;
	
	public Transform DeathEffect;

	
	public enum EAllyType
	{
		eMission = 0,
		eFriend,	
	}
	
	public EAllyType SelectAllyType;
	
	EAllyNpcType Allytype = new  EAllyNpcType();
	
	ESex _gendor = null;

    SFriendAllyEnter EnterInfo;
	
	float RestoreTime = 0f;
	
	int miRestoreValue = 0;

    public int UseAbilityChange = 30;

	public override Transform GetAnimationModel ()
	{
		AnimationModel = null;

		AnimationModel = transform.FindChild("Aka_Model");

		return AnimationModel;
	}

    protected override void Awake()
    {
        base.Awake();

		Level=1;
		
		ObjType = ObjectType.Ally;
		
		//create state machine
		FSM = new StateMachine(transform);
		InitialStates ();
		
		if (abilityManager)
        {
            abilityManager.player = this;
		}
		
		RestoreTime = Time.time;
	}
	
	public void SetAllyKind( EAllyType allyType,bool bEquipCloth)
	{
		SelectAllyType = allyType;
	}

    public void SetInitialAttribute(SFriendAllyEnter info)
    {
        ObjID = info.objectID;

        _gendor = info.sex;

        EnterInfo = info;

        if (AttrMan != null)
        {
            AttrMan.UpdateAttrs(info.attrVec);

            foreach (itemuuid equip in info.equipinfo)
            {
                ItemDropStruct tempItem = ItemDeployInfo.Instance.GetItemObject(equip.itemID, equip.prefabID, equip.gemID, equip.enchantID, equip.elementID, equip.itemLevel);

                if (tempItem == null)
                    continue;

                Transform item = UnityEngine.Object.Instantiate(ItemPrefabs.Instance.GetItemPrefab(tempItem._ItemID, tempItem._TypeID, tempItem._PrefabID)) as Transform;

                if (item != null)
                {
                    SItemInfo _iteminfo = new SItemInfo();
                    _iteminfo.gem = equip.gemID;
                    _iteminfo.element = equip.elementID;
                    _iteminfo.enchant = equip.enchantID;
                    EquipementMan.UpdateItemInfoBySlot((uint)equip.slotPart, item, _iteminfo, true, _gendor);
                }
            }
        }

        //if (info.style == ETalent.eProwess)
            abilityManager.AddSkill((int)AbilityIDs.Shockwave_I_ID);
        //if (info.style == ETalent.eFortitude)
            abilityManager.AddSkill((int)AbilityIDs.IceBarricade_I_ID);
        //if (info.style == ETalent.eCunning)
            abilityManager.AddSkill((int)AbilityIDs.Caltrops_I_ID);

        gameObject.AddComponent<AllySoundHandler>();
        transform.GetComponent<AllySoundHandler>().executer = this;
    }
	
		// Use this for initialization
	void Start () 
	{
		GetComponent<AllyMovement>().OwnerNpc = this;
		
		RaycastHit hit;
		if(Physics.Raycast(transform.position + Vector3.up*5f,Vector3.down,out hit,10f,1 << LayerMask.NameToLayer("Walkable")))
		{
			transform.position = hit.point;
		}
	}
	
	public void InitialStates ()
	{
		GameObject states = new GameObject();
		states.transform.name = "STATES";
		states.transform.parent = transform;
		
		IS = states.AddComponent<Ally_IdleState>();				IS.SetAlly(this);
		RS = states.AddComponent<Ally_RunState>();				RS.SetAlly(this);
        SS = states.AddComponent<Ally_StunState>();             SS.SetAlly(this);
		DS = states.AddComponent<Ally_DeathState>();			DS.SetAlly(this);
		ReviveS = states.AddComponent<Ally_ReviveState>();		ReviveS.SetAlly(this);
		Damage_State = states.AddComponent<Ally_DamageState>();	Damage_State.SetAlly(this);
	}
	
	
	// Update is called once per frame
	void Update () {

        CurState = FSM.GetCurrentState();

        if (Input.GetKeyDown(KeyCode.Alpha7))
        {
            List<AbilityBaseState> _canuseabs = new List<AbilityBaseState>();

            foreach (AbilityBaseState _abi in abilityManager.Abilities)
            {
                AbilityBaseState _allyabi = (AbilityBaseState)_abi;
                if (_allyabi.CanUseAbility() && _allyabi.GetType() != typeof(Ally_NormalAttackState))
                    _canuseabs.Add(_allyabi);
            }

            // if ther is any ability ready, there is a chance (Default : 30%) to use it.
            //if (_canuseabs.Count > 0 && Random.Range(0, 100) < UseAbilityChange)
            {
                AbilityBaseState _usingAbi = null;
                if (_canuseabs.Count > 0)
                    _usingAbi = _canuseabs[Random.Range(0, _canuseabs.Count)];
                if (_usingAbi != null && FSM.IsInState(_usingAbi) == false)
                {
                    FSM.ChangeState(_usingAbi);
                    return;
                }
            }
        }

		if(!bAssetBundleReady)
			return;
		
		if(!bInitialActive)
		{
            EquipementMan.UpdateEquipment(_gendor);
            GetComponent<PreLoadAllyNpc>().usingLatestConfig = true;
			ClickModel();
			bInitialActive = true;
		    return;
		}

		if(CS_SceneInfo.Instance != null)
		   CS_SceneInfo.Instance.PopNewContent(transform);
		
		FSM.Update();
		
		if(AttrMan.Attrs[EAttributeType.ATTR_CurHP] > 0)
		{
		   if(Time.time - RestoreTime >= 0.5f)
		   {
			  RestoreTime = Time.time;
			
			  ReCoverHp(AttrMan.Attrs[EAttributeType.ATTR_HPRecover]);
			  ReCoverMp(AttrMan.Attrs[EAttributeType.ATTR_MPRecover]);
		   }
		}
	}
	
	void ReCoverHp(int hp)
	{
		int iHealth = AttrMan.Attrs[EAttributeType.ATTR_CurHP];
		
		if( iHealth < AttrMan.Attrs[EAttributeType.ATTR_MaxHP])
		    iHealth += hp;
	    if(iHealth > AttrMan.Attrs[EAttributeType.ATTR_MaxHP])
		   iHealth = AttrMan.Attrs[EAttributeType.ATTR_MaxHP];
		
		AttrMan.Attrs[EAttributeType.ATTR_CurHP] = iHealth;
	}
	
	void ReCoverMp(int mp)
	{
		int iMp = AttrMan.Attrs[EAttributeType.ATTR_CurMP];
		if(iMp < AttrMan.Attrs[EAttributeType.ATTR_MaxMP])
		   iMp += mp;
		if(iMp > AttrMan.Attrs[EAttributeType.ATTR_MaxMP])
		   iMp = AttrMan.Attrs[EAttributeType.ATTR_MaxMP];
		AttrMan.Attrs[EAttributeType.ATTR_CurMP] = iMp;
	}
	
    void ClickModel()
    {
        if (SelectAllyType != EAllyType.eMission)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            RaycastHit hit;
            int layer = 1 << LayerMask.NameToLayer("Default");

            if (Physics.Raycast(ray.origin, ray.direction, out hit, 100f, layer))
            {
                if (GetAllyTransform(hit.transform) == transform)
                {
                    bool bAdd = true;

                    foreach (AllyNpc it in CS_SceneInfo.Instance.AllyNpcList)
                    {
                        if (it.ObjID == ObjID)
                        {
                            bAdd = false;
                            break;
                        }
                    }

                    if (bAdd)
                        CS_SceneInfo.Instance.AllyNpcList.Add(this);

                    if (CS_Main.Instance != null)
                    {
                        Allytype.Set((int)SelectAllyType);
                        CS_Main.Instance.g_commModule.SendMessage(ProtocolBattle_SendRequest.CreateAllyNpcRequest(transform.position, Allytype));
                    }
                }
            }
        }
    }
	
    Transform GetAllyTransform(Transform obj)
	{
		Transform T = obj;
		while(T != null)
		{
			if(T.GetComponent<AllyNpc>() != null)
				return T;
			else
				T = T.parent;
		}
		
		return null;
	}
	
	public void AttackNotify()
	{
		// Ally_NormalAttackState attState = (Ally_NormalAttackState)abilityManager.GetAbilityByID((uint)AbilityIDs.NormalAttack_1H_ID);
		//	attState.CalculateResult();
		//if(!AttackTarget) return;
		
		//if(AttackTarget && AttackTarget.GetComponent<BaseObject>())
		//	EquipementManager.Instance.PlayWeaponImpactSound(AttackTarget.GetComponent<BaseHitableObject>());
		//abilityManager.GetAbilityByID((uint)AbilityIDs.NormalAttack_ID).CalculateResult();	
	}
	
	public override void TakeDamage(int damage)
	{
		if(FSM.IsInState(DS))
			return;
		
        base.TakeDamage(damage);

        if (AttrMan.Attrs[EAttributeType.ATTR_CurHP] <= 0 && !FSM.IsInState(DS) && !FSM.IsInState(ReviveS))
		{
			//FatalBlow_Sound = PlaySound(FatalBlowSound,FatalBlow_Sound);
			//FSM.ChangeState(DS);
		}
	}

    public override void GoToHell()
    {
        base.GoToHell();

        PlayDamageAnim();
    }

    public override void PlayDamageImpactFrom(int damage, DamageSource source, EStatusElementType elementType)
	{
        base.PlayDamageImpactFrom(damage, source, elementType);
		
		if(ImpactByAttackParticle!=null)
		{
			Vector3 pos = transform.position + Vector3.up*1.2f;
            Vector3 dir = (source.Owner.position - transform.position).normalized;
			pos += dir * 0.2f;
			Instantiate(ImpactByAttackParticle,pos,Quaternion.LookRotation(dir));
		}
	}
	
	void OnControllerColliderHit (ControllerColliderHit hit)
	{
		if(hit.transform && hit.transform.GetComponent<NpcBase>())
		{
			//Debug.Log("Collide with " + hit.transform.name);
			GetComponent<AllyMovement>().StopMove();
			return;
		}
		
		if(hit.normal.y < 0.2f && (hit.point.y - transform.position.y >0.75f ))
		{
			float angle = Vector3.Dot(transform.forward, hit.normal);
			if(angle <0f && (FSM.IsInState(RS))) //|| FSM.IsInState(GetAbilityByID(AbilityIDs.SwathOfDestruction_ID))) )
			{
				GetComponent<AllyMovement>().StopMove();
			}
		}
	}

	//find where to move
	public void FindMoveTarget()
	{
        // is finding path, return
		if(PathFindingState == EPathFindingState.PathFinding_Processing)
			return;
		
        // is ally is freezed, return
		AllyMovement pm = GetComponent<AllyMovement>();
		if(pm.IsFreezed) return;

        #region Search Enemy

        Transform target = null;

        //search the enemy that attack me or the enemy that attack player
        if (null == target)
        {
            foreach (Transform it in Player.Instance.AllEnemys)
            {
                if (it != null && it.GetComponent<NpcBase>() != null)
                {
                    if (it.GetComponent<NpcBase>().FSM.IsInState(it.GetComponent<NpcBase>().AS))
                    {
                        if (it.GetComponent<NpcBase>().TargetObjectID == Player.Instance.ObjID)
                        {
                            target = it;
                        }
                        else if (it.GetComponent<NpcBase>().TargetObjectID == ObjID)
                        {
                            target = it;
                            break;
                        }
                    }
                }
            }
        }

        float minDot = 100;
        //seach the closest enemy
	    int layer = 1<<LayerMask.NameToLayer("NPC");
        Collider[] attackableObjs = Physics.OverlapSphere(transform.position + Vector3.up * 0.5f, EnemyDistance, layer);
		foreach(Collider obj in attackableObjs)
		{
			if( obj != null && obj.transform != null && obj.transform.GetComponent<NpcBase>() == null)
				continue;
				
			Vector3 objTempPos = new Vector3(obj.transform.position.x, transform.position.y, obj.transform.position.z);
			Vector3 dir = objTempPos - transform.position;
			float tempDot = dir.magnitude;
			if(tempDot < minDot)
			{
				target = obj.transform;
				minDot = tempDot;
			}
		}
		
        // if there isn't target and player has a target, that's ally's target as well.
		if( null == target)
		{
			if( Player.Instance.AttackTarget && Player.Instance.AttackTarget.GetComponent<NpcBase>()!= null)
			    target = Player.Instance.AttackTarget;
		}
		
        // check target is still alive
		if(target != null && target.GetComponent<NpcBase>() != null)
		{
			if(target.GetComponent<NpcBase>().AttrMan.Attrs[EAttributeType.ATTR_CurHP] <= 0 || target.GetComponent<NpcBase>().bNotifyDead)
				 target = null;
        }

        #endregion

        AttackTarget = target;
        if (AttackTarget != null)
		{
            NextActionID = GetNextActionID();
			AttackEnemy(target);
		}
		else
		{
			Vector3 dir3 = transform.position -  Player.Instance.transform.position;
			dir3.y = 0f;
			
			if( dir3.magnitude > AllyDistance)
			{
			    dir3.Normalize();
			    pm.MoveTarget = Player.Instance.transform.position + dir3 * AllyDistance;
			}
			
			float DisToTarget = (transform.position - pm.MoveTarget).magnitude;
		    if(DisToTarget > StartMoveDistance)
		    {
			   PathFindingState = EPathFindingState.PathFinding_Processing;
			   GetComponent<Seeker>().StartPath(transform.position,pm.MoveTarget);
		    }
		}	
	}

    public int NextActionID;
    int GetNextActionID()
    {
        List<AbilityBaseState> _canuseabs = new List<AbilityBaseState>();

        foreach (AbilityBaseState _abi in abilityManager.Abilities)
        {
            AbilityBaseState _allyabi = (AbilityBaseState)_abi;
            if (_allyabi.CanUseAbility() && _allyabi.GetType() != typeof(Ally_NormalAttackState))
                _canuseabs.Add(_allyabi);
        }

        // if ther is any ability ready, there is a chance (Default : 30%) to use it.
        if (_canuseabs.Count > 0 && Random.Range(0, 100) < UseAbilityChange)
        {
            AbilityBaseState _usingAbi = _canuseabs[Random.Range(0, _canuseabs.Count)];
            if (_usingAbi != null && FSM.IsInState(_usingAbi) == false)
            {
                return _usingAbi.id;
            }
        }

        return (int)AbilityIDs.NormalAttack_1H_ID;
    }
	
	public void ForceToFindPath()
	{
		AllyMovement pm = GetComponent<AllyMovement>();
		
		AttackTarget = null;
			
		Vector3 dir3 = transform.position -  Player.Instance.transform.position;
			
		dir3.Normalize();
			
	    pm.MoveTarget = Player.Instance.transform.position + dir3 * AllyDistance;
		
	    PathFindingState = EPathFindingState.PathFinding_Processing;
		
		GetComponent<Seeker>().StartPath(transform.position,pm.MoveTarget);	
	}
	
	public void TeleportSpecifiedPos()
	{
		Vector3 dir3 = transform.position - Player.Instance.transform.position;
		
		dir3.Normalize();
		
	    Vector3 StartPosition = Player.Instance.transform.position + dir3 * (AllyDistance + 2);
		
		AllyMovement pm = GetComponent<AllyMovement>();
		
		pm.MoveTarget = Player.Instance.transform.position + dir3 * AllyDistance;
	
		pm.MoveDir = dir3;
		
		PathFindingState = AllyNpc.EPathFindingState.PathFinding_Processing;
		
		transform.position = StartPosition;
		
		GetComponent<Seeker>().StartPath(StartPosition,pm.MoveTarget);
		
	}

	public void PlaySpawnOutSound()
	{
		//SpawnOut_Sound = PlaySound(SpawnOutSound,SpawnOut_Sound);
	}

	public void PlayBodyFallSound()
	{
		//BodyFall_Sound = PlaySound(BodyFallSound,BodyFall_Sound);
	}

	public Transform PlaySound(Transform prefab, Transform sound)
	{
		if(sound==null && prefab!=null)
		{
			sound = Instantiate(prefab) as Transform;
		}
		
		if(sound!=null)
		{
			sound.position = transform.position;
			sound.rotation = transform.rotation;
			SoundCue.Play(sound.gameObject);
		}
		
		return sound;
	}
	
	public void SetSkinOfStoneMaterial(bool bGray)
	{
		Transform AnimationModel = transform.FindChild("Aka_Model");

		if(AnimationModel && AnimationModel.renderer)
		{
			for(int i = 0; i < AnimationModel.renderer.materials.Length;i++)
			{
				Material mtl =  AnimationModel.renderer.materials[i];
		    	if(mtl.HasProperty("_GrayScaleEff"))
		        {
					if( bGray)
			           mtl.SetFloat("_GrayScaleEff",1.0f);
					else
					   mtl.SetFloat("_GrayScaleEff",0.0f);
		        }
				
			}
		}
	}
	public NpcBase GetNpcBaseByCollider(Collider col)
	{
		NpcBase npc=null;
		
		Transform obj = col.transform;
		npc = obj.GetComponent<NpcBase>();
		
		while(!npc)
		{
			obj = obj.parent;
			if(obj==null)
				break;
			
			npc = obj.GetComponent<NpcBase>();
		}
		
		return npc;
	}

    #region AnimationControl
    public void PlayIdleAnim(bool IsAttackIdle)
	{
		WeaponBase.EWeaponType wt = EquipementMan.GetWeaponType();
		
		if(IsAttackIdle)
		{
            if (wt == WeaponBase.EWeaponType.WT_NoneWeapon || wt == WeaponBase.EWeaponType.WT_OneHandWeapon || wt == WeaponBase.EWeaponType.WT_DualWeapon)
				AnimationModel.animation.CrossFade("Aka_1H_Attack_Idle_1");	
			else if(wt == WeaponBase.EWeaponType.WT_TwoHandWeaponAxe)
				AnimationModel.animation.CrossFade("Aka_2H_Attack_Idle_1");	
			else
				AnimationModel.animation.CrossFade("Aka_2HNodachi_Attack_Idle_1");	
			
		}
		else
		{
            if (wt == WeaponBase.EWeaponType.WT_NoneWeapon || wt == WeaponBase.EWeaponType.WT_OneHandWeapon || wt == WeaponBase.EWeaponType.WT_DualWeapon)
				AnimationModel.animation.CrossFade("Aka_1H_Idle_1");	
			else if(wt == WeaponBase.EWeaponType.WT_TwoHandWeaponAxe)
				AnimationModel.animation.CrossFade("Aka_2H_Idle_1");	
			else
				AnimationModel.animation.CrossFade("Aka_2HNodachi_Idle_1");				
		}		
	}
	
	public void PlayDamageAnim()
	{
		if(!EquipementMan.RightHandWeapon)
			return;

        WeaponBase.EWeaponType wt = EquipementMan.GetWeaponType();

        if (wt == WeaponBase.EWeaponType.WT_NoneWeapon || wt == WeaponBase.EWeaponType.WT_OneHandWeapon || wt == WeaponBase.EWeaponType.WT_DualWeapon)
			AnimationModel.animation.Blend("Aka_1H_Damage_Lt");
		else if(wt == WeaponBase.EWeaponType.WT_TwoHandWeaponAxe)
            AnimationModel.animation.Blend("Aka_2H_Damage_Lt");	
		else
            AnimationModel.animation.Blend("Aka_2HNodachi_Damage_Lt");	
	}

    public void PlayStunAnim()
    {
        WeaponBase.EWeaponType wt = EquipementMan.GetWeaponType();
        if (wt == WeaponBase.EWeaponType.WT_NoneWeapon || wt == WeaponBase.EWeaponType.WT_OneHandWeapon || wt == WeaponBase.EWeaponType.WT_DualWeapon)
            AnimationModel.animation.CrossFade("Aka_1H_Idle_1");
        else if (wt == WeaponBase.EWeaponType.WT_TwoHandWeaponAxe)
            AnimationModel.animation.CrossFade("Aka_2H_Idle_1");
        else
            AnimationModel.animation.CrossFade("Aka_2HNodachi_Idle_1");	
    }
	
	public void PlayDeathAnim()
	{
        WeaponBase.EWeaponType wt = EquipementMan.GetWeaponType();

        if (wt == WeaponBase.EWeaponType.WT_NoneWeapon || wt == WeaponBase.EWeaponType.WT_OneHandWeapon || wt == WeaponBase.EWeaponType.WT_DualWeapon)
			AnimationModel.animation.CrossFade("Aka_1H_Death_1");
		else if(wt == WeaponBase.EWeaponType.WT_TwoHandWeaponAxe)
			AnimationModel.animation.CrossFade("Aka_2H_Death_1");	
		else
			AnimationModel.animation.CrossFade("Aka_2HNodachi_Death_1");	
	}	
	
	public void PlayReviveAnim()
	{
		AnimationModel.animation.CrossFade("Aka_Shockwave_1H");
	}
	
	void SetLayerRecursively(Transform temp,int layer)
    {
		temp.gameObject.layer = layer;
		for(int i = 0; i < temp.GetChildCount();i++)
		{
			SetLayerRecursively(temp.GetChild(i),layer);
		}
    }
    #endregion

    public void PlayAbilitySound(Transform sound, Vector3 pos, Quaternion rot)
	{
		if(sound!=null)
		{
			sound.position = pos;
			sound.rotation = rot;
			SoundCue.Play(sound.gameObject);
		}	
	}
	
	public void SetPlayerWeaponVisible(bool IsVisible)
	{
        if (EquipementMan.RightHandWeapon)
            EquipementMan.RightHandWeapon.gameObject.SetActive(IsVisible);
        if (EquipementMan.LeftHandWeapon)
            EquipementMan.LeftHandWeapon.gameObject.SetActive(IsVisible);
	}

    /// <summary>
    /// Ally find enemy around.
    /// </summary>
    /// <returns>true : Do not attack enemy, false : Attack enemy</returns>
    public bool ThinkAI()
    {
        Vector3 dir3 = transform.position - Player.Instance.transform.position;
        dir3.y = 0f;
        if (dir3.magnitude > AllyDistance)
            return true;

        int layer = 1 << LayerMask.NameToLayer("NPC");

        Collider[] attackableObjs = Physics.OverlapSphere(transform.position + Vector3.up * 0.5f, 10f, layer);

        if (attackableObjs.Length > 0)
            return true;

        if (Player.Instance.AttackTarget != null && Player.Instance.AttackTarget.GetComponent<NpcBase>() != null)
            return true;

        return false;
    }
	
	public void AttackEnemy( Transform enemy )
	{
        if (!enemy) return;

        AttackTarget = enemy;

        float _attackrange = AttackRange;
        if(NextActionID != (int)AbilityIDs.NormalAttack_1H_ID)
            _attackrange = AbilityInfo.Instance.AbilityInfomation.GetAbilityDetailInfoByID(NextActionID).EndDistance / 2.0f;

        if (Vector3.Distance(transform.position, AttackTarget.position) < AttackRange)
        {
            FSM.ChangeState(abilityManager.GetAbilityByID((uint)NextActionID));
        }
        else
        {
            AllyMovement pm = GetComponent<AllyMovement>();
            pm.MoveTarget = AttackTarget.position;
            PathFindingState = EPathFindingState.PathFinding_Processing;
            GetComponent<Seeker>().StartPath(transform.position, pm.MoveTarget);
        }
	}
}
