using UnityEngine;
using System.Collections;

//所以没一个事件需要设置事件类型,是否有连续事件以及子事件数量.//

public class TutorialEvent : MonoBehaviour {
	
	public TutorialEventsType eventType = TutorialEventsType.Empty;
	public TutorialEventsType nextEventType = TutorialEventsType.Empty;
	
	// Use this for initialization
	void Start () {
		curBranchCount = 0;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	//当子事件数完成度达到需求时，向管理器发送完成事件消息.//
	[SerializeField]
	private int branchCount = 0;
	[SerializeField]
	private int curBranchCount = 0;
	public void AddCompleteBranch() {
		curBranchCount++;
		if(curBranchCount >= branchCount) {
			TutorialMan.Instance.EndEvent();
		}
	}
	
	public bool CompareEventType(TutorialEventsType ev) {
		if(ev == eventType) {
			return true;
		}
		return false;
	}
	
	public delegate void Handle_InitDelegate();
    public event Handle_InitDelegate OnInitDelegate;
	public void Init() {
		curBranchCount = 0;
		if(OnInitDelegate != null) {
			OnInitDelegate();
		}
	}
	
	public delegate void Handle_EndDelegate();
    public event Handle_InitDelegate OnEndDelegate;
	public void End() {
		if(OnEndDelegate != null) {
			OnEndDelegate();
		}
		if(nextEventType != TutorialEventsType.Empty) {
			TutorialMan.Instance.StartEvent(nextEventType);
		}
	}
}
