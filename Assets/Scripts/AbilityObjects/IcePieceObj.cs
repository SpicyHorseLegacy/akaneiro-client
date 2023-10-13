using UnityEngine;
using System.Collections;

public class IcePieceObj : MonoBehaviour {
	
	Vector3 templocalScale;
	 
	// Use this for initialization
	void Awake () {
		templocalScale = transform.localScale;
	}
	
	// Update is called once per frame
	public void ResetScale () {
		transform.localScale = templocalScale;
	}
}
