using UnityEngine;
using System.Collections;

public class _UI_CS_MapInfo : MonoBehaviour {
	
	public static _UI_CS_MapInfo Instance;
	
	public _UI_CS_MapItem [] Itemlist;
	
	void Awake()
	{	
		Instance = this;
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
#region Interface
	public int GetLevelIndex(int missionIdx,int threat) {
		int idx = (missionIdx)*4+threat;
		return idx;
	}
	
	public int SceneNameToMissionID(string sceneName){
		if(0 == string.Compare(sceneName,"Hub_Village")){
			return 6000;
		}else if(0 == string.Compare(sceneName,"2012E3Demo")){
			return 6000;
		}else if(0 == string.Compare(sceneName,"template1")){
			return 6000;
		}else if(0 == string.Compare(sceneName,"A1_M1")){
			return 5110;
		}else if(0 == string.Compare(sceneName,"A1_M2")){
			return 5120;
		}else if(0 == string.Compare(sceneName,"A1_M3")){
			return 5130;
		}else if(0 == string.Compare(sceneName,"A1_M4")){
			return 5140;
		}else if(0 == string.Compare(sceneName,"A2_M1")){
			return 5210;
		}else if(0 == string.Compare(sceneName,"A2_M2")){
			return 5220;
		}else if(0 == string.Compare(sceneName,"A2_M3")){
			return 5230;
		}else if(0 == string.Compare(sceneName,"A2_M4")){
			return 5240;
		}else if(0 == string.Compare(sceneName,"A3_M1")){
			return 5310;
		}else if(0 == string.Compare(sceneName,"A3_M2")){
			return 5320;
		}else if(0 == string.Compare(sceneName,"A3_M3")){
			return 5330;
		}else if(0 == string.Compare(sceneName,"A3_M4")){
			return 5340;
		}else if(0 == string.Compare(sceneName,"A4_M1")){
			return 5410;
		}else if(0 == string.Compare(sceneName,"A4_M2")){
			return 5420;
		}else if(0 == string.Compare(sceneName,"A4_M3")){
			return 5430;
		}else if(0 == string.Compare(sceneName,"A4_M4")){
			return 5440;
		}else if(0 == string.Compare(sceneName,"A5_M1")){
			return 5510;
		}else if(0 == string.Compare(sceneName,"A5_M2")){
			return 5520;
		}else if(0 == string.Compare(sceneName,"A5_M3")){
			return 5530;
		}else if(0 == string.Compare(sceneName,"A6_M1")){
			return 5610;
		}else if(0 == string.Compare(sceneName,"A6_M2")){
			return 5620;
		}else if(0 == string.Compare(sceneName,"A6_M3")){
			return 5630;
		}else if(0 == string.Compare(sceneName,"A7_M1")){
			return 5710;
		}else if(0 == string.Compare(sceneName,"A7_M2")){
			return 5720;
		}else if(0 == string.Compare(sceneName,"A7_M3")){
			return 5730;
		}else if(0 == string.Compare(sceneName,"A8_M1")){
			return 5810;
		}else if(0 == string.Compare(sceneName,"A8_M2")){
			return 5820;
		}else if(0 == string.Compare(sceneName,"A8_M3")){
			return 5830;
		}else if(0 == string.Compare(sceneName,"Hub_Village_Tutorial")){
			return 5010;
		}else if(0 == string.Compare(sceneName,"TWILIGHT")){
			return 6000;
		}else if(0 == string.Compare(sceneName,"EmptyScenes")){
			return 6000;
		}else{
			return 6000;
		}
		return 6000;
	}
#endregion
}
