using UnityEngine;
using System.Collections;

public class EventsManager : MonoBehaviour {
	
	public static EventsManager Instance;
	
	void Awake() {
		Instance = this;
		GUIManager.Instance.AddTemplateInitEnd();
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	#region Interface
	[SerializeField]
	private UILabel title;
	[SerializeField]
	private UILabel description1;
	[SerializeField]
	private UILabel description2;
	public void InitEventData(string titleName,string desc1,string desc2) {
		title.text = titleName;description1.text = desc1;description2.text = desc2;
	}
	#endregion
	
	#region 
	public delegate void Handle_ThanksDelegate();
    public event Handle_ThanksDelegate OnThanksDelegate;
	private void ThanksDelegate() {
		if(OnThanksDelegate != null) {
			OnThanksDelegate();
		}
	}
	#endregion
}
