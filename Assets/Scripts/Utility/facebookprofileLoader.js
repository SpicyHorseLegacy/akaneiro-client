//#pragma strict
//
//var profilePic : Texture2D;
//var tempProfilePic : Texture2D ;
//var haveTheProfilePicture : boolean = false ;
//
//var urlTest : String = "https://fbcdn-profile-a.akamaihd.net/hprofile-ak-ash2/1119740_657405860_559039202_q.jpg";
//var url : String = "https" + "://graph.facebook.com/"+ FB.UserId +"/picture";
//
//function loadPicturee () {
//	profilePic = this.renderer.material.mainTexture;
//}
//
//
//function OnGUI(){
//	//GUI.DrawTexture( Rect(1200,70,80,80), tempProfilePic, ScaleMode.ScaleToFit, true, 0);
//		GUI.Label(Rect(1200,70,64, 64),profilePic);
//	if (GUI.Button( Rect(1200, 160, 80, 30), "getImage")){
//		loadPicturee();
//		if (haveTheProfilePicture == true){
//			//GUI.DrawTexture( Rect(1200,70,60,60), this.renderer.material.mainTexture, ScaleMode.ScaleToFit, true, 0);
//			GUI.Label(Rect(1200,70,profilePic.width, profilePic.height),profilePic);
//		}else if (haveTheProfilePicture == false){
//			//GUI.DrawTexture( Rect(1200,70,60,60), this.renderer.material.mainTexture, ScaleMode.ScaleToFit, true, 0);
//			GUI.Label(Rect(1200,70,profilePic.width, profilePic.height),profilePic);
//		}
//	}
//}