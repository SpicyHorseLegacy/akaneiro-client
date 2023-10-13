using UnityEngine;
using UnityEditor;
using System.Collections;
using System.IO;
using System.Threading;

public class AKALevelExporter : ScriptableWizard 
{
	public string ExportedLevelData = "ExportedLevelData";	// ÊäÈë¹Ø¿šÎÄŒþÎ»ÖÃ


	string _strOutPath;
	string _strSceneName;

	[MenuItem("Export/Other/Level Data")]
	static void CreateWizard()
	{
		ScriptableWizard.DisplayWizard("AKA Level Exporter", typeof(AKALevelExporter), "Export");
	}

	void OnWizardCreate()
	{
		//_strOutPath = Application.dataPath + "/" + ExportedLevelData;

        string[] path = EditorApplication.currentScene.Split(CreateAssetbundles.pathSeparator, System.StringSplitOptions.RemoveEmptyEntries);
		_strSceneName = path[path.Length - 1].Replace(".unity", "");
		
		
		_strOutPath = EditorUtility.SaveFilePanel("Level Export .xml file", "", _strSceneName, "xml");
		
		if(_strOutPath.Length == 0)
			return;
		
		CreateCurrentSceneBuddle.ControlFolder(true);
		
	
		XMLFileWriter fileWriter = new XMLFileWriter();
		//fileWriter.BindWithFile(_strOutPath + "/" + _strSceneName + ".xml");
		
		fileWriter.BindWithFile(_strOutPath );
		fileWriter.NodeBegin("Level");
		fileWriter.AddAttribute("mapName", _strSceneName);
		
	
		BaseExportNode[] exportNodes = FindObjectsOfType(typeof(BaseExportNode)) as BaseExportNode[];
		
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
			else if( be.GetComponent<InteractiveHandler>() && !be.GetComponent<InteractiveHandler>().bFinish)
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
	
	void OnWizardUpdate()
	{
		helpString = "Export level config files for use in Server side.";

		if (EditorApplication.currentScene == null)
		{
			errorString = "No Scene is active!";
			isValid = false;
		}
		else if (ExportedLevelData.Length == 0)
		{
			errorString = "Please specify the folder name!";
			isValid = false;
		}
		else
		{
			errorString = "";
			isValid = true;
		}
	}

	// When the user pressed the "Export" button OnWizardOtherButton is called.
	void OnWizardOtherButton()
	{
	}
	
	void GetTitleString(string mStrpast,string mStrnow,XMLFileWriter fileWriter)
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
		
		//return xmlTitleWriter.Result;
	}
}
