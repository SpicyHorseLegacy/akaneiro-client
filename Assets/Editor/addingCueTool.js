#pragma strict

class addingCueTool extends EditorWindow {   
    var sourceObject : GameObject = null;
    var sourceTrigger : GameObject = null;
    
    var firstColor : Color = Color.red;
    var secondColor : Color = Color.red;
    var thirdColor : Color = Color.red;
    
    var componentStatus : String = "No Component Object" ;
    var componentname : String = " " ;
    
    var targetsStatus : String = "No Target Objects Selected" ;
    var targetsName : String = " " ;

	var renderTexture : RenderTexture;
	var camera : Camera = Camera.main ;
	
	var thereIsCamera : boolean = false;
	
	var insertedComponent : MonoScript = null ;
	
	var thereIsSelectedObjectsAndComponent : boolean = false ;
	
	static var totalAppliedComponents : int = 0;
	
    @MenuItem ("Scripts/[+]Cue")
    
    static function Init () {
        var window = ScriptableObject.CreateInstance.<addingCueTool>();
        window.autoRepaintOnSceneChange = true;
        window.minSize = Vector2(400, 250) ;
        window.maxSize = Vector2(400, 250) ;
        totalAppliedComponents = 0 ;
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
		if (insertedComponent != null){
			firstColor = Color.green;
			componentStatus = "The selected Component is : [" + insertedComponent.name + "]" ;
		}else{
			firstColor = Color.red;
			componentStatus = "No Component Object";
		}	
		
		
		if (Selection.gameObjects.length != 0){
			secondColor = Color.green ;
			targetsStatus = "There is : [" + Selection.gameObjects.length + "] Selected Object " ;
			thereIsSelectedObjectsAndComponent = true ;
		}else{
			secondColor = Color.red ;
			targetsStatus = "No Target Objects Selected";
			thereIsSelectedObjectsAndComponent = false ;
		}
	}
		
		  
		        
    function OnGUI () {
    	GUI.contentColor = Color.white;
        EditorGUI.LabelField(Rect(3,3,position.width, 20), "", "The component to Add : ", EditorStyles.boldLabel);
        
        GUI.contentColor = firstColor;
        EditorGUI.LabelField(Rect(3,18,position.width, 20), "", componentStatus+componentname, EditorStyles.boldLabel);
		
		
		GUI.contentColor = Color.white;
        insertedComponent = EditorGUI.ObjectField(Rect(3,37,position.width - 6, 20), "The[+]Component :", insertedComponent, MonoScript);
        //camera = EditorGUI.ObjectField(Rect(3,120,position.width - 6, 20), "Camera View is :", camera, Camera);
        
        GUI.contentColor = secondColor ;
        EditorGUI.LabelField(Rect(100,80,position.width, 20), "", targetsStatus+targetsName, EditorStyles.boldLabel);
        
        
        GUI.contentColor = secondColor;
        if(GUI.Button(Rect(145,110, 125, 30),">>Add-2-All<<")){
			if (thereIsSelectedObjectsAndComponent == true){
				addToAll();
			}
        }

        GUI.contentColor = Color.yellow;
		EditorGUI.LabelField(Rect(3,150,position.width, 75), "", "-Add the Component to it's socket. \n-Select all the Objects/Prefabs \n-If the button turn green, Press it. \n-Check the log for any extra info. \n-That's it.", EditorStyles.boldLabel);
		
		GUI.contentColor = Color.cyan;
		EditorGUI.LabelField(Rect(150,225,position.width, 40), "", "Muhammad ;)", EditorStyles.boldLabel);
	}
	
	function addToAll(){
		if (Selection.gameObjects.length > 0 ){
			var selectedObjects : GameObject[];
			//selectedObjects = Selection.gameObjects ;
			Debug.Log ("the total selected objects is : " + Selection.gameObjects.Length);
			for (var i:int = 0; i<Selection.gameObjects.Length; i++){
				Debug.Log("childs count in the current processing object is : [[[" + Selection.gameObjects[i].gameObject.transform.childCount + "]]] cute Childs" );
				for (var c:int = 0; c<Selection.gameObjects[i].gameObject.transform.childCount; c++){
					if (Selection.gameObjects[i].gameObject.transform.GetChild(c).gameObject.GetComponent(UIButtonSound)){
						//Debug.Log("This Child have the needed component");
						totalAppliedComponents += 1 ;
						Selection.gameObjects[i].gameObject.transform.GetChild(c).gameObject.AddComponent(insertedComponent.name);
					}
					for (var c2:int = 0; c2<Selection.gameObjects[i].gameObject.transform.GetChild(c).gameObject.transform.childCount; c2++){
						if (Selection.gameObjects[i].gameObject.transform.GetChild(c).GetChild(c2).gameObject.GetComponent(UIButtonSound)){
							//Debug.Log("This Child have the needed component");
							totalAppliedComponents += 1 ;
							Selection.gameObjects[i].gameObject.transform.GetChild(c).GetChild(c2).gameObject.AddComponent(insertedComponent.name);
						}
						for (var c3:int = 0; c3<Selection.gameObjects[i].gameObject.transform.GetChild(c).gameObject.transform.GetChild(c2).gameObject.transform.childCount; c3++){
							if (Selection.gameObjects[i].gameObject.transform.GetChild(c).gameObject.transform.GetChild(c2).transform.GetChild(c3).gameObject.GetComponent(UIButtonSound)){
								//Debug.Log("This Child have the needed component");
								totalAppliedComponents += 1 ;
								Selection.gameObjects[i].gameObject.transform.GetChild(c).gameObject.transform.GetChild(c2).transform.GetChild(c3).gameObject.AddComponent(insertedComponent.name);
							}
							for (var c4:int = 0; c4<Selection.gameObjects[i].gameObject.transform.GetChild(c).gameObject.transform.GetChild(c2).gameObject.transform.GetChild(c3).gameObject.transform.childCount; c4++){
								if (Selection.gameObjects[i].gameObject.transform.GetChild(c).gameObject.transform.GetChild(c2).transform.GetChild(c3).transform.GetChild(c4).gameObject.GetComponent(UIButtonSound)){
									//Debug.Log("This Child have the needed component");
									totalAppliedComponents += 1 ;
									Selection.gameObjects[i].gameObject.transform.GetChild(c).gameObject.transform.GetChild(c2).transform.GetChild(c3).transform.GetChild(c4).gameObject.AddComponent(insertedComponent.name);
								}
								for (var c5:int = 0; c5<Selection.gameObjects[i].gameObject.transform.GetChild(c).gameObject.transform.GetChild(c2).gameObject.transform.GetChild(c3).gameObject.transform.GetChild(c4).gameObject.transform.childCount; c5++){
									if (Selection.gameObjects[i].gameObject.transform.GetChild(c).gameObject.transform.GetChild(c2).transform.GetChild(c3).transform.GetChild(c4).transform.GetChild(c5).gameObject.GetComponent(UIButtonSound)){
										//Debug.Log("This Child have the needed component");
										totalAppliedComponents += 1 ;
										Selection.gameObjects[i].gameObject.transform.GetChild(c).gameObject.transform.GetChild(c2).transform.GetChild(c3).transform.GetChild(c4).transform.GetChild(c5).gameObject.AddComponent(insertedComponent.name);
									}
								}
							}
						}
					}
				}
			}
			Debug.Log ("The total Applied Components is [" + totalAppliedComponents + "]");
		}
	}
}