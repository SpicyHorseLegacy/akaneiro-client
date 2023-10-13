using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MissionMapManager : MonoBehaviour {
	
	public static MissionMapManager Instance;
	
	void Awake(){
		Instance = this;
	}
	// Use this for initialization
	void Start () {
//		test();	
	}
	
	// Update is called once per frame
	void Update () {
		MapMove();
	}
	
	#region test
//	void test(){
//		MissionSelectData data = new MissionSelectData();
//		data.isCooldown = false;
//		data.name = "text";
//		data.info = "sdfsdfsdfsdf";
//		data.icon = null;
//		data.address = "address";
//		data.enemyIcon = null;
//		data.enemyInfo = "sdfsdfsdfsf";
//		data.materialIcon = null;
//		data.materialInfo = "dfsdfsdfsdffff";
//		MissionDifficultyData mdata = new MissionDifficultyData(false,true,999,999,"name","9",Color.green);
//		data.difficultyData[(int)MissionLevels.Easy] = mdata;
//		mdata = new MissionDifficultyData(true,false,999,999,"name","10",Color.yellow);
//		data.difficultyData[(int)MissionLevels.Medium] = mdata;
//		mdata = new MissionDifficultyData(true,false,999,999,"name","11",new Color(1.0f,0.5f,0f,1.0f));
//		data.difficultyData[(int)MissionLevels.Hard] = mdata;
//		mdata = new MissionDifficultyData(true,false,999,999,"name","12",Color.red);
//		data.difficultyData[(int)MissionLevels.Overrun] = mdata;
//		data.recommendedLv = "12";
//		data.isEnabled = true;
//		data.coolDownTime = 30*60;
//		data.lastTime = 1375174849;
//		missionDataList.Add(data);
//		
//		
//		data = new MissionSelectData();
//		data.isCooldown = false;
//		data.name = "text";
//		data.info = "sdfsdfsdfsdf";
//		data.icon = null;
//		data.address = "address";
//		data.enemyIcon = null;
//		data.enemyInfo = "sdfsdfsdfsf";
//		data.materialIcon = null;
//		data.materialInfo = "dfsdfsdfsdffff";
//		mdata = new MissionDifficultyData(false,true,999,999,"name","9",Color.green);
//		data.difficultyData[(int)MissionLevels.Easy] = mdata;
//		mdata = new MissionDifficultyData(true,false,999,999,"name","10",Color.yellow);
//		data.difficultyData[(int)MissionLevels.Medium] = mdata;
//		mdata = new MissionDifficultyData(true,false,999,999,"name","11",new Color(1.0f,0.5f,0f,1.0f));
//		data.difficultyData[(int)MissionLevels.Hard] = mdata;
//		mdata = new MissionDifficultyData(true,false,999,999,"name","12",Color.red);
//		data.difficultyData[(int)MissionLevels.Overrun] = mdata;
//		data.recommendedLv = "12";
//		data.isEnabled = true;
//		data.coolDownTime = 30*60;
//		data.lastTime = 1374289680;
//		missionDataList.Add(data);
//
//		InitMissionMap();
//	}
	#endregion
	#region Interface
	[SerializeField]
	private List<Transform> missions = new List<Transform>();
	public List<MissionSelectData> missionDataList = new List<MissionSelectData>();
	private void InitMissionMap() {
		if(missions.Count>=missionDataList.Count) {
			int index = 0;
			foreach(Transform mission in missions) {
				if(index<missionDataList.Count){
					NGUITools.SetActive(mission.gameObject,true);
					mission.GetComponent<MissionSprite>().SetMissionSprite(missionDataList[index]);
				}
				else {
					NGUITools.SetActive(missions[index].gameObject,false);
				}
				index++;
			}
		}else {
			GUILogManager.LogErr("InitMap() faile.MissionDatas.Count<missions.Count");
		}
	}
	
	public delegate void Handle_MissionExitBtnDelegate();
    public event Handle_MissionExitBtnDelegate OnMissionExitBtnDelegate;
	private void MissionExitBtnDelegate() {
		if(OnMissionExitBtnDelegate != null) {
			OnMissionExitBtnDelegate();
		}
	}
	
	#endregion
	
	#region Local
	[SerializeField]
	private Transform target;
	private Vector3 scale;
	private bool isPressed = false;
	private void MapMove(){
		if(isPressed){
			Vector3 pos = target.position;
			pos+=scale;
			Camera curcam = UICamera.currentCamera;
			Bounds bs = NGUIMath.CalculateAbsoluteWidgetBounds(target);
            Vector3 _lb =new Vector3(target.position.x - bs.size.x/2,target.position.y-bs.size.y/2,0f);
            Vector3 lb = curcam.WorldToScreenPoint(_lb);
            Vector3 _rt = new Vector3(target.position.x+bs.size.x/2,target.position.y+bs.size.y/2,0f);
            Vector3 rt = curcam.WorldToScreenPoint(_rt);
			float width = rt.x - lb.x;
            float height = rt.y - lb.y;
			
			Vector3 ClampVector1;
            Vector3 ClampVector2;
			if(Screen.width>width&&Screen.height>height){
				ClampVector1 = new Vector3(width / 2, height / 2, 0f);
				ClampVector2 = new Vector3(Screen.width - width / 2, Screen.height - height / 2, 0f);
			}
			else{
				ClampVector1 = new Vector3(width / 2-(width-Screen.width), height / 2-(height-Screen.height), 0f);
				ClampVector2 = new Vector3(Screen.width- width / 2+(width-Screen.width), Screen.height - height / 2+(height-Screen.height), 0f);
			}
			Vector3 scrPos = curcam.WorldToScreenPoint(pos);
			scrPos.x = Mathf.Clamp(scrPos.x, ClampVector1.x, ClampVector2.x);
            scrPos.y = Mathf.Clamp(scrPos.y, ClampVector1.y, ClampVector2.y);
            target.position = curcam.ScreenToWorldPoint(scrPos);
		}
	}
	[SerializeField]
	private float moveSpeed = 0.03f;
	private void OnPressLeftDelegate(){
		isPressed = true;
		scale = new Vector3(moveSpeed,0,0);
	}
	
	private void OnPressRightDelegate(){
		isPressed = true;
		scale = new Vector3(-moveSpeed,0,0);
	}
	
	private void OnPressTopDelegate(){
		isPressed = true;
		scale = new Vector3(0,-moveSpeed,0);
	}
	
	private void OnPressDownDelegate(){
		isPressed = true;
		scale = new Vector3(0,moveSpeed,0);
	}
	
	private void OnReleaseLeftDelegate(){
		isPressed = false;
	}
	
	private void OnReleaseRightDelegate(){
		isPressed = false;
	}
	
	private void OnReleaseTopDelegate(){
		isPressed = false;
	}
	
	private void OnReleaseDownDelegate(){
		isPressed = false;
	}
	
	#endregion
}
