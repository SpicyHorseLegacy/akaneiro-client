using UnityEngine;
using System.Collections;

public class _AbiMenuObj : MonoBehaviour {
	
	public UIButton 	equipBg;
	public UIButton 	icon1;
	public UIButton 	icon2;
	public UIButton 	black;
	public UIButton 	lockIcon;
	public UIButton 	HighLight;
	public int 			ListID = 0;
	public bool 		isAbiEquip = false;
	public bool 		isAbiEmpty = true;
	public _UI_CS_AbilitiesItem		abilitiesInfo;

	// Use this for initialization
	void Start () {
		icon2.AddInputDelegate(IconDelegate);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void InitObjInfo(_UI_CS_AbilitiesItem abi,bool isLock,int level,bool isEquip){
		abilitiesInfo = abi;
		icon1.Hide(false);
		icon1.SetUVs(new Rect(0,0,1,1));
		icon1.SetTexture(AbilityInfo.Instance.GetAbilityByID((uint)abi.m_AbilitieID).icon);
		icon2.Hide(false);
		icon2.SetUVs(new Rect(0,0,1,1));
		icon2.SetTexture(AbilityInfo.Instance.GetAbilityByID((uint)abi.m_AbilitieID).icon);
		if(isLock){
			black.Hide(false);
			lockIcon.Hide(false);
		}else{
			black.Hide(true);
			lockIcon.Hide(true);
		}
		isAbiEquip = isEquip;
		if(isEquip){
			equipBg.Hide(false);
		}else{
			equipBg.Hide(true);
		}
		HighLight.Hide(true);
	}
	
	public int MoveToTarget(){
//		for(int i = 0;i<targetGroup.Length;i++){
//			if(icon2.transform.position.x>targetGroup[i].transform.position.x-(targetGroup[i].width*0.5f)
//				&&icon2.transform.position.x<targetGroup[i].transform.position.x+(targetGroup[i].width*0.5f)
//				&&icon2.transform.position.y>targetGroup[i].transform.position.y-(targetGroup[i].height*0.5f)
//				&&icon2.transform.position.y<targetGroup[i].transform.position.y+(targetGroup[i].height*0.5f)){
//				return i;
//			}
//		}
		return -1;
	}
	
	void IconDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.MOVE:	
					RightClick();
				break;
		   case POINTER_INFO.INPUT_EVENT.NO_CHANGE:
					AbilitieTip.Instance.AbiTipShow(equipBg.transform.position,abilitiesInfo,AbiPosOffestType.RIGHT_BOT);
					RightClick();
				break;
		   case POINTER_INFO.INPUT_EVENT.MOVE_OFF:
		   case POINTER_INFO.INPUT_EVENT.RELEASE_OFF:
					AbilitieTip.Instance.HideTip();
				break;
		   case POINTER_INFO.INPUT_EVENT.TAP:
//					if(!isAbiEquip && !isAbiEmpty){
					if(!isAbiEquip){
						if(_AbiMenuCtrl.Instance.isBarOperate){
							_AbiMenuCtrl.Instance.CurrentAbiObjIdx = ListID;
							_AbiMenuCtrl.Instance.isOperate = false;
							_AbiMenuCtrl.Instance.isBarOperate = false;
							_AbiMenuCtrl.Instance.HighLightCurrentAbiBar(false);	
							LogManager.Log_Debug("--- SetSkillShortcut ---");
							CS_Main.Instance.g_commModule.SendMessage(
					   			ProtocolBattle_SendRequest.SetSkillShortcut(abilitiesInfo.m_AbilitieID,_AbiMenuCtrl.Instance.GroupIdx,_AbiMenuCtrl.Instance.PreAbiBarObjIdx)
							);
							SoundCue.PlayPrefabAndDestroy(_AbiMenuCtrl.Instance.SwapSound);
							_AbiMenuCtrl.Instance.ChangeAbiBarEleInfo(_AbiMenuCtrl.Instance.GroupIdx,_AbiMenuCtrl.Instance.PreAbiBarObjIdx,abilitiesInfo,false);
							_AbiMenuCtrl.Instance.SetCurrentGroup(_AbiMenuCtrl.Instance.GroupIdx);	
							_AbiMenuCtrl.Instance.InitAbiObjInfo(_AbiMenuCtrl.Instance.CurrentDisciplineType);
						}else{
							_AbiMenuCtrl.Instance.AbiObjList[_AbiMenuCtrl.Instance.CurrentAbiObjIdx].ShowHighLight(false);
							HighLight.Hide(false);
							_AbiMenuCtrl.Instance.HighLightCurrentAbiBar(true);	
							_AbiMenuCtrl.Instance.CurrentAbiObjIdx = ListID;
							_AbiMenuCtrl.Instance.isOperate = true;
						}
					}else{
						_AbiMenuCtrl.Instance.AbiObjList[_AbiMenuCtrl.Instance.CurrentAbiObjIdx].ShowHighLight(false);
						_AbiMenuCtrl.Instance.HighLightCurrentAbiBar(false);	
						_AbiMenuCtrl.Instance.CurrentAbiObjIdx = ListID;
						_AbiMenuCtrl.Instance.isOperate = false;
						_AbiMenuCtrl.Instance.isBarOperate = false;
					}
					_AbiMenuCtrl.Instance.isBarOperate = false;
				break;
		   default:
				break;
		}	
	}
	
	public void HideObj(){
		equipBg.Hide(true);
		icon1.Hide(true);
		icon2.Hide(true);
		black.Hide(true);
		lockIcon.Hide(true);
		HighLight.Hide(true);
		isAbiEmpty = true;
		isAbiEquip = false;
	}
	
	public void ShowHighLight(bool isShow){
		if(isShow){
			HighLight.Hide(false);
		}else{
			HighLight.Hide(true);
		}
	}
	
	public void RightClick(){
		int idx = 0;
//		if(!isAbiEquip && !isAbiEmpty){
		if(!isAbiEquip){
			if(Input.GetButtonDown("Fire2")){
				idx = _AbiMenuCtrl.Instance.GetGroupEmptySlotIdx();
				LogManager.Log_Debug("--- SetSkillShortcut ---");
				CS_Main.Instance.g_commModule.SendMessage(
		   			ProtocolBattle_SendRequest.SetSkillShortcut(abilitiesInfo.m_AbilitieID,_AbiMenuCtrl.Instance.GroupIdx,idx)
				);
				SoundCue.PlayPrefabAndDestroy(_AbiMenuCtrl.Instance.SwapSound);
				_AbiMenuCtrl.Instance.ChangeAbiBarEleInfo(_AbiMenuCtrl.Instance.GroupIdx,idx,abilitiesInfo,false);
				_AbiMenuCtrl.Instance.SetCurrentGroup(_AbiMenuCtrl.Instance.GroupIdx);	
				_AbiMenuCtrl.Instance.InitAbiObjInfo(_AbiMenuCtrl.Instance.CurrentDisciplineType);
				_AbiMenuCtrl.Instance.isOperate 	= false;
				_AbiMenuCtrl.Instance.isBarOperate 	= false;
			}
		}
	}
	
}
