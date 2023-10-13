using UnityEngine;
using System.Collections;

public class ActiveCollect : EventBase {

	// Use this for initialization
	void Start () {
		Regis();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	[SerializeField]
	private int itemID = 0;
	[SerializeField]
	private int Count = 0;
	[SerializeField]
	private TutorialEventsType nextEvent;
	public override void Init() {
		TutorialMan.Instance.AddCollectTarget(itemID,Count,nextEvent);
		TutorialMan.Instance.AddBranchEndFlag();
	}
}
