using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UI_ConsumableItemShop_Manager : MonoBehaviour {

    public static UI_ConsumableItemShop_Manager Instance;
    public UI_TypeDefine.EnumConsumableItemShopUITYPE UIType;

    [SerializeField]  UI_ConsumableItemShop_ItemCard Item_Prefab;
    [SerializeField]  Transform Item_Parent;
    [SerializeField]  UI_ConsumableItemShop_UITYPE_Title_Manager UIType_Title_Manager;
    [SerializeField]  UI_ConsumableItemShop_DetailPanel DetailGroup;
    [SerializeField]  UI_ConsumableItemShop_PurchaseOKPanel BuySucGroup;
	
	[SerializeField] Transform UI_SFX_BuyItemSuccess;

    UI_ConsumableItemShop_ItemCard[] ItemCards = new UI_ConsumableItemShop_ItemCard[0];

    void Awake()
    {
        Instance = this;
        BuySucGroup.gameObject.SetActive(false);
    }

    #region public
    public void UpdateMainConsumableInfo(UI_TypeDefine.UI_ConsumableItemShop_Item_data[] _itemdatas, UI_TypeDefine.EnumConsumableItemShopUITYPE _askType)
    {
        UIType_Title_Manager.ChangeToTargetTitle(_askType);

        List<UI_ConsumableItemShop_ItemCard> _tempCardList = new List<UI_ConsumableItemShop_ItemCard>();
        for (int i = 0; i < _itemdatas.Length; i++)
        {
            if (i < ItemCards.Length)
            {
                _tempCardList.Add(ItemCards[i]);
            }
            else
            {
                UI_ConsumableItemShop_ItemCard _newcard = UnityEngine.Object.Instantiate(Item_Prefab) as UI_ConsumableItemShop_ItemCard;
                _newcard.transform.parent = Item_Parent;
                _newcard.transform.localScale = Vector3.one;
                _newcard.transform.localPosition = Vector3.zero;
                _tempCardList.Add(_newcard);
            }
        }
        for (int i = _itemdatas.Length; i < ItemCards.Length; i++)
        {
            Destroy(ItemCards[i].gameObject);
        }

        ItemCards = _tempCardList.ToArray();
        _tempCardList.Clear();
        _tempCardList = null;

        // update info
        for (int i = 0; i < ItemCards.Length; i++)
        {
            ItemCards[i].UpdateInfo(_itemdatas[i]);
        }

        Item_Parent.GetComponent<UIGrid>().Reposition();
    }

    public void BuyItemSuccess(int _itemType, int itemID, int buycount)
    {
		SoundCue.PlayPrefabAndDestroy(UI_SFX_BuyItemSuccess);
		
        BuySucGroup.gameObject.SetActive(true);

        ConsumableItemInfo _iteminfo = ConsumableItemsInfoManager.GetInfoByID(itemID);
		if(_iteminfo != null)
            BuySucGroup.UpdateBuySucInfo("" + _iteminfo.Name + (buycount > 1 ? " X " + buycount: ""), itemID);
    }

    public void CardClicked(UI_ConsumableItemShop_ItemCard _card)
    {
        if (_card.Data != null)
            DetailGroup.UpdateDetailInfo(_card.Data);
    }

    public void BuyItem(int _id, int _count)
    {
        int isconsumableitemforserver = 2;
        ItemDeployInfo.SItemProps _propinfo = ItemDeployInfo.GetItemPropsByID(_id);
        if (_propinfo.secType == 10 || _propinfo.secType == 11 || _propinfo.secType == 12 || _propinfo.secType == 13)
            isconsumableitemforserver = 1;
        CS_Main.Instance.g_commModule.SendMessage(ProtocolGame_SendRequest.BuyConsumeItem(isconsumableitemforserver, _id, _count));
    }

    #endregion

    #region BTN callback

    public void TopBarBTNClicked(UI_TypeDefine.EnumConsumableItemShopUITYPE _askType)
    {
        if (ConsumableItemTopBarClicked_Event != null && _askType != UI_TypeDefine.EnumConsumableItemShopUITYPE.NONE && _askType != UI_TypeDefine.EnumConsumableItemShopUITYPE.MAX && UIType != _askType)
            ConsumableItemTopBarClicked_Event(_askType);
    }

    void CloseBTNClicked()
    {
        if (ConsumableItemCloseClicked_Event != null)
            ConsumableItemCloseClicked_Event();
    }
    #endregion

    #region Delegates
    public delegate void Handle_UIShopConsumableItemCloseBTNClicked();
    public event Handle_UIShopConsumableItemCloseBTNClicked ConsumableItemCloseClicked_Event;

    public delegate void Handle_UIShopConsumableItemTopBarBTNClicked(UI_TypeDefine.EnumConsumableItemShopUITYPE askUIType);
    public event Handle_UIShopConsumableItemTopBarBTNClicked ConsumableItemTopBarClicked_Event;

    public delegate void Handle_UIShopConsumableItemBuyBTNClicked(int _id, int _count);
    public event Handle_UIShopConsumableItemBuyBTNClicked ConsumableItemBuyBTNClicked_Event;
    #endregion

   
}
