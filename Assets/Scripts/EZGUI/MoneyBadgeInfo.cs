using UnityEngine;
using System.Collections;
using System;

public class MoneyBadgeInfo : MonoBehaviour {

	//Instance
	public static MoneyBadgeInfo Instance = null;
	
	public UIPanel    InfoPanel;
	
	public SpriteText badgeText;
	public SpriteText badgeMaxText;
	public SpriteText badgeTimeText;
	public SpriteText skText;
	public SpriteText fkText;
	
	public UIButton AddKarmaBtn;
	public UIButton AddCrystalBtn;
	
	void Awake(){
		Instance = this;
	}
	
	// Use this for initialization
	void Start () {
		AddKarmaBtn.AddInputDelegate(AddKarmaBtnDelegate);	
		AddCrystalBtn.AddInputDelegate(AddCrystalBtnDelegate);	
		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void Hide(bool isHide){
		if(isHide){
			InfoPanel.Dismiss();
		}else{
			InfoPanel.BringIn();
		}
	}
	
	public void UpdateBadgeNum(int badgeNum){
		badgeText.Text = badgeNum.ToString();
	}
	
	public void UpDateBadge(int isFirst){
		string fileName = "Level.level";
		string _fileName = LocalizeManage.Instance.GetLangPath(fileName);
		TextAsset item = (TextAsset)Resources.Load(_fileName, typeof(TextAsset));
		string[] itemRowsList = item.text.Split('\n');	
		int level = 0;
		if(0 != isFirst){	
			level = isFirst;	
		}else{	
			level = _PlayerData.Instance.playerLevel;	 
		}
		for (int i = 3; i < itemRowsList.Length; ++i){			
			string pp = itemRowsList[i];
		   	string[] vals = pp.Split(new char[] { '	', '	' });		
			if(0 == string.Compare(vals[0],level.ToString())) {	
				badgeMaxText.Text = vals[2];
			}
		}
	}
	
	public void GetBadgeTime(){
		if(int.Parse(badgeText.text) < int.Parse(badgeMaxText.text)){
			badgeTimeText.Text = DateTime.Now.AddHours(1).ToString();
		}else{
			badgeTimeText.Text = "00:00";
		}
	}
	
	void AddKarmaBtnDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		case POINTER_INFO.INPUT_EVENT.TAP:
			if (Steamworks.activeInstance) {
				Steamworks.activeInstance.ShowShop("karma");
			} else {
				BuyKarmaPanel.Instance.basePanel.BringIn();
			}
			break;
		}	
	}	
	void AddCrystalBtnDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		case POINTER_INFO.INPUT_EVENT.TAP:
			if (Steamworks.activeInstance) {
				Steamworks.activeInstance.ShowShop("crystal");
			} else {
				BuyCrystalPanel.Instance.basePanel.BringIn();
			}
			break;
		}	
	}	
}
