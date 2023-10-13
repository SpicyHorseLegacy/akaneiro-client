using UnityEngine;
using System.Collections;

public class ActiveObjArrow : EventBase {

	// Use this for initialization
	void Start () {
		Regis();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	[SerializeField]
	private int objID = 0;
	[SerializeField]
	private string key = "";
	[SerializeField]
	private TutorialEventsType nextType = TutorialEventsType.Empty;
	public override void Init() {
		TutorialMan.Instance.AddObjArrow(objID,key,nextType);
	}
}
