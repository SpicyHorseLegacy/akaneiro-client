using UnityEngine;
using System.Collections;

public enum TypeEnum_Mastery
{
	kMasteryType_1HWeapon = 1,
	kMasteryType_DualWeapon = 2,
	kMasteryType_2HWeapon = 4,
	kMasteryType_LightArmor = 8,
	kMasteryType_MediumArmor = 16,
	kMasteryType_HeavyArmor = 32,
}

public enum EnumMasteryClass
{
    Weapon = 0,
    Armor,
}

public class SingleMastery : MonoBehaviour {
	public Texture2D Icon;
    public EMasteryType MasteryClass;
    public TypeEnum_Mastery SingleMasteryType;
    public EnumMasteryClass MasteryType;
    public int CurLv; 

    public SingleMasteryInfo BaseInfo{get{	return MasteryInfo.Instance.GetBaseInfo(MasteryClass);}}
	public SingleMasteryInfo Info{get{	return MasteryInfo.Instance.GetCurrentLevelInfo(MasteryClass, CurLv);}}
	public SingleMasteryInfo NextInfo{get{	return MasteryInfo.Instance.GetNextLevelInfo(MasteryClass, CurLv);}}

    void Awake()
    {
        MasteryClass = new EMasteryType((int)SingleMasteryType);
        CurLv = 0;
    }

    public void UpdateInfoFromServer(SMasteryLevelInfo masteryInfo)
	{
        CurLv = masteryInfo.level;
	}
}
