using UnityEngine;
using System.Collections;

public class UI_AbilityShop_Main_Abi_Control : MonoBehaviour {

    [SerializeField]  UISprite AbiIcon;
    [SerializeField]  UILabel AbiNameLabel;
    [SerializeField]  UI_LevelStars_Manager StarManager;
    [SerializeField]  UI_CoolDownCycle_Control CoolDownCycle;
	
	UI_TypeDefine.UI_AbiInfo_data abidata;
    UI_TypeDefine.UI_MasteryInfo_data masterydata;

    public void UpdateAllInfo(UI_TypeDefine.UI_AbiInfo_data _data)
    {
        abidata = _data;
        AbiIcon.spriteName = _data.IconSpriteName;
        AbiNameLabel.text = _data.AbiName;
        StarManager.UpdateLevel(_data.Level,6);
    }

    public void UpdateAllInfo(UI_TypeDefine.UI_MasteryInfo_data _data)
    {
        masterydata = _data;
        AbiIcon.spriteName = _data.IconSpriteName;
        AbiNameLabel.text = _data.MasteryName;
        StarManager.UpdateLevel(_data.Level,10);
    }

    public void ResetCoolDown()
    {
        CoolDownCycle.StopCoolDown();
        CoolDownCycle.gameObject.SetActive(false);
    }
	
    public void UpdateCoolDownCycle(UI_TypeDefine.UI_LearnAbilityCoolDown_data _data)
    {
        CoolDownCycle.gameObject.SetActive(true);
        CoolDownCycle.UpdateAllInfo(_data);
        CoolDownCycle.StartCoolDown();
    }

    public UI_TypeDefine.UI_AbiInfo_data GetAbiInfo()
    {
        return abidata;
    }

    public UI_TypeDefine.UI_MasteryInfo_data GetMasteryInfo()
    {
        return masterydata;
    }

    public void Play_Ani_Pop()
    {
        StarManager.PopAllStars();
    }

	public void Beselected()
	{
		GetComponent<UIImageButton>().normalSprite = "Bar_2";
		GetComponent<UIImageButton>().target.spriteName = "Bar_2";
        Play_Ani_Pop();
	}
	
	public void Unselected()
	{
		GetComponent<UIImageButton>().normalSprite = "Bar_1";
		GetComponent<UIImageButton>().target.spriteName = "Bar_1";
	}
	
    #region BTN callback
    void BTNClicked()
	{
		if(UI_AbilityShop_Manager.Instance)
		{
            if (abidata != null)
                UI_AbilityShop_Manager.Instance.AbilityItemBTNClicked(true, (int)abidata.AbiType, this);
            if (masterydata != null)
            {
                UI_AbilityShop_Manager.Instance.AbilityItemBTNClicked(false, masterydata.MasteryType.Get(), this);
            }
			
			Beselected();
		}
    }
    #endregion
}
