using UnityEngine;
using System.Collections;

public class GameConfig : MonoBehaviour {

    static public GameConfig Instance
    {
        get
        {
            if (_instance)
                return _instance;
            else
            {
                GameObject _obj = new GameObject("GameConfig");
                _obj.AddComponent<GameConfig>();
                return _obj.GetComponent<GameConfig>();
            }
        }
        set
        {
            _instance = value;
        }
    }
    static GameConfig _instance = null;

    private const string autoAtkStr = "autoAtk";
    public static bool IsAutoAttack
    {
        get
        {
            return PlayerPrefs.GetInt(autoAtkStr) == 1 ? true : false;
        }
        set
        {
            PlayerPrefs.SetInt(autoAtkStr, value ? 1 : 0);
        }
    }

    private const string autoFadeLootNames = "fadeLootName";
    public static bool IsAutoFadeLootNames
    {
        get
        {
            return PlayerPrefs.GetInt(autoFadeLootNames) == 1 ? true : false;
        }
        set
        {
            PlayerPrefs.SetInt(autoFadeLootNames, value ? 1 : 0);
        }
    }

    private const string fullScreen = "fullScreen";
    public static bool IsFullScreen
    {
        get
        {
            return PlayerPrefs.GetInt(fullScreen) == 1 ? true : false;
        }
        set
        {
            PlayerPrefs.SetInt(fullScreen, value ? 1 : 0);
        }
    }

    private const string shadows = "shadows";
    public static bool IsShadows
    {
        get
        {
            return PlayerPrefs.GetInt(shadows) == 1 ? true : false;
        }
        set
        {
            PlayerPrefs.SetInt(shadows, value ? 1 : 0);
        }
    }

    private const string qualityStr = "QUALITY";
    public static string Quality
    {
        get
        {
            return PlayerPrefs.GetString(qualityStr);
        }
        set
        {
            PlayerPrefs.SetString(qualityStr, value);
        }
    }

    private const string _isShowHood = "isShowHood";
    public static bool IsShowHood
    {
        get
        {
            return PlayerPrefs.GetInt(_isShowHood) == 1 ? true : false;
        }
        set
        {
            PlayerPrefs.SetInt(_isShowHood, value ? 1 : 0);
        }
    }

    private const string _isMusic = "ISMUSIC";
    public static bool IsMusic
    {
        get
        {
            return PlayerPrefs.GetInt(_isMusic) == 1 ? true : false;
        }
        set
        {
            PlayerPrefs.SetInt(_isMusic, value ? 1 : 0);

            GameObject[] sounds = GameObject.FindGameObjectsWithTag("MUSIC");
            foreach (GameObject _sound in sounds)
            {
                if (_sound.GetComponent<SoundCue>())
                    _sound.GetComponent<SoundCue>().UpdateVolumn();
            }
        }
    }

    private const string _isSFX = "ISSFX";
    public static bool IsSFX
    {
        get
        {
            return PlayerPrefs.GetInt(_isSFX) == 1 ? true : false;
        }
        set
        {
            PlayerPrefs.SetInt(_isSFX, value ? 1 : 0);

            GameObject[] sounds = GameObject.FindGameObjectsWithTag("SFX");
            foreach (GameObject _sound in sounds)
            {
                if (_sound.GetComponent<SoundCue>())
                    _sound.GetComponent<SoundCue>().UpdateVolumn();
            }
        }
    }

    private const string _isAMB = "ISAMB";
    public static bool IsAMB
    {
        get
        {
            return PlayerPrefs.GetInt(_isAMB) == 1 ? true : false;
        }
        set
        {
            PlayerPrefs.SetInt(_isAMB, value ? 1 : 0);

            GameObject[] sounds = GameObject.FindGameObjectsWithTag("Ambiance");
            foreach (GameObject _sound in sounds)
            {
                if (_sound.GetComponent<SoundCue>())
                    _sound.GetComponent<SoundCue>().UpdateVolumn();
            }
        }
    }

    private const string _musicVolumn = "MUSICVOLUME";
    public static float MusicVolumn
    {
        get
        {
            return PlayerPrefs.GetFloat(_musicVolumn);
        }
        set
        {
            PlayerPrefs.SetFloat(_musicVolumn, value);

            GameObject[] sounds = GameObject.FindGameObjectsWithTag("MUSIC");
            foreach (GameObject _sound in sounds)
            {
                if (_sound.GetComponent<SoundCue>())
                    _sound.GetComponent<SoundCue>().UpdateVolumn();
            }
        }
    }

    private const string _sfxVolumn = "SFXVOLUME";
    public static float SFXVolumn
    {
        get
        {
            return PlayerPrefs.GetFloat(_sfxVolumn);
        }
        set
        {
            PlayerPrefs.SetFloat(_sfxVolumn, value);

            GameObject[] sounds = GameObject.FindGameObjectsWithTag("SFX");
            foreach (GameObject _sound in sounds)
            {
                if (_sound.GetComponent<SoundCue>())
                    _sound.GetComponent<SoundCue>().UpdateVolumn();
            }
        }
    }

    private const string _ambVolumn = "AMBVOLUME";
    public static float AMBVolumn
    {
        get
        {
            return PlayerPrefs.GetFloat(_ambVolumn);
        }
        set
        {
            PlayerPrefs.SetFloat(_ambVolumn, value);

            GameObject[] sounds = GameObject.FindGameObjectsWithTag("Ambiance");
            foreach (GameObject _sound in sounds)
            {
                if (_sound.GetComponent<SoundCue>())
                    _sound.GetComponent<SoundCue>().UpdateVolumn();
            }
        }
    }

    void Awake()
    {
        Instance = this;
        if (PlayerPrefs.GetInt("ISSETDEFAULTSOUNDVOLUME") == 0)
        {
            SetDefault();
        }
    }

    public static void SetDefault()
    {
        IsAutoAttack = true;
        IsAutoFadeLootNames = true;
        IsFullScreen = false;
        IsShadows = true;

        IsMusic = true;
        IsSFX = true;
        IsAMB = true;
        MusicVolumn = 0.7f;
        SFXVolumn = 1f;
        AMBVolumn = 0.8f;

        PlayerPrefs.SetInt("ISSETDEFAULTSOUNDVOLUME", 1);
    }
}
