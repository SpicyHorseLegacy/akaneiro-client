using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class UI_ChaEquipment_Manager : MonoBehaviour {

    public static UI_ChaEquipment_Manager Instance;

    void Awake()
    {
        Instance = this;
    }

    #region Interface

    [SerializeField]  private ChaInfo_LvGroup_Manager LevelGroup;

    public PlayerModel Model;
	
	public void UpdateCharLevelInfo(int _lv, string _name, float _curexp, float _maxexp)
	{
		LevelGroup.UpdateLvEntireGroup(_lv, _name, _curexp, _maxexp);
	}
	
	[SerializeField]
	public List<InventorySlot> list = new List<InventorySlot>();
	public InventorySlot GetEquipItemData(int solt) {
		foreach(InventorySlot data in list) {
			if(data.GetSlot() == solt) {
				return data;
			}
		}
		return null;
	}
	public int InEquipSlot(Vector3 pos,int type,int slot) {
		foreach(InventorySlot invSlot in list) {
			if(type == 2&&slot == invSlot.GetSlot()) {
				continue;
			}
			float left 	= invSlot.transform.position.x - 0.1f;
			float top	= invSlot.transform.position.y + 0.1f;
			float right	= invSlot.transform.position.x + 0.1f;
			float bootom= invSlot.transform.position.y - 0.1f;
			if(left <= pos.x && top >= pos.y && right >= pos.x && bootom <= pos.y){
				return invSlot.GetSlot();
			}
		}
		return -1;
	}
	
	public bool CheckMainWeaponIsEmpty() {
		foreach(InventorySlot data in list) {
			if(data.GetSlot() == 6) {
				return data.GetEmptyFlag();
			}
		}
		return true;
	}
	public bool CheckOffWeaponIsEmpty() {
		foreach(InventorySlot data in list) {
			if(data.GetSlot() == 7) {
				return data.GetEmptyFlag();
			}
		}
		return true;
	}

    #endregion
}
