using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class TakeGiftPanel : MonoBehaviour {
	
	public static TakeGiftPanel Instance = null;
	
	public  List<SRedeemGift>	giftItemList   = new List<SRedeemGift>();
	public UIPanel 				bgPanel;
	public UIButton 			leaveBtn;
	public UIButton 			takeAllBtn;
	public UIScrollList			list;
	public UIListItemContainer  itemContainer;
//	private string 				redeemCodeUrl = "http://192.168.6.102/api/redeem/";
	private string 				redeemCodeUrl = "http://redeem.spicyhorse.com/api/redeem/";
	
	public UIPanel 				elementItemPanel;
	public UIScrollList			elementItemlist;
	public UIListItemContainer  elementItemContainer;
	public UIButton 			exitBtn;
	public UIButton 			takeBtn;
	public string				redeemCode = "";
	
	public UIButton 			giftIcon;
	
	void Awake() {
		Instance = this;
	}
	
	// Use this for initialization
	void Start () {
		leaveBtn.AddInputDelegate(LeaveBtnDelegate);
		takeAllBtn.AddInputDelegate(TakeAllBtnDelegate);
		exitBtn.AddInputDelegate(ExitBtnDelegate);
		takeBtn.AddInputDelegate(TakeBtnDelegate);
		giftIcon.AddInputDelegate(GiftIconDelegate);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void ShowGiftIcon(bool isShow){
		if(isShow){
			giftIcon.gameObject.layer = LayerMask.NameToLayer("EZGUI");
		}else{
			giftIcon.gameObject.layer = LayerMask.NameToLayer("Default");
		}
	}
	
	public void AwakeTakeGiftPanel(){
		if(giftItemList.Count>0){
			bgPanel.BringIn();
		}
	}
	
	public void AwakeElementItemPanel(SRedeemGift info){
		elementItemlist.ClearList(true);
		foreach (int itemID in info.ItemVec){
			ItemDropStruct sitem = GetUniqueItemInfo(itemID);
			UIListItemContainer item = (UIListItemContainer)elementItemlist.CreateItem((GameObject)elementItemContainer.gameObject);
			_UI_CS_ItemVendor.Instance.SetColorForName(item.GetComponent<Container_GiftItem>().name,sitem);
		}
		foreach (int petID in info.petIDVec){
			UIListItemContainer item = (UIListItemContainer)elementItemlist.CreateItem((GameObject)elementItemContainer.gameObject);
			item.GetComponent<Container_GiftItem>().name.Text = _UI_CS_SpiritTrainer.Instance.GetSpiritHelperName(petID);
		}
		if(info.karma != 0){
			UIListItemContainer item = (UIListItemContainer)elementItemlist.CreateItem((GameObject)elementItemContainer.gameObject);
				LocalizeManage.Instance.GetDynamicText(item.GetComponent<Container_GiftItem>().name,"KS");
		}
		if(info.gold != 0){
			UIListItemContainer item = (UIListItemContainer)elementItemlist.CreateItem((GameObject)elementItemContainer.gameObject);
				LocalizeManage.Instance.GetDynamicText(item.GetComponent<Container_GiftItem>().name,"KC");
		}
		elementItemPanel.GetComponent<CalculateSlider>().Calculate();
		elementItemPanel.BringIn();
	}
	
	public void InitElementItemList(){
		list.ClearList(true);
		foreach (SRedeemGift obj in giftItemList){
			UIListItemContainer item = (UIListItemContainer)list.CreateItem((GameObject)itemContainer.gameObject);
			item.GetComponent<Container_GiftItem>().info = obj;
			item.GetComponent<Container_GiftItem>().name.Text = obj.title;
		}
		transform.GetComponent<CalculateSlider>().Calculate();
		if(giftItemList.Count<=0){
			ShowGiftIcon(false);
		}
	}
	
	public void LeaveTakeGift(){
		bgPanel.Dismiss();
	}
	
	void LeaveBtnDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:
				LeaveTakeGift();
				break;
		}	
	}
	
	void ExitBtnDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:
				elementItemPanel.Dismiss();
				break;
		}	
	}
	
	void GiftIconDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:
				AwakeTakeGiftPanel();
				break;
		}	
	}
	
	void TakeBtnDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:
				CS_Main.Instance.g_commModule.SendMessage(
		   					ProtocolGame_SendRequest.ProcessRedeemGift(redeemCode)
											 			  );
				elementItemPanel.Dismiss();
				break;
		}	
	}
	
	public void ClearList(){
		giftItemList.Clear();
	}
	
	void TakeAllBtnDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
			
		   case POINTER_INFO.INPUT_EVENT.TAP:
				foreach (SRedeemGift obj in giftItemList){
					CS_Main.Instance.g_commModule.SendMessage(
			   					ProtocolGame_SendRequest.ProcessRedeemGift(obj.redeemCode)
												 			  );
				}
				break;
		}	
	}
	
	public void InitGiftList(){
		list.ClearList(true);
		foreach (SRedeemGift obj in giftItemList){
			UIListItemContainer item = (UIListItemContainer)list.CreateItem((GameObject)itemContainer.gameObject);
			item.GetComponent<Container_GiftItem>().info = obj;
			item.GetComponent<Container_GiftItem>().name.Text = obj.title;
		}
		transform.GetComponent<CalculateSlider>().Calculate();
	}
	
	public IEnumerator SendMsgToServerForRedeemCode(string redeemCode){
		WWWForm handelServer = new WWWForm();
		handelServer.AddField("game","akaneiro");
//		LogManager.Log_Error("uid:"+ClientLogicCtrl.Instance.uid);
		handelServer.AddField("uid",ClientLogicCtrl.Instance.uid);
		handelServer.AddField("type",Platform.Instance.platformType.Get());
		handelServer.AddField("token",ClientLogicCtrl.Instance.gameCode);
		handelServer.AddField("code",redeemCode); 
		handelServer.AddField("other",_PlayerData.Instance.NameText.text); 
		WWW url = new WWW(redeemCodeUrl,handelServer);
		yield return url;
		if(url.error != null){
			Debug.LogError("Error."+url.error);
		}else{
			Debug.LogWarning("link server ok :receive alex msg");
			CheckReturnMsg(url.text);
		}
	}
	public string backUpRedeemCode = "";
	private void CheckReturnMsg(string text){
		if(string.Compare(text,"OK")==0){
			LogManager.Log_Warn("Congratulate : alex rec ok.");
			_UI_CS_PopupBoxCtrl.PopUpError("Success! Your items have been sent to your Mailbox");
		}else{
			//please check OptionCtrl.cs line 168.
//			_UI_CS_PopupBoxCtrl.PopUpError(text);
			#region new solution
			CS_Main.Instance.g_commModule.SendMessage(
			   		ProtocolGame_SendRequest.GetGiftReq(backUpRedeemCode)
			);
			#endregion
		}
	}

	public ItemDropStruct GetUniqueItemInfo(int ID){	
		string _fileName = LocalizeManage.Instance.GetLangPath("uniqueItem.rare.txt");
		TextAsset item = (TextAsset)Resources.Load(_fileName, typeof(TextAsset));
		string[] itemRowsList = item.text.Split('\n');
		for (int i = 3; i < itemRowsList.Length -1; ++i){
			string pp = itemRowsList[i];	
			string[] vals = pp.Split(new char[] { '	', '	' });	
			if(int.Parse(vals[0]) == ID){
				ItemDropStruct tempItem = ItemDeployInfo.Instance.GetItemObject(int.Parse(vals[1]),int.Parse(vals[2]),int.Parse(vals[4]),int.Parse(vals[6]),int.Parse(vals[5]),int.Parse(vals[8]));
				return tempItem;
			}
		}
		return null;
	}
}
