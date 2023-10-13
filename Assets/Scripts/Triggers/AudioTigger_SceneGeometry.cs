using UnityEngine;
using System.Collections;

public class AudioTigger_SceneGeometry : AudioTrigger {
    public enum EnumAutioTriggerType_Geometry
    {
        StartNewBGM,
        BackOriginalBGM,
    }

    public EnumAutioTriggerType_Geometry TriggerType;

    void Start()
    {
        if (TriggerType == EnumAutioTriggerType_Geometry.StartNewBGM && PlayerPrefs.GetInt("CombatBGMID") == ID)
        {
            ActiveTrigger();
        }
    }

    public override void ActiveTrigger()
    {
        base.ActiveTrigger();

        switch (TriggerType)
        {
            case EnumAutioTriggerType_Geometry.StartNewBGM:
                StartNewBGM();
                break;

            case EnumAutioTriggerType_Geometry.BackOriginalBGM:
                BackOriginalBGM();
                break;
        }
    }

    protected override void StartNewBGM()
    {
        base.StartNewBGM();

        if (BGManager.Instance)
        {
            PlayerPrefs.SetInt("CombatBGMID", ID);
            BGManager.Instance.PlayOutsideBGM(CombatSFXPrefab, FadeInTime, FadeOutTime);
            if (BGManager.Instance.curOutsideBGM && BGManager.Instance.curOutsideBGM.GetComponent<BossAudioControl>())
            {
                BGManager.Instance.curOutsideBGM.GetComponent<BossAudioControl>().theTriggger = transform;
                BGManager.Instance.curOutsideBGM.GetComponent<BossAudioControl>().bStartFade = true;
            }
        }
    }

    protected override void BackOriginalBGM()
    {
        base.BackOriginalBGM();

        if (BGManager.Instance)
        {
            PlayerPrefs.SetInt("isCombatBGM", 0);
            BGManager.Instance.ExitOutsideAudio(FadeInTime, FadeOutTime);
        }
    }
}
