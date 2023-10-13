//////////////////////////////////////////

//所以 你需要做的有两件事 
// 移动场景里 TOOL_MINIMAP 的位置到起始位置(场景的左上角)
// 设置你game视口的比例为3:2 (算起来太麻烦)
//////////////////////////////////////////

var resWidth : int = 300; 
var resHeight : int = 200; 

var cameraMap : Camera;

//Camera 高度
private var 	  cameraDestHeight : float= 100;

//flag 
public var CameraTl : GameObject;
public var CameraRb : GameObject;
public var CameraLb : GameObject;
public var CameraRt : GameObject;
public var MapRb : GameObject;

private var cameraWide : float;
private var cameraHeight : float;


public var   Traw : int;
public var   Tcow : int;

private var   raw : int;
private var   cow : int;

public var    Size: int;

public var filePath : String = "/UI/Textures/MiniMap/";


public var mapLT : Vector3;
public var mapRB : Vector3;
public var mapWide : float;
public var mapHeight : float;


function Start () {

		cameraWide 	   = (CameraRb.transform.position.x - CameraTl.transform.position.x);
		cameraHeight   = (CameraTl.transform.position.z - CameraRb.transform.position.z);

		raw = Size * Tcow;
		cow = Size * Traw;

		BuildMiniMapTexture();
		
		
}

function Update () {

}

function OnGizmosDraw() {
	//Gizmos.DrawLine( cameraMap.transform.position);
}

function BuildMiniMapTexture () {

		mapLT = CameraTl.transform.position;

		for(var i:int = 0;i < cow;i++){
			for(var j:int = 0;j< raw;j++){
				
			   cameraMap.transform.position = new Vector3(CameraTl.transform.position.x + cameraWide/2 +( j * cameraWide),CameraTl.transform.position.y + cameraDestHeight,CameraTl.transform.position.z - cameraHeight/2 - ( i * cameraHeight));
				
//				cameraMap.transform.position = new Vector3(CameraTl.transform.position.x -  i * (CameraTl.transform.position.x - CameraLb.transform.position.x) +  j * (CameraRt.transform.position.x - CameraTl.transform.position.x),
//														   CameraTl.transform.position.y + cameraDestHeight,
//														   CameraTl.transform.position.z -  i * (CameraTl.transform.position.z - CameraLb.transform.position.z) -  j * (CameraTl.transform.position.z - CameraRt.transform.position.z));
				
			   var rt = new RenderTexture(resWidth, resHeight, 24);    
		       cameraMap.targetTexture = rt; 
		       var screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false); 
		       cameraMap.Render(); 
		       RenderTexture.active = rt;
		       screenShot.ReadPixels(Rect(0, 0, resWidth, resHeight), 0, 0); 
		       RenderTexture.active = null; // JC: added to avoid errors 
		       cameraMap.targetTexture = null;
		       Destroy(rt);
		       var bytes = screenShot.EncodeToPNG(); 
		       System.IO.File.WriteAllBytes(Application.dataPath  + filePath + i + "_" + j + ".png", bytes); 
	       }
	   }
	   
	   mapRB = MapRb.transform.position;
	   
	   
	   
	   
	   mapWide   = (mapRB.x - mapLT.x);
	   mapHeight = (mapLT.z - mapRB.z);

}