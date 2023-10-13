using UnityEngine;
using System.Collections;

public class EventBase : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	protected void Regis() {
		if(transform.GetComponent<TutorialEvent>()) {
			transform.GetComponent<TutorialEvent>().OnInitDelegate += Init;
			transform.GetComponent<TutorialEvent>().OnEndDelegate += End;
		}
	}
	
	public virtual void Init() {
	}
	
	private void End() {
		transform.GetComponent<TutorialEvent>().OnInitDelegate -= Init;
		transform.GetComponent<TutorialEvent>().OnEndDelegate -= End;
	}
}
