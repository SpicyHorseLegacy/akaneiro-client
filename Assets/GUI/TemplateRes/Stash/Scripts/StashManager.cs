using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class StashManager : MonoBehaviour {
	
	public static StashManager Instance;
	
	public Transform UI_SFX_OpenSound;
	public Transform UI_SFX_BuySuccess;
	public Transform UI_SFX_BuyFail;
	
	void Awake() {
		Instance = this;
		SoundCue.PlayPrefabAndDestroy(UI_SFX_OpenSound);
	}
	
	// Use this for initialization
	void Start () {
		HidePopUpPanel(true);
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Escape))
		{
			_ExitDelegate();
		}
	}
	
	[SerializeField]
	public List<InventorySlot> list = new List<InventorySlot>();
	public InventorySlot GetStashItemData(int solt) {
		foreach(InventorySlot data in list) {
			if(data.GetSlot() == solt) {
				return data;
			}
		}
		return null;
	}
	
	public int InStashSlot(Vector3 pos,int type,int slot) {
		foreach(InventorySlot invSlot in list) {
			if(type == 4&&slot == invSlot.GetSlot()&&invSlot.gameObject.activeSelf) {
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

    [SerializeField] UI_Stash_TabsManager TabManager;
    [SerializeField] GameObject BuyNewStashBTN;
	public void UpdateTab(int maxTab,int curTabIdx) {
        UI_TypeDefine.UI_Stash_Tab_data _data = new UI_TypeDefine.UI_Stash_Tab_data();
        _data.CurIDX = curTabIdx-1;
        _data.MaxIDX = 5;
        _data.BoughtIDX = maxTab;
        TabManager.UpdateTabsInfo(_data);

        BuyNewStashBTN.gameObject.SetActive(_data.BoughtIDX < _data.MaxIDX);
	}
	public delegate void Handle_ExitDelegate();
    public event Handle_ExitDelegate OnExitDelegate;
	private void _ExitDelegate() {
//		GUILogManager.LogErr("_ExitDelegate");
		if(OnExitDelegate != null) {
			OnExitDelegate();
		}
	}

	public delegate void Handle_StashTabDelegate(int idx);
    public event Handle_StashTabDelegate OnStashTabDelegate;
	public void StashTabDelegate(int idx) {
//		GUILogManager.LogErr("_StashTabDelegate ids:"+idx);
		if(OnStashTabDelegate != null) {
			OnStashTabDelegate(idx + 1);
		}
	}
	
	public delegate void Handle_CreateTabDelegate();
    public event Handle_CreateTabDelegate OnCreateTabDelegate;
	public void _CreateTabDelegate() {
//		GUILogManager.LogErr("_CreateTabDelegate");
		if(OnCreateTabDelegate != null) {
			OnCreateTabDelegate();
		}
	}
	
	private void _PopUpDelegate() {
		HidePopUpPanel(false);
	}
	
	public void _ClosePopUpDelegate() {
		HidePopUpPanel(true);

        StashManager.Instance.UpdateTab(PlayerDataManager.Instance.GetMaxStashTab(), PlayerDataManager.Instance.GetCurStashIdx());
        StashManager.Instance.SetUnlockTabPice(PlayerDataManager.Instance.GetUnlockStashPice()); 
	}

	[SerializeField]
	private UILabel unlockTabPice;
	public void SetUnlockTabPice(int pice) {
		unlockPice = pice;
	}
	
	[SerializeField]
	private UI_Stash_BuyNewStashPanel PopupPanel;
	private int unlockPice = 0;
	public void HidePopUpPanel(bool hide) {
		NGUITools.SetActive(PopupPanel.gameObject,!hide);
		if(!hide) {
            PopupPanel.Init(unlockPice);
		}
	}

    public void BuyStashSuccess()
    {
        PopupPanel.BuyStashSuccess();
		SoundCue.PlayPrefabAndDestroy(UI_SFX_BuySuccess);
    }
    public void BuyStashFailed()
    {
        PopupPanel.BuyStashFailed();
		SoundCue.PlayPrefabAndDestroy(UI_SFX_BuyFail);
    }
}
