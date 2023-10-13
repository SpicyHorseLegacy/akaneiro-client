using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class ScenesLightCtrl : MonoBehaviour
{
    public static ScenesLightCtrl Instance;

    public List<Transform> LightList = new List<Transform>();
    private List<bool> IsLightsOpen = new List<bool>();

    private bool isOpen = true;

    public bool test;

    void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    void Start()
    {
        InitLightCtrl();
    }


    public void OpenLight()
    {
        for (int i = 0; i < LightList.Count; i++)
        {
            if (IsLightsOpen[i] && LightList[i] != null && LightList[i].GetComponent<Light>() != null)
            {
                LightList[i].GetComponent<Light>().enabled = true;
            }
        }
        isOpen = true;
    }

    public void CloseLight()
    {
        if (isOpen)
        {
            for (int i = 0; i < LightList.Count; i++)
            {
                if (LightList[i] != null && LightList[i].GetComponent<Light>() != null)
                    IsLightsOpen[i] = LightList[i].GetComponent<Light>().enabled;
            }
            for (int i = 0; i < LightList.Count; i++)
            {
                if (LightList[i] != null && LightList[i].GetComponent<Light>() != null)
                    LightList[i].GetComponent<Light>().enabled = false;
            }
            isOpen = false;
        }
    }

    public void InitLightCtrl()
    {
        GameObject currentLight = GameObject.Find("Directional light-Shadow");

        currentLight = currentLight == null ? GameObject.Find("Directional light-shadow") : currentLight;

        if (currentLight != null)
        {
            Light light = currentLight.GetComponent<Light>();
            light.shadows = GameConfig.IsShadows ? LightShadows.Soft : LightShadows.None;
        }

        IsLightsOpen.Clear();
        for (int j = 0; j < LightList.Count; j++)
        {
            IsLightsOpen.Add(false);
        }
    }
}
