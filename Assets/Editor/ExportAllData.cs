using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Xml;
using System.Xml.Serialization;
using System.IO;
using System.Threading;
using System.Security.Cryptography;
using System.Collections.Generic;
using AstarClasses;
using AstarMath;

class ExportAllData  {
	

	[MenuItem("Export/All CurrentScene Data")]
    static void Execute()
    {
		if (EditorUtility.DisplayDialog("Are you sure?", "Are you sure you wish to update?", "Yes", "No"))
		{
			
			string filename =  Application.dataPath  + Path.DirectorySeparatorChar +"ExportPath.txt";
			
			StreamReader sReader;
			
			string _strOutFolder ="";
			
			if (File.Exists(filename))
			{
				sReader = new StreamReader(filename);
				
			}
			else
			{
				Debug.Log("ExportPath.txt does not exist");
				return;
			}
			
			
			if(sReader != null)
		    {
			     string pp = sReader.ReadLine();
				
			     while (pp != null)
                 {

                    if(pp.Contains("ExportFolder"))
				    {
						 string[] vals = pp.Split(new char[] { '=' });
						_strOutFolder = vals[1];
						
						
						break;
						
					}
					
					pp = sReader.ReadLine();
					
				 }
				
			   	  sReader.Close();	
			 }
			
			_strOutFolder = "\\" + _strOutFolder;
			
	        
			
			_strOutFolder = "D:\\AKAServer\\data\\Scenes";
			
			ExportAkaMission();
			ExportScene(_strOutFolder);
			ExportMonsterData(_strOutFolder);
			ExportBreakActorData(_strOutFolder);
			SaveNavMesh(_strOutFolder);
				
		}
			
	}
	static void ExportScene(string _strOutFolder)
	{
		CreateCurrentSceneBuddle.ControlFolder(true);

        string[] path = EditorApplication.currentScene.Split(CreateAssetbundles.pathSeparator, System.StringSplitOptions.RemoveEmptyEntries);
		string _strSceneName = path[path.Length - 1].Replace(".unity", "");
	
		string _strOutPath = _strOutFolder + Path.DirectorySeparatorChar + _strSceneName + ".xml";
		
		if(File.Exists(_strOutPath))
		   File.Delete(_strOutPath);
		
		XMLFileWriter fileWriter = new XMLFileWriter();
		
		fileWriter.BindWithFile(_strOutPath );
		
		fileWriter.NodeBegin("Level");
		fileWriter.AddAttribute("mapName", _strSceneName);
		
		BaseExportNode[] exportNodes =  Object.FindObjectsOfType(typeof(BaseExportNode)) as BaseExportNode[];
		
		int i = 0, j= 0;
		for( i = 0; i < exportNodes.Length - 1;i++)
		{
			for( j = i + 1;j < exportNodes.Length; j++)
			{
				if( exportNodes[j].GetNodeType() == exportNodes[i].GetNodeType())
				{
					BaseExportNode tempNode = exportNodes[j];
					exportNodes[j] = exportNodes[++i];
				    exportNodes[i] = tempNode;
				}
			}
		}
		for( int k = 0; k < exportNodes.Length;)
		{
			if(exportNodes[k].GetNodeType() == "Trigger")
			{
				BaseExportNode tempNode = exportNodes[k];
				bool bRepeat = true;
				for(int q = k+1;q < exportNodes.Length;q++)
				{
					if(exportNodes[q].GetNodeType() != "Trigger")
					{
						bRepeat = false;
						break;
					}
				}
				
				if( bRepeat == true)
					break;
				
				for(int p = k+1; p < exportNodes.Length;p++)
					exportNodes[p-1] = exportNodes[p];
				
				exportNodes[exportNodes.Length - 1] = tempNode;
				
			}
			else 
			{
			   ++k;
			}
		}
	
	    string mPastString ="";
		
		foreach ( BaseExportNode be in exportNodes)
		{
			if(mPastString != be.GetNodeType())
			{
				GetTitleString(mPastString,be.GetNodeType(),fileWriter);
			}
			
			mPastString = be.GetNodeType();
			
			if(be.GetComponent<NpcBase>())
			{
			    string theString = be.GetComponent<NpcBase>().MonsterSceneDoExport();
				
			    fileWriter.AddContent( theString );
			}
			else if( be.GetComponent<InteractiveHandler>()&& !be.GetComponent<InteractiveHandler>().bFinish)
			{
				string theString = be.GetComponent<InteractiveHandler>().BreakableSceneDoExport();
				
				fileWriter.AddContent( theString );
			}
			else 
			{
			    fileWriter.AddContent(be.DoExport());
			}
			
			be.bFinish = true;
			
		}
	
		GetTitleString(mPastString,"",fileWriter);
		
		fileWriter.NodeEnd("Level");
	
		fileWriter.Flush();
		
		fileWriter.ShutDown();
		
		foreach ( BaseExportNode be in exportNodes)
		{
			be.bFinish = false;
		}
	}
	
