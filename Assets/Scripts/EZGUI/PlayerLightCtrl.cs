using UnityEngine;
using System.Collections;

public class PlayerLightCtrl : MonoBehaviour {
	
	public static PlayerLightCtrl Instance = null;
	
	public Transform obj;
	
	void Awake()
    {
        Instance = this;
		
		
		
	}
	
	// Use this for initialization
	void Start () {
		if(PlayerLightCtrl.Instance != null && Player.Instance != null){

			PlayerLightCtrl.Instance.transform.position = new  Vector3(Player.Instance.transform.position.x,Player.Instance.transform.position.y ,Player.Instance.transform.position.z);
			PlayerLightCtrl.Instance.transform.parent = Player.Instance.transform;
			
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void DestroyObj(){
	
		Destroy(gameObject);
		
	}
}
