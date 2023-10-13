using UnityEngine;
using System.Collections;

public class UI_Hud_DragItem_Control : MonoBehaviour {

    void Awake()
    {
        _DefaultIconString = Abi_Icon.spriteName;
    }

    #region Interface
    [SerializeField]  UI_Hud_Border_Control Border;
    [SerializeField]  private UISprite Abi_Icon;
    private string _DefaultIconString;

    public UI_TypeDefine.UI_GameHud_DragItem_data DATA
    {
        get
        {
            return _itemdata;
        }
    }
    UI_TypeDefine.UI_GameHud_DragItem_data _itemdata;

    public void ShowItem(UI_TypeDefine.UI_GameHud_DragItem_data _data)
    {
        gameObject.SetActive(true);
        _itemdata = _data;
        UpdateAllInfo(_itemdata);
        transform.localScale = Vector3.one;
        TweenScale.Begin(gameObject, 0.25f, Vector3.one * 1.1f);
    }

    public void Dispose()
    {
        Destroy(gameObject);
    }

    public void UpdateIcon(string _spriteName)
    {
        if (_spriteName != null)
        {
            Abi_Icon.spriteName = _spriteName;
        }
        else
        {
            Abi_Icon.spriteName = _DefaultIconString;
        }
    }

    public void UpdateAllInfo(UI_TypeDefine.UI_GameHud_DragItem_data _data)
    {
        UpdateIcon(_data.IconSpriteName);
    }

    public void EnterHover()
    {
        Border.ChangeColor(Color.yellow);
    }

    public void ExitHover()
    {
        Border.ChangeColor(Color.white);
    }
    #endregion
}
