using UnityEngine;
using System.Collections;

public class UI_Hud_AbilitySlot_Manager : MonoBehaviour
{
    public static UI_Hud_AbilitySlot_Manager Instance;

    void Awake(){ Instance = this; }
	
	void Start(){ InitSlots();}

    void Update()
    {
		if(Input.GetKeyDown(KeyCode.Alpha1) && !SLOTS[1-1].IsInCD() && (chatBoxManager.Instance == null || (chatBoxManager.Instance!= null && !chatBoxManager.Instance.isInputState)))
		{
            PlayerAbilityManager _abiManager = (PlayerAbilityManager)Player.Instance.abilityManager;
            _abiManager.UseAbility(_abiManager.GetAbilityIDInSlot(1-1), true);
		}
		if(Input.GetKeyDown(KeyCode.Alpha2) && !SLOTS[2-1].IsInCD() && (chatBoxManager.Instance == null || (chatBoxManager.Instance!= null && !chatBoxManager.Instance.isInputState)))
		{
            PlayerAbilityManager _abiManager = (PlayerAbilityManager)Player.Instance.abilityManager;
            _abiManager.UseAbility(_abiManager.GetAbilityIDInSlot(2-1), true);
		}
		if(Input.GetKeyDown(KeyCode.Alpha3) && !SLOTS[3-1].IsInCD() && (chatBoxManager.Instance == null || (chatBoxManager.Instance!= null && !chatBoxManager.Instance.isInputState)))
		{
            PlayerAbilityManager _abiManager = (PlayerAbilityManager)Player.Instance.abilityManager;
            _abiManager.UseAbility(_abiManager.GetAbilityIDInSlot(3-1), true);
		}
		if(Input.GetKeyDown(KeyCode.Alpha4) && !SLOTS[4-1].IsInCD() && (chatBoxManager.Instance == null || (chatBoxManager.Instance!= null && !chatBoxManager.Instance.isInputState)))
		{
            PlayerAbilityManager _abiManager = (PlayerAbilityManager)Player.Instance.abilityManager;
            _abiManager.UseAbility(_abiManager.GetAbilityIDInSlot(4-1), true);
		}
		if(Input.GetKeyDown(KeyCode.Alpha5) && !SLOTS[5-1].IsInCD() && (chatBoxManager.Instance == null || (chatBoxManager.Instance!= null && !chatBoxManager.Instance.isInputState)))
		{
            PlayerAbilityManager _abiManager = (PlayerAbilityManager)Player.Instance.abilityManager;
            _abiManager.UseAbility(_abiManager.GetAbilityIDInSlot(5-1), true);
		}
		if(Input.GetKeyDown(KeyCode.Alpha6) && !SLOTS[6-1].IsInCD() && (chatBoxManager.Instance == null || (chatBoxManager.Instance!= null && !chatBoxManager.Instance.isInputState)))
		{
            PlayerAbilityManager _abiManager = (PlayerAbilityManager)Player.Instance.abilityManager;
            _abiManager.UseAbility(_abiManager.GetAbilityIDInSlot(6-1), true);
		}
		if(Input.GetKeyUp(KeyCode.Alpha1) && (chatBoxManager.Instance == null || (chatBoxManager.Instance!= null && !chatBoxManager.Instance.isInputState)))
		{
            PlayerAbilityManager _abiManager = (PlayerAbilityManager)Player.Instance.abilityManager;
            _abiManager.KeyboardKeyUp(_abiManager.GetAbilityIDInSlot(1-1));
		}
		if(Input.GetKeyUp(KeyCode.Alpha2) && (chatBoxManager.Instance == null || (chatBoxManager.Instance!= null && !chatBoxManager.Instance.isInputState)))
		{
            PlayerAbilityManager _abiManager = (PlayerAbilityManager)Player.Instance.abilityManager;
            _abiManager.KeyboardKeyUp(_abiManager.GetAbilityIDInSlot(2-1));
		}
		if(Input.GetKeyUp(KeyCode.Alpha3) && (chatBoxManager.Instance == null || (chatBoxManager.Instance!= null && !chatBoxManager.Instance.isInputState)))
		{
            PlayerAbilityManager _abiManager = (PlayerAbilityManager)Player.Instance.abilityManager;
            _abiManager.KeyboardKeyUp(_abiManager.GetAbilityIDInSlot(3-1));
		}
		if(Input.GetKeyUp(KeyCode.Alpha4) && (chatBoxManager.Instance == null || (chatBoxManager.Instance!= null && !chatBoxManager.Instance.isInputState)))
		{
            PlayerAbilityManager _abiManager = (PlayerAbilityManager)Player.Instance.abilityManager;
            _abiManager.KeyboardKeyUp(_abiManager.GetAbilityIDInSlot(4-1));
		}
		if(Input.GetKeyUp(KeyCode.Alpha5) && (chatBoxManager.Instance == null || (chatBoxManager.Instance!= null && !chatBoxManager.Instance.isInputState)))
		{
            PlayerAbilityManager _abiManager = (PlayerAbilityManager)Player.Instance.abilityManager;
            _abiManager.KeyboardKeyUp(_abiManager.GetAbilityIDInSlot(5-1));
		}
		if(Input.GetKeyUp(KeyCode.Alpha6) && (chatBoxManager.Instance == null || (chatBoxManager.Instance!= null && !chatBoxManager.Instance.isInputState)))
		{
            PlayerAbilityManager _abiManager = (PlayerAbilityManager)Player.Instance.abilityManager;
            _abiManager.KeyboardKeyUp(_abiManager.GetAbilityIDInSlot(6-1));
		}
		/*
        if (_pressing)
        {
            if (Input.GetMouseButton(0) && Vector3.Distance(Input.mousePosition, _initPressPos) > 5f)
            {
                StartDragging();
            }
            if (Input.GetMouseButtonUp(0))
            {
                _pressing = false;
            }
        }
        if (_Dragging)
        {
            if (Input.GetMouseButton(0))
            {
                if (UI_GameHud_AbiSlot_UpdateDraging_Event != null)
                    UI_GameHud_AbiSlot_UpdateDraging_Event(_draggingslot, Input.mousePosition);
            }

            if (Input.GetMouseButtonUp(0))
            {
                ExitDragging();
            }
        }
        */
    }

