using UnityEngine;
using System.Collections;

public class UI_BlackBackground_Control : MonoBehaviour {

    public static UI_BlackBackground_Control Instance;

    void Awake() { Instance = this; }

    public static void ShowBlackBackground()
    {
        if (Instance != null)
            Instance.gameObject.SetActive(true);

        GameCamera.CloseCamera();
    }

    public static void CloseBlackBackground()
    {
        if (Instance != null)
            Instance.gameObject.SetActive(false);

        GameCamera.OpenCamera();
    }
}
