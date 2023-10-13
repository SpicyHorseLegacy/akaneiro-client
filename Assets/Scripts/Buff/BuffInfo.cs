using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum kTypeBuff
{
	// 100 是针对属性的加成，比如增加最大生命值
	// 200 是针对抗性的加成，比如减少受到冰属性的伤害
	// 300 是减成类Debuff
	// 400 是加成类Buff
	
    None                = 0,

	Enraged				= 100,
	Energetic			= 101,
	Healthy				= 102,
	Fortified			= 103,
	SwiftHits			= 104,
	FleetFeet			= 105,
	Precision			= 106,
	AkaneBlessing		= 107,
	EnergyRegenBoost	= 108,
	HPRegenBoost		= 109,
    InstanceHeal        = 110,
    AttackBonus         = 111,
    DarkHunter          = 112,
	
	AntiFreeze			= 200,
	Burnproof			= 201,
	BlastShield			= 202,
	Insulated			= 203,
	
	Stunned				= 300,
	Shocked				= 301,
	Frozen				= 302,
	Slowed				= 303,
	Knockback			= 304,
	Poisioned			= 305,
	Burning				= 306,
	Bloodthirsty		= 307,
	Fleeing				= 308,
	
	IronThorns			= 400,
	StormShell			= 401,
	StoneSkin			= 402,
	Invulnerable		= 403,
	ItemHunter			= 404,

    SpriteDog           = 500,
    SpriteCat           = 501,
    SpriteBird          = 502,
    SpriteFish          = 503,
    SpriteMonkey        = 504,
    SpriteTurtle        = 505,
    SpriteYokai         = 506,
}

/// <summary>
/// abs
/// </summary>
public class BuffInfo : MonoBehaviour {
	
	enum ColumnName
	{
		ID				= 0,
		Name			= 1,
		Description		= 11,
		Duration		= 6,
		ReplaceType		= 8,
		StatusType		= 12,
		ContinueType	= 13,
		Tick			= 14,
		ActiveAtStart	= 15,
		
	}
	
	public static BuffInfo Instance;
    public AbilityBuffLoader BuffLoader;

	public List<BaseBuff> BaseBuffsPrefabs = new List<BaseBuff>();
    public List<BaseBuff> LoadedBuffs = new List<BaseBuff>();

    string[] StatusInfo;

    public static Texture2D GetIconByID(int _id)
    {
        int buffidx = Instance.BuffLoader.GetBuffInfoByID(_id).IconIdx;
        return BuffIcons.Instance.GetIcon(buffidx);
    }
	
	void Awake()
	{
		DontDestroyOnLoad(gameObject);
		Instance = this;

        BuffLoader = new AbilityBuffLoader();
        BuffLoader.LoadAbilityBuffInfo();

        InitBuffInfo();
	}

    public void InitBuffInfo()
    {
        foreach (AbilityBuffInfo buffInfo in BuffLoader.AbilityBuffInfoList)
        {
            foreach (BaseBuff basebuffInfo in BaseBuffsPrefabs)
            {
                if (buffInfo.BuffType == basebuffInfo.BuffType)
                {
                    BaseBuff _tempBaseBuff = Instantiate(basebuffInfo) as BaseBuff;
                    _tempBaseBuff.ID = buffInfo.BuffID;
                    _tempBaseBuff.Name = buffInfo.Name;
                    _tempBaseBuff.Description = buffInfo.Description;
                    _tempBaseBuff.LifeTime = buffInfo.Duration;
                    _tempBaseBuff.ContinueType = buffInfo.ContinueType;
                    _tempBaseBuff.Tick = buffInfo.Tick;
                    _tempBaseBuff.isActiveAtBeginning = buffInfo.ActiveAtStart;
                    _tempBaseBuff.Priority = buffInfo.UIPriority;
                    _tempBaseBuff.StackNum = 0;
                    _tempBaseBuff.MaxStackNum = (buffInfo.ReplaceType == AbilityBuffInfo.EnumBuffReplaceType.CanMulti) ? 999 : 1;

                    _tempBaseBuff.transform.parent = transform;
                    _tempBaseBuff.transform.position = transform.position;
                    LoadedBuffs.Add(_tempBaseBuff);
                }
            }
        }
    }

