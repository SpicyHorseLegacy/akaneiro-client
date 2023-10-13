using UnityEngine;
using System.Collections;

public class Option_HelpIndex_Manager : MonoBehaviour {

    public static Option_HelpIndex_Manager Instance;

    void Awake()
    {
        Instance = this;
        GUIManager.Instance.AddTemplateInitEnd();
    }
}
