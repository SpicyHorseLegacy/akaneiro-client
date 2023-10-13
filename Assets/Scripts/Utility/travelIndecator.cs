using UnityEngine;
using System.Collections;

public class travelIndecator : MonoBehaviour {
	#if EZGUI
	private GameObject missionHolder ;
	private string missionHolderName ;
	
	ArrayList tasksNames = new ArrayList();
	
	ArrayList positionsX = new ArrayList();
	ArrayList positionsZ = new ArrayList();
	
	private int travelMissionsCount ;
	
	private Transform indecator ;
	
	private Transform player;
	private bool playerAchievedPoint ;
	private int playerAchievedPointTimes ;
	
	public float areaRadius = 1.0f;
		
	void Start () {
		//areaRadius = 1.0f ;	//
		playerAchievedPointTimes = 0 ;
		playerAchievedPoint = false ;
		indecator = this.transform;
		player = GameObject.FindGameObjectWithTag ("player").gameObject.transform;
		//playerLD = GameObject.Find ("PlayerForLD(Clone)").gameObject.transform;
		missionHolder = GameObject.FindGameObjectWithTag("missions").gameObject;
		//Debug.Log ( "The object holder number is : [" + missionHolder.name.ToString() + "]");
		//branchArray --> always 0 // my thoughts
		//taskArray ----> static nuumber // my thoughts
		//SubObject ----> always 0 // my thoughts
		for (int i = 0; i< missionHolder.gameObject.GetComponent <_UI_CS_MapLevelItem>().branchArray[0].taskArray.Capacity; i++){
			tasksNames.Add(missionHolder.gameObject.GetComponent <_UI_CS_MapLevelItem>().branchArray[0].taskArray[i].SubObject[0].typeID.ToString());
			if (missionHolder.gameObject.GetComponent <_UI_CS_MapLevelItem>().branchArray[0].taskArray[i].SubObject[0].typeID.ToString() == "TRAVEL"){
				positionsX.Add (missionHolder.gameObject.GetComponent <_UI_CS_MapLevelItem>().branchArray[0].taskArray[i].SubObject[0].objectID) ;
				positionsZ.Add (missionHolder.gameObject.GetComponent <_UI_CS_MapLevelItem>().branchArray[0].taskArray[i].SubObject[0].recycle) ;
				travelMissionsCount += 1 ;
				Debug.Log ("the TRAVEL X and Z for the SubMission [" + i + "] is" + positionsX[i] + "  " + positionsZ[i]);
			}
		}
		
		Debug.Log ("Travels Number is : <" + travelMissionsCount + ">");
		
	}
	

	void Update () {		
		if (travelMissionsCount > 0 && missionHolder.gameObject.GetComponent <_UI_CS_MapLevelItem>().branchArray[0].taskArray[0].SubObject[0].typeID.ToString() == "TRAVEL"){
			Vector3 temp = transform.position;
			temp.x = System.Convert.ToInt32(positionsX[playerAchievedPointTimes]);
			temp.z = System.Convert.ToInt32(positionsZ[playerAchievedPointTimes]);
			
			indecator.position = temp ;
			if (player.position.x < indecator.position.x+areaRadius && player.position.x > indecator.position.x-areaRadius && player.position.z < indecator.position.z+areaRadius && player.position.z > indecator.position.z-areaRadius && playerAchievedPoint == false){
				playerAchievedPoint = true ;
				travelMissionsCount -= 1 ;
				playerAchievedPointTimes += 1 ;
				//checkAgain();
				playerAchievedPoint = false ;
				Debug.Log ("Travels Number --NOW-- is : <" + travelMissionsCount + ">");
				Debug.Log ("Number of Achieved Missions is : <" + playerAchievedPointTimes + ">");
			}
		}else{
			indecator.gameObject.SetActive(false);	
		}
	}
	
	void checkAgain (){
		for (int i = 0; i< missionHolder.gameObject.GetComponent <_UI_CS_MapLevelItem>().branchArray[0].taskArray.Capacity; i++){
			tasksNames.Add(missionHolder.gameObject.GetComponent <_UI_CS_MapLevelItem>().branchArray[0].taskArray[i].SubObject[0].typeID.ToString());
			if (missionHolder.gameObject.GetComponent <_UI_CS_MapLevelItem>().branchArray[0].taskArray[i].SubObject[0].typeID.ToString() == "TRAVEL"){
				positionsX.Add (missionHolder.gameObject.GetComponent <_UI_CS_MapLevelItem>().branchArray[0].taskArray[i].SubObject[0].objectID) ;
				positionsZ.Add (missionHolder.gameObject.GetComponent <_UI_CS_MapLevelItem>().branchArray[0].taskArray[i].SubObject[0].recycle) ;
				travelMissionsCount += 1 ;
				Debug.Log ("the TRAVEL X and Z for the SubMission [" + i + "] is" + positionsX[i] + "  " + positionsZ[i]);
			}
		}
		
		Debug.Log ("Travels Number is : <" + travelMissionsCount + ">");	
	}
	
	void OnDrawGizmosSelected () {
		Gizmos.color = Color.cyan;
		Gizmos.DrawSphere (transform.position, areaRadius);
	}
	#else
	
	#endif
}
