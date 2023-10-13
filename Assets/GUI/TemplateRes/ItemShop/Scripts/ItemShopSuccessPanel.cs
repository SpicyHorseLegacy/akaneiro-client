using UnityEngine;
using System.Collections;

public class ItemShopSuccessPanel : MonoBehaviour {

	[SerializeField]  UILabel Label_ItemName;
	[SerializeField]  UITexture ItemIcon;
	[SerializeField]  GameObject SuccessVFX;
	
	public void UpdateBuyItemSucInfo(SBuyitemInfo _info)
	{

		ItemDropStruct tempItem = ItemDeployInfo.Instance.GetItemObject(_info.ID,
		                                                                _info.perfrab,
		                                                                _info.gem,
		                                                                _info.enchant,
		                                                                _info.element,
		                                                                (int)_info.level);
		Label_ItemName.text = tempItem.ItemName;
		Label_ItemName.color = tempItem.TextColor;
		ItemPrefabs.Instance.GetItemIcon(tempItem._ItemID, tempItem._TypeID, tempItem._PrefabID, ItemIcon);
		UnityEngine.Object.Instantiate(SuccessVFX, transform.position, transform.rotation);
	}
	
	void BTN_OK_Clicked()
	{
		Label_ItemName.text = "";
		gameObject.SetActive(false);
	}
}
