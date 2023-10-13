using UnityEngine;
using System.Collections;

public class BuffIcon : MonoBehaviour {
	public int buffID = 0;
	public string name;
	public string description;
	public bool   isShow = false;
	// Use this for initialization
	void Start () {
		gameObject.GetComponent<UIButton>().AddInputDelegate(buffIconDelegate);		
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	void buffIconDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){		
		   case POINTER_INFO.INPUT_EVENT.NO_CHANGE:
				if(isShow)
					BuffTips.Instance.ShowBuffTip(name,description,ItemPosOffestType.RIGHT_BOT,gameObject.transform.position);
				break;
		   case POINTER_INFO.INPUT_EVENT.MOVE_OFF:
		   case POINTER_INFO.INPUT_EVENT.RELEASE_OFF:
				if(isShow)
					BuffTips.Instance.DismissTip();
				break;
		   default:
				break;
		}	
	}
}
