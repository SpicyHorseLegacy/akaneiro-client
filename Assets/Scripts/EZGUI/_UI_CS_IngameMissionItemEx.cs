using UnityEngine;
using System.Collections;

public class _UI_CS_IngameMissionItemEx : MonoBehaviour {

	public _UI_CS_IngameMissionItem m_missionInfo;
	
	private Rect  m_rect;
	public int    m_ListID;
	
	public UIButton m_BgButton;
	//public SpriteText m_name;
	public SpriteText m_description;
	public SpriteText m_Val;
	public int type;
	public int max;
	public int current_val;
	public bool Iscomplete = false;
	public int listIdx = 0;
	public int m_recycle;
	
//	public UIPanel missionCompletePanel;
	
	// Use this for initialization
	void Start () {
		m_rect.width = 1;
		m_rect.height = 1;
		current_val = 0;
//		missionCompletePanel.AddTempTransitionDelegate(MissionTextCompleteDelegate);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void MissionTextCompleteDelegate(UIPanelBase panel, EZTransition trans){
		
		//panel.transform.position = new Vector3(1000,1000,1000);	
		
//		if(_UI_CS_GameMission.Instance != null){
//		
//			//删除链表item 
//			_UI_CS_GameMission.Instance.m_List.RemoveItem(listIdx,false);
//
//			if(_UI_CS_GameMission.Instance.m_Store_IsRestTask){
//				
//				_UI_CS_GameMission.Instance.SetRamus(_UI_CS_GameMission.Instance.m_Store_MisssionID,_UI_CS_GameMission.Instance.m_Store_taskID);
//				_UI_CS_GameMission.Instance.SetMissionScore(_UI_CS_GameMission.Instance.m_Store_exp);
//				_UI_CS_GameMission.Instance.m_Store_IsRestTask = false;
//			}
//			
//			_UI_CS_GameMission.Instance.UpdateMissionBG();
//			
//			if(_UI_CS_GameMission.Instance.m_Store_IstMissionFinish){
//	
//				CS_SceneInfo.Instance.KillAllEnemy();
//				
//				if(!_UI_CS_Wanted.Instance.isWanted){
//					
//					_UI_CS_GameMission.Instance.m_Store_IstMissionFinish = false;
//					//Mission 完成面板
//					//Time.timeScale = 0;	
//					//_UI_CS_MissionSummary.Instance.AwakeCompleteSummary();
//					_UI_CS_MissionSummary.Instance.isCompleteMission = true;
//					_UI_CS_Teleport.Instance.bgPanel.BringIn();
//					
//				}
//
//			}
//
//		}
		
	}
	
}
