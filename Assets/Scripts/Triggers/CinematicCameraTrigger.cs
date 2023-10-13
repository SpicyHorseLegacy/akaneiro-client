
using UnityEngine;
using System.Collections;


public class CinematicCameraTrigger : MonoBehaviour {
	
	private GameObject renderingCamera ;
	
	[Tooltip("Enable this option to use the trigger one time only")]
	public bool UseOnce ;
	
	[Tooltip("Enable this option will return the camera to normal after getting outside of the trigger area")]
	public bool CanExitToNormal ;
	
	[Tooltip("Add the targeted FOV. Leave it 0 means use the default FOV [50]")]
	public float FOVValue ;
	
	public Vector3 TargetedPosition ;
	public Vector3 TargetedRotation ;
	
	[Tooltip("Select the blending time of cinematic movement. Leave it 0 will use the default time [2] Seconds - The blending time will not affect the FOV transition")]
	public float BlendingTimeOnSeconds;
	
	public enum CinematicTypeList {None, TPS, FPS, Top, HighAngel, DownQuest, EagleEye};
	
	public CinematicTypeList CinematicType ;

    [Tooltip("Enable this option to add some slow motion to the camera movement")]
    public bool motionEffect;

    [Tooltip("How many times the trigger do slow motion effect")]
    public float motionEffectTimes;

    [Tooltip("The total duration for each slow motion tick. In seconds")]
    public float motionEffectDuration;

    [Tooltip("The amount of slowmotion. >1 means fastmotion. <1 means slowmotion. =1 means normal")]
    public float motionEffectAmount;

    [Tooltip("The amount seconds to wait between motion effects")]
    public float motionEffectSplitBy;

	
	
	[Tooltip("Enable a small freez on time while animating the camera")]
	bool TimeFreez ;
	[Tooltip("If Time Freez feature enabled, you can define the hold amount here. It is between 0 and 1")]
	float FreezingPeriod ;
	
	void Start () {
		renderingCamera = GameCamera.Instance.gameObject;
		renderingCamera.gameObject.SendMessage ("cinematic1", 1.0f);
		renderingCamera.gameObject.SendMessage ("cinematic2", 50.0f);
	}
	
	void Update () {
	
	}
	
	void OnTriggerEnter (Collider other){
		if (other.gameObject.name == "CH_AKA_Prefab" || other.gameObject.name == "PlayerForLD(Clone)"){
			if (CinematicType == CinematicTypeList.None){
				messaging();
				renderingCamera.gameObject.SendMessage ("cinematic1", BlendingTimeOnSeconds);
				renderingCamera.gameObject.SendMessage ("cinematic2", FOVValue);
			}else if (CinematicType == CinematicTypeList.TPS){
				TargetedPosition = new Vector3(-5.83288f, 1.76965f, 7.2388f) ;
				TargetedRotation = new Vector3(1.819901f, 61.40001f, -32.4f) ;
				messaging();
			}else if (CinematicType == CinematicTypeList.FPS){
				TargetedPosition = new Vector3(-0.8667077f, 1.589924f, 9.946441f) ;
				TargetedRotation = new Vector3(-10.5f, 64.01257f, -32.5f) ;
				messaging();
			}else if (CinematicType == CinematicTypeList.Top){
				TargetedPosition = new Vector3(0.4923877f, 7.013456f, 1.613622f) ;
				TargetedRotation = new Vector3(36.7f, -6.6f, 15.95f) ;
				messaging();
			}else if (CinematicType == CinematicTypeList.HighAngel){
				TargetedPosition = new Vector3(-9.690356f, 3.669313f, 3.877897f) ;
				TargetedRotation = new Vector3(20.37004f, 48.93536f, -35.9f) ;
				messaging();
			}else if (CinematicType == CinematicTypeList.DownQuest){
				TargetedPosition = new Vector3(-4.27f, 0.506f, 7.8f) ;
			    TargetedRotation = new Vector3(1.82f, 61.4f, 3.899f) ;
				messaging();
            }else if (CinematicType == CinematicTypeList.EagleEye){
                TargetedPosition = new Vector3(0.0f, 5.0f, -7.0f);
                TargetedRotation = new Vector3(20.0f, 0.0f, 20.0f);
                messaging();
            }
		}
	}
	
	
	void OnTriggerExit (Collider other){
		if (other.gameObject.name == "CH_AKA_Prefab" || other.gameObject.name == "PlayerForLD(Clone)"){
			if (CanExitToNormal == true){
				if (UseOnce == true){
					this.gameObject.collider.enabled = false ;
				}
				if (CinematicType == CinematicTypeList.None){
					renderingCamera.gameObject.SendMessage ("cinematic1", BlendingTimeOnSeconds);
					renderingCamera.gameObject.SendMessage ("cinematic2", 50); //50 is the normal FOV in Akaneiro
					renderingCamera.gameObject.SendMessage ("resetCamera");
				}else{
					renderingCamera.gameObject.SendMessage ("resetCamera");
				}
			}
		}
	}

    void messaging()
    {
		if (TimeFreez == true){
			renderingCamera.gameObject.SendMessage ("addFreez", FreezingPeriod);
		}
		if (FOVValue != 50){
			renderingCamera.gameObject.SendMessage ("cinematic1", BlendingTimeOnSeconds);
			renderingCamera.gameObject.SendMessage ("cinematic2", 50);
		}
        if (motionEffect == true )
        {
            renderingCamera.gameObject.SendMessage("setTime", motionEffectTimes);
            renderingCamera.gameObject.SendMessage("setAmount", motionEffectAmount);
            renderingCamera.gameObject.SendMessage("setDuration", motionEffectDuration);
            renderingCamera.gameObject.SendMessage("setSplit", motionEffectSplitBy);
            renderingCamera.gameObject.SendMessage("loopTheMotionEffect");
            Debug.Log("input is fine");
        }
		renderingCamera.gameObject.SendMessage ("adjustPosition", TargetedPosition);
		renderingCamera.gameObject.SendMessage ("adjustRotation", TargetedRotation);
		renderingCamera.gameObject.SendMessage ("cinematicChange", BlendingTimeOnSeconds);
	}
	
}
