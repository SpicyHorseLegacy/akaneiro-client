using UnityEngine;
using System.Collections;

public class _UI_CS_SpiritShopSound : MonoBehaviour {
	
	public static _UI_CS_SpiritShopSound Instance = null;
	
	public Transform[] BGMS;
	
	void Awake () {
		Instance = this;
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
