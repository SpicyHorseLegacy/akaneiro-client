using UnityEngine;
using System.Collections;



public class TutorialObject : MonoBehaviour {
	
	public string objKey = "";
	
	void Awake() {
		TutorialManager.AddList(gameObject.GetComponent<TutorialObject>());
	}
	
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

#region Local
	#region Hide
	public void Play(bool isHide) {
		//Animation
		if(isHide) {
			PlayAni(Direction.Forward);
			GUILogManager.LogErr("Forward.");
		}else {
			PlayAni(Direction.Reverse);
			GUILogManager.LogErr("Reverse.");
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
#endregion
}
