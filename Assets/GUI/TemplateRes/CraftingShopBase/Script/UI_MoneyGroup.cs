using UnityEngine;
using System.Collections;

public class UI_MoneyGroup : UI_MoneyGroupBase {
    
	
	[SerializeField]  UISprite MoneyBackground;
	
	public override void UpdateAllInfo (bool IsKarmaOrCrystal, string _money)
	{
		base.UpdateAllInfo (IsKarmaOrCrystal, _money);
		
		if(IsKarmaOrCrystal)
		{
            UpdateColor(Color.red);
        	UpdateIcon("Pop_karma1");
            MoneyBackground.spriteName = "Money_Karma_BG";
		}else
		{
            UpdateColor(Color.white);
			UpdateIcon("Money_Crystal");
            MoneyBackground.spriteName = "Money_Crystal_BG";
		}
	}
	
	public override void SetCenter ()
	{
		Bounds _size = NGUIMath.CalculateRelativeWidgetBounds(transform);
        MoneySprite.transform.localPosition = new Vector3(-_size.extents.x + MoneySprite.transform.localScale.x, 0, 0);
        MoneyLabel.transform.localPosition = new Vector3(-_size.extents.x + MoneySprite.transform.localScale.x, 0, 0);
        MoneyBackground.transform.localPosition = Vector3.zero;
        Vector3 _tempsize = MoneyBackground.transform.localScale;
        _tempsize.x = _size.extents.x * 2;
        if (_tempsize.x < 80) _tempsize.x = 80;
        MoneyBackground.transform.localScale = _tempsize;
	}
}
