using UnityEngine;
using System.Collections;

public class LessMoneyMsg : MonoBehaviour {
	
	//Instance
	public static LessMoneyMsg Instance = null;
	
	public UIPanel 			bgPanel;
	public UIButton			YesBtn;
	public UIButton			NoBtn;
	
	
	
	void Awake(){
		Instance = this;
	}
	
	// Use this for initialization
	void Start () {
		YesBtn.AddInputDelegate(YesBtnDelegate);
		NoBtn.AddInputDelegate(NoBtnDelegate);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	public bool isKarma = true;
	void YesBtnDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:
				bgPanel.Dismiss();
				if(isKarma) {
					if (Steamworks.activeInstance) {
						Steamworks.activeInstance.ShowShop("karma");
					} else {
						BuyKarmaPanel.Instance.basePanel.BringIn();
					}
				}else {
					if (Steamworks.activeInstance) {
						Steamworks.activeInstance.ShowShop("crystal");
					} else {
						BuyCrystalPanel.Instance.basePanel.BringIn();
					}
				}
				break;
		}	
	}
	
	void NoBtnDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:
				bgPanel.Dismiss();
				break;
		}	
	}
}
