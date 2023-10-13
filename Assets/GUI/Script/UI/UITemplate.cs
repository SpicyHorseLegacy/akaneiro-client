using UnityEngine;
using System.Collections;

public enum Direction {
	Reverse = -1,
	Toggle = 0,
	Forward = 1,
}

public class UITemplate : MonoBehaviour {

	public string templateName = "";
	public bool isHide = false;
	public bool isDrag = true;
	public STemplateInfo data;
	
	void Awake() {
	}
	
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
//		#region hide
//		//test hide
//		if(Input.GetKeyDown(KeyCode.Alpha1)) {
//			Hide(true,true);
//		}
//		if(Input.GetKeyDown(KeyCode.Alpha2)) {
//			Hide(false,true);
//		}
//		if(Input.GetKeyDown(KeyCode.Alpha3)) {
//			Hide(true,false);
//		}
//		if(Input.GetKeyDown(KeyCode.Alpha4)) {
//			Hide(false,false);
//		}
//		#endregion
	}

#region Interface
	public void Init() {
		if(data != null) {
			this.isDrag = data.isDrag;
			transform.position = data.pos;
			transform.localScale = data.scale;
//			GUILogManager.LogWarn("position :"+data.pos.x+"|"+data.pos.y+"|"+data.pos.z);
			transform.localPosition = data.pos;
		}else {
			GUILogManager.LogErr("Init template fail,data is null");
		}
		Hide(isHide,false);
		InitTemplate();
	}
	
	public delegate void Handle_DestroyDelegate(string _templateName);
    public event Handle_DestroyDelegate OnTemplateDestroy;
	public void DestroyTemplate() {
		if(OnTemplateDestroy != null) {
            OnTemplateDestroy(templateName);
		}
	}
	
	public delegate void Handle_InitDelegate(string _templateName);
    public event Handle_InitDelegate OnTemplateInit;
	public void InitTemplate() {
		if(LocalizeFontManager.Instance) {
			LocalizeFontManager.Instance.ChangeLangFontTarget(transform);
		}
		if(OnTemplateInit != null) {
            OnTemplateInit(templateName);
		}
	}
#endregion
	
#region Local
	#region Hide
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
			if(data == null) {
				transform.localScale = new Vector3(1f,1f,1f);
				GUILogManager.LogErr("Hide Template,but data is null.");
				return;
			}
			transform.localScale = data.scale;
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
