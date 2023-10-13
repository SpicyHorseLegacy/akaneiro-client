using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class _UI_CS_MapLevelItem : MonoBehaviour {
	
	public string name;
	public int ID;
//	public bool isLoop;
//	public bool isOpen;
	public int ThreatLevel = 1;
	public int levelMin;
	public int levelMax;
//	public string Description;
//	public string Destination;
//	public string AcceptCue;
//	public string CompleteCue;
//	public string NewMissionCue;
	public string NewMissionID;
	public string PredecessorsMission;
	public int   MissionTime;
	
	//public _UI_CS_Task [] taskArray;
//	public _UI_CS_Branch [] branchArray;
	public  List<_UI_CS_Branch> 	    branchArray    = new List<_UI_CS_Branch>();
	
	public int xp;
	public int sk;
//	public int IsNew;
//	public int RightIconID;
//	public int LevelID;
	public string mapName;
	public int mapID = 0;
	public int costBadge = 0;
	
	public int threatUp 	= 0;
	public int threatDown 	= 0;
	
	//public bool isUse;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
