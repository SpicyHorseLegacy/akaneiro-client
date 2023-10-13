using UnityEngine;
using System.Collections;

public enum AbiPosOffestType{
	
	LEFT_TOP  = 1,	
	RIGHT_TOP = 2,	
	LEFT_BOT  = 3,	
	RIGHT_BOT = 4,	
	MAXOR
	
}

public class AbilitieTip : MonoBehaviour {
	//Instance
	public static AbilitieTip Instance = null;
	
	public UIButton		TipsBG;
	public UIButton		TipsSBG;
	public SpriteText	TipName;
	public SpriteText	TipDescription1;
	public SpriteText	TipDescription2;
	public SpriteText	TipDescription3Title;
	public SpriteText	TipDescription3Info;
	public SpriteText	TipDescription4Title;
	public SpriteText	TipDescription4Info;
	public SpriteText	TipLastLine;
	public SpriteText	TipLastLineS;
	public Transform    InfoObj;
	
	void Awake(){
		Instance = this;
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void AbiTipShow(Vector3 pos,_UI_CS_AbilitiesItem info,AbiPosOffestType posOffest){
		float x = 0f;
		float y = 0f;
		float z = 0f;

		if(null == info)
			return;
		
		if(info.m_level <= 1){
			switch(posOffest){
			case AbiPosOffestType.LEFT_TOP:
				x = (0-TipsSBG.width);
				y = TipsSBG.height;
				z = 0;
				break;
			case AbiPosOffestType.RIGHT_TOP:
				x = 0;
				y = TipsSBG.height;
				z = 0;
				break;
			case AbiPosOffestType.LEFT_BOT:
				x = (0-TipsSBG.width);
				y = 0;
				z = 0;
				break;
			case AbiPosOffestType.RIGHT_BOT:
				x = 0; y = 0; z = 0;
				break;
			default:
				break;
			}
			TipsBG.transform.position = new Vector3(1000,1000,1000);
			TipsSBG.transform.position = new Vector3(pos.x + x,pos.y + y,pos.z + z -0.6f);
			InfoObj.transform.position = new Vector3(pos.x + x,pos.y + y,pos.z + z -0.7f);
			TipDescription2.Text 		= "";
			TipDescription3Title.Text 	= "";
			TipDescription3Info.Text 	= "";
			TipDescription4Title.Text 	= "";
			TipDescription4Info.Text 	= "";
		}else{
			switch(posOffest){
			case AbiPosOffestType.LEFT_TOP:
				x = (0-TipsBG.width);
				y = TipsBG.height;
				z = 0;
				break;
			case AbiPosOffestType.RIGHT_TOP:
				x = 0;
				y = TipsBG.height;
				z = 0;
				break;
			case AbiPosOffestType.LEFT_BOT:
				x = (0-TipsBG.width);
				y = 0;
				z = 0;
				break;
			case AbiPosOffestType.RIGHT_BOT:
				x = 0; y = 0; z = 0;
				break;
			default:
				break;
			}
			TipsBG.transform.position  = new Vector3(pos.x+ x,pos.y + y,pos.z + z -0.6f);
			TipsSBG.transform.position = new Vector3(1000,1000,1000);
			InfoObj.transform.position = new Vector3(pos.x+ x,pos.y + y,pos.z + z -0.7f);
			//temp-----------------------------------
			TipDescription2.Text 		= "";
			TipDescription3Title.Text 	= "";
			TipDescription3Info.Text 	= "";
			TipDescription4Title.Text 	= "";
			TipDescription4Info.Text 	= "";
			//---------------------------------------
		}
		TipName.Text = info.m_name;
		TipDescription1.Text = info.m_details1;
		switch(info.m_type){
			case 1:
				
//				TipLastLine.Text = "Prowess abilities improve with POWER stat";
//				TipLastLineS.Text = "Prowess abilities improve with POWER stat";
				LocalizeManage.Instance.GetDynamicText(TipLastLine,"PROWESSABISTAT");
				LocalizeManage.Instance.GetDynamicText(TipLastLineS,"PROWESSABISTAT");
				TipName.SetColor(_UI_Color.Instance.color14);
				TipsBG.SetColor(_UI_Color.Instance.color14);
				TipsSBG.SetColor(_UI_Color.Instance.color14);
				break;
			case 2:
//				TipLastLine.Text = "Fortitude abilities improve with DEFENSE stat";
//				TipLastLineS.Text = "Fortitude abilities improve with DEFENSE stat";
				LocalizeManage.Instance.GetDynamicText(TipLastLine,"FORTITUDEABISTAT");
				LocalizeManage.Instance.GetDynamicText(TipLastLineS,"FORTITUDEABISTAT");
				TipName.SetColor(_UI_Color.Instance.color15);
				TipsBG.SetColor(_UI_Color.Instance.color15);
				TipsSBG.SetColor(_UI_Color.Instance.color15);
				break;
			case 4:
//				TipLastLine.Text = "Cunning abilities improve with SKILL stat";
//				TipLastLineS.Text = "Cunning abilities improve with SKILL stat";
				LocalizeManage.Instance.GetDynamicText(TipLastLine,"CUNNINGABISTAT");
				LocalizeManage.Instance.GetDynamicText(TipLastLineS,"CUNNINGABISTAT");
				TipName.SetColor(_UI_Color.Instance.color16);
				TipsBG.SetColor(_UI_Color.Instance.color16);
				TipsSBG.SetColor(_UI_Color.Instance.color16);
				break;
			default:
				break;
		}
	}
	
	public void HideTip(){
		InfoObj.transform.position = new Vector3(1000,1000,1000);
		TipsBG.transform.position  = new Vector3(1000,1000,1000);
		TipsSBG.transform.position = new Vector3(1000,1000,1000);
	}
}
