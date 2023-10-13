using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

public class MakeSelfAnimationIDUnique
{
   
    private static List<int> _idList = new List<int>();
    private static List<SelfAnimation> _invalidList = new List<SelfAnimation>();

    [MenuItem("Tools/MakeSelfAnimationIDUnique")]
    static void Execute()
    {
        _idList.Clear();
        _invalidList.Clear();

        int maxId = - 1;
        SelfAnimation[] selfAnimationList = Object.FindObjectsOfType(typeof(SelfAnimation)) as SelfAnimation[];
        for (int i = 0; i < selfAnimationList.Length; i++)
        { 
            SelfAnimation sa = selfAnimationList[i];
			
            int id = sa.id;
			
            if (IsUnique(id))
            {
               _idList.Add(id);
               if (id > maxId)
                 maxId = id;
            }
            else
               _invalidList.Add(sa);
          
        }

        string info = "New SelfAnimation Ids: ";
        foreach (SelfAnimation tsa in _invalidList)
        {
            tsa.id = ++maxId;
            EditorUtility.SetDirty(tsa);
            info += tsa.id + ", ";
        }

        Debug.Log(info);
        Debug.Log(string.Format("MakeSelfAnimationIDUnique Finish  SelfAnimation Num: {0} ", selfAnimationList.Length));
    }

  

    private static bool IsUnique(int _id)
    {
        return !_idList.Contains(_id);
    }
}
