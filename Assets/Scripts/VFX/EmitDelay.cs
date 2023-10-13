using UnityEngine;
using System.Collections;

public class EmitDelay : MonoBehaviour {


	public float delayTime = 1f;
	
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
			Destroy(this);
		}
	}
}
