#pragma strict

var mapObject : GameObject ;

function Start () {
	yield WaitForSeconds(2);
	mapObject.gameObject.renderer.enabled = false ;
}

function Update () {

}