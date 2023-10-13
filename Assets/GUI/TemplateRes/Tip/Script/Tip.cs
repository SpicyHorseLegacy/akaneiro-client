using UnityEngine;
using System.Collections;

public class Tip : MonoBehaviour
{
    #region Interface
    public void Hide(bool hide)
    {
        if (gameObject) gameObject.SetActive(!hide);
    }

    [SerializeField]
    private UILabel _name;
    [SerializeField]
    private UISprite _titleBG;
    public void SetName(string name, Color color)
    {
        _name.text = name;
        _name.color = color;
    }
    public void SetTitleBgColor(Color color)
    {
        _titleBG.color = color;
    }
    [SerializeField]
    private UILabel _level;
    public void SetLevel(int level, int playerLv)
    {
        if (playerLv >= level)
        {
            _level.color = Color.white;
        }
        else
        {
            _level.color = Color.red;
        }
        _level.text = level.ToString();
    }
    [SerializeField]
    private UILabel _type;
    public void SetType(string type)
    {
        _type.text = type.ToString();
    }
    [SerializeField]
    private UISprite _icon;
    [SerializeField]
    private UILabel _mainTitle;
    [SerializeField]
    private UILabel _mainContent;
    public void SetIcon(int type, string content)
    {
        switch (type)
        {
            case 1:
                _icon.spriteName = "Item_G_Attack";
                _mainTitle.text = "DAMAGE";
                _mainContent.text = content;
                break;
            case 2:
                _icon.spriteName = "Item_G_Shield";
                _mainTitle.text = "DEFENSE";
                _mainContent.text = content;
                break;
            case 3:
                _icon.spriteName = "Load_1";
                _mainTitle.text = "";
                _mainContent.text = "";
                break;
        }
    }
    public void SetOtherItemContent(string title)
    {
        _mainContent.text = title;
        //		_mainContent.text = contnet;
    }
    [SerializeField]
    private UILabel _speedTitle;
    [SerializeField]
    private UILabel _speedContent;
    public void SetSpeed(bool isEmpty, string content)
    {
        if (isEmpty)
        {
            _speedTitle.text = "";
            _speedContent.text = "";
        }
        else
        {
            _speedTitle.text = "SPEED";
            _speedContent.text = content;
        }
    }
    private int eleCount = 0;
    [SerializeField]
    private Transform eleTran;
    [SerializeField]
    private UILabel _eleTitle;
    [SerializeField]
    private UILabel _eleContent;
    [SerializeField]
    private UITexture _eleIcon;
    public void SetEleInfo(bool isEmpty, int count, Texture2D icon, string title, string content)
    {
        float y = 0;
        if (!isEmpty)
        {
            y = -(80 + (count - 1) * 35);
            NGUITools.SetActive(eleTran.gameObject, true);
            eleTran.localPosition = new Vector3(0, y, 0);
            _eleTitle.text = title;
            _eleContent.text = content;
            _eleIcon.mainTexture = icon;
        }
        else
        {
            NGUITools.SetActive(eleTran.gameObject, false);
        }
    }
    [SerializeField]
    private Transform encTran;
    [SerializeField]
    private UILabel _encTitle;
    [SerializeField]
    private UILabel _encContent;
    [SerializeField]
    private UITexture _encIcon;
    public void SetEncInfo(bool isEmpty, int count, Texture2D icon, string title, string content)
    {
        float y = 0;
        if (!isEmpty)
        {
            y = -(80 + (count - 1) * 35);
            NGUITools.SetActive(encTran.gameObject, true);
            encTran.localPosition = new Vector3(0, y, 0);
            _encTitle.text = title;
            _encContent.text = content;
            _encIcon.mainTexture = icon;
        }
        else
        {
            NGUITools.SetActive(encTran.gameObject, false);
        }
    }
    [SerializeField]
    private Transform gemTran;
    [SerializeField]
    private UILabel _gemTitle;
    [SerializeField]
    private UILabel _gemContent;
    [SerializeField]
    private UITexture _gemIcon;
    public void SetGemInfo(bool isEmpty, int count, Texture2D icon, string title, string content)
    {
        float y = 0;
        if (!isEmpty)
        {
            y = -(80 + (count - 1) * 35);
            NGUITools.SetActive(gemTran.gameObject, true);
            gemTran.localPosition = new Vector3(0, y, 0);
            _gemTitle.text = title;
            _gemContent.text = content;
            _gemIcon.mainTexture = icon;
        }
        else
        {
            NGUITools.SetActive(gemTran.gameObject, false);
        }
    }
    [SerializeField]
    private UILabel _money;
    [SerializeField]
    private Transform _moneyRoot;
    public void SetMoney(int val, int count)
    {
        //		float y = 0;
        //		y = -(80+(count)*35);
        //		_moneyRoot.localPosition = new Vector3(0,y,0);
        _money.text = val.ToString();
    }
    [SerializeField]
    private UISlicedSprite _arrowBg;
    public void SetArrowBgSize(int count)
    {
        //		_arrowBg.transform.localScale = new Vector3(35+count*35+9,30,1);
    }
    [SerializeField]
    private UI_Hud_Border_Control _highLight;
    public void SetHighLightSize(int count, bool isShow)
    {
        NGUITools.SetActive(_highLight.gameObject, isShow);
        if (isShow)
        {
            _highLight.ChangeColor((int.Parse(_level.text) <= PlayerDataManager.Instance.CurLV) ? Color.yellow : Color.red);
            _highLight.Pop(1, -1);
        }

    }
    #endregion
}
