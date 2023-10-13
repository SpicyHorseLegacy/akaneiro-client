using UnityEngine;
using System.Collections;

public class WorldBarTip : MonoBehaviour {
	
	public static WorldBarTip Instance = null;
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
	
	public void ShowWorldBarTip(Vector3 pos,float width,float height){
		float x = 0f;
		float y = 0f;
		float z = 0f;
		float tHeight = 0f;
		//left bot
		x = (0-bg.width);
		y = 0;
		z = 0;
		bg.transform.position = new Vector3(pos.x + x + 0.5f,pos.y + y - 0.5f,-2.5f);
	}
	
	public void DismissTip(){
		bg.transform.position = new Vector3(999f,999f,999f);
	}
	
}
