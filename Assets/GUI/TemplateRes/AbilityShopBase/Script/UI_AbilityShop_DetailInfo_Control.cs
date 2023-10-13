using UnityEngine;
using System.Collections;

public class UI_AbilityShop_DetailInfo_Control : MonoBehaviour {

    [SerializeField]  UISprite AbiIcon;
    [SerializeField]  UILabel AbiName;
    [SerializeField]  UI_LevelStars_Manager StarsLevel;
    [SerializeField]  NGUIPanel DescriptionPanel;
    [SerializeField]  UILabel CurAbiDescription;
    [SerializeField]  GameObject CurAbiDescriptionGameObject;
    [SerializeField]  UILabel NextLVAbiDescription;
    [SerializeField]  GameObject NextLevelLine;
    [SerializeField]  UILabel NeedAttrLabel;
    [SerializeField]  UILabel HaveAttrLabel;
    [SerializeField]  UI_CoolDownCycle_Control CoolDownControl;
    [SerializeField]  GameObject KarmaGroup;
    [SerializeField]  UILabel KarmaCostLabel;
    [SerializeField]  GameObject BTNGameObject;
    [SerializeField]  UILabel BTNLabel;
    [SerializeField]  UILabel MaxLevelLabel;

    UI_TypeDefine.UI_AbilityShop_AbiDetail_data _detailData;
	 
    bool isTraining = false;

    public void UpdateAllInfo(UI_TypeDefine.UI_AbilityShop_AbiDetail_data _Data)
    {
        _detailData = _Data;
        AbiIcon.spriteName = _Data.IconSpriteName;
        AbiName.text = _Data.AbiName;
        StarsLevel.UpdateLevel(_Data.AbiCurLV, _Data.AbiMaxLV);

        Vector4 temp = DescriptionPanel.clipRange;
        temp.y = -150;
        DescriptionPanel.clipRange = temp;
        Vector3 temppos = DescriptionPanel.transform.localPosition;
        temppos.y = -76;
        DescriptionPanel.transform.localPosition = temppos;
        Vector3 _pos = new Vector3(-150, 0f, 0);
        if (_Data.AbiCurLV == 0)
        {
            CurAbiDescriptionGameObject.gameObject.SetActive(false);
            NextLevelLine.SetActive(true);
            NextLevelLine.transform.localPosition = _pos;
            _pos.y -= NGUIMath.CalculateRelativeWidgetBounds(NextLevelLine.transform).extents.y * 2 + 5f;
            NextLVAbiDescription.gameObject.SetActive(true);
            NextLVAbiDescription.text = _Data.AbiNextLVDescription;
            NextLVAbiDescription.transform.localPosition = _pos;
        }
        else if (_Data.AbiCurLV >= _Data.AbiMaxLV)
        {
            CurAbiDescriptionGameObject.gameObject.SetActive(true);
            CurAbiDescription.text = _Data.AbiCurLVDescription;
            CurAbiDescriptionGameObject.transform.localPosition = _pos;
            NextLevelLine.SetActive(false);
            NextLVAbiDescription.gameObject.SetActive(false);
        }
        else
        {
            CurAbiDescriptionGameObject.gameObject.SetActive(true);
            CurAbiDescription.text = _Data.AbiCurLVDescription;
            CurAbiDescriptionGameObject.transform.localPosition = _pos;
            _pos.y -= NGUIMath.CalculateRelativeWidgetBounds(CurAbiDescription.transform).extents.y * 2 * CurAbiDescription.transform.localScale.y + 15f;
            NextLevelLine.SetActive(true);
            NextLevelLine.transform.localPosition = _pos;
            _pos.y -= NGUIMath.CalculateRelativeWidgetBounds(NextLevelLine.transform).extents.y * 2 + 5f;
            NextLVAbiDescription.gameObject.SetActive(true);
            NextLVAbiDescription.text = _Data.AbiNextLVDescription;
            NextLVAbiDescription.transform.localPosition = _pos;
        }

        if (_Data.AbiCurLV >= _Data.AbiMaxLV)
        {
			// if the level of ability is the top level. do not need to train. so hide all info to train and show a hint to player.
            NeedAttrLabel.gameObject.SetActive(false);
            HaveAttrLabel.gameObject.SetActive(false);
            CoolDownControl.gameObject.SetActive(false);
            KarmaGroup.SetActive(false);
            BTNGameObject.SetActive(false);
            MaxLevelLabel.gameObject.SetActive(true);
        }
        else
        {
            NeedAttrLabel.gameObject.SetActive(true);
            HaveAttrLabel.gameObject.SetActive(true);
            CoolDownControl.gameObject.SetActive(true);
            KarmaGroup.SetActive(true);
            BTNGameObject.SetActive(true);
            MaxLevelLabel.gameObject.SetActive(false);

            if (_Data.NeedAttr.Length > 0) NeedAttrLabel.text = "[ff0000]Need : [-]" + _Data.NeedAttr; else NeedAttrLabel.text = "";
            if (_Data.HaveAttr.Length > 0) HaveAttrLabel.text = "[ff0000]Have : [-]" + _Data.HaveAttr; else HaveAttrLabel.text = "";
            KarmaCostLabel.text = _Data.KarmaCost.ToString();

            if (isTraining)
            {
                BTNLabel.text = "SPEED UP";
				
				Vector3 _btnpos = BTNGameObject.transform.localPosition;
				_btnpos.x = 0;
				BTNGameObject.transform.localPosition = _btnpos;
				
				KarmaGroup.gameObject.SetActive(false);
            }
            else
            {
                BTNLabel.text = "BEGIN TRAINING";
				
				Vector3 _btnpos = BTNGameObject.transform.localPosition;
				_btnpos.x = 55;
				BTNGameObject.transform.localPosition = _btnpos;
				
				KarmaGroup.gameObject.SetActive(true);
            }
        }
    }

