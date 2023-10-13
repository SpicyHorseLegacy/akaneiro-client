using UnityEngine;
using System.Collections;
using System;

public class MailLeftPanel : MonoBehaviour {
	
	//Instance
	public static MailLeftPanel Instance = null;
	public  UIListItemContainer  	itemContainer;
	public  UIScrollList			list;
	
	void Awake() {
		Instance = this;
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void InitList(){
		list.ClearList(true);
		foreach (SMailInfo mail in MailSystem.Instance.mailList){
			UIListItemContainer item = (UIListItemContainer)list.CreateItem((GameObject)itemContainer.gameObject);
			list.clipContents = true; list.clipWhenMoving = true;
			item.GetComponent<MailItem>().name.Text = mail.title;
			item.GetComponent<MailItem>().fromPlayer.Text = mail.senderName;
			long t1970 = Get1970Time();
			if(mail.timeout == 0) {
				item.GetComponent<MailItem>().time.Text = "Forever";
			}else {
				long t1970s = mail.timeout - t1970;
				item.GetComponent<MailItem>().time.Text = (t1970s/86400).ToString() + "d "+(t1970s/3600%24).ToString() + "h " + (t1970s/60%60).ToString() + "m";
			}
			item.GetComponent<MailItem>().icon.SetUVs(new Rect(0,0,1,1));
			if(mail.type.Get() == EMailType.eMailType_System) {
				item.GetComponent<MailItem>().icon.SetTexture(MailSystem.Instance.systemIcon);
			}else {
				item.GetComponent<MailItem>().icon.SetTexture(MailSystem.Instance.UserIcon);
			}
			if(mail.state.Get() == EMailState.eMailState_Unread) {
				item.GetComponent<MailItem>().IsHideNewIcon(false);
			}else {
				item.GetComponent<MailItem>().IsHideNewIcon(true);
			}
			item.GetComponent<MailItem>().info = mail;
		}
		transform.GetComponent<CalculateSlider>().Calculate();	
	}
	
	public long Get1970Time() {
		TimeSpan ts = DateTime.UtcNow - new DateTime(1970,1,1,0,0,0,0);
		return Convert.ToInt64(ts.TotalSeconds + _PlayerData.Instance.offest1970Time);
	}

	public void DelMail(int mailID) {
		MailRightPanel.Instance.basePanel.Dismiss();	
		for(int i = 0;i<list.Count;i++) {
			if(list.GetItem(i).gameObject.GetComponent<MailItem>().info.id == mailID) {
				list.RemoveItem(i,true);
				return;
			}
		}
	}
	
	public void GetAllItem(int mailID) {
		for(int i = 0;i<list.Count;i++) {
			if(list.GetItem(i).gameObject.GetComponent<MailItem>().info.id == mailID) {
				list.GetItem(i).gameObject.GetComponent<MailItem>().info.karma = 0;
				list.GetItem(i).gameObject.GetComponent<MailItem>().info.crystal = 0;
				list.GetItem(i).gameObject.GetComponent<MailItem>().info.itemVec.Clear();
				list.GetItem(i).gameObject.GetComponent<MailItem>().info.petIDVec.Clear();
				return;
			}
		}
	}
	
	public void DelMailEleInfo(MAILITEMType type,MailItemRight objInfo) {
		for(int j = 0;j<list.Count;j++) {
			if(list.GetItem(j).gameObject.GetComponent<MailItem>().info.id == MailRightPanel.Instance.curMailInfo.id) {
				switch(type) {
				case MAILITEMType.ITEM:
					list.GetItem(j).gameObject.GetComponent<MailItem>().info.itemVec.Remove(objInfo.obj);
					break;
				case MAILITEMType.PET:
					list.GetItem(j).gameObject.GetComponent<MailItem>().info.petIDVec.Remove(objInfo.obj);
					break;
				case MAILITEMType.KARMA:
					list.GetItem(j).gameObject.GetComponent<MailItem>().info.karma = 0;
					break;
				case MAILITEMType.CRYSTAL:
					list.GetItem(j).gameObject.GetComponent<MailItem>().info.crystal = 0;
					break;
				}
			}
		}
	}
	
	public void DelMail(vectorInt mailIDVec) {
		MailRightPanel.Instance.basePanel.Dismiss();
		for(int i = 0;i<list.Count;i++) {
			int id = list.GetItem(i).gameObject.GetComponent<MailItem>().info.id;
			if(CheckMailIsExist(id,mailIDVec)) {
				list.RemoveItem(i,true);
			}
		}
		for(int i = 0;i<MailSystem.Instance.mailList.Count;i++) {
			int id2 = MailSystem.Instance.mailList[i].id;
			if(CheckMailIsExist(id2,mailIDVec)) {
				MailSystem.Instance.mailList.Remove(MailSystem.Instance.mailList[i]);
			}
		}
		transform.GetComponent<CalculateSlider>().Calculate();	
	}
	
	public bool CheckMailIsExist(int id,vectorInt mailIDVec) {
		foreach(int mid in mailIDVec) {
			if(id == mid) {
				return true;
			}
		}
		return false;
	}
}
