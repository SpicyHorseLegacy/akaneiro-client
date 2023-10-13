using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using System.IO;
using System.Xml;

public class ServerDataSanity 
{
    [MenuItem("DataSanity/Teleport Trigger's PathNode")]
    static void Execute()
    { 
        string serverDataFolder = EditorUtility.OpenFolderPanel("Level Data Folder", @"D:\Unity\BulletServer\Akaneiro\AKA_Network\AKA_Server\Bin\data", "Scenes");
        if (serverDataFolder.Length <= 0)
            return;

        string[] files = Directory.GetFiles(serverDataFolder);

        XmlDocument doc = new XmlDocument();

        bool bHasError = false;
        foreach (string file in files)
        {
            doc.Load(file);
            foreach (XmlNode trigger in doc.DocumentElement.SelectNodes(@"/Level/Triggers/TriggerResult"))
            {
                if (trigger.Attributes["Type"].Value == "2")
                {
                    if (trigger.SelectSingleNode("DesPos") == null)
                    {
                        bHasError = true;
                        Debug.LogError(string.Format("[DataSanity] {0} has no PathNode in {1}", trigger.Attributes["Name"].Value, Path.GetFileName(file)));
                    }
                }
            }
        }
        if (!bHasError)
            Debug.Log("[DataSanity] All Trigger PathNode Successfully");
    }


    [MenuItem("DataSanity/Trigger ID")]
    static void Execute_TriggerID()
    {
        string serverDataFolder = EditorUtility.OpenFolderPanel("Level Data Folder", @"D:\Unity\BulletServer\Akaneiro\AKA_Network\AKA_Server\Bin\data", "Scenes");
        if (serverDataFolder.Length <= 0)
            return;

        string[] files = Directory.GetFiles(serverDataFolder);

        XmlDocument doc = new XmlDocument();
        List<int> idlist = new List<int>();
        bool bHasError = false;

        Debug.Log("Checking...");

        foreach (string file in files)
        {
            string filename = Path.GetFileName(file);
            if(filename.Contains("navmesh"))
                continue;

            doc.Load(file);
            idlist.Clear();
            foreach (XmlNode trigger in doc.DocumentElement.SelectNodes(@"/Level/Triggers"))
            {
                int id = int.Parse(trigger.Attributes["id"].Value);
                if (idlist.Contains(id))
                {
                    bHasError = true;
                    Debug.LogError(string.Format(" [ID] {0}, {1} ", filename, id));
                }
                else
                    idlist.Add(id);
            }
        }

        Debug.Log("Finish");

        if (!bHasError)
            Debug.Log("[DataSanity] All Trigger PathNode Successfully");
    }
}
