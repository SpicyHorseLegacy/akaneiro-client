using UnityEngine;
using System.Collections;

public class LevelCapTip : MonoBehaviour {
	
	public static LevelCapTip Instance = null;
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
	
	public void ShowLevelCapTip(bool isleft,Vector3 pos,float width,float height){
		float x = 0f;
		float y = 0f;
		float z = 0f;
		float tHeight = 0f;
		if(isleft){
			x = (0-bg.width);// - width/2;
		}else{
			x =  width/2;
		}
		y = 0 ;//- height/2;
		z = 0;
		bg.transform.position = new Vector3(pos.x + x ,pos.y + y ,-2.5f);
	}
	
	public void DismissTip(){
		bg.transform.position = new Vector3(999f,999f,999f);
	}
}
