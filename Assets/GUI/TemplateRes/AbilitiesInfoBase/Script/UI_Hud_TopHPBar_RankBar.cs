using UnityEngine;
using System.Collections;

public class UI_Hud_TopHPBar_RankBar : MonoBehaviour {

    [SerializeField]  UILabel WantedLabel;
    [SerializeField]  UISprite WantedSprite;

    public void UpdateAllInfo(UI_TypeDefine.UI_GameHud_TopHPBar_data.EnumUI_Hud_TopHPBar_MonsterRankType type)
    {
        if (type == UI_TypeDefine.UI_GameHud_TopHPBar_data.EnumUI_Hud_TopHPBar_MonsterRankType.Wanted ||
            type == UI_TypeDefine.UI_GameHud_TopHPBar_data.EnumUI_Hud_TopHPBar_MonsterRankType.Boss)
        {
            WantedLabel.gameObject.SetActive(true);
            WantedSprite.gameObject.SetActive(true);

            WantedLabel.text = "" + type.ToString();
        }
        else
        {
            WantedLabel.gameObject.SetActive(false);
            WantedSprite.gameObject.SetActive(false);
        }

    }
}
