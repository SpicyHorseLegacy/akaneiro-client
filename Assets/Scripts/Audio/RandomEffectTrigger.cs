using UnityEngine;
using System.Collections;


public class RandomEffectTrigger : MonoBehaviour {
	
	public MonoBehaviour[] mPushedBehavior = new MonoBehaviour[0];
	public bool mTurnFlag;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
		
	
	}
	
	public virtual void  OnTriggerEnter(Collider other)
	{
		foreach(MonoBehaviour iter in mPushedBehavior)
		{
			if( iter == null)
				continue;
			iter.enabled = mTurnFlag;
		}
	}
	
	
	
	
}

