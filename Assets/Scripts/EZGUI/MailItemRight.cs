using UnityEngine;
using System.Collections;



public class MailItemRight : MonoBehaviour {
	
	public UIButton 	icon;
	public SpriteText 	name;
	public SpriteText 	count;
	public UIButton 	take;
	public SMailInfo 	info;
	public int			slotID = 0;
	public MAILITEMType type = MAILITEMType.ITEM;
	//this obj is a pointer.i save source obj.
	public System.Object obj;
	
	// Use this for initialization
	void Start () {
		 take.AddInputDelegate(TakeDelegate);	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void TakeDelegate(ref POINTER_INFO ptr) {
		switch(ptr.evt) {
		   case POINTER_INFO.INPUT_EVENT.TAP:
			switch(type) {
			case MAILITEMType.ITEM:
				CS_Main.Instance.g_commModule.SendMessage(ProtocolGame_SendRequest.GetMailItem(info.id,slotID));
				break;
			case MAILITEMType.PET:
				CS_Main.Instance.g_commModule.SendMessage(ProtocolGame_SendRequest.GetMailPet(info.id,slotID));
				break;
			case MAILITEMType.KARMA:
				CS_Main.Instance.g_commModule.SendMessage(ProtocolGame_SendRequest.GetMailKarma(info.id));
				break;
			case MAILITEMType.CRYSTAL:
				CS_Main.Instance.g_commModule.SendMessage(ProtocolGame_SendRequest.GetMailCrystal(info.id));
				break;
			}
			break;
		}	
	}
	
	public void SetItemIcon(Texture2D img) {
		icon.SetUVs(new Rect(0,0,1,1));
		icon.SetTexture(img);
	}
}
