using UnityEngine;
using System.Collections;

public class AbilitieShopItem : MonoBehaviour {
	
	public UIButton icon;
	public UIButton bg;
	public SpriteText name;
	public UIButton coolDownIcon;
	public UIButton [] stars;
	public UIButton [] starsBg;
	public int starMaxCount;
	public AbilityBaseState sinfo;
	public AbilityBaseState sinfoN;
	public SingleMastery mInfo;
	public bool isMastery = false;
	public int ilevel = 0;
	private ECooldownType type = new ECooldownType();
	// Use this for initialization
	void Start () {
		bg.AddInputDelegate(BgDelegate);
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	 
	void BgDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:	
//				LogManager.Log_Error("test 1");
				AbilitiesShop.Instance.leftPanel.BringIn();
//				LogManager.Log_Error("test 2");
				LearnSkillMsg abiInfo = new LearnSkillMsg();
				abiInfo.isMastery = isMastery;
				if(isMastery) {
					
					if(mInfo.Info == null) {
						abiInfo.abilitieID = mInfo.NextInfo.ID ;
						AbilitiesShop.Instance.UpdateAbilitieInfo((int)mInfo.NextInfo.Discipline,mInfo.BaseInfo.BaseStateLvNeeded,mInfo.Icon,mInfo.BaseInfo.shortName,0,"","","","","","",mInfo.NextInfo.Description,mInfo.NextInfo.Description2,"","","","","");
					}else {
						if(mInfo.NextInfo == null) {
							abiInfo.abilitieID = mInfo.Info.ID+1;
							AbilitiesShop.Instance.UpdateAbilitieInfo((int)mInfo.Info.Discipline,0,mInfo.Icon,mInfo.Info.shortName,mInfo.Info.MasteryLevel,mInfo.Info.Description,mInfo.Info.Description2,"","","","","","","","","","","");
						}else {
							abiInfo.abilitieID = mInfo.NextInfo.ID;
							AbilitiesShop.Instance.UpdateAbilitieInfo((int)mInfo.NextInfo.Discipline,mInfo.NextInfo.BaseStateLvNeeded,mInfo.Icon,mInfo.Info.shortName,mInfo.Info.MasteryLevel,mInfo.Info.Description,mInfo.Info.Description2,"","","","",mInfo.NextInfo.Description,mInfo.NextInfo.Description2,"","","","","");
						}	
					}	
//					abiInfo.abilitieID = mInfo.BaseInfo.ID;
					abiInfo.masteryType = mInfo.MasteryClass;
				}else {
					if(sinfoN != null && sinfo != null) {
						abiInfo.abilitieID = sinfoN.id;
					
						AbilitiesShop.Instance.UpdateAbilitieInfo((int)sinfoN.Info.Discipline,sinfoN.Info.Level,sinfo.icon,sinfo.Info.shortName,sinfo.Level,sinfo.Info.Description1,sinfo.Info.AddEffectTitle1,sinfo.Info.AddEffectDesc1
							,sinfo.Info.AddEffectTitle2,sinfo.Info.AddEffectDesc2,sinfo.Info.ManaCost.ToString(),sinfoN.Info.Value1Prefix,sinfoN.Info.AddEffectTitle1,sinfoN.Info.AddEffectDesc1,sinfoN.Info.AddEffectTitle2
							,sinfoN.Info.AddEffectDesc2,sinfoN.Info.ManaCost.ToString(),sinfoN.Info.Extra);
					}else if(sinfoN != null && sinfo == null) {
						abiInfo.abilitieID = sinfoN.id;
					
						AbilitiesShop.Instance.UpdateAbilitieInfo((int)sinfoN.Info.Discipline,sinfoN.Info.Level,sinfoN.icon,sinfoN.Info.shortName,0,"","",""
							,"","","",sinfoN.Info.Description1,sinfoN.Info.Value1Prefix,sinfoN.Info.AddEffectDesc1,sinfoN.Info.AddEffectTitle2
							,sinfoN.Info.AddEffectDesc2,sinfoN.Info.ManaCost.ToString(),sinfoN.Info.Extra);
					}else {
						abiInfo.abilitieID = 0;
						AbilitiesShop.Instance.UpdateAbilitieInfo((int)sinfo.Info.Discipline,0,sinfo.icon,sinfo.Info.shortName,sinfo.Level,sinfo.Info.Value1Prefix,sinfo.Info.AddEffectTitle1,sinfo.Info.AddEffectDesc1
							,sinfo.Info.AddEffectTitle2,sinfo.Info.AddEffectDesc2,sinfo.Info.ManaCost.ToString(),"","","","","","",sinfo.Info.Extra);
					}
				}
			
				if(isMastery) {
					type.Set(ECooldownType.eCooldownType_Mastery);
					//is already learn to max level.//
					if(mInfo.NextInfo != null) {
						//is cool down.//
						if(GetIsCoolDownAbi()) {
							//calc speed time.//
							AbilitiesShop.Instance.UpdateInfoBtn(GetIsCoolDownAbi(),mInfo.NextInfo.karmaCost,AbilitiesShop.Instance.GetCoolDownTime(type,mInfo.NextInfo.ID-1));
						}else {
							//calc learn time.//
							AbilitiesShop.Instance.UpdateInfoBtn(GetIsCoolDownAbi(),mInfo.NextInfo.karmaCost,(long)mInfo.NextInfo.TrainTime);
						}
					}else {
						AbilitiesShop.Instance.UpdateInfoBtn(GetIsCoolDownAbi(),0,0);
					}
				}else {
					type.Set(ECooldownType.eCooldownType_Skill);
					//is already learn to max level.//
					if(sinfoN != null) {
						//is cool down.//
						if(GetIsCoolDownAbi()) {
							//calc speed time.//
							if(sinfo != null) {
								long tempTime = AbilitiesShop.Instance.GetCoolDownTime(type,sinfo.id);
								AbilitiesShop.Instance.UpdateInfoBtn(true,sinfoN.Info.Karma,tempTime);
							}else {
								long tempTime = AbilitiesShop.Instance.GetCoolDownTime(type,sinfoN.id-1);
								AbilitiesShop.Instance.UpdateInfoBtn(true,sinfoN.Info.Karma,tempTime);
							}
//							LogManager.Log_Error("time### "+tempTime+"id: "+sinfoN.id);
						}else {
							//calc learn time.//
							AbilitiesShop.Instance.UpdateInfoBtn(false,sinfoN.Info.Karma,(long)sinfoN.Info.TrainingTime);
						}
					}else {
						AbilitiesShop.Instance.UpdateInfoBtn(GetIsCoolDownAbi(),0,0);
					}
				}
			
				AbilitiesShop.Instance.UpdateCurrentSelectSkillInfo(abiInfo);
			
				break;
		}	
	}
	
	public bool GetIsCoolDownAbi() {
		bool tempFlag = false;
		ECooldownType type = new ECooldownType();
		
		if(isMastery) {
			type.Set(ECooldownType.eCooldownType_Mastery);
			if(mInfo.Info != null) {
				tempFlag = (AbilitiesShop.Instance.GetCoolDownTime(type,mInfo.Info.ID)!=0);
			}else {
				tempFlag = (AbilitiesShop.Instance.GetCoolDownTime(type,mInfo.BaseInfo.ID-1)!=0);
			}
		}else {
			type.Set(ECooldownType.eCooldownType_Skill);
			if(sinfo != null) {
				tempFlag = (AbilitiesShop.Instance.GetCoolDownTime(type,sinfo.id)!=0);
			}else {
				if(sinfoN != null) {
					tempFlag = (AbilitiesShop.Instance.GetCoolDownTime(type,sinfoN.id-1)!=0);
				}else {
					tempFlag = false;
				}
			}
		}
		return tempFlag;
	}
	
	
	public void SetIcon(Texture2D img) {
		icon.SetUVs(new Rect(0,0,1,1));
		icon.SetTexture(img);
	}
	
	public void HideCoolDown() {
		if(GetIsCoolDownAbi()) {
			coolDownIcon.gameObject.layer = LayerMask.NameToLayer("EZGUI_CanTouch");
		}else {
			coolDownIcon.gameObject.layer = LayerMask.NameToLayer("Default");
		}
	}
	
	public void SetStars(int level) {
		ilevel = level;
		for(int i = 0;i<10;i++) {
			stars[i].gameObject.layer = LayerMask.NameToLayer("Default");
		}
		for(int i = 0;i<level;i++) {
			stars[i].gameObject.layer = LayerMask.NameToLayer("EZGUI_CanTouch");
		}
		InitStarBg();
	}
	
	private void InitStarBg() {
		for(int i = 0;i<10;i++) {
			starsBg[i].gameObject.layer = LayerMask.NameToLayer("Default");
		}
		for(int i = 0;i<starMaxCount;i++) {
			starsBg[i].gameObject.layer = LayerMask.NameToLayer("EZGUI_CanTouch");
		}
	}
}
