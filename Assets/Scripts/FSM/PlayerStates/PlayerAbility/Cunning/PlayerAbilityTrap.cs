using UnityEngine;
using System.Collections;

public class PlayerAbilityTrap : PlayerAbilityBaseState {
	
	public Transform InstallSoundPrefab;
	public Transform TrapPrefab;
	public Transform AbilityRangePrefab;

	Transform vfx;
	Transform abilityRange;
	
	Vector3 trapPos = Vector3.zero;
	
	public override void Enter()
	{
		base.Enter();
		
		step = PrepareStep.WaitForMouseDown;

		Owner.GetComponent<PlayerMovement>().StopMove(false);

		//hide current weapon
		Player.Instance.SetPlayerWeaponVisible(false);
	}
	
	public override void Execute()
	{
		if(step == PrepareStep.WaitForMouseDown || step == PrepareStep.WaitForMouseUp)
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			int layer = 1<<LayerMask.NameToLayer("Walkable");
			if(Physics.Raycast(ray.origin,ray.direction,out hit,100f,layer))
			{
				if(abilityRange)
				{
					abilityRange.position = hit.point;
					abilityRange.position = new Vector3(abilityRange.position.x, Owner.position.y, abilityRange.position.z);
                    if (Vector3.Distance(abilityRange.position, Owner.position) > Info.EndDistance)
					{
						Vector3 fwd = Owner.GetComponent<PlayerMovement>().PlayerObj.TransformDirection(Vector3.forward);
                        abilityRange.position = Owner.position + fwd * Info.EndDistance;
					}					
					abilityRange.position += Vector3.up * 0.3f;
				}
			}
			
			Player.Instance.GetComponent<PlayerMovement>().LookAtMousePoint(true);
#if NGUI
			if(Input.GetMouseButtonDown(0) && step == PrepareStep.WaitForMouseDown)
			{
				step = PrepareStep.WaitForMouseUp;
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
		
		if(abilityRange)
		{
			abilityRange.gameObject.AddComponent<DestructAfterTime>();
			abilityRange.GetComponent<DestructAfterTime>().DestructNow();
		}
		
		//show current weapon
		Player.Instance.SetPlayerWeaponVisible(true);
		Owner.GetComponent<PlayerMovement>().bStopMove = false;
		Player.Instance.CanActiveAbility = true;
	}
	
	public override void PrepareForAbilityWithoutKeyboardInput ()
	{
		base.PrepareForAbilityWithoutKeyboardInput ();
		
		Player.Instance.PlayIdleAnim(true);
		
		// create a MeteorZone for player choosing position
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		int layer = 1<<LayerMask.NameToLayer("Walkable");
		if(Physics.Raycast(ray.origin,ray.direction,out hit,100f,layer))
		{
			abilityRange = CS_Main.Instance.SpawnObject(AbilityRangePrefab, hit.point + Vector3.up * 0.2f, Quaternion.identity);
			abilityRange.GetComponent<AbilityRangeController>().StartShow();
		}
		step = PrepareStep.WaitForMouseDown;
	}
	
	public override void AcitveAbilityWithMousePos (Vector3 mousePos)
	{
		base.AcitveAbilityWithMousePos (mousePos);
		Owner.GetComponent<PlayerMovement>().LookAtMousePoint(mousePos, true);
		
		Ray ray = Camera.main.ScreenPointToRay(mousePos);
		RaycastHit hit;
		int layer = 1<<LayerMask.NameToLayer("Walkable");
		if(Physics.Raycast(ray.origin,ray.direction,out hit,100f,layer) && Time.timeScale > 0){
			
			trapPos = hit.point + Vector3.up * 0.2f;

            if (Vector3.Distance(trapPos, Owner.position) > Info.EndDistance)
			{
				Vector3 fwd = Owner.GetComponent<PlayerMovement>().PlayerObj.TransformDirection(Vector3.forward);
                trapPos = Owner.position + fwd * Info.EndDistance + Vector3.up * 0.2f;
		
				RaycastHit hitInfo;
				if(Physics.Raycast(trapPos + Vector3.up*5f,Vector3.down,out hitInfo,10f,1 << LayerMask.NameToLayer("Walkable")))
				{
					trapPos = hitInfo.point;
				}
			}
			// Send message to server
			SendUseAbilityRequest((uint)id, 0, trapPos);
			
			// 玩家不能使用技能
			Player.Instance.CanActiveAbility = false;
			
			step = PrepareStep.WaitForServerCallback;

			// 清楚标志范围的圈
			if(abilityRange)
			{
				abilityRange.gameObject.AddComponent<DestructAfterTime>();
				abilityRange.GetComponent<DestructAfterTime>().DestructNow();
			}
		}
	}
	
	public override AbilityObject On_SkillObjectEnter (SSkillObjectEnter skillObjectInfo)
	{
		if(TrapPrefab)
		{
			Transform trap = Instantiate(TrapPrefab, skillObjectInfo.pos + Vector3.up * 0.2f, TrapPrefab.rotation) as Transform;
			trap.GetComponent<AbilityObject>().ObjID = skillObjectInfo.objectID;
			trap.GetComponent<AbilityObject>().TypeID = skillObjectInfo.objectTypeID;
            trap.GetComponent<AbilityObject>().DestAbility = this;
            trap.GetComponent<AbilityObject>().SkillObjectInfo = skillObjectInfo;
			trap.GetComponent<AbilityObject>().Init();

            return trap.GetComponent<AbilityObject>();
		}
        return null;
	}

    public virtual void PlaySoundAndVFX()
    {
        SoundCue.PlayPrefabAndDestroy(InstallSoundPrefab, Owner.position);
    }
}
