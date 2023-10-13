using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class MakeTriggerIDUnique
{
    private static int index_start_ = 8000;
    private static List<int> idList_ = new List<int>();
    private static List<Trigger_Unified> invalidList_ = new List<Trigger_Unified>();

    [MenuItem("Tools/MakeTriggerIDUnique")]
    static void Execute()
    {
        idList_.Clear();
        invalidList_.Clear();

        int maxId = index_start_ - 1;
        Trigger_Unified[] triggerList = Object.FindObjectsOfType(typeof(Trigger_Unified)) as Trigger_Unified[];
        for (int i = 0; i < triggerList.Length; i++)
        { 
            Trigger_Unified trigger = triggerList[i];
            int id = trigger.UnityID;
            if (IsValid(id))
            {
                if (IsUnique(id))
                {
                    idList_.Add(id);
                    if (id > maxId)
                        maxId = id;
                }
                else
                    invalidList_.Add(trigger);
            }
            else
                invalidList_.Add(trigger);
        }

        string info = "New Ids: ";
        foreach (Trigger_Unified trigger in invalidList_)
        {
            trigger.SetUnityID(++maxId);
            EditorUtility.SetDirty(trigger);
            info += trigger.UnityID + ", ";
        }

        Debug.Log(info);
        Debug.Log(string.Format("MakeTriggerIDUnique Finish  Trigger Num: {0} ", triggerList.Length));
    }

    private static bool IsValid(int _id)
    {
        return _id >= index_start_;
    }

    private static bool IsUnique(int _id)
    {
        return !idList_.Contains(_id);
    }
}
