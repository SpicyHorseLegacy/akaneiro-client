using UnityEngine;
using System.Collections;

public class IngamePopMsg : MonoBehaviour {
	
	//Instance
	public static IngamePopMsg Instance = null;
	
	public UIButton 	bg;
	public UIButton 	icon;
	public SpriteText 	msg; 
	public float   		showTime = 2;
	
	void Awake(){
		Instance = this;
	}
	
	// Use this for initialization
	void Start () {
		bg.Hide(true);
		icon.Hide(true);
		msg.Text = "";
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void AwakeIngameMsg(string info){
		bg.Hide(false);
		icon.Hide(false);
		msg.Text = info;
		StartCoroutine(DismissPopMsg());
	}
	
	private IEnumerator DismissPopMsg(){
		yield return new WaitForSeconds(showTime);
		bg.Hide(true);
		icon.Hide(true);
		msg.Text = "";
	}
}
