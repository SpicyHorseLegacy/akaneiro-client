using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

[System.Serializable]
public class _UI_CS_Branch{
	public  List<_UI_CS_Task> 	    taskArray    = new List<_UI_CS_Task>();
	public string 	BranchName;
	public int 		BranchID = 0;
	public bool   	isBonus;
	public int 		rewardContentKarma;
	public int 		rewardContentExp;
}
