using UnityEngine;
using System.Collections;

public class GameCamera : MonoBehaviour {

	public Camera gameCamera;
	public Camera ShadowCamera;
	
	public float blendingAnimationTimeFactor ;
	
	public static GameCamera  Instance;

	//public Transform NormalCameraState;
	//public Transform CombatCameraState;

	//public Transform DragCameraState;

	[HideInInspector]
	public bool IsPlayingCameraAnim = false;

	public Transform target;

	public CameraState PlayerMissionComplete_CS = null;
	public CameraState PlayerDeath_CS = null;
	public CameraState NPC_CS = null;
	public CameraState Close_CS = null;
	public CameraState Combat_CS =null;
	public CameraState Normal_CS =null;
	public CameraState Previous_CS = null;
	public CameraState Current_CS =null;

	[HideInInspector]
	public bool IsChangingState = false;
	public bool IsCloseState = false;

	public float StateChangeTime = 6.0f;
	public float DelayTimeToNormal = 5.0f;
	public float collcapRadius = 0.75f;

	float CurFollowSpeed=0.0f;
	Vector3 finalPosition=Vector3.zero;
	Vector3 CurOffsetToLookPoint = Vector3.zero;
	Vector3 CurLookAngle = Vector3.zero;

	Vector3 FromLookAngle = Vector3.zero;
	Vector3 FromOffsetToLookPoint = Vector3.zero;
	Vector3 ToLookAngle = Vector3.zero;
	Vector3 ToOffsetToLookPoint = Vector3.zero;

	//Free Camera Mode
	[HideInInspector]
	public bool IsFreeCameraMode = false;
	public float FreeCameraMovementSpeed = 10.0f;
	public float FreeCameraRotationSpeed = 100.0F;
	float angleH = 0f;
	float angleV = 0f;

	bool m_closeCamera = false;

	public static void CloseCamera()
	{
		Instance.gameCamera.enabled = false;
		Instance.m_closeCamera = true;
	}

	public static void OpenCamera()
	{
		Instance.gameCamera.enabled = true;
		Instance.m_closeCamera = false;
	}

	public static void EnterMissionCompleteState()
	{
		Instance.ChangeCameraState(Instance.PlayerMissionComplete_CS);
	}

	public static void EnterDeathState()
	{
		Instance.target = Player.Instance.transform.Find("Aka_Model/Root/Bip001/Bip001 Pelvis/Bip001 Spine");

		if(Instance.PlayerDeath_CS)
			Instance.ChangeCameraState(Instance.PlayerDeath_CS);
	}

	public static void EnterNomalState()
	{
		Instance.ChangeCameraState(Instance.Normal_CS);
	}

	public static void BackToPlayerCamera()
	{
		Instance.target = Player.Instance.transform;
		EnterNomalState();
	}

	void Awake()
	{
		Instance = this;
		DontDestroyOnLoad(this);
	}

	// Use this for initialization
	void Start ()
	{
		Current_CS = Normal_CS;

		if(Player.Instance != null)
			target = Player.Instance.transform;

		// 设置当先的位置和角度
		if(target)
		{
			CurLookAngle = Current_CS.LookAngle;
			if(gameCamera.enabled == true)gameCamera.fieldOfView = Current_CS.FOV;
			CurOffsetToLookPoint = Current_CS.OffsetToLookPoint;
			transform.position = target.position + CurOffsetToLookPoint;
		}
		else
		{
			Debug.Log("Camere can not find target!");
			return;
		}

		//Drag Camera State
		//if(DragCameraStateInstance == null && DragCameraState != null)
		//	DragCameraStateInstance = Instantiate(DragCameraState) as Transform;

//		if( DragCameraStateInstance && DragCameraStateInstance.GetComponent<DragCamera>() && DragCameraState)
//			DragCameraStateInstance.GetComponent<DragCamera>().SetProto(DragCameraState.GetComponent<DragCamera>());

	}

	public void ChangeCameraState(CameraState NewCameraState)
	{
		if (NewCameraState == null && NewCameraState != Current_CS)
			return;

		Previous_CS = Current_CS;
		Current_CS = NewCameraState;
	}

	public void BackToPreviousCS()
	{
		if (Previous_CS != null)
		{
			Current_CS = Previous_CS;
			Previous_CS = null;
		}
	}

	public void ResetCamera()
	{
		if(Player.Instance) {
			target = Player.Instance.transform;
		}else if(PlayerMoveForLD.Instance) {
			target = Player.Instance.transform;
		}
		
		ChangeCameraState(Normal_CS);
		CurLookAngle = Current_CS.LookAngle;
		Camera.main.fieldOfView = Current_CS.FOV;
		CurOffsetToLookPoint = Current_CS.OffsetToLookPoint;
		transform.position = target.position + Current_CS.OffsetToLookPoint;
	}

