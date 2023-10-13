using UnityEngine;
using System.Collections;

public class _UI_CS_IngameToolTipEz : MonoBehaviour {
	
	public bool 	 		isHide = true;
	public UIPicture 		BG;
	public ItemDropStruct 	info;
	
	private static LocalizeManage localizeMgr_ = null;
	void Awake() {
		if (localizeMgr_ == null) {
            localizeMgr_ = LocalizeFontManager.ManagerInstance;
        }
		localizeMgr_.OnLangChanged += this.UpdateTipsLang;
	}
	
	void Start () {
		Vector3 	 V3PosT = new Vector3(9999,9999,9999);
		gameObject.GetComponent<SpriteText>().transform.position = V3PosT;
	}
	
	private Transform Own;
	private float    YRotate = 45;
	private float    XRotate = 45;
	private float 	 offestX = -0.5f;
	private float 	 offestZ = 0.5f;
	// Update is called once per frame
	void Update () {
		if(Own){ 
			if(Own.renderer != null) {
				if(!Own.renderer.isVisible){
					return;
				}	
			}
			CalcShowTime(Time.realtimeSinceStartup);
			if(!isHide){	
				Vector3 posOnScreen = Vector3.zero;
				posOnScreen = Own.position;
				posOnScreen.y += 0.5f;
				posOnScreen.x += offestX;
				posOnScreen.z += offestZ;	
				gameObject.GetComponent<SpriteText>().transform.position = posOnScreen;
				gameObject.GetComponent<SpriteText>().transform.LookAt( GameCamera.Instance.gameCamera.transform );
				gameObject.GetComponent<SpriteText>().transform.rotation = Quaternion.Euler(XRotate, 
																							YRotate,
																					gameObject.GetComponent<SpriteText>().transform.rotation.eulerAngles.z);
			}
		}else{
			_UI_CS_IngameToolTipMan.Instance.m_List.Remove(gameObject.transform);
			Destroy(gameObject);	
		}
	}
	
#region Interface
	private bool isCalc = false;
	public void InitObj(string name,Transform Pos,bool hide,float Val,ItemDropStruct iteminfo){
        gameObject.GetComponent<SpriteText>().renderer.material = LocalizeFontManager.Instance.GetCurrentMat();
        gameObject.GetComponent<SpriteText>().SetFont(LocalizeFontManager.Instance.GetCurrentFont(), LocalizeFontManager.Instance.GetCurrentMat());
		gameObject.GetComponent<SpriteText>().Text = name;
		info = iteminfo;
		//if item is materials,name color is gold.//
		if(info._TypeID == 11 || info._TypeID == 12 || info._TypeID == 13) {
			Val = 99;
		}
		_UI_Color.Instance.SetNameColor(Val,gameObject.GetComponent<SpriteText>());	
		Own = Pos;
		if(null != Pos.GetComponent<Item>()){
			Pos.GetComponent<Item>().itemTip = gameObject.GetComponent<_UI_CS_IngameToolTipEz>();
		}
		BG.width = gameObject.GetComponent<SpriteText>().TotalWidth+0.1f;
		BG.height = 0.2f;
		ShowObj();
		if(hide)
			StartCalcShowTime();	
	}
	
	public void HideObj(){
		if(!_UI_CS_IngameToolTipMan.Instance.isHide)
			return;
		gameObject.layer 	= LayerMask.NameToLayer("EZGUI");
		BG.gameObject.layer = LayerMask.NameToLayer("EZGUI");
		isHide 	 = true;
		isCalc   = false;
	}
	
	public void ShowObj(){
		gameObject.layer 	= LayerMask.NameToLayer("Default");
		BG.gameObject.layer = LayerMask.NameToLayer("Default");
		isHide 	 = false;	
	}
#endregion

#region Local
	private float     showTime  = 3;
	private float     stratTime = 0;
	private void StartCalcShowTime(){
		isCalc = true;
		stratTime = Time.realtimeSinceStartup;
	}
	
	private void CalcShowTime(float nowTime){
		if(isCalc){
			if((stratTime + showTime) < nowTime){
				isCalc = false;
				HideObj();
			}
		}
	}
	
	private void UpdateTipsLang(LocalizeManage.Language _lang) {
		gameObject.GetComponent<SpriteText>().renderer.material = LocalizeFontManager.Instance.GetCurrentMat();
		gameObject.GetComponent<SpriteText>().SetFont(LocalizeFontManager.Instance.GetCurrentFont(),LocalizeFontManager.Instance.GetCurrentMat());
		if(info._TypeID == 7 || info._TypeID == 8){			
			 gameObject.GetComponent<SpriteText>().Text = info.info_EncName + info.info_GemName + info.info_EleName + info.info_TypeName;			
		}else if(1 == info._TypeID|| 3 == info._TypeID||4 == info._TypeID||6 == info._TypeID){			
			if(info._TypeID == 4){
				gameObject.GetComponent<SpriteText>().Text = _ItemTips.Instance.GetCloakName(info);
			}else{
				gameObject.GetComponent<SpriteText>().Text = info.info_EncName + info.info_GemName + info._TypeName + info._TypelastName;
			}			
		}else if(2 == info._TypeID|| 5 == info._TypeID){			
			 gameObject.GetComponent<SpriteText>().Text = info.info_EncName + info.info_EleName + info.info_GemName + info.info_TypeName;	
		}else if(9 == info._TypeID){
			gameObject.GetComponent<SpriteText>().Text = info._PropsName;
		}
	}
#endregion
	
}
