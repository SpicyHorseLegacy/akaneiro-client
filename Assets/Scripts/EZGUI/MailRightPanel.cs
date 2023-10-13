using UnityEngine;
using System.Collections;

public class MailRightPanel : MonoBehaviour {
	
	//Instance
	public static MailRightPanel Instance = null;
	public  UIListItemContainer  	itemContainer;
	public  UIScrollList			list;
	public  SMailInfo				curMailInfo;
	
	public SpriteText 				title;
	public SpriteText 				description;
	public UIButton 				takeAllBtn;
	public UIButton 				deleteBtn;
	
	public UIPanel 					basePanel;
	
	void Awake() {
		Instance = this;
	}
	
	// Use this for initialization
	void Start () {
		 takeAllBtn.AddInputDelegate(TakeAllBtnDelegate);	
		 deleteBtn.AddInputDelegate(DeleteBtnDelegate);	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void TakeAllBtnDelegate(ref POINTER_INFO ptr) {
		switch(ptr.evt) {
		   case POINTER_INFO.INPUT_EVENT.TAP:
				CS_Main.Instance.g_commModule.SendMessage(ProtocolGame_SendRequest.GetMailAll(curMailInfo.id));
				break;
		}	
	}
	
	void DeleteBtnDelegate(ref POINTER_INFO ptr) {
		switch(ptr.evt) {
		   case POINTER_INFO.INPUT_EVENT.TAP:
				CS_Main.Instance.g_commModule.SendMessage(ProtocolGame_SendRequest.DeleteMail(curMailInfo.id));
				break;
		}	
	}
	
	public void Init(){
		int slotIDItem = 0;
		int slotIDPet = 0;
		list.ClearList(true);
		title.Text = curMailInfo.title;
		description.Text = curMailInfo.content;
		if(curMailInfo.karma != 0) {
			UIListItemContainer item = (UIListItemContainer)list.CreateItem((GameObject)itemContainer.gameObject);
			list.clipContents = true; list.clipWhenMoving = true;
			item.GetComponent<MailItemRight>().name.Text = "Karma";
			item.GetComponent<MailItemRight>().count.Text = curMailInfo.karma.ToString();
			item.GetComponent<MailItemRight>().SetItemIcon(DailyRewardGift.Instance.karmaPic);
			item.GetComponent<MailItemRight>().slotID = 0;
			item.GetComponent<MailItemRight>().type = MAILITEMType.KARMA;
			item.GetComponent<MailItemRight>().info = curMailInfo;
			item.GetComponent<MailItemRight>().obj = null;
		}
		if(curMailInfo.crystal != 0) {
			UIListItemContainer item = (UIListItemContainer)list.CreateItem((GameObject)itemContainer.gameObject);
			list.clipContents = true; list.clipWhenMoving = true;
			item.GetComponent<MailItemRight>().name.Text = "Crystal";
			item.GetComponent<MailItemRight>().count.Text = curMailInfo.crystal.ToString();
			item.GetComponent<MailItemRight>().SetItemIcon(DailyRewardGift.Instance.realMoneyPic);
			item.GetComponent<MailItemRight>().slotID = 0;
			item.GetComponent<MailItemRight>().type = MAILITEMType.CRYSTAL;
			item.GetComponent<MailItemRight>().info = curMailInfo;
			item.GetComponent<MailItemRight>().obj = null;
		}
		foreach (SItemInfo sitem in curMailInfo.itemVec){
			UIListItemContainer item = (UIListItemContainer)list.CreateItem((GameObject)itemContainer.gameObject);
			list.clipContents = true; list.clipWhenMoving = true;
			ItemDropStruct itemInfo =  ItemDeployInfo.Instance.GetItemObject(sitem.ID,sitem.perfrab,sitem.gem,sitem.enchant,sitem.element,(int)sitem.level);
			_UI_CS_ItemVendor.Instance.SetColorForName(item.GetComponent<MailItemRight>().name,itemInfo);
			ItemPrefabs.Instance.GetItemIcon(itemInfo._ItemID,itemInfo._TypeID,itemInfo._PrefabID,item.GetComponent<MailItemRight>().icon);
			item.GetComponent<MailItemRight>().count.Text = sitem.count.ToString();
			item.GetComponent<MailItemRight>().info = curMailInfo;
			item.GetComponent<MailItemRight>().slotID = slotIDItem;
			item.GetComponent<MailItemRight>().type = MAILITEMType.ITEM;
			item.GetComponent<MailItemRight>().info = curMailInfo;
			item.GetComponent<MailItemRight>().obj =  sitem;
			slotIDItem++;
		}
		foreach (int pet in curMailInfo.petIDVec){
			UIListItemContainer item = (UIListItemContainer)list.CreateItem((GameObject)itemContainer.gameObject);
			list.clipContents = true; list.clipWhenMoving = true;
			item.GetComponent<MailItemRight>().name.Text = _UI_CS_SpiritTrainer.Instance.GetSpiritHelperName(pet);
			item.GetComponent<MailItemRight>().count.Text = "1";
			item.GetComponent<MailItemRight>().SetItemIcon(_UI_CS_SpiritInfo.Instance.spirirtSmallIcon[_UI_CS_SpiritTrainer.Instance.GetSpiritHelperIconID(pet)]);
			item.GetComponent<MailItemRight>().info = curMailInfo;
			item.GetComponent<MailItemRight>().type = MAILITEMType.PET;
			item.GetComponent<MailItemRight>().slotID = slotIDPet;
			item.GetComponent<MailItemRight>().info = curMailInfo;
			item.GetComponent<MailItemRight>().obj = pet;
			slotIDPet++;
		}
		transform.GetComponent<CalculateSlider>().Calculate();	
	}
	
	public void DelItem(int slotID,MAILITEMType type) {
		DelDetailItem(slotID,type);
	}
	
	public void DelDetailItem(int slotID,MAILITEMType type) {
		int tempID = 0;
		for(int i = 0;i<list.Count;i++) {
			tempID = list.GetItem(i).gameObject.GetComponent<MailItemRight>().slotID;
			if(list.GetItem(i).gameObject.GetComponent<MailItemRight>().type == type) {
				if(tempID == slotID) {
					MailLeftPanel.Instance.DelMailEleInfo(type,list.GetItem(i).gameObject.GetComponent<MailItemRight>());
					list.RemoveItem(i,true);
					ReSetItemSlot(type);
					break;
				}
			}
		}
	}
	
	public void ReSetItemSlot(MAILITEMType type) {
		int tempID = 0;
		for(int i = 0;i<list.Count;i++) {
			if(list.GetItem(i).gameObject.GetComponent<MailItemRight>().type == type) {
				list.GetItem(i).gameObject.GetComponent<MailItemRight>().slotID = tempID;
				tempID++;
			}
		}
	}
	
	public void DelAllItem() {
		list.ClearList(true);
	}
	
}
