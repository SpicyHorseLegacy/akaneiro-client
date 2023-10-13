using UnityEngine;
using System.Collections;

public class Container_GiftItem : MonoBehaviour {
	
	public UIButton 	bg;
	public SpriteText	name;
	public SRedeemGift	info;
	public bool			isUseDelegate = false;
	
	// Use this for initialization
	void Start () {
		bg.AddInputDelegate(BgBtnDelegate);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void BgBtnDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:
				if(isUseDelegate){
					TakeGiftPanel.Instance.redeemCode = info.redeemCode;
					TakeGiftPanel.Instance.AwakeElementItemPanel(info);
				}
				break;
		}	
	}
}
