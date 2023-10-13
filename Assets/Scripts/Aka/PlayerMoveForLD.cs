using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerMoveForLD : MonoBehaviour {
	
	 public static PlayerMoveForLD Instance;
	
	public float Speed = 5;
    public GameObject AnimationModel;
		
    public float RotateSpeed = 5;
    public float MoveSpeed = 5;
	
	public bool hoveringButton = false;
	
	Animation Ani;
	CharacterController ChaController;
	
	private KGFMapSystem itsKGFMapSystem;
	
	void Awake()
	{
		//TargetPosition.parent = null;
				
		Instance = this;
        Ani = AnimationModel.GetComponent<Animation>();
		ChaController = GetComponent<CharacterController>();
		Ani["Aka_1H_Run"].wrapMode = WrapMode.Loop;
		Ani["Aka_1H_Idle_1"].wrapMode = WrapMode.Loop;
	}
	
	void Start (){
		itsKGFMapSystem = KGFAccessor.GetObject<KGFMapSystem>();
	}
	void Update()
	{
		if(itsKGFMapSystem != null)
		{
			if(itsKGFMapSystem.GetHoverType() == "normal"){
				hoveringButton = false;
			}else{
				hoveringButton = true;
			}
		}
	
	//Debug.DrawLine(TargetPosition.position, TargetPosition.position + Vector3.up * 5, Color.red);
		if(Input.GetMouseButton(0))
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit, 100f, 1<<LayerMask.NameToLayer("Walkable")) /*in addition to fix map bug*/&& hoveringButton == false)
	        {
	            //TargetPosition.position = hit.point;
                GetComponent<Seeker>().StartPath(transform.position, hit.point);
	        }
		}           
	}

    void LateUpdate()
    {
        if (!isMoving) return;

        #region TURN_CONTROL
        // 控制方向，为了让绑在角色身上的特效不会因为角色转向而跟着旋转，所以transform控制移动，PlayerObj用于控制转向
        // --------------------------------------------------------------------------------------------------------------------------------
        Vector3 direction = pathPoints[curPathPointIndex] - transform.position;
        direction.y = 0;

        // if direction is zero, that means player is moving to the original position, so stop player.
        if (direction == Vector3.zero) return;
        // Rotate towards the target
        if (direction != Vector3.zero)
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(direction), RotateSpeed * Time.deltaTime);
        // --------------------------------------------------------------------------------------------------------------------------------
        // 根据角色与目标方向的夹角，控制速度因子，如果方向完全正确，速度因子就是1，如果与目标方向相反，则速度因子就是0，等于完全不动
        // Modify speed so we slow down when we are not facing the target
        // --------------------------------------------------------------------------------------------------------------------------------
        Vector3 forward = transform.forward;
        forward.y = 0;
        float speedModifier = Vector3.Dot(forward, direction.normalized);
        speedModifier = Mathf.Clamp01(speedModifier);
        // --------------------------------------------------------------------------------------------------------------------------------
        #endregion

        float moveSpeed = 0;

        // Move the character
        moveSpeed = MoveSpeed * speedModifier;

        if (moveSpeed < 0) moveSpeed = 0;

        direction = forward * moveSpeed;

        MoveCha(direction, moveSpeed);
    }

    private void MoveCha(Vector3 direction, float moveSpeed)
    {
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
            StopMoving();
            Debug.Log("[Move] END PATH");
        }
        Vector3 _dir = _targetPos - transform.position;
        _dir.y = 0;
        GetComponent<CharacterController>().Move(_dir);

        transform.position = CS_SceneInfo.pointOnTheGround(transform.position);
    }

    public bool isMoving = false;
    public Vector3[] pathPoints = new Vector3[0];
    public int curPathPointIndex = 0;

    /// <summary>
    /// finding path completed, init moving.
    /// 寻路成功，初始化移动
    /// </summary>
    /// <param name="points">
    /// A <see cref="Vector3[]"/>
    /// </param>
    public void PathComplete(Vector3[] points)
    {
        pathPoints = points;
        curPathPointIndex = 0;
        isMoving = true;
        Ani.CrossFade("Aka_1H_Run");
    }

    public void PathError()
    {
    }
	
	void PlayMovementSound()
	{
	}

    void StopMoving()
    {
        pathPoints = new Vector3[0];
        Ani.CrossFade("Aka_1H_Idle_1");
        isMoving = false;
    }

    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Debug.DrawRay(hit.point, hit.normal, Color.white);
        if (hit.gameObject.layer != LayerMask.NameToLayer("Walkable"))
        {
            StopMoving();
        }
    }

    void OnDrawGizmosSelected()
    {
        if (pathPoints.Length > 0)
        {
            Vector3 _lastPoint = pathPoints[0];
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
}
