using UnityEngine;
using System.Collections;

public enum CLIENT_TYPE{
	eClientType_WebPlayer		= 0,		//WebPlayer
	eClientType_Android			= 1,		//Android
	eClientType_IOS_Phone		= 2,		//IOS_Phone
	eClientType_IOS_Pad			= 3,		//IOS_Pad
	eClientType_Standalone_OSX	= 4,		//Standalone_OSX
	eClientType_Standalone_Win	= 5,		//Standalone_Win
	eClientType_Standalone_Lin	= 6,		//Standalone_Lin
}

public class Platform : MonoBehaviour {
	
	public static Platform Instance = null;
	public EClientType clientType = new EClientType();
	public CLIENT_TYPE eClientType = CLIENT_TYPE.eClientType_WebPlayer;
	public EUserType platformType = new EUserType();
	public int defaultplatformType = 2;
	public Texture2D [] platformIcon;

	void Awake(){
		Instance = this;
	}
	
	// Use this for initialization
	void Start () {
		platformType.Set(defaultplatformType);
		clientType.Set((int)eClientType);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
