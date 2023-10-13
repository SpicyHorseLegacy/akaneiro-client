using UnityEngine;
using System.Collections;

public class _UI_CS_ConsumableListCtrl : MonoBehaviour {
	public static _UI_CS_ConsumableListCtrl 			Instance;
	
	public _UI_CS_ConItemList [] consumableListArray;
	
	void Awake(){
		
		Instance = this;
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
