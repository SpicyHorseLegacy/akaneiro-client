using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class GUIManager : MonoBehaviour {
	
	public static GUIManager	Instance;
	private string currentScreenType = "";
	public Transform currentScreenObj;
	public Transform anchorRoot;
	//this is file.
	public List<STemplateInfo> currScreenTemplateInfoList = new List<STemplateInfo>();
	//this is transform.
	public List<UITemplate> currScreenTemplateList = new List<UITemplate>();
	
	public Camera camera;
	
	void Awake() {
		Instance = this;
	}
	
	public static bool IsInUIState(string _tempUIState)
	{
		return _tempUIState == Instance.currentScreenType;
	}

#region Interface
	public void ChangeUIScreenState(string nextUIState){
		Debug.Log("[UI] Start to change screen : " + nextUIState);
		if(string.Compare(nextUIState,currentScreenType) == 0) {
			GUILogManager.LogInfo("next screen and current screen same. screen name: "+nextUIState);
			return;
		}
		ChangeScreenName(nextUIState);
		currentScreenType = nextUIState;
		RestCurrTemplateObj();	
		ResetCurrScreenObj();
		ResetTempInitEndCount();
		FadeInLoading();
		InitScreenObj(nextUIState);
		UIConfig.Instance.InitScreenFile(nextUIState);
		UIConfig.Instance.GetCurrentScreenTemplateList(nextUIState);
		FadeOutLoading();
		LoadCurrTemplateObj();
	}
	
	#region Dynamic Template
	public STemplateInfo GetTemplateData(string templateName) {
		foreach(STemplateInfo data in currScreenTemplateInfoList) {
			if(string.Compare(data.templateName,templateName) == 0) {
				return data;
			}
		}
		return null;
	}
	public void AddTemplate(string templateName) {
		STemplateInfo tData = GetTemplateData(templateName);
		if(tData == null) {
			GUILogManager.LogErr("templateName :" + templateName + " no data.cant load.");
			return;
		}
		if(IsExistTemplateInList(templateName)) {
			GUILogManager.LogErr("" + templateName + " reload.cant load.");
		}else {
			Transform currTemplate = null;
			UILoadManager.Instance.LoadingTemplate(tData,currTemplate);
		}
	}
	//when you want load new tempale.use it. it can protect reload. but if you want load twice,maybe have problem.//
	public bool IsExistTemplateInList(string templateName) {
		foreach(UITemplate template in GUIManager.Instance.currScreenTemplateList) {
			if(string.Compare(template.templateName,templateName) == 0) {
				return true;
			}
		}
		return false;
	}
	
	public UITemplate GetTemplateTarget(string templateName) {
		foreach(UITemplate temp in currScreenTemplateList) {
			if(string.Compare(temp.templateName,templateName) == 0) {
				return temp;
			}
		}
		return null;
	}
	public void RemoveTemplate(string templateName) {
		UITemplate temp = GetTemplateTarget(templateName);
		if(temp == null) {
			GUILogManager.LogErr("curTempList no exist target template: "+templateName);
			return;
		}
		currScreenTemplateList.Remove(temp);
        SingleTemplateDestroy(templateName);
		temp.DestroyTemplate();
		Destroy(temp.gameObject);
	}
	#endregion
	
	#region template init end
	//when template awake tempInitEndCount will ++. screen manager can know ,when init().//
	public delegate void Handle_InitEndDelegate();
    public event Handle_InitEndDelegate OnInitEndDelegate;
	private int tempInitEndCount = 0;
	public void AddTemplateInitEnd() {
		tempInitEndCount++;
		if(OnInitEndDelegate != null) {
			OnInitEndDelegate();
		}
	}
	private void ResetTempInitEndCount() {
		tempInitEndCount = 0;
	}
	public int GetTemplateInitEndCount() {
		return tempInitEndCount;
	}
	#endregion
	
	public delegate void Handle_TmplateInitDelegate(string name);
    public event Handle_TmplateInitDelegate OnTemplateInit;
	public void TemplateInit(string name) {
		if(OnTemplateInit != null) {
			OnTemplateInit(name);
		}
	}

    public delegate void Handle_TemplateSignleDestroyDelegate(string name);
    public event Handle_TemplateSignleDestroyDelegate OnSingleTemplateDestroy;
    public void SingleTemplateDestroy(string name){
        if (OnSingleTemplateDestroy != null){
            OnSingleTemplateDestroy(name);
        }
    }
#endregion
	
#region Local
	public delegate void Handle_TemplateDestroyDelegate();
    public event Handle_TemplateDestroyDelegate OnTemplateDestroy;
	private void RestCurrTemplateObj() {
		if(OnTemplateDestroy != null) {
			OnTemplateDestroy();
		}
		for(int i = 0;i<currScreenTemplateList.Count;i++) {
            if (currScreenTemplateList[i] != null)
			    Destroy(currScreenTemplateList[i].gameObject);
		}
		currScreenTemplateList.Clear();
	}
	
	public delegate void Handle_ScreenManagerDestroyDelegate();
    public event Handle_ScreenManagerDestroyDelegate OnScreenManagerDestroy;
	private void ResetCurrScreenObj() {
		if(OnScreenManagerDestroy != null) {
			OnScreenManagerDestroy();
		}
		if(currentScreenObj != null) {
			Destroy(currentScreenObj.gameObject);
		}
	}
	
	private void InitScreenObj(string screenName) {
		if(LogicManager.Instance == null) {
			GUILogManager.LogErr("LogicManager.Instance is null");
			return;
		}
		for(int i = 0;i<LogicManager.Instance.screenLogicObjList.Count;i++) {
			if(string.Compare(LogicManager.Instance.screenLogicObjList[i].screenName,screenName) == 0) {
				currentScreenObj = (Transform)UnityEngine.Object.Instantiate(LogicManager.Instance.screenLogicObjList[i].transform);
				currentScreenObj.GetComponent<ScreenObj>().Init();
				currentScreenObj.parent = anchorRoot;
				//todo: reset scale, very important.You can try Toggle it.
				currentScreenObj.localScale = new Vector3(1,1,1);
				currentScreenObj.position = Vector3.zero;
				return;
			}
		}
	}
	
	private void LoadCurrTemplateObj() {	
		for(int i= 0;i<currScreenTemplateInfoList.Count;i++) {
			if(!currScreenTemplateInfoList[i].preLoad) {
				continue;
			}
			Transform currTemplate = null;
			if(UILoadManager.Instance == null) {
				GUILogManager.LogErr("UILoadManager.Instance == null");
			}
			UILoadManager.Instance.LoadingTemplate(currScreenTemplateInfoList[i],currTemplate);
		}
//		GUILogManager.LogInfo("LoadCurrTemplateObj finish.Current Template count: "+currScreenTemplateList.Count.ToString());
	}
	
	public delegate void Handle_ChangeScreenNameDelegate(string name);
    public event Handle_ChangeScreenNameDelegate OnChangeScreenName;
	private void ChangeScreenName(string name) {
		if(OnChangeScreenName != null) {
			OnChangeScreenName(name);
		}
	}
	
	private void FadeInLoading() {
		
	}
	
	private void FadeOutLoading() {
		
	}
#endregion
	
}
