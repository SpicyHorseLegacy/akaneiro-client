using UnityEngine;
using System.Collections;

public class DailyRewardScreenCtrl : BaseScreenCtrl {
	
	public static DailyRewardScreenCtrl Instance;

    override protected void Awake() { base.Awake(); Instance = this; }
	
	protected override void DestoryAllEvent() {
        base.DestoryAllEvent();
		if(DailyrewardManger.Instance) {
			DailyrewardManger.Instance.OnNextDelegate -= NextDelegate;
			DailyrewardManger.Instance.OnThanksDelegate -= ThanksDelegate;
		}
	}

    #region Events
    protected override void RegisterSingleTemplateEvent(string _templateName)
    {
        base.RegisterSingleTemplateEvent(_templateName);

        if (_templateName == "DailyReward" && DailyrewardManger.Instance)
        {
            DailyrewardManger.Instance.OnNextDelegate += NextDelegate;
            DailyrewardManger.Instance.OnThanksDelegate += ThanksDelegate;
            InitDailyRewardList(PlayerDataManager.Instance.dailyWardData.dayID);
        }
    }
    #endregion

    [SerializeField]  private Texture2D karmaIcon;
	[SerializeField]  private Texture2D crystalIcon;
	private ItemDropStruct itemData;
	public void InitDailyRewardList(int curDays){
		int rewardType = 0;TextAsset item;string _fileName;
		switch(PlayerDataManager.Instance.dailyRewarType) {
		case 0:
			_fileName = LocalizeManage.Instance.GetLangPath("DayReward.reward");
			break;
		case 1:
			_fileName = LocalizeManage.Instance.GetLangPath("DayReward.reward2");
			break;
		case 2:
			_fileName = LocalizeManage.Instance.GetLangPath("DayReward.reward3");
			break;
		case 3:
			_fileName = LocalizeManage.Instance.GetLangPath("DayReward.reward4");
			break;
		default:
			_fileName = LocalizeManage.Instance.GetLangPath("DayReward.reward");
			break;
		}
		item = (TextAsset)Resources.Load(_fileName, typeof(TextAsset));
		string[] itemRowsList = item.text.Split('\n');
		for (int i = 3; i < itemRowsList.Length; ++i)
        {
            DailyrewardManger.DailyRewardDayState _isReceivedType = DailyrewardManger.DailyRewardDayState.Unknow;
            if (curDays + 2 >= i)
            {
                _isReceivedType = DailyrewardManger.DailyRewardDayState.Receive;
            }
            if (DailyrewardManger.Instance.InitDayState(i - 3, _isReceivedType) && _isReceivedType == DailyrewardManger.DailyRewardDayState.Receive)
            {
                string pp = itemRowsList[i];
                string[] vals = pp.Split(new char[] { '	', '	' });
                switch (int.Parse(vals[1]))
                {
                    case 1:
                        rewardType = 1;
                        DailyrewardManger.Instance.GetItemIconObj(i - 3).mainTexture = karmaIcon;
                        break;
                    case 2:
                        rewardType = 2;
                        DailyrewardManger.Instance.GetItemIconObj(i - 3).mainTexture = crystalIcon;
                        break;
                    case 4:
                        rewardType = 4;
                        itemData = ItemDeployInfo.Instance.GetItemObject(int.Parse(vals[3]), 1, 0, 0, 0, 1);
                        ItemPrefabs.Instance.GetItemIcon(itemData._ItemID, itemData._TypeID, itemData._PrefabID, DailyrewardManger.Instance.GetItemIconObj(i - 3));
                        break;
                    case 5:
                        rewardType = 5;
                        itemData = ItemDeployInfo.Instance.GetItemObject(int.Parse(vals[3]), int.Parse(vals[4]), int.Parse(vals[5]), int.Parse(vals[7]), int.Parse(vals[6]), int.Parse(vals[8]));
                        ItemPrefabs.Instance.GetItemIcon(itemData._ItemID, itemData._TypeID, itemData._PrefabID, DailyrewardManger.Instance.GetItemIconObj(i - 3));
                        break;
                }
                if (i == (curDays + 2))
                {
                    InitPopUpPanel(rewardType, itemData, int.Parse(vals[2]));
                }
            }
		}
	}
	
	public void InitPopUpPanel(int type,ItemDropStruct data,int count) {
		DailyrewardManger.Instance.HidePopUpPanel(false);
		switch(type){
			case 1:
				DailyrewardManger.Instance.GetCurItemTexture().mainTexture = karmaIcon;
				DailyrewardManger.Instance.SetItemName(count.ToString() + " " + LocalizeManage.Instance.GetDynamicText("KS"));
			break;
			case 2:
				DailyrewardManger.Instance.GetCurItemTexture().mainTexture = crystalIcon;
				DailyrewardManger.Instance.SetItemName(count.ToString() + " " + LocalizeManage.Instance.GetDynamicText("KC"));
			break; 
			case 4:
				ItemPrefabs.Instance.GetItemIcon(data._ItemID,data._TypeID,data._PrefabID,DailyrewardManger.Instance.GetCurItemTexture());
				DailyrewardManger.Instance.SetItemName(data._PropsName);
			break;
			case 5:
				ItemPrefabs.Instance.GetItemIcon(data._ItemID,data._TypeID,data._PrefabID,DailyrewardManger.Instance.GetCurItemTexture());
				DailyrewardManger.Instance.SetItemName(data._PropsName);
			break;
		}
	}
	
	private void NextDelegate() {
		CS_Main.Instance.g_commModule.SendMessage(ProtocolGame_SendRequest.GetFriendReward());
		Debug.Log("************************************");
	}
	private void ThanksDelegate() {
		DailyrewardManger.Instance.HidePopUpPanel(true);
	}
}
