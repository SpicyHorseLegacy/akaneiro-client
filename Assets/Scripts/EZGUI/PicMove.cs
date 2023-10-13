using UnityEngine;
using System.Collections;

public class PicMove : MonoBehaviour {
	
	private bool isOperate = false;
	private Vector2 offestPos = new Vector2(0,0); 
	
	public Transform lt;
	public Transform br;
	
	// Use this for initialization
	void Start () {
		transform.GetComponent<UIButton>().AddInputDelegate(BgDelegate);
		wideHalf = transform.GetComponent<UIButton>().width/2;
		heightHalf = transform.GetComponent<UIButton>().height/2;
	}
	
	// Update is called once per frame
	void Update () {
		if(isOperate){
			UpdatePosition();	
		}
	}
	
	void BgDelegate(ref POINTER_INFO ptr) {
		switch(ptr.evt){
		case POINTER_INFO.INPUT_EVENT.PRESS:
				UpdateOffest();
			break;
		case POINTER_INFO.INPUT_EVENT.DRAG:	
				isOperate = true;
			break;
		case POINTER_INFO.INPUT_EVENT.TAP:
		case POINTER_INFO.INPUT_EVENT.RELEASE:
		case POINTER_INFO.INPUT_EVENT.RELEASE_OFF:	
				isOperate = false;
			break;
		}	
	}
	
	private void UpdatePosition() {
		transform.position = _UI_CS_Ctrl.Instance.m_UI_Camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x   , Input.mousePosition.y ,_UI_CS_Ctrl.Instance.m_UI_Camera.nearClipPlane));
		transform.position = new Vector3(transform.position.x + offestPos.x,transform.position.y + offestPos.y,-3);
		CheckOffest(transform);
	}
	
	private void UpdateOffest() {
		Vector3 pos =  _UI_CS_Ctrl.Instance.m_UI_Camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x   , Input.mousePosition.y ,_UI_CS_Ctrl.Instance.m_UI_Camera.nearClipPlane));
		offestPos.x = transform.position.x - pos.x;
		offestPos.y = transform.position.y - pos.y;
	}
	
	private float wideHalf = 0;
	private float heightHalf = 0;
	private void CheckOffest(Transform obj) {
		float x = obj.localPosition.x;
		float y = obj.localPosition.y;
		float z = 0;
		if(obj.localPosition.x - wideHalf > lt.localPosition.x) {
			x = lt.localPosition.x + wideHalf;
		}
		if(obj.localPosition.x + wideHalf < br.localPosition.x) {
			x = br.localPosition.x - wideHalf;
		}
		if(obj.localPosition.y + heightHalf < lt.localPosition.y) {
			y = lt.localPosition.y - heightHalf;
		}
		if(obj.localPosition.y - heightHalf > br.localPosition.y) {
			y = br.localPosition.y + heightHalf;
		}
		obj.localPosition = new Vector3(x,y,z);
	}
	
}
