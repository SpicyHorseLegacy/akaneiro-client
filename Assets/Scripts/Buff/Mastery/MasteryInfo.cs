using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MasteryInfo : MonoBehaviour {
	
	public static MasteryInfo Instance = null;
	
	public const int MAXLEVEL = 10;
	
    public SingleMastery MasteryPrefab;

    public Texture2D[] Icons;

    public MasteryLoader MasteryLoaderInstance = null;
    
    public List<SingleMastery> Masteries = new List<SingleMastery>();
	
    public static SingleMasteryInfo GetSingleMasteryInfoById(int _id)
    {
        SingleMasteryInfo _info = null;

        foreach (SingleMasteryInfo _singleinfo in Instance.MasteryLoaderInstance.MasteryInfoList)
        {
            if (_singleinfo.ID == _id)
            {
                return _singleinfo;
            }
        }

        return _info;
    }

	void Awake () {
		Instance = this;
		DontDestroyOnLoad(this);

        MasteryLoaderInstance = new MasteryLoader();
        MasteryLoaderInstance.LoadMasteryInfo();
	}
	
	void Start () 
	{
		//CS_Main.Instance.g_commModule.SendMessage(ProtocolGame_SendRequest.RoleMasteryListReq());
	}

    public Texture2D GetIconByType(EMasteryType type)
    {
        Texture2D _tex = null;
        switch (type.Get())
        {
            case 1:
                _tex = Icons[0];
                break;
            case 2:
                _tex = Icons[1];
                break;
            case 4:
                _tex = Icons[2];
                break;
            case 8:
                _tex = Icons[3];
                break;
            case 16:
                _tex = Icons[4];
                break;
            case 32:
                _tex = Icons[5];
                break;
        }
        return _tex;
    }
	
	public void ResetAllMasteriesInfo(vectorMasteryLevelInfo MasteryLevelInfoVec)
	{
        foreach (SMasteryLevelInfo masteryInfo in MasteryLevelInfoVec)
        {
            bool hasAnInstance = false;
            for (int i = Masteries.Count - 1; i >= 0; i--)
            {
                SingleMastery _singleMastery = Masteries[i];
                if(_singleMastery.MasteryClass.Get() == masteryInfo.masteryType.Get())
                {
                    _singleMastery.UpdateInfoFromServer(masteryInfo);
                    hasAnInstance = true;
                    break;
                }
            }

            // if there is no Instance for a mastery, create one.
            if (!hasAnInstance)
            {
                // init a new mastery
                SingleMastery _newMastery = Instantiate(MasteryPrefab, transform.position, transform.rotation) as SingleMastery;
                _newMastery.transform.parent = transform;
                Masteries.Add(_newMastery);

                switch (masteryInfo.masteryType.Get())
                {
                    case 1:
                        _newMastery.Icon = Icons[0];
                        break;
                    case 2:
                        _newMastery.Icon = Icons[1];
                        break;
                    case 4:
                        _newMastery.Icon = Icons[2];
                        break;
                    case 8:
                        _newMastery.Icon = Icons[3];
                        break;
                    case 16:
                        _newMastery.Icon = Icons[4];
                        break;
                    case 32:
                        _newMastery.Icon = Icons[5];
                        break;
                }
                _newMastery.MasteryClass = masteryInfo.masteryType;
                _newMastery.MasteryType = GetBaseInfo(masteryInfo.masteryType).Type;

                _newMastery.UpdateInfoFromServer(masteryInfo);
            }
        }
#if NGUI
        Player.Instance.MasteryManager.UpdateMasteryInfo(MasteryLevelInfoVec);
#else
		AbilitiesShop.Instance.UpdateMasteryList();
#endif
//       _UI_CS_AbilitiesTrainer_Weapon.Instance.ResetAllItems();
//        UI_CS_AbilitiesTrainer_Armor.Instance.ResetAllItems();
	}

    public void MasteryLevelUp(int id)
    {
        EMasteryType _type = null;
        switch (id / 1000)
        {
            case 1:
                _type = new EMasteryType(EMasteryType.e1HWeaponType);
                break;
            case 2:
                _type = new EMasteryType(EMasteryType.eDualWeaponType);
                break;
            case 3:
                _type = new EMasteryType(EMasteryType.e2HWeaponType);
                break;
            case 4:
                _type = new EMasteryType(EMasteryType.eLightArmorType);
                break;
            case 5:
                _type = new EMasteryType(EMasteryType.eMediumArmorType);
                break;
            case 6:
                _type = new EMasteryType(EMasteryType.eHeavyArmorType);
                break;
        }
        foreach (SingleMastery mastery in Masteries)
        {
            if (mastery.MasteryClass.Get() == _type.Get())
            {
                mastery.CurLv++;
            }
        }
        foreach (SingleMastery mastery in Player.Instance.MasteryManager.Masteries)
        {
            if (mastery.MasteryClass.Get() == _type.Get())
            {
                mastery.CurLv++;
            }
        }
    }

    public SingleMasteryInfo GetBaseInfo(EMasteryType masteryClass)
    {
        foreach (SingleMasteryInfo _masteryInfo in MasteryLoaderInstance.MasteryInfoList)
        {
            if (_masteryInfo.Class.Get() == masteryClass.Get() && _masteryInfo.MasteryLevel == 1)
            {
                return _masteryInfo;
            }
        }
        return null;
    }

    public SingleMasteryInfo GetNextLevelInfo(EMasteryType masteryClass, int lv)
	{
        foreach (SingleMasteryInfo _masteryInfo in MasteryLoaderInstance.MasteryInfoList)
        {
            if (_masteryInfo.Class.Get() == masteryClass.Get() && _masteryInfo.MasteryLevel == lv + 1)
            {
                return _masteryInfo;
            }
        }
		return null;
	}

    public SingleMasteryInfo GetCurrentLevelInfo(EMasteryType masteryClass, int lv)
	{
		foreach (SingleMasteryInfo _masteryInfo in MasteryLoaderInstance.MasteryInfoList)
        {
            if (_masteryInfo.Class.Get() == masteryClass.Get() && _masteryInfo.MasteryLevel == lv)
            {
                return _masteryInfo;
            }
        }
		return null;
	}
}

