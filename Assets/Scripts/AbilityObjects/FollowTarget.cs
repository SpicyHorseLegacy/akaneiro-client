using UnityEngine;
using System.Collections;

public class FollowTarget : MonoBehaviour {
	
	Transform target;
	Vector3 offset;
	
	Quaternion tarRotation;
	bool go = false;
	
	// Update is called once per frame
	void Update () {
		
		if(go)
		{
			transform.rotation = tarRotation;
		}
	}
	
	public void GO()
	{
		tarRotation = transform.rotation;
		go = true;
	}
	
	public void FollowTargetTransform(Transform tran)
	{
		FollowTargetTransformWithOffset(tran, Vector3.zero);
	}
	
	public void FollowTargetTransformWithOffset(Transform tran, Vector3 off)
	{
		target = tran;
		offset = off;
		go = true;
	}
}
