using UnityEngine;
using System.Collections;

public class miniMapObject : MonoBehaviour {
	
	public GameObject missionMap;
	
	// Use this for initialization
	void Start () {
		Instantiate (missionMap, Vector3.zero, this.transform.rotation);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
