using UnityEngine;
using System.Collections;

public class GlobalGameState : MonoBehaviour {

	// Use this for initialization
	
	public static string state = "s1";
	public static bool bInCave = false;
	//[HideInInspector]
	//public string preState = "s1";
	//[HideInInspector]
	//public bool bCanEdit = false;
	[HideInInspector]
	public static string ChangeMapState = "";
	
	void Awake(){
		
		//state = preState;
		
		if(ChangeMapState != "")
		{
			state = ChangeMapState;
			ChangeMapState="";
		}
		
		DontDestroyOnLoad(gameObject);
	}
	
	void Start () {
		
		//if(bCanEdit)
			//state = preState;
	
	}
	
	// Update is called once per frame
	void Update () {
	
		if(Input.GetKeyDown(KeyCode.F1))
			state = "s1";
		if(Input.GetKeyDown(KeyCode.F2))
			state = "s2";
		if(Input.GetKeyDown(KeyCode.F3))
			state = "s3";
		if(Input.GetKeyDown(KeyCode.F4))
			state = "s4";
	}
}
