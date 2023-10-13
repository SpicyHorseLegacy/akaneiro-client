using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DestroyAfterTimeWithSpeed : MonoBehaviour {

	public float time = -1f;
	public Vector3 Speed = Vector3.zero;

    public List<Transform> MoveObjs = new List<Transform>();
	
	void Update () {
		
		if(time == -1)
			return;
		
		if(time>0f)
		{
			time-=Time.deltaTime;
            foreach (Transform obj in MoveObjs)
            {
                obj.position += Speed * Time.deltaTime;
            }
		}else{
            gameObject.AddComponent<DestructAfterTime>();
            gameObject.GetComponent<DestructAfterTime>().DestructNow();
		}
	}

    public void StartWithTime(float _t)
    {
        time = _t;
    }
}
