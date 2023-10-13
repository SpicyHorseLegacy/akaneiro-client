using UnityEngine;
using System.Collections;

public class OverTrigger : MonoBehaviour {
	
	private BoxCollider box;
	public Transform lt;
	public Transform br;
	
	// Use this for initialization
	void Start () {
		Hide(true);
	}
	
	// Update is called once per frame
	void Update () {
		UpdateDragOver();
	}
	
	public GameObject target;
	public string functionName;
	private void UpdateDragOver() {
		Vector3 mousePos = UICamera.currentCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y,-1));
		float l = lt.position.x;float r = br.position.x;
		float t = lt.position.y;float b = br.position.y;
//		GUILogManager.LogInfo("l:"+l+"|r:"+r+"|t:"+t+"|b:"+b);
//		GUILogManager.LogErr("x:"+mousePos.x+"|y:"+mousePos.y);
		if(mousePos.x > l && mousePos.x < r && mousePos.y > b && mousePos.y < t) {
			Send();
			if(!isShowArrow) {
				isShowArrow = true;
				Hide(false);
			}
		}else {
			if(isShowArrow) {
				isShowArrow = false;
				Hide(true);
			}
		}
	}
	
	private void Send() {
		if (string.IsNullOrEmpty(functionName)) return;
		if (target == null) target = gameObject;
		target.SendMessage(functionName, gameObject, SendMessageOptions.DontRequireReceiver);
	}
	
	private bool isShowArrow = false;
	[SerializeField]
	private UISprite arrow;
	private void Hide(bool hide) {
		NGUITools.SetActive(arrow.gameObject,!hide);
	}
}
