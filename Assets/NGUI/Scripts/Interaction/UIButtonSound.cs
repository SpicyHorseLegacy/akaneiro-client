//----------------------------------------------
//            NGUI: Next-Gen UI kit
// Copyright © 2011-2012 Tasharen Entertainment
//----------------------------------------------

using UnityEngine;

/// <summary>
/// Plays the specified sound.
/// </summary>

[AddComponentMenu("NGUI/Interaction/Button Sound")]
public class UIButtonSound : MonoBehaviour
{
	public enum NGUITrigger
	{
		OnClick,
		OnMouseOver,
		OnMouseOut,
		OnPress,
		OnRelease,
	}
	
	public Transform AudioTransform;
	public AudioClip audioClip;
	public NGUITrigger trigger = NGUITrigger.OnClick;
	public float volume = 1f;
	public float pitch = 1f;
	//----------------------------------------------------------------->>mm
	/*
	void Awake (){
		if (this.gameObject.GetComponent <AudioSource>()){
			this.gameObject.AddComponent <SoundCue>();	
		}
	}
	*/
	//----------------------------------------------------------------->>#mm
	void OnHover (bool isOver)
	{
		if (enabled && ((isOver && trigger == NGUITrigger.OnMouseOver) || (!isOver && trigger == NGUITrigger.OnMouseOut)))
		{
			//NGUITools.PlaySound(audioClip, volume, pitch);
			SoundCue.PlayPrefabAndDestroy(AudioTransform);
		}
	}

	void OnPress (bool isPressed)
	{
		if (enabled && ((isPressed && trigger == NGUITrigger.OnPress) || (!isPressed && trigger == NGUITrigger.OnRelease)))
		{
			//NGUITools.PlaySound(audioClip, volume, pitch);
			SoundCue.PlayPrefabAndDestroy(AudioTransform);
		}
	}

	void OnClick ()
	{
		if (enabled && trigger == NGUITrigger.OnClick)
		{
			//NGUITools.PlaySound(audioClip, volume, pitch);
			SoundCue.PlayPrefabAndDestroy(AudioTransform);
		}
	}
}