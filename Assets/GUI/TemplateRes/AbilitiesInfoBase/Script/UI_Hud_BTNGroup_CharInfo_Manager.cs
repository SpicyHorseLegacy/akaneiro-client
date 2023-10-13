using UnityEngine;
using System.Collections;

public class UI_Hud_BTNGroup_CharInfo_Manager : MonoBehaviour {

    public static UI_Hud_BTNGroup_CharInfo_Manager Instance;

    void Awake()
    {
        Instance = this;
        GUIManager.Instance.AddTemplateInitEnd();
    }

    #region Delegate

    public delegate void Handle_Hud_CharInfoBTNGroup_Clicked_Delegate(UI_TypeDefine.EnumCharInfoUITYPE _targetui);
    public event Handle_Hud_CharInfoBTNGroup_Clicked_Delegate UI_Hud_CharInfoBTNGroup_Clicked_Event;

    #endregion

    #region BTN callback

    void StatBTNClicked()
    {
        if (StashManager.Instance != null)
            return;

        if (UI_Hud_CharInfoBTNGroup_Clicked_Event != null){
            UI_Hud_CharInfoBTNGroup_Clicked_Event(UI_TypeDefine.EnumCharInfoUITYPE.PlayerStats);
			GameObject _cameragameobject = Camera.main.gameObject;
			if(_cameragameobject)
				_cameragameobject.SendMessage("changeCamera0");
			_cameragameobject = GameObject.Find("Camera").gameObject;
			if(_cameragameobject)
				_cameragameobject.SendMessage("changeCamera0");
		}
    }

    void AbilityBTNClicked()
    {
        if (StashManager.Instance != null)
            return;

        if (UI_Hud_CharInfoBTNGroup_Clicked_Event != null){
            UI_Hud_CharInfoBTNGroup_Clicked_Event(UI_TypeDefine.EnumCharInfoUITYPE.Abilities);
			GameObject _cameragameobject = Camera.main.gameObject;
			if(_cameragameobject)
				_cameragameobject.SendMessage("changeCamera0");
			_cameragameobject = GameObject.Find("Camera").gameObject;
			if(_cameragameobject)
				_cameragameobject.SendMessage("changeCamera0");
		}
    }

    void InventoryBTNClicked()
    {
        if (StashManager.Instance != null)
            return;

        if (UI_Hud_CharInfoBTNGroup_Clicked_Event != null){
            UI_Hud_CharInfoBTNGroup_Clicked_Event(UI_TypeDefine.EnumCharInfoUITYPE.Inventory);
			GameObject _cameragameobject = Camera.main.gameObject;
			if(_cameragameobject)
				_cameragameobject.SendMessage("changeCamera0");
			_cameragameobject = GameObject.Find("Camera").gameObject;
			if(_cameragameobject)
				_cameragameobject.SendMessage("changeCamera0");
		}
    }

    void TrialBTNClicked()
    {
        if (StashManager.Instance != null)
            return;

        if (UI_Hud_CharInfoBTNGroup_Clicked_Event != null){
            UI_Hud_CharInfoBTNGroup_Clicked_Event(UI_TypeDefine.EnumCharInfoUITYPE.Trials);
			GameObject _cameragameobject = Camera.main.gameObject;
			if(_cameragameobject)
				_cameragameobject.SendMessage("changeCamera0");
			_cameragameobject = GameObject.Find("Camera").gameObject;
			if(_cameragameobject)
				_cameragameobject.SendMessage("changeCamera0");
		}
    }

    #endregion

}
