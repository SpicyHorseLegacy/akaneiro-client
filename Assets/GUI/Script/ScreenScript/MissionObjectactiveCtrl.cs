using UnityEngine;
using System.Collections;

public class MissionObjectactiveCtrl : MonoBehaviour {
	
	public void RegisterSingleTemplateEvent(string _templateName) {
		if (_templateName == "MissionObjective" && MissionObjectiveManager.Instance) {
			
			if(MissionObjectiveManager.Instance) {
				MissionObjectiveManager.Instance.UpdateMissionObjList();
				MissionObjectiveManager.Instance.InitFirstAniData();
				MissionObjectiveManager.Instance.InitFirstListObjPos();
				
			}

			if(MissionTitleCtrl.Instance) {
				MissionTitleCtrl.Instance.OnTitleOutDelegate += MissionTitleOutDelegate;
				BackCrystalCtrl.Instance.OnBackCrystalDelegate += BackCrystalDelegate;
				MissDescData missionData = PlayerDataManager.Instance.GetMissDescData((int)(PlayerDataManager.Instance.GetMissionID() / 10) * 10 + 1);
				if(missionData != null) {
					MissionTitleCtrl.Instance.SetAreaName(missionData.areaName);
					MissionTitleCtrl.Instance.SetMissionName(missionData.missName);
					MissionTitleCtrl.Instance.Play();
				}
			}
        }

        if(_templateName == "XPBar" && Hud_XPBar_Manager.Instance)
        {
            Hud_XPBar_Manager.Instance.InitXP(PlayerDataManager.Instance.MissionScore);
        }

        if(_templateName == "GameHud_KillChain" && Hud_KillChain_Manager.Instance)
        {
            Hud_KillChain_Manager.Instance.InitKillChain();
        }
    }

    public void UnregisterSingleTemplateEvent(string _templateName) {
		if (_templateName == "MissionObjective" && MissionObjectiveManager.Instance) {
			if(MissionObjectiveManager.Instance) {
				if(MissionTitleCtrl.Instance) {
					MissionTitleCtrl.Instance.OnTitleOutDelegate -= MissionTitleOutDelegate;
					BackCrystalCtrl.Instance.OnBackCrystalDelegate -= BackCrystalDelegate;
				}
			}
        }
    }
	
	private void MissionTitleOutDelegate() {
		MissionObjectiveManager.Instance.PlayMissionAni();
	}
	private void BackCrystalDelegate() {
		int missionID = _UI_CS_MapInfo.Instance.SceneNameToMissionID("Hub_Village")+1;
		TutorialMan.Instance.SetTutorialFlag(false);
		UI_Fade_Control.Instance.FadeOutAndIn("Hub_Village", "Hub_Village", missionID);
	}
	
	
//	private void TaskOutDelegate() {
//	}
	
	#region Interface
	
	#endregion
	
}
