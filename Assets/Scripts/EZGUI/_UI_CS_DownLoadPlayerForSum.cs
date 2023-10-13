using UnityEngine;
using System.Collections;

public class _UI_CS_DownLoadPlayerForSum : MonoBehaviour {
	
	//Instance
//	public static _UI_CS_DownLoadPlayerForSum Instance = null;
	
	[HideInInspector]
	public CharacterGenerator generator;
	[HideInInspector]
	public bool usingLatestConfig = false;
	
	public bool sex = true;						// true为女性， false为男性

    public EquipementManager EquipManager;

	public bool newCharacterRequested = true;
	
	[HideInInspector]
	public GameObject character;
	
	[HideInInspector]
	public Transform[] MyEquiptments = new Transform[10];
	
	void Awake(){
//		Instance = this;
	}
	
	// Use this for initialization
	void Start () {
//		print("start");
        EquipManager = gameObject.AddComponent<EquipementManager>() as EquipementManager;
        EquipManager.Owner = transform;
	}
	
	// Update is called once per frame
	void Update () {
		
		if(!CharacterGenerator.ReadyToUse)
			return;
		
		if (generator == null)
		{
			if(sex){
				generator = CharacterGenerator.CreateWithRandomConfig("ch_aka_f");
                EquipManager.generator = generator;
			}
		}
		
		if (generator == null) return;
		
		if(newCharacterRequested)
	   {
			if (!generator.ConfigReady) 
				return;
			
			newCharacterRequested = false;

			character = generator.Generate();
	
			character.transform.position = transform.position;
			character.transform.rotation = transform.rotation;
			character.transform.localScale = transform.localScale;
			character.transform.parent = transform;
			character.name = "Aka_Model";
			character.layer = LayerMask.NameToLayer("EZGUI");
			character.animation.CrossFade("Aka_1H_Idle_1");
			LoadOKDelegate();
		}
		else
		{
			if(usingLatestConfig)
			{
				if (!generator.ConfigReady) 
				    return;
				usingLatestConfig = false;
				character = generator.Generate(character);
				RestAnimation();
				LoadOKDelegate();
			}
		}
		
	}
	
	public delegate void Handle_LoadOKDelegate();
    public event Handle_LoadOKDelegate OnLoadOKDelegate;
	private void LoadOKDelegate() {
		if(OnLoadOKDelegate != null) {
			OnLoadOKDelegate();
		}
	}
	
	void OnDrawGizmos()
	{
		Gizmos.DrawIcon(transform.position,"AkaneiroFace1");
	}

	public void RestAnimation(){
		
		//Debug.Log("reset aniamtion");

        if (EquipManager.RightHandWeapon != null)
        {
            if (EquipManager.RightHandWeapon.GetComponent<WeaponBase>().WeaponType == WeaponBase.EWeaponType.WT_TwoHandWeaponSword)
            {
                character.animation["Aka_2HNodachi_Idle_1"].wrapMode = WrapMode.Loop;
                character.animation.Play("Aka_2HNodachi_Idle_1", PlayMode.StopAll);
            }
            else if (EquipManager.RightHandWeapon.GetComponent<WeaponBase>().WeaponType == WeaponBase.EWeaponType.WT_TwoHandWeaponAxe)
            {
                character.animation["Aka_2H_Idle_1"].wrapMode = WrapMode.Loop;
                character.animation.Play("Aka_2H_Idle_1", PlayMode.StopAll);
            }
            else
            {
                character.animation["Aka_1H_Idle_1"].wrapMode = WrapMode.Loop;
                character.animation.Play("Aka_1H_Idle_1", PlayMode.StopAll);
            }
        }
        else
        {
            character.animation["Aka_1H_Idle_1"].wrapMode = WrapMode.Loop;
            character.animation.Play("Aka_1H_Idle_1", PlayMode.StopAll);
        }
	}
}