public class MasteryLoader
{
    public List<SingleMasteryInfo> MasteryInfoList = new List<SingleMasteryInfo>();

    public void LoadMasteryInfo()
    {
		string _fileName = LocalizeManage.Instance.GetLangPath("EquipMastery.level");
        TextAsset item = (TextAsset)Resources.Load(_fileName, typeof(TextAsset));
        string[] itemRowsList = item.text.Split('\n');
        for (int i = 3; i < itemRowsList.Length - 1; ++i)
        {
            string pp = itemRowsList[i];
            string[] vals = pp.Split('	');
            SingleMasteryInfo _tempMasteryInfo = new SingleMasteryInfo();

            _tempMasteryInfo.ID = int.Parse(vals[0]);
            _tempMasteryInfo.Class = new EMasteryType(int.Parse(vals[1]));
            _tempMasteryInfo.MasteryLevel = int.Parse(vals[2]);
            _tempMasteryInfo.Type = (EnumMasteryClass)int.Parse(vals[3]);
            _tempMasteryInfo.karmaCost = int.Parse(vals[4]);
            _tempMasteryInfo.BaseStateLvNeeded = int.Parse(vals[5]);
            _tempMasteryInfo.BuffID = int.Parse(vals[6]);
            _tempMasteryInfo.Name = vals[7];
            _tempMasteryInfo.Description = vals[8];
            _tempMasteryInfo.Description2 = vals[9];
            _tempMasteryInfo.Discipline = (AbilityDetailInfo.EDisciplineType)int.Parse(vals[10]);
			_tempMasteryInfo.TrainTime = int.Parse(vals[11]) * 60;
			_tempMasteryInfo.shortName = vals[12];
            MasteryInfoList.Add(_tempMasteryInfo);
        }
    }
}


public class SingleMasteryInfo
{
    public int ID;
    public EMasteryType Class;      //mastery type;
    public int MasteryLevel;
    public EnumMasteryClass Type;   //weapon or armor
    public int karmaCost;
    public int BaseStateLvNeeded;
    public int BuffID;
    public string Name;
    public string Description;
    public string Description2;
    public AbilityDetailInfo.EDisciplineType Discipline;
	public int TrainTime;
	public string shortName;
}
