using UnityEngine;
using System.Collections;

public class PreLoadAllyNpc : MonoBehaviour {

	bool newCharacterRequested = true;
	[HideInInspector]
	public bool usingLatestConfig = false;

	[HideInInspector]
	public GameObject character;

    AllyNpc mAllyNpc = null;

	void Start () {
	
	    mAllyNpc = GetComponent<AllyNpc>();
	}
	
	// Update is called once per frame
	void Update () {
		
	   if(!CharacterGenerator.ReadyToUse)
			return;

       if (mAllyNpc.EquipementMan.generator == null)
		{
            mAllyNpc.EquipementMan.generator = CharacterGenerator.CreateWithRandomConfig("ch_aka_f");
		}

       if (mAllyNpc.EquipementMan.generator == null) 
			return;
		
	   if(newCharacterRequested)
	   {
           if (!mAllyNpc.EquipementMan.generator.ConfigReady) 
				return;
			
			newCharacterRequested = false;

            character = mAllyNpc.EquipementMan.generator.Generate();

			mAllyNpc.AnimationModel = character.transform;

			if(mAllyNpc.abilityManager)
				mAllyNpc.abilityManager.SetAllAbilities();
			
			character.transform.position = transform.position;
			character.transform.rotation = transform.rotation;
			character.transform.parent = transform;
         
			character.name = "Aka_Model";
			character.AddComponent<AllyNpcAnimation>();
			
			character.GetComponent<AllyNpcAnimation>().Executer = mAllyNpc;
			
		
			//set animation
			character.animation["Aka_1H_Idle_1"].layer = -1;
		    character.animation["Aka_1H_Idle_1"].wrapMode= WrapMode.Loop;
			character.animation["Aka_2H_Idle_1"].layer = -1;
		    character.animation["Aka_2H_Idle_1"].wrapMode= WrapMode.Loop;
			character.animation["Aka_2HNodachi_Idle_1"].layer = -1;
		    character.animation["Aka_2HNodachi_Idle_1"].wrapMode= WrapMode.Loop;
			
		    character.animation["Aka_1H_Run"].layer = -1;
			character.animation["Aka_1H_Run"].wrapMode= WrapMode.Loop;
		    character.animation["Aka_2H_Run"].layer = -1;
			character.animation["Aka_2H_Run"].wrapMode= WrapMode.Loop;
		    character.animation["Aka_2HNodachi_Run"].layer = -1;
			character.animation["Aka_2HNodachi_Run"].wrapMode= WrapMode.Loop;
			
			character.animation["Aka_1H_Attack_Idle_1"].layer = -1;
		    character.animation["Aka_1H_Attack_Idle_1"].wrapMode= WrapMode.Loop;
			character.animation["Aka_2H_Attack_Idle_1"].layer = -1;
		    character.animation["Aka_2H_Attack_Idle_1"].wrapMode= WrapMode.Loop;
			character.animation["Aka_2HNodachi_Attack_Idle_1"].layer = -1;
		    character.animation["Aka_2HNodachi_Attack_Idle_1"].wrapMode= WrapMode.Loop;
			
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
			character.animation["Aka_SwathD_0H_B"].wrapMode = WrapMode.Once;
			character.animation["Aka_SwathD_1H_B"].layer = -1;
			character.animation["Aka_SwathD_1H_B"].wrapMode = WrapMode.Once;
			character.animation["Aka_SwathD_2H_B"].layer = -1;
			character.animation["Aka_SwathD_2H_B"].wrapMode = WrapMode.Once;
			character.animation["Aka_SwathD_2HNodachi_B"].layer = -1;
			character.animation["Aka_SwathD_2HNodachi_B"].wrapMode = WrapMode.Once;
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
			
			//character.animation["Aka_Caltrops_A"].layer = -1;
			//character.animation["Aka_Caltrops_A"].wrapMode = WrapMode.Loop;
			character.animation["AKA_Caltrops"].layer = -1;
			character.animation["AKA_Caltrops"].wrapMode = WrapMode.Once;
			
			character.animation["Aka_NinjiaEscape_B"].layer = -1;
			character.animation["Aka_NinjiaEscape_B"].wrapMode = WrapMode.Once;
			
			character.animation.Play("Aka_1H_Idle_1");

			//set idle state
			mAllyNpc.bAssetBundleReady=true;
			mAllyNpc.FSM.SetCurrentState(mAllyNpc.IS);
			mAllyNpc.FSM.ChangeState(mAllyNpc.IS);	
			mAllyNpc.PlaySpawnOutSound();
		}
		else
		{
			if(usingLatestConfig)
			{
                if (!mAllyNpc.EquipementMan.generator.ConfigReady) 
				    return;
				usingLatestConfig = false;
                character = mAllyNpc.EquipementMan.generator.Generate(character);
			}
		}
	}
	
	void OnDrawGizmos()
	{
		Gizmos.DrawIcon(transform.position,"AkaneiroFace1");
	}
}
