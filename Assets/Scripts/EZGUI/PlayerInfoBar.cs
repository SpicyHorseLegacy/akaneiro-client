using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerInfoBar : MonoBehaviour {
	
	public static PlayerInfoBar Instance;
	public Transform	RootObj;
	public Transform [] objPos;
	public ObjStatePrefab playerObj;
	public List<ObjStatePrefab> allyObjList = new List<ObjStatePrefab>();
	public ObjStatePrefab statePrefab;
	public UIButton SommonBtn;
	private int updateCtrlCount = 10;
	private int updateCtrlidx = 0;
	public UIButton levelCap;
	public bool		isLevelCap = false;
	private Vector3 	mousePos;
	
	void Awake(){
		Instance = this;
	}
	
	// Use this for initialization
	void Start () {
		UpdateAllyPos();
		SommonBtn.AddInputDelegate(SommonBtnDelegate);
		levelCap.AddInputDelegate(LevelCapDelegate);
		levelCap.Hide(true);
	}
	
	// Update is called once per frame
	void Update () {
		updateCtrlidx++;
		if(updateCtrlidx > updateCtrlCount){
			updateCtrlidx = 0;
			UpdateAllyStateHp();
		}
	}
	
	void LevelCapDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
 			case POINTER_INFO.INPUT_EVENT.MOVE:	
		    case POINTER_INFO.INPUT_EVENT.NO_CHANGE:
				{
					mousePos = UIManager.instance.uiCameras[0].camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y,UIManager.instance.uiCameras[0].camera.nearClipPlane + 1));
					LevelCapTip.Instance.ShowLevelCapTip(false,mousePos,levelCap.width,levelCap.height);
				}
				break;
		   case POINTER_INFO.INPUT_EVENT.MOVE_OFF:
		   case POINTER_INFO.INPUT_EVENT.RELEASE:
		   case POINTER_INFO.INPUT_EVENT.RELEASE_OFF:
				{
					LevelCapTip.Instance.DismissTip();
				}
				break;
		}
	}
	
	void UpdateAllyStateHp(){
		if(null != CS_SceneInfo.Instance){
			for(int i=0;i<CS_SceneInfo.Instance.AllyNpcList.Count;i++){
				if(null != allyObjList[i] && null != CS_SceneInfo.Instance.AllyNpcList[i]){
					allyObjList[i].hpBar.Value = (float)CS_SceneInfo.Instance.AllyNpcList[i].AttrMan.Attrs[EAttributeType.ATTR_CurHP] / CS_SceneInfo.Instance.AllyNpcList[i].AttrMan.Attrs[EAttributeType.ATTR_MaxHP];
					allyObjList[i].enBar.Value = (float)CS_SceneInfo.Instance.AllyNpcList[i].AttrMan.Attrs[EAttributeType.ATTR_CurMP] / CS_SceneInfo.Instance.AllyNpcList[i].AttrMan.Attrs[EAttributeType.ATTR_MaxMP];
				}
			}
		}
	}
	
	void SommonBtnDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:
				_UI_CS_Summon.Instance.AwakeSummon();
				break;
		}	
	}
	
	public void UpdatePlayerLevel(int level){
		playerObj.level.Text = level.ToString();
	}
	
	public void InitPlayerState(int style,ESex sex){
		playerObj.icon.SetUVs(new Rect(0,0,1,1));
		playerObj.icon.SetTexture(_PlayerData.Instance.GetPlayerIcon(style,sex.Get()));
		playerObj.ClearBuff();
	}
	
	public void InitAllyState(ObjStatePrefab obj,int style,ESex sex,int level){
		obj.icon.SetUVs(new Rect(0,0,1,1));
		obj.icon.SetTexture(_PlayerData.Instance.GetPlayerIcon(style,sex.Get()));
//		obj.level.Text = "";
		obj.ClearBuff();
	}
	
	public void UpdatePlayerBuffState(){
		playerObj.UpdateBuffState(Player.Instance.BuffMan);
		UpdateAllyState();
	}
	
	public void UpdateBuffState(){
		//Debug.LogError("update buff");
		UpdatePlayerBuffState();
	}
	
	public void UpdateAllyState(){
		int i = 0;
		if(null != CS_SceneInfo.Instance){
			for(i=0;i<CS_SceneInfo.Instance.AllyNpcList.Count;i++){
				if(null != allyObjList[i] && null != CS_SceneInfo.Instance.AllyNpcList[i]){
					allyObjList[i].UpdateBuffState(CS_SceneInfo.Instance.AllyNpcList[i].BuffMan);
				}
			}
		}
	}
	
	public void IsHideSommonAllyBtn(bool isHide,int posIdx){
		StartCoroutine(SommonBtnCallBack(isHide,posIdx));
	}
	
	private IEnumerator SommonBtnCallBack(bool isHide,int posIdx){
		yield return null;
		if(isHide){
			SommonBtn.transform.position = new Vector3(999f,999f,999f);
		}else{
			SommonBtn.transform.position = objPos[posIdx].position;
		}
	}
	
	private IEnumerator AllyIconCallBack(){
		yield return null;
		int allyCount = allyObjList.Count;
		for(int i=0;i<allyCount;i++){
			allyObjList[i].transform.position = objPos[i+1].position;
		}
		if(0 == allyCount){
			if(_UI_CS_MapScroll.Instance.IsExistMission){
				SommonBtn.transform.position = new Vector3(999f,999f,999f);
//				IsHideSommonAllyBtn(true,0);
			}else{
				SommonBtn.transform.position = objPos[1].position;
//				IsHideSommonAllyBtn(false,1);
			}
		}else{
			SommonBtn.transform.position = new Vector3(999f,999f,999f);
//			IsHideSommonAllyBtn(true,0);
		}
	}
	
	public void AddNpc(ObjStatePrefab obj){
		allyObjList.Add(obj);
		obj.transform.parent = RootObj;
		UpdateAllyPos();
	}
	
	public void RemoveNpc(ObjStatePrefab obj){
		obj.transform.position = new Vector3(999,999,999);
		//todo: ! destory wait a bit destory.so can first.
		Destroy(obj.gameObject);
		allyObjList.Remove(obj);
		UpdateAllyPos();	
	}
	
	public void ClearNpcState(){
		for(int i = 0;i<allyObjList.Count;i++){
			//todo: ! destory wait a bit destory.so can first.
			Destroy(allyObjList[i].gameObject);
			allyObjList.Remove(allyObjList[i]);
		}
		UpdateAllyPos();	
	}
	
	public void UpdateAllyPos(){
		StartCoroutine(AllyIconCallBack());
	}
	
	public void ClearAllAllyState(){
		int allyCount = allyObjList.Count;
		for(int i = 0;i<allyCount;i++){
			Destroy(allyObjList[i].gameObject);
		}
		allyObjList.Clear();
		
	}
}
