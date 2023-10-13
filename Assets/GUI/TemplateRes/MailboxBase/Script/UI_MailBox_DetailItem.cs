using UnityEngine;
using System.Collections;

public class UI_MailBox_DetailItem : MonoBehaviour {


    [SerializeField] UILabel Label_name;
    [SerializeField] UILabel Label_Count;
    [SerializeField] UITexture Icon;
    [SerializeField] GameObject CountGroup;
    [SerializeField] GameObject GetBTN;
    [SerializeField] UILabel Label_Got;

    [SerializeField] Texture2D DefaultTex;
    [SerializeField] Texture2D KarmaTex;
    [SerializeField] Texture2D CrystalTex;

    public UI_TypeDefine.UI_Mailbox_Item_data Data { get { return _CurData; } }
    UI_TypeDefine.UI_Mailbox_Item_data _CurData;
	
	// Update is called once per frame
	public void UpdateInfo (UI_TypeDefine.UI_Mailbox_Item_data _data) {
        _CurData = _data;
        Label_name.text = _data.ItemName;
        Label_Count.text = _data.Count.ToString();
        CountGroup.SetActive(true);
        GetBTN.SetActive(true);
        Label_Got.gameObject.SetActive(false);

        if (_data.ItemType == UI_TypeDefine.UI_Mailbox_Item_data.EnumMailItemType.Karma)
        {
            Icon.mainTexture = KarmaTex;
            Icon.transform.localScale = new Vector3(45f, 72f, 1f);
            Label_name.text = LocalizeManage.Instance.GetDynamicText("KS");
            return;
        }
        if (_data.ItemType == UI_TypeDefine.UI_Mailbox_Item_data.EnumMailItemType.Crystal)
        {
            Icon.mainTexture = CrystalTex;
            Icon.transform.localScale = new Vector3(64f, 72f, 1f);
            Label_name.text = LocalizeManage.Instance.GetDynamicText("KC");
            return;
        }
        if (_data.ItemType == UI_TypeDefine.UI_Mailbox_Item_data.EnumMailItemType.Pet)
        {
            if (_data.ItemIcon != null)
            {
                Icon.mainTexture = _data.ItemIcon;
                Icon.transform.localScale = new Vector3(89f, 89f, 1f);
            }
            return;
        }

        Icon.mainTexture = DefaultTex;
        ItemPrefabs.Instance.GetItemIcon(_data.ItemID, _data.ItemTypeID, _data.ItemPrefabID, Icon);
	}

    void GetBTNClicked()
    {
        UI_Mailbox_Manager.Instance.DetailManager.GetBTNClickedFromItem(this);
    }

    public void GetSuc()
    {
        GetBTN.SetActive(false);
        Label_Got.gameObject.SetActive(true);
    }
}