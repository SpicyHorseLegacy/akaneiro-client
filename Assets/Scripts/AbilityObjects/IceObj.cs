using UnityEngine;
using System.Collections;

public class IceObj : MonoBehaviour {
	
	bool isGo;
	bool isHold;
	bool isDead;
	Vector3 tarScale = Vector3.one;
	float time;
	
	public float GrowingTime = 0.2f;
	public float HoldTime = 2f;
	public float DyingTime = 2f;
	
	void Awake() {
		Transform[] icepieces = transform.GetComponentsInChildren<Transform>();
		foreach(Transform icepiece in icepieces)
		{
			if(icepiece == transform)
				continue;
			
			icepiece.gameObject.AddComponent<IcePieceObj>();
		}	
	}
	
	// Update is called once per frame
	void Update () {
		if(isGo)
		{
			transform.localScale = Vector3.Lerp(transform.localScale,tarScale,Time.deltaTime / GrowingTime);
			time += Time.deltaTime;
			if(time > GrowingTime)
			{
				transform.localScale = tarScale;
				isGo = false;
				time = 0;
			}
		}else{
			
			if(isHold)
			{
				time += Time.deltaTime;
				if(time > HoldTime)
				{
					isHold = false;
					time = 0;
				}
			}else if(!isDead)
			{
				Transform[] icepieces = transform.GetComponentsInChildren<Transform>();
				foreach(Transform icepiece in icepieces)
				{
					if(icepiece == transform)
						continue;
					
					icepiece.localScale = Vector3.Lerp(icepiece.localScale,Vector3.zero, Time.deltaTime/2 );
				}
				
				transform.position -= Vector3.up * Time.deltaTime;
				
				time += Time.deltaTime;
				if(time > DyingTime){
					isDead = true;
					transform.gameObject.SetActiveRecursively(false);
				}
			}
		}
	}
	
	public void Go()
	{
		isGo = true;
		isHold = true;
		isDead = false;
		time = 0;
		
		transform.gameObject.SetActiveRecursively(true);
		
		transform.localScale = new Vector3(1,0.1f,1);
		IcePieceObj[] icepieces = transform.GetComponentsInChildren<IcePieceObj>();
		foreach(IcePieceObj icepiece in icepieces)
		{
			icepiece.ResetScale();
			
			//float euglurY = Random.Range(0,360);
			//icepiece.transform.localEulerAngles = new Vector3(icepiece.transform.localEulerAngles.x, euglurY, icepiece.transform.eulerAngles.z);
		}
	}
}
