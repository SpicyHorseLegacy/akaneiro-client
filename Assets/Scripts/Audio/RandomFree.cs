using UnityEngine;
using System.Collections;


public class RandomFree : MonoBehaviour {
	
	public Transform SoundEffect;
	public Vector3[] SoundPositions = new Vector3[1];
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
			int sizeT = SoundPositions.Length;
			
			int iNowIndex =  Random.Range(0, sizeT);
			
			
			Vector3 vNowPosition = SoundPositions[iNowIndex];
			
			if( ActualSoundEffect != null)
			{
				ActualSoundEffect.position = vNowPosition;
				ActualSoundEffect.rotation = transform.rotation;
		
				SoundCue.Play(ActualSoundEffect.gameObject);
			}
			
			
			mLastTime = Time.realtimeSinceStartup;
			
			mActualInterval = Random.Range(minSeconds, maxSecons);
		}
	
	}
	
	
}

