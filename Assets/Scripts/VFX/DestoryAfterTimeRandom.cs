using UnityEngine;
using System.Collections;

public class DestoryAfterTimeRandom : MonoBehaviour {

	public float timeMin = 1f;
	public float timeMax = 5f;
	float time=1f;
	void Start()
	{
		time = Random.Range(timeMin,timeMax);
	}
	// Update is called once per frame
	void Update () {
		
		if(time>0f)
		{
			time-=Time.deltaTime;
		}else{
			Destroy(gameObject);
		}
	}
}
