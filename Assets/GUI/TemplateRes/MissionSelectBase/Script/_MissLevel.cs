using UnityEngine;
using System.Collections;

public class _MissLevel : MonoBehaviour {
	
	[SerializeField]
	private Transform infoRoot;
	[SerializeField]
	private UILabel karma;
	[SerializeField]
	private UILabel exp;
	[SerializeField]
	private UISprite lockIcon;
	[SerializeField]
	private int missIdx = 0;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	private MissionSeleData data;
	public void InitLevelInfo(bool _isLock,MissionSeleData info) {
		isLock = _isLock;
		data = info;
		if(isLock) {
			NGUITools.SetActive(infoRoot.gameObject,false);
			NGUITools.SetActive(lockIcon.gameObject,true);
		}else {
			NGUITools.SetActive(infoRoot.gameObject,true);
			int idx = _UI_CS_MapInfo.Instance.GetLevelIndex(_MissSeleWin.Instance.GetCurrentMissionID(),missIdx-1);
			exp.text = _UI_CS_MapInfo.Instance.Itemlist[_MissSeleWin.Instance.GetCurrentRegionID()+1].levelList[idx].xp.ToString();
			data.exp = int.Parse(exp.text);
			karma.text = _UI_CS_MapInfo.Instance.Itemlist[_MissSeleWin.Instance.GetCurrentRegionID()+1].levelList[idx].sk.ToString();
			data.karma = int.Parse(karma.text);
			data.scenseName = _UI_CS_MapInfo.Instance.Itemlist[_MissSeleWin.Instance.GetCurrentRegionID()+1].levelList[idx].mapName;
			NGUITools.SetActive(lockIcon.gameObject,false);
		}
	}
	private bool isLock;
	public void CheckDelegate() {
		_MissSeleWin.Instance.UpdateMainBtns(isLock);
		if(!_MissSeleWin.Instance.isUpdateCoolDownTime && !isLock) {
			_MissSeleWin.Instance.SetCurMissID(data.serData.missionID+missIdx);
			switch(missIdx) {
			case 1:
				_MissSeleWin.Instance.SetRecommedLv(data.localData.rcLow);
				_MissSeleWin.Instance.SetMainBtnTitle("EASY");
				_MissSeleWin.Instance.SetLvIconColor(1);
				break;
			case 2:
				_MissSeleWin.Instance.SetRecommedLv(data.localData.rcMed);
				_MissSeleWin.Instance.SetMainBtnTitle("MEDIUM");
				_MissSeleWin.Instance.SetLvIconColor(2);
				break;
			case 3:
				_MissSeleWin.Instance.SetRecommedLv(data.localData.rcHi);
				_MissSeleWin.Instance.SetMainBtnTitle("HARD");
				_MissSeleWin.Instance.SetLvIconColor(3);
				break;
			case 4:
				_MissSeleWin.Instance.SetRecommedLv(data.localData.rcOvr);
				_MissSeleWin.Instance.SetMainBtnTitle("OVERRUN");
				_MissSeleWin.Instance.SetLvIconColor(4);
				break;
			}
		}else if(_MissSeleWin.Instance.isUpdateCoolDownTime) {
			_MissSeleWin.Instance.SetMainBtnTitle("COOL DOWN");
			_MissSeleWin.Instance.SetRecommedLv("???");
			_MissSeleWin.Instance.SetLvIconColor(0);
		}else {
			_MissSeleWin.Instance.SetLvIconColor(0);
			_MissSeleWin.Instance.SetRecommedLv("???");
			switch(missIdx) {
			case 1:
				_MissSeleWin.Instance.SetMainBtnTitle("COMPLETE EASY TO UNLOCK");
				break;
			case 2:
				_MissSeleWin.Instance.SetMainBtnTitle("COMPLETE EASY TO UNLOCK");
				break;
			case 3:
				_MissSeleWin.Instance.SetMainBtnTitle("COMPLETE MEDIUM TO UNLOCK");
				break;
			case 4:
				_MissSeleWin.Instance.SetMainBtnTitle("COMPLETE HARD TO UNLOCK");
				break;
			}
			
		}
	}
}
