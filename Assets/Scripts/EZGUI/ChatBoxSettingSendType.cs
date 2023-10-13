using UnityEngine;
using System.Collections;

public class ChatBoxSettingSendType : MonoBehaviour {
	//Instance
	public static ChatBoxSettingSendType Instance = null;
	public Transform			root;
	public Transform			maxPos;
	public Transform			minPos;
	public UIButton 		[]	typeBtn;
	public UIButton 			BG;
	public int					typeCount;
	public Transform			cheatPos;
	public Transform			sysPos;
	
	void Awake() {
		Instance = this;
	}
	
	// Use this for initialization
	void Start () {
		for(int i = 0;i<typeCount;i++){
			typeBtn[i].AddValueChangedDelegate(TypeBtnDelegate);
		}
		root.position = minPos.position;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void InitSendTypeSetting(){
		root.position = maxPos.position;
	}
	
	private void TypeBtnDelegate(IUIObject obj){
		UIButton btn  = obj as UIButton;
		if (btn != null){
			ChatBox.Instance.sendType = obj.gameObject.GetComponent<WidgetType>().type;
			ChatBox.Instance.contentTypeText.Text = obj.gameObject.GetComponent<WidgetType>().text;
			root.position = minPos.position;
		}
	}
	
	public void InitSendType(){
		if(_PlayerData.Instance.myAccountInfo.type != 0){
			BG.transform.localScale = new Vector3(1f,1f,1f);
			typeBtn[0].transform.position = sysPos.position;
			typeBtn[1].transform.position = cheatPos.position;
		}else{
			BG.transform.localScale = new Vector3(1f,0.45f,1f);
			typeBtn[0].transform.position = new Vector3(typeBtn[0].transform.position.x,typeBtn[0].transform.position.y,-999f);
			typeBtn[1].transform.position = new Vector3(typeBtn[1].transform.position.x,typeBtn[1].transform.position.y,-999f);
		}
	}
	
}
