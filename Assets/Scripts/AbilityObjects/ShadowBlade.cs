using UnityEngine;
using System.Collections;

public class ShadowBlade : MonoBehaviour {
	
	[HideInInspector]
	public Vector3 target;
	[HideInInspector]
	public bool IsActive=false;

	public float LifeTime = 3f; 
	public float speed = 10f;
	
	
	Vector3 dir;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
		if(!IsActive) return;
		
		LifeTime -= Time.deltaTime;
		
		//fly to target and check collision on flying path
		dir = Vector3.Normalize(target - transform.position);
		transform.position =transform.position + dir * Time.deltaTime * speed;
		
		if(LifeTime < 0f || (target - transform.position).magnitude < 0.2f)
		{
			Destroy(gameObject);
			return;
		}
		
		float dis;
		foreach(Transform npc in Player.Instance.AllEnemys)
		{
			dis = (npc.transform.position - transform.position).magnitude;
			if(dis < 0.2f)
			{
				npc.GetComponent<NpcBase>().IsShadowBladeDamageEffect = true;
				npc.GetComponent<NpcBase>().ShadowBladeDamageEffectTime = 8f;
			}
		}
	}
}
