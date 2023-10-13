using UnityEngine;
using System.Collections;

public class UI_Hud_DragItem_Manager : MonoBehaviour
{
    public static UI_Hud_DragItem_Manager Instance;

    void Awake()
    {
        Instance = this;
        GUIManager.Instance.AddTemplateInitEnd();
    }

    #region Interface

    [SerializeField]
    private UI_Hud_DragItem_Control UI_DragItem;
    UI_Hud_DragItem_Control _activeDragItem;

    public void UpdatePosition(Vector3 _mousepos)
    {
        if (_activeDragItem != null)
        {
            Vector3 _worldPos = UICamera.currentCamera.ScreenToWorldPoint(_mousepos);
            _worldPos.z = transform.position.z;
            _activeDragItem.transform.position = _worldPos;

            if (UI_GameHud_DragItem_UpdateItemPos_Event != null)
                UI_GameHud_DragItem_UpdateItemPos_Event(_activeDragItem.DATA, _mousepos);
        }
    }

    public void ShowItem(UI_TypeDefine.UI_GameHud_DragItem_data _data)
    {
        _activeDragItem = Instantiate(UI_DragItem, UI_DragItem.transform.position, UI_DragItem.transform.rotation) as UI_Hud_DragItem_Control;
        _activeDragItem.transform.parent = transform;
        _activeDragItem.transform.localScale = Vector3.one;
        _activeDragItem.ShowItem(_data);

        if (UI_GameHud_DragItem_ShowItem_Event != null)
            UI_GameHud_DragItem_ShowItem_Event(_data);
    }

    public void Dispose(Vector3 _mousepos)
    {
        if (_activeDragItem != null)
        {
            if (UI_GameHud_DragItem_DisposeItem_Event != null)
                UI_GameHud_DragItem_DisposeItem_Event(_activeDragItem.DATA, _mousepos);
            _activeDragItem.Dispose();
        }
    }

    public void UpdateAllInfo(UI_TypeDefine.UI_GameHud_DragItem_data _data)
    {
        _activeDragItem.UpdateAllInfo(_data);
    }

    public void EnterHover()
    {
        if (_activeDragItem != null)
            _activeDragItem.EnterHover();
    }

    public void ExitHover()
    {
        if (_activeDragItem != null)
            _activeDragItem.ExitHover();
    }
    #endregion

    #region Delegate

    public delegate void Handle_GameHud_DragItem_ShowItem_Delegate(UI_TypeDefine.UI_GameHud_DragItem_data _data);
    public event Handle_GameHud_DragItem_ShowItem_Delegate UI_GameHud_DragItem_ShowItem_Event;

    public delegate void Handle_GameHud_DragItem_UpdateItemPos_Delegate(UI_TypeDefine.UI_GameHud_DragItem_data _data, Vector3 _pos);
    public event Handle_GameHud_DragItem_UpdateItemPos_Delegate UI_GameHud_DragItem_UpdateItemPos_Event;

    public delegate void Handle_GameHud_DragItem_DisposeItem_Delegate(UI_TypeDefine.UI_GameHud_DragItem_data _data, Vector3 _pos);
    public event Handle_GameHud_DragItem_DisposeItem_Delegate UI_GameHud_DragItem_DisposeItem_Event;

    #endregion
}
