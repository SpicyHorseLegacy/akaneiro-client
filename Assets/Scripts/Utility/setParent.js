#pragma strict

private var player : GameObject ;

function Start () {
	loop();
}
function Update () {
	loop();
}

function loop () {
	//this.gameObject.SetActive(false);
	if (player == null ){
		player = GameObject.Find ("Aka_Model"); //CH_AKA_Prefab //Aka_Model
	}
	if (player == null ){
		player = GameObject.Find ("PlayerForLD(Clone)");
	}
	this.gameObject.transform.parent = player.gameObject.transform ;
	if (player == null ){
		Debug.LogError ("NO reference for player. No Player found");
	}	
	
	transform.localRotation.y = 0 ;
}

