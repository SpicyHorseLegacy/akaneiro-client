using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;
//using UnityEditor;


public class PreLoadPlayerAndriod : MonoBehaviour {

	
	bool newCharacterRequested = true;
	
	public Transform CloakPerfab;
	
	void Start () {
	 	//Player.Instance.AnimationModel = transform;
	}
	
	// Update is called once per frame
	void Update () {
		
	   if(newCharacterRequested)
	   {
		   if(Player.Instance == null)
				return;
			
	        Player.Instance.bAssetBundleReady=true;
			Player.Instance.FSM.SetCurrentState(Player.Instance.IS);
			Player.Instance.FSM.ChangeState(Player.Instance.IS);	
			//Player.Instance.PlaySpawnOutSound();
			
			//set animation
			transform.animation["Aka_1H_Idle_1"].layer = -1;
		    transform.animation["Aka_1H_Idle_1"].wrapMode= WrapMode.Loop;
		    transform.animation["Aka_1H_Run"].layer = -1;
			transform.animation["Aka_1H_Run"].wrapMode= WrapMode.Loop;
			transform.animation["Aka_1H_Attack_Idle_1"].layer = -1;
		    transform.animation["Aka_1H_Attack_Idle_1"].wrapMode= WrapMode.Loop;
		    transform.animation["Aka_1H_Attack_1"].layer = -1;
		    transform.animation["Aka_1H_Attack_1"].wrapMode = WrapMode.Once;
		    transform.animation["Aka_1H_Attack_2"].layer = -1;
		    transform.animation["Aka_1H_Attack_2"].wrapMode = WrapMode.Once;
		    transform.animation["Aka_1H_Attack_3"].layer = -1;
		    transform.animation["Aka_1H_Attack_3"].wrapMode = WrapMode.Once;
		    transform.animation["Aka_1H_Attack_4"].layer = -1;
		    transform.animation["Aka_1H_Attack_4"].wrapMode = WrapMode.Once;
		    transform.animation["Aka_1H_Attack_5"].layer = -1;
		    transform.animation["Aka_1H_Attack_5"].wrapMode = WrapMode.Once;
		    transform.animation["Aka_1H_Damage_Lt"].wrapMode = WrapMode.Once;
		    transform.animation["Aka_1H_Death_1"].layer = -1;
		    transform.animation["Aka_1H_Death_1"].wrapMode = WrapMode.Once;
			transform.animation["Aka_Bow_Idle"].layer = -1;
			transform.animation["Aka_Bow_Idle"].wrapMode = WrapMode.Loop;
			transform.animation["Aka_Bow_Mount"].layer = -1;
			transform.animation["Aka_Bow_Mount"].wrapMode = WrapMode.Once;
			transform.animation["Aka_Bow_Shoot"].layer = -1;
			transform.animation["Aka_Bow_Charging"].layer = -1;
			transform.animation["Aka_Bow_Charging"].wrapMode = WrapMode.Once;
			transform.animation["Aka_Bow_Shoot"].wrapMode = WrapMode.Once;
			transform.animation["Aka_1H_Sweep"].layer = -1;
			transform.animation["Aka_1H_Sweep"].wrapMode = WrapMode.Once;
			transform.animation["Aka_RainofBlows_1H"].layer = -1;
			transform.animation["Aka_RainofBlows_1H"].wrapMode = WrapMode.Once;
			transform.animation["AKA_Caltrops"].layer = -1;
			transform.animation["AKA_Caltrops"].wrapMode = WrapMode.Once;
			
			transform.animation.Play("Aka_1H_Idle_1");
			
			
			/*
			//Attach weapon 
			Component[] all = gameObject.GetComponentsInChildren<Component>();
			bool LeftHandFind=false;
			bool RightHandFind=false;
			foreach(Component T in all)
			{
				if(T.name == "Bip001 Prop2")
				{
					Player.Instance.EquiptWeapon(1,T.transform,1001,7,1);
					LeftHandFind=true;
				}
				if(T.name == "Bip001 Prop1")
				{
					Player.Instance.EquiptWeapon(2,T.transform,1001,7,1);
					RightHandFind=true;
				}
				if( T.name == "Bip001 Cloak")
				{
					if(CloakPerfab != null)
					{
						
					  Transform CloakInst = Instantiate(CloakPerfab) as Transform;
						
					   ArmorBase theCloak = CloakInst.GetComponent<ArmorBase>();
						
					   theCloak.Start();
						
					  //Player.Instance.AttachItem(EquipementManager.EEquipmentType.Cloak, CloakInst);
					}
				}
				
				if(RightHandFind && LeftHandFind)
					break;
			}
            */
//		    InGameUIControl.Instance.CharacterCharacterAbilities();
		
		    newCharacterRequested = false;
	   }
	
	}
	
	
	
	
}
