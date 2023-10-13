using UnityEngine;
using System.Collections;

public class AbiInfo_SingleAbi_Control : MonoBehaviour
{
    #region Interface
	
	public AbilityDetailInfo.EnumAbilityType BelongType;
    [SerializeField]  private UISprite Abi_Icon;
	
	public string AbiIconName {get {return Abi_Icon.spriteName;}}
	public AbilityDetailInfo AbilityInfo {get{return _abilityInfo;}}
	AbilityDetailInfo _abilityInfo;
	
    public void UpdateInfo()
	{
		PlayerAbilityManager _tempManager = (PlayerAbilityManager)Player.Instance.abilityManager;
		Debug.Log("Type : " + BelongType.ToString());
		PlayerAbilityBaseState _ability = _tempManager.GetAbilityByType(BelongType);
		Debug.Log("GetAbilityState");
		SetIconLearnedOrNot(_ability.Level > 0);
		_abilityInfo = _ability.Info;
	}
	
	void SetIconLearnedOrNot(bool _islearned)
	{
		if(_islearned)
		{
			Color _tempColor = Abi_Icon.color;
			_tempColor.a = 1;
			Abi_Icon.color = _tempColor;
		}else
		{
			Color _tempColor = Abi_Icon.color;
			_tempColor.a = 0.15f;
			Abi_Icon.color = _tempColor;
		}
	}

    #endregion

    #region local

    void BTNPressed()
    {
        UI_AbiInfo_Manager.Instance.BTNPreesed(this);
    }

    void BTNClicked()
    {
        //UI_AbiInfo_Manager.Instance.BTNClicked(this);
		UI_AbiInfo_Manager.Instance.UpdateDetailInfoForAbility(BelongType, Abi_Icon.spriteName);
    }

    #endregion
}
