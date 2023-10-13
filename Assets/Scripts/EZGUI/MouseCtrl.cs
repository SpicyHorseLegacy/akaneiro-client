using UnityEngine;
using System.Collections;

public enum MouseIconType{
	
	SWARD1 	= 0,
	SWARD2 	= 1,
	LOAD  	= 2,
	FINGER  = 3,
	SELL	= 4,
	PALM	= 5,
	STONE	= 6,
	MAX
	
}

public class MouseCtrl : MonoBehaviour {
	
	public static MouseCtrl Instance = null;
	public MouseIconType iconType = MouseIconType.SWARD1;
	public Texture2D [] icons;
	public UIButton mouseIcon;
	private Rect rect = new Rect(0,0,1,1);
	
	void Awake() {
		Instance = this;
	}
	
	// Use this for initialization
	void Start () {
		HideBaseMouse(false);
		SetMouseStats(MouseIconType.SWARD1);
		GetComponent<UIManager>().AddMouseTouchPtrListener(MouseDelegate);
	}
	
	// Update is called once per frame
	void Update () {
		mouseIcon.transform.position = UIManager.instance.uiCameras[0].camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y,UIManager.instance.uiCameras[0].camera.nearClipPlane + 1));
		mouseIcon.transform.position = new Vector3(mouseIcon.transform.position.x - 0.6f,mouseIcon.transform.position.y + 0.5f,-5.5f);
	}
	
	public void SetMouseStats(MouseIconType stats){
		if(stats == iconType){
			return;
		}
		mouseIcon.SetUVs(rect);
		mouseIcon.SetTexture(icons[(int)stats]);
		iconType = stats;
	}
	
	public void HideBaseMouse(bool isShow){
		Screen.showCursor = isShow;
	}
	
	private void UpdateMousePos(){
		
	}
	
	private void MouseDelegate(POINTER_INFO ptr){
		switch(ptr.evt){
		    case POINTER_INFO.INPUT_EVENT.PRESS:
				if(_UI_CS_ScreenCtrl.Instance.IsScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_BOUNTY_MASTER)) {
					 SetMouseStats(MouseIconType.STONE);
				}else{
					if(iconType != MouseIconType.SELL){
						 SetMouseStats(MouseIconType.SWARD2);
					}
				}
				break;
			case POINTER_INFO.INPUT_EVENT.TAP:
			case POINTER_INFO.INPUT_EVENT.RELEASE:
			case POINTER_INFO.INPUT_EVENT.RELEASE_OFF:
				if(_UI_CS_ScreenCtrl.Instance.IsScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_BOUNTY_MASTER)) {
					 SetMouseStats(MouseIconType.PALM);
				}else{
					if(iconType != MouseIconType.SELL){
					 	SetMouseStats(MouseIconType.SWARD1);
					}
				}
				break;
			}	
	}
	
}
