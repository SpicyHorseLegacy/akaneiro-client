#pragma strict

class cinematicTool extends EditorWindow {   
    var sourceObject : GameObject = null;
    var sourceTrigger : GameObject = null;
    
    var firstColor : Color = Color.red;
    var secondColor : Color = Color.red;
    
    var cameraStatus : String = "No Camera Object" ;
    var cameraName : String = " " ;
    
    var triggerStatus : String = "No Trigger Object" ;
    var triggerName : String = " " ;

	var renderTexture : RenderTexture;
	var camera : Camera = Camera.main ;
	
	var thereIsCamera : boolean = false;
	
    @MenuItem ("Scripts/Cinematic Tool")
    
    static function Init () {
        var window = ScriptableObject.CreateInstance.<cinematicTool>();
        window.autoRepaintOnSceneChange = true;
        window.minSize = Vector2(400, 400) ;
        window.maxSize = Vector2(400, 400) ;
        window.Show();
    }

	function OnInspectorUpdate (){
		Repaint ();
	}
	
	function Awake (){
		thereIsCamera = false ;
		renderTexture = RenderTexture(position.width,  position.height, 24, RenderTextureFormat.ARGB32 );
		camera = Camera.main ;
		
		
	} 
	  
	function Update(){
		if(camera != null){
			thereIsCamera = true ;
			camera.targetTexture = renderTexture;
			camera.Render();
			camera.targetTexture = null;
			firstColor = Color.green ;
			cameraStatus = "Rendering Through : [" + camera.gameObject.name + "]" ;
		}else{
			thereIsCamera = false ;
			camera = Camera.main ;
			renderTexture = RenderTexture(position.width,  position.height, 24, RenderTextureFormat.ARGB32 );
			firstColor = Color.red ;
			cameraStatus = "No Camera Object";
		}
		if((renderTexture.width != position.width || renderTexture.height != position.height) && thereIsCamera == true){
			renderTexture = RenderTexture(position.width,  position.height, 24, RenderTextureFormat.ARGB32 );
		}
		
		if (sourceTrigger != null){
			secondColor = Color.green ;
			triggerStatus = "Set value into the trigger : [" + sourceTrigger.gameObject.name + "]" ;
		}else{
			secondColor = Color.red ;
			triggerStatus = "No Trigger Object";
		}
	}
		
		  
		        
