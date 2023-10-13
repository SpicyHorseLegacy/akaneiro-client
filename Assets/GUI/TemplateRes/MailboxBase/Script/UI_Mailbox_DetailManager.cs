using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UI_Mailbox_DetailManager : MonoBehaviour
{
    [SerializeField]
    GameObject m_confirmToDelete;
    [SerializeField]
    UILabel Label_Title;
    [SerializeField]
    UI_MailBox_DetailItem ItemPrefab;
    [SerializeField]
    UI_MailBox_Detail_Content m_contentmanager;
    [SerializeField]
    UIGrid ItemParent;
    UI_MailBox_DetailItem[] Items = new UI_MailBox_DetailItem[0];

    [SerializeField]
    GameObject BTN_AcceptAll;

    public SMailInfo Data { get { return _curMailInfo; } }
    SMailInfo _curMailInfo;

    public void UpdateDetialInfo(SMailInfo _info)
    {
        _curMailInfo = _info;

        Label_Title.text = _info.title;
        m_contentmanager.UpdateMailContent(_info.content);

        List<UI_TypeDefine.UI_Mailbox_Item_data> _data = new List<UI_TypeDefine.UI_Mailbox_Item_data>();

        if (_info.karma > 0)
        {
            UI_TypeDefine.UI_Mailbox_Item_data _newdata = new UI_TypeDefine.UI_Mailbox_Item_data();
            _newdata.Count = (uint)_info.karma;
            _newdata.ItemType = UI_TypeDefine.UI_Mailbox_Item_data.EnumMailItemType.Karma;

            _data.Add(_newdata);
        }

        if (_info.crystal > 0)
        {
            UI_TypeDefine.UI_Mailbox_Item_data _newdata = new UI_TypeDefine.UI_Mailbox_Item_data();
            _newdata.Count = (uint)_info.crystal;
            _newdata.ItemType = UI_TypeDefine.UI_Mailbox_Item_data.EnumMailItemType.Crystal;

            _data.Add(_newdata);
        }

        int _slotid = 0;
        foreach (SItemInfo serInfo in _info.itemVec)
        {
            ItemDropStruct localInfo = ItemDeployInfo.Instance.GetItemObject(serInfo.ID, serInfo.perfrab, serInfo.gem, serInfo.enchant, serInfo.element, (int)serInfo.level);

            UI_TypeDefine.UI_Mailbox_Item_data _newdata = new UI_TypeDefine.UI_Mailbox_Item_data();
            _newdata.ItemName = localInfo.ItemName;
            _newdata.Count = serInfo.count;
            _newdata.ItemType = UI_TypeDefine.UI_Mailbox_Item_data.EnumMailItemType.Item;
            _newdata.slotID = _slotid;
            _newdata.ItemID = localInfo._ItemID;
            _newdata.ItemTypeID = localInfo._TypeID;
            _newdata.ItemPrefabID = localInfo._PrefabID;

            _data.Add(_newdata);

            _slotid++;
        }

        _slotid = 0;
        foreach (int m in _info.petIDVec)
        {
            SinglePetListInfo _petinfo = PetsInfo.GetPetListInfoByID(m);

            UI_TypeDefine.UI_Mailbox_Item_data _newdata = new UI_TypeDefine.UI_Mailbox_Item_data();
            _newdata.ItemName = _petinfo.m_name;
            _newdata.Count = 1;
            _newdata.ItemType = UI_TypeDefine.UI_Mailbox_Item_data.EnumMailItemType.Pet;
            _newdata.slotID = _slotid;
            _newdata.ItemIcon = PetsInfo.GetPetIconByType(_petinfo.m_Type);

            _data.Add(_newdata);

            _slotid++;
        }

        BTN_AcceptAll.SetActive(_data.Count > 0);

        UpdateItems(_data.ToArray());
        _data.Clear();
        _data = null;
    }

    public void GetAllSuc(int mailID)
    {
        if (mailID == _curMailInfo.id)
        {
            foreach (UI_MailBox_DetailItem _item in Items)
            {
                _item.GetSuc();
            }
        }
    }

    public void GetKarmaSuc(int mailID)
    {
        if (mailID == _curMailInfo.id)
        {
            foreach (UI_MailBox_DetailItem _item in Items)
            {
                if (_item.Data != null && _item.Data.ItemType == UI_TypeDefine.UI_Mailbox_Item_data.EnumMailItemType.Karma)
                {
                    _item.GetSuc();
                    return;
                }
            }
        }

    }

    public void GetCrystalSuc(int mailID)
    {
        if (mailID == _curMailInfo.id)
        {
            foreach (UI_MailBox_DetailItem _item in Items)
            {
                if (_item.Data != null && _item.Data.ItemType == UI_TypeDefine.UI_Mailbox_Item_data.EnumMailItemType.Crystal)
                {
                    _item.GetSuc();
                    return;
                }
            }
        }
    }

    public void GetItemSuc(int mailID, int slotID)
    {
        if (mailID == _curMailInfo.id)
        {
            foreach (UI_MailBox_DetailItem _item in Items)
            {
                if (_item.Data != null && _item.Data.ItemType == UI_TypeDefine.UI_Mailbox_Item_data.EnumMailItemType.Item)
                {
                    if (_item.Data.slotID == slotID)
                    {
                        _item.GetSuc();
                    }
                    if (_item.Data.slotID > slotID)
                        _item.Data.slotID--;
                }
            }
        }
    }

    public void GetPetSuc(int mailID, int slotID)
    {
        if (mailID == _curMailInfo.id)
        {
            foreach (UI_MailBox_DetailItem _item in Items)
            {
                if (_item.Data != null && _item.Data.ItemType == UI_TypeDefine.UI_Mailbox_Item_data.EnumMailItemType.Pet)
                {
                    if (_item.Data.slotID == slotID)
                    {
                        _item.GetSuc();
                    }
                    if (_item.Data.slotID > slotID)
                        _item.Data.slotID--;
                }
            }
        }
    }

    public void GetBTNClickedFromItem(UI_MailBox_DetailItem _item)
    {
        if (_item.Data == null) return;

        switch (_item.Data.ItemType)
        {
            case UI_TypeDefine.UI_Mailbox_Item_data.EnumMailItemType.Karma:
                CS_Main.Instance.g_commModule.SendMessage(ProtocolGame_SendRequest.GetMailKarma(_curMailInfo.id));
                break;
            case UI_TypeDefine.UI_Mailbox_Item_data.EnumMailItemType.Crystal:
                CS_Main.Instance.g_commModule.SendMessage(ProtocolGame_SendRequest.GetMailCrystal(_curMailInfo.id));
                break;
            case UI_TypeDefine.UI_Mailbox_Item_data.EnumMailItemType.Item:
                CS_Main.Instance.g_commModule.SendMessage(ProtocolGame_SendRequest.GetMailItem(_curMailInfo.id, _item.Data.slotID));
                break;
            case UI_TypeDefine.UI_Mailbox_Item_data.EnumMailItemType.Pet:
                CS_Main.Instance.g_commModule.SendMessage(ProtocolGame_SendRequest.GetMailPet(_curMailInfo.id, _item.Data.slotID));
                break;
        }
    }

    void AcceptAllBTNClicked()
    {
        CS_Main.Instance.g_commModule.SendMessage(ProtocolGame_SendRequest.GetMailAll(_curMailInfo.id));
    }

    void DeleteBTNClicked()
    {
        m_confirmToDelete.SetActive(true);
    }

    void YesDelegate()
    {
        m_confirmToDelete.SetActive(false);
        CS_Main.Instance.g_commModule.SendMessage(ProtocolGame_SendRequest.DeleteMail(_curMailInfo.id));
    }

    void NoDelegate()
    {
        m_confirmToDelete.SetActive(false);
    }

    void UpdateItems(UI_TypeDefine.UI_Mailbox_Item_data[] _data)
    {
        List<UI_MailBox_DetailItem> _items = new List<UI_MailBox_DetailItem>();
        for (int i = 0; i < _data.Length; i++)
        {
            _items.Add(getItem(i));
        }

        for (int i = _data.Length; i < Items.Length; i++)
        {
            Destroy(Items[i].gameObject);
        }

        Items = _items.ToArray();

        // update info
        for (int i = 0; i < Items.Length; i++)
        {
            Items[i].UpdateInfo(_data[i]);
        }

        // reposition slots
        RepositionDetialItems();
    }

    UI_MailBox_DetailItem getItem(int _index)
    {
        if (_index < Items.Length)
        {
            return Items[_index];
        }
        else
        {
            UI_MailBox_DetailItem _newitem = UnityEngine.Object.Instantiate(ItemPrefab) as UI_MailBox_DetailItem;
            _newitem.transform.parent = ItemParent.transform;
            _newitem.transform.localScale = Vector3.one;
            _newitem.transform.localPosition = Vector3.zero;
            return _newitem;
        }
    }

    void RepositionDetialItems()
    {
        Vector3 _pos = ItemParent.transform.localPosition;
        _pos.y = m_contentmanager.GetItemGridPosY();
        ItemParent.transform.localPosition = _pos;
        ItemParent.Reposition();
    }
}