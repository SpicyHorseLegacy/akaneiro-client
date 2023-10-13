using UnityEngine;
using System.Collections;

public class InventoryCtrl : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        UpdateIsCreateDragItem();
        UpdatePing();
    }

    public void RegisterSingleTemplateEvent(string _templateName)
    {
        if (_templateName == "CharInfo_Inventory" && CharInfo_InventoryManager.Instance)
        {
            if (CharInfo_InventoryManager.Instance)
            {
                CharInfo_InventoryManager.Instance.OnUpSpaceDelegate += UpSpaceDelegate;
                CharInfo_InventoryManager.Instance.OnDownSpaceDelegate += DownSpaceDelegate;
            }
            foreach (InventorySlot slot in CharInfo_InventoryManager.Instance.list)
            {
                slot.OnPressDelegate += PressDelegate;
                slot.OnReleaseDelegate += ReleaseDelegate;
                slot.OnDoubleClickDelegate += DoubleClickDelegate;
            }
            InitInventorySlot();
            InGameScreenCtrl.Instance.ChaInfoCtrl.CurCharInfoUI = UI_TypeDefine.EnumCharInfoUITYPE.Inventory;
            if (UI_CharInfoBG_Manager.Instance)
            {
                UI_CharInfoBG_Manager.Instance.SetBTNHighLight(UI_TypeDefine.EnumCharInfoUITYPE.Inventory);
            }
        }
        if (_templateName == "CharInfo_Equips" && UI_ChaEquipment_Manager.Instance)
        {
            foreach (InventorySlot slot in UI_ChaEquipment_Manager.Instance.list)
            {
                slot.OnPressDelegate += PressDelegate;
                slot.OnReleaseDelegate += ReleaseDelegate;
                slot.OnDoubleClickDelegate += DoubleClickDelegate;
            }
            InitEquipSlot();
            UpdatePlayerModeEquip();
            InitCharLevelInfo();
        }
        if (_templateName == "TipsManager" && TipManager.Instance)
        {
            TipManager.Instance.OnSellDelegate += SellDelegate;
            TipManager.Instance.OnUseDelegate += UseDelegate;
			TipManager.Instance.OnEquipDelegate += EquipDelegate;
            HideTip();
        }
        if (_templateName == "FoodSlot" && FoodSoltManager.Instance)
        {
            foreach (InventorySlot slot in FoodSoltManager.Instance.list)
            {
                slot.OnPressDelegate += PressDelegate;
                slot.OnReleaseDelegate += ReleaseDelegate;
                slot.OnDoubleClickDelegate += DoubleClickDelegate;
            }
            UpdateFoodSlot();
        }
        if (_templateName == "Stash" && StashManager.Instance)
        {
            foreach (InventorySlot slot in StashManager.Instance.list)
            {
                slot.OnPressDelegate += PressDelegate;
                slot.OnReleaseDelegate += ReleaseDelegate;
                slot.OnDoubleClickDelegate += DoubleClickDelegate;
            }
            StashManager.Instance.OnExitDelegate += ExitStash;
            StashManager.Instance.OnStashTabDelegate += StashTabDelegete;
            StashManager.Instance.OnCreateTabDelegate += CreateNewTab;
            StashManager.Instance.UpdateTab(PlayerDataManager.Instance.GetMaxStashTab(), PlayerDataManager.Instance.GetCurStashIdx());
            StashManager.Instance.SetUnlockTabPice(PlayerDataManager.Instance.GetUnlockStashPice());
            UpdateStashSlot();
        }
        if (_templateName == "MoneyBar" && MoneyBarManager.Instance)
        {
            MoneyBarManager.Instance.OnAddKarmaDelegate += AddKarmaDelegate;
            MoneyBarManager.Instance.OnAddCrystalDelegate += AddCrystalDelegate;
            MoneyBarManager.Instance.SetKarmaVal(PlayerDataManager.Instance.GetKarmaVal());
            MoneyBarManager.Instance.SetCrystalVal(PlayerDataManager.Instance.GetCrystalVal());
        }
        if (_templateName == "KarmaRecharge" && KarmaRechargeManager.Instance)
        {
            KarmaRechargeManager.Instance.OnExitDelegate += ExitRechargeKarmaDelegate;
            KarmaRechargeManager.Instance.OnAddKarmaDelegate += RechargeKarmaValDelegate;
            InitKarmaRechargeData();
        }
        if (_templateName == "CrystalRecharge" && CrystalRechargeManager.Instance)
        {
            CrystalRechargeManager.Instance.OnExitDelegate += ExitRechargeCrystalDelegate;
            CrystalRechargeManager.Instance.OnAddKarmaDelegate += RechargeKarmaValDelegate;
            InitCrystalRechargeData();
        }
        if (_templateName == "Ping" && PingManager.Instance)
        {
            isUpdatePing = true;
            PlayerDataManager.Instance.sendPingTime = Time.realtimeSinceStartup;
        }
        if (_templateName == "TutorialPanel" && TutorialPanelManager.Instance)
        {
            TutorialPanelManager.Instance.OnContinueDelegate += TutorialContinueDelegate;
            TutorialPanelManager.Instance.SetDialogContent(PlayerDataManager.Instance.tutorialTitle, PlayerDataManager.Instance.tutorialContent);
        }
        if (_templateName == "BottomTip" && BottomTipManager.Instance)
        {

        }
    }

    public void UnregisterSingleTemplateEvent(string _templateName)
    {
        if (_templateName == "CharInfo_Inventory" && CharInfo_InventoryManager.Instance)
        {
            if (CharInfo_InventoryManager.Instance)
            {
                CharInfo_InventoryManager.Instance.OnUpSpaceDelegate -= UpSpaceDelegate;
                CharInfo_InventoryManager.Instance.OnDownSpaceDelegate -= DownSpaceDelegate;
            }
            foreach (InventorySlot slot in CharInfo_InventoryManager.Instance.list)
            {
                slot.OnPressDelegate -= PressDelegate;
                slot.OnReleaseDelegate -= ReleaseDelegate;
                slot.OnDoubleClickDelegate -= DoubleClickDelegate;
            }
            HideTip();
        }
        if (_templateName == "CharInfo_Equips" && UI_ChaEquipment_Manager.Instance)
        {
            foreach (InventorySlot slot in UI_ChaEquipment_Manager.Instance.list)
            {
                slot.OnPressDelegate -= PressDelegate;
                slot.OnReleaseDelegate -= ReleaseDelegate;
                slot.OnDoubleClickDelegate -= DoubleClickDelegate;
            }
            HideTip();
        }
        if (_templateName == "TipsManager" && TipManager.Instance)
        {
            TipManager.Instance.OnSellDelegate -= SellDelegate;
            TipManager.Instance.OnUseDelegate -= UseDelegate;
			TipManager.Instance.OnEquipDelegate -= EquipDelegate;
        }
        if (_templateName == "FoodSolt" && FoodSoltManager.Instance)
        {
            foreach (InventorySlot slot in FoodSoltManager.Instance.list)
            {
                slot.OnPressDelegate -= PressDelegate;
                slot.OnReleaseDelegate -= ReleaseDelegate;
                slot.OnDoubleClickDelegate -= DoubleClickDelegate;
            }
        }
        if (_templateName == "Stash" && StashManager.Instance)
        {
            foreach (InventorySlot slot in StashManager.Instance.list)
            {
                slot.OnPressDelegate -= PressDelegate;
                slot.OnReleaseDelegate -= ReleaseDelegate;
                slot.OnDoubleClickDelegate -= DoubleClickDelegate;
            }
            StashManager.Instance.OnStashTabDelegate -= StashTabDelegete;
            StashManager.Instance.OnExitDelegate -= ExitStash;
            StashManager.Instance.OnCreateTabDelegate -= CreateNewTab;
        }
        if (_templateName == "MoneyBar" && MoneyBarManager.Instance)
        {
            MoneyBarManager.Instance.OnAddKarmaDelegate -= AddKarmaDelegate;
            MoneyBarManager.Instance.OnAddCrystalDelegate -= AddCrystalDelegate;
        }
        if (_templateName == "KarmaRecharge" && KarmaRechargeManager.Instance)
        {
            KarmaRechargeManager.Instance.OnExitDelegate -= ExitRechargeKarmaDelegate;
            KarmaRechargeManager.Instance.OnAddKarmaDelegate -= RechargeKarmaValDelegate;
        }
        if (_templateName == "CrystalRecharge" && CrystalRechargeManager.Instance)
        {
            CrystalRechargeManager.Instance.OnExitDelegate -= ExitRechargeCrystalDelegate;
            CrystalRechargeManager.Instance.OnAddKarmaDelegate -= RechargeKarmaValDelegate;
        }
        if (_templateName == "Ping" && PingManager.Instance)
        {
            isUpdatePing = false;
        }
        if (_templateName == "TutorialPanel" && TutorialPanelManager.Instance)
        {
            TutorialPanelManager.Instance.OnContinueDelegate -= TutorialContinueDelegate;
        }
        if (_templateName == "BottomTip" && BottomTipManager.Instance)
        {

        }
    }

    void InitCharLevelInfo()
    {
        string _charname = PlayerDataManager.Instance.ChaName;
        int _curlv = PlayerDataManager.Instance.CurLV;
        int _curexp = (int)(PlayerDataManager.Instance.CurrentExperience - PlayerDataManager.Instance.PreviousMaximumExperience);
        int _maxexp = (int)(PlayerDataManager.Instance.CurrentMaximumExperience - PlayerDataManager.Instance.PreviousMaximumExperience);
        UI_ChaEquipment_Manager.Instance.UpdateCharLevelInfo(_curlv, _charname, _curexp, _maxexp);
    }

    [SerializeField]
    private Texture2D emptyImg;
    public void InitInventorySlot()
    {
        foreach (_ItemInfo data in PlayerDataManager.Instance.bagList)
        {
            data.isChange = false;
            InventorySlot info = CharInfo_InventoryManager.Instance.GetBagItemData(data.slot);
            if (!data.empty)
            {
                info.SetEmptyFlag(false);
                info.SetData(data);
                info.SetCount(data.count);
                info.ChangeBGColor(PlayerDataManager.Instance.GetNameColor(data.localData.info_eleVal + data.localData.info_encVal + data.localData.info_gemVal));
                ItemPrefabs.Instance.GetItemIcon(data.localData._ItemID, data.localData._TypeID, data.localData._PrefabID, info.GetIcon());
            }
            else
            {
                info.EmptySlot(emptyImg);
            }
        }
    }
    public void InitEquipSlot()
    {
        foreach (_ItemInfo data in PlayerDataManager.Instance.equipList)
        {
            data.isChange = false;
            InventorySlot info = UI_ChaEquipment_Manager.Instance.GetEquipItemData(data.slot);
            if (info == null)
            {
                continue;
            }
            if (!data.empty)
            {
                info.SetEmptyFlag(false);
                info.SetCount(0);
                info.SetData(data);
                ItemPrefabs.Instance.GetItemIcon(data.localData._ItemID, data.localData._TypeID, data.localData._PrefabID, info.GetIcon());
                info.ChangeBGColor(PlayerDataManager.Instance.GetNameColor(data.localData.info_eleVal + data.localData.info_encVal + data.localData.info_gemVal));
            }
            else
            {
                info.EmptySlot(emptyImg);
            }
        }
    }
    public void UpdateFoodSlot()
    {
        if (!FoodSoltManager.Instance)
        {
            return;
        }
        int idx = -1;
        for (int i = 0; i < 3; i++)
        {
            InventorySlot info = FoodSoltManager.Instance.GetFoodItemData(i + 1);
            idx = PlayerDataManager.Instance.foodList[i];
            if (idx != -1)
            {
                _ItemInfo data = PlayerDataManager.Instance.GetBagItemData(idx);
                if (!data.empty)
                {
                    info.SetEmptyFlag(false);
                    info.SetData(data);
                    ItemPrefabs.Instance.GetItemIcon(data.localData._ItemID, data.localData._TypeID, data.localData._PrefabID, info.GetIcon());
                    info.ChangeBGColor(PlayerDataManager.Instance.GetNameColor(data.localData.info_eleVal + data.localData.info_encVal + data.localData.info_gemVal));
                    info.SetCount(data.count);
                }
                else
                {
                    PlayerDataManager.Instance.EmptyFoodSlot(i);
                    info.EmptySlot(emptyImg);
                }
            }
            else
            {
                info.EmptySlot(emptyImg);
            }
        }
    }
    public void UpdateEquipSlot()
    {
        _ItemInfo[] equipsList = PlayerDataManager.Instance.equipList.ToArray();
        foreach (_ItemInfo data in PlayerDataManager.Instance.equipList)
        {
            if (data == null)
            {
                continue;
            }
            data.isChange = false;
            InventorySlot info = UI_ChaEquipment_Manager.Instance.GetEquipItemData(data.slot);
            if (info == null)
            {
                continue;
            }
            if (!data.empty)
            {
                info.SetEmptyFlag(false);
                info.SetCount(0);
                info.SetData(data);
                ItemPrefabs.Instance.GetItemIcon(data.localData._ItemID, data.localData._TypeID, data.localData._PrefabID, info.GetIcon());
                info.ChangeBGColor(PlayerDataManager.Instance.GetNameColor(data.localData.info_eleVal + data.localData.info_encVal + data.localData.info_gemVal));
            }
            else
            {
                info.EmptySlot(emptyImg);
            }
        }
    }
    public void UpdateStashSlot()
    {
        int startIdx = (PlayerDataManager.Instance.GetCurStashIdx() - 1) * 12;
        for (int i = startIdx; i < startIdx + PlayerDataManager.Instance.GetStashMaxSlot(); i++)
        {
            _ItemInfo data = PlayerDataManager.Instance.GetStashItemData(i + 1);
            if (data == null)
            {
                continue;
            }
            data.isChange = false;
            InventorySlot info = StashManager.Instance.GetStashItemData((i % 12) + 1);
            if (info == null)
            {
                continue;
            }
            if (!data.empty)
            {
                info.SetEmptyFlag(false);
                info.SetData(data);
                info.SetCount(data.count);
                ItemPrefabs.Instance.GetItemIcon(data.localData._ItemID, data.localData._TypeID, data.localData._PrefabID, info.GetIcon());
                info.ChangeBGColor(PlayerDataManager.Instance.GetNameColor(data.localData.info_eleVal + data.localData.info_encVal + data.localData.info_gemVal));
            }
            else
            {
                info.EmptySlot(emptyImg);
            }
        }
    }
    public void UpdateInventorySlot()
    {
        foreach (_ItemInfo data in PlayerDataManager.Instance.bagList)
        {
			//Debug.Log ("Debugger:: [1]");
            if (!data.isChange)
            {
				//Debug.Log ("Debugger:: [2]");
                continue;
				//Debug.Log ("Debugger:: [3]");
            }
			//Debug.Log ("Debugger:: [4]");
            data.isChange = false;
			//Debug.Log ("Debugger:: [5]");
            if (CharInfo_InventoryManager.Instance == null)
            {
				//Debug.Log ("Debugger:: [6]");
                continue;
				//Debug.Log ("Debugger:: [7]");
            }
			//Debug.Log ("Debugger:: [8]");

            InventorySlot info = CharInfo_InventoryManager.Instance.GetBagItemData(data.slot);
			//Debug.Log ("Debugger:: [9]");
            if (!data.empty)
            {
				//Debug.Log ("Debugger:: [10]");
                info.SetEmptyFlag(false);
                info.SetData(data);
                info.SetCount(data.count);
                ItemPrefabs.Instance.GetItemIcon(data.localData._ItemID, data.localData._TypeID, data.localData._PrefabID, info.GetIcon());
                info.ChangeBGColor(PlayerDataManager.Instance.GetNameColor(data.localData.info_eleVal + data.localData.info_encVal + data.localData.info_gemVal));
				//Debug.Log ("Debugger:: [11]");
			}
            else
            {
				//Debug.Log ("Debugger:: [12]");
                info.EmptySlot(emptyImg);
				//Debug.Log ("Debugger:: [13]");
            }
			//Debug.Log ("Debugger:: [14]");
        }
		//Debug.Log ("Debugger:: [15]");
        if (FoodSoltManager.Instance)
        {
			//Debug.Log ("Debugger:: [16]");
            UpdateFoodSlot();
			//Debug.Log ("Debugger:: [17]");
        }
		//Debug.Log ("Debugger:: [18]");
    }

    InventorySlot tSlotItem;
    private void PressDelegate(int type, int slot)
    {
        switch (type)
        {
            case 1:
                tSlotItem = CharInfo_InventoryManager.Instance.GetBagItemData(slot);
                break;
            case 2:
                tSlotItem = UI_ChaEquipment_Manager.Instance.GetEquipItemData(slot);
                break;
            case 4:
                tSlotItem = StashManager.Instance.GetStashItemData(slot);
                break;
            case 5:
                tSlotItem = FoodSoltManager.Instance.GetFoodItemData(slot);
                break;
        }
        HideAllHighLight();
        HideTip();
        curSoltItem = tSlotItem;
        if (curSoltItem != null)
        {
            curSoltItem.HideHighLight(false);
            ShowTip(curSoltItem.GetData().localData);
        }
        isPress = true;
    }
    private void ReleaseDelegate(int type, int slot)
    {
        isPress = false;
        CheckAction();
        DestoryDragItem();
    }
    private void DoubleClickDelegate(int type, int slot)
    {
        GUILogManager.LogInfo("DoubleClickDelegate type:" + type + "|slot" + slot);
        switch (type)
        {
            case 1:
                if (CharInfo_InventoryManager.Instance)
                {
                    InventorySlot tBagData = CharInfo_InventoryManager.Instance.GetBagItemData(slot);
                    int _slot = GetSlotFromType(tBagData.GetData().localData._TypeID);
                    if (_slot != -1)
                    {
                        if (_slot != 10)
                        {
                            EquipItem(tBagData, _slot);
                        }
                        else
                        {
                            GUILogManager.LogInfo("Use");
                            UseConsumable(2, tBagData.GetSlot() - 1, tBagData.GetData().localData);
                        }
                        HideTip();
                    }
                }
                break;
            case 2:
                if (UI_ChaEquipment_Manager.Instance && CharInfo_InventoryManager.Instance)
                {
                    InventorySlot tEquipData = UI_ChaEquipment_Manager.Instance.GetEquipItemData(slot);
                    int _slotIdx = CharInfo_InventoryManager.Instance.GetEmptySlotIdx() - 1;
                    UnequipItemLogic(tEquipData, _slotIdx);
                    HideTip();
                }
                break;
            case 5:
                _ItemInfo _tData = PlayerDataManager.Instance.GetFoodItemData(slot);
                GUILogManager.LogInfo("Use");
                UseConsumable(2, _tData.slot - 1, _tData.localData);
                HideTip();
                break;
        }
    }

    #region Drag Item
    [SerializeField]
    private DragItem dragObj;
    private Transform curDragItem;
    private void CreateDragItem(Texture2D img, Transform parentTran)
    {
        Transform obj = (Transform)UnityEngine.Object.Instantiate(dragObj.transform);
        curDragItem = obj;
        obj.gameObject.GetComponent<DragItem>().SetIcon(img);
        obj.gameObject.GetComponent<DragItem>().transform.parent = parentTran;
        obj.gameObject.GetComponent<DragItem>().transform.localScale = new Vector3(1, 1, 1);
        //obj.gameObject.GetComponent<DragItem>().transform.localPosition = new Vector3(0,0,-5);
        UpdateTargetHL();
    }

    private void DestoryDragItem()
    {
        if (curDragItem != null)
        {
            Destroy(curDragItem.gameObject);
            curDragItem = null;
            HideTip();
            HideAllHighLight();
        }
    }

    public bool isPress = false;
    private InventorySlot curSoltItem;
    private void UpdateIsCreateDragItem()
    {
        if (isPress)
        {
            Vector3 vector = UICamera.currentCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -1));
            if (CheckIsDrag(vector.x, vector.y))
            {
                switch (curSoltItem.GetType())
                {
                    case 1:
                        CreateDragItem((Texture2D)curSoltItem.GetIcon().mainTexture, CharInfo_InventoryManager.Instance.transform);
                        break;
                    case 2:
                        CreateDragItem((Texture2D)curSoltItem.GetIcon().mainTexture, UI_ChaEquipment_Manager.Instance.transform);
                        break;
                    case 4:
                        CreateDragItem((Texture2D)curSoltItem.GetIcon().mainTexture, StashManager.Instance.transform);
                        break;
                    case 5:
                        CreateDragItem((Texture2D)curSoltItem.GetIcon().mainTexture, FoodSoltManager.Instance.transform);
                        break;
                }
                isPress = false;
            }
        }
    }

    private float _moveOffest = 0.1f;
    private bool CheckIsDrag(float x, float y)
    {
        if (x > curSoltItem.transform.position.x + _moveOffest || x < curSoltItem.transform.position.x - _moveOffest
        || y > curSoltItem.transform.position.y + _moveOffest || y < curSoltItem.transform.position.y - _moveOffest)
        {
            return true;
        }
        return false;
    }

    private void UpSpaceDelegate()
    {
        CharInfo_InventoryManager.Instance.IncScrollBar(-0.05f);
    }
    private void DownSpaceDelegate()
    {
        CharInfo_InventoryManager.Instance.IncScrollBar(0.05f);
    }

    private void UpdateTargetHL()
    {
        if (curSoltItem != null)
        {
            if (!curSoltItem.GetEmptyFlag())
            {
                switch (curSoltItem.GetType())
                {
                    case 1:
                        int _itemType = GetSlotFromType(curSoltItem.GetData().localData._TypeID);
                        if (_itemType < 10)
                        {
                            if (UI_ChaEquipment_Manager.Instance)
                            {
                                if (UI_ChaEquipment_Manager.Instance.GetEquipItemData(_itemType) != null)
                                    UI_ChaEquipment_Manager.Instance.GetEquipItemData(_itemType).HideHighLight(false);
                            }
                        }
                        else if (_itemType == 10)
                        {
                            if (FoodSoltManager.Instance)
                            {
                                FoodSoltManager.Instance.GetFoodItemData(1).HideHighLight(false);
                                FoodSoltManager.Instance.GetFoodItemData(2).HideHighLight(false);
                                FoodSoltManager.Instance.GetFoodItemData(3).HideHighLight(false);
                            }
                        }
                        break;
                    case 2:
                    case 4:
                        if (CharInfo_InventoryManager.Instance)
                        {
                            int _bagSlot = CharInfo_InventoryManager.Instance.GetEmptySlotIdx();
                            if (_bagSlot != -1)
                            {
                                CharInfo_InventoryManager.Instance.GetBagItemData(_bagSlot).HideHighLight(false);
                            }
                        }
                        break;
                    case 5:
                        if (FoodSoltManager.Instance)
                        {
                            FoodSoltManager.Instance.GetFoodItemData(1).HideHighLight(false);
                            FoodSoltManager.Instance.GetFoodItemData(2).HideHighLight(false);
                            FoodSoltManager.Instance.GetFoodItemData(3).HideHighLight(false);
                        }
                        break;
                }
            }
        }
    }
    #endregion

    private void HideAllHighLight()
    {
        if (CharInfo_InventoryManager.Instance)
        {
            foreach (InventorySlot slot in CharInfo_InventoryManager.Instance.list)
            {
                slot.HideHighLight(true);
            }
        }
        if (UI_ChaEquipment_Manager.Instance)
        {
            foreach (InventorySlot slot in UI_ChaEquipment_Manager.Instance.list)
            {
                slot.HideHighLight(true);
            }
        }
        if (StashManager.Instance)
        {
            foreach (InventorySlot slot in StashManager.Instance.list)
            {
                slot.HideHighLight(true);
            }
        }
        if (FoodSoltManager.Instance)
        {
            foreach (InventorySlot slot in FoodSoltManager.Instance.list)
            {
                slot.HideHighLight(true);
            }
        }
    }

    #region Update Slot action
    public void EquipItem(InventorySlot data, int itemIdx)
    {
        int tIdx = 0;
        if (1 == data.GetType())
        {
            //item is weapon.
            if (6 == itemIdx || 7 == itemIdx)
            {
                //two hand weapon.
                if (8 == data.GetData().localData._TypeID)
                {
                    tIdx = 6;
                    //off weapon is empty.
                    bool isOffhand = UI_ChaEquipment_Manager.Instance.CheckOffWeaponIsEmpty();
                    if (isOffhand == false)
                    {
                        tIdx = CharInfo_InventoryManager.Instance.GetEmptySlotIdx(data.GetSlot()) - 1;

                        if (tIdx != -2)
                        {
                            GUILogManager.LogInfo("UnequipItem");
                            CS_Main.Instance.g_commModule.SendMessage(ProtocolGame_SendRequest.UnequipItem((byte)7, (byte)2, (uint)tIdx));
                            tIdx = 6;
                        }
                        else
                        {
                            PopUpBox.PopUpErr("Sorry, Bag lack of space.");
                            return;
                        }
                    }
                }
                else
                {
                    tIdx = 6;
                    if (!UI_ChaEquipment_Manager.Instance.CheckMainWeaponIsEmpty())
                    {
                        //main weapon is two hand.
                        if (8 != UI_ChaEquipment_Manager.Instance.GetEquipItemData(6).GetData().localData._TypeID)
                        {
                            tIdx = 7;
                        }
                    }
                }
                GUILogManager.LogInfo("EquipItem");
                CS_Main.Instance.g_commModule.SendMessage(ProtocolGame_SendRequest.EquipItem((byte)tIdx, (byte)2, (uint)data.GetSlot() - 1));
            }
            else
            {
                GUILogManager.LogInfo("EquipItem");
                CS_Main.Instance.g_commModule.SendMessage(ProtocolGame_SendRequest.EquipItem((byte)itemIdx, (byte)2, (uint)data.GetSlot() - 1));
            }
            return;
        }
    }

    public int GetSlotFromType(int itemType)
    {
        switch (itemType)
        {
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
                return 6;
            case 14:
                return 10;
            default:
                return -1;
        }
    }

    public void CheckAction()
    {
        Vector3 vector = UICamera.currentCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -1));
        int inIndex = -1; int inIndex2 = -1;
        int tType = PlayerDataManager.Instance.SynType(curSoltItem.GetType());
        int tSlot = curSoltItem.GetSlot() - 1;

        if (FoodSoltManager.Instance)
        {
            inIndex = FoodSoltManager.Instance.InFoodSlot(vector, curSoltItem.GetType(), curSoltItem.GetSlot());
            //in food.
            if (inIndex > -1)
            {
                //b-->f
                if (1 == curSoltItem.GetType())
                {
                    //type14 is consumable.
                    if (curSoltItem.GetData().localData._TypeID != 14)
                    {
                        return;
                    }
                    PlayerDataManager.Instance.UpdateFoodSlot(inIndex, curSoltItem.GetSlot());
                    UpdateFoodSlot();
                    return;
                }
                //e-->f
                else if (2 == curSoltItem.GetType())
                {
                    return;
                }
                //s-->f
                else if (3 == curSoltItem.GetType())
                {
                    return;
                }
                //f-->f
                else if (5 == curSoltItem.GetType())
                {
                    PlayerDataManager.Instance.SwapFoodSlot(inIndex, curSoltItem.GetSlot());
                    UpdateFoodSlot();
                    return;
                }
            }
            //			//if cur select Slot is food.Remove it.
            //			if(5 == curSoltItem.GetType()) {
            //				PlayerDataManager.Instance.EmptyFoodSlot(curSoltItem.GetSlot()-1);
            //				UpdateFoodSlot();
            //			}
        }

        if (CharInfo_InventoryManager.Instance)
        {
            inIndex = CharInfo_InventoryManager.Instance.InInventoySlot(vector, curSoltItem.GetType(), curSoltItem.GetSlot()) - 1;
            //in inventory.
            if (inIndex > -1)
            {
                //b-->b
                if (1 == curSoltItem.GetType())
                {
                    GUILogManager.LogInfo("SwapItem");
                    CS_Main.Instance.g_commModule.SendMessage(ProtocolGame_SendRequest.SwapItem((byte)tType, (uint)tSlot, (byte)2, (uint)inIndex));
                    return;
                }
                //e-->b
                else if (2 == curSoltItem.GetType())
                {
                    UnequipItemLogic(curSoltItem, inIndex);
                    return;
                }
                //s-->b
                else if (4 == curSoltItem.GetType())
                {
                    GUILogManager.LogInfo("stash to bag");
                    inIndex2 = (curSoltItem.GetSlot() - 1 + (PlayerDataManager.Instance.GetCurStashIdx() - 1) * 12);
                    CS_Main.Instance.g_commModule.SendMessage(ProtocolGame_SendRequest.SwapItem((byte)4, (uint)inIndex2, (byte)2, (uint)inIndex));
                    return;
                }
                //f-->b
                else if (5 == curSoltItem.GetType())
                {
                    PlayerDataManager.Instance.EmptyFoodSlot(curSoltItem.GetSlot() - 1);
                    UpdateFoodSlot();
                    return;
                }
            }
        }

        if (UI_ChaEquipment_Manager.Instance)
        {
            inIndex = UI_ChaEquipment_Manager.Instance.InEquipSlot(vector, curSoltItem.GetType(), curSoltItem.GetSlot());
            //in equipment.
            if (inIndex > -1)
            {
                //e-->e
                if (2 == curSoltItem.GetType())
                {
                    return;
                }
                //b-->e
                EquipItem(curSoltItem, inIndex);
                //s-->e
                if (2 == curSoltItem.GetType())
                {
                    return;
                }
                //f-->e
                else if (5 == curSoltItem.GetType())
                {
                    PlayerDataManager.Instance.EmptyFoodSlot(curSoltItem.GetSlot() - 1);
                    UpdateFoodSlot();
                    return;
                }
            }
        }

        if (StashManager.Instance)
        {
            inIndex = StashManager.Instance.InStashSlot(vector, curSoltItem.GetType(), curSoltItem.GetSlot()) - 1;
            //in stash.
            if (inIndex > -1)
            {
                inIndex += (PlayerDataManager.Instance.GetCurStashIdx() - 1) * 12;

                //b-->s
                if (1 == curSoltItem.GetType())
                {
                    GUILogManager.LogInfo("bag to stash");
                    CS_Main.Instance.g_commModule.SendMessage(ProtocolGame_SendRequest.SwapItem((byte)2, (uint)curSoltItem.GetSlot() - 1, (byte)4, (uint)inIndex));
                    return;
                }
                //e-->s
                else if (2 == curSoltItem.GetType())
                {
                    return;
                }
                //s-->s
                else if (4 == curSoltItem.GetType())
                {
                    GUILogManager.LogInfo("stash to stash");
                    inIndex2 = curSoltItem.GetSlot() - 1 + (PlayerDataManager.Instance.GetCurStashIdx() - 1) * 12;
                    CS_Main.Instance.g_commModule.SendMessage(ProtocolGame_SendRequest.SwapItem((byte)4, (uint)inIndex2, (byte)4, (uint)inIndex));
                    return;
                }
                //f-->s
                else if (5 == curSoltItem.GetType())
                {
                    PlayerDataManager.Instance.EmptyFoodSlot(curSoltItem.GetSlot() - 1);
                    UpdateFoodSlot();
                    return;
                }
            }
        }
    }
    #endregion

    public void UpdatePlayerModeEquip()
    {
        PlayerModel pm = UI_ChaEquipment_Manager.Instance.Model;
        pm.Gender = PlayerDataManager.Instance.Gender;
        pm.equipmentMan.DetachAllItems(PlayerDataManager.Instance.Gender);
        foreach (_ItemInfo info in PlayerDataManager.Instance.equipList)
        {
            if (info.empty)
            {
                continue;
            }
            Transform item = UnityEngine.Object.Instantiate(ItemPrefabs.Instance.GetItemPrefab(info.localData._ItemID, 0, info.localData._PrefabID)) as Transform;
            pm.equipmentMan.UpdateItemInfoBySlot((uint)info.slot, item, info.serData, true, PlayerDataManager.Instance.Gender);
        }
        pm.equipmentMan.UpdateEquipment(PlayerDataManager.Instance.Gender);
        pm.usingLatestConfig = true;
    }

    public PlayerModel GetEquipModel()
    {
        return UI_ChaEquipment_Manager.Instance.Model;
    }

    private void UnequipItemLogic(InventorySlot data, int detSoltIdx)
    {

        ItemDropStruct lItemInfo = data.GetData().localData;
        int slotIdx = CharInfo_InventoryManager.Instance.GetEmptySlotIdx() - 1;
        if (slotIdx == -1)
        {
            PopUpBox.PopUpErr("Sorry, Bag lack of space.");
            return;
        }
        //item isn't weapon.
        if (7 != lItemInfo._TypeID && 8 != lItemInfo._TypeID)
        {
            GUILogManager.LogInfo("UnequipItem");
            CS_Main.Instance.g_commModule.SendMessage(ProtocolGame_SendRequest.UnequipItem((byte)data.GetSlot(), (byte)2, (uint)detSoltIdx));
        }
        else
        {
            //off weapon is empty.
            if (UI_ChaEquipment_Manager.Instance.CheckOffWeaponIsEmpty())
            {
                GUILogManager.LogInfo("UnequipItem");
                CS_Main.Instance.g_commModule.SendMessage(ProtocolGame_SendRequest.UnequipItem((byte)data.GetSlot(), (byte)2, (uint)detSoltIdx));
            }
            else
            {
                //cur move solt is off weapon.
                if (7 == data.GetSlot())
                {
                    GUILogManager.LogInfo("UnequipItem");
                    CS_Main.Instance.g_commModule.SendMessage(ProtocolGame_SendRequest.UnequipItem((byte)data.GetSlot(), (byte)2, (uint)detSoltIdx));
                }
                else
                {
                    //UnequipItem all weapon.
                    int secSlotIdx = CharInfo_InventoryManager.Instance.GetSecondEmptySlotIdx() - 1;
                    if (-1 == slotIdx || -1 == secSlotIdx)
                    {
                        PopUpBox.PopUpErr("Sorry, Bag lack of space.");
                        return;
                    }
                    else
                    {
                        GUILogManager.LogInfo("UnequipItem All Weapon");
                        CS_Main.Instance.g_commModule.SendMessage(ProtocolGame_SendRequest.UnequipItem((byte)7, (byte)2, (uint)slotIdx));
                        CS_Main.Instance.g_commModule.SendMessage(ProtocolGame_SendRequest.UnequipItem((byte)6, (byte)2, (uint)secSlotIdx));
                    }
                }
            }
        }
    }

    #region Tip
    private void SellDelegate()
    {
        if (curSoltItem != null)
        {
            if (curSoltItem.GetType() == 1)
            {
                GUILogManager.LogInfo("Sell");
                CS_Main.Instance.g_commModule.SendMessage(ProtocolGame_SendRequest.SaleItem((byte)2, (uint)(curSoltItem.GetSlot() - 1)));
                HideTip();
            }
        }
    }
    private void UseDelegate()
    {
        if (curSoltItem != null)
        {
            if (curSoltItem.GetType() == 1)
            {
                GUILogManager.LogInfo("Use");
                UseConsumable(2, curSoltItem.GetSlot() - 1, curSoltItem.GetData().localData);
                HideTip();
            }
        }
    }
	
	private void EquipDelegate()
	{
		curSoltItem._DoubleClickDelegate(); 
	}

    private void ShowTip(ItemDropStruct data)
    {
        if (null == data)
            return;
        if (ItemShopScreenCtrl.Instance != null ||
          (InGameScreenCtrl.Instance != null && InGameScreenCtrl.Instance.ChaInfoCtrl != null && InGameScreenCtrl.Instance.ChaInfoCtrl.CurCharInfoUI == UI_TypeDefine.EnumCharInfoUITYPE.Inventory))
        {
            if (data._TypeID > 8)
            {
				TipManager.Instance.HideEquipBtn(true);
                if (curSoltItem.GetType() == 1)
                {
                    ShowOtherItemTips(data, true);
                    TipManager.Instance.HideUseBtn(false);
                }
                else if (curSoltItem.GetType() == 4)
                {
                    ShowOtherItemTips(data, true);
                    TipManager.Instance.HideUseBtn(true);
                }
            }
            else
            {
                TipManager.Instance.HideUseBtn(true);
				TipManager.Instance.HideEquipBtn(false);
                if (curSoltItem.GetType() == 1)
                {
                    ShowSeleTip(data, true);
                    CheckEquipTip(data, false);
                }
                else if (curSoltItem.GetType() == 2)
                {
                    CheckEquipTip(data, true, curSoltItem);
                }
                else if (curSoltItem.GetType() == 4)
                {
                    ShowSeleTip(data, true);
                    CheckEquipTip(data, false);
                }
            }
        }
    }

    public void ShowOtherItemTips(ItemDropStruct data, bool isShowHighLight)
    {
        string itemName = ""; //string itemType = "";
        string itemLevel = data.info_Level.ToString();
        //string itemAD = ""; string itemSS = ""; 
        float itemVal = 0;
        int eleCount = 0;
        TipManager.Instance.curTip.Hide(false);
        itemVal = (data.info_gemVal + data.info_encVal + data.info_eleVal);
        TipManager.Instance.curTip.SetIcon(3, "");
        TipManager.Instance.curTip.SetSpeed(true, "");
        TipManager.Instance.curTip.SetType(data._PropsDes);
        itemName = PlayerDataManager.Instance.GetItemName(data);
        TipManager.Instance.curTip.SetOtherItemContent(itemName);
        TipManager.Instance.curTip.SetName(itemName, PlayerDataManager.Instance.GetNameTextColor(itemVal));
        TipManager.Instance.curTip.SetTitleBgColor(PlayerDataManager.Instance.GetNameColor(itemVal));
        TipManager.Instance.curTip.SetLevel(data.info_Level, PlayerDataManager.Instance.CurLV);
        eleCount++;
        TipManager.Instance.curTip.SetEleInfo(false, eleCount, emptyImg, data._PropsDes2, "");
        TipManager.Instance.curTip.SetEncInfo(true, 0, null, "", "");
        TipManager.Instance.curTip.SetGemInfo(true, 0, null, "", "");
        TipManager.Instance.curTip.SetMoney(PlayerDataManager.Instance.GetItemValue(2, data.info_Level, data.info_eleVal, data.info_encVal, data.info_gemVal, data._ItemVal), eleCount);
        TipManager.Instance.curTip.SetHighLightSize(eleCount, isShowHighLight);
        TipManager.Instance.curTip.SetArrowBgSize(eleCount);
    }

    public void ShowSeleTip(ItemDropStruct data, bool isShowHighLight)
    {
        string itemName = ""; string itemType = "";
        string itemLevel = data.info_Level.ToString();
        string itemAD = ""; string itemSS = ""; float itemVal = 0;
        int eleCount = 0;
        TipManager.Instance.curTip.Hide(false);
        itemVal = (data.info_gemVal + data.info_encVal + data.info_eleVal);
        if (data._TypeID == 7 || data._TypeID == 8)
        {
            //weapon
            itemAD = ((int)(data.info_MinAtc * data.info_Modifier)).ToString() + " - " + ((int)(data.info_MaxAtc * data.info_Modifier)).ToString();
            itemSS = data.info_Speed;
            itemType = data.info_hand + data.info_TypeName;
            TipManager.Instance.curTip.SetIcon(1, itemAD);
            TipManager.Instance.curTip.SetSpeed(false, itemSS);
            TipManager.Instance.curTip.SetType(itemType);
        }
        else if (1 == data._TypeID || 3 == data._TypeID || 4 == data._TypeID || 6 == data._TypeID)
        {
            //armor
            itemAD = ((int)(data.info_MinDef * data.info_Modifier)).ToString();
            itemSS = data.info_Set;
            itemType = data.info_ArmorLevel + data.info_TypeName;
            TipManager.Instance.curTip.SetIcon(2, itemAD);
            TipManager.Instance.curTip.SetSpeed(true, itemSS);
            TipManager.Instance.curTip.SetType(itemType);
        }
        else if (2 == data._TypeID || 5 == data._TypeID)
        {
            //accesery
            itemAD = ((int)(data.info_MinDef * data.info_Modifier)).ToString();
            itemSS = "";
            itemType = data.info_TypeName;
            TipManager.Instance.curTip.SetIcon(2, itemAD);
            TipManager.Instance.curTip.SetSpeed(true, itemSS);
            TipManager.Instance.curTip.SetType(itemType);
        }
        itemName = PlayerDataManager.Instance.GetItemName(data);
        TipManager.Instance.curTip.SetName(itemName, PlayerDataManager.Instance.GetNameTextColor(itemVal));
        TipManager.Instance.curTip.SetTitleBgColor(PlayerDataManager.Instance.GetNameColor(itemVal));
        TipManager.Instance.curTip.SetLevel(data.info_Level, PlayerDataManager.Instance.CurLV);
        eleCount = 0;
        if (data._EleID != 0)
        {
            eleCount++;
            TipManager.Instance.curTip.SetEleInfo(false, eleCount, _UI_CS_ElementsInfo.Instance.EleIcon[data.info_eleIconIdx - 1], data.info_EleNameLv, data.info_EleDesc1 + data.info_EleDesc2);
        }
        else
        {
            TipManager.Instance.curTip.SetEleInfo(true, 0, null, "", "");
        }
        if (data._EnchantID != 0)
        {
            eleCount++;
            TipManager.Instance.curTip.SetEncInfo(false, eleCount, _UI_CS_ElementsInfo.Instance.EncIcon[data.info_encIconIdx - 1], data.info_EncNameLv, data.info_EncDesc1 + data.info_EncDesc2);
        }
        else
        {
            TipManager.Instance.curTip.SetEncInfo(true, 0, null, "", "");
        }
        if (data._GemID != 0)
        {
            eleCount++;
            TipManager.Instance.curTip.SetGemInfo(false, eleCount, _UI_CS_ElementsInfo.Instance.GemIcon[data.info_gemIconIdx - 1], data.info_GemeNameLv, data.info_GemDesc1 + data.info_GemDesc2);
        }
        else
        {
            TipManager.Instance.curTip.SetGemInfo(true, 0, null, "", "");
        }
        TipManager.Instance.curTip.SetMoney(PlayerDataManager.Instance.GetItemValue(2, data.info_Level, data.info_eleVal, data.info_encVal, data.info_gemVal, data._ItemVal), eleCount);
        TipManager.Instance.curTip.SetHighLightSize(eleCount, isShowHighLight);
        TipManager.Instance.curTip.SetArrowBgSize(eleCount);
    }

    public void ShowEquipTip(ItemDropStruct data, bool isShowHighLight)
    {
        string itemName = ""; string itemType = "";
        string itemLevel = data.info_Level.ToString();
        string itemAD = ""; string itemSS = ""; float itemVal = 0;
        int eleCount = 0;
        TipManager.Instance.equpTip.Hide(false);
        itemVal = (data.info_gemVal + data.info_encVal + data.info_eleVal);
        if (data._TypeID == 7 || data._TypeID == 8)
        {
            //weapon
            itemAD = ((int)(data.info_MinAtc * data.info_Modifier)).ToString() + " - " + ((int)(data.info_MaxAtc * data.info_Modifier)).ToString();
            itemSS = data.info_Speed;
            itemType = data.info_hand + data.info_TypeName;
            TipManager.Instance.equpTip.SetIcon(1, itemAD);
            TipManager.Instance.equpTip.SetSpeed(false, itemSS);
            TipManager.Instance.equpTip.SetType(itemType);
        }
        else if (1 == data._TypeID || 3 == data._TypeID || 4 == data._TypeID || 6 == data._TypeID)
        {
            //armor
            itemAD = ((int)(data.info_MinDef * data.info_Modifier)).ToString();
            itemSS = data.info_Set;
            itemType = data.info_ArmorLevel + data.info_TypeName;
            TipManager.Instance.equpTip.SetIcon(2, itemAD);
            TipManager.Instance.equpTip.SetSpeed(true, itemSS);
            TipManager.Instance.equpTip.SetType(itemType);
        }
        else if (2 == data._TypeID || 5 == data._TypeID)
        {
            //accesery
            itemAD = ((int)(data.info_MinDef * data.info_Modifier)).ToString();
            itemSS = "";
            itemType = data.info_TypeName;
            TipManager.Instance.equpTip.SetIcon(2, itemAD);
            TipManager.Instance.equpTip.SetSpeed(true, itemSS);
            TipManager.Instance.equpTip.SetType(itemType);
        }
        itemName = PlayerDataManager.Instance.GetItemName(data);
        TipManager.Instance.equpTip.SetName(itemName, PlayerDataManager.Instance.GetNameTextColor(itemVal));
        TipManager.Instance.equpTip.SetTitleBgColor(PlayerDataManager.Instance.GetNameColor(itemVal));
        TipManager.Instance.equpTip.SetLevel(data.info_Level, PlayerDataManager.Instance.CurLV);
        eleCount = 0;
        if (data._EleID != 0)
        {
            eleCount++;
            TipManager.Instance.equpTip.SetEleInfo(false, eleCount, _UI_CS_ElementsInfo.Instance.EleIcon[data.info_eleIconIdx - 1], data.info_EleNameLv, data.info_EleDesc1 + data.info_EleDesc2);
        }
        else
        {
            TipManager.Instance.equpTip.SetEleInfo(true, 0, null, "", "");
        }
        if (data._EnchantID != 0)
        {
            eleCount++;
            TipManager.Instance.equpTip.SetEncInfo(false, eleCount, _UI_CS_ElementsInfo.Instance.EncIcon[data.info_encIconIdx - 1], data.info_EncNameLv, data.info_EncDesc1 + data.info_EncDesc2);
        }
        else
        {
            TipManager.Instance.equpTip.SetEncInfo(true, 0, null, "", "");
        }
        if (data._GemID != 0)
        {
            eleCount++;
            TipManager.Instance.equpTip.SetGemInfo(false, eleCount, _UI_CS_ElementsInfo.Instance.GemIcon[data.info_gemIconIdx - 1], data.info_GemeNameLv, data.info_GemDesc1 + data.info_GemDesc2);
        }
        else
        {
            TipManager.Instance.equpTip.SetGemInfo(true, 0, null, "", "");
        }
        TipManager.Instance.equpTip.SetMoney(PlayerDataManager.Instance.GetItemValue(2, data.info_Level, data.info_eleVal, data.info_encVal, data.info_gemVal, data._ItemVal), eleCount);
        TipManager.Instance.equpTip.SetHighLightSize(eleCount, isShowHighLight);
        TipManager.Instance.equpTip.SetArrowBgSize(eleCount);
    }

    public void CheckEquipTip(ItemDropStruct data, bool isShowHighLight, InventorySlot slot = null)
    {
        _ItemInfo tItemData;
        switch (data._TypeID)
        {
            case 1:
                tItemData = PlayerDataManager.Instance.GetEquipItemData(0);
                break;
            case 2:
                tItemData = PlayerDataManager.Instance.GetEquipItemData(1);
                break;
            case 3:
                tItemData = PlayerDataManager.Instance.GetEquipItemData(2);
                break;
            case 4:
                tItemData = PlayerDataManager.Instance.GetEquipItemData(3);
                break;
            case 5:
                tItemData = PlayerDataManager.Instance.GetEquipItemData(4);
                break;
            case 6:
                tItemData = PlayerDataManager.Instance.GetEquipItemData(8);
                break;
            case 7:
                tItemData = PlayerDataManager.Instance.GetEquipItemData(slot != null ? slot.GetSlot() : 6);
                break;
            case 8:
                tItemData = PlayerDataManager.Instance.GetEquipItemData(slot != null ? slot.GetSlot() : 6);
                break;
            default:
                tItemData = null;
                break;
        }
        if (!tItemData.empty)
        {
            ShowEquipTip(tItemData.localData, isShowHighLight);
        }
    }

    private void HideTip()
    {
        TipManager.Instance.curTip.Hide(true);
        TipManager.Instance.equpTip.Hide(true);
    }
    #endregion

    private bool UseConsumable(int type, int slot, ItemDropStruct data)
    {
        if (14 == data._TypeID)
        {
            CS_Main.Instance.g_commModule.SendMessage(ProtocolGame_SendRequest.useItem((byte)type, (uint)slot));
            if (4 == data._SecTypeID)
            {
                //				//play food sound.
                //				SoundCue.PlayPrefabAndDestroy();
            }
            else if (5 == data._SecTypeID)
            {
                //				//play drink sound.
                //				SoundCue.PlayPrefabAndDestroy();
            }
            return true;
        }
        return false;
    }

    #region Stash
    public void AwakeStash()
    {
        GUIManager.Instance.AddTemplate("CharInfo_Inventory");
        GUIManager.Instance.AddTemplate("Stash");
    }
    private void StashTabDelegete(int idx)
    {
        PlayerDataManager.Instance.SetCurStashTapIdx(idx);
        StashManager.Instance.UpdateTab(PlayerDataManager.Instance.GetMaxStashTab(), PlayerDataManager.Instance.GetCurStashIdx());
        StashManager.Instance.SetUnlockTabPice(PlayerDataManager.Instance.GetUnlockStashPice());
        UpdateStashSlot();
    }
    private void ExitStash()
    {
        GUIManager.Instance.RemoveTemplate("CharInfo_Inventory");
        GUIManager.Instance.RemoveTemplate("Stash");
        InGameScreenCtrl.Instance.ChaInfoCtrl.CurCharInfoUI = UI_TypeDefine.EnumCharInfoUITYPE.NONE;
        Player.Instance.ReactivePlayer();
        GameCamera.BackToPlayerCamera();
    }
    private void CreateNewTab()
    {
        CS_Main.Instance.g_commModule.SendMessage(ProtocolGame_SendRequest.buyStash());
    }
    #endregion

    #region MoneyBar
    private void AddKarmaDelegate()
    {
        if (isPopUpRecharge)
        {
            return;
        }
        if (Steamworks.activeInstance)
        {
            Steamworks.activeInstance.ShowShop("karma");
        }
        else
        {
            GUIManager.Instance.AddTemplate("KarmaRecharge");
            isPopUpRecharge = true;
        }
    }
    private void AddCrystalDelegate()
    {
        if (isPopUpRecharge)
        {
            return;
        }
        if (Steamworks.activeInstance)
        {
            Steamworks.activeInstance.ShowShop("crystal");
        }
        else
        {
            GUIManager.Instance.AddTemplate("CrystalRecharge");
            isPopUpRecharge = true;
        }
    }
    #endregion

    #region Recharge
    private bool isPopUpRecharge = false;
    private void ExitRechargeKarmaDelegate()
    {
        GUIManager.Instance.RemoveTemplate("KarmaRecharge");
        isPopUpRecharge = false;
    }
    private void ExitRechargeCrystalDelegate()
    {
        GUIManager.Instance.RemoveTemplate("CrystalRecharge");
        isPopUpRecharge = false;
    }
    private void RechargeKarmaValDelegate(string content)
    {
        if (Steamworks.activeInstance != null)
        {
            Steamworks.activeInstance.StartPayment(content);
        }
        switch (VersionManager.Instance.GetVersionType())
        {
            case VersionType.WebVersion:
                Application.ExternalCall("select_gold", content);
                break;
            case VersionType.NormalClientVersion:
                StartCoroutine(VersionManager.Instance.SendMsgToServerForPay(content));
                break;
            default:
                StartCoroutine(VersionManager.Instance.SendMsgToServerForPay(content));
                break;
        }
    }

    private void InitKarmaRechargeData()
    {
        KarmaRechargeManager.Instance.SetKarmaInfo(PlayerDataManager.Instance.karmaRechargTitle);
        for (int i = 0; i < 7; i++)
        {
            KarmaRechargeManager.Instance.SetKarmaVal(i, PlayerDataManager.Instance.rechargeValData.karmaVal[i]);
            KarmaRechargeManager.Instance.SetPayVal(i, PlayerDataManager.Instance.rechargeValData.karmaPayVal[i]);
        }
    }
    private void InitCrystalRechargeData()
    {
        CrystalRechargeManager.Instance.SetKarmaInfo(PlayerDataManager.Instance.crystalRechargTitle);
        for (int i = 0; i < 7; i++)
        {
            CrystalRechargeManager.Instance.SetKarmaVal(i, PlayerDataManager.Instance.rechargeValData.crystalVal[i]);
            CrystalRechargeManager.Instance.SetPayVal(i, PlayerDataManager.Instance.rechargeValData.crystalPayVal[i]);
        }
    }
    #endregion

    #region Ping
    private bool isUpdatePing = false;
    public void UpdatePingRender()
    {
        if (isUpdatePing)
        {
            int pingTime = (int)((Time.realtimeSinceStartup - PlayerDataManager.Instance.sendPingTime) * 1000);
            if (PingManager.Instance)
            {
                PingManager.Instance.UpdatePing(pingTime);
            }
        }
    }
    private float pingMaxTime = 2;
    private float pingCurTime = 0;
    private void UpdatePing()
    {
        if (isUpdatePing)
        {
            pingCurTime += Time.deltaTime;
            if (pingCurTime > pingMaxTime)
            {
                pingCurTime = 0;
                SendPingToServer();
            }
        }
    }
    private void SendPingToServer()
    {
        PlayerDataManager.Instance.sendPingTime = Time.realtimeSinceStartup;
        CS_Main.Instance.g_commModule.SendMessage(ProtocolGame_SendRequest.pingRequest());
    }
    #endregion

    #region TutorialPanel
    private void TutorialContinueDelegate()
    {
        GUIManager.Instance.RemoveTemplate("TutorialPanel");
        TutorialMan.Instance.AddBranchEndFlag();
        Player.Instance.ReactivePlayer();
        GameCamera.BackToPlayerCamera();
    }
    #endregion

}
