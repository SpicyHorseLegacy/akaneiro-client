using UnityEngine;
using System.Collections;

public class AnimationPlayer : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	#region Hide
	private bool isHide = false;
	public void Hide(bool isHide,bool isUseAni) {
		//Animation
		if(isUseAni) {
			this.isHide = isHide;
			if(isHide) {
				PlayAni(Direction.Forward);
			}else {
				PlayAni(Direction.Reverse);
			}
			return;
		}
		//No Animation
		if(isHide) {
			transform.localScale = new Vector3(0.001f,0.001f,0.001f);
		}else {
			transform.localScale = new Vector3(1f,1f,1f);
		}
	}
	private void PlayAni(Direction playDirection) {
		// Update the animation speed based on direction -- forward or back
		Animation mAnim = transform.gameObject.GetComponent<Animation>();
		if(mAnim == null) {
			GUILogManager.LogErr("Animation is null");
			return;
		}
		if (!mAnim.isPlaying) mAnim.Play();
		foreach (AnimationState state in mAnim) {
			float speed = Mathf.Abs(state.speed);
			state.speed = speed * (int)playDirection;
			// Automatically start the animation from the end if it's playing in reverse
			if (playDirection == Direction.Reverse && state.time == 0f) state.time = state.length;
			else if (playDirection == Direction.Forward && state.time == state.length) state.time = 0f;
		}
	}
	#endregion
}
