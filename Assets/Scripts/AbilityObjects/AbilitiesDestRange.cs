using UnityEngine;
using System.Collections;

public class AbilitiesDestRange : MonoBehaviour {
	public  static AbilitiesDestRange 	 Instance;
	public  float			 m_range;
	public  GameObject		 m_cube;
	public  bool			 m_isHide;
	public  Camera			 m_camera;
	public  LayerMask 		 m_groundLayer;
	private Ray 			 m_ray;
	public  RaycastHit 	     out_hitInfo;

	
	void Awake()
	{
		Instance = this;	
	}
	
	// Use this for initialization
	void Start () {
		m_isHide			  = true;
	}
	
	// Update is called once per frame
	void Update () {
		
		if(!m_isHide){
			m_cube.transform.localScale = new Vector3(m_range,0f,m_range);
		}else{
			m_cube.transform.localScale = new Vector3(0f,0f,0f);
		}
	}
	
	public void SetAbilitiesRange(float x){
		m_range = x;		
	}
	
	public void SetIsHide(bool isHide){
		m_isHide = isHide;
	}
	
	public Ray GetRay(){
		m_ray  = m_camera.ScreenPointToRay (new Vector3(Input.mousePosition.x+25 ,Input.mousePosition.y-30,0));
		return m_ray;
	}
	
	public RaycastHit GetAbilitiesPos(){
		GetRay();
		out_hitInfo.point = new Vector3(out_hitInfo.point.x,out_hitInfo.point.y+0.3f,out_hitInfo.point.z);
		return out_hitInfo;
	}
	
	public void SetRangePos(){
		m_cube.transform.localPosition = out_hitInfo.point;
	}
}
