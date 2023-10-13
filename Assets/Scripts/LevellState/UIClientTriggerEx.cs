using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class UIClientTriggerEx : MonoBehaviour {
	
	public bool bTouch = false;
	Rect myRect;
	void Awake() {
	    if(transform.GetComponent<BoxCollider>()) {
			float sizex = transform.GetComponent<BoxCollider>().size.x;
		    sizex *= transform.localScale.x;
		    float sizez = transform.GetComponent<BoxCollider>().size.z;
		    sizez *= transform.localScale.z;
			myRect = new Rect(transform.position.x - sizex/2f,transform.position.z - sizez/2f,sizex,sizez);
			
		}
	}
	
	// Update is called once per frame
	void Update() {
		if(Player.Instance) {
		   Vector3 tempPoint = Player.Instance.transform.position;	
		   tempPoint.y = tempPoint.z;	
		   if(myRect.Contains(tempPoint) && !bTouch) {
				bTouch = true;
				ActiveTrigger();
				ArrowHide(false);
		    }
		}
	}
	
	[SerializeField]
	private Transform arrow;
	public void ArrowHide(bool hide) {
		arrow.gameObject.SetActive(!hide);
	}
	[SerializeField]
	private UIClientTriggerEx [] actTriggerList;
	private void ActiveTrigger() {
		foreach(UIClientTriggerEx trigger in actTriggerList) {
			trigger.bTouch = false;
			trigger.ArrowHide(true);
		}
	}
}
