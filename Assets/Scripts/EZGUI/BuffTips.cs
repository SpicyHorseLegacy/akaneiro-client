using UnityEngine;
using System.Collections;

public class BuffTips : MonoBehaviour {
	
	//Instance
	public static BuffTips Instance = null;
	
	public Transform  Root;
	public UIButton   BG;
	public SpriteText Name;
	public SpriteText Description;
	
	void Awake(){
		Instance = this;
	}
	
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public void ShowBuffTip(string name ,string description,ItemPosOffestType poType,Vector3 pos){
		Name.Text = name;
		Description.Text = description;
		SetTipPos(poType,pos);
	}
	
	public void DismissTip(){
		Root.position = new Vector3(999,999,999);
	}
	
	private void SetTipPos(ItemPosOffestType poType,Vector3 pos){
		float x = 0f;
		float y = 0f;
		float z = 0f;
		
		switch(poType){
		case ItemPosOffestType.LEFT_TOP:
			x = (0-BG.width);
			y = BG.height;
			z = 0;
			break;
		case ItemPosOffestType.RIGHT_TOP:
			x = 0;
			y = BG.height;
			z = 0;
			break;
		case ItemPosOffestType.LEFT_BOT:
			x = (0-BG.width);
			y = 0;
			z = 0;
			break;
		case ItemPosOffestType.RIGHT_BOT:
			x = 0; y = 0; z = 0;
			break;
		case ItemPosOffestType.LEFT_MIDDLE:
			x = (0-BG.width); 
			y = BG.height/2; 
			z = 0;
			break;
		case ItemPosOffestType.RIGHT_MIDDLE:
			x = 0; 
			y = BG.height/2; 
			z = 0;
			break;
		default:
			break;
		}
		
		Root.position = new Vector3(pos.x + x + 2f,pos.y + y,pos.z + z -0.1f);
	}
}
