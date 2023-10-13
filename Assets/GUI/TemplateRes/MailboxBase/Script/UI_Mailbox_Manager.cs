using UnityEngine;
using System.Collections;

public class UI_Mailbox_Manager : MonoBehaviour
{
    public static UI_Mailbox_Manager Instance;
    public UI_Mailbox_ListManager ListManager;
    public UI_Mailbox_DetailManager DetailManager;
	
	void Awake(){	Instance = this;	}
	
    public void UpdateList(SMailInfo[] _mails)
    {
        ListManager.UpdateList(_mails);
        ListManager.ChooseFirstItem();
    }

    public void UpdateDetail(SMailInfo _mails)
    {
        DetailManager.UpdateDetialInfo(_mails);
    }

    public void GetAllSuc(int mailID)
    {
        DetailManager.GetAllSuc(mailID);
    }

    public void GetKarmaSuc(int mailID)
    {
        DetailManager.GetKarmaSuc(mailID);
    }

    public void GetCrystalSuc(int mailID)
    {
        DetailManager.GetCrystalSuc(mailID);
    }

    public void GetMailItemSuc(int mailID, int itemSlotID)
    {
        DetailManager.GetItemSuc(mailID, itemSlotID);
    }

    public void GetPetSuc(int mailID, int itemSlotID)
    {
        DetailManager.GetPetSuc(mailID, itemSlotID);
    }
	
	public delegate void Handle_UIMailCloseBTNClicked();
    public event Handle_UIMailCloseBTNClicked MailCloseClicked_Event;
    
	void CloseBTNClicked()
	{
		if(MailCloseClicked_Event != null)
			MailCloseClicked_Event();
	}
}