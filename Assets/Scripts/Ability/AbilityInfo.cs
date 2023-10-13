using UnityEngine;
//using UnityEditor;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public enum AbilityIDs
{
    NormalAttack_1H_ID = 10001,
    NormalAttack_2H_ID = 10002,
    NormalAttack_Fist_ID = 10003,

	SwathOfDestruction_ID = 10201,
	SwathOfFlame_ID = 10202,
    SwathOfDestructionIII_ID = 10203,
    SwathOfDestructionIV_ID = 10204,
    SwathOfDestructionV_ID = 10205,
    SwathOfDestructionVI_ID = 10206,

    SeeingRed_I_ID = 10301,
    SeeingRed_II_ID = 10302,
    SeeingRed_III_ID = 10303,
    SeeingRed_IV_ID = 10304,
    SeeingRed_V_ID = 10305,
    SeeingRed_VI_ID = 10306,

    HungryCleave_I_ID = 10401,
    HungryCleave_II_ID = 10402,
    HungryCleave_III_ID = 10403,
    HungryCleave_IV_ID = 10404,
    HungryCleave_V_ID = 10405,
    HungryCleave_VI_ID = 10406,

    RainOfBlows_I_ID = 10501,
    RainOfBlows_II_ID = 10502,
    RainOfBlows_III_ID = 10503,
    RainOfBlows_IV_ID = 10504,
    RainOfBlows_V_ID = 10505,
    RainOfBlows_VI_ID = 10506,

    Shockwave_I_ID = 10601,
    Shockwave_II_ID = 10602,
    Shockwave_III_ID = 10603,
    Shockwave_IV_ID = 10604,
    Shockwave_V_ID = 10605,
    Shockwave_VI_ID = 10606,

    MeteorOfRain_I_ID = 10701,
    MeteorOfRain_II_ID = 10702,
    MeteorOfRain_III_ID = 10703,
    MeteorOfRain_IV_ID = 10704,
    MeteorOfRain_V_ID = 10705,
    MeteorOfRain_VI_ID = 10706,

    SkinOfStone_I_ID = 10801,
    SkinOfStone_II_ID = 10802,
    SkinOfStone_III_ID = 10803,
    SkinOfStone_IV_ID = 10804,
    SkinOfStone_V_ID = 10805,
    SkinOfStone_VI_ID = 10806,

    HauntingScream_I_ID = 10901,
    HauntingScream_II_ID = 10902,
    HauntingScream_III_ID = 10903,
    HauntingScream_IV_ID = 10904,
    HauntingScream_V_ID = 10905,
    HauntingScream_VI_ID = 10906,

    IronThorns_I_ID = 11001,
    IronThorns_II_ID = 11002,
    IronThorns_III_ID = 11003,
    IronThorns_IV_ID = 11004,
    IronThorns_V_ID = 11005,
    IronThorns_VI_ID = 11006,

    IceBarricade_I_ID = 11101,
    IceBarricade_II_ID = 11102,
    IceBarricade_III_ID = 11103,
    IceBarricade_IV_ID = 11104,
    IceBarricade_V_ID = 11105,
    IceBarricade_VI_ID = 11106,

    ChiPrayer_I_ID = 11201,
    ChiPrayer_II_ID = 11202,
    ChiPrayer_III_ID = 11203,
    ChiPrayer_IV_ID = 11204,
    ChiPrayer_V_ID = 11205,
    ChiPrayer_VI_ID = 11206,

    ChiMend_I_ID = 11301,
    ChiMend_II_ID = 11302,
    ChiMend_III_ID = 11303,
    ChiMend_IV_ID = 11304,
    ChiMend_V_ID = 11305,
    ChiMend_VI_ID = 11306,

	SteadyShot_I_ID = 11401,
    SteadyShot_II_ID = 11402,
    SteadyShot_III_ID = 11403,
    SteadyShot_IV_ID = 11404,
    SteadyShot_V_ID = 11405,
    SteadyShot_VI_ID = 11406,

    StingingShot_I_ID = 11501,
    StingingShot_II_ID = 11502,
    StingingShot_III_ID = 11503,
    StingingShot_IV_ID = 11504,
    StingingShot_V_ID = 11505,
    StingingShot_VI_ID = 11506,

    NinjaEscape_I_ID = 11601,
    NinjaEscape_II_ID = 11602,
    NinjaEscape_III_ID = 11603,
    NinjaEscape_IV_ID = 11604,
    NinjaEscape_V_ID = 11605,
    NinjaEscape_VI_ID = 11606,

    Caltrops_I_ID = 11701,
    Caltrops_II_ID = 11702,
    Caltrops_III_ID = 11703,
    Caltrops_IV_ID = 11704,
    Caltrops_V_ID = 11705,
    Caltrops_VI_ID = 11706,

    FirebombTrap_I_ID = 11801,
    FirebombTrap_II_ID = 11802,
    FirebombTrap_III_ID = 11803,
    FirebombTrap_IV_ID = 11804,
    FirebombTrap_V_ID = 11805,
    FirebombTrap_VI_ID = 11806,

	BearTrap_ID = 11901,

    DarkHunter_I_ID = 12101,
    DarkHunter_II_ID = 12102,
    DarkHunter_III_ID = 12103,
    DarkHunter_IV_ID = 12104,
    DarkHunter_V_ID = 12105,
    DarkHunter_VI_ID = 12106,

    Revive_ID = 12001,
    ReviveEX_ID = 12002,
	
	Imp_Rock_Toss = 30006,
	Werewolf_Range_ElectricBall = 30007,
	Oni_Poacher_Shockwave = 30012,
	Oni_Lord_Meteor = 30013,
	Imp_SlowDown = 30014,
    Imp_Rock_Toss_Slow = 30015,
    Werewolf_WhirlWind = 30021,
    Futakuchi_RainOfBlow = 30025,
    Futakuchi_SkyStrike = 30026,
}

