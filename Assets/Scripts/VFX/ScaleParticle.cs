using UnityEngine;
using System.Collections;

public class ScaleParticle : MonoBehaviour {
	
	public float scale = 1.0f;
	// Use this for initialization
	void Start () {
		
		if(!transform)
			return;
		
		transform.localScale *= scale;
		ParticleEmitter[] pes = GetComponentsInChildren<ParticleEmitter>();
		ParticleAnimator[] pas = GetComponentsInChildren<ParticleAnimator>();
		foreach(ParticleEmitter pe in pes)
		{
			pe.minSize *= scale;
			pe.maxSize *= scale;
			pe.worldVelocity *= scale;
			pe.localVelocity *= scale;
			pe.rndVelocity *= scale;
			pe.emitterVelocityScale *= scale;
			//print("" + pe.angularVelocity);
			//pe.angularVelocity = 100;
			//pe.rndRotation *= scale;
			//pe.constantForce. *= scale;
			
		}
		foreach(ParticleAnimator pa in pas)
		{
			pa.worldRotationAxis *= scale;
			pa.localRotationAxis *= scale;
			pa.rndForce *= scale;
			pa.force *= scale;
		}
		
	}
}
