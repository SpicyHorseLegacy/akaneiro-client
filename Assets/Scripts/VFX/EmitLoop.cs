using UnityEngine;
using System.Collections;

public class EmitLoop : MonoBehaviour {
	
	public float waitTime = 1f;
	float wTime;
	// Use this for initialization
	void Start () {
		wTime = waitTime;
	}
	
	// Update is called once per frame
	void Update () {
		if(particleEmitter == null)
			Destroy(this);
		
		if(particleEmitter.emit)
		{
			particleEmitter.emit = false;
			wTime = waitTime;
		}else{
			if(wTime>0)
			{		
				wTime-=Time.deltaTime;
			}else{
				particleEmitter.emit = true;
			}
		}
	}
}
