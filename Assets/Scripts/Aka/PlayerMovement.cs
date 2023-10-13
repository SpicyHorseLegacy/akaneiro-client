using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerMovement : MonoBehaviour {
	
	public Transform PlayerObj;
		
    public float MoveSpeed = 6f;
	public float DashSpeed = 50f;
	public float RotateSpeed = 5f;
	public float PickNextWaypointDistance = 3.0f;
	public float PickUpRange=1f;
	
	public bool hoveringButton = false;
	
	public Transform MovementSound;
	private KGFMapSystem itsKGFMapSystem;
	
	[HideInInspector]
	public Vector3 MoveDir = Vector3.zero;
    [HideInInspector]
    public Vector3 MoveTarget
    {
        get
        {
            return _moveTarget;
        }
        set
        {
            _moveTarget = value;
            Debug.DrawLine(_moveTarget, _moveTarget + Vector3.up * 10, Color.red, 5.0f);
        }
    }
    Vector3 _moveTarget = Vector3.zero;
	//[HideInInspector]
	public bool IsMoving = false;
    [HideInInspector]
    public bool IsFreezed
    {
        get
        {
            return _isFreezed;
        }
        set
        {
            _isFreezed = value;
            if (_isFreezed)
            {
                StopMove(false);
            }
            else
            {
                bStopMove = false;
            }
        }
    }
    bool _isFreezed;
	
	Vector3 [] pathPoints = new Vector3[0];
	[HideInInspector]
	public int curPathPointIndex=0;
	float ResearchTime=0f;
	
	//net variables
	float TimeInterval = 0f;
	
	//[HideInInspector]
	public bool bStopMove=false;		// 控制是否在移动过程中不断的切换到Runstate TRUE : Complete DO NOT move
	//[HideInInspector]
	public bool IsDash = false;
	
	public List<Transform> togetherObjs = new List<Transform>();
	
	[HideInInspector] public bool dashHitEnemy;
	
	// Use this for initialization
	void Start () {
		itsKGFMapSystem = KGFAccessor.GetObject<KGFMapSystem>();
	}

    void Update()
    {
        if (PlayerObj != null && CreateScreenCtrl.Instance && PlayerObj.gameObject.activeSelf)
            PlayerObj.gameObject.SetActive(false);
        else if (PlayerObj != null && CreateScreenCtrl.Instance == null && !PlayerObj.gameObject.activeSelf)
            PlayerObj.gameObject.SetActive(true);
    }

    Vector3 startPoint;
    void OnDrawGizmosSelected()
    {
        if (pathPoints.Length > 0)
        {
            Vector3 _lastPoint = startPoint;
            for (int i = 0; i < pathPoints.Length; i++)
            {
                Gizmos.color = Color.blue;
                Gizmos.DrawSphere(pathPoints[i], 0.1f);
                if (_lastPoint != pathPoints[i])
                {
                    Gizmos.color = Color.red;
                    Gizmos.DrawLine(pathPoints[i], _lastPoint);
                    _lastPoint = pathPoints[i];
                }
            }
        }
    }
	
	// called from Player.Instance Update, once per frame
	public void Execute () 
	{
		if(IsMoving)
		{
			// 如果角色在跑步状态而且有攻击目标，则不用再次搜索了，因为跑步状态也在搜索，而且这里搜索的结果会在runstate走一圈后才出现，而这时可能runstate已经因为离目标近而进去攻击状态，反而出错
			// 角色会反复退出攻击状态进入跑步状态，结果因为和敌人距离很近，又退出跑步状态，进入攻击状态，而出现问题
			//------------------------------------------------------------------------------------
			if(Player.Instance.FSM.IsInState(Player.Instance.RS) && Player.Instance.AttackTarget)
				return;
			//------------------------------------------------------------------------------------
			
			//Debug.DrawLine (transform.position, pathPoints[curPathPointIndex], Color.blue);
//			ResearchTime+= Time.deltaTime;
//			if(ResearchTime>0.02f && !Input.GetMouseButton(0)&&!Input.GetMouseButton(1))
//			{
//				ResearchTime=0f;
//	
//				PlayerMovement pm = GetComponent<PlayerMovement>();
//				pm.MoveDir = pm.MoveTarget - transform.position; 
//				pm.MoveDir = Vector3.Normalize(pm.MoveDir);
//	
//				Player.Instance.PathFindingState = Player.EPathFindingState.PathFinding_Processing;
//				GetComponent<Seeker>().StartPath(transform.position,pm.MoveTarget);
//			}
		}
	}
	
	void LateUpdate ()
	{
		MoveToTarget();
	}
	

	public void MoveToTarget()
	{
		if(!IsMoving || IsFreezed)
			return;

        Debug.DrawRay(MoveTarget, Vector3.up * 4);

        #region TURN_CONTROL
        // 控制方向，为了让绑在角色身上的特效不会因为角色转向而跟着旋转，所以transform控制移动，PlayerObj用于控制转向
		// --------------------------------------------------------------------------------------------------------------------------------
		Vector3 direction = pathPoints[curPathPointIndex] - PlayerObj.position;
		if(IsDash)
			direction = MoveTarget - PlayerObj.position;
		
		direction.y = 0;
		
		// if direction is zero, that means player is moving to the original position, so stop player.
		if(direction == Vector3.zero)	StopMoveForAbility();
		
		if(IsDash){
			// If isDash, Player rotate to face the target position instantly.
			LookAtDirection(direction);
		}else{
			// Rotate towards the target
			if(direction != Vector3.zero)
				PlayerObj.rotation = Quaternion.Slerp (PlayerObj.rotation, Quaternion.LookRotation(direction), RotateSpeed * Time.deltaTime);
		}
		// --------------------------------------------------------------------------------------------------------------------------------
		// 根据角色与目标方向的夹角，控制速度因子，如果方向完全正确，速度因子就是1，如果与目标方向相反，则速度因子就是0，等于完全不动
		// Modify speed so we slow down when we are not facing the target
		// --------------------------------------------------------------------------------------------------------------------------------
		Vector3 forward = PlayerObj.forward;
		forward.y=0;
		float speedModifier = Vector3.Dot(forward, direction.normalized);
		speedModifier = Mathf.Clamp01(speedModifier);
        // --------------------------------------------------------------------------------------------------------------------------------
        #endregion

        float moveSpeed = 0;
		
		// Move the character
		if(IsDash)
			moveSpeed = DashSpeed * speedModifier;
		else 
			moveSpeed = MoveSpeed * (Player.Instance.AttrMan.Attrs[EAttributeType.ATTR_MoveSpeed] / 100.0f) * speedModifier;

        if (moveSpeed < 0) moveSpeed = 0;

        Player.Instance.AnimationModel.animation["Aka_1H_Run"].speed = Player.Instance.AttrMan.Attrs[EAttributeType.ATTR_MoveSpeed] / 100.0f;
        Player.Instance.AnimationModel.animation["Aka_2H_Run"].speed = Player.Instance.AttrMan.Attrs[EAttributeType.ATTR_MoveSpeed] / 100.0f;
        Player.Instance.AnimationModel.animation["Aka_2HNodachi_Run"].speed = Player.Instance.AttrMan.Attrs[EAttributeType.ATTR_MoveSpeed] / 100.0f;
		
		// 如果不是在冲刺的情况下，判断角色停下来的代码
        //if(!IsDash)
        //{
        //    float StopDistance=0.3f;
        //    if(curPathPointIndex+1 >= pathPoints.Length)
        //    {
        //        if(Player.Instance.PickupItem)
        //        {
        //            //StopDistance = PickUpRange;
        //        }
        //    }
        //    //if (direction.magnitude <= StopDistance ) 
        //    //{
        //    //    //curPathPointIndex++;
        //    //    if(curPathPointIndex>= pathPoints.Length)
        //    //    {
        //    //        StopMoveForAbility();
        //    //        if(CS_Main.Instance)
        //    //            CS_Main.Instance.g_commModule.SendMessage(ProtocolBattle_SendRequest.UpdateMoveRequest(transform.position, pathPoints[pathPoints.Length - 1], PlayerObj.eulerAngles.y, CS_SceneInfo.Instance.SyncTime));
        //    //    }
        //    //    return;
        //    //}
        //}else{
        //    //float distance = Vector3.Distance(pathPoints[curPathPointIndex], new Vector3(transform.position.x, pathPoints[curPathPointIndex].y, transform.position.z));
        //    float distance = Vector3.Distance(MoveTarget,new Vector3(transform.position.x, MoveTarget.y, transform.position.z));
			
        //    if(distance < moveSpeed * Time.deltaTime)
        //    {
        //        RaycastHit hit;
        //        if(Physics.Raycast(MoveTarget + Vector3.up*5f,Vector3.down,out hit,10f,1 << LayerMask.NameToLayer("Walkable")))
        //        {
        //            MoveTarget = hit.point;
        //        }

        //        CheckIfWall(MoveTarget);

        //        StopMoveForAbility();
        //        CS_Main.Instance.g_commModule.SendMessage(ProtocolBattle_SendRequest.UpdateMoveRequest(transform.position, transform.position, PlayerObj.eulerAngles.y, CS_SceneInfo.Instance.SyncTime));
        //        return;
        //    }
        //}
		
		// --------------------------------------------------------------------------------------------------------------------------------
		// check if there is collider in the front of character. ignore their collider
		// 可以穿过的墙体在冲刺前就进行了判断，所以这里只需要判断前面的敌人
		// --------------------------------------------------------------------------------------------------------------------------------
        //if(IsDash)
        //{
        //    Vector3 nextPos = transform.position + transform.forward * moveSpeed * Time.deltaTime;
        //}
		
		if(togetherObjs.Count > 0)
		{
			for(int i = togetherObjs.Count - 1; i >= 0; i --)
			{
				// if object has been destroied, continue.
				if(!togetherObjs[i])
				{
					togetherObjs.RemoveAt(i);
					continue;
				}
					
				float dis = Vector3.Distance(togetherObjs[i].position, transform.position);
				if(dis > 2)
				{
					if(togetherObjs[i].collider)
						togetherObjs[i].collider.isTrigger = false;
					togetherObjs.RemoveAt(i);
				}
			
			}
		}
		// --------------------------------------------------------------------------------------------------------------------------------
		if(itsKGFMapSystem != null)
		{
			/*in addition to fix map bug*/
			if(itsKGFMapSystem.GetHoverType() == "normal"){
				hoveringButton = false;
			}else{
				hoveringButton = true;
			}
		}
		if (hoveringButton == false){
			direction = forward * moveSpeed;
		
			MoveCha(direction, moveSpeed);
		}
	}
	
	private void MoveCha(Vector3 direction, float moveSpeed)
    {

        #region CHECK_IF_AUDIO_TRIGGER
        // check if there is a audiotrigger in front of avatar.
        if (moveSpeed > 0)
        {
            RaycastHit[] hits = Physics.RaycastAll(transform.position + Vector3.up * 1f, PlayerObj.forward, moveSpeed * Time.deltaTime);
            if (hits.Length > 0)
            {
                foreach (RaycastHit _hit in hits)
                {
                    if (_hit.transform.GetComponent<AudioTrigger>())
                    {
                        _hit.transform.GetComponent<AudioTrigger>().ActiveTrigger();
                    }
                }
            }
        }
        #endregion
        
        #region MOVE_CONTROL
        if (!IsDash)
        {
            #region CHECK_IF_OBSCALTE_IN_FRONT_OF_PLAYER
            /*
            int layer = 1 << LayerMask.NameToLayer("NPC") | 1 << LayerMask.NameToLayer("Breakable") | 1 << LayerMask.NameToLayer("InteractiveOBJ");
            layer |= 1 << LayerMask.NameToLayer("Collision") | 1 << LayerMask.NameToLayer("DashThroughWall");
            Vector3 _point1 = transform.position + Vector3.up * (Player.Instance.GetComponent<CharacterController>().radius + 0.15f);
            Vector3 _point2 = _point1 + Vector3.up * (Player.Instance.GetComponent<CharacterController>().height - Player.Instance.GetComponent<CharacterController>().radius);
            RaycastHit hit;
            if (Physics.CapsuleCast(_point1, _point2, Player.Instance.GetComponent<CharacterController>().radius + 0.1f, direction.normalized, out hit, 0.8f, layer))
            {
                StopMoveForAbility();
                return;
            }
            */
            #endregion

            float _moveLength = Vector3.Distance(transform.position, transform.position + (direction * Time.deltaTime));
            Vector3 _lastPoint = transform.position;
            bool isEnd = false;
            while (Vector3.Distance(_lastPoint, pathPoints[curPathPointIndex]) < _moveLength)
            {
                _moveLength -= Vector3.Distance(transform.position, pathPoints[curPathPointIndex]);
                _lastPoint = pathPoints[curPathPointIndex];
                curPathPointIndex++;
                if (curPathPointIndex >= pathPoints.Length)
                {
                    curPathPointIndex = pathPoints.Length - 1;
                    isEnd = true;
                    break;
                }
            }
            Vector3 _targetPos = pathPoints[curPathPointIndex];
            if (!isEnd)
            {
                _targetPos = _lastPoint + (pathPoints[curPathPointIndex] - _lastPoint).normalized * _moveLength;
            }
            else
            {
               // transform.position = CS_SceneInfo.pointOnTheGround(_targetPos);
                ;// Debug.Log("[Move] END PATH");
            }
            Vector3 _dir = _targetPos - transform.position;
            _dir.y = 0;
            GetComponent<CharacterController>().Move(_dir);
            //transform.position = _targetPos;
        }
        else
        {
            float distance = Vector3.Distance(MoveTarget, new Vector3(transform.position.x, MoveTarget.y, transform.position.z));

            if (distance < direction.magnitude * Time.deltaTime)
            {
                transform.position = MoveTarget;
                StopMoveForAbility();
                CS_Main.Instance.g_commModule.SendMessage(ProtocolBattle_SendRequest.UpdateMoveRequest(transform.position, transform.position, PlayerObj.eulerAngles.y, CS_SceneInfo.Instance.SyncTime));
                return;
            }
            else
            {
                Vector3 _targetPos = transform.position + direction * Time.deltaTime;
                transform.position = _targetPos;
            }
        }

        transform.position = CS_SceneInfo.pointOnTheGround(transform.position);

        #endregion

        #region SEND_POS_TO_SERVER
        //send aka's pos and rot to server
		TimeInterval += Time.deltaTime;
		if(TimeInterval > 0.1f)
		{
			TimeInterval = 0f;
            SendPositionToServer();
        }
        #endregion
    }

    void CheckIfWall(Vector3 tempNextPos)
    {
        return;
        RaycastHit hit;

        tempNextPos = pointOnTheGround(tempNextPos);
        Vector3 tempDir = tempNextPos - transform.position;
        int tempLayer = 1 << LayerMask.NameToLayer("Default") | 1 << LayerMask.NameToLayer("Collision");
        //Debug.DrawLine(transform.position + Vector3.up * 1f, tempNextPos + Vector3.up, Color.blue, 3f);
        if (Physics.Raycast(transform.position + Vector3.up * 1f, tempDir, out hit, Vector3.Distance(tempNextPos, transform.position) *2, tempLayer))
        {
            StopMoveForAbility();
            tempNextPos = pointOnTheGround(hit.point - tempDir.normalized);
        }

        transform.position = tempNextPos;
    }

    void OnCollisionEnter(Collision collision)
    {
        foreach (ContactPoint contact in collision.contacts)
        {
            Debug.DrawRay(contact.point, contact.normal, Color.white);
        }
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Debug.DrawRay(hit.point, hit.normal, Color.white);
        if (hit.gameObject.layer != LayerMask.NameToLayer("Walkable"))
        {
            StopMoveForAbility();
        }
    }
	
	/// <summary>
	/// 停下来，并且判断怎么停下来的然后进行进行处理
	/// </summary> 
	public void StopMoveForAbility()
	{
		// 用于控制这一帧发送玩家的位置信息
		TimeInterval = 1;

        IsMoving = false;
		curPathPointIndex=0;
		
		// if Aka is in Dash mode, send message to play the last animation.
        if (IsDash)
        {
            if (Player.Instance.FSM.IsInState(Player.Instance.abilityManager.GetAbilityByID((uint)AbilityIDs.SwathOfDestruction_ID))
                || Player.Instance.FSM.IsInState(Player.Instance.abilityManager.GetAbilityByID((uint)AbilityIDs.SwathOfFlame_ID))
                || Player.Instance.FSM.IsInState(Player.Instance.abilityManager.GetAbilityByID((uint)AbilityIDs.SwathOfDestructionIII_ID))
                || Player.Instance.FSM.IsInState(Player.Instance.abilityManager.GetAbilityByID((uint)AbilityIDs.SwathOfDestructionIV_ID))
                || Player.Instance.FSM.IsInState(Player.Instance.abilityManager.GetAbilityByID((uint)AbilityIDs.SwathOfDestructionV_ID))
                || Player.Instance.FSM.IsInState(Player.Instance.abilityManager.GetAbilityByID((uint)AbilityIDs.SwathOfDestructionVI_ID)))
            {
                PlayerAbilitySwath swaState = (PlayerAbilitySwath)Player.Instance.FSM.GetCurrentState();
                swaState.EndStep(dashHitEnemy);
            }
            IsDash = false;
            SetTriggersAroundPlayer();
            return;
        }
        else
        {
        }
		
		if(Player.Instance.PGS.IsSkillOn)
		{
			PlayerAbilityBaseState curState = (PlayerAbilityBaseState) Player.Instance.FSM.GetCurrentState();
			curState.PlayerAnimationAfterRun();
			return;
		}
		
		Player.Instance.FSM.ChangeState(Player.Instance.IS);
		
		/*
		if(Player.Instance.PGS.IsMovingToUseRainOfBlows)
			Player.Instance.FSM.ChangeState(Player.Instance.abilityManager.GetAbilityByID((uint)AbilityIDs.RainOfBlows_ID));
		else
			Player.Instance.FSM.ChangeState(Player.Instance.IS);
		*/
	}

    public void Freeze()
    {
        IsFreezed = true;
    }

    public void ReleaseFreeze()
    {
        IsFreezed = false;
    }

    public void ModifyTargetPosition(Vector3 _pos)
    {
  //      if()
    }
	
    /// <summary>
    /// Stop Character moving.
    /// </summary>
    /// <param name="continuePathfinding">If contineue moving if path completed.</param>
	public void StopMove( bool continuePathfinding )
	{
		// 用于控制这一帧发送玩家的位置信息
		TimeInterval = 1;
		
		IsDash = false;
		IsMoving = false;
		curPathPointIndex=0;
		bStopMove = !continuePathfinding;
	}
	
	/// <summary>
	/// 无条件停止并且进入另外的State
	/// </summary> 
	public void StopMoveToNextState( bool continuePathfinding, State nextState )
	{
		// 用于控制这一帧发送玩家的位置信息
		TimeInterval = 1;
		
		IsDash = false;
		IsMoving = false;
		curPathPointIndex=0;
		bStopMove = !continuePathfinding;

        //StopMoveForAbility();
		
        if(!Player.Instance.FSM.IsInState(Player.Instance.DS))
		    Player.Instance.FSM.ChangeState(nextState);
	}
	
	/// <summary>
	/// finding path completed, init moving.
	/// 寻路成功，初始化移动
	/// </summary>
	/// <param name="points">
	/// A <see cref="Vector3[]"/>
	/// </param>
	public void PathComplete (Vector3[] points) 
	{
		Player.Instance.PathFindingState = Player.EPathFindingState.PathFinding_Succeed;
        startPoint = transform.position;
		
		// if Aka is in Dash Mode(2 kinds of swath ability would enter dash mode)
		// 如果Aka在冲刺模式中（两个Swath技能会进入这种模式）
		//if(IsDash)
		//	return;
		
		// 如果是在技能中，则大部分会停住然后放技能，而寻路会有一个延迟，所以这个会在Change到技能状态后调用，所以需要停住的技能都会把bStopMove设置为true，调用时就忽略
		// 有次操作的技能包括
		//	---------------
		// ShockWave
		// Shoot * 2
		// MeteorRain
		// HungryCleave
		// RainBlow
		// IceBarricade
		// Swath * 2    <--- 有特殊处理
		// Scream
		// Trap * 2
		// NinjaEscape
		// Caltrop
		// -------------------
		if(!bStopMove)
		{
			pathPoints = points;
			curPathPointIndex=0;
			IsMoving = true;
			
			if(!Player.Instance.FSM.IsInState(Player.Instance.RS))
			{
				Player.Instance.FSM.ChangeState(Player.Instance.RS);
			}
		}
		
		// 如果是冲刺技能的话，则还要调用这里，因为冲刺也会用到寻路，但是不能进入RS状态，所以单独处理//
        if (Player.Instance.FSM.IsInState(Player.Instance.abilityManager.GetAbilityByID((uint)AbilityIDs.SwathOfDestruction_ID))
            || Player.Instance.FSM.IsInState(Player.Instance.abilityManager.GetAbilityByID((uint)AbilityIDs.SwathOfFlame_ID))
            || Player.Instance.FSM.IsInState(Player.Instance.abilityManager.GetAbilityByID((uint)AbilityIDs.SwathOfDestructionIII_ID))
            || Player.Instance.FSM.IsInState(Player.Instance.abilityManager.GetAbilityByID((uint)AbilityIDs.SwathOfDestructionIV_ID))
            || Player.Instance.FSM.IsInState(Player.Instance.abilityManager.GetAbilityByID((uint)AbilityIDs.SwathOfDestructionV_ID))
            || Player.Instance.FSM.IsInState(Player.Instance.abilityManager.GetAbilityByID((uint)AbilityIDs.SwathOfDestructionVI_ID)))
        {
            pathPoints = points;
            curPathPointIndex = 0;
            IsMoving = true;
        }

        Vector3 _lastPoint = transform.position;
        foreach (Vector3 point in pathPoints)
        {
            //Debug.DrawLine(_lastPoint + Vector3.up, point + Vector3.up, new Color(Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f), Random.Range(0.0f, 1.0f)), 5);
            _lastPoint = point;
        }
	}
	
	public void PathError()
	{
		Player.Instance.PathFindingState = Player.EPathFindingState.PathFinding_Failed;
		//Debug.Log("No way is finded.");
		
		//if(Player.Instance.FSM.IsInState(AbilityInfo.Instance.GetAbilityByID(AbilityInfo.Instance.SwathOfFlame_ID)) ||
		//   Player.Instance.FSM.IsInState(AbilityInfo.Instance.GetAbilityByID(AbilityInfo.Instance.SwathOfDestruction_ID)) ||
		//   Player.Instance.FSM.IsInState(AbilityInfo.Instance.GetAbilityByID(AbilityInfo.Instance.NinjaEscape_ID)))
			StopMoveToNextState(true, Player.Instance.IS);
	}
	
	public void CancelInvaildMovement(UnityEngine.Vector3 pos, float rot, uint continueTime)
	{
		//return;
		if(pos.magnitude <0.001f)
		{
			transform.position = Player.Instance.SpawnPosition;
			PlayerObj.eulerAngles = Player.Instance.SpawnRotation;			
		}
		else
		{
			transform.position = pos;
			Vector3 angle = PlayerObj.eulerAngles;
			angle.y = rot;
			PlayerObj.eulerAngles = angle;
		}
		
		StopMoveToNextState(true, Player.Instance.IS);
	}

    public void SetTriggersAroundPlayer()
    {
        Collider[] colliders;
        // find all collisions around player
        colliders = Physics.OverlapSphere(transform.position, Player.Instance.GetComponent<CharacterController>().radius);

        if (colliders.Length > 0)
        {
            foreach (Collider collider in colliders)
            {
                if (collider.transform == transform) continue;
                if (collider.transform.gameObject.layer == LayerMask.NameToLayer("NPC") ||
                    collider.transform.gameObject.layer == LayerMask.NameToLayer("Breakable") ||
                    collider.transform.gameObject.layer == LayerMask.NameToLayer("InteractiveOBJ"))
                {
                    print("got :" + collider.transform.name);
                    // if the object is already be triggered, ignore it.
                    if (!togetherObjs.Contains(collider.transform))
                    {
                        collider.isTrigger = true;
                        togetherObjs.Add(collider.transform);
                        print("Put \"" + collider.transform.name + "\" into temp trigger list!");
                    }
                }
            }
        }
    }

    public void SendPositionToServer()
    {
        if (CS_Main.Instance != null)
        {
            CS_Main.Instance.g_commModule.SendMessage(ProtocolBattle_SendRequest.UpdateMoveRequest(transform.position, transform.position, PlayerObj.eulerAngles.y, CS_SceneInfo.Instance.SyncTime));
#if NGUI
			PlayerDataManager.Instance.CheckMissionProgress((int)_UI_CS_RamusTask.MISSION_TYPE.TRAVEL,(int)transform.position.x, (int)transform.position.z);
			PlayerDataManager.Instance.CheckMissionProgress((int)_UI_CS_RamusTask.MISSION_TYPE.PROTECT,(int)transform.position.x, (int)transform.position.z);
#else
            _UI_CS_MissionLogic.Instance.CheckMissionProgress(_UI_CS_MissionLogic.MissionType.TRAVEL, (int)transform.position.x, (int)transform.position.z);
            _UI_CS_MissionLogic.Instance.CheckMissionProgress(_UI_CS_MissionLogic.MissionType.PROTECT, (int)transform.position.x, (int)transform.position.z);
#endif

        }
    }
	
	public void LookAtDirection( Vector3 direction){
		if(direction != Vector3.zero)
			PlayerObj.forward = direction;
		else
			Debug.LogError("Direction is Vector3.zero!");
	}
	
	public void LookAtPosition( Vector3 position ){
		Vector3 dir = position - transform.position;
		dir.y = 0;
		LookAtDirection(dir);
	}
	
	/// <summary>
	/// Looks at mouse point.
	/// </summary>
	/// <param name='ignoreEnemy'>
	/// If false, client would check if there is hovering on an enemy? If so, set direction to look at the enemy .
	/// </param>
    public void LookAtMousePoint(bool ignoreEnemy)
    {
        LookAtMousePoint(Input.mousePosition, ignoreEnemy);
	}
	
	public void LookAtMousePoint(Vector3 mousePos, bool ignoreEnemy){
		//Ray ray = Camera.main.ScreenPointToRay(mousePos);
        Ray ray = GameCamera.Instance.gameCamera.ScreenPointToRay(mousePos);
		RaycastHit hit;
		
		int layer = 0;
        if (!ignoreEnemy)
        {
            layer = 1 << LayerMask.NameToLayer("NPC")
                   | 1 << LayerMask.NameToLayer("Breakable")
                   | 1 << LayerMask.NameToLayer("InteractiveOBJ");
            if (Physics.Raycast(ray.origin, ray.direction, out hit, 100f, layer) && Time.timeScale > 0)
            {
                LookAtPosition(hit.transform.position);
				return;
            }
        }
		
		layer = 1<<LayerMask.NameToLayer("Walkable");
		if(Physics.Raycast(ray.origin,ray.direction,out hit,100f,layer) && Time.timeScale > 0){
			LookAtPosition(hit.point);
		}
	}
	
	public void LookAtShootDir()
	{
        if (Player.Instance.HoverTarget)
        {
            LookAtPosition(Player.Instance.HoverTarget.position);
            return;
        }

		//Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Ray ray = GameCamera.Instance.gameCamera.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		int layer  = 0;
		
		// check if hovering on an enemy?
        layer = 1 << LayerMask.NameToLayer("NPC")
               | 1 << LayerMask.NameToLayer("Breakable")
               | 1 << LayerMask.NameToLayer("InteractiveOBJ");
        if (Physics.Raycast(ray.origin, ray.direction, out hit, 100f, layer) && Time.timeScale > 0)
        {
            LookAtPosition(hit.transform.position);
			return;
        }
		
		// check no enmey, look at the point mouse points.
		layer = 1<<LayerMask.NameToLayer("AbilityShootPlane");
		if(Physics.Raycast(ray.origin,ray.direction,out hit,100f,layer) && Time.timeScale > 0){
			LookAtPosition(hit.point);
		}
	}
	
	public void LookAtTargetMouseInShootMode(Vector3 targetMousePos)
	{
        //Ray ray = Camera.main.ScreenPointToRay(targetMousePos);
        Ray ray = GameCamera.Instance.gameCamera.ScreenPointToRay(targetMousePos);
		RaycastHit hit;
		int layer = 1<<LayerMask.NameToLayer("AbilityShootPlane");
		if(Physics.Raycast(ray.origin,ray.direction,out hit,100f,layer) && Time.timeScale > 0){
			//print("!!!yes");
			LookAtPosition(hit.point);
		}

        layer = 1 << LayerMask.NameToLayer("NPC")
               | 1 << LayerMask.NameToLayer("Breakable")
               | 1 << LayerMask.NameToLayer("InteractiveOBJ");
        if (Physics.Raycast(ray.origin, ray.direction, out hit, 100f, layer) && Time.timeScale > 0)
        {
            LookAtPosition(hit.transform.position);
        }
	}

    public Vector3 pointOnTheGround(Vector3 pos)
    {
        RaycastHit hitInfo;
        int layer = 1 << LayerMask.NameToLayer("Walkable");
        if (Physics.Raycast(pos + Vector3.up * 5, Vector3.down * 5, out hitInfo, 100f, layer))
            pos.y = hitInfo.point.y;
        return pos;
    }
}
