using UnityEngine;
using System.Collections;

public class MissionSeleData{
	public MissDescData localData;
	public SAcceptMissionRelate2 serData;
	public int karma;
	public int exp;
	public string scenseName;
}

public class _MissionSelectBaseManager : MonoBehaviour {
	
	public static _MissionSelectBaseManager Instance;
	
	void Awake() {
		Instance = this;	
	}
	// Use this for initialization
	void Start () {
		GUIManager.Instance.AddTemplateInitEnd();
	}
	
	// Update is called once per frame
	void Update () {
		MapMove();
	}
	
	#region Interface
	public delegate void Handle_SetUITextureDelegate(string imgName,UITexture missionIcon);
    public event Handle_SetUITextureDelegate OnSetUITextureDelegate;
	public void SetUITextureDelegate(string imgName,UITexture missionIcon) {
		if(OnSetUITextureDelegate != null) {
			OnSetUITextureDelegate(imgName,missionIcon);
		}
	}
	
	public _MissSpirte [] areas;
	
	public delegate void Handle_CloseDelegate();
    public event Handle_CloseDelegate OnCloseDelegate;
	public void CloseDelegate() {
		if(OnCloseDelegate != null) {
			OnCloseDelegate();
		}
	}
	
	#endregion
	
	#region Local
	[SerializeField]
	private Transform target;
	private Vector3 scale;
	private bool isPressed = false;
	private void MapMove(){
		if(isPressed){
			Vector3 pos = target.position;
			pos+=scale;
			Camera curcam = UICamera.currentCamera;
			Bounds bs = NGUIMath.CalculateAbsoluteWidgetBounds(target);
            Vector3 _lb =new Vector3(target.position.x - bs.size.x/2,target.position.y-bs.size.y/2,0f);
            Vector3 lb = curcam.WorldToScreenPoint(_lb);
            Vector3 _rt = new Vector3(target.position.x+bs.size.x/2,target.position.y+bs.size.y/2,0f);
            Vector3 rt = curcam.WorldToScreenPoint(_rt);
			float width = rt.x - lb.x;
            float height = rt.y - lb.y;
			
			Vector3 ClampVector1;
            Vector3 ClampVector2;
			if(Screen.width>width&&Screen.height>height){
				ClampVector1 = new Vector3(width / 2, height / 2, 0f);
				ClampVector2 = new Vector3(Screen.width - width / 2, Screen.height - height / 2, 0f);
			}
			else{
				ClampVector1 = new Vector3(width / 2-(width-Screen.width), height / 2-(height-Screen.height), 0f);
				ClampVector2 = new Vector3(Screen.width- width / 2+(width-Screen.width), Screen.height - height / 2+(height-Screen.height), 0f);
			}
			Vector3 scrPos = curcam.WorldToScreenPoint(pos);
			scrPos.x = Mathf.Clamp(scrPos.x, ClampVector1.x, ClampVector2.x);
            scrPos.y = Mathf.Clamp(scrPos.y, ClampVector1.y, ClampVector2.y);
            target.position = curcam.ScreenToWorldPoint(scrPos);
		}
	}
	[SerializeField]
	private float moveSpeed = 0.03f;
	private void OnPressLeftDelegate(){
		isPressed = true;
		scale = new Vector3(moveSpeed,0,0);
	}
	
	private void OnPressRightDelegate(){
		isPressed = true;
		scale = new Vector3(-moveSpeed,0,0);
	}
	
	private void OnPressTopDelegate(){
		isPressed = true;
		scale = new Vector3(0,-moveSpeed,0);
	}
	
	private void OnPressDownDelegate(){
		isPressed = true;
		scale = new Vector3(0,moveSpeed,0);
	}
	
	private void OnReleaseLeftDelegate(){
		isPressed = false;
	}
	
	private void OnReleaseRightDelegate(){
		isPressed = false;
	}
	
	private void OnReleaseTopDelegate(){
		isPressed = false;
	}
	
	private void OnReleaseDownDelegate(){
		isPressed = false;
	}
	#endregion
}
