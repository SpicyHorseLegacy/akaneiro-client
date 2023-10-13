using System;
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UILoadManager : MonoBehaviour {
	
	//if you define struct, can't modify value.
	public class DownLoadData{
		public string 		bundleNameString;
		public bool   		bHasDownloaded;
		public Transform 	obj;
		public WWW 			cachedWWW;
	}
	
	//Instance
	public static UILoadManager Instance  = null;
	
	[HideInInspector]
	public static Dictionary<string,UILoadManager.DownLoadData> CachedObjectMap = new Dictionary<string,UILoadManager.DownLoadData>();
	
	void Awake() 
    {
		Instance = this;
	}
	
	void Start () 
    {
#if UNITY_WEBPLAYER
        StartCoroutine(PreLoadUITemplates());    
#endif
	}
	
	private void InitTemplate(STemplateInfo templateInfo,Transform obj) {
		if(!obj.GetComponent<UITemplate>()) {
			obj.localScale = Vector3.zero;
			GUILogManager.LogErr("InitTemplate faile! UITemplate script is null.");
			return;
		}
		STemplateInfo tData = GUIManager.Instance.GetTemplateData(templateInfo.templateName);
		if(tData == null) {
//			GUILogManager.LogErr("templateName no reload.cant load.May be you load last screen template.");
			Destroy(obj.gameObject);
			return;
		}
		obj.GetComponent<UITemplate>().data = templateInfo;
		GUIManager.Instance.currScreenTemplateList.Add(obj.GetComponent<UITemplate>());
		//父节点从NGUI根节点改为屏节点
//		obj.parent = GUIManager.Instance.anchorRoot; //before
		obj.parent = GUIManager.Instance.currentScreenObj; //now
		//todo: reset scale, very important.You can try Toggle it.
		obj.localScale = new Vector3(1,1,1);
		obj.position = Vector3.zero;
		obj.GetComponent<UITemplate>().Init();
		GUIManager.Instance.TemplateInit(templateInfo.templateName);
	}
	
	public void LoadingTemplate(STemplateInfo templateInfo,Transform obj){
		StartCoroutine(StartDownloadTmplate(templateInfo,obj));
	}
	
	IEnumerator StartDownloadTmplate(STemplateInfo templateInfo,Transform obj){
		float _starttime = Time.time;
		string strDownLoadFolder = BundlePath.UIbundleBaseURL;
		string strDownLoadFile = "";
		WWW tempWWW = null;
	  if(templateInfo.templateName.Length > 0){
		 UnityEngine.Object tempObj = null;
		 if(!CachedObjectMap.ContainsKey(templateInfo.templateName)){
		    strDownLoadFile = strDownLoadFolder + templateInfo.templateName + ".assetbundle";
//			GUILogManager.LogWarn("file path:"+strDownLoadFile);
		    tempWWW = new WWW(strDownLoadFile);
			DownLoadData newData = new DownLoadData();
			newData.bHasDownloaded 		= false;
			newData.bundleNameString 	= templateInfo.templateName;
			newData.obj 				= null;
			newData.cachedWWW		 	= tempWWW;
			CachedObjectMap.Add(templateInfo.templateName,newData);
            yield return tempWWW;
			if(tempWWW.error != null) {
				GUILogManager.LogErr("WWW Err: "+tempWWW.error);
			}
		    tempObj = tempWWW.assetBundle.mainAsset;
			if(tempObj != null){
			  CachedObjectMap[templateInfo.templateName].bHasDownloaded = true;
			  CachedObjectMap[templateInfo.templateName].obj = (tempObj as GameObject).transform;
//			  GUILogManager.LogInfo("<"+bundleName +"> bundle download success");
		    }
		 }
		 if(CachedObjectMap.ContainsKey(templateInfo.templateName)){
			if(!CachedObjectMap[templateInfo.templateName].bHasDownloaded) {
				 yield return null;
			}else {
				obj = (Transform)UnityEngine.Object.Instantiate(CachedObjectMap[templateInfo.templateName].obj);
				if(obj == null) {
					GUILogManager.LogErr("bundle instantiate fail.");
				}
				InitTemplate(templateInfo,obj);
			}
	     }
		}else {
	   		GUILogManager.LogInfo("bundle download fail,name length <= 0");
		}
		
		Debug.Log("[UI] Assetbundle [" + templateInfo.templateName + "] Download Done. Cost : " + (Time.time - _starttime) + "sec");
	}


    IEnumerator PreLoadUITemplates()
    {
        //yield return new WaitForSeconds(3f);
        string[] templates = { 
                                "AvatarPop",
                                "BottomTip",
                                "CharInfo_Abilities",
                                "CharInfo_BG",
                                "CharInfo_Equips",
                                "CharInfo_Inventory",
                                "CharInfo_Stat",
                                "CharInfo_Tooltips",
                                "CharInfo_Trials",
                                "ChatBoxGroup",
                                "CreateBase",
                                "CrystalRecharge",
                                "DailyReward",
                                "Events",
                                //"FaceBookPop",
                                "FoodSlot",
                                "GameHud_AbilitySlots",
                                "GameHud_BloodBorder",
                                "GameHud_BTNGroup_CharInfo",
                                "GameHud_BuffBar",
                                "GameHud_DamageTXT",
                                "GameHud_DragItem",
                                "GameHud_HPMP",
                                "GameHud_KillChain",
                                "GameHud_Revive",
                                "GameHud_Social",
                                "GameHud_TopHPBar",
                                "InGameLootTooltip",
                                "ItemSell",
                                "ItemShop",
                                "ItemTooltips",
                                "KarmaRecharge",
                                "LevelUpBase",
                                "LoadingBase",
                                "LoginBase",
                                "LoginInput",
                                "LoginLogo",
                                "MissionObjective",
                                "MissionSelectBase",
                                "MoneyBar",
                                "Option_BugReport",
                                "Option_GiftCode",
                                "Option_HelpIndex",
                                "Option_Main",
                                "Option_Setting",
                                "OtherBTNS",
                                "Ping",
                                "SelectBase",
                                "SelectList",
                                "Shop_Ability",
                                "Shop_BG",
                                "Shop_BG2",
                                "Shop_ConsumableItem",
                                "Shop_Consumable_Success",
                                "Shop_Crafting",
                                "Shop_Mail",
                                "Shop_Sprite",
                                "Shop_Sprite_Success",
                                "Shop_Title",
                                "Social_FriendInformation",
                                "Social_FriendList",
                                "SpeedUpBox",
                                "Stash",
                                "SuccessBase",
                                "SuccessBaseLevel",
                                "SummonReward",
                                "TipsManager",
                                "TutorialPanel",
                                "XPBar"
                             };


        foreach (string template in templates)
        {
            float _starttime = Time.time;
            string strDownLoadFolder = BundlePath.UIbundleBaseURL;
            string strDownLoadFile = "";
            WWW tempWWW = null;

            if (template.Length > 0)
            {
                UnityEngine.Object tempObj = null;
                if (!CachedObjectMap.ContainsKey(template))
                {
                    strDownLoadFile = strDownLoadFolder + template + ".assetbundle";
                    tempWWW = new WWW(strDownLoadFile);
                    DownLoadData newData = new DownLoadData();
                    newData.bHasDownloaded = false;
                    newData.bundleNameString = template;
                    newData.obj = null;
                    newData.cachedWWW = tempWWW;
                    CachedObjectMap.Add(template, newData);
                    yield return tempWWW;
                    if (tempWWW.error != null)
                    {
                        GUILogManager.LogErr("WWW Err: " + tempWWW.error);
                    }
                    tempObj = tempWWW.assetBundle.mainAsset;
                    if (tempObj != null)
                    {
                        CachedObjectMap[template].bHasDownloaded = true;
                        CachedObjectMap[template].obj = (tempObj as GameObject).transform;
                    }
                }
                if (CachedObjectMap.ContainsKey(template))
                {
                    if (!CachedObjectMap[template].bHasDownloaded)
                    {
                        yield return null;
                    }
                }
            }
            else
            {
                GUILogManager.LogInfo("bundle download fail,name length <= 0");
            }

            logStr = "[UI] Assetbundle [" + template + "] Download Done. Cost : " + (Time.time - _starttime) + "sec";
            nbBundles++;
            GUILogManager.LogInfo(logStr);
        }
    }

    string logStr = "";
    int nbBundles = 0;

    void OnGUI()
    {   
        // Uncomment to debug UI preloading
        //GUILayout.Label(nbBundles + " preloaded as so far, last one : " + logStr);
    }
}
