using UnityEngine;
using System.Collections;

public class SelectCharaStats : MonoBehaviour {
	
	public UIButton 			icon;
	public SpriteText 			name;
	public SpriteText 			level;
	public bool 				isEmpty  = true;
	public bool 				isSelect = false;
	public UIRadioBtn			touchBtn;
	public SCharacterInfoLogin  info;
	public UIProgressBar 		LevelBar;
	public int 					idx;
	public UIButton 			createBtn;
	
	// Use this for initialization
	void Start () {
		touchBtn.AddInputDelegate(touchBtnDelegate);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void touchBtnDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:
				SelectChara.Instance.SetActiveCharaBtn(idx);
				SelectChara.Instance.UpdateModelEquip(idx);
				break;
		   default:
				break;
		}	
	}	
	
	public void UpdateSelectStats(SCharacterInfoLogin newInfo){
		isEmpty 		= false;
		info			= newInfo;
		name.Text 		= newInfo.nickname;
		level.Text 		= newInfo.level.ToString();
		long curexp		= _PlayerData.Instance.readCurExpVal(newInfo.level);
		long maxExp		= _PlayerData.Instance.ReadMaxExpVal(newInfo.level);
		LevelBar.Value  = (float)((long)newInfo.Exp-curexp) / (maxExp-curexp);
		icon.SetUVs(new Rect(0,0,1,1));
		icon.SetTexture(_PlayerData.Instance.GetPlayerIcon(newInfo.style,newInfo.sex.Get()));	
	}
	
	public void SetStats(bool isShow,SCharacterInfoLogin newInfo){
		if(isShow){
			UpdateSelectStats(newInfo);
			isEmpty = false;
			Hide(false);
		}else{
			DelSelectStats();
			isEmpty = true;
			isSelect = false;
			Hide(true);
		}
	}
	
	public void DelSelectStats(){
		isEmpty 		= true;
		info			= null;
		name.Text 		= "";
		level.Text 		= "";
		LevelBar.Value  = 0;
		icon.SetUVs(new Rect(0,0,1,1));
		icon.SetTexture(_UI_CS_Resource.Instance.m_EquipmentIcon[0]);	
	}
	
	public void Hide(bool isHids){
		if(isHids){
			touchBtn.transform.position = new Vector3(touchBtn.transform.position.x,touchBtn.transform.position.y,999f);
		}else{
			touchBtn.transform.position = new Vector3(touchBtn.transform.position.x,touchBtn.transform.position.y,0);
		}
	}
	
	public void ShowCreateBtn(){
		createBtn.transform.position = new Vector3(touchBtn.transform.position.x,touchBtn.transform.position.y,0);
	}
	
	
	
}
