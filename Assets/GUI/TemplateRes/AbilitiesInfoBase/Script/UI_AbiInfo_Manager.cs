using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UI_AbiInfo_Manager : MonoBehaviour {

    public static UI_AbiInfo_Manager Instance;
	
	public UI_AbiInfo_DetailGroup DetailGroup;
	
	public UI_Hud_DragItem_Ability DragItemPrefab;
	
	public UI_Hud_Border_Control SelectBorder;
	
    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        if (GetComponent<UITemplate>())
            GetComponent<UITemplate>().OnTemplateDestroy += new UITemplate.Handle_DestroyDelegate(OnTemplateDestroy);
    }

    void Update()
    {
        if (_pressing)
        {
            if (Input.GetMouseButton(0) && Vector3.Distance(Input.mousePosition, PressPos) > 5f)
            {
                StartDragging();
            }
            if (Input.GetMouseButtonUp(0))
            {
                _pressing = false;
            }
        }
    }

    #region Interface
    public AbiInfo_SingleAbi_Control[] UI_Abis = new AbiInfo_SingleAbi_Control[0];

    [SerializeField]  Color PowerNameColor;
    [SerializeField]  Color DefenceNameColor;
    [SerializeField]  Color SkillNameColor;
	
	public void InitAll()
	{
		
	}
	
	public void UpdateAllAbilitiesInfo()
	{
		for(int i = 0; i < UI_Abis.Length; i++)
		{
			UI_Abis[i].UpdateInfo();
		}
	}
	
	public void UpdateDetailInfoForAbility(AbilityDetailInfo.EnumAbilityType _type, string _iconName)
	{
		DetailGroup.UpdateDetailInfo(true, _type, GetColorByAbilityType(_type), _iconName);
		
		for(int i = 0; i < UI_Abis.Length; i++)
		{
			if(UI_Abis[i].BelongType == _type)
			{
				Vector3 _tempPos = SelectBorder.transform.position;
				_tempPos.x = UI_Abis[i].transform.position.x;
				_tempPos.y = UI_Abis[i].transform.position.y;
				SelectBorder.transform.position = _tempPos;
				break;
			}
		}
	}
	
	public Color GetColorByAbilityType(AbilityDetailInfo.EnumAbilityType _type)
	{
		if(_type == AbilityDetailInfo.EnumAbilityType.SwathOfDestruction ||_type == AbilityDetailInfo.EnumAbilityType.SeeingRed ||
			_type == AbilityDetailInfo.EnumAbilityType.HungryCleave ||_type == AbilityDetailInfo.EnumAbilityType.RainOfBlow ||
			_type == AbilityDetailInfo.EnumAbilityType.Shockwave ||_type == AbilityDetailInfo.EnumAbilityType.MeteorOfRain)
		{
			return PowerNameColor;
		}
		if(_type == AbilityDetailInfo.EnumAbilityType.SkinOfStone ||_type == AbilityDetailInfo.EnumAbilityType.HauntingScream ||
			_type == AbilityDetailInfo.EnumAbilityType.IronThrone ||_type == AbilityDetailInfo.EnumAbilityType.IceBarricade ||
			_type == AbilityDetailInfo.EnumAbilityType.Chiprayer ||_type == AbilityDetailInfo.EnumAbilityType.Chimend)
		{
			return DefenceNameColor;
		}
		if(_type == AbilityDetailInfo.EnumAbilityType.SteadyShoot ||_type == AbilityDetailInfo.EnumAbilityType.StringingShoot ||
			_type == AbilityDetailInfo.EnumAbilityType.NinjaEscape ||_type == AbilityDetailInfo.EnumAbilityType.Caltrops ||
			_type == AbilityDetailInfo.EnumAbilityType.FirebomTrap ||_type == AbilityDetailInfo.EnumAbilityType.DarkHunter)
		{
			return SkillNameColor;
		}
		
		return Color.white;
	}
	
    #region Dragging & Clicking
    // Dragging stuff
    Vector3 PressPos;
    AbiInfo_SingleAbi_Control _draggingAbi;
    AbiInfo_SingleAbi_Control _clickAbi;
    public bool _Dragging = false;
    public bool _pressing = false;

    public void BTNPreesed(AbiInfo_SingleAbi_Control _btn)
    {
		if(_btn.AbilityInfo!= null)
		{
	        _draggingAbi = _btn;
	        PressPos = Input.mousePosition;
	        _pressing = true;
		}
    }

    public void BTNClicked(AbiInfo_SingleAbi_Control _btn)
    {
        if (!_Dragging)
        {
            bool isSameBTN = false;
            if (_clickAbi == null || (_clickAbi != null && _clickAbi != _btn))
            {
                _clickAbi = _btn;
            }
            else
            {
                _clickAbi = null;
                isSameBTN = true;
            }

            if (UI_Abi_ClickIcon_Event != null)
                UI_Abi_ClickIcon_Event(_btn, isSameBTN);

            _draggingAbi = null;
            _pressing = false;
            _Dragging = false;

            Debug.Log("Clicked");
        }
    }
    #endregion

    #endregion

    #region Local

    #region delegate

    public delegate void Handle_CharInfo_Abi_ClickIcon_Delegate(AbiInfo_SingleAbi_Control _abi_control, bool _isSameBTN);
    public event Handle_CharInfo_Abi_ClickIcon_Delegate UI_Abi_ClickIcon_Event;

    public delegate void Handle_CharInfo_Abi_StartDragging_Delete(AbiInfo_SingleAbi_Control _abi_control, Vector3 _mousepos);
    public event Handle_CharInfo_Abi_StartDragging_Delete UI_Abi_StartDraging_Event;

    public delegate void Handle_CharInfo_Abi_UpdateDragging_Delete(AbiInfo_SingleAbi_Control _abi_control, Vector3 _mousepos);
    public event Handle_CharInfo_Abi_UpdateDragging_Delete UI_Abi_UpdateDraging_Event;

    public delegate void Handle_CharInfo_Abi_ExitDragging_Delete(AbiInfo_SingleAbi_Control _abi_control, Vector3 _mousepos);
    public event Handle_CharInfo_Abi_ExitDragging_Delete UI_Abi_ExitDraging_Event;

    #endregion

    #region Ondestroy

    void OnTemplateDestroy(string _templateName)
    {
        if (_templateName == GetComponent<UITemplate>().templateName)
        {
            ExitDragging();
        }
    }

    #endregion

    void StartDragging()
    {
        _Dragging = true;
        _pressing = false;
		
		UI_Hud_DragItem_Ability _dragitem = UnityEngine.Object.Instantiate(DragItemPrefab) as UI_Hud_DragItem_Ability;
		_dragitem.AbilityType = _draggingAbi.BelongType;
		_dragitem.transform.parent= transform.parent.parent;
		_dragitem.transform.localScale = Vector3.one;
		_dragitem.UpdateIcon(_draggingAbi.AbiIconName);
		_dragitem.transform.localPosition = new Vector3(1000, 1000, -100);
    }

    public void ExitDragging()
    {
        _Dragging = false;
        _pressing = false;
        _draggingAbi = null;

    }
    #endregion
}
