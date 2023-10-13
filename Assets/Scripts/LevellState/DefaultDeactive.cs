
using UnityEngine;
using System.Collections;

public class DefaultDeactive : MonoBehaviour {

	// Use this for initialization
	
	
	void Awake(){
		this.gameObject.SetActiveRecursively(false);
		
	}
	
	void Start () {
		
	}

}

