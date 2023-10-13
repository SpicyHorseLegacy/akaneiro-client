using UnityEngine;
using System.Collections;

public class testdeath : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Alpha0))
		{
			EChatType type = new EChatType(5);
			CS_Main.Instance.g_commModule.SendMessage(
				   		ProtocolGame_SendRequest.ChatRequest(type,0,0,".attr 0 -100000")
			);
		}
	}
}
