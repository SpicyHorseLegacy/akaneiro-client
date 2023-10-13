using UnityEngine;
using System.Collections;

public class _UI_CS_TopState : MonoBehaviour {
	
	public static _UI_CS_TopState Instance = null;
	
	//top state tip
	public UIProgressBar CurrentNpcHPBar;
	public SpriteText    CurrentNpcNameText;
	public UIButton      Bg;
	public UIPanel		 TopStatePanel;
	public UIButton      WantedBg;
	public SpriteText    WantedText;
	public UIButton      IconBg;
	public bool 		 isShow = true;
	
	void Awake(){
		Instance = this;
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
	}

#region Local
	private void DismissBar(){
		if(isShow){
			_UI_CS_DebugInfo.Instance.SetTargetInfo(false,"",0,0);
			isShow = false;
			Bg.Hide(true);
			IconBg.Hide(true);
			CurrentNpcNameText.Hide(true);
			CurrentNpcHPBar.Hide(true);
			CurrentNpcNameText.Text = "";
			CurrentNpcHPBar.Value   = 1;
			WantedBg.Hide(true);	
			WantedText.Text = "";
		}
	}
#endregion

#region Interface
	public void ShowTargetBar(Transform obj){
		if(null == obj){
			DismissBar();
			return;
		}
		if(null != Player.Instance){
			if(obj.GetComponent<NpcBase>() != null){
				isShow = true;
				Bg.Hide(false);
				IconBg.Hide(false);
				CurrentNpcNameText.Hide(false);
				CurrentNpcHPBar.Hide(false);
				CurrentNpcNameText.Text = obj.GetComponent<NpcBase>().NpcName;
				CurrentNpcNameText.SetColor(_UI_Color.Instance.color1);
                CurrentNpcHPBar.Value = (obj.GetComponent<NpcBase>().AttrMan.Attrs[EAttributeType.ATTR_CurHP] * 1.0f) / obj.GetComponent<NpcBase>().AttrMan.Attrs[EAttributeType.ATTR_MaxHP];
				if(obj.GetComponent<NpcBase>().IsWanted){	
					WantedBg.Hide(false);	
					WantedText.Text = "WANTED !";
					LocalizeManage.Instance.GetDynamicText(WantedText,"WANTED");
					SetColor(0,true,false);
				}else{
						if(obj.GetComponent<NpcBase>().IsBoss){	
							WantedBg.Hide(false);	
							WantedText.Text = "Boss";
							LocalizeManage.Instance.GetDynamicText(WantedText,"BOSS");
							SetColor(0,false,false);
						}else{
							SetColor(obj.transform.GetComponent<NpcBase>().EnemyLevel,false,false);
						}
				}
                _UI_CS_DebugInfo.Instance.SetTargetInfo(true, CurrentNpcNameText.Text, (obj.GetComponent<NpcBase>().AttrMan.Attrs[EAttributeType.ATTR_CurHP] * 1.0f), obj.GetComponent<NpcBase>().AttrMan.Attrs[EAttributeType.ATTR_MaxHP]);	
			}else if(obj.GetComponent<ShopNpc>() != null){
				isShow = true;
				Bg.Hide(false);
				IconBg.Hide(true);
				CurrentNpcNameText.Hide(false);
				CurrentNpcHPBar.Hide(false);
				if(obj.GetComponent<NPC_Well>()) {
					CurrentNpcNameText.Text = ((int)obj.GetComponent<NPC_Well>().CurCollectKarma).ToString() + " / " + ((int)obj.GetComponent<NPC_Well>().ShouldGetKarma).ToString();
					CurrentNpcNameText.SetColor(_UI_Color.Instance.color29);
					CurrentNpcHPBar.SetColor(_UI_Color.Instance.color14);
	                CurrentNpcHPBar.Value = obj.GetComponent<NPC_Well>().CurPercent;
				}else {
					CurrentNpcNameText.Text = obj.GetComponent<ShopNpc>().npcName;
					CurrentNpcNameText.SetColor(_UI_Color.Instance.color1);
					CurrentNpcHPBar.SetColor(_UI_Color.Instance.color15);
	                CurrentNpcHPBar.Value = 1f;
				}
			}else if(obj.GetComponent<InteractiveHandler>() != null){
				if(!obj.GetComponent<InteractiveHandler>().IsUsed){
					isShow = true;
					Bg.Hide(false);
					IconBg.Hide(true);
					CurrentNpcNameText.Hide(false);
					CurrentNpcHPBar.Hide(false);
					CurrentNpcNameText.Text = obj.GetComponent<InteractiveHandler>().name;
					CurrentNpcNameText.SetColor(_UI_Color.Instance.color1);
					CurrentNpcHPBar.SetColor(_UI_Color.Instance.color27);
	                CurrentNpcHPBar.Value = 1f;
				}
			}else{
				DismissBar();
			}
			
		}
	}
	
	public void SetColor(int level,bool isWanted,bool isBoss){
		if(isWanted){
			CurrentNpcHPBar.SetColor(_UI_Color.Instance.color17);
			IconBg.SetColor(_UI_Color.Instance.color17);
			return ;
		}
		if(isBoss){
			CurrentNpcHPBar.SetColor(_UI_Color.Instance.color26);
			IconBg.SetColor(_UI_Color.Instance.color26);
			return ;
		}
		int idx = (level - _PlayerData.Instance.playerLevel);
		if(idx < 0){
			idx = 0;
		}
		switch(idx){
		case 0:
			CurrentNpcHPBar.SetColor(_UI_Color.Instance.color14);
			IconBg.SetColor(_UI_Color.Instance.color2);
			break;
		case 1:
			CurrentNpcHPBar.SetColor(_UI_Color.Instance.color14);
			IconBg.SetColor(_UI_Color.Instance.color27);
			break;
		case 2:
		case 3:
		case 4:
			CurrentNpcHPBar.SetColor(_UI_Color.Instance.color14);
			IconBg.SetColor(_UI_Color.Instance.color26);
			break;
		default:
			CurrentNpcHPBar.SetColor(_UI_Color.Instance.color14);
			IconBg.SetColor(_UI_Color.Instance.color14);
			break;
		}	
	}
#endregion
	
	
	
	
	
	
	
	
	
	
}
