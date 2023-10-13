using UnityEngine;
using System.Collections;

public class LevelUpScreenCtrl : BaseScreenCtrl
{
    public static LevelUpScreenCtrl Instance;

    override protected void Awake()
    {
        base.Awake(); 
        Instance = this;
    }

    protected override void DestoryAllEvent()
    {
        base.DestoryAllEvent();
        if (LevelUpBase.Instance)
        {
            LevelUpBase.Instance.OnUpgradeDelegate -= UpgradeDelegate;
        }
    }

    #region Events
    protected override void RegisterSingleTemplateEvent(string _templateName)
    {
        base.RegisterSingleTemplateEvent(_templateName);

        if (_templateName == "LevelUpBase" && LevelUpBase.Instance)
        {
            LevelUpBase.Instance.OnUpgradeDelegate += UpgradeDelegate;
            InitBaseData();
        }
    }
    #endregion

    private void InitBaseData()
    {
        LevelUpAttribute data = new LevelUpAttribute();
        data.curPowerVal = PlayerDataManager.Instance.GetBaseAttrs(EAttributeType.ATTR_Power);
        data.curDefenseVal = PlayerDataManager.Instance.GetBaseAttrs(EAttributeType.ATTR_Defense);
        data.curSkillVal = PlayerDataManager.Instance.GetBaseAttrs(EAttributeType.ATTR_Skill);
        data.curHealthVal = PlayerDataManager.Instance.GetBaseAttrs(EAttributeType.ATTR_MaxHP);

        data.proPowerVal = 30; data.proDefenseVal = 10; data.proSkillVal = 20; 
        data.forPowerVal = 20; data.forDefenseVal = 30; data.forSkillVal = 10; 
        data.cunPowerVal = 10; data.cunDefenseVal = 20; data.cunSkillVal = 30;  

        data.IncHealthVal = PlayerDataManager.Instance.ReadHpIncVal(PlayerDataManager.Instance.CurLV);

        LevelUpBase.Instance.Init(data);
    }

    private void UpgradeDelegate()
    {
        SendAddPointMsg();
    }

    public void SendAddPointMsg()
    {
        switch (LevelUpBase.Instance.GetPressedJob())
        {
            case 0:
                CS_Main.Instance.g_commModule.SendMessage(
                    ProtocolGame_SendRequest.assignTalentReq(1, 0, 0)
                );
                break;
            case 1:
                CS_Main.Instance.g_commModule.SendMessage(
                    ProtocolGame_SendRequest.assignTalentReq(0, 1, 0)
                );
                break;
            case 2:
                CS_Main.Instance.g_commModule.SendMessage(
                    ProtocolGame_SendRequest.assignTalentReq(0, 0, 1)
                );
                break;
            default:
                PopUpBox.PopUpErr(LocalizeManage.Instance.GetDynamicText("CHOOSEDISCIPLINE"));
                break;
        }
    }
}
