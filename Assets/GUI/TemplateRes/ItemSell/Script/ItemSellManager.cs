using UnityEngine;
using System.Collections;

public class ItemSellManager : MonoBehaviour {
	
	public static ItemSellManager Instance;
	
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
	private Transform lt;
	[SerializeField]
	private Transform br;
	public bool InEquipSlot() {
		Vector3 mousePos = UICamera.currentCamera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x,Input.mousePosition.y,-1));
		float l = lt.position.x;float r = br.position.x;
		float t = lt.position.y;float b = br.position.y;
		if(mousePos.x > l && mousePos.x < r && mousePos.y > b && mousePos.y < t) {
			return true;
		}
		return false;
	}
	
}
