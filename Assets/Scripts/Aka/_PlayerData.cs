using UnityEngine;
using System.Collections;
using System;

public class _PlayerData : MonoBehaviour {

	//Instance
	public static _PlayerData Instance = null;

	public int      playerLevel;
	public long     playerCurrentExp;
	public long     playerMaxExp;
	
	public SCharacterInfoBasic CharactorInfo;
	public int[] BaseAttrs = new int[EAttributeType.ATTR_Max];
	///////////////////////////////////////////////////////////////
	public SpriteText    	LevelText;
//	public SpriteText    	LevelExText;
	public UIProgressBar    ExpBar;
	public SpriteText    	POWERText;
	public SpriteText    	DefenseText;
	public SpriteText    	HpText;
	public SpriteText    	MpText; 
	public SpriteText    	CritText;
	public SpriteText    	NameText;
	public SpriteText    	CriticalChanceText;
	public SpriteText    	CritacalDmgBonusText;
	public SpriteText    	DpsLText;
	public SpriteText    	DpsRText;
	public SpriteText    	DmgReductionText;
	public SpriteText    	FlameDmgText;
	public SpriteText    	FlameResText;
	public SpriteText    	FrostDmgText;
	public SpriteText    	FrostResText;
	public SpriteText    	BlastDmgText;
	public SpriteText    	BlastResText;
	public SpriteText    	StormDmgText;
	public SpriteText    	StormResText;
	
	public SAccountInfo		myAccountInfo;
	
	public SpriteText    	ExpShowText;
	
	public SpriteText    	playerReviveItemNumText;
	public int    			playerReviveItemNum;
	
	public Texture2D [] PlayerIcon;
	
	void Awake(){
		Instance = this;
	}
	
	public long ReadMaxExpVal(int level) {
		string fileName = null;
		fileName = "Level.level";
		string _fileName = LocalizeManage.Instance.GetLangPath(fileName);
		if(null != fileName){
		TextAsset item = (TextAsset)Resources.Load(_fileName, typeof(TextAsset));
			string[] itemRowsList = item.text.Split('\n');
			for (int i = 0; i < itemRowsList.Length; ++i){
				string pp = itemRowsList[i];
				string[] ppList = pp.Split('\t');
				if(ppList[0].Equals((level+1).ToString())) {
				   	string[] vals = pp.Split(new char[] { '	', '	' });	
						return long.Parse(vals[1]);
				}
			}
		}
		return 0;
	}
	
	public long readCurExpVal(int level) {
		string fileName = null;
		fileName = "Level.level";
		string _fileName = LocalizeManage.Instance.GetLangPath(fileName);
		if(null != fileName){
		TextAsset item = (TextAsset)Resources.Load(_fileName, typeof(TextAsset));
			string[] itemRowsList = item.text.Split('\n');
			for (int i = 0; i < itemRowsList.Length; ++i){
				string pp = itemRowsList[i];
				string[] ppList = pp.Split('\t');
				if(ppList[0].Equals((level).ToString())) {
				   	string[] vals = pp.Split(new char[] { '	', '	' });	
						return long.Parse(vals[1]);
				}
			}
		}
		return 0;
	}
	
	public int ReadHpIncVal(int level){
	
		string fileName = null;
		int hpS = 0;
		int hpD = 0;
		int hpF = 0;
		fileName = "Level.level";
		
		string _fileName = LocalizeManage.Instance.GetLangPath(fileName);
		
		if(null != fileName){
		
		TextAsset item = (TextAsset)Resources.Load(_fileName, typeof(TextAsset));

				string[] itemRowsList = item.text.Split('\n');
		
				for (int i = 0; i < itemRowsList.Length; ++i){
					
					string pp = itemRowsList[i];
					
					string[] ppList = pp.Split('\t');
				
					if(ppList[0].Equals((level-1).ToString())) {
						
					   	string[] vals = pp.Split(new char[] { '	', '	' });	

							hpS =  int.Parse(vals[4]);

					}
				
					if(ppList[0].Equals((level).ToString())) {
						
					   	string[] vals = pp.Split(new char[] { '	', '	' });	

							hpD =  int.Parse(vals[4]);
					
							hpF = hpD - hpS;
							return hpF;
					}
				}
		}
		
		
		
		return 0;
		
	}
	
