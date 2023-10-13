using UnityEngine;
using System.Collections;

public class OptionScreenCtrl : BaseScreenCtrl
{
    public static OptionScreenCtrl Instance;

    public Option_Main_Manager.EnumOptionBTNType CurContent;
    public UITemplate CurrentContentTemplate;

    protected override void Awake()
    {
        base.Awake();
        Instance = this;
        CurContent = Option_Main_Manager.EnumOptionBTNType.None;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Close_BTNClicked();
        }
    }

    #region event create and destory
    protected override void RegisterInitEvent()
    {
        base.RegisterInitEvent();
    }

    protected override void TemplateInitEnd()
    {
        base.TemplateInitEnd();
    }

    protected override void DestoryAllEvent()
    {
        base.DestoryAllEvent();
    }

    protected override void RegisterTemplateEvent()
    {
        base.RegisterTemplateEvent();
    }

    protected override void RegisterSingleTemplateEvent(string _templateName)
    {
        base.RegisterSingleTemplateEvent(_templateName);

        if (_templateName == "Option_Main" && Option_Main_Manager.Instance)
        {
            Option_Main_Manager.Instance.OptionMenuBTNClicked_Event += Main_BTNClicked;
            Option_Main_Manager.Instance.CloseBTNClicked_Event += Close_BTNClicked;
            Option_Main_Manager.Instance.Init();
            GUIManager.Instance.AddTemplate("Option_Setting");
            Player.Instance.FreezePlayer();
        }

        if (_templateName == "Option_Setting" && Option_Setting_Manager.Instance)
        {
            CurContent = Option_Main_Manager.EnumOptionBTNType.Setting;
            CurrentContentTemplate = Option_Setting_Manager.Instance.transform.GetComponent<UITemplate>();

            SettingInit();

            StartCoroutine(LoadOk());

            Option_Setting_Manager.Instance.transform.GetComponent<UITemplate>().OnTemplateDestroy += UnregisterSingleTemplateEvent;
        }

        if (_templateName == "Option_HelpIndex")
        {
            CurContent = Option_Main_Manager.EnumOptionBTNType.HelpIndex;
            CurrentContentTemplate = Option_HelpIndex_Manager.Instance.transform.GetComponent<UITemplate>();

            Option_HelpIndex_Manager.Instance.transform.GetComponent<UITemplate>().OnTemplateDestroy += UnregisterSingleTemplateEvent;
        }

        if (_templateName == "Option_GiftCode")
        {
            CurContent = Option_Main_Manager.EnumOptionBTNType.Gift;
            CurrentContentTemplate = Option_GiftCode_Manager.Instance.transform.GetComponent<UITemplate>();
            Option_GiftCode_Manager.Instance.SendBTNPressed_Event += GiftSendBTNPressed;

            Option_GiftCode_Manager.Instance.transform.GetComponent<UITemplate>().OnTemplateDestroy += UnregisterSingleTemplateEvent;
        }

        if (_templateName == "Option_BugReport")
        {
            CurContent = Option_Main_Manager.EnumOptionBTNType.BugReport;
            CurrentContentTemplate = Option_BugReport_Manager.Instance.transform.GetComponent<UITemplate>();
            Option_BugReport_Manager.Instance.BugReportBtnPressed_Event += BugReportBtnPressed;

            Option_BugReport_Manager.Instance.transform.GetComponent<UITemplate>().OnTemplateDestroy += UnregisterSingleTemplateEvent;
        }

        if (_templateName == "MoneyBar" && MoneyBarManager.Instance)
        {
            MoneyBarManager.Instance.OnAddKarmaDelegate += AddKarmaDelegate;
            MoneyBarManager.Instance.OnAddCrystalDelegate += AddCrystalDelegate;
            MoneyBarManager.Instance.SetKarmaVal(PlayerDataManager.Instance.GetKarmaVal());
            MoneyBarManager.Instance.SetCrystalVal(PlayerDataManager.Instance.GetCrystalVal());
        }

        if (_templateName == "KarmaRecharge" && KarmaRechargeManager.Instance)
        {
            KarmaRechargeManager.Instance.OnExitDelegate += ExitRechargeKarmaDelegate;
            KarmaRechargeManager.Instance.OnAddKarmaDelegate += RechargeKarmaValDelegate;
            InitKarmaRechargeData();
        }

        if (_templateName == "CrystalRecharge" && CrystalRechargeManager.Instance)
        {
            CrystalRechargeManager.Instance.OnExitDelegate += ExitRechargeCrystalDelegate;
            CrystalRechargeManager.Instance.OnAddKarmaDelegate += RechargeKarmaValDelegate;
            InitCrystalRechargeData();
        }

        if (_templateName == "FoodSlot" && FoodSoltManager.Instance)
        {
            UpdateFoodSlot();
        }
    }

    protected override void UnregisterSingleTemplateEvent(string _templateName)
    {
        base.UnregisterSingleTemplateEvent(_templateName);

        if (_templateName == "Option_Main" && Option_Main_Manager.Instance)
        {
            Option_Main_Manager.Instance.OptionMenuBTNClicked_Event -= Main_BTNClicked;
            Option_Main_Manager.Instance.CloseBTNClicked_Event -= Close_BTNClicked;
        }

        if (_templateName == "Option_Setting" && Option_Setting_Manager.Instance)
        {
            Option_Setting_Manager.Instance.CheckBoxChanged_Event -= SettingCheckBoxChanged;
            Option_Setting_Manager.Instance.AudioSFXVolChanged_Event -= AudioSFXVolChanged;
            Option_Setting_Manager.Instance.AudioMusicVolChanged_Event -= AudioMusicVolChanged;
            Option_Setting_Manager.Instance.VideoQualityChanged_Event -= VideoQualityChanged;
            Option_Setting_Manager.Instance.DefaultBTNPressed_Event -= DefaultBTNPressed;

            Option_Setting_Manager.Instance.transform.GetComponent<UITemplate>().OnTemplateDestroy -= UnregisterSingleTemplateEvent;
        }

        if (_templateName == "Option_HelpIndex")
        {
            Option_HelpIndex_Manager.Instance.transform.GetComponent<UITemplate>().OnTemplateDestroy -= UnregisterSingleTemplateEvent;
        }

        if (_templateName == "Option_GiftCode")
        {
            Option_GiftCode_Manager.Instance.SendBTNPressed_Event -= GiftSendBTNPressed;

            Option_GiftCode_Manager.Instance.transform.GetComponent<UITemplate>().OnTemplateDestroy -= UnregisterSingleTemplateEvent;
        }

        if (_templateName == "Option_BugReport")
        {
            Option_BugReport_Manager.Instance.BugReportBtnPressed_Event -= BugReportBtnPressed;

            Option_BugReport_Manager.Instance.transform.GetComponent<UITemplate>().OnTemplateDestroy -= UnregisterSingleTemplateEvent;
        }

        if (_templateName == "MoneyBar" && MoneyBarManager.Instance)
        {
            MoneyBarManager.Instance.OnAddKarmaDelegate -= AddKarmaDelegate;
            MoneyBarManager.Instance.OnAddCrystalDelegate -= AddCrystalDelegate;
        }

        if (_templateName == "KarmaRecharge" && KarmaRechargeManager.Instance)
        {
            KarmaRechargeManager.Instance.OnExitDelegate -= ExitRechargeKarmaDelegate;
            KarmaRechargeManager.Instance.OnAddKarmaDelegate -= RechargeKarmaValDelegate;
        }

        if (_templateName == "CrystalRecharge" && CrystalRechargeManager.Instance)
        {
            CrystalRechargeManager.Instance.OnExitDelegate -= ExitRechargeCrystalDelegate;
            CrystalRechargeManager.Instance.OnAddKarmaDelegate -= RechargeKarmaValDelegate;
        }
    }

    #region Recharge
    private bool isPopUpRecharge = false;
    private void ExitRechargeKarmaDelegate()
    {
        GUIManager.Instance.RemoveTemplate("KarmaRecharge");
        isPopUpRecharge = false;
    }
    private void ExitRechargeCrystalDelegate()
    {
        GUIManager.Instance.RemoveTemplate("CrystalRecharge");
        isPopUpRecharge = false;
    }
    private void RechargeKarmaValDelegate(string content)
    {
        if (Steamworks.activeInstance != null)
        {
            Steamworks.activeInstance.StartPayment(content);
        }
        switch (VersionManager.Instance.GetVersionType())
        {
            case VersionType.WebVersion:
                Application.ExternalCall("select_gold", content);
                break;
            case VersionType.NormalClientVersion:
                StartCoroutine(VersionManager.Instance.SendMsgToServerForPay(content));
                break;
            default:
                StartCoroutine(VersionManager.Instance.SendMsgToServerForPay(content));
                break;
        }
    }

    private void InitKarmaRechargeData()
    {
        KarmaRechargeManager.Instance.SetKarmaInfo(PlayerDataManager.Instance.karmaRechargTitle);
        for (int i = 0; i < 7; i++)
        {
            KarmaRechargeManager.Instance.SetKarmaVal(i, PlayerDataManager.Instance.rechargeValData.karmaVal[i]);
            KarmaRechargeManager.Instance.SetPayVal(i, PlayerDataManager.Instance.rechargeValData.karmaPayVal[i]);
        }
    }
    private void InitCrystalRechargeData()
    {
        CrystalRechargeManager.Instance.SetKarmaInfo(PlayerDataManager.Instance.crystalRechargTitle);
        for (int i = 0; i < 7; i++)
        {
            CrystalRechargeManager.Instance.SetKarmaVal(i, PlayerDataManager.Instance.rechargeValData.crystalVal[i]);
            CrystalRechargeManager.Instance.SetPayVal(i, PlayerDataManager.Instance.rechargeValData.crystalPayVal[i]);
        }
    }
    #endregion

    #region MoneyBar
    private void AddKarmaDelegate()
    {
        if (isPopUpRecharge)
        {
            return;
        }
        if (Steamworks.activeInstance)
        {
            Steamworks.activeInstance.ShowShop("karma");
        }
        else
        {
            GUIManager.Instance.AddTemplate("KarmaRecharge");
            isPopUpRecharge = true;
        }
    }
    private void AddCrystalDelegate()
    {
        if (isPopUpRecharge)
        {
            return;
        }
        if (Steamworks.activeInstance)
        {
            Steamworks.activeInstance.ShowShop("crystal");
        }
        else
        {
            GUIManager.Instance.AddTemplate("CrystalRecharge");
            isPopUpRecharge = true;
        }
    }
    #endregion

    // start listening to the option setting after all widgets init done.
    // otherwise, the checkbox/ slider/ popup menu could callback when they run Start() function.
    IEnumerator LoadOk()
    {
        yield return new WaitForSeconds(0.25f);
        if (Option_Setting_Manager.Instance)
        {
            Option_Setting_Manager.Instance.CheckBoxChanged_Event += SettingCheckBoxChanged;
            Option_Setting_Manager.Instance.AudioSFXVolChanged_Event += AudioSFXVolChanged;
            Option_Setting_Manager.Instance.AudioMusicVolChanged_Event += AudioMusicVolChanged;
            Option_Setting_Manager.Instance.VideoQualityChanged_Event += VideoQualityChanged;
            Option_Setting_Manager.Instance.DefaultBTNPressed_Event += DefaultBTNPressed;
        }
    }

    #region Food Bar...
    [SerializeField]
    private static Texture2D emptyImg = null;

    public void UpdateFoodSlot()
    {
        if (!FoodSoltManager.Instance)
        {
            return;
        }

        if (emptyImg == null)
        {
            emptyImg = new Texture2D(1, 1, TextureFormat.ARGB32, false);
            emptyImg.SetPixel(0, 0, new Color(1f, 1f, 1f, 0f));
            emptyImg.Apply();
        }

        int idx = -1;
        for (int i = 0; i < 3; i++)
        {
            InventorySlot info = FoodSoltManager.Instance.GetFoodItemData(i + 1);
            idx = PlayerDataManager.Instance.foodList[i];
            if (idx != -1)
            {
                _ItemInfo data = PlayerDataManager.Instance.GetBagItemData(idx);
                if (!data.empty)
                {
                    info.SetEmptyFlag(false);
                    info.SetData(data);
                    ItemPrefabs.Instance.GetItemIcon(data.localData._ItemID, data.localData._TypeID, data.localData._PrefabID, info.GetIcon());
                    info.ChangeBGColor(PlayerDataManager.Instance.GetNameColor(data.localData.info_eleVal + data.localData.info_encVal + data.localData.info_gemVal));
                    info.SetCount(data.count);
                }
                else
                {
                    PlayerDataManager.Instance.EmptyFoodSlot(i);
                    info.EmptySlot(emptyImg);
                }
            }
            else
            {
                info.EmptySlot(emptyImg);
            }
        }
    }
    #endregion

    #region DelegateFromMain

    void Main_BTNClicked(Option_Main_Manager.EnumOptionBTNType _btnType)
    {
        switch (_btnType)
        {
            case Option_Main_Manager.EnumOptionBTNType.Setting:
                {
                    if (CurContent != Option_Main_Manager.EnumOptionBTNType.Setting && CurContent != Option_Main_Manager.EnumOptionBTNType.None)
                    {
                        CurContent = Option_Main_Manager.EnumOptionBTNType.None;
                        GUIManager.Instance.RemoveTemplate(CurrentContentTemplate.templateName);
                        GUIManager.Instance.AddTemplate("Option_Setting");
                    }
                }
                break;

            case Option_Main_Manager.EnumOptionBTNType.HelpIndex:
                {
                    // TODO : Help index
                    /*if (CurContent != Option_Main_Manager.EnumOptionBTNType.HelpIndex && CurContent != Option_Main_Manager.EnumOptionBTNType.None)
                    {
                        CurContent = Option_Main_Manager.EnumOptionBTNType.None;
                        GUIManager.Instance.RemoveTemplate(CurrentContentTemplate.templateName);
                        GUIManager.Instance.AddTemplate("Option_HelpIndex");
                        
                    }*/
                }
                break;

            case Option_Main_Manager.EnumOptionBTNType.Gift:
                {
                    if (CurContent != Option_Main_Manager.EnumOptionBTNType.Gift && CurContent != Option_Main_Manager.EnumOptionBTNType.None)
                    {
                        CurContent = Option_Main_Manager.EnumOptionBTNType.None;
                        GUIManager.Instance.RemoveTemplate(CurrentContentTemplate.templateName);
                        GUIManager.Instance.AddTemplate("Option_GiftCode");
                    }
                }
                break;

            case Option_Main_Manager.EnumOptionBTNType.BugReport:
                {
                    if (CurContent != Option_Main_Manager.EnumOptionBTNType.BugReport && CurContent != Option_Main_Manager.EnumOptionBTNType.None)
                    {

                        CurContent = Option_Main_Manager.EnumOptionBTNType.None;
                        GUIManager.Instance.RemoveTemplate(CurrentContentTemplate.templateName);
                        GUIManager.Instance.AddTemplate("Option_BugReport");
                    }
                }
                break;

            case Option_Main_Manager.EnumOptionBTNType.BackToVillage:
                {
                    Player.Instance.FSM.ChangeState(Player.Instance.IS);
                    Player.Instance.ReactivePlayer();
                    TutorialMan.Instance.SetTutorialFlag(false);
                    int missionID = _UI_CS_MapInfo.Instance.SceneNameToMissionID("Hub_Village") + 1;
                    UI_Fade_Control.Instance.FadeOutAndIn("Hub_Village", "Hub_Village", missionID - 1);
                }
                break;

            case Option_Main_Manager.EnumOptionBTNType.LogOut:
                {
                    Player.Instance.FSM.ChangeState(Player.Instance.IS);
                    Player.Instance.BuffMan.RemoveAllBuffs();
                    Player.Instance.abilityManager.SetAllDefaultSkills();
                    Player.Instance.CallOffSpirit();
                    CS_Main.Instance.g_commModule.SendMessage(ProtocolGame_SendRequest.LeaveWorld());
                    int missionID = _UI_CS_MapInfo.Instance.SceneNameToMissionID("Hub_Village") + 1;
                    UI_Fade_Control.Instance.FadeOutAndIn("EmptyScenes", "Hub_Village", missionID - 1);
                }
                break;

            case Option_Main_Manager.EnumOptionBTNType.QuitGame:
                {
                    Option_Main_Manager.Instance.ShowConfirm();
                }
                break;
        }
    }

    void Close_BTNClicked()
    {
        GUIManager.Instance.ChangeUIScreenState("IngameScreen");
        Player.Instance.ReactivePlayer();
    }

    #endregion

    #region DelegateFromSetting

    void SettingCheckBoxChanged(bool ischecked, Option_Setting_Manager.EnumOptionSettingCheckBoxType checkboxType)
    {
        Debug.Log("SETTING CB : " + checkboxType.ToString() + (ischecked ? "YES" : "NO"));
        switch (checkboxType)
        {
            case Option_Setting_Manager.EnumOptionSettingCheckBoxType.AutoAttack:
                {
                    GameConfig.IsAutoAttack = ischecked;
                }
                break;

            case Option_Setting_Manager.EnumOptionSettingCheckBoxType.AutoFadingLootName:
                {
                    GameConfig.IsAutoFadeLootNames = ischecked;

                    if (ischecked)
                    {
                        _UI_CS_IngameToolTipMan.Instance.Hide();
                    }
                    else
                    {
                        _UI_CS_IngameToolTipMan.Instance.Show();
                    }
                }
                break;

            case Option_Setting_Manager.EnumOptionSettingCheckBoxType.FullScreen:
                {
                    GameConfig.IsFullScreen = ischecked;

                    if (ischecked)
                        Screen.SetResolution(Screen.currentResolution.width, Screen.currentResolution.height, true);
                    else
                        Screen.SetResolution(1280, 720, false);
                }
                break;

            case Option_Setting_Manager.EnumOptionSettingCheckBoxType.Shadows:
                {
                    GameConfig.IsShadows = ischecked;

                    GameObject currentLight = GameObject.Find("Directional light-Shadow");

                    currentLight = currentLight == null ? GameObject.Find("Directional light-shadow") : currentLight;

                    if (currentLight != null)
                    {
                        Light light = currentLight.GetComponent<Light>();
                        light.shadows = ischecked ? LightShadows.Soft : LightShadows.None;
                    }
                }
                break;

            case Option_Setting_Manager.EnumOptionSettingCheckBoxType.MuteSFX:
                {
                    GameConfig.IsSFX = !ischecked;
                }
                break;

            case Option_Setting_Manager.EnumOptionSettingCheckBoxType.MuteMusic:
                {
                    GameConfig.IsMusic = !ischecked;
                }
                break;
        }
    }

    void AudioSFXVolChanged(float _value)
    {
        GameConfig.SFXVolumn = _value;
    }

    void AudioMusicVolChanged(float _value)
    {
        GameConfig.MusicVolumn = _value;
    }

    void VideoQualityChanged(string _option)
    {
        if (GameConfig.Quality != _option)
            PopUpBox.PopUpErr(LocalizeManage.Instance.GetDynamicText("RESTARTGAME"));

        // TODO : fix quality, it breaks texture when modifying ingame
        GameConfig.Quality = "Normal";  // _option;

        switch (_option.ToLower())
        {
            case "low":
                QualitySettings.SetQualityLevel(0, false);
                break;
            case "normal":
                UnityEngine.QualitySettings.SetQualityLevel(2, false);
                break;
            case "high":
                UnityEngine.QualitySettings.SetQualityLevel(4, false);
                break;
            case "best":
                UnityEngine.QualitySettings.SetQualityLevel(5, false);
                break;
        }
    }

    void DefaultBTNPressed()
    {
        SettingCheckBoxChanged(true, Option_Setting_Manager.EnumOptionSettingCheckBoxType.AutoAttack);
        SettingCheckBoxChanged(true, Option_Setting_Manager.EnumOptionSettingCheckBoxType.AutoAttack);
        SettingCheckBoxChanged(false, Option_Setting_Manager.EnumOptionSettingCheckBoxType.MuteSFX);
        SettingCheckBoxChanged(false, Option_Setting_Manager.EnumOptionSettingCheckBoxType.MuteMusic);
        AudioSFXVolChanged(0.8f);
        AudioMusicVolChanged(0.8f);

        SettingInit();
    }

    public void SettingInit()
    {
        if (Option_Setting_Manager.Instance)
        {
            Option_Setting_Manager.Instance.Get_GP_AutoAttack().isChecked = GameConfig.IsAutoAttack;
            Option_Setting_Manager.Instance.Get_GP_Auto_Fade_Loot_Names().isChecked = GameConfig.IsAutoFadeLootNames;
            Option_Setting_Manager.Instance.Get_Video_FullScreen().isChecked = GameConfig.IsFullScreen;
            Option_Setting_Manager.Instance.Get_Video_Shadows().isChecked = GameConfig.IsShadows;
            Option_Setting_Manager.Instance.Get_Audio_Mute_SFX().isChecked = !GameConfig.IsSFX;
            Option_Setting_Manager.Instance.Get_Audio_Mute_Music().isChecked = !GameConfig.IsMusic;

            Option_Setting_Manager.Instance.Get_Audio_Music_Vol().sliderValue = GameConfig.MusicVolumn;
            Option_Setting_Manager.Instance.Get_Audio_SFX_Vol().sliderValue = GameConfig.SFXVolumn;

            Option_Setting_Manager.Instance.Get_Video_Quality().selection = GameConfig.Quality = "Normal"; // TODO : fix quality choice
        }
    }

    #endregion

    #region DelegateFromGift
    public string backUpRedeemCode = "";
    private string redeemCodeUrl = @"http://redeem.spicyhorse.com/api/redeem/";// @"http://192.168.6.102/api/redeem/"; // real addres : 

    void GiftSendBTNPressed(string _giftCode)
    {
        backUpRedeemCode = _giftCode;
        StartCoroutine(SendMsgToServerForRedeemCode(_giftCode));
    }

    public IEnumerator SendMsgToServerForRedeemCode(string redeemCode)
    {
        WWWForm handelServer = new WWWForm();
        handelServer.AddField("game", "akaneiro");
        handelServer.AddField("uid", DataManager.Instance.GetMapValue(DataListType.AccountData, "uid"));
        handelServer.AddField("type", VersionManager.Instance.GetPlatformType().Get());
        handelServer.AddField("token", DataManager.Instance.GetMapValue(DataListType.AccountData, "gameCode")); 
        handelServer.AddField("code", redeemCode);
        handelServer.AddField("other", PlayerDataManager.Instance.ChaName);
        WWW url = new WWW(redeemCodeUrl, handelServer);
        yield return url;
        if (url.error != null)
        {
            Debug.LogError("Error." + url.error);
        }
        else
        {
            Debug.LogWarning("link server ok :receive alex msg");
            CheckReturnMsg(url.text);
        }
    }

    private void CheckReturnMsg(string text)
    {
        if (string.Compare(text, "OK") == 0)
        {
            LogManager.Log_Warn("Congratulate : alex rec ok.");
            PopUpBox.PopUpErr("Success! Your items have been sent to your Mailbox");
        }
        else
        {
            //please check OptionCtrl.cs line 168.
            //			_UI_CS_PopupBoxCtrl.PopUpError(text);
            #region new solution
            CS_Main.Instance.g_commModule.SendMessage(
                    ProtocolGame_SendRequest.GetGiftReq(backUpRedeemCode)
            );
            #endregion
        }
    }

    #endregion

    #region DelegateFromBugReport

    void BugReportBtnPressed()
    {
#if UNITY_WEBPLAYER
        Application.ExternalEval(@"window.open('http://support.spicyhorse.com/','Spicy Support')");
#else
        Application.OpenURL(@"http://support.spicyhorse.com/");
#endif
    }

    #endregion

    #endregion
}
