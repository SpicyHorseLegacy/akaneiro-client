using UnityEngine;
using System.Collections;

public class TooltipManager : MonoBehaviour {
	
	private GameObject curGameObj = null;
	
	public TooltipInfoBase[] Tooltips= new TooltipInfoBase[0];
	
	
	// Use this for initialization
	void Start () {
		Regis();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	#region Interface
	public void DistoryTipsEventDelegate(){
		UICamera.OnSelObjChanged -= TipsEventDelegate;
	}
	
	
	#endregion
	
	#region Local
	private void Regis() {
		UICamera.OnSelObjChanged += TipsEventDelegate;
	}
	
	private GameObject preTip=null;
	private void TipsEventDelegate(GameObject obj) {
		//hide all code.//
		HideAll();
		if(obj!=null){
			CheckTipObj(obj);
		}
		
	}
	
	private void CheckTipObj(GameObject obj){
		TooltipObj tipObj = obj.GetComponent<TooltipObj>();
		if(tipObj!=null){
			Debug.Log(Tooltips.Length);
			TooltipInfoBase tipInfo = Tooltips[(int)(tipObj.GetType())];
			Vector3 v = obj.transform.position;
			v.z = -0.01f;
			tipInfo.Show(v,tipObj.GetData());
			if(preTip!=null){
				HideAll();
			}
			preTip = obj;
		}
	}
	
	private void HideAll(){
		foreach(TooltipInfoBase obj in Tooltips){
			obj.Hide();
		}
		preTip=null;
	}
	#endregion
}
