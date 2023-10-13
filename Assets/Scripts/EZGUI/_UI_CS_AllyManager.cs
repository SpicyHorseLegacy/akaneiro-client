using UnityEngine;
using System.Collections;

public class _UI_CS_AllyManager : MonoBehaviour {
	
	public static _UI_CS_AllyManager Instance = null;
	
	public 		  UIPanel	[]	m_AllyPanel;
	
	public 		  UIButton 	[]  m_AllyIconBtn;

	public 		  UIProgressBar []	m_AllyHPBar;
	
	public  	  int			m_AllyCount;
	
	public 		  Texture2D		[]  m_AllyTextures;
	
	void Awake(){
		Instance = this;
	}
	
	// Use this for initialization
	void Start () {
	
		
		
	}
	
	// Update is called once per frame
	void Update () {
		
		if(0 != m_AllyCount){
		
			for(int i = 0;i< m_AllyCount;i++){

                m_AllyHPBar[m_AllyCount - 1].Value = CS_SceneInfo.Instance.AllyNpcList[i].AttrMan.Attrs[EAttributeType.ATTR_CurHP] / CS_SceneInfo.Instance.AllyNpcList[i].AttrMan.Attrs[EAttributeType.ATTR_MaxHP];
			
			}
		
		}
		
	}
	
	public void AllyAdd(){
	
		m_AllyCount++;
		m_AllyPanel[m_AllyCount-1].BringIn();
		m_AllyIconBtn[m_AllyCount-1].SetUVs(new Rect(0,0,1,1));
		m_AllyIconBtn[m_AllyCount-1].SetTexture(m_AllyTextures[0]);
		
	}
	
	public void AllyDissmiss(){
	
		m_AllyCount = 0;
		m_AllyPanel[0].Dismiss();
		m_AllyPanel[1].Dismiss();
		
	}
	
}
