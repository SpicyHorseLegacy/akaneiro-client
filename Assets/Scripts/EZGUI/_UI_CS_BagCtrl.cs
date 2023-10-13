using UnityEngine;
using System.Collections;

public class _UI_CS_BagCtrl : MonoBehaviour {
	
	//Instance
	public static _UI_CS_BagCtrl Instance = null;
	
	public GameObject ob;
	private Vector3 PrePosition;
	public Vector3  DestPosition;
	private bool isSave = false;
	public Vector3 DP;
	
	void Awake(){
		Instance = this;
	}
		
	// Use this for initialization
	void Start () {
		PrePosition = ob.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void ShowBag(Vector3 Position){
		DP = Position;
		StartCoroutine(_UI_CS_Ctrl.Instance.SkipOneFrameForBag());
	}
	
	public void HideBag(){
		ob.transform.position = new Vector3(1000f,1000f,1000f);
	}
	
	public void Hide(bool tf){
		if(tf){
			isSave = false;
			ob.transform.position = PrePosition;
		}else{
			SavePrePosition(ob.transform.position);
			ob.transform.position = DestPosition;
		}
	}
	
	public void SetBagPos(Vector3 pos){
		SavePrePosition(ob.transform.position);
		StartCoroutine(CallBackF(pos));
		
	}
	
	public void SavePrePosition(Vector3 pos){
		if(isSave){
			isSave = true;
			PrePosition = pos;
		}
	}
	
	private IEnumerator CallBackF(Vector3 pos){
		yield return new WaitForSeconds(0.1f);
		ob.transform.position = pos;
	}
}
