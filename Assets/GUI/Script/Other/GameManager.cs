using UnityEngine;
using System.Collections;

public class GameManager : MonoBehaviour {
	
	public static GameManager Instance = null;
	
	void Awake() {
		Instance = this;
		Application.targetFrameRate = 60;
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	#region Interface
	[SerializeField]
	private bool isReconnect = false;
	public void SetReconnectFlag(bool rec) {
		isReconnect = rec;
	}
	public bool GetReconnectFlag() {
		return isReconnect;
	}
	
	[SerializeField]
	private bool isCheat = false;
	public void SetCheatFlag(bool cheat) {
		isCheat = cheat;
	}
	public bool GetCheatFlag() {
		return isCheat;
	}
	
	[SerializeField]
	private bool isDisconnect = false;
	public void SetDisconnectFlag(bool disconnect) {
		isDisconnect = disconnect;
	}
	public bool GetDisconnectFlag() {
		return isDisconnect;
	}
	
	[SerializeField]
	private bool isFullScreen = false;
	public void SetFullScreenFlag(bool fullScreen) {
		isFullScreen = fullScreen;
	}
	public bool GetFullScreenFlag() {
		return isFullScreen;
	}
	
//	[SerializeField]
//	private bool isTutorial = false;
//	public void SetTutorialFlag(bool istutorial) {
//		isTutorial = istutorial;
//	}
//	public bool GetTutorialFlag() {
//		return isTutorial;
//	}
	#endregion
}
