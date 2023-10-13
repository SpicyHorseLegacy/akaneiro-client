using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Player : BaseAttackableObject {
	
	public static Player Instance;

    public PlayerBuffManager BuffManager
    {
        get
        {
            return (PlayerBuffManager)BuffMan;
        }
    }
	public PlayerMasteryManager MasteryManager;
	public PlayerPetManager PetManager;
	
    [HideInInspector]
    public PlayerSoundHandler SoundHandler;

    public static Vector3 Position
    {
        get
        {
            return Instance.transform.position;
        }
    }
	 
    [HideInInspector]
    public PlayerMovement MovementController;

	public State curstate;

	public float StartMoveDistance=1.0f;
    public float ChaseRange = 4f;           // distance between enmey and player is lower than this AND autoattack is on, player start chasing enmey.
    public float AttackRange = 1.5f;        // distance between enmey and player is lower than this, player start attacking enemy.
	public float DetectRadius=0.5f;         // distance between mouse touching point and player is lower than this, player doesn't move to target position.
	
	public int KarmaVal = 0;
	
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
	public EPathFindingState PathFindingState = Player.EPathFindingState.PathFinding_End;
	//[HideInInspector]
    public Transform HoverTarget;
	public Transform AttackTarget=null;
	//[HideInInspector]
	public Transform PickupItem=null;
	[HideInInspector]
	public List<Transform> AllEnemys;
	[HideInInspector]
	public bool[] AllAroundPos;
	[HideInInspector]
	public int AroundAngle = 45;
	[HideInInspector]
	public int[] NpcDistanceSortIndex = {0,0,0,0,0,0,0,0,0,0};
	TransformCompare myComparer = new TransformCompare();


	[HideInInspector]
	public int NpcMoveDir=1;
	
	[HideInInspector]
	public List<Transform> AllPickupItems;
	[HideInInspector]
	public bool IsDead=false;
	[HideInInspector]
	public bool IsAttackTargetLocked=false;
	
	[HideInInspector]
	public int Level=1;
	
	[HideInInspector]
	public int Exp = 0;
	
	//state variables
	[HideInInspector]	public PlayerGlobalState PGS;
	[HideInInspector]	public IdleState IS;
	[HideInInspector]	public RunState  RS;
    [HideInInspector]   public StunState SS;
    [HideInInspector]   public KnockBackState KS;
	[HideInInspector]	public DeathState DS;
	[HideInInspector]	public ReviveState ReviveS;
	[HideInInspector]	public DamageState Damage_State;
	
	[HideInInspector]
	public bool IsFirstPathFinding=true;
    public float MouseClickRadius = 1.0f;
	
	//SpriteText DamageTextTip;
	SpriteText ExpTextTip;
	SpriteText LevelUpTip;
    Vector3 ScreenPos = new Vector3();
	Color m_Color = new Color(0,0,0,1);
	
    public bool showExp = true;
	Vector3 ExpText_Initial_Pos;
	
	public bool showLevelUp = true;
	bool m_bLevelUp = false;
	Vector3 LevelUp_Initial_Pos;
	
	//spirit helper
	[HideInInspector]
	public SpiritBase AttachedSpirit = null;
	
	[HideInInspector]
	public Vector3 SpawnPosition = Vector3.zero;
	public Vector3 SpawnRotation = Vector3.zero;

	[HideInInspector]
	public bool CanActiveAbility = true;
	
	public Transform MoveMarkerPrefab;

    public Transform KarmaVFXPrefab = null;
    Transform _karmaVFX = null;

    public static void GetKarma()
    {
        if (Instance._karmaVFX) return;
        Instance._karmaVFX = Instantiate(Instance.KarmaVFXPrefab) as Transform;
        Instance._karmaVFX.parent = Instance.AnimationModel;
        Instance._karmaVFX.localPosition = Vector3.zero;
    }
	
	public override Transform GetAnimationModel ()
	{
		AnimationModel = null;

		AnimationModel = transform.FindChild("Aka_Model");

		return AnimationModel;
	}
	
	protected override void Awake()
	{
        base.Awake();

		DontDestroyOnLoad(gameObject);
		
		Instance = this;

        if (GameCamera.Instance != null)
            GameCamera.Instance.target = transform;

        GameObject _tempbuffobj = BuffMan.gameObject;
        Destroy(BuffMan);
        BuffMan = (BuffManager)_tempbuffobj.AddComponent<PlayerBuffManager>();
        BuffMan.Initial(transform);

        SoundHandler = GetComponent<PlayerSoundHandler>();
        MovementController = GetComponent<PlayerMovement>();
		
		ObjType  = ObjectType.Player;
		
		Level=1;
		
		AllAroundPos = new bool[20];
		
		for(int i=0;i<20;i++)
		{
			AllAroundPos[i]=false;
		}
		FSM = new PlayerStateMachine(transform);

		AnimationModel = transform.FindChild("Aka_Model");
		
		if(abilityManager){
			abilityManager.player = this;
		}

        GameObject attrManObj = AttrMan.gameObject;
        Destroy(AttrMan.transform.GetComponent<AttributionManager>());
        AttrMan = attrManObj.AddComponent<PlayerAttributionManager>();
        AttrMan.Owner = transform;
		
		InitialStates();

        FreezePlayer();
	}
	
	// Use this for initialization
	void Start () 
	{
        transform.position = CS_SceneInfo.pointOnTheGround(transform.position);
		
		if( PlayerLeveling.Instance != null)
		   Exp =  PlayerLeveling.Instance.KarmaRequiredTotal;
	}
	
	public void InitialStates ()
	{
		GameObject states = new GameObject();
		states.transform.name = "STATES";
		states.transform.parent = transform;
		
		PGS = states.AddComponent<PlayerGlobalState>();
		PGS.Enter();
		FSM.SetGlobalState(PGS);

        IS = states.AddComponent<IdleState>();                  IS.Initial();
        RS = states.AddComponent<RunState>();                   RS.Initial();
        SS = states.AddComponent<StunState>();                  SS.Initial();
        KS = states.AddComponent<KnockBackState>();             KS.Initial();
        DS = states.AddComponent<DeathState>();                 DS.Initial();
        ReviveS = states.AddComponent<ReviveState>();           ReviveS.Initial();
        Damage_State = states.AddComponent<DamageState>();      Damage_State.Initial();
	}

	// Update is called once per frame
	void Update () {
	
		curstate = FSM.GetCurrentState();
		if(!bAssetBundleReady) return;
		
		GetComponent<PlayerMovement>().Execute();
		
		FSM.Update();
		BuffMan.Execute();
		
		if(CS_SceneInfo.Instance != null)
		   CS_SceneInfo.Instance.PopNewContent(transform);
	}

    public void FreezePlayer()
    {
		Debug.Log("freeze");
		#if NGUI
			GameObject.Find("mouseCursors").gameObject.SendMessage("cursorNormalF");
		#endif
		//--------------------------------------------------------------->>mm
		GameObject _cameragameobject = Camera.main.gameObject;
		#if NGUI
		if(_cameragameobject)
			_cameragameobject.SendMessage("changeCamera0");
		_cameragameobject = GameObject.Find("Camera").gameObject;
		if(_cameragameobject)
			_cameragameobject.SendMessage("changeCamera0");
		#endif
		//--------------------------------------------------------------->>#mm
        if (!FSM.IsInState(DS))
            FSM.ChangeState(IS);
        GetComponent<PlayerMovement>().Freeze();
    }

    public void ReactivePlayer()
    {
		Debug.Log("reactive");
		#if NGUI
			GameObject.Find("mouseCursors").gameObject.SendMessage("cursorNormalF");
		#endif
		//--------------------------------------------------------------->>mm
#if NGUI
		GameCamera.Instance.gameCamera.SendMessage("changeCamera1");
		GameObject _cameragameobject = GameObject.Find("Camera").gameObject;
		if(_cameragameobject)
			_cameragameobject.SendMessage("changeCamera1");
#endif
		//--------------------------------------------------------------->>#mm
        if (!FSM.IsInState(DS))
            GetComponent<PlayerMovement>().ReleaseFreeze();
    }
	
    public void AttackNotify()
	{
        //MeleeAttackState attState = (MeleeAttackState)abilityManager.GetAbilityByID((uint)AbilityIDs.NormalAttack_1H_ID);
		//attState.CalculateResult();
	}
	
	public override void TakeDamage(int damage)
	{
        base.TakeDamage(damage);
		if(damage < 0)
		{
			PlayDamageAnim();
		}
	}

    public override void GoToHell()
    {
        base.GoToHell();
    }

    public override BaseBuff AddBuff(int id, Transform _sourceTransform)
    {
        // special for get knockback buff.
        BaseBuff _buff = BuffMan.AddBuffByID(id, _sourceTransform);
        if (_buff)
        {
            if (_buff.GetType() == typeof(Knockback) && !FSM.IsInState(DS))
            {
                MovementController.LookAtPosition(_sourceTransform.position);
                GetComponent<CharacterController>().Move(-MovementController.PlayerObj.forward * 1f);
                MovementController.SendPositionToServer();

                FSM.ChangeState(KS);
            }
        }

        return _buff;
    }

	public bool isFindMoveTarget()
	{
		return isFindMoveTarget(false);
	}
	
	//find where to move
	public bool isFindMoveTarget(bool isIgnoreEnemy)
	{
		//if free camera mode is active ,aka not move
        if (GameCamera.Instance && GameCamera.Instance.IsFreeCameraMode)
        {
           // Debug.Log("FreeCameraMode!");
            return false;
        }

        // transform.parent is for real aka_controller. If player.instance has a parent, that means it runs in map.
        if (!transform.parent && !_UI_CS_ScreenCtrl.Instance.IsScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_INGAME_NORMAL))
            return false;
		
		if(PathFindingState == Player.EPathFindingState.PathFinding_Processing)
			return false;
		
		if(IsClickOnUI())
			return false;

		PlayerMovement pm = GetComponent<PlayerMovement>();
		if(pm.IsFreezed)    return false;

		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		
        if (HoverTarget)
        {
            if (HoverTarget.gameObject.layer == LayerMask.NameToLayer("NPC") && !isIgnoreEnemy)
            {
                AttackEnemy(HoverTarget);
                return true;
            }
            else if ((HoverTarget.gameObject.layer == LayerMask.NameToLayer("InteractiveOBJ") || HoverTarget.gameObject.layer == LayerMask.NameToLayer("Breakable"))  && !isIgnoreEnemy)
            {
                if (HoverTarget.GetComponent<InteractiveObj>() != null)
                {
                    if (HoverTarget.GetComponent<InteractiveObj>().IsUsed)
                        return true;
                }
                AttackEnemy(HoverTarget);
                return true;
            }
            else if (HoverTarget.gameObject.layer == LayerMask.NameToLayer("DropItem") && !isIgnoreEnemy)
            {
                PickItem(HoverTarget);
                return true;
            }
        }
        else if (Physics.Raycast(ray, out hit, 100f, 1 << LayerMask.NameToLayer("NPC")) && !isIgnoreEnemy)
        {
            AttackEnemy(hit.transform);
            return true;
        }

		//if(Physics.SphereCast(ray, DetectNpcRadius, out hit,100f,1<<LayerMask.NameToLayer("NPC")) && !isIgnoreEnemy)
        //if (Physics.Raycast(ray, out hit, 100f, 1 << LayerMask.NameToLayer("NPC")) && !isIgnoreEnemy)
        //Transform _tempTarget = FindTarget();
        //if (_tempTarget != null && !isIgnoreEnemy)
        //{
        //    //if(hit.transform.GetComponent<BaseObject>() && hit.transform.GetComponent<BaseObject>().ObjType == ObjectType.NPC)
        //        //moveToNPC(hit.transform);
        //    //else
        //    AttackEnemy(_tempTarget);
        //    return true;
        //}
        //else if (Physics.Raycast(ray, out hit, 100f, layer))
        //{
        //    if(hit.transform.gameObject.layer == LayerMask.NameToLayer("Breakable") && !isIgnoreEnemy)
        //    {
        //        AttackEnemy(hit.transform);
        //        return true;
        //    }
        //    else if(hit.transform.gameObject.layer == LayerMask.NameToLayer("InteractiveOBJ") && !isIgnoreEnemy)
        //    {
        //        if(hit.transform.GetComponent<InteractiveObj>() != null)
        //        {
        //            if(hit.transform.GetComponent<InteractiveObj>().IsUsed)
        //                return true;
        //        }
        //        AttackEnemy(hit.transform);
        //        return true;
        //    }
        //}
        //else if (Physics.Raycast(ray, out hit, 100f, 1 << LayerMask.NameToLayer("DropItem")) && !isIgnoreEnemy)
        //{
        //    PickItem(hit.transform);
        //    return true;
        //}
        //else
        if (Physics.Raycast(ray, out hit, 100f, 1<<LayerMask.NameToLayer("Walkable")))
        {
            if (!isIgnoreEnemy)
                AttackTarget = null;
            pm.MoveTarget = hit.point;
        }
		else
		{
			return false;
		}

        float DisToTarget = (transform.position - pm.MoveTarget).magnitude;
        if (DisToTarget > StartMoveDistance)
        {
            //find path
            PathFindingState = Player.EPathFindingState.PathFinding_Processing;
            GetComponent<Seeker>().StartPath(transform.position, pm.MoveTarget);
        }
		
		if(Input.GetMouseButtonDown(0))
		{
			DrawMoveMarker();
		}
		
		return false;
	}

    public Transform FindTarget()
    {
        Transform _tempTarget = null;

        float minDis = 999;
        foreach(Transform _monster in CS_SceneInfo.Instance.MonsterList.Values)
        {
            float _tempDetectRadius = DetectRadius;
            Vector3 _tempPos = _monster.position;
            if (_monster.GetComponent<BaseHitableObject>())
            {
                _tempPos += Vector3.up * _monster.GetComponent<BaseHitableObject>().AvoidanceRadius / 2;
                _tempDetectRadius *= (1 + _monster.GetComponent<BaseHitableObject>().AvoidanceRadius / 2);
            }

            Vector3 _screenPos = GameCamera.Instance.gameCamera.WorldToScreenPoint(_tempPos);
            float _dis = Vector3.Distance(_screenPos, Input.mousePosition);
            if (_dis < _tempDetectRadius && _dis < minDis)
            {
                _tempTarget = _monster;
                minDis = _dis;
            }
        }

        if (!_tempTarget)
        {
            minDis = 999;
            foreach (Transform _enemy in AllEnemys)
            {
                float _tempDetectRadius = DetectRadius;
                Vector3 _tempPos = _enemy.position;
                if (_enemy.GetComponent<BaseHitableObject>())
                {
                    _tempPos += Vector3.up * _enemy.GetComponent<BaseHitableObject>().AvoidanceRadius / 2;
                    _tempDetectRadius *= (1 + _enemy.GetComponent<BaseHitableObject>().AvoidanceRadius / 2);
                }

                Vector3 _screenPos = GameCamera.Instance.gameCamera.WorldToScreenPoint(_tempPos);
                float _dis = Vector3.Distance(_screenPos, Input.mousePosition);
                if (_dis < _tempDetectRadius && _dis < minDis)
                {
                    _tempTarget = _enemy;
                    minDis = _dis;
                }
            }
        }

        if (!_tempTarget)
        {
            List<Transform> interactiveObjsAndItems = new List<Transform>();
            foreach(Transform _interactiveObj in CS_SceneInfo.Instance.MiscThingList.Values)
            {
                interactiveObjsAndItems.Add(_interactiveObj);
            }
            foreach (Transform _interactiveObj in CS_SceneInfo.Instance.ItemList.Values)
            {
                interactiveObjsAndItems.Add(_interactiveObj);
            }


            foreach (Transform _interactiveObj in interactiveObjsAndItems)
            {
                Vector3 _tempPos = _interactiveObj.position;
                if (_interactiveObj.GetComponent<BaseHitableObject>())
                    _tempPos += Vector3.up * _interactiveObj.GetComponent<BaseHitableObject>().AvoidanceRadius / 2;
                Vector3 _screenPos = GameCamera.Instance.gameCamera.WorldToScreenPoint(_tempPos);
                float _dis = Vector3.Distance(_screenPos, Input.mousePosition);
                if (_dis < 50 && _dis < minDis)
                {
                    _tempTarget = _interactiveObj;
                    minDis = _dis;
                }
            }
        }

        return _tempTarget;
    }
	
	private void moveToNPC( Transform NPC )
	{
		AttackTarget = NPC;
		Vector3 dir = AttackTarget.position - transform.position;
		dir.y = 0;
		//transform.forward = dir;
		if(AttackTarget)
		{
			if(Vector3.Distance(transform.position, AttackTarget.position) > 2 )
				FSM.ChangeState(RS);
		}
	}

    #region PickUp Items
    public void PickItem(Transform _item)
    {
        PickupItem = _item;
		//------------------------------------------------------------------------------->>mm
		#if NGUI
		GameObject.Find("mouseCursors").gameObject.SendMessage("cursorClosedHandF");
		#endif
		//------------------------------------------------------------------------------->>#mm
        if (PickupItem)
        {
            if (Vector3.Distance(transform.position, PickupItem.position) < 2)
            {
                PickupItems();
            }
            else
            {
                GetComponent<PlayerMovement>().MoveTarget = PickupItem.position;
				PathFindingState = Player.EPathFindingState.PathFinding_Processing;
                GetComponent<Seeker>().StartPath(transform.position, PickupItem.position);
            }
        }
    }

    public void PickupItems()
    {
        if (PickupItem)
        {
			
            Item Myitem = PickupItem.GetComponent<Item>();

            if (Myitem == null && PickupItem.parent != null)
                Myitem = PickupItem.parent.GetComponent<Item>();

            if (Myitem != null)
                CS_Main.Instance.g_commModule.SendMessage(ProtocolBattle_SendRequest.PickupItemReq(Myitem.ObjectID));

            PickupItem = null;
			#if NGUI
			GameObject.Find("mouseCursors").gameObject.SendMessage("cursorNormalF");
			#endif
        }
    }

    /// <summary>
    /// call back from server if pick up item successful.
    /// </summary>
    /// <param name="tempItem"></param>
    public void NotifyPickUpItem(Transform tempItem)
    {
        SoundHandler.PlayPickuPItemSound(tempItem);
        if (tempItem.GetComponent<LootDropMoving>())
        {
            tempItem.GetComponent<LootDropMoving>().RemoveLootVFX();
        }
        PickupItem = null;
    }
    #endregion

    #region Attack
    public void AttackEnemy()
    {
        AttackEnemy(AttackTarget);
    }
	
	public void AttackEnemy( Transform enemy )
	{
		AttackTarget = enemy;
		if(AttackTarget != null)
		{
			if(isInAttackRange(AttackTarget)){
#if NGUI
				if (AttackTarget.GetComponent<ShopNpc>()) {
#else
                if (AttackTarget.GetComponent<ShopNpc>() && (int)_UI_CS_ScreenCtrl.Instance.currentScreenType == (int)_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_INGAME_NORMAL) {
#endif
					AttackTarget.GetComponent<ShopNpc>().PopMenu();
					GetComponent<PlayerMovement>().StopMoveToNextState(true, IS);
					AttackTarget = null;
					return;
				}
				
                FSM.ChangeState(abilityManager.GetAbilityByID((uint)AbilityIDs.NormalAttack_1H_ID));
			}else{
				
				GetComponent<PlayerMovement>().MoveTarget = AttackTarget.position;
				PathFindingState = Player.EPathFindingState.PathFinding_Processing;
				GetComponent<Seeker>().StartPath(transform.position, AttackTarget.position);
			}
        }
	}

    public bool isInChaseRange(Transform enemy)
    {
        float dis = Vector3.Distance(enemy.position, transform.position);
        dis -= enemy.GetComponent<BaseHitableObject>().AvoidanceRadius;
        if (dis < ChaseRange) return true;
        return false;
    }
	
	public bool isInAttackRange( Transform enemy )
	{
        float dis = Vector3.Distance(enemy.position, transform.position);
        dis -= enemy.GetComponent<BaseHitableObject>().AvoidanceRadius;
		if(dis < AttackRange)	return true;
		return false;
	}
    #endregion

    public void DrawMoveMarker()
	{
		if(FSM.IsInState(IS) || FSM.IsInState(RS))
		{
			//draw move marker
			if(MoveMarkerPrefab != null)
			{
				Object.Instantiate(MoveMarkerPrefab, GetComponent<PlayerMovement>().MoveTarget + Vector3.up*0.2f, Quaternion.identity);
			}
		}
	}
	
	public void SetSkinOfStoneMaterial(bool isSkinStone)
	{
        if (!AnimationModel) return;

        Renderer[] renderers = AnimationModel.GetComponentsInChildren<Renderer>();

        foreach (Renderer renderer in renderers)
        {
            for (int i = 0; i < renderer.materials.Length; i++)
            {
                Material mtl = renderer.materials[i];
                if (mtl.HasProperty("_IsColor"))
                {
                    if (isSkinStone)
                        mtl.SetFloat("_IsColor", 0.0f);
                    else
                        mtl.SetFloat("_IsColor", 1.0f);
                }
            }
        }
	}
	
	public void SortEnemyPos()
	{
		AllEnemys.Sort(myComparer);
	}

    #region Animation Control
    public void PlayIdleAnim(bool IsAttackIdle)
	{
        if (!AnimationModel) return;
		
		AnimationModel.gameObject.renderer.useLightProbes = true ;
		
        WeaponBase.EWeaponType wt = EquipementMan.GetWeaponType();
		if(IsAttackIdle)
		{
			if(wt == WeaponBase.EWeaponType.WT_OneHandWeapon || wt == WeaponBase.EWeaponType.WT_NoneWeapon || wt == WeaponBase.EWeaponType.WT_DualWeapon)
				AnimationModel.animation.CrossFade("Aka_1H_Attack_Idle_1");	
			else if(wt == WeaponBase.EWeaponType.WT_TwoHandWeaponAxe)
				AnimationModel.animation.CrossFade("Aka_2H_Attack_Idle_1");	
			else
				AnimationModel.animation.CrossFade("Aka_2HNodachi_Attack_Idle_1");	
			
		}
		else
		{
            if (wt == WeaponBase.EWeaponType.WT_OneHandWeapon || wt == WeaponBase.EWeaponType.WT_NoneWeapon || wt == WeaponBase.EWeaponType.WT_DualWeapon)
				AnimationModel.animation.CrossFade("Aka_1H_Idle_1");
			else if(wt == WeaponBase.EWeaponType.WT_TwoHandWeaponAxe)
				AnimationModel.animation.CrossFade("Aka_2H_Idle_1");	
			else
				AnimationModel.animation.CrossFade("Aka_2HNodachi_Idle_1");				
		}		
	}
	
	public void PlayDamageAnim()
	{
        if (!FSM.IsInState(IS) && !FSM.IsInState(RS) && !FSM.IsInState(abilityManager.GetAbilityByID((uint)AbilityIDs.NormalAttack_1H_ID))) return;

        WeaponBase.EWeaponType wt = EquipementMan.GetWeaponType();

        if (wt == WeaponBase.EWeaponType.WT_OneHandWeapon || wt == WeaponBase.EWeaponType.WT_NoneWeapon || wt == WeaponBase.EWeaponType.WT_DualWeapon)
		{
			AnimationModel.animation.Blend("Aka_1H_Damage_Lt");
		}else if(wt == WeaponBase.EWeaponType.WT_TwoHandWeaponAxe){
			AnimationModel.animation.Blend("Aka_2H_Damage_Lt");	
		}else{
			AnimationModel.animation.Blend("Aka_2HNodachi_Damage_Lt");
		}
	}
	
	public void PlayDeathAnim()
	{
        WeaponBase.EWeaponType wt = EquipementMan.GetWeaponType();

        if (wt == WeaponBase.EWeaponType.WT_OneHandWeapon || wt == WeaponBase.EWeaponType.WT_NoneWeapon || wt == WeaponBase.EWeaponType.WT_DualWeapon)
            AnimationModel.animation.CrossFade("Aka_1H_Death_1");
        else if (wt == WeaponBase.EWeaponType.WT_TwoHandWeaponAxe)
            AnimationModel.animation.CrossFade("Aka_2H_Death_1");
        else
            AnimationModel.animation.CrossFade("Aka_2HNodachi_Death_1");
	}

    public void PlayStunAnim()
    {
        Debug.Log("[Player] Play Stun Animation.");
        if (!AnimationModel) return;
        WeaponBase.EWeaponType wt = EquipementMan.GetWeaponType();
        if (wt == WeaponBase.EWeaponType.WT_NoneWeapon)
            AnimationModel.animation.CrossFade("Aka_0H_Stun_1");
        if (wt == WeaponBase.EWeaponType.WT_OneHandWeapon || wt == WeaponBase.EWeaponType.WT_DualWeapon)
            AnimationModel.animation.CrossFade("Aka_1H_Stun_1");
        else if (wt == WeaponBase.EWeaponType.WT_TwoHandWeaponAxe)
            AnimationModel.animation.CrossFade("Aka_2H_Stun_1");
        else
            AnimationModel.animation.CrossFade("Aka_2HNodachi_Stun_1");	
    }

    public void PlayKnockbackAnim()
    {
        if (!AnimationModel) return;
        WeaponBase.EWeaponType wt = EquipementMan.GetWeaponType();
        if (wt == WeaponBase.EWeaponType.WT_NoneWeapon)
            AnimationModel.animation.CrossFade("Aka_0H_KnockBack_1");
        if (wt == WeaponBase.EWeaponType.WT_OneHandWeapon || wt == WeaponBase.EWeaponType.WT_DualWeapon)
            AnimationModel.animation.CrossFade("Aka_1H_KnockBack_1");
        else if (wt == WeaponBase.EWeaponType.WT_TwoHandWeaponAxe)
            AnimationModel.animation.CrossFade("Aka_2H_KnockBack_1");
        else
            AnimationModel.animation.CrossFade("Aka_2HNodachi_KnockBack_1");	
    }
	
	public void PlayReviveAnim()
	{
		//AnimationModel.animation.CrossFade("Aka_Shockwave_1H");
	}

    public void PlayLevelUpAnim()
    {
        if (!AnimationModel) return;
        WeaponBase.EWeaponType wt = EquipementMan.GetWeaponType();
        Debug.LogError(wt.ToString());
        if (wt == WeaponBase.EWeaponType.WT_NoneWeapon)
        {
            AnimationModel.animation.CrossFade("Aka_RainofBlows_0H");
            AnimationModel.animation.CrossFadeQueued("Aka_1H_Idle_1");	
			
        }
        else if (wt == WeaponBase.EWeaponType.WT_OneHandWeapon)
        {
            AnimationModel.animation.CrossFade("CUN_UI_Idle_2");
            AnimationModel.animation.CrossFadeQueued("Aka_1H_Idle_1");	
			levelUp.Instance.WaitAniCTime();
        }
        else if (wt == WeaponBase.EWeaponType.WT_DualWeapon)
        {
            AnimationModel.animation.CrossFade("POW_UI_Idle_2");
            AnimationModel.animation.CrossFadeQueued("Aka_1H_Idle_1");	
			levelUp.Instance.WaitAniPTime();
        }
        else if (wt == WeaponBase.EWeaponType.WT_TwoHandWeaponAxe)
        {
            AnimationModel.animation.CrossFade("FOR_UI_Idle_2");
            AnimationModel.animation.CrossFadeQueued("Aka_2H_Idle_1");	
			levelUp.Instance.WaitAniFTime();
        }
        else
        {
            Debug.Log("1231");
            //AnimationModel.animation.CrossFade("FOR_UI_Idle_2");
            AnimationModel.animation.CrossFadeQueued("Aka_2HNodachi_Idle_1");	
        }
    }
    #endregion

    void SetLayerRecursively(Transform temp,int layer)
    {
		temp.gameObject.layer = layer;
		for(int i = 0; i < temp.GetChildCount();i++)
		{
			SetLayerRecursively(temp.GetChild(i),layer);
		}
    }

    public IEnumerator AutoSelectattackTarget()
    {
        yield return new WaitForSeconds(0.5f);

        foreach (Transform monster in AllEnemys)
        {
            if (monster.GetComponent<NpcBase>() != null && monster.GetComponent<NpcBase>().AttrMan.Attrs[EAttributeType.ATTR_CurHP] > 0)
            {
                float DisToTarget = (transform.position - monster.position).magnitude;

                if (DisToTarget <= AttackRange + monster.GetComponent<NpcBase>().AvoidanceRadius)
                {
                    AttackTarget = monster;

                    break;
                }
            }
        }

        if (AttackTarget == null)
        {
            FSM.ChangeState(IS);
        }
        else
        {
            Renderer NpcRenderer = AttackTarget.GetComponentInChildren<Renderer>();
            if (DrawOutline.Instance)
            {
                DrawOutline.Instance.DrawOulineForObject(AttackTarget);
            }
            FSM.ChangeState(abilityManager.GetAbilityByID((uint)AbilityIDs.NormalAttack_1H_ID));
        }
    }
	
	//All Utiliy functions
	public bool IsClickOnUI()
	{
#if NGUI
		Ray ray = UICamera.currentCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        int layer = 1 << LayerMask.NameToLayer("NGUI");
        if (Physics.Raycast(ray, out hit, 100f, layer)) {
            return true;
        }
		return false;
#else
		if(_UI_CS_Ctrl.Instance)
		{
			if((_UI_CS_Ctrl.Instance.m_UI_Manager && _UI_CS_Ctrl.Instance.m_UI_Manager.GetComponent<UIManager>().DidAnyPointerHitUI())|| Time.timeScale == 0)
				return true;
		}
		return false;
#endif
	}
	
	public void SetPlayerWeaponVisible(bool IsVisible)
	{
        if (EquipementMan.RightHandWeapon != null)
		{
            EquipementMan.RightHandWeapon.gameObject.SetActive(IsVisible);
		}
        if (EquipementMan.LeftHandWeapon != null)
		{
            EquipementMan.LeftHandWeapon.gameObject.SetActive(IsVisible);
			
		}
		
	}
	
	public override string DoExport()
    {
	     XMLStringWriter xmlWriter = new XMLStringWriter();
		
		 xmlWriter.NodeBegin("Player");
		
		 xmlWriter.AddAttribute("SpawnPosX",transform.position.x);
		
		 xmlWriter.AddAttribute("SpawnPosY",transform.position.y);
		
		 xmlWriter.AddAttribute("SpawnPosZ",transform.position.z);
        
		 xmlWriter.NodeEnd("Player");
		
		 return xmlWriter.Result;
    }
	
	public void CallOnSpirit( SpiritBase.eSpiriteType kType)
	{
		if( AttachedSpirit != null)
		{
			CallOffSpirit(AttachedSpirit.mSpiritType);
		}
		
		foreach( Transform it in  CS_SceneInfo.Instance.SpiritPrefabs)
		{
			SpiritBase mySpiritPrefab   = null;
			
			if( it != null)
				mySpiritPrefab = it.GetComponent<SpiritBase>();
			
			if( mySpiritPrefab != null &&  mySpiritPrefab.mSpiritType == kType)
			{
				AttachedSpirit = Instantiate(mySpiritPrefab) as SpiritBase;
				AttachedSpirit.CallOn();
				break;
			}
		}
	}
	
	public void CallOffSpirit()
	{
		if( AttachedSpirit != null)
		{
			AttachedSpirit.CallOff();
			AttachedSpirit = null;	
		}
	}
		
	public void CallOffSpirit( SpiritBase.eSpiriteType kType)
	{
		if( AttachedSpirit != null && AttachedSpirit.mSpiritType == kType)
		{
			AttachedSpirit.CallOff();
			AttachedSpirit = null;	
		}
	}
	
	public Vector3 GetEulerAngles(){
		return transform.GetComponent<PlayerMovement>().PlayerObj.eulerAngles;
	}
}

public class TransformCompare: IComparer<Transform>
{
	public int Compare(Transform x,Transform y)
	{
		if(x == null || y == null)
			return 0;
		
		float disX = (Player.Instance.transform.position - x.position).magnitude;
		float disY = (Player.Instance.transform.position - y.position).magnitude;
		
		if(disX < disY)
			return -1;
		else
			return 1;
	}
}







	
