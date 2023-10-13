using UnityEngine;
using System.Collections;

public class UI_CharInfoBG_Title_Control : MonoBehaviour {

    Vector3 _initPos;

    public UI_TypeDefine.EnumCharInfoUITYPE UI_Type;

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
        UI_CharInfoBG_Manager.Instance.HoverTitle(UI_Type);
    }

    void BTNHoverOut()
    {
        UI_CharInfoBG_Manager.Instance.HoverTitle(UI_TypeDefine.EnumCharInfoUITYPE.NONE);
    }

    void BTNClicked()
    {
        UI_CharInfoBG_Manager.Instance.TitleClicked(UI_Type);
    }

    #endregion
}
