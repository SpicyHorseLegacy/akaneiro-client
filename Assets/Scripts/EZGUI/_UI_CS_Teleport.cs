using UnityEngine;
using System.Collections;

public class _UI_CS_Teleport : MonoBehaviour {
	
	//Instance
	public static _UI_CS_Teleport Instance = null;
	
	public UIPanel 	bgPanel;
	public UIButton teleportBtn;
	public Transform TelSound;
	public Transform Pos;

	
	void Awake(){
		
		Instance = this;
	}
	
	// Use this for initialization
	void Start () {
	
		teleportBtn.AddInputDelegate(TeleportDelegate);
		
	}
	
	public void HideTelport(){
		bgPanel.transform.position = Pos.position;
		_UI_CS_MissionLogic.Instance.MissionBgPanel.transform.position = Pos.position;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	
	void TeleportDelegate(ref POINTER_INFO ptr){
		switch(ptr.evt){
		   case POINTER_INFO.INPUT_EVENT.TAP:
				PlayVFX();
				StartCoroutine(WaitOneFrameBC());
				break;
		   default:
				break;
		}	
	}
	
	public Transform telOutVFX;
	public void PlayVFX() {
		Transform vfxInstance = UnityEngine.Object.Instantiate(telOutVFX)as Transform;
		if(vfxInstance != null&&Player.Instance != null){
			vfxInstance.position = Player.Instance.transform.position;
		}
	}
	
	private IEnumerator WaitOneFrameBC(){
		yield return new WaitForSeconds(1f);
		Time.timeScale = 1;
		bgPanel.Dismiss();
		_UI_CS_ScreenCtrl.Instance.SetNextScreenType(_UI_CS_ScreenCtrl.EM_SCREEN_TYPE.EM_LOADING);
		_UI_CS_FightScreen.Instance.m_CS_Ingame_NormalPanel.Dismiss();
		_UI_CS_FightScreen.Instance.m_fightPanel.Dismiss();
		_UI_MiniMap.Instance.isShowSmallMap	 = false;
		_UI_MiniMap.Instance.isShowBigMap	 = false;
//		Tutorial.Instance.isTutorial = false;
		TutorialMan.Instance.SetTutorialFlag(false);
		_UI_CS_LoadProgressCtrl.Instance.m_LoadingMapName.Text = _UI_CS_MapScroll.Instance.SceneNameToMapName("Hub_Village");
		CS_Main.Instance.g_commModule.SendMessage(ProtocolBattle_SendRequest.EnterScene("Hub_Village"));	
		_UI_CS_LoadProgressCtrl.Instance.AwakeLoading(0);
	}
	
	public void AwakeTel(){
		bgPanel.BringIn();
		SoundCue.PlayPrefabAndDestroy(TelSound);
	}
		
		
}
