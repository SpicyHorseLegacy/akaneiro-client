using UnityEngine;
using System.Collections;

public class UI_AbiInfo_DetailGroup : MonoBehaviour {
	
	[SerializeField] GameObject NoneSelectGroup;
	[SerializeField] GameObject DetailGroup;
	[SerializeField] GameObject DragToEquipHintGroup;
	
	[SerializeField] UISprite AbiNameBackground;
	[SerializeField] UILabel AbiNameLabel;
	[SerializeField] UILabel AbiLevelLabel;
	[SerializeField] UISprite AbiIconSprite;
	[SerializeField] UILabel AbiDescription;
	
	AbilityDetailInfo.EnumAbilityType m_tempType;
	Color m_tempColor;
	string  m_tempIconName;
	
	public void FlipToDetail()
	{
		TweenScale.Begin(gameObject, 0.15f, new Vector3(0, 1, 1));
		TweenDelay.Begin(gameObject, 0.15f, "FlipToDetailStep2", null);
	}
	
	void FlipToDetailStep2()
	{
		DetailGroup.gameObject.SetActive(true);
		NoneSelectGroup.gameObject.SetActive(false);
		
		UpdateDetailInfoReal(m_tempType, m_tempColor, m_tempIconName);
		
		TweenScale.Begin(gameObject, 0.15f, new Vector3(1, 1, 1));
	}
	
	public void UpdateDetailInfo(bool _showAni, AbilityDetailInfo.EnumAbilityType _type, Color _color, string _iconName)
	{
		if(m_tempType == _type) return;
		
		m_tempType = _type;
		m_tempColor = _color;
		m_tempIconName = _iconName;
		if(_showAni)
			FlipToDetail();
		else
		{
			DetailGroup.gameObject.SetActive(true);
			NoneSelectGroup.gameObject.SetActive(false);
			UpdateDetailInfoReal(m_tempType, m_tempColor, m_tempIconName);
		}
	}
	
	void UpdateDetailInfoReal(AbilityDetailInfo.EnumAbilityType _type, Color _color, string _iconName)
	{
		PlayerAbilityManager _tempManager = (PlayerAbilityManager)Player.Instance.abilityManager;
		PlayerAbilityBaseState _ability = _tempManager.GetAbilityByType(_type);
		AbiNameBackground.color = _color;
		AbiNameLabel.text = _ability.AbilityName;
		AbiNameLabel.color = _color;
		AbiLevelLabel.text = "Rank " + _ability.Level + " / 10";
		AbiLevelLabel.color = _color;
		AbiIconSprite.spriteName = _iconName;
		
		Color _tempColor = AbiIconSprite.color;
		if(_ability.Level > 0)
		{
			_tempColor.a = 1;
		}else
		{
			_tempColor.a = 0.2f;
		}
		AbiIconSprite.color = _tempColor;
		
		DragToEquipHintGroup.SetActive(_ability.Level > 0);
		
		if(_ability.Info != null)
		{
			string _temptext =_ability.Info.Description1;
            _temptext += "\n[00c6ff]Energy Required : " + _ability.Info.ManaCost + "[-]";
            if (_ability.Info.AddEffectTitle1.Length > 0) _temptext += "\n[ff0000]" + _ability.Info.AddEffectTitle1 +" : [-]";
            if (_ability.Info.AddEffectDesc1.Length > 0) _temptext += "\n" + _ability.Info.AddEffectDesc1;
            if (_ability.Info.AddEffectTitle2.Length > 0) _temptext += "\n[ff0000]" + _ability.Info.AddEffectTitle2 + " : [-]";
            if (_ability.Info.AddEffectDesc2.Length > 0) _temptext += "\n" + _ability.Info.AddEffectDesc2;
			AbiDescription.text = _temptext;
		}else
		{
			AbiDescription.text = "You haven't learnt this Ability.\n\nPlease visit the Ability Trainer to learn this Ability. ";
		}
	}
}
