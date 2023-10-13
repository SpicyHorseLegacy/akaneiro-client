using UnityEngine;
using System.Collections;

public class PersistentBGM : MonoBehaviour {

	// Use this for initialization
	void Start () {
        if (BGManager.Instance)
            BGManager.Instance.PlayOriginalBG();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
