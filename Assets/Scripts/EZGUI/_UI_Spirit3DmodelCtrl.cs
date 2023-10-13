using UnityEngine;
using System.Collections;

public class _UI_Spirit3DmodelCtrl : MonoBehaviour {
	
	public static _UI_Spirit3DmodelCtrl Instance = null;
	
	public Transform [] spiritArray;
//	public Transform [] spiritAniArray;
	
	public enum eSpiriteType
	{
		eTypeBird 	= 0,
		eTypeCat 	= 1,
		eTypeDog 	= 2,
		eTypeKoi 	= 3,
		eTypeMonkey = 4,
		eTypeTurtle = 5,
		eTypeYokai 	= 6,
		eTypeDog_Gold = 7,
		eTypeCat_Black = 8,
		eTypePengin = 9,
		eTypeGaben = 10,
		eTypeMax,
	}
	
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
	
	public void Show(int idx){
		Vector3 tPos = _UI_CS_Ctrl.Instance.m_UI_Camera.WorldToScreenPoint(_UI_CS_SpiritTrainer.Instance.SpiritPos.position);
		spiritArray[idx].GetComponent<SurveillanceCamera>().ShowAt(new Vector2(tPos.x,tPos.y), new Vector2(Screen.width/10, Screen.height));
//		spiritAniArray[idx].GetComponent<Animation>().Play();
	}
	
	public void Hide(int idx){
	
		spiritArray[idx].GetComponent<SurveillanceCamera>().ShutDown();
//		spiritAniArray[idx].GetComponent<Animation>().Stop();
	
	}
	
}