    #region Interface

    [SerializeField]
    private UI_Hud_AbilitySlot_Single_Control[] SLOTS;

    #endregion

    #region Public
	
	public void InitSlots()
	{
		for(int i = 0; i < SLOTS.Length; i++)
		{
			PlayerAbilityManager _abilitymanager = (PlayerAbilityManager)Player.Instance.abilityManager;
			int _slotAbiID = _abilitymanager.GetAbilityIDInSlot(i);
			if(_slotAbiID > 0)
			{
				UI_TypeDefine.UI_GameHud_Abi_data _newdata = new UI_TypeDefine.UI_GameHud_Abi_data();
				_newdata.AbiID = _slotAbiID;
				AbilityBaseState _tempAbi = _abilitymanager.GetAbilityByID((uint)_newdata.AbiID);
				if(_tempAbi != null)
				{
					_newdata.IconSpriteName = _tempAbi.icon.name;
					SLOTS[i].UpdateAllInfo(_newdata);
				}
			}
		}
	}
	
	/// <summary>
	/// Gets the slot abi identifier by slot I.
	/// </summary>
	/// <returns>
	/// The slot abi identifier by slot I.
	/// </returns>
	/// <param name='_index'>
	/// _index.
	/// </param>
	public int GetSlotAbiIDBySlotID(int _index)
	{
		int _tempid = -1;
		if(_index < 0 || _index >= SLOTS.Length ||	// index is out of list length
			SLOTS[_index]._AbiData == null ||		// no data in slot
			SLOTS[_index].IsInCooldDown) 			// slot is in cd.
		{
			return _tempid;
		}
		return SLOTS[_index]._AbiData.AbiID;
	}
	
