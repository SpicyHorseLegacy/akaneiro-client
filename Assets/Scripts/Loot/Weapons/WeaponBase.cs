using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public enum EWeaponCategory
{
    None = 0,
	Katana =1001,
	Axe = 1002,
	Club = 1003,
	Mace = 1004,
	Nodachi = 1005,
	GreatAxe = 1006,
	GreatMaul = 1007,
	Sickle = 1008,
	Cutlass = 1009,
	Claymore = 1010,
	Claw = 1011,
	Fist = 1012,
    RazzleDazzle = 4001,
	Bow = 9999,
}

public class WeaponBase : Item {
	
	public enum EWeaponType
	{
		WT_NoneWeapon = 0,
		WT_OneHandWeapon,
		WT_TwoHandWeaponSword,
		WT_TwoHandWeaponAxe,
        WT_DualWeapon,
	};

    public enum EWeaponVFXSize
    {
        S = 0,
        M = 1,
        L = 2,
        XL = 3,
    }

    #region Define
    [HideInInspector]
	public string Weapon_Name;
	[HideInInspector]
	public string Attack_Speed;
	
	public float AttackSpeedFactor = 1;
	
	public EWeaponCategory WeaponCategory = EWeaponCategory.Axe;    // choose impact sound
	public EWeaponType WeaponType;
	
	[HideInInspector]
    public float Weapon_AttackRange = 1.5f;
	[HideInInspector]
	public float Weapon_AttackAngle = 90f;
	[HideInInspector]
    public int Weapon_DamageMin = 10;
	[HideInInspector]
    public int Weapon_DamageMax = 15;
	[HideInInspector]
	public int Suffix_Damage=5;
	[HideInInspector]
	public float Suffix_ChancePerHit=0.1f;
	[HideInInspector]
	public float Prefix2_ChancePerHit=1.0f;
	[HideInInspector]
	public float AdditionRate = 1;
	
	public Transform sheath;
	[HideInInspector]
	public Transform Sheahth1,Sheahth2;

	//sound prefab
	public Transform AttackSound;
	public Transform ImpactSound;   // override common impact sound
	
	Transform attack_Sound;
	
	List<Transform> ImpactSounds = new List<Transform>();

    public EWeaponVFXSize VFXSize = EWeaponVFXSize.M;
    #endregion

    void Awake()
    {
        WeaponCategory = (EWeaponCategory)TypeID;
    }

    public override void Start ()
	{
		base.Start ();
		
		ItemType = Item.EItem_Type.EItem_Weapon;
		
		if(sheath)
		{
			Sheahth1 = Object.Instantiate(sheath) as Transform;
			Sheahth1.gameObject.SetActiveRecursively(false);
			if(WeaponType == WeaponBase.EWeaponType.WT_OneHandWeapon)
			{
				Sheahth2 = Object.Instantiate(sheath) as Transform;
				Sheahth2.gameObject.SetActiveRecursively(false);
			}
		}
		
		string _fileName = LocalizeManage.Instance.GetLangPath("WeaponAttribute.weapon");
        TextAsset weaponTXT = (TextAsset)Resources.Load(_fileName, typeof(TextAsset));

        string[] weaponRowsList = weaponTXT.text.Split('\n');

        for (int i = 3; i < weaponRowsList.Length - 1; ++i)
        {
            string pp = weaponRowsList[i];
            string[] vals = pp.Split('	');
            int _id = int.Parse(vals[0]);
            if(_id == TypeID)
                AttackSpeedFactor = float.Parse(vals[vals.Length - 1]);
        }
	}
    /*
    public void OnGUI ()
    {
        if(IsEquipted) return;
		
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        int layer = 1<<LayerMask.NameToLayer("DropItem");
        if(Physics.Raycast(ray.origin,ray.direction,out hit,100f,layer))
        {
            if(hit.transform == transform)
            {
                //mouse over me, show item info
                GUI.Box(new Rect(190,85,300,300),"");
                GUI.Label(new Rect(200,90,200,20),Prefix1_Name+" "+Prefix2_Name+" "+Weapon_Name);
                GUI.Label(new Rect(200,110,200,20),"Required level: "+Prefix1_Level);
                GUI.Label(new Rect(200,130,200,20),Attack_Speed + " Attack Speed");
                GUI.Label(new Rect(200,150,200,20),"Base Damage: " + Weapon_DamageMin + " to " + Weapon_DamageMax);
                GUI.Label(new Rect(200,170,200,20),"Color: " + PrefixColor_Color);
                if(Prefix1_Level>4)
                {
                    GUI.Label(new Rect(200,170,200,20),Prefix2_Description+Prefix2_Modifier_1+"%");
                }
            }
        }
    }

    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        int layer = 1 << LayerMask.NameToLayer("DropItem");
        if (Physics.Raycast(ray.origin, ray.direction, out hit, 100f, layer))
        {
            if (hit.transform == transform)
            {

                ShowWeaponText();
                ShowTip();
            }
        }
        else
        {
            HideTip();
        }
    }
    

    public void ShowWeaponText()
	{
//		if(IsEquipted) return;
//		
//		UIButton tipTransform = InGameUIControl.Instance.LootToolTip;
//		if( tipTransform == null)
//		   return;
//		
//		string Context = Prefix1_Name+" "+Prefix2_Name+" "+Weapon_Name + "\n";
//		Context += "Required level: "+Prefix1_Level + "\n";
//		Context += Attack_Speed +  " Attack Speed" + "\n";
//		Context += "Base Damage: " + Weapon_DamageMin + " to " + Weapon_DamageMax + "\n";
//		Context += "Color: " + PrefixColor_Color + "\n";
//		if(Prefix1_Level>4)
//	    {
//		    Context += Prefix2_Description+Prefix2_Modifier_1+"%" + "\n";
//	    }
//		
//		tipTransform.Text = Context;
	}
    */

