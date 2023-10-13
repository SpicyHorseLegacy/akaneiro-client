using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ConsumableItemsInfoManager : MonoBehaviour {

	public static ConsumableItemsInfoManager Instance;
	
	ConsumableItemInfo[] AllInfo;
	
    public static ConsumableItemInfo[] GetAllInfoByType(ConsumableItemInfo.Enum_ConsumableType _type)
    {
        List<ConsumableItemInfo> _templist = new List<ConsumableItemInfo>();
        foreach (ConsumableItemInfo _info in Instance.AllInfo)
        {
            if (_info.ItemType == _type) _templist.Add(_info);
        }
        if (_templist.Count > 0) return _templist.ToArray();
        else return null;
    }

    public static ConsumableItemInfo GetInfoByID(int _id)
    {
        ConsumableItemInfo _temp = null;
        for (int i = 0; i < Instance.AllInfo.Length; i++ )
        {
            if (_id == Instance.AllInfo[i].ID)
            {
                _temp = Instance.AllInfo[i];
                break;
            }
        }
        return _temp;
    }

	void Awake(){Instance = this;}
	
	void Start(){LoadInfo();}
	
	void LoadInfo()
	{
		List<ConsumableItemInfo> _templist = new List<ConsumableItemInfo>();
        string _fileName = LocalizeManage.Instance.GetLangPath("ConsumableItem.General");
        TextAsset item = (TextAsset)Resources.Load(_fileName, typeof(TextAsset));
        string[] itemRowsList = item.text.Split('\n');
        for (int i = 3; i < itemRowsList.Length - 1; ++i)
        {
            ConsumableItemInfo _newitem = new ConsumableItemInfo();
            string pp = itemRowsList[i];
            string[] vals = pp.Split(new char[] { '	', '	' });
            _newitem.ID = int.Parse(vals[0]);
            _newitem.Name = vals[1];
            _newitem.ItemCategory = int.Parse(vals[2]);
            _newitem.StatusEffect = int.Parse(vals[4]);
            _newitem.BuffID = int.Parse(vals[5]);
            _newitem.StatusEffectBonus = int.Parse(vals[6]);
			//俄罗斯版本没有这个文件。临时return;//
            _newitem.ItemType = (ConsumableItemInfo.Enum_ConsumableType)int.Parse(vals[7]);
            _newitem.ShortDescription = vals[8];
            _templist.Add(_newitem);
        }
        AllInfo = _templist.ToArray();
	}
}

public class ConsumableItemInfo
{
	public enum Enum_ConsumableType
	{
		E_NONE = 0,
		E_Bundle = 1,
		E_Food= 3,
		E_Drink = 2,
		E_Material = 4,
		E_DontShow = -1,
		E_MAX = 99,
	}
	
	public int ID;
	public string Name;
	public int ItemCategory;
	public int StatusEffect;
	public int BuffID;
	public int StatusEffectBonus;
	public Enum_ConsumableType ItemType;
	public string ShortDescription;
	public string FullDescription;
}
