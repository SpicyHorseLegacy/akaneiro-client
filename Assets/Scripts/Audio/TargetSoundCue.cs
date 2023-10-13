using UnityEngine;
using System.Collections;


public class TargetSoundCue : SoundCue
{	
	public Transform SFXSetting;

	public static void Play(GameObject emitter, string SFXname)
	{
		var soundSets = emitter.GetComponentsInChildren<TargetSoundCue>();
		if (soundSets.Length == 0)
		{
			Debug.LogError("No SoundCue found on " + emitter.name);
			return;
		}

		Transform spawnSetting = null;
		foreach (var item in soundSets)
		{
			if (item.Name == SFXname)
			{
				spawnSetting = (Transform)Instantiate(item.SFXSetting);
				spawnSetting.transform.position = emitter.transform.position;
				spawnSetting.audio.clip = item.Randomize ? item.AudioClips[Random.Range(0, item.AudioClips.Length)] : item.AudioClips[0];
				break;
			}
		}


	}
	
	/*public static void Play(GameObject emitter, string SFXname, bool isLoop)
	{
		var soundSets = emitter.GetComponentsInChildren<TargetSoundCue>();
		if (soundSets.Length == 0)
		{
			Debug.LogError("No SoundCue found on " + emitter.name);
			return;
		}

		Transform spawnSetting = null;
		foreach (var item in soundSets)
		{
			if (item.Name == SFXname)
			{
				spawnSetting = (Transform)Instantiate(item.SFXSetting);
				spawnSetting.transform.position = emitter.transform.position;
				spawnSetting.audio.clip = item.Randomize ? item.AudioClips[Random.Range(0, item.AudioClips.Length)] : item.AudioClips[0];
				break;
			}
		}

		if (spawnSetting)
		{
			if(emitter.GetComponent<Bullet>())
				emitter.GetComponent<Bullet>().inPlayingSounds.Add(spawnSetting);
			else if(emitter.GetComponent<Player>())
				emitter.GetComponent<Player>().inPlayingSounds.Add(spawnSetting);
			else if(emitter.GetComponent<Item>())
				emitter.GetComponent<Item>().inPlayingSounds.Add(spawnSetting);
			else if(emitter.GetComponent<GameUI>())
				emitter.GetComponent<GameUI>().inPlayingSounds.Add(spawnSetting);
			else if(emitter.GetComponent<Zombie>())
				emitter.GetComponent<Zombie>().inPlayingSounds.Add(spawnSetting);
			else if(emitter.GetComponent<PickupObject>())
				emitter.GetComponent<PickupObject>().inPlayingSounds.Add(spawnSetting);
			
			if(isLoop)
			{
				spawnSetting.audio.loop = true;
				spawnSetting.audio.Play();
			}
			else
			{
				spawnSetting.audio.loop = false;
				spawnSetting.audio.Play();
				Destroy(spawnSetting.gameObject, spawnSetting.audio.clip.length);
			}
		}
	}*/
	
	public static void Play(GameObject emitter, string SFXname, Vector3 SoundLoc)
	{
		var soundSets = emitter.GetComponentsInChildren<TargetSoundCue>();
		if (soundSets.Length == 0)
		{
			Debug.LogError("No SoundCue found on " + emitter.name);
			return;
		}

		Transform spawnSetting = null;
		foreach (var item in soundSets)
		{
			if (item.Name == SFXname)
			{
				spawnSetting = (Transform)GameObject.Instantiate(item.SFXSetting);
				spawnSetting.transform.position = SoundLoc;
				spawnSetting.audio.clip = item.Randomize ? item.AudioClips[Random.Range(0, item.AudioClips.Length)] : item.AudioClips[0];
				break;
			}
		}

	}
	
