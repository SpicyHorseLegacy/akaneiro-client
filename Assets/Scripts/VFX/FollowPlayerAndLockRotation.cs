using UnityEngine;
using System.Collections;

public class FollowPlayerAndLockRotation : MonoBehaviour {

	Quaternion tarRotation;
	
	// Use this for initialization
	void Awake () {
		transform.parent = Player.Instance.transform;
		tarRotation = transform.rotation;
	}
	
	// Update is called once per frame
	void Update () {
		transform.rotation = tarRotation;
	}
}
