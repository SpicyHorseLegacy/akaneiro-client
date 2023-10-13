using UnityEngine;
using System.Collections;

public enum SoundState
{
	Null,
	Ambient,
	Intermediate,
	Danger,
}

public class AdaptiveSoundCue : MonoBehaviour {

	public string Name;
	public AudioClip AmbientClip;
	public AudioClip IntermediateClip;
	public AudioClip DangerClip;
	public float FadeInDuration = 0f;
	public float FadeOutDuration = 0f;
	public static int playBackTime = 0;

	[HideInInspector]
	public SoundState curState;
	[HideInInspector]
	public SoundState nextState;
	[HideInInspector]
	public bool fadeouting = false;
	[HideInInspector]
	public bool fadeining = false;

	public static void Play(GameObject emitter, string soundName, SoundState newState)
	{
		emitter.audio.Pause();
		var soundSets = emitter.GetComponents<AdaptiveSoundCue>();
		if (soundSets.Length == 0)
		{
			Debug.LogError("No AdaptiveSoundCue found on this gameobject.");
			return;
		}

		foreach (var item in soundSets)
		{
			if (item.Name == soundName)
			{
				if (emitter.audio.isPlaying)
				{
					if (item.curState != newState)
					{
						item.fadeouting = true;
						item.fadeining = false;
						item.nextState = newState;
					}
				}
				else
				{
					emitter.audio.volume = 0.0f;
					item.fadeining = true;
					item.fadeouting = false;
					item.curState = newState;
					item.nextState = newState;
					emitter.audio.clip = GetNextClip(item, newState);
					emitter.audio.loop = true;
					emitter.audio.timeSamples = playBackTime;
					emitter.audio.Play();
				}					
				break;
			}
		}
	}

	
	// Use this for initialization
	void Start () {
		fadeouting = false;
		fadeining = false;
	}

	private static AudioClip GetNextClip(AdaptiveSoundCue item, SoundState state)
	{
		if (state == SoundState.Ambient)
		{
			return item.AmbientClip;
		}
		else if (state == SoundState.Intermediate)
		{
			return item.IntermediateClip;
		}
		else
		{
			return item.DangerClip;
		}
	}

	private AudioClip GetNextClip(SoundState state)
	{
		if (state == SoundState.Ambient)
		{
			return AmbientClip;
		}
		else if (state == SoundState.Intermediate)
		{
			return IntermediateClip;
		}
		else  
		{
			return DangerClip;
		}
	}

	// Update is called once per frame
	void Update () {
		if (fadeouting)
		{
			audio.volume -= Time.deltaTime/FadeOutDuration;
			if (audio.volume <= 0.0f)
			{
				audio.volume = 0.0f;
				fadeouting = false;
				fadeining = true;
				audio.clip = GetNextClip(nextState);				
				audio.loop = true;
				if(playBackTime > 0)
				{
					audio.timeSamples = playBackTime;
				}
				audio.Play();
				curState = nextState;
			}
			
		}
		else if (fadeining)
		{
			audio.volume += Time.deltaTime / FadeInDuration;
			if (audio.volume >= 1.0f)
			{
				audio.volume = 1.0f;
				fadeouting = false;
				fadeining = false;
			}
		}
		
		if(audio.timeSamples > 0)
		{
			playBackTime = audio.timeSamples;
		}
	}
}
