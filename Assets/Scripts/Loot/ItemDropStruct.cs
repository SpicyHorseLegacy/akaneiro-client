using UnityEngine;
using System.Collections;

public class ItemDropStruct{
	
	public int 		_ItemID;
	public int 		_TypeID;
	public int 		_SecTypeID;
	public int 		_SaleVal;
	public float 	_ItemVal;
	public string 	_PropsDes;
	public string 	_PropsDes2;
	public string 	_PropsName;
	public int 		_isUseRealMoney;
	public int 		_PrefabID;
	public string 	_PrefabName;
	public string 	_TypeName;
	public string 	_TypelastName;
	public string 	_QualityName;
	public string 	_CloakName;
	public int 		_GemID;
	public float 	_GemEffectVal;
	public int		_EnchantID;
	public int 		_EleID;
    public int      _LVID{get{return info_Level;}}
	
	public int  	info_Level;
	
	public string  	info_hand;
	public string  	info_ArmorLevel;
	
	public string  	info_Speed;
	public string  	info_Set;
	public string  	info_TypeName;
	public string  	info_EncNameLv;
	public string  	info_EncName;
	public string  	info_EleNameLv;
	public string  	info_EleName;
	public string  	info_GemeNameLv;
	public string  	info_GemName;
		
	public string  	info_EncDesc1;
	public string  	info_EncDesc2;
	public string  	info_EleDesc1;
	public string  	info_EleDesc2;
	public string  	info_GemDesc1;
	public string  	info_GemDesc2;
	
	public float  	info_MaxAtc;
	public float  	info_MinAtc;
	
	public float  	info_MinDef;
	
	public float  	info_Modifier;
	
	public float  	info_gemVal;
	public float  	info_encVal;
	public float  	info_eleVal;
	public string  	info_elePercentVal;
	
	public int  	info_gemIconIdx;
	public int  	info_encIconIdx;
    public int      info_eleIconIdx;

    public string ItemName { get { return PlayerDataManager.Instance.GetItemName(this); } }
    public Color BGColor { get { return PlayerDataManager.Instance.GetNameColor(info_gemVal + info_encVal + info_eleVal); } }
    public Color TextColor { get { return PlayerDataManager.Instance.GetNameTextColor(info_gemVal + info_encVal + info_eleVal); } }
}
