using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BossAudioControl : MonoBehaviour {

	// Use this for initialization
	
	public List<int> BossTypeIDs = new List<int>();
	
	public float FadeInTimes = 0f;
	
	public float FadeOutTimes = 0f;
	
	[HideInInspector]
	public Transform theTriggger = null;
	[HideInInspector]
	public bool bStartFade = false;
	
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		if(CS_SceneInfo.Instance != null)
		{
			foreach(int BossTypeID in BossTypeIDs)
			{
				if(CS_SceneInfo.Instance.gClientBossTypeID == BossTypeID)
				{
					if(bStartFade)
					{
						if(theTriggger != null)
							theTriggger.gameObject.SetActive(false);
						theTriggger = null;
						
						BGManager.Instance.ExitOutsideAudio(FadeInTimes,FadeOutTimes);
							
						bStartFade = false;
					}
					break;
				}
			}
		}
	}
}
