using UnityEngine;
using System.Collections;

public class BottomTipManager : MonoBehaviour {
	
	public static BottomTipManager Instance;
	
	void Awake() {
		Instance = this;
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	[SerializeField]
	private UILabel content;
	
	public void Show(string msg) {
		content.text = msg;
		TweenAlpha.Begin(gameObject,1,1);
		gameObject.GetComponent<TweenAlpha>().eventReceiver = gameObject;
		gameObject.GetComponent<TweenAlpha>().callWhenFinished = "DelayDelegate";
	}
	
	private void DelayDelegate() {
		TweenDelay.Begin(gameObject,2,"Hide",null);
	}
	
	public void Hide() {
		TweenAlpha.Begin(gameObject,1,0);
	}
}
