using UnityEngine;
using System.Collections;

public class TutorialWolfAnimationEvent : MonoBehaviour {

	public GameObject[] BloodVFX01;
	
	public GameObject[] BloodVFX02;
	
	public GameObject[] BloodVFX03;
	
	void PlayBloodVFXEvent01()
	{
		if(BloodVFX01.Length > 0)
		{
			foreach(GameObject _go in BloodVFX01)
			{
				_go.SetActive(true);
			}
		}
		BloodVFX01 = null;
	}
	
	void PlayBloodVFXEvent02()
	{
		if(BloodVFX02.Length > 0)
		{
			foreach(GameObject _go in BloodVFX02)
			{
				_go.SetActive(true);
			}
		}
		BloodVFX02 = null;
	}
	
	void PlayBloodVFXEvent03()
	{
		if(BloodVFX03.Length > 0)
		{
			foreach(GameObject _go in BloodVFX03)
			{
				_go.SetActive(true);
			}
		}
		BloodVFX03 = null;
	}
}
