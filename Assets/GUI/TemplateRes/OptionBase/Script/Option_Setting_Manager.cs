using UnityEngine;
using System.Collections;

public class Option_Setting_Manager : MonoBehaviour {

    public static Option_Setting_Manager Instance;

    void Awake()
    {
        Instance = this;
        GUIManager.Instance.AddTemplateInitEnd();
    }
    
    #region Interface

    public enum EnumOptionSettingCheckBoxType
    {
        None,
        AutoAttack,
        AutoFadingLootName,
        FullScreen,
        Shadows,
        MuteSFX,
        MuteMusic,
        Max,
    }

    [SerializeField]  private UICheckbox GP_AutoAttack;
    public UICheckbox Get_GP_AutoAttack() { return GP_AutoAttack; }

    [SerializeField]  private UICheckbox GP_Auto_Fade_Loot_Names;
    public UICheckbox Get_GP_Auto_Fade_Loot_Names() { return GP_Auto_Fade_Loot_Names; }

    [SerializeField]  private UICheckbox Video_FullScreen;
    public UICheckbox Get_Video_FullScreen() { return Video_FullScreen; }

    [SerializeField]  private UICheckbox Video_Shadows;
    public UICheckbox Get_Video_Shadows() { return Video_Shadows; }

    [SerializeField]  private UICheckbox Audio_Mute_SFX;
    public UICheckbox Get_Audio_Mute_SFX() { return Audio_Mute_SFX; }

    [SerializeField]  private UICheckbox Audio_Mute_Music;
    public UICheckbox Get_Audio_Mute_Music() { return Audio_Mute_Music; }

    [SerializeField]  private UIPopupList Video_Quality;
    public UIPopupList Get_Video_Quality() { return Video_Quality; }

    [SerializeField]  private NGUISlider Audio_SFX_Vol;
    public NGUISlider Get_Audio_SFX_Vol() { return Audio_SFX_Vol; }

    [SerializeField]  private NGUISlider Audio_Music_Vol;
    public NGUISlider Get_Audio_Music_Vol() { return Audio_Music_Vol; }
    #endregion

    #region local

    #region Delegates
    public delegate void Handle_CheckBoxChanged_Delegate(bool isCheck, EnumOptionSettingCheckBoxType checkboxType);
    public event Handle_CheckBoxChanged_Delegate CheckBoxChanged_Event;
    void AutoAttackChanged(bool ischecked)
    {
        if (CheckBoxChanged_Event != null)
            CheckBoxChanged_Event(ischecked, EnumOptionSettingCheckBoxType.AutoAttack);
    }
    void AutoFadeLootNamesChanged(bool ischecked)
    {
        if (CheckBoxChanged_Event != null)
            CheckBoxChanged_Event(ischecked, EnumOptionSettingCheckBoxType.AutoFadingLootName);
    }

    void VideoFullScreenChanged(bool ischecked)
    {
        if (CheckBoxChanged_Event != null)
            CheckBoxChanged_Event(ischecked, EnumOptionSettingCheckBoxType.FullScreen);
    }

    void VideoShadowChanged(bool ischecked)
    {
        if (CheckBoxChanged_Event != null)
            CheckBoxChanged_Event(ischecked, EnumOptionSettingCheckBoxType.Shadows);
    }

    void AudioMuteSFXChanged(bool ischecked)
    {
        if (CheckBoxChanged_Event != null)
            CheckBoxChanged_Event(ischecked, EnumOptionSettingCheckBoxType.MuteSFX);
    }

    void AudioMuteMusicChanged(bool ischecked)
    {
        if (CheckBoxChanged_Event != null)
            CheckBoxChanged_Event(ischecked, EnumOptionSettingCheckBoxType.MuteMusic);
    }

    public delegate void Handle_AudioSFXVolChanged_Delegate(float value);
    public event Handle_AudioSFXVolChanged_Delegate AudioSFXVolChanged_Event;
    void AudioSFXVolChanged(float _value)
    {
        if (AudioSFXVolChanged_Event != null)
            AudioSFXVolChanged_Event(_value);
    }

    public delegate void Handle_AudioMusicVolChanged_Delegate(float value);
    public event Handle_AudioMusicVolChanged_Delegate AudioMusicVolChanged_Event;
    void AudioMusicVolChanged(float _value)
    {
        if (AudioMusicVolChanged_Event != null)
            AudioMusicVolChanged_Event(_value);
    }

    public delegate void Handle_VideoQualityChanged_Delegate(string option);
    public event Handle_VideoQualityChanged_Delegate VideoQualityChanged_Event;
    void VideoQualityChanged(string _option)
    {
        if (VideoQualityChanged_Event != null)
            VideoQualityChanged_Event(_option);
    }

    public delegate void Handle_DefaultBTNPressed_Delegate();
    public event Handle_DefaultBTNPressed_Delegate DefaultBTNPressed_Event;
    void DefaultBTNPressed()
    {
        if (DefaultBTNPressed_Event != null)
            DefaultBTNPressed_Event();
    }
    #endregion

    #endregion

}
