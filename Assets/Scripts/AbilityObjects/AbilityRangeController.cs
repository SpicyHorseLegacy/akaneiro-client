using UnityEngine;
using System.Collections;

public class AbilityRangeController : MonoBehaviour {

	public ParticleEmitter[] emits = new ParticleEmitter[0];
	bool isStarted = false;
	bool isDestroyFinished = false;
	float lifeTime = -1;
	
	void Awake()
	{
		if(emits.Length > 0)
		{
			foreach(ParticleEmitter emit in emits)
			{
				emit.emit = false;
			}
		}
	}
	
	void Update()
	{
		if(isStarted && lifeTime > 0)
		{
			lifeTime -= Time.deltaTime;
			if(lifeTime <= 0)
			{
				ShutDownWithIsDestroyFinished(isDestroyFinished);
			}
		}
	}
	
	public void StartShow()
	{
		foreach(ParticleEmitter emit in emits)
		{
			emit.emit = true;
		}
		lifeTime = -1;
		isDestroyFinished = false;
		isStarted = true;
	}
	
	public void StartWithRadiusAndDurationAndIsDestroyFinished(float radius, float duration, bool isDestroy)
	{
		foreach(ParticleEmitter emit in emits)
		{
			emit.minSize = radius;
			emit.maxSize = radius;
			emit.emit = true;
		}
		
		lifeTime = duration;
		isDestroyFinished = isDestroy;
		isStarted = true;
	}
	
	public void ShutDownWithIsDestroyFinished( bool isDestroy )
	{
		foreach(ParticleEmitter emit in emits)
		{
			emit.emit = false;
		}
		
		isDestroyFinished = isDestroy;
		isStarted = false;
	
		if(isDestroyFinished)
		{
			gameObject.AddComponent<DestructAfterTime>();
			gameObject.GetComponent<DestructAfterTime>().DestructNow();
		}
	}
}
