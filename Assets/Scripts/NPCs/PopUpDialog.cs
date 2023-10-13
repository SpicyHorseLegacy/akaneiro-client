using UnityEngine;
using System.Collections;

public class PopUpDialog : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	[SerializeField]
	private Transform ActiveSound;
	[SerializeField]
	private int dialogID = 0;
	public virtual void PopMenu() {
		Player.Instance.FreezePlayer();
        if (ActiveSound)
            SoundCue.PlayPrefabAndDestroy(ActiveSound);
#if NGUI
		PlayerDataManager.Instance.ShowTutorialPanel(TutorialMan.Instance.GetTutorialStrTitle(dialogID),TutorialMan.Instance.GetTutorialStrContent(dialogID));
#else
		TutorialDialogEZ.Instance.PopUpDialog(dialogID);
#endif
	}
}
