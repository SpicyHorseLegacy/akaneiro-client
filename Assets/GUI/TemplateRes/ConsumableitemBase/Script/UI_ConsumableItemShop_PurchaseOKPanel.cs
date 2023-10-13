using UnityEngine;
using System.Collections;

public class UI_ConsumableItemShop_PurchaseOKPanel : MonoBehaviour 
{
    [SerializeField]  UILabel Label_ItemName;
    [SerializeField]  UITexture ItemIcon;
	[SerializeField]  GameObject SuccessVFX;

    public void UpdateBuySucInfo(string _buyiteminfo, int _itemid)
    {
        Label_ItemName.text = _buyiteminfo;
        ItemPrefabs.Instance.GetItemIcon(_itemid, 0, 1, ItemIcon);
		UnityEngine.Object.Instantiate(SuccessVFX, transform.position, transform.rotation);
    }

    void BTN_OK_Clicked()
    {
        Label_ItemName.text = "";
        ItemIcon.mainTexture = null;
        gameObject.SetActive(false);
    }
}