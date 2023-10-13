using UnityEngine;
using System.Collections;

public class testmail : MonoBehaviour {

	void Update()
	{
		if(Input.GetKeyDown(KeyCode.Space))
		{
			CS_Main.Instance.g_commModule.SendMessage(ProtocolGame_SendRequest.GetPlayerMail());
			GUIManager.Instance.ChangeUIScreenState("Mailbox_Screen");
		}
	}
}