	// Update is called once per frame
	void Update ()
	{
		if(Input.GetKey(KeyCode.Home) && Input.GetKeyDown(KeyCode.LeftAlt) && Input.GetKeyDown(KeyCode.LeftControl))
		{
			IsFreeCameraMode = !IsFreeCameraMode;
			if(!IsFreeCameraMode)
			{
				ResetCamera();
				//Debug.Log("[CAM] Free Camera Mode is Turn Off.");
			}else{
				//Debug.Log("[CAM] Free Camera Mode is Turn ON.");
			}
		}

		if (Input.GetKeyDown(KeyCode.Z))
		{
			IsCloseState = !IsCloseState;
			ChangeCameraState((IsCloseState) ? Close_CS : Normal_CS);
		}

		// 透明化挡在镜头和角色之间的物体
		SetBlockCameraObjTransparent();

		if(IsFreeCameraMode)
			return;
	}

	public IEnumerator DelayBackToNormalCametaState()
	{
		yield return new WaitForSeconds(DelayTimeToNormal);

		//change camera to normal state
		IsChangingState = true;
	}

	void LateUpdate ()
	{
		if(target==null || IsPlayingCameraAnim || m_closeCamera)
			return;

		if(IsFreeCameraMode)
		{
			UpdateFreeCamera();
			return;
		}

		float _changingSpeed = Time.deltaTime;	// position changing speed and fov changing speed should be same. So, record movement speed, then use this speed for FOV changing.

		// 如果玩家在移动，则视角中心点跟随着玩家，而如果玩家没有移动，则会把中心点定位玩家面朝方向的一点距离，这样就算玩家原地转向，摄像机也会位移，看起来更有动感
		//when player is moving
		if (Player.Instance && Player.Instance.transform.GetComponent<PlayerMovement>().IsMoving)
		{
			if (Current_CS.IsNPCCamera)
			{
				if (target.GetComponent<ShopNpc>() && target.GetComponent<ShopNpc>().OverrideCamera && target.GetComponent<ShopNpc>().CameraPos)
				{
					finalPosition = target.GetComponent<ShopNpc>().CameraPos.position;
				}
				else
				{
					finalPosition = target.TransformPoint(Current_CS.OffsetToLookPoint);
				}
			}
			else
			{
				finalPosition = target.position + Current_CS.OffsetToLookPoint;

				CurFollowSpeed += Current_CS.Acceleration * Time.deltaTime;
				CurFollowSpeed = Mathf.Clamp(CurFollowSpeed, 0.0f, Current_CS.followSpeed);
			}

			_changingSpeed = CurFollowSpeed * Time.deltaTime;

			if (transform.position != finalPosition)
			{
				transform.position = Vector3.Lerp(transform.position, finalPosition, _changingSpeed);
			}
		}
		//when player is stop
		else
		{
			if (Current_CS.IsNPCCamera)
			{
				if (target.GetComponent<ShopNpc>() && target.GetComponent<ShopNpc>().OverrideCamera && target.GetComponent<ShopNpc>().CameraPos)
				{
					finalPosition = target.GetComponent<ShopNpc>().CameraPos.position;
				}
				else
				{
					finalPosition = target.TransformPoint(Current_CS.OffsetToLookPoint);
				}
			}
			else
			{
				if (Player.Instance && target == Player.Instance.transform && target.GetComponent<PlayerMovement>().PlayerObj)
					finalPosition = target.position + target.GetComponent<PlayerMovement>().PlayerObj.forward * Current_CS.TargetOffset + Current_CS.OffsetToLookPoint;
				else
					finalPosition = target.position + target.forward * Current_CS.TargetOffset + Current_CS.OffsetToLookPoint;
			}

			_changingSpeed = Current_CS.SnapSmoothLag * Time.deltaTime;

			if (transform.position != finalPosition)
			{
				transform.position = Vector3.Lerp(transform.position, finalPosition, _changingSpeed);
			}
		}

		// change FOV
		Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, Current_CS.FOV, _changingSpeed);

		// Rotation
		Vector3 _lookAngle = Current_CS.LookAngle;
		if (Current_CS.IsNPCCamera)
		{
			Vector3 lookTargetPos;
			if (target.GetComponent<ShopNpc>() && target.GetComponent<ShopNpc>().OverrideCamera && target.GetComponent<ShopNpc>().CameraLookPos)
			{
				lookTargetPos = target.GetComponent<ShopNpc>().CameraLookPos.position;
			}
			else
			{
				lookTargetPos = target.position + Current_CS.NearCameraLookTarget;
			}
			transform.LookAt(lookTargetPos);
			_lookAngle = transform.eulerAngles;
		}
		CurLookAngle = Vector3.Lerp(CurLookAngle, _lookAngle, _changingSpeed);
		transform.eulerAngles = CurLookAngle;

		//CurOffsetToLookPoint = Vector3.Lerp(CurOffsetToLookPoint, Current_CS.OffsetToLookPoint, _changingSpeed);

