using UnityEngine;
using System.Collections;

public class PingTips : MonoBehaviour {
	
	public static PingTips Instance = null;
	
	public UIButton		bg;
	public SpriteText	pingNum;
	
	void Awake(){
		Instance = this;
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void ShowPingTip(Vector3 pos,float width,float height){
		pingNum.Text = ((int)_UI_CS_FightScreen.Instance.pingTime).ToString();
		float x = 0f;
		float y = 0f;
		float z = 0f;
		float tHeight = 0f;
		//left med
		x = (0-bg.width) - width/2;
		y = //0 - height/2;
		z = 0;
		bg.transform.position = new Vector3(pos.x + x ,pos.y + y ,-2.5f);
	}
	
	public void DismissTip(){
		bg.transform.position = new Vector3(999f,999f,999f);
	}
	
}
