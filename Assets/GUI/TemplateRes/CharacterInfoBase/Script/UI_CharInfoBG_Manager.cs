using UnityEngine;
using System.Collections;

public class UI_CharInfoBG_Manager : MonoBehaviour {

    public static UI_CharInfoBG_Manager Instance;

    void Awake()
    {
        Instance = this;
    }

    #region Interface
    [SerializeField]
    GameObject BTNCover;

    [SerializeField]
    UI_CharInfoBG_Title_Control BTN_Inventroy;

    [SerializeField]
    UI_CharInfoBG_Title_Control BTN_PlayerStats;

    [SerializeField]
    UI_CharInfoBG_Title_Control BTN_Abilities;

    [SerializeField]
    UI_CharInfoBG_Title_Control BTN_Trails;

    //[SerializeField]
    //UI_TypeDefine.EnumCharInfoUITYPE CurInfoUIType;

    public void TitleClicked(UI_TypeDefine.EnumCharInfoUITYPE _type)
    {
        switch (_type)
        {
            case UI_TypeDefine.EnumCharInfoUITYPE.Inventory:
                InventoryBTNPressed();
                break;
            case UI_TypeDefine.EnumCharInfoUITYPE.PlayerStats:
                StatsBTNPressed();
                break;
            case UI_TypeDefine.EnumCharInfoUITYPE.Abilities:
                AbilitiesBTNPressed();
                break;
            case UI_TypeDefine.EnumCharInfoUITYPE.Trials:
                TrialsBTNPressed();
                break;
        }
    }

    public void SetBTNHighLight(UI_TypeDefine.EnumCharInfoUITYPE _type)
    {
        GameObject _targetBTN = null;
        switch(_type)
        {
            case UI_TypeDefine.EnumCharInfoUITYPE.Inventory:
                _targetBTN = BTN_Inventroy.gameObject;
                break;
            case UI_TypeDefine.EnumCharInfoUITYPE.PlayerStats:
                _targetBTN = BTN_PlayerStats.gameObject;
                break;
            case UI_TypeDefine.EnumCharInfoUITYPE.Abilities:
                _targetBTN = BTN_Abilities.gameObject;
                break;
            case UI_TypeDefine.EnumCharInfoUITYPE.Trials:
                _targetBTN = BTN_Trails.gameObject;
                break;
        }
        if (_targetBTN != null)
        {
            BTNCover.SetActive(true);
            BTNCover.transform.position = _targetBTN.transform.position;
            BTNCover.transform.parent = _targetBTN.transform;
            BTNCover.transform.localScale = Vector3.one;
			BTNCover.GetComponentInChildren<UISprite>().panel.Refresh();
			NGUITools.SetActiveSelf(BTNCover.gameObject, true);
        }
    }

    public void HoverTitle(UI_TypeDefine.EnumCharInfoUITYPE _type)
    {
        float inventoryOffset = 0;
        float statsOffset = 0;
        float abiOffset = 0;
        float trialsOffset = 0;

        switch (_type)
        {
            case UI_TypeDefine.EnumCharInfoUITYPE.Inventory:
                statsOffset = 10;
                abiOffset = 10;
                trialsOffset = 10;
                break;
            case UI_TypeDefine.EnumCharInfoUITYPE.PlayerStats:
                inventoryOffset = -10;
                abiOffset = 10;
                trialsOffset = 10;
                break;
            case UI_TypeDefine.EnumCharInfoUITYPE.Abilities:
                inventoryOffset = -10;
                statsOffset = -10;
                trialsOffset = 10;
                break;
            case UI_TypeDefine.EnumCharInfoUITYPE.Trials:
                inventoryOffset = -10;
                statsOffset = -10;
                abiOffset = -10;
                break;
        }

        BTN_Inventroy.MoveToOffset(inventoryOffset);
        BTN_PlayerStats.MoveToOffset(statsOffset);
        BTN_Abilities.MoveToOffset(abiOffset);
        BTN_Trails.MoveToOffset(trialsOffset);
    }

    #endregion

    #region local

    #region delegates

    public delegate void Handle_UICharInfoTopBarBTNPressed_Delegate(UI_TypeDefine.EnumCharInfoUITYPE askUIType);
    public event Handle_UICharInfoTopBarBTNPressed_Delegate TopbarPressed_Event;

    public delegate void Handle_UICharInfoCloseBTNClicked_Delegate();
    public event Handle_UICharInfoCloseBTNClicked_Delegate UICharInfo_CloseBTN_Event;

    #endregion

    void InventoryBTNPressed()
    {
        if (TopbarPressed_Event != null)
            TopbarPressed_Event(UI_TypeDefine.EnumCharInfoUITYPE.Inventory);
    }

    void StatsBTNPressed()
    {
        if (TopbarPressed_Event != null)
            TopbarPressed_Event(UI_TypeDefine.EnumCharInfoUITYPE.PlayerStats);
    }

    void AbilitiesBTNPressed()
    {
        if (TopbarPressed_Event != null)
            TopbarPressed_Event(UI_TypeDefine.EnumCharInfoUITYPE.Abilities);
    }

    void TrialsBTNPressed()
    {
        if (TopbarPressed_Event != null)
            TopbarPressed_Event(UI_TypeDefine.EnumCharInfoUITYPE.Trials);
    }
    
    

    #region BTN callback

    void CloseBTNClicked()
    {
        if (UICharInfo_CloseBTN_Event != null){
            UICharInfo_CloseBTN_Event();
			Camera.main.gameObject.SendMessage("changeCamera1");
			GameObject _cameragameobject = GameObject.Find("Camera").gameObject;
			if(_cameragameobject)
				_cameragameobject.SendMessage("changeCamera1");
		}
    }

    #endregion 

    #endregion
}
