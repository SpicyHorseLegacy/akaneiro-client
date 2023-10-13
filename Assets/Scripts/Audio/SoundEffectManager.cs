using UnityEngine;
using System.Collections;

public class SoundEffectManager : MonoBehaviour {
	
	public static SoundEffectManager Instance = null;

    public Transform ImpactSound_None_Light;
    public Transform ImpactSound_None_Medium;
    public Transform ImpactSound_None_Heavy;

	public Transform ImpactSound_Axe_Light;
	public Transform ImpactSound_Axe_Medium;
	public Transform ImpactSound_Axe_Heavy;
	
	public Transform ImpactSound_Claymore_Light;
	public Transform ImpactSound_Claymore_Medium;
	public Transform ImpactSound_Claymore_Heavy;
	
	public Transform ImpactSound_Club_Light;
	public Transform ImpactSound_Club_Medium;
	public Transform ImpactSound_Club_Heavy;
	
	public Transform ImpactSound_Cutlass_Light;
	public Transform ImpactSound_Cutlass_Medium;
	public Transform ImpactSound_Cutlass_Heavy;
	
	public Transform ImpactSound_GreatAxe_Light;
	public Transform ImpactSound_GreatAxe_Medium;
	public Transform ImpactSound_GreatAxe_Heavy;
	
	public Transform ImpactSound_GreatMaul_Light;
	public Transform ImpactSound_GreatMaul_Medium;
	public Transform ImpactSound_GreatMaul_Heavy;
	
	public Transform ImpactSound_Katana_Light;
	public Transform ImpactSound_Katana_Medium;
	public Transform ImpactSound_Katana_Heavy;
	
	public Transform ImpactSound_Mace_Light;
	public Transform ImpactSound_Mace_Medium;
	public Transform ImpactSound_Mace_Heavy;
	
	public Transform ImpactSound_Nodachi_Light;
	public Transform ImpactSound_Nodachi_Medium;
	public Transform ImpactSound_Nodachi_Heavy;
	
	public Transform ImpactSound_Sickle_Light;
	public Transform ImpactSound_Sickle_Medium;
	public Transform ImpactSound_Sickle_Heavy;
	
	public Transform ImpactSound_Claw_Light;
	public Transform ImpactSound_Claw_Medium;
	public Transform ImpactSound_Claw_Heavy;
	
	public Transform ImpactSound_Fist_Light;
    public Transform ImpactSound_Fist_Medium;
    public Transform ImpactSound_Fist_Heavy;
	
	public Transform Enemy_FlameImpactSound;
    public Transform Enemy_FrostImpactSound;
    public Transform Enemy_ExplosionImpactSound;
    public Transform Enemy_StormImpactSound;
    public Transform Enemy_Big_FlameImpactSound;
    public Transform Enemy_Big_FrostImpactSound;
    public Transform Enemy_Big_ExplosionImpactSound;
    public Transform Enemy_Big_StormImpactSound;
	
	void Awake()
	{
		Instance = this;
	}
	