public class AbilityInfo : MonoBehaviour
{
	public static AbilityInfo Instance;

    public AbilityInfoLoader AbilityInfomation;
    public AbilityObjectLoader AbilityObjectInfomation;
	
	public Texture2D[] AbilityIcons;
	
	public List<AbilityBaseState> PlayerAbilityPool = new List<AbilityBaseState>();
	public List<AbilityBaseState> AllyAbilityPool = new List<AbilityBaseState>();
	public List<AbilityBaseState> EnemyAbilityPool = new List<AbilityBaseState>();
	
	public static AbilityObjectInfo GetAbilityObjectInfomationByID(int id)
	{
		if(Instance && Instance.AbilityObjectInfomation != null)
		{
			return Instance.AbilityObjectInfomation.GetObjectInfoByID(id);
		}
		return null;
	}

    public static AbilityDetailInfo GetAbilityDetailInfoByID(int id)
    {
        if (Instance && Instance.AbilityInfomation != null)
        {
            return Instance.AbilityInfomation.GetAbilityDetailInfoByID(id);
        }
        return null;
    }
	
	void Awake()
	{
		DontDestroyOnLoad(gameObject);
		
		Instance = this;

        AbilityInfomation = new AbilityInfoLoader();
        AbilityInfomation.LoadAbilityDetailInfo();
        AbilityObjectInfomation = new AbilityObjectLoader();
        AbilityObjectInfomation.LoadAbilityObjectInfo();
		
		SetAbilityInfo();
	}

    void SetAbilityInfo()
    {
        foreach (AbilityBaseState abi in PlayerAbilityPool)
        {
            if (abi)
            {
                AbilityDetailInfo _abilityDetailInfo = AbilityInfomation.GetAbilityDetailInfoByID(abi.id);
                if (_abilityDetailInfo != null)
                {
                    abi.Info = _abilityDetailInfo;
                    abi.name = _abilityDetailInfo.Name;
                }
            }
        }

        foreach (AbilityBaseState abi in AllyAbilityPool)
        {
            AbilityDetailInfo _abilityDetailInfo = AbilityInfomation.GetAbilityDetailInfoByID(abi.id);
            abi.Info = _abilityDetailInfo;
            abi.name = _abilityDetailInfo.Name;
        }

        print("All Ability Info Set Done!");
    }

