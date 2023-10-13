using UnityEngine;
using System.Collections;

public enum DeathResultType
{
	DeathResultObject,
	DeathResultBuff,
}

[System.Serializable]
public class DeathResult
{
	//public DeathResultType Type = DeathResultType.DeathResultBuff;
	public DeathResultType Type = DeathResultType.DeathResultBuff;
	public int ID = 0;
	public float Chance = 1;
	
}
