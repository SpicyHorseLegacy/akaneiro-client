using UnityEngine;
using System.Collections;

public class Arrow3d : MonoBehaviour {
	
	void Awake() {
		isActive = false;
		startTime = Time.time;
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		UpdateLifeCycle();
	}
	
	#region Interface
	private float time = 0;
	private bool autoDestory = false;
	public void SetTime(float t) {
		time = t;
		if(t <= 0.1f) {
			autoDestory = false;
		}else {
			autoDestory = true;
		}
	}
	
	private string key = "";
	public void SetKey(string k) {
		key = k;
	}
	public string GetKey() {
		return key;
	}
	
	public void SetPos(Vector3 p) {
		transform.localPosition = p;
	}
	
	public void RemoveIt() {
		Destroy(gameObject);
	}
	#endregion
	
	#region Local
	private bool isActive = false;
	private float startTime = 0f;
	private void UpdateLifeCycle() {
		if(isActive) {
			float tempT = Time.time - startTime;
			if(tempT < 0) {
				isActive = false;
				if(autoDestory) {
					RemoveIt();
				}
			}
		}
	}
	#endregion
}
