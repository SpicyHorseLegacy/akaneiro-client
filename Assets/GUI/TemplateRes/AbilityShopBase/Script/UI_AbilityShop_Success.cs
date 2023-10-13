using UnityEngine;
using System.Collections;

public class UI_AbilityShop_Success : MonoBehaviour {
	[SerializeField]  UILabel Label_ItemName;
	[SerializeField]  UISprite ItemIcon;
	[SerializeField]  GameObject SuccessVFX;
	
	public void UpdateBuySucInfo(string _buyiteminfo, string _iconname)
	{
		Label_ItemName.text = _buyiteminfo;
		ItemIcon.spriteName = _iconname;
		UnityEngine.Object.Instantiate(SuccessVFX, transform.position, transform.rotation);
	}
	
	void BTN_OK_Clicked()
	{
		Label_ItemName.text = "";
		gameObject.SetActive(false);
	}
}
