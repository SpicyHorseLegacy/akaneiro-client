using UnityEngine;
using System.Collections;

public class UI_CraftingShop_ItemDetail_AttributeCraftPanel : UI_CraftingShop_ItemDetail_Attribute
{
    [SerializeField]  UILabel Label_LevelNum;

    public override void UpdateAllInfo(ItemDropStruct _ItemInfo)
    {
        base.UpdateAllInfo(_ItemInfo);
        switch (AttType)
        {
            case UI_TypeDefine.ENUM_UI_CraftingShop_ItemAttribute.Level:
                Label_LevelNum.text = "" + _ItemInfo.info_Level % 100;
                break;

            case UI_TypeDefine.ENUM_UI_CraftingShop_ItemAttribute.Element:
                Label_LevelNum.text = "" + _ItemInfo._EleID % 100;
                break;

            case UI_TypeDefine.ENUM_UI_CraftingShop_ItemAttribute.Enchant:
                Label_LevelNum.text = "" + _ItemInfo._EnchantID % 100;
                break;

            case UI_TypeDefine.ENUM_UI_CraftingShop_ItemAttribute.Gem:
                Label_LevelNum.text = "" + _ItemInfo._GemID % 100;
                break;
        }
    }

    public void ResetDescriptionPosAtCenter(bool _iscenter)
    {
        Vector3 _temppos = Label_Content.transform.localPosition;
        _temppos.y = _iscenter ? 0 : 15;
        Label_Content.transform.localPosition = _temppos;
    }
}
