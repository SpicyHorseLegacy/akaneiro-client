using UnityEngine;
using System.Collections;

public class TutorialDialogEZ : MonoBehaviour {
	
	public UIPanel dialogPanel;
	public UIButton continueBtn;
	public SpriteText titleText;
	public SpriteText contentText;
	
	public static TutorialDialogEZ Instance = null;
	
	void Awake() {
		Instance = this;
	}
	
	// Use this for initialization
	void Start () {
		continueBtn.AddInputDelegate(ContinueDelegate);
	}
	
	// Update is called once per frame
	void Update () {
//		if(Input.GetKeyDown(KeyCode.E)){
//			TutorialMan.Instance.StartEvent(TutorialEventsType.Event1);
//		}
	}
	
	public void PopUpDialog(int id) {
		SetDialogContent(TutorialMan.Instance.GetTutorialStrTitle(id),TutorialMan.Instance.GetTutorialStrContent(id));
		dialogPanel.BringIn();
	}
	public void DismissDialog() {
		dialogPanel.Dismiss();
	}
	public void SetDialogContent(string title,string content) {
		titleText.Text = title;
		contentText.Text = content;
	}
	void ContinueDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:	
				DismissDialog();
				TutorialMan.Instance.AddBranchEndFlag();
				Player.Instance.ReactivePlayer();
				GameCamera.BackToPlayerCamera();
				break;
		}	
	}
	public void HideContinue(bool hide) {
		continueBtn.Hide(hide);
	}
	
}
