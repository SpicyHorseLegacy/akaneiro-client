using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;


[System.Serializable]
public class _UI_CS_Task  {
	public  List<_UI_CS_RamusTask> 	    SubObject    = new List<_UI_CS_RamusTask>();
	public int    TaskTime = 0;
	public string triggerId = "0";
	public string triggerType = "1";
	public int rewardContentKarma = 0;
	public int rewardContentExp = 0;
	public int 		 TaskID = 0;
	[HideInInspector]
	public float CurrentTimeVal = 0;
	[HideInInspector]
	public float CurrentPassimeVal = 0;
	[HideInInspector]
	public float StartTimeVal = 0;
	
	public string taskName = "Wicked Beasts";
}