	static void GetTitleString(string mStrpast,string mStrnow,XMLFileWriter fileWriter)
	{
		//XMLStringWriter xmlTitleWriter = new XMLStringWriter();
				
	    if( mStrpast == "TelePort")
		{
		   fileWriter.NodeEnd("Teleports");
		}
	    else if(mStrpast == "MonsterSpawner")
		{
		   fileWriter.NodeEnd("MonsterSpawner");
	    }
		else if(mStrpast == "MapItem")
		{
		   fileWriter.NodeEnd("MapItems");
		}
				
		if( mStrnow == "TelePort")
		{
			fileWriter.NodeBegin("Teleports");
		}
		else if( mStrnow == "MonsterSpawner")
		{
		    fileWriter.NodeBegin("MonsterSpawner");  	
		} 
		else if( mStrnow == "MapItem")
		{
			fileWriter.NodeBegin("MapItems");
		}
		
		
	}
	
	static void ExportMonsterData(string _strOutFolder)
	{
		string[] existingMonsters = new string[0];
	
		List<NpcBase> MonsterList = new List<NpcBase>();
		
		string outPath = _strOutFolder + Path.DirectorySeparatorChar + "Monster.xml";
		
		if(File.Exists(outPath))
		   File.Delete(outPath);
			
		//for(int i = 0; i < 1;i++)
		//{
		   /*
		   if(i == 1)
			 existingMonsters = Directory.GetFiles("Assets/Prefabs/CH/Enemies/Area1_Common","*.prefab",SearchOption.AllDirectories);
		   else if( i == 2)
			 existingMonsters = Directory.GetFiles("Assets/Prefabs/CH/Enemies/Area2_Common","*.prefab",SearchOption.AllDirectories);
		   else if( i == 3)
			 existingMonsters = Directory.GetFiles("Assets/Prefabs/CH/Enemies/Area3_Common","*.prefab",SearchOption.AllDirectories);
	       else if(i == 4)
			 existingMonsters = Directory.GetFiles("Assets/Prefabs/CH/Enemies/Area4_Common","*.prefab",SearchOption.AllDirectories);
	       else if(i == 5)
			 existingMonsters = Directory.GetFiles("Assets/Prefabs/CH/Enemies/Mission","*.prefab",SearchOption.AllDirectories);
			*/
			
		  existingMonsters = Directory.GetFiles("Assets/Prefabs/CH/Enemies","*.prefab",SearchOption.AllDirectories);
			
          foreach (string MonsterFile in existingMonsters)
          {
			  NpcBase theMonster =  AssetDatabase.LoadAssetAtPath(MonsterFile,typeof(NpcBase)) as NpcBase;
			  if( theMonster != null)
			     MonsterList.Add(theMonster);
		  }
		//}
		
		
		XMLFileWriter fileWriter = new XMLFileWriter();
		fileWriter.BindWithFile(outPath );
		
		fileWriter.NodeBegin("Monsters");
		
		foreach( NpcBase EveryMonster in MonsterList)
		{
			fileWriter.AddContent(EveryMonster.DoExport());
			
		}
		
		fileWriter.NodeEnd("Monsters");
		fileWriter.Flush();
		fileWriter.ShutDown();
	}
	
