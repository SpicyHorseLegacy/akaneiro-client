using UnityEngine;
using System.Collections;

public class MissionSuccessStruct
{
    public string missionName;
    public string threatText;
    public int threat;
    public int threatBonusKarma;
    public int threatBonusExp;
    public int missionEarningKarma;
    public int missionEarningExp;
    public int curPlayerLv;
    public long curPlayerExp;
    public long curPlayerExpMax;
    public long prePlayerExpMax;
}

public class SuccessBase : MonoBehaviour
{
    public static SuccessBase Instance;

    void Awake()
    {
        Instance = this;
        GUIManager.Instance.AddTemplateInitEnd();
    }

    #region Interface
    private MissionSuccessStruct curMissInfo;
    public MissionSuccessStruct CurrentMissionInfo { get { return curMissInfo; } }
    public void AwakeMissionSuccess(MissionSuccessStruct info)
    {
        curMissInfo = info;
        SetMissionName(info.missionName);
        SetDifficultyIcon(info.threat);
        SetBonusXp(info.threatBonusExp);
        SetBonusKarma(info.threatBonusKarma);
        SetMissionXp(info.missionEarningExp);
        SetMissionKarma(info.missionEarningKarma);
        SetThreatText(info.threatText);
    }

    #region ScrollViewManager
    [SerializeField]
    private ScrollViewManager scrollViewManager;

    public void AddMaterial(Transform obj)
    {
        scrollViewManager.AddElement(obj);
    }

    public void DelMaterial(Transform transform)
    {
        scrollViewManager.DelElement(transform);
    }

    public void CleanMaterials()
    {
        scrollViewManager.CleanList();
    }
    #endregion
    #endregion

    #region Local
    [SerializeField]
    private UISprite star;
    private void SetStarLightIsOpen(bool isopen)
    {
        NGUITools.SetActive(star.gameObject, isopen);
    }

    [SerializeField]
    private UISprite difficultyIcon;
    private void SetDifficultyIcon(int difficultyId)
    {
        switch (difficultyId)
        {
            case 0://g
                difficultyIcon.color = Color.green;
                break;
            case 1://y
                difficultyIcon.color = Color.yellow;
                break;
            case 2://o
                difficultyIcon.color = new Color(1f, 0.5f, 0f, 1f);
                break;
            case 3://r
                difficultyIcon.color = Color.red;
                break;
        }
    }

    [SerializeField]
    private UILabel threatText;
    private void SetThreatText(string text)
    {
        threatText.text = text;
    }

    [SerializeField]
    private UILabel missionName;
    private void SetMissionName(string name)
    {
        missionName.text = name;
    }

    [SerializeField]
    private UILabel bonusXp;
    private void SetBonusXp(int xp)
    {
        bonusXp.text = xp.ToString();
    }

    [SerializeField]
    private UILabel bonusKarma;
    private void SetBonusKarma(int karma)
    {
        bonusKarma.text = karma.ToString();
    }

    [SerializeField]
    private UILabel missionXp;
    private void SetMissionXp(int xp)
    {
        missionXp.text = xp.ToString();
    }

    [SerializeField]
    private UILabel missionKarma;
    private void SetMissionKarma(int karma)
    {
        missionKarma.text = karma.ToString();
    }

    [SerializeField]
    private UILabel level;
    private void SetLevel(int val)
    {
        if (level != null)
            level.text = val.ToString();
    }

    public delegate void Handle_ThanksDelegate();
    public event Handle_ThanksDelegate OnThanksDelegate;
    private void ThanksDelegate()
    {
        if (OnThanksDelegate != null)
            OnThanksDelegate();
    }
    #endregion
}
