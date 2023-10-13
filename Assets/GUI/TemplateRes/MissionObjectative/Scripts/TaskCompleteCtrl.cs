using UnityEngine;
using System.Collections;

public class TaskCompleteCtrl : MonoBehaviour {
	
	public static TaskCompleteCtrl Instance;

	void Awake() {
		Instance = this;
	}
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
//		if(Input.GetKeyDown(KeyCode.A)) {
//			Play();
//		}
	}
	
	#region Interface
	[SerializeField]
	private UILabel taskName;
	public void SetTaskName(string name) {
		taskName.text = name;
	}
//	[SerializeField]
//	private UILabel xpVal;
	public void SetXpVal(string val) {
//		xpVal.text = val;
	}
//	[SerializeField]
//	private UILabel karmaVal;
	public void SetKarmaVal(string val) {
//		karmaVal.text = val;
	}

	public void Play() {
		TweenAlpha.Begin(gameObject,1,1);
		gameObject.GetComponent<TweenAlpha>().eventReceiver = gameObject;
		gameObject.GetComponent<TweenAlpha>().callWhenFinished = "DelayDelegate";
	}
	
	private void DelayDelegate() {
		TweenDelay.Begin(gameObject,2,"DismissDelegate",null);
	}
	
	public void DismissDelegate() {
		TweenAlpha.Begin(gameObject.gameObject,1,0);
	}
	#endregion
}