	static void ExportBreakActorData(string _strOutFolder)
	{
        string outPath = _strOutFolder + Path.DirectorySeparatorChar + "BreakableActor.xml";
		
		if(File.Exists(outPath))
		   File.Delete(outPath);
				
		string[] existingGams = Directory.GetFiles("Assets/Prefabs/GAM/Breakables","*.prefab",SearchOption.AllDirectories);
	
		List<InteractiveObj> BreakableActorList = new List<InteractiveObj>();
		
        foreach (string GamFile in existingGams)
        {
		   InteractiveObj theBreakableActor =  AssetDatabase.LoadAssetAtPath(GamFile,typeof(InteractiveObj)) as InteractiveObj;
		   if( theBreakableActor != null)
			     BreakableActorList.Add(theBreakableActor);
		}
		
		existingGams = Directory.GetFiles("Assets/Prefabs/GAM/Chest","*.prefab",SearchOption.AllDirectories);
		
		foreach (string GamFile in existingGams)
        {
		   InteractiveObj theBreakableActor =  AssetDatabase.LoadAssetAtPath(GamFile,typeof(InteractiveObj)) as InteractiveObj;
		   if( theBreakableActor != null)
			     BreakableActorList.Add(theBreakableActor);
		}
		
		existingGams = Directory.GetFiles("Assets/Prefabs/GAM/Switches","*.prefab",SearchOption.AllDirectories);
		
		foreach (string GamFile in existingGams)
        {
		   Transform theBreakableActor2  =  AssetDatabase.LoadAssetAtPath(GamFile,typeof(Transform)) as Transform;
			
		   if( theBreakableActor2 != null)
			{
				//for(int i = 0; i < theBreakableActor2.childCount;i++)
			   //{
			        InteractiveObj temp =  theBreakableActor2.GetComponent<InteractiveObj>();//theBreakableActor2.GetChild(i).GetComponent<InteractiveObj>();
					
					if(temp != null)
						 BreakableActorList.Add(temp);
					
			  // }
			}
		}
	
		
		existingGams = Directory.GetFiles("Assets/Prefabs/GAM/Gating","*.prefab",SearchOption.AllDirectories);
		
		foreach (string GamFile in existingGams)
        {
		   InteractiveObj theBreakableActor =  AssetDatabase.LoadAssetAtPath(GamFile,typeof(InteractiveObj)) as InteractiveObj;
		   if( theBreakableActor != null)
			     BreakableActorList.Add(theBreakableActor);
		}
		
		
		XMLFileWriter fileWriter = new XMLFileWriter();
		fileWriter.BindWithFile(outPath );
		
		fileWriter.NodeBegin("BreakableActors");
		
		foreach( InteractiveObj EveryBreakableActor in BreakableActorList)
		{
		   fileWriter.AddContent(EveryBreakableActor.DoExport());
		}
		
		fileWriter.NodeEnd("BreakableActors");
		fileWriter.Flush();
		fileWriter.ShutDown();
	}
	
	static void ExportAStarData(string _strOutFolder)
	{
		
	}
	
