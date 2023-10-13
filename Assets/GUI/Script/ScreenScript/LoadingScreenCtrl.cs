using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class SComicData 
{
    public string areaName = "";
	public string missionName = "";
	public int ID = 0;
	public int count = 0;
	public string comicImgPart1 = "";
	public string comicImgPart2 = "";
	public string comicImgPart3 = "";
	public string comicDescriptionPart1 = "";
	public string comicDescriptionPart2 = "";
	public string comicDescriptionPart3 = "";
}

public class LoadingScreenCtrl : MonoBehaviour
{
    public static LoadingScreenCtrl Instance;

    void Awake()
    {
        Instance = this;
        ReadTipsData();
        ReadComicData();
        RegisterInitEvent();
    }

    void Update()
    {
        UpdateProgressBar();
    }

    #region Interface
    private int curMissionID = 0;
    private string missionName = "";
    //if you want change screen to loading,need call this.//
    public void AwakeLoading(int missionID, string missionName)
    {
        PopUpBox.Hide(true);
        curMissionID = missionID;
        this.missionName = missionName;
        SetMapName(missionName);

        UI_Fade_Control.Instance.FadeIn();

		UI_BlackBackground_Control.ShowBlackBackground();
    }
    #region LoadScense
    private float progressVal = 0f;
    private string progressTitle = "Loading :";
    private bool isLoading = false;
    public void SetDownLoadProgress(string title, float val)
    {
        progressTitle = title;
        progressVal = val;
    }
    private void UpdateProgressBar()
    {
        if (isLoading)
        {
            string strTitle = progressTitle + " " + (int)(progressVal * 100) + "%";
            LoadingBase.Instance.SetLoadingInfo(strTitle);
            LoadingBase.Instance.SetProgressValue(progressVal);
        }
    }
    public void DownloadScense(string mapName)
    {
        string missionName = mapName.ToLower();
        if (missionName == "emptyscenes")
        {
            CS_SceneInfo.Instance.ClearSceneProgress();
            StartCoroutine(CS_SceneInfo.Instance.DownloadScense(mapName));
        }
        else if (string.Compare("Hub_Village", mapName) == 0)
        {
            StartCoroutine(CS_SceneInfo.Instance.DownloadMonsterBundle(mapName));
        }
        else
        {
            StartCoroutine(CS_SceneInfo.Instance.DownloadMonsterBundle(mapName));
        }
    }
    #endregion
    public void ExitLoadScreen()
    {
        isLoading = false;
        if (!TutorialMan.Instance.GetTutorialFlag())
        {
            CS_Main.Instance.g_commModule.SendMessage(ProtocolGame_SendRequest.RequestDayReward());
			Debug.Log("ExitLoadScreen Screen RUNNING + RequestDayReward()");
			CS_Main.Instance.g_commModule.SendMessage(ProtocolGame_SendRequest.GetFriendReward());
			Debug.Log("ECalled the GetFriendReward() FIX!!!");
        }
        GUIManager.Instance.ChangeUIScreenState("IngameScreen");
    }
    #endregion

    #region Local

    #region Event create and destroy...
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
    private void RegisterInitEvent()
    {
        GUIManager.Instance.OnInitEndDelegate += TemplateInitEnd;
        GUIManager.Instance.OnScreenManagerDestroy += DestoryAllEvent;
    }

    private void RegisterTemplateEvent()
    {
        if (LoadingBase.Instance)
        {
            LoadingBase.Instance.OnDescriptionDelegate += DescriptionDelegate;
            LoadingBase.Instance.OnArrowLeftDelegate += LeftArrowDelegate;
            LoadingBase.Instance.OnArrowRightDelegate += RightArrowDelegate;
        }
    }

    private void DestoryAllEvent()
    {
        if (LoadingBase.Instance)
        {
            LoadingBase.Instance.OnDescriptionDelegate -= DescriptionDelegate;
            LoadingBase.Instance.OnArrowLeftDelegate -= LeftArrowDelegate;
            LoadingBase.Instance.OnArrowRightDelegate -= RightArrowDelegate;
        }
        GUIManager.Instance.OnInitEndDelegate -= TemplateInitEnd;
        GUIManager.Instance.OnScreenManagerDestroy -= DestoryAllEvent;
    }
    #endregion

    private bool m_isGoingToVillage = false;

    private void Init()
    {
        ChangeLoadTips();

        if (PlayerDataManager.Instance.GetMissionID() == 6001 || PlayerDataManager.Instance.GetMissionID() == 6002)
        {
            m_isGoingToVillage = true;
        }

        ResetComic(((int)((PlayerDataManager.Instance.GetMissionID() - 1) / 10.0)) * 10);
        LoadingBase.Instance.SetAreaName(_areaName);
        LoadingBase.Instance.SetMissionName(_missionName);
        LoadingBase.Instance.HideLeftArrow(true);
        LoadingBase.Instance.HideRightArrow(m_isGoingToVillage ? true : false);
        isLoading = true;

        SComicData data = GetCurMissionData(_missionID);

        if (data != null)
        {
            TextureDownLoadingMan.Instance.DownLoadingTexture(data.comicImgPart1, LoadingBase.Instance.GetComicWidget(0));
            LoadingBase.Instance.SetStoryInfo(data.comicDescriptionPart1, 1);

            if (!m_isGoingToVillage)
            {
                TextureDownLoadingMan.Instance.DownLoadingTexture(data.comicImgPart2, LoadingBase.Instance.GetComicWidget(1));
                TextureDownLoadingMan.Instance.DownLoadingTexture(data.comicImgPart3, LoadingBase.Instance.GetComicWidget(2));
                LoadingBase.Instance.SetStoryInfo(data.comicDescriptionPart2, 2);
                LoadingBase.Instance.SetStoryInfo(data.comicDescriptionPart3, 3);
            }
        }        
    }

