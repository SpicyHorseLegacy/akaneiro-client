using UnityEngine;
using System.Collections;

public class ThreatTips : MonoBehaviour {
	
	public static ThreatTips Instance = null;
	public UIButton		bg;
	public SpriteText	title;
	public SpriteText	description;
	
	void Awake(){
		Instance = this;
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void ShowThreatTips(bool isIncrease,Vector3 pos,float width,float height){
		if(isIncrease){
//			title.Text 		 = "Increase Threat ";
			LocalizeManage.Instance.GetDynamicText(title,"INCTHREAT");
//			description.Text = "Spend Karma Shards to raise the threat level.";
			LocalizeManage.Instance.GetDynamicText(description,"INCTHREATINFO");
		}else{
//			title.Text 		 = "Decrease Threat ";
			LocalizeManage.Instance.GetDynamicText(title,"DECTHREAT");
//			description.Text = "Spend Karma Shards to lower the threat level.";
			LocalizeManage.Instance.GetDynamicText(description,"DECTHREATINFO");
		}
		float x = 0f;
		float y = 0f;
		float z = 0f;
		float tHeight = 0f;
		//left bot
		x = (0-bg.width) - width/2;
		y = 0 - height/2;
		z = 0;
		bg.transform.position = new Vector3(pos.x + x + 1f,pos.y + y + 0.5f,-2.5f);
	}
	
	public void DismissTip(){
		bg.transform.position = new Vector3(999f,999f,999f);
	}
}
