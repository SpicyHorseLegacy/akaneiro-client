using UnityEngine;
using System.Collections;

public class ItemShopObject : MonoBehaviour {
	
	public ItemShopObjData data;
	public bool isSpecial;
	
	[SerializeField]
	private UI_Hud_Border_Control highLight;
	public void HideHighLight(bool isHide) {
		if(highLight == null) {
			GUILogManager.LogErr("highLightObj is null.");
			return;
		}
		if(isHide) {
			NGUITools.SetActive(highLight.gameObject,false);
		}else {
			NGUITools.SetActive(highLight.gameObject,true);
            highLight.ChangeColor((data.localData.info_Level <= PlayerDataManager.Instance.CurLV) ? Color.yellow : Color.red);
			highLight.Pop(1,-1);
		}
	}
	
	[SerializeField]
	private UISprite bg;
	public void SetBGColor(Color color) {
		bg.color = color;
	}
	
	[SerializeField]
	private UITexture icon;
	public UITexture GetIcon() {
		return icon;
	}
	
	[SerializeField]
	private UISprite piceBg;
	//1:crystal 2:karma.
	public void SetPiceType(int type) {
		if(1 == type) {
			piceBg.spriteName = "Item_Money";
		}else if(2 == type) {
			piceBg.spriteName = "Item_Price";
		}
	}
	
	[SerializeField]
	private UILabel piceVal;
	public void SetPice(int pice) {
		piceVal.text = pice.ToString();
	}
	
	[SerializeField]
	private UILabel countVal;
	public void SetCount(int count) {
		if(count == -1) {
			countVal.text = "";
			return;
		}
		countVal.text = count.ToString();
	}
	
	public delegate void Handle_SelectDelegate(ItemShopObjData _data,ItemShopObject obj);
    public event Handle_SelectDelegate OnSelectDelegate;
	private void _SelectDelegate() {
//		GUILogManager.LogErr("_SelectDelegate");
		if(OnSelectDelegate != null) {
			OnSelectDelegate(data,gameObject.GetComponent<ItemShopObject>());
		}
	}
	
	public delegate void Handle_BuyDelegate(ItemShopObjData _data,bool _isSpecial);
    public event Handle_BuyDelegate OnBuyDelegate;
	private void _BuyDelegate() {
//		GUILogManager.LogErr("_BuyDelegate");
		if(OnBuyDelegate != null) {
			OnBuyDelegate(data,isSpecial);
		}
	}
	
	
}