		// execute camera effect every tick
		if(transform.GetComponent<CameraEffectManager>())
			transform.GetComponent<CameraEffectManager>().Execute();
	}

	public void SetBlockCameraObjTransparent()
	{
		if (!target)
			return;

		// shift camera position back, handle cases when camera is inside collider
		Vector3 camera_position		= Vector3.MoveTowards(transform.position, target.position, -10.0f);
		Vector3 player_position		= target.position;
		Vector3 player_direction	= player_position - camera_position;

		// basic collision params
		float distance				= Vector3.Distance(camera_position, player_direction) - collcapRadius;
		int layer					= 1 << LayerMask.NameToLayer("TranslucentObject");

		// cast sphere toward player
		RaycastHit[] hits = Physics.SphereCastAll(camera_position, collcapRadius, player_direction, distance, layer);
		foreach (RaycastHit hit in hits) {
			float alpha1 = AlphaV1(camera_position, player_position, hit);
			float alpha2 = AlphaV2(camera_position, player_position, hit);
			
			float alpha;
			if (alpha1 < alpha2) { 
				alpha = alpha1;
			} else {
				alpha = alpha2;
			}
			
			if (alpha < 1.0f) CameraKakurenbo.hideGameObject(hit.transform, alpha, 0.5f);
		}
	}
	
	private float AlphaV1(Vector3 camera_position, Vector3 player_position, RaycastHit hit) {
		Vector3 to_player_projection	= Vector3.MoveTowards(camera_position, player_position, hit.distance);
		Vector3 closest_point			= hit.collider.ClosestPointOnBounds(to_player_projection);

		Debug.DrawLine(to_player_projection, closest_point, Color.green);

		float distance_to_projection = Vector3.Distance(to_player_projection, closest_point);
		float alpha = distance_to_projection / collcapRadius * 0.8f + 0.2f;
		
		return alpha;
	}
	
	
	private float AlphaV2(Vector3 camera_position, Vector3 player_position, RaycastHit hit) {
		Vector3 hit_object_position		= hit.transform.collider.transform.position;
		float hit_object_distance		= Vector3.Distance(camera_position, hit_object_position);

		// project point of collision on camera-player axis and closest point on collideer
		Vector3 to_player_projection	= Vector3.MoveTowards(camera_position, player_position, hit_object_distance);
		Vector3 closest_point			= hit.collider.ClosestPointOnBounds(to_player_projection);

		// some debug staff
		Debug.DrawLine(to_player_projection, closest_point, Color.blue);

		// calculate distance and alpha component
		float distance_to_projection = Vector3.Distance(to_player_projection, closest_point);
		float alpha = distance_to_projection / collcapRadius * 0.8f + 0.2f;
		return alpha;
	}
	
	public void UpdateFreeCamera()
	{
		float freeCamMoveSpeed = FreeCameraMovementSpeed;

		if(Input.GetKey(KeyCode.LeftShift) || Input.GetKey(KeyCode.RightShift))
		{
			freeCamMoveSpeed *= 6;
		}

		if(Input.GetKey(KeyCode.A))
		{
			transform.position +=  -1 * transform.right * freeCamMoveSpeed * Time.deltaTime;
		}
		else if(Input.GetKey(KeyCode.D))
		{
			transform.position +=  transform.right * freeCamMoveSpeed * Time.deltaTime;
		}
		else if(Input.GetKey(KeyCode.W))
		{
			transform.position +=  transform.forward * freeCamMoveSpeed * Time.deltaTime;
		}
		else if(Input.GetKey(KeyCode.S))
		{
			transform.position +=  -1 * transform.forward * freeCamMoveSpeed * Time.deltaTime;
		}
		else if(Input.GetKey(KeyCode.Q))
		{
			transform.position +=  -1 * transform.up * freeCamMoveSpeed * Time.deltaTime;
		}
		else if(Input.GetKey(KeyCode.E))
		{
			transform.position +=  transform.up * freeCamMoveSpeed * Time.deltaTime;
		}

		//mouse control
		if(Input.GetMouseButton(1))
		{
			angleH += Mathf.Clamp(Input.GetAxis("Mouse X"), -1, 1) * FreeCameraRotationSpeed * Time.deltaTime;
			angleV += Mathf.Clamp(Input.GetAxis("Mouse Y"), -1, 1) * FreeCameraRotationSpeed * Time.deltaTime;
			transform.rotation = Quaternion.Euler(-angleV, angleH, 0);
//			Screen.showCursor = false;
		}
		else{
//			Screen.showCursor = true;
		}
	}
	
	public void cinematic1 (float blend){
		blendingAnimationTimeFactor = blend ;
	}
	
	public void cinematic2 (float fovValue){
		if (fovValue <=0.0f){
			fovValue = 50.0f ;	
		}
		Current_CS.FOV = fovValue ;
		Camera.main.fieldOfView = Mathf.Lerp(Camera.main.fieldOfView, Current_CS.FOV, /*Time.deltaTime*blendingAnimationTimeFactor*0.1f*/0.05f*0.5f*0.005f*0.005f*0.005f);
	}
}
