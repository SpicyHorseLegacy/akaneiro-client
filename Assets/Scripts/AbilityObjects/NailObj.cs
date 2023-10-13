using UnityEngine;
using System.Collections;

public class NailObj : MonoBehaviour {

	Vector3 templocalPosition;
	 
	// Use this for initialization
	void Awake () {
		templocalPosition = transform.localPosition;
	}
	
	// Update is called once per frame
	public void ResetPosition () {
		transform.localPosition = templocalPosition;
	}
}
