using UnityEngine;
using System.Collections;

public class AbilitiesRange : MonoBehaviour {
	public  static AbilitiesRange 	 Instance;
	private Player 			 player;
	public  float			 m_range;
	public  GameObject		 m_cube;
	public  bool			 m_isHide;
	
	void Awake()
	{
		Instance = this;	
	}
	
	// Use this for initialization
	void Start () {
		m_isHide			  = true;
		player				  = Player.Instance;
	}
	
	// Update is called once per frame
	void Update () {
		
		if(!m_isHide){
			m_cube.transform.localScale = new Vector3(m_range,0f,m_range);
		}else{
			m_cube.transform.localScale = new Vector3(0f,0f,0f);
		}
		
		m_cube.transform.position = new Vector3(player.transform.position.x,player.transform.position.y+0.3f,player.transform.position.z);
	}
	
	public void SetAbilitiesRange(float x){
		m_range = x;		
	}
	
	public void SetIsHide(bool isHide){
		m_isHide = isHide;
	}
	
}
