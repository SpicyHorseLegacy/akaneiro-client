using UnityEngine;
using System.Collections;

[System.Serializable]
public class _UI_CS_RamusTask {

	public enum MISSION_TYPE {
		TRAVEL = 1,
		HUNT,
		COLLECT,
		PROTECT,
		SURVIVE,
		INTERACT,
		MEET,
		DESTORY,
		MAX
	}
	
	public MISSION_TYPE typeID;
	public int objectID;
	public int count;
	public int recycle;
	public string name;
	public string description;
	public int ramusID;
	
	[HideInInspector]
	public int CurrentVal = 0;
	[HideInInspector]
	public string CurrentValToString;
	
}
