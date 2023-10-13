using UnityEngine;
using System.Collections;

public class NpcAni : MonoBehaviour {
	
	public string [] aniName;
	private int count = 0;
	public int maxCount = 7;
	
	public GameObject sound;
	
//	public bool 	  isPlaySound = true;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
		if(!gameObject.GetComponent<Animation>().isPlaying){
		
			int idx;
			
			if(count < maxCount){
			
				idx = 0;
				
				if(null != gameObject.GetComponent<NpcAni>().aniName[idx])
					gameObject.animation.CrossFade(gameObject.GetComponent<NpcAni>().aniName[idx]);
				
				if(BGMInfo.Instance && BGMInfo.Instance.isPlayUpGradeEffectSound){
					if(null != sound)
						SoundCue.Play(sound);
					
				}
				
				count++;
				
			}else{
				
				idx 	= 1;
				count 	= 0;
				
				if(null != gameObject.GetComponent<NpcAni>().aniName[idx])
					gameObject.animation.CrossFade(gameObject.GetComponent<NpcAni>().aniName[idx]);

			}
		
		}
		
	}
}
