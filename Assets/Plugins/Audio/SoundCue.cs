using UnityEngine;
//using UnityEditor;
using System.Collections;


public class SoundCue : MonoBehaviour {
	public string Name;
	public AudioClip[] AudioClips;
	public bool Randomize = true;

    public bool PlayOnAwake = false;
    public bool IsLoop = false;

    public float _fixedVOL = 0.6f;

    public float VOLUMN
    {
        get
        {
            if (gameObject.tag == "MUSIC")
            {
                if (GameConfig.IsMusic)
                    return _fixedVOL * GameConfig.MusicVolumn;
                else
                    return 0;
            }
            else if (gameObject.tag == "Ambiance")
            {
                if (GameConfig.IsAMB)
                    return _fixedVOL * GameConfig.AMBVolumn;
                else
                    return 0;
            }
            else
            {
                if (GameConfig.IsSFX)
                    return _fixedVOL * GameConfig.SFXVolumn;
                else
                    return 0;
            }
        }

        set
        {
            _fixedVOL = value;
        }
    }

//    [UnityEditor.MenuItem("Tools/FixedVolu")]
//    //[ExecuteInEditMode]
//    static void ResetAllFixedvol()
//    {
//        SoundCue[] sounds = Resources.FindObjectsOfTypeAll(typeof(SoundCue)) as SoundCue[];
//        foreach (SoundCue sound in sounds)
//        {
//            sound.GetComponent<SoundCue>()._fixedVOL = sound.audio.volume;
//        }
//
//    }

    void Awake()
    {
        VOLUMN = audio.volume;
        if (PlayOnAwake)
            Play(gameObject);
    }
	
    //public static void Play( GameObject emitter, string SFXname )
    //{
    //    var soundSets = emitter.GetComponentsInChildren<SoundCue>();
    //    if (soundSets.Length == 0)
    //    {
    //        Debug.LogError("No SoundCue found on " + emitter.name);
    //        return;
    //    }

    //    bool bFound = false;
    //    foreach (var item in soundSets)
    //    {
    //        if (item.Name == SFXname)
    //        {
    //            emitter.audio.clip = item.Randomize ? item.AudioClips[Random.Range(0, item.AudioClips.Length)] : item.AudioClips[0];
    //            bFound = true;
    //            break;
    //        }
    //    }
		
    //    if (bFound)
    //    {
    //        emitter.audio.loop = false;
    //        emitter.audio.Play();
    //    }
    //}
	
    //public static void Play( GameObject emitter, string SFXname, bool isLoop )
    //{
    //    var soundSets = emitter.GetComponentsInChildren<SoundCue>();
    //    if (soundSets.Length == 0)
    //    {
    //        Debug.LogError("No SoundCue found on " + emitter.name);
    //        return;
    //    }

    //    bool bFound = false;
    //    foreach (var item in soundSets)
    //    {
    //        if (item.Name == SFXname)
    //        {
    //            emitter.audio.clip = item.Randomize ? item.AudioClips[Random.Range(0, item.AudioClips.Length)] : item.AudioClips[0];
    //            bFound = true;
    //            break;
    //        }
    //    }
		
    //    if (bFound)
    //    {
    //        if(isLoop)
    //            emitter.audio.loop = true;
    //        else
    //            emitter.audio.loop = false;
    //        emitter.audio.Play();
    //    }
    //}
	
    //public static void Play(GameObject emitter, string SFXname, Vector3 SoundLoc)
    //{
    //    var soundSets = emitter.GetComponentsInChildren<SoundCue>();
    //    if (soundSets.Length == 0)
    //    {
    //        Debug.LogError("No SoundCue found on " + emitter.name);
    //        return;
    //    }

    //    GameObject newObj = null;
    //    foreach (var item in soundSets)
    //    {
    //        if (item.Name == SFXname)
    //        {
    //            newObj = new GameObject();
    //            newObj.AddComponent("AudioSource");
    //            newObj.transform.position = SoundLoc;
    //            newObj.audio.clip = item.Randomize ? item.AudioClips[Random.Range(0, item.AudioClips.Length)] : item.AudioClips[0];
    //            break;
    //        }
    //    }

    //    if (newObj)
    //    {
    //        newObj.audio.loop = false;
    //        newObj.audio.Play();
    //        Destroy(newObj, newObj.audio.clip.length);
    //    }
    //}
	
    //public static void Play(GameObject emitter, string SFXname, Vector3 SoundLoc, bool isLoop)
    //{
    //    var soundSets = emitter.GetComponentsInChildren<SoundCue>();
    //    if (soundSets.Length == 0)
    //    {
    //        Debug.LogError("No SoundCue found on " + emitter.name);
    //        return;
    //    }

