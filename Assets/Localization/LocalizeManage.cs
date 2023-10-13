using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class LocalizeManage
{
    #region singleton instance
    private static LocalizeManage instance_ = null;
    public static LocalizeManage Instance{
        get {
			if(instance_ == null)
			{
				instance_ = new LocalizeManage();
				instance_.Awake();
			}
			return instance_;
		}
    }
    #endregion

    #region language EN/RU/ZH;

    public enum Language { EN, RU, ZH}

    [HideInInspector]
    private Language lang_ = Language.EN;
    public Language Lang { get { return lang_; } }

    public delegate void Handle_LangChanged(Language _lang);
    public event Handle_LangChanged OnLangChanged;

    private void LoadLangFrom() {

		Language newlang = LocalizeFontManager.Instance.defaultLang;
       
//        GUILogManager.LogErr("lang_: "+(int)lang_+"newlang: "+(int)newlang);
        if (newlang != lang_) {
            lang_ = newlang;
            SetDictionary();
#if NGUI
			if(LocalizeFontManager.Instance) {
				LocalizeFontManager.Instance.ChangeLangFontAll();
				PlayerDataManager.Instance.InitData();
			}
#endif				
            if (OnLangChanged != null) {
                OnLangChanged(lang_);
            } 
        }
        else
            SetDictionary();
    }

    public void SetLangInMenu(Language _newlang) {
        if (_newlang != lang_){
            lang_ = _newlang;
            SetDictionary();
			InitFile();
#if NGUI
			if(LocalizeFontManager.Instance) {
				LocalizeFontManager.Instance.ChangeLangFontAll();
				PlayerDataManager.Instance.InitData();
			}
#endif			
            if (OnLangChanged != null){
                OnLangChanged(lang_);
            }
            SaveLang();
        }
    }
	
	private void InitFile() {
		ItemDeployInfo.Instance.InitItemFile();
	}

    private void SaveLang() { 
    
    }
	
    #endregion

    #region Dictionary

    private Dictionary<string, string> dic_static_current_ = null;
    private Dictionary<string, string> dic_static_en_ = new Dictionary<string,string>();
    private Dictionary<string, string> dic_static_ru_ = new Dictionary<string, string>();

    private Dictionary<string, string> dic_dynamic_current_ = null;
    private Dictionary<string, string> dic_dynamic_en_ = new Dictionary<string, string>();
    private Dictionary<string, string> dic_dynamic_ru_ = new Dictionary<string, string>();

    private void InitAllDictionary() { 
        //EN
        InitDictionary(dic_static_en_, Language.EN, true);
        InitDictionary(dic_dynamic_en_, Language.EN, false);

        //RU
        InitDictionary(dic_static_ru_, Language.RU, true);
        InitDictionary(dic_dynamic_ru_, Language.RU, false);
    }

    private void InitDictionary(Dictionary<string, string> _dic, Language _lang, bool bstatic) {
        _dic.Clear();
        string sLang = _lang.ToString();
        string path = "";
        if(bstatic)
            path = string.Format(@"Localization/{0}/Text.Static.{1}", sLang, sLang);
        else
            path = string.Format(@"Localization/{0}/Text.Dynamic.{1}", sLang, sLang);
        TextAsset text = (TextAsset)Resources.Load(path, typeof(TextAsset));
        string[] itemRowsList = text.text.Split('\n');
        for (int i = 3; i < itemRowsList.Length - 1; ++i)
        {
            string item = itemRowsList[i];
            string[] vals = item.Split(new char[] { '	', '	' });
			
            try
            {
               _dic.Add(vals[0], vals[1]);
            }
            catch (Exception e) {
                Debug.LogError(string.Format(" {0}  {1} ", vals[0], vals[1]));
            }
        }
    }

    private void SetDictionary() { 
        switch(Lang){
            case Language.EN:
                {
                    dic_static_current_ = dic_static_en_;
                    dic_dynamic_current_ = dic_dynamic_en_;
                    break;
                }
            case Language.RU:
                {
                    dic_static_current_ = dic_static_ru_;
                    dic_dynamic_current_ = dic_dynamic_ru_;
                    break;
                }
            case Language.ZH:
                {
                
                    break;
                }
            default:
                break;
        }
    }

    #endregion

    #region Translate

    public string GetStaticText(string idxStr, string uiName)
    {
        string text = "";
        if (dic_static_current_.TryGetValue(idxStr, out text)){
            return text;
        }
        else {
            Debug.LogError(string.Format("[Localize Error] No index: {0}, {1}", idxStr, uiName));
            return "No Localize Text";
        }
    }

    public void GetDynamicText(SpriteText textComp, string idxStr)
    {
		//Refactory
        string text = "";
        if (dic_dynamic_current_.TryGetValue(idxStr, out text)){
			if(Lang == Language.EN){
				textComp.transform.renderer.material = LocalizeFontManager.Instance.Mat_en;
                textComp.SetFont(LocalizeFontManager.Instance.Font_en, LocalizeFontManager.Instance.Mat_en);
			}
			else if(Lang == Language.RU){
                textComp.transform.renderer.material = LocalizeFontManager.Instance.Mat_ru;
                textComp.SetFont(LocalizeFontManager.Instance.Font_ru, LocalizeFontManager.Instance.Mat_ru);
			}
            textComp.Text = text;
            return;
        }
        else{
            Debug.LogError(string.Format("[Localize Error] No index: {0}, {1}", idxStr, textComp.gameObject.name));
            textComp.Text = "No Localize Text";
            return;
        }
    }
	
	//this for ngui. now this is temp. 20130621//
	public string GetDynamicText(string idxStr) {
		string text = "";
		if (dic_dynamic_current_.TryGetValue(idxStr, out text)) {
			return text;
		}else {
			Debug.LogError(string.Format("[Localize Error] No index: {0}", idxStr));
			return idxStr;
		}
	}

    #endregion

    #region Unity Function  

    public void Awake() {

        instance_ = this;

        InitAllDictionary();
		
    }

    public void Start() {

        //Call this in Start(), not in Awake()
        //Because UI object register event in Awake, so we must make sure that event occur is after event register
        LoadLangFrom();
		#region first screen
#if NGUI
        GUIManager.Instance.ChangeUIScreenState("LoginScreen");
#endif
		#endregion
    }

    private string inputBuff_ = "";
    void Update() {
//        return;
//        inputBuff_ += Input.inputString;
//        if (inputBuff_.ToLower().Contains("ru")) {
//            inputBuff_ = "";
//            SetLangInMenu(Language.RU);
//        
//        else if (inputBuff_.ToLower().Contains("en")) {
//            inputBuff_ = "";
//            SetLangInMenu(Language.EN);
//        }
		
    }

    #endregion
	
	public string GetLangPath(string fileName) {
		string path = "";
		path = string.Format(@"Localization/{0}/{1}.{2}", lang_.ToString(),fileName,lang_.ToString());
        return path;
    }
    
}


#region Unused

//[SerializeField]
//private LocalizeConfig config_ = null;

//[HideInInspector]
//public LocalizeConfig.Language Lang
//{
//    get { return config_.Lang; }
//}

#endregion