	public AbilityBaseState GetAbilityByID(uint id)
	{
		foreach(AbilityBaseState ability in PlayerAbilityPool)
		{
			if(ability.id == id)
				return ability;
		}
		
		return null;
	}
	
	public AbilityBaseState newAbilityByIDAndOwner(uint id, ObjectType type)
	{
		List<AbilityBaseState> abilityPool = null;
			
		switch(type)
		{
		case ObjectType.Player:
				abilityPool = AbilityInfo.Instance.PlayerAbilityPool;
			break;
			
		case ObjectType.Ally:
				abilityPool = AbilityInfo.Instance.AllyAbilityPool;
			break;
			
		case ObjectType.Enermy:
				abilityPool = AbilityInfo.Instance.EnemyAbilityPool;
			break;
			
		default:
				abilityPool = AbilityInfo.Instance.PlayerAbilityPool;
			break;
		}
		
		
		foreach(AbilityBaseState obj_state in abilityPool)
		{
			if(obj_state.id == id)
			{
				AbilityBaseState newability = Instantiate(obj_state, obj_state.transform.position, obj_state.transform.rotation) as AbilityBaseState;
                newability.Info = obj_state.Info;
				return newability;
			}
		}
		return null;
	}
}

public class AbilityInfoLoader
{
    public List<AbilityDetailInfo> AbilityDetailInfoList = new List<AbilityDetailInfo>();

    public void LoadAbilityDetailInfo()
    {
		string fileName = LocalizeManage.Instance.GetLangPath("Ability.Ability");
        TextAsset item = (TextAsset)Resources.Load(fileName, typeof(TextAsset));

        string[] itemRowsList = item.text.Split('\n');

        for (int i = 3; i < itemRowsList.Length - 1; ++i)
        {
            string pp = itemRowsList[i];
            string[] vals = pp.Split('	');
            AbilityDetailInfo _tempAbilityDeatilInfo = new AbilityDetailInfo();

            _tempAbilityDeatilInfo.ID = int.Parse(vals[0]);
            _tempAbilityDeatilInfo.Name = vals[1];

            int _type = _tempAbilityDeatilInfo.ID;
			// because the revive ID is different, 12001 & 12002 are two different ability but not the normal ability like.
			if(_tempAbilityDeatilInfo.ID != 12001 && _tempAbilityDeatilInfo.ID != 12002)
			{
				_type = (int)(_type / 10) * 10;
				if(_type > 13000)
                	_type = 20000;
			}
            
            _tempAbilityDeatilInfo.AbilityType = (AbilityDetailInfo.EnumAbilityType)_type;

            _tempAbilityDeatilInfo.ProgramType = (AbilityDetailInfo.EnumProgramType)int.Parse(vals[2]);
            _tempAbilityDeatilInfo.TargetType = (AbilityDetailInfo.EnumTargetType)int.Parse(vals[3]);
            _tempAbilityDeatilInfo.ElementType = new EStatusElementType(int.Parse(vals[4]));
            _tempAbilityDeatilInfo.ManaCost = int.Parse(vals[5]);
            _tempAbilityDeatilInfo.PrepareTime = int.Parse(vals[6]) / 1000.0f;
            _tempAbilityDeatilInfo.TimeToMaxPower = int.Parse(vals[7]) / 1000.0f;
            _tempAbilityDeatilInfo.IncreamentDMG = int.Parse(vals[8]);
            _tempAbilityDeatilInfo.AnimationDuration = int.Parse(vals[9]) / 1000.0f;
            _tempAbilityDeatilInfo.CoolDown = int.Parse(vals[10]) / 1000.0f;
            _tempAbilityDeatilInfo.AttackCount = int.Parse(vals[11]);
            _tempAbilityDeatilInfo.AttackTime = int.Parse(vals[12]) / 1000.0f;
            _tempAbilityDeatilInfo.AttackCoefficient = int.Parse(vals[13]);
            _tempAbilityDeatilInfo.DMGType = (AbilityDetailInfo.EnumDMGType)int.Parse(vals[14]);
            _tempAbilityDeatilInfo.BaseValue = float.Parse(vals[15]);
            _tempAbilityDeatilInfo.AbilityBonus = float.Parse(vals[16]);
            _tempAbilityDeatilInfo.AttrType = new EAttributeType(int.Parse(vals[17]));
            _tempAbilityDeatilInfo.AttackPosType = (AbilityDetailInfo.EnumAttackPosType)int.Parse(vals[18]);
            _tempAbilityDeatilInfo.AttackAreaType = (AbilityDetailInfo.EnumAttackAreaType)int.Parse(vals[19]);
            _tempAbilityDeatilInfo.StartDistance = float.Parse(vals[20]);
            _tempAbilityDeatilInfo.EndDistance = float.Parse(vals[21]);
            _tempAbilityDeatilInfo.Level = int.Parse(vals[22]);
            _tempAbilityDeatilInfo.Discipline = (AbilityDetailInfo.EDisciplineType)int.Parse(vals[23]);
            _tempAbilityDeatilInfo.Karma = int.Parse(vals[24]);
			_tempAbilityDeatilInfo.ObjectID = convertStringToIntArray(vals[25]);
            _tempAbilityDeatilInfo.SelfStatusID = convertStringToIntArray(vals[26]);
            _tempAbilityDeatilInfo.DestStatusID = convertStringToIntArray(vals[27]);
			_tempAbilityDeatilInfo.TrainingTime = int.Parse(vals[28])*60;
            _tempAbilityDeatilInfo.Description1 = vals[29];
            _tempAbilityDeatilInfo.Value1Prefix = vals[30];
            _tempAbilityDeatilInfo.Value1Suffix = vals[31];
            _tempAbilityDeatilInfo.Value2Prefix = vals[32];
            _tempAbilityDeatilInfo.Value2Suffix = vals[33];
            _tempAbilityDeatilInfo.AddEffectTitle1 = vals[34];
            _tempAbilityDeatilInfo.AddEffectDesc1 = vals[35];
            _tempAbilityDeatilInfo.AddEffectTitle2 = vals[36];
            _tempAbilityDeatilInfo.AddEffectDesc2 = vals[37];
			_tempAbilityDeatilInfo.shortName = vals[39];
			_tempAbilityDeatilInfo.Extra = vals[38];
//            _tempAbilityDeatilInfo.IconID = int.Parse(vals[40]);
            AbilityDetailInfoList.Add(_tempAbilityDeatilInfo);
        }
    }
	
