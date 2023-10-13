using UnityEngine;
using System.Collections;

public class TransmuteTips : MonoBehaviour {
	
	public static TransmuteTips Instance = null;
	public UIButton		bg;
	
	void Awake(){
		Instance = this;
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void ShowTransmuteTips(Vector3 pos,float width,float height){
		float x = 0f;
		float y = 0f;
		float z = 0f;
		float tHeight = 0f;
		//left top
		x = (0-bg.width) - width/2;
		y = 0 + height/2;
		z = 0;
		bg.transform.position = new Vector3(pos.x + x,pos.y + y ,-2.5f);
	}
	
	public void DismissTip(){
		bg.transform.position = new Vector3(999f,999f,999f);
	}
}
