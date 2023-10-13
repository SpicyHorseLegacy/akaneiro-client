using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UI_CraftingShop_CraftGroup : MonoBehaviour {

    [SerializeField]  UI_CraftingShop_MaterialGroup MaterialPrefab;
    [SerializeField]  GameObject MaterialGroupParent;
    [SerializeField]  UI_CraftingShop_ItemDetail_AttributeCraftPanel AttrGroup;
    [SerializeField]  UI_MoneyGroup KarmaGroup;
    [SerializeField]  UI_MoneyGroup CrystalGroup;
    [SerializeField]  GameObject CraftBTN_KarmaGroup;
    [SerializeField]  GameObject CraftBTN_CrystalGroup;
    [SerializeField]  UILabel Label_KarmaSuccess;
    [SerializeField]  UILabel Label_CrystalSuccess;
    [SerializeField]  UI_LevelStars_Manager Level_Stars;
    [SerializeField]  GameObject CraftMaterialsGroup;
    [SerializeField]  UILabel CraftMaxLevelLabel;
    [SerializeField]  GameObject MaterialsGroup;
    [SerializeField]  UI_CraftingShop_WorkingAnimPanel WorkingAnimPanel;

    UpgradeRecipe _currecipe;
    UI_TypeDefine.ENUM_UI_CraftingShop_ItemAttribute _curattrtype;
    UI_CraftingShop_MaterialGroup[] curMaterials = new UI_CraftingShop_MaterialGroup[0];
    int CurMaterialCrystalNeed = 0;

    mapCraftMaterial matsMap = new mapCraftMaterial();

    public void CraftingOK()
    {
       	showCraftMaterialGroup(false);
        WorkingAnimPanel.StartAnim(true);
    }

    public void CraftingFailed()
    {
        showCraftMaterialGroup(false);
        WorkingAnimPanel.StartAnim(false);
    }

    public void WorkingAniPlayComplete(ItemDropStruct _curlevelinfo, bool success)
    {
        if (success)
        {
            //UpdateAttrInfo(_curlevelinfo, _curattrtype);
            Level_Stars.StumpAllStars();
        }
    }

    public void ExitWorkingAnimation()
    {
        UI_CraftingShop_Manager.Instance.CraftItemAttribute(_curattrtype);
        showCraftMaterialGroup(true);
    }
	
    public void UpdateRecipeInfo(UpgradeRecipe _recipe, ItemDropStruct _curlevelinfo, UI_TypeDefine.ENUM_UI_CraftingShop_ItemAttribute _attrType)
    {
        showCraftMaterialGroup(true);

        _currecipe = _recipe;
        _curattrtype = _attrType;

        UpdateAttrInfo(_curlevelinfo, _attrType);

        showMaxlevellabel(_currecipe == null);

        if (_currecipe != null)
        {
            SetupAllMaterials(_currecipe);
            KarmaGroup.UpdateAllInfo(true, _recipe.ksPrize.ToString());
            Label_KarmaSuccess.text = "[00ff00]" + _recipe.ksChance + "%[-] SUCCESS";
            Label_CrystalSuccess.text = "[ffff00]" + _recipe.crChance + "%[-] SUCCESS";
            CrystalGroup.UpdateAllInfo(false, "" + (CurMaterialCrystalNeed + _recipe.crPrize));
        }
    }

    void showMaxlevellabel(bool show)
    {
        CraftMaxLevelLabel.gameObject.SetActive(show);
        CraftBTN_KarmaGroup.SetActive(!show);
        CraftBTN_CrystalGroup.SetActive(!show);
        MaterialsGroup.SetActive(!show);
    }
	
	void showCraftMaterialGroup(bool show)
	{
		CraftMaterialsGroup.SetActive(show);
        WorkingAnimPanel.gameObject.SetActive(!show);
	}
	
    void UpdateAttrInfo(ItemDropStruct _curlevelinfo, UI_TypeDefine.ENUM_UI_CraftingShop_ItemAttribute _attrType)
    {
        if (_curlevelinfo == null || _attrType == UI_TypeDefine.ENUM_UI_CraftingShop_ItemAttribute.NONE || _attrType == UI_TypeDefine.ENUM_UI_CraftingShop_ItemAttribute.MAX)
            return;

        int _levelid = _curlevelinfo.info_Level;
        int _eleID = _curlevelinfo._EleID;
        int _enchantID = _curlevelinfo._EnchantID;
        int _gemID = _curlevelinfo._GemID;
        bool _descriptionatcenter = false;
        bool _showstars = true;
        int _starlv = 0;
        switch (_attrType)
        {
            case UI_TypeDefine.ENUM_UI_CraftingShop_ItemAttribute.Level:
                _levelid++;
                _descriptionatcenter = true;
                _showstars = false;
                break;
            case UI_TypeDefine.ENUM_UI_CraftingShop_ItemAttribute.Element:
                if(_eleID % 100 < 10)
                    _eleID++;
                _starlv = _eleID % 100;
                break;
            case UI_TypeDefine.ENUM_UI_CraftingShop_ItemAttribute.Enchant:
                if (_enchantID % 100 < 10)
                    _enchantID++;
                _starlv = _enchantID % 100;
                break;
            case UI_TypeDefine.ENUM_UI_CraftingShop_ItemAttribute.Gem:
                if (_gemID % 100 < 10)
                    _gemID++;
                _starlv = _gemID % 100;
                break;
        }

        ItemDropStruct _tempnextlevelinfo = ItemDeployInfo.Instance.GetItemObject(_curlevelinfo._ItemID, _curlevelinfo._PrefabID, _gemID, _enchantID, _eleID, _levelid);;
		
		// check if there is info for these attrs? if no info, that means it reaches the top level or it's kickstarter special attrs
		// show current info
		switch (_attrType)
        {
            case UI_TypeDefine.ENUM_UI_CraftingShop_ItemAttribute.Element:
               	if(_tempnextlevelinfo.info_EleName == null)
				{
					_tempnextlevelinfo.info_EleDesc1 = _curlevelinfo.info_EleDesc1;
					_tempnextlevelinfo.info_EleDesc2 = _curlevelinfo.info_EleDesc2;
					_tempnextlevelinfo.info_eleIconIdx = _curlevelinfo.info_eleIconIdx;
					_tempnextlevelinfo.info_EleName = _curlevelinfo.info_EleName;
					_tempnextlevelinfo.info_EleNameLv = _curlevelinfo.info_EleNameLv;
					_tempnextlevelinfo.info_elePercentVal = _curlevelinfo.info_elePercentVal;
					_tempnextlevelinfo.info_eleVal = _curlevelinfo.info_eleVal;
					_tempnextlevelinfo._EleID = _curlevelinfo._EleID;
					_descriptionatcenter = true;
				 	_showstars = false;
				}
                break;
            case UI_TypeDefine.ENUM_UI_CraftingShop_ItemAttribute.Enchant:
                if(_tempnextlevelinfo.info_EncName == null)
				{
					_tempnextlevelinfo.info_EncDesc1 = _curlevelinfo.info_EncDesc1;
					_tempnextlevelinfo.info_EncDesc2 = _curlevelinfo.info_EncDesc2;
					_tempnextlevelinfo.info_encIconIdx = _curlevelinfo.info_encIconIdx;
					_tempnextlevelinfo.info_EncName = _curlevelinfo.info_EncName;
					_tempnextlevelinfo.info_EncNameLv = _curlevelinfo.info_EncNameLv;
					_tempnextlevelinfo.info_encVal = _curlevelinfo.info_encVal;
					_tempnextlevelinfo._EnchantID = _curlevelinfo._EnchantID;
					_descriptionatcenter = true;
				 	_showstars = false;
				}
                break;
            case UI_TypeDefine.ENUM_UI_CraftingShop_ItemAttribute.Gem:
                if(_tempnextlevelinfo.info_EncName == null)
				{
					_tempnextlevelinfo.info_GemDesc1 = _curlevelinfo.info_GemDesc1;
					_tempnextlevelinfo.info_GemDesc2 = _curlevelinfo.info_GemDesc2;
					_tempnextlevelinfo.info_GemeNameLv = _curlevelinfo.info_GemeNameLv;
					_tempnextlevelinfo.info_gemIconIdx = _curlevelinfo.info_gemIconIdx;
					_tempnextlevelinfo.info_GemName = _curlevelinfo.info_GemName;
					_tempnextlevelinfo.info_gemVal = _curlevelinfo.info_gemVal;
					_tempnextlevelinfo._GemEffectVal = _curlevelinfo._GemEffectVal;
					_tempnextlevelinfo._GemID = _curlevelinfo._GemID;
					_descriptionatcenter = true;
				 	_showstars = false;
				}
                break;
        }
        AttrGroup.AttType = _attrType;
        AttrGroup.UpdateAllInfo(_tempnextlevelinfo);
        
		
		Level_Stars.gameObject.SetActive(_showstars);
        if (_showstars)
            Level_Stars.UpdateLevel(_starlv, 10);
		AttrGroup.ResetDescriptionPosAtCenter(_descriptionatcenter);
    }

    void SetupAllMaterials(UpgradeRecipe _recipe)
    {
		matsMap.Clear();
        CurMaterialCrystalNeed = 0;
        List<UI_CraftingShop_MaterialGroup> _newgroup = new List<UI_CraftingShop_MaterialGroup>();
        int _materialcount = 0;
        if (_recipe.mat1 != 0)
        {
            UI_CraftingShop_MaterialGroup _temp = _getMaterialBTN(0);
            _temp.UpdateallInfo(_recipe.mat1, _recipe.mat1Count, matsMap);
            _newgroup.Add(_temp);
            _materialcount++;
            CurMaterialCrystalNeed += _temp.NeedCrystal();
        }
        if (_recipe.mat2 != 0)
        {
            UI_CraftingShop_MaterialGroup _temp = _getMaterialBTN(1);
            _temp.UpdateallInfo(_recipe.mat2, _recipe.mat2Count, matsMap);
            _newgroup.Add(_temp);
            _materialcount++;
            CurMaterialCrystalNeed += _temp.NeedCrystal();
        }
        if (_recipe.mat3 != 0)
        {
            UI_CraftingShop_MaterialGroup _temp = _getMaterialBTN(2);
            _temp.UpdateallInfo(_recipe.mat3, _recipe.mat3Count, matsMap);
            _newgroup.Add(_temp);
            _materialcount++;
            CurMaterialCrystalNeed += _temp.NeedCrystal();
        }
        if (_recipe.mat4 != 0)
        {
            UI_CraftingShop_MaterialGroup _temp = _getMaterialBTN(3);
            _temp.UpdateallInfo(_recipe.mat4, _recipe.mat4Count, matsMap);
            _newgroup.Add(_temp);
            _materialcount++;
            CurMaterialCrystalNeed += _temp.NeedCrystal();
        }
        for (int i = _materialcount; i < curMaterials.Length; i++ )
        {
            Destroy(curMaterials[i].gameObject);
        }
        curMaterials = _newgroup.ToArray();
        _newgroup.Clear();
        _newgroup = null;
		
		RepositionAllMaterials();
    }

    UI_CraftingShop_MaterialGroup _getMaterialBTN(int _index)
    {
        if (_index < curMaterials.Length)
        {
            return curMaterials[_index];
        }
        else
        {
            UI_CraftingShop_MaterialGroup _newGroup = UnityEngine.Object.Instantiate(MaterialPrefab) as UI_CraftingShop_MaterialGroup;
            _newGroup.transform.parent = MaterialGroupParent.transform;
            _newGroup.transform.localPosition = Vector3.zero;
            _newGroup.transform.localScale = Vector3.one;
            return _newGroup;
        }
    }

    void RepositionAllMaterials()
    {
        float _slotslength = (curMaterials.Length - 1) * (110 + 20);
        for (int i = 0; i < curMaterials.Length; i++)
        {
            Vector3 _tempPos = curMaterials[i].transform.localPosition;
            _tempPos.x = i * (110 + 12) - _slotslength / 2;
            curMaterials[i].transform.localPosition = _tempPos;
        }
    }

#region BTN Callback
    void KarmaBuyBTNClicked()
    {
        byte _belongequip = (byte)(UI_CraftingShop_Manager.Instance.CurItemSlot.IsBelongEquipment ? 1 : 2);
		// because the slot num on server and client is different for inventory, but it's the same for equipments.
		// so if the equip is in inventroy, we need to minus 1 to match the id on server.
        int _slotOffset = UI_CraftingShop_Manager.Instance.CurItemSlot.IsBelongEquipment ? 0 : -1;
        CS_Main.Instance.g_commModule.SendMessage(ProtocolGame_SendRequest.CraftingItem(_belongequip, (uint)(UI_CraftingShop_Manager.Instance.CurItemInfo.slot + _slotOffset), new ECraftType(_currecipe.type), new EMoneyType(EMoneyType.eMoneyType_SK), _currecipe.attrID - 1, matsMap, false));
    }

    void CrystalBuyBTNClicked()
    {
        byte _belongequip = (byte)(UI_CraftingShop_Manager.Instance.CurItemSlot.IsBelongEquipment ? 1 : 2);
		// because the slot num on server and client is different for inventory, but it's the same for equipments.
		// so if the equip is in inventroy, we need to minus 1 to match the id on server.
        int _slotOffset = UI_CraftingShop_Manager.Instance.CurItemSlot.IsBelongEquipment ? 0 : -1;
        CS_Main.Instance.g_commModule.SendMessage(ProtocolGame_SendRequest.CraftingItem(_belongequip, (uint)(UI_CraftingShop_Manager.Instance.CurItemInfo.slot + _slotOffset), new ECraftType(_currecipe.type), new EMoneyType(EMoneyType.eMoneyType_FK), _currecipe.attrID - 1, matsMap, true));
    }

    void OnContinue()
    {
        UpdateAttrInfo(UI_CraftingShop_Manager.Instance.CurItemInfo.localData, _curattrtype);
        ExitWorkingAnimation();
    }

#endregion
}
