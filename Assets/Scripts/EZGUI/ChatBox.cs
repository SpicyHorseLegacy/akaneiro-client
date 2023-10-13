using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

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

public class ChatBox : MonoBehaviour {
	
	//Instance
	public static ChatBox Instance = null;
	public Transform			root;
	public UIButton				sendBtn;
	public UIButton				fontColorBtn;
	public bool					isShowFontColorSetting = false;
	public UIButton				filterBtn;
	public UIButton				clearBtn;
	public UIButton				contentTypeBtn;
	public SpriteText			contentTypeText;
	public UIButton				maxBtn;
	public UIButton				minBtn;
	public UIButton				info1Bg;
	public bool					isMaxChatBox = true;
	public Transform			maxPos;
	public Transform			minPos;
	public UITextField			inputEdit;
	public SpriteText			information1;
	public SpriteText			information2;
	public UIListItemContainer	contentItemContainer;
	public UIScrollList			information3List;
	public Color				systemTextColor;
	public Color				privateTextColor;
	public  Queue<MsgStruct> 	contentQueue	 = new Queue<MsgStruct>(); 
	public 	int 				maxContentCount  = 23;
	public CalculateSlider 		calculateSlider;
	public bool					isInputState = false;
	public  List<SaveMsgStruct> saveMsgList	 = new List<SaveMsgStruct>(); 
	public 	int 				maxSvaeCount  = 10;
	public 	int 				currentSveIdx  = 0;
	public	int					sendType = 4;
	//todo: because return key ,no send at web ver;
	public bool 				isSend = true;
	
	public float 	 info1MaxTime = 10;
	private float 	 info1CurTime = 0;
	string info1 = "Chat feature is under construction.";
	string info2 = "Feedback: [#ffff00]Akaneiro@spicyhorse.com";
	int	   changeTextIdx = 0;
	
	void Awake() {
		Instance = this;
	}
	
	// Use this for initialization
	void Start () {
//		information1.Text = info2;
		ResetChatContent();
		calculateSlider.Calculate();
		minBtn.gameObject.layer = LayerMask.NameToLayer("Default");
		root.position = minPos.position;
		inputEdit.SetValidationDelegate(InputEditDelegate);
		inputEdit.SetCommitDelegate(MyCommitDelegate);
		inputEdit.AddInputDelegate(InputStateDelegate);
		sendBtn.AddInputDelegate(SendBtnDelegate);
		// todo: temp,  old A.
		maxBtn.AddInputDelegate(MaxBtnDelegate);
		minBtn.AddInputDelegate(MinBtnDelegate);
		info1Bg.AddInputDelegate(InfoBgDelegate);
		
		clearBtn.AddInputDelegate(ClearBtnDelegate);
		fontColorBtn.AddInputDelegate(fontColorBtnDelegate);
		filterBtn.AddInputDelegate(filterBtnDelegate);
		contentTypeBtn.AddInputDelegate(contentTypeBtnDelegate);
		isInputState = false;
		isMaxChatBox = false;
		isSend = true;
		ShowChatText();
	}
	
	// Update is called once per frame
	void Update () {
//		if(_UI_CS_FightScreen.Instance.isCheckPing){
//			info1CurTime += Time.deltaTime;
//			if(info1CurTime > info1MaxTime){
//				info1CurTime = 0;
//				ChatBox.Instance.ShowChatText();
//			}
//		}
//		LogManager.Log_Info("Focus: "+_UI_CS_Ctrl.Instance.m_UI_Manager.FocusObject.gameObject.name);
	}

	void UpdateInformation1(EChatType type,string name,string info){
//			information1.SetColor(UpdateContentDefaultColor(type));
			information1.Text = "[" + name + "] " + info;
	}
	
	void UpdateInformation2(EChatType type,string name,string info){
		if(EChatType.eChatType_System == type.Get()){
			information2.Text = "[System] " + info;
		}
	}
	
