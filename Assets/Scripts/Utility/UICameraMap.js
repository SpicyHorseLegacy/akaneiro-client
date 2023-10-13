#pragma strict

static var cameraRendering : int ;
private var cursorObject : GameObject ;

function Awake (){
	cursorObject = GameObject.Find("mouseCursors").gameObject ;
}
function Start () {
	cameraRendering = 1 ;
}

function changeCamera0 (){
	cameraRendering = 0 ;
}
function changeCamera1 (){
	cameraRendering = 1 ;
}
function Update () {
	var ray : Ray = this.gameObject.camera.ScreenPointToRay(Input.mousePosition);
	var hit : RaycastHit;
	if (Physics.Raycast (ray, hit, 100)){
		//Debug.DrawLine (ray.origin, hit.point, Color.cyan);
		#if NGUI
		if (/*this.gameObject.name == "Camera"*/cameraRendering == 0){
			if(hit.collider.gameObject.name == "WoraldMap"){
				cursorObject.gameObject.SendMessage("cursorOpenedHandF");
			}
			else if(hit.collider.gameObject.name.StartsWith ("Area ")){
				cursorObject.gameObject.SendMessage("cursorPointingHandF");
			}
			else if(hit.collider.gameObject.name.StartsWith ("MissionWindow") || hit.collider.gameObject.name.StartsWith ("Hunt Btn") || hit.collider.gameObject.name.StartsWith ("exit")){
				cursorObject.gameObject.SendMessage("cursorNormalF");
			}
		}else if (/*this.gameObject.name == "Camera"*/cameraRendering == 1){
			if (hit.transform.tag == "villageNpc"){
				cursorObject.gameObject.SendMessage("cursorSpeechF");
			}
			else if (hit.transform.gameObject.layer == 10 && hit.transform.tag != "villageNpc"){
				cursorObject.gameObject.SendMessage("cursorBloodyF");
			}
			else if (hit.transform.gameObject.layer == 12){
				cursorObject.gameObject.SendMessage("cursorOpenedHandF");
			}
			else if (hit.transform.gameObject.layer == 17){
				cursorObject.gameObject.SendMessage("cursorBloodyF");
			}
			else if (hit.transform.gameObject.layer == 18){
				cursorObject.gameObject.SendMessage("cursorOpenedHandF");
			}
			else if (hit.transform.gameObject.name == "GAM_Switch_Forest" || hit.transform.gameObject.name == "GAM_Switch_Forest_temp(Clone)(Clone)"){
				cursorObject.gameObject.SendMessage("cursorOpenedHandF");
			}
			else if (hit.transform.gameObject.layer == 9 || hit.transform.gameObject.layer == 11 || hit.transform.gameObject.name == "Terrain" || hit.transform.gameObject.name == "triggerCinematicCamera" ||hit.transform.gameObject.name == "Walkable_Cube" || hit.transform.gameObject.name == "Blocking_Cube"){
				cursorObject.gameObject.SendMessage("cursorNormalF");
			}
			//else {
				//cursorObject.gameObject.SendMessage("cursorNormalF");
			//}
		}
			
		#endif
	}
	
	//Debug.Log ("cam value is : " + cameraRendering);

}