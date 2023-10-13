using UnityEngine;
using System.Collections;

public class MessageSender : MonoBehaviour {

    public enum EnumMessageSenderType
    {
        OnTriggerEnter,
        OnTriggerExit,
        WaitForOtherSourceToActive,
    }

    public Transform[] Targets;
    public string CallBackFunctionName;
    public EnumMessageSenderType SendType;

    public void ActiveMessage()
    {
        if (SendType == EnumMessageSenderType.WaitForOtherSourceToActive)
        {
            SendMessage();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (SendType == EnumMessageSenderType.OnTriggerEnter)
        {
            SendMessage();
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (SendType == EnumMessageSenderType.OnTriggerExit)
        {
            SendMessage();
        }
    }

    private void SendMessage()
    {
        foreach (Transform _tar in Targets)
        {
            _tar.SendMessage(CallBackFunctionName, SendMessageOptions.DontRequireReceiver);
        }
    }

}
