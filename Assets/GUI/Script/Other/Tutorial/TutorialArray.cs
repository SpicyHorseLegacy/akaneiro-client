using UnityEngine;
using System.Collections;

public class TutorialArray : MonoBehaviour {

	// Use this for initialization
	void Start () {
		InitEvents();
//		TutorialMan.Instance.StartEvent(TutorialEventsType.Event12);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	[SerializeField]
	private TutorialEvent [] events;
	private void InitEvents() {
		if(TutorialMan.Instance != null) {
			TutorialMan.Instance.Reset();
			TutorialMan.Instance.ResetHuntList();
			TutorialMan.Instance.ResetCollectList();
			foreach(TutorialEvent ev in events) {
				Transform tran = (Transform)UnityEngine.Object.Instantiate(ev.transform);
				if(tran != null) {
					TutorialMan.Instance.AddEvent(tran.GetComponent<TutorialEvent>());
				}else {
					LogManager.Log_Error("Instantiate tran fial."+ev.name);
				}
			}
		}else {
			LogManager.Log_Error("Add event faile.TutorialMan.Instance is null");
		}
	}
	
}
