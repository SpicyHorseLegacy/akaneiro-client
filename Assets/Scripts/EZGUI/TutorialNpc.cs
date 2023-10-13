using UnityEngine;
using System.Collections;

public class TutorialNpc : MonoBehaviour {
	
	public static TutorialNpc Instance = null;
	
	public Transform oldLady1Pos;
	public Transform oldLady1;
	public Transform oldLady2Pos;
	public Transform oldLady2;
	
	void Awake(){
		Instance = this;
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	
	public void ShowObj(Transform obj,Vector3 pos){
		obj.position = pos;
	}
	
	public void HideObj(Transform obj){
		obj.position = new Vector3(999f,999f,999f);
	}
}
