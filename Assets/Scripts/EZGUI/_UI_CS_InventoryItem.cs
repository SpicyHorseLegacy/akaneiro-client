using UnityEngine;
using System.Collections;

public class _UI_CS_InventoryItem : _UI_CS_BaseItem{
	
	//物品信息
	[SerializeField]
	public SItemInfo					m_ItemInfo = new SItemInfo();
	public ItemDropStruct 				ItemStruct = new ItemDropStruct();
	//可交互组
	// 0 < 装备组 >
	// 1 < 背包组 >
	// 2 < 仓库组 >
	public _UI_CS_InventoryItemGroup  	[] m_Group;
	public int							m_GroupCount;
	public SpriteText					m_MyCountText;
	private bool IsChang = false;
	public UIButton HighLightBG;
	public UIButton highLevel;
	public UIButton BG;
	public UIButton ClonIcon;
	public UIButton Shadow;
	public _UI_CS_InvItemTip tip;
	public ItemPosOffestType m_m_offestPos;
	private Vector3 mousePos;
	private bool isShowItemTips = true;
	
	// Use this for initialization
	void Start () {
		m_rect.width  = 1;
		m_rect.height = 1;
		if(!m_IsNotMove){
			if(BG != null) {
				BG.AddInputDelegate(ItemDelegate);
			}
			if(m_MyIconBtn != null) {
				m_MyIconBtn.AddInputDelegate(ItemDelegate);
			}
		}
		HighLightBG.gameObject.layer = LayerMask.NameToLayer("EZGUI_CanTouch");
		HighLightBG.Hide(true);
		StartCoroutine(InitObjectLayer());
	}
	
	public IEnumerator InitObjectLayer(){	
		yield return null;
		if(highLevel != null){
			highLevel.gameObject.layer = LayerMask.NameToLayer("EZGUI_CanTouch");
			highLevel.Hide(true);
		}
	}

	// Update is called once per frame
	void Update () {
		if(m_IsOperate){
			UpdateMyIconPosition();	
		}
//		if(Input.GetKey(KeyCode.Mouse1)){
//			MouseRightPress();
//		}
		if(IsChang != m_IsEmpty){
			IsChang = m_IsEmpty;
			if(m_IsEmpty){
				m_MyIconBtn.Hide(true);
				ClonIcon.Hide(true);
			}else{
				m_MyIconBtn.Hide(false);
				ClonIcon.Hide(false);
				if(m_ItemInfo.count > 1){
					m_MyCountText.Text =  m_ItemInfo.count.ToString();
				}else{
					m_MyCountText.Text = "";
				}
			}
		}
	}
#region Interface
	public void SetItemTypeID(int id){
		m_ItemTypeID = id;
	}
	
	public void CancelPress(){
		HideALLHighLight();
		CancelALLOp();
		_UI_CS_IngameMenu.Instance.IsMustPTap  = false;
//		_UI_CS_ToolsTip.Instance.DismissToolTips();	
	}
	
	public void ItemBtnTapPress(){	
		switch(Inventory.Instance.preBagItmeIndex){
				case 1:					
					Inventory.Instance.equipmentArray[Inventory.Instance.preSlotItmeIndex].HighLightBG.Hide(true);
					Inventory.Instance.equipmentArray[Inventory.Instance.preSlotItmeIndex].m_IsOperate = false;					
					HighLightBG.Hide(true);
					m_IsOperate = false;					
					break;
				case 2:
					Inventory.Instance.bagItemArray[Inventory.Instance.preSlotItmeIndex].HighLightBG.Hide(true);
					Inventory.Instance.bagItemArray[Inventory.Instance.preSlotItmeIndex].m_IsOperate = false;	
					HighLightBG.Hide(true);
					m_IsOperate = false;
					break;
				}
	}
	
	public void UpdateGroupElementPosition(){
		for(int i = 0;i<m_GroupCount;i++){
			for(int j = 0;j<m_Group[i].m_GroupItemsCount;j++){
				m_Group[i].m_InventoryGroup[j].SetInViewPosition();
			}
		}
		Vector3 pos =  _UI_CS_Ctrl.Instance.m_UI_Camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x   , Input.mousePosition.y ,_UI_CS_Ctrl.Instance.m_UI_Camera.nearClipPlane));
		m_offestPos.x = m_MyIconBtn.transform.position.x - pos.x;
		m_offestPos.y = m_MyIconBtn.transform.position.y - pos.y;
	}
	
	//set icon pic <equip>
	public void SetIconTexture(Texture2D texture){
		if(null != texture){
			m_MyIconBtn.SetUVs(m_rect);
			m_MyIconBtn.SetTexture(texture);	
		}else{
			m_MyIconBtn.SetUVs(m_rect);
			m_MyIconBtn.SetTexture(_UI_CS_Resource.Instance.m_EquipmentIcon[0]);
		}
	}
	
	//set icon pic <equip>
	public void SetClonIconTexture(Texture2D texture){
		if(null != texture){
			ClonIcon.SetUVs(m_rect);
			ClonIcon.SetTexture(texture);	
		}else{
			ClonIcon.SetUVs(m_rect);
			ClonIcon.SetTexture(_UI_CS_Resource.Instance.m_EquipmentIcon[0]);
		}
	}

	public void UpdateItemHighLevel(){
		if(!highLevel)
			return;
		if(!m_IsEmpty){
			if(ItemStruct.info_Level <= _PlayerData.Instance.playerLevel){
				highLevel.Hide(true);
			}else{
				highLevel.Hide(false);
			}
		}else{
			highLevel.Hide(true);
		}
	}
	
	/// <summary>
	/// Clears the item info.
	/// </summary>
	public void ClearItemInfo() {
		transform.GetComponent<_UI_CS_InventoryItem>().setIsEmpty(true);
		transform.GetComponent<_UI_CS_InventoryItem>().m_ItemInfo = null;
		transform.GetComponent<_UI_CS_InventoryItem>().UpdateItemHighLevel();
		transform.GetComponent<_UI_CS_InventoryItem>().SetIconTexture(null);
		transform.GetComponent<_UI_CS_InventoryItem>().SetClonIconTexture(null);
	}
	
	public void UpdateCountText() {
		if(m_IsEmpty) {
			m_MyCountText.Text = "";
		}else {
			if(ItemStruct._TypeID > 8 ) {
				if(m_ItemInfo.count > 1) {
					m_MyCountText.Text = m_ItemInfo.count.ToString();
				}else {
					m_MyCountText.Text = "";
				}
			}else {
				m_MyCountText.Text = "";
			}
		}
	}
#endregion
	
