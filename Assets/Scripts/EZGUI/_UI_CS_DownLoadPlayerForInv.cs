using UnityEngine;
using System.Collections;

public class _UI_CS_DownLoadPlayerForInv : MonoBehaviour {
	
	//Instance
	public static _UI_CS_DownLoadPlayerForInv Instance = null;
	
	[HideInInspector]
	public CharacterGenerator generator;
	[HideInInspector]
	public bool usingLatestConfig = false;
	
	public bool sex = true;						// true为女性， false为男性
	
	public bool newCharacterRequested = true;
	
	[HideInInspector]
	public GameObject character;
	
	public Transform InvLight;

    public EquipementManager equipmentMan;
	
	void Awake(){
		Instance = this;
        equipmentMan = gameObject.AddComponent<EquipementManager>();
        equipmentMan.Owner = transform;
	}
	
	// Use this for initialization
	void Start () {
		print("start");
	}
	
	// Update is called once per frame
	void Update () {
		
		if(!CharacterGenerator.ReadyToUse)
			return;

        if (equipmentMan.generator == null)
		{
			if(sex){
                equipmentMan.generator = CharacterGenerator.CreateWithRandomConfig("ch_aka_f");	
				//generator = CharacterGenerator.CreateWithConfig();	
			}
		}

        if (equipmentMan.generator == null) return;
		
		if(newCharacterRequested)
	   {
           if (!equipmentMan.generator.ConfigReady) 
				return;
			
			newCharacterRequested = false;

            character = equipmentMan.generator.Generate();
	
			character.transform.position = transform.position;
			character.transform.rotation = transform.rotation;
			character.transform.localScale = transform.localScale;
			character.transform.parent = transform;
			character.name = "Aka_Model";
			character.layer = LayerMask.NameToLayer("EZGUI");
			character.animation["Aka_1H_Idle_1"].wrapMode= WrapMode.Loop;
			character.animation.CrossFade("Aka_1H_Idle_1");
		}
		else
		{
			if(usingLatestConfig)
			{
                if (!equipmentMan.generator.ConfigReady) 
				    return;
				usingLatestConfig = false;
                character = equipmentMan.generator.Generate(character);
				
				// 根据性别调整角色的比例
				//character.transform.localScale = Vector3.one;
				//if(!sex)
				//	character.transform.localScale = Vector3.one * 1.1f;
				
				RestAnimation();
			}
		}
	}
	
	public void OpenLight(){
		InvLight.GetComponent<Light>().enabled = true;
	}
	public void CloseLight(){
		InvLight.GetComponent<Light>().enabled = false;
	}
	
	void OnDrawGizmos()
	{
		Gizmos.DrawIcon(transform.position,"AkaneiroFace1");
	}

	public void RestAnimation(){
		
		//Debug.Log("reset aniamtion");

        if (equipmentMan.RightHandWeapon != null)
        {
            if (equipmentMan.RightHandWeapon.GetComponent<WeaponBase>().WeaponType == WeaponBase.EWeaponType.WT_TwoHandWeaponSword)
            {
				character.animation["Aka_2HNodachi_Idle_1"].wrapMode= WrapMode.Loop;
                character.animation.Play("Aka_2HNodachi_Idle_1", PlayMode.StopAll);
            }
            else if (equipmentMan.RightHandWeapon.GetComponent<WeaponBase>().WeaponType == WeaponBase.EWeaponType.WT_TwoHandWeaponAxe)
            {
				character.animation["Aka_2H_Idle_1"].wrapMode= WrapMode.Loop;
                character.animation.Play("Aka_2H_Idle_1", PlayMode.StopAll);
            }
            else
            {
            	character.animation["Aka_1H_Idle_1"].wrapMode= WrapMode.Loop;
                character.animation.Play("Aka_1H_Idle_1", PlayMode.StopAll);
            }
        }
        else
        {
			character.animation["Aka_1H_Idle_1"].wrapMode= WrapMode.Loop;
            character.animation.Play("Aka_1H_Idle_1", PlayMode.StopAll);
        }
	}
}
