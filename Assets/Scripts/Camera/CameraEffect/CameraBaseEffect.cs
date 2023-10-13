using UnityEngine;
using System.Collections;

public class CameraBaseEffect : MonoBehaviour {
	
	public CameraEffectManager EffectManager;
	public Vector3 temp;
	
	public virtual void Enter(){}
	public virtual void Execute(){}
	public virtual void Exit(){}
	
}
