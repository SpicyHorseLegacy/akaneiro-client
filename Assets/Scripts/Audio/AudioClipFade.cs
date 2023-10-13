using UnityEngine;
using System.Collections;

public class AudioClipFade : MonoBehaviour {
	
	bool isGO = false;
	
	AudioSource audiosource;
	
	bool isDestroyThis = false;
	bool isDestroyGO = false;
	float target_vol = 1;
	float time = 0;
	
	
	// Update is called once per frame
	void Update () {
		if(isGO)
		{
            float _targetVolFactor = 0;
            if (gameObject.tag == "MUSIC" && GameConfig.IsMusic)
                _targetVolFactor = GameConfig.MusicVolumn;
            else if (gameObject.tag == "SFX" && GameConfig.IsSFX)
                _targetVolFactor = GameConfig.SFXVolumn;
            else if (gameObject.tag == "AMB" && GameConfig.IsAMB)
                _targetVolFactor = GameConfig.AMBVolumn;
            float _tempTargetVol = target_vol * _targetVolFactor;

            audiosource.volume = Mathf.Lerp(audiosource.volume, _tempTargetVol, Time.deltaTime / time);
			
			time -= Time.deltaTime;
			if(time < 0){
				isGO = false;
				if(isDestroyThis)
					Destroy(this);
				if(isDestroyGO)
					Destroy(gameObject);
			}
		}
	}
	
	/// <summary>
	/// 根据时间，目标音量，fade结束是否删除当前脚本，fade结束是否删除gameobject开始播放
	/// </summary>
	/// <param name='t'>
	/// Fade时间
	/// </param>
	/// <param name='mv'>
	/// 目标音量
	/// </param>
	/// <param name='dthis'>
	/// 是否结束fade后摧毁当前脚本
	/// </param>
	/// <param name='dGO'>
	/// 是否结束fade后摧毁当前物体
	/// </param>
	public void GO(float t, float mv, bool dthis, bool dGO)
	{
		isGO = true;
		audiosource = transform.GetComponent<AudioSource>();
		//if(mv > 0)
			//audiosource.volume = 0;
		//audiosource.Play();
		
		isDestroyThis = dthis;
		isDestroyGO = dGO;
        target_vol = mv;
		
		time = t;
	}
}
