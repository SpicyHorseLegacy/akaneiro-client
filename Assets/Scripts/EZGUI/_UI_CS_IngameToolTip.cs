using UnityEngine;
using System.Collections;

public class _UI_CS_IngameToolTip : MonoBehaviour {
	
	public bool 	 isHide = true;
	public Transform Own;
	public float ItemVal;
	
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if(Own){
			if(isHide){
				Vector2 	 V2PosT = new Vector2(0,0);
				gameObject.GetComponent<GUIText>().pixelOffset = V2PosT;
			}else{	
				Vector3 posOnScreen = Vector3.zero;
				Vector2 	 V2PosT = new Vector2(0,0);
				posOnScreen = Camera.mainCamera.WorldToScreenPoint(Own.position - Vector3.up);
				V2PosT.x = posOnScreen.x;
				V2PosT.y = posOnScreen.y;
				gameObject.GetComponent<GUIText>().pixelOffset = V2PosT;
			}
		}else{
			_UI_CS_IngameToolTipMan.Instance.m_List.Remove(gameObject.transform);
			Destroy(gameObject);
		}
	}
	
	public void InitObj(string name,Transform Pos,bool hide,float Val){
		
		gameObject.GetComponent<GUIText>().text = name;
		
		ItemVal = Val;
		
		
		
//		gameObject.GetComponent<GUIText>().font.material.color
		Own = Pos;
		isHide = hide;
	}
	
	public void HideObj(){
	
		isHide 	 = true;
		
	}
	
	public void ShowObj(){
	
		isHide 	 = false;
		
	}

	
}
