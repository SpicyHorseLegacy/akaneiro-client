using UnityEngine;
using System.Collections;

public class CameraState : MonoBehaviour {
	
	public string StateName="Normal";
	
	//the property to check when two or more states are triggered 
	public int Priority=1;
	
	//how long to blend in to the new camera state
	public float BlendInTime = 1.5f;
	
	//how long to blend out of the current camera state
	public float BlendOutTime = 1.5f;
	
	//FOV of the camera in degrees
	public float FOV=60.0f;
	
	//how fast the camera accelerates to its max speed when it is tracking the player
	public float Acceleration = 2.0f;
	
	//max speed of the camera in meters/second 
	//public float MaxSpeed=10.0f;
	
	//how quickly in seconds that the camera interpolates to the final position when the PC stops.
	public float SnapSmoothLag =2.0f;
	
	//distance in meters from the PC's pivot in the forward-facing direction. 
	//It is the location that the camera should be tracking. A positive value 
	//is a lead to the PC's position, a negative value is a lag.
    public bool IsNPCCamera = false;    // if it's npc camera, the camera should move to face NPC and look at the left side of NPC
	public float TargetOffset = 1.0f; 
	public float followSpeed = 0.5f;
	public Vector3 OffsetToLookPoint = new Vector3(0f,15.0f,-10.0f);
    public Vector3 NearCameraLookTarget = new Vector3(-2, 1, 0);
	public Vector3 LookAngle =  new Vector3(0.0f,-0.8f,0.6f);
}
