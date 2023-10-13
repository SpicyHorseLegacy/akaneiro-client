using UnityEngine;
using System.Collections;

public class ObjTweenAlpha : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public float durationTime = 1;
	
	#region Iterface
	public void Play() {
		TweenAlpha.Begin<TweenAlpha>(gameObject,durationTime);
	}
	#endregion
	
	#region Local
	private void ActiveObj(bool isActive) {
		NGUITools.SetActive(gameObject,isActive);
	}
	[SerializeField]
	private Transform dismissDelegateObj;
	public void DismissDelegate() {
		TweenAlpha.Begin<TweenAlpha>(dismissDelegateObj.gameObject,durationTime);
	}
	#endregion
}
