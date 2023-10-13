using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class UIClientTrigger : MonoBehaviour {
	
	public int triggerID = 0;
    
	Rect myRect;
	
	public bool bTouch = false;
	

	void Awake() 
	{
	    if(transform.GetComponent<BoxCollider>())
		{
			float sizex = transform.GetComponent<BoxCollider>().size.x;
		    sizex *= transform.localScale.x;
		    float sizez = transform.GetComponent<BoxCollider>().size.z;
		    sizez *= transform.localScale.z;
			myRect = new Rect(transform.position.x - sizex/2f,transform.position.z - sizez/2f,sizex,sizez);
			
		}
	}
	
	// Update is called once per frame
	void Update() 
	{
		if(Player.Instance == null) {
			return;
		}
		
	   Vector3 tempPoint = Player.Instance.transform.position;
			
	   tempPoint.y = tempPoint.z;
			
	   if(myRect.Contains(tempPoint) && !bTouch)
	   {
		  bTouch = true;
			
		  switch(triggerID)
		  {
		     case 10001:
		     {
				Player.Instance.FreezePlayer();
			    StartCoroutine(Wait2sec());
			    break;
			 }
			 case 10002:
			 {
				TutorialMan.Instance.DelArrow3D("event3");
				TutorialMan.Instance.StartEvent(TutorialEventsType.Event5);
			   break;
			 }
			 case 10003:
			 {
				TutorialMan.Instance.StartEvent(TutorialEventsType.Event7);
			   break;
			 }
			 case 10004:
			 {
				TutorialMan.Instance.DelArrow3D("event3");
			   break;
			 }	
			 case 10008:
			 {
				TutorialMan.Instance.StartEvent(TutorialEventsType.Event10);
			   break;
			 }
			 case 10009:
			 {
				TutorialMan.Instance.StartEvent(TutorialEventsType.Event17);
			   break;
			 }
		  }
	    }
	}
	
	private IEnumerator Wait2sec(){
		yield return new WaitForSeconds(1f);
//		Tutorial.Instance.AwakeOldManUI(0);
//		TutorialNpc.Instance.HideObj(TutorialNpc.Instance.oldLady2);
		Player.Instance.ReactivePlayer();
		TutorialMan.Instance.StartEvent(TutorialEventsType.Event1);
	}
}
