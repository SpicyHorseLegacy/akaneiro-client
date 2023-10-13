using UnityEngine;
using System.Collections;

public class UI_CraftingShop_ItemDetail_Attribute : MonoBehaviour
{
    public UI_TypeDefine.ENUM_UI_CraftingShop_ItemAttribute AttType;
    [SerializeField]  protected UISprite AttributeIcon;
    [SerializeField]  protected UILabel Label_Title;
    [SerializeField]  protected UILabel Label_Content;

    public virtual void UpdateAllInfo(ItemDropStruct _ItemInfo)
    {
        gameObject.SetActive(true);
        switch (AttType)
        {
            case UI_TypeDefine.ENUM_UI_CraftingShop_ItemAttribute.Level:
                if (_ItemInfo._TypeID == 7 || _ItemInfo._TypeID == 8)
                {
                    Label_Content.text = "Damage : [ffff32]" + ((int)(_ItemInfo.info_MinAtc * _ItemInfo.info_Modifier)).ToString() + " - " + ((int)(_ItemInfo.info_MaxAtc * _ItemInfo.info_Modifier)).ToString() + "[-]";

                    if (_ItemInfo._EleID == 0)
                    {
                        AttributeIcon.spriteName = "Item_G_Attack";
                    }
                    else
                    {
                        switch (_ItemInfo._EleID/100)
                        {
                            case 1:
                                AttributeIcon.spriteName = "ICN_A_Frost";
                                break;
                            case 2:
                                AttributeIcon.spriteName = "ICN_A_Flame";
                                break;
                            case 3:
                                AttributeIcon.spriteName = "ICN_A_Blast";
                                break;
                            case 4:
                                AttributeIcon.spriteName = "ICN_A_Stormt";
                                break;
                        }
                    }
                }else
                {
                    if (_ItemInfo._TypeID == 1 || _ItemInfo._TypeID == 3 || _ItemInfo._TypeID == 4 || _ItemInfo._TypeID == 6)
                    {
                        Label_Content.text = "Armor : [ffff32]" +((int)(_ItemInfo.info_MinDef * _ItemInfo.info_Modifier)).ToString() + "[-]";
                    }else if(_ItemInfo._TypeID == 2 || _ItemInfo._TypeID == 5)
                    {
                        Label_Content.text = "Armor : [ffff32]" + ((int)(_ItemInfo.info_MinDef * _ItemInfo.info_Modifier)).ToString() + "[-]";
                    }
                    AttributeIcon.spriteName = "Item_G_Shield";
                }
                Label_Title.text = "Level " + _ItemInfo.info_Level;
                break;

            case UI_TypeDefine.ENUM_UI_CraftingShop_ItemAttribute.Element:
                AttributeIcon.spriteName = _UI_CS_ElementsInfo.Instance.EleIcon[_ItemInfo.info_eleIconIdx-1].name;
                Label_Title.text = _ItemInfo.info_EleNameLv;
                Label_Content.text = _ItemInfo.info_EleDesc1 + _ItemInfo.info_EleDesc2;
                break;

            case UI_TypeDefine.ENUM_UI_CraftingShop_ItemAttribute.Enchant:
                AttributeIcon.spriteName = _UI_CS_ElementsInfo.Instance.EncIcon[_ItemInfo.info_encIconIdx-1].name;
                Label_Title.text = _ItemInfo.info_EncNameLv;
                Label_Content.text = _ItemInfo.info_EncDesc1 + _ItemInfo.info_EncDesc2;
                break;

            case UI_TypeDefine.ENUM_UI_CraftingShop_ItemAttribute.Gem:
                AttributeIcon.spriteName = _UI_CS_ElementsInfo.Instance.GemIcon[_ItemInfo.info_gemIconIdx-1].name;
                Label_Title.text = _ItemInfo.info_GemeNameLv;
                Label_Content.text = _ItemInfo.info_GemDesc1 + _ItemInfo.info_GemDesc2;
                break;
        }
    }

    #region BTN callback

    void BTNClicked()
    {
        UI_CraftingShop_Manager.Instance.ShowItemsOrCraft(false);
        UI_CraftingShop_Manager.Instance.CraftItemAttribute(AttType);
    }

    #endregion
}
