using UnityEngine;
using System.Collections;

public class LoginLogoManager : MonoBehaviour {
	
	public static LoginLogoManager Instance;
	
	void Awake() {
		Instance = this;
		GUIManager.Instance.AddTemplateInitEnd();
	}
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	#region Interface
	public Transform logoObj;
	public void PlayLogoFadeIn() {
		if(logoObj == null) {
		GUILogManager.LogErr("logoObj is null");
			return;
		}
		TweenAlpha.Begin<TweenAlpha>(logoObj.gameObject,3f);
	}
	#endregion
	
	#region Local
	
	#endregion
}
