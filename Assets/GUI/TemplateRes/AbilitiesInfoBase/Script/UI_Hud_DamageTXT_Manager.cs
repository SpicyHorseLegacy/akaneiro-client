using UnityEngine;
using System.Collections;

public class UI_Hud_DamageTXT_Manager : MonoBehaviour {

    public static UI_Hud_DamageTXT_Manager Instance;

    void Awake()
    {
        Instance = this;
        GUIManager.Instance.AddTemplateInitEnd();
    }

    #region Interface

    [SerializeField]
    private UI_Hud_DamageTXT_Control UI_DamageTXT_Prefab;

    #endregion

    #region Public

    public void ShowDamageAtPos(UI_TypeDefine.UI_GameHud_DamageTXT_data _data, Vector3 _worldpos)
    {
        UI_Hud_DamageTXT_Control _damagetxt = UnityEngine.Object.Instantiate(UI_DamageTXT_Prefab) as UI_Hud_DamageTXT_Control;
        _damagetxt.transform.parent = transform;

        Vector3 _screenPos = Camera.main.WorldToScreenPoint(_worldpos);
        Vector3 _uiPos = UICamera.currentCamera.ScreenToWorldPoint(_screenPos);
        _uiPos.z = transform.position.z;

        _damagetxt.transform.position = _uiPos;
        _damagetxt.transform.localScale = Vector3.one;

        _damagetxt.UpdateAllInfo(_data, _worldpos);
    }

    #endregion
}
