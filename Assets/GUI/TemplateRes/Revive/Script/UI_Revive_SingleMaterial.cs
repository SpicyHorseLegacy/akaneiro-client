using UnityEngine;
using System.Collections;

public class UI_Revive_SingleMaterial : MonoBehaviour {

	[SerializeField]  private UITexture m_Icon;
	[SerializeField]  private UILabel m_Count;
	
	public void UpdateReviveMaterial(IngameMaterialStruct _materialStruct)
	{
		m_Count.text = "" + _materialStruct.count;
		ItemPrefabs.Instance.GetItemIcon(_materialStruct.data._ItemID, _materialStruct.data._TypeID, _materialStruct.data._PrefabID, m_Icon);
	}
}
