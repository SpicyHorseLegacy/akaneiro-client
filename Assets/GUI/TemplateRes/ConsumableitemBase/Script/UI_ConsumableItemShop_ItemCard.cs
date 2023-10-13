using UnityEngine;
using System.Collections;

public class UI_ConsumableItemShop_ItemCard : MonoBehaviour
{
    [SerializeField]  UILabel Label_Name;
    [SerializeField]  UILabel Label_ShortDes;
    [SerializeField]  UI_MoneyGroupBase MoneyGroup;
    [SerializeField]  UITexture Item_Icon;

    public UI_TypeDefine.UI_ConsumableItemShop_Item_data Data { get { return _curdata; } }
    UI_TypeDefine.UI_ConsumableItemShop_Item_data _curdata;

    public void UpdateInfo(UI_TypeDefine.UI_ConsumableItemShop_Item_data _data)
    {
        _curdata = _data;
        Label_Name.text = _data.ItemName;
        Label_ShortDes.text = _data.ShortDescription;
        MoneyGroup.UpdateAllInfo((_data.PriceType.Get() == EMoneyType.eMoneyType_FK), _data.Price.ToString());
        ItemPrefabs.Instance.GetItemIcon(_data.ID, 0, 1, Item_Icon);
    }

    void ItemClicked()
    {
        UI_ConsumableItemShop_Manager.Instance.CardClicked(this);
    }
}
