
using UnityEngine;
using System.Collections;

public class RandomLighting : MonoBehaviour {

	// Use this for initialization
	
	public float minLoopTime =0f;
	public float maxLoopTime = 1f;
	
	public float minTurnOnTime = 0.3f;
    public float maxTurnOnTime = 0.4f;
	
	
	
	float mNowTime = 0f;
	float mNowOnTime = 0f;
	float mInterval = 0f;
	float mContinueOn = 0f;
	Light mTHisLight = null;
	void Awake(){
		
		
	}
	
	void Start () {
	    
		mTHisLight = transform.GetComponent<Light>();
		mNowTime = Time.realtimeSinceStartup;
		mInterval = Random.Range(minLoopTime,maxLoopTime);
		mTHisLight.enabled = false;
		
	}
	
	void Update () {
		
		if(mTHisLight == null)
			return;
		if( Time.realtimeSinceStartup - mNowTime >= mInterval)
		{
			mTHisLight.enabled = true;
			mInterval = Random.Range(minLoopTime,maxLoopTime);
			mContinueOn = Random.Range(minTurnOnTime,maxTurnOnTime);
			mInterval += mContinueOn;
			mNowTime = Time.realtimeSinceStartup;
			mNowOnTime = mNowTime;
		}
		if(mTHisLight.enabled && Time.realtimeSinceStartup - mNowOnTime >= mContinueOn)
		{
		   mTHisLight.enabled = false;
		}
		
	}

}