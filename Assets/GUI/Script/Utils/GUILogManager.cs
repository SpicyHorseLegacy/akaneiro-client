using UnityEngine;
using System.Collections;

public enum LOG_LEVEL{
	INFO = 1,
	WARN,
	ERR,
	NOLOG,
	MAX
}

public class GUILogManager : MonoBehaviour {
	
	void Awake() {
	}
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public static LOG_LEVEL CurrentLogLevel = LOG_LEVEL.INFO;
	private static string time = "";
	
	public static void LogInfo(string info) {
		if(CurrentLogLevel > LOG_LEVEL.INFO) {
			return;
		}
		time = System.DateTime.Now.ToString();
		Debug.Log("["+time+"] "+info);
	}
	
	public static void LogWarn(string info) {
		if(CurrentLogLevel > LOG_LEVEL.WARN) {
			return;
		}
		time = System.DateTime.Now.ToString();
		Debug.LogWarning("["+time+"] "+info);
	}
	
	public static void LogErr(string info) {
		if(CurrentLogLevel > LOG_LEVEL.ERR) {
			return;
		}
		time = System.DateTime.Now.ToString();
		Debug.LogError("["+time+"] "+info);
	}
}
