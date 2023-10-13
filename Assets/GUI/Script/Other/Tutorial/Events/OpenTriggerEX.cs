using UnityEngine;
using System.Collections;

public class OpenTriggerEX : EventBase {

	// Use this for initialization
	void Start () {
		Regis();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public override void Init() {
		if(TriggerExManager.Instance) {
			TriggerExManager.Instance.OpenAllObj();
		}
	}
}
