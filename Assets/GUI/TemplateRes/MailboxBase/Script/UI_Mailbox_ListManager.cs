using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UI_Mailbox_ListManager : MonoBehaviour {

    public GameObject ListParent;
	[SerializeField] UILabel m_emptylabel;
    [SerializeField] UI_Mailbox_ListItem ItemPrefab;
	UI_Mailbox_ListItem[] ListItems = new UI_Mailbox_ListItem[0];

    public void UpdateList(SMailInfo[] _mailinfo)
    {
		m_emptylabel.gameObject.SetActive(_mailinfo.Length == 0);

        List<UI_Mailbox_ListItem> _items = new List<UI_Mailbox_ListItem>();
        for (int i = 0; i < _mailinfo.Length; i++ )
        {
            _items.Add(GetListItem(i));
        }

        for (int i = _mailinfo.Length; i < ListItems.Length; i++)
        {
            Destroy(ListItems[i].gameObject);
        }

        ListItems = _items.ToArray();

        // update info
        for (int i = 0; i < ListItems.Length; i++)
        {
            ListItems[i].UpdateInfo(_mailinfo[i]);
        }

        // reposition slots
        RepositionEquipmentslots();
    }

    public void ChooseFirstItem()
    {
        ChooseItem(0);
    }

    public void ChooseItem(int index)
    {
        if (index < ListItems.Length)
        {
            if (ListItems[index] != null && ListItems[index].Data != null)
            {
                CS_Main.Instance.g_commModule.SendMessage(ProtocolGame_SendRequest.OpenMail(ListItems[index].Data.id));

                foreach(SMailInfo mail in PlayerDataManager.Instance.MailList)
                    if (mail.id == ListItems[index].Data.id)
                        mail.state = new EMailState(EMailState.eMailState_Read);

            }

            UI_Mailbox_Manager.Instance.UpdateDetail(ListItems[index].Data);
        }
    }

    public void ChooseItem(UI_Mailbox_ListItem _item)
    {
        for (int i = 0; i < ListItems.Length; i++)
        {
            if (_item == ListItems[i])
            {
                ChooseItem(i);
                return;
            }
        }
    }

    UI_Mailbox_ListItem GetListItem(int _index)
    {
        if (_index < ListItems.Length)
        {
            return ListItems[_index];
        }
        else
        {
            UI_Mailbox_ListItem _newitem = UnityEngine.Object.Instantiate(ItemPrefab) as UI_Mailbox_ListItem;
            _newitem.transform.parent = ListParent.transform;
            _newitem.transform.localScale = Vector3.one;
            _newitem.transform.localPosition = Vector3.zero;
            return _newitem;
        }
    }

    void RepositionEquipmentslots()
    {
    }
}
