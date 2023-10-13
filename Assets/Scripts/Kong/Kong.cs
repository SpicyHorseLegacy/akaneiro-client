using UnityEngine;
using System.Collections;

public class Kong : MonoBehaviour {
	
	//Instance
	public static Kong Instance  = null;
	
	void Awake(){
		Instance = this;
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void KongRate(){
#if NGUI
		if(VersionManager.Instance.GetVersionType() == VersionType.InternalWebVersion) {
			if(VersionManager.Instance.GetPlatformType().Get() == EUserType.eUserType_Kongregate){
				GUILogManager.LogInfo("kongregate publishStats.");
				Application.ExternalCall("publishStats","initialized","1");
			}	
		}
#else
		if(WebLoginCtrl.Instance.IsWebLogin){
			if(Platform.Instance.platformType.Get() == EUserType.eUserType_Kongregate){
				LogManager.Log_Info("Kong publishStats initialized.");
				Application.ExternalCall("publishStats","initialized","1");
			}
		}
#endif
	}
}
