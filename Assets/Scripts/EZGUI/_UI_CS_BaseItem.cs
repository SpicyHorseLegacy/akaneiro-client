using UnityEngine;
using System.Collections;

public class _UI_CS_BaseItem : MonoBehaviour {
	
	//TypeID
	// 1 < 装备栏 >
	// 2 < 背包栏 >
	// 3 < 仓库栏 >
	//	类型ID
	public int					m_TypeID;	
	public int					m_Slot;
	//	在屏幕中起始坐标
	public 	Vector3 			m_StartPosition;
	//	自身对象
	public UIButton				m_MyIconBtn;
	public bool					m_IsOperate  = false;
	
	public Rect					m_rect;
	
	public bool					m_IsEmpty = true;
	
	public bool 				m_IsNotMove = false;
	
	public int					m_ItemTypeID;

	public Vector2				m_offestPos = new  Vector2(0,0); 
	
	// Use this for initialization
	void Start () {
		//m_rect.width  = 1;
		//m_rect.height = 1;
	}
	
	// Update is called once per frame
	void Update () {
	
		
	}

	//更新icon 坐标
	public void UpdateMyIconPosition(){
		if(!m_IsOperate)
			return;
		m_MyIconBtn.transform.position = _UI_CS_Ctrl.Instance.m_UI_Camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x   , Input.mousePosition.y ,_UI_CS_Ctrl.Instance.m_UI_Camera.nearClipPlane));
		m_MyIconBtn.transform.position = new Vector3(m_MyIconBtn.transform.position.x + m_offestPos.x,m_MyIconBtn.transform.position.y + m_offestPos.y,-3);
	}

//	//设置主视口位置 
//	public void SetInViewPosition(){
//		m_StartPosition	= m_MyIconBtn.transform.position;
//	}
	
	
	//元素交换逻辑
	virtual public void ElementToSwap(int myID,int destID){
		
	}
	
	public void setIsEmpty(bool tf){
		m_IsEmpty = tf;
	}

}
