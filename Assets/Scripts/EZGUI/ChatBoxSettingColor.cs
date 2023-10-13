using UnityEngine;
using System.Collections;

public class ChatBoxSettingColor : MonoBehaviour {
	//Instance
	public static ChatBoxSettingColor Instance = null;
	public Transform			root;
	public Transform			maxPos;
	public Transform			minPos;
	public UIButton 			currentColorBtn;
	public UIButton []			colorBtn;
	public int colorCount	=	15;
	public UIButton 			minBtn;
	
	void Awake() {
		Instance = this;
	}
	
	// Use this for initialization
	void Start () {
		for(int i = 0;i<colorCount;i++){
			colorBtn[i].AddValueChangedDelegate(ColorBtnDelegate);
		}
		minBtn.AddInputDelegate(MinBtnDelegate);
		root.position = minPos.position;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void InitColorSetting(){
		currentColorBtn.SetColor(_UI_Color.Instance.color1);
		root.position = maxPos.position;
	}

	private void ColorBtnDelegate(IUIObject obj){
		UIButton btn  = obj as UIButton;
		if (btn != null){
			currentColorBtn.SetColor(btn.color);
			ChatBox.Instance.inputEdit.spriteText.SetColor(currentColorBtn.color);
			root.position = minPos.position;
		}
	}
	
	void MinBtnDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:
				ChatBox.Instance.isShowFontColorSetting = false;
				root.position = minPos.position;
				break;
		}	
	}
	
	public void AddColorString(){
		string color = "";
		//tofo: hex --> dec
		//todo: i dont konw english how to say ->. 
		//中间变量在不通编译时会采用临时不可确定寄存器.所以分开.
		float fr = currentColorBtn.color.r*255;
		float fg = currentColorBtn.color.g*255;
		float fb = currentColorBtn.color.b*255;
		string r = ((int)fr).ToString("X2");
		string g = ((int)fg).ToString("X2");
		string b = ((int)fb).ToString("X2");
		color = "[#" + r + g + b + "]";
		ChatBox.Instance.inputEdit.Text = ChatBox.Instance.inputEdit.text.Insert(0,color);
	}
}
