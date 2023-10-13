using UnityEngine;
using System.Collections;

public class FXManager {
	
	public Transform[] FXPrefabs;   
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	public void Execute () {
	
	}
	
	public void PlayFXWithID(int id)
	{
		PlayFXWithIDAndPos(id,Vector3.zero);
	}
	
	public void PlayFXWithIDAndPos(int id, Vector3 pos)
	{
		
	}
}
