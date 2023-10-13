using UnityEngine;
using System.Collections;

public class EmitRandom : MonoBehaviour {

	public float delayTimeMin = 1f;
	public float delayTimeMax = 5f;
	float delayTime = 1f;
	
	// Update is called once per frame
	void Update () {
		if(particleEmitter == null)
			Destroy(this);
		
		if(delayTime>0f)
		{
			delayTime -= Time.deltaTime;
			particleEmitter.emit = false;
		}else{
			particleEmitter.emit = true;
			delayTime = Random.Range(delayTimeMin,delayTimeMax);
		}
	}
}
