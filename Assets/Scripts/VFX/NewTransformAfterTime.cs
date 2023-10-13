using UnityEngine;
using System.Collections;

public class NewTransformAfterTime : MonoBehaviour {
	
	public Transform[] Transforms;
	
	public float timer = 1;
	
	// Update is called once per frame
	void Update () {
		timer -= Time.deltaTime;
		if(timer < 0)
		{
			if(Transforms.Length > 0)
			{
				foreach(Transform tran in Transforms)
				{
					Instantiate(tran, transform.position,transform.rotation);
				}
			}
			Destroy(this);
		}
	}
}
