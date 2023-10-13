using UnityEngine;
using System.Collections;

public class UI_ConsumableItemShop_UITYPE_Title_Manager : MonoBehaviour
{
    [SerializeField]  GameObject BTN_Cover;
    [SerializeField]  UI_ConsumableItemShop_UITYPE_Control BTN_Bundles;
    [SerializeField]  UI_ConsumableItemShop_UITYPE_Control BTN_Food;
    [SerializeField]  UI_ConsumableItemShop_UITYPE_Control BTN_Drink;
    [SerializeField]  UI_ConsumableItemShop_UITYPE_Control BTN_Materials;

    public void HoverTitle(UI_TypeDefine.EnumConsumableItemShopUITYPE _type)
    {
        float bundlesOffset = 0;
        float foodOffset = 0;
        float drinkOffset = 0;
        float materialOffset = 0;

        switch (_type)
        {
            case UI_TypeDefine.EnumConsumableItemShopUITYPE.Bundles:
                foodOffset = 10;
                drinkOffset = 10;
                materialOffset = 10;
                break;
            case UI_TypeDefine.EnumConsumableItemShopUITYPE.Food:
                bundlesOffset = -10;
                drinkOffset = 10;
                materialOffset = 10;
                break;
            case UI_TypeDefine.EnumConsumableItemShopUITYPE.Drink:
                bundlesOffset = -10;
                foodOffset = -10;
                materialOffset = 10;
                break;
            case UI_TypeDefine.EnumConsumableItemShopUITYPE.Materials:
                bundlesOffset = -10;
                foodOffset = -10;
                drinkOffset = -10;
                break;
        }

        BTN_Bundles.MoveToOffset(bundlesOffset);
        BTN_Food.MoveToOffset(foodOffset);
        BTN_Drink.MoveToOffset(drinkOffset);
        BTN_Materials.MoveToOffset(materialOffset);
    }

    public void ChangeToTargetTitle(UI_TypeDefine.EnumConsumableItemShopUITYPE _type)
    {
        GameObject _targetTitle = null;
        switch (_type)
        {
            case UI_TypeDefine.EnumConsumableItemShopUITYPE.Bundles:
                _targetTitle = BTN_Bundles.gameObject;
                break;
            case UI_TypeDefine.EnumConsumableItemShopUITYPE.Food:
                _targetTitle = BTN_Food.gameObject;
                break;
            case UI_TypeDefine.EnumConsumableItemShopUITYPE.Drink:
                _targetTitle = BTN_Drink.gameObject;
                break;
            case UI_TypeDefine.EnumConsumableItemShopUITYPE.Materials:
                _targetTitle = BTN_Materials.gameObject;
                break;
        }
        if (_targetTitle)
        {
            BTN_Cover.transform.parent = _targetTitle.transform;
            BTN_Cover.transform.localPosition = Vector3.zero;
            BTN_Cover.transform.localScale = Vector3.one;
        }
    }
}
