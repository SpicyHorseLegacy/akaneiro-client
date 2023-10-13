using UnityEngine;
using System.Collections;

public class mapDisableEnable : MonoBehaviour {
	
	public static mapDisableEnable Instance;
	private GameObject mapObject ;
	
	private bool mapZoom;
	
	void Awake (){
		mapZoom = false;
		Instance = this;
		mapObject = GameObject.Find("KGFMapSystem").gameObject;	
	}
	// Use this for initialization
	public void turnMapOff () {
		//KGFMapSystem.itsHideGUI = false ;
		if(mapObject) {
			mapObject.gameObject.SetActive(false);
		}
		
	}
	
	// Update is called once per frame
	public void turnMapOn () {
		if(mapObject) {
			mapObject.gameObject.SetActive(true);
		}
	}
	
	void Update (){
//		if (Input.GetKeyDown(KeyCode.H)){
//			turnMapOff();	
//		}else if (Input.GetKeyDown(KeyCode.J)){
//			turnMapOn();	
//		}
		if (Input.GetKeyDown(KeyCode.M) && mapZoom == false && PlayerPrefs.GetInt("isChat") == 0){
			mapZoom = true;
			mapObject.gameObject.SendMessage("SetFullscreen", true);	
		}else if (Input.GetKeyDown(KeyCode.M) && mapZoom == true && PlayerPrefs.GetInt("isChat") == 0){
			mapZoom = false;
			mapObject.gameObject.SendMessage("SetFullscreen", false);	
		}
	}
	
	void OnDestroy (){
		//Destroy(GameObject.Find("playerPoint").gameObject);
	}
}
