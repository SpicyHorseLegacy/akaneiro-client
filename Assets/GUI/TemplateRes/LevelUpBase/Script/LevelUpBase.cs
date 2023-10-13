using UnityEngine;
using System.Collections;

public class LevelUpAttribute
{
    public int curPowerVal;
    public int curSkillVal;
    public int curDefenseVal;
    public int curHealthVal;

    public int proPowerVal;
    public int proSkillVal;
    public int proDefenseVal;

    public int forPowerVal;
    public int forSkillVal;
    public int forDefenseVal;

    public int cunPowerVal;
    public int cunSkillVal;
    public int cunDefenseVal;

    public int IncHealthVal;
}

public class LevelUpBase : MonoBehaviour
{
    public static LevelUpBase Instance;

    void Awake()
    {
        Instance = this;
        GUIManager.Instance.AddTemplateInitEnd();
    }

    #region Interface
    private LevelUpAttribute attrInfo;
    public void Init(LevelUpAttribute info)
    {
        attrInfo = info;
        SetAttributeVal(0, 0, 0, 0);
    }

    private int pressedJob = -1;
    public int GetPressedJob()
    {
        return pressedJob;
    }

    public delegate void Handle_UpgradeDelegate();
    public event Handle_UpgradeDelegate OnUpgradeDelegate;
    private void UpgradeDelegate()
    {
        if (OnUpgradeDelegate != null)
        {
            OnUpgradeDelegate();
        }
    }
    #endregion

    #region local

    private void SetAttributeVal(int prowerVal, int skillVal, int defenseVal, int healthVal)
    {
        if (attrInfo == null)
            return;

        SetPowerNewVal(attrInfo.curPowerVal + prowerVal);
        SetPowerUpVal(prowerVal);
        SetSkillNewVal(attrInfo.curSkillVal + skillVal);
        SetSkillUpVal(skillVal);
        SetDefenseNewVal(attrInfo.curDefenseVal + defenseVal);
        SetDefenseUpVal(defenseVal);
        SetHealthNewVal(attrInfo.curHealthVal - (healthVal == 0 ? healthVal : 0));
        SetHealthUpVal(healthVal);
    }

    [SerializeField]
    private UILabel powerNewVal;
    private void SetPowerNewVal(int val)
    {
        powerNewVal.text = val.ToString();
    }

    [SerializeField]
    private UILabel powerUpVal;
    private void SetPowerUpVal(int val)
    {
        powerUpVal.text = val.ToString();
    }

    [SerializeField]
    private UILabel skillNewVal;
    private void SetSkillNewVal(int val)
    {
        skillNewVal.text = val.ToString();
    }

    [SerializeField]
    private UILabel skillUpVal;
    private void SetSkillUpVal(int val)
    {
        skillUpVal.text = val.ToString();
    }

    [SerializeField]
    private UILabel defenseNewVal;
    private void SetDefenseNewVal(int val)
    {
        defenseNewVal.text = val.ToString();
    }

    [SerializeField]
    private UILabel defenseUpVal;
    private void SetDefenseUpVal(int val)
    {
        defenseUpVal.text = val.ToString();
    }

    [SerializeField]
    private UILabel healthNewVal;
    private void SetHealthNewVal(int val)
    {
        healthNewVal.text = val.ToString();
    }

    [SerializeField]
    private UILabel healthUpVal;
    private void SetHealthUpVal(int val)
    {
        healthUpVal.text = val.ToString();
    }
    private void OnProwessDelegate()
    {
        pressedJob = 0;
        SetAttributeVal(attrInfo.proPowerVal, attrInfo.proSkillVal, attrInfo.proDefenseVal, attrInfo.IncHealthVal);
    }

    private void OnFortitudeDelegate()
    {
        pressedJob = 1;
        SetAttributeVal(attrInfo.forPowerVal, attrInfo.forSkillVal, attrInfo.forDefenseVal, attrInfo.IncHealthVal);
    }

    private void OnCunningDelegate()
    {
        pressedJob = 2;
        SetAttributeVal(attrInfo.cunPowerVal, attrInfo.cunSkillVal, attrInfo.cunDefenseVal, attrInfo.IncHealthVal);
    }
    #endregion
}
