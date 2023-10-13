using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class SpeedUpBoxManager : MonoBehaviour {
	
	public static SpeedUpBoxManager Instance;
	
	void Awake() {
		Instance = this;
	}
	
	// Use this for initialization
	void Start () {
		GUIManager.Instance.AddTemplateInitEnd();
	}
	
	// Update is called once per frame
	void Update () {
		if(isUpdateCoolDownTime) {
			CalcCoolDownTime();
		}
	}
	
	#region Interface
	public void PopUpSpeedUpBox(float time,Texture2D img) {
		//transform.position = new Vector3(0,0,-2);
		isUpdateCoolDownTime = true;
		fcoolDownTime = time;
		gameObject.GetComponent<UITemplate>().Hide(false,false);
		SetIcon(img);
	}
	public void PopUpSpeedUpBox(float time,string imgName) {
		//transform.position = new Vector3(0,0,-2);
		isUpdateCoolDownTime = true;
		fcoolDownTime = time;
		gameObject.GetComponent<UITemplate>().Hide(false,false);
		SetIcon(imgName);
	}
	public void PopUpSpeedUpBox(float time, string imgName, UIAtlas _atlas)
	{
		isUpdateCoolDownTime = true;
		fcoolDownTime = time;
		iconSprite.atlas = _atlas;
		iconSprite.spriteName = imgName;
		
		gameObject.GetComponent<UITemplate>().Hide(false,false);
	}
	
	public void Hide() {
		gameObject.GetComponent<UITemplate>().Hide(true,false);
	}
	#endregion
	
	#region Local
	[SerializeField]
	private UITexture iconTexture;
	private void SetIcon(Texture2D img) {
		NGUITools.SetActive(iconTexture.gameObject,true);
		NGUITools.SetActive(iconSprite.gameObject,false);
		iconTexture.mainTexture = img;
	}
	[SerializeField]
	private UISprite iconSprite;
	private void SetIcon(string imgName) {
		NGUITools.SetActive(iconTexture.gameObject,false);
		NGUITools.SetActive(iconSprite.gameObject,true);
		iconSprite.spriteName = imgName;
	}
	
	private void CloseDelegate() {
		gameObject.GetComponent<UITemplate>().Hide(true,false);
	}
	public delegate void Handle_NowDelegate();
    public event Handle_NowDelegate OnNowDelegate;
	private void NowDelegate() {
		if(OnNowDelegate != null) {
			OnNowDelegate();
		}
	}
	public delegate void Handle_HalfHourDelegate();
    public event Handle_HalfHourDelegate OnHalfHourDelegate;
	private void HalfHourDelegate() {
		if(OnHalfHourDelegate != null) {
			OnHalfHourDelegate();
		}
	}
	public delegate void Handle_HourDelegate();
    public event Handle_HourDelegate OnHourDelegate;
	private void HourDelegate() {
		if(OnHourDelegate != null) {
			OnHourDelegate();
		}
	}
	
	[SerializeField]
	private Transform underBtnsRoot;
	private void HideUnderBtns(bool hide) {
		NGUITools.SetActive(underBtnsRoot.gameObject,!hide);
	}
	
	private float receiveSerTime = 0;
	public void SetReceiveSerTime(float time) {
		receiveSerTime = time;
	}
	public float GetReceiveTime() {
		return receiveSerTime;
	}
	
	private bool  isUpdateCoolDownTime = false;
	private float fcoolDownTime = 0f;
	[SerializeField]
	private UILabel time;
	[SerializeField]
	private UI_CoolDownCycle_Control CooldownTimer;
	private void CalcCoolDownTime(){
		string strCdt = "";
		//float tempTime = Time.time - receiveSerTime;
		//tempTime = (fcoolDownTime - tempTime);
		fcoolDownTime-= Time.deltaTime;
		float tempTime = fcoolDownTime;
		UpdatePrice(fcoolDownTime);
		if(time != null)
		{
			if(tempTime < 0.1f){
				isUpdateCoolDownTime = false;
				gameObject.GetComponent<UITemplate>().Hide(true,false);
			}else{
				strCdt = ((int)tempTime/3600).ToString()+"h "+((int)tempTime/60%60).ToString()+"m "+((int)tempTime%60).ToString()+"s";	
				//why 5400 sec? plz,ask matt.//
				if(tempTime >= 5400) {
					NGUITools.SetActive(underBtnsRoot.gameObject,true);
				}else {
					NGUITools.SetActive(underBtnsRoot.gameObject,false);
				}
				time.text = strCdt;
			}
		}
	}
	[SerializeField]
	private UILabel nowPrice;
	private void UpdatePrice(float time) {
		int imin = (int)time/60;
		int isec = (int)time%60;
		if(isec != 0) {
			isec = 1;
		}
		for(int i = 0;i<PlayerDataManager.Instance.coolDownCostList.Count;i++) {
			if(imin+isec <= PlayerDataManager.Instance.coolDownCostList[i].time) {
				if(PlayerDataManager.Instance.coolDownCostList[i].crystal == 0) {
					nowPrice.text = "FREE";
				}else {
					nowPrice.text = PlayerDataManager.Instance.coolDownCostList[i].crystal.ToString();
				}
				return;
			}
		}
	}
	#endregion
}
