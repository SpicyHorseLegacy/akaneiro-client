using UnityEngine;
using System.Collections;

public class _UI_CreateChara : MonoBehaviour {
	
	public static _UI_CreateChara Instance;
	
	public Vector3 CunningPos;
	public Vector3 FortitudePos;
	public Vector3 ProwessPos;
	
	public Vector3 CunningRat;
	public Vector3 FortitudeRat;
	public Vector3 ProwessRat;
	
	public Camera  camera;
	
	private bool   isUpdatePos = false;
	private int    disIndex = 1;
	
	public float   xOffest = 0;
	public float   yOffest = 0;
	public float   zOffest = 0;
	
	public Vector3 DestPos;
	
	void Awake(){
		
		Instance = this;
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
		if(isUpdatePos){
			
			float x = camera.transform.position.x;
			float y = camera.transform.position.y;
			float z = camera.transform.position.z;
			
			if(x < DestPos.x){
				
				x += xOffest;
				
				if(x > DestPos.x){
				
					x = DestPos.x;
					
				}
				
			}else{
				
				x -= xOffest;
				
				if(x < DestPos.x){
				
					x = DestPos.x;
					
				}
				
			}
			
			if(y < DestPos.y){
				
				y += yOffest;
				
				if(y > DestPos.y){
				
					y = DestPos.y;
					
				}
				
			}else{
				
				y -= yOffest;
				
				if(y < DestPos.y){
				
					y = DestPos.y;
					
				}
				
			}
			
			if(z < DestPos.z){
				
				z += zOffest;
				
				if(z > DestPos.z){
				
					z = DestPos.z;
					
				}
				
			}else{
				
				z -= zOffest;
				
				if(z < DestPos.z){
				
					z = DestPos.z;
					
				}
				
			}
			
			camera.transform.position = new  Vector3(x,y,z);
			
		}
		
	}
	
	public void Init(){
	
		camera.transform.position = FortitudePos;
		disIndex = 1;
		isUpdatePos = false;
		DestPos = FortitudePos;
		
	}
	
	public void SelectDis(int dis){
	
		disIndex = dis;
		
		switch(dis){
			
		case 0:
			
			isUpdatePos = true;
			DestPos = ProwessPos;
//			camera.transform.localRotation.Set(CunningRat.x,CunningRat.y,CunningRat.z,1);
			camera.transform.localRotation = Quaternion.Euler(ProwessRat.x,ProwessRat.y,ProwessRat.z);
	
			break;
			
		case 1:
			
			isUpdatePos = true;
			DestPos = FortitudePos;
//			camera.transform.r.localRotation.Set(FortitudeRat.x,FortitudeRat.y,FortitudeRat.z,1);
			camera.transform.localRotation  = Quaternion.Euler(FortitudeRat.x,FortitudeRat.y,FortitudeRat.z);
			
			break;
			
		case 2:
			
			isUpdatePos = true;
			DestPos = CunningPos;
//			camera.transform.localRotation.Set(ProwessRat.x,ProwessRat.y,ProwessRat.z,1);
			camera.transform.localRotation  = Quaternion.Euler(CunningRat.x,CunningRat.y,CunningRat.z);
			
			break;
			
		default:
			break;
			
		}
		
	}
	
	
}
