using UnityEngine;
using System.Collections;

public class OniThrowObj : AbilityObject {
	
	public float speed = 20f;
	[HideInInspector]   public Vector3 dir=Vector3.zero;

    public bool IsPlayImpactSound;                  //
    public Transform ImpactSoundPrefab;

    public bool IsPlayImpactVFX = false;            // control if show vfx
    public Transform ImpactVFXPrefab;  
	
	// Update is called once per frame
	void Update () {
		
		transform.position += dir * speed * Time.deltaTime;
		
	}
}
