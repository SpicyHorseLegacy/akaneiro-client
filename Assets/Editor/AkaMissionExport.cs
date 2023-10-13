using UnityEngine;
using UnityEditor;
using System;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Text;


public class AkaMissionExport :  EditorWindow
{
	public static string version = "1.500";
	public static string address = Application.dataPath;
	public static string MissionFileAdd = Application.dataPath + "/Mission/";
	public static string serviceadd = "//192.168.6.81/data/Missions.xml";
	public static string xmlName = "/Missions.xml";
	public static string xmlName2 = "/192.168.6.81/data/Missions.xml";
	public static string xmlName3 = "D:/AKAServer/data/";
	public static string missionCount;
	
	private const int MissionCount = 2000;
	
	[UnityEditor.MenuItem("Export/Other/Mission Data")]	
	public static void Initialize() {
		EditorWindow window = EditorWindow.GetWindow(typeof(AkaMissionExport),true,"AkaMissionTool",true);
		window.Show();
		missionCount = "2";
	}

	public void OnGUI() {

		GUILayout.Label("Version :" + version);
		GUILayout.Space (5);
		
//		GUILayout.Label("Please Set Mission Count");
//		missionCount = GUILayout.TextField (missionCount, 50);
//		GUILayout.Space (5);
		
		GUILayout.Label("Server task file address:");
		GUILayout.TextField (serviceadd, 50);
		GUILayout.Space (5);
		
		GUILayout.Label("Export address :");
		GUILayout.TextField (address + xmlName, 50);
		GUILayout.Space (30);
		
		if(GUILayout.Button(new GUIContent("Export", "Export aka mission."))){
			Debug.Log("--- ExportAkaMission ---");
			ExportAkaMission();
		}
		
	}
	
