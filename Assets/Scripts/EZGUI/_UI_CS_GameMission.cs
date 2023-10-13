using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class _UI_CS_GameMission : MonoBehaviour {
	
//	public enum MissionType{
//	
//		TRAVEL = 1,
//		HUNT,
//		COLLECT,
//		PROTECT ,
//		SURVIVE ,
//		MEET,
//		MAX_TYPE
//	
//	}
	
	public static _UI_CS_GameMission Instance;
	
//	public  List<_UI_CS_Task> 	    TaskList    = new List<_UI_CS_Task>();
//	public  List<_UI_CS_RamusTask> 	RamusList   = new List<_UI_CS_RamusTask>();
	
	public UIPanel  MissionPanelBg;
	public UIButton MissionBg;
//	public UIButton IOMissionBtn;
	public bool	    IsIOMission = false;
	
//	public int      rectOffest = 5;
	
	public vectorInt unShowIDVector = new vectorInt();
	
//	public  UIListItemContainer  				m_ItemContainer;
//	public  List<_UI_CS_IngameMissionItem> 		m_ItemList   = new List<_UI_CS_IngameMissionItem>();
//	public  UIScrollList						m_List;
//	public  int 								m_CurrentIdx;
//	private Rect 					 			m_rect;
//	public  int 								m_count;
	
	public int 									m_MissionScore;
	public int 									m_MissionScoreCurrent;
	public int 									m_MissionScoreOffest;
	public SpriteText						 	m_MissionScoreText;
	public int 									m_Store_MisssionID; 
	public int 									m_Store_taskID; 
	public int 									m_Store_exp; 
	public int 									m_Store_karma;
	public bool 								m_Store_IsRestTask = false;
	
	public bool 								m_Store_IstMissionFinish = false;

	public SpriteText						 	m_TaskNameText;
	
	
	void Awake(){
		Instance = this;
	}

	// Use this for initialization
	void Start () {
//		m_CurrentIdx = 0;
//		m_rect.width = 1;
//		m_rect.height = 1;
//		m_count = 0;
		//m_MissionListPos = m_MissionListObj.transform.position;
		
//		IOMissionBtn.AddInputDelegate(IOMissionBtnDelegate);
		
	}
	
	// Update is called once per frame
	void Update () {
		
		if(m_MissionScoreCurrent < m_MissionScore){
			
			m_MissionScoreCurrent += m_MissionScoreOffest;
			
			if(m_MissionScoreCurrent > m_MissionScore){
				m_MissionScoreCurrent = m_MissionScore;
			}
			
			m_MissionScoreText.Text = m_MissionScoreCurrent.ToString();
			
		}
		
	}
	
//	void IOMissionBtnDelegate(ref POINTER_INFO ptr)
//	{
//		switch(ptr.evt)
//		{
//		   case POINTER_INFO.INPUT_EVENT.TAP:
//				if(IsIOMission){
//					IsIOMission = false;
//					MissionPanelBg.Dismiss();
//				}else{
//					IsIOMission = true;
//					MissionPanelBg.BringIn();
//				}
//				break;
//		   default:
//				break;
//		}	
//	}
	
	public bool IsUnShowID(int id){
	
		for(int i = 0;i<unShowIDVector.Count;i++){
			if((int)unShowIDVector[i] == id){
				return true;
			}
		}
		return false;
		
	}
	
	public void RsetMissionScore(){
		m_MissionScoreCurrent = 0;
		m_MissionScore = 0;
		m_MissionScoreText.Text = "0";
	}
	
	public void SetMissionScore(int score){
		
		m_MissionScore += score;
		
	}
	
//	public void AddElement(_UI_CS_IngameMissionItem element){
//		_UI_CS_IngameMissionItem temp = new _UI_CS_IngameMissionItem();
//		temp = element;
//		m_ItemList.Add(temp);
//	}
	
	//更新任务列表
//	public void InitTaskList(int missionID){
//
//		SetTask(missionID);
//		
//		SetRamus(missionID,0);
//		
//		UpdateMissionBG();
//		
//	}
	
//	public int SetRamus(int missionID,int taskIdx){
		
//		RamusList.Clear();
//		
//		for(int i = 0;i<_UI_CS_MapInfo.Instance.Itemlist[_UI_CS_BountyMaster.Instance.CurrentIdx].levelList.Length;i++){
//			
//			if(missionID == _UI_CS_MapInfo.Instance.Itemlist[_UI_CS_BountyMaster.Instance.CurrentIdx].levelList[i].ID){
//		
//				if(taskIdx >= _UI_CS_MapInfo.Instance.Itemlist[_UI_CS_BountyMaster.Instance.CurrentIdx].levelList[i].taskArray.Length){
//					
//					m_CurrentIdx = 0;
//					m_ItemList.Clear();
//					m_List.ClearList(false);	
//
//					return 0;
//					
//				}else{
//				
//					for(int k =0;k<_UI_CS_MapInfo.Instance.Itemlist[_UI_CS_BountyMaster.Instance.CurrentIdx].levelList[i].taskArray[taskIdx].SubObject.Length;k++){
//								
//						m_TaskNameText.Text = _UI_CS_MapInfo.Instance.Itemlist[_UI_CS_BountyMaster.Instance.CurrentIdx].levelList[i].taskArray[taskIdx].TaskName;
//						
//						RamusList.Add(_UI_CS_MapInfo.Instance.Itemlist[_UI_CS_BountyMaster.Instance.CurrentIdx].levelList[i].taskArray[taskIdx].SubObject[k]);
//						
//					}
//				}
//			}
//		}
//		
//		m_CurrentIdx = 0;
//		m_ItemList.Clear();
//		m_List.ClearList(false);
//		
//		for(int m = 0;m<RamusList.Count;m++){
//			
//			_UI_CS_IngameMissionItem temp = new _UI_CS_IngameMissionItem();
//			temp.m_type = (int)RamusList[m].typeID;
//			temp.m_name = RamusList[m].name;
//			temp.m_description = RamusList[m].description;
//			temp.m_maxVal = RamusList[m].count;
//			temp.m_objID = RamusList[m].objectID;
//			temp.m_recycle = RamusList[m].recycle;
//			temp.m_missionID = missionID;
//			temp.m_taskID = (taskIdx +1);
//			
//			AddElement(temp);
//		}
//		
//		Init_ItemList();
//
//		return 1;
//		
//		
//	}
	
//	public void UpdateMissionBG(){
//		
//		if(0 < m_List.Count){
//			
////			if(!IsIOMission){
////				MissionPanelBg.BringIn();
////			}
//			IsIOMission = true;
//			MissionBg.height = 2.2f * m_List.Count;
//		}else{
//			if(IsIOMission){
//				MissionPanelBg.Dismiss();
//			}
//			IsIOMission = false;
//		}
//		
//	}
	
//	public void SetTask(int missionID){
//		
//		TaskList.Clear();
//		
//		for(int i = 0;i<_UI_CS_MapInfo.Instance.Itemlist[_UI_CS_BountyMaster.Instance.CurrentIdx].levelList.Length;i++){
//			
//			if(missionID == _UI_CS_MapInfo.Instance.Itemlist[_UI_CS_BountyMaster.Instance.CurrentIdx].levelList[i].ID){
//				
//				for(int j =0;j<_UI_CS_MapInfo.Instance.Itemlist[_UI_CS_BountyMaster.Instance.CurrentIdx].levelList[i].taskArray.Length;j++){
//						
//					TaskList.Add(_UI_CS_MapInfo.Instance.Itemlist[_UI_CS_BountyMaster.Instance.CurrentIdx].levelList[i].taskArray[j]);
//					
//					
//				}
//			}
//		}
//		
//	}
	
	//初始化列表 
//	public void Init_ItemList(){
//
//		m_count = m_ItemList.Count;
//		
//		for(int j =0;j<m_count;j++){	
//			
//			AddItemListChild();
//				
//		}
//	}
	
//	public void CheckMissionLogic(MissionType type,int objID,int recycle){
//		
//		
//		//kill mission
//		if(type == MissionType.HUNT){
//			for(int i=0;i<m_List.Count;i++){
//				UIListItemContainer item =  (UIListItemContainer)m_List.GetItem(i);
//				
//					
//				if(!item.GetComponent<_UI_CS_IngameMissionRawItemCtrl>()
//				.item[0].transform.GetComponent<_UI_CS_IngameMissionItemEx>()
//				.Iscomplete
//				 && (int)MissionType.HUNT == item.GetComponent<_UI_CS_IngameMissionRawItemCtrl>()
//				.item[0].transform.GetComponent<_UI_CS_IngameMissionItemEx>()
//			    .m_missionInfo.m_type){
//				
//					if(objID == item.GetComponent<_UI_CS_IngameMissionRawItemCtrl>()
//					.item[0].transform.GetComponent<_UI_CS_IngameMissionItemEx>()
//					.m_missionInfo.m_objID){
//				
//					
//						item.GetComponent<_UI_CS_IngameMissionRawItemCtrl>()
//						.item[0].transform.GetComponent<_UI_CS_IngameMissionItemEx>()
//						.current_val++;
//	
//						item.GetComponent<_UI_CS_IngameMissionRawItemCtrl>()
//						.item[0].transform.GetComponent<_UI_CS_IngameMissionItemEx>()
//						.m_Val.Text = (item.GetComponent<_UI_CS_IngameMissionRawItemCtrl>()
//						.item[0].transform.GetComponent<_UI_CS_IngameMissionItemEx>()
//						.current_val.ToString() + " / " + item.GetComponent<_UI_CS_IngameMissionRawItemCtrl>()
//						.item[0].transform.GetComponent<_UI_CS_IngameMissionItemEx>().max.ToString());
//						
//						
//						//判断是否完成
//						if(item.GetComponent<_UI_CS_IngameMissionRawItemCtrl>()
//						.item[0].transform.GetComponent<_UI_CS_IngameMissionItemEx>()
//						.current_val >= item.GetComponent<_UI_CS_IngameMissionRawItemCtrl>()
//						.item[0].transform.GetComponent<_UI_CS_IngameMissionItemEx>().max){
//	
//								item.GetComponent<_UI_CS_IngameMissionRawItemCtrl>()
//								.item[0].transform.GetComponent<_UI_CS_IngameMissionItemEx>()
//								.Iscomplete = true;
//								
//								item.GetComponent<_UI_CS_IngameMissionRawItemCtrl>()
//								.item[0].transform.GetComponent<_UI_CS_IngameMissionItemEx>()
//								.listIdx = i;
//								
//								item.GetComponent<_UI_CS_IngameMissionRawItemCtrl>()
//								.item[0].transform.GetComponent<_UI_CS_IngameMissionItemEx>()
//								.missionCompletePanel.transform.position = new Vector3(item.transform.position.x -4.2f,item.transform.position.y-0.22f,item.transform.position.z);
//								item.GetComponent<_UI_CS_IngameMissionRawItemCtrl>()
//								.item[0].transform.GetComponent<_UI_CS_IngameMissionItemEx>()
//								.missionCompletePanel.BringIn();
//							
//						}
//					}
//				}
//			}
//		}
//		
//		//travel
//		if(type == MissionType.TRAVEL){
//			for(int i=0;i<m_List.Count;i++){
//				UIListItemContainer item =  (UIListItemContainer)m_List.GetItem(i);
//				
//				if(!item.GetComponent<_UI_CS_IngameMissionRawItemCtrl>()
//				.item[0].transform.GetComponent<_UI_CS_IngameMissionItemEx>()
//				.Iscomplete 
//				&& (int)MissionType.TRAVEL == item.GetComponent<_UI_CS_IngameMissionRawItemCtrl>()
//				.item[0].transform.GetComponent<_UI_CS_IngameMissionItemEx>()
//				.m_missionInfo.m_type){
//				
//					if(
//					   
//						objID >= item.GetComponent<_UI_CS_IngameMissionRawItemCtrl>()
//						.item[0].transform.GetComponent<_UI_CS_IngameMissionItemEx>()
//						.m_missionInfo.m_objID - rectOffest
//						   
//						&& objID <= item.GetComponent<_UI_CS_IngameMissionRawItemCtrl>()
//						.item[0].transform.GetComponent<_UI_CS_IngameMissionItemEx>()
//						.m_missionInfo.m_objID + rectOffest
//						
//						&&recycle >= item.GetComponent<_UI_CS_IngameMissionRawItemCtrl>()
//						.item[0].transform.GetComponent<_UI_CS_IngameMissionItemEx>()
//						.m_missionInfo.m_recycle - rectOffest 
//						   
//					    &&recycle <= item.GetComponent<_UI_CS_IngameMissionRawItemCtrl>()
//						.item[0].transform.GetComponent<_UI_CS_IngameMissionItemEx>()
//						.m_missionInfo.m_recycle + rectOffest 
//
//					  ){
//					
//						
//							CS_Main.Instance.g_commModule.SendMessage(
//				   					ProtocolGame_SendRequest.exploretaskcompleteReq(item.GetComponent<_UI_CS_IngameMissionRawItemCtrl>()
//									.item[0].transform.GetComponent<_UI_CS_IngameMissionItemEx>()
//									.m_missionInfo.m_missionID,
//						            item.GetComponent<_UI_CS_IngameMissionRawItemCtrl>()
//									.item[0].transform.GetComponent<_UI_CS_IngameMissionItemEx>()
//									.m_missionInfo.m_taskID,
//						            item.GetComponent<_UI_CS_IngameMissionRawItemCtrl>()
//									.item[0].transform.GetComponent<_UI_CS_IngameMissionItemEx>()
//									.m_missionInfo.m_type,
//						            item.GetComponent<_UI_CS_IngameMissionRawItemCtrl>()
//									.item[0].transform.GetComponent<_UI_CS_IngameMissionItemEx>()
//									.m_missionInfo.m_objID)
//							);
//						
//						
//						
//							//临时
//							item.GetComponent<_UI_CS_IngameMissionRawItemCtrl>() 
//							.item[0].transform.GetComponent<_UI_CS_IngameMissionItemEx>()
//							.Iscomplete = true;
//							
//							item.GetComponent<_UI_CS_IngameMissionRawItemCtrl>()
//							.item[0].transform.GetComponent<_UI_CS_IngameMissionItemEx>()
//							.listIdx = i;
//							
//							item.GetComponent<_UI_CS_IngameMissionRawItemCtrl>()
//							.item[0].transform.GetComponent<_UI_CS_IngameMissionItemEx>()
//							.missionCompletePanel.transform.position = new Vector3(item.transform.position.x -4.2f,item.transform.position.y-0.22f,item.transform.position.z);
//							item.GetComponent<_UI_CS_IngameMissionRawItemCtrl>()
//							.item[0].transform.GetComponent<_UI_CS_IngameMissionItemEx>()
//							.missionCompletePanel.BringIn();
//						
//					  }
//				}
//			}
//		}
//		
//		//collect
//		if(type == MissionType.COLLECT){
//			
//			//return;
//			
//			for(int i=0;i<m_List.Count;i++){
//				UIListItemContainer item =  (UIListItemContainer)m_List.GetItem(i);
//				
//				if(!item.GetComponent<_UI_CS_IngameMissionRawItemCtrl>()
//				.item[0].transform.GetComponent<_UI_CS_IngameMissionItemEx>()
//				.Iscomplete
//				 && (int)MissionType.COLLECT == item.GetComponent<_UI_CS_IngameMissionRawItemCtrl>()
//				.item[0].transform.GetComponent<_UI_CS_IngameMissionItemEx>()
//				.m_missionInfo.m_type){
//				
//					if(
//
//					    objID == item.GetComponent<_UI_CS_IngameMissionRawItemCtrl>()
//						.item[0].transform.GetComponent<_UI_CS_IngameMissionItemEx>()
//						.m_missionInfo.m_objID
//
//						&&recycle == item.GetComponent<_UI_CS_IngameMissionRawItemCtrl>()
//						.item[0].transform.GetComponent<_UI_CS_IngameMissionItemEx>()
//						.m_missionInfo.m_recycle
//					   
//					   
//					  ){
//							
//							if(item.GetComponent<_UI_CS_IngameMissionRawItemCtrl>().item[0].transform.GetComponent<_UI_CS_IngameMissionItemEx>().current_val
//						   		< item.GetComponent<_UI_CS_IngameMissionRawItemCtrl>().item[0].transform.GetComponent<_UI_CS_IngameMissionItemEx>()
//								.m_missionInfo.m_maxVal-1){
//							
//								item.GetComponent<_UI_CS_IngameMissionRawItemCtrl>()
//								.item[0].transform.GetComponent<_UI_CS_IngameMissionItemEx>()
//								.current_val++;
//							
//								item.GetComponent<_UI_CS_IngameMissionRawItemCtrl>().item[0].transform.GetComponent<_UI_CS_IngameMissionItemEx>().m_Val.Text 
//								= item.GetComponent<_UI_CS_IngameMissionRawItemCtrl>().item[0].transform.GetComponent<_UI_CS_IngameMissionItemEx>().current_val.ToString() 
//								+ " / " + item.GetComponent<_UI_CS_IngameMissionRawItemCtrl>().item[0].transform.GetComponent<_UI_CS_IngameMissionItemEx>()
//								.m_missionInfo.m_maxVal.ToString();
//									
//							
//							}else{
//						
//						
//								item.GetComponent<_UI_CS_IngameMissionRawItemCtrl>() 
//								.item[0].transform.GetComponent<_UI_CS_IngameMissionItemEx>()
//								.Iscomplete = true;
//								
//								item.GetComponent<_UI_CS_IngameMissionRawItemCtrl>()
//								.item[0].transform.GetComponent<_UI_CS_IngameMissionItemEx>()
//								.listIdx = i;
//								
//								item.GetComponent<_UI_CS_IngameMissionRawItemCtrl>()
//								.item[0].transform.GetComponent<_UI_CS_IngameMissionItemEx>()
//								.missionCompletePanel.transform.position = new Vector3(item.transform.position.x -4.2f,item.transform.position.y-0.22f,item.transform.position.z);
//								item.GetComponent<_UI_CS_IngameMissionRawItemCtrl>()
//								.item[0].transform.GetComponent<_UI_CS_IngameMissionItemEx>()
//								.missionCompletePanel.BringIn();
//							
//							}
//						
//					  }
//				}
//			}
//		}
//		
//		
//		
//		
//	}
	
	
//	public void AddItemListChild(){
//		
//		UIListItemContainer item;
//
//		item = (UIListItemContainer)m_List.CreateItem((GameObject)m_ItemContainer.gameObject);
//		//Reset after manipulations
//		m_List.clipContents 	= true;
//		m_List.clipWhenMoving 	= true;
//		Calculate();
//		
//		item.transform.GetComponent<_UI_CS_IngameMissionRawItemCtrl>()
//		.item[0].transform.GetComponent<_UI_CS_IngameMissionItemEx>()
//		.m_missionInfo = m_ItemList[m_CurrentIdx];
//		
//		item.transform.GetComponent<_UI_CS_IngameMissionRawItemCtrl>()
//		.item[0].transform.GetComponent<_UI_CS_IngameMissionItemEx>()
//		.m_ListID = m_CurrentIdx;
//		
//		//item.transform.GetComponent<_UI_CS_IngameMissionRawItemCtrl>().item[0].m_name.Text = m_ItemList[m_CurrentIdx].m_name.ToString();
//		item.transform.GetComponent<_UI_CS_IngameMissionRawItemCtrl>().item[0].m_description.Text = m_ItemList[m_CurrentIdx].m_description.ToString();
//		
//		item.transform.GetComponent<_UI_CS_IngameMissionRawItemCtrl>().item[0].max = m_ItemList[m_CurrentIdx].m_maxVal;
//		item.transform.GetComponent<_UI_CS_IngameMissionRawItemCtrl>().item[0].type = m_ItemList[m_CurrentIdx].m_type;
//		
//		item.transform.GetComponent<_UI_CS_IngameMissionRawItemCtrl>().item[0].m_recycle = m_ItemList[m_CurrentIdx].m_recycle;
//		
//		if((int)MissionType.HUNT == m_ItemList[m_CurrentIdx].m_type || (int)MissionType.COLLECT == m_ItemList[m_CurrentIdx].m_type){
//			
//			item.transform.GetComponent<_UI_CS_IngameMissionRawItemCtrl>().item[0].m_Val.Text = "0" + " / " + m_ItemList[m_CurrentIdx].m_maxVal.ToString();
//			
//		}
//
////		item.transform.GetComponent<_UI_CS_SpiritHelperItem>().item[0].m_iconButton.SetUVs(m_rect);
////		item.transform.GetComponent<_UI_CS_SpiritHelperItem>().item[0].m_iconButton.SetTexture(
////		_UI_CS_Resource.Instance.m_AccomplishmentIcon[m_SHItemList[m_CurrentIdx].m_iconID]);
//
//		m_CurrentIdx++;
//		if(m_CurrentIdx >=  m_ItemList.Count){
//			m_CurrentIdx = 0;
//			//LogManager.Log_Debug("m_CurrentIdx >=  m_ItemList.Count");
//		}
//
//	}
	
//	public void Calculate()
//	{
//		if (m_List != null && m_List.slider != null)
//        {
//            // Ask scroll list to position items
//            m_List.PositionItems();
//
//            // Var to hold new knob size
//            Vector2 newKnobSize;
//
//            // Determine the new knob size as a percentage of the size of the viewable area
//            // If the content is smaller than the viewable size then we won't show a knob
//            if (m_List.ContentExtents > m_List.viewableArea.y)
//            {
//                float ratio = m_List.ContentExtents / m_List.viewableArea.y;
//                newKnobSize = new Vector2((m_List.slider.width / ratio), m_List.slider.knobSize.y);
//				m_List.slider.Hide(false);
//            }
//            else
//            {
//                newKnobSize = new Vector2(0f, 0f);
//				m_List.slider.Hide(true);
//            }
//
//            // Get a handle to the knob so we can change it
//            UIScrollKnob theKnob = m_List.slider.GetKnob();
//            //Debug.Log(theKnob);
//            // Set the knob size based on our previous calculation
//            theKnob.SetSize(newKnobSize.x, newKnobSize.y);
//
//            // Now we need to make sure the knob doesn't go past the ends of the scrollview window size
//            m_List.slider.stopKnobFromEdge = newKnobSize.x / 2;
//            //Vector3 newStartPos = m_IngameMenu_AbilitiesTempList.slider.CalcKnobStartPos();
//			Vector3 newStartPos = m_List.slider.CalcKnobStartPos();
//            theKnob.SetStartPos(newStartPos);
//            theKnob.SetMaxScroll(m_List.slider.width - (m_List.slider.stopKnobFromEdge * 2f));
//
//            // Make sure the new text is scrolled to the top of the viewable area
//            m_List.ScrollListTo(0f);
//            // Added by me.
//            theKnob.SetPosition(0f);
//        }
//	}
	
}
