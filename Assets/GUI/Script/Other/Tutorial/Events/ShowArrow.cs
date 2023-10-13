using UnityEngine;
using System.Collections;

public class ShowArrow : EventBase {

	// Use this for initialization
	void Start () {
		Regis();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	[SerializeField]
	private Transform arrowPos;
	[SerializeField]
	private string arrowKey;
	[SerializeField]
	private float arrowTime;
	[SerializeField]
	private Transform arrowParent;
	[SerializeField]
	private string layerStr;
	public override void Init() {
		TutorialMan.Instance.CreateArrow3D(arrowKey,arrowPos.position,arrowTime,arrowParent,layerStr);
		TutorialMan.Instance.AddBranchEndFlag();
	}
}
