using UnityEngine;
using System.Collections;

public class StaticInit : MonoBehaviour {

    private static LocalizeManage localizeMgr_ = null;
	private SpriteText 		spiritTextComp_;
	public string			index;
	private UILabel contentText;

    //Mat,Font for EN
    private Material mat_en_ = null;
    private TextAsset font_en_ = null;
    private Material mat_ru_ = null;
    private TextAsset font_ru_ = null;
	
	void Start() {
		 //Can not use LocalizeManage.Instance, Instance is inited in Awake, here is also Awake
        if (localizeMgr_ == null) {
            localizeMgr_ = LocalizeFontManager.ManagerInstance;
        }
        localizeMgr_.OnLangChanged += this.setLocalizeText;
#if NGUI
		contentText = transform.GetComponent<UILabel>();
		contentText.text = LocalizeManage.Instance.GetStaticText(index, transform.gameObject.name);
#else		
		spiritTextComp_ = transform.GetComponent<SpriteText>();
        mat_en_ = transform.renderer.material;
        font_en_ = spiritTextComp_.font;
        mat_ru_ = LocalizeFontManager.Instance.Mat_ru;
        font_ru_ = LocalizeFontManager.Instance.Font_ru;
#endif
	}

    private void setLocalizeText(LocalizeManage.Language _lang) {
		
        if (_lang == LocalizeManage.Language.EN) {
#if NGUI
			 contentText.text = LocalizeManage.Instance.GetStaticText(index, transform.gameObject.name);
#else			
            transform.renderer.material = mat_en_;
            spiritTextComp_.SetFont(font_en_, mat_en_);
            spiritTextComp_.Text = LocalizeManage.Instance.GetStaticText(index, transform.gameObject.name);
#endif 			
        }
        else if (_lang == LocalizeManage.Language.RU) {
#if NGUI
			contentText.text = LocalizeManage.Instance.GetStaticText(index, transform.gameObject.name);
#else			
            transform.renderer.material = mat_ru_;
            spiritTextComp_.SetFont(font_ru_, mat_ru_);
            spiritTextComp_.Text = LocalizeManage.Instance.GetStaticText(index, transform.gameObject.name);
#endif			
        }
    }
}
