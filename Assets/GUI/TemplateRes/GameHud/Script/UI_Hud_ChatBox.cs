using UnityEngine;
using System.Collections;

public class UI_Hud_ChatBox : MonoBehaviour {
	
	public GameObject chatBoxClosedGroup ;
	public GameObject chatBoxOpenedGroup ;

	void Start () {
		closeChat();
	}
	
	
	void expandChat () {
		chatBoxManager.Instance.isInputState = true;
		chatBoxClosedGroup.gameObject.SetActive(false);
		chatBoxOpenedGroup.gameObject.SetActive(true);
		PlayerPrefs.SetInt ("isChat", 1);
		GameObject.Find("IngameScreen(Clone)").gameObject.GetComponent<InGameScreencharInfoCtrl>().chatWindowStatus = true;
	}
	
	void closeChat () {
		chatBoxManager.Instance.isInputState = false;
		chatBoxClosedGroup.gameObject.SetActive(true);
		chatBoxOpenedGroup.gameObject.SetActive(false);
		PlayerPrefs.SetInt ("isChat", 0);

        GameObject screen = GameObject.Find("IngameScreen(Clone)");

        if (screen != null)
        {
            if (screen.GetComponent<InGameScreencharInfoCtrl>() != null)
                screen.GetComponent<InGameScreencharInfoCtrl>().chatWindowStatus = false;
        }
	}
	
	void submitMessage (){
		
	}
}
