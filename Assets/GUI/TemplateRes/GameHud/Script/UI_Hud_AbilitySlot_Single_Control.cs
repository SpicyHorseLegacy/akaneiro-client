using UnityEngine;
using System.Collections;

public class UI_Hud_AbilitySlot_Single_Control : MonoBehaviour
{
    void Awake()
    {
        _DefaultIconName = AbiIcon.spriteName;
        _DefaultColor = AbiIcon.color;
        _DefaultPos = transform.localPosition;

        Border.SetAlpha(0);
    }

    void Update()
    {
        if (isCD)
        {
            _curCD += Time.deltaTime;
            
            if (_curCD > _maxCD)
            {
                _curCD = _maxCD;
                CDFinish();
            }

            UpdateCooldown(_curCD / _maxCD);
        }
		 
		if(_info != null)
		{
			if(_info.ManaCost <= Player.Instance.AttrMan.Attrs[EAttributeType.ATTR_CurMP])
			{
				AbiIcon.alpha = 1;
				CDIcon.alpha = 1;
			}
			else
			{
				AbiIcon.alpha = 0.3f;
				CDIcon.alpha = 0.3f;
			}
		}
    }

    #region Interface

    public UI_TypeDefine.UI_GameHud_Abi_data _AbiData;
	AbilityDetailInfo _info;

    [SerializeField]  UI_Hud_Border_Control Border;

    [SerializeField]  UISprite AbiIcon;
    string _DefaultIconName;
    Color _DefaultColor;

    [SerializeField]  UIFilledSprite CDIcon;

    Vector3 _DefaultPos;
	
	public bool IsInCooldDown{get{return isCD;}}
    bool isCD;
    float _curCD;
    float _maxCD;

    #endregion

    #region Public

    public void UpdateIcon(string _spriteStringName)
    {
        if (_spriteStringName != null && _spriteStringName.Length > 0)
        {
            AbiIcon.spriteName = _spriteStringName;
            Color _a = _DefaultColor;
            _a.a = 1;
            AbiIcon.color = _a;

            CDIcon.spriteName = _spriteStringName;
            CDIcon.color = Color.white;
        }
        else
        {
            AbiIcon.spriteName = _DefaultIconName;
            AbiIcon.color = _DefaultColor;

            CDIcon.spriteName = _DefaultIconName;
            CDIcon.color = new Color(1,1,1,0);
        }
    }

    public void StartCooldown(float cd)
    {
        isCD = true;
        _curCD = 0;
        _maxCD = cd;
    }

    public void UpdateCooldown(float _per)
    {
        CDIcon.fillAmount = _per;
    }

    public void UpdateAllInfo(UI_TypeDefine.UI_GameHud_Abi_data _data)
    {
        if (_data == null)
        {
            RestoreSelf();
        }
        else
        {
            _AbiData = _data;
			_info = AbilityInfo.GetAbilityDetailInfoByID(_AbiData.AbiID);
            UpdateIcon(_data.IconSpriteName);
			
            StartCooldown(0.5f);
        }
    }

    public void RestoreSelf()
    {
        UpdateIcon("");
        _AbiData = null;
		_info = null;
    }

    public void HighLight(bool isHighLight)
    {
        if (isHighLight)
        {
            TweenAlpha.Begin(Border.gameObject, 0.25f, 1);
            TweenScale.Begin(gameObject, 0.25f, Vector3.one * 1.1f);
        }
        else
        {
            Color _c = Color.white;
            _c.a = 0;
            Border.ChangeColor(_c);
            TweenScale.Begin(gameObject, 0.25f, Vector3.one);
            
        }
    }

    public void EnterHover()
    {
        TweenScale.Begin(gameObject, 0.25f, Vector3.one * 1.2f);
        Border.ChangeColor(Color.yellow);
    }

    public void ExitHover()
    {
        TweenScale.Begin(gameObject, 0.25f, Vector3.one * 1.1f);
        Border.ChangeColor(Color.white);
    }

	public bool IsInCD()
	{
		return isCD;
	}

    #endregion

    #region Local

    void CDFinish()
    {
        isCD = false;

        Pop();
    }

    void Pop()
    {
        TweenScale.Begin(gameObject, 0.05f, Vector3.one * 1.1f);
        TweenDelay.Begin(gameObject, 0.1f, "ResizeObj", null);
    }

    void ResizeObj()
    {
        TweenScale.Begin(gameObject, 0.1f, Vector3.one);
    }


    #region BTNCallback

    void BTNPressed()
    {
        if (UI_Hud_AbilitySlot_Manager.Instance != null)
        {
            UI_Hud_AbilitySlot_Manager.Instance.BTNPressed(this, isCD);
        }
    }

    void BTNClickedByLeft()
    {
        if (!isCD)
        {
            if (UI_Hud_AbilitySlot_Manager.Instance)
            {
                UI_Hud_AbilitySlot_Manager.Instance.AbilitySlotIsClicked(this, true);
            }
        }
    }

    void BTNClickedByRight()
    {
        if (!isCD)
        {
            if (UI_Hud_AbilitySlot_Manager.Instance)
            {
                UI_Hud_AbilitySlot_Manager.Instance.AbilitySlotIsClicked(this, false);
            }
        }
    }
    #endregion

    #endregion
}