    function OnGUI () {
    	GUI.contentColor = Color.white;
        EditorGUI.LabelField(Rect(3,3,position.width, 20), "", "Camera Object Status :", EditorStyles.boldLabel);
        
        GUI.contentColor = firstColor;
        EditorGUI.LabelField(Rect(3,23,position.width, 20), "", cameraStatus+cameraName, EditorStyles.boldLabel);
		
		
		GUI.contentColor = Color.white;
        sourceTrigger = EditorGUI.ObjectField(Rect(3,60,position.width - 6, 20), "In progress Trigger :", sourceTrigger, GameObject);
        camera = EditorGUI.ObjectField(Rect(3,120,position.width - 6, 20), "Camera View is :", camera, Camera);
        
        GUI.contentColor = secondColor ;
        EditorGUI.LabelField(Rect(3,80,position.width, 20), "", triggerStatus+triggerName, EditorStyles.boldLabel);
        
        if (thereIsCamera == true){
        	GUI.DrawTexture( Rect( 100, 170, position.width/2, position.height/2), renderTexture, ScaleMode.ScaleToFit, false, 1 );
        }
        
        GUI.contentColor = firstColor;
        if(GUI.Button(Rect(15,145, 130, 20),">>Pick Coords<<")){
        	if (camera){
        		EditorGUIUtility.PingObject(camera);
        		EditorPrefs.SetFloat("cinematicCameraPosX", camera.gameObject.transform.localPosition.x);
        		EditorPrefs.SetFloat("cinematicCameraPosY", camera.gameObject.transform.localPosition.y);
        		EditorPrefs.SetFloat("cinematicCameraPosZ", camera.gameObject.transform.localPosition.z);
        		
        		EditorPrefs.SetFloat("cinematicCameraRotX", camera.gameObject.transform.localRotation.eulerAngles.x);
        		EditorPrefs.SetFloat("cinematicCameraRotY", camera.gameObject.transform.localRotation.eulerAngles.y);
        		EditorPrefs.SetFloat("cinematicCameraRotZ", camera.gameObject.transform.localRotation.eulerAngles.z);
        		
        		
        		Debug.Log ("Captured position is : " + EditorPrefs.GetFloat("cinematicCameraPosX") + "/" + EditorPrefs.GetFloat("cinematicCameraPosY") + "/" + EditorPrefs.GetFloat("cinematicCameraPosZ"));
        		Debug.Log ("Captured rotation is : " + EditorPrefs.GetFloat("cinematicCameraRotX") + "/" + EditorPrefs.GetFloat("cinematicCameraRotY") + "/" + EditorPrefs.GetFloat("cinematicCameraRotZ"));
        	}
        }
        
        
        GUI.contentColor = secondColor;
        if(GUI.Button(Rect(155,145, 130, 20),"<<Paste Coords>>")){
        	if (sourceTrigger){
        		
        		var adjustedRotX : float ;
        		var adjustedRotY : float ;
        		var adjustedRotZ : float ;
        		// positions
        		sourceTrigger.gameObject.GetComponent(CinematicCameraTrigger).TargetedPosition.x = EditorPrefs.GetFloat("cinematicCameraPosX");
        		sourceTrigger.gameObject.GetComponent(CinematicCameraTrigger).TargetedPosition.y = EditorPrefs.GetFloat("cinematicCameraPosY");
        		sourceTrigger.gameObject.GetComponent(CinematicCameraTrigger).TargetedPosition.z = EditorPrefs.GetFloat("cinematicCameraPosZ");
        		//redajust rotations
        		// [X]
        		if (EditorPrefs.GetFloat("cinematicCameraRotX") > 180){
        			adjustedRotX = EditorPrefs.GetFloat("cinematicCameraRotX") - 360.0;
        		}else{
        			adjustedRotX = EditorPrefs.GetFloat("cinematicCameraRotX");
        		}
        		// [Y]
        		if (EditorPrefs.GetFloat("cinematicCameraRotY") > 180){
        			adjustedRotY = EditorPrefs.GetFloat("cinematicCameraRotY") - 360.0;
        		}else{
        			adjustedRotY = EditorPrefs.GetFloat("cinematicCameraRotY");
        		}
        		// [Z]
        		if (EditorPrefs.GetFloat("cinematicCameraRotZ") > 180){
        			adjustedRotZ = EditorPrefs.GetFloat("cinematicCameraRotZ") - 360.0;
        		}else{
        			adjustedRotZ = EditorPrefs.GetFloat("cinematicCameraRotZ");
        		}
        		
        		//rotations
        		sourceTrigger.gameObject.GetComponent(CinematicCameraTrigger).TargetedRotation.x = adjustedRotX;
        		sourceTrigger.gameObject.GetComponent(CinematicCameraTrigger).TargetedRotation.y = adjustedRotY;
        		sourceTrigger.gameObject.GetComponent(CinematicCameraTrigger).TargetedRotation.z = adjustedRotZ;
        		
        		EditorGUIUtility.PingObject(sourceTrigger);
        	}
        }
        
        GUI.contentColor = Color.yellow;
        if(GUI.Button(Rect(295,145, 45, 20),"^_^")){
        	camera.gameObject.transform.localPosition = Vector3.zero ;
        	camera.gameObject.transform.localRotation.eulerAngles = Vector3.zero ;
        }
        
        GUI.contentColor = Color.cyan;
		EditorGUI.LabelField(Rect(160,375,position.width, 40), "", "Muhammad ;)", EditorStyles.boldLabel);
	}
}