	int[] convertStringToIntArray(string _string)
	{
		int[] newArray = new int[1];
		if(_string == "")
		{
			newArray[0] = 0;
		}else
		{
			string[] _temp = _string.Split('+');
			newArray = new int[_temp.Length];
			for(int i = 0; i < _temp.Length; i++)	
			{
				newArray[i] = int.Parse(_temp[i]);
			}
		}
		return newArray;
	}

    public AbilityDetailInfo GetAbilityDetailInfoByID(int _abilityID)
    {
        foreach (AbilityDetailInfo objInfo in AbilityDetailInfoList)
        {
            if (objInfo.ID == _abilityID)
            {
                return objInfo;
            }
        }
        return null;
    }

    public AbilityDetailInfo GetAbilityDetailInfoByTypeAndLV(AbilityDetailInfo.EnumAbilityType _type, int _lv)
    {
        foreach (AbilityDetailInfo objInfo in AbilityDetailInfoList)
        {
            if (objInfo.AbilityType == _type && objInfo.Level == _lv)
            {
                return objInfo;
            }
        }
        return null;
    }
}

public class AbilityDetailInfo
{
    public enum EnumAbilityType
    {
        None = 0,
        NormalAttack = 10000,
        SwathOfDestruction = 10200,
        SeeingRed = 10300,
        HungryCleave = 10400,
        RainOfBlow = 10500,
        Shockwave = 10600,
        MeteorOfRain = 10700,
        SkinOfStone = 10800,
        HauntingScream = 10900,
        IronThrone = 11000,
        IceBarricade = 11100,
        Chiprayer = 11200,
        Chimend = 11300,
        SteadyShoot = 11400,
        StringingShoot = 11500,
        NinjaEscape = 11600,
        Caltrops = 11700,
        FirebomTrap = 11800,
        DarkHunter = 12100,
        Revive = 12001,
		ReviveSuper = 12002,
        Others = 20000,
        Max = 99999,
    }

