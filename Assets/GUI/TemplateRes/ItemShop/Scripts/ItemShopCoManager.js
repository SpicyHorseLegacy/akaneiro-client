#pragma strict

var cover : GameObject;
	
var position1 : Transform;
var position2 : Transform;
var position3 : Transform;
var position4 : Transform;

	
function Start () {

}

function UpdateTab1 () {
	cover.transform.position.x = position1.position.x;
}
function UpdateTab2 () {
	cover.transform.position.x = position2.position.x;
}
function UpdateTab3 () {
	cover.transform.position.x = position3.position.x;
}
function UpdateTab4 () {
	cover.transform.position.x = position4.position.x;
}