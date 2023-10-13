using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BGManager : MonoBehaviour {

	public static BGManager Instance;
	
	public Transform[] BGS;
	public Transform AttackAudioPrefab;
	
	public float FadeInTime = 0.5f;
	public float FadeOutTime = 4.5f;

	int playingindex = 0;

    //bool isPlayBGM = true;
	public Transform curPlayingBGM;
	Transform attackAudio;
	[HideInInspector]public Transform curOutsideBGM;
	public float curPlayingBGMVol;
	
	List<Transform> AudioTriggers = new List<Transform>();
	
	public void ResetInstance()
	{
		Instance = this;
	}
	
	void Awake () {
		ResetInstance();

        // init random orders of bgms
        if (BGS.Length > 0)
        {
            for (int i = 0; i < BGS.Length; i++)
            {
                int temp = Random.Range(0, BGS.Length);
                if (temp != i)
                {
                    Transform tempclip = BGS[temp];
                    BGS[temp] = BGS[i];
                    BGS[i] = tempclip;
                }
            }
        }

#if NGUI
		_playOriginalBGM(0.1f);
#else
		//if mission complete,stop sound.because mission have background sound.//
		if(MissionComplete.Instance && !MissionComplete.Instance.isCompleteMission) {
			_playOriginalBGM(0.1f);
		}
#endif 
        //isPlayBGM = false;
        if (!attackAudio && AttackAudioPrefab)
        {
            attackAudio = Instantiate(AttackAudioPrefab) as Transform;
            attackAudio.parent = transform;
            attackAudio.audio.volume = 0;
            attackAudio.gameObject.AddComponent<AudioClipFade>();
        }
	}
	
	// Use this for initialization
	void Start () {
	}
	
	void Update()
	{
		if(BGS.Length > 0) {
		
			if(!curPlayingBGM || !curPlayingBGM.audio.isPlaying)
			{
				playingindex ++;
				if(playingindex > BGS.Length - 1)
				{
					playingindex = 0;
				}
                BGMInfo.AutoPlayBGM = true;
                _playOriginalBGM(0.1f);
                BGMInfo.AutoPlayBGM = false;
			}
		}

        if (isPlayingOutsideBGMOnce)
        {
            duration -= Time.deltaTime;
            if (duration < 0)
            {
                ExitOutsideAudio();
                isPlayingOutsideBGMOnce = false;
            }
        }
	}

    void PlayBGMAudio(Transform clip, float fadeinTime)
    {
        _stopOriginalBGM();

        //Debug.LogError("Music");

        curPlayingBGM = Instantiate(clip) as Transform;
        if (curPlayingBGM)
        {
            curPlayingBGM.parent = transform;
            curPlayingBGM.audio.volume = 0;

            float targetVol = clip.audio.volume;
            if (curOutsideBGM || !BGMInfo.AutoPlayBGM) targetVol = 0;
            AudioClipFade fadein = curPlayingBGM.gameObject.GetComponent<AudioClipFade>();
            if (fadein == null)
            {
                fadein = curPlayingBGM.gameObject.AddComponent<AudioClipFade>();
            }
            fadein.GO(fadeinTime, targetVol, true, false);
            curPlayingBGMVol = clip.audio.volume;
            curPlayingBGM.audio.Play();
        }
    }
	
	// If there is an original bgm playing, ignore play command.
	public void PlayOriginalBGIgnoreIfPlaying()
	{
		if(!curPlayingBGM && curPlayingBGM.audio.volume > 0)
			PlayOriginalBG();
	}

    public void PlayOriginalBG()
    {
        PlayOriginalBG(0.1f);
    }

    public void PlayOriginalBG(float fadeinTime)
    {
        ExitOutsideAudio(0.1f);
        //isPlayBGM = true;
        _playOriginalBGM(fadeinTime);
    }

    void _playOriginalBGM(float fadeinTime)
    {
        PlayBGMAudio(BGS[playingindex], fadeinTime);
    }

    public void StopOriginalBG()
    {
        //isPlayBGM = false;
        _stopOriginalBGM();
    }

    void _stopOriginalBGM()
    {
        if (curPlayingBGM)
        {
            AudioClipFade fadeout = curPlayingBGM.gameObject.GetComponent<AudioClipFade>();
            if (fadeout == null)
            {
                fadeout = curPlayingBGM.gameObject.AddComponent<AudioClipFade>();
            }
            fadeout.GO(0.1f, 0, true, true);
            curPlayingBGM = null;
        }
    }

    #region outPort

    bool isPlayingOutsideBGMOnce;
    float duration;

    public void PlayOutsideBGMOnce(Transform clip)
    {
        isPlayingOutsideBGMOnce = true;
        duration = clip.audio.clip.length - 1;

        PlayOutsideBGM(clip);
    }

    // Play audio when somewhere needs to play audio as bgm
    public void PlayOutsideBGM(Transform clip)
    {
		if(clip != null) {
       		PlayOutsideBGM(clip, 2f);
		}
    }

    public void PlayOutsideBGM(Transform clip, float fadeTime)
    {
        PlayOutsideBGM(clip, fadeTime, fadeTime);
    }

    public void PlayOutsideBGM(Transform clip, float fadeinTime, float fadeoutTime)
    {
        if (!clip) return;

        // if there is already has a same clip playing, ignore this message.
        if (curOutsideBGM && curOutsideBGM.audio.clip == clip.audio.clip)
        {
            return;
        }

        // but if there is a different clip playing, fade it out and play a new one.
        if (curOutsideBGM)
        {
            AudioClipFade fadeoutNPC = curOutsideBGM.gameObject.GetComponent<AudioClipFade>();
            if (fadeoutNPC == null)
            {
                fadeoutNPC = curOutsideBGM.gameObject.AddComponent<AudioClipFade>();
            }
            fadeoutNPC.GO(fadeoutTime, 0, true, true);
            curOutsideBGM = null;
        }

        // fade Original BGM out
        if (curPlayingBGM)
        {
            AudioClipFade fadeoutBGM = curPlayingBGM.gameObject.GetComponent<AudioClipFade>();
            if (fadeoutBGM == null)
            {
                fadeoutBGM = curPlayingBGM.gameObject.AddComponent<AudioClipFade>();
            }
            fadeoutBGM.GO(fadeoutTime, 0, true, false);
        }

        curOutsideBGM = Instantiate(clip) as Transform;
        if (curOutsideBGM)
        {
            curOutsideBGM.parent = transform;
            curOutsideBGM.audio.volume = 0;
        }

        AudioClipFade fadein = curOutsideBGM.gameObject.GetComponent<AudioClipFade>();
        if (fadein == null)
        {
            fadein = curOutsideBGM.gameObject.AddComponent<AudioClipFade>();
        }
        fadein.GO(fadeinTime, clip.audio.volume, true, false);
        curOutsideBGM.audio.Play();
    }

    // Somewhere turn the bgm off, resume the original bgm
    public void ExitOutsideAudio()
    {
        ExitOutsideAudio(3f);
    }

    public void ExitOutsideAudio(float fadeTime)
    {
        ExitOutsideAudio(fadeTime, fadeTime);
    }

    public void ExitOutsideAudio(float fadeinTime, float fadeoutTime)
    {
        PlayerPrefs.SetInt("CombatBGMID", 0);
        if (curOutsideBGM)
        {
            AudioClipFade fadein = curOutsideBGM.gameObject.GetComponent<AudioClipFade>();
            if (fadein == null)
            {
                fadein = curOutsideBGM.gameObject.AddComponent<AudioClipFade>();
            }
            fadein.GO(fadeoutTime, 0, true, true);
            curOutsideBGM = null;
        }

        if (curPlayingBGM)
        {
            AudioClipFade fadeout = curPlayingBGM.gameObject.GetComponent<AudioClipFade>();
            if (fadeout == null)
            {
                fadeout = curPlayingBGM.gameObject.AddComponent<AudioClipFade>();
            }
            fadeout.GO(fadeinTime, curPlayingBGMVol, true, false);
        }
    }
    #endregion

    #region AttackAudioControl
    // when combat is started, fade in attack audio.
	void FadeInAttackAudio()
	{
		if(attackAudio)
			attackAudio.GetComponent<AudioClipFade>().GO(FadeInTime, AttackAudioPrefab.audio.volume, false, false);
	}
	
    // when combat is finished, fade out attack audio.
	void FadeOutAttackAudio()
	{
		if(attackAudio)
			attackAudio.GetComponent<AudioClipFade>().GO(FadeOutTime, 0, false, false);
	}
	
    // when player enters a audio trigger, play attack audio.
	public void AudioTriggerEnter(Transform audioTrigger)
	{
		if(AudioTriggers.Count == 0)
		{
			FadeInAttackAudio();
		}
		if(!AudioTriggers.Contains(audioTrigger))
			AudioTriggers.Add(audioTrigger);
	}
	
    // when player exits, stop attack audio.
	public void AudioTriggerExit(Transform audioTrigger)
	{
		AudioTriggers.Remove(audioTrigger);
		
		if(AudioTriggers.Count == 0)
		{
			FadeOutAttackAudio();
		}
    }
    #endregion
}
