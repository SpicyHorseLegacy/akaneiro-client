using UnityEngine;
using System.Collections;

public class UI_AbilityShop_UITYPE_Title_Manager : MonoBehaviour {

    [SerializeField]  GameObject BTN_Cover;
    [SerializeField]  UI_AbilityShop_UITYPE_Control BTN_Prowess;
    [SerializeField]  UI_AbilityShop_UITYPE_Control BTN_Fortitude;
    [SerializeField]  UI_AbilityShop_UITYPE_Control BTN_Cunning;

    public void HoverTitle(UI_TypeDefine.EnumAbilityShopUITYPE _type)
    {
        float prowessOffset = 0;
        float fortitudeOffset = 0;
        float cunningOffset = 0;

        switch (_type)
        {
            case UI_TypeDefine.EnumAbilityShopUITYPE.Prowess:
                fortitudeOffset = 10;
                cunningOffset = 10;
                break;
            case UI_TypeDefine.EnumAbilityShopUITYPE.Fortitude:
                prowessOffset = -10;
                cunningOffset = 10;
                break;
            case UI_TypeDefine.EnumAbilityShopUITYPE.Cunning:
                prowessOffset = -10;
                fortitudeOffset = -10;
                break;
        }

        BTN_Prowess.MoveToOffset(prowessOffset);
        BTN_Fortitude.MoveToOffset(fortitudeOffset);
        BTN_Cunning.MoveToOffset(cunningOffset);
    }

    public void ChangeToTargetTitle(UI_TypeDefine.EnumAbilityShopUITYPE _type)
    {
        GameObject _targetTitle = null;
        switch (_type) 
        {
            case UI_TypeDefine.EnumAbilityShopUITYPE.Prowess:
                _targetTitle = BTN_Prowess.gameObject;
                break;
            case UI_TypeDefine.EnumAbilityShopUITYPE.Fortitude:
                _targetTitle = BTN_Fortitude.gameObject;
                break;
            case UI_TypeDefine.EnumAbilityShopUITYPE.Cunning:
                _targetTitle = BTN_Cunning.gameObject;
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
