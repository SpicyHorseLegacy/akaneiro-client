using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerPetManager : MonoBehaviour
{
    public Enum_PetType CurCallingPetType = Enum_PetType.NONE;

    // dictionary for pet, every type of pets have only one single pet info which is used by client.
    public Dictionary<Enum_PetType, SinglePetInfo> BasePetsInfo = new Dictionary<Enum_PetType, SinglePetInfo>();

    public SPetInfo[] ServerPetsInfo
    {
        get
        {
            return _petsinfo;
        }
        set
        {
            _petsinfo = value;
            for (int i = 0; i < value.Length; i++)
            {
                if (PetsInfo.GetPetListInfoByID(value[i].petTypeID) != null)
                {
                    SinglePetInfo _petinfo = GetPetInfoByType(PetsInfo.GetPetListInfoByID(value[i].petTypeID).m_Type);
                    if (_petinfo != null)
                    {
                        _petinfo.IsShow = true;
                        _petinfo.CurLevel = value[i].petLevel;
                        _petinfo.CurEXP = value[i].petExp;
                        _petinfo.BuyTime = value[i].buypetBegTime;
                        _petinfo.LeftTime = value[i].leftTime;
                    }
                }
                else
                {
                    Debug.LogWarning("[Pet] Can't find the Pet Info for pet : " + value[i].petTypeID);
                }
                
            }
        }
    }
    SPetInfo[] _petsinfo;

    void Start()
    {
        // init all single pet info
        for (int i = 0; i < PetsInfo.Instance.PetsList.Length; i++)
            GetPetInfoByType(PetsInfo.Instance.PetsList[i].m_Type);
    }

    SinglePetInfo GetPetInfoByType(Enum_PetType _type)
    {
        if (!BasePetsInfo.ContainsKey(_type))
        {
            GameObject _newinfogo = new GameObject(_type.ToString());
            _newinfogo.transform.parent = transform;
            _newinfogo.transform.localPosition = Vector3.zero;
            SinglePetInfo _newinfo = _newinfogo.AddComponent<SinglePetInfo>();
            _newinfo.IsShow = PetsInfo.GetPetListInfoByLVAndType(1, _type).m_IsShopShow;
            _newinfo.PetType = _type;
            _newinfo.CurEXP = 0;
            _newinfo.CurLevel = 1;
            BasePetsInfo.Add(_type, _newinfo);
        }
        return BasePetsInfo[_type];
    }
}
