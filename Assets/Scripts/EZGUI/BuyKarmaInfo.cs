using UnityEngine;
using System.Collections;

public class BuyKarmaInfo : MonoBehaviour {
	
	public UIButton		bg;
	public SpriteText	karmaText;
	public SpriteText	payMoneyText;
	public UIButton 	icon;
	public int 			idx;
	public int 			type;
	public string		facebookIdx;
	private GameObject	facebookPaymentHolder;
	private string		productURL;
	
	void Awake (){
		facebookPaymentHolder = GameObject.Find("facebookDownloader").gameObject;	
	}
	// Use this for initialization
	void Start () {
		bg.AddInputDelegate(BgDelegate);	
		icon.SetUVs(new Rect(0,0,1,1));
		icon.SetTexture(Platform.Instance.platformIcon[Platform.Instance.platformType.Get()]);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void BgDelegate(ref POINTER_INFO ptr) {
		switch(ptr.evt) {
		   case POINTER_INFO.INPUT_EVENT.MOVE:
				BuyKarmaPanel.Instance.ChangeInfoText(type);
				BuyCrystalPanel.Instance.ChangeInfoText(type);
			break;
		   case POINTER_INFO.INPUT_EVENT.TAP:
                if (Screen.fullScreen){
                    OptionCtrl.Instance.ToWindows(); 
                }
				//不弹出确认界面，而改为直接购买 by 20130423//
//				BuyKarma.Instance.popupMsgPanel.BringIn();
			if (isFacebookBuild.isFaceBook == true){
				//Debug.Log("is facebook version");
				if (facebookPaymentHolder != null){
					if (facebookIdx == "k1"){
						productURL = "http://facebook.angry-red.com/KS_1.html";
					}else if (facebookIdx == "k2"){
						productURL = "http://facebook.angry-red.com/KS_2.html";
					}else if (facebookIdx == "k3"){
						productURL = "http://facebook.angry-red.com/KS_3.html";
					}else if (facebookIdx == "k4"){
						productURL = "http://facebook.angry-red.com/KS_4.html";
					}else if (facebookIdx == "k5"){
						productURL = "http://facebook.angry-red.com/KS_5.html";
					}else if (facebookIdx == "k6"){
						productURL = "http://facebook.angry-red.com/KS_6.html";
					}else if (facebookIdx == "k7"){
						productURL = "http://facebook.angry-red.com/KS_7.html";
					}else if (facebookIdx == "c1"){
						productURL = "http://facebook.angry-red.com/CP_1.html";
					}else if (facebookIdx == "c2"){
						productURL = "http://facebook.angry-red.com/CP_2.html";
					}else if (facebookIdx == "c3"){
						productURL = "http://facebook.angry-red.com/CP_3.html";
					}else if (facebookIdx == "c4"){
						productURL = "http://facebook.angry-red.com/CP_4.html";
					}else if (facebookIdx == "c5"){
						productURL = "http://facebook.angry-red.com/CP_5.html";
					}else if (facebookIdx == "c6"){
						productURL = "http://facebook.angry-red.com/CP_6.html";
					}else if (facebookIdx == "c7"){
						productURL = "http://facebook.angry-red.com/CP_7.html";
					}
					facebookPaymentHolder.gameObject.SendMessage("buySomething", productURL);
				}
			}else if (isFacebookBuild.isFaceBook == false){
				//Debug.Log("not facebook version");
				BuyKarma.Instance.SendBuyKarma(idx);
			}
			
			break;
		}	
	}	
	
}
