using UnityEngine;
using System.Collections;

public class _UICaneraCtrl : MonoBehaviour {
	
	public static _UICaneraCtrl Instance = null;
	
	public Camera uiCamera;
	public bool   isEnable = true;
	
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
	
	public void setUICameraEnable(bool isShow){
	
		isEnable = isShow;
		uiCamera.enabled = isShow;

	}
	
	void OnGUI() {       
     
		if (GUI.Button(new Rect(10, 10, 80, 30), " -isHideUI-")){       
			if(isEnable){
				setUICameraEnable(false);
			}else{
				setUICameraEnable(true);
			}
		}
		   
	}
}