	/*public static void Play(GameObject emitter, string SFXname, Vector3 SoundLoc, bool isLoop)
	{
		var soundSets = emitter.GetComponentsInChildren<TargetSoundCue>();
		if (soundSets.Length == 0)
		{
			Debug.LogError("No SoundCue found on " + emitter.name);
			return;
		}

		Transform spawnSetting = null;
		foreach (var item in soundSets)
		{
			if (item.Name == SFXname)
			{
				spawnSetting = (Transform)GameObject.Instantiate(item.SFXSetting);
				spawnSetting.transform.position = SoundLoc;
				spawnSetting.audio.clip = item.Randomize ? item.AudioClips[Random.Range(0, item.AudioClips.Length)] : item.AudioClips[0];
				break;
			}
		}

		if (spawnSetting)
		{
			if(emitter.GetComponent<Bullet>())
				emitter.GetComponent<Bullet>().inPlayingSounds.Add(spawnSetting);
			else if(emitter.GetComponent<Player>())
				emitter.GetComponent<Player>().inPlayingSounds.Add(spawnSetting);
			else if(emitter.GetComponent<Item>())
				emitter.GetComponent<Item>().inPlayingSounds.Add(spawnSetting);
			else if(emitter.GetComponent<GameUI>())
				emitter.GetComponent<GameUI>().inPlayingSounds.Add(spawnSetting);
			else if(emitter.GetComponent<Zombie>())
				emitter.GetComponent<Zombie>().inPlayingSounds.Add(spawnSetting);
			else if(emitter.GetComponent<PickupObject>())
				emitter.GetComponent<PickupObject>().inPlayingSounds.Add(spawnSetting);
			
			if(isLoop)
			{
				spawnSetting.audio.loop = true;
				spawnSetting.audio.Play();
			}
			else
			{
				spawnSetting.audio.loop = false;
				spawnSetting.audio.Play();
				Destroy(spawnSetting.gameObject, spawnSetting.audio.clip.length);
			}
		}
	}*/
	
	public static void Stop(GameObject emitter)
	{
//		ArrayList soundCues = new ArrayList();
//		if(emitter.GetComponent<Bullet>())
//		{
//			if(emitter.GetComponent<Bullet>().inPlayingSounds.Count > 0)
//			{
//				soundCues = emitter.GetComponent<Bullet>().inPlayingSounds;
//				foreach(Transform sound in soundCues)
//				{
//					if(sound)
//					{
//						sound.audio.Stop();
//						Destroy(sound.gameObject);
//					}
//				}
//				emitter.GetComponent<Bullet>().inPlayingSounds.Clear();
//			}
//		}
//		else if(emitter.GetComponent<Player>())
//		{
//			if(emitter.GetComponent<Player>().inPlayingSounds.Count > 0)
//			{
//				soundCues = emitter.GetComponent<Player>().inPlayingSounds;
//				foreach(Transform sound in soundCues)
//				{
//					if(sound)
//					{
//						sound.audio.Stop();
//						Destroy(sound.gameObject);
//					}
//				}
//				emitter.GetComponent<Player>().inPlayingSounds.Clear();
//			}
//		}
//		else if(emitter.GetComponent<Item>())
//		{
//			if(emitter.GetComponent<Item>().inPlayingSounds.Count > 0)
//			{
//				soundCues = emitter.GetComponent<Item>().inPlayingSounds;
//				foreach(Transform sound in soundCues)
//				{
//					if(sound)
//					{
//						sound.audio.Stop();
//						Destroy(sound.gameObject);
//					}
//				}
//				emitter.GetComponent<Item>().inPlayingSounds.Clear();
//			}
//		}
//		else if(emitter.GetComponent<GameUI>())
//		{
//			if(emitter.GetComponent<GameUI>().inPlayingSounds.Count > 0)
//			{
//				soundCues = emitter.GetComponent<GameUI>().inPlayingSounds;
//				foreach(Transform sound in soundCues)
//				{
//					if(sound)
//					{
//						sound.audio.Stop();
//						Destroy(sound.gameObject);
//					}
//				}
//				emitter.GetComponent<GameUI>().inPlayingSounds.Clear();
//			}
//		}
//		else if(emitter.GetComponent<Zombie>())
//		{
//			if(emitter.GetComponent<Zombie>().inPlayingSounds.Count > 0)
//			{
//				soundCues = emitter.GetComponent<Zombie>().inPlayingSounds;
//				foreach(Transform sound in soundCues)
//				{
//					if(sound)
//					{
//						sound.audio.Stop();
//						Destroy(sound.gameObject);
//					}
//				}
//				emitter.GetComponent<Zombie>().inPlayingSounds.Clear();
//			}
//		}
//		else if(emitter.GetComponent<PickupObject>())
//		{
//			if(emitter.GetComponent<PickupObject>().inPlayingSounds.Count > 0)
//			{
//				soundCues = emitter.GetComponent<PickupObject>().inPlayingSounds;
//				foreach(Transform sound in soundCues)
//				{
//					if(sound)
//					{
//						sound.audio.Stop();
//						Destroy(sound.gameObject);
//					}
//				}
//				emitter.GetComponent<PickupObject>().inPlayingSounds.Clear();
//			}
//		}
	}
	
	// Use this for initialization
	void Start()
	{
		if (SFXSetting == null)
		{
//			Debug.LogError("No SFXSetting for " + gameObject.name);
		}
	}

	// Update is called once per frame
	void Update()
	{

	}
}

