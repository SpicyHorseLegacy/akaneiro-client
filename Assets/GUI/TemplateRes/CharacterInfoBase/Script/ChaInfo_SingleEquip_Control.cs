using UnityEngine;
using System.Collections;

public class ChaInfo_SingleEquip_Control : MonoBehaviour
{
    void Awake()
    {
        _DefaultRareColor = Rare_Texture.color;
        _DefaultIconName = Equip_Icon.spriteName;
    }

    #region Interface

    public enum EnumChaEquipmentInfoType
    {
        NONE = -1,
        Helm = 0,
        Breastplate = 2,
        Breeches = 8,
        Cloak = 3,
        Neckless = 1,
        Ring = 4,
        MainWeapon = 6,
        OffWeapon = 7,
        MAX= 999,
    }

    public class ChaSingleEquipment_UI_Data
    {
        public ChaInfo_SingleEquip_Control.EnumChaEquipmentInfoType EquipType;
        public Color Rare_Color;
        public Texture2D UIIcon;
    }

    public EnumChaEquipmentInfoType EquipType;

    [SerializeField]
    UISprite Rare_Texture;
    Color _DefaultRareColor;

    [SerializeField]
    UISprite Equip_Icon;
    string _DefaultIconName;

    public void SetRare_Color(Color _color)
    {
        if (_color != null)
            Rare_Texture.color = _color;
        else
            Rare_Texture.color = _DefaultRareColor;
    }

    public void SetEquipIcon(Texture2D _tex)
    {
        if (_tex != null)
            Equip_Icon.spriteName = _tex.name;
        else
            Equip_Icon.spriteName = _DefaultIconName;
    }

    public void SetEquipInfo(ChaSingleEquipment_UI_Data _data)
    {
        if(_data.EquipType == EquipType)
        {
            SetEquipIcon(_data.UIIcon);
            SetRare_Color(_data.Rare_Color);
        }
    }

    #endregion
}
