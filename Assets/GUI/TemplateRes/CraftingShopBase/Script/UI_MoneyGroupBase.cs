using UnityEngine;
using System.Collections;

public class UI_MoneyGroupBase : MonoBehaviour {

    [SerializeField]  protected UISprite MoneySprite;
    [SerializeField]  protected UILabel MoneyLabel;

    public void UpdateIcon(string _spriteName)
    {
        MoneySprite.spriteName = _spriteName;
    }

    public void UpdateMoney(string _money)
    {
        MoneyLabel.text = _money;
    }

    public void UpdateColor(Color _color)
    {
        MoneyLabel.color = _color;
    }

    public virtual void UpdateAllInfo(UI_TypeDefine.UI_Money_data _data)
    {
        UpdateAllInfo(_data.Type == UI_TypeDefine.ENUM_UI_Money_Type.Karma, _data.MoneyString);
    }

    public virtual void UpdateAllInfo(bool IsKarmaOrCrystal, string _money)
    {
        UpdateColor(Color.white);

		if(IsKarmaOrCrystal)
		{           
        	UpdateIcon("Pop_karma1");
		}
        else
		{
			UpdateIcon("Money_Crystal");
		}

        UpdateMoney(_money);
    }

    public virtual void SetCenter()
    {
        Bounds _size = NGUIMath.CalculateRelativeWidgetBounds(transform);
        MoneySprite.transform.localPosition = new Vector3(-_size.extents.x + MoneySprite.transform.localScale.x, 0, 0);
        MoneyLabel.transform.localPosition = new Vector3(-_size.extents.x + MoneySprite.transform.localScale.x, 0, 0);
    }
}
