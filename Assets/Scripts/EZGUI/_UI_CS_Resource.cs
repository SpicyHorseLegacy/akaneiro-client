using UnityEngine;
using System.Collections;

public class _UI_CS_Resource : MonoBehaviour {
	
	//Instance
	public static _UI_CS_Resource Instance = null;
	public Texture2D [] m_EquipmentIcon;
	
	void Awake()
	{	
		Instance = this;
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