    private string mapName = "";
    private void SetMapName(string name)
    {
        mapName = name;
    }
    private string GetMapName()
    {
        return mapName;
    }

    #region Tips...
    private List<string> tipsList = new List<string>();
    void ReadTipsData()
    {
        tipsList.Clear();
        string fileName = "LoadTips.Description";
        string _fileName = LocalizeManage.Instance.GetLangPath(fileName);
        TextAsset item = (TextAsset)Resources.Load(_fileName, typeof(TextAsset));
        string[] itemRowsList = item.text.Split('\n');
        for (int i = 3; i < itemRowsList.Length - 1; ++i)
        {
            string pp = itemRowsList[i];
            string[] vals = pp.Split(new char[] { '	', '	' });
            tipsList.Add(vals[0]);
        }
        GUILogManager.LogInfo("ReadTipsData ok");
    }
    private void ChangeLoadTips()
    {
        int ran = Random.Range(0, tipsList.Count);
        if (tipsList.Count > 0)
        {
            LoadingBase.Instance.SetRandomInfo(tipsList[ran]);
        }
    }
    #endregion

    private bool isUseDescription = false;
    private void DescriptionDelegate()
    {
        if (isUseDescription)
        {
            if (VersionManager.Instance.GetVersionType() == VersionType.NormalClientVersion ||
                VersionManager.Instance.GetVersionType() == VersionType.SteamClientVersion)
            {
                UrlOpener.Open("https://aliceotherlands.backerkit.com/pledge");
            }
            else
            {
                Application.ExternalEval("window.open('https://aliceotherlands.backerkit.com/pledge','_blank')");
            }
        }
    }

    #region Comic Data
    private List<SComicData> comicDataList = new List<SComicData>();
    private void ReadComicData()
    {
        comicDataList.Clear();
        string fileName = "MissionDescription.LoadingComics";
        string _fileName = LocalizeManage.Instance.GetLangPath(fileName);
        TextAsset item = (TextAsset)Resources.Load(_fileName, typeof(TextAsset));
        string[] itemRowsList = item.text.Split('\n');
        for (int i = 3; i < itemRowsList.Length - 1; ++i)
        {
            string pp = itemRowsList[i];
            string[] vals = pp.Split(new char[] { '	', '	' });
            SComicData data = new SComicData();
            data.ID = int.Parse(vals[0]);
            data.areaName = vals[1];
            data.missionName = vals[2];
            data.count = int.Parse(vals[3]);
            data.comicDescriptionPart1 = vals[4];
            data.comicImgPart1 = vals[5];
            data.comicDescriptionPart2 = vals[6];
            data.comicImgPart2 = vals[7];
            data.comicDescriptionPart3 = vals[8];
            data.comicImgPart3 = vals[9].Trim();
            comicDataList.Add(data);
        }
    }
    private SComicData GetCurMissionData(int missionID)
    {
        foreach (SComicData data in comicDataList)
        {
            if (missionID == data.ID)
            {
                return data;
            }
        }
        return null;
    }
    private int _missionID = 0;
    private string _missionName = "Unknow";
    private string _areaName = "Unknow";
    private void ResetComic(int missionID)
    {
        SComicData data = GetCurMissionData(missionID);
        if (data == null)
        {
            GUILogManager.LogErr("GetCurMissionData fail,missionID: " + missionID.ToString());
            return;
        }
        _missionID = missionID;
        maxCount = data.count;
        _missionName = data.missionName;
        _areaName = data.areaName;
        curPageIdx = 0;

    }
    private int curPageIdx = 0;
    private int maxCount = 0;

    private void LeftArrowDelegate()
    {
        if (m_isGoingToVillage)
            return;

        curPageIdx--;
        LoadingBase.Instance.HideRightArrow(false);
        if (curPageIdx <= 0)
        {
            curPageIdx = 0;
            LoadingBase.Instance.HideLeftArrow(true);
        }
        LoadingBase.Instance.GoLeft();
    }

    private void RightArrowDelegate(bool automaticSwitch = false)
    {
        if (m_isGoingToVillage)
            return;

        curPageIdx++;
        LoadingBase.Instance.HideLeftArrow(false);
        if (curPageIdx >= (maxCount - 1))
        {
            curPageIdx = (maxCount - 1);
            LoadingBase.Instance.HideRightArrow(true);
        }
        LoadingBase.Instance.GoRight(automaticSwitch);
    }
    #endregion
    #endregion
}
