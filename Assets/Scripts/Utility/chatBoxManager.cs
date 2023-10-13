using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class chatBoxManager : MonoBehaviour {
	
	public struct MsgStruct{
		public string 		msg;
		public string 		name;
		public int 			id;
		public EChatType 	type;
	}
	
	public struct SaveMsgStruct{
		public string 		msg;
		public float		time;
	}
	
	public static chatBoxManager Instance = null;
	[SerializeField] UIInput InputController;
	public UILabel			inputEdit;
	public UILabel			information1;
	public UILabel			information2;
	public UITextList 		information3List;
	public	int				sendType = 4;
	public bool				isInputState = false;
	public bool 			isSend = true;
	public 	int 			maxSvaeCount  = 10;
	public 	int 			maxContentCount  = 23;
	public Color			systemTextColor;
	public Color			privateTextColor;
	public  List<SaveMsgStruct> saveMsgList	 = new List<SaveMsgStruct>();
	public  Queue<MsgStruct> 	contentQueue	 = new Queue<MsgStruct>(); 
	
	
	void Awake() {
		Instance = this;
	}
	
	// Use this for initialization
	void Start () {
		isInputState = false;
		isSend = true;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void UpdateInformation1(EChatType type,string name,string info){
//			information1.SetColor(UpdateContentDefaultColor(type));
			//information1.text = "[" + name + "] " + info;
		Debug.Log ("1");
	}
	
	void UpdateInformation2(EChatType type,string name,string info){
		if(EChatType.eChatType_System == type.Get()){
			//information2.text = "[System] " + info;
			Debug.Log ("2");
		}
	}
	
	public void ReceiveNewMsg(EChatType type,string info,string name,int id){
		//todo: Old A.
//		return;

		switch(type.Get()){
			case EChatType.eChatType_Game:
				if(4 == ChatBoxSettingFilter.Instance.GetPublicState()){
					return;
				}
			break;
//			case EChatType.eChatType_Private:
//				if(1 == ChatBoxSettingFilter.Instance.GetPrivateState()){
//					return;
//				}
//			break;
//			case EChatType.eChatType_Broadcast:
//				if(3 == ChatBoxSettingFilter.Instance.GetTeamState()){
//					return;
//				}
//			break;
		}
		//update new info.
		UpdateInformation1(type,name,info);
		//update system info.
		UpdateInformation2(type,name,info);
		//save to queue.
		MsgStruct msg = new MsgStruct();
		msg.type = type;
		msg.msg = info;
		msg.name = name;
		msg.id = id;
		SaveToQueue(msg);
		//update main information.
		UpdateMainInformation();
		Debug.Log ("**//*/**/*//*/**/*/*/*/*/*//**/ end segnal");
	}
	
	
	void SaveToQueue(MsgStruct msg){
		contentQueue.Enqueue(msg);
		if(contentQueue.Count > maxContentCount){
			contentQueue.Dequeue();
		}
	}
	
	public void SendMsg(){
		if(inputEdit.text.Length > 0){
			
			if(IsBannedToPost()) {
				EChatType tempType = new EChatType();
				tempType.Set(EChatType.eChatType_System); //eChatType_Game  eChatType_System
				ReceiveNewMsg(tempType,"Banned to post!","System",100000);
				return;
			}
			/*
			//localize RU/EN temp
			if(string.Compare(inputEdit.Text,"RU") == 0) {
				LocalizeManage.Instance.SetLangInMenu(LocalizeManage.Language.RU);
				restInputState();
				return;
			} else if(string.Compare(inputEdit.Text,"EN") == 0) {
				LocalizeManage.Instance.SetLangInMenu(LocalizeManage.Language.EN);
				restInputState();
				return;
			}
			*/
			
			SaveMsgStruct saveMag = new SaveMsgStruct();
			saveMag.msg = inputEdit.text;
			saveMag.time = Time.time;
			//save chat list.
			saveMsgList.Add(saveMag);
			if(saveMsgList.Count > maxSvaeCount){
				saveMsgList.RemoveAt(0);
			}
			//system and gm do not add color code.
			if(sendType != EChatType.eChatType_GM && sendType != EChatType.eChatType_System){
				//ChatBoxSettingColor.Instance.AddColorString();
			}
			//send msg.
			EChatType type = new EChatType(sendType);
			CS_Main.Instance.g_commModule.SendMessage(
				ProtocolGame_SendRequest.ChatRequest(type,0,0,InputController.text)
			);
			restInputState();
		}
	}
	
	public float BannedToPostTime = 60f;
	private bool IsBannedToPost() {
		if(saveMsgList.Count >= 30) {
			if(Time.time - (saveMsgList[saveMsgList.Count-5].time) > BannedToPostTime) {
				return false;
			}else {
				return true;
			}
		}else {
			return false;
		}
	}
	
	Color UpdateContentDefaultColor(EChatType type){
		if(EChatType.eChatType_System == type.Get()){
			return systemTextColor;
		}else if(EChatType.eChatType_Private == type.Get()){
			return privateTextColor;
		}
		return _UI_Color.Instance.color1;
	}
	
	private void restInputState() {
		//reset input content
		inputEdit.text = "";
		isInputState = false;
		isSend = true;
	}
	
	void UpdateMainInformation(){
		//information3List.ClearList(true);
		//UIListItemContainer item;
		
		information3List.Clear();
			
		MsgStruct [] chatArray = contentQueue.ToArray();
		int length = chatArray.Length-1;
		for(int i = 0;i <chatArray.Length;i++){
			//item = (UIListItemContainer)information3List.CreateItem((GameObject)contentItemContainer.gameObject,0,true);
			//item.GetComponent<ChatElement>().content.Text = "[" + chatArray[length - i].name + "] " + chatArray[length - i].msg;
			information3List.Add ("[" + chatArray[length - i].name + "]: " + chatArray[length - i].msg.TrimEnd('|'));
			Debug.Log ("//////////////////////////////some stuff added to the list : [" + chatArray[length - i].msg + "]");
		}
		Debug.Log(">> text was : " + information3List.textLabel.text);
		//calculateSlider.Calculate();
	}

	[SerializeField] UILabel ChatTypeBar;
	void changeChatType()
	{
		
		if(sendType == 4)
			sendType = EChatType.eChatType_GM;
		else if(sendType == EChatType.eChatType_GM)
			sendType = 4;

		ChatTypeBar.text = new EChatType(sendType).GetString();
	}
}
