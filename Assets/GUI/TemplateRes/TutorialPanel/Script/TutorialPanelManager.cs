using UnityEngine;
using System.Collections;

public class TutorialPanelManager : MonoBehaviour {
	
	public static TutorialPanelManager Instance = null;
	
	void Awake() {
		Instance = this;
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	[SerializeField]
	private UILabel titleText;
	[SerializeField]
	private UILabel contentText;
	#region Interface
	public void SetDialogContent(string title,string content) {
		titleText.text = title;
		contentText.text = content;
	}
	
	public delegate void Handle_ContinueDelegate();
    public event Handle_ContinueDelegate OnContinueDelegate;
	private void _ContinueDelegate() {
		if(OnContinueDelegate != null) {
			OnContinueDelegate();
		}
	} 
	#endregion
}