	public void SetPlayerName(string name){
		NameText.Text 		= name;
	}
	
	public void UpdatePlayerInfo(){

		playerMaxExp 		= ReadMaxExpVal(playerLevel);
		long curexp			= readCurExpVal(playerLevel);
		LevelText.Text 		= playerLevel.ToString();
		float exp 			= (float)(playerCurrentExp-curexp)/(float)(playerMaxExp-curexp);
		
		ExpBar.Value 		= exp;
		
		ExpShowText.Text 	= (playerCurrentExp-curexp).ToString() + " / " + (playerMaxExp-curexp).ToString();
		
		DpsRText.Text		= ((int)_UI_CS_DebugInfo.Instance.GetDPS(true)).ToString();
		int dpsL = (int)_UI_CS_DebugInfo.Instance.GetDPS(false);
		if(0 == dpsL){
			DpsLText.Text		= "None";
		}else{
			DpsLText.Text		= dpsL.ToString();
		}

		if((Player.Instance.AttrMan.Attrs[EAttributeType.ATTR_Power]-BaseAttrs[EAttributeType.ATTR_Power]) != 0){
			POWERText.Text 		= BaseAttrs[EAttributeType.ATTR_Power].ToString() + " ( +" + (Player.Instance.AttrMan.Attrs[EAttributeType.ATTR_Power]-BaseAttrs[EAttributeType.ATTR_Power]).ToString() + " )";
		}else{
			POWERText.Text 		= Player.Instance.AttrMan.Attrs[EAttributeType.ATTR_Power].ToString();
		}
		if((Player.Instance.AttrMan.Attrs[EAttributeType.ATTR_Defense]-BaseAttrs[EAttributeType.ATTR_Defense]) != 0){
			DefenseText.Text 	= BaseAttrs[EAttributeType.ATTR_Defense].ToString() + " ( +" + (Player.Instance.AttrMan.Attrs[EAttributeType.ATTR_Defense]-BaseAttrs[EAttributeType.ATTR_Defense]).ToString() + " )";
		}else{
			DefenseText.Text 	= Player.Instance.AttrMan.Attrs[EAttributeType.ATTR_Defense].ToString();
		}
		if((Player.Instance.AttrMan.Attrs[EAttributeType.ATTR_Skill]-BaseAttrs[EAttributeType.ATTR_Skill]) != 0){
			CritText.Text		= BaseAttrs[EAttributeType.ATTR_Skill].ToString() + " ( +" + (Player.Instance.AttrMan.Attrs[EAttributeType.ATTR_Skill]-BaseAttrs[EAttributeType.ATTR_Skill]).ToString() + " )";
		}else{
			CritText.Text		= Player.Instance.AttrMan.Attrs[EAttributeType.ATTR_Skill].ToString();
		}
		if((Player.Instance.AttrMan.Attrs[EAttributeType.ATTR_MaxHP]-BaseAttrs[EAttributeType.ATTR_MaxHP]) != 0){
			HpText.Text 		= Player.Instance.AttrMan.Attrs[EAttributeType.ATTR_CurHP].ToString() + " / ( " + BaseAttrs[EAttributeType.ATTR_MaxHP].ToString() + " + " + (Player.Instance.AttrMan.Attrs[EAttributeType.ATTR_MaxHP]-BaseAttrs[EAttributeType.ATTR_MaxHP]).ToString() + " )";
		}else{
			HpText.Text 		= Player.Instance.AttrMan.Attrs[EAttributeType.ATTR_CurHP].ToString() + " / " + Player.Instance.AttrMan.Attrs[EAttributeType.ATTR_MaxHP].ToString();
		}
		if((Player.Instance.AttrMan.Attrs[EAttributeType.ATTR_MaxMP]-BaseAttrs[EAttributeType.ATTR_MaxMP]) != 0){
			MpText.Text 		= Player.Instance.AttrMan.Attrs[EAttributeType.ATTR_CurMP].ToString() + " / ( " + BaseAttrs[EAttributeType.ATTR_MaxMP].ToString() + " + " + (Player.Instance.AttrMan.Attrs[EAttributeType.ATTR_MaxMP]-BaseAttrs[EAttributeType.ATTR_MaxMP]).ToString() + " )";
		}else{
			MpText.Text 		= Player.Instance.AttrMan.Attrs[EAttributeType.ATTR_CurMP].ToString() + " / " + Player.Instance.AttrMan.Attrs[EAttributeType.ATTR_MaxMP].ToString();
		}
		
		FlameDmgText.Text 				= (Player.Instance.AttrMan.EleAttrs[EStatusElementType.StatusElement_Flame]*100).ToString() + " %";
		FlameResText.Text 				= (Player.Instance.AttrMan.EleAttrs[EStatusElementType.StatusElement_FlameResist]*100).ToString() + " %";
		FrostDmgText.Text 				= (Player.Instance.AttrMan.EleAttrs[EStatusElementType.StatusElement_Frost]*100).ToString() + " %";
		FrostResText.Text 				= (Player.Instance.AttrMan.EleAttrs[EStatusElementType.StatusElement_FrostResist]*100).ToString() + " %";
		BlastDmgText.Text 				= (Player.Instance.AttrMan.EleAttrs[EStatusElementType.StatusElement_Explosion]*100).ToString() + " %";
		BlastResText.Text 				= (Player.Instance.AttrMan.EleAttrs[EStatusElementType.StatusElement_ExplosionResist]*100).ToString() + " %";
		StormDmgText.Text 				= (Player.Instance.AttrMan.EleAttrs[EStatusElementType.StatusElement_Storm]*100).ToString() + " %";
		StormResText.Text 				= (Player.Instance.AttrMan.EleAttrs[EStatusElementType.StatusElement_StormResist]*100).ToString() + " %";
		CriticalChanceText.Text			= string.Format("{0:0.00}", (Player.Instance.AttrMan.Attrs[EAttributeType.ATTR_Critical]/100).ToString()) + " %";
		CritacalDmgBonusText.Text		= Player.Instance.AttrMan.Attrs[EAttributeType.ATTR_CriticalDamage].ToString() + " %";
		DmgReductionText.Text			= string.Format("{0:0.00}", (Player.Instance.AttrMan.Attrs[EAttributeType.ATTR_DamageReduction]/100).ToString()) + " %";
	}
	
	public int PlayerDisToInt(int disType){
	
		switch(disType){
		case 1:
			return 0;
		case 2:
			return 1;
		case 4:
			return 2;
		default:
			return 0;
		}
	}
	
//	public Texture2D GetPlayerIcon(int disType,ESex sex){
	public Texture2D GetPlayerIcon(int disType,int sex){
		if(sex == ESex.eSex_Female){
			switch(disType){
			case 1:
				return PlayerIcon[0];
			case 2:
				return PlayerIcon[1];
			case 4:
				return PlayerIcon[2];
			}
		}else{
			switch(disType){
			case 1:
				return PlayerIcon[3];
			case 2:
				return PlayerIcon[4];
			case 4:
				return PlayerIcon[5];
			}
		}
		return null;
	}
	
	public long offest1970Time = 0;
	public long Update1970OffestTime() {
		TimeSpan ts = DateTime.UtcNow - new DateTime(1970,1,1,0,0,0,0);
		return Convert.ToInt64(ts.TotalSeconds);
	}
}
