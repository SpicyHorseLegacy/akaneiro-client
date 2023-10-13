#pragma strict

var cursorNormal : Texture2D ; // the normal state of the mosue cursor
var cursorBloody : Texture2D ; // use it when hover an enemy or a breakable object
var cursorHandOpened : Texture2D ; // used on the map
var cursorHandClosed : Texture2D ; // used when picking an object or pressing in a map mission
var cursorHandPointing : Texture2D ; // used when pointing to a map mission
var cursorSpeechBubble : Texture2D ; // used hovering a village vendor
		
var current : Texture2D ;

//var maincam : GameObject ;

function Start () {
//#if NGUI
	Screen.showCursor = false ;
	current = cursorNormal;
	//maincam = GameObject.FindGameObjectWithTag ("MainCamera");
//#else
	//this.gameObject.SetActive(false);
//#endif
	
}
function setCursor (curID : int){
	/*if (curID == 1){
		current = cursorNormal;
	}else if (curID == 2){
		current = cursorBloody;
	}else if (curID == 3){
		current = cursorHandOpened;
	}else if (curID == 4){
		current = cursorHandClosed;
	}else if (curID == 5){
		current = cursorHandPointing;
	}else if (curID == 6){
		current = cursorSpeechBubble;
	}else{
		current = cursorNormal;
	}*/
}

function OnGUI (){
	GUI.DrawTexture(Rect(Input.mousePosition.x-25, Screen.height - Input.mousePosition.y-25, 64.0f, 64.0f), current);	
}

function cursorNormalF (){
	current = cursorNormal;
}
function cursorBloodyF (){
	current = cursorBloody;
}
function cursorOpenedHandF (){
	current = cursorHandOpened;
}
function cursorClosedHandF (){
	current = cursorHandClosed;
}
function cursorPointingHandF (){
	current = cursorHandPointing;
}
function cursorSpeechF (){
	current = cursorSpeechBubble;
}