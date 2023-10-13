using UnityEngine;
using System.Collections;

public class DestoryAfterTime : MonoBehaviour {

	public float time = 1f;
	
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
