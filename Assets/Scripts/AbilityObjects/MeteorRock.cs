using UnityEngine;
using System.Collections;

public class MeteorRock : MonoBehaviour {
	
	public GameObject ImpactPrefab;
	public float Duration = 1f;
	
	private bool isGo = false;
	private Vector3 vel;
	private Vector3 dropPoint;
	private float t;
	
	void Update () {
		if(!isGo)
			return;
		
		transform.Translate(vel * Time.deltaTime);
		t += Time.deltaTime;
		if(t > Duration)
		{
			Transform impact = Instantiate(ImpactPrefab, dropPoint, ImpactPrefab.transform.rotation) as Transform;
			Destroy(gameObject);
		}
	}
	
	public void GoTo(Vector3 drop){
		dropPoint = drop;
		vel = (dropPoint - transform.position) / Duration;
		t = 0;
		isGo = true;
	}
}
