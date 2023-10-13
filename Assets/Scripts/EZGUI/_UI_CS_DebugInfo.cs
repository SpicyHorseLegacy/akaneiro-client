using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class _UI_CS_DebugInfo : MonoBehaviour {
	
	public static _UI_CS_DebugInfo Instance;
	
	public UIPanel  DebugInfoPanel;
	public bool     isBring = false;
	
	public SpriteText	CurrentMapText;
	public SpriteText	ThreatLevelText;
	public SpriteText	MissionXPText;
	public SpriteText	MissionKarmaText;
	
	public SpriteText	PlayerLevelText;
	public SpriteText	PlayerExpText;
	
	public SpriteText	PlayerHPText;
	public SpriteText	PlayerEnergyText;
	public SpriteText	BadgesText;
	
	public SpriteText	PowerText;
	public SpriteText	DefenseText;
	public SpriteText	SkillText;
	public SpriteText	FlameResText;
	public SpriteText	FlameDmgText;
	public SpriteText	FrostResText;
	public SpriteText	FrostDmgText;
	public SpriteText	BlastResText;
	public SpriteText	BlastDmgText;
	public SpriteText	StormResText;
	public SpriteText	StormDmgText;
	
	public SpriteText	Damage_ProwerText;
	public SpriteText	Damage_MultiplerText;

    public SpriteText   Damage_WPN1_NameText;
    public SpriteText Damage_WPN1_SpeedText;
	public SpriteText	Damage_WPN1_DmgText;
    public SpriteText   Damage_WPN2_NameText;
    public SpriteText   Damage_WPN2_SpeedText;
	public SpriteText	Damage_WPN2_DmgText;

    float avgDamage_WPN1;
    float avgDamage_WPN2;
	
	public SpriteText	AttackElement1Text;
	public SpriteText	AttackElement2Text;
	public SpriteText	AttackElementPercent1Text;
	public SpriteText	AttackElementPercent2Text;

    public SpriteText   Dps_WPN1_Text;
    public SpriteText   Dps_WPN2_Text;
	
	public SpriteText	Armor_DefenseText;
	public SpriteText	Armor_EquipmentText;
    public SpriteText   DamageReductionText;
	
	public SpriteText	CriticalChance_SkillText;
	public SpriteText	Critical_MultiplerText;
	
	public SpriteText	TargetNameText;
	public SpriteText	TargetHpText;
	
	public SpriteText	KillChainText;
	
	public SpriteText	PlayerLocationXText;
	public SpriteText	PlayerLocationYText;
	public SpriteText	PlayerLocationZText;

    public SpriteText IncomingDPSAll;
    public SpriteText IncomingDPSTarget;
    public List<NpcBase>  AttackPlayerEnemies = new List<NpcBase>();
    
	
	void Awake(){
		
		Instance = this;
	}
	
	// Use this for initialization
	void Start () {
		Damage_MultiplerText.Text	=  "--";
		Critical_MultiplerText.Text = "0.25";
	}
	
	// Update is called once per frame
	void Update () {
		if(isBring){
			
			PlayerLevelText.Text 			= _PlayerData.Instance.playerLevel.ToString();
			PlayerExpText.Text	 			= _PlayerData.Instance.playerCurrentExp.ToString() + " / " + _PlayerData.Instance.playerMaxExp.ToString();
			PowerText.Text 		 			= Player.Instance.AttrMan.Attrs[EAttributeType.ATTR_Power].ToString();
			DefenseText.Text 	 			= Player.Instance.AttrMan.Attrs[EAttributeType.ATTR_Defense].ToString();
			SkillText.Text 		 			= Player.Instance.AttrMan.Attrs[EAttributeType.ATTR_Skill].ToString();
			PlayerHPText.Text 	 			= Player.Instance.AttrMan.Attrs[EAttributeType.ATTR_CurHP].ToString() + " / " + Player.Instance.AttrMan.Attrs[EAttributeType.ATTR_MaxHP].ToString();
			PlayerEnergyText.Text			= Player.Instance.AttrMan.Attrs[EAttributeType.ATTR_CurMP].ToString() + " / " + Player.Instance.AttrMan.Attrs[EAttributeType.ATTR_MaxMP].ToString();
			Damage_ProwerText.Text  		= Player.Instance.AttrMan.Attrs[EAttributeType.ATTR_PhyAtk].ToString();
			Armor_DefenseText.Text  		= (Player.Instance.AttrMan.Attrs[EAttributeType.ATTR_Defense] * 6).ToString();
            DamageReductionText.Text        = (Player.Instance.AttrMan.Attrs[EAttributeType.ATTR_DamageReduction]/100.0f).ToString() + "%";
			CriticalChance_SkillText.Text   =  ClacCritical().ToString() + "%";
			
			FlameDmgText.Text 				= Player.Instance.AttrMan.EleAttrs[EStatusElementType.StatusElement_Flame].ToString();
			FlameResText.Text 				= Player.Instance.AttrMan.EleAttrs[EStatusElementType.StatusElement_FlameResist].ToString();
			FrostDmgText.Text 				= Player.Instance.AttrMan.EleAttrs[EStatusElementType.StatusElement_Frost].ToString();
			FrostResText.Text 				= Player.Instance.AttrMan.EleAttrs[EStatusElementType.StatusElement_FrostResist].ToString();
			BlastDmgText.Text 				= Player.Instance.AttrMan.EleAttrs[EStatusElementType.StatusElement_Explosion].ToString();
			BlastResText.Text 				= Player.Instance.AttrMan.EleAttrs[EStatusElementType.StatusElement_ExplosionResist].ToString();
			StormDmgText.Text 				= Player.Instance.AttrMan.EleAttrs[EStatusElementType.StatusElement_Storm].ToString();
			StormResText.Text 				= Player.Instance.AttrMan.EleAttrs[EStatusElementType.StatusElement_StormResist].ToString();
			
			KillChainText.Text				= _UI_CS_KillChain.Instance.KillChainCount.ToString();

            Damage_WPN1_SpeedText.Text = "Attack Speed : " + string.Format("{0:.00}", getWeaponAttackSpeed(0)) + " sec";
            float dpsWPN = (avgDamage_WPN1 + Player.Instance.AttrMan.Attrs[EAttributeType.ATTR_PhyAtk])/ getWeaponAttackSpeed(0);
            Dps_WPN1_Text.Text = "" + dpsWPN;
            if (Player.Instance.EquipementMan.GetWeaponType() == WeaponBase.EWeaponType.WT_TwoHandWeaponAxe || Player.Instance.EquipementMan.GetWeaponType() == WeaponBase.EWeaponType.WT_TwoHandWeaponSword || Player.Instance.EquipementMan.GetWeaponType() == WeaponBase.EWeaponType.WT_OneHandWeapon)
            {
                Damage_WPN2_SpeedText.Text = "Attack Speed : --";
                Dps_WPN2_Text.Text = "--";
            }
            else
            {
                dpsWPN = (avgDamage_WPN2 + Player.Instance.AttrMan.Attrs[EAttributeType.ATTR_PhyAtk]) / getWeaponAttackSpeed(1);
                Damage_WPN2_SpeedText.Text = "Attack Speed : " + string.Format("{0:.00}", getWeaponAttackSpeed(1)) + " sec";
                Dps_WPN2_Text.Text = "" + dpsWPN;
            }
			
			if(null != Player.Instance){
			
				PlayerLocationXText.Text	= Player.Instance.transform.position.x.ToString();
				PlayerLocationYText.Text	= Player.Instance.transform.position.y.ToString();
				PlayerLocationZText.Text	= Player.Instance.transform.position.z.ToString();
					
			}
			
            float incomingDpsAllNumber = 0;
            foreach(NpcBase npc in AttackPlayerEnemies)
            {
                incomingDpsAllNumber += npc.DPS;
            }
            IncomingDPSAll.Text = "" + incomingDpsAllNumber;

            if (Player.Instance.AttackTarget)
            {
                if (Player.Instance.AttackTarget.GetComponent<NpcBase>())
                    IncomingDPSTarget.Text = "" + Player.Instance.AttackTarget.GetComponent<NpcBase>().DPS;
            }
            else
            {
                IncomingDPSTarget.Text = "--";
            }
		}
	}
	
	public float GetDPS(bool isR){
		 float dpsWPN1 = (avgDamage_WPN1 + Player.Instance.AttrMan.Attrs[EAttributeType.ATTR_PhyAtk])/ getWeaponAttackSpeed(0);
         if(isR){
             if (Player.Instance.EquipementMan.GetWeaponType() == WeaponBase.EWeaponType.WT_NoneWeapon)
                 dpsWPN1 /= 2.0f;
			 return dpsWPN1;
		 }
         if (Player.Instance.EquipementMan.GetWeaponType() == WeaponBase.EWeaponType.WT_TwoHandWeaponAxe || Player.Instance.EquipementMan.GetWeaponType() == WeaponBase.EWeaponType.WT_TwoHandWeaponSword || Player.Instance.EquipementMan.GetWeaponType() == WeaponBase.EWeaponType.WT_OneHandWeapon)
         {
            return 0;
         }else{
             float dpsWPN2 = (avgDamage_WPN2 + Player.Instance.AttrMan.Attrs[EAttributeType.ATTR_PhyAtk]) / getWeaponAttackSpeed(1);
             if (Player.Instance.EquipementMan.GetWeaponType() == WeaponBase.EWeaponType.WT_NoneWeapon)
                 dpsWPN2 /= 2.0f;
             return dpsWPN2;
         }
	}
	
	public float ClacCritical(){
		float crit = (Player.Instance.AttrMan.Attrs[EAttributeType.ATTR_Critical] * 0.25f);
		crit = crit + (crit/(crit+135));
        return (Player.Instance.AttrMan.Attrs[EAttributeType.ATTR_Critical]/100.0f);
	}
	
	public void NotifyDebugInfo(){
		
		if(isBring){
			
			isBring = false;
			DebugInfoPanel.Dismiss();
			
		}else{
			
			isBring = true;
			DebugInfoPanel.BringIn();
			
		}
		
	}
	
	public void SetMapName(string mapName){
		
		CurrentMapText.Text = mapName;
		
	}
	
	public void SetMissionInfo(bool isShow,int level,int xp,int karma){
		
		if(!isShow){
			
			ThreatLevelText.Text = "0";
			MissionXPText.Text = "0";
//			MissionKarmaText.Text = "0";
			return;
		}
		
		ThreatLevelText.Text = level.ToString();
//		MissionXPText.Text = xp.ToString();
//		MissionKarmaText.Text = karma.ToString();
		
	}
	
	public void AddMissionXP(int xp){
	
		MissionXPText.Text = (int.Parse(MissionXPText.text) + xp).ToString();
	
	}
	
	public void AddMissionKarma(int karma){
	
		MissionKarmaText.Text = (int.Parse(MissionKarmaText.text) + karma).ToString();
	
	}
	
	public void SetBadge(int count){
	
		BadgesText.Text = count.ToString();
		
	}
	
	public void SetWpn1Info(SItemInfo itemInfo,bool isAttach){
	
		if(!isAttach){
            avgDamage_WPN1 = 0;
			Damage_WPN1_NameText.Text = "RightHand";
			Damage_WPN1_DmgText.Text = "0";
			AttackElement1Text.Text  = "None";
			AttackElementPercent1Text.Text = "0%";
			return;
		}
		
		ItemDropStruct tempItem = ItemDeployInfo.Instance.GetItemObject(itemInfo.ID,itemInfo.perfrab,itemInfo.gem,itemInfo.enchant,itemInfo.element,(int)itemInfo.level);
        Damage_WPN1_NameText.Text = "(WPN ID : " + itemInfo.ID.ToString() + ")";
		string ad = "";
		ad    =  string.Format("{0:.0}", (tempItem.info_MinAtc * tempItem.info_Modifier )) + " - " + string.Format("{0:.0}", (tempItem.info_MaxAtc * tempItem.info_Modifier ));
        avgDamage_WPN1 = ((tempItem.info_MinAtc * tempItem.info_Modifier) + (tempItem.info_MaxAtc * tempItem.info_Modifier)) / 2;
        ad += "(avg : " + avgDamage_WPN1 + ")";
		Damage_WPN1_DmgText.Text = ad;
		
		
		if(0 != tempItem._EleID){
			AttackElement1Text.Text  = tempItem.info_EleName;
			AttackElementPercent1Text.Text = tempItem.info_elePercentVal + "%";
		}else{
			AttackElement1Text.Text  = "None";
			AttackElementPercent1Text.Text = "0%";
		}
			
	}
	
	public void SetWpn2Info(SItemInfo itemInfo,bool isAttach){
	
		if(!isAttach){
            avgDamage_WPN2 = 0;
            Damage_WPN2_NameText.Text = "LeftHand";
			Damage_WPN2_DmgText.Text = "0";
			AttackElement2Text.Text  = "None";
			AttackElementPercent2Text.Text = "0%";
			return;
		}
		
		ItemDropStruct tempItem = ItemDeployInfo.Instance.GetItemObject(itemInfo.ID,itemInfo.perfrab,itemInfo.gem,itemInfo.enchant,itemInfo.element,(int)itemInfo.level);
        Damage_WPN2_NameText.Text = "(WPN ID : "  + itemInfo.ID.ToString() + ")";
		string ad = "";
		ad    =  string.Format("{0:.0}", (tempItem.info_MinAtc * tempItem.info_Modifier )) + " - " + string.Format("{0:.0}", (tempItem.info_MaxAtc * tempItem.info_Modifier ));
        avgDamage_WPN2 = ((tempItem.info_MinAtc * tempItem.info_Modifier) + (tempItem.info_MaxAtc * tempItem.info_Modifier)) / 2;
        ad += "(avg : " + avgDamage_WPN2 + ")";
		Damage_WPN2_DmgText.Text = ad;
		
		if(0 != tempItem._EleID){
			AttackElement2Text.Text  = tempItem.info_EleName;
			AttackElementPercent2Text.Text = tempItem.info_elePercentVal + "%";
		}else{
			AttackElement2Text.Text  = "None";
			AttackElementPercent2Text.Text = "0%";
		}
		
	}
	
	public void CheckEquipArmor(){
		
		float armorPoint = 0;
		
		if(!Inventory.Instance.equipmentArray[0].m_IsEmpty){
		
			ItemDropStruct tempItem = ItemDeployInfo.Instance.GetItemObject(Inventory.Instance.equipmentArray[0].m_ItemInfo.ID,
																			Inventory.Instance.equipmentArray[0].m_ItemInfo.perfrab,
																			Inventory.Instance.equipmentArray[0].m_ItemInfo.gem,
																			Inventory.Instance.equipmentArray[0].m_ItemInfo.enchant,
																			Inventory.Instance.equipmentArray[0].m_ItemInfo.element,
																			(int)Inventory.Instance.equipmentArray[0].m_ItemInfo.level);

            armorPoint += tempItem.info_MinDef * tempItem.info_Modifier;

		}
		
		if(!Inventory.Instance.equipmentArray[1].m_IsEmpty){
		
			ItemDropStruct tempItem = ItemDeployInfo.Instance.GetItemObject(Inventory.Instance.equipmentArray[1].m_ItemInfo.ID,
																			Inventory.Instance.equipmentArray[1].m_ItemInfo.perfrab,
																			Inventory.Instance.equipmentArray[1].m_ItemInfo.gem,
																			Inventory.Instance.equipmentArray[1].m_ItemInfo.enchant,
																			Inventory.Instance.equipmentArray[1].m_ItemInfo.element,
																			(int)Inventory.Instance.equipmentArray[1].m_ItemInfo.level);

            armorPoint += tempItem.info_MinDef * tempItem.info_Modifier;

		}
		
		if(!Inventory.Instance.equipmentArray[2].m_IsEmpty){
		
			ItemDropStruct tempItem = ItemDeployInfo.Instance.GetItemObject(Inventory.Instance.equipmentArray[2].m_ItemInfo.ID,
																			Inventory.Instance.equipmentArray[2].m_ItemInfo.perfrab,
																			Inventory.Instance.equipmentArray[2].m_ItemInfo.gem,
																			Inventory.Instance.equipmentArray[2].m_ItemInfo.enchant,
																			Inventory.Instance.equipmentArray[2].m_ItemInfo.element,
																			(int)Inventory.Instance.equipmentArray[2].m_ItemInfo.level);

            armorPoint += tempItem.info_MinDef * tempItem.info_Modifier;

		}
		
		if(!Inventory.Instance.equipmentArray[3].m_IsEmpty){
		
			ItemDropStruct tempItem = ItemDeployInfo.Instance.GetItemObject(Inventory.Instance.equipmentArray[3].m_ItemInfo.ID,
																			Inventory.Instance.equipmentArray[3].m_ItemInfo.perfrab,
																			Inventory.Instance.equipmentArray[3].m_ItemInfo.gem,
																			Inventory.Instance.equipmentArray[3].m_ItemInfo.enchant,
																			Inventory.Instance.equipmentArray[3].m_ItemInfo.element,
																			(int)Inventory.Instance.equipmentArray[3].m_ItemInfo.level);

            armorPoint += tempItem.info_MinDef * tempItem.info_Modifier;

		}
		
		if(!Inventory.Instance.equipmentArray[4].m_IsEmpty){
		
			ItemDropStruct tempItem = ItemDeployInfo.Instance.GetItemObject(Inventory.Instance.equipmentArray[4].m_ItemInfo.ID,
																			Inventory.Instance.equipmentArray[4].m_ItemInfo.perfrab,
																			Inventory.Instance.equipmentArray[4].m_ItemInfo.gem,
																			Inventory.Instance.equipmentArray[4].m_ItemInfo.enchant,
																			Inventory.Instance.equipmentArray[4].m_ItemInfo.element,
																			(int)Inventory.Instance.equipmentArray[4].m_ItemInfo.level);

            armorPoint += tempItem.info_MinDef * tempItem.info_Modifier;

		}
		
		if(!Inventory.Instance.equipmentArray[8].m_IsEmpty){
		
			ItemDropStruct tempItem = ItemDeployInfo.Instance.GetItemObject(Inventory.Instance.equipmentArray[8].m_ItemInfo.ID,
																			Inventory.Instance.equipmentArray[8].m_ItemInfo.perfrab,
																			Inventory.Instance.equipmentArray[8].m_ItemInfo.gem,
																			Inventory.Instance.equipmentArray[8].m_ItemInfo.enchant,
																			Inventory.Instance.equipmentArray[8].m_ItemInfo.element,
																			(int)Inventory.Instance.equipmentArray[8].m_ItemInfo.level);
			
			armorPoint += tempItem.info_MinDef * tempItem.info_Modifier;

		}
		
		Armor_EquipmentText.Text = armorPoint.ToString();
		
	}
	
	public void SetTargetInfo(bool isShow,string name,float currentHp,float maxHp){
		
		if(!isShow){
			TargetNameText.Text = "None";
			TargetHpText.Text	= "0 / 0";
			return;
		}
		
		TargetNameText.Text = name;
		TargetHpText.Text	= currentHp + " / " + maxHp;
	}

    float getWeaponAttackSpeed(int hand)
    {
		if(Player.Instance == null)
			return 0;
			
        float aniLength = Player.Instance.AnimationModel.animation["Aka_0H_Attack_1"].length;

        Transform wp = null;
        if (hand == 0)
            wp = Player.Instance.EquipementMan.RightHandWeapon;
        else
            wp = Player.Instance.EquipementMan.LeftHandWeapon;

        if (wp != null)
        {
            if (wp && wp.GetComponent<WeaponBase>())
            {
                switch (wp.GetComponent<WeaponBase>().WeaponType)
                {
                    case WeaponBase.EWeaponType.WT_NoneWeapon:
                        aniLength = Player.Instance.AnimationModel.animation["Aka_0H_Attack_1"].length;
                        break;

                    case WeaponBase.EWeaponType.WT_OneHandWeapon:
                    case WeaponBase.EWeaponType.WT_DualWeapon:
                        aniLength = Player.Instance.AnimationModel.animation["Aka_1H_Attack_1"].length;
                        break;

                    case WeaponBase.EWeaponType.WT_TwoHandWeaponAxe:
                        aniLength = Player.Instance.AnimationModel.animation["Aka_2H_Attack_1"].length;
                        break;

                    case WeaponBase.EWeaponType.WT_TwoHandWeaponSword:
                        aniLength = Player.Instance.AnimationModel.animation["Aka_2HNodachi_Attack_1"].length;
                        break;
                }
                aniLength /= Player.Instance.AttrMan.Attrs[EAttributeType.ATTR_AttackSpeed] / 100.0f;
                aniLength *= wp.GetComponent<WeaponBase>().AttackSpeedFactor;
            }
        }

        return aniLength;
    }
	
}
