using UnityEngine;
using System.Collections;

public class SelfSpin : MonoBehaviour {
	
	public Vector3 SpinSpeed;
	public float Life;
	public float DieTime;
	
	float life;
	float dietimer;
	
	// Use this for initialization
	void Awake () {
		life = Life;
		dietimer = DieTime;
	}
	
	// Update is called once per frame
	void Update () {
		
		transform.localEulerAngles += SpinSpeed * Time.deltaTime;
		if(Life == -1)
			return;
		
		if(life > 0)
			life -= Time.deltaTime;
		else if(dietimer > 0)
		{
			dietimer -= Time.deltaTime;
			transform.localScale = Vector3.Lerp(transform.localScale,Vector3.zero,Time.deltaTime / DieTime);
		}
		else
		{
			Destroy(gameObject);
		}
	}
	
	public void GoToHell()
	{
		life = 0;
	}
}
