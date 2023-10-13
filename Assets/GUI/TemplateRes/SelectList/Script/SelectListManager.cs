using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class SelectElementInfo{
	public int type;
	public long maxExp;
	public long curExp;
	public int level;
	public string name;
	public int sex;
	public int ID;
}

public class SelectListManager : MonoBehaviour {
	
	public static SelectListManager Instance;

	void Awake() {
		Instance = this;
		GUIManager.Instance.AddTemplateInitEnd();
	}
	
	#region Local
	[SerializeField]
	private NGUIPanel rootLst;
	private Transform curRoot;
	
	private void UpdateObjList() {
		//clearn list//
		if(curRoot != null) {
			Destroy(curRoot.gameObject);
		}
		//create root//
		CreateRoot();
		//element obj;
		for(int i=0;i<list.Count;i++){
			CreateElement(list[i]);
		}
		//create button
		CreateButton();
		//update
		if(curRoot != null) {
			curRoot.GetComponent<UIGrid>().Reposition();
//			GUILogManager.LogInfo("Update list.");
		}
	}

	// this root for copy//
	[SerializeField]
	private Transform Root;
	private void CreateRoot() {
		GameObject root  =(GameObject)Instantiate(Root.gameObject);
		root.transform.parent = rootLst.transform;
        root.transform.localPosition = new Vector3(0,0,0); 
        root.transform.localScale= new Vector3(1,1,1);
		root.AddComponent<UIGrid>();
		root.GetComponent<UIGrid>().arrangement = UIGrid.Arrangement.Vertical;
		root.GetComponent<UIGrid>().cellHeight = 100;
		curRoot = root.transform;
	}
	
	[SerializeField]
	private Transform listElement;
	public delegate void Handle_TypeIconChange(int type,int sex,SelectElement obj);
    public event Handle_TypeIconChange OnTypeIconChange;
	private void CreateElement(SelectElementInfo info) {
		//create root//
		GameObject obj  =(GameObject)Instantiate(listElement.gameObject);
		obj.transform.parent = curRoot.transform;
        obj.transform.localPosition = new Vector3(0,0,0); 
        obj.transform.localScale= new Vector3(1,1,1);
		//update info//
		obj.GetComponent<SelectElement>().SetName(info.name);
		obj.GetComponent<SelectElement>().SetLevel(info.level);
		obj.GetComponent<SelectElement>().data = info;
		if(OnTypeIconChange != null) {
			OnTypeIconChange(info.type,info.sex,obj.GetComponent<SelectElement>());
		}
		float fTemp = 0;
		if(info.maxExp != 0) {
			fTemp = (float)(info.curExp/info.maxExp);
		}
		obj.GetComponent<SelectElement>().SetLevelBarVal(fTemp);
	}
	private void SetEleName(string name) {
		
	}
	[SerializeField]
	private Object CreateBtn;
	private void CreateButton() {
		if(list.Count<elementMax){
			GameObject obj  =(GameObject)Instantiate(CreateBtn);
			obj.transform.parent = curRoot.transform;
	        obj.transform.localPosition = new Vector3(0,0,0); 
			obj.transform.localScale= new Vector3(1,1,1);
		}
	}
	#endregion
	
	#region Interface
	private List<SelectElementInfo> list = new List<SelectElementInfo>();
	public void CleanList() {
		list.Clear();
		UpdateObjList();
	}
	public void AddElement(SelectElementInfo ele) {
		list.Add(ele);
		UpdateObjList();
	}
	public void DelElement(SelectElementInfo ele) {
		list.Remove(ele);
		UpdateObjList();
	}
	
	private int elementMax = 3;
	public void SetElementMax(int max) {
		elementMax = max;
	}
	public int GetElementMax() {
		return elementMax;
	}
	
	public int CurSelectCharaIdx = 0;
	public void SetCurSelectChara(int idx) {
		CurSelectCharaIdx = idx;
	}
	public int GetCurSelectChara() {
		return CurSelectCharaIdx;
	}
	
	public delegate void Handle_SelectCharaChange(SelectElementInfo data);
    public event Handle_SelectCharaChange OnSelectCharaChange;
	//chara btn delegate.//
	public void ChangeSelectCharaDelegate(SelectElementInfo data) {
		if(OnSelectCharaChange != null) {
			OnSelectCharaChange(data);
		}
	}
	
	public delegate void Handle_CreateBtnDelegate();
    public event Handle_CreateBtnDelegate OnCreateBtnDelegate;
	//chara btn delegate.//
	public void CreateBtnDelegate() {
		if(OnCreateBtnDelegate != null) {
			OnCreateBtnDelegate();
		}
	}
	#endregion
	
}