#region Local
	//设置主视口位置 
	private void SetInViewPosition(){
//		m_StartPosition	= m_MyIconBtn.transform.position;
		m_StartPosition	= new Vector3(ClonIcon.transform.position.x,ClonIcon.transform.position.y,ClonIcon.transform.position.z-0.1f);
	}
	
	private IEnumerator UpdateItemPos() {
		yield return null;
		UpdateMyIconPosition();
	}
	
	void ItemDelegate(ref POINTER_INFO ptr) {
		//锁控制,防止再没收到服务器返回消息时的其他操作//
		if(_UI_CS_IngameMenu.Instance.isLockInvLogic)
			return;
		switch(ptr.evt){
			//当按下时//
			case POINTER_INFO.INPUT_EVENT.PRESS:
				{
					//记录本次操作的物品位置//
					Inventory.Instance.preBagItmeIndex  = m_TypeID; 
					Inventory.Instance.preSlotItmeIndex = m_Slot;
					isShowItemTips = false;
					//判断是否是双击//
					if(_UI_CS_KeyboardCtrl.Instance.IsDoubleInput()){
						if(_UI_CS_IngameMenu.Instance.IsMustPTap){
							CancelPress();
						}
						//判断是否在背包界面//
						if(_UI_CS_ScreenCtrl.Instance.currentScreenType == _UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_INGAME_MENU_INV){	
							DoubleCLogic();	
						}
					}else{	
						if(_UI_CS_IngameMenu.Instance.IsMustPTap){
							CancelPress();
						}
						//这个格子的物品当前有没有被操作 (我也忘了是为什么用的，反正不判断会有什么不对)//
						if(!m_IsOperate){
							if(!m_IsEmpty){
								HighLightBG.Hide(false);
								HighLightBG.gameObject.layer = LayerMask.NameToLayer("EZGUI_CanTouch");
								m_IsOperate = true;;
								UpdateGroupElementPosition();
								ShowHightLightTarget(true);
							}	
						}
					}
				}	
				break;
			//当拖拽时//
		   case POINTER_INFO.INPUT_EVENT.DRAG:
				//隐藏一些TPS//
				_ItemTips.Instance.DismissItemTip();
				ItemEquipTips.Instance.DismissCompareTips();
				_ItemTips.Instance.ItemTipFadeOut();
				break;
			//当进入区域或没移动式//
		   case POINTER_INFO.INPUT_EVENT.MOVE:	
		   case POINTER_INFO.INPUT_EVENT.NO_CHANGE:
			
					if(!isShowItemTips){
						return;
					}	
			
					if(ptr.evt == POINTER_INFO.INPUT_EVENT.NO_CHANGE) {
						_ItemTips.Instance.isShowItemTip = true;
					}
					
					//判断这个物品是否为空//
					if(!m_IsEmpty){
						mousePos = UIManager.instance.uiCameras[0].camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y,UIManager.instance.uiCameras[0].camera.nearClipPlane + 1));
						//判断是否为在普通商店//
						if(_UI_CS_ScreenCtrl.Instance.IsScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_ITEM_VENDOR)){
							if(_ItemTips.Instance.isShowItemTip) {
								_ItemTips.Instance.UpdateToolsTipInfo(ItemStruct,_ItemTips.Instance.GetItemValue(ItemValueType.SALE,ItemStruct.info_Level,ItemStruct.info_eleVal,ItemStruct.info_encVal,ItemStruct._GemEffectVal,ItemStruct._ItemVal),ItemRightClickType.SALE,m_m_offestPos,
									new Vector3(mousePos.x,mousePos.y,m_MyIconBtn.transform.position.z),m_MyIconBtn.width,m_MyIconBtn.height);
								ItemEquipTips.Instance.ShowCompareTips(ItemStruct,false);
							}
							//右键逻辑 (如果你这个也支持右键！)//
							RCLogic();
						}else{
							//判断是否为销毁状态//
							if(_UI_CS_IngameMenu.Instance.isTransmute){
								//判断背包类型//
								if(_ItemTips.Instance.isShowItemTip) {
									if(1 == m_TypeID){
										_ItemTips.Instance.UpdateToolsTipInfo(ItemStruct,_ItemTips.Instance.GetItemValue(ItemValueType.TRANSMUTE,ItemStruct.info_Level,ItemStruct.info_eleVal,ItemStruct.info_encVal,ItemStruct._GemEffectVal,ItemStruct._ItemVal),ItemRightClickType.UNEQUIP,m_m_offestPos,
												new Vector3(mousePos.x,mousePos.y,m_MyIconBtn.transform.position.z),m_MyIconBtn.width,m_MyIconBtn.height);
										ItemEquipTips.Instance.ShowCompareTips(ItemStruct,true);
									}else{
										_ItemTips.Instance.UpdateToolsTipInfo(ItemStruct,_ItemTips.Instance.GetItemValue(ItemValueType.TRANSMUTE,ItemStruct.info_Level,ItemStruct.info_eleVal,ItemStruct.info_encVal,ItemStruct._GemEffectVal,ItemStruct._ItemVal),ItemRightClickType.TRANSMUTE,m_m_offestPos,
												new Vector3(mousePos.x,mousePos.y,m_MyIconBtn.transform.position.z),m_MyIconBtn.width,m_MyIconBtn.height);
										ItemEquipTips.Instance.ShowCompareTips(ItemStruct,false);
									}
								}
								//右键逻辑//
								RCLogic();
							}else{
								//判断背包类型//
								if(_ItemTips.Instance.isShowItemTip) {
								if(1 == m_TypeID){
										_ItemTips.Instance.UpdateToolsTipInfo(ItemStruct,_ItemTips.Instance.GetItemValue(ItemValueType.SALE,ItemStruct.info_Level,ItemStruct.info_eleVal,ItemStruct.info_encVal,ItemStruct._GemEffectVal,ItemStruct._ItemVal),ItemRightClickType.UNEQUIP,m_m_offestPos,
											new Vector3(mousePos.x,mousePos.y,m_MyIconBtn.transform.position.z),m_MyIconBtn.width,m_MyIconBtn.height);
										ItemEquipTips.Instance.ShowCompareTips(ItemStruct,true);
									}else{
										_ItemTips.Instance.UpdateToolsTipInfo(ItemStruct,_ItemTips.Instance.GetItemValue(ItemValueType.SALE,ItemStruct.info_Level,ItemStruct.info_eleVal,ItemStruct.info_encVal,ItemStruct._GemEffectVal,ItemStruct._ItemVal),ItemRightClickType.EQUIP,m_m_offestPos,
											new Vector3(mousePos.x,mousePos.y,m_MyIconBtn.transform.position.z),m_MyIconBtn.width,m_MyIconBtn.height);
										ItemEquipTips.Instance.ShowCompareTips(ItemStruct,false);
									}
								}
								//右键逻辑//
								RCLogic();
							}
						}
					}
				break;
			//当离开区域//
		   case POINTER_INFO.INPUT_EVENT.MOVE_OFF:
					_ItemTips.Instance.DismissItemTip();
					_ItemTips.Instance.ItemTipFadeOut();
				break;
			//当快速按下抬起时//
		case POINTER_INFO.INPUT_EVENT.TAP:
			SoundCue.PlayPrefabAndDestroy(_UI_CS_IngameMenu.Instance.pickUpnSound);
			isShowItemTips = true;
			if(!m_IsEmpty){
				ShowHightLightTarget(false);
				m_MyIconBtn.transform.position = m_StartPosition;
				if(_UI_CS_IngameMenu.Instance.IsMustPTap){
					_UI_CS_IngameMenu.Instance.IsMustPTap = false;
					HideALLHighLight();
				}else{
						m_IsOperate = false;	
						HighLightBG.Hide(false);
						Inventory.Instance.preBagItmeIndex  = m_TypeID; 
						Inventory.Instance.preSlotItmeIndex = m_Slot;
						_UI_CS_IngameMenu.Instance.IsMustPTap = true;
				}
			}
			break;
			//当抬起时(跟TAP状态有区别)
		case POINTER_INFO.INPUT_EVENT.RELEASE_OFF:
		case POINTER_INFO.INPUT_EVENT.RELEASE:
			_UI_CS_IngameMenu.Instance.PlayDropSound(ItemStruct._TypeID);
			isShowItemTips = true;
			if(_UI_CS_IngameMenu.Instance.IsMustPTap){
				_UI_CS_IngameMenu.Instance.IsMustPTap = false;
			}else{
				if(m_IsOperate){
					if(!m_IsEmpty){
						Inventory.Instance.preBagItmeIndex  = m_TypeID; 
						Inventory.Instance.preSlotItmeIndex = m_Slot;
						
						HighLightBG.Hide(true);
						_UI_CS_IngameMenu.Instance.BagEquipShadow(0,false);
						m_IsOperate = false;	
						IsMove(m_MyIconBtn);
					}
				}
			}	
			HideALLHighLight();
			break;
		   default:
				break;
		}	
	}
	
	private void ShowHightLightTarget(bool isShow){
		//LogManager.Log_Error("hight light ShowHightLightTarget !");
		ItemDropStruct tempItem = ItemDeployInfo.Instance.GetItemObject(m_ItemInfo.ID,m_ItemInfo.perfrab,m_ItemInfo.gem,m_ItemInfo.enchant,m_ItemInfo.element,(int)m_ItemInfo.level);
		if(null == tempItem)
			return;	
		int idx = 0;
		switch(tempItem._TypeID){
			case 1:
				if(m_TypeID == 1){	
					idx = _UI_CS_IngameMenu.Instance.EmptyBagIndex();
					if(-1 != idx){
						if(isShow){
							Inventory.Instance.bagItemArray[idx].HighLightBG.Hide(false);		
						}else{	
							Inventory.Instance.bagItemArray[idx].HighLightBG.Hide(true);
						}
						Inventory.Instance.bagItemArray[idx].HighLightBG.gameObject.layer = LayerMask.NameToLayer("EZGUI_CanTouch");
					}
				}else{
					idx = 0;
					if(isShow){
						Inventory.Instance.equipmentArray[idx].HighLightBG.Hide(false);
						_UI_CS_IngameMenu.Instance.BagEquipShadow(idx,true);
					}else{	
						Inventory.Instance.equipmentArray[idx].HighLightBG.Hide(true);
						_UI_CS_IngameMenu.Instance.BagEquipShadow(tempItem._TypeID,false);
					}
					Inventory.Instance.equipmentArray[idx].HighLightBG.gameObject.layer = LayerMask.NameToLayer("EZGUI_CanTouch");
				}
				break;
			case 3:
				if(m_TypeID == 1){	
					idx = _UI_CS_IngameMenu.Instance.EmptyBagIndex();
					if(-1 != idx){	
						if(isShow){
							Inventory.Instance.bagItemArray[idx].HighLightBG.Hide(false);
						}else{	
							Inventory.Instance.bagItemArray[idx].HighLightBG.Hide(true);
						}
						Inventory.Instance.bagItemArray[idx].HighLightBG.gameObject.layer = LayerMask.NameToLayer("EZGUI_CanTouch");
					}
				}else{
					idx = 2;
					if(isShow){
						Inventory.Instance.equipmentArray[idx].HighLightBG.Hide(false);
						_UI_CS_IngameMenu.Instance.BagEquipShadow(idx,true);
					}else{	
						Inventory.Instance.equipmentArray[idx].HighLightBG.Hide(true);
						_UI_CS_IngameMenu.Instance.BagEquipShadow(tempItem._TypeID,false);
					}				
					Inventory.Instance.equipmentArray[idx].HighLightBG.gameObject.layer = LayerMask.NameToLayer("EZGUI_CanTouch");
				}
				break;
			case 6:	
				if(m_TypeID == 1){	
					idx = _UI_CS_IngameMenu.Instance.EmptyBagIndex();
					if(-1 != idx){	
						if(isShow){
							Inventory.Instance.bagItemArray[idx].HighLightBG.Hide(false);
						}else{	
							Inventory.Instance.bagItemArray[idx].HighLightBG.Hide(true);
						}
						Inventory.Instance.bagItemArray[idx].HighLightBG.gameObject.layer = LayerMask.NameToLayer("EZGUI_CanTouch");
					}
				}else{
					idx = 8;
					if(isShow){
						Inventory.Instance.equipmentArray[idx].HighLightBG.Hide(false);
						_UI_CS_IngameMenu.Instance.BagEquipShadow(idx,true);
					}else{	
						Inventory.Instance.equipmentArray[idx].HighLightBG.Hide(true);
						_UI_CS_IngameMenu.Instance.BagEquipShadow(tempItem._TypeID,false);
					}
					Inventory.Instance.equipmentArray[idx].HighLightBG.gameObject.layer = LayerMask.NameToLayer("EZGUI_CanTouch");
				}
				break;
			case 2:	
				if(m_TypeID == 1){
					idx = _UI_CS_IngameMenu.Instance.EmptyBagIndex();
					if(-1 != idx){
						if(isShow){
							Inventory.Instance.bagItemArray[idx].HighLightBG.Hide(false);
						}else{	
							Inventory.Instance.bagItemArray[idx].HighLightBG.Hide(true);
						}
						Inventory.Instance.bagItemArray[idx].HighLightBG.gameObject.layer = LayerMask.NameToLayer("EZGUI_CanTouch");
					}
				}else{
					idx = 1;
					if(isShow){
						Inventory.Instance.equipmentArray[idx].HighLightBG.Hide(false);
						_UI_CS_IngameMenu.Instance.BagEquipShadow(idx,true);
					}else{	
						Inventory.Instance.equipmentArray[idx].HighLightBG.Hide(true);
						_UI_CS_IngameMenu.Instance.BagEquipShadow(tempItem._TypeID,false);
					}
					Inventory.Instance.equipmentArray[idx].HighLightBG.gameObject.layer = LayerMask.NameToLayer("EZGUI_CanTouch");
				}
				break;			
			case 5:				
				if(m_TypeID == 1){					
					idx = _UI_CS_IngameMenu.Instance.EmptyBagIndex();
					if(-1 != idx){						
						if(isShow){
							Inventory.Instance.bagItemArray[idx].HighLightBG.Hide(false);
						}else{	
							Inventory.Instance.bagItemArray[idx].HighLightBG.Hide(true);
						}
						Inventory.Instance.bagItemArray[idx].HighLightBG.gameObject.layer = LayerMask.NameToLayer("EZGUI_CanTouch");
					}				
				}else{				
					idx = 4;
					if(isShow){
						Inventory.Instance.equipmentArray[idx].HighLightBG.Hide(false);
						_UI_CS_IngameMenu.Instance.BagEquipShadow(idx,true);
					}else{	
						Inventory.Instance.equipmentArray[idx].HighLightBG.Hide(true);
						_UI_CS_IngameMenu.Instance.BagEquipShadow(tempItem._TypeID,false);
					}
					Inventory.Instance.equipmentArray[idx].HighLightBG.gameObject.layer = LayerMask.NameToLayer("EZGUI_CanTouch");
				}
				break;
			case 4:					
				if(m_TypeID == 1){					
					idx = _UI_CS_IngameMenu.Instance.EmptyBagIndex();
					if(-1 != idx){						
						if(isShow){
							Inventory.Instance.bagItemArray[idx].HighLightBG.Hide(false);
						}else{	
							Inventory.Instance.bagItemArray[idx].HighLightBG.Hide(true);
						}
						Inventory.Instance.bagItemArray[idx].HighLightBG.gameObject.layer = LayerMask.NameToLayer("EZGUI_CanTouch");
					}				
				}else{				
					idx = 3;
					if(isShow){
						Inventory.Instance.equipmentArray[idx].HighLightBG.Hide(false);
						_UI_CS_IngameMenu.Instance.BagEquipShadow(idx,true);
					}else{	
						Inventory.Instance.equipmentArray[idx].HighLightBG.Hide(true);
						_UI_CS_IngameMenu.Instance.BagEquipShadow(tempItem._TypeID,false);
					}
					Inventory.Instance.equipmentArray[idx].HighLightBG.gameObject.layer = LayerMask.NameToLayer("EZGUI_CanTouch");
				}
				break;			
			case 7:
			case 8:				
				if(m_TypeID == 1){					
					idx = _UI_CS_IngameMenu.Instance.EmptyBagIndex();
					if(-1 != idx){						
						if(isShow){
							Inventory.Instance.bagItemArray[idx].HighLightBG.Hide(false);
						}else{	
							Inventory.Instance.bagItemArray[idx].HighLightBG.Hide(true);
						}
						Inventory.Instance.bagItemArray[idx].HighLightBG.gameObject.layer = LayerMask.NameToLayer("EZGUI_CanTouch");
					}				
				}else{				
					idx = 6;
					if(isShow){
						Inventory.Instance.equipmentArray[idx].HighLightBG.Hide(false);
//						if(tempItem._TypeID == 7)
//							idx = 7;
						_UI_CS_IngameMenu.Instance.BagEquipShadow(idx,true);
					}else{	
						Inventory.Instance.equipmentArray[idx].HighLightBG.Hide(true);
						_UI_CS_IngameMenu.Instance.BagEquipShadow(tempItem._TypeID,false);
					}
					Inventory.Instance.equipmentArray[idx].HighLightBG.gameObject.layer = LayerMask.NameToLayer("EZGUI_CanTouch");
				}
				break;
		default:
			break;
		}		
	}
	
	private void HideALLHighLight(){
		//LogManager.Log_Error("HideALLHighLight");
		for(int i = 0;i<m_GroupCount;i++){
			for(int j = 0;j<m_Group[i].m_GroupItemsCount;j++){
				m_Group[i].m_InventoryGroup[j].HighLightBG.Hide(true);
			}
		}
	}
	
	private void CancelALLOp(){
		for(int i = 0;i<m_GroupCount;i++){
			for(int j = 0;j<m_Group[i].m_GroupItemsCount;j++){
				m_Group[i].m_InventoryGroup[j].m_IsOperate = false;
			}
		}
	}
	
	private void CancelALLMPT(){
		for(int i = 0;i<m_GroupCount;i++){
			for(int j = 0;j<m_Group[i].m_GroupItemsCount;j++){
				m_Group[i].m_InventoryGroup[j].m_IsOperate = false;
			}
		}
	}
	
	private void PopUpTransmute(){
		float itemVal = (ItemStruct.info_gemVal + ItemStruct.info_encVal + ItemStruct.info_eleVal);
		Inventory.Instance.transmuteType = m_TypeID;
		Inventory.Instance.transmuteSlot = m_Slot;
		if(itemVal >= _UI_CS_ItemVendor.Instance.greenVal){
			_UI_CS_ItemVendor.Instance.SetColorForName(Inventory.Instance.transmuteNameText,ItemStruct);
			Inventory.Instance.transmutePanel.BringIn();
		}else{
			Inventory.Instance.SendTransmuteMsg();
		}
		_ItemTips.Instance.DismissItemTip();
	}
	
	private void IsMove(UIButton btn){
		int index;
		int emptyBagSlotIndex;
//		UpdateGroupElementPosition();
		index  = IsMoveB(btn);
		if(-1 != index){
			// e --> b
			if(1 == m_TypeID){
				UnequipItemLogic(false,index);
				return;
			}
			// b --> b
			else if(2 == m_TypeID){
				// ... ...
				LogManager.Log_Debug("--- SwapItem ---");
				CS_Main.Instance.g_commModule.SendMessage(
			   		ProtocolGame_SendRequest.SwapItem((byte)m_TypeID,(uint)m_Slot,(byte)2, (uint)index)
				);
				_UI_CS_IngameMenu.Instance.isLockInvLogic = true;
				return;
			}
			// s --> b
			else if(4 == m_TypeID){
				LogManager.Log_Debug("--- stash to bag ---");
				CS_Main.Instance.g_commModule.SendMessage(
			   		ProtocolGame_SendRequest.SwapItem((byte)4,(uint)Stash.Instance.GetCurSoltIdx(m_Slot),(byte)2, (uint)index)
				);
				_UI_CS_IngameMenu.Instance.isLockInvLogic = true;
				return;
			}
		}
		
		index  = IsMoveE(btn);
		if(-1 != index){
			// e --> e
			if(1 == m_TypeID){
				// ... ...
				//return;
			}
			// b --> e
			else if(2 == m_TypeID){ 
//				if(6 == index || 7 == index){
//					ItemDropStruct tempItem = ItemDeployInfo.Instance.GetItemObject(m_ItemInfo.ID,m_ItemInfo.perfrab,m_ItemInfo.gem,m_ItemInfo.enchant,m_ItemInfo.element,(int)m_ItemInfo.level);
//					int indexEx = 0;
//					if("2h" == tempItem.info_hand){
//						if(Inventory.Instance.equipmentArray[7].m_IsEmpty){
//							indexEx = 6;
//						}else{
//							indexEx = _UI_CS_IngameMenu.Instance.EmptyBagIndex();
//							if(-1 != indexEx){
//								LogManager.Log_Debug("--- UnequipItem ---");
//								CS_Main.Instance.g_commModule.SendMessage(
//							   		ProtocolGame_SendRequest.UnequipItem((byte)7,(byte)2,(uint)indexEx)
//								);
//								_UI_CS_IngameMenu.Instance.isLockInvLogic = true;
//							}else{
//						
//								LogManager.Log_Debug("--- bag find err ---");
//							}
//							indexEx = 6;
//						}
//					}else{
//						if(Inventory.Instance.equipmentArray[6].m_IsEmpty){
//							indexEx = 6;
//						}else{
//							ItemDropStruct temp2Item = ItemDeployInfo.Instance.GetItemObject(Inventory.Instance.equipmentArray[6].m_ItemInfo.ID,
//					                                                                 Inventory.Instance.equipmentArray[6].m_ItemInfo.perfrab,
//					                                                                 Inventory.Instance.equipmentArray[6].m_ItemInfo.gem,
//					                                                                 Inventory.Instance.equipmentArray[6].m_ItemInfo.enchant,
//					                                                                 Inventory.Instance.equipmentArray[6].m_ItemInfo.element,
//					                                                                 (int)Inventory.Instance.equipmentArray[6].m_ItemInfo.level);
//							if("2h" == temp2Item.info_hand){
//								indexEx = 6;
//							}else{
//								indexEx = 7;
//							}
//						}
//					}
//					LogManager.Log_Debug("--- EquipItem ---");
//					CS_Main.Instance.g_commModule.SendMessage( 
//				   		ProtocolGame_SendRequest.EquipItem((byte)indexEx,(byte)2,(uint)m_Slot)
//					);
//					_UI_CS_IngameMenu.Instance.isLockInvLogic = true;
//				}else{
//					LogManager.Log_Debug("--- EquipItem ---");
//					CS_Main.Instance.g_commModule.SendMessage( 
//				   		ProtocolGame_SendRequest.EquipItem((byte)index,(byte)2,(uint)m_Slot)
//					);
//					_UI_CS_IngameMenu.Instance.isLockInvLogic = true;
//				}
				EquipItemLogic(index);
				return;
			}
			// s --> e
			else if(2 == m_TypeID){
				// ... ...
				//return;
			}
		}

		index  = IsMoveM(btn);
		if(-1 != index){
			if(2 == m_TypeID){
				//transmute
				if(2 == index){
					PopUpTransmute();
//					LogManager.Log_Debug("--- DelItem ---");
//					CS_Main.Instance.g_commModule.SendMessage(
//				   		ProtocolGame_SendRequest.DelItem((byte)m_TypeID,(uint)m_Slot)
//					);
					_UI_CS_IngameMenu.Instance.isLockInvLogic = true;
					return;
					//bag tab
				}else if(0 == index){
					emptyBagSlotIndex = _UI_CS_IngameMenu.Instance.EmptyBagIndex(1);
					if(-1 != emptyBagSlotIndex){
						LogManager.Log_Debug("--- B1->B2 SwapItem ---");
						CS_Main.Instance.g_commModule.SendMessage(
			   			ProtocolGame_SendRequest.SwapItem((byte)2,(uint)m_Slot,(byte)2, (uint)emptyBagSlotIndex)
						);
						_UI_CS_IngameMenu.Instance.isLockInvLogic = true;
					}else{
						m_MyIconBtn.transform.position = m_StartPosition;
//						TouchBG.transform.position = m_StartPosition;
					}
					return;
					// bag tab
				}else if(1 == index){
					emptyBagSlotIndex = _UI_CS_IngameMenu.Instance.EmptyBagIndex(2);
					if(-1 != emptyBagSlotIndex){
						LogManager.Log_Debug("--- B1->B2 SwapItem ---");
						CS_Main.Instance.g_commModule.SendMessage(
			   			ProtocolGame_SendRequest.SwapItem((byte)2,(uint)m_Slot,(byte)2, (uint)emptyBagSlotIndex)
						);
						_UI_CS_IngameMenu.Instance.isLockInvLogic = true;
					}else{
						m_MyIconBtn.transform.position = m_StartPosition;
//						TouchBG.transform.position = m_StartPosition;
					}
					return;
					//crafting
				}else if(3 == index) {
					if(ItemStruct._TypeID < 9) {
						//更新图标信息
						CraftingWeaponPanel.Instance.craftingItem.setIsEmpty(false);
						CraftingWeaponPanel.Instance.craftingItem.m_ItemInfo = m_ItemInfo;
						CraftingWeaponPanel.Instance.craftingItem.ItemStruct = ItemStruct;
						CraftingWeaponPanel.Instance.craftingItem.UpdateItemHighLevel();
						//slot
						CraftingWeaponPanel.Instance.SetSourceItemSlot(m_Slot);
						CraftingWeaponPanel.Instance.craftingItem.m_Slot = m_Slot;
						ItemPrefabs.Instance.GetItemIcon(ItemStruct._ItemID,ItemStruct._TypeID,ItemStruct._PrefabID,CraftingWeaponPanel.Instance.craftingItem.m_MyIconBtn);
						ItemPrefabs.Instance.GetItemIcon(ItemStruct._ItemID,ItemStruct._TypeID,ItemStruct._PrefabID,CraftingWeaponPanel.Instance.craftingItem.ClonIcon);
						//通知更新物品//
						CraftingWeaponPanel.Instance.ClearBtnState();
						CraftingWeaponPanel.Instance.UpdateItemInfo();
						//还原图标坐标//
						m_MyIconBtn.transform.position = m_StartPosition;
					}
				}	
			}
			// s --> m
			else if(2 == m_TypeID){
				// ... ...
				//return;
			}
		}
		
		index  = IsMoveStash(btn);
		if(-1 != index){
			//b->s//
			if(2 == m_TypeID) {
				LogManager.Log_Debug("--- move to stash ---");
				CS_Main.Instance.g_commModule.SendMessage(
			   		ProtocolGame_SendRequest.SwapItem((byte)m_TypeID,(uint)m_Slot,(byte)4, (uint)Stash.Instance.GetCurSoltIdx(index))
				);
				_UI_CS_IngameMenu.Instance.isLockInvLogic = true;
				return;	
			}
			//s->s//
			else if(4 == m_TypeID) {
				LogManager.Log_Debug("--- stash to stash ---");
				CS_Main.Instance.g_commModule.SendMessage(
			   		ProtocolGame_SendRequest.SwapItem((byte)m_TypeID,(uint)Stash.Instance.GetCurSoltIdx(m_Slot),(byte)4, (uint)Stash.Instance.GetCurSoltIdx(index))
				);
				_UI_CS_IngameMenu.Instance.isLockInvLogic = true;
				return;	
			}
		}
		
		m_MyIconBtn.transform.position = m_StartPosition;
	}

	private void RCLogic(){
		if(!m_IsEmpty){
			
			#if  UNITY_STANDALONE_OSX
			if (Input.GetKey(KeyCode.LeftControl)||Input.GetKey(KeyCode.RightControl)){
				if(Input.GetMouseButtonDown(0)){
					RightClickFun();
					return;
				}
	     	 }
			#endif
			
			if(Input.GetButtonDown("Fire2")){
				RightClickFun();
				return;
			}
		}
	}
	
	private void RightClickFun() {
		UpdateGroupElementPosition();
		if( _UI_CS_ScreenCtrl.Instance.IsScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_INGAME_MENU_INV)){
			if(_UI_CS_IngameMenu.Instance.isTransmute){
				if(1 == m_TypeID){
					EquipOrUnequipItemLogic();
				}else{
					PopUpTransmute();
				}
				_UI_CS_IngameMenu.Instance.isLockInvLogic = true;
			}else{
				if(ItemStruct._TypeID > 8){
					//非装备 消耗品
					if(UseConsumableLogic()){
					}
				}else{
					//装备
					EquipOrUnequipItemLogic();
				}
			}
		}else if( _UI_CS_ScreenCtrl.Instance.IsScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_ITEM_VENDOR)){
			SoundCue.PlayPrefabAndDestroy(_UI_CS_ItemVendor.Instance.SellSound);
			LogManager.Log_Debug("--- SellItem ---");
			CS_Main.Instance.g_commModule.SendMessage(
   				ProtocolGame_SendRequest.SaleItem((byte)m_TypeID,(uint)m_Slot)
			);
		}
	}
	
	private bool UseConsumableLogic(){
		if(14 == ItemStruct._TypeID){
			if(4 == ItemStruct._SecTypeID){
				//play sound food.
				SoundCue.PlayPrefabAndDestroy(_UI_CS_ElementsInfo.Instance.consumableFoodSound);
				CS_Main.Instance.g_commModule.SendMessage(
					   	ProtocolGame_SendRequest.useItem((byte)m_TypeID,(uint)m_Slot)
				);
			}else if(5 == ItemStruct._SecTypeID){
				//play sound drink.
				SoundCue.PlayPrefabAndDestroy(_UI_CS_ElementsInfo.Instance.consumableDrinkSound);
				CS_Main.Instance.g_commModule.SendMessage(
					   	ProtocolGame_SendRequest.useItem((byte)m_TypeID,(uint)m_Slot)
				);
			}
			return true;
		}
		return false;
	}
	
	private void EquipOrUnequipItemLogic(){
		if(1 == m_TypeID){
			UnequipItemLogic(true,0);
		}else{
			ItemDropStruct tempItem = ItemDeployInfo.Instance.GetItemObject(m_ItemInfo.ID,m_ItemInfo.perfrab,m_ItemInfo.gem,m_ItemInfo.enchant,m_ItemInfo.element,(int)m_ItemInfo.level);
			int idx = GetEquipIdx(tempItem);
			if(idx < 9){
				EquipItemLogic(idx);
			}else{
				//消耗品
			}
		}
	}
	
	
	private void EquipItemLogic(int index){
		ItemDropStruct tempItem = ItemDeployInfo.Instance.GetItemObject(m_ItemInfo.ID,m_ItemInfo.perfrab,m_ItemInfo.gem,m_ItemInfo.enchant,m_ItemInfo.element,(int)m_ItemInfo.level);
		if(tempItem.info_Level > _PlayerData.Instance.playerLevel){
			IngamePopMsg.Instance.AwakeIngameMsg("Item level too high");
			m_MyIconBtn.transform.position = m_StartPosition;
			SoundCue.PlayPrefabAndDestroy(_UI_CS_PopupBoxCtrl.Instance.failSound);
			return;
		}
		if(6 == index || 7 == index){	
			int indexEx = 0;
			if(8 == tempItem._TypeID){
				if(Inventory.Instance.equipmentArray[7].m_IsEmpty){
					indexEx = 6;
				}else{
					indexEx = _UI_CS_IngameMenu.Instance.EmptyBagIndex();
					if(-1 != indexEx){
						LogManager.Log_Debug("--- UnequipItem ---");
						CS_Main.Instance.g_commModule.SendMessage(
					   		ProtocolGame_SendRequest.UnequipItem((byte)7,(byte)2,(uint)indexEx)
						);
						_UI_CS_IngameMenu.Instance.isLockInvLogic = true;
					}else{
						LogManager.Log_Debug("--- bag find err ---");
						IngamePopMsg.Instance.AwakeIngameMsg("Sorry, Bag lack of space.");
						m_MyIconBtn.transform.position = m_StartPosition;
						return;
					}
					indexEx = 6;
				}
			}else{
				if(Inventory.Instance.equipmentArray[6].m_IsEmpty){
					indexEx = 6;
				}else{
					ItemDropStruct temp2Item = ItemDeployInfo.Instance.GetItemObject(Inventory.Instance.equipmentArray[6].m_ItemInfo.ID,
			                                                                 Inventory.Instance.equipmentArray[6].m_ItemInfo.perfrab,
			                                                                 Inventory.Instance.equipmentArray[6].m_ItemInfo.gem,
			                                                                 Inventory.Instance.equipmentArray[6].m_ItemInfo.enchant,
			                                                                 Inventory.Instance.equipmentArray[6].m_ItemInfo.element,
			                                                                 (int)Inventory.Instance.equipmentArray[6].m_ItemInfo.level);
					if(8 == temp2Item._TypeID){
						indexEx = 6;
					}else{
						indexEx = index;
					}
				}
			}
			LogManager.Log_Debug("--- EquipItem ---");
			CS_Main.Instance.g_commModule.SendMessage( 
		   		ProtocolGame_SendRequest.EquipItem((byte)indexEx,(byte)2,(uint)m_Slot)
			);
			_UI_CS_IngameMenu.Instance.isLockInvLogic = true;
		}else{
			LogManager.Log_Debug("--- EquipItem ---");
			CS_Main.Instance.g_commModule.SendMessage( 
		   		ProtocolGame_SendRequest.EquipItem((byte)index,(byte)2,(uint)m_Slot)
			);
			_UI_CS_IngameMenu.Instance.isLockInvLogic = true;
		}
	}
	
	private void UnequipItemLogic(bool isFastLogic,int idx){
		int flag = _UI_CS_IngameMenu.Instance.EmptyBagIndex();
		int flag2 = 0;
		if(-1 != flag){
			ItemDropStruct tempItemEx = ItemDeployInfo.Instance.GetItemObject(m_ItemInfo.ID,m_ItemInfo.perfrab,m_ItemInfo.gem,m_ItemInfo.enchant,m_ItemInfo.element,(int)m_ItemInfo.level);
				if(isFastLogic){
					if(7!= tempItemEx._TypeID && 8!= tempItemEx._TypeID){
						LogManager.Log_Debug("--- UnequipItem ---");
						CS_Main.Instance.g_commModule.SendMessage(
					   		ProtocolGame_SendRequest.UnequipItem((byte)m_Slot,(byte)2,(uint)flag)
						);
						_UI_CS_IngameMenu.Instance.isLockInvLogic = true;
					}else{
						if(Inventory.Instance.equipmentArray[7].m_IsEmpty){
								LogManager.Log_Debug("--- UnequipItem ---");
								CS_Main.Instance.g_commModule.SendMessage(
							   		ProtocolGame_SendRequest.UnequipItem((byte)m_Slot,(byte)2,(uint)flag)
								);
								_UI_CS_IngameMenu.Instance.isLockInvLogic = true;
							}else{
								if(7 == m_Slot){
									LogManager.Log_Debug("--- UnequipItem ---");
									CS_Main.Instance.g_commModule.SendMessage(
								   		ProtocolGame_SendRequest.UnequipItem((byte)7,(byte)2,(uint)flag)
									);
								}else{
									flag2 = _UI_CS_IngameMenu.Instance.EmptyBagIndexSkipIdx(flag);
									if(-1 == flag || -1 == flag2){
										IngamePopMsg.Instance.AwakeIngameMsg("Sorry, Bag lack of space.");
										m_MyIconBtn.transform.position = m_StartPosition;
									}else{
										LogManager.Log_Debug("--- UnequipItem ---");
										CS_Main.Instance.g_commModule.SendMessage(
									   		ProtocolGame_SendRequest.UnequipItem((byte)7,(byte)2,(uint)flag)
										);
										LogManager.Log_Debug("--- UnequipItem ---");
										CS_Main.Instance.g_commModule.SendMessage(
									   		ProtocolGame_SendRequest.UnequipItem((byte)6,(byte)2,(uint)flag2)
										);
										_UI_CS_IngameMenu.Instance.isLockInvLogic = true;
									}
								}
							}
						}
				}else{
					if(7!= tempItemEx._TypeID && 8!= tempItemEx._TypeID){
						LogManager.Log_Debug("--- UnequipItem ---");
						CS_Main.Instance.g_commModule.SendMessage(
					   		ProtocolGame_SendRequest.UnequipItem((byte)m_Slot,(byte)2,(uint)idx)
						);
						_UI_CS_IngameMenu.Instance.isLockInvLogic = true;
					}else{
						if(Inventory.Instance.equipmentArray[7].m_IsEmpty){
								LogManager.Log_Debug("--- UnequipItem ---");
								CS_Main.Instance.g_commModule.SendMessage(
							   		ProtocolGame_SendRequest.UnequipItem((byte)m_Slot,(byte)2,(uint)idx)
								);
								_UI_CS_IngameMenu.Instance.isLockInvLogic = true;
							}else{
								if(7 == m_Slot){
									LogManager.Log_Debug("--- UnequipItem ---");
									CS_Main.Instance.g_commModule.SendMessage(
								   		ProtocolGame_SendRequest.UnequipItem((byte)7,(byte)2,(uint)idx)
									);
								}else{
									LogManager.Log_Debug("--- UnequipItem ---");
									CS_Main.Instance.g_commModule.SendMessage(
								   		ProtocolGame_SendRequest.UnequipItem((byte)7,(byte)2,(uint)flag)
									);
									flag = _UI_CS_IngameMenu.Instance.EmptyBagIndexSkipIdx(flag);
									if(flag != -1){
										LogManager.Log_Debug("--- UnequipItem ---");
										CS_Main.Instance.g_commModule.SendMessage(
									   		ProtocolGame_SendRequest.UnequipItem((byte)6,(byte)2,(uint)flag)
										);
										_UI_CS_IngameMenu.Instance.isLockInvLogic = true;
									}else{
										IngamePopMsg.Instance.AwakeIngameMsg("Sorry, Bag lack of space.");
										m_MyIconBtn.transform.position = m_StartPosition;
									}
								}
							}
						}
					
				}
		}else{
			IngamePopMsg.Instance.AwakeIngameMsg("Sorry, Bag lack of space.");
			m_MyIconBtn.transform.position = m_StartPosition;
		}
	}
	
	private void IsMove2(UIButton btn){
		int index;
		int emptyBagSlotIndex;
//		UpdateGroupElementPosition();
		index  = IsMoveB2(btn);
		if(-1 != index){
			
			// e --> b
			if(1 == btn.GetComponent<_UI_CS_InventoryItem>().m_TypeID){
				UnequipItemLogic(false,index);
				return;
			}
			// b --> b
			else if(2 == btn.GetComponent<_UI_CS_InventoryItem>().m_TypeID){
				// ... ...
				LogManager.Log_Debug("--- SwapItem ---");
				CS_Main.Instance.g_commModule.SendMessage(
			   		ProtocolGame_SendRequest.SwapItem((byte)btn.GetComponent<_UI_CS_InventoryItem>().m_TypeID,(uint)btn.GetComponent<_UI_CS_InventoryItem>().m_Slot,(byte)2, (uint)index)
				);
				return;
			}
			// s --> b
			else if(2 == btn.GetComponent<_UI_CS_InventoryItem>().m_TypeID){
				// ... ...
				//return;
			}
		}
		
		index  = IsMoveE2(btn);
		if(-1 != index){
			// e --> e
			if(1 == btn.GetComponent<_UI_CS_InventoryItem>().m_TypeID){
				// ... ...
				//return;
			}
			// b --> e
			else if(2 == btn.GetComponent<_UI_CS_InventoryItem>().m_TypeID){ 
				
				if(6 == index || 7 == index){
				
					ItemDropStruct tempItem = ItemDeployInfo.Instance.GetItemObject(btn.GetComponent<_UI_CS_InventoryItem>().m_ItemInfo.ID,btn.GetComponent<_UI_CS_InventoryItem>().m_ItemInfo.perfrab,btn.GetComponent<_UI_CS_InventoryItem>().m_ItemInfo.gem,btn.GetComponent<_UI_CS_InventoryItem>().m_ItemInfo.enchant,btn.GetComponent<_UI_CS_InventoryItem>().m_ItemInfo.element,(int)btn.GetComponent<_UI_CS_InventoryItem>().m_ItemInfo.level);
					int indexEx = 0;
					
					if(8 == tempItem._TypeID){
						if(Inventory.Instance.equipmentArray[7].m_IsEmpty){
							indexEx = 6;
						}else{
							indexEx = _UI_CS_IngameMenu.Instance.EmptyBagIndex();
							if(-1 != indexEx){
								LogManager.Log_Debug("--- UnequipItem ---");
								CS_Main.Instance.g_commModule.SendMessage(
							   		ProtocolGame_SendRequest.UnequipItem((byte)7,(byte)2,(uint)indexEx)
								);
							
							}else{
						
								LogManager.Log_Debug("--- bag find err ---");
							}
							indexEx = 6;
						}
						
					}else{
				
						if(Inventory.Instance.equipmentArray[6].m_IsEmpty){
							indexEx = 6;
						}else{
							ItemDropStruct temp2Item = ItemDeployInfo.Instance.GetItemObject(Inventory.Instance.equipmentArray[6].m_ItemInfo.ID,
					                                                                 Inventory.Instance.equipmentArray[6].m_ItemInfo.perfrab,
					                                                                 Inventory.Instance.equipmentArray[6].m_ItemInfo.gem,
					                                                                 Inventory.Instance.equipmentArray[6].m_ItemInfo.enchant,
					                                                                 Inventory.Instance.equipmentArray[6].m_ItemInfo.element,
					                                                                 (int)Inventory.Instance.equipmentArray[6].m_ItemInfo.level);
							if(8 == temp2Item._TypeID){
								indexEx = 6;
							}else{
								indexEx = 7;
							}
						}
					}
					
					LogManager.Log_Debug("--- EquipItem ---");
					CS_Main.Instance.g_commModule.SendMessage( 
				   		ProtocolGame_SendRequest.EquipItem((byte)indexEx,(byte)2,(uint)btn.GetComponent<_UI_CS_InventoryItem>().m_Slot)
					);
					
				}else{
				
					LogManager.Log_Debug("--- EquipItem ---");
					CS_Main.Instance.g_commModule.SendMessage( 
				   		ProtocolGame_SendRequest.EquipItem((byte)index,(byte)2,(uint)btn.GetComponent<_UI_CS_InventoryItem>().m_Slot)
					);
					
				}
				return;
			}
			// s --> e
			else if(2 == btn.GetComponent<_UI_CS_InventoryItem>().m_TypeID){
				// ... ...
				//return;
			}
		}
		
		index  = IsMoveM2(btn);
		if(-1 != index){
			if(2 == btn.GetComponent<_UI_CS_InventoryItem>().m_TypeID){
				if(0 == index || 1 == index || 2 == index){
//					LogManager.Log_Debug("--- DelItem ---");
//					CS_Main.Instance.g_commModule.SendMessage(
//				   		ProtocolGame_SendRequest.DelItem((byte)btn.GetComponent<_UI_CS_InventoryItem>().m_TypeID,(uint)btn.GetComponent<_UI_CS_InventoryItem>().m_Slot)
//					);
					PopUpTransmute();
					return;
				}else if(3 == index){
					emptyBagSlotIndex = _UI_CS_IngameMenu.Instance.EmptyBagIndex(1);
					if(-1 != emptyBagSlotIndex){
						LogManager.Log_Debug("--- B1->B2 SwapItem ---");
						CS_Main.Instance.g_commModule.SendMessage(
			   			ProtocolGame_SendRequest.SwapItem((byte)2,(uint)btn.GetComponent<_UI_CS_InventoryItem>().m_Slot,(byte)2, (uint)emptyBagSlotIndex)
						);
					}else{
						m_MyIconBtn.transform.position = m_StartPosition;
//						TouchBG.transform.position = m_StartPosition;
					}
					return;
				}else if(4 == index){
					emptyBagSlotIndex = _UI_CS_IngameMenu.Instance.EmptyBagIndex(2);
					if(-1 != emptyBagSlotIndex){
						LogManager.Log_Debug("--- B1->B2 SwapItem ---");
						CS_Main.Instance.g_commModule.SendMessage(
			   			ProtocolGame_SendRequest.SwapItem((byte)2,(uint)btn.GetComponent<_UI_CS_InventoryItem>().m_Slot,(byte)2, (uint)emptyBagSlotIndex)
						);
					}else{
						m_MyIconBtn.transform.position = m_StartPosition;
//						TouchBG.transform.position = m_StartPosition;
					}
					return;
				}
				
				
			}
			// s --> m
			else if(2 == btn.GetComponent<_UI_CS_InventoryItem>().m_TypeID){
				// ... ...
				//return;
			}
		}
			
		m_MyIconBtn.transform.position = m_StartPosition;
	}
	
	private int IsMoveE(UIButton btn){
		int j = 0;
		for(j = 0;j<m_Group[0].m_GroupItemsCount;j++){		
			if(btn.GetComponent<_UI_CS_InventoryItem>().m_TypeID ==  m_Group[0].m_InventoryGroup[j].m_TypeID && btn.GetComponent<_UI_CS_InventoryItem>().m_Slot ==  j){	
			}else{
					float left 		= m_Group[0].m_InventoryGroup[j].transform.position.x;
					float top		= m_Group[0].m_InventoryGroup[j].transform.position.y;
					float right		= m_Group[0].m_InventoryGroup[j].transform.position.x + m_Group[0].m_InventoryGroup[j].transform.GetComponent<UIButton>().width;
					float bootom	= m_Group[0].m_InventoryGroup[j].transform.position.y - m_Group[0].m_InventoryGroup[j].transform.GetComponent<UIButton>().height;
					float x			= btn.transform.position.x;
					float y			= btn.transform.position.y;
				
					x += m_Group[0].m_InventoryGroup[j].transform.GetComponent<UIButton>().width/2;
					y -= m_Group[0].m_InventoryGroup[j].transform.GetComponent<UIButton>().height/2;
					
					if(left <= x && top >= y && right >= x && bootom <= y){
						return j;
					}
			}
		}
		return -1;
	}
	
	private int IsMoveB(UIButton btn){
		int j = 0;
		for(j = 0;j<m_Group[1].m_GroupItemsCount;j++){	
			if(btn.GetComponent<_UI_CS_InventoryItem>().m_TypeID ==  m_Group[1].m_InventoryGroup[j].m_TypeID && btn.GetComponent<_UI_CS_InventoryItem>().m_Slot ==  j){		
//				LogManager.Log_Info("j: " + j);
			}else{	
				float left 		= m_Group[1].m_InventoryGroup[j].transform.position.x;
				float top		= m_Group[1].m_InventoryGroup[j].transform.position.y;
				float right		= m_Group[1].m_InventoryGroup[j].transform.position.x + m_Group[1].m_InventoryGroup[j].transform.GetComponent<UIButton>().width;
				float bootom	= m_Group[1].m_InventoryGroup[j].transform.position.y - m_Group[1].m_InventoryGroup[j].transform.GetComponent<UIButton>().height;
				float x			= btn.transform.position.x;
				float y			= btn.transform.position.y;
				
				x += m_Group[1].m_InventoryGroup[j].transform.GetComponent<UIButton>().width/2;
				y -= m_Group[1].m_InventoryGroup[j].transform.GetComponent<UIButton>().height/2;
				
				if(left <= x && top >= y && right >= x && bootom <= y){
					return j;
				}
			}
		}
		return -1;
	}
	
	private int IsMoveM(UIButton btn){
		int j = 0;
		for(j = 0;j<m_Group[2].m_GroupItemsCount;j++){	
			if(btn.GetComponent<_UI_CS_InventoryItem>().m_TypeID ==  m_Group[2].m_InventoryGroup[j].m_TypeID && btn.GetComponent<_UI_CS_InventoryItem>().m_Slot ==  j){	
			}else{	
				float left 		= m_Group[2].m_InventoryGroup[j].transform.position.x;
				float top		= m_Group[2].m_InventoryGroup[j].transform.position.y;
				float right		= m_Group[2].m_InventoryGroup[j].transform.position.x + m_Group[2].m_InventoryGroup[j].transform.GetComponent<UIButton>().width;
				float bootom	= m_Group[2].m_InventoryGroup[j].transform.position.y - m_Group[2].m_InventoryGroup[j].transform.GetComponent<UIButton>().height;
				float x			= btn.transform.position.x;
				float y			= btn.transform.position.y;
				
				x += m_Group[2].m_InventoryGroup[j].transform.GetComponent<UIButton>().width/2;
				y -= m_Group[2].m_InventoryGroup[j].transform.GetComponent<UIButton>().height/2;
				
				if(left <= x && top >= y && right >= x && bootom <= y){
					return j;
				}
			}
		}
		return -1;
	}
	
	private int IsMoveStash(UIButton btn){
		int j = 0;
		for(j = 0;j<m_Group[3].m_GroupItemsCount;j++){	
			if(btn.GetComponent<_UI_CS_InventoryItem>().m_TypeID ==  m_Group[3].m_InventoryGroup[j].m_TypeID && btn.GetComponent<_UI_CS_InventoryItem>().m_Slot ==  j){	
			}else{	
				float left 		= m_Group[3].m_InventoryGroup[j].transform.position.x;
				float top		= m_Group[3].m_InventoryGroup[j].transform.position.y;
				float right		= m_Group[3].m_InventoryGroup[j].transform.position.x + m_Group[3].m_InventoryGroup[j].transform.GetComponent<UIButton>().width;
				float bootom	= m_Group[3].m_InventoryGroup[j].transform.position.y - m_Group[3].m_InventoryGroup[j].transform.GetComponent<UIButton>().height;
				float x			= btn.transform.position.x;
				float y			= btn.transform.position.y;
				
				x += m_Group[3].m_InventoryGroup[j].transform.GetComponent<UIButton>().width/2;
				y -= m_Group[3].m_InventoryGroup[j].transform.GetComponent<UIButton>().height/2;
				
				if(left <= x && top >= y && right >= x && bootom <= y){
					return j;
				}
			}
		}
		return -1;
	}
	
	private int IsMoveE2(UIButton btn){
		int j = 0;
		for(j = 0;j<m_Group[0].m_GroupItemsCount;j++){	
			if(btn.GetComponent<_UI_CS_InventoryItem>().m_TypeID ==  m_Group[0].m_InventoryGroup[j].m_TypeID && btn.GetComponent<_UI_CS_InventoryItem>().m_Slot ==  j){	
			}else{
					float left 		= m_Group[0].m_InventoryGroup[j].transform.position.x;
					float top		= m_Group[0].m_InventoryGroup[j].transform.position.y;
					float right		= m_Group[0].m_InventoryGroup[j].transform.position.x + m_Group[0].m_InventoryGroup[j].transform.GetComponent<UIButton>().width;
					float bootom	= m_Group[0].m_InventoryGroup[j].transform.position.y - m_Group[0].m_InventoryGroup[j].transform.GetComponent<UIButton>().height;
					float x			= m_MyIconBtn.transform.position.x;
					float y			= m_MyIconBtn.transform.position.y;
				
					x += m_Group[0].m_InventoryGroup[j].transform.GetComponent<UIButton>().width/2;
					y -= m_Group[0].m_InventoryGroup[j].transform.GetComponent<UIButton>().height/2;
					
					if(left <= x && top >= y && right >= x && bootom <= y){
						return j;
					}
			}
		}
		return -1;
	}
	
	private int IsMoveB2(UIButton btn){
		int j = 0;
		for(j = 0;j<m_Group[1].m_GroupItemsCount;j++){
			if(btn.GetComponent<_UI_CS_InventoryItem>().m_TypeID ==  m_Group[1].m_InventoryGroup[j].m_TypeID && btn.GetComponent<_UI_CS_InventoryItem>().m_Slot ==  j){			
			}else{	
				float left 		= m_Group[1].m_InventoryGroup[j].transform.position.x;
				float top		= m_Group[1].m_InventoryGroup[j].transform.position.y;
				float right		= m_Group[1].m_InventoryGroup[j].transform.position.x + m_Group[1].m_InventoryGroup[j].transform.GetComponent<UIButton>().width;
				float bootom	= m_Group[1].m_InventoryGroup[j].transform.position.y - m_Group[1].m_InventoryGroup[j].transform.GetComponent<UIButton>().height;
				float x			= m_MyIconBtn.transform.position.x;
				float y			= m_MyIconBtn.transform.position.y;
				
				x += m_Group[1].m_InventoryGroup[j].transform.GetComponent<UIButton>().width/2;
				y -= m_Group[1].m_InventoryGroup[j].transform.GetComponent<UIButton>().height/2;
				
				if(left <= x && top >= y && right >= x && bootom <= y){
					return j;
				}
			}
		}
		return -1;
	}
	
	private int IsMoveM2(UIButton btn){
		int j = 0;
		for(j = 0;j<m_Group[2].m_GroupItemsCount;j++){	
			if(btn.GetComponent<_UI_CS_InventoryItem>().m_TypeID ==  m_Group[2].m_InventoryGroup[j].m_TypeID && btn.GetComponent<_UI_CS_InventoryItem>().m_Slot ==  j){	
			}else{	
				float left 		= m_Group[2].m_InventoryGroup[j].transform.position.x;
				float top		= m_Group[2].m_InventoryGroup[j].transform.position.y;
				float right		= m_Group[2].m_InventoryGroup[j].transform.position.x + m_Group[2].m_InventoryGroup[j].transform.GetComponent<UIButton>().width;
				float bootom	= m_Group[2].m_InventoryGroup[j].transform.position.y - m_Group[2].m_InventoryGroup[j].transform.GetComponent<UIButton>().height;
				float x			= m_MyIconBtn.transform.position.x;
				float y			= m_MyIconBtn.transform.position.y;
				
				x += m_Group[2].m_InventoryGroup[j].transform.GetComponent<UIButton>().width/2;
				y -= m_Group[2].m_InventoryGroup[j].transform.GetComponent<UIButton>().height/2;
				
				if(left <= x && top >= y && right >= x && bootom <= y){
					return j;
				}
			}
		}
		return -1;
	}

	private void ElementToSwap(int myID,int destID){

	}
	
	private int GetEquipIdx(ItemDropStruct tempItem){
		switch(tempItem._TypeID){
			case 1:
				return 0;
			case 2:
				return 1;
			case 3:
				return 2;
			case 4:
				return 3;
			case 5:
				return 4;
			case 6:
				return 8;
			case 7:
			case 8:
				if(8 == tempItem._TypeID){
					return 6;
				}else{
					if(Inventory.Instance.equipmentArray[6].m_IsEmpty){
						return 6;
					}else{
						ItemDropStruct temp2Item = ItemDeployInfo.Instance.GetItemObject(Inventory.Instance.equipmentArray[6].m_ItemInfo.ID,
				                                                                 Inventory.Instance.equipmentArray[6].m_ItemInfo.perfrab,
				                                                                 Inventory.Instance.equipmentArray[6].m_ItemInfo.gem,
				                                                                 Inventory.Instance.equipmentArray[6].m_ItemInfo.enchant,
				                                                                 Inventory.Instance.equipmentArray[6].m_ItemInfo.element,
				                                                                 (int)Inventory.Instance.equipmentArray[6].m_ItemInfo.level);
						if(8 == temp2Item._TypeID){
							return 6;
						}else{
							return 7;
						}
					}
				}
			break;
		default:
			return 8;
		}
	}
	
	private void DoubleCLogic(){
//		_UI_CS_ToolsTip.Instance.DismissToolTips();		
		if(!m_IsEmpty){	
			m_IsOperate = false;
			IsMove(m_MyIconBtn);
			ItemDropStruct tempItem = ItemDeployInfo.Instance.GetItemObject(m_ItemInfo.ID,m_ItemInfo.perfrab,m_ItemInfo.gem,m_ItemInfo.enchant,m_ItemInfo.element,(int)m_ItemInfo.level);
			int index = 0;
			if(2 == m_TypeID){
			LogManager.Log_Debug("--- EquipItem ---");
			switch(tempItem._TypeID){
				case 1:
					index = 0;
				break;
				case 2:
					index = 1;
				break;
				case 3:
					index = 2;
				break;
				case 4:
					index = 3;
				break;
				case 5:
					index = 4;
				break;
				case 6:
					index = 8;
				break;
				case 7:
				case 8:
					if(8 == tempItem._TypeID){
						if(Inventory.Instance.equipmentArray[7].m_IsEmpty){
							index = 6;
						}else{
							index = _UI_CS_IngameMenu.Instance.EmptyBagIndex();
							if(-1 != index){
								LogManager.Log_Debug("--- UnequipItem ---");
								CS_Main.Instance.g_commModule.SendMessage(
							   		ProtocolGame_SendRequest.UnequipItem((byte)7,(byte)2,(uint)index)
								);
								_UI_CS_IngameMenu.Instance.isLockInvLogic = true;
							}else{
						
								LogManager.Log_Debug("--- bag find err ---");
							}
							index = 6;
						}
					}else{
				
						if(Inventory.Instance.equipmentArray[6].m_IsEmpty){
							index = 6;
						}else{
							ItemDropStruct temp2Item = ItemDeployInfo.Instance.GetItemObject(Inventory.Instance.equipmentArray[6].m_ItemInfo.ID,
					                                                                 Inventory.Instance.equipmentArray[6].m_ItemInfo.perfrab,
					                                                                 Inventory.Instance.equipmentArray[6].m_ItemInfo.gem,
					                                                                 Inventory.Instance.equipmentArray[6].m_ItemInfo.enchant,
					                                                                 Inventory.Instance.equipmentArray[6].m_ItemInfo.element,
					                                                                 (int)Inventory.Instance.equipmentArray[6].m_ItemInfo.level);
							if(8 == temp2Item._TypeID){
								index = 6;
							}else{
								index = 7;
							}
						}
					}
				break;
				//USE CONSUMABLE
				case 14:
					switch(tempItem._SecTypeID){
					case 4:
						//play sound food.
						SoundCue.PlayPrefabAndDestroy(_UI_CS_ElementsInfo.Instance.consumableFoodSound);
						CS_Main.Instance.g_commModule.SendMessage(
							   	ProtocolGame_SendRequest.useItem((byte)m_TypeID,(uint)m_Slot)
						);
						break;
					case 5:
						//play sound drink.
						SoundCue.PlayPrefabAndDestroy(_UI_CS_ElementsInfo.Instance.consumableDrinkSound);
						CS_Main.Instance.g_commModule.SendMessage(
							   	ProtocolGame_SendRequest.useItem((byte)m_TypeID,(uint)m_Slot)
						);
						break;
					default:
						break;
					}
				break;
				default:
					break;
				}
				if(index < 9){
					if(tempItem.info_Level <= _PlayerData.Instance.playerLevel){
						CS_Main.Instance.g_commModule.SendMessage(
				   			ProtocolGame_SendRequest.EquipItem((byte)index,(byte)2,(uint)m_Slot)
						);
						_UI_CS_IngameMenu.Instance.isLockInvLogic = true;
					}else{
						IngamePopMsg.Instance.AwakeIngameMsg("Item level too high");
					}
				}
		
			}else if(1 == m_TypeID){
		
				index = _UI_CS_IngameMenu.Instance.EmptyBagIndex();
				if(-1 != index){
					LogManager.Log_Debug("--- UnequipItem ---");
				
						int tempTypeIdx = 0;
			
						switch(tempItem._TypeID){
			
							case 1:
								tempTypeIdx = 0;
							break;
							case 2:
								tempTypeIdx = 1;
							break;
							case 3:
								tempTypeIdx = 2;
							break;
							case 4:
								tempTypeIdx = 3;
							break;
							case 5:
								tempTypeIdx = 4;
							break;
							case 6:
								tempTypeIdx = 8;
							break;
					
							case 7:
							case 8:
								tempTypeIdx = 6;
								if(Inventory.Instance.equipmentArray[7].m_IsEmpty){
							
									CS_Main.Instance.g_commModule.SendMessage(
								   		ProtocolGame_SendRequest.UnequipItem((byte)6,(byte)2,(uint)index)
									);
									_UI_CS_IngameMenu.Instance.isLockInvLogic = true;
								}else{
							
									CS_Main.Instance.g_commModule.SendMessage(
								   		ProtocolGame_SendRequest.UnequipItem((byte)7,(byte)2,(uint)index)
									);	
									_UI_CS_IngameMenu.Instance.isLockInvLogic = true;
								}
								break;
							default:
								break;
						}
				
						
						if(tempTypeIdx < 9){
				
							if(tempTypeIdx != 6){
					
								CS_Main.Instance.g_commModule.SendMessage(
								   		ProtocolGame_SendRequest.UnequipItem((byte)tempTypeIdx,(byte)2,(uint)index)
									);	
								_UI_CS_IngameMenu.Instance.isLockInvLogic = true;
							}
				
						}

				}else{
			
					LogManager.Log_Debug("--- bag find err ---");
				}
			}
		}
	
	}
	
	private void MouseRightPress(){
		m_MyIconBtn.transform.position = m_StartPosition;
		CancelPress();
	}
#endregion
}
