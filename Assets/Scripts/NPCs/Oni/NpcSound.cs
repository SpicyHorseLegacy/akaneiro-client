using UnityEngine;
using System.Collections;

public class NpcSound : MonoBehaviour {
	
	public GameObject sound;
	
	public bool 	  isPlaySound = true;
	
	// Use this for initialization
	void Start () {
		SoundCue.PlayPrefabAndDestroy(sound.transform);
	}
	
	// Update is called once per frame
	void Update () {
	}
}
