using UnityEngine;
using System.Collections;

public class UI_CraftingShop_MaterialGroup : MonoBehaviour {
    [SerializeField]  UITexture MaterialIcon;
    [SerializeField]  UILabel NumLabel;
    [SerializeField]  UI_MoneyGroup MoneyGroup;
    [SerializeField]  GameObject BuyBTN;

    int CurMatID;
    int CurNeedCount;
    int CurHaveCount;

    public void UpdateallInfo(int matID, int matNeedCount, mapCraftMaterial _craftmaterialmap)
    {
        CurMatID = matID;
        CurNeedCount = matNeedCount;
        ItemDropStruct matInfo = ItemDeployInfo.Instance.GetItemObject(matID,1,0,0,0,1);
        ItemPrefabs.Instance.GetItemIcon(matInfo._ItemID, matInfo._TypeID, matInfo._PrefabID, MaterialIcon);

        CurHaveCount = GetItemCountInInv(matID, matNeedCount, _craftmaterialmap);
        if (CurHaveCount < matNeedCount)
        {
            MoneyGroup.gameObject.SetActive(true);
            BuyBTN.gameObject.SetActive(true);
            MoneyGroup.UpdateMoney("" + matInfo._SaleVal * (matNeedCount - CurHaveCount));
            MoneyGroup.SetCenter();
            NumLabel.color = Color.red;
        }
        else
        {
            MoneyGroup.gameObject.SetActive(false);
            BuyBTN.gameObject.SetActive(false);
            NumLabel.color = Color.white;
        }
        NumLabel.text = "" + CurHaveCount + " /" + matNeedCount;
    }

    public int NeedCrystal()
    {
        ItemDropStruct matInfo = ItemDeployInfo.Instance.GetItemObject(CurMatID, 1, 0, 0, 0, 1);
        int price = matInfo._SaleVal * (CurNeedCount - CurHaveCount);
        if (price < 0) price = 0;
        return price;
    }

    private int GetItemCountInInv(int itemID, int needCount, mapCraftMaterial _craftmaterialmap)
    {
        int usedCount = 0;
        int _count = 0;
        foreach (_ItemInfo data in PlayerDataManager.Instance.bagList)
        {
            if (!data.empty && data.localData != null && data.localData._ItemID == itemID)
            {
                _count += data.count;

                // Add this slot to the mat map
                if (!_craftmaterialmap.ContainsKey(itemID))
                    _craftmaterialmap.Add(itemID, new vectorItemSlot());

                var itemSlots = _craftmaterialmap[itemID];
                SItemSlot slot = new SItemSlot();
                slot.slot = (uint)data.slot-1;
                if (usedCount + data.count > needCount)
                {
                    long delta = usedCount + data.count - needCount;
                    slot.count = (uint)needCount;
                    usedCount = needCount;
                }
                else
                {
                    slot.count = (uint)data.count;
                    usedCount += (int)(usedCount + data.count);
                }
                itemSlots.Add(slot);
            }
        }
        return _count;
    }

    #region Button Callback
    void BuyBTNClicked()
    {
        CS_Main.Instance.g_commModule.SendMessage(ProtocolGame_SendRequest.BuyConsumeItem(1, CurMatID, CurNeedCount - CurHaveCount));
    }
    #endregion
}
