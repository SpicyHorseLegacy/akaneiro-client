using UnityEngine;
using System.Collections;

public class _UI_CS_AccomplishmentItemEx : MonoBehaviour {

	public _UI_CS_AccomplishmentItem m_accInfo;
	
	private Rect  m_rect;
	public int    m_ListID;
	
	public UIButton m_iconButton;
	public UIProgressBar   m_ProgressBar;
	public SpriteText m_ksVal;
	public SpriteText m_info;
	public SpriteText m_name;
	public SpriteText m_ks;
	
	// Use this for initialization
	void Start () {
		m_rect.width = 1;
		m_rect.height = 1;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
}
