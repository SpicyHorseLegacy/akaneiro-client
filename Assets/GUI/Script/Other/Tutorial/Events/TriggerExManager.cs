using UnityEngine;
using System.Collections;

public class TriggerExManager : MonoBehaviour {
	
	public static TriggerExManager Instance = null;

	void Awake(){
		Instance = this;
	}
	
	[SerializeField]
	private UIClientTriggerEx [] triggerList;
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void OpenAllObj() {
		for(int i = 0;i<triggerList.Length;i++) {
			triggerList[i].bTouch = false;
		}
	}
}
