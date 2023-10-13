using UnityEngine;
using System.Collections;

public class _UI_CS_SmallMap : MonoBehaviour {
	
	public Camera m_Ingame_SmallMapCamera;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(null != Player.Instance){
			//m_Ingame_SmallMapCamera.transform.position = Player.Instance.SpawnPosition;
			m_Ingame_SmallMapCamera.transform.position = Player.Instance.transform.position;
			m_Ingame_SmallMapCamera.transform.position = new Vector3(m_Ingame_SmallMapCamera.transform.position.x,m_Ingame_SmallMapCamera.transform.position.y,m_Ingame_SmallMapCamera.transform.position.z+50);
		}
	}
}
