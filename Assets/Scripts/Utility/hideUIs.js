#pragma strict

private var theUIObject : GameObject ;
private var uiS : boolean ;

function Awake () {
	theUIObject = GameObject.Find ("UI Root (2D)").gameObject ;
	uiS = false ;
}

function Update () {
	if (Input.GetKeyDown (KeyCode.O) && uiS == false){
		theUIObject.gameObject.SetActive(false);
		uiS = true;
	}else if (Input.GetKeyDown (KeyCode.O) && uiS == true){
		theUIObject.gameObject.SetActive(true);
		uiS = false;
	}
}