using UnityEngine;
using System.Collections;

public class ShowDialog : EventBase {

	// Use this for initialization
	void Start () {
		Regis();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	[SerializeField]
	private int dialogID = 0;
	public override void Init() {
#if NGUI
		PlayerDataManager.Instance.ShowTutorialPanel(TutorialMan.Instance.GetTutorialStrTitle(dialogID),TutorialMan.Instance.GetTutorialStrContent(dialogID));
#else
		TutorialDialogEZ.Instance.PopUpDialog(dialogID);
#endif
	}
}
