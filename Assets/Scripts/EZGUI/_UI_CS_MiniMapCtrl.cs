using UnityEngine;
using System.Collections;

public class _UI_CS_MiniMapCtrl : MonoBehaviour {
	
	//public UIButton MiniMap;
	public Texture2D texture;
	
	public float offestX;
	public float offestY;
	
	public float UV_OffestX;
	public float UV_OffestY;
	
	public float width;
	public float height;
	
	public bool IsShow =false;
	
	public float playerPerX;
	public float playerPerY;
	
	public float K;
	
	public bool isInitPlayerPosition = true;
	
	
	/// <summary>
	//public UIButton mapTest;
	/// </summary>
	
	
	// Use this for initialization
	void Start () {
		//mapTest.SetUVs(new Rect(0,0,1,1));
		//mapTest.SetTexture(_UI_CS_Resource.Instance.m_EquipmentIcon[3]);
	}
	
	// Update is called once per frame
	void Update () {
		
		if (Input.GetKey ("m")){
			if(IsShow){
				IsShow = false;
			}else{
				IsShow = true;
			}
		}
		
		//if (Input.GetKey ("left")){
		//	UV_OffestX -= 0.01f;
		//}
		//if (Input.GetKey ("right")){
		//	UV_OffestX += 0.01f;
		//}
		//
		//if (Input.GetKey ("up")){
		//	UV_OffestY += 0.01f;
		//}
		//
		//if (Input.GetKey ("down")){
		//	UV_OffestY -= 0.01f;
		//}
		//
		if(Player.Instance != null){
			
			if(isInitPlayerPosition){
				isInitPlayerPosition = false;
				playerPerX = Player.Instance.SpawnPosition.x;
				playerPerY = Player.Instance.SpawnPosition.z;
			}
			
			if(playerPerX != Player.Instance.transform.position.x){
				UV_OffestX -= (playerPerX - Player.Instance.transform.position.x)/K;
				playerPerX = Player.Instance.transform.position.x;
			}
			
			if(playerPerY != Player.Instance.transform.position.z){
				UV_OffestY += (playerPerY - Player.Instance.transform.position.z)/K;
				playerPerY = Player.Instance.transform.position.z;
			}
			
		}
		
	}
	
	void OnGUI(){
		if(IsShow){
			if(Player.Instance != null){
				if(Event.current.type.Equals(EventType.Repaint)){
					Graphics.DrawTexture(new Rect(offestX, offestY, width, height), texture,
					                     new Rect(UV_OffestX, UV_OffestY, 0.1f, 0.1f),0,0,0,0);
				}
			}
		}
	}
	
}
