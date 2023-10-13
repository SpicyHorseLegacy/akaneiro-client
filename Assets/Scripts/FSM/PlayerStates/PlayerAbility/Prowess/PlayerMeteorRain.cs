using UnityEngine;
using System.Collections;

public class PlayerMeteorRain : PlayerAbilityBaseState {
	
	public Transform CastSoundPrefab;
	public Transform MeteorZonePrefab;
	public Transform MeteorRainPrefab;
	public Transform VFX_MeteorChargingPrefab;
    public Transform MeteorRainSoundPrefab;
	
	[HideInInspector] public Transform meteorZone;
	[HideInInspector] public Vector3 meteorPoint;
	[HideInInspector] public Transform vfx_charging;
	
	public override void Enter()
	{
		base.Enter();

		step = PrepareStep.WaitForMouseDown;
		
		// Aka stop movement
		Owner.GetComponent<PlayerMovement>().StopMove(false);
		
		// hide weapons
		Player.Instance.SetPlayerWeaponVisible(false);
	}
	
	public override void Execute()
	{
		base.Execute();
		
		if(step == PrepareStep.WaitForMouseDown || step == PrepareStep.WaitForMouseUp)
		{
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			int layer = 1<<LayerMask.NameToLayer("Walkable");
			if(Physics.Raycast(ray.origin,ray.direction,out hit,100f,layer))
			{
				// set position of meteorzone
				if(meteorZone)
				{
					meteorZone.position = hit.point;
					meteorZone.position = new Vector3(meteorZone.position.x, Owner.position.y, meteorZone.position.z);
                    if (Vector3.Distance(meteorZone.position, Owner.position) > Info.EndDistance)
					{
						Vector3 fwd = Owner.GetComponent<PlayerMovement>().PlayerObj.TransformDirection(Vector3.forward);
                        meteorZone.position = Owner.position + fwd * Info.EndDistance;
					}
					meteorZone.position += Vector3.up * 0.3f;
				}
				
				// player look at the mouse position
				Owner.GetComponent<PlayerMovement>().LookAtPosition(hit.point);
				
				if(step == PrepareStep.WaitForMouseDown)
				{
					if(Input.GetMouseButtonDown(0))
					{
						AcitveAbilityWithMousePos(Input.mousePosition);
					}
				}
			}
		}
	}
	
	public override void Exit()
	{
		base.Exit();
		
		if(vfx_charging)
			vfx_charging.GetComponent<DestructAfterTime>().DestructNow();
		
		if(meteorZone)
			UnityEngine.Object.Destroy(meteorZone.gameObject);
		
		// show weapons
		Player.Instance.SetPlayerWeaponVisible(true);
		
		// switch pathfinding enabled
		Owner.GetComponent<PlayerMovement>().bStopMove = false;
		
		// player can active ability
		Player.Instance.CanActiveAbility = true;
	}
	
	public override void PrepareForAbilityWithoutKeyboardInput ()
	{
		base.PrepareForAbilityWithoutKeyboardInput ();
		
		// if enter this state by clicking GUI, that means player needs to choose the position, so after prepare animation, avatar should play idle animation for waiting for player
		AnimationModel.animation[playerController.abilityManager.AbiAniManager.MeteorPrepare.name].wrapMode = WrapMode.Loop;
		AnimationModel.animation.CrossFade(playerController.abilityManager.AbiAniManager.MeteorPrepare.name);
		
		// create a MeteorZone for player choosing position
		Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
		RaycastHit hit;
		int layer = 1<<LayerMask.NameToLayer("Walkable");
		if(Physics.Raycast(ray.origin,ray.direction,out hit,100f,layer))
		{
			meteorZone = CS_Main.Instance.SpawnObject(MeteorZonePrefab);
			meteorZone.position = hit.point + Vector3.up * 0.3f;
            if (meteorZone.GetComponent<MeteorRangeController>())
            {
                int _objID = AbilityInfo.Instance.AbilityInfomation.GetAbilityDetailInfoByID(id).ObjectID[0];
                float _area = AbilityInfo.Instance.AbilityObjectInfomation.GetObjectInfoByID(_objID).Param;
                meteorZone.GetComponent<MeteorRangeController>().ResizeCircle(_area);
            }
		}
		
		if(!vfx_charging && VFX_MeteorChargingPrefab)
		{
			vfx_charging = CS_Main.Instance.SpawnObject(VFX_MeteorChargingPrefab, Owner.position + Vector3.up, Quaternion.identity);

			Component[] all = AnimationModel.GetComponentsInChildren<Component>();
			foreach(Component T in all)
			{
				if(T.name == "Bip001 Prop1")
				{
					vfx_charging.position = T.transform.position;
					vfx_charging.parent = T.transform;
				}
			}
		}
		
		step = PrepareStep.WaitForMouseDown;
	}
	
