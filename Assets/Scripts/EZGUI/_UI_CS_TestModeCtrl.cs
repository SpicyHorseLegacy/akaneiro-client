using UnityEngine;
using System.Collections;

public class _UI_CS_TestModeCtrl : MonoBehaviour {

	//Instance
	public static _UI_CS_TestModeCtrl Instance = null;
	
	public UIPanel testModePanel;
	public bool IsTestMode = false;
//	public SpriteText test;
	
	public SpriteText mapName;
	public SpriteText Info;
	public float r = 0;
	private float g = 0;
	private float b = 0;
	
	private bool IsGo = false;
	
//	public UIButton [] MapBtn;
	
	public UIButton  GoBtn;
	
	private bool isFirstStart = false;
	
	public UITextField 	m_login_NameEditText;
	public UITextField 	m_login_PassWordEditText;
	public UITextField 	m_login_IPEditText;
	
	public UIRadioBtn fixBtn;
	
	public UIRadioBtn [] LvBtn;
	
	public int GetLvState(){
	
		for(int i = 0;i<4;i++){
			
			if(true == LvBtn[i].Value){
				
				return i;
				
			}
			
		}
		
		return 1;
		
	}
	
	public bool GetFixBtnState(){
	
		return fixBtn.Value;
		
	}
	
	void Awake()
	{
		Instance = this;
	}
	
	// Use this for initialization
	void Start () {
		
		GoBtn.AddInputDelegate(GoDelegate);

		if(IsTestMode){
			
			testModePanel.BringIn();

		}
		
		
	}
	
	
	// Update is called once per frame
	void Update () {
		
		if(!isFirstStart&&IsTestMode){
			isFirstStart = true;
			_UI_CS_Login.Instance.m_CS_loginPanel.Dismiss();
		}
		
	}
	
	void GoDelegate(ref POINTER_INFO ptr)
	{
		switch(ptr.evt)
		{
		   case POINTER_INFO.INPUT_EVENT.TAP:
				IsGo = true;
				Info.Text = "--- Connect ---";
				CS_Main.Instance.g_commModule.Connect(m_login_IPEditText.text,7001);
				break;
		   default:
				break;
		}	
	}

	public int SceneNameToMissionID(string sceneName){
		
		if(0 == string.Compare(sceneName,"Hub_Village")){
			return 6000;
		}else if(0 == string.Compare(sceneName,"2012E3Demo")){
			return 6000;
		}else if(0 == string.Compare(sceneName,"template1")){
			return 6000;
		}else if(0 == string.Compare(sceneName,"A1_M1")){
			return 5110;
		}else if(0 == string.Compare(sceneName,"A1_M2")){
			return 5120;
		}else if(0 == string.Compare(sceneName,"A1_M3")){
			return 5130;
		}else if(0 == string.Compare(sceneName,"A1_M4")){
			return 5140;
		}else if(0 == string.Compare(sceneName,"A2_M1")){
			return 5210;
		}else if(0 == string.Compare(sceneName,"A2_M2")){
			return 5220;
		}else if(0 == string.Compare(sceneName,"A2_M3")){
			return 5230;
		}else if(0 == string.Compare(sceneName,"A2_M4")){
			return 5240;
		}else if(0 == string.Compare(sceneName,"A3_M1")){
			return 5310;
		}else if(0 == string.Compare(sceneName,"A3_M2")){
			return 5320;
		}else if(0 == string.Compare(sceneName,"A3_M3")){
			return 5330;
		}else if(0 == string.Compare(sceneName,"A3_M4")){
			return 5340;
		}else if(0 == string.Compare(sceneName,"A4_M1")){
			return 5410;
		}else if(0 == string.Compare(sceneName,"A4_M2")){
			return 5420;
		}else if(0 == string.Compare(sceneName,"A4_M3")){
			return 5430;
		}else if(0 == string.Compare(sceneName,"A4_M4")){
			return 5440;
		}else if(0 == string.Compare(sceneName,"A5_M1")){
			return 5510;
		}else if(0 == string.Compare(sceneName,"A5_M2")){
			return 5520;
		}else if(0 == string.Compare(sceneName,"A5_M3")){
			return 5530;
		}else if(0 == string.Compare(sceneName,"A6_M1")){
			return 5610;
		}else if(0 == string.Compare(sceneName,"A6_M2")){
			return 5620;
		}else if(0 == string.Compare(sceneName,"A6_M3")){
			return 5630;
		}else if(0 == string.Compare(sceneName,"A7_M1")){
			return 5710;
		}else if(0 == string.Compare(sceneName,"A7_M2")){
			return 5720;
		}else if(0 == string.Compare(sceneName,"A7_M3")){
			return 5730;
		}else if(0 == string.Compare(sceneName,"A8_M1")){
			return 5810;
		}else if(0 == string.Compare(sceneName,"A8_M2")){
			return 5820;
		}else if(0 == string.Compare(sceneName,"A8_M3")){
			return 5830;
		}else if(0 == string.Compare(sceneName,"Hub_Village_Tutorial")){
			return 5010;
		}else if(0 == string.Compare(sceneName,"TWILIGHT")){
			return 6000;
		}else if(0 == string.Compare(sceneName,"EmptyScenes")){
			return 6000;
		}else{
			return 6000;
		}
		return 6000;
	}
	

}
