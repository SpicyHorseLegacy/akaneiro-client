using UnityEngine;
using System.Collections;

public class ActiveObj : EventBase {

	// Use this for initialization
	void Start () {
		Regis();
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	[SerializeField]
	private Transform tran;
	[SerializeField]
	private bool isActive = true;
	public override void Init() {
		tran.gameObject.SetActive(isActive);
	}
}
