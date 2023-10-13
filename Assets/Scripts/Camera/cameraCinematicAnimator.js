#pragma strict

@script RequireComponent(Animation)

var curvePosX : AnimationCurve ;
var curvePosY : AnimationCurve ;
var curvePosZ : AnimationCurve ;

var curveRotX : AnimationCurve ;
var curveRotY : AnimationCurve ;
var curveRotZ : AnimationCurve ;
var curveRotW : AnimationCurve ;

var clip : AnimationClip ;


function playAnimation () {
	
	curvePosX = AnimationCurve.EaseInOut(0, this.gameObject.transform.localPosition.x, cameraCinematicMessages.bndTime, cameraCinematicMessages.targetedPosition.x);
	curvePosY = AnimationCurve.EaseInOut(0, this.gameObject.transform.localPosition.y, cameraCinematicMessages.bndTime, cameraCinematicMessages.targetedPosition.y);
	curvePosZ = AnimationCurve.EaseInOut(0, this.gameObject.transform.localPosition.z, cameraCinematicMessages.bndTime, cameraCinematicMessages.targetedPosition.z);
	
	curveRotX = AnimationCurve.EaseInOut(0, this.gameObject.transform.localRotation.x, cameraCinematicMessages.bndTime, cameraCinematicMessages.q1.x);
	curveRotY = AnimationCurve.EaseInOut(0, this.gameObject.transform.localRotation.y, cameraCinematicMessages.bndTime, cameraCinematicMessages.q1.y);
	curveRotZ = AnimationCurve.EaseInOut(0, this.gameObject.transform.localRotation.z, cameraCinematicMessages.bndTime, cameraCinematicMessages.q1.z);
	curveRotW = AnimationCurve.EaseInOut(0, this.gameObject.transform.localRotation.w, cameraCinematicMessages.bndTime, cameraCinematicMessages.q1.w);
	
	clip = new AnimationClip();

	clip.ClearCurves();
	clip.SetCurve("", Transform, "localPosition.x", curvePosX);
	clip.SetCurve("", Transform, "localPosition.y", curvePosY);
	clip.SetCurve("", Transform, "localPosition.z", curvePosZ);
	
	clip.SetCurve("", Transform, "localRotation.x", curveRotX);
	clip.SetCurve("", Transform, "localRotation.y", curveRotY);
	clip.SetCurve("", Transform, "localRotation.z", curveRotZ);
	clip.SetCurve("", Transform, "localRotation.w", curveRotW);
        
	animation.AddClip(clip, "cinematicMovement");
	animation.CrossFade("cinematicMovement");
}





function resetAnimation () {
	
	curvePosX = AnimationCurve.EaseInOut(0, this.gameObject.transform.localPosition.x, 3, 0);
	curvePosY = AnimationCurve.EaseInOut(0, this.gameObject.transform.localPosition.y, 3, 0);
	curvePosZ = AnimationCurve.EaseInOut(0, this.gameObject.transform.localPosition.z, 3, 0);
	
	curveRotX = AnimationCurve.EaseInOut(0, this.gameObject.transform.localRotation.x, 3, cameraCinematicMessages.q1.x);
	curveRotY = AnimationCurve.EaseInOut(0, this.gameObject.transform.localRotation.y, 3, cameraCinematicMessages.q1.y);
	curveRotZ = AnimationCurve.EaseInOut(0, this.gameObject.transform.localRotation.z, 3, cameraCinematicMessages.q1.z);
	curveRotW = AnimationCurve.EaseInOut(0, this.gameObject.transform.localRotation.w, 3, cameraCinematicMessages.q1.w);
	
	clip = new AnimationClip();

	clip.ClearCurves();
	clip.SetCurve("", Transform, "localPosition.x", curvePosX);
	clip.SetCurve("", Transform, "localPosition.y", curvePosY);
	clip.SetCurve("", Transform, "localPosition.z", curvePosZ);
	
	clip.SetCurve("", Transform, "localRotation.x", curveRotX);
	clip.SetCurve("", Transform, "localRotation.y", curveRotY);
	clip.SetCurve("", Transform, "localRotation.z", curveRotZ);
	clip.SetCurve("", Transform, "localRotation.w", curveRotW);
        
	animation.AddClip(clip, "cinematicMovement");
	animation.CrossFade("cinematicMovement");
}