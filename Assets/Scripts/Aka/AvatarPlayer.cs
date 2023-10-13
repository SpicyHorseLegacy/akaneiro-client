using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvatarPlayer : MonoBehaviour {
	
    [HideInInspector]
    public static AvatarPlayer Instance;

	// Use this for initialization
	//bool mBinitial = true;
	GameObject mPlayer = null;
	Vector3 MouseDownVec;
	public Camera EZGUICamera;
	public float RotateSpeed = 5;
	public string SpecialShader = "Transparent/Cutout/Diffuse_2Sided";
	Dictionary<string,GameObject> mAvatarConfig = new Dictionary<string, GameObject>();
	string HeadPart = "head";
	string ChestPart = "chest";
	string LegPart = "leg";
	bool bChangeBody = false;
	
	bool bTouchPlayer = false;
	float hightime = 0;
	
	void Awake()
	{
		Instance = this;
	}
	
	void Start () {
		
	
	}
	
	// Update is called once per frame
	void Update () {
		
		if(mPlayer == null)
		{
	        if(Player.Instance.GetComponent<PreLoadPlayer>().character != null && Player.Instance.EquipementMan.generator != null)	
			{

                mPlayer = Player.Instance.EquipementMan.generator.Generate();
				
				//CharacterGenerator vs = Player.Instance.GetComponent<PreLoadPlayer>().generator;
				
				//Dictionary<string, CharacterElement> PlayerConfig = Player.Instance.GetComponent<PreLoadPlayer>().generator.GetCurrentConfig();
				/*	
				foreach (KeyValuePair<string,CharacterElement> category in PlayerConfig)
				{
					 SkinnedMeshRenderer smr = category.Value.GetSkinnedMeshRenderer();
					 mAvatarConfig.Add(category.Key,smr.gameObject);
					
				}*/
				
			    //mPlayer = Player.Instance.GetComponent<PreLoadPlayer>().generator.GenerateOriginal();
				
				//ConsistPart("all",null);
				
				//mPlayer.layer = transform.gameObject.layer;
				
				
				mPlayer.transform.parent = transform;
				mPlayer.transform.position = transform.position;
				
				SetLayerRecursively(mPlayer.transform, transform.gameObject.layer);
			    transform.localScale = new Vector3(5,5,5);
				
				mPlayer.animation["Aka_1H_Idle_1"].wrapMode = WrapMode.Loop;
			    //mPlayer.animation["Aka_1H_Idle_1"].layer = -1;
				
				
				for(int i = 0; i < mPlayer.renderer.materials.Length;i++)
			       mPlayer.renderer.materials[i].shader = Shader.Find(SpecialShader);

                AttachItem(EquipementManager.EEquipmentType.LeftHand_Weapon, Player.Instance.EquipementMan.LeftHandWeapon);

                AttachItem(EquipementManager.EEquipmentType.RightHand_Weapon, Player.Instance.EquipementMan.RightHandWeapon);
				
				//AttachItem(EquipementManager.EEquipmentType.Cloak,Player.Instance.EquipementMan.MyEquiptments[(int)EquipementManager.EEquipmentType.Cloak]);
				
				
				//comment it for load scene problem
//				if(Time.timeScale > 0)
//			       Time.timeScale = 0;
			   
				hightime = Time.realtimeSinceStartup;
			}
			
		}
		else
		{
			if(EZGUICamera)
			{
				if(Input.GetMouseButton(0))
				{   
			       if(bTouchPlayer)
				   {
						  
					  Vector3 offset = Input.mousePosition - MouseDownVec;
					  if(offset.x < 0)
						 transform.Rotate(Vector3.up * RotateSpeed );
					  else if(offset.x > 0)
					     transform.Rotate(Vector3.up * RotateSpeed * -1);
					}
					else
					{
					    if( Input.GetMouseButtonDown(0))
						{
				          RaycastHit hit;
				          int layer = 1<<LayerMask.NameToLayer("EZGUI");
					      Ray ray = EZGUICamera.ScreenPointToRay(Input.mousePosition);
		                  if(Physics.Raycast(ray.origin,ray.direction,out hit,100f,layer))
				          {
					        if(hit.transform == transform)
						    {
							   bTouchPlayer = true;
						    }
					      }
						}
					}
					
					MouseDownVec = Input.mousePosition;
					  
				}
				else
				{
					bTouchPlayer = false;
				}
				
			}
			
			
			if(!mPlayer.animation.IsPlaying("Aka_1H_Idle_1"))
			    mPlayer.animation.Play("Aka_1H_Idle_1");
			
		    if(Time.realtimeSinceStartup - hightime > mPlayer.animation["Aka_1H_Idle_1"].length)
			{
				mPlayer.animation["Aka_1H_Idle_1"].time = 0;
				hightime = Time.realtimeSinceStartup;
			}
			else
			{
			   mPlayer.animation["Aka_1H_Idle_1"].time = Time.realtimeSinceStartup - hightime;
			  
			}
			
			NeedforUpdateComponent();
		}
	
	}
	
	void SetLayerRecursively(Transform temp,int layer)
	{
		temp.gameObject.layer = layer;
		for(int i = 0; i < temp.GetChildCount();i++)
		{
			SetLayerRecursively(temp.GetChild(i),layer);
		}
	}
	
	
	
	public void AttachItem( EquipementManager.EEquipmentType ItemType,Transform Item)
	{
		print("Attackitem in avatar player");
	    if(!Item)
		   return;
	    
	    Transform AvatarItem = Instantiate(Item) as Transform;
	
	    AvatarItem.transform.localScale = new Vector3(1.0f,1.0f,1.0f);
	
	    if( ItemType == EquipementManager.EEquipmentType.LeftHand_Weapon || 
	        ItemType == EquipementManager.EEquipmentType.RightHand_Weapon)
	    {
	        for(int i = 0;i < AvatarItem.GetChildCount();i++)
	            Destroy(AvatarItem.GetChild(i).gameObject);
	    }
	
	    if( AvatarItem.renderer )
	    {
	       for(int i = 0; i < AvatarItem.renderer.materials.Length;i++)
		       AvatarItem.renderer.materials[i].shader = Shader.Find(SpecialShader);
		
		   AvatarItem.renderer.enabled = true;
	    }
	
	    SetLayerRecursively(AvatarItem,mPlayer.layer);
	
		Component[] all = mPlayer.GetComponentsInChildren<Component>();
	
		foreach(Component T in all)
		{
			if(T.name == "Bip001 Prop2")
			{
			    if( ItemType == EquipementManager.EEquipmentType.LeftHand_Weapon )
			    {
				    
				   AvatarItem.gameObject.SetActiveRecursively(true);
			       AvatarItem.transform.parent = T.transform;
				   AvatarItem.transform.localScale = new Vector3(1,1,1);
	               AvatarItem.transform.localPosition = Vector3.zero;
	               AvatarItem.transform.localRotation = Quaternion.identity;
				
				  
				   if( !mAvatarConfig.ContainsKey("lefthand"))
				      mAvatarConfig.Add("lefthand",AvatarItem.gameObject);
				   else
					  mAvatarConfig["lefthand"] = AvatarItem.gameObject;
				  
			    }
			    
			}
			else if(T.name == "Bip001 Prop1")
			{
				 if( ItemType == EquipementManager.EEquipmentType.RightHand_Weapon )
			     {
				    AvatarItem.gameObject.SetActiveRecursively(true);
				    AvatarItem.transform.parent = T.transform;
				    AvatarItem.transform.localScale = new Vector3(1,1,1);
				 
	                AvatarItem.transform.localPosition = Vector3.zero;
	                AvatarItem.transform.localRotation = Quaternion.identity;
				
				    if( !mAvatarConfig.ContainsKey("righthand"))
				      mAvatarConfig.Add("righthand",AvatarItem.gameObject);
				    else
					  mAvatarConfig["righthand"] = AvatarItem.gameObject;
			        
			     }
			}
		    else if( T.name == "Bip001 Cloak")
			{
			     if( ItemType == EquipementManager.EEquipmentType.Cloak )
			     {
				    AvatarItem.gameObject.SetActiveRecursively(true);
				    AvatarItem.transform.parent = T.transform;
				    
				    AvatarItem.transform.localScale = new Vector3(1,1,1);
	                AvatarItem.transform.localPosition = Vector3.zero;
	                AvatarItem.localEulerAngles = new Vector3(270, 0, 0);
				
				    if( !mAvatarConfig.ContainsKey("Cloak"))
				      mAvatarConfig.Add("Cloak",AvatarItem.gameObject);
				    else
					  mAvatarConfig["Cloak"] = AvatarItem.gameObject;
			     }
			
			}
		    
		   
		}
	  
	    switch(ItemType)
	    {
		   case EquipementManager.EEquipmentType.Helm:
		        
		        bChangeBody = true;
		        //HeadElement = Player.Instance.GetComponent<PreLoadPlayer>().generator.ConfigInNeed(AvatarItem.name,"head");

                Player.Instance.EquipementMan.generator.ConfigComponent(ItemType, AvatarItem.gameObject.name, new ESex(), 0);
			    Player.Instance.GetComponent<PreLoadPlayer>().usingLatestConfig = true;
		       
		        
		        if( !mAvatarConfig.ContainsKey(HeadPart))
			        mAvatarConfig.Add(HeadPart,AvatarItem.gameObject);
		        else
			        mAvatarConfig[HeadPart] = AvatarItem.gameObject;
		
		         AvatarItem.gameObject.SetActiveRecursively(false);
		        
                //Destroy(AvatarItem.gameObject);
		        
		        break;
		   case EquipementManager.EEquipmentType.Breastplate:
		       
		        bChangeBody = true;
		        //ChestElement = Player.Instance.GetComponent<PreLoadPlayer>().generator.ConfigInNeed(AvatarItem.name,"chest");
                Player.Instance.EquipementMan.generator.ConfigComponent(ItemType, AvatarItem.gameObject.name, new ESex(), 0);
			    Player.Instance.GetComponent<PreLoadPlayer>().usingLatestConfig = true;
		        
		        if( !mAvatarConfig.ContainsKey(ChestPart))
			        mAvatarConfig.Add(ChestPart,AvatarItem.gameObject);
		        else
			        mAvatarConfig[ChestPart] = AvatarItem.gameObject;
		
		         AvatarItem.gameObject.SetActiveRecursively(false);
		
		       // Destroy(AvatarItem.gameObject);
		        break;
		   case EquipementManager.EEquipmentType.Breeches:
		        
		        bChangeBody = true;
		       // LegElement = Player.Instance.GetComponent<PreLoadPlayer>().generator.ConfigInNeed(AvatarItem.name,"leg");
                Player.Instance.EquipementMan.generator.ConfigComponent(ItemType, AvatarItem.gameObject.name, new ESex(), 0);
			    Player.Instance.GetComponent<PreLoadPlayer>().usingLatestConfig = true;
		        
		        if( !mAvatarConfig.ContainsKey(LegPart))
			        mAvatarConfig.Add(LegPart,AvatarItem.gameObject);
		        else
			        mAvatarConfig[LegPart] = AvatarItem.gameObject;
		        
		          AvatarItem.gameObject.SetActiveRecursively(false);
		        //Destroy(AvatarItem.gameObject);
		        break;
	    }
		   
	}
	
	public void DetachItem(EquipementManager.EEquipmentType ItemType)
	{
	
		Component[] all = mPlayer.GetComponentsInChildren<Component>();
		
	    foreach(Component T in all)
	    {
		    if(T.name == "Bip001 Prop2")
		    {
			   if( ItemType == EquipementManager.EEquipmentType.LeftHand_Weapon )
			   {
					    
				   if( mAvatarConfig.ContainsKey("lefthand"))
					{
						mAvatarConfig["lefthand"].SetActiveRecursively(false);
						mAvatarConfig["lefthand"].transform.parent = null;
						Destroy(mAvatarConfig["lefthand"]);
					
					}
					  
				}
				    
		    }
		    else if(T.name == "Bip001 Prop1")
	        {
			    if( ItemType == EquipementManager.EEquipmentType.RightHand_Weapon )
				{
				  
				    if( mAvatarConfig.ContainsKey("righthand"))
					{
						mAvatarConfig["righthand"].SetActiveRecursively(false);
						mAvatarConfig["righthand"].transform.parent = null;
						Destroy(mAvatarConfig["righthand"]);
					
					}
				}
		     }
			 else if( T.name == "Bip001 Cloak")
			 {
				 if( ItemType == EquipementManager.EEquipmentType.Cloak )
				 {
					 if( mAvatarConfig.ContainsKey("Cloak"))
					 {
						mAvatarConfig["Cloak"].SetActiveRecursively(false);
						mAvatarConfig["Cloak"].transform.parent = null;
						Destroy(mAvatarConfig["Cloak"]);
					 }
				 }
			 }
				   
	     }
		 
		 switch(ItemType)
		 {
			case EquipementManager.EEquipmentType.Helm:
			  if( mAvatarConfig.ContainsKey(HeadPart))
			  {
                  Player.Instance.EquipementMan.generator.ConfigComponent(ItemType, "nj", new ESex(), 0);
				 Player.Instance.GetComponent<PreLoadPlayer>().usingLatestConfig = true;
				 bChangeBody = true;
				 mAvatarConfig[HeadPart].SetActiveRecursively(false);
				 mAvatarConfig[HeadPart].transform.parent = null;
				 Destroy(mAvatarConfig[HeadPart]);
			  }
		     break;
			case EquipementManager.EEquipmentType.Breastplate:
			  if( mAvatarConfig.ContainsKey(ChestPart))
			  {
                  Player.Instance.EquipementMan.generator.ConfigComponent(ItemType, "nj", new ESex(), 0);
				 Player.Instance.GetComponent<PreLoadPlayer>().usingLatestConfig = true;
				 bChangeBody = true;
				
				 mAvatarConfig[ChestPart].SetActiveRecursively(false);
				 mAvatarConfig[ChestPart].transform.parent = null;
				 Destroy(mAvatarConfig[ChestPart]);
			  }
			 break;
		    case EquipementManager.EEquipmentType.Breeches:
			  if( mAvatarConfig.ContainsKey(LegPart))
			  {
                  Player.Instance.EquipementMan.generator.ConfigComponent(ItemType, "nj", new ESex(), 0);
				 Player.Instance.GetComponent<PreLoadPlayer>().usingLatestConfig = true;
				 bChangeBody = true;
				
				 mAvatarConfig[LegPart].SetActiveRecursively(false);
				 mAvatarConfig[LegPart].transform.parent = null;
				 Destroy(mAvatarConfig[LegPart]);
			  }
			 break;
		 }
			
		
		
	}
	
	void NeedforUpdateComponent()
	{
		if( bChangeBody)
	    {
			if( !Player.Instance.GetComponent<PreLoadPlayer>().usingLatestConfig)
			{
			   bChangeBody = false;
               Player.Instance.EquipementMan.generator.Generate(mPlayer);
			   for(int i = 0; i < mPlayer.renderer.materials.Length;i++)
			       mPlayer.renderer.materials[i].shader = Shader.Find(SpecialShader);
				
			}
	        
	     }
	     
	}
	
	
}
