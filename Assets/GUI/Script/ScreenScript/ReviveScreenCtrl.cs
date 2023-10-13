using UnityEngine;
using System.Collections;

public class ReviveScreenCtrl : BaseScreenCtrl
{
    public static ReviveScreenCtrl Instance;

    override protected void Awake() { base.Awake(); Instance = this; }

    #region Event
    protected override void RegisterSingleTemplateEvent(string _templateName)
    {
        base.RegisterSingleTemplateEvent(_templateName);

        if (_templateName == "GameHud_Revive" && UI_Revive_Manager.Instance)
        {
            InitScreen();
        }
        if (_templateName == "KarmaRecharge" && KarmaRechargeManager.Instance)
        {
            KarmaRechargeManager.Instance.OnExitDelegate += ExitRechargeKarmaDelegate;
            KarmaRechargeManager.Instance.OnAddKarmaDelegate += RechargeKarmaValDelegate;
            InitKarmaRechargeData();
        }
    }

    protected override void UnregisterSingleTemplateEvent(string _templateName)
    {
        base.UnregisterSingleTemplateEvent(_templateName);

        if (_templateName == "GameHud_Revive" && UI_Revive_Manager.Instance)
        {
        }
        if (_templateName == "KarmaRecharge" && KarmaRechargeManager.Instance)
        {
            KarmaRechargeManager.Instance.OnExitDelegate -= ExitRechargeKarmaDelegate;
            KarmaRechargeManager.Instance.OnAddKarmaDelegate -= RechargeKarmaValDelegate;
        }
    }
    #endregion

    void InitScreen()
    {
        UI_TypeDefine.UI_Money_data _karma = new UI_TypeDefine.UI_Money_data(UI_TypeDefine.ENUM_UI_Money_Type.Karma);
        int reviveKarma = 0;
        reviveKarma = GetRevivalKarma();

        if (reviveKarma == 0)
        {
            _karma.MoneyString = LocalizeManage.Instance.GetDynamicText("FREE");
        }
        else
        {
            _karma.MoneyString = reviveKarma.ToString();
        }

		float _countdown = GetRevivalCountdown();

		UI_Revive_Manager.Instance.UpdateAllReviveInfo(PlayerDataManager.Instance.MissionScore, PlayerDataManager.Instance.MissionKarma, _karma, null, _countdown);
    }

	float GetRevivalCountdown()
	{
		string _fileName = LocalizeManage.Instance.GetLangPath("RevivalCost.Costs");
		TextAsset item = (TextAsset)Resources.Load(_fileName, typeof(TextAsset));
		string[] itemRowsList = item.text.Split('\n');

		int idx = PlayerDataManager.Instance.GetRevivalCount() + 1;

		string pp = itemRowsList[3];
		string[] vals = pp.Split(new char[] { '	', '	' });
		if ((idx) > 7)
		{
			idx = 7;
		}
		return int.Parse(vals[(idx)]);
	}

    int GetRevivalKarma()
    {
        string _fileName = LocalizeManage.Instance.GetLangPath("RevivalCost.Costs");
        TextAsset item = (TextAsset)Resources.Load(_fileName, typeof(TextAsset));
        string[] itemRowsList = item.text.Split('\n');
        int idx = 0;
        //because first time is 0;//
		idx = PlayerDataManager.Instance.GetRevivalCount() + 1;
        for (int i = 4; i < itemRowsList.Length; ++i)
        {
            string pp = itemRowsList[i];
            string[] vals = pp.Split(new char[] { '	', '	' });
            if (int.Parse(vals[0]) == PlayerDataManager.Instance.CurLV)
            {
                if ((idx) > 7)
                {
                    idx = 7;
                }
                return int.Parse(vals[(idx)]);
            }
        }
        return 0;
    }

    #region Recharge
    private bool isPopUpRecharge = false;
    private void ExitRechargeKarmaDelegate()
    {
        GUIManager.Instance.RemoveTemplate("KarmaRecharge");
        isPopUpRecharge = false;
    }
    private void ExitRechargeCrystalDelegate()
    {
        GUIManager.Instance.RemoveTemplate("CrystalRecharge");
        isPopUpRecharge = false;
    }
    private void RechargeKarmaValDelegate(string content)
    {
        if (Steamworks.activeInstance != null)
        {
            Steamworks.activeInstance.StartPayment(content);
        }
        switch (VersionManager.Instance.GetVersionType())
        {
            case VersionType.WebVersion:
                Application.ExternalCall("select_gold", content);
                break;
            case VersionType.NormalClientVersion:
                StartCoroutine(VersionManager.Instance.SendMsgToServerForPay(content));
                break;
            default:
                StartCoroutine(VersionManager.Instance.SendMsgToServerForPay(content));
                break;
        }
    }

    private void InitKarmaRechargeData()
    {
        KarmaRechargeManager.Instance.SetKarmaInfo(PlayerDataManager.Instance.karmaRechargTitle);
        for (int i = 0; i < 7; i++)
        {
            KarmaRechargeManager.Instance.SetKarmaVal(i, PlayerDataManager.Instance.rechargeValData.karmaVal[i]);
            KarmaRechargeManager.Instance.SetPayVal(i, PlayerDataManager.Instance.rechargeValData.karmaPayVal[i]);
        }
    }
    private void InitCrystalRechargeData()
    {
        CrystalRechargeManager.Instance.SetKarmaInfo(PlayerDataManager.Instance.crystalRechargTitle);
        for (int i = 0; i < 7; i++)
        {
            CrystalRechargeManager.Instance.SetKarmaVal(i, PlayerDataManager.Instance.rechargeValData.crystalVal[i]);
            CrystalRechargeManager.Instance.SetPayVal(i, PlayerDataManager.Instance.rechargeValData.crystalPayVal[i]);
        }
    }
    #endregion
}
