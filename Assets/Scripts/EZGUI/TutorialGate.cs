using UnityEngine;
using System.Collections;

public class TutorialGate : MonoBehaviour {

	public static TutorialGate Instance = null;
	
	public GateClient door1;
	public GateClient door2;
	
	void Awake(){
		Instance = this;
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	
}
