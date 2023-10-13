using UnityEngine;
using System.Collections;

public class SinglePetInfo : MonoBehaviour {

    public Enum_PetType PetType;
    public bool IsShow;
    public int CurLevel;
    public float CurEXP;
    public float BuyTime;
    public float LeftTime;

    public SinglePetListInfo CurLvPetInfo { get { return PetsInfo.GetPetListInfoByLVAndType(CurLevel, PetType); } }
    public SinglePetListInfo NextLvPetInfo { get { return PetsInfo.GetPetListInfoByLVAndType(CurLevel + 1, PetType); } }
}