    public void UpdateAbilityCoolDown(bool isshow, bool isstart, UI_TypeDefine.UI_LearnAbilityCoolDown_data _data)
    {
        if (!isshow) { CoolDownControl.gameObject.SetActive(false); return; }
        CoolDownControl.gameObject.SetActive(true);
        CoolDownControl.StopCoolDown();
        CoolDownControl.UpdateAllInfo(_data);
        isTraining = isstart;
        if (isstart)
        {
            CoolDownControl.StartCoolDown();
            BTNLabel.text = "SPEED UP";
			
			Vector3 _btnpos = BTNGameObject.transform.localPosition;
			_btnpos.x = 0;
			BTNGameObject.transform.localPosition = _btnpos;
			
			KarmaGroup.gameObject.SetActive(false);
        }
        else
        {
            BTNLabel.text = "BEGIN TRAINING";
			
			Vector3 _btnpos = BTNGameObject.transform.localPosition;
			_btnpos.x = 55;
			BTNGameObject.transform.localPosition = _btnpos;
			
			KarmaGroup.gameObject.SetActive(true);
        }
    }

    void LearnAndSpeedUpBTNClicked()
    {
        if (_detailData !=null && UI_AbilityShop_Manager.Instance)
        {
            if (!isTraining)
            {
				if(_detailData.IsAbility)
					UI_AbilityShop_Manager.Instance.LearnBTNClicked(_detailData.IsAbility, _detailData.AbiID + _detailData.AbiCurLV + 1);
				else
					UI_AbilityShop_Manager.Instance.LearnBTNClicked(_detailData.IsAbility, _detailData.AbiID);
                
            }else
            {
                if (_detailData.IsAbility)
                    UI_AbilityShop_Manager.Instance.SpeedUpBTNClicked(_detailData.IsAbility, _detailData.AbiID + _detailData.AbiCurLV + 1, (float)CoolDownControl.GetCurCooldown());
                else
                    UI_AbilityShop_Manager.Instance.SpeedUpBTNClicked(_detailData.IsAbility, _detailData.AbiID,  (float)CoolDownControl.GetCurCooldown());
            }
            
        }
    }
}
