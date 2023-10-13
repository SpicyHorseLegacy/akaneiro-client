using UnityEngine;
using System.Collections;


public class RandomStereo : MonoBehaviour {
	
	public Transform SoundEffect;
	public float minSeconds;
	public float maxSecons;
	float mLastTime;
	float mActualInterval;
	Transform  ActualSoundEffect;
	
	
	// Use this for initialization
	void Start () {
		
		mLastTime = Time.realtimeSinceStartup;
		mActualInterval = Random.Range(minSeconds, maxSecons);
		if( SoundEffect != null)
			ActualSoundEffect = Instantiate(SoundEffect) as Transform;
		
	
	}
	
	// Update is called once per frame
	void Update () {
		
		if(Time.realtimeSinceStartup - mLastTime >= mActualInterval)
		{
			
			if( ActualSoundEffect != null)
			{
				ActualSoundEffect.position = transform.position;
				ActualSoundEffect.rotation = transform.rotation;
				SoundCue.Play(ActualSoundEffect.gameObject);
			}
			mLastTime = Time.realtimeSinceStartup;
			
			mActualInterval = Random.Range(minSeconds, maxSecons);
		}
	
	}
	
	
}