	public void ExportAkaMission(){

        XmlDocument xmlDoc = new XmlDocument();

        XmlElement root = xmlDoc.CreateElement("MissionsConfig");
        xmlDoc.AppendChild(root);
		
		for(int n = 1;n<9;n++){
			
			string areaName = "Area_" + n.ToString() + "/Mission/";
		
			//for(int i =0;i<int.Parse(missionCount);i++){
			for(int i =0;i<MissionCount;i++){
				
				int id = 5000 + i;
				string filePath = "Assets/Scenes/Areas/" + areaName + id.ToString()+".prefab";
				//Debug.Log("filePath : "+filePath);
				
				GameObject MissionPrefab;
				XmlElement missionInfo;
				XmlElement branchInfo;
				XmlElement taskInfo;
				XmlElement trackInfo;
				XmlElement rewardInfo;
				XmlElement RewardContent;
				XmlElement ChoiceInfo;
				XmlElement taskTime;
				
				MissionPrefab = (GameObject)AssetDatabase.LoadAssetAtPath(filePath,typeof( GameObject ));
				
				if(null == MissionPrefab){
					
					continue;
					
				}
				
	       		XmlElement mission = xmlDoc.CreateElement("Mission");
				root.AppendChild(mission);
				
				missionInfo = xmlDoc.CreateElement("ID");
				missionInfo.SetAttribute("value", MissionPrefab.GetComponent<_UI_CS_MapLevelItem>().ID.ToString());
				//Debug.Log("value  : "+MissionPrefab.GetComponent<_UI_CS_MapLevelItem>().ID.ToString());
				mission.AppendChild(missionInfo);
				
	        	missionInfo = xmlDoc.CreateElement("Name");
				missionInfo.SetAttribute("value", MissionPrefab.GetComponent<_UI_CS_MapLevelItem>().name.ToString());
				//Debug.Log("value  : "+MissionPrefab.GetComponent<_UI_CS_MapLevelItem>().name.ToString());
				mission.AppendChild(missionInfo);
				
//				missionInfo = xmlDoc.CreateElement("State");
//				missionInfo.SetAttribute("value", ((MissionPrefab.GetComponent<_UI_CS_MapLevelItem>().isOpen)?1:0).ToString());
//				//Debug.Log("value  : "+((MissionPrefab.GetComponent<_UI_CS_MapLevelItem>().isOpen)?1:0).ToString());
//				mission.AppendChild(missionInfo);
				
//				missionInfo = xmlDoc.CreateElement("IsRepeat");
//				missionInfo.SetAttribute("value", ((MissionPrefab.GetComponent<_UI_CS_MapLevelItem>().isLoop)?1:0).ToString());
//				//Debug.Log("value  : "+((MissionPrefab.GetComponent<_UI_CS_MapLevelItem>().isLoop)?1:0).ToString());
//				mission.AppendChild(missionInfo);
				
				missionInfo = xmlDoc.CreateElement("MapID");
				missionInfo.SetAttribute("value", MissionPrefab.GetComponent<_UI_CS_MapLevelItem>().mapID.ToString());
				//Debug.Log("value  : "+((MissionPrefab.GetComponent<_UI_CS_MapLevelItem>().isLoop)?1:0).ToString());
				mission.AppendChild(missionInfo);
				
				missionInfo = xmlDoc.CreateElement("CostBadge");
				missionInfo.SetAttribute("value", MissionPrefab.GetComponent<_UI_CS_MapLevelItem>().costBadge.ToString());
				//Debug.Log("value  : "+((MissionPrefab.GetComponent<_UI_CS_MapLevelItem>().isLoop)?1:0).ToString());
				mission.AppendChild(missionInfo);
				
				missionInfo = xmlDoc.CreateElement("ThreatUpValue");
				missionInfo.SetAttribute("value", MissionPrefab.GetComponent<_UI_CS_MapLevelItem>().threatUp.ToString());
				mission.AppendChild(missionInfo);
				missionInfo = xmlDoc.CreateElement("ThreatDownValue");
				missionInfo.SetAttribute("value", MissionPrefab.GetComponent<_UI_CS_MapLevelItem>().threatDown.ToString());
				mission.AppendChild(missionInfo);
				
//				missionInfo = xmlDoc.CreateElement("TriggerId");
//				missionInfo.SetAttribute("value", MissionPrefab.GetComponent<_UI_CS_MapLevelItem>().triggerId.ToString());
//				//Debug.Log("value  : "+((MissionPrefab.GetComponent<_UI_CS_MapLevelItem>().isLoop)?1:0).ToString());
//				mission.AppendChild(missionInfo);
				
				missionInfo = xmlDoc.CreateElement("MissionLevel");
				missionInfo.SetAttribute("value", MissionPrefab.GetComponent<_UI_CS_MapLevelItem>().ThreatLevel.ToString());
				//Debug.Log("value  : "+MissionPrefab.GetComponent<_UI_CS_MapLevelItem>().Missionlevel.ToString());
				mission.AppendChild(missionInfo);
				
				missionInfo = xmlDoc.CreateElement("Minlevel");
				missionInfo.SetAttribute("value", MissionPrefab.GetComponent<_UI_CS_MapLevelItem>().levelMin.ToString());
				//Debug.Log("value  : "+MissionPrefab.GetComponent<_UI_CS_MapLevelItem>().levelMin.ToString());
				mission.AppendChild(missionInfo);
				
				missionInfo = xmlDoc.CreateElement("Maxlevel");
				missionInfo.SetAttribute("value", MissionPrefab.GetComponent<_UI_CS_MapLevelItem>().levelMax.ToString());
				//Debug.Log("value  : "+MissionPrefab.GetComponent<_UI_CS_MapLevelItem>().levelMax.ToString());
				
				mission.AppendChild(missionInfo);
				
				missionInfo = xmlDoc.CreateElement("PredecessorsMission");
				missionInfo.SetAttribute("value", MissionPrefab.GetComponent<_UI_CS_MapLevelItem>().PredecessorsMission.ToString());
				
				mission.AppendChild(missionInfo);
				
				missionInfo = xmlDoc.CreateElement("MissionTime");
				missionInfo.SetAttribute("value", MissionPrefab.GetComponent<_UI_CS_MapLevelItem>().MissionTime.ToString());
				
				mission.AppendChild(missionInfo);
				
				/////////////////////////////////////
				for(int x = 0;x<MissionPrefab.GetComponent<_UI_CS_MapLevelItem>().branchArray.Count;x++){
					
//					string branchName = "Branch" + (x+1).ToString();
					string branchName = "Branch";
					branchInfo = xmlDoc.CreateElement(branchName);
					
					missionInfo = xmlDoc.CreateElement("BranchID");
					missionInfo.SetAttribute("value", (x+1).ToString());
					branchInfo.AppendChild(missionInfo);
					
					missionInfo = xmlDoc.CreateElement("isBranchMustComplete");
					missionInfo.SetAttribute("value", MissionPrefab.GetComponent<_UI_CS_MapLevelItem>().branchArray[x].isBonus?"False":"True");
					branchInfo.AppendChild(missionInfo);
					
					rewardInfo = xmlDoc.CreateElement("Reward");
					branchInfo.AppendChild(rewardInfo);
					RewardContent = xmlDoc.CreateElement("BranchRewardContent");	
					RewardContent.SetAttribute("karma", MissionPrefab.GetComponent<_UI_CS_MapLevelItem>().branchArray[x].rewardContentKarma.ToString());
					RewardContent.SetAttribute("Exp", MissionPrefab.GetComponent<_UI_CS_MapLevelItem>().branchArray[x].rewardContentExp.ToString());
					rewardInfo.AppendChild(RewardContent);
					
//					missionInfo = xmlDoc.CreateElement("BranchExp");
//					missionInfo.SetAttribute("value", MissionPrefab.GetComponent<_UI_CS_MapLevelItem>().branchArray[x].rewardContentExp.ToString());
//					branchInfo.AppendChild(missionInfo);
//					missionInfo = xmlDoc.CreateElement("BranchKarma");
//					missionInfo.SetAttribute("value", MissionPrefab.GetComponent<_UI_CS_MapLevelItem>().branchArray[x].rewardContentKarma.ToString());
//					branchInfo.AppendChild(missionInfo);
					mission.AppendChild(branchInfo);

					/////////////////////////////////////
					//Task list
					for(int j = 0;j<MissionPrefab.GetComponent<_UI_CS_MapLevelItem>().branchArray[x].taskArray.Count;j++){
						
//						string taskName = "Task" + (j+1).ToString();
						string taskName = "Task";
						missionInfo = xmlDoc.CreateElement(taskName);
						branchInfo.AppendChild(missionInfo);
						
						taskInfo = xmlDoc.CreateElement("TaskID");
						taskInfo.SetAttribute("value", (j+1).ToString());
						missionInfo.AppendChild(taskInfo);
						
						taskInfo = xmlDoc.CreateElement("TriggerId");
						taskInfo.SetAttribute("value", MissionPrefab.GetComponent<_UI_CS_MapLevelItem>().branchArray[x].taskArray[j].triggerId.ToString());
						missionInfo.AppendChild(taskInfo);
						
						taskInfo = xmlDoc.CreateElement("TriggerType");
						taskInfo.SetAttribute("value", MissionPrefab.GetComponent<_UI_CS_MapLevelItem>().branchArray[x].taskArray[j].triggerType.ToString());
						missionInfo.AppendChild(taskInfo);
						
						taskInfo = xmlDoc.CreateElement("MissionTrack");
						missionInfo.AppendChild(taskInfo);
						
						rewardInfo = xmlDoc.CreateElement("Reward");
						missionInfo.AppendChild(rewardInfo);
						
						RewardContent = xmlDoc.CreateElement("TaskRewardContent");	
						RewardContent.SetAttribute("karma", MissionPrefab.GetComponent<_UI_CS_MapLevelItem>().branchArray[x].taskArray[j].rewardContentKarma.ToString());
						RewardContent.SetAttribute("Exp", MissionPrefab.GetComponent<_UI_CS_MapLevelItem>().branchArray[x].taskArray[j].rewardContentExp.ToString());
						rewardInfo.AppendChild(RewardContent);
						
						ChoiceInfo = xmlDoc.CreateElement("isTaskMustComplete");
//						ChoiceInfo.SetAttribute("value", MissionPrefab.GetComponent<_UI_CS_MapLevelItem>().branchArray[x].taskArray[j].isChoice?"True":"False");
						ChoiceInfo.SetAttribute("value", "True");
						missionInfo.AppendChild(ChoiceInfo);
						
						taskTime   = xmlDoc.CreateElement("TaskTime");
						taskTime.SetAttribute("value", MissionPrefab.GetComponent<_UI_CS_MapLevelItem>().branchArray[x].taskArray[j].TaskTime.ToString());
						missionInfo.AppendChild(taskTime);
						/////////////////////////////////////
						//Track list
						for(int k = 0;k<MissionPrefab.GetComponent<_UI_CS_MapLevelItem>().branchArray[x].taskArray[j].SubObject.Count;k++){
							
							trackInfo = xmlDoc.CreateElement("trackContent");
							
							trackInfo.SetAttribute("trackType",((int) MissionPrefab.GetComponent<_UI_CS_MapLevelItem>().branchArray[x].taskArray[j].SubObject[k].typeID).ToString());
							trackInfo.SetAttribute("objectId", MissionPrefab.GetComponent<_UI_CS_MapLevelItem>().branchArray[x].taskArray[j].SubObject[k].objectID.ToString());
							trackInfo.SetAttribute("count", MissionPrefab.GetComponent<_UI_CS_MapLevelItem>().branchArray[x].taskArray[j].SubObject[k].count.ToString());
							trackInfo.SetAttribute("recycle", MissionPrefab.GetComponent<_UI_CS_MapLevelItem>().branchArray[x].taskArray[j].SubObject[k].recycle.ToString());
							
							taskInfo.AppendChild(trackInfo);
		
						}
						
						/////////////////////////////////////	
					}
				/////////////////////////////////////
				}
				
				missionInfo = xmlDoc.CreateElement("MissionRewardKarma");
				missionInfo.SetAttribute("value", MissionPrefab.GetComponent<_UI_CS_MapLevelItem>().sk.ToString());
				//Debug.Log("value  : "+MissionPrefab.GetComponent<_UI_CS_MapLevelItem>().sk.ToString());
				mission.AppendChild(missionInfo);
				
				missionInfo = xmlDoc.CreateElement("MissionRewardExp");
				missionInfo.SetAttribute("value", MissionPrefab.GetComponent<_UI_CS_MapLevelItem>().xp.ToString());
				//Debug.Log("value  : "+MissionPrefab.GetComponent<_UI_CS_MapLevelItem>().xp.ToString());
				mission.AppendChild(missionInfo);
				
				missionInfo = xmlDoc.CreateElement("NewMissionID");
				missionInfo.SetAttribute("value", MissionPrefab.GetComponent<_UI_CS_MapLevelItem>().NewMissionID.ToString());
				//Debug.Log("value  : "+MissionPrefab.GetComponent<_UI_CS_MapLevelItem>().NewMissionID.ToString());
				mission.AppendChild(missionInfo);
			}
		}
		 xmlDoc.Save(address + xmlName);
		 xmlDoc.Save(xmlName3 + xmlName);
		
	}
	
	

}
