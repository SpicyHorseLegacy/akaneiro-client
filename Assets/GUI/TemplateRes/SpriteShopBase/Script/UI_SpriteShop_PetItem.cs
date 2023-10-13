using UnityEngine;
using System.Collections;

public class UI_SpriteShop_PetItem : MonoBehaviour
{
    public UI_TypeDefine.UI_SpriteShop_PetItem_data GetData() { return itemdata; }
    private UI_TypeDefine.UI_SpriteShop_PetItem_data itemdata;

    [SerializeField]  UILabel Label_Name;
    [SerializeField]  UILabel Label_Level;
    [SerializeField]  UILabel Label_SimpleDescrption;
    [SerializeField]  NGUISlider EXP_Processbar;
    [SerializeField]  UISprite Pet_Icon;

    [SerializeField]  UILabel AlreadySummonedLabel;

    public void UpdateAllInfo(UI_TypeDefine.UI_SpriteShop_PetItem_data data)
    {
        itemdata = data;
        Label_Name.text = data.PetName;
        Label_Level.text = "LV. " + data.CurLevel;
        Label_SimpleDescrption.text = "Increases " + data.PetSimpleDescription;
        Debug.LogWarning(data.PetName + " || " + data.CurExp + " :: " + data.MaxExp);
        if (!data.IsMaxLv)
            EXP_Processbar.sliderValue = Mathf.Clamp01(data.CurExp / data.MaxExp);
        else
            EXP_Processbar.sliderValue = 0;
        Pet_Icon.spriteName = data.PetIconName;
        Pet_Icon.color = data.isLocked ? Color.black : Color.white;
        Pet_Icon.MakePixelPerfect();

        if (PlayerDataManager.Instance.CurrentPetId == data.PetID)
            AlreadySummonedLabel.gameObject.SetActive(true);

        else
            AlreadySummonedLabel.gameObject.SetActive(false);
    }

    void ItemClicked()
    {
        if (UI_SpriteShop_Manager.Instance != null)
        {
            UI_SpriteShop_Manager.Instance.BTNClicked(this);
        }
    }
}
