using UnityEngine;
using System.Collections;

public class _UI_TranBtn : MonoBehaviour {
	
	public UIButton tranBtn;
	private Vector3 	mousePos;
	// Use this for initialization
	void Start () {
		tranBtn.AddInputDelegate(Delegate);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void Delegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		  case POINTER_INFO.INPUT_EVENT.PRESS:
			if(_UI_CS_IngameMenu.Instance.isTransmute){
				_UI_CS_IngameMenu.Instance.SetTransmuteState(false);
//				MouseCtrl.Instance.iconType = MouseIconType.SWARD1;
				MouseCtrl.Instance.SetMouseStats(MouseIconType.SWARD1);
			}else{
				_UI_CS_IngameMenu.Instance.SetTransmuteState(true);
				MouseCtrl.Instance.SetMouseStats(MouseIconType.SELL);
			}
			if(_UI_CS_IngameMenu.Instance.IsMustPTap){			
				tranBtn.GetComponent<_UI_CS_InventoryItem>().ItemBtnTapPress();
			}
			break;
			case POINTER_INFO.INPUT_EVENT.MOVE:	
		    case POINTER_INFO.INPUT_EVENT.NO_CHANGE:
				{
					mousePos = UIManager.instance.uiCameras[0].camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y,UIManager.instance.uiCameras[0].camera.nearClipPlane + 1));
					TransmuteTips.Instance.ShowTransmuteTips(mousePos,tranBtn.width,tranBtn.height);
				}
				break;
		   case POINTER_INFO.INPUT_EVENT.MOVE_OFF:
		   case POINTER_INFO.INPUT_EVENT.RELEASE:
		   case POINTER_INFO.INPUT_EVENT.RELEASE_OFF:
				{
					TransmuteTips.Instance.DismissTip();
				}
				break;
		}
		
	}
}
