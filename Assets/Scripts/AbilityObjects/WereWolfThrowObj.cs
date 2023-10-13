using UnityEngine;
using System.Collections;

public class WereWolfThrowObj : MonoBehaviour {

	public float speed = 12f;
	public int damage = 15;
	[HideInInspector]
	public WerewolfRanged werewolf;
	
	float LifeTime = 1.5f;
	Vector3 dir=Vector3.zero;
	
	// Use this for initialization
	void Start () {
		
		dir = (Player.Instance.transform.position + Vector3.up*0.8f - transform.position).normalized;
	
	}
	
	// Update is called once per frame
	void Update () {
		
		LifeTime -= Time.deltaTime;
		
		if(LifeTime > 0f)
		{
			transform.position += dir * speed * Time.deltaTime;
		}
		else
		{
			Destroy(transform.gameObject);
		}
	
	}
	
	void OnTriggerEnter(Collider other)
	{
		if((Player.Instance.transform.position + Vector3.up*0.8f - transform.position).magnitude < 0.5f)
		{
		    werewolf.AttackPlayer(0);		
			Destroy(transform.gameObject);
		}
	}
}
