using UnityEngine;
using System.Collections;

public class BGMInfo : MonoBehaviour {
	
	public static BGMInfo Instance = null;
	public Transform[] BGMS;
	
	public bool isPlayUpGradeEffectSound = true;
	
	// Auto Play BGM when entering a new scene. It works only start persistent scene first time.
	public static bool AutoPlayBGM{
		get
		{
			if(Instance)
				return Instance._autoPlayBGM;
			else
				return true;
		}
		set
		{
			if(Instance)
				Instance._autoPlayBGM = value;
		}
	}
	bool _autoPlayBGM;
	
	Transform curBGM;
	
	void Awake () {
		Instance = this;
		DontDestroyOnLoad(this);
		_autoPlayBGM = true;
	}

    void Start()
    {
        PlayBGMByStageName("EmptyScenes");
		_autoPlayBGM = false;
    }
	
	public void PlayBGMByStageName( string stageName )
	{
		//Debug.LogError("123");
		if(0 == string.Compare(stageName,"EmptyScenes")){
			PlayBGM(BGMS[0]);
			transform.parent = null;
            curBGM.GetComponent<BGManager>().PlayOriginalBG();
		}else{
			PlayBGM(null);
		}

        if (stageName == "Hub_Village")
        {
            PlayerPrefs.SetInt("CombatBGMID", 0);
        }
	}
	
	void PlayBGM( Transform bgm )
	{
		if(curBGM)
			Destroy(curBGM.gameObject);
		
		if(bgm){
			curBGM = Instantiate(bgm) as Transform;
			curBGM.parent = transform;
		}
	}
}