	public void ShowChatText(){
		return;
		if(changeTextIdx == 0){
			changeTextIdx = 1;
		}else{
			changeTextIdx = 0;
		}
		
		switch(changeTextIdx){
		case 0:
			information1.Text = info1;
			break;
		case 1:
			information1.Text = info2;
			break;
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
		msg.type = type; msg.msg = info; msg.name = name; msg.id = id;
		SaveToQueue(msg);
		//update main information.
		UpdateMainInformation();
	}
	
	void SaveToQueue(MsgStruct msg){
		contentQueue.Enqueue(msg);
		if(contentQueue.Count > maxContentCount){
			contentQueue.Dequeue();
		}
	}
	
	void UpdateMainInformation(){
		information3List.ClearList(true);
		UIListItemContainer item;
		//todo: show top -> bot;
		MsgStruct [] chatArray = contentQueue.ToArray();
		int length = chatArray.Length-1;
		for(int i = 0;i <chatArray.Length;i++){
			item = (UIListItemContainer)information3List.CreateItem((GameObject)contentItemContainer.gameObject,0,true);
			item.GetComponent<ChatElement>().content.Text = "[" + chatArray[length - i].name + "] " + chatArray[length - i].msg;
		}
//		foreach (MsgStruct msg in contentQueue){  
//           item = (UIListItemContainer)information3List.CreateItem((GameObject)contentItemContainer.gameObject,0,true);
//		   item.GetComponent<ChatElement>().content.Text = "[" + msg.name + "] " + msg.msg;
//        }
		calculateSlider.Calculate();
	}
	
	Color UpdateContentDefaultColor(EChatType type){
		if(EChatType.eChatType_System == type.Get()){
			return systemTextColor;
		}else if(EChatType.eChatType_Private == type.Get()){
			return privateTextColor;
		}
		return _UI_Color.Instance.color1;
	}
	
	public void ResetChatContent(){
		information3List.ClearList(true);
		contentQueue.Clear();
		calculateSlider.Calculate();
	}
	
	void SendBtnDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:
				SendMsg();
				break;
		}	
	}
	
	public void SendMsg(){
		if(inputEdit.Text.Length > 0){
			
			if(IsBannedToPost()) {
				EChatType tempType = new EChatType();
				tempType.Set(EChatType.eChatType_System);
				ReceiveNewMsg(tempType,"Banned to post!","System",100000);
				return;
			}
			
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
			
			SaveMsgStruct saveMag = new SaveMsgStruct();
			saveMag.msg = inputEdit.Text;
			saveMag.time = Time.time;
			//save chat list.
			saveMsgList.Add(saveMag);
			if(saveMsgList.Count > maxSvaeCount){
				saveMsgList.RemoveAt(0);
			}
			//system and gm do not add color code.
			if(sendType != EChatType.eChatType_GM && sendType != EChatType.eChatType_System){
				ChatBoxSettingColor.Instance.AddColorString();
			}
			//send msg.
			EChatType type = new EChatType(sendType);
			CS_Main.Instance.g_commModule.SendMessage(
				   		ProtocolGame_SendRequest.ChatRequest(type,0,0,inputEdit.Text)
			);
			restInputState();
		}
	}
	
	public float BannedToPostTime = 60f;
	private bool IsBannedToPost() {
		if(saveMsgList.Count >= 5) {
			if(Time.time - (saveMsgList[saveMsgList.Count-5].time) > BannedToPostTime) {
				return false;
			}else {
				return true;
			}
		}else {
			return false;
		}
	}
	
	private void restInputState() {
		//reset input content
		inputEdit.Text = "";
		isInputState = false;
		isSend = true;
	}
	
	void ClearBtnDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:
				ResetChatContent();
				information1.Text = "";
				information2.Text = "";
				break;
		}	
	}
		
	void fontColorBtnDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:
				if(isShowFontColorSetting){
					isShowFontColorSetting = false;
					ChatBoxSettingColor.Instance.root.position = ChatBoxSettingColor.Instance.minPos.position;
				}else{
					isShowFontColorSetting = true;
					ChatBoxSettingColor.Instance.InitColorSetting();
				}
				
				break;
		}	
	}
	
	void filterBtnDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:
				ChatBoxSettingFilter.Instance.InitFilterSetting();
				break;
		}	
	}
		
	void contentTypeBtnDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:
				ChatBoxSettingSendType.Instance.InitSendTypeSetting();
				break;
		}	
	}
	
	void MaxBtnDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:
				isMaxChatBox = true;
				maxBtn.gameObject.layer = LayerMask.NameToLayer("Default");
				minBtn.gameObject.layer = LayerMask.NameToLayer("EZGUI");
				root.position = maxPos.position;
				inputEdit.Text = "";
				isInputState = true;
				break;
		}	
	}
	
	void MinBtnDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:
				isMaxChatBox = false;
				maxBtn.gameObject.layer = LayerMask.NameToLayer("EZGUI");
				minBtn.gameObject.layer = LayerMask.NameToLayer("Default");
				root.position = minPos.position;
				isInputState = false;
				inputEdit.Text = "";
				break;
		}	
	}
	
	void InfoBgDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:
				if(isMaxChatBox){
					isMaxChatBox = false;
					inputEdit.Text = "";
					maxBtn.gameObject.layer = LayerMask.NameToLayer("EZGUI");
					minBtn.gameObject.layer = LayerMask.NameToLayer("Default");
					root.position = minPos.position;
					isInputState = false;
				}else{
					isMaxChatBox = true;
					maxBtn.gameObject.layer = LayerMask.NameToLayer("Default");
					minBtn.gameObject.layer = LayerMask.NameToLayer("EZGUI");
					root.position = maxPos.position;
					inputEdit.Text = "";
					isInputState = true;
				}
				break;
		}	
	}
	
	public void PopUpCharBox() {
		isMaxChatBox = true;
		maxBtn.gameObject.layer = LayerMask.NameToLayer("Default");
		minBtn.gameObject.layer = LayerMask.NameToLayer("EZGUI");
		root.position = maxPos.position;
		_UI_CS_Ctrl.Instance.m_UI_Manager.FocusObject = inputEdit;
		isInputState  = true;
		isSend = false;
		currentSveIdx = saveMsgList.Count;
	}
	
	void InputStateDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:
				isInputState  = true;
				isSend = false;
				currentSveIdx = saveMsgList.Count;
				break;
		}	
	}
	
	public string InputEditDelegate(UITextField field,string text){
		return text;
	}
	
	public void MyCommitDelegate(IKeyFocusable control){
		isInputState = false;
	}

}
