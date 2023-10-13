using UnityEngine;
using System.Collections;

public class _UI_CS_InvItemTip : MonoBehaviour {
	
	public UIButton tipBG;
	public SpriteText infoText;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void ShowTip(){
		
		tipBG.Hide(false);
		
	}
	
	public void HideTip(){
		
		infoText.Text = "";
		tipBG.Hide(true);
		
	}
}
