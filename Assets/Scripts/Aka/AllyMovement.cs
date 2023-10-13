using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AllyMovement : MonoBehaviour {

	public float MoveSpeed = 6f;
	public float DashSpeed = 18f;
	public float RotateSpeed = 5f;
	public float PickNextWaypointDistance = 3.0f;
	
	public Transform MovementSound;

	[HideInInspector]
	public Vector3 MoveDir = Vector3.zero;
	[HideInInspector]
	public Vector3 MoveTarget = Vector3.zero;
	[HideInInspector]
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
                StopMove();
            }
            else
            {

            }
        }
    }
    bool _isFreezed;
	
	Vector3 [] pathPoints;
	[HideInInspector]
	public int curPathPointIndex=0;

	//net variables
	float TimeInterval = 0f;
	
	[HideInInspector]
	public bool IsMoveCanceled=false;
	//[HideInInspector]
	public bool bStopMove=false;
	[HideInInspector]
	public bool IsDash = false;
	[HideInInspector]
	public AllyNpc OwnerNpc = null;
	[HideInInspector]
	public List<Transform> togetherObjs = new List<Transform>();
	
	
	// Update is called once per frame
	void Update () 
	{
		MoveToTarget();
	}

	public void MoveToTarget()
	{
		if(!IsMoving || IsFreezed)
			return;
		
		Vector3 direction = pathPoints[curPathPointIndex] - transform.position;
		direction.y = 0;
		
		float StopDistance=0.3f;
		
		if (direction.magnitude <= StopDistance ) 
		{
			curPathPointIndex++;
			if(curPathPointIndex>= pathPoints.Length)
			{
				//reach target point,stop move
				StopMove();
                CS_Main.Instance.g_commModule.SendMessage(ProtocolBattle_SendRequest.AllyUpdateMoveRequest(OwnerNpc.ObjID, transform.position, pathPoints[pathPoints.Length - 1], transform.eulerAngles.y, CS_SceneInfo.Instance.SyncTime));
			}
			return;
		}
		
		// Rotate towards the target
		transform.rotation = Quaternion.Slerp (transform.rotation, Quaternion.LookRotation(direction), RotateSpeed * Time.deltaTime);
	
		// Modify speed so we slow down when we are not facing the target
		Vector3 forward = transform.forward;
		forward.y=0;
		float speedModifier = Vector3.Dot(forward, direction.normalized);
		speedModifier = Mathf.Clamp01(speedModifier);
		
		// Move the character
		if(IsDash)
			direction = forward * DashSpeed * speedModifier;
		else
			direction = forward * MoveSpeed * speedModifier;
	
		MoveCha(direction);
	}
	
	private void MoveCha(Vector3 direction){

		CharacterController pc = GetComponent<CharacterController>();
		if(pc != null && pc.enabled)
			pc.Move(direction*Time.deltaTime);
		else
			transform.position = transform.position + direction*Time.deltaTime;
		
		RaycastHit hit;
		if(Physics.Raycast(transform.position + Vector3.up*5f,Vector3.down,out hit,10f,1 << LayerMask.NameToLayer("Walkable")))
		{
			transform.position = hit.point;
		}
		
	
		Collider[] colliders;
		
		int layer = 1 << LayerMask.NameToLayer("NPC")
               | 1 << LayerMask.NameToLayer("Breakable")
               | 1 << LayerMask.NameToLayer("InteractiveOBJ")
			   | 1 << LayerMask.NameToLayer("Default")
			   | 1 << LayerMask.NameToLayer("Trigger")
		       | 1 << LayerMask.NameToLayer("Collision")
		       | 1 << LayerMask.NameToLayer("Walkable");
		
	    colliders = Physics.OverlapSphere(transform.position,2,layer);
		
		if(colliders.Length > 0)
		{
			bool btrigger = true;
			foreach(Collider it in colliders)
			{
				if(it.GetComponent<TerrainCollider>())
					continue;
				
				if(!it.isTrigger && it != transform.collider)
				{
					btrigger = false;
					break;
				}
			}
			
			//Debug.Log("Pathing stopped; Object in Ally's path:" + " | " + hit.transform.name);
			if(pc != null && !btrigger)
			   pc.enabled = false;
		    else
			   pc.enabled = true;
		}
		else
		{
			if(pc != null)
			   pc.enabled = true;
		}
		
		//send aka's pos and rot to server
		TimeInterval += Time.deltaTime;
		if(TimeInterval > 0.1f)
		{
			TimeInterval = 0f;
			if(CS_Main.Instance != null)
			{
                CS_Main.Instance.g_commModule.SendMessage(ProtocolBattle_SendRequest.AllyUpdateMoveRequest(OwnerNpc.ObjID, transform.position, pathPoints[curPathPointIndex], transform.eulerAngles.y, CS_SceneInfo.Instance.SyncTime));
			}
		}		
	}

    public void Freeze()
    {
        IsFreezed = true;
    }

    public void ReleaseFreeze()
    {
        IsFreezed = false;
    }
	
	public void StopMove()
	{
		IsMoving = false;
		curPathPointIndex=0;

        OwnerNpc.FSM.ChangeState(OwnerNpc.IS);
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
        OwnerNpc.PathFindingState = AllyNpc.EPathFindingState.PathFinding_Succeed;
        if (bStopMove)
        {
            return;
        }
		pathPoints = points;
		
		bool bVerySmall = true;
		
		foreach( Vector3 it in pathPoints)
		{
			Vector3 direction = it - transform.position;
			
		    direction.y = 0;
		
		    float StopDistance=0.3f;
			
			if (direction.magnitude > StopDistance )
			{
			   bVerySmall = false;
			   break;	
			}
		}
		
		if(bVerySmall)
		{ 
			if(pathPoints.Length > 0)
			{
			  RaycastHit hit;
			  Vector3 temp = pathPoints[0];
		      if(Physics.Raycast(temp + Vector3.up*5f,Vector3.down,out hit,10f,1 << LayerMask.NameToLayer("Walkable")))
		      {
			     transform.position = hit.point;
		      }
			}
			//Debug.Log("Ally Move Position is too near from Ally Orig Location");
			return;
		}
		
		curPathPointIndex=0;
		IsMoving = true;
	
		if(IsDash)
		{
			IsMoving = true;
			return;
		}
		
		if(!OwnerNpc.FSM.IsInState(OwnerNpc.RS))
		{
			OwnerNpc.FSM.ChangeState(OwnerNpc.RS);
		}
	}
	
	public void PathError()
	{
		OwnerNpc.PathFindingState = AllyNpc.EPathFindingState.PathFinding_Failed;
		IsMoving = false;
		//Debug.Log("No way is finded.");
	}

    public void StopMove(bool continuePathfinding)
    {
        // 用于控制这一帧发送玩家的位置信息
        TimeInterval = 1;

        IsDash = false;
        IsMoving = false;
        curPathPointIndex = 0;
        bStopMove = !continuePathfinding;
    }
	
}
