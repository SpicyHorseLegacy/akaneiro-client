using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class _UI_CS_IngameToolTipMan : MonoBehaviour {
	
	public Transform ToolTip;
	public  List<Transform> m_List = new List<Transform>();
	public bool isHide = false;
	public static _UI_CS_IngameToolTipMan Instance = null;
	
	void Awake(){
		
		Instance = this;
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
#region Interface
	public void AddTip(Transform vec3,string name,float val,ItemDropStruct iteminfo){
		if(ToolTip){
			Transform tip  = Instantiate(ToolTip, Vector3.zero, Quaternion.identity) as Transform;	
			tip.GetComponent<_UI_CS_IngameToolTipEz>().InitObj(name,vec3,isHide,val,iteminfo);
			m_List.Add(tip);
		}
	}
	
	public void Hide(){
		isHide = true;
		for(int i = 0;i<m_List.Count;i++){
			m_List[i].GetComponent<_UI_CS_IngameToolTipEz>().HideObj();	
		}	
	}
	
	public void Show(){
		isHide = false;
		for(int i = 0;i<m_List.Count;i++){
			m_List[i].GetComponent<_UI_CS_IngameToolTipEz>().ShowObj();	
		}	
	}
#endregion
	
}
