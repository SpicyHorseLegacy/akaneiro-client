using UnityEngine;
using System.Collections;

public class AudioTrigger : MonoBehaviour {
    public int ID = 1;
    
	public Transform CombatSFXPrefab;
    public float FadeInTime = 2f;
    public float FadeOutTime = 2f;

	void Update()
	{
		// 如果是绑在怪物身上的，则触发条件不跟距离相关，所以return掉
        //if(transform.GetComponent<BaseObject>())	return;
		
        //float totalDis = Vector3.Distance(transform.position, Player.Instance.transform.position);
		
        //// 如果玩家走进这个区域，则生成一个音效，如果走出这个区域，则fadeout这个音效
        //if( totalDis < Radius && !isPlay )
        //{
        //    triggerEnter();
        //}
		
        //if( totalDis > Radius && isPlay )
        //{
        //    triggerExit();
        //}		
	}

    public virtual void ActiveTrigger() { }

    protected virtual void StartNewBGM() { }

    protected virtual void BackOriginalBGM() { }
}