    public BaseBuff GetBuffPrefab(int id)
    {
        BaseBuff newBuffInfo = null;

        foreach (BaseBuff basebuffInfo in LoadedBuffs)
        {
            if (basebuffInfo.ID == id)
            {
                newBuffInfo = Instantiate(basebuffInfo) as BaseBuff;
                newBuffInfo.transform.name = newBuffInfo.Name;
                break;
            }
        }

        return newBuffInfo;
    }
}

public class AbilityBuffLoader
{
    public List<AbilityBuffInfo> AbilityBuffInfoList = new List<AbilityBuffInfo>();

    public void LoadAbilityBuffInfo()
    {
		string _fileName = LocalizeManage.Instance.GetLangPath("Ability.Status");
        TextAsset item = (TextAsset)Resources.Load(_fileName, typeof(TextAsset));

        string[] itemRowsList = item.text.Split('\n');

        for (int i = 3; i < itemRowsList.Length - 1; ++i)
        {
            string pp = itemRowsList[i];
            string[] vals = pp.Split('	');
            AbilityBuffInfo _tempBuffInfo = new AbilityBuffInfo();
            _tempBuffInfo.BuffID = int.Parse(vals[0]);
            _tempBuffInfo.Name = vals[1];
            _tempBuffInfo.ReplaceType = (AbilityBuffInfo.EnumBuffReplaceType)int.Parse(vals[2]);
            _tempBuffInfo.StatusType = (AbilityBuffInfo.EnumBuffStatusType)int.Parse(vals[3]);
            _tempBuffInfo.TargetType = (AbilityBuffInfo.EnumBuffTargetType)int.Parse(vals[4]);
            _tempBuffInfo.Bonus = float.Parse(vals[5]);
            if (int.Parse(vals[6]) != -1)
                _tempBuffInfo.Duration = int.Parse(vals[6]) / 1000.0f;
            else
                _tempBuffInfo.Duration = int.Parse(vals[6]);
            _tempBuffInfo.Chance = int.Parse(vals[7]);
            _tempBuffInfo.EndCondition = (AbilityBuffInfo.EnumBuffEndCondition)int.Parse(vals[9]);
            _tempBuffInfo.Effect = vals[10];
            _tempBuffInfo.Description = vals[11];
            _tempBuffInfo.BuffType = (kTypeBuff)int.Parse(vals[12]);
            _tempBuffInfo.ContinueType = (kBuffContinueType)int.Parse(vals[13]);
            _tempBuffInfo.Tick = int.Parse(vals[14]);
            _tempBuffInfo.ActiveAtStart = (int.Parse(vals[15]) == 1);
            _tempBuffInfo.ElementType = new EStatusElementType(int.Parse(vals[16]));
            _tempBuffInfo.IconIdx = int.Parse(vals[17]);
            _tempBuffInfo.UIPriority = int.Parse(vals[18]);

            AbilityBuffInfoList.Add(_tempBuffInfo);
        }
    }

    public AbilityBuffInfo GetBuffInfoByID(int _buffID)
    {
        foreach (AbilityBuffInfo buffInfo in AbilityBuffInfoList)
        {
            if (buffInfo.BuffID == _buffID)
            {
                return buffInfo;
            }
        }
        return null;
    }
}

public class AbilityBuffInfo
{
    public enum EnumBuffReplaceType
    {
        CanMulti,
        Control,
        Absorb,
    }

    public enum EnumBuffStatusType
    {
        Common,
        BeAttacked,
        Attack,
        Absorb,
    }

    public enum EnumBuffTargetType
    {
        Enermy,
        All,
        Self,
        Self_Ally,
    }

    public enum EnumBuffEndCondition
    {
        Death = 1,
        Move,
        BeHurt,
        Attack,
        Defence,
        BeStun,
        BeSilence,
    }

    public int BuffID;
    public string Name;
    public EnumBuffReplaceType ReplaceType;
    public EnumBuffStatusType StatusType;
    public EnumBuffTargetType TargetType;
    public float Bonus;
    public float Duration;
    public int Chance;
    public EnumBuffEndCondition EndCondition;
    public string Effect;
    public string Description;
    public kTypeBuff BuffType;
    public kBuffContinueType ContinueType;
    public int Tick;
    public bool ActiveAtStart;
    public EStatusElementType ElementType;
    public int IconIdx;
    public int UIPriority;
}
