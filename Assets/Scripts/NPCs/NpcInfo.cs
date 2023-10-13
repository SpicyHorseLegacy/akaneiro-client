using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class NpcInfo : MonoBehaviour {

    public enum NPCType
    {

        MISSIONPC = 1001,

        SHOPNPC,

        SKILLNPC,

        PETNPC,

        MAILNPC,

        DAYREWARDNPC,

        STEELNPC,

        AUCTIONNPC,

        SHOPRARE,

        WELL,
		
		STASH,
		
		DIALOG,

        NOTHING,

        MAXNPC
    }

	public static NpcInfo Instance;
    public static WellTable WellInfoFromTable;
    public List<ShopNpc> NPCS = new List<ShopNpc>();    // npc list that cur scnen has.

	//perfabs
	public ShopNpc [] npcObject;

	void Awake(){
		Instance = this;
        WellInfoFromTable = new WellTable();
        WellInfoFromTable.LoadInfo();
        DontDestroyOnLoad(this);
	}

    public static ShopNpc GetPrefabByID(int _id)
    {
        //Debug.Log(_id);
        ShopNpc _p = null;
        foreach(ShopNpc _prefab in Instance.npcObject)
        {
            if (_id == (int)_prefab.npcType)
            {
                _p = _prefab;
                break;
            }
        }
        return _p;
    }

    /// <summary>
    /// Add new npc into list.
    /// </summary>
    /// <param name="_npc"> new npc </param>
    public static void AddNPC(ShopNpc _npc)
    {
        for (int i = Instance.NPCS.Count - 1; i >= 0; i--)
        {
            if (!Instance.NPCS[i])
                Instance.NPCS.RemoveAt(i);
            else
            {
                if (Instance.NPCS[i] == _npc)
                    return;
            }
        }
        Instance.NPCS.Add(_npc);
    }

    public static ShopNpc GetNPCByType(NPCType _type)
    {
        ShopNpc _p = null;
        foreach (ShopNpc _npc in Instance.NPCS)
        {
            if (_npc.npcType == _type)
            {
                _p = _npc;
                break;
            }
        }
        return _p;
    }
}

public class WellTable
{
    public List<WellInfo> WellInfos = new List<WellInfo>();

    public void LoadInfo()
    {
		string _fileName = LocalizeManage.Instance.GetLangPath("Well.Quality");
        TextAsset item = (TextAsset)Resources.Load(_fileName, typeof(TextAsset));

        string[] itemRowsList = item.text.Split('\n');

        for (int i = 3; i < itemRowsList.Length - 1; ++i)
        {
            string pp = itemRowsList[i];
            string[] vals = pp.Split('	');
            WellInfo _tempWellInfo = new WellInfo();
            _tempWellInfo.Level = int.Parse(vals[0]);
            _tempWellInfo.Value = float.Parse(vals[1]);
            _tempWellInfo.Cost = int.Parse(vals[2]);
            _tempWellInfo.KarmaPerHour = int.Parse(vals[3]);
            _tempWellInfo.Description = vals[4];
            _tempWellInfo.UpgradeButtonString = vals[5];
            WellInfos.Add(_tempWellInfo);
        }
		
		LoadWellSizeInfo();
    }
	
	private void LoadWellSizeInfo() {
		string _fileName = LocalizeManage.Instance.GetLangPath("Well.Size");
        TextAsset item = (TextAsset)Resources.Load(_fileName, typeof(TextAsset));
		string[] itemRowsList = item.text.Split('\n');
		for (int i = 3; i < itemRowsList.Length - 1; ++i) {
			string pp = itemRowsList[i];
            string[] vals = pp.Split('	');
			SetWellInfoTime(int.Parse(vals[0]),int.Parse(vals[1]));
		}
	}
	
	private void SetWellInfoTime(int _lv,int time) {
		foreach (WellInfo _wellinfo in WellInfos)
        {
            if (_lv == _wellinfo.Level)
            {
                _wellinfo.iTime = time;
            }
        }
	}

    public WellInfo GetWellInfoByLevel(int _lv)
    {
        foreach (WellInfo _wellinfo in WellInfos)
        {
            if (_lv == _wellinfo.Level)
            {
                return _wellinfo;
            }
        }
        return null;
    }
}

public class WellInfo
{
    public int Level;
    public float Value;
    public int Cost;
	public int iTime;
    public int KarmaPerHour;
    public string Description;
    public string UpgradeButtonString;
}
