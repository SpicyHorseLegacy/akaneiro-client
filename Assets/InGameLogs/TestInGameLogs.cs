using UnityEngine;
using System.Collections;

//this script used for test purpose ,it do by default 1000 logs  + 1000 warnings + 1000 errors
//so you can check the functionality of in game logs
//just drop this scrip to any empty game object on first scene your game start at
public class TestInGameLogs : MonoBehaviour {
	
	public int logTestCount = 1000 ;
	int currentLogTestCount;
	InGameLog inGameLogs ;
	GUIStyle style ;
	// Use this for initialization
	void Start () {
		inGameLogs = FindObjectOfType( typeof(InGameLog)) as InGameLog ;
		Debug.Log("test long text sdf asdfg asdfg sdfgsdfg sdfg sfg sdfgsdfg sdfg sdf gsdfg sfdg sf gsdfg sdfg asdfg asdf asdf asdf asdf adsf ");
		
		style = new GUIStyle();
		style.alignment = TextAnchor.MiddleCenter ;
		style.normal.textColor = Color.white ;
		style.wordWrap = true ;
		
		for( int i = 0 ; i < 10 ; i ++ )
		{
			Debug.Log("Test Collapsed log");
			Debug.LogWarning("Test Collapsed Warning");
			Debug.LogError("Test Collapsed Error");
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		int drawn = 0;
		while ( currentLogTestCount < logTestCount && drawn < 10)
		{
			Debug.Log("Test Log " + currentLogTestCount );
			Debug.LogError("Test LogError " + currentLogTestCount );
			Debug.LogWarning("Test LogWarning " + currentLogTestCount );
			drawn++;
			currentLogTestCount++;
		}
		
		//test exception
		//GameObject o = null;
		//o.name = "opps this is null";
	}
	void OnGUI()
	{
		if( !inGameLogs.show )
		{
			GUI.Label (new Rect (Screen.width/2-120, Screen.height/2-25, 240, 50), "Draw circle on screen to show logs" , style);
			GUI.Label (new Rect (Screen.width/2-120, Screen.height/2+25, 240, 100), "To use InGameLogs just create empty game object in first scene your game start at ,then add InGameLogs component to this object" , style);
		}
	}
}
