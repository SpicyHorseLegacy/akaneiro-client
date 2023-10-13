using UnityEngine;
using System.Collections;

public class Option_BugReport_Manager : MonoBehaviour {

    public static Option_BugReport_Manager Instance;

    void Awake()
    {
        Instance = this;
        GUIManager.Instance.AddTemplateInitEnd();
    }

    #region local

    public delegate void Handle_BugReportBtnPressed_Delegate();
    public event Handle_BugReportBtnPressed_Delegate BugReportBtnPressed_Event;
    void BugReportBtnPressed()
    {
        if (BugReportBtnPressed_Event != null)
            BugReportBtnPressed_Event();
    }

    #endregion
}
