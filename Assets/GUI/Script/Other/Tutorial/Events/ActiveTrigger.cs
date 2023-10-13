using UnityEngine;
using System.Collections;

public class ActiveTrigger : EventBase {

	// Use this for initialization
	void Start () {
		Regis();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	[SerializeField]
	private UIClientTrigger trigger;
	public override void Init() {
		trigger.bTouch = false;
	}
}