    #region PlayAttackAndImpactSound
    /// <summary>
    /// Play swipe sound.
    /// </summary>
    public void PlayAttackSound()
    {
        if (attack_Sound == null && AttackSound != null)
        {
            attack_Sound = Instantiate(AttackSound) as Transform;
            attack_Sound.position = transform.position;
            attack_Sound.parent = transform;
        }

        if (attack_Sound != null)
        {
            SoundCue.Play(attack_Sound.gameObject);
        }
    }
	
    /// <summary>
    /// Check if it's success to play impact sound to target object.
    /// </summary>
    /// <param name="target">
    /// Target object(BaseHitableObject)
    /// </param>
    /// <returns>
    /// Ture : Success
    /// False : Unsuccess
    /// </returns>
    public bool PlayImpactSound(BaseHitableObject target)
    {
        // find impact sound prefab
        Transform tempSoundPrefab = null;
        if (ImpactSound)
        {
            tempSoundPrefab = ImpactSound;
        }
        else
        {
            tempSoundPrefab = SoundEffectManager.Instance.GetImpactSoundPrefab(WeaponCategory, target.AudioArmorCategory);
        }

        // if find prefab, check if there is a instance. If so, play impact sound. Otherwise, play impact sound after creating an instance.
        if (tempSoundPrefab)
        {
            foreach (Transform sound in ImpactSounds)
            {
                if (sound.name == tempSoundPrefab.name)
                {
                    SoundCue.Play(sound.gameObject);
                    return true;
                }
            }

            Transform newImpactSound = Instantiate(tempSoundPrefab, transform.position, transform.rotation) as Transform;
            newImpactSound.parent = transform;
            newImpactSound.name = tempSoundPrefab.name;
            SoundCue.Play(newImpactSound.gameObject);
            ImpactSounds.Add(newImpactSound);
            return true;
        }
        return false;
    }
    #endregion

    #region CreateVFXForGemAndEnchant
    public void InitVFXWithGemID(int id)
	{
		MeleeWeaponTrail trail = transform.GetComponent<MeleeWeaponTrail>();
		
		if(MeleeTrailManager.Instance)
		{
			Color[] targetColors = null;
		
			switch(id)
			{
			case 0:// none
				targetColors = MeleeTrailManager.Instance.None;
				break;
				
			case 1:// Ruby
				targetColors = MeleeTrailManager.Instance.Ruby;
				break;
				
			case 2:// Sapphire
				targetColors = MeleeTrailManager.Instance.Sapphire;
				break;
				
			case 3:// Emerald
				targetColors = MeleeTrailManager.Instance.Emerald;
				break;
				
			case 4:// Garnet
				targetColors = MeleeTrailManager.Instance.Garnet;
				break;
				
			case 5:// Amethyst
				targetColors = MeleeTrailManager.Instance.Amethyst;
				break;
				
			case 6://Malachite
				targetColors = MeleeTrailManager.Instance.Malachite;
				break;
				
			case 7://Obsidian
				targetColors = MeleeTrailManager.Instance.Obsidian;
				break;
				
			case 8://Golden
				targetColors = MeleeTrailManager.Instance.Golden;
				break;
				
			case 9://Jade
				targetColors = MeleeTrailManager.Instance.Jade;
				break;
				
			default:
				targetColors = MeleeTrailManager.Instance.None;
				break;
			}
			
			if(targetColors.Length > 0)
			{
				foreach(Color c in targetColors)
				{
					trail._colors.Add(c);
				}
			}
		}
	}

    Transform enchantvfx;
	public Transform InitEnchantVFXWithEchantID(int id, Vector3 _eulerangle)
	{
        if (enchantvfx) return null;
		Transform prefab = null;

		if(id == 101 || id == 102 || id == 103 || id == 104 || id == 105 || id == 106 || id == 107 || id == 108 || id == 109 || id == 110){
            if (ItemPrefabs.Instance.EnchantRagePrefab.Length >= (int)VFXSize)
                prefab = ItemPrefabs.Instance.EnchantRagePrefab[(int)VFXSize];
		}
		if(id == 201 || id == 202 || id == 203 || id == 204 || id == 205 || id == 206 || id == 207 || id == 208 || id == 209 || id == 210){
            if (ItemPrefabs.Instance.EnchantThirstPrefab.Length >= (int)VFXSize)
                prefab = ItemPrefabs.Instance.EnchantThirstPrefab[(int)VFXSize];
		}
		if(id == 301 || id == 302 || id == 303 || id == 304 || id == 305 || id == 306 || id == 307 || id == 308 || id == 309 || id == 310){
            if (ItemPrefabs.Instance.EnchantBrutalPrefab.Length >= (int)VFXSize)
                prefab = ItemPrefabs.Instance.EnchantBrutalPrefab[(int)VFXSize];
		}
		if(prefab)
            enchantvfx = Instantiate(prefab) as Transform;

        if (enchantvfx)
		{
            enchantvfx.parent = transform;
            enchantvfx.position = transform.position;
            enchantvfx.localEulerAngles = _eulerangle;
            Transform[] children = enchantvfx.GetComponentsInChildren<Transform>();
            foreach (Transform child in children)
            {
                child.gameObject.layer = gameObject.layer;
            }
		}
		
		return prefab;
	}
	
	public void InitVFXWithItemInfo(SItemInfo itemInfo, Vector3 _eulerangle)
	{
		InitVFXWithGemID(itemInfo.gem);
		InitEnchantVFXWithEchantID(itemInfo.enchant, _eulerangle);
    }

    Vector3[,] vectors = new Vector3[10,10];

    float x = 0.4f % 0.8f;

    #endregion
}