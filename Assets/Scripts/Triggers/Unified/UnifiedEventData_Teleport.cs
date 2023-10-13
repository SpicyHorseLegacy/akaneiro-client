using UnityEngine;
using System.Collections;

public class UnifiedEventData_Teleport : UnifiedEventData_Base
{
    public Transform TeleportPos;
    public bool CameraFollowAtOnce = true;
    public Transform EntryMesh;
    public float FadeInTime = 1f;
    public float FadeOutTime = 1f;
	
}
