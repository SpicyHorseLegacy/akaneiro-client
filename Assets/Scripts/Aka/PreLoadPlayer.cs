using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
//using UnityEditor;


public class PreLoadPlayer : MonoBehaviour {
	
	bool newCharacterRequested = true;
	[HideInInspector]
	public bool usingLatestConfig = false;
	//GameObject character;
	
	[HideInInspector]
	//public CharacterGenerator generator;
	
	//[HideInInspector]
	public GameObject character;

    public  string prefName = "AKACHA";
	
	void Start () {
	
		
	}
	
	// Update is called once per frame
    void Update()
    {

        if (!CharacterGenerator.ReadyToUse)
            return;

        if (Player.Instance.EquipementMan.generator == null)
        {
            Player.Instance.EquipementMan.generator = CharacterGenerator.CreateWithRandomConfig("ch_aka_f");
        }

        if (Player.Instance.EquipementMan.generator == null) return;

        if (newCharacterRequested)
        {
            if (!Player.Instance.EquipementMan.generator.ConfigReady)
                return;

            newCharacterRequested = false;

            character = Player.Instance.EquipementMan.generator.Generate();

            Player.Instance.AnimationModel = character.transform;

            if (Player.Instance.abilityManager)
                Player.Instance.abilityManager.SetAllAbilities();

            character.transform.position = transform.position + Vector3.up * 0.05f;
            character.transform.rotation = transform.rotation;
            character.transform.parent = transform;
            character.layer = LayerMask.NameToLayer("Player");
            character.name = "Aka_Model";
            //character.AddComponent<AudioListener>();
            //if(BGMInfo.Instance && BGMInfo.Instance.transform.GetComponent<AudioListener>())
            //	Destroy(BGMInfo.Instance.transform.GetComponent<AudioListener>());
            character.AddComponent<PlayerAnimation>();

            transform.GetComponent<PlayerMovement>().PlayerObj = character.transform;
            //			SurveillanceCamera.Instance.transform.parent = character.transform;

            //set animation
            character.animation["Aka_1H_Idle_1"].layer = -1;
            character.animation["Aka_1H_Idle_1"].wrapMode = WrapMode.Loop;
            character.animation["Aka_2H_Idle_1"].layer = -1;
            character.animation["Aka_2H_Idle_1"].wrapMode = WrapMode.Loop;
            character.animation["Aka_2HNodachi_Idle_1"].layer = -1;
            character.animation["Aka_2HNodachi_Idle_1"].wrapMode = WrapMode.Loop;

            character.animation["Aka_1H_Run"].layer = -1;
            character.animation["Aka_1H_Run"].wrapMode = WrapMode.Loop;
            character.animation["Aka_2H_Run"].layer = -1;
            character.animation["Aka_2H_Run"].wrapMode = WrapMode.Loop;
            character.animation["Aka_2HNodachi_Run"].layer = -1;
            character.animation["Aka_2HNodachi_Run"].wrapMode = WrapMode.Loop;

            character.animation["Aka_1H_Attack_Idle_1"].layer = -1;
            character.animation["Aka_1H_Attack_Idle_1"].wrapMode = WrapMode.Loop;
            character.animation["Aka_2H_Attack_Idle_1"].layer = -1;
            character.animation["Aka_2H_Attack_Idle_1"].wrapMode = WrapMode.Loop;
            character.animation["Aka_2HNodachi_Attack_Idle_1"].layer = -1;
            character.animation["Aka_2HNodachi_Attack_Idle_1"].wrapMode = WrapMode.Loop;

            character.animation["Aka_0H_Attack_1"].layer = -1;
            character.animation["Aka_0H_Attack_1"].wrapMode = WrapMode.Once;
            character.animation["Aka_0H_Attack_2"].layer = -1;
            character.animation["Aka_0H_Attack_2"].wrapMode = WrapMode.Once;

            character.animation["Aka_1H_Attack_1"].layer = -1;
            character.animation["Aka_1H_Attack_1"].wrapMode = WrapMode.Once;
            character.animation["Aka_1H_Attack_2"].layer = -1;
            character.animation["Aka_1H_Attack_2"].wrapMode = WrapMode.Once;
            character.animation["Aka_1H_Attack_3"].layer = -1;
            character.animation["Aka_1H_Attack_3"].wrapMode = WrapMode.Once;
            character.animation["Aka_1H_Attack_4"].layer = -1;
            character.animation["Aka_1H_Attack_4"].wrapMode = WrapMode.Once;
            character.animation["Aka_1H_Attack_5"].layer = -1;
            character.animation["Aka_1H_Attack_5"].wrapMode = WrapMode.Once;

            character.animation["Aka_2H_Attack_1"].layer = -1;
            character.animation["Aka_2H_Attack_1"].wrapMode = WrapMode.Once;
            character.animation["Aka_2H_Attack_2"].layer = -1;
            character.animation["Aka_2H_Attack_2"].wrapMode = WrapMode.Once;
            character.animation["Aka_2H_Attack_3"].layer = -1;
            character.animation["Aka_2H_Attack_3"].wrapMode = WrapMode.Once;
            character.animation["Aka_2H_Attack_4"].layer = -1;
            character.animation["Aka_2H_Attack_4"].wrapMode = WrapMode.Once;

            character.animation["Aka_2HNodachi_Attack_1"].layer = -1;
            character.animation["Aka_2HNodachi_Attack_1"].wrapMode = WrapMode.Once;
            character.animation["Aka_2HNodachi_Attack_2"].layer = -1;
            character.animation["Aka_2HNodachi_Attack_2"].wrapMode = WrapMode.Once;
            character.animation["Aka_2HNodachi_Attack_3"].layer = -1;
            character.animation["Aka_2HNodachi_Attack_3"].wrapMode = WrapMode.Once;
            character.animation["Aka_2HNodachi_Attack_4"].layer = -1;
            character.animation["Aka_2HNodachi_Attack_4"].wrapMode = WrapMode.Once;

            character.animation["Aka_1H_Damage_Lt"].layer = 1;
            character.animation["Aka_1H_Damage_Lt"].wrapMode = WrapMode.Once;
            character.animation["Aka_2H_Damage_Lt"].layer = 1;
            character.animation["Aka_2H_Damage_Lt"].wrapMode = WrapMode.Once;
            character.animation["Aka_2HNodachi_Damage_Lt"].layer = 1;
            character.animation["Aka_2HNodachi_Damage_Lt"].wrapMode = WrapMode.Once;

            character.animation["Aka_1H_Death_1"].layer = -1;
            character.animation["Aka_1H_Death_1"].wrapMode = WrapMode.Once;
            character.animation["Aka_2H_Death_1"].layer = -1;
            character.animation["Aka_2H_Death_1"].wrapMode = WrapMode.Once;
            character.animation["Aka_2HNodachi_Death_1"].layer = -1;
            character.animation["Aka_2HNodachi_Death_1"].wrapMode = WrapMode.Once;

            character.animation["Aka_0H_Stun_1"].layer = -1;
            character.animation["Aka_0H_Stun_1"].wrapMode = WrapMode.Loop;
            character.animation["Aka_1H_Stun_1"].layer = -1;
            character.animation["Aka_1H_Stun_1"].wrapMode = WrapMode.Loop;
            character.animation["Aka_2H_Stun_1"].layer = -1;
            character.animation["Aka_2H_Stun_1"].wrapMode = WrapMode.Loop;
            character.animation["Aka_2HNodachi_Stun_1"].layer = -1;
            character.animation["Aka_2HNodachi_Stun_1"].wrapMode = WrapMode.Loop;

            character.animation["Aka_0H_KnockBack_1"].layer = -1;
            character.animation["Aka_0H_KnockBack_1"].speed = 3;
            character.animation["Aka_0H_KnockBack_1"].wrapMode = WrapMode.Once;
            character.animation["Aka_1H_KnockBack_1"].layer = -1;
            character.animation["Aka_1H_KnockBack_1"].speed = 3;
            character.animation["Aka_1H_KnockBack_1"].wrapMode = WrapMode.Once;
            character.animation["Aka_2H_KnockBack_1"].layer = -1;
            character.animation["Aka_2H_KnockBack_1"].speed = 3;
            character.animation["Aka_2H_KnockBack_1"].wrapMode = WrapMode.Once;
            character.animation["Aka_2HNodachi_KnockBack_1"].layer = -1;
            character.animation["Aka_2HNodachi_KnockBack_1"].speed = 3;
            character.animation["Aka_2HNodachi_KnockBack_1"].wrapMode = WrapMode.Once;

            character.animation["Aka_Bow_Idle"].layer = -1;
            character.animation["Aka_Bow_Idle"].wrapMode = WrapMode.Loop;
            character.animation["Aka_Bow_Mount"].layer = -1;
            character.animation["Aka_Bow_Mount"].wrapMode = WrapMode.Once;
            character.animation["Aka_Bow_Shoot"].layer = -1;
            character.animation["Aka_Bow_Charging"].layer = -1;
            character.animation["Aka_Bow_Charging"].wrapMode = WrapMode.Once;
            character.animation["Aka_Bow_Shoot"].wrapMode = WrapMode.Once;
            character.animation["Aka_1H_Sweep"].layer = -1;
            character.animation["Aka_1H_Sweep"].wrapMode = WrapMode.Once;

            character.animation["Aka_RainofBlows_0H"].layer = -1;
            character.animation["Aka_RainofBlows_0H"].wrapMode = WrapMode.Once;
            character.animation["Aka_RainofBlows_1H"].layer = -1;
            character.animation["Aka_RainofBlows_1H"].wrapMode = WrapMode.Once;
            character.animation["Aka_RainofBlows_2H"].layer = -1;
            character.animation["Aka_RainofBlows_2H"].wrapMode = WrapMode.Once;
            character.animation["Aka_RainofBlows_2HNodachi"].layer = -1;
            character.animation["Aka_RainofBlows_2HNodachi"].wrapMode = WrapMode.Once;

            character.animation["AKA_HauntingScream"].layer = -1;
            character.animation["AKA_HauntingScream"].wrapMode = WrapMode.Once;
            //character.animation["AKA_HauntingScream_1H"].layer = -1;
            //character.animation["AKA_HauntingScream_1H"].wrapMode = WrapMode.Once;
            //character.animation["AKA_HauntingScream_2H"].layer = -1;
            //character.animation["AKA_HauntingScream_2H"].wrapMode = WrapMode.Once;
            //character.animation["AKA_HauntingScream_2HNodachi"].layer = -1;
            //character.animation["AKA_HauntingScream_2HNodachi"].wrapMode = WrapMode.Once;

            character.animation["AKA_Caltrops"].layer = -1;
            character.animation["AKA_Caltrops"].wrapMode = WrapMode.Once;
            character.animation["AKA_Trap_A"].layer = -1;
            character.animation["AKA_Trap_A"].wrapMode = WrapMode.Once;
            character.animation["AKA_Trap_B"].layer = -1;
            character.animation["AKA_Trap_B"].wrapMode = WrapMode.Loop;
            character.animation["AKA_Trap_C"].layer = -1;
            character.animation["AKA_Trap_C"].wrapMode = WrapMode.Once;

            character.animation["Aka_MeteorRain_A"].layer = -1;
            character.animation["Aka_MeteorRain_A"].wrapMode = WrapMode.Once;
            character.animation["Aka_MeteorRain_B"].layer = -1;
            character.animation["Aka_MeteorRain_B"].wrapMode = WrapMode.Once;

            character.animation["Aka_HungryCleave_0H"].layer = -1;
            character.animation["Aka_HungryCleave_0H"].wrapMode = WrapMode.Once;
            character.animation["Aka_HungryCleave_1H"].layer = -1;
            character.animation["Aka_HungryCleave_1H"].wrapMode = WrapMode.Once;
            character.animation["Aka_HungryCleave_2H"].layer = -1;
            character.animation["Aka_HungryCleave_2H"].wrapMode = WrapMode.Once;
            character.animation["Aka_HungryCleave_2HNodachi"].layer = -1;
            character.animation["Aka_HungryCleave_2HNodachi"].wrapMode = WrapMode.Once;

            character.animation["Aka_ChiPrayer_B"].layer = -1;
            character.animation["Aka_ChiPrayer_B"].wrapMode = WrapMode.Once;

            character.animation["Aka_Shockwave_0H"].layer = -1;
            character.animation["Aka_Shockwave_0H"].wrapMode = WrapMode.Once;
            character.animation["Aka_Shockwave_1H"].layer = -1;
            character.animation["Aka_Shockwave_1H"].wrapMode = WrapMode.Once;
            character.animation["Aka_Shockwave_2H"].layer = -1;
            character.animation["Aka_Shockwave_2H"].wrapMode = WrapMode.Once;
            character.animation["Aka_Shockwave_2HNodachi"].layer = -1;
            character.animation["Aka_Shockwave_2HNodachi"].wrapMode = WrapMode.Once;

            character.animation["Aka_SwathD_0H_A"].layer = -1;
            character.animation["Aka_SwathD_0H_A"].wrapMode = WrapMode.Once;
            character.animation["Aka_SwathD_1H_A"].layer = -1;
            character.animation["Aka_SwathD_1H_A"].wrapMode = WrapMode.Once;
            character.animation["Aka_SwathD_2H_A"].layer = -1;
            character.animation["Aka_SwathD_2H_A"].wrapMode = WrapMode.Once;
            character.animation["Aka_SwathD_2HNodachi_A"].layer = -1;
            character.animation["Aka_SwathD_2HNodachi_A"].wrapMode = WrapMode.Once;
            character.animation["Aka_SwathD_0H_B"].layer = -1;
            character.animation["Aka_SwathD_0H_B"].wrapMode = WrapMode.Loop;
            character.animation["Aka_SwathD_1H_B"].layer = -1;
            character.animation["Aka_SwathD_1H_B"].wrapMode = WrapMode.Loop;
            character.animation["Aka_SwathD_2H_B"].layer = -1;
            character.animation["Aka_SwathD_2H_B"].wrapMode = WrapMode.Loop;
            character.animation["Aka_SwathD_2HNodachi_B"].layer = -1;
            character.animation["Aka_SwathD_2HNodachi_B"].wrapMode = WrapMode.Loop;
            character.animation["Aka_SwathD_0H_C"].layer = -1;
            character.animation["Aka_SwathD_0H_C"].wrapMode = WrapMode.Once;
            character.animation["Aka_SwathD_1H_C"].layer = -1;
            character.animation["Aka_SwathD_1H_C"].wrapMode = WrapMode.Once;
            character.animation["Aka_SwathD_2H_C"].layer = -1;
            character.animation["Aka_SwathD_2H_C"].wrapMode = WrapMode.Once;
            character.animation["Aka_SwathD_2HNodachi_C"].layer = -1;
            character.animation["Aka_SwathD_2HNodachi_C"].wrapMode = WrapMode.Once;

            character.animation["Aka_IceBarricade"].layer = -1;
            character.animation["Aka_IceBarricade"].wrapMode = WrapMode.Once;

            character.animation["AKA_Caltrops"].layer = -1;
            character.animation["AKA_Caltrops"].wrapMode = WrapMode.Once;

            character.animation["Aka_NinjiaEscape_B"].layer = -1;
            character.animation["Aka_NinjiaEscape_B"].wrapMode = WrapMode.Once;

            character.animation.Play("Aka_1H_Idle_1");

            //set idle state
            Player.Instance.bAssetBundleReady = true;
            //Player.Instance.FSM.SetCurrentState(Player.Instance.IS);
            Player.Instance.FSM.ChangeState(Player.Instance.IS);
            Player.Instance.SoundHandler.PlaySpawnOutSound();
        }
        else
        {
            if (usingLatestConfig)
            {
                if (!Player.Instance.EquipementMan.generator.ConfigReady)
                    return;
                usingLatestConfig = false;
                character = Player.Instance.EquipementMan.generator.Generate(character);
                Player.Instance.EquipementMan.UpdateVFXForArmorEnchant();
            }
        }

        if (Input.GetKeyDown(KeyCode.K))
        {
            // generator.ChangeCharactorComponent("ch_aka_sm","leg",true);

            //PlayerPrefs.SetString(prefName, generator.GetConfig());
            //usingLatestConfig = true;
            /*
            Transform Item = null;
			
            if(Player.Instance.AllPickupItems.Count > 0)
               Item = Player.Instance.AllPickupItems[0];
			 
            if(Item)
            {
               Player.Instance.DetachItem( EquipementManager.EEquipmentType.LeftHand_Weapon,Player.Instance.LeftWeapon);
               Player.Instance.AttachItem( EquipementManager.EEquipmentType.LeftHand_Weapon,Item);
            }
            */

            //Player.Instance.EquipementMan.generator.ConfigComponent(EquipementManager.EEquipmentType.Helm, "sm", _PlayerData.Instance.CharactorInfo.sex, 0);
            //usingLatestConfig = true;

            /*
            Transform root =  Instantiate(Player.Instance.EquipementMan.Armors[0].ArmorPrefabs[0]) as Transform;
			
            if(root.renderer && root.renderer.material)
            {
                generator.ConfigComponent(root.renderer.material.mainTexture.name);
                usingLatestConfig = true;

            }
			
            Object.Destroy(root.gameObject);
            */
            //generator.ConfigInNeed("head",character,root.gameObject);

        }
    }
	
	void OnDrawGizmos()
	{
		Gizmos.DrawIcon(transform.position,"AkaneiroFace1");
	}
}
