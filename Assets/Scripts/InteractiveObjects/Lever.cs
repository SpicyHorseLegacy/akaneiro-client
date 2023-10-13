using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Lever : BreakableActor {
	
	
	public Transform active_sound;
	
	
	Transform LeverSound;
	
	// Use this for initialization
	void Start () {
		
		if(active_sound != null && LeverSound == null)
		{
			LeverSound = Object.Instantiate(active_sound) as Transform;
		}
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public override void Active(int damage)
	{
		//Debug.Log("Lever open : TriggerAllEvent");
		
		if( IsUsed )
			return;
		
		IsUsed = true;
		
		//play active sound
		if(LeverSound)
		{
			LeverSound.position = transform.position;
			LeverSound.rotation = transform.rotation;
			SoundCue.Play(LeverSound.gameObject);
		}
		CS_Main.Instance.g_commModule.SendMessage(ProtocolBattle_SendRequest.ActiveTriggerReq(TriggerID));
	}
}
