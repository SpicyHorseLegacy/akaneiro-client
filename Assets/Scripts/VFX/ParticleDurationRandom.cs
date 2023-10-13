using UnityEngine;
using System.Collections;

public class ParticleDurationRandom : MonoBehaviour {

	public float delayTimeMin = 1f;
	public float delayTimeMax = 5f;
	float delayTime = 1f;
	ParticleEmitter[] pes = null;
	
	void Start()
	{
		pes = GetComponentsInChildren<ParticleEmitter>();
	}
	
	// Update is called once per frame
	void Update () {
		
		if(delayTime>0f)
		{
			delayTime -= Time.deltaTime;
			foreach(ParticleEmitter pe in pes)
				pe.emit = false;
		}else{
			foreach(ParticleEmitter pe in pes)
				pe.emit = true;
			delayTime = Random.Range(delayTimeMin,delayTimeMax);
		}
	}
}
