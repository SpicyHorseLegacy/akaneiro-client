using UnityEngine;
using System.Collections;

public class LocalizeFontManager : MonoBehaviour {

    public static LocalizeFontManager Instance
    {
        get
        {
            if (_instance == null)
            {
#if NGUI
				_instance = GameObject.Find("LocalizationManager").GetComponent<LocalizeFontManager>();
#else
                _instance = GameObject.Find("UI manager").GetComponent<LocalizeFontManager>();
#endif
            }
            return _instance;
        }
        set
        {
            _instance = value;
        }
    }
    static LocalizeFontManager _instance;

    public static LocalizeManage ManagerInstance
    {
        get
        {
            if (Instance._managerInstance == null)
            {
                Instance._managerInstance = new LocalizeManage();
                Instance._managerInstance.Awake();
            }
            return Instance._managerInstance;
        }
    }
    LocalizeManage _managerInstance = null;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        ManagerInstance.Start();
    }
	
	void Update() {
//		if(Input.GetKeyDown(KeyCode.R)) {
//			_managerInstance.SetLangInMenu(LocalizeManage.Language.RU);
//		}
//		if(Input.GetKeyDown(KeyCode.E)) {
//			_managerInstance.SetLangInMenu(LocalizeManage.Language.EN);
//		}
	}
	
	
	
    #region Mat,Font
	
	public LocalizeManage.Language defaultLang = LocalizeManage.Language.EN;
	
	public UIFont cabin;
	public UIFont englishFont;
	public UIFont russianFont;
	[SerializeField]
	private Transform UIRoot;
	
	public void ChangeLangFontAll() {
		Component[] tComponents;
		tComponents = UIRoot.GetComponentsInChildren<UILabel>();
		foreach (UILabel label in tComponents) {
			if(Instance._managerInstance != null) {
				if(Instance._managerInstance.Lang == LocalizeManage.Language.EN) {
					label.font.replacement = englishFont;
				}else if(Instance._managerInstance.Lang == LocalizeManage.Language.RU) {
					label.font.replacement = russianFont;;
				}
			}
		}
	}
	
	public void ChangeLangFontTarget(Transform target) {
		Component[] tComponents;
		tComponents = target.GetComponentsInChildren<UILabel>();
		foreach (UILabel label in tComponents) {
			if(Instance._managerInstance != null) {
				if(Instance._managerInstance.Lang == LocalizeManage.Language.EN) {
					label.font.replacement = englishFont;
				}else if(Instance._managerInstance.Lang == LocalizeManage.Language.RU) {
					label.font.replacement = russianFont;;
				}
			}
		}
	}
	
    public Material Mat_en = null;
    public TextAsset Font_en = null;
    public Material Mat_ru = null;
    public TextAsset Font_ru = null;

    public Material GetCurrentMat()
    {
        if (LocalizeManage.Instance.Lang == LocalizeManage.Language.EN)
            return Mat_en;
        else if (LocalizeManage.Instance.Lang == LocalizeManage.Language.RU)
            return Mat_ru;
        else
            return Mat_en;
    }

    public TextAsset GetCurrentFont()
    {
        if (LocalizeManage.Instance.Lang == LocalizeManage.Language.EN)
            return Font_en;
        else if (LocalizeManage.Instance.Lang == LocalizeManage.Language.RU)
            return Font_ru;
        else
            return Font_en;
    }

    #endregion
}
