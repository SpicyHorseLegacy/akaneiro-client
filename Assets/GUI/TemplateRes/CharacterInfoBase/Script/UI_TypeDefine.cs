using UnityEngine;
using System.Collections;

namespace UI_TypeDefine {

    public enum EnumCharInfoUITYPE
    {
        NONE,
        Inventory,
        PlayerStats,
        Abilities,
        Trials,
        MAX,
        LoadingBundle,
    }

    public enum EnumAbilityShopUITYPE
    {
        NONE,
        Prowess,
        Fortitude,
        Cunning,
        MAX,
        LoadingBundle,
    }

    public enum EnumConsumableItemShopUITYPE
    {
        NONE = 0,
        Bundles = 1,
        Food = 3,
        Drink = 2,
        Materials =4,
        MAX = 99,
    }

    public enum Enum_UI_AbilityType
    {
        NONE = 0,
        P_SwathOfDestruction = 10200,
        P_HungryCleave = 10400,
        P_Shockwave = 10600,
        P_SeeingRed = 10300,
        P_RainOfBlow = 10500,
        P_MeteorRain = 10700,
        D_SkinOfStone = 10800,
        D_IronThorns = 11000,
        D_ChiPrayer = 11200,
        D_HauntingStream = 10900,
        D_RingOfFrost = 11100,
        D_ChiMend = 11300,
        S_SteadyShoot = 11400,
        S_StingingShoot = 11500,
        S_NinjaEscape = 11600,
        S_Caltrops = 11700,
        S_FirebomTrap = 11800,
        S_DarkHunter = 12100,
        MAX = 99999,
    }

    public enum ENUM_UI_CraftingShop_ItemAttribute
    {
        NONE,
        Level,
        Enchant,
        Element,
        Gem,
        MAX,
    }

    public enum ENUM_UI_Money_Type
    {
        NONE = 0,
        Karma = 1,
        Crystal = 2,
        MAX = 3,
    }

    public class UI_Money_data
    {
        public ENUM_UI_Money_Type Type;
        public string MoneyString;

        public UI_Money_data()
        {
            Type = ENUM_UI_Money_Type.Karma;
            MoneyString = "0";
        }

        public UI_Money_data(ENUM_UI_Money_Type _type)
        {
            Type = _type;
            MoneyString = "0";
        }
    }

    public class UI_AbiInfo_data
    {
        public AbilityDetailInfo.EnumAbilityType AbiType;
        public string IconSpriteName;
		public int AbiID;
        public string AbiName;
        public Color AbiColor;
        public int Level;
        public string Description;

        public bool IsLearned
        {
            get
            {
                return (Level != 0);
            }
        }
    }

    public class UI_MasteryInfo_data
    {
        public EMasteryType MasteryType;
        public string IconSpriteName;
        public string MasteryName;
        public int Level;
        public string Description;

        public bool IsLearned
        {
            get
            {
                return (Level != 0);
            }
        }
    }

    public class UI_LearnAbilityCoolDown_data
    {
        // true = ability // false = mastery
        public bool IsAbilityOrMastery;
        public AbilityDetailInfo.EnumAbilityType AbiType;
        public EMasteryType MasteryType;
        public string AbiName;
        public float CurAbiCooldown;
        public float MaxAbiCooldown;
    }

    public class UI_GameHud_Abi_data
    {
        public string IconSpriteName;
        public int AbiID;
        public float MAXCD;
        public Color AbiColor;
    }

    public class UI_GameHud_DragItem_data
    {
        public enum EnumDragItemType
        {
            NONE,
            Ability,
            AbilitySlot,
            Item,
            MAX,
        }

		public int AbiID;
        public string IconSpriteName;
        public EnumDragItemType ItemType;
        public Color ItemColor;
        public object Param = null;

        public UI_GameHud_DragItem_data(string _string)
        {
            IconSpriteName = _string;
        }
    }

    public class UI_GameHud_DamageTXT_data
    {
        public enum EnumDamageTXTAnimationType
        {
            NONE,
            LinearUp,
            LinearDown,
            Jump,
            MAX,
        }

        public string DamageTEXT;
        public Color DamageColor = Color.white;
        public bool IsCritical = false;
        public float LifeTime = 0.5f;
        public Vector3 ScaleSize = Vector3.one;
        public EnumDamageTXTAnimationType AniType;
    }

    public class UI_GameHud_TopHPBar_data
    {
        public enum EnumUI_Hud_TopHPBar_TargetType
        {
            NONE,
            Friendly,
            Monster,
            MonsterBoss,
            Interactive,
            Well,
            Switch,
            Dialog,
            MAX,
        }

        public enum EnumUI_Hud_TopHPBar_MonsterRankType
        {
            NONE,
            Normal,
            Wanted,
            Boss,
            MAX,
        }

        public string TargetName;
        public EnumUI_Hud_TopHPBar_TargetType TargetType;
        public float CurHP;
        public float MAXHP;
        public EnumUI_Hud_TopHPBar_MonsterRankType MonsterRankType; 
        public UI_GameHud_TopHPBar_ElementalIcons_data ElementalData;
    }

    public class UI_GameHud_TopHPBar_ElementalIcons_data
    {
        public bool IsFlame = false;
        public bool IsFrost = false;
        public bool IsBlast = false;
        public bool IsStorm = false;
    }

    public class UI_GameHud_AllyInfo_data
    {
        public int ID;
        public Texture2D PortraitTex;
        public int Level;
        public float CurHP;
        public float MAXHP;
        public float CurMP;
        public float MAXMP;
    }

    public class UI_AbilityShop_AbiDetail_data
    {
        public bool IsAbility;
        public string IconSpriteName;
        public int AbiID;
        public string AbiName;
        public int AbiCurLV;
        public int AbiMaxLV;
        public string AbiCurLVDescription;
        public string AbiNextLVDescription;
        public string NeedAttr;
        public string HaveAttr;
        public int TrainingTime;
        public int KarmaCost;
    }

    public class UI_SpriteShop_PetItem_data
    {
        public int PetID;
        public bool isLocked;
		public bool isKSPet;
        public int RequirdLevel;
        public string PetName;
        public string PetIconName;
        public string PetSimpleIconName;
        public int CurLevel;
        public bool IsMaxLv;
        public float CurExp;
        public float MaxExp;
        public string PetSimpleDescription;
        public string PetDetailDescription;
		public bool IsCalled;
        public long BuyTime;
		public long LastTime
		{
			get
			{
				return (BuyTime + MaxTime - PlayerDataManager.Instance.GetCurrect1970Time());
			}
		}
		public long MaxTime;
        public int Price1HourValue;
        public EMoneyType Price1HourType;
        public int Price1DayValue;
        public EMoneyType Price1DayType;
        public int Price1WeekValue;
        public EMoneyType  Price1WeekType;
    }

    public class UI_ConsumableItemShop_Item_data
    {
        public int ID;
        public string ItemName;
        public string IconString;
        public EMoneyType PriceType;
        public float Price;
        public string ShortDescription;
        public string FullDescription;
    }

    public class UI_Mailbox_Item_data
    {
        public enum EnumMailItemType
        {
            NONE,
            Item,
            Pet,
            Karma,
            Crystal,
            MAX,
        }
        public string ItemName;
        public EnumMailItemType ItemType;
        public uint Count;
        public Texture2D ItemIcon;
        public int slotID;
        public int ItemID;
        public int ItemTypeID;
        public int ItemPrefabID;
    }

    public class UI_Stash_Tab_data
    {
        public int MaxIDX;
        public int CurIDX;
        public int BoughtIDX;
    }
}
