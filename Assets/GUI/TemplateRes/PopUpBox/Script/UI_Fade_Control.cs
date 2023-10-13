using UnityEngine;
using System.Collections;

public class UI_Fade_Control : MonoBehaviour {
	
	public static UI_Fade_Control Instance;
	
	void Awake()
	{
		Instance = this;
	}
	
	public void FadeOutAndIn(string _newsceneName)
	{
		FadeOut();
		TweenDelay.Begin(gameObject, 0.5f, "AddNewScene", _newsceneName);
	}
	
	public void FadeOutAndIn()
	{
		FadeOut();
		TweenDelay.Begin(gameObject, 0.5f, "FadeOut", null);
	}
	
	public void FadeIn()
	{
		TweenAlpha.Begin(gameObject, 2.0f, 0);
	}
	
	public void FadeOut()
	{
		TweenAlpha.Begin(gameObject, 0.5f, 1);
	}
	
	void AddNewScene(string _newsceneName)
	{
		GUIManager.Instance.ChangeUIScreenState(_newsceneName);
	}
	
	struct LoadingSneneInfo
	{
		public string m_nextSceneName;
		public int m_missionID;
		public string m_missionName;
	}

	public void FadeOutAndIn(string _nextSceneName, string _missionName, int _missionID)
	{
		FadeOut();
		LoadingSneneInfo _nextscneninfo = new LoadingSneneInfo();
		_nextscneninfo.m_nextSceneName = _nextSceneName;
		_nextscneninfo.m_missionName = _missionName;
		_nextscneninfo.m_missionID = _missionID;
		TweenDelay.Begin(gameObject, 0.5f, "AddNewLoadingScene", _nextscneninfo);
	}
	
	void AddNewLoadingScene(LoadingSneneInfo _nextSceneInfo)
	{
        // Fix, like this we set the current mission id before the loading screen read it
        PlayerDataManager.Instance.SetMissionID(_nextSceneInfo.m_missionID + 1);

		GUIManager.Instance.ChangeUIScreenState("LoadingScreen");
		LoadingScreenCtrl.Instance.AwakeLoading(_nextSceneInfo.m_missionID,_nextSceneInfo.m_missionName);
		CS_Main.Instance.g_commModule.SendMessage(ProtocolBattle_SendRequest.EnterScene(_nextSceneInfo.m_nextSceneName));
	}
}
