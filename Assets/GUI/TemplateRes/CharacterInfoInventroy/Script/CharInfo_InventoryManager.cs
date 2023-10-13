using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class CharInfo_InventoryManager : MonoBehaviour {
	
	public static CharInfo_InventoryManager Instance;
	
	void Awake() {
		Instance = this;
	}
	
	// Use this for initialization
	void Start () {
		GUIManager.Instance.AddTemplateInitEnd();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	#region Interface
	private bool isOperating = false;
	public void SetOperating(bool isOper) {
		isOperating = isOper;
	}

	[SerializeField]
	public List<InventorySlot> list = new List<InventorySlot>();
	public InventorySlot GetBagItemData(int solt) {
		foreach(InventorySlot data in list) {
			if(data.GetSlot() == solt) {
				return data;
			}
		}
		return null;
	}
	
	public int InInventoySlot(Vector3 pos,int type,int slot) {
		foreach(InventorySlot invSlot in list) {
			if(type == 1&&slot == invSlot.GetSlot()&&invSlot.gameObject.activeSelf) {
				continue;
			}
			float left 	= invSlot.transform.position.x - 0.1f;
			float top	= invSlot.transform.position.y + 0.1f;
			float right	= invSlot.transform.position.x + 0.1f;
			float bootom= invSlot.transform.position.y - 0.1f;
			if(left <= pos.x && top >= pos.y && right >= pos.x && bootom <= pos.y){
				return invSlot.GetSlot();
			}
		}
		return -1;
	}
	
	public int GetEmptySlotIdx(int notThisSlot = -1) {
        foreach(InventorySlot invSlot in list) 
        {
            if (invSlot.GetEmptyFlag() && invSlot.GetSlot() < 21 && invSlot.GetSlot() != notThisSlot)
            {
                return invSlot.GetSlot();
            }
        }

		return -1;
	}
	public int GetSecondEmptySlotIdx() {
		bool isContinue = false;
		foreach(InventorySlot invSlot in list) {
			if(invSlot.GetEmptyFlag()) {
				if(isContinue) {
					return invSlot.GetSlot();
				}else {
					isContinue = true;
				}
			}
		}
		return -1;
	}
	
	[SerializeField]
	private UIScrollBar scrollBar;
	public void IncScrollBar(float val) {
		float tVal = scrollBar.scrollValue;
		 tVal += val;
		if(tVal < 0) {
			scrollBar.scrollValue = 0;
			return;
		}
		if(tVal > 1) {
			scrollBar.scrollValue = 1;
			return;
		}
		scrollBar.scrollValue = tVal;
	}
	#endregion

	public delegate void Handle_UpSpaceDelegate();
    public event Handle_UpSpaceDelegate OnUpSpaceDelegate;
	private void _UpSpaceDelegate() {
//		GUILogManager.LogErr("_UpSpaceDelegate");
		if(OnUpSpaceDelegate != null) {
			OnUpSpaceDelegate();
		}
	}
	public delegate void Handle_DownSpaceDelegate();
    public event Handle_DownSpaceDelegate OnDownSpaceDelegate;
	private void _DownSpaceDelegate() {
//		GUILogManager.LogErr("_DownSpaceDelegate");
		if(OnDownSpaceDelegate != null) {
			OnDownSpaceDelegate();
		}
	}
}
