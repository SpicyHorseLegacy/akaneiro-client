using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class ItemShopManager : MonoBehaviour {

	public static ItemShopManager Instance;
	
	void Awake() {
		Instance = this;
		GUIManager.Instance.AddTemplateInitEnd();
	}
	
	// Use this for initialization
	void Start () {
		m_buyitemsucpanel.gameObject.SetActive(false);
	}
	
	#region Local
	private void _TabDelegate1() {
		_TabDelegate(1);
		this.gameObject.SendMessage("UpdateTab1");
	}
	private void _TabDelegate2() {
		_TabDelegate(2);
		this.gameObject.SendMessage("UpdateTab2");
	}
	private void _TabDelegate3() {
		_TabDelegate(3);
		this.gameObject.SendMessage("UpdateTab3");
	} 
	private void _TabDelegate4() {
		_TabDelegate(4);
		this.gameObject.SendMessage("UpdateTab4");
	}
	
	[SerializeField]
	private Transform rootLst;
	private Transform curRoot;

	#endregion`
	
	#region Interface
	public Transform GetCurRoot() {
		return curRoot;
	}
	public void CreateRoot() {
		GameObject root  =(GameObject)Instantiate(new GameObject("Root"));
		root.transform.parent = rootLst.transform;
        root.transform.localPosition = new Vector3(0,0,0); 
        root.transform.localScale= new Vector3(1,1,1);
		root.AddComponent<UIGrid>();
		root.GetComponent<UIGrid>().arrangement = UIGrid.Arrangement.Horizontal;
		root.GetComponent<UIGrid>().maxPerLine = 3;
		root.GetComponent<UIGrid>().cellWidth = 120;
		root.GetComponent<UIGrid>().cellHeight = 160;
		curRoot = root.transform;
	}
	
	public  List<ItemShopObject> list = new List<ItemShopObject>();
	
	public ItemShopObject itemObj;
	public ItemShopObject AddItemObj(ItemShopObjData data,bool isSpecial) {
		float _val = data.localData.info_eleVal+data.localData.info_gemVal+data.localData.info_encVal;
		//create root//
		GameObject obj  =(GameObject)Instantiate(itemObj.gameObject);
		obj.transform.parent = curRoot.transform;
        obj.transform.localPosition = new Vector3(0,0,0); 
        obj.transform.localScale= new Vector3(1,1,1);
		//update info//
		obj.GetComponent<ItemShopObject>().data = data;
		obj.GetComponent<ItemShopObject>().isSpecial = isSpecial;
		if(_val > (PlayerDataManager.Instance.levelNmb[1]-0.01f)) {
			obj.GetComponent<ItemShopObject>().SetPiceType(1);
		}else {
			if(isSpecial) {
				obj.GetComponent<ItemShopObject>().SetPiceType(1);
			}else {
				obj.GetComponent<ItemShopObject>().SetPiceType(2);
			}
		}
		obj.GetComponent<ItemShopObject>().SetCount(data.serData.leftBuyCount);
		obj.GetComponent<ItemShopObject>().SetPice(data.serData.price);
		ItemPrefabs.Instance.GetItemIcon(data.localData._ItemID,data.localData._TypeID,data.localData._PrefabID,obj.GetComponent<ItemShopObject>().GetIcon());
		obj.GetComponent<ItemShopObject>().SetBGColor(PlayerDataManager.Instance.GetNameColor(_val));
		obj.GetComponent<ItemShopObject>().HideHighLight(true);
		list.Add(obj.GetComponent<ItemShopObject>());
		return obj.GetComponent<ItemShopObject>();
	}
	
	public void HideAllHighLight() {
		foreach(ItemShopObject item in list) {
			item.HideHighLight(true);
		}
	}
	
	public delegate void Handle_TabDelegate(int idx);
    public event Handle_TabDelegate OnTabDelegate;
	private void _TabDelegate(int idx) {
//		GUILogManager.LogErr("_TabDelegate ids:"+idx);
		if(OnTabDelegate != null) {
			OnTabDelegate(idx);
		}
	}
	
	public delegate void Handle_ExitDelegate();
    public event Handle_ExitDelegate OnExitDelegate;
	private void _ExitDelegate() {
//		GUILogManager.LogErr("_ExitDelegate");
		if(OnExitDelegate != null) {
			OnExitDelegate();
		}
	}
	
	[SerializeField]
	private UICheckbox [] tabs;
	//[SerializeField]
	//private GameObject TabCover;
	public void UpdateTab(int idx) {
		tabs[idx-1].isChecked = true;
		//TabCover.transform.parent = tabs[idx - 1].transform;
		//TabCover.transform.position = tabs[idx - 1].transform.position;
		//TabCover.GetComponentInChildren<UISprite>().panel.Refresh();
	}

	[SerializeField] ItemShopSuccessPanel m_buyitemsucpanel;
	public void BuyItemSuc(SBuyitemInfo _info)
	{
		m_buyitemsucpanel.gameObject.SetActive(true);
		m_buyitemsucpanel.UpdateBuyItemSucInfo(_info);
	}
	
	#endregion
}
