using UnityEngine;
using System.Collections;

public class State : MonoBehaviour
{
	[HideInInspector]
	public Transform Owner;
	
	public virtual void Enter(){}
	public virtual void Execute(){}
	public virtual void Exit(){}
}
