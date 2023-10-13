using UnityEngine;
using System.Collections;

public class PlayerCtrl : MonoBehaviour {
	
	public float faceAngle = 0;
	
	void Start(){
		if(Player.Instance)
		{
	        Player.Instance.transform.position = Player.Instance.GetComponent<PlayerMovement>().pointOnTheGround(transform.position);
			Player.Instance.transform.rotation = Quaternion.identity;
			if(Player.Instance.GetComponent<PlayerMovement>().PlayerObj)
				Player.Instance.GetComponent<PlayerMovement>().PlayerObj.transform.eulerAngles = new Vector3(0, faceAngle, 0);
	
	        Player.Instance.ReactivePlayer();
		}
		
		if(GameCamera.Instance)
		{
			GameCamera.Instance.gameCamera.camera.enabled = true;
			GameCamera.Instance.gameCamera.transform.localPosition = Vector3.zero;
			GameCamera.Instance.gameCamera.transform.localEulerAngles = Vector3.zero;
			GameCamera.Instance.ResetCamera();
		}
	}
}