	/// <summary>
	/// Find the slot which is this ability, and start cooldown N sec.
	/// </summary>
	/// <param name='_abiid'>
	/// _abiid.
	/// </param>
	/// <param name='_cooldown'>
	/// _cooldown.
	/// </param>
	public void CoolDown(int _abiid, float _cooldown)
	{
		foreach(UI_Hud_AbilitySlot_Single_Control _slot in SLOTS)
		{
			if(_slot != null && _slot._AbiData != null && _slot._AbiData.AbiID == _abiid)
			{
				_slot.StartCooldown(_cooldown);
			}
		}
	}
	
    /// <summary>
    /// When an ability slot is clicked.
    /// </summary>
    /// <param name="_slot">which slot is clicked</param>
    /// <param name="_isLeftOrRightKey">TRUE : Left Key || FALSE : Right Key</param>
    public void AbilitySlotIsClicked(UI_Hud_AbilitySlot_Single_Control _slot, bool _isLeftOrRightKey)
    {
        if (_pressing || _Dragging)
        {
            if (UI_GameHud_AbiSlot_Click_Event != null)
                UI_GameHud_AbiSlot_Click_Event(_slot, _isLeftOrRightKey);
        }
    }

    #region Dragging
    bool _pressing;
    bool _Dragging;
    Vector3 _initPressPos;
    UI_Hud_AbilitySlot_Single_Control _draggingslot;

    public void BTNPressed(UI_Hud_AbilitySlot_Single_Control _slot, bool isCD)
    {
        _draggingslot = _slot;
        _initPressPos = Input.mousePosition;
        if (!isCD)
            _pressing = true;
        else
            _pressing = false;
    }

    #endregion

    #endregion

    #region Local

    #region delegate
    public delegate void Handle_GameHud_AbiSlots_ClickIcon_Delegate(UI_Hud_AbilitySlot_Single_Control _slot, bool _isLeftOrRightKey);
    public event Handle_GameHud_AbiSlots_ClickIcon_Delegate UI_GameHud_AbiSlot_Click_Event;

    public delegate void Handle_GameHud_AbiSlots_StartDragging_Delete(UI_Hud_AbilitySlot_Single_Control _abi_slot, Vector3 _mousepos);
    public event Handle_GameHud_AbiSlots_StartDragging_Delete UI_GameHud_AbiSlot_StartDraging_Event;

    public delegate void Handle_GameHud_AbiSlots_UpdateDragging_Delete(UI_Hud_AbilitySlot_Single_Control _abi_slot, Vector3 _mousepos);
    public event Handle_GameHud_AbiSlots_UpdateDragging_Delete UI_GameHud_AbiSlot_UpdateDraging_Event;

    public delegate void Handle_GameHud_AbiSlots_ExitDragging_Delete(UI_Hud_AbilitySlot_Single_Control _abi_slot, Vector3 _mousepos);
    public event Handle_GameHud_AbiSlots_ExitDragging_Delete UI_GameHud_AbiSlot_ExitDraging_Event;

    public delegate void Handle_GameHud_AbiSlots_HoveringSlot_Delete(UI_Hud_AbilitySlot_Single_Control _abi_slot, bool isHover);
    public event Handle_GameHud_AbiSlots_HoveringSlot_Delete UI_GameHud_AbiSlot_HoveringSlot_Event;
    #endregion

    #region delegate from dragitem

    public void HighLightAllSlots()
    {
        _lastHoveringSlot = null;

        foreach (UI_Hud_AbilitySlot_Single_Control _slot in SLOTS)
        {
            _slot.HighLight(true);
        }       
    }