	public Transform GetImpactSoundPrefab(EWeaponCategory weaponCategory, EArmorCaterogy armorCategory)
	{
		Transform tempSound = null;
		
		switch(weaponCategory)
		{
        case EWeaponCategory.None:
            {
                switch (armorCategory)
                {
                    case EArmorCaterogy.light:
                        if (ImpactSound_None_Light)
                            tempSound = ImpactSound_None_Light;
                        break;

                    case EArmorCaterogy.medium:
                        if (ImpactSound_None_Medium)
                            tempSound = ImpactSound_None_Medium;
                        break;

                    case EArmorCaterogy.heavy:
                        if (ImpactSound_None_Heavy)
                            tempSound = ImpactSound_None_Heavy;
                        break;
                }
            }
            break;

		case EWeaponCategory.Axe:
			{
				switch(armorCategory)
				{
					case EArmorCaterogy.light:
						if(ImpactSound_Axe_Light)
							tempSound = ImpactSound_Axe_Light;
						break;
						
					case EArmorCaterogy.medium:
						if(ImpactSound_Axe_Medium)
							tempSound = ImpactSound_Axe_Medium;
						break;
						
					case EArmorCaterogy.heavy:
						if(ImpactSound_Axe_Heavy)
							tempSound = ImpactSound_Axe_Heavy;
						
						break;
				}
			}
			break;
			
		case EWeaponCategory.Claymore:
			{
				switch(armorCategory)
				{
					case EArmorCaterogy.light:
						if(ImpactSound_Axe_Light)
							tempSound = ImpactSound_Claymore_Light;
						break;
						
					case EArmorCaterogy.medium:
						if(ImpactSound_Claymore_Medium)
							tempSound = ImpactSound_Claymore_Medium;
						break;
						
					case EArmorCaterogy.heavy:
						if(ImpactSound_Claymore_Heavy)
							tempSound = ImpactSound_Claymore_Heavy;
						break;
				}
			}
			break;
			
		case EWeaponCategory.Club:
			{
				switch(armorCategory)
				{
					case EArmorCaterogy.light:
						if(ImpactSound_Club_Light)
							tempSound = ImpactSound_Club_Light;
						break;
						
					case EArmorCaterogy.medium:
						if(ImpactSound_Club_Medium)
							tempSound = ImpactSound_Club_Medium;
						break;
						
					case EArmorCaterogy.heavy:
						if(ImpactSound_Club_Heavy)
							tempSound = ImpactSound_Club_Heavy;
						break;
				}
			}
			break;
			
		case EWeaponCategory.Cutlass:
			{
				switch(armorCategory)
				{
					case EArmorCaterogy.light:
						if(ImpactSound_Cutlass_Light)
							tempSound = ImpactSound_Cutlass_Light;
						break;
						
					case EArmorCaterogy.medium:
						if(ImpactSound_Cutlass_Medium)
							tempSound = ImpactSound_Cutlass_Medium;
						break;
						
					case EArmorCaterogy.heavy:
						if(ImpactSound_Cutlass_Heavy)
							tempSound = ImpactSound_Cutlass_Heavy;
						break;
				}
			}
			break;
			
		case EWeaponCategory.GreatAxe:
			{
				switch(armorCategory)
				{
					case EArmorCaterogy.light:
						if(ImpactSound_GreatAxe_Light)
							tempSound = ImpactSound_GreatAxe_Light;
						break;
						
					case EArmorCaterogy.medium:
						if(ImpactSound_GreatAxe_Medium)
							tempSound = ImpactSound_GreatAxe_Medium;
						break;
						
					case EArmorCaterogy.heavy:
						if(ImpactSound_GreatAxe_Heavy)
							tempSound = ImpactSound_GreatAxe_Heavy;
						break;
				}
			}
			break;
			
		case EWeaponCategory.GreatMaul:
			{
				switch(armorCategory)
				{
					case EArmorCaterogy.light:
						if(ImpactSound_GreatMaul_Light)
							tempSound = ImpactSound_GreatMaul_Light;
						break;
						
					case EArmorCaterogy.medium:
						if(ImpactSound_GreatMaul_Medium)
							tempSound = ImpactSound_GreatMaul_Medium;
						break;
						
					case EArmorCaterogy.heavy:
						if(ImpactSound_GreatMaul_Heavy)
							tempSound = ImpactSound_GreatMaul_Heavy;
						break;
				}
			}
			break;
			
		case EWeaponCategory.Katana:
			{
				switch(armorCategory)
				{
					case EArmorCaterogy.light:
						if(ImpactSound_Axe_Light)
							tempSound = ImpactSound_Katana_Light;
						break;
						
					case EArmorCaterogy.medium:
						if(ImpactSound_Axe_Medium)
							tempSound = ImpactSound_Katana_Medium;
						break;
						
					case EArmorCaterogy.heavy:
						if(ImpactSound_Axe_Heavy)
							tempSound = ImpactSound_Katana_Heavy;
						break;
				}
			}
			break;
			
		case EWeaponCategory.Mace:
			{
				switch(armorCategory)
				{
					case EArmorCaterogy.light:
						if(ImpactSound_Mace_Light)
							tempSound = ImpactSound_Mace_Light;
						break;
						
					case EArmorCaterogy.medium:
						if(ImpactSound_Mace_Medium)
							tempSound = ImpactSound_Mace_Medium;
						break;
						
					case EArmorCaterogy.heavy:
						if(ImpactSound_Mace_Heavy)
							tempSound = ImpactSound_Mace_Heavy;
						break;
				}
			}
			break;
			
		case EWeaponCategory.Nodachi:
			{
				switch(armorCategory)
				{
					case EArmorCaterogy.light:
						if(ImpactSound_Nodachi_Light)
							tempSound = ImpactSound_Nodachi_Light;
						break;
						
					case EArmorCaterogy.medium:
						if(ImpactSound_Nodachi_Medium)
							tempSound = ImpactSound_Nodachi_Medium;
						break;
						
					case EArmorCaterogy.heavy:
						if(ImpactSound_Nodachi_Heavy)
							tempSound = ImpactSound_Nodachi_Heavy;
						break;
				}
			}
			break;
			
		case EWeaponCategory.Sickle:
			{
				switch(armorCategory)
				{
					case EArmorCaterogy.light:
						if(ImpactSound_Sickle_Light)
							tempSound = ImpactSound_Sickle_Light;
						break;
						
					case EArmorCaterogy.medium:
						if(ImpactSound_Sickle_Medium)
							tempSound = ImpactSound_Sickle_Medium;
						break;
						
					case EArmorCaterogy.heavy:
						if(ImpactSound_Sickle_Heavy)
							tempSound = ImpactSound_Sickle_Heavy;
						break;
				}
			}
			break;
			
		case EWeaponCategory.Claw:
			{
				switch(armorCategory)
				{
					case EArmorCaterogy.light:
						if(ImpactSound_Claw_Light)
							tempSound = ImpactSound_Claw_Light;
						break;
						
					case EArmorCaterogy.medium:
						if(ImpactSound_Claw_Medium)
							tempSound = ImpactSound_Claw_Medium;
						break;
						
					case EArmorCaterogy.heavy:
						if(ImpactSound_Claw_Heavy)
							tempSound = ImpactSound_Claw_Heavy;
						break;
				}
			}
			break;
			
		case EWeaponCategory.Fist:
            {
                switch (armorCategory)
                {
                    case EArmorCaterogy.light:
                        if (ImpactSound_Fist_Light)
                            tempSound = ImpactSound_Fist_Light;
                        break;

                    case EArmorCaterogy.medium:
                        if (ImpactSound_Fist_Medium)
                            tempSound = ImpactSound_Fist_Medium;
                        break;

                    case EArmorCaterogy.heavy:
                        if (ImpactSound_Fist_Heavy)
                            tempSound = ImpactSound_Fist_Heavy;
                        break;
                }
            }
            break;
		}
		return tempSound;
	}
	public Transform PlayElementalSound(EStatusElementType element, bool _isb)
    {
        Transform _sound = null;
        switch (element.Get())
        {
            case EStatusElementType.StatusElement_Flame:
                if (_isb)
                    _sound = Enemy_Big_FlameImpactSound;
                else
                    _sound = Enemy_FlameImpactSound;
                break;

            case EStatusElementType.StatusElement_Frost:
                if (_isb)
                    _sound = Enemy_Big_FrostImpactSound;
                else
                    _sound = Enemy_FrostImpactSound;
                break;

            case EStatusElementType.StatusElement_Explosion:
                if (_isb)
                    _sound = Enemy_Big_ExplosionImpactSound;
                else
                    _sound = Enemy_ExplosionImpactSound;
                break;

            case EStatusElementType.StatusElement_Storm:
                if (_isb)
                    _sound = Enemy_Big_StormImpactSound;
                else
                    _sound = Enemy_StormImpactSound;
                break;
        }
        return _sound;
	}
	
}
