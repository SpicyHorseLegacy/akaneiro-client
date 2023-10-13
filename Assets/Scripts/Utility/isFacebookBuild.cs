using UnityEngine;
using System.Collections;

public class isFacebookBuild : MonoBehaviour {
	
	public static bool isFaceBook;
	public bool isFacebook ;
	public static bool NeedToPay ;
	
	// Use this for initialization
	void Start () {
		isFaceBook = isFacebook;
	}
	
	// Update is called once per frame
	void Update () {
		isFaceBook = isFacebook;
	}
	
	void Awake () {
		isFaceBook = isFacebook;	
	}
	/*
	void OnGUI(){
		if (GUI.Button (new Rect(10, 10, 60, 25), "test")){
			//int kamaMoney = 0;
			//kamaMoney = int.Parse( MoneyBadgeInfo.Instance.skText.Text );
			//kamaMoney += 200;
			//MoneyBadgeInfo.Instance.skText.Text = kamaMoney.ToString();
			//Debug.Log("completely done !!! ** " + MoneyBadgeInfo.Instance.skText.Text);
			
			//SAccountInfo accountInfo;
			int val = _PlayerData.Instance.myAccountInfo.SK;
			val += 200;
			_PlayerData.Instance.myAccountInfo.SK  = val ;
			MoneyBadgeInfo.Instance.skText.Text = val.ToString();
			Debug.Log("completely done !!! ** " + MoneyBadgeInfo.Instance.skText.Text);
			
			//int val = 0;
			//val = PlayerDataManager.Instance.GetInGameKarma();
			//val+=200;
			//PlayerDataManager.Instance.SetInGameKarma(val);
			//Debug.Log("completely done !!! ** " + PlayerDataManager.Instance.GetKarmaVal());
			
			//ClientLogicCtrl.Instance.ShowPaySurplus("shard1");
		}
	}*/
}
