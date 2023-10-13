using UnityEngine;
using System.Collections;

public class Materials : MonoBehaviour {
	public ItemDropStruct info;
	public int count;
	public UIButton icon;
	public SpriteText countText;
	
	// Use this for initialization
	void Start () {
		icon.AddInputDelegate(Delegate);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	private Vector3 mousePos;
	void Delegate(ref POINTER_INFO ptr) {
		switch(ptr.evt){
			case POINTER_INFO.INPUT_EVENT.MOVE:	
			case POINTER_INFO.INPUT_EVENT.NO_CHANGE:
				if(info != null) {
					mousePos = UIManager.instance.uiCameras[0].camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y,UIManager.instance.uiCameras[0].camera.nearClipPlane + 1));
					_ItemTips.Instance.UpdateToolsTipInfo(info,0,ItemRightClickType.BUY,ItemPosOffestType.RIGHT_TOP,mousePos,icon.width,icon.height);
				}
			break;
			case POINTER_INFO.INPUT_EVENT.RELEASE_OFF:
			case POINTER_INFO.INPUT_EVENT.MOVE_OFF:
				_ItemTips.Instance.DismissItemTip();
			break;
		}
	}
}
