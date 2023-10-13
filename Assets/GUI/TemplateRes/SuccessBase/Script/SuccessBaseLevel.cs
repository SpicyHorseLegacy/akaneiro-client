using UnityEngine;
using System.Collections;

public class SuccessBaseLevel : MonoBehaviour 
{
    public static SuccessBaseLevel Instance = null;

    [SerializeField]
    public UILabel m_missionNameLabel;

    [SerializeField]
    public UILabel m_playerLevelLabel;

    [SerializeField]
    public NGUISlider m_playerLevelBar;

    [SerializeField]
    public UILabel m_petLevelLabel;

    [SerializeField]
    public NGUISlider m_petLevelBar;

    [SerializeField]
    public GameObject m_congratulationsAnimation = null;

    private MissionSuccessStruct m_currentMissionInfo;

    private float m_animationSpeed = 0.5f;

    private bool m_isPlaying = false;
    private bool m_isLevelingUp = false;
    private bool m_hasLeveledUp = false;

    private float m_previousExperience;
    private float m_previousMaxExperience;
    private float m_currentExperience;
    private float m_currentMaxExperience;
    private float m_previousLevel;

    void Awake()
    {
        Instance = this;
    }

    void Update()
    {
        if (m_isPlaying)
            Animation();
    }

    private void CongratsAnimation(int seconds)
    {
        int i = 0;
        do
        {
            GameObject anim = Instantiate(m_congratulationsAnimation) as GameObject;

            foreach (Transform _t in anim.GetComponentsInChildren<Transform>())
                _t.gameObject.layer = LayerMask.NameToLayer("NGUI"); 

        } while (i++ < seconds);
    }

    private void PlayModelAni()
    {
        string aniName = "";
        int r = Random.Range(0, 3);
        switch (r)
        {
            case 0:
                aniName = "POW_UI_Idle_2";
                PlayModelSound(AbilityDetailInfo.EDisciplineType.EDT_Prowess);
                break;
            case 1:
                aniName = "FOR_UI_Idle_2";
                PlayModelSound(AbilityDetailInfo.EDisciplineType.EDT_Fortitude);
                break;
            case 2:
                aniName = "CUN_UI_Idle_2";
                PlayModelSound(AbilityDetailInfo.EDisciplineType.EDT_Cunning);
                break;
        }

        PlayerAnimation pm = GameObject.FindObjectOfType(typeof(PlayerAnimation)) as PlayerAnimation;

        Animation _ani = null;
        Animation[] allanims = pm.GetComponentsInChildren<Animation>();
        for (int i = 0; i < allanims.Length; i++)
            if (allanims[i].transform.name == "Aka_Model")
                _ani = allanims[i];

        _ani[aniName].layer = 99;
        _ani.CrossFade(aniName);
    }

    private void PlayModelSound(AbilityDetailInfo.EDisciplineType type)
    {
        /* TODO : implement sounds
         * switch (type)
        {
            case AbilityDetailInfo.EDisciplineType.EDT_Prowess:
                if (fortitudeSound.GetComponent<AudioSource>().isPlaying)
                {
                    fortitudeSound.GetComponent<AudioSource>().Stop();
                }
                if (cunningSound.GetComponent<AudioSource>().isPlaying)
                {
                    cunningSound.GetComponent<AudioSource>().Stop();
                }
                SoundCue.PlayPrefabAndDestroy(prowessSound);
                break;
            case AbilityDetailInfo.EDisciplineType.EDT_Fortitude:
                if (prowessSound.GetComponent<AudioSource>().isPlaying)
                {
                    prowessSound.GetComponent<AudioSource>().Stop();
                }
                if (cunningSound.GetComponent<AudioSource>().isPlaying)
                {
                    cunningSound.GetComponent<AudioSource>().Stop();
                }
                SoundCue.PlayPrefabAndDestroy(fortitudeSound);
                break;
            case AbilityDetailInfo.EDisciplineType.EDT_Cunning:
                if (prowessSound.GetComponent<AudioSource>().isPlaying)
                {
                    prowessSound.GetComponent<AudioSource>().Stop();
                }
                if (fortitudeSound.GetComponent<AudioSource>().isPlaying)
                {
                    fortitudeSound.GetComponent<AudioSource>().Stop();
                }
                SoundCue.PlayPrefabAndDestroy(cunningSound);
                break;
        }*/
    }

