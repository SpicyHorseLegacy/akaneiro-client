using UnityEngine;
using System.Collections;

public class SuccessScreenCtrl : MonoBehaviour
{
    public static SuccessScreenCtrl Instance;
    public UITemplate CurrentContentTemplate;

    void Awake()
    {
        Instance = this;
        RegisterInitEvent();
    }

    #region Local
    #region event create and destory
    private void RegisterInitEvent()
    {
        GUIManager.Instance.OnInitEndDelegate += TemplateInitEnd;
        GUIManager.Instance.OnScreenManagerDestroy += DestoryAllEvent;
    }

    //MAX template count.//
    private int initDelegateCount = 1;
    private void TemplateInitEnd()
    {
        if (GUIManager.Instance.GetTemplateInitEndCount() >= initDelegateCount)
        {
            RegisterTemplateEvent();
            Init();
        }
    }

    private void RegisterTemplateEvent()
    {
        if (SuccessBase.Instance)
        {
            SuccessBase.Instance.OnThanksDelegate += ThanksDelegate;
        }
    }

    private void DestoryAllEvent()
    {
        if (SuccessBase.Instance)
            SuccessBase.Instance.OnThanksDelegate -= ThanksDelegate;
        if (SuccessBaseLevel.Instance)
            SuccessBaseLevel.Instance.OnThanksDelegate -= ThanksDelegate;

        GUIManager.Instance.OnInitEndDelegate -= TemplateInitEnd;
        GUIManager.Instance.OnScreenManagerDestroy -= DestoryAllEvent;
    }
    #endregion
    private void Init()
    {
        PlayerDataManager.Instance.SetMissionCompleteFlag(false);
        GameCamera.EnterMissionCompleteState();
        InitMissionDataInfo();
        CurrentContentTemplate = SuccessBase.Instance.GetComponent<UITemplate>();
    }

    private void InitMissionDataInfo()
    {
        SuccessBase.Instance.CleanMaterials();

        MissionSuccessStruct ms = new MissionSuccessStruct();
        ms.missionName = PlayerDataManager.Instance.GetMissionName();
        ms.threat = PlayerDataManager.Instance.GetMissionThreat();
        ms.threatText = PlayerDataManager.Instance.GetThreatText();
        ms.threatBonusKarma = PlayerDataManager.Instance.GetThreatBoundKarma();
        ms.threatBonusExp = PlayerDataManager.Instance.GetThreatBoundExp();
        ms.curPlayerLv = PlayerDataManager.Instance.CurLV;
        ms.curPlayerExp = PlayerDataManager.Instance.CurrentExperience;
        ms.curPlayerExpMax = PlayerDataManager.Instance.CurrentMaximumExperience;
        ms.prePlayerExpMax = PlayerDataManager.Instance.PreviousMaximumExperience;
        int rc = PlayerDataManager.Instance.GetReviveCount();
        int pr = 50;
        if (rc != 0)
        {
            pr = PlayerDataManager.Instance.GetPunismentReward(rc);
            ms.missionEarningKarma = PlayerDataManager.Instance.GetInGameKarma() * pr / 100;
            ms.missionEarningExp = PlayerDataManager.Instance.GetInGameExp() * pr / 100;
            AddIngameMaterials(pr);
        }
        else
        {
            ms.missionEarningKarma = PlayerDataManager.Instance.GetInGameKarma();
            ms.missionEarningExp = PlayerDataManager.Instance.GetInGameExp();
            AddIngameMaterials(pr);
        }
        
        SuccessBase.Instance.AwakeMissionSuccess(ms);
    }
    [SerializeField]
    private Transform materialPerfab;
    private void AddIngameMaterials(int pr)
    {
        foreach (IngameMaterialStruct mat in PlayerDataManager.Instance.materialsList)
        {
            GameObject obj = (GameObject)Instantiate(materialPerfab.gameObject);
            if (obj != null)
            {
                obj.GetComponent<MaterialObject>().SetCount((int)((pr + 50) * mat.count * 0.01));
                ItemPrefabs.Instance.GetItemIcon(mat.data._ItemID, mat.data._TypeID, mat.data._PrefabID, obj.GetComponent<MaterialObject>().GetIcon());
                SuccessBase.Instance.AddMaterial(obj.transform);
            }
        }
    }

    private void ThanksDelegate()
    {
        if (CurrentContentTemplate != null && CurrentContentTemplate.templateName == "SuccessBase")
        {
            SuccessBase.Instance.OnThanksDelegate -= ThanksDelegate;
            GUIManager.Instance.RemoveTemplate(CurrentContentTemplate.templateName);
            GUIManager.Instance.AddTemplate("SuccessBaseLevel");
            StartCoroutine(InitSuccessBaseLevel());
        }
        else
        {
            CS_Main.Instance.g_commModule.SendMessage(ProtocolGame_SendRequest.hasAssignedTalentPointReq());
        }
    }

    private IEnumerator InitSuccessBaseLevel()
    {
        while (SuccessBaseLevel.Instance == null)
            yield return new WaitForFixedUpdate();

        SuccessBaseLevel.Instance.Init(SuccessBase.Instance.CurrentMissionInfo);
        SuccessBaseLevel.Instance.OnThanksDelegate += ThanksDelegate;
    }
    #endregion
}
