using UnityEngine;
using System.Collections;

public class InGameScreenMailBoxCtrl : BaseScreenCtrl {

	public static InGameScreenMailBoxCtrl Instance;
	
	override protected void Awake() { base.Awake(); Instance = this; }
	
	#region Events
    protected override void RegisterSingleTemplateEvent(string _templateName)
    {
		base.RegisterSingleTemplateEvent(_templateName);

		if (_templateName == "Shop_Mail" && UI_Mailbox_Manager.Instance)
        {
			UI_Mailbox_Manager.Instance.MailCloseClicked_Event += CloseMail;
			
			initMails();
			
			Player.Instance.FreezePlayer();
		}

        if (_templateName == "FoodSlot" && FoodSoltManager.Instance)
        {
            UpdateFoodSlot();
        }
	}
	
	protected override void UnregisterSingleTemplateEvent(string _templateName)
    {
		base.UnregisterSingleTemplateEvent(_templateName);
		if (_templateName == "Shop_Mail" && UI_Mailbox_Manager.Instance)
        {
			UI_Mailbox_Manager.Instance.MailCloseClicked_Event -= CloseMail;
		}
	}
	#endregion

    #region Food Bar...
    private static Texture2D emptyImg = null;

    public void UpdateFoodSlot()
    {
        if (!FoodSoltManager.Instance)
        {
            return;
        }

        if (emptyImg == null)
        {
            emptyImg = new Texture2D(1, 1, TextureFormat.ARGB32, false);
            emptyImg.SetPixel(0, 0, new Color(1f, 1f, 1f, 0f));
            emptyImg.Apply();
        }

        int idx = -1;
        for (int i = 0; i < 3; i++)
        {
            InventorySlot info = FoodSoltManager.Instance.GetFoodItemData(i + 1);
            idx = PlayerDataManager.Instance.foodList[i];
            if (idx != -1)
            {
                _ItemInfo data = PlayerDataManager.Instance.GetBagItemData(idx);
                if (!data.empty)
                {
                    info.SetEmptyFlag(false);
                    info.SetData(data);
                    ItemPrefabs.Instance.GetItemIcon(data.localData._ItemID, data.localData._TypeID, data.localData._PrefabID, info.GetIcon());
                    info.ChangeBGColor(PlayerDataManager.Instance.GetNameColor(data.localData.info_eleVal + data.localData.info_encVal + data.localData.info_gemVal));
                    info.SetCount(data.count);
                }
                else
                {
                    PlayerDataManager.Instance.EmptyFoodSlot(i);
                    info.EmptySlot(emptyImg);
                }
            }
            else
            {
                info.EmptySlot(emptyImg);
            }
        }
    }
    #endregion


	void CloseMail()
	{
        if (GUIManager.Instance != null)
            GUIManager.Instance.ChangeUIScreenState("IngameScreen");

        Player.Instance.ReactivePlayer();
	}
	
	void initMails()
	{
		UI_Mailbox_Manager.Instance.UpdateList(PlayerDataManager.Instance.MailList);
	}
}
