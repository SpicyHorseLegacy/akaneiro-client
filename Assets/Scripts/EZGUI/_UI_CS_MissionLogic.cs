using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class _UI_CS_MissionLogic : MonoBehaviour {
	
	public static _UI_CS_MissionLogic Instance;
	
	public enum MissionType{
	
		TRAVEL = 1,
		HUNT,
		COLLECT,
		PROTECT ,
		SURVIVE ,
		INTERACT,
		MEET,
		DESTORY,
		MAX_TYPE
	
	}
	
	public GameObject CurrentMissionObj;
	
	public  Transform  							BranchItemPrefab;
	public  Transform  							TrackItemPrefab;
	public  Transform  							RootItemPrefab;
	
	private Transform 							root;
	
	public int									BranchCount 				= 0;
	public  List<int> 	    					TaskIdx    					= new List<int>();
	
	public  List<_UI_CS_Branch> 	    		BranchList   				= new List<_UI_CS_Branch>();
	
	private int 								TravelRange 				= 5;
	
	public  Transform 							MissionTitleObj;
	
	private int 	  							trackNoodCount 				= 0;
	
	public SpriteText							MissionNameText;
	
	public UIPanel  							MissionBgPanel;
	
//	[HideInInspector]
	public int 									MissionScore;
	public int 									MissionKarma;
//	[HideInInspector]
	public int 									CurrentMissionScore;
//	public int 									CurrentMissionKarma;
	public int 									MissionScoreIncremental;
	
	public SpriteText						 	MissionScoreText;
//	public SpriteText						 	MissionKarmaText;
	
	public bool 								IstMissionFinish 			= false;
	
	public vectorInt 							isOpenMissionIDVector 				= new vectorInt();
	
	public float 								missionTime = 0;
	public float 								StartMissionTime = 0;
	public SpriteText							missionTimeText;
	
//	public  List<Transform> 	    			TransformList   				= new List<Transform>();
	public Dictionary<int,Transform > TransformList = new Dictionary<int, Transform>();
	
	private Transform tans;
	
	void Awake(){
		
		Instance = this;
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
		UpdateMissionTime();
		UpdateTaskTime();
		
		//更新分数
		if(CurrentMissionScore < MissionScore){
			
			CurrentMissionScore += MissionScoreIncremental;
			
			if(CurrentMissionScore > MissionScore){
				CurrentMissionScore = MissionScore;
			}
			
			MissionScoreText.Text = CurrentMissionScore.ToString();
			
		}
		
//		if(CurrentMissionKarma < MissionKarma){
//			
//			CurrentMissionKarma += MissionScoreIncremental;
//			
//			if(CurrentMissionKarma > MissionKarma){
//				CurrentMissionKarma = MissionKarma;
//			}
//			
//			MissionKarmaText.Text = CurrentMissionKarma.ToString();
//			
//		}
		
	}
	
	
	
	public void SetMissionScore(int score){
		
		MissionScore += score;
		_UI_CS_DebugInfo.Instance.AddMissionXP(score);
		
	}
	
	
	public bool IsOpenMissionID(int id){
	
		for(int i = 0;i<isOpenMissionIDVector.Count;i++){
			if((int)isOpenMissionIDVector[i] == id){
				return true;
			}
		}
		return false;
		
	}
	
	//重置分数
	public void RsetMissionScore(){
		CurrentMissionScore = 0;
//		CurrentMissionKarma = 0;
		MissionScore = 0;
		MissionKarma = 0;
		MissionScoreText.Text = "0";
//		MissionKarmaText.Text = "0";
//		LogManager.Log_Error("RsetMissionScore");
	}
	
	//更新任务UI界面
	public void UpdateMissionUI(){
		int idx = 0;
		if( null != root )
			Destroy(root.gameObject);
		
		TransformList.Clear();
		
		trackNoodCount = 0;

		root = UnityEngine.Object.Instantiate(RootItemPrefab)as Transform;
		root.parent = MissionTitleObj.transform;
		
		root.transform.position = new Vector3(MissionTitleObj.transform.position.x,MissionTitleObj.transform.position.y,MissionTitleObj.transform.position.z);
		
		//生成Task节点
		for(int i = 0;i< BranchList.Count;i++){
		
			Transform Branch = UnityEngine.Object.Instantiate(BranchItemPrefab)as Transform;
			Branch.parent = root.transform;
			Branch.GetComponent<_UI_MissionBranchItem>().CurrentTaskName.Text = BranchList[i].BranchName;
			
			Branch.transform.position = new Vector3(root.transform.position.x,
													root.transform.position.y 
													- i * (Branch.GetComponent<_UI_MissionBranchItem>().BG.height )
													- (trackNoodCount) * (TrackItemPrefab.GetComponent<_UI_MissionTrackItem>().BG.height) ,
													root.transform.position.z);
			
			
			
			
			//生成Track节点
			for(int j = 0;j< BranchList[i].taskArray[TaskIdx[i]].SubObject.Count;j++){
				
				
				
				Transform Track = UnityEngine.Object.Instantiate(TrackItemPrefab)as Transform;
				Track.parent = Branch.transform;
				Track.GetComponent<_UI_MissionTrackItem>().Descripiton.Text = BranchList[i].taskArray[TaskIdx[i]].SubObject[j].description;
				if( 
					1 < (int)BranchList[i].taskArray[TaskIdx[i]].SubObject[j].count){
					
						BranchList[i].taskArray[TaskIdx[i]].SubObject[j].CurrentValToString = ( BranchList[i].taskArray[TaskIdx[i]].SubObject[j].CurrentVal.ToString() 
																								+ " / " 
																								+ BranchList[i].taskArray[TaskIdx[i]].SubObject[j].count.ToString() );
					
					if((int)MissionType.SURVIVE == (int)BranchList[i].taskArray[TaskIdx[i]].SubObject[j].typeID && 0 != BranchList[i].taskArray[TaskIdx[i]].SubObject[j].recycle){
					
						BranchList[i].taskArray[TaskIdx[i]].SubObject[j].CurrentValToString = "";
							
					}
					
				}else{
				
					BranchList[i].taskArray[TaskIdx[i]].SubObject[j].CurrentValToString = "";
				
				}
				
				Track.GetComponent<_UI_MissionTrackItem>().CurrentVal.Text  = BranchList[i].taskArray[TaskIdx[i]].SubObject[j].CurrentValToString;
				
				Track.transform.position = Branch.transform.position;
				
				Track.transform.position = new Vector3(root.transform.position.x -0.9f,
														root.transform.position.y 
														- i * (Branch.GetComponent<_UI_MissionBranchItem>().BG.height) -  1.15f
														- (trackNoodCount * ( Track.GetComponent<_UI_MissionTrackItem>().BG.height) ),
														root.transform.position.z);
				
				
				idx++;
				trackNoodCount++;
				
				TransformList.Add(idx,Track);
				
			}
			
		}
		
	}
	
	// init quest list
	public void InitMissionList(int missionID){
		
		if(null != CurrentMissionObj)
			Destroy(CurrentMissionObj.gameObject);
		
		//初始化branch数据结构
		for(int i = 0;i<_UI_CS_MapInfo.Instance.Itemlist[MissionPanel.Instance.GetCurrentRegionID()+1].levelList.Length;i++){

			if(missionID == _UI_CS_MapInfo.Instance.Itemlist[MissionPanel.Instance.GetCurrentRegionID()+1].levelList[i].ID){

				BranchCount = 	_UI_CS_MapInfo.Instance.Itemlist[MissionPanel.Instance.GetCurrentRegionID()+1].levelList[i].branchArray.Count;	
				
				CurrentMissionObj = UnityEngine.Object.Instantiate(_UI_CS_MapInfo.Instance.Itemlist[MissionPanel.Instance.GetCurrentRegionID()+1].levelList[i].gameObject)as GameObject;
				CurrentMissionObj.AddComponent("_UI_CS_DontDestroyOnLoad");
				
				BranchList.Clear();
				TransformList.Clear();
				
				if(null != CurrentMissionObj){

					for(int j = 0;j< CurrentMissionObj.GetComponent<_UI_CS_MapLevelItem>().branchArray.Count;j++){
					
						BranchList.Add(CurrentMissionObj.GetComponent<_UI_CS_MapLevelItem>().branchArray[j]);
						TaskIdx.Add(0);
						
					}	
					
					_UI_CS_DebugInfo.Instance.SetMissionInfo(true,CurrentMissionObj.GetComponent<_UI_CS_MapLevelItem>().ID%10,CurrentMissionObj.GetComponent<_UI_CS_MapLevelItem>().xp,CurrentMissionObj.GetComponent<_UI_CS_MapLevelItem>().sk);
						
				}else{
				
					LogManager.Log_Error("CurrentMissionObj == null.");
					
				}
				
			}
			
		}
		
		//生成任务UI界面
		UpdateMissionUI();
		
	}
	
	//完成track逻辑
	public void CompleteTrack(int branchIdx , int trackIdx){
		
		BranchList[branchIdx].taskArray[TaskIdx[branchIdx]].SubObject.Remove(BranchList[branchIdx].taskArray[TaskIdx[branchIdx]].SubObject[trackIdx]);
		
		//判断track是否完成
		if(0 == BranchList[branchIdx].taskArray[TaskIdx[branchIdx]].SubObject.Count){
		
			//增加Task经验
//			SetMissionScore(BranchList[branchIdx].taskArray[TaskIdx[branchIdx]].rewardContentExp);
			
			BranchList[branchIdx].taskArray.Remove(BranchList[branchIdx].taskArray[TaskIdx[branchIdx]]);
			
			//判断task 是否完成
			if(0 == BranchList[branchIdx].taskArray.Count){
			
				BranchList.Remove(BranchList[branchIdx]);
				TaskIdx.Remove(TaskIdx[branchIdx]);
				
				//判断任务是否完成
				if(0 == BranchList.Count){
					
					//增加Mission经验
					if(null != CurrentMissionObj){
					
//						SetMissionScore(CurrentMissionObj.GetComponent<_UI_CS_MapLevelItem>().xp);
						LogManager.Log_Warn(" C -->  MissionComplete. ");
						MissionBgPanel.Dismiss();
//						_UI_CS_Teleport.Instance.bgPanel.BringIn();
		
					}else{
					
						LogManager.Log_Error("add mission exp, but CurrentMissionObj is Null");
					
					}
					
				}
				
				
			}
			
		}
		
		//更新任务UI界面
		UpdateMissionUI();
		
	}
	
	// check mission process
	public void CheckMissionProgress(MissionType type,int objID,int recycle){
	
		if(null == BranchList){
			
			return;
			
		}
		
		//Hunt
		if(type == MissionType.HUNT){
			
			
			for(int i=0;i<BranchList.Count;i++){
			
				for(int j = 0;j< BranchList[i].taskArray[TaskIdx[i]].SubObject.Count;j++){
				
					//判断行为是否对任务进程有影响
					if((int)MissionType.HUNT == (int)BranchList[i].taskArray[TaskIdx[i]].SubObject[j].typeID 
						&& objID == BranchList[i].taskArray[TaskIdx[i]].SubObject[j].objectID){
					
						BranchList[i].taskArray[TaskIdx[i]].SubObject[j].CurrentVal++;
						
						
						//判断是否完成
						if(BranchList[i].taskArray[TaskIdx[i]].SubObject[j].CurrentVal >= BranchList[i].taskArray[TaskIdx[i]].SubObject[j].count){
							
							CompleteTrack(i,j);
							
							break;
						}
					
						//界面UI显示更新
						UpdateMissionUI();
					
					}
					
				}
				
			}

		}
		
		//DESTORY
		if(type == MissionType.DESTORY){
			
			
			for(int i=0;i<BranchList.Count;i++){
			
				for(int j = 0;j< BranchList[i].taskArray[TaskIdx[i]].SubObject.Count;j++){
				
					//Debug.LogError("-------------------------------------------------------------------");
					//Debug.LogError("typeID:"+(int)BranchList[i].taskArray[TaskIdx[i]].SubObject[j].typeID);
					//Debug.LogError("objectID:"+(int)BranchList[i].taskArray[TaskIdx[i]].SubObject[j].objectID);
					//Debug.LogError("-------------------------------------------------------------------");
					//判断行为是否对任务进程有影响
					if((int)MissionType.DESTORY == (int)BranchList[i].taskArray[TaskIdx[i]].SubObject[j].typeID 
						&& objID == BranchList[i].taskArray[TaskIdx[i]].SubObject[j].objectID){
					
						BranchList[i].taskArray[TaskIdx[i]].SubObject[j].CurrentVal++;
						
						
						//判断是否完成
						if(BranchList[i].taskArray[TaskIdx[i]].SubObject[j].CurrentVal >= BranchList[i].taskArray[TaskIdx[i]].SubObject[j].count){
							
							CompleteTrack(i,j);
							
							break;
						}
					
						//界面UI显示更新
						UpdateMissionUI();
					
					}
					
				}
				
			}

		}
		
		//Travel
		if(type == MissionType.TRAVEL){
			
//			return;
			
			for(int i=0;i<BranchList.Count;i++){
				
				for(int j = 0;j< BranchList[i].taskArray[TaskIdx[i]].SubObject.Count;j++){
				
					//判断行为是否对任务进程有影响
					if((int)MissionType.TRAVEL == (int)BranchList[i].taskArray[TaskIdx[i]].SubObject[j].typeID ){
						
						BranchList[i].taskArray[TaskIdx[i]].SubObject[j].CurrentValToString = "";
						
						if(
					   
							objID >= BranchList[i].taskArray[TaskIdx[i]].SubObject[j].objectID - TravelRange   
							&& objID <= BranchList[i].taskArray[TaskIdx[i]].SubObject[j].objectID + TravelRange
							&&recycle >= BranchList[i].taskArray[TaskIdx[i]].SubObject[j].recycle - TravelRange   
						    &&recycle <= BranchList[i].taskArray[TaskIdx[i]].SubObject[j].recycle + TravelRange 

					 	 ){
				
							
							// send reach infomation
							CS_Main.Instance.g_commModule.SendMessage(
				   					ProtocolGame_SendRequest.exploretaskcompleteReq(MissionPanel.Instance.currentMissionID,(int)BranchList[i].BranchID,(int)BranchList[i].taskArray[TaskIdx[i]].TaskID,(int)BranchList[i].taskArray[TaskIdx[i]].SubObject[j].typeID,BranchList[i].taskArray[TaskIdx[i]].SubObject[j].objectID)
																		);
							
							CompleteTrack(i,j);
							
							//界面UI显示更新
							UpdateMissionUI();
							
						}
					
					}
				
				}

			}
		
		}
		
		//PROTECT
		if(type == MissionType.PROTECT){
			
			for(int i=0;i<BranchList.Count;i++){
				
				for(int j = 0;j< BranchList[i].taskArray[TaskIdx[i]].SubObject.Count;j++){
				
					//判断行为是否对任务进程有影响
					if((int)MissionType.TRAVEL == (int)BranchList[i].taskArray[TaskIdx[i]].SubObject[j].typeID ){
						
						BranchList[i].taskArray[TaskIdx[i]].SubObject[j].CurrentValToString = "";
						
						if(
					   
							objID >= BranchList[i].taskArray[TaskIdx[i]].SubObject[j].objectID - TravelRange   
							&& objID <= BranchList[i].taskArray[TaskIdx[i]].SubObject[j].objectID + TravelRange
							&&recycle >= BranchList[i].taskArray[TaskIdx[i]].SubObject[j].recycle - TravelRange   
						    &&recycle <= BranchList[i].taskArray[TaskIdx[i]].SubObject[j].recycle + TravelRange 

					 	 ){
						
							CompleteTrack(i,j);
							//发送护送成功消息
							//...  ...
							//界面UI显示更新
							UpdateMissionUI();
						}
					
					}
				
				}

			}
		
		}
		
		//Collect
		if(type == MissionType.COLLECT){
			for(int i=0;i<BranchList.Count;i++){
				
				for(int j = 0;j< BranchList[i].taskArray[TaskIdx[i]].SubObject.Count;j++){
				
					//判断行为是否对任务进程有影响
					if((int)MissionType.COLLECT == (int)BranchList[i].taskArray[TaskIdx[i]].SubObject[j].typeID 
						&& objID == BranchList[i].taskArray[TaskIdx[i]].SubObject[j].objectID
						&& recycle == BranchList[i].taskArray[TaskIdx[i]].SubObject[j].recycle){
					
						BranchList[i].taskArray[TaskIdx[i]].SubObject[j].CurrentVal++;
						
						//判断是否完成
						if(BranchList[i].taskArray[TaskIdx[i]].SubObject[j].CurrentVal >= BranchList[i].taskArray[TaskIdx[i]].SubObject[j].count){
						
							CompleteTrack(i,j);
							
						}
				
						//界面UI显示更新
						UpdateMissionUI();
						
					}
					
				}
				
			}

		}
		
		//Survive
		if(type == MissionType.SURVIVE){
			
			
			for(int i=0;i<BranchList.Count;i++){
			
				for(int j = 0;j< BranchList[i].taskArray[TaskIdx[i]].SubObject.Count;j++){
				
					//判断行为是否对任务进程有影响
					if((int)MissionType.SURVIVE == (int)BranchList[i].taskArray[TaskIdx[i]].SubObject[j].typeID){
					
						BranchList[i].taskArray[TaskIdx[i]].SubObject[j].CurrentVal++;
						
						//判断是否完成
						if( 0 == BranchList[i].taskArray[TaskIdx[i]].SubObject[j].count ){
							
							if(BranchList[i].taskArray[TaskIdx[i]].CurrentPassimeVal >= BranchList[i].taskArray[TaskIdx[i]].SubObject[j].recycle){
								
								CompleteTrack(i,j);
								
								return;
							}
							
							
						}else{
						
							if(BranchList[i].taskArray[TaskIdx[i]].SubObject[j].CurrentVal >= BranchList[i].taskArray[TaskIdx[i]].SubObject[j].count){
								
								CompleteTrack(i,j);
								
								return;
							}
							
	
						}
						
						//界面UI显示更新
						UpdateMissionUI();
					
					}
					
				}
				
			}

		}
	}
	
	public void RestMissionTime(){
	
		missionTime = 0;
		missionTimeText.Text = "00:00";	
		StartMissionTime = Time.time;
		
	}
	
	public void UpdateMissionTime(){

		int m = (int)( missionTime / 60 );
		int s = (int)( missionTime % 60 );
		
		string timeM = "";
		string timeS = "";
		
		if(_UI_CS_MapScroll.Instance.IsExistMission){
		
			missionTime = ( Time.time - StartMissionTime );
				
			if(m < 10){
				
				timeM = "0" + m.ToString();
			
			}else{
				
				timeM = m.ToString();
			
			}
			
			if(s < 10){
				
				timeS = "0" + s.ToString();
			
			}else{
				
				timeS = s.ToString();
			
			}
			
			missionTimeText.Text = timeM + ":" + timeS;	
			
		}else{
			
			missionTimeText.Text = "00:00";
			
		}
		
		
		
	}
	
	public void UpdateTaskTime(){
		
		
		int idx = 0;
		
		//生成Task节点
		for(int i = 0;i< BranchList.Count;i++){

			if(BranchList[i].taskArray[TaskIdx[i]].TaskTime != 0){
				
				if( 0 == BranchList[i].taskArray[TaskIdx[i]].StartTimeVal ){
					
					BranchList[i].taskArray[TaskIdx[i]].StartTimeVal = Time.time;
					 
				}
			
				BranchList[i].taskArray[TaskIdx[i]].CurrentPassimeVal = Time.time - BranchList[i].taskArray[TaskIdx[i]].StartTimeVal;
				
				
				for(int j = 0;j< BranchList[i].taskArray[TaskIdx[i]].SubObject.Count;j++){
				
					idx++;
					if((int)MissionType.SURVIVE == (int)BranchList[i].taskArray[TaskIdx[i]].SubObject[j].typeID){
					
						float time = BranchList[i].taskArray[TaskIdx[i]].SubObject[j].recycle - (Time.time - BranchList[i].taskArray[TaskIdx[i]].StartTimeVal);
	
						if(time <= 0){
							
							BranchList[i].taskArray[TaskIdx[i]].CurrentTimeVal = 0;
							
							//发送ission超时协议
	
						}else{
						
							BranchList[i].taskArray[TaskIdx[i]].CurrentTimeVal = time;
							
						}
					
						
						if(0 != BranchList[i].taskArray[TaskIdx[i]].CurrentTimeVal){
						
							int m = (int)( BranchList[i].taskArray[TaskIdx[i]].CurrentTimeVal / 60 );
							int s = (int)( BranchList[i].taskArray[TaskIdx[i]].CurrentTimeVal % 60 );
							string timeM = "";
							string timeS = "";
							
							if(m < 10){
								
								timeM = "0" + m.ToString();
							
							}else{
								
								timeM = m.ToString();
							
							}
							
							if(s < 10){
								
								timeS = "0" + s.ToString();
							
							}else{
								
								timeS = s.ToString();
							
							}

								TransformList.TryGetValue(idx,out tans);
								tans.GetComponent<_UI_MissionTrackItem>().TaskTime.Text = timeM+":"+timeS;
						}else{
						
								TransformList.TryGetValue(idx,out tans);
								tans.GetComponent<_UI_MissionTrackItem>().TaskTime.Text = "";
						}
						
					
					}
				}
			}
		}
		
	}
	
}
