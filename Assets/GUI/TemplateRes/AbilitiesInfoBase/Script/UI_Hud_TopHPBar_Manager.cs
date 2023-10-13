using UnityEngine;
using System.Collections;

public class UI_Hud_TopHPBar_Manager : MonoBehaviour
{
    public static UI_Hud_TopHPBar_Manager Instance;

    void Awake(){        Instance = this;    }

    #region Interface

    [SerializeField]  GameObject TopHPBar;

    [SerializeField]  NGUISlider HPBar;
    [SerializeField]  UILabel NameLable;
    [SerializeField]  UISprite TypeIcon;
    [SerializeField]  UI_Hud_TopHPBar_RankBar RankBar;
    [SerializeField]  UI_Hud_TopHPBar_ElementalIcons ElementIconsManager;

    [SerializeField]  Color FriendlyBarColor = Color.green;
    [SerializeField]  Color NormalMonsterBarColor = Color.blue;
    [SerializeField]  Color WantedMonsterBarColor = Color.blue;
    [SerializeField]  Color BossMonsterBarColor = Color.blue;
    [SerializeField]  Color InteractiveBarColor = Color.yellow;

    #endregion

    #region Public

    public void Show() { TopHPBar.gameObject.SetActive(true); }

    public void Dispoose(){ TopHPBar.gameObject.SetActive(false); }

    public void UpdateAllInfo(UI_TypeDefine.UI_GameHud_TopHPBar_data _data)
    {
        NameLable.text = _data.TargetName;

        Color _tempColor = new Color();

        switch (_data.TargetType)
        {
            case UI_TypeDefine.UI_GameHud_TopHPBar_data.EnumUI_Hud_TopHPBar_TargetType.Friendly:
                _tempColor = FriendlyBarColor;
                TypeIcon.spriteName = "HUD_LifeBar_Friend";
                break;
            case UI_TypeDefine.UI_GameHud_TopHPBar_data.EnumUI_Hud_TopHPBar_TargetType.Monster:
                _tempColor = NormalMonsterBarColor;
                if (_data.MonsterRankType == UI_TypeDefine.UI_GameHud_TopHPBar_data.EnumUI_Hud_TopHPBar_MonsterRankType.Wanted) _tempColor = WantedMonsterBarColor;
                if (_data.MonsterRankType == UI_TypeDefine.UI_GameHud_TopHPBar_data.EnumUI_Hud_TopHPBar_MonsterRankType.Boss) _tempColor = BossMonsterBarColor;
                TypeIcon.spriteName = "HUD_LifeBar_Threat";
                break;
            case UI_TypeDefine.UI_GameHud_TopHPBar_data.EnumUI_Hud_TopHPBar_TargetType.MonsterBoss:
                _tempColor = NormalMonsterBarColor;
                if (_data.MonsterRankType == UI_TypeDefine.UI_GameHud_TopHPBar_data.EnumUI_Hud_TopHPBar_MonsterRankType.Wanted) _tempColor = WantedMonsterBarColor;
                if (_data.MonsterRankType == UI_TypeDefine.UI_GameHud_TopHPBar_data.EnumUI_Hud_TopHPBar_MonsterRankType.Boss) _tempColor = BossMonsterBarColor;
                TypeIcon.spriteName = "HUD_LifeBar_Boss";
                break;
            case UI_TypeDefine.UI_GameHud_TopHPBar_data.EnumUI_Hud_TopHPBar_TargetType.Interactive:
                _tempColor = InteractiveBarColor;
                TypeIcon.spriteName = "HUD_LifeBar_Box";
                break;
            case UI_TypeDefine.UI_GameHud_TopHPBar_data.EnumUI_Hud_TopHPBar_TargetType.Well:
                _tempColor = InteractiveBarColor;
                TypeIcon.spriteName = "HUD_LifeBar_Well";
                break;
            case UI_TypeDefine.UI_GameHud_TopHPBar_data.EnumUI_Hud_TopHPBar_TargetType.Switch:
                _tempColor = InteractiveBarColor;
                TypeIcon.spriteName = "HUD_LifeBar_Switch";
                break;
            case UI_TypeDefine.UI_GameHud_TopHPBar_data.EnumUI_Hud_TopHPBar_TargetType.Dialog:
                _tempColor = InteractiveBarColor;
                TypeIcon.spriteName = "HUD_LifeBar_Talk";
                break;
        }

        HPBar.foreground.GetComponent<UIWidget>().color = _tempColor;
        TypeIcon.color = _tempColor;

        RankBar.UpdateAllInfo(_data.MonsterRankType);
        ElementIconsManager.UpdateAllInfo(_data.ElementalData);

        if (_data.MAXHP > 0)
            HPBar.sliderValue = _data.CurHP / _data.MAXHP;
        else
            HPBar.sliderValue = 1;
    }

    #endregion

    #region Local



    #endregion
}
