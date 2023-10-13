using UnityEngine;
using System.Collections;

public class MenuBtnCtrl : EventBase {

	// Use this for initialization
	void Start () {
		Regis();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	[SerializeField]
	private int menuBtnState = -1;
	public override void Init() {
		_UI_CS_FightScreen.Instance.SetMainMenuBtnState(menuBtnState);
	}
}
