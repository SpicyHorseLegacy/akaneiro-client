using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UI_Hud_AllyGroup_Manager : MonoBehaviour 
{
	#region addedByMatt
	void BTN_Social_Clicked()
	{
		if(GUIManager.Instance)
			GUIManager.Instance.ChangeUIScreenState("Social_Screen");
	}
	#endregion

    public static UI_Hud_AllyGroup_Manager Instance;

    bool isStart = false;
    double m_currentCooldownValue;

    void Awake()
    {
        Instance = this;
        GUIManager.Instance.AddTemplateInitEnd();
    }

    void Start()
    {
        Init(PlayerDataManager.Instance.GetPlayerInfoBase().style, PlayerDataManager.Instance.Gender.Get(), PlayerDataManager.Instance.CurLV, PlayerDataManager.Instance.CurrentPetId);
        InvokeRepeating("UpdateUI", 0.5f, 0.5f);
    }

    void UpdateUI()
    {
        UpdateLevel(PlayerDataManager.Instance.CurLV);
        UpdatePet(PlayerDataManager.Instance.CurrentPetId);

        if (isStart && m_currentCooldownValue > 0)
        {       
            m_currentPetTimer.text = ConvertTimestampToString((int)m_currentCooldownValue);
        }
        else if (isStart && m_currentCooldownValue < 0)
        {
            isStart = false;
            m_currentCooldownValue = 0;
            m_currentPetTimer.text = "";
        }
    }
    
    string ConvertTimestampToString(int time)
    {
        string _timestring = "";
        _timestring += (time / 3600 >= 10 ? "" : "0") + (Mathf.Clamp(time / 3600, 0, 99)) + ":";
        _timestring += (time % 3600 / 60 >= 10 ? "" : "0") + (time % 3600 / 60);

        return _timestring;
    }

    #region Current Player...
    [SerializeField]
    public UISprite m_currentPlayer;

    [SerializeField]
    public UILabel m_currentPlayerLevel;

    [SerializeField]
    public UISprite m_currentPlayerPet;

    [SerializeField]
    public UILabel m_currentPetTimer;

    public void Init(int type, int sex, int currentLevel, int petId = -1)
    {
        m_currentPlayer.spriteName = PlayerDataManager.Instance.GetPlayerIcon(type, sex);
        m_currentPlayerLevel.text = currentLevel.ToString();

        if (petId != -1)
        {
            m_currentPlayerPet.spriteName = PlayerDataManager.Instance.GetPetIcon(petId);
            m_currentPlayerPet.transform.localScale = new Vector3(40f, 40f, 1f);
            m_currentPlayerPet.enabled = true;
            m_currentPetTimer.enabled = true;
        }
        else
        {
            m_currentPlayerPet.enabled = false;
            m_currentPetTimer.enabled = false;
        }
    }

    public void UpdateLevel(int level)
    {
        m_currentPlayerLevel.text = level.ToString();
    }

    public void UpdatePet(int petId)
    {
        if (petId != -1)
        {
            m_currentPlayerPet.spriteName = PlayerDataManager.Instance.GetPetIcon(petId);
            m_currentPlayerPet.transform.localScale = new Vector3(40f, 40f, 1f);
            m_currentPlayerPet.enabled = true;
            m_currentPetTimer.enabled = true;

            foreach (SinglePetInfo _petinfo in Player.Instance.PetManager.BasePetsInfo.Values)
            {
                if (_petinfo.CurLvPetInfo.m_ID == petId)
                {
                    UI_TypeDefine.UI_SpriteShop_PetItem_data _data = new UI_TypeDefine.UI_SpriteShop_PetItem_data();
                    _data.PetID = _petinfo.CurLvPetInfo.m_ID;
                    _data.BuyTime = (long)_petinfo.BuyTime;
                    _data.MaxTime = (long)_petinfo.LeftTime;

                    m_currentCooldownValue = _data.LastTime;
                    isStart = true;
                    break;
                }
            }
        }
        else
        {
            m_currentPlayerPet.enabled = false;
            m_currentPetTimer.enabled = false;
        }
    }
    #endregion


    #region Interface

    [SerializeField]
    GameObject AllyParent;

    [SerializeField]
    UI_Hud_AllyGroup_Control AllyPrefab;

    [SerializeField]
    UISprite BG;

    [SerializeField]
    GameObject AddAllyBTN;

    List<UI_Hud_AllyGroup_Control> Allies = new List<UI_Hud_AllyGroup_Control>();

    #endregion

    #region Public
    public void UpdateAllAllies(UI_TypeDefine.UI_GameHud_AllyInfo_data[] _alldata)
    {
        CleanAllAllies();

        foreach (UI_TypeDefine.UI_GameHud_AllyInfo_data _data in _alldata)
        {
            AddNewAlly(_data);
        }
    }

    public void AddNewAlly(UI_TypeDefine.UI_GameHud_AllyInfo_data _data)
    {
        Vector3 _pos = Vector3.zero;
        if (Allies.Count > 0) _pos = Allies[Allies.Count - 1].transform.localPosition + Vector3.up * -70f;

        UI_Hud_AllyGroup_Control _newally = UnityEngine.Object.Instantiate(AllyPrefab) as UI_Hud_AllyGroup_Control;
        _newally.transform.parent = AllyParent.transform;
        _newally.transform.localPosition = _pos;
        _newally.UpdateAllInfo(_data);
        Allies.Add(_newally);

        _pos.y -= 70;
        AddAllyBTN.transform.localPosition = _pos;
        BG.transform.localPosition = new Vector3(BG.transform.localPosition.x, _pos.y, BG.transform.localPosition.z);
    }

    public void UpdateAllyHPBar(int ID, float _curhp, float _maxhp)
    {
        foreach (UI_Hud_AllyGroup_Control _ally in Allies)
        {
            if (ID == _ally.DATA.ID)
            {
                _ally.UpdateHP(_curhp, _maxhp);
                return;
            }
        }
    }

    public void UpdateAllyMPBar(int ID, float _curmp, float _maxmp)
    {
        foreach (UI_Hud_AllyGroup_Control _ally in Allies)
        {
            if (ID == _ally.DATA.ID)
            {
                _ally.UpdateMP(_curmp, _maxmp);
                return;
            }
        }
    }


    public void CleanAllAllies()
    {
        for(int i = Allies.Count - 1; i >= 0; i--)
        {
            Destroy(Allies[i].gameObject);
        }
        Allies.Clear();

        AddAllyBTN.transform.localPosition = Vector3.zero;
        BG.transform.localPosition = new Vector3(BG.transform.localPosition.x, 0, BG.transform.localPosition.z);
    }

    #endregion

    #region Delegate

    public delegate void Handle_Hud_AllyInfoGroupAddAllyBTN_Clicked_Delegate();
    public event Handle_Hud_AllyInfoGroupAddAllyBTN_Clicked_Delegate UI_Hud_AllyInfoGroupAddAllyBTN_Clicked_Event;

    #endregion

    #region BTNCallback

    void AddNewAllyBTNClicked()
    {
        if (UI_Hud_AllyInfoGroupAddAllyBTN_Clicked_Event != null)
            UI_Hud_AllyInfoGroupAddAllyBTN_Clicked_Event();
    }

    #endregion

}
