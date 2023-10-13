#pragma strict

function Start () {
	
}

function OnGUI () {
	  if (GUI.Button(Rect(10,70,50,30),"Hide UI")){
	  	GameObject.Find ("UI manager").gameObject.SetActiveRecursively(false);
	  }
}