    public enum EnumProgramType
    {
        CommonAttack,
        Normal,
        Shoot,
        Teleport,
    }

    public enum EnumTargetType
    {
        Enemy,
        All,
        Self,
        Self_Ally,
    }

    public enum EnumDMGType
    {
        Weapon,
        Body,
        Value,
        Percent,
    }

    public enum EnumAttackPosType
    {
        Single,
        CurrentPos,
        MapPos,
    }

    public enum EnumAttackAreaType
    {
        Single,
        Circle,
        Half_Circle,
        Square,
        Dash_Square,
    }

    public enum EDisciplineType
    {
        EDT_NormalAttack = 0,
        EDT_Prowess = 1,
        EDT_Fortitude = 2,
        EDT_Cunning = 4,
    }

    public int ID;
    public string Name;
    public EnumAbilityType AbilityType;
    public EnumProgramType ProgramType;
    public EnumTargetType TargetType;
    public EStatusElementType ElementType;
    public int ManaCost;
    public float PrepareTime;
    public float TimeToMaxPower;
    public int IncreamentDMG;
    public float AnimationDuration;
    public float CoolDown;
    public int AttackCount;
    public float AttackTime;                              // some ability has serveal damage point. this is the interval.
    public int AttackCoefficient;
    public EnumDMGType DMGType;
    // ---------------------------
    // for calculating damage
    public float BaseValue;
    public float AbilityBonus;
    public EAttributeType AttrType;
    // end
    // ---------------------------
    public EnumAttackPosType AttackPosType;
    public EnumAttackAreaType AttackAreaType;
    public float StartDistance;
    public float EndDistance;
    public int Level;										// train requirment points.
    public EDisciplineType Discipline;
    public int Karma;
    public int[] ObjectID;
    public int[] SelfStatusID;
    public int[] DestStatusID;
	public float TrainingTime;
    public string Description1;
    public string Value1Prefix;
    public string Value1Suffix;
    public string Value2Prefix;
    public string Value2Suffix;
    public string AddEffectTitle1;
    public string AddEffectDesc1;
    public string AddEffectTitle2;
    public string AddEffectDesc2;  
	public string shortName;  
	public string Extra;
    public int IconID;
}

public class AbilityObjectLoader
{
    public static AbilityObjectLoader Instance;
    public List<AbilityObjectInfo> AbilityObjectInfoList = new List<AbilityObjectInfo>();

    public void LoadAbilityObjectInfo()
    {
		string fileName = LocalizeManage.Instance.GetLangPath("Ability.Object");
        TextAsset item = (TextAsset)Resources.Load(fileName, typeof(TextAsset));

        string[] itemRowsList = item.text.Split('\n');

        for (int i = 3; i < itemRowsList.Length - 1; ++i)
        {
            string pp = itemRowsList[i];
            string[] vals = pp.Split('	');
            AbilityObjectInfo _tempAbilityObjectInfo = new AbilityObjectInfo();
            _tempAbilityObjectInfo.ObjectTypeID = int.Parse(vals[0]);
            _tempAbilityObjectInfo.ObjectType = (AbilityObjectInfo.EnumObjectType)int.Parse(vals[1]);
            _tempAbilityObjectInfo.DelayTime = int.Parse(vals[2]) / 1000.0f;
            _tempAbilityObjectInfo.LifeTime = int.Parse(vals[3]) / 1000.0f;
            _tempAbilityObjectInfo.Param = float.Parse(vals[4]);

            AbilityObjectInfoList.Add(_tempAbilityObjectInfo);
        }
    }

    public AbilityObjectInfo GetObjectInfoByID(int _objTypeID)
    {
        foreach (AbilityObjectInfo objInfo in AbilityObjectInfoList)
        {
            if (objInfo.ObjectTypeID == _objTypeID)
            {
                return objInfo;
            }
        }
        return null;
    }
}

public class AbilityObjectInfo
{
    public enum EnumObjectType
    {
        Bullet,
        Trap,
        TrapOnce,
    }

    public int ObjectTypeID;
    public EnumObjectType ObjectType;
    public float DelayTime;
    public float LifeTime;
    public float Param;                     // bullet : speed // trap : area

}