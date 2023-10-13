using UnityEngine;
using System.Collections;

/// <summary>
/// AB ctrl.this script is collect infomation. some player can see A info.some player can see B info. 
/// we can konw which one more good.
/// </summary>
public class ABCtrl : MonoBehaviour {
	
	public static ABCtrl Instance = null;
	
	public bool isUseABCtrl = false;
	
	void Awake()
	{
		Instance = this;
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	/// <summary>
	/// Gets A or b. Return A or B mode.
	/// </summary>
	/// <returns>
	/// The A or b. A = 0,B = 1;
	/// </returns>
	public int GetAOrB(){
		if(isUseABCtrl){
			if(ClientLogicCtrl.Instance.isClientVer){
				//client
				if((int.Parse(ClientLogicCtrl.Instance.uid)%2) == 1){
					return 1;
				}else{
					return 0;
				}
			}else{
				//web
				if((int.Parse(ClientLogicCtrl.Instance.uid)%2) == 1){
					return 1;
				}else{
					return 0;
				}	
			}
		}else{
			return 0;
		}
	}
}
