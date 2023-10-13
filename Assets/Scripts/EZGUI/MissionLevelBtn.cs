using UnityEngine;
using System.Collections;

public class MissionLevelBtn : MonoBehaviour {
	
	public UIButton star;
	public SpriteText difficulty;
	public UIButton karmaIcon;
	public SpriteText karma;
	
	public SpriteText exp;
	
	public UIButton highLight;
	public UIButton highLightOver;
	public UIButton lockPic;
	
	public bool isUnlock = false;
	
	public int difficultIdx = 0;
	
	public UIButton icon;
	
	// Use this for initialization
	void Start () {
		transform.GetComponent<UIButton>().AddInputDelegate(LevelDelegate);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
#region Interface
	public void UpdateShowBtnState() {
		if(isUnlock) {
			if(MissionSelect.Instance.isUpdateCoolDownTime) {
				MissionSelect.Instance.ShowNextBtn(2);
			}else {
				MissionSelect.Instance.ShowNextBtn(1);
			}
		}else {
			MissionSelect.Instance.ShowNextBtn(3);
		}
	}
#endregion
	
#region Loacl
	void LevelDelegate(ref POINTER_INFO ptr) {
		switch(ptr.evt){
		case POINTER_INFO.INPUT_EVENT.TAP:
			MissionSelect.Instance.ChooseLevelThreat(difficultIdx);
			break;
		}	
	}
	
	public void IsLockLevel() {
		if(MissionSelect.Instance.currentInfo.canAcceptMaxLvl >= difficultIdx) {
			isUnlock = true;
			lockPic.Hide(true);
		}else {
			isUnlock = false;
			lockPic.Hide(false);
		}
	}
#endregion
}
