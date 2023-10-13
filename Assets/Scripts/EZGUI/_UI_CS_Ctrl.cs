using UnityEngine;
using System.Collections;
using System.IO;

public class _UI_CS_Ctrl : MonoBehaviour {
	//Instance
	public static _UI_CS_Ctrl Instance  = null;
	//Camera
	public Camera		m_UI_Camera;
	//UIManager
	public UIManager	m_UI_Manager;
	public bool 		m_isReconnect 	= false;
	public bool 		m_isCheat 		= false;
	void Awake(){
		Instance = this;
	}
	
	// Use this for initialization
	void Start () {
	}
	
	void Update () {

	}
	
	public void WaitOneFrame(){
	    StartCoroutine(WaitOneFrameBC());
	}
	
	private IEnumerator WaitOneFrameBC()
	{
		yield return new WaitForSeconds(0.5f);
		
	}
	
	
	// bag frame ctrl;
	public IEnumerator SkipOneFrameForBag()
	{	
		yield return null;
		_UI_CS_BagCtrl.Instance.ob.transform.position = _UI_CS_BagCtrl.Instance.DP;
		if(Inventory.Instance.bagItemArray[0] != null) {
			Inventory.Instance.bagItemArray[0].UpdateGroupElementPosition();
		}
	}
	
	
}
