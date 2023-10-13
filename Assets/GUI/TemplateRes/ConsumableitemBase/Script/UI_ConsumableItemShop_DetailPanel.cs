using UnityEngine;
using System.Collections;

public class UI_ConsumableItemShop_DetailPanel : MonoBehaviour {

    [SerializeField]  UI_Amout_Manager AmountManager;
    [SerializeField]  UI_MoneyGroupBase MoneyGroup;
    [SerializeField]  UILabel Label_Name;
    [SerializeField]  UILabel Label_FullDescrition;
    [SerializeField]  UITexture Item_Icon;

    UI_TypeDefine.UI_ConsumableItemShop_Item_data _curdata;

    void Start()
    {
        AmountManager.Count_Changed_Event += CountChanged;
    }

    public void UpdateDetailInfo( UI_TypeDefine.UI_ConsumableItemShop_Item_data _data)
    {
        _curdata = _data;
        Label_Name.text = _data.ItemName;
        Label_FullDescrition.text = _data.FullDescription;
        AmountManager.SetValue(1);
        ItemPrefabs.Instance.GetItemIcon(_data.ID, 0, 1, Item_Icon);
    }

    void CountChanged(int _curcount)
    {
        if (_curdata != null)
            MoneyGroup.UpdateAllInfo((_curdata.PriceType.Get() == EMoneyType.eMoneyType_FK), (_curdata.Price * _curcount).ToString());
    }

    void BuyBTN_Clicked()
    {
        UI_ConsumableItemShop_Manager.Instance.BuyItem(_curdata.ID, AmountManager.CurCount);
    }
}
