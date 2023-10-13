using UnityEngine;
using System.Collections;

public class UI_AbilityShop_UITYPE_Control : MonoBehaviour
{
    Vector3 _initPos;

    [SerializeField]  UI_AbilityShop_UITYPE_Title_Manager TitleParent;
    public UI_TypeDefine.EnumAbilityShopUITYPE UI_Type;

    void Awake()
    {
        _initPos = transform.localPosition;
    }

    public void MoveToOffset(float _offset)
    {
        TweenPosition.Begin(gameObject, 0.2f, _initPos + new Vector3(1, 0, 0) * _offset);
    }

    #region BTN callback

    void BTNHover()
    {
        TitleParent.HoverTitle(UI_Type);
    }

    void BTNHoverOut()
    {
        TitleParent.HoverTitle(UI_TypeDefine.EnumAbilityShopUITYPE.NONE);
    }

    void BTNClicked()
    {
        if (UI_AbilityShop_Manager.Instance)
        {
            UI_AbilityShop_Manager.Instance.TopBarBTNClicked(UI_Type);
        }
    }

    #endregion

} 
