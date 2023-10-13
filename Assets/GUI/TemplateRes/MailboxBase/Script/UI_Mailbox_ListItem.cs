using UnityEngine;
using System.Collections;

public class UI_Mailbox_ListItem : MonoBehaviour
{
    [SerializeField]
    UILabel Label_name;
    [SerializeField]
    UILabel Label_Title;
    [SerializeField]
    UILabel Label_ExpiresTime;

    [SerializeField]
    UITexture IconRead;

    [SerializeField]
    UITexture IconNonRead;

    public SMailInfo Data { get { return _curdata; } }
    SMailInfo _curdata;

    public void UpdateInfo(SMailInfo _info)
    {
        _curdata = _info;
        Label_name.text = _info.senderName;
        Label_Title.text = _info.title;
        Label_ExpiresTime.text = "Expires : " + (_info.timeout == 0f ? "Never" : ParseDurationToString(_info.timeout - _info.time));

        if (_info.state.Get() == EMailState.eMailState_Read)
        {
            IconRead.gameObject.SetActive(true);
            IconNonRead.gameObject.SetActive(false);
        }
        else
        {
            IconRead.gameObject.SetActive(false);
            IconNonRead.gameObject.SetActive(true);
        }
    }

    private string ParseDurationToString(long durationInSeconds)
    {
        long current = durationInSeconds;
        int days = (int)(current / (3600 * 24));
        current -= days * 3600 * 24;
        int hours = (int)(current / 3600);
        current -= hours * 3600;
        int minutes = (int)(current / 60);
        current -= minutes * 60;
        int seconds = (int)current;

        return days + "d " + hours + "h " + minutes + "m " + seconds + "s";
    }

    void ItemClicked()
    {
        UI_Mailbox_Manager.Instance.ListManager.ChooseItem(this);
        IconRead.gameObject.SetActive(true);
        IconNonRead.gameObject.SetActive(false);
    }
}