	static void SaveNavMesh(string _strOutFolder)
    {
		AstarPath AStart = AstarPath.active as AstarPath;
		
		AStart.Scan (true,-1);
		
		if (AStart == null || AStart.staticNodes == null)
		{
			Debug.Log("No AStar data! Scan map first!");

			return;
		}


        string[] path = EditorApplication.currentScene.Split(CreateAssetbundles.pathSeparator, System.StringSplitOptions.RemoveEmptyEntries);
		string _strSceneName = path[path.Length - 1].Replace(".unity", "");

        string strNavMeshFile = _strOutFolder + Path.DirectorySeparatorChar + _strSceneName + ".navmesh.xml";
		
		if(File.Exists(strNavMeshFile))
		    File.Delete(strNavMeshFile);

       

		XMLFileWriter xmlWriter = new XMLFileWriter();
		if (!xmlWriter.BindWithFile(strNavMeshFile))
		{
			Debug.Log("Can not write file: " + strNavMeshFile + " !");
			return;
		}

		Debug.Log("navMesh generating start...");

        xmlWriter.NodeBegin("navmesh");
		xmlWriter.AddAttribute("scene", _strSceneName);

            for (int y = 0; y < AStart.grids.Length; y++)
            {
                if (AStart.staticNodes.GetLength(0) == 0)
                    continue;

                Grid grid = AStart.grids[y];

                xmlWriter.NodeBegin("grid");
                xmlWriter.AddAttribute("index", y);

                xmlWriter.AddAttribute("offsetX", grid.realOffset.x);
                xmlWriter.AddAttribute("offsetY", grid.realOffset.y);
                xmlWriter.AddAttribute("offsetZ", grid.realOffset.z);
			    xmlWriter.AddAttribute("nodeSize", grid.nodeSize);
                xmlWriter.AddAttribute("Width", grid.globalWidth);  // * grid.nodeSize
                xmlWriter.AddAttribute("Depth", grid.globalDepth);  // * grid.nodeSize
			    xmlWriter.AddAttribute("Height", grid.globalHeight);

			
				ArrayList nodeList = new ArrayList();
				int nodeWidth = AStart.staticNodes[y].GetLength(0);
				int nodeDepth = AStart.staticNodes[y].GetLength(1);
				byte[] nodeString = new byte[nodeWidth*nodeDepth];

				for (int x = 0; x < nodeWidth; x++)
				{
					for (int z = 0; z < nodeDepth; z++)
					{
						Node node = AStart.staticNodes[y][x, z];

                        if (nodeList.Contains(node))
							continue;
				
						if (node.walkable)
						{
							nodeString[x*nodeDepth+z] = (byte)('1');
						}
						else
						{
							nodeString[x*nodeDepth+z] = (byte)('0');
						}

						nodeList.Add(node);
					}
				}
			
				xmlWriter.NodeBegin("allNodes");				
				string str = System.Text.ASCIIEncoding.ASCII.GetString( nodeString );
				xmlWriter.AddAttribute("Str", str);
				xmlWriter.NodeEnd("allNodes");
			
                xmlWriter.NodeEnd("grid");
            }


			xmlWriter.NodeBegin("dynamicLinks");
			for (int link = 0; link < AStart.links.Length; link++)
			{
				NodeLink nodeLink = AStart.links[link];

				Int3 from = AstarPath.ToLocal(nodeLink.fromVector);
				Node fromNode = null;
				if (from != new Int3(-1, -1, -1))
					fromNode = AstarPath.GetNode(from);

				Int3 to = AstarPath.ToLocal(nodeLink.toVector);
				Node toNode = null;
				if (to != new Int3(-1, -1, -1))
					toNode = AstarPath.GetNode(to);


				xmlWriter.NodeBegin("link");
					//xmlWriter.AddAttribute("name", nodeLink.name);
					xmlWriter.AddAttribute("type", nodeLink.linkType);
					//xmlWriter.AddAttribute("enableInEditor", nodeLink.enableInEditor);
					if (fromNode != null)
					{
						xmlWriter.AddAttribute("fromX", fromNode.pos.x);
						xmlWriter.AddAttribute("fromY", fromNode.pos.y);
						xmlWriter.AddAttribute("fromZ", fromNode.pos.z);
					}
					if (nodeLink.linkType == LinkType.Link && toNode != null)
					{
						xmlWriter.AddAttribute("toX", toNode.pos.x);
						xmlWriter.AddAttribute("toY", toNode.pos.y);
						xmlWriter.AddAttribute("toZ", toNode.pos.z);
					}
				xmlWriter.NodeEnd("link");
			}
			xmlWriter.NodeEnd("dynamicLinks");

        xmlWriter.NodeEnd("navmesh");
		xmlWriter.Flush();
		
		Debug.Log("navMesh generated finished!");
	}
	
	public static void ExportAkaMission(){

        XmlDocument xmlDoc = new XmlDocument();

        XmlElement root = xmlDoc.CreateElement("MissionsConfig");
        xmlDoc.AppendChild(root);
		
		for(int n = 1;n<5;n++){
			
			string areaName = "Area_" + n.ToString() + "/Mission/";
		
			//for(int i =0;i<int.Parse(missionCount);i++){
			for(int i =0;i<2000;i++){
				
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
//		 xmlDoc.Save(address + xmlName);
		 xmlDoc.Save("D:/AKAServer/data/Missions.xml");
		
	}
	
}
