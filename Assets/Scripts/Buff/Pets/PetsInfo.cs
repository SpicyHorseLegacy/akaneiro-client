using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PetsInfo : MonoBehaviour {

    public static PetsInfo Instance;

    public SinglePetListInfo[] PetsList = new SinglePetListInfo[0];

    [SerializeField] Texture2D[] PetIcons;
    [SerializeField] Texture2D[] PetSimpleIcons;
    [SerializeField] GameObject[] PetModels;

    public static SinglePetListInfo GetPetListInfoByID(int _petid)
    {
        foreach (SinglePetListInfo _info in Instance.PetsList)
        {
            if (_info.m_ID == _petid)
                return _info;
        }
        return null;
    }

    public static SinglePetListInfo GetPetListInfoByLVAndType(int _level, Enum_PetType _type)
    {
        foreach (SinglePetListInfo _info in Instance.PetsList)
        {
            if (_info.m_Type == _type && _info.m_CurLV == _level)
                return _info;
        }
        return null;
    }

    public static Texture2D GetPetIconByType(Enum_PetType _type)
    {
        foreach (SinglePetListInfo _info in Instance.PetsList)
        {
            if (_info.m_Type == _type)
                return Instance.PetIcons[_info.m_IconID];
        }
        return null;
    }

    public static Texture2D GetPetSimpleIconByType(Enum_PetType _type)
    {
        foreach (SinglePetListInfo _info in Instance.PetsList)
        {
            if (_info.m_Type == _type)
                return Instance.PetSimpleIcons[_info.m_IconID];
        }
        return null;
    }

    public static GameObject GetModelByName(string _name)
    {
        foreach (SinglePetListInfo _info in Instance.PetsList)
        {
            if (_info.m_name == _name)
                return Instance.PetModels[_info.m_IconID];
        }
        return null;
    }

    void Awake()
    {
        Instance = this;
        LoadFileInfo();
    }

    void LoadFileInfo()
    {
        List<SinglePetListInfo> _templist = new List<SinglePetListInfo>();
        string _fileName = LocalizeManage.Instance.GetLangPath("PetPrice.Price");
        TextAsset item = (TextAsset)Resources.Load(_fileName, typeof(TextAsset));
        string[] itemRowsList = item.text.Split('\n');
        for (int i = 3; i < itemRowsList.Length - 1; ++i)
        {
            SinglePetListInfo shItem = new SinglePetListInfo();
            string pp = itemRowsList[i];
            string[] vals = pp.Split(new char[] { '	', '	' });
            shItem.m_ID = int.Parse(vals[0]);
            shItem.m_IconID = (int.Parse(vals[1]) - 1);
            shItem.m_name = vals[2];
            shItem.m_RentShards = int.Parse(vals[3]);
            shItem.m_ShortDescription = vals[4];
            shItem.m_BuffDescription = vals[5];
            shItem.m_EffectsID = convertStringToIntArray(vals[6]);
            shItem.m_LevelRequid = int.Parse(vals[7]);
            shItem.m_IsShopShow = int.Parse(vals[8]) == 1;
            if(vals.Length > 9)
			{
				shItem.m_CurLV = int.Parse(vals[9]);
	            shItem.m_MaxExp = int.Parse(vals[10]);
	            shItem.Price_Hour = int.Parse(vals[11]);
	            shItem.MoneyType_Hour = new EMoneyType(int.Parse(vals[12]) + 1);
	            shItem.Price_Day = int.Parse(vals[13]);
	            shItem.MoneyType_Day = new EMoneyType(int.Parse(vals[14]) + 1);
	            shItem.Price_Week = int.Parse(vals[15]);
	            shItem.MoneyType_Week = new EMoneyType(int.Parse(vals[16]) + 1);
	            string type = vals[17].Replace("\r", "");
	            shItem.m_Type = (Enum_PetType)int.Parse(type);
	            _templist.Add(shItem);
			}
        }
        PetsList = _templist.ToArray();
    }

    int[] convertStringToIntArray(string _string)
    {
        int[] newArray = new int[1];
        if (_string == "")
        {
            newArray[0] = 0;
        }
        else
        {
            string[] _temp = _string.Split('+');
            newArray = new int[_temp.Length];
            for (int i = 0; i < _temp.Length; i++)
            {
                newArray[i] = int.Parse(_temp[i]);
            }
        }
        return newArray;
    }
	
}

// load pet info for every id from petprice.price file.
public class SinglePetListInfo
{
    public int m_ID;
    public int m_IconID;
    public string   m_name;
    public int m_RentShards;
    public string m_ShortDescription;
    public string m_BuffDescription;
    public int[] m_EffectsID;
    public int m_LevelRequid;
    public bool m_IsShopShow;
    public int m_CurLV;
    public int m_MaxExp;
    public int Price_Hour;
    public EMoneyType MoneyType_Hour;
    public int Price_Day;
    public EMoneyType MoneyType_Day;
    public int Price_Week;
    public EMoneyType MoneyType_Week;
    public Enum_PetType m_Type;
}

public class PetModelInfo
{

}

public enum Enum_PetType
{
    NONE = 0,
    Inugami = 1000,
    Maneki_Neko = 2000,
    Yosuzume = 3000,
    Nishikigoi = 4000,
    Nihonzaru = 5000,
    Ishigame = 6000,
    Kabuki_Kozo = 7000,
    Golden_Inugami = 1100,
    Singularity_Neko = 2100,
    Pen_gin = 8000,
    Karakuri_Gaben = 9000,
    MAX = 999000,
}