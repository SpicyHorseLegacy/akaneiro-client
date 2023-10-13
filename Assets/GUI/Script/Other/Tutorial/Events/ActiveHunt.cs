using UnityEngine;
using System.Collections;

public class ActiveHunt : EventBase {

	// Use this for initialization
	void Start () {
		Regis();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	[SerializeField]
	private int monsterID = 0;
	[SerializeField]
	private int HuntCount = 0;
	[SerializeField]
	private TutorialEventsType nextEvent;
	public override void Init() {
		TutorialMan.Instance.AddHuntTarget(monsterID,HuntCount,nextEvent);
		TutorialMan.Instance.AddBranchEndFlag();
	}
}
