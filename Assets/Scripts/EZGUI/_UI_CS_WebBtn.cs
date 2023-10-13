using UnityEngine;
using System.Collections;

public class _UI_CS_WebBtn : MonoBehaviour {
	
	public UIButton WebBtn;
	
	// Use this for initialization
	void Start () {
		WebBtn.AddInputDelegate(WebDelegate);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void WebDelegate(ref POINTER_INFO ptr)
	{
		switch(ptr.evt)
		{
		  case POINTER_INFO.INPUT_EVENT.PRESS:
				{
					UrlOpener.Open("www.spicyhorse.com");
				}	
				break;
		   default:
				break;
		}	
	}
}
