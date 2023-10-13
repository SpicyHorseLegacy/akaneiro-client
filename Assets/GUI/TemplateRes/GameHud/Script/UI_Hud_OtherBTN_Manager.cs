using UnityEngine;
using System.Collections;

public class UI_Hud_OtherBTN_Manager : MonoBehaviour
{
    [SerializeField]
    public UILabel m_newMailsCount;

    private int m_previousMailCount = 0;

    private int CountUnreadMails()
    {
        // Count current unread mails
        int count = 0;
        foreach (SMailInfo info in PlayerDataManager.Instance.MailList)
            if (info.state.Get() == EMailState.eMailState_Unread)
                count++;

        return count;
    }

    void RefreshMailList()
    {
        // Request the server to refresh mailbox
        if (InGameScreenMailBoxCtrl.Instance == null)
            CS_Main.Instance.g_commModule.SendMessage(ProtocolGame_SendRequest.GetPlayerMail());
    }

    void Start()
    {
        int newMailsCount = CountUnreadMails();
        m_newMailsCount.text = newMailsCount.ToString();
        m_previousMailCount = newMailsCount;

        // Refresh Mailbox every minute
        InvokeRepeating("RefreshMailList", 0f, 60f);
    }

    void FixedUpdate()
    {
        int newMailsCount = CountUnreadMails();
        if (newMailsCount != m_previousMailCount)
        {
            m_newMailsCount.text = newMailsCount == 0 ? "" : newMailsCount.ToString();
            m_previousMailCount = newMailsCount;
        }
    }

	void BTN_Setting_Clicked()
	{
		if(GUIManager.Instance)
			GUIManager.Instance.ChangeUIScreenState("OptionScreen");
	}
	
	void BTN_Email_Clicked()
	{
		if(GUIManager.Instance)
		{
			CS_Main.Instance.g_commModule.SendMessage(ProtocolGame_SendRequest.GetPlayerMail());
			GUIManager.Instance.ChangeUIScreenState("Mailbox_Screen");
		}
	}
}
