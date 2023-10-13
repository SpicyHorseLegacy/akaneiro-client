using UnityEngine;
using System.Collections;

public class InputKeyForArrow : EventBase {
	
	
	// Use this for initialization
	void Start () {
		Regis();
	}
	
	// Update is called once per frame
	void Update () {
		if(isActive) {
			if(Input.GetKeyDown(keyC)) {
				isActive = false;
				if(isShowArrow) {
					TutorialMan.Instance.CreateArrow3D(arrowKey,arrowPos.position,arrowTime,arrowParent,layerStr);
				}else {
					TutorialMan.Instance.DelArrow3D(arrowKey);
				}
			}
		}
	}
	
	[SerializeField]
	private KeyCode keyC;
	private bool isActive = false;
	[SerializeField]
	private Transform arrowPos;
	[SerializeField]
	private string arrowKey;
	[SerializeField]
	private float arrowTime;
	[SerializeField]
	private Transform arrowParent;
	[SerializeField]
	private bool isShowArrow = true;
	[SerializeField]
	private string layerStr;
	public override void Init() {
		isActive = true;
	}
}
