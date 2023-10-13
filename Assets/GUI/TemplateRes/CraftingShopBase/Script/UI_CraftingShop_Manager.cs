using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UI_CraftingShop_Manager : MonoBehaviour {

    public static UI_CraftingShop_Manager Instance;

    void Awake()
    {
        Instance = this;
    }

    #region Interface
    [SerializeField] UI_Hud_Border_Control EquipmentBorder;
    [SerializeField] UI_Hud_Border_Control InventoryBorder;// because panel cutting, if the object is not belong this panel when object created, could not cut the object correctly. so i create two border, one in inventory panel, another is not.
    [SerializeField] UI_CraftingShop_Slot SlotPrefab;

    [SerializeField] Transform EquipmentSlotParent;
	UI_CraftingShop_Slot[] _Equipmentslots = new UI_CraftingShop_Slot[0];

    [SerializeField] Transform InventorySlotParent;
    UI_CraftingShop_Slot[] _Inventoryslots = new UI_CraftingShop_Slot[0];

    [SerializeField]  UI_CraftingShop_ItemsGroup ItemsGroup;
    [SerializeField]  UI_CraftingShop_ItemDetailInfo DetailInfoGroup;
    [SerializeField]  UI_CraftingShop_CraftGroup CraftGroup;

    public UI_CraftingShop_Slot CurItemSlot;
    public _ItemInfo CurItemInfo { get { return CurItemSlot.GetData(); } }
    public UI_TypeDefine.ENUM_UI_CraftingShop_ItemAttribute CurAttribute;
	
	[SerializeField] Transform UI_SFX_BuySuccess;
    #endregion

    #region Public

    public void CraftingAniFinished(bool success)
    {
        if (success)
        {
            DetailInfoGroup.UpdateAllInfo(CurItemInfo.localData);
        }
        CraftGroup.WorkingAniPlayComplete(CurItemInfo.localData, success);
    }

    public void ShowItemsOrCraft(bool isShowItem)
    {
        ItemsGroup.gameObject.SetActive(isShowItem);
        CraftGroup.gameObject.SetActive(!isShowItem);
    }

    public void UpdateEquipItems(_ItemInfo[] itemsinfo)
    {
        // sort all slots, if slots if more than info, delete some useless slots, if less, create some.
        List<UI_CraftingShop_Slot> _tempSlotsList = new List<UI_CraftingShop_Slot>();
        for (int i = 0; i < itemsinfo.Length; i++)
        {
            if (i < _Equipmentslots.Length)
            {
                _tempSlotsList.Add(_Equipmentslots[i]);
            }
            else
            {
                UI_CraftingShop_Slot _newslot = UnityEngine.Object.Instantiate(SlotPrefab) as UI_CraftingShop_Slot;
                _newslot.transform.parent = EquipmentSlotParent;
                _newslot.transform.localScale = Vector3.one;
                _newslot.transform.localPosition = Vector3.zero;
                _tempSlotsList.Add(_newslot);
            }
        }
        for (int i = itemsinfo.Length; i < _Equipmentslots.Length; i++)
        {
            Destroy(_Equipmentslots[i].gameObject);
        }

        _Equipmentslots = _tempSlotsList.ToArray();

        // update info
        for (int i = 0; i < itemsinfo.Length; i++)
        {
            _Equipmentslots[i].UpdateInfo(itemsinfo[i]);
            _Equipmentslots[i].IsBelongEquipment = true;
        }

        // reposition slots
        RepositionEquipmentslots();
    }

    public void UpdateBagItems(_ItemInfo[] itemsinfo)
    {
        // sort all slots, if slots if more than info, delete some useless slots, if less, create some.
        List<UI_CraftingShop_Slot> _tempSlotsList = new List<UI_CraftingShop_Slot>();
        for (int i = 0; i < itemsinfo.Length; i++)
        {
            if (i < _Inventoryslots.Length)
            {
                _tempSlotsList.Add(_Inventoryslots[i]);
            }
            else
            {
                UI_CraftingShop_Slot _newslot = UnityEngine.Object.Instantiate(SlotPrefab) as UI_CraftingShop_Slot;
                _newslot.transform.parent = InventorySlotParent;
                _newslot.transform.localScale = Vector3.one;
                _newslot.transform.localPosition = Vector3.zero;
                _tempSlotsList.Add(_newslot);
            }
        }
        for (int i = itemsinfo.Length; i < _Inventoryslots.Length; i++)
        {
            Destroy(_Inventoryslots[i].gameObject);
        }

        _Inventoryslots = _tempSlotsList.ToArray();

        // update info
        for (int i = 0; i < itemsinfo.Length; i++)
        {
            _Inventoryslots[i].UpdateInfo(itemsinfo[i]);
            _Inventoryslots[i].IsBelongEquipment = false;
        }

        // reposition slots
        RepositionInventoryslots();
    }

    public void UpdateCraftAllInfo(UpgradeRecipe curRecipe, ItemDropStruct _newiteminfo, UI_TypeDefine.ENUM_UI_CraftingShop_ItemAttribute _attrType)
    {
        CraftGroup.UpdateRecipeInfo(curRecipe, _newiteminfo, _attrType);
    }

    public void CraftingOK()
    {
        CraftGroup.CraftingOK();
    }
	
	public void CraftingFailed()
	{
		CraftGroup.CraftingFailed();
	}

    #endregion

    #region Click callback
    // select a item from main frame.
    public void ItemSlotClicked(UI_CraftingShop_Slot _slot)
    {
        CurItemSlot = _slot;

        if (CraftingShopSlotClicked_Event != null)
        {
            CraftingShopSlotClicked_Event(CurItemInfo);
        }
		
		// show detail infomation
        DetailInfoGroup.UpdateAllInfo(CurItemInfo.localData);
        Vector3 _pos = _slot.transform.position;
		
		// show the border
        UI_Hud_Border_Control _tempBorder;
        if (_slot.IsBelongEquipment)
        {
            _tempBorder = EquipmentBorder;
            // hide the inventory border box
            InventoryBorder.gameObject.SetActive(false);
        }
        else
        {
            _tempBorder = InventoryBorder;
            // hide the equipment border box
            EquipmentBorder.gameObject.SetActive(false);
        }
        _pos.z = _tempBorder.transform.position.z;
        _tempBorder.gameObject.SetActive(true);
        _tempBorder.transform.position = _pos;
        _tempBorder.transform.parent = _slot.transform;
        _tempBorder.transform.localScale = Vector3.one;
        _tempBorder.Pop(1, -1);
    }

    // select a attribute to craft, show recipe for this attribute.
    public void CraftItemAttribute(UI_TypeDefine.ENUM_UI_CraftingShop_ItemAttribute _attrType)
    {
        CurAttribute = _attrType;
        if (CraftingShopAttrClicked_Event != null)
            CraftingShopAttrClicked_Event(CurItemInfo.localData, CurAttribute);
    }
	
	public void BuyMaterialSuccess()
	{
		SoundCue.PlayPrefabAndDestroy(UI_SFX_BuySuccess);
	}
	
	void BackToInventoryMode()
	{
		ShowItemsOrCraft(true);
	}
	
    #endregion

    #region Local

    void RepositionEquipmentslots()
    {
        float _slotslength = (_Equipmentslots.Length - 1) * (74 + 12);
        for (int i = 0; i < _Equipmentslots.Length; i++ )
        {
            Vector3 _tempPos = _Equipmentslots[i].transform.localPosition;
            _tempPos.x = i * (74 + 12) - _slotslength / 2;
            _Equipmentslots[i].transform.localPosition = _tempPos;
        }
    }

    void RepositionInventoryslots()
    {
        InventorySlotParent.GetComponent<UIGrid>().Reposition();
    }

    #endregion

    #region Delegate
    public delegate void Handle_UIShopCraftingCloseBTNClicked();
    public event Handle_UIShopCraftingCloseBTNClicked CraftingShopCloseClicked_Event;
    public delegate void Handle_UIShopCraftingSlotBTNClicked(_ItemInfo _itemInfo);
    public event Handle_UIShopCraftingSlotBTNClicked CraftingShopSlotClicked_Event;
    public delegate void Handle_UIShopCraftingAttrBTNClicked(ItemDropStruct _iteminfo, UI_TypeDefine.ENUM_UI_CraftingShop_ItemAttribute _attrType);
    public event Handle_UIShopCraftingAttrBTNClicked CraftingShopAttrClicked_Event;
    #endregion

    #region BTN callback

    void CloseBTNClicked()
    {
        if (CraftingShopCloseClicked_Event != null)
        {
            CraftingShopCloseClicked_Event();
        }
    }
    #endregion
}