    //    GameObject newObj = null;
    //    foreach (var item in soundSets)
    //    {
    //        if (item.Name == SFXname)
    //        {
    //            newObj = new GameObject();
    //            newObj.AddComponent("AudioSource");
    //            newObj.transform.position = SoundLoc;
    //            newObj.audio.clip = item.Randomize ? item.AudioClips[Random.Range(0, item.AudioClips.Length)] : item.AudioClips[0];
    //            break;
    //        }
    //    }

    //    if (newObj)
    //    {
    //        if(isLoop)
    //            newObj.audio.loop = true;
    //        else
    //            newObj.audio.loop = false;
    //        newObj.audio.Play();
    //        Destroy(newObj, newObj.audio.clip.length);
    //    }
    //}
	
	public static void Play(Transform emitter)
	{
        Play(emitter.gameObject);
	}
	
	public static void Play(GameObject emitter)
	{
        PlayAtPosAndRotation(emitter, emitter.transform.position, emitter.transform.rotation);
	}
	
	public static void PlayAtPosAndRotation(Transform emitter, Vector3 pos, Quaternion rot)
	{
		PlayAtPosAndRotation(emitter.gameObject, pos, rot);
	}
	
	public static void PlayAtPosAndRotation(GameObject emitter, Vector3 pos, Quaternion rot)
	{
        if (emitter == null)
            Debug.LogError("[SoundCue]No emitter!");

        var soundSet = emitter.GetComponent<SoundCue>();
        if (soundSet == null)
        {
            Debug.LogWarning("[SoundCue]No SoundCue found on " + emitter.name);
            return;
        }

        emitter.audio.clip = soundSet.Randomize ? soundSet.AudioClips[Random.Range(0, soundSet.AudioClips.Length)] : soundSet.AudioClips[0];
        emitter.audio.loop = soundSet.IsLoop;
        emitter.audio.volume = soundSet.VOLUMN;
        emitter.audio.Play();

        emitter.transform.position = pos;
        emitter.transform.rotation = rot;
	}
	
	public static Transform PlayInstance(GameObject emitter)
	{
		return PlayInstanceAtPosAndRotation(emitter, emitter.transform.position, emitter.transform.rotation);
	}
	
	public static Transform PlayInstanceAtPosAndRotation(Transform emitter, Vector3 pos, Quaternion rot)
	{
		return PlayInstanceAtPosAndRotation(emitter.gameObject, pos, rot);
	}
	
	public static Transform PlayInstanceAtPosAndRotation(GameObject emitter, Vector3 pos, Quaternion rot)
	{
        if (emitter == null)
            Debug.LogError("[SoundCue]No emitter!");

		Transform _temp = Object.Instantiate(emitter.transform, pos, rot) as Transform;
		var soundSet = _temp.GetComponent<SoundCue>();
		
		if (soundSet == null)
        {
            Debug.LogWarning("[SoundCue]No SoundCue found on " + emitter.name);
            return null;
        }
		
        _temp.audio.clip = soundSet.Randomize ? soundSet.AudioClips[Random.Range(0, soundSet.AudioClips.Length)] : soundSet.AudioClips[0];
        _temp.audio.loop = soundSet.IsLoop;
        _temp.audio.volume = soundSet.VOLUMN;
        _temp.audio.Play();
		
		return _temp;
	}

    public static void PlayPrefabAndDestroy(Transform emitterprefab)
    {
		if(emitterprefab != null)
        	PlayPrefabAndDestroy(emitterprefab, emitterprefab.position);
    }

    public static void PlayPrefabAndDestroy(Transform emitterprefab, Vector3 pos)
    {
        if (emitterprefab != null)
        {
            Transform emitter = Instantiate(emitterprefab, pos, Quaternion.identity) as Transform;
            Play(emitter.gameObject);
            emitter.gameObject.AddComponent<DestoryAfterTime>();
            emitter.GetComponent<DestoryAfterTime>().time = emitter.audio.clip.length;
        }
        else
        {
            Debug.LogWarning("[SoundCue]Play an empty sound!");
        }
    }

	public static void Pause(GameObject emitter)
	{
		emitter.audio.Pause();
	}
	
	public static void Stop(GameObject emitter)
	{
		emitter.audio.Stop();
	}
	
	public static void StopAndDestroyInstance(GameObject emitter)
	{
		emitter.audio.Stop();
		Destroy(emitter);
	}

    public void UpdateVolumn()
    {
        audio.volume = VOLUMN;
    }
}