	public override void AcitveAbilityWithMousePos (Vector3 mousePos)
	{
		base.AcitveAbilityWithMousePos (mousePos);
		
		Ray ray = Camera.main.ScreenPointToRay(mousePos);
		RaycastHit hit;
		int layer = 1<<LayerMask.NameToLayer("Walkable");
		if(Physics.Raycast(ray.origin,ray.direction,out hit,100f,layer))
		{
			// get position and send to server
			Owner.GetComponent<PlayerMovement>().LookAtPosition(hit.point);
			meteorPoint = hit.point + Vector3.up * 0.2f;

            if (Vector3.Distance(meteorPoint, Owner.position) > Info.EndDistance)
			{
				Vector3 fwd = Owner.GetComponent<PlayerMovement>().PlayerObj.TransformDirection(Vector3.forward);
                meteorPoint = Owner.position + fwd * Info.EndDistance + Vector3.up * 0.2f;
			}

			// destroy meteor zone
			if(meteorZone)
			{
				DestructAfterTime dt = meteorZone.gameObject.AddComponent<DestructAfterTime>() as DestructAfterTime;
				dt.DestructNow();
				meteorZone = null;
			}
			
			// clean vfx in player's hand
			if(vfx_charging)
				vfx_charging.GetComponent<DestructAfterTime>().DestructNow();
			
			// player can't active any other abilities
			Player.Instance.CanActiveAbility = false;
			
			// play animation
			// if the backtoidle evnent is not at the end of animation and player keep using the same ability, the next animation request would come before the first animation finished.
			// if don't restore the animation, the 2nd animation won't play, but just crossfade the idle animation, so player enters ability state and keep playing the 1st animation which has actived the event
			// so player couldn't back to normal state again.
			AnimationModel.animation[playerController.abilityManager.AbiAniManager.MeteorActive.name].time = 0;
			AnimationModel.animation[playerController.abilityManager.AbiAniManager.MeteorActive.name].wrapMode = WrapMode.ClampForever;
			AnimationModel.animation.CrossFade(playerController.abilityManager.AbiAniManager.MeteorActive.name);

            // play cast sound
            SoundCue.PlayPrefabAndDestroy(CastSoundPrefab, meteorPoint);

            // Send message to server
            SendUseAbilityRequest((uint)id, 0, meteorPoint);
			step = PrepareStep.WaitForServerCallback;
		}
	}
	
	public override AbilityObject On_SkillObjectEnter (SSkillObjectEnter skillObjectInfo)
	{
        if (MeteorRainPrefab)
        {
            Transform meteorRain = CS_Main.Instance.SpawnObject(MeteorRainPrefab, skillObjectInfo.pos + Vector3.one * 0.2f, Quaternion.identity);
            MeteorRainObj mrobj = meteorRain.GetComponent<MeteorRainObj>();
            mrobj.ObjID = skillObjectInfo.objectID;
            mrobj.DestAbility = this;
            mrobj.SkillObjectInfo = skillObjectInfo;
            mrobj.ActiveSoundPrefab = MeteorRainSoundPrefab;	// because in higher level, meteor rain could last longer, so play the longer sfx.
            return mrobj;
        }
        return null;
	}
}
