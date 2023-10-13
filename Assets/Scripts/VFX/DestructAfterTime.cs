using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DestructAfterTime : MonoBehaviour {

    static public void DestructGameObjectNow(GameObject _target)
    {
        DestructAfterTime _destruct = _target.GetComponent<DestructAfterTime>();
        if (!_destruct)
            _destruct = _target.AddComponent<DestructAfterTime>();

        _destruct.DestructNow();
    }
	
	static public void DestructGameObjectNow(Transform _target)
    {
        DestructGameObjectNow(_target.gameObject);
    }
	
	static public void DestructGameObjectAfterTime(GameObject _target, float time)
	{
		DestructAfterTime _destruct = _target.GetComponent<DestructAfterTime>();
        if (!_destruct)
            _destruct = _target.AddComponent<DestructAfterTime>();

        _destruct.time = time;
	}
	
	static public void DestructGameObjectAfterTime(Transform _target, float time)
	{
		DestructGameObjectAfterTime(_target.gameObject, time);
	}

	public float time = 1f;
	
	// Update is called once per frame
	void Update () {
		if(time == -1)
			return;
		
		if(time>0f)
		{
			time -= Time.deltaTime;
		}
		else
		{
			DestructNow();
		}
	}
	
	public void DestructNow()
	{
		ParticleEmitter[] pars = transform.GetComponentsInChildren<ParticleEmitter>();
		foreach(ParticleEmitter par in pars)
		{
			// Stop emitting.And set the particle autodestruct
			par.emit = false;
			ParticleAnimator parAnim = par.GetComponent<ParticleAnimator>();
			if(parAnim)
				parAnim.autodestruct = true;
			
			if(par.maxEnergy == 0 && par.minEnergy == 0)
			{
				// Destroy the never end pars
				Destroy(par.gameObject);
				continue;
			}
		}
		
		TrailRenderer[] trails = transform.GetComponentsInChildren<TrailRenderer>();
		foreach(TrailRenderer trail in trails)
		{
			trail.autodestruct = true;
			trail.transform.parent = null;
			trail.gameObject.AddComponent<DestroyAfterFadeOut>();
			trail.GetComponent<DestroyAfterFadeOut>().GoToHell();
		}
		
		Transform[] children = transform.GetComponentsInChildren<Transform>();
		foreach(Transform child in children)
		{
			if(child == transform) continue;
            if (child.GetComponent<DestructAfterTime>())
                child.GetComponent<DestructAfterTime>().DestructNow();
            else
            {
                if (!child.GetComponent<DestroyAfterFadeOut>())
                {
                    child.gameObject.AddComponent<DestructAfterTime>();
                    child.GetComponent<DestructAfterTime>().time = 5;
                }
            }
			child.SendMessage("GoToHell",SendMessageOptions.DontRequireReceiver);
		}

        if (transform.parent)
        {
            Transform[] children2 = transform.GetComponentsInChildren<Transform>();
            foreach (Transform child in children2)
            {
                child.parent = transform.parent;
            }
        }
        else
        {
            transform.DetachChildren();
        }

		Destroy(gameObject);
	}
}
