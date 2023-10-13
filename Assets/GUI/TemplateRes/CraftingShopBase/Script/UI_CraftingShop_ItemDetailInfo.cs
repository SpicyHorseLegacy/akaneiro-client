using UnityEngine;
using System.Collections;

public class UI_CraftingShop_ItemDetailInfo : MonoBehaviour {

    [SerializeField]  UILabel Label_ItemName;
    [SerializeField]  UITexture Tex_ItemIcon;
    [SerializeField]  UILabel Label_ItemType;

    [SerializeField]  UI_CraftingShop_ItemDetail_Attribute Attribute_Level_Group;
    [SerializeField]  UI_CraftingShop_ItemDetail_Attribute Attribute_Enchant_Group;
    [SerializeField]  UI_CraftingShop_ItemDetail_Attribute Attribute_Element_Group;
    [SerializeField]  UI_CraftingShop_ItemDetail_Attribute Attribute_Gem_Group;

    public void UpdateAllInfo(ItemDropStruct _ItemInfo)
    {
        Label_ItemName.text = _ItemInfo.ItemName;
        Label_ItemName.color = _ItemInfo.TextColor;

        Label_ItemType.text = _ItemInfo.info_TypeName;

        ItemPrefabs.Instance.GetItemIcon(_ItemInfo._ItemID, _ItemInfo._TypeID, _ItemInfo._PrefabID, Tex_ItemIcon);

        Attribute_Enchant_Group.gameObject.SetActive(false);
        Attribute_Element_Group.gameObject.SetActive(false);
        Attribute_Gem_Group.gameObject.SetActive(false);

        Attribute_Level_Group.UpdateAllInfo(_ItemInfo);

        Vector3 newPos = Attribute_Level_Group.transform.localPosition;
        newPos.y -= 55;

        newPos = UpdateAttribute(Attribute_Enchant_Group, _ItemInfo, _ItemInfo._EnchantID, newPos);
        newPos = UpdateAttribute(Attribute_Element_Group, _ItemInfo, _ItemInfo._EleID, newPos);
        newPos = UpdateAttribute(Attribute_Gem_Group, _ItemInfo, _ItemInfo._GemID, newPos);
    }

    Vector3 UpdateAttribute(UI_CraftingShop_ItemDetail_Attribute _temp, ItemDropStruct _ItemInfo, int _condition, Vector3 _newPos)
    {
        _temp.gameObject.SetActive(_condition != 0);
        if (_condition != 0)
        {
            _temp.UpdateAllInfo(_ItemInfo);
            _temp.transform.localPosition = _newPos;
            _newPos.y -= 55;
        }
        return _newPos;
    }

}
