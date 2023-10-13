#pragma strict

static var targetedPosition : Vector3 ;
static var targetedRotation : Vector3 ;

var time : float ;
var i : float ;
var rate : float;

var waitingTime : float ;

var complete : int ;

var motionTimesWrap : float ;
var motionAmountWrap : float ;
var motionDurationWrap : float ;
var motionSplitWrap : float ;


static var currentPosition : Vector3 ;
static var currentRotation : Vector3 ;

static var bndTime : float ;
static var q1 : Quaternion ;

function Start (){
	complete = 1 ;
	
}

function adjustPosition (pos : Vector3){
		targetedPosition = pos ;
}


function adjustRotation (rot : Vector3){
		targetedRotation = rot ;
		q1 = Quaternion.Euler(targetedRotation);
}


//
function setTime (val : float){
	motionTimesWrap = val ;
	Debug.Log("set 1");
}
function setAmount (val : float){
	motionAmountWrap = val ;
	Debug.Log("set 2");
}
function setDuration (val : float){
	motionDurationWrap = val ;
	Debug.Log("set 3");
}
function setSplit (val : float){
	motionSplitWrap = val ;
	Debug.Log("set 4");
}
function loopTheMotionEffect(){
	for (var i = 1; i <= motionTimesWrap; i++){
		Time.timeScale = motionAmountWrap;
		yield WaitForSeconds(motionDurationWrap);
		Debug.Log("A motion effect toke place !!!");
		Time.timeScale = 1.0 ;
		yield WaitForSeconds (motionSplitWrap);
	}
	Debug.Log("FINISHED ALL MOTIOn EFFECTS");
}
//



function cinematicChange (blendingTime : float){
	#if NGUI
	bndTime = blendingTime ;
	Camera.main.gameObject.SendMessage ("playAnimation");
	#endif
}
function resetCamera (){
	targetedPosition = Vector3.zero ;
	targetedRotation = Vector3.zero ;
	q1 = Quaternion.Euler(targetedRotation);
	Camera.main.gameObject.SendMessage ("resetAnimation");
}



function addFreez (tm : float){
	
}