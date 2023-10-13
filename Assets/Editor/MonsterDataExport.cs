using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

public class MonsterDataExport {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	
	
    [MenuItem("Export/Other/Monster Data Export")]
    static void Execute()
	{
		//string outPath  = Application.dataPath + "/ExportedLevelData/";
		
		string outPath = EditorUtility.SaveFilePanel("MonsterData Export .xml file", "", "Monster", "xml");
		
		if(outPath.Length == 0)
			return;
				
		string[] existingMonsters = new  string[0];
	    
		List<NpcBase> MonsterList = new List<NpcBase>();

		existingMonsters = Directory.GetFiles("Assets/Prefabs/CH/Enemies","*.prefab",SearchOption.AllDirectories);

        foreach (string MonsterFile in existingMonsters)
        {
            NpcBase theMonster = AssetDatabase.LoadAssetAtPath(MonsterFile, typeof(NpcBase)) as NpcBase;
            if (theMonster != null)
                MonsterList.Add(theMonster);
        }
		
		XMLFileWriter fileWriter = new XMLFileWriter();
		fileWriter.BindWithFile(outPath);
		
		fileWriter.NodeBegin("Monsters");
		
		foreach( NpcBase EveryMonster in MonsterList)
		{
			fileWriter.AddContent(EveryMonster.DoExport());
			
		}
		
		fileWriter.NodeEnd("Monsters");
		fileWriter.Flush();
		fileWriter.ShutDown();
		
	}
		
}
