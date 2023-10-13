#pragma strict

//var url = "https" + "://graph.facebook.com/"+ FB.UserId +"/picture?type=large";
var url = "https" + "://graph.facebook.com/"+ FB.UserId +"/picture";
var profilePic : Texture2D;

var canLoadFirstTime : boolean = true;

function Start () {
	canLoadFirstTime = true;
	renderer.material.mainTexture = new Texture2D(16, 16, TextureFormat.RGB24, false);
	//renderer.material.mainTexture = new Texture2D(4, 4, TextureFormat.DXT1, false);
	while(true) {
		var www = new WWW(url);
		yield www;
		www.LoadImageIntoTexture(renderer.material.mainTexture);
	}
}

function loadPicture () {
	canLoadFirstTime = false ;
	profilePic = this.renderer.material.mainTexture;
	//url = "https" + "://graph.facebook.com/"+ FB.UserId +"/picture?type=large";
	url = "https" + "://graph.facebook.com/"+ FB.UserId +"/picture";
	url += "?access_token=" + FB.AccessToken;
	renderer.material.mainTexture = new Texture2D(16, 16, TextureFormat.RGB24, false);
	//renderer.material.mainTexture = new Texture2D(4, 4, TextureFormat.DXT1, false);
	while(true) {
		var www = new WWW(url);
		yield www;
		www.LoadImageIntoTexture(renderer.material.mainTexture);
		profilePic = this.renderer.material.mainTexture;
	}
	profilePic = this.renderer.material.mainTexture;
}

function Update (){          
	if(FB.IsLoggedIn == true && canLoadFirstTime == true){
		loadPicture();
	}
}
function OnGUI(){
	if(FB.IsLoggedIn == true){
		profilePic = this.renderer.material.mainTexture;
		GUI.DrawTexture(Rect(1150,10,80,80), profilePic, ScaleMode.StretchToFill, true, 10.0); //880
		GUI.Label(Rect(1150,100,200,20), ("ID :"+FB.UserId));
		
	}
	if (GUI.Button(Rect(1150,140,90,30), "levelUp")){
		Debug.Log("level up post picture without prmission");
		this.gameObject.SendMessage("postPicture");
	}
	if (GUI.Button(Rect(1150,175,90,30), "invite")){
		Debug.Log("invite friends to play");
		this.gameObject.SendMessage("inviteFriends");
	}
	if (GUI.Button(Rect(1150,210,90,30), "shareST")){
		Debug.Log("share progress with STATIC image from our server");
		this.gameObject.SendMessage("postScreenShotST");
	}
	/* Test Button
	if (GUI.Button(Rect(1150,245,90,30), "Purchase")){
		Debug.Log("start the purchase process");
		this.gameObject.SendMessage("buySomething", "http://facebook.angry-red.com/unlock.html");
	}
	*/
	/*if (GUI.Button( Rect(1200, 160, 80, 30), "getImage")){
		loadPicture();
	}*/
}