    UI_Hud_AbilitySlot_Single_Control _lastHoveringSlot;
    public void CheckIfInAnySlot(UI_Hud_BaseDragItem _dragitem, Vector3 _mousePos)
    {
		if(_dragitem.GetComponent<UI_Hud_DragItem_Ability>() != null)
        {
            float _nearestDis = 100;
            Vector3 _worldPos = UICamera.currentCamera.ScreenToWorldPoint(_mousePos);
            UI_Hud_AbilitySlot_Single_Control _temp = null;
            foreach (UI_Hud_AbilitySlot_Single_Control _slot in SLOTS)
            {
                _worldPos.z = _slot.transform.position.z;
                if (Vector3.Distance(_slot.transform.position, _slot.transform.InverseTransformPoint(_worldPos)) < _nearestDis)
                {
                    _nearestDis = Vector3.Distance(_slot.transform.position, _slot.transform.InverseTransformPoint(_worldPos));
                    _temp = _slot;
                }
            }

            if (_temp != null)
            {
                if (_temp != _lastHoveringSlot)
                {
                    if (UI_GameHud_AbiSlot_HoveringSlot_Event != null)
                        UI_GameHud_AbiSlot_HoveringSlot_Event(_temp, true);


                    _temp.EnterHover();
                    if (_lastHoveringSlot != null)
                        _lastHoveringSlot.ExitHover();
                    _lastHoveringSlot = _temp;
                }
            }
            else
            {
                if (_lastHoveringSlot != null)
                {
                    if (UI_GameHud_AbiSlot_HoveringSlot_Event != null)
                        UI_GameHud_AbiSlot_HoveringSlot_Event(_lastHoveringSlot, false);
                    _lastHoveringSlot.ExitHover();
                    _lastHoveringSlot = null;
                }
            }
        }
    }

    public void CheckEquipSlot(UI_Hud_BaseDragItem _dragitem, Vector3 _mousePos)
    {
		if(_dragitem.GetComponent<UI_Hud_DragItem_Ability>() != null)
        //if (_data.ItemType == UI_TypeDefine.UI_GameHud_DragItem_data.EnumDragItemType.Ability || _data.ItemType == UI_TypeDefine.UI_GameHud_DragItem_data.EnumDragItemType.AbilitySlot)
        {
            float _nearestDis = 100;
            Vector3 _worldPos = UICamera.currentCamera.ScreenToWorldPoint(_mousePos);
            UI_Hud_AbilitySlot_Single_Control _temp = null;
			int _slotid = 0;
            for(int i = 0; i < SLOTS.Length; i++)
            {
				UI_Hud_AbilitySlot_Single_Control _slot = SLOTS[i];
                _worldPos.z = _slot.transform.position.z;
                if (Vector3.Distance(_slot.transform.position, _slot.transform.InverseTransformPoint(_worldPos)) < _nearestDis)
                {
                    _nearestDis = Vector3.Distance(_slot.transform.position, _slot.transform.InverseTransformPoint(_worldPos));
                    _temp = _slot;
					_slotid = i;
                }

                _slot.HighLight(false);
            }
			
			if(_temp != null)
			{
				// if dragging the ability from ability page.
				PlayerAbilityManager _abimanager = (PlayerAbilityManager)Player.Instance.abilityManager;
				UI_Hud_DragItem_Ability _dragabi = (UI_Hud_DragItem_Ability)_dragitem;
				
				UI_TypeDefine.UI_GameHud_Abi_data _newdata = new UI_TypeDefine.UI_GameHud_Abi_data();
	            _newdata.IconSpriteName = _dragitem.SpriteName;
				_newdata.AbiID = _abimanager.GetAbilityByType(_dragabi.AbilityType).id;
	            _temp.UpdateAllInfo(_newdata);
				
				CS_Main.Instance.g_commModule.SendMessage(
					   		ProtocolBattle_SendRequest.SetSkillShortcut(_temp._AbiData.AbiID, (int)_slotid / 3, _slotid % 3)
					);

				_abimanager.SetAbilitySlot(_slotid, _temp._AbiData.AbiID);
			}
        }
        _lastHoveringSlot = null;
    }

    #endregion

    #region Dragging

    void StartDragging()
    {
        _Dragging = true;
        _pressing = false;

        if (UI_GameHud_AbiSlot_StartDraging_Event != null)
            UI_GameHud_AbiSlot_StartDraging_Event(_draggingslot, Input.mousePosition);

        _draggingslot.RestoreSelf();
    }

    void ExitDragging()
    {
        _Dragging = false;
        _pressing = false;

        if (UI_GameHud_AbiSlot_ExitDraging_Event != null)
            UI_GameHud_AbiSlot_ExitDraging_Event(_draggingslot, Input.mousePosition);

        _draggingslot = null;
    }

    #endregion 

    #endregion
}
