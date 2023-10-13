using UnityEngine;
using System.Collections;

public class UI_CraftingShop_Slot : MonoBehaviour {

    [SerializeField] UISprite ItemBG;
    [SerializeField] UITexture ItemIcon;
    public _ItemInfo GetData() { return _data; }
    _ItemInfo _data;

    public bool IsBelongEquipment;

    public void UpdateInfo(_ItemInfo _tempdata)
    {
        _data = _tempdata;
        if (_data != null)
        {
            ItemBG.color = _data.localData.BGColor;
            ItemPrefabs.Instance.GetItemIcon(_data.localData._ItemID, _data.localData._TypeID, _data.localData._PrefabID, ItemIcon);
        }
    }

    #region BTN callback

    void _ClickDelegate()
    {
        if (UI_CraftingShop_Manager.Instance)
        {
            UI_CraftingShop_Manager.Instance.ItemSlotClicked(this);
        }
    }

    #endregion
}
