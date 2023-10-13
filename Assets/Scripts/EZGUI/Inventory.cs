using UnityEngine;
using System.Collections;

public class Inventory : MonoBehaviour {
	
	public static Inventory Instance = null;
	
	public UIPanel 			panel;
	public _UI_CS_BagCtrl 	BagPanel;
	public Vector3 		 	bagPosition;
	public _UI_CS_InventoryItem  []	bagItemArray;
	public _UI_CS_InventoryItem  []	equipmentArray;
	public UIButton  			 []	equipmentIconArray;
	public  int 		 	equipmentCount = 9;
	public  int 		 	preBagItmeIndex; 
	public  int 			preSlotItmeIndex;
	private int				tabIndex = 0;
	public UIPanelTab		bagTab1;
	public UIPanelTab	 	bagTab2;
	
	public UIPanel 			transmutePanel;
	public UIButton 		transmuteYesBtn;
	public UIButton 		transmuteNoBtn;
	public SpriteText		transmuteNameText;
	public int 				transmuteType = 0;
	public int 				transmuteSlot = 0;
	public Transform 		transmuteSound;
	
	void Awake() {
		Instance = this;
	}
	
	// Use this for initialization
	void Start () {
		bagTab1.AddInputDelegate(BagTab1Delegate);
		bagTab2.AddInputDelegate(BagTab2Delegate);
		transmuteYesBtn.AddInputDelegate(TransmutYesBtnDelegate);
		transmuteNoBtn.AddInputDelegate(TransmutNoBtnDelegate);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

#region Local 
	void BagTab1Delegate(ref POINTER_INFO ptr) {
		switch(ptr.evt) {
		   case POINTER_INFO.INPUT_EVENT.TAP:
				tabIndex = 0;
				bagItemArray[0].CancelPress();
				break;
		}	
	}
	
	void BagTab2Delegate(ref POINTER_INFO ptr) {
		switch(ptr.evt) {
		   case POINTER_INFO.INPUT_EVENT.TAP:
				tabIndex = 1;
				bagItemArray[0].CancelPress();
				break;
		}	
	}
	
	void TransmutYesBtnDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:
				SendTransmuteMsg();
				transmutePanel.Dismiss();
				_ItemTips.Instance.DismissItemTip();
			break;
		}	
	}
	
	void TransmutNoBtnDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:
				transmutePanel.Dismiss();
				_UI_CS_IngameMenu.Instance.isLockInvLogic = false;
				bagItemArray[Inventory.Instance.preSlotItmeIndex].UpdateGroupElementPosition();
				bagItemArray[Inventory.Instance.preSlotItmeIndex].transform.position = bagItemArray[Inventory.Instance.preSlotItmeIndex].m_StartPosition;
				_ItemTips.Instance.DismissItemTip();
			break;
		}	
	}
	
	
#endregion
	
#region Interface
	public void SendTransmuteMsg(){
		LogManager.Log_Debug("--- DelItem ---");
		CS_Main.Instance.g_commModule.SendMessage(
				 ProtocolGame_SendRequest.DelItem((byte)transmuteType,(uint)transmuteSlot)
		);
		//why play sound at here.because if use item is last one. will play 2 time sound.(food sound and transmute sound)//
		SoundCue.PlayPrefabAndDestroy(Inventory.Instance.transmuteSound);
	}
#endregion
}