    private void Animation()
    {
        // TODO : handle several levels up
        // If the player is levelling up, can handle one level up
        if (m_isLevelingUp)
        {
            // Previous level
            if (!m_hasLeveledUp)
            {
                if (m_playerLevelBar.sliderValue < 1f)
                    m_playerLevelBar.sliderValue += m_animationSpeed * Time.deltaTime;
                else
                {
                    m_playerLevelBar.sliderValue = 0f;
                    m_hasLeveledUp = true;
                }
            }
            
            // New Level
            else
            {
                if (m_playerLevelBar.sliderValue < m_currentExperience)
                    m_playerLevelBar.sliderValue += m_animationSpeed * Time.deltaTime;
                else
                    AnimationOver();
            }

            m_playerLevelLabel.text = "Level " + (m_previousLevel + (m_hasLeveledUp ? 1 : 0)) + " (" + Mathf.RoundToInt(m_playerLevelBar.sliderValue * (m_hasLeveledUp ? m_currentMaxExperience : m_previousMaxExperience)) + " / " + (m_hasLeveledUp ? m_currentMaxExperience : m_previousMaxExperience) + ")";
        }

        // No level up
        else
        {
            if (m_playerLevelBar.sliderValue < m_currentExperience)
                m_playerLevelBar.sliderValue += m_animationSpeed * Time.deltaTime;
            else
                AnimationOver();

            m_playerLevelLabel.text = "Level " + m_previousLevel + " (" + Mathf.RoundToInt(m_playerLevelBar.sliderValue * m_currentMaxExperience) + " / " + m_currentMaxExperience + ")";
        }
    }

    private void AnimationOver()
    {
        m_playerLevelBar.sliderValue = m_currentExperience;
        CongratsAnimation(3);
        PlayModelAni();
        m_isPlaying = false;
    }
    
    public void Init(MissionSuccessStruct info)
    {
        m_currentMissionInfo = info;
        
        // Mission Name
        m_missionNameLabel.text = info.missionName;
    
        // Player Experience
        long prePlayerExp = (info.curPlayerExp - info.prePlayerExpMax) - info.threatBonusExp - info.missionEarningExp; // OK
        
        // Is levelling up
        if (prePlayerExp < 0)
        {
            m_isLevelingUp = true;

            m_previousMaxExperience = (float)(info.prePlayerExpMax - PlayerDataManager.Instance.GetMaxExp(info.curPlayerLv - 1));
            m_currentMaxExperience = (float)(info.curPlayerExpMax - info.prePlayerExpMax);

            m_previousExperience = (float)(info.curPlayerExp - info.threatBonusExp - info.missionEarningExp - PlayerDataManager.Instance.GetMaxExp(info.curPlayerLv - 1)) / m_previousMaxExperience;
            m_currentExperience = (float)(info.curPlayerExp - info.prePlayerExpMax) / m_currentMaxExperience;
        }

        // Is not levelling up
        else
        {
            m_isLevelingUp = false;

            m_previousMaxExperience = (float)(info.curPlayerExpMax - info.prePlayerExpMax);
            m_currentMaxExperience = m_previousMaxExperience;

            m_previousExperience = (float)(info.curPlayerExp - info.threatBonusExp - info.missionEarningExp - info.prePlayerExpMax) / m_currentMaxExperience;
            m_currentExperience = (float)(info.curPlayerExp - info.prePlayerExpMax) / m_currentMaxExperience;
        }

        m_playerLevelBar.sliderValue = m_previousExperience;

        // Player Level
        m_previousLevel = (info.curPlayerLv + (m_isLevelingUp ? -1 : 0));

        // TODO : Pet Experience
        // TODO : Pet Level

        m_isPlaying = true;
    }

    public delegate void Handle_ThanksDelegate();
    public event Handle_ThanksDelegate OnThanksDelegate;
    private void ThanksDelegate()
    {
        if (OnThanksDelegate != null)
            OnThanksDelegate();
    }
}
