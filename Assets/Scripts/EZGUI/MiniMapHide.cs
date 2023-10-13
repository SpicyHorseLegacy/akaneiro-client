using UnityEngine;
using System.Collections;

public class MiniMapHide : MonoBehaviour {
	
	public Transform MiniMapobj;
	public UIButton  Playerobj;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void Hide(bool isShow){
		if(isShow){
			MiniMapobj.gameObject.layer = LayerMask.NameToLayer("Default");
			Playerobj.Hide(true);
		}else{
			MiniMapobj.gameObject.layer = LayerMask.NameToLayer("EZGUI_CanTouch");
			Playerobj.Hide(false);
		}
	}
}
