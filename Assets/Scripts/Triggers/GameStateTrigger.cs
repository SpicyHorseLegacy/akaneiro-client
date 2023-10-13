using UnityEngine;
using System.Collections;

public class GameStateTrigger : TriggerBase {

	// Use this for initialization
	//private int iCounter = 0;
	
    public string GameState = "s1";
	
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void  OnTriggerEnter(Collider other)
	{
		GlobalGameState.state = GameState;
	}
	/*
	void OnTriggerExit(Collider other)
	{
		GlobalGameState.state = "day";
	}
	*/
	/*
	public override void TriggerMe ()
	{
		base.TriggerMe ();
		
		OnTriggerEnter(Player.Instance.transform.collider);
	}
	*/
}
