using UnityEngine;
using System.Collections;

public class InputKeyForDialog : EventBase {
	
	
	// Use this for initialization
	void Start () {
		Regis();
	}
	
	// Update is called once per frame
	void Update () {
		if(isActive) {
			if(Input.GetKeyDown(keyC)) {
				isActive = false;
				TutorialDialogEZ.Instance.HideContinue(false);
			}
		}
	}
	
	[SerializeField]
	private KeyCode keyC;
	private bool isActive = false;
	public override void Init() {
		isActive = true;
		TutorialDialogEZ.Instance.HideContinue(true);
	}
}
