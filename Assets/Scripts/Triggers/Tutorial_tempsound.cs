using UnityEngine;
using System.Collections;

public class Tutorial_tempsound : MonoBehaviour {
	
	public Transform[] TempSounds;
	
	void OnTriggerEnter(Collider other)
	{
		if(other.transform == Player.Instance.transform)
		{
			PlayTempSound();
			Destroy(gameObject);
		}
	}
	
	void PlayTempSound()
	{
		if(TempSounds.Length > 0)
		{
			foreach(Transform _temp in TempSounds)
			{
				SoundCue.Play(_temp);
			}
		}
	}
}
