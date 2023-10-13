using UnityEngine;
using System.Collections;

public class SummonPlayer : MonoBehaviour {
	
	public SpriteText			Name;
	public SpriteText			Level;
	public UIProgressBar 		ExpBar;
	public UIButton 			Icon;
	
	public UIPanel				MyPanel;
	
	public _UI_CS_DownLoadPlayerForSum Model;
	public SurveillanceCamera 	SCamera;
	
	public Transform			PosObj;
	
	public bool 				IsShow = false;
	
	public SFriendCharInfo		MyInfo;
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	 
	public void SetFriendModelInfo(SFriendCharInfo info){
		MyInfo = info;
		Name.Text = info.nickname;
		Level.Text = info.level.ToString();
		long maxExp = (long)_PlayerData.Instance.ReadMaxExpVal(info.level);
		long curexp	= _PlayerData.Instance.readCurExpVal(info.level);
		ExpBar.Value = ((float)((long)info.Exp-curexp) / (float)(maxExp-curexp));
		Icon.SetTexture(_PlayerData.Instance.GetPlayerIcon(info.style,info.sex.Get()));
        Model.EquipManager.DetachAllItems(info.sex);
		foreach(itemuuid equip in info.equipinfo){	
			Transform item = UnityEngine.Object.Instantiate(ItemPrefabs.Instance.GetItemPrefab(equip.itemID,0,equip.prefabID))as Transform;
            if (equip.slotPart == 0 || equip.slotPart == 2 || equip.slotPart == 3 || equip.slotPart == 6 || equip.slotPart == 7 || equip.slotPart == 8)
            {
                SItemInfo _iteminfo = new SItemInfo();
                _iteminfo.gem = equip.gemID;
                _iteminfo.enchant = equip.enchantID;
                Model.EquipManager.UpdateItemInfoBySlot((uint)equip.slotPart, item, _iteminfo, true, info.sex);
            }
		}
        Model.EquipManager.UpdateEquipment(info.sex);
        Model.usingLatestConfig = true;
	}
}
