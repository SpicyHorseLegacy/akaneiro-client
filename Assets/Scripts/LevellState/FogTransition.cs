using UnityEngine;
using System.Collections;

public class FogTransition : MonoBehaviour {

	// Use this for initialization
	
	Rect myRect;
	
	public Color DestFogColor;
	
	public Color DestAmbientColor;
	
	public float DestFogStartDist;
	
	public float DestFogEndDist;
	
	public float TransitionSpeed = 5f;
	
	void Start () {
		
		if(transform.GetComponent<BoxCollider>())
		{
			float sizex = transform.GetComponent<BoxCollider>().size.x;
		    sizex *= transform.localScale.x;
		    float sizez = transform.GetComponent<BoxCollider>().size.z;
		    sizez *= transform.localScale.z;
			myRect = new Rect(transform.position.x - sizex/2f,transform.position.z - sizez/2f,sizex,sizez);
		}
	
	}
	
	// Update is called once per frame
	void Update () {
	    float rotY = transform.eulerAngles.y;
		rotY *= Mathf.Deg2Rad;
		
		Vector3 tempPos = new Vector3(0,0,0);
		if(Player.Instance) {
			tempPos = Player.Instance.transform.position;
		}else if(PlayerMoveForLD.Instance) {
			tempPos = PlayerMoveForLD.Instance.transform.position;
		}

	    tempPos.x -= transform.position.x;
		tempPos.z -= transform.position.z;
		
		Vector3 curPos = tempPos;
		
		if( rotY > 0.0001 || rotY < -0.0001)
		{
		   //curPos.x = Mathf.Cos(rotY)*tempPos.x - Mathf.Sin(rotY)*tempPos.z;
		   //curPos.z = Mathf.Cos(rotY)*tempPos.z + Mathf.Sin(rotY)*tempPos.x;
			
		   curPos.x = Mathf.Cos(-rotY)*tempPos.x + Mathf.Sin(-rotY)*tempPos.z;
		   curPos.z = Mathf.Cos(-rotY)*tempPos.z - Mathf.Sin(-rotY)*tempPos.x;
		}
		
		curPos = transform.position + curPos;
		
		curPos.y = curPos.z;
		
		if(myRect.Contains(curPos))
		{
			 Color CurrentColor =  RenderSettings.fogColor;
			 Color CurrentAmbientColor = RenderSettings.ambientLight;
			
			 float CurrentStart =  RenderSettings.fogStartDistance;
			 float CurrentEnd = RenderSettings.fogEndDistance;
			 
			 float dis = TransitionSpeed * Time.deltaTime;
			
			 CurrentColor = Color.Lerp(CurrentColor,DestFogColor,dis);
			 CurrentStart = Mathf.Lerp(CurrentStart,DestFogStartDist,dis);
			 CurrentEnd = Mathf.Lerp(CurrentEnd,DestFogEndDist,dis);
			 CurrentAmbientColor = Color.Lerp(CurrentAmbientColor,DestAmbientColor,dis);
			
			 RenderSettings.fogColor = CurrentColor;
			 RenderSettings.fogStartDistance = CurrentStart;
			 RenderSettings.fogEndDistance = CurrentEnd;
			 RenderSettings.ambientLight = CurrentAmbientColor;
		}
	}
}
