using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class _UI_CS_LoadProgressCtrl : MonoBehaviour
{
    //Instance
    public static _UI_CS_LoadProgressCtrl Instance = null;
    public UIPanel m_CS_Ingame_LoadMapPanel;
    public UIProgressBar m_LoadingBar;
    public SpriteText m_LoadingMapName;
    public bool isLoading = false;
    public UIButton BgPic;
    public WWW database;
    public SpriteText LoadObjNameText;
    public SpriteText LoadNumText;
    public SpriteText LoadTipsText;
    public List<string> LoadTipsList = new List<string>();
    public string MapName = "";
    //JW add. i think it is load obj use.
    public bool IsLoad = false;

    public UIButton playBtn;
    public UIButton leftBtn;
    public UIButton rightBtn;
    public UIPicture comicPic;
    public bool isCanPlayer = false;
    public Texture2D defaultPic;
    public SpriteText mainComicDescription;
    public SpriteText comicDescription;
    public UIButton comicDescriptionBg;
    public string[] comicDescriptionArray;
    public string[] comicBundlesArray;
    public int comicCount;
    public int currComicIndex;
    public Transform fireVFX;
    public Transform VFXpos;
    private Transform fireInstance;

    public UIButton bg;

    public enum EnumLoadingSteps
    {
        None,
        Analysising,
        DownloadingBundles,
        BuildingBundles,
        DownloadingScene,
        Done,
    }

    public EnumLoadingSteps LoadingSteps;

    void Awake()
    {
        Instance = this;
    }

    // Use this for initialization
    void Start()
    {
        ReadLoadTipsInfo();
        playBtn.AddInputDelegate(playBtnDelegate);
        leftBtn.AddInputDelegate(leftBtnDelegate);
        rightBtn.AddInputDelegate(rightBtnDelegate);
        comicDescriptionBg.AddInputDelegate(comicDescriptionBgDelegate);
    }

    // Update is called once per frame
    void Update()
    {

        switch (LoadingSteps)
        {
            case EnumLoadingSteps.Analysising:
                break;
            case EnumLoadingSteps.DownloadingBundles:
                break;
            case EnumLoadingSteps.BuildingBundles:
                break;
            case EnumLoadingSteps.DownloadingScene:
                if (null != database)
                {
                    float fStep = 0.9f;
                    fStep += database.progress * 0.1f;
                    LoadBarAni(fStep);
                    if (fStep == 1)
                    {
                        LoadObjNameText.Text = "Loading...";
                        LoadingSteps = EnumLoadingSteps.Done;
                    }
                }
                break;
        }
    }

    public void AwakeLoading(int missionId)
    {

        LoadingSteps = EnumLoadingSteps.None;

        InitImage();
        _UI_CS_ScreenCtrl.Instance.SetNextScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_LOADING);
        MouseCtrl.Instance.SetMouseStats(MouseIconType.SWARD1);
        BGManager.Instance.StopOriginalBG();
        MoneyBadgeInfo.Instance.Hide(true);
        isLoading = true;
        m_LoadingBar.Value = 0;
		LoadObjNameText.Text = "";
        LoadNumText.gameObject.SetActive(true);
        LoadNumText.Text = "0%";
        ChangePlayBtnState(false);
        comicCount = 0;
        currComicIndex = 0;
        mainComicDescription.Text = "";
        comicDescription.Text = "";
        ResetComic();
        ReadComicInfo(missionId);
        m_CS_Ingame_LoadMapPanel.BringIn();
    }

    private void InitImage()
    {
        bg.SetUVs(new Rect(0, 0, 1, 1));
        //downloading image
        TextureDownLoadingMan.Instance.DownLoadingTexture("Load_BK", bg.transform);
    }

    public void LeaveLoading()
    {
        isLoading = false;
        m_CS_Ingame_LoadMapPanel.Dismiss();
        MoneyBadgeInfo.Instance.Hide(false);
        BGMInfo.AutoPlayBGM = true;
		if(BGManager.Instance != null)
        	BGManager.Instance.PlayOriginalBG();
        BGMInfo.AutoPlayBGM = false;
        if (fireInstance != null)
        {
            Destroy(fireInstance.gameObject);
        }

        ChangePlayBtnState(true);

        LoadingSteps = EnumLoadingSteps.Done;
    }

    void ResetComic()
    {
        comicPic.GetComponent<MeshRenderer>().materials[0].mainTexture = defaultPic;
    }

    void ReadComicInfo(int missionId)
    {
        string fileName = "MissionDescription.LoadingComics";
        string _fileName = LocalizeManage.Instance.GetLangPath(fileName);
        TextAsset item = (TextAsset)Resources.Load(_fileName, typeof(TextAsset));
        string[] itemRowsList = item.text.Split('\n');
        for (int i = 3; i < itemRowsList.Length - 1; ++i)
        {
            string pp = itemRowsList[i];
            string[] vals = pp.Split(new char[] { '	', '	' });
            // 0 village.
            if (5000 == missionId || 0 == missionId)
            {
                missionId = 6000;
            }
            else if (1 == missionId)
            {
                missionId = 6000;
            }
            else
            {
                missionId = (missionId / 10 * 10);
            }
            if (missionId == 5000 || missionId == 5010 || missionId == 6000 )
            {
                comicDescriptionBg.controlIsEnabled = true;
            }
            else
            {
                comicDescriptionBg.controlIsEnabled = false;
            }
            if (missionId == int.Parse(vals[0]))
            {
                mainComicDescription.Text = vals[1];
                comicCount = int.Parse(vals[2]);
                comicDescriptionArray[0] = vals[3];
                comicDescriptionArray[1] = vals[5];
                comicDescriptionArray[2] = vals[7];
                comicBundlesArray[0] = vals[4];
                comicBundlesArray[1] = vals[6];
                comicBundlesArray[2] = vals[8].Trim();
                InitComic();
                return;
            }
        }
    }

    private bool isUseLinkBtn = false;
    void InitComic()
    {
        ResetComic();
        currComicIndex = 0;
        if (comicCount != 0)
        {
            comicDescription.Text = comicDescriptionArray[0];
            ResetArrowBtnState(0);
            //downloading image
            TextureDownLoadingMan.Instance.DownLoadingTexture(comicBundlesArray[0], comicPic.transform);
            if (string.Compare(comicBundlesArray[0], "HelpAka") == 0)
            {
                isUseLinkBtn = true;
            }
            else
            {
                isUseLinkBtn = false;
            }
        }
        else
        {
            comicDescription.Text = "";
        }
    }

    public void ChangePlayBtnState(bool canUse)
    {
        if (canUse)
        {
            playBtn.controlIsEnabled = true;
            LoadNumText.Text = "";
            LoadNumText.gameObject.SetActive(false);
            LoadObjNameText.Text = "Ready";
            //fireInstance = UnityEngine.Object.Instantiate(fireVFX) as Transform;
            //if (fireInstance != null)
            //{
            //    fireInstance.position = VFXpos.position;
            //}
            StartCoroutine(IntoGame());
        }
        else
        {
            playBtn.controlIsEnabled = false;
        }
    }

    void playBtnDelegate(ref POINTER_INFO ptr)
    {
        switch (ptr.evt)
        {
            case POINTER_INFO.INPUT_EVENT.PRESS:
                //				intoTheGame();
                break;
        }
    }

    private void intoTheGame()
    {
        if (!TutorialMan.Instance.GetTutorialFlag() && !_UI_CS_FightScreen.Instance.m_isLogout && !_UI_CS_MapScroll.Instance.IsExistMission)
        {
            //每日奖励请求//
            CS_Main.Instance.g_commModule.SendMessage(ProtocolGame_SendRequest.RequestDayReward());
        }
        if (_UI_CS_FightScreen.Instance.m_isLogout)
        {
            _UI_CS_EventRewards.Instance.AwkeBonusMenu(3);
        }
        else
        {
            _UI_CS_EventRewards.Instance.AwkeBonusMenu(4);
        }
        _UI_CS_MissionLogic.Instance.RestMissionTime();

        _UI_MiniMap.Instance.isShowSmallMap = true;
        if (MapName == "EmptyScenes")
        {
            SelectChara.Instance.UpdateModelEquip(SelectChara.Instance.GetCurrentSelectIdx());
        }
    }

    private IEnumerator IntoGame()
    {
        yield return new WaitForSeconds(0.5f);
        intoTheGame();
    }

    void ResetArrowBtnState(int currentIdx)
    {
        if (comicCount <= 1)
        {
            leftBtn.controlIsEnabled = false;
            rightBtn.controlIsEnabled = false;
        }
        else
        {
            leftBtn.controlIsEnabled = true;
            rightBtn.controlIsEnabled = true;
            if (currentIdx == 0)
            {
                leftBtn.controlIsEnabled = false;
            }
            if (currentIdx == (comicCount - 1))
            {
                rightBtn.controlIsEnabled = false;
            }
        }
    }

    void leftBtnDelegate(ref POINTER_INFO ptr)
    {
        switch (ptr.evt)
        {
            case POINTER_INFO.INPUT_EVENT.TAP:
                if (comicCount != 0)
                {
                    currComicIndex--;
                    if (currComicIndex < 0)
                    {
                        currComicIndex = 0;
                    }
                    ResetArrowBtnState(currComicIndex);
                    //downloading image
                    TextureDownLoadingMan.Instance.DownLoadingTexture(comicBundlesArray[currComicIndex], comicPic.transform);
                    comicDescription.Text = comicDescriptionArray[currComicIndex];
                }
                break;
        }
    }

    void rightBtnDelegate(ref POINTER_INFO ptr)
    {
        switch (ptr.evt)
        {
            case POINTER_INFO.INPUT_EVENT.TAP:
                if (comicCount != 0)
                {
                    currComicIndex++;
                    if (currComicIndex >= comicCount)
                    {
                        currComicIndex = (comicCount - 1);
                    }
                    ResetArrowBtnState(currComicIndex);
                    //downloading image
                    TextureDownLoadingMan.Instance.DownLoadingTexture(comicBundlesArray[currComicIndex], comicPic.transform);
                    comicDescription.Text = comicDescriptionArray[currComicIndex];
                }
                break;
        }
    }

    void comicDescriptionBgDelegate(ref POINTER_INFO ptr)
    {
        switch (ptr.evt)
        {
            case POINTER_INFO.INPUT_EVENT.TAP:
                if (isUseLinkBtn)
                {
                    if (ClientLogicCtrl.Instance.isClientVer)
                    {
                        UrlOpener.Open("https://aliceotherlands.backerkit.com/pledge");
                    }
                    else
                    {
                        Application.ExternalEval("window.open('https://aliceotherlands.backerkit.com/pledge','_blank')");
                    }
                }
                break;
        }
    }

    void ReadLoadTipsInfo()
    {
        string fileName = "LoadTips.Description";
        string _fileName = LocalizeManage.Instance.GetLangPath(fileName);
        TextAsset item = (TextAsset)Resources.Load(_fileName, typeof(TextAsset));
        string[] itemRowsList = item.text.Split('\n');
        for (int i = 3; i < itemRowsList.Length - 1; ++i)
        {
            string pp = itemRowsList[i];
            string[] vals = pp.Split(new char[] { '	', '	' });
            LoadTipsList.Add(vals[0]);
        }
        LogManager.Log_Info("ReadLoadTipsInfo Ok");
    }

    public void LoadBarAni(float progress)
    {
        if (progress > 1)
        {
            progress = 1.0f;
        }
        m_LoadingBar.Value = progress;
        LoadNumText.Text = ((int)(progress * 100)).ToString() + " %";
    }

    public void ChangeLoadTips()
    {
        int tempIdx = Random.Range(0, LoadTipsList.Count);
        if (LoadTipsList.Count > 0)
            LoadTipsText.Text = LoadTipsList[tempIdx];
    }

    public void LoadLevel(string mapName)
    {
        MapName = mapName;
        m_LoadingMapName.Text = _UI_CS_MapScroll.Instance.SceneNameToMapName(mapName);
        string tempname = mapName.ToLower();
        ChangeLoadTips();
        if (tempname == "emptyscenes")
        {
            CS_SceneInfo.Instance.ClearSceneProgress();
            MailSystem.Instance.IsHideMailIcon(true);
            MissionPanel.Instance.LoadImg();
            StartCoroutine(WiteOneFrame_Goto(mapName));
        }else if(string.Compare("Hub_Village",mapName) == 0) {
			 MailSystem.Instance.IsHideMailIcon(false);
            StartCoroutine(CS_SceneInfo.Instance.DownloadMonsterBundle(mapName));
		}
        else
        {
            MailSystem.Instance.IsHideMailIcon(true);
            StartCoroutine(CS_SceneInfo.Instance.DownloadMonsterBundle(mapName));
        }
        SetMainCameraTag(tempname, 1);
    }

    //io: 1 in ,2 out; 因为进入场景时有两个接口，如果放于load之前切换状态会导致一些异常，
    //虽然没有太大影响.但还是没什么必要，所以+io条件判断；//
    public void SetMainCameraTag(string scenesName, int io)
    {
        if (string.Compare("emptyscenes", scenesName, true) == 0)
        {
            //Unity40 主摄像机切换ui camera to game play camera//
            _UI_CS_Ctrl.Instance.m_UI_Camera.tag = "MainCamera";
        }
        else
        {
            if (io == 1)
            {
                return;
            }
            //Unity40 主摄像机切换ui camera to game play camera//
            _UI_CS_Ctrl.Instance.m_UI_Camera.tag = "Untagged";
        }
    }

    public IEnumerator WiteOneFrame_Goto(string mapName)
    {
        if (mapName.ToLower() != "emptyscenes")
        {
            string url = BundlePath.AssetbundleBaseURL + "Scenes/";
            url += (mapName + ".unity3d");

            database = new WWW(url);
            IsLoad = true;
            LoadingSteps = EnumLoadingSteps.DownloadingScene;

            yield return database;
            IsLoad = false;

            AssetBundle temp = database.assetBundle;
        }

        AsyncOperation async = Application.LoadLevelAsync(mapName);

        if (mapName.ToLower() == "emptyscenes")
        {
            LoadObjNameText.Text = "Loading...";
            LoadBarAni(1);
        }

        yield return async;

        if (mapName.ToLower() == "emptyscenes")
        {
            LeaveLoading();
        }

        SetMainCameraTag(mapName, 2);

        if (database != null)
        {
            database.assetBundle.Unload(false);
            database.Dispose();
            database = null;
        }
    }
}
