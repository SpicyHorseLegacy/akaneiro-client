
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class BreakableDataExport {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
    [MenuItem("Export/Other/Breakable Actor Data")]
    static void Execute()
	{
		string outPath = EditorUtility.SaveFilePanel("BreakableActorData Export .xml file", "", "BreakableActor", "xml");
		
		if(outPath.Length == 0)
			return;
				
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
		
		existingGams = Directory.GetFiles("Assets/Prefabs/GAM/Shrines","*.prefab",SearchOption.AllDirectories);
		
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
		
}

