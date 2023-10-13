using UnityEngine;
using System.Collections;

public class _MissSpirte : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		if(isUpdateCoolDownTime) {
			CalcCoolDownTime();
		}
	}

	public int missID;
	
	#region Interface
	private MissionSeleData data;
	public void SetMissData(MissionSeleData info) {
		Hide(false);
		data = info;
		fcoolDownTime = info.serData.coolDownTime;
		CalcCoolDownTime();
	}
	public void Hide(bool hide) {
		NGUITools.SetActive(gameObject,!hide);
	}
	#endregion
	
	#region Local
	#region stars
	[SerializeField]
	private Transform starsRoot;
	private void HideStars(){
		NGUITools.SetActive(starsRoot.gameObject,false);
	}
	private void ShowStars(int count) {
		NGUITools.SetActive(starsRoot.gameObject,true);
		SetStarsState(count);
	}
	[SerializeField]
	private UISprite [] stars;
	private void SetStarsState(int count) {
		int i = 0;
		for(i = 0;i<stars.Length;i++) {
			NGUITools.SetActive(stars[i].gameObject,false);
		}
		if((count & 8) == 8) {
			NGUITools.SetActive(stars[3].gameObject,true);
		}
		if((count & 4) == 4) {
			NGUITools.SetActive(stars[2].gameObject,true);
		}
		if((count & 2) == 2) {
			NGUITools.SetActive(stars[1].gameObject,true);
		}
		if((count & 1) == 1) {
			NGUITools.SetActive(stars[0].gameObject,true);
		}
	}
	#endregion
	
	#region coolDwon
	private float fcoolDownTime = 0f;
	private bool  isUpdateCoolDownTime = false;
	private void CalcCoolDownTime(){
		string strCdt = "";
		float tempTime = Time.time - PlayerDataManager.Instance.GetReceiveSerMsgTime();
		tempTime = (fcoolDownTime - tempTime);
		if(tempTime < 0.1f){
			HideCoolDown();
			ShowStars(data.serData.star);
		}else{
			strCdt = ((int)tempTime/3600).ToString()+"h "+((int)tempTime/60%60).ToString()+"m "+((int)tempTime%60).ToString()+"s";	
			ShowCoolDown(strCdt);
			HideStars();
		}
	}
	[SerializeField]
	private Transform coolDownRoot;
	[SerializeField]
	private UILabel coolTimeText;
	private void HideCoolDown() {
		isUpdateCoolDownTime = false;
		NGUITools.SetActive(coolDownRoot.gameObject,false);
	}
	private void ShowCoolDown(string time) {
		isUpdateCoolDownTime = true;
		NGUITools.SetActive(coolDownRoot.gameObject,true);
		coolTimeText.text = time;
	}
	#endregion
	
	private void OnMissionBtnDegelate() {
		_MissSeleWin.Instance.InitSeleWinData(data,missID);
	}
	
	#endregion
	
	//----------------------------------------------------->>mm
	#if NGUI
	void OnMouseOver (){
		GameObject.Find("mouseCursors").gameObject.SendMessage("cursorPointingHandF");
	}
	void OnMouseExit (){
		GameObject.Find("mouseCursors").gameObject.SendMessage("cursorOpenedHandF");
	}
	#endif
	//----------------------------------------------------->>#mm
}
