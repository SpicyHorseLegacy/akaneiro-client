using UnityEngine;
using System.Collections;

public class Option_GiftCode_Manager : MonoBehaviour {

    public static Option_GiftCode_Manager Instance;

    void Awake()
    {
        Instance = this;
        GUIManager.Instance.AddTemplateInitEnd();
        PasteHelper.Paste += this.Paste;
    }

    void OnDestroy()
    {
        PasteHelper.Paste -= this.Paste;
    }

    #region Interface

    [SerializeField]
    private UIInput Code_Input;
    public UIInput Get_Code_Input() { return Code_Input; }

    public void ResetCode(string _code)
    {
        Get_Code_Input().text = _code;
    }

    public void Paste(string pastedString)
    {
        Code_Input.text = pastedString;
    }
    #endregion

    #region local

    public delegate void Handle_SendBTNPressed_Delegate(string _code);
    public event Handle_SendBTNPressed_Delegate SendBTNPressed_Event;
    void SendBTNPressed()
    {
        if (SendBTNPressed_Event != null)
            SendBTNPressed_Event(Get_Code_Input().text);
    }

    #endregion
}
